using Hidistro.Context;
using Hidistro.SaleSystem.Promotions;
using Hidistro.UI.Common.Controls;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WAPMemberCoupons : WAPMemberTemplatedWebControl
	{
		private Repeater rptCoupons;

		private HtmlInputHidden hidHasCoupon;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-vmemberCoupons.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.hidHasCoupon = (HtmlInputHidden)this.FindControl("hidHasCoupon");
			int useType = 1;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["usedType"]))
			{
				int.TryParse(this.Page.Request.QueryString["usedType"], out useType);
			}
			DataTable userCoupons = CouponHelper.GetUserCoupons(HiContext.Current.UserId, useType);
			if (userCoupons != null)
			{
				this.rptCoupons = (Repeater)this.FindControl("rptCoupons");
				this.rptCoupons.DataSource = userCoupons;
				this.rptCoupons.DataBind();
			}
			if (userCoupons == null || userCoupons.Rows.Count <= 0)
			{
				this.hidHasCoupon.Value = "0";
			}
			PageTitle.AddSiteNameTitle("优惠券");
		}
	}
}
