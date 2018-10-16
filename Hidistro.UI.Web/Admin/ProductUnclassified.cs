using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.ProductUnclassified)]
	public class ProductUnclassified : AdminPage
	{
		protected CalendarPanel calendarStartDate;

		protected CalendarPanel calendarEndDate;

		protected ProductCategoriesDropDownList dropCategories;

		protected ProductTypeDownList dropType;

		protected BrandCategoriesDropDownList dropBrandList;

		protected PageSizeDropDownList PageSizeDropDownList;

		protected HiddenField txtCurrentExtendIndex;

		protected ProductCategoriesDropDownList dropMoveToCategories;

		protected ProductCategoriesDropDownList dropAddToAllCategories;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.dropBrandList.DataBind();
			this.dropMoveToCategories.DataBind();
			this.dropAddToAllCategories.DataBind();
			this.dropCategories.DataBind();
			this.dropType.DataBind();
		}
	}
}
