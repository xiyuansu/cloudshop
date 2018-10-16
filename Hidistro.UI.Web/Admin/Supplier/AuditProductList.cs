using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Supplier
{
	[PrivilegeCheck(Privilege.SupplierAuditPdList)]
	public class AuditProductList : AdminPage
	{
		protected HiddenField hidProductId;

		protected HtmlInputText txtSearchText;

		protected SuplierDropDownList dropSuplier;

		protected ProductCategoriesDropDownList dropCategories;

		protected PageSizeDropDownList PageSizeDropDownList;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.Page.IsPostBack && !this.Page.IsPostBack)
			{
				this.dropCategories.DataBind();
				this.dropSuplier.DataBind();
				string value = base.Request.QueryString["ProductName"].ToNullString();
				if (!string.IsNullOrEmpty(value))
				{
					this.txtSearchText.Value = value;
				}
				int num = this.Page.Request["CategoryId"].ToInt(0);
				this.dropCategories.DataBind();
				if (num > 0)
				{
					this.dropCategories.SelectedValue = num;
				}
				int num2 = this.Page.Request["SupplierId"].ToInt(0);
				if (num2 > 0)
				{
					this.dropSuplier.SelectedValue = num2.ToString();
				}
				if (!string.IsNullOrEmpty(base.Request.QueryString["productId"]))
				{
					this.hidProductId.Value = base.Request.QueryString["productId"];
				}
			}
		}
	}
}
