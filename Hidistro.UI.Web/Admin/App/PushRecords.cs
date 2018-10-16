using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;

namespace Hidistro.UI.Web.Admin.App
{
	[PrivilegeCheck(Privilege.AppPushRecords)]
	public class PushRecords : AdminPage
	{
		protected PushTypeDropDownList ddlPushType;

		protected CalendarPanel calendarStart;

		protected CalendarPanel calendarEnd;

		protected PushStatusDropDownList ddlPushStatus;

		protected PageSizeDropDownList PageSizeDropDownList;
	}
}
