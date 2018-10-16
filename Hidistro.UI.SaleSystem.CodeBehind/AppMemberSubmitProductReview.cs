using Hidistro.Context;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Member;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class AppMemberSubmitProductReview : AppshopMemberTemplatedWebControl
	{
		private string orderId;

		private AppshopTemplatedRepeater orderItems;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-MemberSubmitProductReview.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			PageTitle.AddSiteNameTitle("商品评价");
			if (string.IsNullOrEmpty(this.Page.Request.QueryString["orderId"]))
			{
				this.ShowWapMessage("订单还未完成，不能进行评价", "goHomeUrl");
			}
			this.orderId = this.Page.Request.QueryString["orderId"];
			this.orderItems = (AppshopTemplatedRepeater)this.FindControl("rptRegisterCoupons");
			if (!this.Page.IsPostBack)
			{
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(this.orderId);
				if (orderInfo != null && HiContext.Current.UserId != 0 && HiContext.Current.UserId == orderInfo.UserId)
				{
					this.CanToProductReviews(orderInfo);
					if (orderInfo.OrderStatus != OrderStatus.Finished && (orderInfo.OrderStatus != OrderStatus.Closed || orderInfo.OnlyReturnedCount != orderInfo.LineItems.Count))
					{
						this.ShowWapMessage("订单还未完成，不能进行评价", "MemberOrderDetails.aspx?OrderId=" + orderInfo.OrderId);
					}
					this.BindOrderItems(orderInfo);
				}
				else
				{
					this.ShowWapMessage("该订单不存在或者不属于当前用户的订单", "goHomeUrl");
				}
			}
		}

		private void CanToProductReviews(OrderInfo order)
		{
			if (ProductBrowser.CheckAllProductReview(order.OrderId))
			{
				this.Page.Response.Redirect("MemberProductReview.aspx?OrderId=" + this.orderId);
			}
		}

		private void BindOrderItems(OrderInfo order)
		{
			DataTable productReviewAll = ProductBrowser.GetProductReviewAll(this.orderId);
			Dictionary<string, LineItemInfo> dictionary = new Dictionary<string, LineItemInfo>();
			LineItemInfo lineItemInfo = new LineItemInfo();
			int num = 0;
			int num2 = 0;
			bool flag = false;
			foreach (KeyValuePair<string, LineItemInfo> lineItem in order.LineItems)
			{
				flag = false;
				lineItemInfo = lineItem.Value;
				for (int i = 0; i < productReviewAll.Rows.Count; i++)
				{
					if (lineItemInfo.ProductId.ToString() == productReviewAll.Rows[i][0].ToString() && lineItemInfo.SkuId.ToString().Trim() == productReviewAll.Rows[i][1].ToString().Trim())
					{
						flag = true;
					}
				}
				if (flag)
				{
					dictionary.Add(lineItem.Key, lineItemInfo);
				}
				else
				{
					num2++;
				}
			}
			if (num + num2 == order.LineItems.Count)
			{
				this.Page.Response.Redirect("MemberProductReview.aspx?OrderId=" + this.orderId);
			}
			this.orderItems.DataSource = dictionary.Values;
			this.orderItems.DataBind();
		}
	}
}
