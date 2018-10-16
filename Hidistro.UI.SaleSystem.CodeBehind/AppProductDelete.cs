using Hidistro.Context;
using Hidistro.Core;
using Hidistro.SaleSystem.Catalog;
using Hidistro.UI.Common.Controls;
using System;
using System.Data;
using System.Web.UI;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class AppProductDelete : AppshopMemberTemplatedWebControl
	{
		private AppshopTemplatedRepeater rptHotProduct;

		protected override void AttachChildControls()
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			this.rptHotProduct = (AppshopTemplatedRepeater)this.FindControl("rptHotProducts");
			DataTable hotProductList = ProductBrowser.GetHotProductList();
			for (int i = 0; i < hotProductList.Rows.Count; i++)
			{
				hotProductList.Rows[i]["ThumbnailUrl220"] = (string.IsNullOrEmpty(hotProductList.Rows[i]["ThumbnailUrl220"].ToString()) ? Globals.GetImageServerUrl("http://", masterSettings.DefaultProductThumbnail4) : Globals.GetImageServerUrl("http://", hotProductList.Rows[i]["ThumbnailUrl220"].ToString()));
			}
			this.rptHotProduct.DataSource = hotProductList;
			this.rptHotProduct.DataBind();
			PageTitle.AddSiteNameTitle("商品不存在");
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
