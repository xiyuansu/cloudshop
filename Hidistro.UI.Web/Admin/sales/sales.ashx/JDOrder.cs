using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Shopping;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Web.ashxBase;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace Hidistro.UI.Web.Admin.sales.ashx
{
	public class JDOrder : AdminBaseHandler, IRequiresSessionState
	{
		private HttpContext CurContext;

		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			this.CurContext = context;
			if (string.IsNullOrWhiteSpace(base.action))
			{
				throw new HidistroAshxException("错误的参数");
			}
			base.action = base.action.ToLower();
			string action = base.action;
			if (!(action == "getlist"))
			{
				if (action == "down")
				{
					this.Down(context);
					return;
				}
				throw new HidistroAshxException("错误的参数");
			}
			this.GetList(context);
		}

		public void GetList(HttpContext context)
		{
			DateTime? dateTimeParam = base.GetDateTimeParam(context, "StartDate");
			DateTime? dateTimeParam2 = base.GetDateTimeParam(context, "EndDate");
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList(base.CurrentPageIndex, base.CurrentPageSize, dateTimeParam, dateTimeParam2);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList(int page, int pagesize, DateTime? StartDate = default(DateTime?), DateTime? EndDate = default(DateTime?))
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			dataGridViewModel.rows = new List<Dictionary<string, object>>();
			string text = "";
			string text2 = "";
			DateTime dateTime;
			if (StartDate.HasValue && EndDate.HasValue)
			{
				dateTime = StartDate.Value;
				text = dateTime.ToString("yyyy-MM-dd 00:00:00");
				dateTime = EndDate.Value;
				text2 = dateTime.ToString("yyyy-MM-dd 23:59:59");
			}
			string value = $"{{\"start_date\":\"{text}\",\"end_date\":\"{text2}\",\"order_state\":\"WAIT_SELLER_STOCK_OUT\",\"page\":\"{page}\",\"page_size\":\"{pagesize}\",\"optional_fields\":\"\",\"sortType\":\"\",\"dateType\":\"\"}}";
			SortedDictionary<string, string> sortedDictionary = new SortedDictionary<string, string>();
			sortedDictionary.Add("method", "360buy.order.search");
			sortedDictionary.Add("access_token", base.CurrentSiteSetting.JDAccessToken);
			sortedDictionary.Add("app_key", base.CurrentSiteSetting.JDAppKey);
			SortedDictionary<string, string> sortedDictionary2 = sortedDictionary;
			dateTime = DateTime.Now;
			sortedDictionary2.Add("timestamp", dateTime.ToString("yyyy-MM-dd HH:mm:ss"));
			sortedDictionary.Add("360buy_param_json", value);
			sortedDictionary.Add("v", "2.0");
			string value2 = JDHelper.Sign(base.CurrentSiteSetting.JDAppSecret, sortedDictionary);
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<string, string> item2 in sortedDictionary)
			{
				stringBuilder.Append(item2.Key + "=" + HttpUtility.UrlEncode(item2.Value, Encoding.UTF8) + "&");
			}
			stringBuilder.Append("sign=");
			stringBuilder.Append(value2);
			string postResult = Globals.GetPostResult("https://api.jd.com/routerjson", stringBuilder.ToString());
			PageModel<JDOrderModel> pageModel = new PageModel<JDOrderModel>();
			try
			{
				pageModel = JDHelper.ParseJDOrderList(postResult);
			}
			catch (Exception ex)
			{
				base.ReturnFailResult(this.CurContext, ex.Message, -1, true);
			}
			this.CurContext.Session["jdOrder"] = pageModel;
			dataGridViewModel.total = pageModel.Total;
			foreach (JDOrderModel model in pageModel.Models)
			{
				model.IsExsit = ShoppingProcessor.IsExistOuterOrder("jd_" + model.OrderId);
				if (model.IsExsit)
				{
					model.Status = "已下载";
				}
				Dictionary<string, object> item = model.ToDictionary();
				dataGridViewModel.rows.Add(item);
			}
			return dataGridViewModel;
		}

		private void Down(HttpContext context)
		{
			string parameter = base.GetParameter(context, "ids", true);
			if (context.Session["jdOrder"] != null && !string.IsNullOrEmpty(parameter))
			{
				string text = "";
				text = parameter;
				string[] idArray = text.Split(',');
				PageModel<JDOrderModel> pageModel = (PageModel<JDOrderModel>)context.Session["jdOrder"];
				int i;
				for (i = 0; i < idArray.Length; i++)
				{
					JDOrderModel jDOrderModel = ((List<JDOrderModel>)pageModel.Models).Find(delegate(JDOrderModel jdOrder)
					{
						if (jdOrder.OrderId.Equals(idArray[i]))
						{
							return true;
						}
						return false;
					});
					if (jDOrderModel != null)
					{
						OrderInfo orderInfo = new OrderInfo();
						try
						{
							if (!ShoppingProcessor.IsExistOuterOrder("jd_" + jDOrderModel.OrderId))
							{
								orderInfo.OrderId = this.GenerateOrderId();
								orderInfo.OuterOrderId = "jd_" + jDOrderModel.OrderId;
								orderInfo.Remark = jDOrderModel.OrderReMark;
								orderInfo.ManagerRemark = jDOrderModel.OrderManagerReMark;
								orderInfo.OrderDate = DateTime.Parse(jDOrderModel.CreatedAt);
								orderInfo.PayDate = DateTime.Parse(jDOrderModel.ModifyAt);
								orderInfo.UserId = 1100;
								OrderInfo orderInfo2 = orderInfo;
								OrderInfo orderInfo3 = orderInfo;
								string text3 = orderInfo2.RealName = (orderInfo3.Username = jDOrderModel.Consignee.FullName);
								orderInfo.EmailAddress = "";
								orderInfo.ShipTo = jDOrderModel.Consignee.FullName;
								orderInfo.ShippingRegion = jDOrderModel.Consignee.Province + jDOrderModel.Consignee.City + jDOrderModel.Consignee.County;
								orderInfo.RegionId = RegionHelper.GetRegionId(jDOrderModel.Consignee.County, jDOrderModel.Consignee.City, jDOrderModel.Consignee.Province);
								orderInfo.FullRegionPath = RegionHelper.GetFullPath(orderInfo.RegionId, true);
								orderInfo.Address = jDOrderModel.Consignee.FullAddress;
								orderInfo.TelPhone = jDOrderModel.Consignee.Telephone;
								orderInfo.CellPhone = jDOrderModel.Consignee.Mobile;
								orderInfo.ZipCode = "";
								OrderInfo orderInfo4 = orderInfo;
								OrderInfo orderInfo5 = orderInfo;
								int num3 = orderInfo4.RealShippingModeId = (orderInfo5.ShippingModeId = 0);
								OrderInfo orderInfo6 = orderInfo;
								OrderInfo orderInfo7 = orderInfo;
								text3 = (orderInfo6.RealModeName = (orderInfo7.ModeName = ""));
								orderInfo.PaymentType = jDOrderModel.PayType;
								orderInfo.Gateway = "";
								orderInfo.AdjustedDiscount = decimal.Zero;
								if (jDOrderModel.Products.Count > 0)
								{
									decimal num8;
									foreach (JDOrderItemModel product in jDOrderModel.Products)
									{
										LineItemInfo lineItemInfo = new LineItemInfo();
										lineItemInfo.SkuId = product.SkuId;
										lineItemInfo.ProductId = Convert.ToInt32(product.ProductId);
										lineItemInfo.SKU = "";
										LineItemInfo lineItemInfo2 = lineItemInfo;
										LineItemInfo lineItemInfo3 = lineItemInfo;
										num3 = (lineItemInfo2.Quantity = (lineItemInfo3.ShipmentQuantity = Convert.ToInt32(product.Total)));
										LineItemInfo lineItemInfo4 = lineItemInfo;
										LineItemInfo lineItemInfo5 = lineItemInfo;
										num8 = (lineItemInfo4.ItemCostPrice = (lineItemInfo5.ItemAdjustedPrice = decimal.Parse(product.Price)));
										lineItemInfo.ItemListPrice = decimal.Parse(product.Price);
										lineItemInfo.ItemDescription = "";
										lineItemInfo.ThumbnailsUrl = "";
										lineItemInfo.ItemWeight = decimal.Zero;
										lineItemInfo.SKUContent = product.SkuName;
										lineItemInfo.PromotionId = 0;
										lineItemInfo.PromotionName = "";
										orderInfo.LineItems.Add(lineItemInfo.SkuId, lineItemInfo);
									}
									OrderInfo orderInfo8 = orderInfo;
									OrderInfo orderInfo9 = orderInfo;
									num8 = (orderInfo8.AdjustedFreight = (orderInfo9.Freight = decimal.Parse(jDOrderModel.Freight)));
									orderInfo.OrderStatus = OrderStatus.BuyerAlreadyPaid;
									orderInfo.OrderSource = OrderSource.JD;
									if (ShoppingProcessor.CreatOrder(orderInfo))
									{
										jDOrderModel.IsExsit = true;
										jDOrderModel.Status = "已下载";
									}
									else
									{
										jDOrderModel.Status = "下载失败";
									}
									continue;
								}
								goto end_IL_00c4;
							}
							return;
							end_IL_00c4:;
						}
						catch (Exception ex)
						{
							NameValueCollection param = new NameValueCollection
							{
								HttpContext.Current.Request.Form,
								HttpContext.Current.Request.QueryString
							};
							Globals.WriteExceptionLog_Page(ex, param, "JDOrder");
							continue;
						}
						break;
					}
				}
				base.ReturnSuccessResult(context, "操作完成", 0, true);
				return;
			}
			throw new HidistroAshxException("无数据");
		}

		private string GenerateOrderId()
		{
			string text = string.Empty;
			Random random = new Random();
			for (int i = 0; i < 7; i++)
			{
				int num = random.Next();
				text += ((char)(ushort)(48 + (ushort)(num % 10))).ToString();
			}
			return DateTime.Now.ToString("yyyyMMdd") + text;
		}
	}
}
