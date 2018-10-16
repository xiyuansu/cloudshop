using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Store;
using Hidistro.Entities.Supplier;
using Hidistro.SaleSystem.Store;
using Hidistro.SaleSystem.Supplier;
using Hidistro.UI.Common.Controls;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Supplier.Balance
{
	[PrivilegeCheck(Privilege.SupplierBalanceDetail)]
	public class BalanceDrawRequestDetail : AdminPage
	{
		private int orderId;

		public bool isAlipay = false;

		public bool isWeixin = false;

		protected Label lblUserName;

		protected Label lblAlipayRealName;

		protected Label lblAlipayCode;

		protected Label lblBankName;

		protected Label lblAccountName;

		protected Label lblMerchantCode;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["OrderId"], out this.orderId))
			{
				base.GotoResourceNotFound();
			}
			else if (!this.Page.IsPostBack)
			{
				this.lblUserName.Text = this.Page.Request.QueryString["Name"].ToNullString();
				BalanceDrawRequestSupplierQuery query = new BalanceDrawRequestSupplierQuery
				{
					Id = this.orderId,
					PageSize = 1,
					PageIndex = 1,
					IsCount = false
				};
				PageModel<SupplierBalanceDrawRequestInfo> balanceDrawRequests = BalanceHelper.GetBalanceDrawRequests(query, false);
				if (balanceDrawRequests != null && balanceDrawRequests.Models.Count() > 0)
				{
					SupplierBalanceDrawRequestInfo supplierBalanceDrawRequestInfo = balanceDrawRequests.Models.FirstOrDefault();
					this.lblAlipayCode.Text = supplierBalanceDrawRequestInfo.AlipayCode.ToNullString();
					this.lblAlipayRealName.Text = supplierBalanceDrawRequestInfo.AlipayRealName.ToNullString();
					this.lblAccountName.Text = supplierBalanceDrawRequestInfo.AccountName.ToNullString();
					this.lblBankName.Text = supplierBalanceDrawRequestInfo.BankName.ToNullString();
					this.lblMerchantCode.Text = supplierBalanceDrawRequestInfo.MerchantCode.ToNullString();
					if (supplierBalanceDrawRequestInfo.IsAlipay.ToBool())
					{
						this.isAlipay = true;
					}
					if (supplierBalanceDrawRequestInfo.IsWeixin.ToBool())
					{
						this.isWeixin = true;
					}
				}
			}
		}
	}
}
