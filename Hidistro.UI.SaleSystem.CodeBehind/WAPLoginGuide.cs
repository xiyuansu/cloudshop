using Hidistro.Context;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WAPLoginGuide : WAPTemplatedWebControl
	{
		private HtmlImage imgWeixin;

		private HtmlInputHidden hidWeixinNumber;

		private HtmlInputHidden hidWeixinLoginUrl;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-vLoginGuide.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.imgWeixin = (HtmlImage)this.FindControl("imgWeixin");
			this.hidWeixinNumber = (HtmlInputHidden)this.FindControl("hidWeixinNumber");
			this.hidWeixinLoginUrl = (HtmlInputHidden)this.FindControl("hidWeixinLoginUrl");
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			this.hidWeixinNumber.Value = masterSettings.WeixinNumber;
			this.imgWeixin.Src = masterSettings.WeiXinCodeImageUrl;
			this.hidWeixinLoginUrl.Value = masterSettings.WeixinLoginUrl;
			PageTitle.AddSiteNameTitle("登录向导");
		}
	}
}
