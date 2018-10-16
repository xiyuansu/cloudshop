using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WapReferralRegisterAgreement : WAPTemplatedWebControl
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
				this.SkinName = "Skin-ReferralRegisterAgreement.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.litReferralRegisterAgreement = (Literal)this.FindControl("litReferralRegisterAgreement");
			this.divRecruitmentAgreement = (HtmlGenericControl)this.FindControl("divRecruitmentAgreement");
			this.hidApplyCondition = (HtmlInputHidden)this.FindControl("hidApplyCondition");
			this.hidApplyReferralNeedAmount = (HtmlInputHidden)this.FindControl("hidApplyReferralNeedAmount");
			this.hidExpenditure = (HtmlInputHidden)this.FindControl("hidExpenditure");
			MemberInfo user = Users.GetUser();
			if (user == null || user.UserId == 0)
			{
				this.Page.Response.Redirect("MemberCenter.aspx?action=register&returnUrl=/" + HiContext.Current.GetClientPath + "/ReferralRegisterAgreement");
			}
			if (!user.IsReferral() && user.Referral != null)
			{
				this.Page.Response.Redirect("ReferralRegisterresults.aspx");
			}
			this.hidExpenditure.Value = user.Expenditure.F2ToString("f2");
			this.hidApplyCondition.Value = HiContext.Current.SiteSettings.ApplyReferralCondition.ToString();
			this.hidApplyReferralNeedAmount.Value = HiContext.Current.SiteSettings.ApplyReferralNeedAmount.F2ToString("f2");
			if (this.divRecruitmentAgreement != null && !HiContext.Current.SiteSettings.OpenRecruitmentAgreement)
			{
				this.divRecruitmentAgreement.Visible = false;
			}
			int num = 0;
			int.TryParse(HttpContext.Current.Request.QueryString["ReferralUserId"], out num);
			if (user.IsReferral() && num <= 0)
			{
				this.Page.Response.Redirect($"/{HiContext.Current.GetClientPath}/ReferralRegisterresults.aspx");
			}
			PageTitle.AddSiteNameTitle("我要成为分销员");
			this.litReferralRegisterAgreement.Text = HiContext.Current.SiteSettings.ReferralIntroduction;
		}
	}
}
