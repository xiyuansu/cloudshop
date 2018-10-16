using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class AppShopInfo : AppshopMemberTemplatedWebControl
	{
		private TextBox txtEmail;

		private TextBox txtPhone;

		private TextBox txtShopName;

		private HtmlInputHidden hidUploadImages;

		private HiddenField hdLink;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-ShopInfo.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.hdLink = (HiddenField)this.FindControl("hdLink");
			this.txtEmail = (TextBox)this.FindControl("txtEmail");
			this.txtPhone = (TextBox)this.FindControl("txtPhone");
			this.txtShopName = (TextBox)this.FindControl("txtShopName");
			this.hidUploadImages = (HtmlInputHidden)this.FindControl("hidOldImages");
			PageTitle.AddSiteNameTitle("分销员店铺信息设置");
			if (!this.Page.IsPostBack)
			{
				MemberInfo user = Users.GetUser();
				if (!user.IsReferral())
				{
					if (user.Referral != null && user.Referral.ReferralStatus != 2.GetHashCode())
					{
						this.Page.Response.Redirect($"/{HiContext.Current.GetClientPath}/ReferralRegisterresults");
					}
					else
					{
						this.Page.Response.Redirect($"/{HiContext.Current.GetClientPath}/ReferralRegisterAgreement");
					}
				}
				this.hdLink.Value = Globals.FullPath($"/{HiContext.Current.GetClientPath}/Referral");
				string fullRegion = RegionHelper.GetFullRegion(user.RegionId, " ", true, 0);
				this.txtEmail.Text = (string.IsNullOrEmpty(user.Referral.Email) ? user.Email : user.Referral.Email);
				this.txtPhone.Text = (string.IsNullOrEmpty(user.Referral.CellPhone) ? user.CellPhone : user.Referral.CellPhone);
				this.txtShopName.Text = user.Referral.ShopName;
				this.hidUploadImages.Value = user.Referral.BannerUrl;
			}
		}
	}
}
