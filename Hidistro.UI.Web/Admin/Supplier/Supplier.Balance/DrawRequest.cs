using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Supplier.Balance
{
	[PrivilegeCheck(Privilege.SupplierDrawList)]
	public class DrawRequest : AdminPage
	{
		protected SuplierDropDownList dropSuplier;

		protected CalendarPanel calendarStartDate;

		protected CalendarPanel calendarEndDate;

		protected TextBox txtReason;

		protected HiddenField BDRID;

		protected HiddenField ChargeType;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.LoadParameters();
			if (!base.IsPostBack)
			{
				this.dropSuplier.DataBind();
			}
		}

		private void LoadParameters()
		{
		}
	}
}
