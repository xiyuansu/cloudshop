using Hidistro.Context;
using Hidistro.Core;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Depot.product
{
	public class StoreStockAdjustLog : StoreAdminPage
	{
		private DateTime? startTime;

		private DateTime? endTime;

		protected int ProductId;

		protected HtmlGenericControl divSearchBox;

		protected HiddenField hidProductId;

		protected HiddenField hidStoreId;

		protected CalendarPanel cldStartDate;

		protected CalendarPanel cldEndDate;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.ProductId = base.Request.QueryString["ProductId"].ToInt(0);
			if (this.ProductId <= 0)
			{
				base.GotoResourceNotFound();
			}
			else
			{
				this.LoadParameters();
			}
		}

		private void LoadParameters()
		{
			this.hidProductId.Value = this.ProductId.ToString();
			this.hidStoreId.Value = HiContext.Current.Manager.StoreId.ToString();
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["startTime"]))
			{
				this.startTime = Globals.UrlDecode(this.Page.Request.QueryString["startTime"]).ToDateTime();
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["endTime"]))
			{
				this.endTime = Globals.UrlDecode(this.Page.Request.QueryString["endTime"]).ToDateTime();
			}
			CalendarPanel calendarPanel = this.cldStartDate;
			object text;
			DateTime value;
			if (!this.startTime.HasValue)
			{
				text = string.Empty;
			}
			else
			{
				value = this.startTime.Value;
				text = value.ToString("yyyy-MM-dd");
			}
			calendarPanel.Text = (string)text;
			CalendarPanel calendarPanel2 = this.cldEndDate;
			object text2;
			if (!this.endTime.HasValue)
			{
				text2 = string.Empty;
			}
			else
			{
				value = this.endTime.Value;
				text2 = value.ToString("yyyy-MM-dd");
			}
			calendarPanel2.Text = (string)text2;
		}
	}
}
