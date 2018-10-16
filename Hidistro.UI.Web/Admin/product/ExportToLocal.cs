using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;

namespace Hidistro.UI.Web.Admin.product
{
	[PrivilegeCheck(Privilege.ProductBatchExport)]
	public class ExportToLocal : AdminPage
	{
		protected CalendarPanel calendarStartDate;

		protected CalendarPanel calendarEndDates;

		protected ProductCategoriesDropDownList dropCategories;

		protected OnOff chkExportImages;

		protected PageSizeDropDownList PageSizeDropDownList;

		protected override void OnInitComplete(EventArgs e)
		{
			base.OnInitComplete(e);
			this.dropCategories.DataBind();
		}
	}
}
