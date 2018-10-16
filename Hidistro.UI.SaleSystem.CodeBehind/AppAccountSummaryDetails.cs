using Hidistro.Context;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Member;
using Hidistro.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class AppAccountSummaryDetails : AppshopMemberTemplatedWebControl
	{
		private AppshopTemplatedRepeater rptAccountSummary;

		private HtmlInputHidden pageTotal;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-AccountSummaryDetails.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			PageTitle.AddSiteNameTitle("预付款详细信息");
			this.rptAccountSummary = (this.FindControl("rptAccountSummary") as AppshopTemplatedRepeater);
			this.pageTotal = (this.FindControl("pageTotal") as HtmlInputHidden);
			if (!this.Page.IsPostBack)
			{
				MemberInfo user = HiContext.Current.User;
				if (!user.IsOpenBalance)
				{
					this.Page.Response.Redirect($"/appshop/OpenBalance.aspx?ReturnUrl={HttpContext.Current.Request.Url}");
				}
				if (this.rptAccountSummary != null)
				{
					this.BindBalanceDetails();
				}
			}
		}

		private void BindBalanceDetails()
		{
			BalanceDetailQuery balanceDetailQuery = this.GetBalanceDetailQuery();
			DbQueryResult balanceDetails = MemberProcessor.GetBalanceDetails(balanceDetailQuery);
			this.rptAccountSummary.DataSource = balanceDetails.Data;
			this.rptAccountSummary.DataBind();
			if (this.pageTotal != null)
			{
				this.pageTotal.Value = balanceDetails.TotalRecords.ToString();
			}
		}

		private BalanceDetailQuery GetBalanceDetailQuery()
		{
			BalanceDetailQuery balanceDetailQuery = new BalanceDetailQuery();
			int pageIndex = default(int);
			if (!int.TryParse(this.Page.Request.QueryString["page"], out pageIndex))
			{
				pageIndex = 1;
			}
			int pageSize = default(int);
			if (!int.TryParse(this.Page.Request.QueryString["size"], out pageSize))
			{
				pageSize = 20;
			}
			balanceDetailQuery.UserId = HiContext.Current.UserId;
			balanceDetailQuery.PageIndex = pageIndex;
			balanceDetailQuery.PageSize = pageSize;
			return balanceDetailQuery;
		}
	}
}
