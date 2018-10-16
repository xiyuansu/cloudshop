using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.UI.Common.Controls;
using System;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class AppDevelopReferrals : AppshopMemberTemplatedWebControl
	{
		private HtmlImage imgQRCode;

		private HtmlAnchor linDevelopReferrals;

		private HtmlInputHidden hidWebSiteTitle;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-DevelopReferrals.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.linDevelopReferrals = (HtmlAnchor)this.FindControl("linDevelopReferrals");
			this.imgQRCode = (HtmlImage)this.FindControl("imgQRCode");
			this.hidWebSiteTitle = (HtmlInputHidden)this.FindControl("hidWebSiteTitle");
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			this.hidWebSiteTitle.Value = masterSettings.SiteName;
			MemberInfo user = HiContext.Current.User;
			if (!user.IsReferral())
			{
				this.Page.Response.Redirect("ReferralRegisterAgreement.aspx");
			}
			Uri url = HttpContext.Current.Request.Url;
			string text = "";
			this.linDevelopReferrals.HRef = string.Format(CultureInfo.InvariantCulture, "{0}://{1}{2}/WapShop/ReferralAgreement.aspx", new object[3]
			{
				Globals.GetProtocal(HttpContext.Current),
				HttpContext.Current.Request.Url.Host,
				text
			}) + "?ReferralUserId=" + HiContext.Current.UserId;
			string text2 = this.Page.Request.MapPath("/Storage/master/QRCode/");
			if (File.Exists(text2 + "referral_" + user.UserId + ".png"))
			{
				this.imgQRCode.Src = "/Storage/master/QRCode/referral_1.4_" + user.UserId + ".png";
			}
			else
			{
				string content = this.linDevelopReferrals.HRef;
				if (this.linDevelopReferrals.HRef.IndexOf("/WapShop/ReferralAgreement.aspx") == -1)
				{
					content = this.linDevelopReferrals.HRef.Replace("/ReferralAgreement.aspx", "/WapShop/ReferralAgreement.aspx");
				}
				string qrCodeUrl = "/Storage/master/QRCode/referral_" + HiContext.Current.Config.Version + "_" + user.UserId + ".png";
				this.imgQRCode.Src = Globals.CreateQRCode(content, qrCodeUrl, false, ImageFormats.Png);
			}
			PageTitle.AddSiteNameTitle("发展分销员");
		}
	}
}
