using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Sales;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.Admin.sales
{
	public class BatchPrintSendOrderGoods : AdminCallBackPage
	{
		protected HtmlHead Head1;

		protected HtmlGenericControl divContent;

		protected void Page_Load(object sender, EventArgs e)
		{
			string orderIds = base.Request["orderIds"].Trim(',');
			if (!string.IsNullOrEmpty(base.Request["orderIds"]))
			{
				foreach (OrderInfo printDatum in this.GetPrintData(orderIds))
				{
					HtmlGenericControl htmlGenericControl = new HtmlGenericControl("div");
					htmlGenericControl.Attributes["class"] = "order1 print";
					StringBuilder stringBuilder = new StringBuilder("");
					stringBuilder.AppendFormat("<div class=\"info\"><div class=\"prime-info\" style=\"margin-right: 20px;\"><p><span><h3>{0}</h3></span></p></div><ul class=\"sub-info\"><li><span>手机号码： </span>{3}</li><li><span>生成时间： </span>{1}</li><li><span>订单编号： </span>{2}</li></ul><br class=\"clear\" /></div>", printDatum.ShipTo.ToNullString(), printDatum.OrderDate.ToString("yyyy-MM-dd HH:mm"), printDatum.OrderId, printDatum.CellPhone);
					stringBuilder.Append("<table><col class=\"col-0\" /><col class=\"col-1\" /><col class=\"col-2\" /><col class=\"col-3\" /><col class=\"col-4\" /><col class=\"col-5\" /><thead><tr><th>货号</th><th>商品名称</th><th>规格</th><th>数量</th><th nowrap=\"nowrap\">单价</th><th>总价</th></tr></thead><tbody>");
					Dictionary<string, LineItemInfo> lineItems = printDatum.LineItems;
					if (lineItems != null)
					{
						foreach (string key in lineItems.Keys)
						{
							LineItemInfo lineItemInfo = lineItems[key];
							if (lineItemInfo.Status != LineItemStatus.Refunded && lineItemInfo.Status != LineItemStatus.Returned)
							{
								stringBuilder.AppendFormat("<tr><td nowrap=\"nowrap\">{0}</td>", lineItemInfo.SKU);
								stringBuilder.AppendFormat("<td>{0}</td>", lineItemInfo.ItemDescription);
								stringBuilder.AppendFormat("<td>{0}</td>", lineItemInfo.SKUContent);
								stringBuilder.AppendFormat("<td nowrap=\"nowrap\">{0}</td>", lineItemInfo.ShipmentQuantity);
								stringBuilder.AppendFormat("<td nowrap=\"nowrap\">{0}</td>", Math.Round(lineItemInfo.ItemListPrice, 2));
								stringBuilder.AppendFormat("<td nowrap=\"nowrap\">{0}</td></tr>", Math.Round(lineItemInfo.GetSubTotal(), 2));
							}
						}
					}
					stringBuilder.Append("</tbody></table>");
					string value = "";
					IList<OrderGiftInfo> gifts = printDatum.Gifts;
					if (gifts != null && gifts.Count > 0)
					{
						OrderGiftInfo orderGiftInfo = gifts[0];
						stringBuilder.Append("<p style=\"text-align:left;\"><b>&nbsp;</b></p>");
						stringBuilder.Append("<table><col class=\"col-0\" /><col class=\"col-1\" /><col class=\"col-2\" /><col class=\"col-3\" /><thead><tr><th>礼品名称</th><th>市场参考价</th><th>数量</th><th>类型</th></tr></thead><tbody>");
						foreach (OrderGiftInfo gift in printDatum.Gifts)
						{
							stringBuilder.AppendFormat("<tr><td nowrap=\"nowrap\">{0}</td>", gift.GiftName);
							stringBuilder.AppendFormat("<td>{0}</td>", gift.CostPrice.F2ToString("f2"));
							stringBuilder.AppendFormat("<td>{0}</td>", gift.Quantity);
							stringBuilder.AppendFormat("<td nowrap=\"nowrap\">{0}</td>", (gift.PromoteType == 5) ? "商品促销" : ((gift.PromoteType == 15) ? "订单促销" : "积分兑换"));
						}
						stringBuilder.Append("</tbody></table>");
					}
					stringBuilder.AppendFormat("<ul class=\"price\">");
					if (!string.IsNullOrEmpty(printDatum.InvoiceTitle))
					{
						stringBuilder.AppendFormat("<li><span>发票抬头：</span>{0}</li>", printDatum.InvoiceTitle);
						if (printDatum.InvoiceType == InvoiceType.Enterprise)
						{
							stringBuilder.AppendFormat("<li><span>企业纳税人识别号：</span>{0}</li>", printDatum.InvoiceTaxpayerNumber);
						}
					}
					stringBuilder.AppendFormat("<li><span>收货地址： </span>{0}</li><li><span>送货上门时间： </span>{1}</li></ul>", printDatum.ShippingRegion + printDatum.Address, printDatum.ShipToDate);
					stringBuilder.AppendFormat("<br class=\"clear\" /><ul class=\"price\"><li><span>商品总价： </span>{0}</li><li><span>运费： </span>{1}</li>", Math.Round(printDatum.GetAmount(false), 2), Math.Round(printDatum.AdjustedFreight, 2));
					decimal reducedPromotionAmount = printDatum.ReducedPromotionAmount;
					if (reducedPromotionAmount > decimal.Zero)
					{
						stringBuilder.AppendFormat("<li><span>优惠金额：</span>{0}</li>", Math.Round(reducedPromotionAmount, 2));
					}
					if (!string.IsNullOrEmpty(printDatum.CouponCode))
					{
						decimal couponValue = printDatum.CouponValue;
						if (couponValue > decimal.Zero)
						{
							stringBuilder.AppendFormat("<li><span>优惠券：</span>{0}</li>", Math.Round(couponValue, 2));
						}
					}
					decimal adjustedDiscount = printDatum.AdjustedDiscount;
					if (adjustedDiscount > decimal.Zero)
					{
						stringBuilder.AppendFormat("<li><span>管理员手工加价：</span>{0}</li>", Math.Round(adjustedDiscount, 2));
					}
					else
					{
						stringBuilder.AppendFormat("<li><span>管理员手工减价：</span>{0}</li>", Math.Round(-adjustedDiscount, 2));
					}
					stringBuilder.Append(value);
					stringBuilder.AppendFormat("<li><span>实付金额：</span>{0}</li>", Math.Round(printDatum.GetTotal(false), 2));
					stringBuilder.AppendFormat("<li><span>支付方式：</span>{0}</li></ul>", printDatum.PaymentType);
					if (HiContext.Current.SiteSettings.IsOpenCertification && printDatum.IsincludeCrossBorderGoods && printDatum.OrderStatus != OrderStatus.WaitBuyerPay)
					{
						StringBuilder stringBuilder2 = new StringBuilder();
						if (!string.IsNullOrWhiteSpace(printDatum.IDNumber) || !string.IsNullOrWhiteSpace(printDatum.IDImage1) || !string.IsNullOrWhiteSpace(printDatum.IDImage2))
						{
							stringBuilder2.AppendFormat("<li><span class=\"txtright\"> 收货人：</span>{0}</li>", printDatum.ShipTo);
						}
						if (!string.IsNullOrWhiteSpace(printDatum.IDNumber))
						{
							stringBuilder2.AppendFormat("<li><span>身份证号：</span>{0}</li>", HiCryptographer.Decrypt(printDatum.IDNumber));
						}
						if (HiContext.Current.SiteSettings.CertificationModel == 2 && ((!string.IsNullOrWhiteSpace(printDatum.IDImage1) && this.IsImageUrlExists(printDatum.IDImage1)) || (!string.IsNullOrWhiteSpace(printDatum.IDImage2) && this.IsImageUrlExists(printDatum.IDImage2))))
						{
							stringBuilder2.AppendFormat("<li><span>身份证照：</span><div>");
							if (!string.IsNullOrWhiteSpace(printDatum.IDImage1))
							{
								stringBuilder2.AppendFormat("<img src='{0}' runat ='server' alt='证件照正面' clientidmode='Static'/>", printDatum.IDImage1);
							}
							if (!string.IsNullOrWhiteSpace(printDatum.IDImage2))
							{
								stringBuilder2.AppendFormat("<img src='{0}' runat ='server' alt='证件照反面' clientidmode ='Static'/>", printDatum.IDImage2);
							}
							stringBuilder2.AppendFormat("{0}", stringBuilder2.ToString().Contains("img") ? "</div></li>" : "");
						}
						if (stringBuilder2.ToString() != string.Empty)
						{
							stringBuilder2.AppendFormat("</ul>");
							stringBuilder2.Insert(0, "<br class=\"clear\" /><ul class=\"idInfo\">");
							stringBuilder.Append(stringBuilder2.ToString());
						}
					}
					stringBuilder.AppendFormat("<br class=\"clear\" /><ul class=\"price\"><li><span>备注： </span>{0}</li></ul><br class=\"clear\" /><br><br>", printDatum.Remark);
					htmlGenericControl.InnerHtml = stringBuilder.ToString();
					this.divContent.Controls.AddAt(0, htmlGenericControl);
				}
			}
		}

		private List<OrderInfo> GetPrintData(string orderIds)
		{
			List<OrderInfo> list = new List<OrderInfo>();
			string[] array = orderIds.Split(',');
			foreach (string orderId in array)
			{
				OrderInfo orderInfo = OrderHelper.GetOrderInfo(orderId);
				if (orderInfo != null && orderInfo.ItemStatus == OrderItemStatus.Nomarl && orderInfo.StoreId == 0 && (orderInfo.OrderStatus == OrderStatus.BuyerAlreadyPaid || (orderInfo.Gateway == "hishop.plugins.payment.podrequest" && orderInfo.OrderStatus == OrderStatus.WaitBuyerPay) || orderInfo.OrderStatus == OrderStatus.SellerAlreadySent))
				{
					list.Add(orderInfo);
				}
			}
			return list;
		}

		private bool IsImageUrlExists(string imageUrl)
		{
			bool result = false;
			try
			{
				if (File.Exists(base.Server.MapPath("~" + imageUrl)))
				{
					result = true;
				}
			}
			catch
			{
			}
			return result;
		}
	}
}
