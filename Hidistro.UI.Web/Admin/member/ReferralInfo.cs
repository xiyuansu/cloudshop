using Hidistro.Core;
using Hidistro.Core.Urls;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Members;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.member
{
	public class ReferralInfo : AdminPage
	{
		protected Literal litRefferralInfo;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!base.IsPostBack)
			{
				string parameter = RouteConfig.GetParameter(this, "UserId", false);
				Hidistro.Entities.Members.ReferralInfo referral = MemberHelper.GetReferral(parameter.ToInt(0));
				string referralExtShowInfo = MemberProcessor.GetReferralExtShowInfo(referral.RequetReason);
				this.litRefferralInfo.Text = (string.IsNullOrEmpty(referralExtShowInfo) ? "该分销员没有提交任何信息" : referralExtShowInfo);
			}
		}
	}
}
