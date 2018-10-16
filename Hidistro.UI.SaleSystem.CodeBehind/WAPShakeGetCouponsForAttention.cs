using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Promotions;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class WAPShakeGetCouponsForAttention : WAPMemberTemplatedWebControl
	{
		private Literal litPrompt;

		private HyperLink buy;

		private int CouponID = 0;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-ShakeGetCouponsForAttention.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			PageTitle.AddSiteNameTitle("关注公众号领取优惠券");
			this.litPrompt = (Literal)this.FindControl("litPrompt");
			this.buy = (HyperLink)this.FindControl("Buy");
			this.CouponID = this.Context.Request["cid"].ToInt(0);
			if (!this.Page.IsPostBack)
			{
				CouponInfo coupon = CouponHelper.GetCoupon(this.CouponID);
				this.litPrompt.Text = coupon.CouponName;
				this.SendSendCouponToUsers(coupon.CouponId);
			}
		}

		private void SendSendCouponToUsers(int couponId)
		{
			MemberInfo user = HiContext.Current.User;
			if (HiContext.Current.User != null && !HiContext.Current.User.IsSubscribe)
			{
				this.litPrompt.Text = "您尚未关注公众号!";
				this.buy.NavigateUrl = "Default.aspx";
				this.buy.Text = "返回商城首页";
			}
			else
			{
				CouponActionStatus couponActionStatus = CouponHelper.AddCouponItemInfo(user, couponId);
				if (couponActionStatus == CouponActionStatus.Success)
				{
					this.litPrompt.Text = "优惠券领取成功!";
					this.buy.NavigateUrl = "Default.aspx";
				}
				else
				{
					this.litPrompt.Text = "优惠券领取失败!";
					if (couponActionStatus == CouponActionStatus.InadequateInventory)
					{
						this.litPrompt.Text = "抱歉，优惠券已领完！";
					}
					if (couponActionStatus == CouponActionStatus.CannotReceive)
					{
						this.litPrompt.Text = "您已领过该优惠券！";
					}
					this.buy.NavigateUrl = "Default.aspx";
					this.buy.Text = "返回商城";
				}
			}
		}
	}
}
