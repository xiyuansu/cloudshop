using Hidistro.Context;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.BalanceDrawRequest)]
	public class BalanceDrawRequestAlipay : AdminPage
	{
		protected HiddenField hidIsDemoSite;

		protected CalendarPanel calendarStart;

		protected CalendarPanel calendarEnd;

		protected PageSizeDropDownList PageSizeDropDownList;

		protected TextBox txtReason;

		protected HiddenField BDRID;

		protected HiddenField ChargeType;

		protected void Page_Load(object sender, EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			this.hidIsDemoSite.Value = (masterSettings.IsDemoSite ? "1" : "0");
		}
	}
}
