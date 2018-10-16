using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Sales;
using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Xml;

namespace Hidistro.UI.Web.pay
{
	public class app_alipay_Submit : Page
	{
		public string pay_json = string.Empty;

		public string isFightGroup = "false";

		protected HtmlHead Head1;

		protected void Page_Load(object sender, EventArgs e)
		{
			OrderInfo orderInfo = null;
			decimal num = default(decimal);
			string text = base.Request.QueryString.Get("orderId");
			int num2 = base.Request.QueryString.Get("isrecharge").ToInt(0);
			string value;
			if (!string.IsNullOrEmpty(text))
			{
				value = text;
				if (num2 == 1)
				{
					InpourRequestInfo inpourBlance = MemberProcessor.GetInpourBlance(text);
					if (inpourBlance != null)
					{
						num = inpourBlance.InpourBlance;
						goto IL_0183;
					}
				}
				else
				{
					orderInfo = OrderHelper.GetOrderInfo(text);
					if (orderInfo != null)
					{
						this.isFightGroup = ((orderInfo.FightGroupId > 0) ? "true" : "false");
						if (orderInfo.PreSaleId > 0)
						{
							if (!orderInfo.DepositDate.HasValue && orderInfo.OrderStatus == OrderStatus.WaitBuyerPay)
							{
								num = ((orderInfo.Deposit - orderInfo.BalanceAmount > decimal.Zero) ? (orderInfo.Deposit - orderInfo.BalanceAmount) : decimal.Zero);
							}
							else if (orderInfo.DepositDate.HasValue && orderInfo.OrderStatus == OrderStatus.WaitBuyerPay)
							{
								num = orderInfo.FinalPayment;
							}
						}
						else
						{
							num = orderInfo.GetTotal(true);
						}
						value = text + new Random().Next(10000, 99999).ToString();
						goto IL_0183;
					}
				}
			}
			return;
			IL_0183:
			try
			{
				PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode("hishop.plugins.payment.ws_apppay.wswappayrequest");
				string xml = HiCryptographer.Decrypt(paymentMode.Settings);
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(xml);
				XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName("Partner");
				XmlNodeList elementsByTagName2 = xmlDocument.GetElementsByTagName("Seller_account_name");
				XmlNodeList elementsByTagName3 = xmlDocument.GetElementsByTagName("Key");
				if (elementsByTagName == null || elementsByTagName2 == null || elementsByTagName3 == null)
				{
					this.pay_json = "config_error";
				}
				else
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append("partner=\"");
					stringBuilder.Append(xmlDocument.GetElementsByTagName("Partner")[0].InnerText);
					stringBuilder.Append("\"&out_trade_no=\"");
					stringBuilder.Append(value);
					stringBuilder.Append("\"&subject=\"");
					stringBuilder.Append((num2 == 1) ? "预付款充值" : "订单支付");
					stringBuilder.Append("\"&body=\"");
					stringBuilder.Append("订单号-").Append(text);
					stringBuilder.Append("\"&total_fee=\"");
					stringBuilder.Append(num.F2ToString("f2"));
					stringBuilder.Append("\"&notify_url=\"");
					stringBuilder.Append(Globals.UrlEncode(Globals.FullPath("/pay/app_alipay_notify_url")));
					stringBuilder.Append("\"&service=\"mobile.securitypay.pay");
					stringBuilder.Append("\"&_input_charset=\"UTF-8");
					stringBuilder.Append("\"&return_url=\"");
					stringBuilder.Append(Globals.UrlEncode("http://m.alipay.com"));
					stringBuilder.Append("\"&payment_type=\"1");
					stringBuilder.Append("\"&seller_id=\"");
					stringBuilder.Append(xmlDocument.GetElementsByTagName("Seller_account_name")[0].InnerText);
					stringBuilder.Append("\"&it_b_pay=\"1m\"");
					string str = Globals.UrlEncode(RSAFromPkcs8.sign(stringBuilder.ToString(), xmlDocument.GetElementsByTagName("Key")[0].InnerText, "utf-8"));
					this.pay_json = stringBuilder.ToString() + "&sign=\"" + str + "\"&sign_type=\"RSA\"";
				}
			}
			catch (Exception)
			{
				this.pay_json = "config_error";
			}
		}
	}
}
