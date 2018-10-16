using Hidistro.Context;
using Hidistro.Core;
using Hidistro.SaleSystem.Catalog;
using Hidistro.UI.Common.Controls;
using System;
using System.Data;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class ProductDelete : HtmlTemplatedWebControl
	{
		private ThemedTemplatedRepeater rptHotProduct;

		protected override void AttachChildControls()
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			this.rptHotProduct = (ThemedTemplatedRepeater)this.FindControl("rptHotProduct");
			DataTable hotProductList = ProductBrowser.GetHotProductList();
			for (int i = 0; i < hotProductList.Rows.Count; i++)
			{
				hotProductList.Rows[i]["ThumbnailUrl220"] = (string.IsNullOrEmpty(hotProductList.Rows[i]["ThumbnailUrl220"].ToString()) ? Globals.GetImageServerUrl("http://", masterSettings.DefaultProductThumbnail4) : Globals.GetImageServerUrl("http://", hotProductList.Rows[i]["ThumbnailUrl220"].ToString()));
				hotProductList.Rows[i]["ThumbnailUrl40"] = (string.IsNullOrEmpty(hotProductList.Rows[i]["ThumbnailUrl40"].ToString()) ? Globals.GetImageServerUrl("http://", masterSettings.DefaultProductThumbnail1) : Globals.GetImageServerUrl("http://", hotProductList.Rows[i]["ThumbnailUrl40"].ToString()));
				hotProductList.Rows[i]["ImageUrl2"] = (string.IsNullOrEmpty(hotProductList.Rows[i]["ImageUrl2"].ToString()) ? "" : Globals.GetImageServerUrl("http://", hotProductList.Rows[i]["ImageUrl2"].ToString().ToLower().Replace("/storage/master/product/images/", "/storage/master/product/thumbs40/40_")));
				hotProductList.Rows[i]["ImageUrl3"] = (string.IsNullOrEmpty(hotProductList.Rows[i]["ImageUrl3"].ToString()) ? "" : Globals.GetImageServerUrl("http://", hotProductList.Rows[i]["ImageUrl3"].ToString().ToLower().Replace("/storage/master/product/images/", "/storage/master/product/thumbs40/40_")));
				hotProductList.Rows[i]["ImageUrl4"] = (string.IsNullOrEmpty(hotProductList.Rows[i]["ImageUrl4"].ToString()) ? "" : Globals.GetImageServerUrl("http://", hotProductList.Rows[i]["ImageUrl4"].ToString().ToLower().Replace("/storage/master/product/images/", "/storage/master/product/thumbs40/40_")));
				hotProductList.Rows[i]["ImageUrl5"] = (string.IsNullOrEmpty(hotProductList.Rows[i]["ImageUrl5"].ToString()) ? "" : Globals.GetImageServerUrl("http://", hotProductList.Rows[i]["ImageUrl5"].ToString().ToLower().Replace("/storage/master/product/images/", "/storage/master/product/thumbs40/40_")));
			}
			this.rptHotProduct.DataSource = hotProductList;
			this.rptHotProduct.DataBind();
		}

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-ProductDelete.html";
			}
			base.OnInit(e);
		}
	}
}
