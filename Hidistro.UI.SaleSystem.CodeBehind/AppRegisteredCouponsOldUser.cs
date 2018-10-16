using Hidistro.Context;
using Hidistro.Entities.Members;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class AppRegisteredCouponsOldUser : AppshopMemberTemplatedWebControl
	{
		private Literal litUserName;

		private Image userPicture;

		private HtmlAnchor linkInvitation;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-registeredcouponsolduser.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			PageTitle.AddSiteNameTitle("注册送券老用户");
			MemberInfo user = HiContext.Current.User;
			this.userPicture = (Image)this.FindControl("userPicture");
			this.litUserName = (Literal)this.FindControl("litUserName");
			this.linkInvitation = (HtmlAnchor)this.FindControl("linkInvitation");
			if (!string.IsNullOrEmpty(user.Picture))
			{
				this.userPicture.ImageUrl = user.Picture;
			}
			this.litUserName.Text = (string.IsNullOrEmpty(user.RealName) ? user.UserName : user.RealName);
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (masterSettings.OpenReferral != 1)
			{
				this.linkInvitation.Visible = false;
			}
			else if (user.Referral == null)
			{
				this.linkInvitation.HRef = "ReferralRegisterAgreement.aspx";
			}
			else if (user.Referral.ReferralStatus == 1 || user.Referral.ReferralStatus == 3)
			{
				this.linkInvitation.HRef = "ReferralRegisterresults.aspx";
			}
			else
			{
				this.linkInvitation.HRef = "SplittinRule.aspx";
			}
		}
	}
}
