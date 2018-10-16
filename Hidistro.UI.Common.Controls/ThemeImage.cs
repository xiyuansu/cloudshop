using Hidistro.Context;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Common.Controls
{
	public class ThemeImage : HtmlImage
	{
		protected override void Render(HtmlTextWriter writer)
		{
			if (base.Src.StartsWith("~"))
			{
				base.Src = base.ResolveUrl(base.Src);
			}
			else if (base.Src.StartsWith("/"))
			{
				base.Src = HiContext.Current.GetSkinPath() + base.Src;
			}
			else if (base.Src.ToLower().StartsWith("http://"))
			{
				base.Src = base.ResolveUrl(base.Src);
			}
			else
			{
				base.Src = HiContext.Current.GetSkinPath() + "/" + base.Src;
			}
			base.Render(writer);
		}
	}
}
