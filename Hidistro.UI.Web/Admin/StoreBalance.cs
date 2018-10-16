using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.StoreBalance)]
	public class StoreBalance : AdminPage
	{
		protected CalendarPanel startDate;

		protected CalendarPanel endDate;

		protected StoreDropDownList ddlStores;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.ddlStores.DataBind();
			this.ddlStores.Items.Remove(new ListItem("平台", "0"));
			CalendarPanel calendarPanel = this.startDate;
			object[] obj = new object[4];
			DateTime dateTime = DateTime.Now;
			obj[0] = dateTime.Year;
			obj[1] = "-";
			dateTime = DateTime.Now;
			obj[2] = dateTime.Month;
			obj[3] = "-01";
			calendarPanel.SelectedDate = string.Concat(obj).ToDateTime();
			CalendarPanel calendarPanel2 = this.endDate;
			object[] obj2 = new object[4];
			dateTime = DateTime.Now;
			dateTime = dateTime.AddMonths(1);
			obj2[0] = dateTime.Year;
			obj2[1] = "-";
			dateTime = DateTime.Now;
			dateTime = dateTime.AddMonths(1);
			obj2[2] = dateTime.Month;
			obj2[3] = "-01";
			dateTime = string.Concat(obj2).ToDateTime().Value;
			calendarPanel2.SelectedDate = dateTime.AddDays(-1.0);
			this.LoadParameters();
		}

		private void LoadParameters()
		{
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["startDate"]))
			{
				this.startDate.SelectedDate = this.Page.Request.QueryString["startDate"].Trim().ToDateTime();
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["endDate"]))
			{
				this.endDate.SelectedDate = this.Page.Request.QueryString["endDate"].Trim().ToDateTime();
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["storeId"]))
			{
				this.ddlStores.SelectedValue = this.Page.Request.QueryString["storeId"].Trim();
			}
		}
	}
}
