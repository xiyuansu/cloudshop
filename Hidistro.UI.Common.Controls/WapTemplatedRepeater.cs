using Hidistro.Context;
using Hidistro.Entities;
using System.Web;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class WapTemplatedRepeater : Repeater
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
				if (!string.IsNullOrEmpty(value))
				{
					this.skinName = value;
				}
			}
		}

		public ClientType ClientType
		{
			get;
			set;
		}

		protected override void CreateChildControls()
		{
			if (this.ItemTemplate == null && !string.IsNullOrEmpty(this.TemplateFile))
			{
				string commonSkinPath = HiContext.Current.GetCommonSkinPath();
				if (this.TemplateFile.StartsWith("/"))
				{
					this.TemplateFile = commonSkinPath + this.TemplateFile;
				}
				else
				{
					this.TemplateFile = commonSkinPath + "/" + this.TemplateFile;
				}
				this.ItemTemplate = this.Page.LoadTemplate(this.TemplateFile);
			}
		}
	}
}
