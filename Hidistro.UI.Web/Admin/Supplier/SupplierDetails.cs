using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.Entities.Supplier;
using Hidistro.SaleSystem.Store;
using Hidistro.SaleSystem.Supplier;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Supplier
{
	[PrivilegeCheck(Privilege.SupplierDetails)]
	public class SupplierDetails : AdminPage
	{
		private int supplierId = 0;

		protected Literal lblUserName;

		protected Literal lblSupplierName;

		protected Literal lblStatus;

		protected Literal lblContactMan;

		protected Literal lblTel;

		protected Literal lblAddress;

		[AdministerCheck(true)]
		protected void Page_Load(object sender, EventArgs e)
		{
			this.supplierId = base.Request.QueryString["SupplierId"].ToInt(0);
			if (this.supplierId == 0)
			{
				base.Response.Redirect("SuppliersList.aspx");
			}
			if (!this.Page.IsPostBack)
			{
				this.BindData();
			}
		}

		public void BindData()
		{
			SupplierInfo supplierById = SupplierHelper.GetSupplierById(this.supplierId);
			ManagerInfo managerInfo = ManagerHelper.FindManagerByStoreIdAndRoleId(this.supplierId, -2);
			if (supplierById == null || managerInfo == null)
			{
				base.Response.Redirect("SuppliersList.aspx");
			}
			else
			{
				this.lblUserName.Text = managerInfo.UserName;
				this.lblSupplierName.Text = supplierById.SupplierName;
				this.lblStatus.Text = (supplierById.Status.Equals(1) ? "正常" : "<font color='red'>冻结</font>");
				this.lblContactMan.Text = supplierById.ContactMan;
				this.lblTel.Text = supplierById.Tel;
				this.lblAddress.Text = RegionHelper.GetFullRegion(supplierById.RegionId, string.Empty, true, 0);
				Literal literal = this.lblAddress;
				literal.Text += supplierById.Address;
			}
		}
	}
}
