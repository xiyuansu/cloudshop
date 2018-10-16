using Hidistro.Context;
using System.Web;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class AppshopTemplatedRepeater : Repeater
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
				string appshopSkinPath = HiContext.Current.GetAppshopSkinPath();
				if (!string.IsNullOrEmpty(value))
				{
					if (value.StartsWith("/"))
					{
						this.skinName = appshopSkinPath + value;
					}
					else
					{
						this.skinName = appshopSkinPath + "/" + value;
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
	}
}
