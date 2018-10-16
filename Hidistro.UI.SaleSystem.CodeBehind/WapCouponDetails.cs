using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Promotions;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WapCouponDetails : WAPTemplatedWebControl
	{
		private int couponId;

		private Literal litCouponName;

		private Literal litPrice;

		private Literal litCanUseProducts;

		private Literal litOrderUseLimit;

		private Literal litStartTime;

		private Literal litClosingTime;

		private HtmlInputHidden hidCouponId;

		private HtmlInputHidden hidCanUseProducts;

		private HtmlInputHidden hidCanShow;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-coupondetails.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			if (!int.TryParse(this.Page.Request.QueryString["couponId"], out this.couponId))
			{
				this.hidCanShow.Value = "1";
			}
			else
			{
				this.litCouponName = (Literal)this.FindControl("litCouponName");
				this.litPrice = (Literal)this.FindControl("litPrice");
				this.litCanUseProducts = (Literal)this.FindControl("litCanUseProducts");
				this.litOrderUseLimit = (Literal)this.FindControl("litOrderUseLimit");
				this.litStartTime = (Literal)this.FindControl("litStartTime");
				this.litClosingTime = (Literal)this.FindControl("litClosingTime");
				this.hidCouponId = (HtmlInputHidden)this.FindControl("hidCouponId");
				this.hidCanUseProducts = (HtmlInputHidden)this.FindControl("hidCanUseProducts");
				this.hidCanShow = (HtmlInputHidden)this.FindControl("hidCanShow");
				CouponInfo eFCoupon = CouponHelper.GetEFCoupon(this.couponId);
				if (eFCoupon != null)
				{
					this.litCouponName.Text = eFCoupon.CouponName;
					this.litPrice.Text = eFCoupon.Price.F2ToString("f2");
					this.litCanUseProducts.Text = (string.IsNullOrEmpty(eFCoupon.CanUseProducts) ? "全场商品可用" : "部分商品可用");
					Literal literal = this.litOrderUseLimit;
					decimal? orderUseLimit = eFCoupon.OrderUseLimit;
					literal.Text = ((orderUseLimit.GetValueOrDefault() > default(decimal) && orderUseLimit.HasValue) ? ("订单满" + $"{eFCoupon.OrderUseLimit:F2}" + "元可用") : "无限制");
					Literal literal2 = this.litStartTime;
					DateTime dateTime = eFCoupon.StartTime;
					literal2.Text = dateTime.ToString("yyyy.MM.dd");
					Literal literal3 = this.litClosingTime;
					dateTime = eFCoupon.ClosingTime;
					literal3.Text = dateTime.ToString("yyyy.MM.dd");
					this.hidCouponId.Value = this.couponId.ToString();
					this.hidCanUseProducts.Value = eFCoupon.CanUseProducts;
				}
				else
				{
					this.hidCanShow.Value = "1";
				}
			}
		}
	}
}
