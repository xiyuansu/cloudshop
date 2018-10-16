using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.Admin.Supplier
{
	[PrivilegeCheck(Privilege.SupplierPdList)]
	public class ProductList : AdminPage
	{
		protected bool isWarningStock;

		private string saleState = string.Empty;

		protected ProductSaleStatus saleStatus = ProductSaleStatus.All;

		protected HtmlInputText txtSearchText;

		protected HtmlInputText txtSKU;

		protected ProductCategoriesDropDownList dropCategories;

		protected SuplierDropDownList dropSuplier;

		protected BrandCategoriesDropDownList dropBrandList;

		protected ProductTagsDropDownList dropTagList;

		protected HtmlInputText so_more_input;

		protected HtmlGenericControl so_more_none;

		protected ProductTypeDownList dropType;

		protected PageSizeDropDownList PageSizeDropDownList;

		protected TrimTextBox txtProductTag;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.LoadParameters();
			if (!this.Page.IsPostBack)
			{
				this.dropCategories.DataBind();
				this.dropBrandList.DataBind();
				this.dropTagList.DataBind();
				this.dropType.DataBind();
				this.dropSuplier.DataBind();
				this.dropCategories.DataBind();
				this.dropTagList.DataBind();
				string value = base.Request.QueryString["ProductName"].ToNullString();
				if (!string.IsNullOrEmpty(value))
				{
					this.txtSearchText.Value = value;
				}
				string value2 = base.Request.QueryString["productCode"].ToNullString();
				if (!string.IsNullOrEmpty(value2))
				{
					this.txtSKU.Value = value2;
				}
				int num = base.Request.QueryString["BrandId"].ToInt(0);
				if (num > 0)
				{
					this.dropBrandList.SelectedValue = num;
				}
				int num2 = base.Request.QueryString["TagId"].ToInt(0);
				if (num2 > 0)
				{
					this.dropTagList.SelectedValue = num2;
				}
				int num3 = base.Request.QueryString["TypeId"].ToInt(0);
				if (num3 > 0)
				{
					this.dropType.SelectedValue = num3;
				}
				int num4 = this.Page.Request["CategoryId"].ToInt(0);
				this.dropCategories.DataBind();
				if (num4 > 0)
				{
					this.dropCategories.SelectedValue = num4;
				}
			}
		}

		private void LoadParameters()
		{
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["SaleStatus"]))
			{
				this.saleStatus = (ProductSaleStatus)Enum.Parse(typeof(ProductSaleStatus), this.Page.Request.QueryString["SaleStatus"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["isWarningStock"]))
			{
				this.isWarningStock = (this.Page.Request.QueryString["isWarningStock"] == "1" && true);
			}
			int num = this.Page.Request["SupplierId"].ToInt(0);
			if (num > 0)
			{
				this.dropSuplier.SelectedValue = num.ToString();
			}
		}
	}
}
