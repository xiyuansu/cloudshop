using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;

namespace Hidistro.UI.Web.Admin.depot
{
	[PrivilegeCheck(Privilege.HIPOSDeal)]
	[HiPOSCheck(true)]
	public class HiPOSDetails : AdminPage
	{
		protected string storeName;

		private DateTime? startDate;

		private DateTime? endDate;

		protected CalendarPanel caStartDate;

		protected CalendarPanel caEndDate;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.LoadParameters();
			Dictionary<string, object> calendarParameter = this.caStartDate.CalendarParameter;
			DateTime now = DateTime.Now;
			calendarParameter.Add("endDate", now.ToString("yyyy-MM-dd"));
			Dictionary<string, object> calendarParameter2 = this.caEndDate.CalendarParameter;
			now = DateTime.Now;
			calendarParameter2.Add("endDate ", now.ToString("yyyy-MM-dd"));
		}

		private void LoadParameters()
		{
			if (!this.Page.IsPostBack)
			{
				this.storeName = this.Page.Request.QueryString["storeName"];
				if (!string.IsNullOrWhiteSpace(this.storeName))
				{
					this.storeName = base.Server.UrlDecode(this.storeName);
				}
				DateTime value;
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["startDate"]))
				{
					this.startDate = DateTime.Parse(this.Page.Request.QueryString["startDate"]);
					CalendarPanel calendarPanel = this.caStartDate;
					value = this.startDate.Value;
					calendarPanel.Text = value.ToString("yyyy-MM-dd");
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["endDate"]))
				{
					this.endDate = DateTime.Parse(this.Page.Request.QueryString["endDate"]);
					CalendarPanel calendarPanel2 = this.caEndDate;
					value = this.endDate.Value;
					calendarPanel2.Text = value.ToString("yyyy-MM-dd");
				}
				this.caEndDate.SelectedDate = this.endDate;
				this.caStartDate.SelectedDate = this.startDate;
			}
		}
	}
}
