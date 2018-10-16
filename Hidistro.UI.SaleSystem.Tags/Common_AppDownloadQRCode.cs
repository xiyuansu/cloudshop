using Hidistro.Context;
using Hidistro.Core;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_AppDownloadQRCode : Literal
	{
		protected override void Render(HtmlTextWriter writer)
		{
			if (HiContext.Current.SiteSettings.EnableAppDownload)
			{
				string text = "/Storage/master/QRCode/" + HttpContext.Current.Request.Url.Host + "_appdownload.png";
				string siteUrl = SettingsManager.GetMasterSettings().SiteUrl;
				string str = (siteUrl.Contains("http://") || siteUrl.Contains("https://")) ? siteUrl : ("http://" + siteUrl);
				str += (siteUrl.EndsWith("/") ? "" : "/WapShop/AppDownload");
				Globals.CreateQRCode(str, text, false, ImageFormats.Png);
				base.Text = $"<div class=\"app-icon dt cw-icon\"><em><a href=\"javascript:;\">手机版</a><i><img src=\"/templates/master/{SettingsManager.GetMasterSettings().Theme}/images/jiantou_03.png\"></i></em></div><div class=\"dorpdown-layer\"><img src=\"{text}\" /></div>";
				base.Render(writer);
			}
		}
	}
}
