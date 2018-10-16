using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Products)]
	public class ProductOnDeleted : AdminPage
	{
		protected CalendarPanel calendarStartDate;

		protected CalendarPanel calendarEndDate;

		protected ProductCategoriesDropDownList dropCategories;

		protected BrandCategoriesDropDownList dropBrandList;

		protected PageSizeDropDownList PageSizeDropDownList;

		protected CheckBox chkDeleteImage;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.dropBrandList.DataBind();
			this.dropCategories.DataBind();
		}
	}
}
