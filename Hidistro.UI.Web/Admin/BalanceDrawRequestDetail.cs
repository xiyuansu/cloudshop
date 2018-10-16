using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Members;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.MemberDrawRequestStatistics)]
	public class BalanceDrawRequestDetail : AdminPage
	{
		private int inpourId;

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
			if (!int.TryParse(this.Page.Request.QueryString["InpourId"], out this.inpourId))
			{
				base.GotoResourceNotFound();
			}
			else if (!this.Page.IsPostBack)
			{
				this.lblUserName.Text = this.Page.Request.QueryString["Name"].ToNullString();
				BalanceDrawRequestQuery query = new BalanceDrawRequestQuery
				{
					Id = this.inpourId,
					PageSize = 1,
					PageIndex = 1,
					IsCount = false
				};
				DbQueryResult balanceDrawRequests = MemberHelper.GetBalanceDrawRequests(query, false);
				if (balanceDrawRequests != null && balanceDrawRequests.Data.Rows.Count > 0)
				{
					DataRow dataRow = balanceDrawRequests.Data.Rows[0];
					this.lblAlipayCode.Text = dataRow["AlipayCode"].ToNullString();
					this.lblAlipayRealName.Text = dataRow["AlipayRealName"].ToNullString();
					this.lblAccountName.Text = dataRow["AccountName"].ToNullString();
					this.lblBankName.Text = dataRow["BankName"].ToNullString();
					this.lblMerchantCode.Text = dataRow["MerchantCode"].ToNullString();
					if (dataRow["IsAlipay"].ToBool())
					{
						this.isAlipay = true;
					}
					if (dataRow["IsWeixin"].ToBool())
					{
						this.isWeixin = true;
					}
				}
			}
		}
	}
}
