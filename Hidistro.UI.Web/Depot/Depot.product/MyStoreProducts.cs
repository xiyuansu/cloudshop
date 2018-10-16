using Hidistro.Context;
using Hidistro.Entities.Depot;
using Hidistro.SaleSystem.Depot;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Depot.product
{
	public class MyStoreProducts : StoreAdminPage
	{
		private bool isWarningStock = false;

		protected HtmlGenericControl divSearchBox;

		protected HiddenField hidIsWarning;

		protected HiddenField hidStoreId;

		protected ProductCategoriesDropDownList dropCategories;

		protected DropDownList ddlProductType;

		protected HtmlGenericControl spbacthStock;

		protected HtmlGenericControl spbacthWarningStock;

		protected HtmlGenericControl spbacthSalePrice;

		protected PageSizeDropDownList PageSizeDropDownList;

		protected HtmlInputHidden hidIsModifyPrice;

		protected HtmlInputHidden hidIsShelvesProduct;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.hidStoreId.Value = HiContext.Current.Manager.StoreId.ToString();
			StoresInfo storeById = StoresHelper.GetStoreById(HiContext.Current.Manager.StoreId);
			if (storeById != null)
			{
				if (!storeById.IsModifyPrice)
				{
					this.spbacthSalePrice.Visible = false;
					this.hidIsModifyPrice.Value = "0";
				}
				if (!storeById.IsShelvesProduct)
				{
					this.hidIsShelvesProduct.Value = "1";
				}
			}
			else
			{
				this.spbacthSalePrice.Visible = false;
				this.spbacthStock.Visible = false;
				this.spbacthWarningStock.Visible = false;
			}
			this.dropCategories.DataBind();
			this.LoadParameters();
		}

		private void LoadParameters()
		{
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["isWarning"]))
			{
				this.isWarningStock = (this.Page.Request.QueryString["isWarning"] == "1" && true);
				this.hidIsWarning.Value = (this.isWarningStock ? "1" : "");
			}
		}
	}
}
