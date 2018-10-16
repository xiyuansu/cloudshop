using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Member;
using Hidistro.UI.Common.Controls;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class RequestBalanceDrawConfirm : MemberTemplatedWebControl
	{
		private Literal litUserName;

		private FormatedMoneyLabel lblDrawBanlance;

		private Literal litBankName;

		private Literal litAccountName;

		private Literal litMerchantCode;

		private Literal litRemark;

		private IButton btnDrawConfirm;

		private Literal lblDrawRequestType;

		private Literal litAlipayRealName;

		private Literal litAlipayCode;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-RequestBalanceDrawConfirm.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.litUserName = (Literal)this.FindControl("litUserName");
			this.lblDrawBanlance = (FormatedMoneyLabel)this.FindControl("lblDrawBanlance");
			this.litBankName = (Literal)this.FindControl("litBankName");
			this.litAccountName = (Literal)this.FindControl("litAccountName");
			this.litMerchantCode = (Literal)this.FindControl("litMerchantCode");
			this.litRemark = (Literal)this.FindControl("litRemark");
			this.btnDrawConfirm = ButtonManager.Create(this.FindControl("btnDrawConfirm"));
			this.lblDrawRequestType = (Literal)this.FindControl("lblDrawRequestType");
			this.litAlipayRealName = (Literal)this.FindControl("litAlipayRealName");
			this.litAlipayCode = (Literal)this.FindControl("litAlipayCode");
			PageTitle.AddSiteNameTitle("确认申请提现");
			this.btnDrawConfirm.Click += this.btnDrawConfirm_Click;
			if (!this.Page.IsPostBack)
			{
				BalanceDrawRequestInfo balanceDrawRequest = this.GetBalanceDrawRequest();
				this.litUserName.Text = HiContext.Current.User.UserName;
				this.lblDrawBanlance.Money = balanceDrawRequest.Amount;
				this.litBankName.Text = balanceDrawRequest.BankName;
				this.litAccountName.Text = balanceDrawRequest.AccountName;
				this.litMerchantCode.Text = balanceDrawRequest.MerchantCode;
				this.litRemark.Text = balanceDrawRequest.Remark;
				if (balanceDrawRequest.IsAlipay.HasValue && balanceDrawRequest.IsAlipay.Value)
				{
					this.lblDrawRequestType.Text = "支付宝支付";
				}
				else if (balanceDrawRequest.IsWeixin.HasValue && balanceDrawRequest.IsWeixin.Value)
				{
					this.lblDrawRequestType.Text = "微信支付";
				}
				else
				{
					this.lblDrawRequestType.Text = "银行卡转账";
				}
				this.litAlipayRealName.Text = balanceDrawRequest.AlipayRealName;
				this.litAlipayCode.Text = balanceDrawRequest.AlipayCode;
			}
		}

		private void btnDrawConfirm_Click(object sender, EventArgs e)
		{
			MemberInfo user = HiContext.Current.User;
			if (user.RequestBalance > decimal.Zero)
			{
				this.ShowMessage("上笔提现管理员还没有处理，只有处理完后才能再次申请提现", false, "", 1);
			}
			else
			{
				BalanceDrawRequestInfo balanceDrawRequest = this.GetBalanceDrawRequest();
				if (this.ValidateBalanceDrawRequest(balanceDrawRequest))
				{
					if (MemberProcessor.BalanceDrawRequest(balanceDrawRequest))
					{
						this.Page.Response.Redirect("/User/MyAccountSummary");
					}
					else
					{
						this.ShowMessage("申请提现过程中出现未知错误", false, "", 1);
					}
				}
			}
		}

		private BalanceDrawRequestInfo GetBalanceDrawRequest()
		{
			BalanceDrawRequestInfo balanceDrawRequestInfo = new BalanceDrawRequestInfo();
			balanceDrawRequestInfo.UserId = HiContext.Current.UserId;
			balanceDrawRequestInfo.UserName = HiContext.Current.User.UserName;
			balanceDrawRequestInfo.RequestTime = DateTime.Now;
			if (!string.IsNullOrEmpty(base.GetParameter("bankName", false)))
			{
				balanceDrawRequestInfo.BankName = Globals.UrlDecode(base.GetParameter("bankName", false));
			}
			else
			{
				balanceDrawRequestInfo.BankName = string.Empty;
			}
			if (!string.IsNullOrEmpty(base.GetParameter("accountName", false)))
			{
				balanceDrawRequestInfo.AccountName = Globals.UrlDecode(base.GetParameter("accountName", false));
			}
			else
			{
				balanceDrawRequestInfo.AccountName = string.Empty;
			}
			if (!string.IsNullOrEmpty(base.GetParameter("merchantCode", false)))
			{
				balanceDrawRequestInfo.MerchantCode = Globals.UrlDecode(base.GetParameter("merchantCode", false));
			}
			else
			{
				balanceDrawRequestInfo.MerchantCode = string.Empty;
			}
			decimal amount = default(decimal);
			if (!string.IsNullOrEmpty(base.GetParameter("amount", false)) && decimal.TryParse(base.GetParameter("amount", false), out amount))
			{
				balanceDrawRequestInfo.Amount = amount;
			}
			else
			{
				balanceDrawRequestInfo.Amount = decimal.Zero;
			}
			if (!string.IsNullOrEmpty(base.GetParameter("remark", false)))
			{
				balanceDrawRequestInfo.Remark = Globals.UrlDecode(base.GetParameter("remark", false));
			}
			else
			{
				balanceDrawRequestInfo.Remark = string.Empty;
			}
			bool flag = false;
			OnLinePayment onLinePayment;
			if (!string.IsNullOrEmpty(base.GetParameter("isalipay", false)) && bool.TryParse(base.GetParameter("isalipay", false).ToString(), out flag))
			{
				balanceDrawRequestInfo.IsAlipay = flag;
				if (flag)
				{
					BalanceDrawRequestInfo balanceDrawRequestInfo2 = balanceDrawRequestInfo;
					onLinePayment = OnLinePayment.NoPay;
					balanceDrawRequestInfo2.RequestState = onLinePayment.GetHashCode().ToNullString();
				}
			}
			else
			{
				balanceDrawRequestInfo.IsAlipay = false;
			}
			bool flag2 = false;
			if (!string.IsNullOrEmpty(base.GetParameter("isweixin", false)) && bool.TryParse(base.GetParameter("isweixin", false).ToString(), out flag2))
			{
				balanceDrawRequestInfo.IsWeixin = flag2;
				if (flag2)
				{
					BalanceDrawRequestInfo balanceDrawRequestInfo3 = balanceDrawRequestInfo;
					onLinePayment = OnLinePayment.NoPay;
					balanceDrawRequestInfo3.RequestState = onLinePayment.GetHashCode().ToNullString();
				}
			}
			else
			{
				balanceDrawRequestInfo.IsWeixin = false;
			}
			if (!string.IsNullOrEmpty(base.GetParameter("alipaycode", false)))
			{
				balanceDrawRequestInfo.AlipayCode = Globals.UrlDecode(base.GetParameter("alipaycode", false));
			}
			else
			{
				balanceDrawRequestInfo.AlipayCode = string.Empty;
			}
			if (!string.IsNullOrEmpty(base.GetParameter("alipayrealname", false)))
			{
				balanceDrawRequestInfo.AlipayRealName = Globals.UrlDecode(base.GetParameter("alipayrealname", false));
			}
			else
			{
				balanceDrawRequestInfo.AlipayRealName = string.Empty;
			}
			return balanceDrawRequestInfo;
		}

		private bool ValidateBalanceDrawRequest(BalanceDrawRequestInfo balanceDrawRequest)
		{
			ValidationResults validationResults = Validation.Validate(balanceDrawRequest, "ValBalanceDrawRequestInfo");
			string text = string.Empty;
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult item in (IEnumerable<ValidationResult>)validationResults)
				{
					text += Formatter.FormatErrorMessage(item.Message);
				}
				this.ShowMessage(text, false, "", 1);
			}
			return validationResults.IsValid;
		}
	}
}
