using Hidistro.Context;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class TemplateStyle : Literal
	{
		private string href;

		private const string linkFormat = "<link rel=\"stylesheet\" href=\"{0}\" type=\"text/css\" media=\"{1}\" />";

		[DefaultValue("screen")]
		public string Media
		{
			get;
			set;
		}

		public virtual string Href
		{
			get
			{
				if (!string.IsNullOrEmpty(this.href))
				{
					if (!this.href.ToLower().Contains("home.css") && !this.href.ToLower().Contains("menu.css"))
					{
						if (this.href.StartsWith("/"))
						{
							return HiContext.Current.GetSkinPath() + this.href;
						}
						return "/" + HiContext.Current.GetSkinPath() + this.href;
					}
					string text = this.href.Substring(this.href.LastIndexOf('/'));
					if (this.href.StartsWith("/"))
					{
						return HiContext.Current.GetPCHomePageSkinPath() + "/style/" + text;
					}
					return "/" + HiContext.Current.GetPCHomePageSkinPath() + "/style/" + text;
				}
				return null;
			}
			set
			{
				this.href = value;
			}
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (!string.IsNullOrEmpty(this.Href))
			{
				writer.Write("<link rel=\"stylesheet\" href=\"{0}\" type=\"text/css\" media=\"{1}\" />", this.Href, this.Media);
			}
		}
	}
}
