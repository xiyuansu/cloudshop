using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Member;
using Hidistro.SqlDal.Members;
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
	public class SplittinDraws : MemberTemplatedWebControl
	{
		private Literal lblminDraws;

		private Literal lblBanlance;

		private TextBox txtAmount;

		private TextBox txtTradePassword;

		private IButton btnDraw;

		private Common_Referral_SplittinDraw rptSplittinDraw;

		private TextBox txtBankName;

		private TextBox txtAccountName;

		private TextBox txtMerchantCode;

		private TextBox txtAlipayRealName;

		private TextBox txtAlipayCode;

		private TextBox txtRemark;

		private RadioButton IsWithdrawToAccount;

		private RadioButton IsDefault;

		private RadioButton IsWeixin;

		private RadioButton IsAlipay;

		private Pager pager;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-SplittinDraws.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.lblminDraws = (Literal)this.FindControl("lblminDraws");
			this.lblBanlance = (Literal)this.FindControl("lblBanlance");
			this.txtAmount = (TextBox)this.FindControl("txtAmount");
			this.txtTradePassword = (TextBox)this.FindControl("txtTradePassword");
			this.btnDraw = ButtonManager.Create(this.FindControl("btnDraw"));
			this.txtBankName = (TextBox)this.FindControl("txtBankName");
			this.txtAccountName = (TextBox)this.FindControl("txtAccountName");
			this.txtMerchantCode = (TextBox)this.FindControl("txtMerchantCode");
			this.txtAlipayRealName = (TextBox)this.FindControl("txtAlipayRealName");
			this.txtAlipayCode = (TextBox)this.FindControl("txtAlipayCode");
			this.txtRemark = (TextBox)this.FindControl("txtRemark");
			this.IsWithdrawToAccount = (RadioButton)this.FindControl("IsWithdrawToAccount");
			this.IsDefault = (RadioButton)this.FindControl("IsDefault");
			this.IsWeixin = (RadioButton)this.FindControl("IsWeixin");
			this.IsAlipay = (RadioButton)this.FindControl("IsAlipay");
			this.rptSplittinDraw = (Common_Referral_SplittinDraw)this.FindControl("Common_Referral_Splitin");
			this.pager = (Pager)this.FindControl("pager");
			this.btnDraw.Click += this.btnDraw_Click;
			PageTitle.AddSiteNameTitle("申请提现");
			if (!this.Page.IsPostBack)
			{
				MemberInfo user = HiContext.Current.User;
				if (!user.IsOpenBalance || string.IsNullOrEmpty(user.TradePassword))
				{
					this.Page.Response.Redirect($"/user/OpenBalance?ReturnUrl={HttpContext.Current.Request.Url}");
				}
				BalanceDrawRequestQuery balanceDrawRequestQuery = new BalanceDrawRequestQuery();
				balanceDrawRequestQuery.PageIndex = 1;
				balanceDrawRequestQuery.PageSize = this.pager.PageSize;
				balanceDrawRequestQuery.UserId = HiContext.Current.UserId;
				DbQueryResult mySplittinDraws = MemberProcessor.GetMySplittinDraws(balanceDrawRequestQuery, 1);
				if (mySplittinDraws.TotalRecords > 0)
				{
					this.ShowMessage("提现申请成功，等待管理员审核", true, "", 1);
					this.btnDraw.Visible = false;
					this.BindSplittinDraw();
				}
				else
				{
					this.lblBanlance.Text = MemberProcessor.GetUserUseSplittin(HiContext.Current.UserId).F2ToString("f2");
					SiteSettings masterSettings = SettingsManager.GetMasterSettings();
					this.lblminDraws.Text = masterSettings.MinimumSingleShot.F2ToString("f2");
					if (!masterSettings.EnableBulkPaymentAliPay || !masterSettings.SplittinDraws_CashToALiPay)
					{
						this.IsAlipay.Visible = false;
					}
					if (!masterSettings.EnableBulkPaymentWeixin || !masterSettings.SplittinDraws_CashToWeiXin)
					{
						this.IsWeixin.Visible = false;
					}
					if (!masterSettings.SplittinDraws_CashToDeposit)
					{
						this.IsWithdrawToAccount.Visible = false;
					}
					if (!masterSettings.SplittinDraws_CashToBankCard)
					{
						this.IsDefault.Visible = false;
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
					this.IsWithdrawToAccount.Checked = true;
					this.BindSplittinDraw();
				}
			}
		}

		private void btnDraw_Click(object sender, EventArgs e)
		{
			BalanceDrawRequestQuery balanceDrawRequestQuery = new BalanceDrawRequestQuery();
			balanceDrawRequestQuery.PageIndex = 1;
			balanceDrawRequestQuery.PageSize = this.pager.PageSize;
			balanceDrawRequestQuery.UserId = HiContext.Current.UserId;
			decimal num = default(decimal);
			num = MemberProcessor.GetUserUseSplittin(HiContext.Current.UserId);
			DbQueryResult mySplittinDraws = MemberProcessor.GetMySplittinDraws(balanceDrawRequestQuery, 1);
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (mySplittinDraws.TotalRecords > 0)
			{
				this.ShowMessage("上笔提现管理员还没有处理，只有处理完后才能再次申请提现", false, "", 1);
			}
			else if (!this.IsDefault.Visible && !this.IsWithdrawToAccount.Visible && !this.IsWeixin.Visible && !this.IsAlipay.Visible)
			{
				this.ShowMessage("没有合适的提现方式，请与管理员联系", false, "", 1);
			}
			else if (!this.IsDefault.Checked && !this.IsWithdrawToAccount.Checked && !this.IsWeixin.Checked && !this.IsAlipay.Checked)
			{
				this.ShowMessage("请选择提现方式", false, "", 1);
			}
			else
			{
				decimal num2 = default(decimal);
				if (!decimal.TryParse(this.txtAmount.Text.Trim(), out num2))
				{
					this.ShowMessage("提现金额输入错误,请重新输入提现金额", false, "", 1);
				}
				else if (num2 < masterSettings.MinimumSingleShot)
				{
					this.ShowMessage("每次提现金额必须多于" + this.lblminDraws.Text + "元", false, "", 1);
				}
				else if (num2 > num)
				{
					this.ShowMessage("可提现奖励不足,请重新输入提现金额", false, "", 1);
				}
				else if (string.IsNullOrEmpty(this.txtTradePassword.Text))
				{
					this.ShowMessage("请输入交易密码", false, "", 1);
				}
				else if (!MemberProcessor.ValidTradePassword(this.txtTradePassword.Text))
				{
					this.ShowMessage("交易密码不正确,请重新输入", false, "", 1);
				}
				else
				{
					MemberInfo user = HiContext.Current.User;
					if (!user.IsOpenBalance)
					{
						this.ShowMessage("请先开通预付款账户", false, "", 1);
					}
					else
					{
						SplittinDrawInfo splittinDrawInfo = new SplittinDrawInfo();
						splittinDrawInfo.UserId = user.UserId;
						splittinDrawInfo.UserName = user.UserName;
						splittinDrawInfo.Amount = num2;
						splittinDrawInfo.RequestDate = DateTime.Now;
						splittinDrawInfo.BankName = this.txtBankName.Text.Trim();
						splittinDrawInfo.AccountName = this.txtAccountName.Text.Trim();
						splittinDrawInfo.MerchantCode = this.txtMerchantCode.Text.Trim();
						splittinDrawInfo.AlipayCode = this.txtAlipayCode.Text.Trim();
						splittinDrawInfo.AlipayRealName = this.txtAlipayRealName.Text;
						splittinDrawInfo.Remark = this.txtRemark.Text.Trim();
						splittinDrawInfo.ManagerRemark = "";
						splittinDrawInfo.IsWithdrawToAccount = this.IsWithdrawToAccount.Checked;
						splittinDrawInfo.IsAlipay = this.IsAlipay.Checked;
						splittinDrawInfo.IsWeixin = this.IsWeixin.Checked;
						if (splittinDrawInfo.IsWithdrawToAccount)
						{
							splittinDrawInfo.AuditStatus = 2;
							splittinDrawInfo.AccountDate = DateTime.Now;
							splittinDrawInfo.ManagerRemark = "提现至预付款账户" + (string.IsNullOrEmpty(splittinDrawInfo.Remark.ToNullString()) ? "" : ("（买家备注：" + splittinDrawInfo.Remark.ToNullString() + "）"));
						}
						else
						{
							splittinDrawInfo.AuditStatus = 1;
						}
						OnLinePayment onLinePayment;
						if (this.IsAlipay.Checked)
						{
							splittinDrawInfo.BankName = "0";
							splittinDrawInfo.AccountName = "0";
							splittinDrawInfo.MerchantCode = "0";
							SplittinDrawInfo splittinDrawInfo2 = splittinDrawInfo;
							onLinePayment = OnLinePayment.NoPay;
							splittinDrawInfo2.RequestState = onLinePayment.GetHashCode().ToNullString();
						}
						else if (this.IsWeixin.Checked || splittinDrawInfo.IsWithdrawToAccount)
						{
							splittinDrawInfo.BankName = "0";
							splittinDrawInfo.AccountName = "0";
							splittinDrawInfo.MerchantCode = "0";
							splittinDrawInfo.AlipayCode = "0";
							splittinDrawInfo.AlipayRealName = "0";
							if (this.IsWeixin.Checked)
							{
								SplittinDrawInfo splittinDrawInfo3 = splittinDrawInfo;
								onLinePayment = OnLinePayment.NoPay;
								splittinDrawInfo3.RequestState = onLinePayment.GetHashCode().ToNullString();
							}
						}
						else
						{
							splittinDrawInfo.AlipayCode = "0";
							splittinDrawInfo.AlipayRealName = "0";
						}
						if (this.ValidateBalanceDrawRequest(splittinDrawInfo))
						{
							if (MemberProcessor.SplittinDrawRequest(splittinDrawInfo))
							{
								if (splittinDrawInfo.IsWithdrawToAccount)
								{
									ReferralDao referralDao = new ReferralDao();
									SplittinDetailInfo splittinDetailInfo = new SplittinDetailInfo();
									splittinDetailInfo.OrderId = string.Empty;
									splittinDetailInfo.UserId = splittinDrawInfo.UserId;
									splittinDetailInfo.UserName = splittinDrawInfo.UserName;
									splittinDetailInfo.IsUse = true;
									splittinDetailInfo.TradeDate = DateTime.Now;
									splittinDetailInfo.TradeType = SplittingTypes.DrawRequest;
									splittinDetailInfo.Expenses = splittinDrawInfo.Amount;
									splittinDetailInfo.Balance = referralDao.GetUserUseSplittin(splittinDrawInfo.UserId) - splittinDrawInfo.Amount;
									splittinDetailInfo.Remark = "";
									referralDao.Add(splittinDetailInfo, null);
									this.SaveBalance(splittinDrawInfo.UserId, splittinDrawInfo.Amount);
									this.ShowMessage("提现成功，申请金额已转至您的预付款账户", true, "", 1);
									Users.ClearUserCache(splittinDrawInfo.UserId, user.SessionId);
									this.BindSplittinDraw();
								}
								else
								{
									this.btnDraw.Visible = false;
									this.ShowMessage("提现申请成功，等待管理员的审核", true, "", 1);
									this.BindSplittinDraw();
								}
								this.Clear();
							}
							else
							{
								this.ShowMessage("提现申请失败，请重试", false, "", 1);
							}
						}
					}
				}
			}
		}

		private void BindSplittinDraw()
		{
			BalanceDrawRequestQuery balanceDrawRequestQuery = new BalanceDrawRequestQuery();
			balanceDrawRequestQuery.PageIndex = this.pager.PageIndex;
			balanceDrawRequestQuery.PageSize = this.pager.PageSize;
			balanceDrawRequestQuery.UserId = HiContext.Current.UserId;
			DbQueryResult mySplittinDraws = MemberProcessor.GetMySplittinDraws(balanceDrawRequestQuery, null);
			this.rptSplittinDraw.DataSource = mySplittinDraws.Data;
			this.rptSplittinDraw.DataBind();
			this.pager.TotalRecords = mySplittinDraws.TotalRecords;
		}

		private bool ValidateBalanceDrawRequest(SplittinDrawInfo balanceDrawRequest)
		{
			ValidationResults validationResults = Validation.Validate(balanceDrawRequest, "ValSplittinDrawInfo");
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

		private void Clear()
		{
			this.txtAmount.Text = "";
			this.txtTradePassword.Attributes.Add("value", "");
			this.txtBankName.Text = "";
			this.txtAccountName.Text = "";
			this.txtMerchantCode.Text = "";
			this.txtAlipayRealName.Text = "";
			this.txtAlipayCode.Text = "";
			this.txtRemark.Text = "";
			this.IsWithdrawToAccount.Checked = true;
		}

		private void SaveBalance(int UserId, decimal Amount)
		{
			MemberInfo user = Users.GetUser(UserId);
			decimal balance = user.Balance + Amount;
			BalanceDetailInfo balanceDetailInfo = new BalanceDetailInfo();
			balanceDetailInfo.UserId = UserId;
			balanceDetailInfo.UserName = user.UserName;
			balanceDetailInfo.TradeDate = DateTime.Now;
			balanceDetailInfo.TradeType = TradeTypes.Commission;
			balanceDetailInfo.Income = Amount;
			balanceDetailInfo.Balance = balance;
			MemberProcessor.AddSplittinDrawBalance(balanceDetailInfo);
		}
	}
}
