using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Helps)]
	public class HelpList : AdminPage
	{
		protected HelpCategoryDropDownList dropHelpCategory;

		protected CalendarPanel calendarStartDataTime;

		protected CalendarPanel calendarEndDataTime;

		protected PageSizeDropDownList PageSizeDropDownList;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.dropHelpCategory.DataBind();
		}
	}
}
