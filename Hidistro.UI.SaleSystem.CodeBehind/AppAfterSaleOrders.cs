using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Member;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class AppAfterSaleOrders : AppshopMemberTemplatedWebControl
	{
		private Common_AfterSaleOrders rptOrders;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-AfterSaleOrders.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			PageTitle.AddSiteNameTitle("售后订单");
			AfterSalesQuery afterSalesQuery = new AfterSalesQuery();
			afterSalesQuery.PageSize = 1000000;
			afterSalesQuery.PageIndex = 1;
			this.rptOrders = (Common_AfterSaleOrders)this.FindControl("Common_AfterSaleOrder");
			this.rptOrders.ItemDataBound += this.rptOrders_ItemDataBound;
			this.rptOrders.DataSource = MemberProcessor.GetUserAfterOrders(HiContext.Current.UserId, afterSalesQuery).Models;
			this.rptOrders.DataBind();
		}

		private void rptOrders_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				AfterSaleRecordModel afterSaleRecordModel = (AfterSaleRecordModel)e.Item.DataItem;
				HtmlAnchor htmlAnchor = (HtmlAnchor)e.Item.FindControl("lkbtnViewMessage");
				HtmlGenericControl htmlGenericControl = (HtmlGenericControl)e.Item.FindControl("AfterSaleIdSpan");
				HtmlAnchor htmlAnchor2 = (HtmlAnchor)e.Item.FindControl("lkbtnSendGoodsForReturn");
				Literal literal = (Literal)e.Item.FindControl("PayMoneySpan");
				HtmlAnchor htmlAnchor3 = (HtmlAnchor)e.Item.FindControl("lnkViewLogistics");
				HtmlGenericControl htmlGenericControl2 = (HtmlGenericControl)e.Item.FindControl("litRefundMoney");
				HtmlAnchor htmlAnchor4 = (HtmlAnchor)e.Item.FindControl("lnkToDetail");
				HtmlGenericControl htmlGenericControl3 = (HtmlGenericControl)e.Item.FindControl("StatusLabel1");
				Repeater repeater = (Repeater)e.Item.FindControl("Repeater1");
				if (htmlGenericControl2 != null)
				{
					htmlGenericControl2.InnerText = afterSaleRecordModel.RefundAmount.F2ToString("f2");
				}
				int afterSaleId = afterSaleRecordModel.AfterSaleId;
				htmlGenericControl.InnerText = afterSaleRecordModel.OrderId;
				literal.Text = afterSaleRecordModel.TradeTotal.F2ToString("f2");
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(afterSaleRecordModel.OrderId);
				if (htmlAnchor3 != null && (afterSaleRecordModel.AfterSaleType == AfterSaleTypes.ReturnAndRefund || afterSaleRecordModel.AfterSaleType == AfterSaleTypes.Replace))
				{
					if (afterSaleRecordModel.HandleStatus == 4 || afterSaleRecordModel.HandleStatus == 4 || afterSaleRecordModel.HandleStatus == 6)
					{
						if (afterSaleRecordModel.AfterSaleType == AfterSaleTypes.ReturnAndRefund)
						{
							htmlAnchor3.HRef = "MyLogistics?returnsId=" + afterSaleRecordModel.AfterSaleId;
						}
						else
						{
							htmlAnchor3.HRef = "MyLogistics?ReplaceId=" + afterSaleRecordModel.AfterSaleId;
						}
						htmlAnchor3.Visible = true;
					}
					else
					{
						htmlAnchor3.Visible = false;
					}
				}
				if (htmlAnchor4 != null)
				{
					if (afterSaleRecordModel.AfterSaleType == AfterSaleTypes.OrderRefund)
					{
						htmlAnchor4.HRef = "UserRefundDetail?RefundId=" + afterSaleId.ToString();
					}
					else if (afterSaleRecordModel.AfterSaleType == AfterSaleTypes.ReturnAndRefund || afterSaleRecordModel.AfterSaleType == AfterSaleTypes.OnlyRefund)
					{
						htmlAnchor4.HRef = "UserReturnDetail?ReturnId=" + afterSaleId.ToString();
					}
					else
					{
						htmlAnchor4.Visible = false;
					}
				}
				if (htmlAnchor2 != null && afterSaleRecordModel.AfterSaleType == AfterSaleTypes.ReturnAndRefund && afterSaleRecordModel.HandleStatus == 3)
				{
					htmlAnchor2.Visible = true;
					htmlAnchor2.HRef = "ReturnSendGoods?ReturnsId=" + afterSaleRecordModel.AfterSaleId;
				}
				htmlGenericControl3.InnerText = afterSaleRecordModel.StatusText;
				string skuId = afterSaleRecordModel.SkuId;
				IList<LineItemInfo> list = orderInfo.LineItems.Values.ToList();
				if (!string.IsNullOrEmpty(skuId) && orderInfo.LineItems.ContainsKey(skuId))
				{
					list = new List<LineItemInfo>();
					list.Add(orderInfo.LineItems[skuId]);
				}
				repeater.DataSource = list;
				repeater.ItemDataBound += this.Repeater1_ItemDataBound;
				repeater.DataBind();
			}
		}

		private void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				LineItemInfo lineItemInfo = e.Item.DataItem as LineItemInfo;
				HtmlGenericControl htmlGenericControl = e.Item.FindControl("hylinkProductName") as HtmlGenericControl;
				Literal literal = e.Item.FindControl("ltlSKUContent") as Literal;
				Literal literal2 = e.Item.FindControl("ltlPrice") as Literal;
				Literal literal3 = (Literal)e.Item.FindControl("ltlProductCount");
				htmlGenericControl.InnerText = lineItemInfo.ItemDescription.ToNullString();
				literal.Text = lineItemInfo.SKUContent.ToNullString();
				literal2.Text = lineItemInfo.ItemListPrice.ToDecimal(0).F2ToString("f2");
				literal3.Text = lineItemInfo.Quantity.ToString();
			}
		}
	}
}
