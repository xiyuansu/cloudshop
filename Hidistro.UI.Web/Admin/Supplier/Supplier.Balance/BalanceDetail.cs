using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Supplier.Balance
{
	[PrivilegeCheck(Privilege.SupplierBalanceDetail)]
	public class BalanceDetail : AdminPage
	{
		public int SupplierId;

		protected Label lblName;

		protected HiddenField hidSupplierId;

		protected CalendarPanel calendarStartDate;

		protected CalendarPanel calendarEndDate;

		protected DropDownList ddlType;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.SupplierId = base.Request.QueryString["SupplierId"].ToInt(0);
			this.hidSupplierId.Value = this.SupplierId.ToString();
			this.lblName.Text = base.Request.QueryString["Name"].ToNullString();
		}
	}
}
