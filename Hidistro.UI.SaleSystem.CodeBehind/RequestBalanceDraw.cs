using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Members;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class RequestBalanceDraw : MemberTemplatedWebControl
	{
		private Literal litUserName;

		private FormatedMoneyLabel lblBanlance;

		private TextBox txtAmount;

		private TextBox txtBankName;

		private TextBox txtAccountName;

		private TextBox txtMerchantCode;

		private TextBox txtRemark;

		private TextBox txtTradePassword;

		private IButton btnDrawNext;

		private RadioButton IsDefault;

		private RadioButton IsWeixin;

		private RadioButton IsAlipay;

		private TextBox txtAlipayRealName;

		private TextBox txtAlipayCode;

		private Literal lblminDraws;

		private Common_Referral_RequestBalanceDraw common_referral_requestbalancedraw;

		private Pager pager;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-RequestBalanceDraw.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.litUserName = (Literal)this.FindControl("litUserName");
			this.lblBanlance = (FormatedMoneyLabel)this.FindControl("lblBanlance");
			this.txtAmount = (TextBox)this.FindControl("txtAmount");
			this.txtBankName = (TextBox)this.FindControl("txtBankName");
			this.txtAccountName = (TextBox)this.FindControl("txtAccountName");
			this.txtMerchantCode = (TextBox)this.FindControl("txtMerchantCode");
			this.txtRemark = (TextBox)this.FindControl("txtRemark");
			this.txtTradePassword = (TextBox)this.FindControl("txtTradePassword");
			this.btnDrawNext = ButtonManager.Create(this.FindControl("btnDrawNext"));
			this.IsDefault = (RadioButton)this.FindControl("IsDefault");
			this.IsWeixin = (RadioButton)this.FindControl("IsWeixin");
			this.IsAlipay = (RadioButton)this.FindControl("IsAlipay");
			this.txtAlipayRealName = (TextBox)this.FindControl("txtAlipayRealName");
			this.txtAlipayCode = (TextBox)this.FindControl("txtAlipayCode");
			this.lblminDraws = (Literal)this.FindControl("lblminDraws");
			this.btnDrawNext.Click += this.btnDrawNext_Click;
			this.common_referral_requestbalancedraw = (Common_Referral_RequestBalanceDraw)this.FindControl("Common_Referral_RequestBalance");
			this.pager = (Pager)this.FindControl("pager");
			PageTitle.AddSiteNameTitle("申请提现");
			if (!this.Page.IsPostBack)
			{
				MemberInfo user = HiContext.Current.User;
				if (!user.IsOpenBalance || string.IsNullOrEmpty(user.TradePassword))
				{
					this.Page.Response.Redirect($"/user/OpenBalance?ReturnUrl={HttpContext.Current.Request.Url}");
				}
				BalanceDrawRequestQuery balanceDrawRequestQuery = new BalanceDrawRequestQuery();
				balanceDrawRequestQuery.UserId = HiContext.Current.User.UserId;
				balanceDrawRequestQuery.PageIndex = this.pager.PageIndex;
				balanceDrawRequestQuery.PageSize = this.pager.PageSize;
				DbQueryResult balanceDrawRequests = MemberHelper.GetBalanceDrawRequests(balanceDrawRequestQuery, false);
				this.common_referral_requestbalancedraw.DataSource = balanceDrawRequests.Data;
				this.common_referral_requestbalancedraw.DataBind();
				if (user.RequestBalance > decimal.Zero)
				{
					this.ShowMessage("提现申请成功，等待管理员审核", true, "", 1);
					this.btnDrawNext.Visible = false;
				}
				else
				{
					this.litUserName.Text = HiContext.Current.User.UserName;
					this.lblBanlance.Money = user.Balance - user.RequestBalance;
					SiteSettings masterSettings = SettingsManager.GetMasterSettings();
					this.lblminDraws.Text = masterSettings.MinimumSingleShot.F2ToString("f2");
					if (!masterSettings.EnableBulkPaymentAliPay)
					{
						this.IsAlipay.Visible = false;
					}
					if (!masterSettings.EnableBulkPaymentWeixin)
					{
						this.IsWeixin.Visible = false;
					}
					else if (user.MemberOpenIds == null)
					{
						this.IsWeixin.Visible = false;
					}
					else
					{
						MemberOpenIdInfo memberOpenIdInfo = user.MemberOpenIds.FirstOrDefault((MemberOpenIdInfo item) => item.OpenIdType.ToLower() == "hishop.plugins.openid.weixin");
						if (memberOpenIdInfo == null)
						{
							this.IsWeixin.Visible = false;
						}
					}
					this.IsDefault.Checked = true;
				}
			}
		}

		private void btnDrawNext_Click(object sender, EventArgs e)
		{
			MemberInfo user = HiContext.Current.User;
			if (user.RequestBalance > decimal.Zero)
			{
				this.ShowMessage("上笔提现管理员还没有处理，只有处理完后才能再次申请提现", false, "", 1);
			}
			else
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				decimal num = default(decimal);
				if (!decimal.TryParse(this.txtAmount.Text.Trim(), out num))
				{
					this.ShowMessage("提现金额输入错误,请重新输入提现金额", false, "", 1);
				}
				else if (num > user.Balance)
				{
					this.ShowMessage("预付款余额不足,请重新输入提现金额", false, "", 1);
				}
				else if (num < masterSettings.MinimumSingleShot || (num < decimal.One && this.IsWeixin.Checked))
				{
					this.ShowMessage("请输入提现金额大于或者等于单次提现最小限额" + masterSettings.MinimumSingleShot + "元,使用微信提现必须大于等于1元", false, "", 1);
				}
				else if (string.IsNullOrEmpty(this.txtTradePassword.Text))
				{
					this.ShowMessage("请输入交易密码", false, "", 1);
				}
				else if (!MemberProcessor.ValidTradePassword(this.txtTradePassword.Text))
				{
					this.ShowMessage("交易密码不正确,请重新输入！", false, "", 1);
				}
				else if (!string.IsNullOrEmpty(this.txtRemark.Text) && this.txtRemark.Text.Length > 100)
				{
					this.ShowMessage("备注不能超过100个字", false, "", 1);
				}
				else
				{
					BalanceDrawRequestInfo balanceDrawRequestInfo = new BalanceDrawRequestInfo();
					balanceDrawRequestInfo.BankName = this.txtBankName.Text.Trim();
					balanceDrawRequestInfo.AccountName = this.txtAccountName.Text.Trim();
					balanceDrawRequestInfo.MerchantCode = this.txtMerchantCode.Text.Trim();
					balanceDrawRequestInfo.Amount = num;
					balanceDrawRequestInfo.Remark = this.txtRemark.Text.Trim();
					balanceDrawRequestInfo.IsAlipay = this.IsAlipay.Checked;
					balanceDrawRequestInfo.IsWeixin = this.IsWeixin.Checked;
					balanceDrawRequestInfo.AlipayCode = this.txtAlipayCode.Text.Trim();
					balanceDrawRequestInfo.AlipayRealName = this.txtAlipayRealName.Text;
					if (this.IsAlipay.Checked)
					{
						balanceDrawRequestInfo.BankName = "0";
						balanceDrawRequestInfo.AccountName = "0";
						balanceDrawRequestInfo.MerchantCode = "0";
					}
					else if (this.IsWeixin.Checked)
					{
						balanceDrawRequestInfo.BankName = "0";
						balanceDrawRequestInfo.AccountName = "0";
						balanceDrawRequestInfo.MerchantCode = "0";
						balanceDrawRequestInfo.AlipayCode = "0";
						balanceDrawRequestInfo.AlipayRealName = "0";
					}
					else
					{
						balanceDrawRequestInfo.AlipayCode = "0";
						balanceDrawRequestInfo.AlipayRealName = "0";
					}
					if (this.ValidateBalanceDrawRequest(balanceDrawRequestInfo))
					{
						this.Page.Response.Redirect($"/User/RequestBalanceDrawConfirm.aspx?bankName={Globals.UrlEncode(Globals.HtmlEncode(balanceDrawRequestInfo.BankName))}&accountName={Globals.UrlEncode(Globals.HtmlEncode(balanceDrawRequestInfo.AccountName))}&merchantCode={Globals.UrlEncode(Globals.HtmlEncode(balanceDrawRequestInfo.MerchantCode))}&amount={balanceDrawRequestInfo.Amount}&remark={Globals.UrlEncode(Globals.HtmlEncode(balanceDrawRequestInfo.Remark))}&isalipay={balanceDrawRequestInfo.IsAlipay}&isweixin={balanceDrawRequestInfo.IsWeixin}&alipaycode={Globals.UrlEncode(Globals.HtmlEncode(balanceDrawRequestInfo.AlipayCode))}&alipayrealname={Globals.UrlEncode(Globals.HtmlEncode(balanceDrawRequestInfo.AlipayRealName))}");
					}
				}
			}
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
