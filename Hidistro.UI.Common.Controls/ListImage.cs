using Hidistro.Context;
using Hidistro.Core;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class ListImage : Image
	{
		public bool IsDelayedLoading
		{
			get;
			set;
		}

		public string DataField
		{
			get;
			set;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (!string.IsNullOrEmpty(base.ImageUrl) && !Globals.IsUrlAbsolute(base.ImageUrl.ToLower()))
			{
				string text = "";
				text = (base.ImageUrl = Globals.GetImageServerUrl() + base.ImageUrl);
			}
			base.Render(writer);
		}

		protected override void OnDataBinding(EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (!string.IsNullOrEmpty(this.DataField))
			{
				object obj = DataBinder.Eval(this.Page.GetDataItem(), this.DataField);
				if (obj != null && obj != DBNull.Value && !string.IsNullOrEmpty(obj.ToString()))
				{
					base.ImageUrl = (string)obj;
				}
				else if (this.DataField.Equals("ThumbnailUrl40"))
				{
					base.ImageUrl = Globals.GetImageServerUrl() + masterSettings.DefaultProductThumbnail1;
				}
				else if (this.DataField.Equals("ThumbnailUrl60"))
				{
					base.ImageUrl = Globals.GetImageServerUrl() + masterSettings.DefaultProductThumbnail2;
				}
				else if (this.DataField.Equals("ThumbnailUrl100"))
				{
					base.ImageUrl = Globals.GetImageServerUrl() + masterSettings.DefaultProductThumbnail3;
				}
				else if (this.DataField.Equals("ThumbnailUrl160"))
				{
					base.ImageUrl = Globals.GetImageServerUrl() + masterSettings.DefaultProductThumbnail4;
				}
				else if (this.DataField.Equals("ThumbnailUrl180"))
				{
					base.ImageUrl = Globals.GetImageServerUrl() + masterSettings.DefaultProductThumbnail5;
				}
				else if (this.DataField.Equals("ThumbnailUrl220"))
				{
					base.ImageUrl = Globals.GetImageServerUrl() + masterSettings.DefaultProductThumbnail6;
				}
				else if (this.DataField.Equals("ThumbnailUrl310"))
				{
					base.ImageUrl = Globals.GetImageServerUrl() + masterSettings.DefaultProductThumbnail7;
				}
				else if (this.DataField.Equals("ThumbnailUrl410"))
				{
					base.ImageUrl = Globals.GetImageServerUrl() + masterSettings.DefaultProductThumbnail8;
				}
				if (string.IsNullOrEmpty(base.ImageUrl))
				{
					base.ImageUrl = Globals.GetImageServerUrl() + masterSettings.DefaultProductImage;
				}
				if (this.IsDelayedLoading)
				{
					base.Attributes.Add("data-url", base.ImageUrl);
					base.ImageUrl = "";
				}
			}
		}
	}
}
