using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.SaleSystem.Supplier;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Supplier.Balance
{
	public class RequestConfirm : SupplierAdminPage
	{
		protected Literal litUserName;

		protected FormatedMoneyLabel lblDrawBanlance;

		protected Literal lblDrawRequestType;

		protected Literal litAlipayRealName;

		protected Literal litAlipayCode;

		protected Literal litBankName;

		protected Literal litAccountName;

		protected Literal litMerchantCode;

		protected Literal litRemark;

		protected Button btnDrawConfirm;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!base.IsPostBack)
			{
				SupplierBalanceDrawRequestInfo balanceDrawRequest = this.GetBalanceDrawRequest();
				this.litUserName.Text = HiContext.Current.Manager.UserName;
				this.lblDrawBanlance.Money = balanceDrawRequest.Amount;
				this.litBankName.Text = balanceDrawRequest.BankName;
				this.litAccountName.Text = balanceDrawRequest.AccountName;
				this.litMerchantCode.Text = balanceDrawRequest.MerchantCode;
				this.litRemark.Text = balanceDrawRequest.Remark;
				if (balanceDrawRequest.IsAlipay)
				{
					this.lblDrawRequestType.Text = "支付宝支付";
				}
				else
				{
					this.lblDrawRequestType.Text = "银行卡转账";
				}
				this.litAlipayRealName.Text = balanceDrawRequest.AlipayRealName;
				this.litAlipayCode.Text = balanceDrawRequest.AlipayCode;
			}
		}

		private SupplierBalanceDrawRequestInfo GetBalanceDrawRequest()
		{
			SupplierBalanceDrawRequestInfo supplierBalanceDrawRequestInfo = new SupplierBalanceDrawRequestInfo();
			supplierBalanceDrawRequestInfo.SupplierId = HiContext.Current.Manager.StoreId;
			supplierBalanceDrawRequestInfo.UserName = HiContext.Current.Manager.UserName;
			supplierBalanceDrawRequestInfo.RequestTime = DateTime.Now;
			if (!string.IsNullOrEmpty(base.GetParameter("bankName")))
			{
				supplierBalanceDrawRequestInfo.BankName = Globals.UrlDecode(base.GetParameter("bankName"));
			}
			else
			{
				supplierBalanceDrawRequestInfo.BankName = string.Empty;
			}
			if (!string.IsNullOrEmpty(base.GetParameter("accountName")))
			{
				supplierBalanceDrawRequestInfo.AccountName = Globals.UrlDecode(base.GetParameter("accountName"));
			}
			else
			{
				supplierBalanceDrawRequestInfo.AccountName = string.Empty;
			}
			if (!string.IsNullOrEmpty(base.GetParameter("merchantCode")))
			{
				supplierBalanceDrawRequestInfo.MerchantCode = Globals.UrlDecode(base.GetParameter("merchantCode"));
			}
			else
			{
				supplierBalanceDrawRequestInfo.MerchantCode = string.Empty;
			}
			decimal amount = default(decimal);
			if (!string.IsNullOrEmpty(base.GetParameter("amount")) && decimal.TryParse(base.GetParameter("amount"), out amount))
			{
				supplierBalanceDrawRequestInfo.Amount = amount;
			}
			else
			{
				supplierBalanceDrawRequestInfo.Amount = decimal.Zero;
			}
			if (!string.IsNullOrEmpty(base.GetParameter("remark")))
			{
				supplierBalanceDrawRequestInfo.Remark = Globals.UrlDecode(base.GetParameter("remark"));
			}
			else
			{
				supplierBalanceDrawRequestInfo.Remark = string.Empty;
			}
			bool flag = false;
			if (!string.IsNullOrEmpty(base.GetParameter("isalipay")) && bool.TryParse(base.GetParameter("isalipay").ToString(), out flag))
			{
				supplierBalanceDrawRequestInfo.IsAlipay = flag;
				if (flag)
				{
					supplierBalanceDrawRequestInfo.RequestState = 1.GetHashCode().ToNullString();
				}
			}
			else
			{
				supplierBalanceDrawRequestInfo.IsAlipay = false;
			}
			supplierBalanceDrawRequestInfo.IsWeixin = false;
			if (!string.IsNullOrEmpty(base.GetParameter("alipaycode")))
			{
				supplierBalanceDrawRequestInfo.AlipayCode = Globals.UrlDecode(base.GetParameter("alipaycode"));
			}
			else
			{
				supplierBalanceDrawRequestInfo.AlipayCode = string.Empty;
			}
			if (!string.IsNullOrEmpty(base.GetParameter("alipayrealname")))
			{
				supplierBalanceDrawRequestInfo.AlipayRealName = Globals.UrlDecode(base.GetParameter("alipayrealname"));
			}
			else
			{
				supplierBalanceDrawRequestInfo.AlipayRealName = string.Empty;
			}
			return supplierBalanceDrawRequestInfo;
		}

		protected void btnDrawConfirm_Click(object sender, EventArgs e)
		{
			SupplierBalanceDrawRequestInfo balanceDrawRequest = this.GetBalanceDrawRequest();
			if (BalanceHelper.BalanceDrawRequest(balanceDrawRequest))
			{
				this.Page.Response.Redirect("Default.aspx");
			}
			else
			{
				this.ShowMsg("申请提现过程中出现未知错误", false);
			}
		}
	}
}
