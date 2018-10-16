using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Depot
{
	[PrivilegeCheck(Privilege.EditOrders)]
	public class EditOrder : StoreAdminPage
	{
		private string orderId;

		private OrderInfo order;

		protected Repeater grdProducts;

		protected FormatedMoneyLabel lblAllPrice;

		protected Label lblWeight;

		protected Repeater grdOrderGift;

		protected FormatedMoneyLabel fullDiscountAmount;

		protected HyperLink lkbtnFullDiscount;

		protected HyperLink lkbtnFullFree;

		protected TextBox txtAdjustedFreight;

		protected Literal litShipModeName;

		protected TextBox txtAdjustedPayCharge;

		protected Literal litPayName;

		protected FormatedMoneyLabel couponAmount;

		protected TextBox txtAdjustedDiscount;

		protected Literal litTax;

		protected Literal litInvoiceTitle;

		protected Literal litPoint;

		protected HyperLink hlkSentTimesPointPromotion;

		protected Literal litTotal;

		protected Button btnUpdateOrderAmount;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
			{
				base.GotoResourceNotFound();
			}
			else
			{
				this.orderId = this.Page.Request.QueryString["OrderId"];
				this.btnUpdateOrderAmount.Click += this.btnUpdateOrderAmount_Click;
				this.grdOrderGift.ItemCommand += this.grdOrderGift_ItemCommand;
				this.grdProducts.ItemCommand += this.grdProducts_ItemCommand;
				this.order = OrderHelper.GetOrderInfo(this.orderId);
				if (!this.Page.IsPostBack)
				{
					if (this.order == null)
					{
						base.GotoResourceNotFound();
					}
					else
					{
						this.BindProductList(this.order);
						this.BindOtherAmount(this.order);
						this.BindTatolAmount(this.order);
					}
				}
			}
		}

		public int GetPoint(OrderInfo order)
		{
			int result = 0;
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if ((order.GetTotal(false) - order.AdjustedFreight - order.AdjustedDiscount) * order.TimesPoint / masterSettings.PointsRate > 2147483647m)
			{
				result = 2147483647;
			}
			else if (masterSettings.PointsRate != decimal.Zero)
			{
				result = (int)Math.Round((order.GetTotal(false) - order.AdjustedFreight - order.AdjustedDiscount) * order.TimesPoint / masterSettings.PointsRate, 0);
			}
			return result;
		}

		private void btnUpdateOrderAmount_Click(object sender, EventArgs e)
		{
			decimal adjustedFreight = default(decimal);
			decimal adjustedDiscount = default(decimal);
			if (!this.order.CheckAction(OrderActions.SELLER_MODIFY_TRADE))
			{
				this.ShowMsg("你当前订单的状态不能进行修改订单费用操作", false);
			}
			else if (this.ValidateValues(out adjustedFreight, out adjustedDiscount))
			{
				string empty = string.Empty;
				this.order.AdjustedFreight = adjustedFreight;
				this.order.AdjustedDiscount = adjustedDiscount;
				decimal total = this.order.GetTotal(true);
				this.order.Points = this.GetPoint(this.order);
				ValidationResults validationResults = Validation.Validate(this.order, "ValOrder");
				if (!validationResults.IsValid)
				{
					using (IEnumerator<ValidationResult> enumerator = ((IEnumerable<ValidationResult>)validationResults).GetEnumerator())
					{
						if (enumerator.MoveNext())
						{
							ValidationResult current = enumerator.Current;
							empty += Formatter.FormatErrorMessage(current.Message);
							this.ShowMsg(empty, false);
							return;
						}
					}
				}
				if (total > decimal.Zero)
				{
					if (OrderHelper.UpdateOrderAmount(this.order))
					{
						this.BindTatolAmount(this.order);
						this.ShowMsg("成功的修改了订单金额", true);
					}
					else
					{
						this.ShowMsg("修改订单金额失败", false);
					}
				}
				else
				{
					this.ShowMsg("订单的应付总金额不应该是负数,请重新输入订单折扣", false);
				}
			}
		}

		private void BindProductList(OrderInfo order)
		{
			this.grdProducts.DataSource = order.LineItems.Values;
			this.grdProducts.DataBind();
			this.grdOrderGift.DataSource = order.Gifts;
			this.grdOrderGift.DataBind();
		}

		private void grdOrderGift_ItemCommand(object sender, RepeaterCommandEventArgs e)
		{
			if (e.CommandName == "Delete")
			{
				if (!this.order.CheckAction(OrderActions.SELLER_MODIFY_TRADE))
				{
					this.ShowMsg("你当前订单的状态不能进行修改订单费用操作", false);
				}
				else
				{
					int giftId = (int)DataBinder.Eval(e.Item.DataItem, "GiftId");
					if (OrderHelper.DeleteOrderGift(this.order, giftId))
					{
						this.order = OrderHelper.GetOrderInfo(this.orderId);
						this.BindProductList(this.order);
						this.BindTatolAmount(this.order);
						this.ShowMsg("成功删除了一件礼品", true);
					}
					else
					{
						this.ShowMsg("删除礼品失败", false);
					}
				}
			}
		}

		private void grdProducts_ItemCommand(object sender, RepeaterCommandEventArgs e)
		{
			if (e.CommandName == "Delete")
			{
				if (!this.order.CheckAction(OrderActions.SELLER_MODIFY_TRADE))
				{
					this.ShowMsg("你当前订单的状态不能进行修改订单费用操作", false);
					return;
				}
				if (this.order.LineItems.Values.Count <= 1)
				{
					this.ShowMsg("订单的最后一件商品不允许删除", false);
					return;
				}
				string sku = e.CommandArgument.ToString();
				if (OrderHelper.DeleteLineItem(sku, this.order))
				{
					this.order = OrderHelper.GetOrderInfo(this.orderId);
					this.BindProductList(this.order);
					this.BindTatolAmount(this.order);
					this.ShowMsg("成功删除了一件商品", true);
				}
				else
				{
					this.ShowMsg("删除商品失败", false);
				}
			}
			if (e.CommandName == "setQuantity")
			{
				OrderInfo orderInfo = this.order;
				if (!this.order.CheckAction(OrderActions.SELLER_MODIFY_TRADE))
				{
					this.ShowMsg("你当前订单的状态不能进行修改订单费用操作", false);
				}
				else
				{
					string text = e.CommandArgument.ToString();
					TextBox textBox = e.Item.FindControl("txtQuantity") as TextBox;
					int quantity = orderInfo.LineItems[text].Quantity;
					int num = default(int);
					if (!int.TryParse(textBox.Text.Trim(), out num))
					{
						this.ShowMsg("商品数量填写错误", false);
					}
					else if (num > OrderHelper.GetSkuStock(text))
					{
						this.ShowMsg("此商品库存不够", false);
					}
					else if (num <= 0)
					{
						this.ShowMsg("商品购买数量不能为0", false);
					}
					else
					{
						orderInfo.LineItems[text].Quantity = num;
						orderInfo.LineItems[text].ShipmentQuantity = num;
						orderInfo.LineItems[text].ItemAdjustedPrice = orderInfo.LineItems[text].ItemListPrice;
						if (this.order.PreSaleId > 0)
						{
							if (orderInfo.AdjustedDiscount < decimal.Zero && orderInfo.FinalPayment == decimal.Zero && num < quantity)
							{
								this.ShowMsg("数量减少后尾款金额为负数,请先更改手工折扣", false);
								return;
							}
						}
						else if (orderInfo.GetTotal(false) <= decimal.Zero)
						{
							this.ShowMsg("订单总金额必须大于0", false);
							return;
						}
						if (quantity != num)
						{
							if (OrderHelper.UpdateLineItem(text, this.order, num, 0))
							{
								this.BindProductList(this.order);
								this.BindTatolAmount(this.order);
								this.ShowMsg("修改商品购买数量成功", true);
							}
							else
							{
								this.ShowMsg("修改商品购买数量失败", false);
							}
						}
					}
				}
			}
		}

		private void BindTatolAmount(OrderInfo order)
		{
			decimal amount = order.GetAmount(false);
			this.lblAllPrice.Money = amount;
			this.lblWeight.Text = order.Weight.ToString("f2", CultureInfo.InvariantCulture);
			this.litPoint.Text = order.Points.ToString(CultureInfo.InvariantCulture);
			this.litTotal.Text = Globals.FormatMoney(order.GetTotal(true));
		}

		private void BindOtherAmount(OrderInfo order)
		{
			if (order.IsReduced)
			{
				this.fullDiscountAmount.Money = order.ReducedPromotionAmount;
				this.lkbtnFullDiscount.NavigateUrl = base.GetRouteUrl("FavourableDetails", new
				{
					activityId = order.ReducedPromotionId
				});
				this.lkbtnFullDiscount.Text = order.ReducedPromotionName;
			}
			if (order.IsFreightFree)
			{
				this.lkbtnFullFree.NavigateUrl = base.GetRouteUrl("FavourableDetails", new
				{
					activityId = order.FreightFreePromotionId
				});
				this.lkbtnFullFree.Text = order.FreightFreePromotionName;
			}
			if (order.IsSendTimesPoint)
			{
				this.hlkSentTimesPointPromotion.Text = order.SentTimesPointPromotionName;
				this.hlkSentTimesPointPromotion.NavigateUrl = base.GetRouteUrl("FavourableDetails", new
				{
					activityId = order.SentTimesPointPromotionId
				});
			}
			TextBox textBox = this.txtAdjustedFreight;
			decimal num = order.AdjustedFreight;
			textBox.Text = num.ToString("F", CultureInfo.InvariantCulture);
			TextBox textBox2 = this.txtAdjustedDiscount;
			num = order.AdjustedDiscount;
			textBox2.Text = num.ToString("F", CultureInfo.InvariantCulture);
			if (!string.IsNullOrEmpty(order.PaymentType))
			{
				this.litPayName.Text = "(" + order.PaymentType + ")";
			}
			if (!string.IsNullOrEmpty(order.CouponName))
			{
				this.couponAmount.Text = "[" + order.CouponName + "]-" + Globals.FormatMoney(order.CouponValue);
			}
			else
			{
				this.couponAmount.Text = "-" + Globals.FormatMoney(order.CouponValue);
			}
			if (order.Tax > decimal.Zero && order.SupplierId == 0)
			{
				this.litTax.Text = "<tr class=\"bg\"><td align=\"right\">税金(元)：</td><td colspan=\"2\"><span class='Name'>" + Globals.FormatMoney(order.Tax);
				Literal literal = this.litTax;
				literal.Text += "</span></td></tr>";
			}
			if (order.InvoiceTitle.Length > 0 && order.SupplierId == 0)
			{
				this.litInvoiceTitle.Text = "<tr class=\"bg\"><td align=\"right\">发票抬头：</td><td colspan=\"2\"><span class='Name'>" + order.InvoiceTitle;
				Literal literal2 = this.litInvoiceTitle;
				literal2.Text += "</span></td></tr>";
				if (!string.IsNullOrWhiteSpace(order.InvoiceTaxpayerNumber))
				{
					Literal literal3 = this.litInvoiceTitle;
					literal3.Text = literal3.Text + "<tr class=\"bg\"><td align=\"right\">纳税人识别号：</td><td colspan=\"2\"><span class='Name'>" + order.InvoiceTaxpayerNumber;
					Literal literal4 = this.litInvoiceTitle;
					literal4.Text += "</span></td></tr>";
				}
			}
		}

		private bool ValidateValues(out decimal adjustedFreight, out decimal discountAmout)
		{
			string text = string.Empty;
			if (!decimal.TryParse(this.txtAdjustedFreight.Text.Trim(), out adjustedFreight))
			{
				text += Formatter.FormatErrorMessage("运费必须在0-1000万之间");
			}
			int num = 0;
			if (this.txtAdjustedDiscount.Text.Trim().IndexOf(".") > 0)
			{
				num = this.txtAdjustedDiscount.Text.Trim().Substring(this.txtAdjustedDiscount.Text.Trim().IndexOf(".") + 1).Length;
			}
			if (!decimal.TryParse(this.txtAdjustedDiscount.Text.Trim(), out discountAmout) || num > 2)
			{
				text += Formatter.FormatErrorMessage("订单折扣填写错误,订单折扣只能是数值，且不能超过2位小数");
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.ShowMsg(text, false);
				return false;
			}
			return true;
		}
	}
}
