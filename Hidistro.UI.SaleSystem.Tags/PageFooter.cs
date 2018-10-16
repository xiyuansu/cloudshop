using Hidistro.Context;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class PageFooter : WebControl
	{
		protected override void Render(HtmlTextWriter writer)
		{
			if (!string.IsNullOrEmpty(HiContext.Current.SiteSettings.Footer))
			{
				writer.Write(HiContext.Current.SiteSettings.Footer.Replace("&amp;", "&").Replace("&quot;", "\"").Replace("&#39;", "'")
					.Replace("&lt;", "<")
					.Replace("&gt;", ">"));
			}
		}
	}
}
