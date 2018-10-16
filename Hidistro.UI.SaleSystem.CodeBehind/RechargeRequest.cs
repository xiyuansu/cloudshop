using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Promotions;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class RechargeRequest : MemberTemplatedWebControl
	{
		private Literal litUserName;

		private RadioButtonList rbtnPaymentMode;

		private FormatedMoneyLabel litUseableBalance;

		private TextBox txtReChargeBalance;

		private IButton btnReCharge;

		private FormatedMoneyLabel litAccountAmount;

		private FormatedMoneyLabel litRequestBalance;

		private FormatedMoneyLabel litUseableBalance1;

		private HtmlGenericControl spaccountamount;

		private HtmlGenericControl sprequestbalace;

		private HtmlGenericControl spadvancetip;

		private HtmlGenericControl divReCharge;

		private HtmlGenericControl divReChargeGift;

		private HtmlAnchor link_balancedraw;

		private ThemedTemplatedRepeater rptReChargeGift;

		private HtmlInputHidden hidRechargeMoney;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-RechargeRequest.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.litUserName = (Literal)this.FindControl("litUserName");
			this.rbtnPaymentMode = (RadioButtonList)this.FindControl("rbtnPaymentMode");
			this.txtReChargeBalance = (TextBox)this.FindControl("txtReChargeBalance");
			this.btnReCharge = ButtonManager.Create(this.FindControl("btnReCharge"));
			this.litUseableBalance = (FormatedMoneyLabel)this.FindControl("litUseableBalance");
			this.litAccountAmount = (FormatedMoneyLabel)this.FindControl("litAccountAmount");
			this.litRequestBalance = (FormatedMoneyLabel)this.FindControl("litRequestBalance");
			this.litUseableBalance1 = (FormatedMoneyLabel)this.FindControl("litUseableBalance1");
			this.spaccountamount = (HtmlGenericControl)this.FindControl("spaccountamount");
			this.sprequestbalace = (HtmlGenericControl)this.FindControl("sprequestbalace");
			this.link_balancedraw = (HtmlAnchor)this.FindControl("link_balancedraw");
			this.spadvancetip = (HtmlGenericControl)this.FindControl("spadvancetip");
			this.divReCharge = (HtmlGenericControl)this.FindControl("divReCharge");
			this.divReChargeGift = (HtmlGenericControl)this.FindControl("divReChargeGift");
			this.rptReChargeGift = (ThemedTemplatedRepeater)this.FindControl("rptReChargeGift");
			this.hidRechargeMoney = (HtmlInputHidden)this.FindControl("hidRechargeMoney");
			this.rbtnPaymentMode.RepeatLayout = RepeatLayout.Table;
			this.rbtnPaymentMode.RepeatColumns = 6;
			PageTitle.AddSiteNameTitle("预付款充值");
			this.btnReCharge.Click += this.btnReCharge_Click;
			if (!this.Page.IsPostBack)
			{
				MemberInfo user = HiContext.Current.User;
				if (!user.IsOpenBalance || string.IsNullOrWhiteSpace(user.TradePassword))
				{
					this.Page.Response.Redirect($"/user/OpenBalance.aspx?ReturnUrl={HttpContext.Current.Request.Url}");
				}
				this.BindPaymentMode();
				this.litUserName.Text = HiContext.Current.User.UserName;
				this.litUseableBalance.Money = user.Balance - user.RequestBalance;
				this.litAccountAmount.Money = user.Balance;
				this.litRequestBalance.Money = user.RequestBalance;
				this.litUseableBalance1.Money = user.Balance - user.RequestBalance;
				HtmlGenericControl htmlGenericControl = this.spaccountamount;
				HtmlGenericControl htmlGenericControl2 = this.sprequestbalace;
				HtmlAnchor htmlAnchor = this.link_balancedraw;
				bool flag = htmlAnchor.Visible = HiContext.Current.SiteSettings.EnableBulkPaymentAdvance;
				bool visible = htmlGenericControl2.Visible = flag;
				htmlGenericControl.Visible = visible;
				if (HiContext.Current.SiteSettings.IsOpenRechargeGift)
				{
					List<RechargeGiftInfo> rechargeGiftItemList = PromoteHelper.GetRechargeGiftItemList();
					this.rptReChargeGift.DataSource = rechargeGiftItemList;
					this.rptReChargeGift.DataBind();
					this.divReCharge.Visible = false;
					this.divReChargeGift.Visible = true;
					this.spadvancetip.Visible = true;
				}
				else
				{
					this.divReCharge.Visible = true;
					this.divReChargeGift.Visible = false;
					this.spadvancetip.Visible = !HiContext.Current.SiteSettings.EnableBulkPaymentAdvance;
				}
			}
		}

		protected void btnReCharge_Click(object sender, EventArgs e)
		{
			try
			{
				if (this.rbtnPaymentMode.Items.Count == 0)
				{
					this.ShowMessage("无法充值,因为后台没有添加支付方式", false, "", 1);
				}
				else if (this.rbtnPaymentMode.SelectedValue == null)
				{
					this.ShowMessage("选择要充值使用的支付方式", false, "", 1);
				}
				else
				{
					decimal num = default(decimal);
					if (HiContext.Current.SiteSettings.IsOpenRechargeGift)
					{
						if (!decimal.TryParse(this.hidRechargeMoney.Value, out num) || decimal.Parse(this.hidRechargeMoney.Value) <= decimal.Zero)
						{
							this.ShowMessage("请选择充值金额", false, "", 1);
							goto end_IL_0001;
						}
					}
					else
					{
						int num2 = 0;
						if (this.txtReChargeBalance.Text.Trim().IndexOf(".") > 0)
						{
							num2 = this.txtReChargeBalance.Text.Trim().Substring(this.txtReChargeBalance.Text.Trim().IndexOf(".") + 1).Length;
						}
						if (!decimal.TryParse(this.txtReChargeBalance.Text, out num) || decimal.Parse(this.txtReChargeBalance.Text) <= decimal.Zero || num2 > 2)
						{
							this.ShowMessage("请输入大于0的充值金额且金额整数位数在1到10之间,且不能超过2位小数", false, "", 1);
							goto end_IL_0001;
						}
					}
					this.Page.Response.Redirect($"/user/RechargeConfirm?modeId={this.Page.Server.UrlEncode(this.rbtnPaymentMode.SelectedValue)}&blance={num}");
				}
				end_IL_0001:;
			}
			catch (Exception ex)
			{
				Globals.WriteExceptionLog(ex, null, "RechargeConfirm");
			}
		}

		private void BindPaymentMode()
		{
			IList<PaymentModeInfo> paymentModes = TradeHelper.GetPaymentModes(PayApplicationType.payOnAll);
			if (paymentModes.Count > 0)
			{
				foreach (PaymentModeInfo item in paymentModes)
				{
					string text = item.Gateway.ToLower();
					if (item.IsUseInpour && !text.Equals("hishop.plugins.payment.advancerequest") && !text.Equals("hishop.plugins.payment.bankrequest"))
					{
						int modeId;
						if (text.Equals("hishop.plugins.payment.alipay_shortcut.shortcutrequest"))
						{
							HttpCookie httpCookie = HiContext.Current.Context.Request.Cookies["Token_" + HiContext.Current.UserId.ToString()];
							if (httpCookie != null && !string.IsNullOrEmpty(httpCookie.Value))
							{
								ListItemCollection items = this.rbtnPaymentMode.Items;
								string name = item.Name;
								modeId = item.ModeId;
								items.Add(new ListItem(name, modeId.ToString()));
							}
						}
						else
						{
							ListItemCollection items2 = this.rbtnPaymentMode.Items;
							string name2 = item.Name;
							modeId = item.ModeId;
							items2.Add(new ListItem(name2, modeId.ToString()));
						}
					}
				}
				this.rbtnPaymentMode.SelectedIndex = 0;
			}
		}
	}
}
