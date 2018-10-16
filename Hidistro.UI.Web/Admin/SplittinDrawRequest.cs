using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.SplittinDrawRequest)]
	public class SplittinDrawRequest : AdminPage
	{
		protected Literal litUserName;

		protected CalendarPanel calendarStart;

		protected CalendarPanel calendarEnd;

		protected TextBox txtReason;

		protected HiddenField JournalNumber;

		protected HiddenField ChargeType;
	}
}
