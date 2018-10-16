using Hidistro.Context;
using System.Globalization;
using System.Web.UI;

namespace Hidistro.UI.Common.Controls
{
	public class DisplayThemesImages : Control
	{
		private string imageFormat = "<a><img border=\"0\" src=\"{0}\" /></a>";

		private bool isDistributorThemes = false;

		public bool IsDistributorThemes
		{
			get
			{
				return this.isDistributorThemes;
			}
			set
			{
				this.isDistributorThemes = value;
			}
		}

		public string Src
		{
			get
			{
				if (this.ViewState["Src"] == null)
				{
					return null;
				}
				return (string)this.ViewState["Src"];
			}
			set
			{
				this.ViewState["Src"] = value;
			}
		}

		public string ThemeName
		{
			get
			{
				if (this.ViewState["ThemeName"] == null)
				{
					return null;
				}
				return (string)this.ViewState["ThemeName"];
			}
			set
			{
				this.ViewState["ThemeName"] = value;
			}
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (this.Src.StartsWith("~"))
			{
				this.Src = base.ResolveUrl(this.Src);
			}
			else if (this.Src.StartsWith("/"))
			{
				this.Src = this.GetImagePath() + this.Src;
			}
			else
			{
				this.Src = this.GetImagePath() + "/" + this.Src;
			}
			writer.Write(string.Format(CultureInfo.InvariantCulture, this.imageFormat, new object[1]
			{
				this.Src
			}));
		}

		protected string GetImagePath()
		{
			if (this.IsDistributorThemes)
			{
				return "/Templates/sites/" + HiContext.Current.UserId + "/" + this.ThemeName;
			}
			return "/Templates/master/" + this.ThemeName;
		}
	}
}
