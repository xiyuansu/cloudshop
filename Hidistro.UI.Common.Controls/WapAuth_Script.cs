using Hidistro.Context;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class WapAuth_Script : Literal
	{
		private const string RightFormat = "<script type=\"text/javascript\">{0}</script>";

		protected override void Render(HtmlTextWriter writer)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (masterSettings.OpenWap == 1)
			{
				writer.Write("<script type=\"text/javascript\">{0}</script>", "var HasWapRight = true;");
			}
			else
			{
				writer.Write("<script type=\"text/javascript\">{0}</script>", "var HasWapRight = false;");
			}
		}
	}
}
