using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.depot
{
	public class StoreMoveProductList : AdminPage
	{
		private int StoreId;

		protected HiddenField hidStoreId;

		protected HyperLink StoreList;

		protected HyperLink StoreProducts;

		protected ProductCategoriesDropDownList dropCategories;

		protected BrandCategoriesDropDownList dropBrandList;

		protected PageSizeDropDownList PageSizeDropDownList;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!base.IsPostBack)
			{
				if (!int.TryParse(this.Page.Request.QueryString["StoreId"], out this.StoreId))
				{
					base.GotoResourceNotFound();
				}
				else
				{
					if (this.StoreId > 0)
					{
						this.hidStoreId.Value = this.StoreId.ToString();
						this.dropBrandList.DataBind();
						this.dropCategories.DataBind();
						this.StoreProducts.NavigateUrl = "StoreProducts.aspx?StoreId=" + this.StoreId;
					}
					this.StoreList.NavigateUrl = "StoresList.aspx";
				}
			}
		}
	}
}
