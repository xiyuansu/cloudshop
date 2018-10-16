using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.MemberBalanceStatistics)]
	public class BalanceStatistics : AdminPage
	{
		protected CalendarPanel calendarStart;

		protected CalendarPanel calendarEnd;

		protected void Page_Load(object sender, EventArgs e)
		{
		}
	}
}
