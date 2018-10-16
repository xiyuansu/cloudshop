using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.ReferralRequest)]
	public class ReferralRequest : AdminPage
	{
		protected CalendarPanel calendarStartDate;

		protected CalendarPanel calendarEndDate;

		protected Label lblUserName;

		protected Label lblPersonMsg;

		protected TextBox txtRefusalReason;

		protected HtmlInputHidden hidUserId;

		protected HtmlInputHidden hidRefusalReason;

		protected void Page_Load(object sender, EventArgs e)
		{
		}
	}
}
