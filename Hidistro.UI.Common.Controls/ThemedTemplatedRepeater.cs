using Hidistro.Context;
using System.Web;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class ThemedTemplatedRepeater : Repeater
	{
		private string skinName = string.Empty;

		public string TemplateFile
		{
			get
			{
				if (!string.IsNullOrEmpty(this.skinName))
				{
					return HttpContext.Current.Request.ApplicationPath + this.skinName;
				}
				return this.skinName;
			}
			set
			{
				if (this.IsThemeTags(value))
				{
					if (!string.IsNullOrEmpty(value))
					{
						if (value.StartsWith("/"))
						{
							this.skinName = HiContext.Current.GetPCHomePageSkinPath() + value;
						}
						else
						{
							this.skinName = HiContext.Current.GetPCHomePageSkinPath() + "/" + value;
						}
					}
				}
				else if (!string.IsNullOrEmpty(value))
				{
					if (value.StartsWith("/"))
					{
						this.skinName = HiContext.Current.GetSkinPath() + value;
					}
					else
					{
						this.skinName = HiContext.Current.GetSkinPath() + "/" + value;
					}
				}
				if (!this.skinName.StartsWith("/templates"))
				{
					this.skinName = this.skinName.Substring(this.skinName.IndexOf("/templates"));
				}
			}
		}

		protected override void CreateChildControls()
		{
			if (this.ItemTemplate == null && !string.IsNullOrEmpty(this.TemplateFile))
			{
				this.ItemTemplate = this.Page.LoadTemplate(this.TemplateFile);
			}
		}

		private bool IsThemeTags(string skinName)
		{
			return skinName.ToLower().Contains("hometags");
		}
	}
}
