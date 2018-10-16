using Hidistro.Context;
using Hidistro.Entities;
using Hidistro.Entities.Orders;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Sales;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Depot.sales
{
	public class ConfirmTake : StoreAdminCallBackPage
	{
		protected bool NeedTakeCode = true;

		private static readonly object submitLock = new object();

		private OrderInfo orderInfo = null;

		protected HtmlGenericControl divorderinfo;

		protected Literal ltlOrderId;

		protected HiddenField hidOrderId;

		protected Literal ltlPaymentType;

		protected Literal ltlUserName;

		protected Literal ltlTelPhone;

		protected OrderStatusLabel lblOrderStatus;

		protected HiddenField hidOrderStatue;

		protected Literal ltlIsTickt;

		protected Literal litInvoiceType;

		protected Literal litInvoiceTitle;

		protected Literal ltlComments;

		protected Repeater dlstOrders;

		protected Repeater grdOrderGift;

		protected FormatedMoneyLabel lblOrderTotal;

		protected TextBox txtTakeCode;

		protected Button btnCheck;

		protected void Page_Load(object sender, EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			this.NeedTakeCode = masterSettings.StoreNeedTakeCode;
			if (!base.IsPostBack)
			{
				this.BindOrder();
			}
		}

		private void BindOrder()
		{
			string text = base.Request.QueryString["code"];
			string text2 = base.Request.QueryString["orderId"];
			if (string.IsNullOrEmpty(text) && string.IsNullOrEmpty(text2))
			{
				this.ShowMsg("参数错误", false);
			}
			else
			{
				if (!string.IsNullOrEmpty(text))
				{
					this.txtTakeCode.Text = text;
					this.orderInfo = OrderHelper.GetOrderInfoByTakeCode(text.Trim());
				}
				else if (!string.IsNullOrEmpty(text2))
				{
					this.orderInfo = OrderHelper.GetOrderInfo(text2.Trim());
				}
				if (this.orderInfo == null)
				{
					this.divorderinfo.Visible = false;
				}
				else
				{
					this.lblOrderTotal.Money = this.orderInfo.GetTotal(false);
					this.ltlOrderId.Text = this.orderInfo.PayOrderId;
					this.hidOrderId.Value = this.orderInfo.OrderId;
					this.lblOrderStatus.OrderStatusCode = this.orderInfo.OrderStatus;
					this.lblOrderStatus.OrderItemStatus = this.orderInfo.ItemStatus;
					this.lblOrderStatus.ShipmentModelId = this.orderInfo.ShippingModeId;
					this.lblOrderStatus.IsConfirm = this.orderInfo.IsConfirm;
					this.lblOrderStatus.Gateway = this.orderInfo.Gateway;
					this.hidOrderStatue.Value = ((int)this.orderInfo.OrderStatus).ToString();
					this.ltlTelPhone.Text = this.orderInfo.TelPhone;
					this.ltlUserName.Text = this.orderInfo.ShipTo;
					this.ltlIsTickt.Text = ((this.orderInfo.Tax > decimal.Zero) ? "是" : "否");
					this.litInvoiceTitle.Text = ((this.orderInfo.Tax > decimal.Zero) ? this.orderInfo.InvoiceTitle : "");
					this.litInvoiceType.Text = ((this.orderInfo.Tax > decimal.Zero) ? EnumDescription.GetEnumDescription((Enum)(object)this.orderInfo.InvoiceType, 0) : "");
					this.ltlPaymentType.Text = this.orderInfo.PaymentType;
					this.ltlComments.Text = this.orderInfo.Remark;
					this.dlstOrders.DataSource = this.orderInfo.LineItems.Values;
					this.dlstOrders.DataBind();
					if (this.orderInfo.Gifts.Count > 0)
					{
						this.grdOrderGift.DataSource = this.orderInfo.Gifts;
						this.grdOrderGift.DataBind();
					}
				}
			}
		}

		protected void btnCheck_Click(object sender, EventArgs e)
		{
			lock (ConfirmTake.submitLock)
			{
				OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.hidOrderId.Value);
				if (orderInfo == null)
				{
					this.ShowMsg("订单不存在", false);
				}
				if (orderInfo.OrderStatus == OrderStatus.WaitBuyerPay)
				{
					orderInfo.IsStoreCollect = true;
					orderInfo.Gateway = "hishop.plugins.payment.cashreceipts";
					orderInfo.PaymentType = "现金支付";
					orderInfo.PaymentTypeId = -99;
				}
				if (orderInfo.FightGroupId > 0 && orderInfo.FightGroupStatus != FightGroupStatus.FightGroupSuccess)
				{
					this.ShowMsg("该订单是火拼团订单，但是组团还没未成功,不能提货。", false);
				}
				else if (OrderHelper.ConfirmTakeGoods(orderInfo, false))
				{
					this.ShowMsg("成功的完成了该订单", true);
				}
				else
				{
					this.ShowMsg("完成订单失败,可能是库存不足", false);
				}
			}
		}

		protected void dlstOrders_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				string key = (string)DataBinder.Eval(e.Item.DataItem, "SkuId");
				if (this.orderInfo.LineItems.ContainsKey(key))
				{
					LineItemInfo lineItemInfo = this.orderInfo.LineItems[key];
					string text = lineItemInfo.StatusText;
					if (lineItemInfo.Status == LineItemStatus.Normal)
					{
						text = TradeHelper.GetOrderStatusText(this.orderInfo.OrderStatus, this.orderInfo.Gateway);
					}
					Literal literal = (Literal)e.Item.FindControl("litStatusText");
					if (literal != null)
					{
						literal.Text = text;
					}
				}
			}
		}
	}
}
