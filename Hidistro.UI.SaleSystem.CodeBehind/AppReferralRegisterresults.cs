using Hidistro.Context;
using Hidistro.Entities.Members;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class AppReferralRegisterresults : AppshopMemberTemplatedWebControl
	{
		private Literal litReferralRegisterresults;

		private Literal litRefuseReasons;

		private HtmlGenericControl divWaitReview;

		private HtmlGenericControl divRefuseReasons;

		private HtmlGenericControl divRefuseReasonsText;

		private HtmlGenericControl divBtnToRegister;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-ReferralRegisterresults.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.litReferralRegisterresults = (Literal)this.FindControl("litReferralRegisterresults");
			this.litRefuseReasons = (Literal)this.FindControl("litRefuseReasons");
			this.divWaitReview = (HtmlGenericControl)this.FindControl("divWaitReview");
			this.divRefuseReasons = (HtmlGenericControl)this.FindControl("divRefuseReasons");
			this.divRefuseReasonsText = (HtmlGenericControl)this.FindControl("divRefuseReasonsText");
			this.divBtnToRegister = (HtmlGenericControl)this.FindControl("divBtnToRegister");
			PageTitle.AddSiteNameTitle("我要成为分销员");
			ReferralInfo referralInfo = Users.GetReferralInfo(HiContext.Current.UserId);
			if (referralInfo != null)
			{
				if (referralInfo.ReferralStatus == 2)
				{
					this.Page.Response.Redirect("/appshop/Referral.aspx");
				}
				else if (referralInfo.ReferralStatus == 1)
				{
					this.litReferralRegisterresults.Text = "您提交的申请正在审核中...";
					if (this.divRefuseReasons != null)
					{
						HtmlGenericControl htmlGenericControl = this.divRefuseReasons;
						HtmlGenericControl htmlGenericControl2 = this.divBtnToRegister;
						bool visible = htmlGenericControl2.Visible = false;
						htmlGenericControl.Visible = visible;
					}
				}
				else if (referralInfo.ReferralStatus == 3)
				{
					this.divWaitReview.Visible = false;
					this.litReferralRegisterresults.Text = "您提交的申请被拒绝了...";
					if (string.IsNullOrEmpty(referralInfo.RefusalReason.Trim()))
					{
						this.divRefuseReasonsText.Visible = false;
					}
					else
					{
						this.litRefuseReasons.Text = "拒绝理由：" + referralInfo.RefusalReason;
					}
				}
			}
			else
			{
				this.Page.Response.Redirect("/appshop/ReferralRegisterAgreement.aspx");
			}
		}
	}
}
