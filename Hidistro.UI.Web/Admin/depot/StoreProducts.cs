using Hidistro.Entities.Depot;
using Hidistro.SaleSystem.Depot;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.depot
{
	public class StoreProducts : AdminPage
	{
		private int StoreId;

		protected HiddenField hidStoreId;

		protected HyperLink OnStore;

		protected HyperLink OnSale;

		protected Literal storeName;

		protected ProductCategoriesDropDownList dropCategories;

		protected PageSizeDropDownList PageSizeDropDownList;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!base.IsPostBack && !int.TryParse(this.Page.Request.QueryString["StoreId"], out this.StoreId))
			{
				base.GotoResourceNotFound();
			}
			else if (this.StoreId > 0)
			{
				StoresInfo storeById = StoresHelper.GetStoreById(this.StoreId);
				if (storeById != null)
				{
					this.storeName.Text = storeById.StoreName;
				}
				this.hidStoreId.Value = this.StoreId.ToString();
				this.dropCategories.DataBind();
				this.OnStore.NavigateUrl = "StoresList.aspx";
				this.OnSale.NavigateUrl = "StoreMoveProductList.aspx?StoreId=" + this.StoreId;
			}
		}
	}
}
