using Hidistro.Context;
using Hidistro.Entities.Members;
using Hidistro.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class AppMyAccountSummary : AppshopMemberTemplatedWebControl
	{
		private FormatedMoneyLabel ableBalance;

		private FormatedMoneyLabel accountAmount;

		private FormatedMoneyLabel requestBalance;

		private HyperLink accountSummaryDetails;

		private HtmlAnchor btnWithdrawals;

		private HtmlGenericControl divmoney;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-MyAccountSummary.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			PageTitle.AddSiteNameTitle("预付款账户");
			this.ableBalance = (this.FindControl("ableBalance") as FormatedMoneyLabel);
			this.accountAmount = (this.FindControl("accountAmount") as FormatedMoneyLabel);
			this.requestBalance = (this.FindControl("requestBalance") as FormatedMoneyLabel);
			this.accountSummaryDetails = (this.FindControl("accountSummaryDetails") as HyperLink);
			this.btnWithdrawals = (HtmlAnchor)this.FindControl("btnWithdrawals");
			this.divmoney = (HtmlGenericControl)this.FindControl("divmoney");
			if (!this.Page.IsPostBack)
			{
				MemberInfo user = HiContext.Current.User;
				if (user != null)
				{
					this.accountSummaryDetails.NavigateUrl = "/appshop/AccountSummaryDetails.aspx";
					if (string.IsNullOrWhiteSpace(user.TradePassword))
					{
						this.Page.Response.Redirect($"/appshop/OpenBalance.aspx?ReturnUrl={HttpContext.Current.Request.Url}");
					}
					if (this.accountAmount != null)
					{
						this.accountAmount.Money = user.Balance;
					}
					if (this.ableBalance != null)
					{
						this.ableBalance.Money = user.Balance - user.RequestBalance;
					}
					if (this.requestBalance != null)
					{
						this.requestBalance.Money = user.RequestBalance;
					}
					HtmlAnchor htmlAnchor = this.btnWithdrawals;
					HtmlGenericControl htmlGenericControl = this.divmoney;
					bool visible = htmlGenericControl.Visible = HiContext.Current.SiteSettings.EnableBulkPaymentAdvance;
					htmlAnchor.Visible = visible;
				}
			}
		}
	}
}
