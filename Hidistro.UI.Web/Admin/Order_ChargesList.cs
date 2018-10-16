using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Sales;
using Hidistro.UI.Common.Controls;
using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	public class Order_ChargesList : UserControl
	{
		private OrderInfo order;

		protected FormatedMoneyLabel lblTotalPrice;

		protected HtmlTableRow tdWeight;

		protected Literal litWeight;

		protected HtmlTableRow tdFreight;

		protected Literal litFreight;

		protected Literal litShippingMode;

		protected Literal litPayMode;

		protected HtmlTableRow tdBalanceAmount;

		protected Literal liBalanceAmount;

		protected HtmlTableRow tdCouponValue;

		protected Literal litCoupon;

		protected Literal litCouponValue;

		protected HtmlTableRow tdDeductionMoney;

		protected Literal lblDeductionMoney;

		protected HtmlTableRow tdDiscount;

		protected Literal litDiscount;

		protected Literal litTax;

		protected Literal litInvoiceTitle;

		protected Literal litPoints;

		protected HtmlTableRow tdRefundPoint;

		protected Literal litRefundPoint;

		protected HtmlTableRow tdReducedPromotion;

		protected Literal litPromotionPrice;

		protected HyperLink hlkReducedPromotion;

		protected HtmlTableRow tdBundlingPrice;

		protected Literal lblBundlingPrice;

		protected Literal litTotalPrice;

		protected HtmlTableRow tdRefundAmout;

		protected Literal litRefundAmount;

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
			decimal num = default(decimal);
			foreach (LineItemInfo value in this.order.LineItems.Values)
			{
				if ((value.Status == LineItemStatus.Refunded || value.Status == LineItemStatus.Returned) && value.ReturnInfo != null && value.Status == LineItemStatus.Returned)
				{
					num += value.ReturnInfo.RefundAmount;
				}
			}
			if (this.order.BalanceAmount > decimal.Zero)
			{
				this.tdBalanceAmount.Visible = true;
				this.liBalanceAmount.Text = this.order.BalanceAmount.F2ToString("f2");
			}
			else
			{
				this.tdBalanceAmount.Visible = false;
			}
			if (this.order.RefundAmount > decimal.Zero || num > decimal.Zero)
			{
				int sumRefundPoint = SalesHelper.GetSumRefundPoint(this.order.OrderId);
				this.litRefundPoint.Text = sumRefundPoint.ToString();
				this.litRefundAmount.Text = ((this.order.RefundAmount > decimal.Zero) ? this.order.RefundAmount.F2ToString("f2") : num.F2ToString("f2"));
				this.tdRefundAmout.Visible = true;
				if (sumRefundPoint > 0)
				{
					this.tdRefundPoint.Visible = true;
				}
			}
			this.litFreight.Text = Globals.FormatMoney(this.order.AdjustedFreight);
			this.litPayMode.Text = ((this.order.PaymentType.ToNullString().Length > 8) ? (this.order.PaymentType.Substring(0, 8) + "...") : this.order.PaymentType);
			if (!string.IsNullOrEmpty(this.order.CouponName))
			{
				this.litCoupon.Text = "[" + ((this.order.CouponName.Length > 6) ? (this.order.CouponName.Substring(0, 6) + "...") : this.order.CouponName) + "]-" + Globals.FormatMoney(this.order.CouponValue);
			}
			else
			{
				this.litCoupon.Text = "-" + Globals.FormatMoney(this.order.CouponValue);
			}
			if (this.order.CouponValue > decimal.Zero)
			{
				this.tdCouponValue.Visible = true;
			}
			this.litCouponValue.Text = "-" + Globals.FormatMoney(this.order.CouponValue);
			if (!this.order.DeductionMoney.HasValue || this.order.DeductionMoney.Value <= decimal.Zero)
			{
				this.lblDeductionMoney.Text = "-0.00";
			}
			else
			{
				this.tdDeductionMoney.Visible = true;
				this.lblDeductionMoney.Text = "-" + Math.Round(this.order.DeductionMoney.Value, 2).ToString();
			}
			if (this.order.AdjustedDiscount != decimal.Zero)
			{
				this.tdDiscount.Visible = true;
				this.litDiscount.Text = Globals.FormatMoney(this.order.AdjustedDiscount);
			}
			this.litPoints.Text = this.order.Points.ToString(CultureInfo.InvariantCulture);
			this.litTotalPrice.Text = Globals.FormatMoney(this.order.GetPayTotal());
			if (this.order.Tax > decimal.Zero && this.order.SupplierId == 0)
			{
				this.litTax.Text = "<tr class=\"bg\"><td align=\"right\">税金(元)：</td><td colspan=\"2\"><span class='Name'>" + Globals.FormatMoney(this.order.Tax);
				Literal literal = this.litTax;
				literal.Text += "</span></td></tr>";
			}
			if (this.order.InvoiceTitle.Length > 0 && this.order.SupplierId == 0)
			{
				this.litInvoiceTitle.Text = "<tr class=\"bg\"><td align=\"right\">发票抬头：</td><td colspan=\"2\"><span class='Name'>" + this.order.InvoiceTitle;
				Literal literal2 = this.litInvoiceTitle;
				literal2.Text += "</span></td></tr>";
				if (!string.IsNullOrWhiteSpace(this.order.InvoiceTaxpayerNumber))
				{
					Literal literal3 = this.litInvoiceTitle;
					literal3.Text = literal3.Text + "<tr class=\"bg\"><td align=\"right\">纳税人识别号：</td><td colspan=\"2\"><span class='Name'>" + this.order.InvoiceTaxpayerNumber;
					Literal literal4 = this.litInvoiceTitle;
					literal4.Text += "</span></td></tr>";
				}
			}
			if (this.order.OrderType == OrderType.ServiceOrder)
			{
				this.tdFreight.Visible = false;
			}
			this.lblTotalPrice.Money = this.order.GetAmount(false);
			if (this.order.IsReduced)
			{
				this.tdReducedPromotion.Visible = true;
				this.litPromotionPrice.Text = "-" + Globals.FormatMoney(this.order.ReducedPromotionAmount);
				string reducedPromotionName = this.order.ReducedPromotionName;
				this.hlkReducedPromotion.Text = ((reducedPromotionName.ToNullString().Length > 7) ? (reducedPromotionName.Substring(0, 7) + "...") : reducedPromotionName);
				this.hlkReducedPromotion.ToolTip = reducedPromotionName;
				this.hlkReducedPromotion.NavigateUrl = base.GetRouteUrl("FavourableDetails", new
				{
					activityId = this.order.ReducedPromotionId
				});
			}
		}
	}
}
