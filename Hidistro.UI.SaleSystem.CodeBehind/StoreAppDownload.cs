using Hidistro.Context;
using Hidistro.Core;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class StoreAppDownload : HtmlTemplatedWebControl
	{
		private string baseDir = "/Storage/master/QRCode/";

		private Image AndroidQrCode;

		private Image IosQrCode;

		private HtmlAnchor androidLink;

		private HtmlAnchor iosLink;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-StoreAppDownload.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.AndroidQrCode = (Image)this.FindControl("AndroidQrCode");
			this.IosQrCode = (Image)this.FindControl("IosQrCode");
			this.androidLink = (HtmlAnchor)this.FindControl("androidlink");
			this.iosLink = (HtmlAnchor)this.FindControl("ioslink");
			string text = "";
			string text2 = "";
			string text3 = "";
			text = Globals.FullPath(HiContext.Current.SiteSettings.StoreAppAndroidUrl);
			if (string.IsNullOrEmpty(text))
			{
				text = "请先设置下载地址";
			}
			text2 = Globals.FullPath(HiContext.Current.SiteSettings.StoreAppIOSUrl);
			text3 = this.baseDir + "StoreAppAndroidDownload.png";
			this.AndroidQrCode.ImageUrl = Globals.CreateQRCode(text, text3, true, ImageFormats.Png);
			if (string.IsNullOrEmpty(text2))
			{
				text2 = "请先设置下载地址";
			}
			text3 = this.baseDir + "StoreAppIOSDownload.png";
			this.IosQrCode.ImageUrl = Globals.CreateQRCode(text2, text3, true, ImageFormats.Png);
			Random random = new Random();
			int num = random.Next(1000, 10000000);
			this.iosLink.HRef = text2 + "?rnd=" + num;
			this.androidLink.HRef = text + "?rnd=" + num;
		}
	}
}
