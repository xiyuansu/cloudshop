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
	public class ReferralRegisterresults : MemberTemplatedWebControl
	{
		private Literal litReferralRegisterresults;

		private Literal litRefuseReason;

		private HtmlInputHidden hidIsTrueOrFalse;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-ReferralRegisterresults.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.litReferralRegisterresults = (Literal)this.FindControl("litReferralRegisterresults");
			this.litRefuseReason = (Literal)this.FindControl("litRefuseReason");
			this.hidIsTrueOrFalse = (HtmlInputHidden)this.FindControl("hidIsTrueOrFalse");
			PageTitle.AddSiteNameTitle("我要成为分销员");
			ReferralInfo referralInfo = Users.GetReferralInfo(HiContext.Current.UserId);
			if (referralInfo != null)
			{
				if (referralInfo.ReferralStatus == 2)
				{
					this.Page.Response.Redirect("/user/PopularizeGift.aspx");
				}
				else if (referralInfo.ReferralStatus == 1)
				{
					this.litReferralRegisterresults.Text = "您提交的申请正在审核中，请耐心等待审核结果。";
					this.hidIsTrueOrFalse.Value = "1";
				}
				else if (referralInfo.ReferralStatus == 3)
				{
					this.litReferralRegisterresults.Text = "";
					if (!string.IsNullOrEmpty(referralInfo.RefusalReason))
					{
						this.litRefuseReason.Text = "理由：" + referralInfo.RefusalReason;
					}
					this.hidIsTrueOrFalse.Value = "0";
				}
			}
			else
			{
				this.Page.Response.Redirect("/user/ReferralRegisterAgreement.aspx");
			}
		}
	}
}
