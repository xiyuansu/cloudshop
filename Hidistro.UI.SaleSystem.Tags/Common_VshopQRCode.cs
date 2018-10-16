using Hidistro.Context;
using Hidistro.Core;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_VshopQRCode : Literal
	{
		protected override void Render(HtmlTextWriter writer)
		{
			if (SettingsManager.GetMasterSettings().OpenVstore == 1)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				string text = "/Storage/master/QRCode/" + masterSettings.SiteUrl.ToLower().Replace("http://", "").Replace("https://", "") + "_vshop.png";
				string text2 = masterSettings.SiteUrl.ToLower();
				Globals.CreateQRCode((text2.StartsWith("http://") || text2.StartsWith("https://")) ? text2 : ("http://" + text2), text, false, ImageFormats.Png);
				base.Text = $"<div class=\"cw-icon\"><em><a href=\"javascript:;\">微信商城</a><i><img src=\"/templates/master/{masterSettings.Theme}/images/jiantou_03.png\"></i></em></div><div class=\"dorpdown-layer\"><img src=\"{text}\" /></div>";
				base.Render(writer);
			}
		}
	}
}
