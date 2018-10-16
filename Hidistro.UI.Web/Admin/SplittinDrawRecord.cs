using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.SplittinDrawRecord)]
	public class SplittinDrawRecord : AdminPage
	{
		protected CalendarPanel calendarStart;

		protected CalendarPanel calendarEnd;
	}
}
