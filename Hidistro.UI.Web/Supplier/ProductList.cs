using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Supplier
{
	public class ProductList : SupplierAdminPage
	{
		private bool isWarningStock;

		private ProductSaleStatus saleStatus = ProductSaleStatus.All;

		protected HiddenField hidFilter;

		protected ProductCategoriesDropDownList dropCategories;

		protected CheckBox chkIsWarningStock;

		protected PageSizeDropDownList PageSizeDropDownList;

		protected TrimTextBox txtProductTag;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.dropCategories.DataBind();
			this.LoadParameters();
		}

		private void LoadParameters()
		{
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["SaleStatus"]))
			{
				this.saleStatus = (ProductSaleStatus)Enum.Parse(typeof(ProductSaleStatus), this.Page.Request.QueryString["SaleStatus"]);
				this.hidFilter.Value = ((int)this.saleStatus).ToNullString();
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["isWarningStock"]))
			{
				this.isWarningStock = (this.Page.Request.QueryString["isWarningStock"] == "1" && true);
			}
			this.chkIsWarningStock.Checked = this.isWarningStock;
		}
	}
}
