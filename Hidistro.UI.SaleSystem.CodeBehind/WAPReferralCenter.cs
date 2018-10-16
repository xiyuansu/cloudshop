using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Member;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WAPReferralCenter : WAPMemberTemplatedWebControl
	{
		private Literal litShopName;

		private Literal litGradeName;

		private Literal litUpgradeMoney;

		private Literal litNextGradeName;

		private Literal litUserSplittin;

		private Literal litHisotrySplittin;

		private Literal litNoUserSplittin;

		private HtmlGenericControl divUpgrade;

		private HtmlGenericControl divReferralCenter;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-ReferralCenter.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			PageTitle.AddSiteNameTitle("分销中心");
			this.litShopName = (Literal)this.FindControl("litShopName");
			this.litGradeName = (Literal)this.FindControl("litGradeName");
			this.litUpgradeMoney = (Literal)this.FindControl("litUpgradeMoney");
			this.litNextGradeName = (Literal)this.FindControl("litNextGradeName");
			this.litUserSplittin = (Literal)this.FindControl("litUserSplittin");
			this.litHisotrySplittin = (Literal)this.FindControl("litHisotrySplittin");
			this.litNoUserSplittin = (Literal)this.FindControl("litNoUserSplittin");
			this.divUpgrade = (HtmlGenericControl)this.FindControl("divUpgrade");
			this.divReferralCenter = (HtmlGenericControl)this.FindControl("divReferralCenter");
			MemberInfo user = Users.GetUser(HiContext.Current.UserId);
			if (HiContext.Current.SiteSettings.OpenReferral == 1)
			{
				if (!user.IsReferral())
				{
					this.Page.Response.Redirect("ReferralRegisterAgreement");
				}
				else if (user.Referral.IsRepeled)
				{
					this.Page.Response.Redirect("SplittinRule");
				}
			}
			else
			{
				this.divReferralCenter.Visible = false;
			}
			ReferralGradeInfo nextReferralGradeInfo = MemberProcessor.GetNextReferralGradeInfo(user.Referral.GradeId);
			decimal d = MemberProcessor.GetSplittinTotal(user.UserId).F2ToString("f2").ToDecimal(0);
			this.litHisotrySplittin.Text = MemberProcessor.GetUserAllSplittin(user.UserId).F2ToString("f2");
			this.litUserSplittin.Text = MemberProcessor.GetUserUseSplittin(user.UserId).F2ToString("f2");
			this.litNoUserSplittin.Text = MemberProcessor.GetUserNoUseSplittin(user.UserId).F2ToString("f2");
			this.litShopName.Text = user.Referral.ShopName;
			this.litGradeName.Text = user.Referral.GradeName;
			if (nextReferralGradeInfo == null)
			{
				this.divUpgrade.Visible = false;
			}
			else
			{
				this.divUpgrade.Visible = true;
				this.litUpgradeMoney.Text = ((nextReferralGradeInfo.CommissionThreshold - d > decimal.Zero) ? (nextReferralGradeInfo.CommissionThreshold - d).F2ToString("f2") : "0.00");
				this.litNextGradeName.Text = nextReferralGradeInfo.Name;
			}
		}
	}
}
