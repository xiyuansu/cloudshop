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
	public class AppReferralRegister : AppshopMemberTemplatedWebControl
	{
		private TextBox txtRealName;

		private TextBox txtAddress;

		private TextBox txtEmail;

		private TextBox txtPhone;

		private TextBox txtShopName;

		private HtmlInputHidden regionText;

		private HtmlInputHidden region;

		private HtmlInputHidden hidOldBanner;

		private HtmlInputHidden hidValidateRealName;

		private HtmlInputHidden hidValidatePhone;

		private HtmlInputHidden hidValidateEmail;

		private HtmlInputHidden hidValidateAddress;

		private HtmlInputHidden hidIsPromoterValidatePhone;

		private HtmlInputHidden hdRegisterBecomePromoter;

		private HiddenField hdLink;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-ReferralRegister.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.txtRealName = (TextBox)this.FindControl("txtRealName");
			this.txtAddress = (TextBox)this.FindControl("txtAddress");
			this.regionText = (HtmlInputHidden)this.FindControl("regionText");
			this.region = (HtmlInputHidden)this.FindControl("region");
			this.hdLink = (HiddenField)this.FindControl("hdLink");
			this.txtEmail = (TextBox)this.FindControl("txtEmail");
			this.txtPhone = (TextBox)this.FindControl("txtPhone");
			this.hidValidateRealName = (HtmlInputHidden)this.FindControl("hidValidateRealName");
			this.hidValidatePhone = (HtmlInputHidden)this.FindControl("hidValidatePhone");
			this.hidValidateEmail = (HtmlInputHidden)this.FindControl("hidValidateEmail");
			this.hidValidateAddress = (HtmlInputHidden)this.FindControl("hidValidateAddress");
			this.hidIsPromoterValidatePhone = (HtmlInputHidden)this.FindControl("hidIsPromoterValidatePhone");
			this.hdRegisterBecomePromoter = (HtmlInputHidden)this.FindControl("hdRegisterBecomePromoter");
			this.hidOldBanner = (HtmlInputHidden)this.FindControl("hidOldBanner");
			this.txtShopName = (TextBox)this.FindControl("txtShopName");
			PageTitle.AddSiteNameTitle("分销员申请表单");
			if (!this.Page.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				if (masterSettings.RegisterBecomePromoter)
				{
					MemberInfo user = HiContext.Current.User;
					MemberProcessor.ReferralRequest(HiContext.Current.UserId, user.RealName, user.CellPhone, user.TopRegionId, user.RegionId, user.Address, user.Email, "", "");
					this.hdRegisterBecomePromoter.Value = "/appshop/SplittinRule.aspx";
					Users.ClearUserCache(HiContext.Current.UserId, "");
				}
				else
				{
					string promoterNeedInfo = masterSettings.PromoterNeedInfo;
					MemberInfo user2 = Users.GetUser();
					ReferralInfo referralInfo = Users.GetReferralInfo(HiContext.Current.UserId);
					if (referralInfo != null && referralInfo.ReferralStatus != 0 && this.Page.Request.QueryString["again"] != "1")
					{
						this.Page.Response.Redirect($"/{HiContext.Current.GetClientPath}/ReferralRegisterresults.aspx");
					}
					ReferralExtInfo referralExtInfo = new ReferralExtInfo();
					if (referralInfo != null)
					{
						this.txtShopName.Text = referralInfo.ShopName;
						this.hidOldBanner.Value = referralInfo.BannerUrl;
						referralExtInfo = MemberProcessor.GetReferralExtInfo(referralInfo.RequetReason);
					}
					this.hdLink.Value = Globals.FullPath("/appshop/ReferralRegisterresults.aspx");
					string fullRegion = RegionHelper.GetFullRegion((referralExtInfo.RegionId > 0) ? referralExtInfo.RegionId : user2.RegionId, " ", true, 0);
					this.txtAddress.Text = (string.IsNullOrEmpty(referralExtInfo.Address) ? user2.Address : referralExtInfo.Address);
					this.txtRealName.Text = (string.IsNullOrEmpty(referralExtInfo.RealName) ? user2.RealName : referralExtInfo.RealName);
					this.txtEmail.Text = (string.IsNullOrEmpty(referralExtInfo.Email) ? user2.Email : referralExtInfo.Email);
					this.txtPhone.Text = (string.IsNullOrEmpty(referralExtInfo.CellPhone) ? user2.CellPhone : referralExtInfo.CellPhone);
					this.hidValidateRealName.Value = (promoterNeedInfo.Contains("1") ? "1" : "");
					this.hidValidatePhone.Value = (promoterNeedInfo.Contains("2") ? "1" : "");
					this.hidValidateEmail.Value = (promoterNeedInfo.Contains("3") ? "1" : "");
					this.hidValidateAddress.Value = (promoterNeedInfo.Contains("4") ? "1" : "");
					this.hidIsPromoterValidatePhone.Value = (masterSettings.IsPromoterValidatePhone ? "1" : "");
					this.regionText.SetWhenIsNotNull(fullRegion);
					this.region.SetWhenIsNotNull(((referralExtInfo.RegionId > 0) ? referralExtInfo.RegionId : user2.RegionId).ToString());
				}
			}
		}
	}
}
