using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Shopping;
using System;
using System.Collections.Specialized;
using System.Reflection;
using System.Web;

namespace Hidistro.UI.Web.API
{
	public class TaobaoOrderHandler : IHttpHandler
	{
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
			GzipExtention.Gzip(context);
			string text = context.Request.Form["action"];
			string a = text;
			if (a == "OrderAdd")
			{
				this.ProcessOrderAdd(context);
			}
		}

		private void ProcessOrderAdd(HttpContext context)
		{
			OrderInfo orderInfo = new OrderInfo();
			try
			{
				string text = context.Request.Form["TaobaoOrderId"];
				if (string.IsNullOrEmpty(text) || ShoppingProcessor.IsExistOuterOrder("tb_" + text))
				{
					context.Response.Write("0");
				}
				else
				{
					orderInfo.OrderId = this.GenerateOrderId();
					orderInfo.OuterOrderId = "tb_" + context.Request.Form["TaobaoOrderId"];
					orderInfo.Remark = context.Request.Form["BuyerMemo"] + context.Request.Form["BuyerMessage"];
					string text2 = context.Request.Form["SellerFlag"];
					if (!string.IsNullOrEmpty(text2) && text2 != "0")
					{
						orderInfo.ManagerMark = (OrderMark)int.Parse(text2);
					}
					orderInfo.ManagerRemark = context.Request.Form["SellerMemo"];
					orderInfo.OrderDate = DateTime.Parse(context.Request.Form["OrderDate"]);
					orderInfo.PayDate = DateTime.Parse(context.Request.Form["PayDate"]);
					orderInfo.UserId = 1100;
					OrderInfo orderInfo2 = orderInfo;
					OrderInfo orderInfo3 = orderInfo;
					string text5 = orderInfo2.RealName = (orderInfo3.Username = context.Request.Form["Username"]);
					orderInfo.EmailAddress = context.Request.Form["EmailAddress"];
					orderInfo.ShipTo = context.Request.Form["ShipTo"];
					orderInfo.ShippingRegion = context.Request.Form["ReceiverState"] + context.Request.Form["ReceiverCity"] + context.Request.Form["ReceiverDistrict"];
					orderInfo.RegionId = RegionHelper.GetRegionId(context.Request.Form["ReceiverDistrict"], context.Request.Form["ReceiverCity"], context.Request.Form["ReceiverState"]);
					orderInfo.FullRegionPath = RegionHelper.GetFullPath(orderInfo.RegionId, true);
					orderInfo.Address = context.Request.Form["ReceiverAddress"];
					orderInfo.TelPhone = context.Request.Form["TelPhone"];
					orderInfo.CellPhone = context.Request.Form["CellPhone"];
					OrderInfo orderInfo4 = orderInfo;
					OrderInfo orderInfo5 = orderInfo;
					int num3 = orderInfo4.RealShippingModeId = (orderInfo5.ShippingModeId = 0);
					OrderInfo orderInfo6 = orderInfo;
					OrderInfo orderInfo7 = orderInfo;
					text5 = (orderInfo6.RealModeName = (orderInfo7.ModeName = context.Request.Form["ModeName"]));
					orderInfo.PaymentType = "支付宝担宝交易";
					orderInfo.Gateway = "hishop.plugins.payment.alipayassure.assurerequest";
					orderInfo.AdjustedDiscount = decimal.Zero;
					string text8 = context.Request.Form["Products"];
					if (string.IsNullOrEmpty(text8))
					{
						context.Response.Write("-1");
					}
					else
					{
						string[] array = text8.Split('|');
						if (array.Length == 0)
						{
							context.Response.Write("-2");
						}
						else
						{
							string[] array2 = array;
							decimal num9;
							foreach (string text9 in array2)
							{
								string[] array3 = text9.Split(',');
								LineItemInfo lineItemInfo = new LineItemInfo();
								int productId = 0;
								int.TryParse(array3[1], out productId);
								int num4 = 1;
								int.TryParse(array3[3], out num4);
								lineItemInfo.SkuId = array3[0];
								lineItemInfo.ProductId = productId;
								lineItemInfo.SKU = array3[2];
								LineItemInfo lineItemInfo2 = lineItemInfo;
								LineItemInfo lineItemInfo3 = lineItemInfo;
								num3 = (lineItemInfo2.Quantity = (lineItemInfo3.ShipmentQuantity = num4));
								LineItemInfo lineItemInfo4 = lineItemInfo;
								LineItemInfo lineItemInfo5 = lineItemInfo;
								num9 = (lineItemInfo4.ItemCostPrice = (lineItemInfo5.ItemAdjustedPrice = decimal.Parse(array3[4])));
								lineItemInfo.ItemListPrice = decimal.Parse(array3[5]);
								lineItemInfo.ItemDescription = HttpUtility.UrlDecode(array3[6]);
								lineItemInfo.ThumbnailsUrl = array3[7];
								lineItemInfo.ItemWeight = decimal.Zero;
								lineItemInfo.SKUContent = array3[8];
								lineItemInfo.PromotionId = 0;
								lineItemInfo.PromotionName = "";
								orderInfo.ParentOrderId = "0";
								orderInfo.LineItems.Add(lineItemInfo.SkuId, lineItemInfo);
							}
							OrderInfo orderInfo8 = orderInfo;
							OrderInfo orderInfo9 = orderInfo;
							num9 = (orderInfo8.AdjustedFreight = (orderInfo9.Freight = decimal.Parse(context.Request.Form["PostFee"])));
							orderInfo.OrderStatus = OrderStatus.BuyerAlreadyPaid;
							orderInfo.OrderSource = OrderSource.Taobao;
							if (ShoppingProcessor.CreatOrder(orderInfo))
							{
								context.Response.Write("1");
							}
							else
							{
								context.Response.Write("0");
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				NameValueCollection nameValueCollection = new NameValueCollection
				{
					HttpContext.Current.Request.Form,
					HttpContext.Current.Request.QueryString
				};
				nameValueCollection.Add("ErrorMessage", ex.Message);
				nameValueCollection.Add("StackTrace", ex.StackTrace);
				if (ex.InnerException != null)
				{
					nameValueCollection.Add("InnerException", ex.InnerException.ToString());
				}
				if (ex.GetBaseException() != null)
				{
					nameValueCollection.Add("BaseException", ex.GetBaseException().Message);
				}
				if (ex.TargetSite != (MethodBase)null)
				{
					nameValueCollection.Add("TargetSite", ex.TargetSite.ToString());
				}
				nameValueCollection.Add("ExSource", ex.Source);
				Globals.AppendLog(nameValueCollection, "", "", HttpContext.Current.Request.Url.ToString(), "TaobaoOrder");
			}
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
