using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Xml;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WapAppDownload : WAPTemplatedWebControl
	{
		private string appBaseDir = "/Storage/data/app/";

		private HtmlGenericControl loadPanel;

		private HtmlGenericControl sharePanel;

		private HtmlAnchor androidLink;

		private HtmlAnchor iosLink;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-AppDownload.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			string text = "";
			this.loadPanel = (HtmlGenericControl)this.FindControl("loadPanel");
			this.sharePanel = (HtmlGenericControl)this.FindControl("sharePanel");
			this.androidLink = (HtmlAnchor)this.FindControl("androidlink");
			this.iosLink = (HtmlAnchor)this.FindControl("ioslink");
			string text2 = HttpContext.Current.Request.UserAgent;
			if (string.IsNullOrEmpty(text2))
			{
				text2 = "";
			}
			if (text2.ToLower().IndexOf("micromessenger") > -1)
			{
				this.loadPanel.Visible = false;
				this.sharePanel.Visible = true;
			}
			else
			{
				this.loadPanel.Visible = true;
				this.sharePanel.Visible = false;
			}
			string text3 = "";
			string text4 = "";
			string text5 = "";
			string text6 = "";
			XmlDocument xmlDocument = new XmlDocument();
			AppVersionRecordInfo latestAppVersionRecord = APPHelper.GetLatestAppVersionRecord("android");
			if (latestAppVersionRecord != null)
			{
				text5 = latestAppVersionRecord.Version.ToString();
				text3 = latestAppVersionRecord.UpgradeUrl;
			}
			if (string.IsNullOrEmpty(text3))
			{
				text3 = "http://" + Globals.DomainName + text + this.appBaseDir + "android/MEC1.4_android1.0.0.apk";
			}
			if (text3.ToLower().IndexOf("http://") == -1)
			{
				text3 = "http://" + Globals.DomainName + text + text3;
			}
			AppVersionRecordInfo latestAppVersionRecord2 = APPHelper.GetLatestAppVersionRecord("ios");
			if (latestAppVersionRecord2 != null)
			{
				text6 = latestAppVersionRecord2.Version.ToString();
			}
			text4 = HiContext.Current.SiteSettings.AppIOSDownLoadUrl;
			if (string.IsNullOrEmpty(text4))
			{
				text4 = "https://itunes.apple.com/us/app/yi-dong-yun-shang-cheng/id880544709?mt=8";
			}
			this.iosLink.HRef = text4;
			this.androidLink.HRef = text3;
			PageTitle.AddSiteNameTitle("APP下载页面");
		}
	}
}
