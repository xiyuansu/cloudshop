using Hidistro.Context;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.SplittinDrawRequest)]
	public class SplittinDrawRequestWeixin : AdminPage
	{
		private SiteSettings siteSettings;

		protected HiddenField hidIsDemoSite;

		protected Literal litUserName;

		protected CalendarPanel calendarStart;

		protected CalendarPanel calendarEnd;

		protected PageSizeDropDownList PageSizeDropDownList;

		protected TextBox txtReason;

		protected HiddenField JournalNumber;

		protected HiddenField ChargeType;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.siteSettings = SettingsManager.GetMasterSettings();
			this.hidIsDemoSite.Value = (this.siteSettings.IsDemoSite ? "1" : "0");
		}
	}
}
