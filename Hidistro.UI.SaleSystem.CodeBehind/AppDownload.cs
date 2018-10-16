using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class AppDownload : HtmlTemplatedWebControl
	{
		private string baseDir = "/Storage/data/app/QRCode/";

		private string appBaseDir = "/Storage/data/app/";

		private Image AndroidQrCode;

		private Image IosQrCode;

		private HtmlAnchor androidLink;

		private HtmlAnchor iosLink;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-AppDownload.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			string text = "";
			this.AndroidQrCode = (Image)this.FindControl("AndroidQrCode");
			this.IosQrCode = (Image)this.FindControl("IosQrCode");
			this.androidLink = (HtmlAnchor)this.FindControl("androidlink");
			this.iosLink = (HtmlAnchor)this.FindControl("ioslink");
			string text2 = "";
			string text3 = "";
			string text4 = "";
			string str = "";
			string text5 = "";
			XmlDocument xmlDocument = new XmlDocument();
			AppVersionRecordInfo latestAppVersionRecord = APPHelper.GetLatestAppVersionRecord("android");
			if (latestAppVersionRecord != null)
			{
				text4 = latestAppVersionRecord.Version.ToString();
				text2 = latestAppVersionRecord.UpgradeUrl;
			}
			if (string.IsNullOrEmpty(text2))
			{
				text2 = "http://" + Globals.DomainName + text + this.appBaseDir + "android/MEC1.4_android1.0.0.apk";
			}
			if (text2.ToLower().IndexOf("http://") == -1)
			{
				text2 = "http://" + Globals.DomainName + text + text2;
			}
			text5 = this.baseDir + "AndroidDownload_" + text4 + ".png";
			if (Directory.Exists(this.Page.Request.MapPath(text5)))
			{
				this.AndroidQrCode.ImageUrl = "http://" + this.baseDir + "AndroidDownload_" + text4 + ".png";
			}
			else
			{
				this.AndroidQrCode.ImageUrl = Globals.CreateQRCode(text2, text5, false, ImageFormats.Png);
			}
			AppVersionRecordInfo latestAppVersionRecord2 = APPHelper.GetLatestAppVersionRecord("ios");
			if (latestAppVersionRecord2 != null)
			{
				str = latestAppVersionRecord2.Version.ToString();
			}
			text3 = HiContext.Current.SiteSettings.AppIOSDownLoadUrl;
			if (string.IsNullOrEmpty(text3))
			{
				text3 = "https://itunes.apple.com/us/app/yi-dong-yun-shang-cheng/id880544709?mt=8";
			}
			text5 = this.baseDir + "IosDownload_" + str + ".png";
			if (Directory.Exists(this.Page.Request.MapPath(text5)))
			{
				this.IosQrCode.ImageUrl = this.baseDir + "IosDownload_" + str + ".png";
			}
			else
			{
				this.IosQrCode.ImageUrl = Globals.CreateQRCode(text3, text5, false, ImageFormats.Png);
			}
			this.iosLink.HRef = text3;
			this.androidLink.HRef = text2;
		}
	}
}
