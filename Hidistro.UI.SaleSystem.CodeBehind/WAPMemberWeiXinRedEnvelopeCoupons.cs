using Hidistro.Context;
using Hidistro.Core;
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
	public class WAPMemberWeiXinRedEnvelopeCoupons : WAPMemberTemplatedWebControl
	{
		private Repeater rptCoupons;

		private HtmlInputHidden hidHasCoupon;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-vmemberWeiXinRedEnvelopeCoupons.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.hidHasCoupon = (HtmlInputHidden)this.FindControl("hidHasCoupon");
			int useType = this.Page.Request.QueryString["usedType"].ToInt(0);
			DataTable userWeiXinRedEnvelope = CouponHelper.GetUserWeiXinRedEnvelope(HiContext.Current.UserId, useType);
			if (userWeiXinRedEnvelope != null)
			{
				this.rptCoupons = (Repeater)this.FindControl("rptCoupons");
				this.rptCoupons.DataSource = userWeiXinRedEnvelope;
				this.rptCoupons.DataBind();
			}
			if (userWeiXinRedEnvelope == null || userWeiXinRedEnvelope.Rows.Count <= 0)
			{
				this.hidHasCoupon.Value = "0";
			}
			PageTitle.AddSiteNameTitle("代金红包");
		}
	}
}
