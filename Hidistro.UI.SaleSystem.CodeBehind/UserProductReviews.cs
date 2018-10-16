using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Comments;
using Hidistro.SaleSystem.Catalog;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class UserProductReviews : MemberTemplatedWebControl
	{
		private ThemedTemplatedRepeater dlstPts;

		private Pager pager;

		private Literal litReviewCount;

		private string orderId;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UserProductReviews.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.dlstPts = (ThemedTemplatedRepeater)this.FindControl("dlstPts");
			this.pager = (Pager)this.FindControl("pager");
			this.litReviewCount = (Literal)this.FindControl("litReviewCount");
			PageTitle.AddSiteNameTitle("我参与的评论");
			if (!this.Page.IsPostBack)
			{
				this.orderId = this.Context.Request.QueryString["orderId"].ToNullString();
				if (this.litReviewCount != null)
				{
					this.litReviewCount.Text = ProductBrowser.GetUserProductReviewsCount().ToString();
				}
				this.BindPtAndReviewsAndReplys();
			}
		}

		private void BindPtAndReviewsAndReplys()
		{
			ProductReviewQuery productReviewQuery = new ProductReviewQuery();
			productReviewQuery.PageIndex = this.pager.PageIndex;
			productReviewQuery.PageSize = this.pager.PageSize;
			productReviewQuery.SortBy = "ReviewDate";
			productReviewQuery.SortOrder = SortAction.Desc;
			productReviewQuery.orderId = this.orderId;
			DbQueryResult userProductReviewsAndReplys = ProductBrowser.GetUserProductReviewsAndReplys(productReviewQuery);
			this.dlstPts.DataSource = userProductReviewsAndReplys.Data;
			this.dlstPts.DataBind();
			this.pager.TotalRecords = userProductReviewsAndReplys.TotalRecords;
		}
	}
}
