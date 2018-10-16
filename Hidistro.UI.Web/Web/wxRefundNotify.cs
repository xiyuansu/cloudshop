using Hidistro.Context;
using Hishop.Weixin.Pay;
using Hishop.Weixin.Pay.Domain;
using System;
using System.Web.UI;

namespace Hidistro.UI.Web
{
	public class wxRefundNotify : Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			PayConfig payConfig = new PayConfig();
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			payConfig.AppId = masterSettings.WeixinAppId;
			payConfig.AppSecret = masterSettings.WeixinAppSecret;
			payConfig.Key = masterSettings.WeixinPartnerKey;
			payConfig.MchID = masterSettings.WeixinPartnerID;
			payConfig.SSLCERT_PATH = masterSettings.WeixinCertPath;
			payConfig.SSLCERT_PASSWORD = masterSettings.WeixinCertPassword;
			ResultNotify resultNotify = new ResultNotify(this.Page, payConfig);
			resultNotify.ProcessNotify();
		}
	}
}
