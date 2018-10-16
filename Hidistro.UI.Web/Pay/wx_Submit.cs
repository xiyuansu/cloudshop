using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Shopping;
using Hishop.Weixin.Pay;
using Hishop.Weixin.Pay.Domain;
using Hishop.Weixin.Pay.Lib;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;

namespace Hidistro.UI.Web.Pay
{
	public class wx_Submit : Page
	{
		public string pay_json = string.Empty;

		public string pay_uri = string.Empty;

		public string isFightGroup = "false";

		public string isOfflineOrder = "false";

		public string IsServiceOrder = "false";

		public bool isWeiXin = false;

		protected void Page_Load(object sender, EventArgs e)
		{
			OrderInfo orderInfo = null;
			PackageInfo packageInfo = new PackageInfo();
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			bool flag = this.Page.Request["IsOffline"].ToBool();
			string a = this.Page.Request["from"].ToNullString().ToLower();
			string text = this.Page.Request.QueryString.Get("orderId");
			string empty = string.Empty;
			string empty2 = string.Empty;
			string userAgent = base.Request.UserAgent;
			if (userAgent.ToLower().IndexOf("micromessenger") > -1)
			{
				this.isWeiXin = true;
			}
			if (a == "appstore")
			{
				this.isOfflineOrder = "true";
				if (string.IsNullOrEmpty(text))
				{
					this.Page.Response.Write("<h2 style=\"color:red;width:100%; text-align:center;\">错误的订单号，不能进行支付!<h2>");
					this.Page.Response.End();
				}
				empty = text;
				decimal d = default(decimal);
				StoreCollectionInfo storeCollectionInfo = null;
				if (flag)
				{
					this.isOfflineOrder = "true";
					storeCollectionInfo = StoresHelper.GetStoreCollectionInfo(text);
					if (storeCollectionInfo == null)
					{
						this.Page.Response.Write("<h2 style=\"color:red;width:100%; text-align:center;\">订单状态错误，不能进行支付!<h2>");
						this.Page.Response.End();
					}
					if (storeCollectionInfo.Status != 0)
					{
						this.Page.Response.Write("<h2 style=\"color:red;width:100%; text-align:center;\">订单状态错误，不能进行支付!<h2>");
						this.Page.Response.End();
					}
					d = storeCollectionInfo.PayAmount;
				}
				else
				{
					orderInfo = ShoppingProcessor.GetOrderInfo(text);
					if (orderInfo == null)
					{
						this.Page.Response.Write("<h2 style=\"color:red;width:100%; text-align:center;\">订单状态错误，不能进行支付!<h2>");
						this.Page.Response.End();
					}
					if (orderInfo.OrderStatus != OrderStatus.WaitBuyerPay)
					{
						this.Page.Response.Write("<h2 style=\"color:red;width:100%; text-align:center;\">订单状态错误，不能进行支付!<h2>");
						this.Page.Response.End();
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
				}
				packageInfo.Body = text;
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
			}
			else
			{
				if (string.IsNullOrEmpty(text))
				{
					this.Page.Response.Write("<h2 style=\"color:red;width:100%; text-align:center;\">订单状态错误，不能进行支付!<h2>");
					this.Page.Response.End();
				}
				orderInfo = OrderHelper.GetOrderInfo(text);
				if (orderInfo == null)
				{
					this.Page.Response.Write("<h2 style=\"color:red;width:100%; text-align:center;\">订单状态错误，不能进行支付!<h2>");
					this.Page.Response.End();
				}
				if (orderInfo.OrderStatus != OrderStatus.WaitBuyerPay)
				{
					this.Page.Response.Write("<h2 style=\"color:red;width:100%; text-align:center;\">订单状态错误，不能进行支付!<h2>");
					this.Page.Response.End();
				}
				this.IsServiceOrder = ((orderInfo.OrderType == OrderType.ServiceOrder) ? "true" : "false");
				empty = text;
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
					empty = orderInfo.OrderId;
					d2 = orderInfo.GetTotal(true);
				}
				packageInfo.Body = orderInfo.OrderId + orderInfo.PayRandCode;
				packageInfo.NotifyUrl = Globals.GetProtocal(HttpContext.Current) + "://" + $"{this.Page.Request.Url.Host}/pay/wx_Pay";
				if (orderInfo.OrderType != OrderType.ServiceOrder && orderInfo.OrderSource == OrderSource.Applet)
				{
					packageInfo.NotifyUrl = Globals.GetProtocal(HttpContext.Current) + "://" + $"{this.Page.Request.Url.Host}/pay/O2OApplet_Pay";
				}
				packageInfo.OutTradeNo = orderInfo.PayOrderId;
				packageInfo.Attach = empty2;
				packageInfo.TotalFee = (int)(d2 * 100m);
				if (packageInfo.TotalFee < decimal.One)
				{
					packageInfo.TotalFee = decimal.One;
				}
			}
			string text2 = masterSettings.WeixinAppId;
			string appSecret = masterSettings.WeixinAppSecret;
			string text3 = masterSettings.WeixinPartnerID;
			string text4 = masterSettings.WeixinPartnerKey;
			string text5 = masterSettings.Main_Mch_ID;
			string text6 = masterSettings.Main_AppId;
			if (a != "appstore")
			{
				if (orderInfo.OrderType == OrderType.ServiceOrder && orderInfo.OrderSource != OrderSource.WeiXin)
				{
					text2 = masterSettings.O2OAppletAppId;
					appSecret = masterSettings.O2OAppletAppSecrect;
					text3 = masterSettings.O2OAppletMchId;
					text4 = masterSettings.O2OAppletKey;
					text5 = "";
					text6 = "";
				}
				else if (orderInfo.OrderSource == OrderSource.Applet)
				{
					text2 = masterSettings.WxAppletAppId;
					appSecret = masterSettings.WxAppletAppSecrect;
					text3 = masterSettings.WxApplectMchId;
					text4 = masterSettings.WxApplectKey;
					text5 = "";
					text6 = "";
				}
			}
			string text7 = "";
			MemberInfo user = HiContext.Current.User;
			if (user.UserId > 0 && a != "appstore")
			{
				MemberOpenIdInfo memberOpenIdInfo = null;
				if (orderInfo.OrderType == OrderType.ServiceOrder && orderInfo.OrderSource != OrderSource.WeiXin)
				{
					memberOpenIdInfo = user.MemberOpenIds.FirstOrDefault((MemberOpenIdInfo item) => item.OpenIdType.ToLower() == "hishop.plugins.openid.o2owxapplet");
					if (memberOpenIdInfo != null)
					{
						text7 = memberOpenIdInfo.OpenId;
					}
				}
				else if (orderInfo.OrderSource == OrderSource.Applet)
				{
					memberOpenIdInfo = user.MemberOpenIds.FirstOrDefault((MemberOpenIdInfo item) => item.OpenIdType.ToLower() == "hishop.plugins.openid.wxapplet");
					if (memberOpenIdInfo != null)
					{
						text7 = memberOpenIdInfo.OpenId;
					}
				}
				else
				{
					memberOpenIdInfo = user.MemberOpenIds.FirstOrDefault((MemberOpenIdInfo item) => item.OpenIdType.ToLower() == "hishop.plugins.openid.weixin");
					if (memberOpenIdInfo != null && user.IsDefaultDevice)
					{
						text7 = memberOpenIdInfo.OpenId;
					}
				}
			}
			if (string.IsNullOrEmpty(text7))
			{
				PayConfig payConfig = new PayConfig();
				payConfig.AppId = text2;
				payConfig.Key = text4;
				payConfig.MchID = text3;
				payConfig.AppSecret = appSecret;
				JsApiPay jsApiPay = new JsApiPay();
				try
				{
					NameValueCollection openidAndAccessToken = JsApiPay.GetOpenidAndAccessToken(this.Page, payConfig.AppId, payConfig.AppSecret, false);
					if (openidAndAccessToken.HasKeys())
					{
						text7 = openidAndAccessToken["openId"];
					}
				}
				catch (Exception ex)
				{
					if (!(ex is ThreadAbortException))
					{
						IDictionary<string, string> dictionary = new Dictionary<string, string>();
						dictionary.Add("AppId", payConfig.AppId);
						dictionary.Add("Key", payConfig.Key);
						dictionary.Add("MchID", payConfig.MchID);
						dictionary.Add("AppSecret", payConfig.AppSecret);
						dictionary.Add("Exception", ex.Message);
						dictionary.Add("StackTrace", ex.StackTrace);
						dictionary.Add("TargetSite", ex.TargetSite.ToString());
						Globals.WriteLog(dictionary, "获取用户OpenId失败", "", "", "GetOpenId");
					}
				}
			}
			if (!string.IsNullOrEmpty(text6) && !string.IsNullOrEmpty(text5))
			{
				packageInfo.sub_openid = text7;
			}
			else
			{
				packageInfo.OpenId = text7;
			}
			PayClient payClient = null;
			payClient = ((string.IsNullOrEmpty(text6) || string.IsNullOrEmpty(text5)) ? new PayClient(text2, appSecret, text3, text4, "", "", "", "") : new PayClient(text6, appSecret, text5, text4, "", text3, text2, ""));
			PayRequestInfo req = payClient.BuildPayRequest(packageInfo);
			this.pay_json = this.ConvertPayJson(req);
			this.pay_uri = this.ConvertPayUri(req);
			if (!this.isWeiXin)
			{
				base.Response.Redirect(this.pay_uri);
			}
		}

		public string ConvertPayJson(PayRequestInfo req)
		{
			string str = "{";
			str = str + "\"appId\":\"" + req.appId + "\",";
			str = str + "\"timeStamp\":\"" + req.timeStamp + "\",";
			str = str + "\"nonceStr\":\"" + req.nonceStr + "\",";
			str = str + "\"package\":\"" + req.package + "\",";
			str = str + "\"signType\":\"" + req.signType + "\",";
			str = str + "\"paySign\":\"" + req.paySign + "\"";
			return str + "}";
		}

		public string ConvertPayUri(PayRequestInfo req)
		{
			string empty = string.Empty;
			string format = "weixin://wap/pay?appid%3D{0}%26noncestr%3D{1}%26package%3DWAP%26prepayid%3D{2}%26timestamp%3D{3}%26sign%3D{4}";
			return string.Format(format, req.appId, req.nonceStr, req.prepayid, req.timeStamp, req.paySign);
		}
	}
}
