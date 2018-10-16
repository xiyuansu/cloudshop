using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;

namespace Hidistro.UI.Web.Admin.Supplier
{
	[PrivilegeCheck(Privilege.SupplierList)]
	public class SupplierList : AdminPage
	{
		protected int Status = 0;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.LoadParameters();
		}

		public void LoadParameters()
		{
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Status"]) && this.Page.Request.QueryString["Status"] != null)
			{
				int.TryParse(this.Page.Request.QueryString["Status"], out this.Status);
			}
		}
	}
}
