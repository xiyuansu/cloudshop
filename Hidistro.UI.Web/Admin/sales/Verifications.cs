using Hidistro.Context;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.sales
{
	public class Verifications : AdminPage
	{
		public int UserStoreId = 0;

		protected int? HandleStatus;

		protected StoreDropDownList ddlSearchStore;

		protected CalendarPanel cldCreateStart;

		protected CalendarPanel cldCreateEnd;

		protected CalendarPanel cldVerificationStart;

		protected CalendarPanel cldVerificationEnd;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (HiContext.Current.Manager != null)
			{
				this.UserStoreId = HiContext.Current.Manager.StoreId;
			}
			this.ddlSearchStore.DataBind();
			this.LoadParameters();
		}

		private void LoadParameters()
		{
			string b = this.Page.Request["StoreId"];
			foreach (ListItem item in this.ddlSearchStore.Items)
			{
				if (item.Value == b)
				{
					item.Selected = true;
				}
			}
		}
	}
}
