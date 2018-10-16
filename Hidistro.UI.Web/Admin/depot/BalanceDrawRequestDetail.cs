using Hidistro.Core;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.depot
{
	[PrivilegeCheck(Privilege.StoreBalance)]
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
				StoreBalanceDrawRequestQuery query = new StoreBalanceDrawRequestQuery
				{
					Id = this.orderId,
					PageSize = 1,
					PageIndex = 1,
					IsCount = false
				};
				PageModel<StoreBalanceDrawRequestInfo> balanceDrawRequests = StoreBalanceHelper.GetBalanceDrawRequests(query, false);
				if (balanceDrawRequests != null && balanceDrawRequests.Models.Count() > 0)
				{
					StoreBalanceDrawRequestInfo storeBalanceDrawRequestInfo = balanceDrawRequests.Models.FirstOrDefault();
					this.lblAlipayCode.Text = storeBalanceDrawRequestInfo.AlipayCode.ToNullString();
					this.lblAlipayRealName.Text = storeBalanceDrawRequestInfo.AlipayRealName.ToNullString();
					this.lblAccountName.Text = storeBalanceDrawRequestInfo.AccountName.ToNullString();
					this.lblBankName.Text = storeBalanceDrawRequestInfo.BankName.ToNullString();
					this.lblMerchantCode.Text = storeBalanceDrawRequestInfo.MerchantCode.ToNullString();
					if (storeBalanceDrawRequestInfo.IsAlipay.ToBool())
					{
						this.isAlipay = true;
					}
				}
			}
		}
	}
}
