using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Member;
using Hidistro.SqlDal.Members;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class OrderReviews : MemberTemplatedWebControl
	{
		private string orderId;

		private Common_OrderManage_ReviewsOrderItems orderItems;

		private IButton btnRefer;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-OrderReviews.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			if (string.IsNullOrEmpty(base.GetParameter("orderId", false)))
			{
				base.GotoResourceNotFound();
			}
			this.orderId = base.GetParameter("orderId", false);
			this.orderItems = (Common_OrderManage_ReviewsOrderItems)this.FindControl("Common_OrderManage_ReviewsOrderItems");
			this.btnRefer = ButtonManager.Create(this.FindControl("btnRefer"));
			this.btnRefer.Click += this.btnRefer_Click;
			OrderInfo orderInfo = TradeHelper.GetOrderInfo(this.orderId);
			if (orderInfo == null || orderInfo.UserId != HiContext.Current.UserId || HiContext.Current.UserId == 0)
			{
				this.ShowMessage("错误的订单信息", false, "", 1);
			}
			if (orderInfo.OrderStatus != OrderStatus.Finished && (orderInfo.OrderStatus != OrderStatus.Closed || orderInfo.OnlyReturnedCount != orderInfo.LineItems.Count))
			{
				this.ShowMessage("订单还未完成，不能进行评价", false, "", 1);
				this.btnRefer.Visible = false;
			}
			if (!this.Page.IsPostBack)
			{
				if (orderInfo != null && HiContext.Current.UserId != 0 && HiContext.Current.UserId == orderInfo.UserId)
				{
					this.CanToProductReviews(orderInfo);
					this.btnRefer.Text = "提交评论";
					this.BindOrderItems(orderInfo);
					SiteSettings masterSettings = SettingsManager.GetMasterSettings();
					if (masterSettings != null && masterSettings.ProductCommentPoint > 0)
					{
						foreach (RepeaterItem item in this.orderItems.Items)
						{
							HtmlTextArea htmlTextArea = item.FindControl("txtcontent") as HtmlTextArea;
							htmlTextArea?.Attributes.Add("placeholder", "写评价可获得积分奖励");
						}
					}
				}
				else
				{
					this.Page.Response.Redirect("/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("该订单不存在或者不属于当前用户的订单"));
				}
			}
		}

		private void CanToProductReviews(OrderInfo order)
		{
			if (ProductBrowser.CheckAllProductReview(order.OrderId))
			{
				this.Page.Response.Redirect("/user/UserProductReviews.aspx?orderId=" + this.orderId);
			}
		}

		private void BindOrderItems(OrderInfo order)
		{
			DataTable productReviewAll = ProductBrowser.GetProductReviewAll(this.orderId);
			Dictionary<string, LineItemInfo> dictionary = new Dictionary<string, LineItemInfo>();
			LineItemInfo lineItemInfo = new LineItemInfo();
			int num = 0;
			int num2 = 0;
			bool flag = false;
			foreach (KeyValuePair<string, LineItemInfo> lineItem in order.LineItems)
			{
				flag = false;
				lineItemInfo = lineItem.Value;
				for (int i = 0; i < productReviewAll.Rows.Count; i++)
				{
					if (lineItemInfo.ProductId.ToString() == productReviewAll.Rows[i][0].ToString() && lineItemInfo.SkuId.ToString().Trim() == productReviewAll.Rows[i][1].ToString().Trim())
					{
						flag = true;
					}
				}
				if (flag)
				{
					dictionary.Add(lineItem.Key, lineItemInfo);
				}
				else
				{
					num2++;
				}
			}
			if (num + num2 == order.LineItems.Count)
			{
				this.Page.Response.Redirect("UserProductReviews.aspx?orderId=" + this.orderId);
			}
			this.orderItems.DataSource = dictionary.Values;
			this.orderItems.DataBind();
			if (dictionary.Count == 0)
			{
				this.btnRefer.Visible = false;
			}
		}

		private bool ValidateConvert()
		{
			string text = string.Empty;
			if (HiContext.Current.User == null)
			{
				text += Formatter.FormatErrorMessage("请填写用户名和密码");
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.ShowMessage(text, false, "", 1);
				return false;
			}
			return true;
		}

		public void btnRefer_Click(object sender, EventArgs e)
		{
			int num = 0;
			if (this.ValidateConvert())
			{
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(this.orderId);
				if (orderInfo.OrderStatus != OrderStatus.Finished && (orderInfo.OrderStatus != OrderStatus.Closed || orderInfo.OnlyReturnedCount != orderInfo.LineItems.Count))
				{
					this.ShowMessage("您的订单还未完成，因此不能对该商品进行评论！", false, "", 1);
				}
				else
				{
					int num2 = 0;
					int num3 = 0;
					Dictionary<string, ProductReviewInfo> dictionary = new Dictionary<string, ProductReviewInfo>();
					foreach (RepeaterItem item in this.orderItems.Items)
					{
						HtmlInputHidden htmlInputHidden = item.FindControl("hdproductId") as HtmlInputHidden;
						HtmlInputHidden htmlInputHidden2 = item.FindControl("hidScore") as HtmlInputHidden;
						HtmlTextArea htmlTextArea = item.FindControl("txtcontent") as HtmlTextArea;
						HtmlInputHidden htmlInputHidden3 = item.FindControl("hidUploadImages") as HtmlInputHidden;
						Literal literal = item.FindControl("litSKUContent") as Literal;
						string text = Globals.StripAllTags(htmlInputHidden.Value);
						int productId = text.Split('&')[0].ToInt(0);
						ProductBrowser.LoadProductReview(productId, out num2, out num3, this.orderId);
						if (num2 == 0)
						{
							this.ShowMessage("您没有购买此商品(或此商品的订单尚未完成)，因此不能进行评论！", false, "", 1);
							return;
						}
						if (num3 >= num2)
						{
							this.ShowMessage("您已经对此商品进行过评论(或此商品的订单尚未完成)，因此不能再次进行评论！", false, "", 1);
							return;
						}
						if (!string.IsNullOrEmpty(Globals.StripAllTags(htmlTextArea.Value.Trim())) && !string.IsNullOrEmpty(Globals.StripAllTags(htmlInputHidden.Value.Trim())))
						{
							string obj = Globals.StripAllTags(htmlInputHidden2.Value);
							string reviewText = Globals.StripAllTags(htmlTextArea.Value);
							string text2 = Globals.StripAllTags(htmlInputHidden3.Value);
							string skuContent = Globals.StripAllTags(literal.Text);
							if (ProductBrowser.GetProductSimpleInfo(productId) == null)
							{
								this.ShowMessage("您要评论的商品已经不存在！", false, "", 1);
								return;
							}
							string skuId = text.Split('&')[2].ToString();
							ProductReviewInfo productReviewInfo = new ProductReviewInfo();
							productReviewInfo.ReviewDate = DateTime.Now;
							productReviewInfo.ProductId = productId;
							productReviewInfo.UserId = HiContext.Current.UserId;
							productReviewInfo.UserName = HiContext.Current.User.UserName.ToNullString();
							productReviewInfo.UserEmail = HiContext.Current.User.Email.ToNullString();
							productReviewInfo.ReviewText = reviewText;
							productReviewInfo.OrderId = this.orderId;
							productReviewInfo.SkuId = skuId;
							productReviewInfo.SkuContent = skuContent;
							productReviewInfo.Score = obj.ToInt(0);
							int num4 = 0;
							string imageServerUrl = Globals.GetImageServerUrl();
							string[] array = text2.Split(',');
							foreach (string text3 in array)
							{
								if (!string.IsNullOrEmpty(text3))
								{
									string text4 = text3.Replace("//", "/");
									string text5 = string.IsNullOrEmpty(imageServerUrl) ? Globals.SaveFile("review", text4, "/Storage/master/", true, false, "") : text4;
									num4++;
									switch (num4)
									{
									case 1:
										productReviewInfo.ImageUrl1 = text5;
										break;
									case 2:
										productReviewInfo.ImageUrl2 = text5;
										break;
									case 3:
										productReviewInfo.ImageUrl3 = text5;
										break;
									case 4:
										productReviewInfo.ImageUrl4 = text5;
										break;
									case 5:
										productReviewInfo.ImageUrl5 = text5;
										break;
									}
								}
							}
							dictionary.Add(text, productReviewInfo);
						}
					}
					if (dictionary.Count <= 0)
					{
						this.ShowMessage("请输入评价内容呀！", false, "", 1);
					}
					else
					{
						string text6 = "";
						foreach (KeyValuePair<string, ProductReviewInfo> item2 in dictionary)
						{
							ValidationResults validationResults = Validation.Validate(item2.Value, "Refer");
							text6 = string.Empty;
							if (!validationResults.IsValid)
							{
								foreach (ValidationResult item3 in (IEnumerable<ValidationResult>)validationResults)
								{
									text6 += Formatter.FormatErrorMessage(item3.Message);
								}
								break;
							}
							if (!ProductBrowser.InsertProductReview(item2.Value))
							{
								text6 = "评论失败，请重试";
								break;
							}
							MemberInfo user = Users.GetUser(HiContext.Current.UserId);
							SiteSettings masterSettings = SettingsManager.GetMasterSettings();
							if (user != null && masterSettings != null && masterSettings.ProductCommentPoint > 0 && OrderReviews.AddPoints(user, masterSettings.ProductCommentPoint, PointTradeType.ProductCommentPoint))
							{
								num += masterSettings.ProductCommentPoint;
							}
						}
						if (text6 != "")
						{
							this.ShowMessage(text6, false, "", 1);
						}
						else
						{
							if (num > 0)
							{
								Literal literal2 = (Literal)this.FindControl("litCommentCompleted");
								literal2.Text = $"恭喜您获得{num.ToString()}积分奖励！";
							}
							string str = "setCommentCompletedshow();setTimeout(function(){ window.location.href='/User/UserOrders.aspx?orderStatus=21'}," + 3000 + ");";
							this.Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScript", "<script language='JavaScript' defer='defer'>setTimeout(function(){" + str + "},300);</script>");
						}
					}
				}
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
	}
}
