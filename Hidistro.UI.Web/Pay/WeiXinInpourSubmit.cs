using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Member;
using Hishop.Weixin.Pay;
using Hishop.Weixin.Pay.Domain;
using Hishop.Weixin.Pay.Lib;
using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;

namespace Hidistro.UI.Web.pay
{
	public class WeiXinInpourSubmit : Page
	{
		public string pay_json = string.Empty;

		protected void Page_Load(object sender, EventArgs e)
		{
			string text = base.Request.QueryString.Get("orderId");
			if (!string.IsNullOrEmpty(text))
			{
				InpourRequestInfo inpourBlance = MemberProcessor.GetInpourBlance(text);
				if (inpourBlance != null)
				{
					SiteSettings masterSettings = SettingsManager.GetMasterSettings();
					PackageInfo packageInfo = new PackageInfo();
					packageInfo.Body = inpourBlance.InpourId;
					packageInfo.NotifyUrl = Globals.GetProtocal(HttpContext.Current) + "://" + $"{base.Request.Url.Host}/pay/WeiXinInpourNotify";
					packageInfo.OutTradeNo = inpourBlance.InpourId;
					packageInfo.TotalFee = (int)(inpourBlance.InpourBlance * 100m);
					if (packageInfo.TotalFee < decimal.One)
					{
						packageInfo.TotalFee = decimal.One;
					}
					string text2 = "";
					if (string.IsNullOrEmpty(text2))
					{
						PayConfig payConfig = new PayConfig();
						payConfig.AppId = masterSettings.WeixinAppId;
						payConfig.Key = masterSettings.WeixinPartnerKey;
						payConfig.MchID = masterSettings.WeixinPartnerID;
						payConfig.AppSecret = masterSettings.WeixinAppSecret;
						JsApiPay jsApiPay = new JsApiPay();
						NameValueCollection openidAndAccessToken = JsApiPay.GetOpenidAndAccessToken(this.Page, payConfig.AppId, payConfig.AppSecret, false);
						if (openidAndAccessToken.HasKeys())
						{
							text2 = openidAndAccessToken["openId"];
						}
					}
					if (!string.IsNullOrEmpty(masterSettings.Main_AppId) && !string.IsNullOrEmpty(masterSettings.Main_Mch_ID))
					{
						packageInfo.sub_openid = text2;
					}
					else
					{
						packageInfo.OpenId = text2;
					}
					packageInfo.sub_mch_id = masterSettings.WeixinPartnerID;
					PayClient payClient = null;
					payClient = ((string.IsNullOrEmpty(masterSettings.Main_AppId) || string.IsNullOrEmpty(masterSettings.Main_Mch_ID)) ? new PayClient(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, masterSettings.WeixinPartnerID, masterSettings.WeixinPartnerKey, masterSettings.WeixinPaySignKey, "", "", "") : new PayClient(masterSettings.Main_AppId, masterSettings.WeixinAppSecret, masterSettings.Main_Mch_ID, masterSettings.WeixinPartnerKey, masterSettings.WeixinPaySignKey, masterSettings.WeixinPartnerID, masterSettings.WeixinAppId, ""));
					PayRequestInfo req = payClient.BuildPayRequest(packageInfo);
					this.pay_json = this.ConvertPayJson(req);
				}
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
	}
}
