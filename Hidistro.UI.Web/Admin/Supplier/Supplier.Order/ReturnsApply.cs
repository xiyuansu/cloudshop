using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.Admin.Supplier.Order
{
	[PrivilegeCheck(Privilege.SupplierReturns)]
	public class ReturnsApply : AdminPage
	{
		public int UserStoreId = 0;

		protected int? HandleStatus;

		protected SuplierDropDownList ddlSuppliers;

		protected PageSizeDropDownList PageSizeDropDownList;

		protected HtmlInputHidden hidOrderId;

		protected HtmlInputHidden hidRetrunsId;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (HiContext.Current.Manager != null)
			{
				this.UserStoreId = HiContext.Current.Manager.StoreId;
			}
			this.LoadParameters();
			if (!base.IsPostBack)
			{
				this.ddlSuppliers.DataBind();
			}
		}

		private void LoadParameters()
		{
			string text = this.Page.Request["HandleStatus"];
			if (!string.IsNullOrWhiteSpace(text))
			{
				this.HandleStatus = text.ToInt(0);
			}
		}
	}
}
