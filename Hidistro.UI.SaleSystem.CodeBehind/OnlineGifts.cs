using Hidistro.Core.Entities;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Promotions;
using Hidistro.UI.Common.Controls;
using System;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class OnlineGifts : HtmlTemplatedWebControl
	{
		private ThemedTemplatedRepeater rptPointsCoupons;

		private ThemedTemplatedRepeater rptGifts;

		private Pager pager;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-OnlineGifts.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.rptPointsCoupons = (ThemedTemplatedRepeater)this.FindControl("rptPointsCoupons");
			this.rptGifts = (ThemedTemplatedRepeater)this.FindControl("rptGifts");
			this.pager = (Pager)this.FindControl("pager");
			if (!this.Page.IsPostBack)
			{
				this.BindPointsCoupons();
				this.BindGift();
			}
		}

		private void BindPointsCoupons()
		{
			this.rptPointsCoupons.DataSource = CouponHelper.GetPointsCouponList();
			this.rptPointsCoupons.DataBind();
		}

		private void BindGift()
		{
			GiftQuery giftQuery = new GiftQuery();
			giftQuery.Page.PageIndex = this.pager.PageIndex;
			giftQuery.Page.PageSize = this.pager.PageSize;
			DbQueryResult onlineGifts = ProductBrowser.GetOnlineGifts(giftQuery);
			this.rptGifts.DataSource = onlineGifts.Data;
			this.rptGifts.DataBind();
			this.pager.TotalRecords = onlineGifts.TotalRecords;
		}
	}
}
