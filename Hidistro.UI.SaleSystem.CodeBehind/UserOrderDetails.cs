using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using System;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class UserOrderDetails : MemberTemplatedWebControl
	{
		private string orderId;

		private FormatedTimeLabel litAddOrderDate;

		private FormatedTimeLabel litPayDate;

		private FormatedTimeLabel litShipDate;

		private FormatedTimeLabel litResultDate;

		private FormatedTimeLabel litPayBack;

		private FormatedTimeLabel litBackShip;

		private Literal litOrderId;

		private FormatedTimeLabel litAddDate;

		private FormatedMoneyLabel lbltotalPrice;

		private OrderStatusLabel lblOrderStatus;

		private Literal litCloseReason;

		private Literal litRemark;

		private Literal litShipTo;

		private Literal litRegion;

		private Literal litAddress;

		private Literal litZipcode;

		private Literal litEmail;

		private Literal litPhone;

		private Literal litTellPhone;

		private Literal litShipToDate;

		private Literal litBalanceAmount;

		private HtmlInputHidden hdorderId;

		private Literal litPaymentType;

		private Literal litModeName;

		private Panel plOrderSended;

		private Literal litRealModeName;

		private Literal litShippNumber;

		private HyperLink litDiscountName;

		private HyperLink litFreeName;

		private Literal litInvoiceTitle;

		private Literal litInvoiceTaxpayerNumber;

		private FormatedMoneyLabel litTax;

		private Panel plExpress;

		private HtmlAnchor power;

		private Literal litTakeCode;

		private Literal litStoreName;

		private Literal litStoreInfo;

		private Literal litStoreTel;

		private HtmlGenericControl divstoreinfo;

		private Common_OrderManage_OrderItems listOrders;

		private Common_OrderManage_OrderGifts orderGifts;

		private Panel plOrderGift;

		private Literal lblBundlingPrice;

		private Literal litPoints;

		private HyperLink litSentTimesPointPromotion;

		private Literal litWeight;

		private Literal litFree;

		private FormatedMoneyLabel lblFreight;

		private Literal litCouponValue;

		private Literal litPointMoney;

		private FormatedMoneyLabel lblDiscount;

		private FormatedMoneyLabel litTotalPrice;

		private FormatedMoneyLabel lblAdjustedDiscount;

		private FormatedMoneyLabel lblRefundTotal;

		private FormatedMoneyLabel lblDeposit;

		private FormatedMoneyLabel lblFinalPayment;

		private TextBox txtReplaceRemark;

		private TextBox txtReturnRemark;

		private TextBox txtRemark;

		private RefundTypeDropDownList dropReturnRefundType;

		private RefundTypeDropDownList dropRefundType;

		private DropDownList dropPayType;

		private Button btnPay;

		private Label lbRefundMoney;

		private Label lbCloseReason;

		private LinkButton lkbtnConfirmOrder;

		private LinkButton lkbtnCloseOrder;

		private Panel plRefund;

		private FormatedMoneyLabel lblTotalBalance;

		private Literal litRefundOrderRemark;

		private HiddenField hidExpressCompanyName;

		private HiddenField hidShipOrderNumber;

		private HiddenField hidHiPOSTakeCode;

		private HiddenField hidIsPaymentStore;

		private HtmlGenericControl divInvoiceTitle;

		private HtmlGenericControl divTax;

		private HtmlGenericControl divInvoiceTaxpayerNumber;

		private OrderInfo order = null;

		private HtmlAnchor lkbtnApplyForRefund;

		private HtmlAnchor lkbtnUserRealNameVerify;

		private FightGroupStatusLabel litFightGroupStatusLabel;

		private HiddenField hidPreSaleId;

		private HiddenField hidIsPayDeposit;

		private FormatedTimeLabel litDepositPayDate;

		private Literal litFinalPamentPayDate;

		private Literal litGetGoodsRemark;

		private HtmlGenericControl divPickUpRemark;

		private HtmlInputHidden hidExpressStatus;

		private Literal litInvoiceType;

		private Literal litRegisterAddress;

		private Literal litRegisterTel;

		private Literal litOpenBank;

		private Literal litBankName;

		private Literal litReceiveName;

		private Literal litReceiveMobbile;

		private Literal litReceiveEmail;

		private Literal litReceiveRegionName;

		private Literal litReceiveAddress;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UserOrderDetails.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.orderId = base.GetParameter("orderId", false);
			this.litFightGroupStatusLabel = (FightGroupStatusLabel)this.FindControl("litFightGroupStatusLabel");
			this.litOrderId = (Literal)this.FindControl("litOrderId");
			this.lbltotalPrice = (FormatedMoneyLabel)this.FindControl("lbltotalPrice");
			this.litAddDate = (FormatedTimeLabel)this.FindControl("litAddDate");
			this.lblOrderStatus = (OrderStatusLabel)this.FindControl("lblOrderStatus");
			this.litCloseReason = (Literal)this.FindControl("litCloseReason");
			this.litRemark = (Literal)this.FindControl("litRemark");
			this.litShipTo = (Literal)this.FindControl("litShipTo");
			this.litRegion = (Literal)this.FindControl("litRegion");
			this.litAddress = (Literal)this.FindControl("litAddress");
			this.litZipcode = (Literal)this.FindControl("litZipcode");
			this.litEmail = (Literal)this.FindControl("litEmail");
			this.litPhone = (Literal)this.FindControl("litPhone");
			this.litTellPhone = (Literal)this.FindControl("litTellPhone");
			this.litShipToDate = (Literal)this.FindControl("litShipToDate");
			this.litPaymentType = (Literal)this.FindControl("litPaymentType");
			this.litModeName = (Literal)this.FindControl("litModeName");
			this.plOrderSended = (Panel)this.FindControl("plOrderSended");
			this.litRealModeName = (Literal)this.FindControl("litRealModeName");
			this.litShippNumber = (Literal)this.FindControl("litShippNumber");
			this.litDiscountName = (HyperLink)this.FindControl("litDiscountName");
			this.lblAdjustedDiscount = (FormatedMoneyLabel)this.FindControl("lblAdjustedDiscount");
			this.litFreeName = (HyperLink)this.FindControl("litFreeName");
			this.plExpress = (Panel)this.FindControl("plExpress");
			this.power = (HtmlAnchor)this.FindControl("power");
			this.litTakeCode = (Literal)this.FindControl("litTakeCode");
			this.litStoreInfo = (Literal)this.FindControl("litStoreInfo");
			this.litStoreName = (Literal)this.FindControl("litStoreName");
			this.litStoreTel = (Literal)this.FindControl("litStoreTel");
			this.divstoreinfo = (HtmlGenericControl)this.FindControl("divstoreinfo");
			this.divInvoiceTitle = (HtmlGenericControl)this.FindControl("divInvoiceTitle");
			this.divInvoiceTaxpayerNumber = (HtmlGenericControl)this.FindControl("divInvoiceTaxpayerNumber");
			this.divTax = (HtmlGenericControl)this.FindControl("divTax");
			this.listOrders = (Common_OrderManage_OrderItems)this.FindControl("Common_OrderManage_OrderItems");
			this.orderGifts = (Common_OrderManage_OrderGifts)this.FindControl("Common_OrderManage_OrderGifts");
			this.plOrderGift = (Panel)this.FindControl("plOrderGift");
			this.lblBundlingPrice = (Literal)this.FindControl("lblBundlingPrice");
			this.litPoints = (Literal)this.FindControl("litPoints");
			this.litSentTimesPointPromotion = (HyperLink)this.FindControl("litSentTimesPointPromotion");
			this.litWeight = (Literal)this.FindControl("litWeight");
			this.litFree = (Literal)this.FindControl("litFree");
			this.lblFreight = (FormatedMoneyLabel)this.FindControl("lblFreight");
			this.litCouponValue = (Literal)this.FindControl("litCouponValue");
			this.litPointMoney = (Literal)this.FindControl("litPointMoney");
			this.lblDiscount = (FormatedMoneyLabel)this.FindControl("lblDiscount");
			this.litTotalPrice = (FormatedMoneyLabel)this.FindControl("litTotalPrice");
			this.lblRefundTotal = (FormatedMoneyLabel)this.FindControl("lblRefundTotal");
			this.litAddOrderDate = (FormatedTimeLabel)this.FindControl("litAddOrderDate");
			this.litPayDate = (FormatedTimeLabel)this.FindControl("litPayDate");
			this.litPayBack = (FormatedTimeLabel)this.FindControl("litPayBack");
			this.litBackShip = (FormatedTimeLabel)this.FindControl("litBackShip");
			this.litShipDate = (FormatedTimeLabel)this.FindControl("litShipDate");
			this.litResultDate = (FormatedTimeLabel)this.FindControl("litResultDate");
			this.lkbtnConfirmOrder = (LinkButton)this.FindControl("lkbtnConfirmOrder");
			this.lkbtnCloseOrder = (LinkButton)this.FindControl("lkbtnCloseOrder");
			this.hidExpressCompanyName = (HiddenField)this.FindControl("hidExpressCompanyName");
			this.hidShipOrderNumber = (HiddenField)this.FindControl("hidShipOrderNumber");
			this.hidHiPOSTakeCode = (HiddenField)this.FindControl("hidHiPOSTakeCode");
			this.btnPay = (Button)this.FindControl("btnPay");
			this.lbRefundMoney = (Label)this.FindControl("lbRefundMoney");
			this.lbRefundMoney = (Label)this.FindControl("lbRefundMoney");
			this.lbCloseReason = (Label)this.FindControl("lbCloseReason");
			this.txtRemark = (TextBox)this.FindControl("txtRemark");
			this.txtReturnRemark = (TextBox)this.FindControl("txtReturnRemark");
			this.txtReplaceRemark = (TextBox)this.FindControl("txtReplaceRemark");
			this.dropRefundType = (RefundTypeDropDownList)this.FindControl("dropRefundType");
			this.dropReturnRefundType = (RefundTypeDropDownList)this.FindControl("dropReturnRefundType");
			this.dropPayType = (DropDownList)this.FindControl("dropPayType");
			this.plRefund = (Panel)this.FindControl("plRefund");
			this.lblTotalBalance = (FormatedMoneyLabel)this.FindControl("lblTotalBalance");
			this.litRefundOrderRemark = (Literal)this.FindControl("litRefundOrderRemark");
			this.litInvoiceTitle = (Literal)this.FindControl("litInvoiceTitle");
			this.litInvoiceTaxpayerNumber = (Literal)this.FindControl("litInvoiceTaxpayerNumber");
			this.litTax = (FormatedMoneyLabel)this.FindControl("litTax");
			this.hdorderId = (HtmlInputHidden)this.FindControl("hdorderId");
			this.lkbtnApplyForRefund = (HtmlAnchor)this.FindControl("lkbtnApplyForRefund");
			this.lkbtnUserRealNameVerify = (HtmlAnchor)this.FindControl("lkbtnUserRealNameVerify");
			this.hidPreSaleId = (HiddenField)this.FindControl("hidPreSaleId");
			this.hidIsPayDeposit = (HiddenField)this.FindControl("hidIsPayDeposit");
			this.hidIsPaymentStore = (HiddenField)this.FindControl("hidIsPaymentStore");
			this.litDepositPayDate = (FormatedTimeLabel)this.FindControl("litDepositPayDate");
			this.litFinalPamentPayDate = (Literal)this.FindControl("litFinalPamentPayDate");
			this.lblDeposit = (FormatedMoneyLabel)this.FindControl("lblDeposit");
			this.lblFinalPayment = (FormatedMoneyLabel)this.FindControl("lblFinalPayment");
			this.litGetGoodsRemark = (Literal)this.FindControl("litGetGoodsRemark");
			this.divPickUpRemark = (HtmlGenericControl)this.FindControl("divPickUpRemark");
			this.litBalanceAmount = (Literal)this.FindControl("litBalanceAmount");
			this.hidExpressStatus = (HtmlInputHidden)this.FindControl("hidExpressStatus");
			this.litInvoiceType = (Literal)this.FindControl("litInvoiceType");
			this.litRegisterAddress = (Literal)this.FindControl("litRegisterAddress");
			this.litRegisterTel = (Literal)this.FindControl("litRegisterTel");
			this.litOpenBank = (Literal)this.FindControl("litOpenBank");
			this.litBankName = (Literal)this.FindControl("litBankName");
			this.litReceiveName = (Literal)this.FindControl("litReceiveName");
			this.litReceiveMobbile = (Literal)this.FindControl("litReceiveMobbile");
			this.litReceiveEmail = (Literal)this.FindControl("litReceiveEmail");
			this.litReceiveRegionName = (Literal)this.FindControl("litReceiveRegionName");
			this.litReceiveAddress = (Literal)this.FindControl("litReceiveAddress");
			this.listOrders.ItemDataBound += this.orderItems_ItemDataBound;
			PageTitle.AddTitle("订单详细页", HiContext.Current.Context);
			this.order = TradeHelper.GetOrderInfo(this.orderId);
			this.divInvoiceTitle.Visible = (this.order.Tax > decimal.Zero && !string.IsNullOrEmpty(this.order.InvoiceTitle));
			this.divInvoiceTaxpayerNumber.Visible = (this.order.Tax > decimal.Zero && !string.IsNullOrEmpty(this.order.InvoiceTaxpayerNumber));
			this.divTax.Visible = (this.order.Tax > decimal.Zero);
			this.btnPay.Click += this.btnPay_Click;
			this.lkbtnConfirmOrder.Click += this.lkbtnConfirmOrder_Click;
			this.lkbtnCloseOrder.Click += this.lkbtnCloseOrder_Click;
			if (!this.Page.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.order.TakeCode))
				{
					this.hidHiPOSTakeCode.Value = Globals.HIPOSTAKECODEPREFIX + this.order.TakeCode;
				}
				if (this.order == null || this.order.UserId != HiContext.Current.UserId)
				{
					this.Page.Response.Redirect("/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("该订单不存在或者不属于当前用户的订单"));
				}
				else
				{
					this.BindOrderBase(this.order);
					this.BindOrderAddress(this.order);
					this.BindOrderItems(this.order);
					this.BindStoreInfo(this.order);
					if (!string.IsNullOrEmpty(this.order.InvoiceTitle))
					{
						UserInvoiceDataInfo userInvoiceDataInfo = this.order.InvoiceInfo;
						if (userInvoiceDataInfo == null)
						{
							userInvoiceDataInfo = new UserInvoiceDataInfo
							{
								InvoiceType = this.order.InvoiceType,
								InvoiceTaxpayerNumber = this.order.InvoiceTaxpayerNumber.ToNullString(),
								InvoiceTitle = this.order.InvoiceTitle.ToNullString()
							};
						}
						this.litInvoiceTaxpayerNumber.SetWhenIsNotNull(this.order.InvoiceTaxpayerNumber);
						this.litInvoiceTitle.SetWhenIsNotNull(this.order.InvoiceTitle);
						this.litInvoiceType.SetWhenIsNotNull(EnumDescription.GetEnumDescription((Enum)(object)this.order.InvoiceType, 0));
						this.litRegisterAddress.SetWhenIsNotNull(userInvoiceDataInfo.RegisterAddress.ToNullString());
						this.litRegisterTel.SetWhenIsNotNull(userInvoiceDataInfo.RegisterTel.ToNullString());
						this.litOpenBank.SetWhenIsNotNull(userInvoiceDataInfo.OpenBank.ToNullString());
						this.litBankName.SetWhenIsNotNull(userInvoiceDataInfo.BankAccount.ToNullString());
						this.litReceiveName.SetWhenIsNotNull(userInvoiceDataInfo.ReceiveName.ToNullString());
						if (this.order.InvoiceType != InvoiceType.Enterprise)
						{
							this.litReceiveMobbile.SetWhenIsNotNull(userInvoiceDataInfo.ReceivePhone.ToNullString());
							if (this.order.InvoiceType != InvoiceType.VATInvoice)
							{
								this.litReceiveEmail.SetWhenIsNotNull(userInvoiceDataInfo.ReceiveEmail.ToNullString());
							}
						}
						this.litReceiveRegionName.SetWhenIsNotNull(userInvoiceDataInfo.ReceiveRegionName.ToNullString());
						this.litReceiveAddress.SetWhenIsNotNull(userInvoiceDataInfo.ReceiveAddress.ToNullString());
					}
					if (this.order.FightGroupId > 0)
					{
						FightGroupInfo fightGroup = VShopHelper.GetFightGroup(this.order.FightGroupId);
						this.lkbtnApplyForRefund.Visible = (fightGroup.Status != 0 && this.order.OrderStatus == OrderStatus.BuyerAlreadyPaid && this.order.GetPayTotal() > decimal.Zero);
						this.litFightGroupStatusLabel.Order = this.order;
					}
				}
			}
		}

		public bool IsOnlyOneShop(OrderInfo order)
		{
			if (order != null)
			{
				return order.LineItems.Count > 1;
			}
			return false;
		}

		private void btnPay_Click(object sender, EventArgs e)
		{
			string value = this.hdorderId.Value;
			int modeId = 0;
			int.TryParse(this.dropPayType.SelectedValue, out modeId);
			PaymentModeInfo paymentMode = TradeHelper.GetPaymentMode(modeId);
			if (paymentMode != null)
			{
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(value);
				orderInfo.PaymentTypeId = paymentMode.ModeId;
				orderInfo.PaymentType = paymentMode.Name;
				orderInfo.Gateway = paymentMode.Gateway;
				TradeHelper.UpdateOrderPaymentType(orderInfo);
			}
			this.Page.Response.Redirect(base.GetRouteUrl("sendPayment", new
			{
				orderId = value
			}));
		}

		private void lkbtnConfirmOrder_Click(object sender, EventArgs e)
		{
			OrderInfo orderInfo = TradeHelper.GetOrderInfo(this.orderId);
			if (TradeHelper.ConfirmOrderFinish(orderInfo))
			{
				this.ShowMessage("成功的完成了该订单", true, "", 1);
				HiContext.Current.Context.Response.Redirect("OrderDetails.aspx?OrderId=" + this.orderId);
			}
			else
			{
				this.ShowMessage("完成订单失败，订单状态错误或者订单商品有退款、退货或者换货正在进行中!", false, "", 1);
			}
		}

		private void lkbtnCloseOrder_Click(object sender, EventArgs e)
		{
			if (TradeHelper.CloseOrder(this.orderId, "会员主动关闭"))
			{
				this.ShowMessage("成功的关闭了该订单", true, "", 1);
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(this.orderId);
				this.BindOrderBase(orderInfo);
			}
			else
			{
				this.ShowMessage("关闭订单失败!", false, "", 1);
			}
		}

		private bool CanRefundBalance()
		{
			if (Convert.ToInt32(this.dropRefundType.SelectedValue) != 1)
			{
				return true;
			}
			return HiContext.Current.User.IsOpenBalance;
		}

		private void BindOrderBase(OrderInfo order)
		{
			this.litBalanceAmount.Text = "-" + order.BalanceAmount.F2ToString("f2");
			this.hidPreSaleId.Value = order.PreSaleId.ToString();
			this.litOrderId.Text = order.OrderId;
			this.lbltotalPrice.Money = order.GetAmount(false);
			this.litAddDate.Time = order.OrderDate;
			this.litAddOrderDate.Time = order.OrderDate;
			if (order.PreSaleId > 0)
			{
				this.lblDeposit.Money = order.Deposit;
				this.lblFinalPayment.Money = order.FinalPayment;
				if (order.DepositDate.HasValue)
				{
					this.hidIsPayDeposit.Value = "1";
					this.litDepositPayDate.Time = order.DepositDate;
					ProductPreSaleInfo productPreSaleInfo = ProductPreSaleHelper.GetProductPreSaleInfo(order.PreSaleId);
					int num;
					if (!(order.PayDate == DateTime.MinValue))
					{
						DateTime payDate = order.PayDate;
						num = 0;
					}
					else
					{
						num = 1;
					}
					DateTime dateTime;
					if (num != 0)
					{
						if (order.OrderStatus != OrderStatus.Closed)
						{
							if (productPreSaleInfo.PaymentStartDate > DateTime.Now)
							{
								Literal literal = this.litFinalPamentPayDate;
								dateTime = productPreSaleInfo.PaymentStartDate;
								literal.Text = dateTime.ToString("yyyy.MM.dd") + "后开始尾款支付";
							}
							else
							{
								Literal literal2 = this.litFinalPamentPayDate;
								dateTime = productPreSaleInfo.PaymentEndDate;
								literal2.Text = dateTime.ToString("yyyy.MM.dd") + "后终止交易";
							}
						}
					}
					else
					{
						Literal literal3 = this.litFinalPamentPayDate;
						dateTime = order.PayDate;
						literal3.Text = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
					}
				}
			}
			else
			{
				this.litPayDate.Time = order.PayDate;
			}
			this.litShipDate.Time = order.ShippingDate;
			this.litResultDate.Time = order.FinishDate;
			this.lblOrderStatus.OrderStatusCode = order.OrderStatus;
			this.lblOrderStatus.OrderItemStatus = order.ItemStatus;
			this.lblOrderStatus.ShipmentModelId = order.ShippingModeId;
			this.lblOrderStatus.IsConfirm = order.IsConfirm;
			this.lblOrderStatus.Gateway = order.Gateway;
			this.lblOrderStatus.PaymentTypeId = order.PaymentTypeId;
			this.lblOrderStatus.PreSaleId = order.PreSaleId;
			this.lblOrderStatus.DepositDate = order.DepositDate;
			if (order.OrderStatus == OrderStatus.Closed)
			{
				this.lbCloseReason.Visible = true;
				this.litCloseReason.Text = order.CloseReason;
			}
			if (order.RefundAmount > decimal.Zero)
			{
				this.lbRefundMoney.Visible = true;
				this.lblRefundTotal.Money = order.RefundAmount;
			}
			else
			{
				this.lbRefundMoney.Visible = false;
			}
			this.litRemark.Text = order.Remark;
			this.JudgeOrderStatus(order);
		}

		private void BindOrderAddress(OrderInfo order)
		{
			this.litShipTo.Text = order.ShipTo;
			this.litRegion.Text = order.ShippingRegion;
			this.litAddress.Text = order.Address;
			this.litEmail.Text = order.EmailAddress;
			this.litTellPhone.Text = order.TelPhone;
			this.litPhone.Text = order.CellPhone;
			this.litShipToDate.Text = order.ShipToDate;
			this.litPaymentType.Text = order.PaymentType;
			this.litModeName.Text = order.ModeName;
			if ((order.OrderStatus == OrderStatus.SellerAlreadySent || order.OrderStatus == OrderStatus.Finished) && order.ShippingModeId != -2)
			{
				this.plOrderSended.Visible = true;
				this.litShippNumber.Text = order.ShipOrderNumber;
				if (!string.IsNullOrEmpty(order.ExpressCompanyAbb) && !string.IsNullOrEmpty(order.ShipOrderNumber))
				{
					this.litRealModeName.Text = order.ExpressCompanyName;
				}
				else
				{
					this.litRealModeName.Text = "自有物流";
				}
			}
			if (order.OrderStatus != OrderStatus.SellerAlreadySent && order.OrderStatus != OrderStatus.Finished && string.IsNullOrEmpty(order.ExpressCompanyAbb))
			{
				if (this.plExpress != null)
				{
					this.plExpress.Visible = true;
				}
				if (this.power != null)
				{
					this.power.Visible = true;
				}
			}
			if (HiContext.Current.SiteSettings.IsOpenCertification && order.IDStatus == 0 && order.IsincludeCrossBorderGoods)
			{
				this.lkbtnUserRealNameVerify.Visible = true;
				this.lkbtnUserRealNameVerify.Attributes.Add("OrderId", order.OrderId);
			}
			else
			{
				this.lkbtnUserRealNameVerify.Visible = false;
			}
		}

		private void BindOrderItems(OrderInfo order)
		{
			if (order.LineItems.Count > 0)
			{
				var dataSource = (from i in (from i in order.LineItems.Values
				select new
				{
					i.SupplierId,
					i.SupplierName
				}).Distinct()
				orderby i.SupplierId
				select i).ToList();
				this.listOrders.DataSource = dataSource;
				this.listOrders.orderInfo = order;
				this.listOrders.DataBind();
			}
			else
			{
				this.FindControl("goodstitle").Visible = false;
				this.FindControl("goodslst").Visible = false;
			}
			if (order.Gifts.Count > 0)
			{
				if (order.UserAwardRecordsId > 0)
				{
					foreach (OrderGiftInfo gift in order.Gifts)
					{
						gift.PromoteType = -1;
					}
				}
				this.plOrderGift.Visible = true;
				this.orderGifts.DataSource = order.Gifts;
				this.orderGifts.DataBind();
			}
			this.litWeight.Text = order.Weight.F2ToString("f2");
			this.lblFreight.Money = order.AdjustedFreight;
			if (order.IsFreightFree)
			{
				this.litFreeName.Text = order.FreightFreePromotionName;
				this.litFreeName.NavigateUrl = base.GetRouteUrl("FavourableDetails", new
				{
					activityId = order.FreightFreePromotionId
				});
			}
			this.litInvoiceTitle.Text = order.InvoiceTitle;
			this.litInvoiceTaxpayerNumber.Text = order.InvoiceTaxpayerNumber;
			this.litTax.Money = order.Tax;
			this.lblAdjustedDiscount.Money = order.AdjustedDiscount;
			this.litCouponValue.Text = ((order.CouponName + " -" + Globals.FormatMoney(order.CouponValue)) ?? "");
			this.litPointMoney.Text = ((!order.DeductionMoney.HasValue) ? "-0.00" : (" -" + Math.Round(order.DeductionMoney.Value, 2).ToString()));
			this.lblDiscount.Money = order.ReducedPromotionAmount;
			if (order.IsReduced)
			{
				this.litDiscountName.Text = order.ReducedPromotionName;
				this.litDiscountName.NavigateUrl = base.GetRouteUrl("FavourableDetails", new
				{
					activityId = order.ReducedPromotionId
				});
			}
			this.litPoints.Text = order.Points.ToString(CultureInfo.InvariantCulture);
			if (order.IsSendTimesPoint)
			{
				this.litSentTimesPointPromotion.Text = order.SentTimesPointPromotionName;
				this.litSentTimesPointPromotion.NavigateUrl = base.GetRouteUrl("FavourableDetails", new
				{
					activityId = order.SentTimesPointPromotionId
				});
			}
			this.litTotalPrice.Money = order.GetPayTotal();
		}

		protected void orderItems_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				LineItemStatus lineItemStatus = (LineItemStatus)DataBinder.Eval(e.Item.DataItem, "Status");
				string text = DataBinder.Eval(e.Item.DataItem, "SkuId").ToString();
				OrderStatus orderStatus = this.order.OrderStatus;
				DateTime finishDate = this.order.FinishDate;
				string gateway = this.order.Gateway;
				LineItemInfo lineItemInfo = this.order.LineItems[text];
				HtmlAnchor htmlAnchor = (HtmlAnchor)e.Item.FindControl("lkbtnAfterSalesApply");
				Label label = (Label)e.Item.FindControl("Logistics");
				HtmlAnchor htmlAnchor2 = (HtmlAnchor)e.Item.FindControl("lkbtnViewMessage");
				htmlAnchor.Attributes.Add("OrderId", this.order.OrderId);
				htmlAnchor.Attributes.Add("SkuId", text);
				htmlAnchor.Attributes.Add("GateWay", gateway);
				ReplaceInfo replaceInfo = lineItemInfo.ReplaceInfo;
				ReturnInfo returnInfo = lineItemInfo.ReturnInfo;
				string text2 = (string)DataBinder.Eval(e.Item.DataItem, "StatusText");
				if (lineItemStatus == LineItemStatus.Normal)
				{
					text2 = "";
				}
				else if (returnInfo != null)
				{
					text2 = ((returnInfo.AfterSaleType != AfterSaleTypes.OnlyRefund) ? EnumDescription.GetEnumDescription((Enum)(object)returnInfo.HandleStatus, 1) : EnumDescription.GetEnumDescription((Enum)(object)returnInfo.HandleStatus, 3));
				}
				else if (replaceInfo != null)
				{
					text2 = EnumDescription.GetEnumDescription((Enum)(object)replaceInfo.HandleStatus, 1);
				}
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				HtmlAnchor htmlAnchor3 = htmlAnchor;
				int visible;
				switch (orderStatus)
				{
				case OrderStatus.Finished:
					visible = ((!this.order.IsServiceOver) ? 1 : 0);
					break;
				default:
					visible = 0;
					break;
				case OrderStatus.SellerAlreadySent:
					visible = 1;
					break;
				}
				htmlAnchor3.Visible = ((byte)visible != 0);
				if (htmlAnchor.Visible)
				{
					htmlAnchor.Visible = (this.order.LineItems.Count >= 1 && (returnInfo == null || returnInfo.HandleStatus == ReturnStatus.Refused) && (replaceInfo == null || replaceInfo.HandleStatus == ReplaceStatus.Refused));
				}
				Literal literal = (Literal)e.Item.FindControl("litStatusText");
				if (literal != null && lineItemStatus != 0)
				{
					if (returnInfo != null && (lineItemStatus == LineItemStatus.DeliveryForReturn || lineItemStatus == LineItemStatus.GetGoodsForReturn || lineItemStatus == LineItemStatus.MerchantsAgreedForReturn || lineItemStatus == LineItemStatus.ReturnApplied || lineItemStatus == LineItemStatus.Returned || lineItemStatus == LineItemStatus.ReturnsRefused))
					{
						literal.Text = "<a href=\"/User/UserReturnsApplyDetails?ReturnsId=" + returnInfo.ReturnId + "\">" + text2 + "</a>";
					}
					else if (replaceInfo != null)
					{
						literal.Text = "<a href=\"/User/UserReplaceApplyDetails?ReplaceId=" + replaceInfo.ReplaceId + "\">" + text2 + "</a>";
					}
				}
				int num;
				if (replaceInfo != null && (replaceInfo.HandleStatus == ReplaceStatus.Replaced || replaceInfo.HandleStatus == ReplaceStatus.UserDelivery || replaceInfo.HandleStatus == ReplaceStatus.MerchantsDelivery))
				{
					label.Attributes.Add("action", "replace");
					AttributeCollection attributes = label.Attributes;
					num = replaceInfo.ReplaceId;
					attributes.Add("ReplaceId", num.ToString());
					label.Visible = true;
				}
				if (returnInfo != null && returnInfo.AfterSaleType == AfterSaleTypes.ReturnAndRefund && (returnInfo.HandleStatus == ReturnStatus.Deliverying || returnInfo.HandleStatus == ReturnStatus.Returned || returnInfo.HandleStatus == ReturnStatus.GetGoods))
				{
					label.Attributes.Add("action", "return");
					AttributeCollection attributes2 = label.Attributes;
					num = returnInfo.ReturnId;
					attributes2.Add("returnId", num.ToString());
					label.Visible = true;
				}
			}
		}

		private void BindStoreInfo(OrderInfo order)
		{
			if (order.ShippingModeId == -2)
			{
				this.divstoreinfo.Visible = true;
				if (order.Gateway.ToLower() == "hishop.plugins.payment.payonstore")
				{
					this.hidIsPaymentStore.Value = "1";
				}
			}
			this.litTakeCode.SetWhenIsNotNull(order.TakeCode);
			StoresInfo storeById = DepotHelper.GetStoreById(order.StoreId);
			if (storeById != null)
			{
				this.litStoreName.Text = storeById.StoreName;
				this.litStoreInfo.Text = RegionHelper.GetFullRegion(storeById.RegionId, " ", true, 0) + " " + storeById.Address;
				this.litStoreTel.Text = storeById.Tel;
			}
		}

		private int GetSkuQuantity(string SkuId)
		{
			if (this.order == null)
			{
				return 0;
			}
			if (!this.order.LineItems.ContainsKey(SkuId))
			{
				return 0;
			}
			return this.order.LineItems[SkuId].Quantity;
		}

		private bool CanReturnBalance()
		{
			if (Convert.ToInt32(this.dropReturnRefundType.SelectedValue) != 1)
			{
				return true;
			}
			return HiContext.Current.User.IsOpenBalance;
		}

		private OrderQuery GetOrderQuery()
		{
			OrderQuery orderQuery = new OrderQuery();
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["orderId"]))
			{
				orderQuery.OrderId = this.Page.Request.QueryString["orderId"];
			}
			return orderQuery;
		}

		private void JudgeOrderStatus(OrderInfo order)
		{
			this.lkbtnConfirmOrder.Visible = (order.OrderStatus == OrderStatus.SellerAlreadySent && order.ItemStatus == OrderItemStatus.Nomarl);
			if (order.PreSaleId > 0)
			{
				if (order.OrderStatus == OrderStatus.WaitBuyerPay && order.ItemStatus == OrderItemStatus.Nomarl && !order.DepositDate.HasValue)
				{
					this.lkbtnCloseOrder.Visible = true;
				}
				else
				{
					this.lkbtnCloseOrder.Visible = false;
				}
			}
			else
			{
				this.lkbtnCloseOrder.Visible = (order.OrderStatus == OrderStatus.WaitBuyerPay && order.ItemStatus == OrderItemStatus.Nomarl);
			}
			RefundInfo refundInfo = TradeHelper.GetRefundInfo(order.OrderId);
			ReturnInfo returnInfo = TradeHelper.GetReturnInfo(order.OrderId, "");
			ReplaceInfo replaceInfo = TradeHelper.GetReplaceInfo(order.OrderId, "");
			DateTime finishDate = order.FinishDate;
			OrderStatus orderStatus = order.OrderStatus;
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			HtmlAnchor htmlAnchor = this.lkbtnApplyForRefund;
			int visible;
			if (order.GetTotal(false) > decimal.Zero && !TradeHelper.OrderHasRefundOrReturning(order))
			{
				if ((refundInfo == null || refundInfo.HandleStatus == RefundStatus.Refused) && returnInfo == null && replaceInfo == null)
				{
					visible = ((order.OrderStatus == OrderStatus.BuyerAlreadyPaid && order.ItemStatus == OrderItemStatus.Nomarl) ? 1 : 0);
					goto IL_0136;
				}
				visible = 0;
			}
			else
			{
				visible = 0;
			}
			goto IL_0136;
			IL_0136:
			htmlAnchor.Visible = ((byte)visible != 0);
			this.lkbtnApplyForRefund.Attributes.Add("orderId", this.orderId);
			if (!masterSettings.OpenMultStore && masterSettings.IsOpenPickeupInStore && order.SupplierId == 0 && order.ShippingModeId == -2)
			{
				this.litGetGoodsRemark.Text = masterSettings.PickeupInStoreRemark;
			}
			else
			{
				this.divPickUpRemark.Visible = false;
			}
		}
	}
}
