using Hidistro.Context;
using Hidistro.UI.Common.Controls;
using Senparc.Weixin.MP.CommonAPIs;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class AttentionHNYSJY : WAPTemplatedWebControl
	{
		private Image qRCode;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-AttentionHNYSJY.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			string sendRecordId = this.Page.Request["SendRecordId"];
			this.qRCode = (Image)this.FindControl("QRCode");
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (!string.IsNullOrEmpty(masterSettings.WeixinAppId) && !string.IsNullOrEmpty(masterSettings.WeixinAppSecret))
			{
				string accesstoken = AccessTokenContainer.TryGetToken(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, false);
				string qRSCENETicket = base.GetQRSCENETicket(accesstoken, sendRecordId, true);
				this.qRCode.ImageUrl = $"https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket={qRSCENETicket}";
			}
		}
	}
}
