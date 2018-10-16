using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Shopping;
using Hishop.Weixin.Pay;
using Hishop.Weixin.Pay.Domain;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.pay
{
	public class H5WxPay_Submit : Page
	{
		public string pay_json = string.Empty;

		public string pay_uri = string.Empty;

		public string isFightGroup = "false";

		public string isOfflineOrder = "false";

		public string IsServiceOrder = "false";

		protected HtmlInputHidden hidOrderId;

		protected HtmlInputHidden hidOrderSource;

		protected Literal litError;

		protected void Page_Load(object sender, EventArgs e)
		{
			NameValueCollection headers = base.Request.Headers;
			string text = "";
			if (headers.AllKeys.Contains("Forwarded"))
			{
				text = headers["Forwarded"].ToNullString();
			}
			OrderInfo orderInfo = null;
			PackageInfo packageInfo = new PackageInfo();
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			bool flag = this.Page.Request["IsOffline"].ToBool();
			string a = this.Page.Request["from"].ToNullString().ToLower();
			string text2 = this.Page.Request.QueryString.Get("orderId");
			string empty = string.Empty;
			string empty2 = string.Empty;
			decimal d;
			if (a == "appstore")
			{
				this.isOfflineOrder = "true";
				if (string.IsNullOrEmpty(text2))
				{
					this.litError.Text = "错误的订单号，不能进行支付!";
					return;
				}
				empty = text2;
				d = default(decimal);
				StoreCollectionInfo storeCollectionInfo = null;
				if (flag)
				{
					this.isOfflineOrder = "true";
					storeCollectionInfo = StoresHelper.GetStoreCollectionInfo(text2);
					if (storeCollectionInfo == null)
					{
						this.litError.Text = "订单状态错误，不能进行支付!";
					}
					else if (storeCollectionInfo.Status == 0)
					{
						d = storeCollectionInfo.PayAmount;
						goto IL_0309;
					}
					return;
				}
				orderInfo = ShoppingProcessor.GetOrderInfo(text2);
				if (orderInfo == null)
				{
					this.litError.Text = "错误的订单信息，不能进行支付!";
					return;
				}
				if (orderInfo.OrderStatus != OrderStatus.WaitBuyerPay)
				{
					if (orderInfo.OrderSource == OrderSource.Alioh)
					{
						base.Response.Redirect("/AliOH/MemberOrderDetails?OrderId=" + orderInfo.OrderId);
					}
					else
					{
						base.Response.Redirect("/WapShop/MemberOrderDetails?OrderId=" + orderInfo.OrderId);
					}
					return;
				}
				this.IsServiceOrder = ((orderInfo.OrderType == OrderType.ServiceOrder) ? "true" : "false");
				if (orderInfo.PreSaleId > 0)
				{
					if (!orderInfo.DepositDate.HasValue && orderInfo.OrderStatus == OrderStatus.WaitBuyerPay)
					{
						empty = orderInfo.OrderId;
						d = orderInfo.Deposit - orderInfo.BalanceAmount;
					}
					if (orderInfo.DepositDate.HasValue && orderInfo.OrderStatus == OrderStatus.WaitBuyerPay)
					{
						if (orderInfo.PayRandCode.ToInt(0) == 0)
						{
							int num = orderInfo.PayRandCode.ToInt(0);
							num = ((num >= 100) ? (num + 1) : 100);
							orderInfo.PayRandCode = num.ToString();
							OrderHelper.UpdateOrderPaymentTypeOfAPI(orderInfo);
						}
						empty = orderInfo.PayOrderId;
						d = orderInfo.FinalPayment;
					}
				}
				else
				{
					empty = orderInfo.PayOrderId;
					d = orderInfo.GetTotal(true);
				}
				goto IL_0309;
			}
			if (string.IsNullOrEmpty(text2))
			{
				this.litError.Text = "订单状态错误，不能进行支付!";
				return;
			}
			orderInfo = OrderHelper.GetOrderInfo(text2);
			if (orderInfo == null)
			{
				this.litError.Text = "订单状态错误，不能进行支付!";
				return;
			}
			if (orderInfo.OrderStatus != OrderStatus.WaitBuyerPay)
			{
				if (orderInfo.OrderSource == OrderSource.Alioh)
				{
					base.Response.Redirect("/AliOH/MemberOrderDetails?OrderId=" + orderInfo.OrderId);
				}
				else
				{
					base.Response.Redirect("/WapShop/MemberOrderDetails?OrderId=" + orderInfo.OrderId);
				}
				return;
			}
			empty = text2;
			this.isFightGroup = ((orderInfo.FightGroupId > 0) ? "true" : "false");
			decimal d2 = default(decimal);
			if (orderInfo.PreSaleId > 0)
			{
				if (!orderInfo.DepositDate.HasValue && orderInfo.OrderStatus == OrderStatus.WaitBuyerPay)
				{
					empty = orderInfo.OrderId;
					d2 = ((orderInfo.Deposit - orderInfo.BalanceAmount > decimal.Zero) ? (orderInfo.Deposit - orderInfo.BalanceAmount) : decimal.Zero);
				}
				if (orderInfo.DepositDate.HasValue && orderInfo.OrderStatus == OrderStatus.WaitBuyerPay)
				{
					if (orderInfo.PayRandCode.ToInt(0) == 0)
					{
						int num2 = orderInfo.PayRandCode.ToInt(0);
						num2 = ((num2 >= 100) ? (num2 + 1) : 100);
						orderInfo.PayRandCode = num2.ToString();
						OrderHelper.UpdateOrderPaymentTypeOfAPI(orderInfo);
					}
					empty = orderInfo.PayOrderId;
					d2 = ((orderInfo.FinalPayment > decimal.Zero) ? orderInfo.FinalPayment : decimal.Zero);
				}
			}
			else
			{
				empty = orderInfo.PayOrderId;
				d2 = orderInfo.GetTotal(true);
			}
			packageInfo.Body = orderInfo.OrderId;
			packageInfo.NotifyUrl = Globals.GetProtocal(HttpContext.Current) + "://" + $"{this.Page.Request.Url.Host}/pay/wx_Pay";
			if (orderInfo.OrderType != OrderType.ServiceOrder && orderInfo.OrderSource == OrderSource.Applet)
			{
				packageInfo.NotifyUrl = Globals.GetProtocal(HttpContext.Current) + "://" + $"{this.Page.Request.Url.Host}/pay/O2OApplet_Pay";
			}
			packageInfo.OutTradeNo = empty;
			packageInfo.Attach = empty2;
			packageInfo.TotalFee = (int)(d2 * 100m);
			if (packageInfo.TotalFee < decimal.One)
			{
				packageInfo.TotalFee = decimal.One;
			}
			goto IL_06db;
			IL_06db:
			packageInfo.SpbillCreateIp = ((string.IsNullOrEmpty(text) || !Globals.IsIpAddress(text)) ? Globals.GetIPAddress(HttpContext.Current) : text);
			string text3 = masterSettings.WeixinAppId;
			string appSecret = masterSettings.WeixinAppSecret;
			string text4 = masterSettings.WeixinPartnerID;
			string partnerKey = masterSettings.WeixinPartnerKey;
			string text5 = masterSettings.Main_Mch_ID;
			string text6 = masterSettings.Main_AppId;
			if (orderInfo.OrderType == OrderType.ServiceOrder)
			{
				text3 = masterSettings.O2OAppletAppId;
				appSecret = masterSettings.O2OAppletAppSecrect;
				text4 = masterSettings.O2OAppletMchId;
				partnerKey = masterSettings.O2OAppletKey;
				text5 = "";
				text6 = "";
			}
			else if (orderInfo.OrderSource == OrderSource.Applet)
			{
				text3 = masterSettings.WxAppletAppId;
				appSecret = masterSettings.WxAppletAppSecrect;
				text4 = masterSettings.WxApplectMchId;
				partnerKey = masterSettings.WxApplectKey;
				text5 = "";
				text6 = "";
			}
			string text7 = "";
			if (!string.IsNullOrEmpty(text6) && !string.IsNullOrEmpty(text5))
			{
				packageInfo.sub_openid = text7;
			}
			else
			{
				packageInfo.OpenId = text7;
			}
			PayClient payClient = null;
			payClient = ((string.IsNullOrEmpty(text6) || string.IsNullOrEmpty(text5)) ? new PayClient(text3, appSecret, text4, partnerKey, "", "", "", "") : new PayClient(text6, appSecret, text5, partnerKey, "", text4, text3, ""));
			Globals.AppendLog(JsonHelper.GetJson(packageInfo), "", "", "H5PaySubmit");
			PayRequestInfo payRequestInfo = payClient.BuildH5PayRequest(packageInfo);
			this.pay_uri = payRequestInfo.mweb_url;
			return;
			IL_0309:
			packageInfo.Body = text2;
			packageInfo.NotifyUrl = Globals.GetProtocal(HttpContext.Current) + "://" + $"{this.Page.Request.Url.Host}/pay/AppStore_wxPay";
			packageInfo.OutTradeNo = empty;
			packageInfo.TotalFee = (int)(d * 100m);
			if (orderInfo != null)
			{
				packageInfo.OutTradeNo = orderInfo.PayOrderId;
				packageInfo.Attach = empty2;
			}
			else
			{
				packageInfo.OutTradeNo = empty;
				packageInfo.Attach = empty2;
			}
			if (packageInfo.TotalFee < decimal.One)
			{
				packageInfo.TotalFee = decimal.One;
			}
			goto IL_06db;
		}
	}
}
