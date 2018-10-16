using Hidistro.Context;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.Supplier.sales
{
	[AdministerCheck(true)]
	public class ReturnsApply : SupplierAdminPage
	{
		public int UserStoreId = 0;

		protected int handleStatus = -1;

		protected HtmlInputHidden hidOrderId;

		protected HtmlInputHidden hidRetrunsId;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.LoadParameters();
			if (HiContext.Current.Manager != null)
			{
				this.UserStoreId = HiContext.Current.Manager.StoreId;
			}
		}

		private void LoadParameters()
		{
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["HandleStatus"]))
			{
				int num = 0;
				if (int.TryParse(this.Page.Request.QueryString["HandleStatus"], out num) && num > -1)
				{
					num = (this.handleStatus = ((num == 0) ? 4 : num));
				}
			}
		}
	}
}
