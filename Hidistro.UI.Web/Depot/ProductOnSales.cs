using Hidistro.Context;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Depot
{
	[PrivilegeCheck(Privilege.Products)]
	public class ProductOnSales : StoreAdminPage
	{
		protected ProductCategoriesDropDownList dropCategories;

		protected BrandCategoriesDropDownList dropBrandList;

		protected ProductTagsDropDownList dropTagList;

		protected ProductTypeDownList dropType;

		protected DropDownList ddlProductType;

		protected PageSizeDropDownList PageSizeDropDownList;

		public bool IsShelvesProduct
		{
			get;
			set;
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				int storeId = HiContext.Current.Manager.StoreId;
				this.IsShelvesProduct = false;
				StoresInfo storeById = StoresHelper.GetStoreById(storeId);
				if (storeById != null && storeById.IsShelvesProduct)
				{
					this.IsShelvesProduct = true;
				}
				this.dropCategories.DataBind();
				this.dropBrandList.DataBind();
				this.dropTagList.DataBind();
				this.dropType.DataBind();
			}
		}
	}
}
