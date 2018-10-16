using Hidistro.Context;
using Hidistro.UI.Common.Controls;
using Senparc.Weixin.MP.CommonAPIs;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class AttentionYDGZ : WAPTemplatedWebControl
	{
		private Image qRCode;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-AttentionYDGZ.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			string text = this.Page.Request["SendRecordId"];
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			this.qRCode = (Image)this.FindControl("QRCode");
			if (!string.IsNullOrEmpty(masterSettings.WeixinAppId) && !string.IsNullOrEmpty(masterSettings.WeixinAppSecret))
			{
				string accesstoken = AccessTokenContainer.TryGetToken(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, false);
				string empty = string.Empty;
				empty = ((!(text == "-9")) ? base.GetQRSCENETicket(accesstoken, text, true) : base.GetQRLIMITSTRSCENETicket(accesstoken, "referralregister", true));
				this.qRCode.ImageUrl = $"https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket={empty}";
			}
		}
	}
}
