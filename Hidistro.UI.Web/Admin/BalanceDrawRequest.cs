using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.BalanceDrawRequest)]
	public class BalanceDrawRequest : AdminPage
	{
		protected CalendarPanel calendarStart;

		protected CalendarPanel calendarEnd;

		protected TextBox txtReason;

		protected HiddenField BDRID;

		protected HiddenField ChargeType;

		protected void Page_Load(object sender, EventArgs e)
		{
		}
	}
}
