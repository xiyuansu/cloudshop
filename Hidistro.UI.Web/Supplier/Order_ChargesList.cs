using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Orders;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Supplier
{
	public class Order_ChargesList : UserControl
	{
		private OrderInfo order;

		protected FormatedMoneyLabel lblTotalPrice;

		protected HtmlTableRow tdWeight;

		protected Literal litWeight;

		protected HtmlTableRow tdFreight;

		protected Literal litFreight;

		protected HtmlTableRow tdRefundAmout;

		protected Literal litRefundAmount;

		protected Literal litTotalPrice;

		public OrderInfo Order
		{
			get
			{
				return this.order;
			}
			set
			{
				this.order = value;
			}
		}

		public bool IsStoreAdminView
		{
			get;
			set;
		}

		protected override void OnLoad(EventArgs e)
		{
			this.LoadControls();
		}

		public void LoadControls()
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			decimal amount_Cost = this.order.GetAmount_Cost(false);
			this.lblTotalPrice.Money = amount_Cost;
			this.litTotalPrice.Text = Globals.FormatMoney(this.order.OrderCostPrice + this.order.Freight);
			this.litFreight.Text = Globals.FormatMoney(this.order.Freight);
			decimal num = default(decimal);
			decimal num2 = default(decimal);
			foreach (LineItemInfo value in this.order.LineItems.Values)
			{
				num2 = value.GetSubTotal_Cost();
				if ((value.Status == LineItemStatus.Refunded || value.Status == LineItemStatus.Returned) && value.ReturnInfo != null && value.Status == LineItemStatus.Returned)
				{
					num += ((num2 > value.ReturnInfo.RefundAmount) ? value.ReturnInfo.RefundAmount : num2);
				}
			}
			if (this.order.RefundAmount > decimal.Zero && num == decimal.Zero)
			{
				num = ((this.order.RefundAmount > amount_Cost) ? amount_Cost : this.order.RefundAmount);
			}
			if (num > decimal.Zero)
			{
				this.litRefundAmount.Text = num.F2ToString("f2");
				this.tdRefundAmout.Visible = true;
			}
		}
	}
}
