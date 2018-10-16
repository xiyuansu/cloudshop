using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Promotions;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class WAPShakeCouponsForAttention : WAPTemplatedWebControl
	{
		private Label lblAmount;

		private Label lblDiscountValue;

		private Literal startTime;

		private Literal closingTime;

		private HtmlGenericControl shakeCoupon;

		private HtmlGenericControl notCoupon;

		private int CouponID = 0;

		private HtmlAnchor aToGet;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-ShakeCouponsForAttention.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			PageTitle.AddSiteNameTitle("关注有礼优惠券");
			this.lblAmount = (Label)this.FindControl("lblAmount");
			this.lblDiscountValue = (Label)this.FindControl("lblDiscountValue");
			this.startTime = (Literal)this.FindControl("startTime");
			this.closingTime = (Literal)this.FindControl("closingTime");
			this.shakeCoupon = (HtmlGenericControl)this.FindControl("shakeCoupon");
			this.notCoupon = (HtmlGenericControl)this.FindControl("notCoupon");
			this.aToGet = (HtmlAnchor)this.FindControl("aToGet");
			this.CouponID = this.Context.Request["cid"].ToInt(0);
			if (!this.Page.IsPostBack)
			{
				CouponInfo coupon = CouponHelper.GetCoupon(this.CouponID);
				if (coupon != null && coupon.ClosingTime > DateTime.Now)
				{
					this.shakeCoupon.Visible = true;
					this.aToGet.HRef = "ShakeGetCouponsForAttention.aspx?cid=" + this.CouponID;
					this.lblAmount.Text = coupon.OrderUseLimit.Value.F2ToString("f2");
					this.lblAmount.Style.Add("color", "#AE0000");
					this.lblDiscountValue.Text = coupon.Price.F2ToString("f2");
					Literal literal = this.startTime;
					DateTime dateTime = coupon.StartTime;
					literal.Text = dateTime.ToString("yyyy-MM-dd");
					Literal literal2 = this.closingTime;
					dateTime = coupon.ClosingTime;
					literal2.Text = dateTime.ToString("yyyy-MM-dd");
				}
				else
				{
					this.notCoupon.Visible = true;
					this.aToGet.InnerHtml = "返回商城";
					this.aToGet.HRef = "Default.aspx";
				}
			}
		}
	}
}
