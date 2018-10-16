using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.EditProducts)]
	public class DisplaceCategory : AdminCallBackPage
	{
		protected ProductCategoriesDropDownList dropCategoryFrom;

		protected ProductCategoriesDropDownList dropCategoryTo;

		protected Button btnSaveCategory;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnSaveCategory.Click += this.btnSaveCategory_Click;
			if (!this.Page.IsPostBack)
			{
				this.dropCategoryFrom.DataBind();
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["CategoryId"]))
				{
					int value = 0;
					if (int.TryParse(this.Page.Request.QueryString["CategoryId"], out value))
					{
						this.dropCategoryFrom.SelectedValue = value;
					}
				}
			}
		}

		private void btnSaveCategory_Click(object sender, EventArgs e)
		{
			if (!this.dropCategoryFrom.SelectedValue.HasValue || !this.dropCategoryTo.SelectedValue.HasValue)
			{
				this.ShowMsg("请选择需要替换的商品分类或需要替换至的商品分类", false);
			}
			else if (this.dropCategoryFrom.SelectedValue.Value == this.dropCategoryTo.SelectedValue.Value)
			{
				this.ShowMsg("请选择不同的商品分类进行替换", false);
			}
			else if (CatalogHelper.DisplaceCategory(this.dropCategoryFrom.SelectedValue.Value, this.dropCategoryTo.SelectedValue.Value) == 0)
			{
				this.ShowMsg("此分类下没有可以替换的商品", false);
			}
			else
			{
				base.CloseWindow(null);
			}
		}
	}
}
