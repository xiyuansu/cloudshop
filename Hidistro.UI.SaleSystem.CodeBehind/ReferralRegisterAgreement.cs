using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Member;
using Hidistro.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class ReferralRegisterAgreement : MemberTemplatedWebControl
	{
		private Literal litReferralRegisterAgreement;

		private HtmlGenericControl divRecruitmentAgreement;

		private HtmlInputHidden hidApplyCondition;

		private HtmlInputHidden hidApplyReferralNeedAmount;

		private HtmlInputHidden hidExpenditure;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-ReferralRegisterAgreement.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.divRecruitmentAgreement = (HtmlGenericControl)this.FindControl("divRecruitmentAgreement");
			this.litReferralRegisterAgreement = (Literal)this.FindControl("litReferralRegisterAgreement");
			this.hidApplyCondition = (HtmlInputHidden)this.FindControl("hidApplyCondition");
			this.hidApplyReferralNeedAmount = (HtmlInputHidden)this.FindControl("hidApplyReferralNeedAmount");
			this.hidExpenditure = (HtmlInputHidden)this.FindControl("hidExpenditure");
			MemberInfo user = HiContext.Current.User;
			this.hidExpenditure.Value = user.Expenditure.F2ToString("f2");
			this.hidApplyCondition.Value = HiContext.Current.SiteSettings.ApplyReferralCondition.ToString();
			this.hidApplyReferralNeedAmount.Value = HiContext.Current.SiteSettings.ApplyReferralNeedAmount.F2ToString("f2");
			ReferralInfo referralInfo = Users.GetReferralInfo(user.UserId);
			if (this.divRecruitmentAgreement != null && !HiContext.Current.SiteSettings.OpenRecruitmentAgreement)
			{
				this.divRecruitmentAgreement.Visible = false;
			}
			int referralUserId = user.ReferralUserId;
			int.TryParse(HttpContext.Current.Request.QueryString["ReferralUserId"], out referralUserId);
			bool flag = false;
			if (this.Page.Request.QueryString["again"] != null && this.Page.Request.QueryString["again"] == "1")
			{
				flag = true;
			}
			if (referralInfo != null)
			{
				if (referralInfo.ReferralStatus != 2 && !flag && referralUserId <= 0 && !HiContext.Current.SiteSettings.RegisterBecomePromoter)
				{
					this.Page.Response.Redirect("/user/ReferralRegisterresults.aspx");
				}
				else if (referralInfo.ReferralStatus == 2)
				{
					this.Page.Response.Redirect("/user/PopularizeGift.aspx");
				}
				else if (referralInfo.ReferralStatus == 3 && HiContext.Current.SiteSettings.RegisterBecomePromoter && MemberProcessor.ReferralRequest(HiContext.Current.UserId, "", "", 0, 0, "", "", "", ""))
				{
					Users.ClearUserCache(user.UserId, user.SessionId);
					this.Page.Response.Redirect("/user/PopularizeGift.aspx", true);
				}
			}
			else if (HiContext.Current.SiteSettings.RegisterBecomePromoter && MemberProcessor.AddRegisterReferral(user.UserId, ReferralApplyStatus.Audited))
			{
				Users.ClearUserCache(user.UserId, user.SessionId);
				this.Page.Response.Redirect("/user/PopularizeGift.aspx");
			}
			PageTitle.AddSiteNameTitle("我要成为分销员");
			this.litReferralRegisterAgreement.Text = HiContext.Current.SiteSettings.ReferralIntroduction;
		}
	}
}
