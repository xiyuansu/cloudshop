using Hidistro.Context;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class WAPMyAccountSummary : WAPMemberTemplatedWebControl
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
				if (base.ClientType == ClientType.WAP)
				{
					this.accountSummaryDetails.NavigateUrl = "/Wapshop/AccountSummaryDetails.aspx";
				}
				else if (base.ClientType == ClientType.VShop)
				{
					this.accountSummaryDetails.NavigateUrl = "/Vshop/AccountSummaryDetails.aspx";
				}
				else if (base.ClientType == ClientType.AliOH)
				{
					this.accountSummaryDetails.NavigateUrl = "/AliOH/AccountSummaryDetails.aspx";
				}
				MemberInfo user = HiContext.Current.User;
				if (string.IsNullOrWhiteSpace(user.TradePassword))
				{
					this.Page.Response.Redirect(string.Format("/{1}/OpenBalance.aspx?ReturnUrl={0}", HttpContext.Current.Request.Url, HiContext.Current.GetClientPath));
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
