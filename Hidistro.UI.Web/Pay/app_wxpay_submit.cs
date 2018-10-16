using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Store;
using Hishop.Weixin.Pay;
using Hishop.Weixin.Pay.Domain;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.pay
{
	public class app_wxpay_submit : Page
	{
		public string pay_json = string.Empty;

		public string isFightGroup = "false";

		protected HtmlHead Head1;

		protected void Page_Load(object sender, EventArgs e)
		{
			OrderInfo orderInfo = null;
			decimal d = default(decimal);
			string text = base.Request.QueryString.Get("orderId");
			int num = base.Request.QueryString.Get("isrecharge").ToInt(0);
			if (!string.IsNullOrEmpty(text))
			{
				if (num == 1)
				{
					InpourRequestInfo inpourBlance = MemberProcessor.GetInpourBlance(text);
					if (inpourBlance != null)
					{
						d = inpourBlance.InpourBlance;
						goto IL_0134;
					}
				}
				else
				{
					orderInfo = OrderHelper.GetOrderInfo(text);
					if (orderInfo != null)
					{
						if (orderInfo.PreSaleId > 0)
						{
							if (!orderInfo.DepositDate.HasValue && orderInfo.OrderStatus == OrderStatus.WaitBuyerPay)
							{
								d = orderInfo.Deposit - orderInfo.BalanceAmount;
							}
							else if (orderInfo.DepositDate.HasValue && orderInfo.OrderStatus == OrderStatus.WaitBuyerPay)
							{
								d = orderInfo.FinalPayment;
							}
						}
						else
						{
							d = orderInfo.GetTotal(true);
						}
						this.isFightGroup = ((orderInfo.FightGroupId > 0) ? "true" : "false");
						goto IL_0134;
					}
				}
			}
			return;
			IL_0134:
			SiteSettings siteSettings = HiContext.Current.SiteSettings;
			string appWxAppId = siteSettings.AppWxAppId;
			string appWxAppSecret = siteSettings.AppWxAppSecret;
			string appWxMchId = siteSettings.AppWxMchId;
			string appWxPartnerKey = siteSettings.AppWxPartnerKey;
			string appWX_Main_MchID = siteSettings.AppWX_Main_MchID;
			string appWX_Main_AppId = siteSettings.AppWX_Main_AppId;
			if (siteSettings.OpenAppWxPay && !string.IsNullOrEmpty(siteSettings.AppWxAppId) && !string.IsNullOrEmpty(siteSettings.AppWxMchId) && !string.IsNullOrEmpty(siteSettings.AppWxPartnerKey))
			{
				try
				{
					PackageInfo packageInfo = new PackageInfo();
					if (num == 1)
					{
						packageInfo.Attach = "1";
					}
					else
					{
						packageInfo.Attach = "";
					}
					packageInfo.Body = text;
					packageInfo.NotifyUrl = Globals.GetProtocal(HttpContext.Current) + "://" + $"{base.Request.Url.Host}/pay/app_wxPay";
					if (num == 1)
					{
						packageInfo.OutTradeNo = text;
					}
					else if (orderInfo.PreSaleId > 0 && !orderInfo.DepositDate.HasValue && orderInfo.OrderStatus == OrderStatus.WaitBuyerPay)
					{
						packageInfo.OutTradeNo = text + new Random().Next(10000, 99999).ToString();
					}
					else
					{
						packageInfo.OutTradeNo = text + orderInfo.PayRandCode;
					}
					packageInfo.TotalFee = (int)(d * 100m);
					if (packageInfo.TotalFee < decimal.One)
					{
						packageInfo.TotalFee = decimal.One;
					}
					string text3 = packageInfo.OpenId = "";
					PayClient payClient = null;
					payClient = ((string.IsNullOrEmpty(appWX_Main_AppId) || string.IsNullOrEmpty(appWX_Main_MchID)) ? new PayClient(appWxAppId, appWxAppSecret, appWxMchId, appWxPartnerKey, "", "", "", "") : new PayClient(appWX_Main_AppId, appWxAppSecret, appWX_Main_MchID, appWxPartnerKey, "", appWxMchId, appWxAppId, ""));
					PayRequestInfo req = payClient.BuildAppPayRequest(packageInfo);
					this.pay_json = Globals.UrlEncode(this.ConvertPayJson(siteSettings.AppWxAppId, siteSettings.AppWxMchId, req));
				}
				catch (Exception ex)
				{
					Globals.WriteExceptionLog(ex, null, "APPWxPay");
					this.pay_json = "config_error";
				}
			}
			else
			{
				this.pay_json = "config_error";
			}
		}

		public string ConvertPayJson(string appId, string PartnerId, PayRequestInfo req)
		{
			SiteSettings siteSettings = HiContext.Current.SiteSettings;
			AppVersionRecordInfo latestAppVersionRecord = APPHelper.GetLatestAppVersionRecord("android");
			string text = "";
			if (latestAppVersionRecord == null || latestAppVersionRecord.Version.ToDecimal_MoreDot(0) > 3.3m)
			{
				text = "{";
				text = text + "\"appid\":\"" + appId + "\",";
				text = text + "\"noncestr\":\"" + req.nonceStr + "\",";
				text += "\"package\":\"Sign=WXPay\",";
				text = text + "\"partnerid\":\"" + PartnerId + "\",";
				text = text + "\"prepayid\":\"" + req.prepayid + "\",";
				text = text + "\"timestamp\":" + req.timeStamp + ",";
				text = text + "\"sign\":\"" + req.paySign + "\"";
				return text + "}";
			}
			text = "{";
			text = text + "\"partnerId\":\"" + PartnerId + "\",";
			text = text + "\"prepayId\":\"" + req.prepayid + "\",";
			text = text + "\"nonceStr\":\"" + req.nonceStr + "\",";
			text = text + "\"timeStamp\":\"" + req.timeStamp + "\",";
			text = text + "\"sign\":\"" + req.paySign + "\"";
			return text + "}";
		}
	}
}
