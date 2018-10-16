using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Members;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class AppReferral : AppshopMemberTemplatedWebControl
	{
		private Literal litAllSplittin;

		private Literal litUserSplittin;

		private Literal litNoUserSplittin;

		private Literal litMonthLowerNum;

		private Literal litLowerNum;

		private Literal litDrawSplittin;

		private AppshopTemplatedRepeater rptAccountSummary;

		private HtmlInputHidden pageTotal;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-Referral.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			PageTitle.AddSiteNameTitle("分销员");
			MemberInfo user = Users.GetUser(HiContext.Current.UserId);
			if (user != null)
			{
				if (!user.IsReferral())
				{
					this.Page.Response.Redirect("ReferralRegisterAgreement.aspx");
				}
				this.litAllSplittin = (Literal)this.FindControl("litAllSplittin");
				this.litUserSplittin = (Literal)this.FindControl("litUserSplittin");
				this.litNoUserSplittin = (Literal)this.FindControl("litNoUserSplittin");
				this.litMonthLowerNum = (Literal)this.FindControl("litMonthLowerNum");
				this.litLowerNum = (Literal)this.FindControl("litLowerNum");
				this.rptAccountSummary = (AppshopTemplatedRepeater)this.FindControl("rptAccountSummary");
				this.pageTotal = (this.FindControl("pageTotal") as HtmlInputHidden);
				this.litDrawSplittin = (Literal)this.FindControl("litDrawSplittin");
				int userId = HiContext.Current.User.UserId;
				this.litAllSplittin.Text = MemberProcessor.GetUserAllSplittin(userId).F2ToString("f2");
				this.litUserSplittin.Text = MemberProcessor.GetUserUseSplittin(userId).F2ToString("f2");
				this.litNoUserSplittin.Text = MemberProcessor.GetUserNoUseSplittin(userId).F2ToString("f2");
				this.litDrawSplittin.Text = MemberProcessor.GetUserDrawSplittin().F2ToString("f2");
				BalanceDetailQuery balanceDetailQuery = new BalanceDetailQuery();
				balanceDetailQuery.UserId = userId;
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
				balanceDetailQuery.PageIndex = pageIndex;
				balanceDetailQuery.PageSize = pageSize;
				DbQueryResult splittinDetails = MemberHelper.GetSplittinDetails(balanceDetailQuery);
				this.rptAccountSummary.DataSource = splittinDetails.Data;
				this.rptAccountSummary.DataBind();
				if (this.pageTotal != null)
				{
					this.pageTotal.Value = splittinDetails.TotalRecords.ToString();
				}
			}
		}
	}
}
