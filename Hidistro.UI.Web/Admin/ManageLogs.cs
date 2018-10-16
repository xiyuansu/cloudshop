using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;

namespace Hidistro.UI.Web.Admin
{
	[AdministerCheck(true)]
	public class ManageLogs : AdminPage
	{
		protected LogsUserNameDropDownList dropOperationUserNames;

		protected CalendarPanel calenderFromDate;

		protected CalendarPanel calenderToDate;

		protected PageSizeDropDownList PageSizeDropDownList;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.dropOperationUserNames.DataBind();
		}
	}
}
