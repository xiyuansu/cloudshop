using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Member;
using Hidistro.SqlDal.Members;
using Hidistro.UI.Common.Controls;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace Hidistro.UI.Web.API
{
	public class ReviewHandler : IHttpHandler
	{
		private string Result = "{{ \"state\": \"{0}\",\"msg\": \"{1}\"}}";

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "text/plain";
			try
			{
				string text = context.Request["action"];
				if (string.IsNullOrEmpty(text))
				{
					context.Response.Write(string.Format(this.Result, "false", "参数错误"));
				}
				else
				{
					string text2 = text;
					switch (text2)
					{
					default:
						if (text2 == "LoadLineItems")
						{
							this.LoadLineItems(context);
						}
						break;
					case "SubmitReview":
						this.SubmitReview(context);
						break;
					case "LoadProductReview":
						this.LoadProductReview(context);
						break;
					case "LoadReview":
						this.LoadReview(context);
						break;
					case "StatisticsReview":
						this.StatisticsReview(context);
						break;
					}
				}
			}
			catch (Exception ex)
			{
				context.Response.Write(string.Format(this.Result, "false", ex.Message.ToString()));
			}
		}

		public void LoadLineItems(HttpContext context)
		{
			if (!string.IsNullOrEmpty(context.Request["PageSize"]) && !string.IsNullOrEmpty(context.Request["CurrentPage"]) && !string.IsNullOrEmpty(context.Request["ProductId"]))
			{
				int pageSize = int.Parse(context.Request["PageSize"]);
				int currentPage = int.Parse(context.Request["CurrentPage"]);
				int productId = int.Parse(context.Request["ProductId"]);
				DbQueryResult lineItems = ProductBrowser.GetLineItems(productId, currentPage, pageSize);
				DataTable data = lineItems.Data;
				string str = "{\"totalCount\":\"" + lineItems.TotalRecords + "\",\"data\":[";
				string text = "";
				for (int i = 0; i < data.Rows.Count; i++)
				{
					if (text != "")
					{
						text += ",";
					}
					string text2 = data.Rows[i]["Picture"].ToString();
					if (string.IsNullOrWhiteSpace(text2))
					{
						text2 = "/templates/pccommon/images/users/hyzx_25.jpg";
					}
					text = text + "{\"UserName\":\"" + DataHelper.GetHiddenUsername(data.Rows[i]["UserName"].ToNullString()) + "\",\"Picture\":\"" + Globals.GetImageServerUrl("http://", text2) + "\",\"Quantity\":\"" + data.Rows[i]["Quantity"] + "\",\"SKUContent\":\"" + data.Rows[i]["SKUContent"] + "\",\"PayDate\":\"" + (data.Rows[i]["PayDate"].ToDateTime().HasValue ? data.Rows[i]["PayDate"].ToDateTime().Value.ToString("yyyy/MM/dd") : "") + "\"}";
				}
				str += text.Replace("\n", "").Replace("\t", "").Replace("\r", "");
				str += "]}";
				context.Response.Write(str);
			}
		}

		public void SubmitReview(HttpContext context)
		{
			string text = "";
			string szJson = context.Request["DataJson"];
			int num = 0;
			List<ProductReviewInfo> list = JsonHelper.ParseFormJson<List<ProductReviewInfo>>(szJson);
			if (list != null && list.Count > 0)
			{
				string orderId = list[0].OrderId;
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(orderId);
				if (orderInfo.OrderStatus != OrderStatus.Finished && (orderInfo.OrderStatus != OrderStatus.Closed || orderInfo.OnlyReturnedCount != orderInfo.LineItems.Count))
				{
					context.Response.Write(string.Format(this.Result, "false", "您的订单还未完成，因此不能对该商品进行评论！"));
					return;
				}
				int num2 = 0;
				int num3 = 0;
				foreach (ProductReviewInfo item in list)
				{
					ProductBrowser.LoadProductReview(item.ProductId, out num2, out num3, item.OrderId);
					if (num2 == 0)
					{
						context.Response.Write(string.Format(this.Result, "false", "您没有购买此商品(或此商品的订单尚未完成)，因此不能进行评论！"));
						return;
					}
					if (num3 >= num2)
					{
						context.Response.Write(string.Format(this.Result, "false", "您已经对此商品进行过评论(或此商品的订单尚未完成)，因此不能再次进行评论！"));
						return;
					}
					item.ReviewDate = DateTime.Now;
					item.UserId = HiContext.Current.UserId;
					item.UserName = HiContext.Current.User.UserName.ToNullString();
					item.UserEmail = HiContext.Current.User.Email.ToNullString();
					int num4 = 0;
					string[] array = item.ImageUrl1.Split(',');
					foreach (string text2 in array)
					{
						if (!string.IsNullOrEmpty(text2))
						{
							string fileURL = text2.Replace("//", "/");
							string text3 = Globals.SaveFile("review", fileURL, "/Storage/master/", true, false, "");
							num4++;
							switch (num4)
							{
							case 1:
								item.ImageUrl1 = text3;
								break;
							case 2:
								item.ImageUrl2 = text3;
								break;
							case 3:
								item.ImageUrl3 = text3;
								break;
							case 4:
								item.ImageUrl4 = text3;
								break;
							case 5:
								item.ImageUrl5 = text3;
								break;
							}
						}
					}
					ValidationResults validationResults = Validation.Validate(item, "Refer");
					text = string.Empty;
					if (!validationResults.IsValid)
					{
						foreach (ValidationResult item2 in (IEnumerable<ValidationResult>)validationResults)
						{
							text += Formatter.FormatErrorMessage(item2.Message);
						}
						break;
					}
					if (!ProductBrowser.InsertProductReview(item))
					{
						text = "评论失败，请重试";
						break;
					}
					MemberInfo user = Users.GetUser(HiContext.Current.UserId);
					SiteSettings masterSettings = SettingsManager.GetMasterSettings();
					if (user != null && masterSettings != null && masterSettings.ProductCommentPoint > 0 && ReviewHandler.AddPoints(user, masterSettings.ProductCommentPoint, PointTradeType.ProductCommentPoint))
					{
						num += masterSettings.ProductCommentPoint;
					}
				}
			}
			else
			{
				text = "请输入评价内容呀";
			}
			if (text != "")
			{
				context.Response.Write(string.Format(this.Result, "false", text));
			}
			else
			{
				context.Response.Write(string.Format(this.Result, "true", (num > 0) ? $"评价成功！<br/>恭喜您获得{num}积分奖励！" : "评价成功"));
			}
		}

		private static bool AddPoints(MemberInfo member, int points, PointTradeType type)
		{
			PointDetailDao pointDetailDao = new PointDetailDao();
			PointDetailInfo pointDetailInfo = new PointDetailInfo();
			pointDetailInfo.UserId = member.UserId;
			pointDetailInfo.TradeDate = DateTime.Now;
			pointDetailInfo.TradeType = type;
			pointDetailInfo.Increased = points;
			pointDetailInfo.Points = points + member.Points;
			if (pointDetailInfo.Points > 2147483647)
			{
				pointDetailInfo.Points = 2147483647;
			}
			if (pointDetailInfo.Points < 0)
			{
				pointDetailInfo.Points = 0;
			}
			pointDetailInfo.Remark = "评论获得积分";
			member.Points = pointDetailInfo.Points;
			return pointDetailDao.Add(pointDetailInfo, null) > 0;
		}

		public void LoadProductReview(HttpContext context)
		{
			if (!string.IsNullOrEmpty(context.Request["PageSize"]) && !string.IsNullOrEmpty(context.Request["CurrentPage"]))
			{
				int pageSize = context.Request["PageSize"].ToInt(0);
				int pageIndex = context.Request["CurrentPage"].ToInt(0);
				string orderId = context.Request["OrderId"].ToNullString();
				ProductReviewQuery productReviewQuery = new ProductReviewQuery();
				productReviewQuery.PageIndex = pageIndex;
				productReviewQuery.PageSize = pageSize;
				productReviewQuery.SortBy = "ReviewDate";
				productReviewQuery.orderId = orderId;
				productReviewQuery.SortOrder = SortAction.Desc;
				DbQueryResult userProductReviewsAndReplys = ProductBrowser.GetUserProductReviewsAndReplys(productReviewQuery);
				DataTable data = userProductReviewsAndReplys.Data;
				string str = "{\"totalCount\":\"" + userProductReviewsAndReplys.TotalRecords + "\",\"data\":[";
				string text = "";
				for (int i = 0; i < data.Rows.Count; i++)
				{
					if (text != "")
					{
						text += ",";
					}
					object[] obj = new object[30]
					{
						text,
						"{\"ProductId\":\"",
						data.Rows[i]["ProductId"],
						"\",\"ThumbnailUrl100\":\"",
						data.Rows[i]["ThumbnailUrl100"],
						"\",\"ProductName\":\"",
						data.Rows[i]["ProductName"],
						"\",\"SKUContent\":\"",
						data.Rows[i]["SKUContent"],
						"\",\"ReviewText\":\"",
						data.Rows[i]["ReviewText"].ToNullString().Replace("\\", ""),
						"\",\"Score\":\"",
						data.Rows[i]["Score"],
						"\",\"ApplicationPath\":\"\",\"ImageUrl1\":\"",
						Globals.GetImageServerUrl("http://", data.Rows[i]["ImageUrl1"].ToNullString()),
						"\",\"ImageUrl2\":\"",
						Globals.GetImageServerUrl("http://", data.Rows[i]["ImageUrl2"].ToNullString()),
						"\",\"ImageUrl3\":\"",
						Globals.GetImageServerUrl("http://", data.Rows[i]["ImageUrl3"].ToNullString()),
						"\",\"ImageUrl4\":\"",
						Globals.GetImageServerUrl("http://", data.Rows[i]["ImageUrl4"].ToNullString()),
						"\",\"ImageUrl5\":\"",
						Globals.GetImageServerUrl("http://", data.Rows[i]["ImageUrl5"].ToNullString()),
						"\",\"ReplyText\":\"",
						data.Rows[i]["ReplyText"],
						"\",\"ReviewDate\":\"",
						null,
						null,
						null,
						null
					};
					object obj2;
					DateTime value;
					if (!data.Rows[i]["ReviewDate"].ToDateTime().HasValue)
					{
						obj2 = "";
					}
					else
					{
						value = data.Rows[i]["ReviewDate"].ToDateTime().Value;
						obj2 = value.ToString("yyyy.MM.dd");
					}
					obj[26] = obj2;
					obj[27] = "\",\"ReplyDate\":\"";
					object obj3;
					if (!data.Rows[i]["ReplyDate"].ToDateTime().HasValue)
					{
						obj3 = "";
					}
					else
					{
						value = data.Rows[i]["ReplyDate"].ToDateTime().Value;
						obj3 = value.ToString("yyyy.MM.dd");
					}
					obj[28] = obj3;
					obj[29] = "\"}";
					text = string.Concat(obj);
				}
				str += text;
				str += "]}";
				context.Response.Write(str);
			}
		}

		public void LoadReview(HttpContext context)
		{
			if (!string.IsNullOrEmpty(context.Request["PageSize"]) && !string.IsNullOrEmpty(context.Request["CurrentPage"]) && !string.IsNullOrEmpty(context.Request["ProductId"]))
			{
				int pageSize = int.Parse(context.Request["PageSize"]);
				int pageIndex = int.Parse(context.Request["CurrentPage"]);
				int productId = int.Parse(context.Request["ProductId"]);
				int value = context.Request["type"].ToInt(0);
				ProductReviewQuery productReviewQuery = new ProductReviewQuery();
				productReviewQuery.PageIndex = pageIndex;
				productReviewQuery.PageSize = pageSize;
				productReviewQuery.ProductId = productId;
				productReviewQuery.SortBy = "ReviewDate";
				productReviewQuery.ProductSearchType = value;
				productReviewQuery.SortOrder = SortAction.Desc;
				DbQueryResult productReviews = ProductBrowser.GetProductReviews(productReviewQuery);
				DataTable data = productReviews.Data;
				string str = "{\"totalCount\":\"" + productReviews.TotalRecords + "\",\"data\":[";
				string text = "";
				for (int i = 0; i < data.Rows.Count; i++)
				{
					if (text != "")
					{
						text += ",";
					}
					string text2 = data.Rows[i]["Picture"].ToString();
					if (string.IsNullOrWhiteSpace(text2))
					{
						text2 = "/templates/pccommon/images/users/hyzx_25.jpg";
					}
					object[] obj = new object[34]
					{
						text,
						"{\"UserName\":\"",
						DataHelper.GetHiddenUsername(data.Rows[i]["UserName"].ToNullString()),
						"\",\"Picture\":\"",
						Globals.GetImageServerUrl("http://", text2),
						"\",\"ProductId\":\"",
						data.Rows[i]["ProductId"],
						"\",\"ThumbnailUrl100\":\"",
						data.Rows[i]["ThumbnailUrl100"],
						"\",\"ProductName\":\"",
						data.Rows[i]["ProductName"],
						"\",\"SKUContent\":\"",
						data.Rows[i]["SKUContent"],
						"\",\"ReviewText\":\"",
						data.Rows[i]["ReviewText"].ToNullString().Replace("\\", ""),
						"\",\"Score\":\"",
						data.Rows[i]["Score"],
						"\",\"ApplicationPath\":\"\",\"ImageUrl1\":\"",
						Globals.GetImageServerUrl("http://", data.Rows[i]["ImageUrl1"].ToNullString()),
						"\",\"ImageUrl2\":\"",
						Globals.GetImageServerUrl("http://", data.Rows[i]["ImageUrl2"].ToNullString()),
						"\",\"ImageUrl3\":\"",
						Globals.GetImageServerUrl("http://", data.Rows[i]["ImageUrl3"].ToNullString()),
						"\",\"ImageUrl4\":\"",
						Globals.GetImageServerUrl("http://", data.Rows[i]["ImageUrl4"].ToNullString()),
						"\",\"ImageUrl5\":\"",
						Globals.GetImageServerUrl("http://", data.Rows[i]["ImageUrl5"].ToNullString()),
						"\",\"ReplyText\":\"",
						data.Rows[i]["ReplyText"],
						"\",\"ReviewDate\":\"",
						null,
						null,
						null,
						null
					};
					object obj2;
					DateTime value2;
					if (!data.Rows[i]["ReviewDate"].ToDateTime().HasValue)
					{
						obj2 = "";
					}
					else
					{
						value2 = data.Rows[i]["ReviewDate"].ToDateTime().Value;
						obj2 = value2.ToString("yyyy/MM/dd");
					}
					obj[30] = obj2;
					obj[31] = "\",\"ReplyDate\":\"";
					object obj3;
					if (!data.Rows[i]["ReplyDate"].ToDateTime().HasValue)
					{
						obj3 = "";
					}
					else
					{
						value2 = data.Rows[i]["ReplyDate"].ToDateTime().Value;
						obj3 = value2.ToString("yyyy/MM/dd");
					}
					obj[32] = obj3;
					obj[33] = "\"}";
					text = string.Concat(obj);
				}
				str += text.Replace("\n", "").Replace("\t", "").Replace("\r", "");
				str += "]}";
				context.Response.Write(str);
			}
		}

		public void StatisticsReview(HttpContext context)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			string format = "{{\"reviewNum\":\"{0}\",\"reviewNum1\":\"{1}\",\"reviewNum2\":\"{2}\",\"reviewNum3\":\"{3}\",\"reviewNumImg\":\"{4}\"}}";
			if (!string.IsNullOrEmpty(context.Request["ProductId"]))
			{
				DataTable productReviewScore = ProductBrowser.GetProductReviewScore(context.Request["ProductId"].ToInt(0));
				if (productReviewScore != null && productReviewScore.Rows.Count > 0)
				{
					num = productReviewScore.Rows.Count;
					foreach (DataRow row in productReviewScore.Rows)
					{
						if (row["Score"].ToInt(0) > 3)
						{
							num2++;
						}
						else if (row["Score"].ToInt(0) > 1)
						{
							num3++;
						}
						else
						{
							num4++;
						}
						if (row["ImageUrl1"].ToNullString() != "")
						{
							num5++;
						}
					}
				}
			}
			context.Response.Write(string.Format(format, num, num2, num3, num4, num5));
		}
	}
}
