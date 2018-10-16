using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Promotions;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class MyCoupons_WeiXinRedEnvelope : MemberTemplatedWebControl
	{
		private ThemedTemplatedRepeater rptMyCoupons;

		private Pager pager;

		private HiddenField hitypeId;

		public int typeId = 0;

		private CouponItemInfo couponItemInfo = new CouponItemInfo();

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-MyCoupons_WeiXinRedEnvelope.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.rptMyCoupons = (ThemedTemplatedRepeater)this.FindControl("rptMyCoupons");
			this.pager = (Pager)this.FindControl("pager");
			this.hitypeId = (HiddenField)this.FindControl("hitypeId");
			this.typeId = this.Page.Request.QueryString["typeId"].ToInt(0);
			this.hitypeId.Value = this.typeId.ToString();
			if (!this.Page.IsPostBack)
			{
				this.BindCoupons();
			}
		}

		private void BindCoupons()
		{
			CouponItemInfoQuery couponItemInfoQuery = new CouponItemInfoQuery();
			couponItemInfoQuery.PageIndex = this.pager.PageIndex;
			couponItemInfoQuery.PageSize = this.pager.PageSize;
			couponItemInfoQuery.UserId = HiContext.Current.UserId;
			couponItemInfoQuery.CouponStatus = this.typeId + 1;
			DbQueryResult weiXinRedEnvelopeList = CouponHelper.GetWeiXinRedEnvelopeList(couponItemInfoQuery);
			this.rptMyCoupons.DataSource = weiXinRedEnvelopeList.Data;
			this.rptMyCoupons.DataBind();
			this.pager.TotalRecords = weiXinRedEnvelopeList.TotalRecords;
		}
	}
}
