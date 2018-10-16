using Hidistro.Context;
using Hidistro.Core;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class HiImage : Image
	{
		private string dataField;

		public string DataField
		{
			get
			{
				return this.dataField;
			}
			set
			{
				this.dataField = value;
			}
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (!string.IsNullOrEmpty(base.ImageUrl))
			{
				if (!string.IsNullOrEmpty(base.ImageUrl) && !Globals.IsUrlAbsolute(base.ImageUrl.ToLower()))
				{
					base.ImageUrl = Globals.GetImageServerUrl() + base.ImageUrl;
				}
				base.Render(writer);
			}
		}

		protected override void OnDataBinding(EventArgs e)
		{
			if (!string.IsNullOrEmpty(this.DataField))
			{
				string text = DataBinder.Eval(this.Page.GetDataItem(), this.DataField).ToNullString();
				if (!string.IsNullOrEmpty(text))
				{
					if (!Globals.IsUrlAbsolute(text.ToLower()))
					{
						base.ImageUrl = Globals.GetImageServerUrl() + text;
					}
					else
					{
						base.ImageUrl = text;
					}
				}
				else
				{
					base.ImageUrl = HiContext.Current.SiteSettings.DefaultProductImage;
				}
			}
		}
	}
}
