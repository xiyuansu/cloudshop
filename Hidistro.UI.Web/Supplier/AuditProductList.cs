using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Supplier
{
	public class AuditProductList : SupplierAdminPage
	{
		protected HiddenField hidProductId;

		protected ProductCategoriesDropDownList dropCategories;

		protected PageSizeDropDownList PageSizeDropDownList;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.dropCategories.DataBind();
		}
	}
}
