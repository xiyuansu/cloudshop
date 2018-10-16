using Hidistro.Context;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Promotions;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class WAPShakeGetCoupon : WAPMemberTemplatedWebControl
	{
		private Literal litPrompt;

		private HyperLink buy;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-ShakeGetCoupon.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			PageTitle.AddSiteNameTitle("摇一摇优惠券领取");
			this.litPrompt = (Literal)this.FindControl("litPrompt");
			this.buy = (HyperLink)this.FindControl("Buy");
			this.buy.Visible = false;
			if (!this.Page.IsPostBack)
			{
				CouponInfo shakeCoupon = PromoteHelper.GetShakeCoupon();
				this.litPrompt.Text = shakeCoupon.CouponName;
				this.SendSendCouponToUsers(shakeCoupon.CouponId);
			}
		}

		private void SendSendCouponToUsers(int couponId)
		{
			MemberInfo user = HiContext.Current.User;
			if (CouponHelper.AddCouponItemInfo(user, couponId) == CouponActionStatus.Success)
			{
				this.litPrompt.Text = "优惠券领取成功!";
				this.buy.Visible = true;
				this.buy.NavigateUrl = "Default.aspx";
			}
		}
	}
}
