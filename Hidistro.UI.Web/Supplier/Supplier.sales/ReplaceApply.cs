using Hidistro.Context;
using Hidistro.Core;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Supplier.sales
{
	[AdministerCheck(true)]
	public class ReplaceApply : SupplierAdminPage
	{
		public int UserStoreId = 0;

		protected int handleStatus = -1;

		protected Label lblStatus;

		protected HtmlInputHidden hidOrderId;

		protected HtmlInputHidden hidReplaceId;

		protected HtmlInputHidden hidHandleStatus;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (HiContext.Current.Manager != null)
			{
				this.UserStoreId = HiContext.Current.Manager.StoreId;
			}
			this.LoadParameters();
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
			this.hidHandleStatus.Value = this.handleStatus.ToNullString();
		}
	}
}
