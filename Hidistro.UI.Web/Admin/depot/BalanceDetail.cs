using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.depot
{
	[PrivilegeCheck(Privilege.StoreBalanceDetail)]
	public class BalanceDetail : AdminPage
	{
		public int storeId;

		protected Label lblName;

		protected HiddenField hidStoreId;

		protected CalendarPanel calendarStartDate;

		protected CalendarPanel calendarEndDate;

		protected DropDownList ddlType;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.storeId = base.Request.QueryString["StoreId"].ToInt(0);
			this.hidStoreId.Value = this.storeId.ToString();
			this.lblName.Text = base.Request.QueryString["Name"].ToNullString();
		}
	}
}
