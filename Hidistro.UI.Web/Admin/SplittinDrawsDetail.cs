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
	[PrivilegeCheck(Privilege.SplittinDrawRecord)]
	public class SplittinDrawsDetail : AdminPage
	{
		private int journalNumber;

		public bool isAlipay = false;

		public bool isWeixin = false;

		public bool isWithdrawToAccount = false;

		protected Label lblUserName;

		protected Label lblAlipayRealName;

		protected Label lblAlipayCode;

		protected Label lblBankName;

		protected Label lblAccountName;

		protected Label lblMerchantCode;

		protected Label lblMoney;

		protected Label LabRemark;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["JournalNumber"], out this.journalNumber))
			{
				base.GotoResourceNotFound();
			}
			else if (!this.Page.IsPostBack)
			{
				this.lblUserName.Text = this.Page.Request.QueryString["Name"].ToNullString();
				BalanceDrawRequestQuery query = new BalanceDrawRequestQuery
				{
					JournalNumber = this.journalNumber,
					PageSize = 1,
					PageIndex = 1,
					IsCount = false
				};
				DbQueryResult splittinDraws = MemberHelper.GetSplittinDraws(query, null);
				if (splittinDraws != null && splittinDraws.Data.Rows.Count > 0)
				{
					DataRow dataRow = splittinDraws.Data.Rows[0];
					this.lblAlipayCode.Text = dataRow["AlipayCode"].ToNullString();
					this.lblAlipayRealName.Text = dataRow["AlipayRealName"].ToNullString();
					this.lblAccountName.Text = dataRow["AccountName"].ToNullString();
					this.lblBankName.Text = dataRow["BankName"].ToNullString();
					this.lblMerchantCode.Text = dataRow["MerchantCode"].ToNullString();
					this.lblMoney.Text = dataRow["Amount"].ToDecimal(0).ToString("0.00");
					this.LabRemark.Text = dataRow["ManagerRemark"].ToNullString();
					if (dataRow["IsAlipay"].ToBool())
					{
						this.isAlipay = true;
					}
					if (dataRow["IsWeixin"].ToBool())
					{
						this.isWeixin = true;
					}
					if (dataRow["IsWithdrawToAccount"].ToBool())
					{
						this.isWithdrawToAccount = true;
					}
				}
			}
		}
	}
}
