using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Shopping;
using Hidistro.SaleSystem.Store;
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
	public class WAPMemberOrderDetails : WAPMemberTemplatedWebControl
	{
		private string orderId;

		private HtmlGenericControl liFullReduction;

		private Literal litShipTo;

		private Literal litPhone;

		private Literal litAddress;

		private Literal litShipToDate;

		private Literal litBuildPrice;

		private Literal litDisCountPrice;

		private Literal litTax;

		private Literal litPointsPrice;

		private Literal litOrderId;

		private Literal litTakeCode;

		private Literal litCounponPrice;

		private Literal litStoreInfo;

		private Literal litStoreName;

		private Literal litStoreTel;

		private Literal litCloseReason;

		private Literal litOrderDate;

		private Literal litRemark;

		private OrderStatusLabel litOrderStatus;

		private FightGroupStatusLabel litFightGroupStatusLabel;

		private Literal litTotalPrice;

		private Literal litFreight;

		private Literal litFreight2;

		private Literal litInvoiceTitle;

		private Literal litInvoiceTaxpayerNumber;

		private Literal litFullCapacityReduction;

		private Literal litPaymentMode;

		private Literal litPayTime;

		private HtmlInputHidden orderStatus;

		private HtmlInputHidden hidOrderId;

		private HtmlInputHidden hidExpressStatus;

		private HtmlGenericControl divstoreinfo;

		private HtmlGenericControl liInvoiceTitle;

		private HtmlGenericControl liInvoiceTaxpayerNumber;

		private HtmlGenericControl liCounponPrice;

		private HtmlGenericControl liDiscountPrice;

		private HtmlGenericControl liPointPrice;

		private HtmlGenericControl liTax;

		private HtmlGenericControl giftFreight;

		private HtmlGenericControl liBalanceAmount;

		private HtmlGenericControl divStoreAddress;

		private HtmlGenericControl divLogists;

		private HtmlGenericControl divShipAddress;

		private HtmlControl dvClose;

		private HtmlControl btnOrderClose;

		private HtmlAnchor lookupTrans;

		private HtmlAnchor ensureRecieved;

		private HtmlAnchor lookupQRCode;

		private HtmlAnchor btnOrderRefund;

		private HtmlAnchor btnOrderCancel;

		private HtmlAnchor btnToPay;

		private HtmlAnchor lnkProductReview;

		private HtmlGenericControl spandemo;

		private HtmlInputHidden hidpresaleStaut;

		private Literal litDepositDate;

		private Literal litFinalDate;

		private Literal litDeposit;

		private Literal litFinal;

		private Common_MemberOrderProducts rptOrderProducts;

		private WapTemplatedRepeater rptCartGifts;

		private WapTemplatedRepeater rptPromotionGifts;

		private HtmlGenericControl divRefund;

		private HtmlGenericControl divRefundPoint;

		private HtmlGenericControl divGifts;

		private HtmlGenericControl divProducts;

		private Literal litRefundMoney;

		private Literal litRefundPoint;

		private OrderInfo order;

		private Common_WAPPaymentTypeSelect paymenttypeselect;

		private Literal litGetGoodsRemark;

		private HtmlGenericControl divPickUpRemark;

		private HtmlInputHidden hidHasTradePassword;

		private Literal litExpressInfo;

		private Literal litBalanceAmount;

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
				this.SkinName = "Skin-VMemberOrderDetails.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.litExpressInfo = (Literal)this.FindControl("litExpressInfo");
			this.hidHasTradePassword = (HtmlInputHidden)this.FindControl("hidHasTradePassword");
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			this.orderId = this.Page.Request.QueryString["orderId"];
			this.spandemo = (HtmlGenericControl)this.FindControl("spandemo");
			this.litBalanceAmount = (Literal)this.FindControl("litBalanceAmount");
			this.liBalanceAmount = (HtmlGenericControl)this.FindControl("liBalanceAmount");
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
			this.spandemo.Visible = masterSettings.IsDemoSite;
			this.LoadAllControls();
			this.order = ShoppingProcessor.GetOrderInfo(this.orderId);
			if (this.order == null || this.order.UserId != HiContext.Current.UserId)
			{
				base.GotoResourceNotFound("此订单已不存在");
			}
			if (this.order.ParentOrderId != "0" && this.order.ParentOrderId != "-1" && this.order.OrderStatus == OrderStatus.WaitBuyerPay)
			{
				this.order = ShoppingProcessor.GetOrderInfo(this.order.ParentOrderId);
				this.orderId = this.order.OrderId;
			}
			if (this.hidHasTradePassword != null && HiContext.Current.User.UserId > 0)
			{
				this.hidHasTradePassword.Value = (string.IsNullOrWhiteSpace(HiContext.Current.User.TradePassword) ? "0" : "1");
			}
			else
			{
				this.hidHasTradePassword.Value = "0";
			}
			if ((this.order.OrderStatus == OrderStatus.SellerAlreadySent || this.order.OrderStatus == OrderStatus.Finished) && this.order.ShippingModeId != -2)
			{
				if (!string.IsNullOrEmpty(this.order.ExpressCompanyAbb) && !string.IsNullOrEmpty(this.order.ShipOrderNumber))
				{
					this.hidExpressStatus.Value = "2";
				}
				else
				{
					this.hidExpressStatus.Value = "1";
				}
			}
			this.BindOrderInfo();
			if (!masterSettings.OpenMultStore && masterSettings.IsOpenPickeupInStore && this.order.SupplierId == 0 && this.order.ShippingModeId == -2)
			{
				this.litGetGoodsRemark.Text = masterSettings.PickeupInStoreRemark;
				this.divStoreAddress.Visible = false;
			}
			else
			{
				this.divPickUpRemark.Visible = false;
			}
			if (!string.IsNullOrEmpty(this.order.InvoiceTitle))
			{
				UserInvoiceDataInfo userInvoiceDataInfo = this.order.InvoiceInfo;
				if (userInvoiceDataInfo == null)
				{
					userInvoiceDataInfo = new UserInvoiceDataInfo
					{
						InvoiceType = this.order.InvoiceType,
						InvoiceTaxpayerNumber = this.order.InvoiceTaxpayerNumber,
						InvoiceTitle = this.order.InvoiceTitle
					};
				}
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
			this.paymenttypeselect.ClientType = base.ClientType;
			if (this.order.CountDownBuyId > 0)
			{
				this.paymenttypeselect.OrderSalesPromotion = Common_WAPPaymentTypeSelect.EnumOrderSalesPromotion.CountDownBuy;
			}
			this.paymenttypeselect.IsFireGroup = (this.order.FightGroupId > 0);
			this.paymenttypeselect.IsServiceProduct = (this.order.OrderType == OrderType.ServiceOrder);
			this.litFightGroupStatusLabel.Order = this.order;
			this.SetOperatorsStatus();
			this.BindStoreInfo();
			this.rptOrderProducts = (Common_MemberOrderProducts)this.FindControl("Common_MemberOrderProducts");
			this.rptOrderProducts.ItemDataBound += this.rptOrderProducts_ItemDataBound;
			var dataSource = (from i in (from i in this.order.LineItems.Values
			select new
			{
				i.SupplierId,
				i.SupplierName
			}).Distinct()
			orderby i.SupplierId
			select i).ToList();
			this.rptOrderProducts.DataSource = dataSource;
			this.rptOrderProducts.orderInfo = this.order;
			this.rptOrderProducts.DataBind();
			IList<OrderGiftInfo> gifts = this.order.Gifts;
			if (gifts.Count() > 0)
			{
				this.rptCartGifts.DataSource = gifts;
				this.rptCartGifts.DataBind();
			}
			else
			{
				this.divGifts.Visible = false;
			}
			this.divProducts.Visible = (this.order.LineItems.Count > 0);
			this.rptPromotionGifts.DataSource = from a in this.order.Gifts
			where a.PromoteType > 0
			select a;
			this.rptPromotionGifts.DataBind();
			this.litExpressInfo.Text = this.order.ExpressCompanyName + ":" + this.order.ShipOrderNumber;
			PageTitle.AddSiteNameTitle("订单详情");
		}

		private void SetOperatorsStatus()
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (this.order.AdjustedDiscount == decimal.Zero)
			{
				this.liDiscountPrice.Visible = false;
			}
			else
			{
				this.litDisCountPrice.SetWhenIsNotNull(this.order.AdjustedDiscount.F2ToString("f2"));
			}
			if (this.order.CouponValue > decimal.Zero)
			{
				this.litCounponPrice.SetWhenIsNotNull("-" + this.order.CouponValue.F2ToString("f2"));
			}
			else
			{
				this.liCounponPrice.Visible = false;
			}
			int num;
			if (this.order.DeductionMoney.HasValue)
			{
				decimal? deductionMoney = this.order.DeductionMoney;
				num = ((deductionMoney.GetValueOrDefault() > default(decimal) && deductionMoney.HasValue) ? 1 : 0);
			}
			else
			{
				num = 0;
			}
			if (num != 0)
			{
				this.litPointsPrice.Text = "-" + this.order.DeductionMoney.Value.F2ToString("f2");
			}
			else
			{
				this.liPointPrice.Visible = false;
			}
			if (this.order.Tax > decimal.Zero)
			{
				this.litTax.SetWhenIsNotNull(this.order.Tax.F2ToString("f2"));
			}
			else
			{
				this.liTax.Visible = false;
			}
			if (!string.IsNullOrEmpty(this.order.InvoiceTitle.Trim()))
			{
				this.liInvoiceTitle.Visible = true;
				this.litInvoiceTitle.SetWhenIsNotNull(this.order.InvoiceTitle);
			}
			if (!string.IsNullOrWhiteSpace(this.order.InvoiceTaxpayerNumber))
			{
				this.liInvoiceTaxpayerNumber.Visible = true;
				this.litInvoiceTaxpayerNumber.SetWhenIsNotNull(this.order.InvoiceTaxpayerNumber);
			}
			if (this.order.RefundAmount > decimal.Zero)
			{
				this.divRefund.Visible = true;
				this.litRefundMoney.Text = this.order.RefundAmount.F2ToString("f2");
				int sumRefundPoint = TradeHelper.GetSumRefundPoint(this.order.OrderId);
				if (sumRefundPoint > 0)
				{
					this.divRefundPoint.Visible = true;
					this.litRefundPoint.Text = sumRefundPoint.ToString();
				}
				else
				{
					this.divRefundPoint.Visible = false;
				}
			}
			else
			{
				this.divRefund.Visible = false;
				this.divRefundPoint.Visible = false;
			}
			if (this.order.OrderStatus == OrderStatus.Closed)
			{
				this.dvClose.Visible = true;
				this.litCloseReason.SetWhenIsNotNull(this.order.CloseReason);
			}
			else
			{
				this.dvClose.Visible = false;
			}
			if (this.btnOrderClose != null && this.order.OrderStatus == OrderStatus.WaitBuyerPay)
			{
				this.btnOrderClose.Visible = true;
			}
			if (this.btnOrderRefund != null)
			{
				this.btnOrderRefund.HRef = "ApplyRefund.aspx?OrderId=" + this.order.OrderId;
				this.btnOrderRefund.Visible = (this.order.OrderStatus == OrderStatus.BuyerAlreadyPaid && this.order.ItemStatus == OrderItemStatus.Nomarl && this.order.LineItems.Count != 0 && this.order.GetTotal(true) > decimal.Zero);
				if (this.btnOrderRefund.Visible && this.order.FightGroupId > 0)
				{
					FightGroupInfo fightGroup = VShopHelper.GetFightGroup(this.order.FightGroupId);
					if (fightGroup != null)
					{
						this.btnOrderRefund.Visible = (fightGroup.Status != FightGroupStatus.FightGroupIn);
					}
				}
			}
			if (!string.IsNullOrEmpty(masterSettings.HiPOSAppId) && !string.IsNullOrEmpty(this.order.TakeCode))
			{
				this.lookupQRCode.HRef = "ViewQRCode.aspx?orderId=" + this.order.OrderId;
			}
		}

		private void BindOrderInfo()
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (this.order.BalanceAmount > decimal.Zero)
			{
				this.litBalanceAmount.Text = "-" + this.order.BalanceAmount.F2ToString("f2");
			}
			else
			{
				this.liBalanceAmount.Visible = false;
			}
			DateTime? nullable;
			DateTime dateTime;
			if (this.order.PreSaleId > 0)
			{
				this.litDeposit.Text = this.order.Deposit.F2ToString("f2");
				this.litFinal.Text = this.order.FinalPayment.F2ToString("f2");
				nullable = this.order.DepositDate;
				if (nullable.HasValue)
				{
					Literal literal = this.litDepositDate;
					nullable = this.order.DepositDate;
					literal.Text = nullable.ToString();
				}
				DateTime payDate = this.order.PayDate;
				if (this.order.PayDate != DateTime.MinValue)
				{
					Literal literal2 = this.litFinalDate;
					dateTime = this.order.PayDate;
					literal2.Text = dateTime.ToString();
				}
			}
			if (this.order.ShippingModeId == -2 && this.order.StoreId > 0)
			{
				this.divStoreAddress.Visible = true;
				this.divLogists.Visible = false;
				this.divShipAddress.Visible = false;
			}
			else
			{
				this.divStoreAddress.Visible = false;
				this.divLogists.Visible = (this.order.OrderStatus == OrderStatus.SellerAlreadySent || this.order.OrderStatus == OrderStatus.Finished);
				this.divShipAddress.Visible = true;
			}
			if (this.order.OrderStatus == OrderStatus.SellerAlreadySent && this.order.ItemStatus == OrderItemStatus.Nomarl)
			{
				this.ensureRecieved.Visible = true;
			}
			else
			{
				this.ensureRecieved.Visible = false;
			}
			if ((this.order.OrderStatus == OrderStatus.SellerAlreadySent || this.order.OrderStatus == OrderStatus.Finished) && !string.IsNullOrEmpty(this.order.ExpressCompanyAbb) && !string.IsNullOrEmpty(this.order.ShipOrderNumber))
			{
				this.lookupTrans.Visible = true;
				this.lookupTrans.HRef = "MyLogistics.aspx?orderId=" + this.order.OrderId;
			}
			else
			{
				this.lookupTrans.Visible = false;
			}
			if (string.IsNullOrEmpty(this.order.TakeCode) || string.IsNullOrEmpty(masterSettings.HiPOSAppId) || this.order.OrderStatus == OrderStatus.Finished || this.order.OrderStatus == OrderStatus.Closed)
			{
				this.lookupQRCode.Visible = false;
			}
			ProductPreSaleInfo productPreSaleInfo = null;
			int paymentTypeId;
			if (this.order.OrderStatus == OrderStatus.WaitBuyerPay && this.order.Gateway != EnumDescription.GetEnumDescription((Enum)(object)EnumPaymentType.CashOnDelivery, 1) && this.order.PaymentTypeId != -3)
			{
				this.btnToPay.Attributes.Add("IsServiceOrder", (this.order.OrderType == OrderType.ServiceOrder).ToString().ToLower());
				if (this.order.PreSaleId > 0)
				{
					productPreSaleInfo = ProductPreSaleHelper.GetProductPreSaleInfo(this.order.PreSaleId);
					nullable = this.order.DepositDate;
					if (!nullable.HasValue)
					{
						if (productPreSaleInfo.PreSaleEndDate > DateTime.Now)
						{
							this.btnToPay.Visible = true;
							AttributeCollection attributes = this.btnToPay.Attributes;
							paymentTypeId = this.order.PaymentTypeId;
							attributes.Add("PaymentTypeId", paymentTypeId.ToString());
							this.btnToPay.Attributes.Add("OrderId", this.orderId);
							this.btnToPay.Attributes.Add("orderTotal", (this.order.Deposit - this.order.BalanceAmount).F2ToString("f2"));
						}
					}
					else if (!(productPreSaleInfo.PaymentStartDate > DateTime.Now) && !(productPreSaleInfo.PaymentEndDate < DateTime.Now))
					{
						this.btnToPay.Visible = true;
						AttributeCollection attributes2 = this.btnToPay.Attributes;
						paymentTypeId = this.order.PaymentTypeId;
						attributes2.Add("PaymentTypeId", paymentTypeId.ToString());
						this.btnToPay.Attributes.Add("OrderId", this.orderId);
						this.btnToPay.Attributes.Add("orderTotal", this.order.FinalPayment.F2ToString("f2"));
					}
				}
				else
				{
					AttributeCollection attributes3 = this.btnToPay.Attributes;
					paymentTypeId = this.order.PaymentTypeId;
					attributes3.Add("PaymentTypeId", paymentTypeId.ToString());
					this.btnToPay.Visible = true;
					this.btnToPay.Attributes.Add("OrderId", this.orderId);
					this.btnToPay.Attributes.Add("orderTotal", this.order.GetTotal(true).F2ToString("f2"));
					if (HiContext.Current.SiteSettings.OpenMultStore && this.order.StoreId > 0 && !SettingsManager.GetMasterSettings().Store_IsOrderInClosingTime)
					{
						StoresInfo storeById = StoresHelper.GetStoreById(this.order.StoreId);
						dateTime = DateTime.Now;
						string str = dateTime.ToString("yyyy-MM-dd");
						dateTime = storeById.OpenStartDate;
						nullable = (str + " " + dateTime.ToString("HH:mm")).ToDateTime();
						DateTime value = nullable.Value;
						dateTime = DateTime.Now;
						string str2 = dateTime.ToString("yyyy-MM-dd");
						dateTime = storeById.OpenEndDate;
						nullable = (str2 + " " + dateTime.ToString("HH:mm")).ToDateTime();
						DateTime dateTime2 = nullable.Value;
						if (dateTime2 <= value)
						{
							dateTime2 = dateTime2.AddDays(1.0);
						}
						if (DateTime.Now < value || DateTime.Now > dateTime2)
						{
							this.btnToPay.Attributes.Add("NeedNotInTimeTip", "1");
						}
					}
				}
				if (this.order.Gateway == EnumDescription.GetEnumDescription((Enum)(object)EnumPaymentType.OfflinePay, 1))
				{
					this.btnToPay.InnerText = "线下支付帮助";
					this.btnToPay.HRef = "FinishOrder.aspx?OrderId=" + this.order.OrderId + "&onlyHelp=true";
				}
			}
			if (this.order.ReducedPromotionAmount <= decimal.Zero)
			{
				this.liFullReduction.Visible = false;
			}
			this.litShipTo.Text = this.order.ShipTo;
			this.litPhone.Text = this.order.CellPhone;
			this.litAddress.Text = this.order.ShippingRegion + this.order.Address;
			this.litOrderId.Text = this.orderId;
			Literal literal3 = this.litOrderDate;
			dateTime = this.order.OrderDate;
			literal3.Text = dateTime.ToString();
			this.litTotalPrice.SetWhenIsNotNull(this.order.GetAmount(false).F2ToString("f2"));
			this.litOrderStatus.OrderStatusCode = this.order.OrderStatus;
			this.litOrderStatus.OrderItemStatus = this.order.ItemStatus;
			this.litOrderStatus.ShipmentModelId = this.order.ShippingModeId;
			this.litOrderStatus.IsConfirm = this.order.IsConfirm;
			this.litOrderStatus.Gateway = this.order.Gateway;
			this.litOrderStatus.PaymentTypeId = this.order.PaymentTypeId;
			this.litOrderStatus.PreSaleId = this.order.PreSaleId;
			this.litOrderStatus.DepositDate = this.order.DepositDate;
			Literal control = this.litPayTime;
			object value2;
			if (!(this.order.PayDate != DateTime.MinValue))
			{
				value2 = "";
			}
			else
			{
				dateTime = this.order.PayDate;
				value2 = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
			}
			control.SetWhenIsNotNull((string)value2);
			HtmlInputHidden control2 = this.orderStatus;
			paymentTypeId = (int)this.order.OrderStatus;
			control2.SetWhenIsNotNull(paymentTypeId.ToString());
			this.hidOrderId.SetWhenIsNotNull(this.orderId.ToString());
			this.litPaymentMode.SetWhenIsNotNull(this.order.PaymentType);
			this.litShipToDate.SetWhenIsNotNull(this.order.ShipToDate);
			if (this.order.PreSaleId > 0)
			{
				this.litBuildPrice.SetWhenIsNotNull((this.order.Deposit + this.order.FinalPayment).F2ToString("f2"));
			}
			else
			{
				this.litBuildPrice.SetWhenIsNotNull(this.order.GetPayTotal().F2ToString("f2"));
			}
			this.litRemark.SetWhenIsNotNull(this.order.Remark);
			this.litTakeCode.SetWhenIsNotNull((this.order.ShippingModeId == -2) ? this.order.TakeCode : "");
			this.litFreight.SetWhenIsNotNull(this.order.AdjustedFreight.F2ToString("f2"));
			this.litFreight2.SetWhenIsNotNull(this.order.AdjustedFreight.F2ToString("f2"));
			this.giftFreight.Visible = (this.order.LineItems.Count == 0);
			this.litFullCapacityReduction.SetWhenIsNotNull("-" + this.order.ReducedPromotionAmount.F2ToString("f2"));
			if (this.order.PreSaleId > 0)
			{
				if (productPreSaleInfo == null)
				{
					productPreSaleInfo = ProductPreSaleHelper.GetProductPreSaleInfo(this.order.PreSaleId);
				}
				nullable = this.order.DepositDate;
				if (!nullable.HasValue)
				{
					this.hidpresaleStaut.Value = "1";
					if (this.order.OrderStatus == OrderStatus.Closed)
					{
						this.hidpresaleStaut.Value = "6";
					}
				}
				else if (productPreSaleInfo.PaymentStartDate > DateTime.Now)
				{
					this.hidpresaleStaut.Value = "2";
				}
				else if (productPreSaleInfo.PaymentEndDate < DateTime.Now)
				{
					if (this.order.PayDate == DateTime.MinValue)
					{
						this.hidpresaleStaut.Value = "5";
					}
					else
					{
						this.hidpresaleStaut.Value = "4";
					}
				}
				else if (this.order.PayDate == DateTime.MinValue)
				{
					this.hidpresaleStaut.Value = "3";
				}
				else
				{
					this.hidpresaleStaut.Value = "4";
				}
			}
		}

		private void LoadAllControls()
		{
			this.liFullReduction = (HtmlGenericControl)this.FindControl("liFullReduction");
			this.litFightGroupStatusLabel = (FightGroupStatusLabel)this.FindControl("litFightGroupStatusLabel");
			this.ensureRecieved = (HtmlAnchor)this.FindControl("ensureRecieved");
			this.btnOrderRefund = (HtmlAnchor)this.FindControl("btnOrderRefund");
			this.btnOrderClose = (HtmlAnchor)this.FindControl("btnOrderClose");
			this.lookupQRCode = (HtmlAnchor)this.FindControl("lookupQRCode");
			this.lookupTrans = (HtmlAnchor)this.FindControl("lookupTrans");
			this.litShipTo = (Literal)this.FindControl("litShipTo");
			this.litPhone = (Literal)this.FindControl("litPhone");
			this.divStoreAddress = (HtmlGenericControl)this.FindControl("divStoreAddress");
			this.divLogists = (HtmlGenericControl)this.FindControl("divLogists");
			this.divShipAddress = (HtmlGenericControl)this.FindControl("divShipAddress");
			this.litAddress = (Literal)this.FindControl("litAddress");
			this.litOrderId = (Literal)this.FindControl("litOrderId");
			this.litOrderDate = (Literal)this.FindControl("litOrderDate");
			this.litOrderStatus = (OrderStatusLabel)this.FindControl("litOrderStatus");
			this.rptOrderProducts = (Common_MemberOrderProducts)this.FindControl("rptOrderProducts");
			this.litTotalPrice = (Literal)this.FindControl("litTotalPrice");
			this.litPayTime = (Literal)this.FindControl("litPayTime");
			this.divstoreinfo = (HtmlGenericControl)this.FindControl("divstoreinfo");
			this.orderStatus = (HtmlInputHidden)this.FindControl("orderStatus");
			this.hidOrderId = (HtmlInputHidden)this.FindControl("hidOrderId");
			this.dvClose = (HtmlControl)this.FindControl("dvClose");
			this.litRemark = (Literal)this.FindControl("litRemark");
			this.litShipToDate = (Literal)this.FindControl("litShipToDate");
			this.litTakeCode = (Literal)this.FindControl("litTakeCode");
			this.litStoreInfo = (Literal)this.FindControl("litStoreInfo");
			this.litStoreName = (Literal)this.FindControl("litStoreName");
			this.litStoreTel = (Literal)this.FindControl("litStoreTel");
			this.litCloseReason = (Literal)this.FindControl("litCloseReason");
			this.litCounponPrice = (Literal)this.FindControl("litCounponPrice");
			this.litPointsPrice = (Literal)this.FindControl("litPointsPrice");
			this.liPointPrice = (HtmlGenericControl)this.FindControl("liPointPrice");
			this.litBuildPrice = (Literal)this.FindControl("litBuildPrice");
			this.litDisCountPrice = (Literal)this.FindControl("litDisCountPrice");
			this.litFreight = (Literal)this.FindControl("litFreight");
			this.litFreight2 = (Literal)this.FindControl("litFreight2");
			this.giftFreight = (HtmlGenericControl)this.FindControl("giftFreight");
			this.litInvoiceTitle = (Literal)this.FindControl("litInvoiceTitle");
			this.litInvoiceTaxpayerNumber = (Literal)this.FindControl("litInvoiceTaxpayerNumber");
			this.liInvoiceTitle = (HtmlGenericControl)this.FindControl("liInvoiceTitle");
			this.liInvoiceTaxpayerNumber = (HtmlGenericControl)this.FindControl("liInvoiceTaxpayerNumber");
			this.liCounponPrice = (HtmlGenericControl)this.FindControl("liCounponPrice");
			this.liDiscountPrice = (HtmlGenericControl)this.FindControl("liDiscountPrice");
			this.litFullCapacityReduction = (Literal)this.FindControl("litFullCapacityReduction");
			this.litPaymentMode = (Literal)this.FindControl("litPaymentMode");
			this.btnOrderCancel = (HtmlAnchor)this.FindControl("btnOrderCancel");
			this.litTax = (Literal)this.FindControl("litTax");
			this.rptCartGifts = (WapTemplatedRepeater)this.FindControl("rptCartGifts");
			this.rptPromotionGifts = (WapTemplatedRepeater)this.FindControl("rptPromotionGifts");
			this.divRefund = (HtmlGenericControl)this.FindControl("divRefund");
			this.litRefundMoney = (Literal)this.FindControl("litRefundMoney");
			this.divRefundPoint = (HtmlGenericControl)this.FindControl("divRefundPoint");
			this.divGifts = (HtmlGenericControl)this.FindControl("divGifts");
			this.divProducts = (HtmlGenericControl)this.FindControl("divProducts");
			this.litRefundPoint = (Literal)this.FindControl("litRefundPoint");
			this.liTax = (HtmlGenericControl)this.FindControl("liTax");
			this.btnToPay = (HtmlAnchor)this.FindControl("btnToPay");
			this.paymenttypeselect = (Common_WAPPaymentTypeSelect)this.FindControl("paymenttypeselect");
			this.hidpresaleStaut = (HtmlInputHidden)this.FindControl("hidpresaleStaut");
			this.litDepositDate = (Literal)this.FindControl("litDepositDate");
			this.litFinalDate = (Literal)this.FindControl("litFinalDate");
			this.litDeposit = (Literal)this.FindControl("litDeposit");
			this.litFinal = (Literal)this.FindControl("litFinal");
			this.litGetGoodsRemark = (Literal)this.FindControl("litGetGoodsRemark");
			this.divPickUpRemark = (HtmlGenericControl)this.FindControl("divPickUpRemark");
		}

		private void BindStoreInfo()
		{
			StoresInfo storeById = DepotHelper.GetStoreById(this.order.StoreId);
			if (storeById != null)
			{
				this.litStoreName.SetWhenIsNotNull(storeById.StoreName);
				this.litStoreTel.SetWhenIsNotNull(storeById.Tel);
				this.litStoreInfo.SetWhenIsNotNull(RegionHelper.GetFullRegion(storeById.RegionId, " ", true, 0) + " " + storeById.Address);
			}
		}

		private void rptOrderProducts_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				HtmlAnchor htmlAnchor = (HtmlAnchor)e.Item.FindControl("lnkProductReview");
				Literal literal = (Literal)e.Item.FindControl("litPrice");
				Literal literal2 = (Literal)e.Item.FindControl("ltlProductCount");
				HtmlAnchor htmlAnchor2 = (HtmlAnchor)e.Item.FindControl("lkbtnApplyAfterSale");
				HtmlAnchor htmlAnchor3 = (HtmlAnchor)e.Item.FindControl("hylinkProductName");
				Literal literal3 = (Literal)e.Item.FindControl("ltlSKUContent");
				HtmlAnchor htmlAnchor4 = (HtmlAnchor)e.Item.FindControl("lkbtnItemStatus");
				Label label = e.Item.FindControl("litSendCount") as Label;
				string text = DataBinder.Eval(e.Item.DataItem, "SkuId").ToString();
				int num = 0;
				int.TryParse(DataBinder.Eval(e.Item.DataItem, "ProductId").ToString(), out num);
				LineItemInfo lineItemInfo = this.order.LineItems[text];
				literal.Text = lineItemInfo.ItemAdjustedPrice.F2ToString("f2");
				Literal literal4 = literal2;
				int num2 = lineItemInfo.Quantity;
				literal4.Text = num2.ToString();
				if (lineItemInfo.ShipmentQuantity > lineItemInfo.Quantity)
				{
					Label label2 = label;
					num2 = lineItemInfo.ShipmentQuantity - lineItemInfo.Quantity;
					label2.Text = "赠" + num2.ToString();
				}
				htmlAnchor3.InnerText = lineItemInfo.ItemDescription;
				if (this.order.StoreId > 0)
				{
					HtmlAnchor htmlAnchor5 = htmlAnchor;
					HtmlAnchor htmlAnchor6 = htmlAnchor3;
					string text4 = htmlAnchor5.HRef = (htmlAnchor6.HRef = $"StoreProductDetails.aspx?ProductId={num}&StoreId={this.order.StoreId}");
				}
				else
				{
					HtmlAnchor htmlAnchor7 = htmlAnchor;
					HtmlAnchor htmlAnchor8 = htmlAnchor3;
					string text4 = htmlAnchor7.HRef = (htmlAnchor8.HRef = $"ProductDetails.aspx?ProductId={num}");
				}
				if (this.order.OrderType == OrderType.ServiceOrder)
				{
					HtmlAnchor htmlAnchor9 = htmlAnchor;
					HtmlAnchor htmlAnchor10 = htmlAnchor3;
					string text4 = htmlAnchor9.HRef = (htmlAnchor10.HRef = $"ServiceProductDetails.aspx?ProductId={num}&StoreId={this.order.StoreId}");
				}
				htmlAnchor2.HRef = "ApplyReturn?OrderId=" + this.order.OrderId + "&SkuId=" + lineItemInfo.SkuId;
				literal3.Text = lineItemInfo.SKUContent;
				LineItemStatus lineItemStatus = (LineItemStatus)DataBinder.Eval(e.Item.DataItem, "Status");
				string text9 = (string)DataBinder.Eval(e.Item.DataItem, "StatusText");
				ReplaceInfo replaceInfo = lineItemInfo.ReplaceInfo;
				ReturnInfo returnInfo = lineItemInfo.ReturnInfo;
				if (lineItemStatus == LineItemStatus.Normal)
				{
					text9 = "";
				}
				else if (returnInfo != null)
				{
					text9 = ((returnInfo.AfterSaleType != AfterSaleTypes.OnlyRefund) ? EnumDescription.GetEnumDescription((Enum)(object)returnInfo.HandleStatus, 1) : EnumDescription.GetEnumDescription((Enum)(object)returnInfo.HandleStatus, 3));
				}
				else if (replaceInfo != null)
				{
					text9 = ((replaceInfo.HandleStatus == ReplaceStatus.Replaced) ? "" : EnumDescription.GetEnumDescription((Enum)(object)replaceInfo.HandleStatus, 1));
				}
				Literal literal5 = (Literal)e.Item.FindControl("litStatusText");
				if (literal5 != null)
				{
					literal5.Text = text9;
				}
				if (lineItemInfo.Status != 0 && this.order.OrderStatus != OrderStatus.Refunded && this.order.OrderStatus != OrderStatus.Closed)
				{
					string innerText = "";
					if (lineItemInfo.ReturnInfo != null && (lineItemInfo.Status == LineItemStatus.RefundApplied || lineItemInfo.Status == LineItemStatus.Refunded || lineItemInfo.Status == LineItemStatus.RefundRefused || lineItemInfo.Status == LineItemStatus.ReturnApplied || lineItemInfo.Status == LineItemStatus.Returned || lineItemInfo.Status == LineItemStatus.ReturnsRefused || lineItemInfo.Status == LineItemStatus.MerchantsAgreedForReturn || lineItemInfo.Status == LineItemStatus.GetGoodsForReturn || lineItemInfo.Status == LineItemStatus.DeliveryForReturn))
					{
						innerText = ((lineItemInfo.ReturnInfo.AfterSaleType != AfterSaleTypes.ReturnAndRefund) ? EnumDescription.GetEnumDescription((Enum)(object)lineItemInfo.ReturnInfo.HandleStatus, 2) : EnumDescription.GetEnumDescription((Enum)(object)lineItemInfo.ReturnInfo.HandleStatus, 0));
						htmlAnchor4.Visible = true;
						AttributeCollection attributes = htmlAnchor4.Attributes;
						num2 = lineItemInfo.ReturnInfo.ReturnId;
						attributes.Add("returnsId", num2.ToString());
					}
					else if (lineItemInfo.ReplaceInfo != null && lineItemInfo.ReplaceInfo.HandleStatus != ReplaceStatus.Replaced)
					{
						innerText = EnumDescription.GetEnumDescription((Enum)(object)lineItemInfo.ReplaceInfo.HandleStatus, 0);
						htmlAnchor4.Visible = true;
						AttributeCollection attributes2 = htmlAnchor4.Attributes;
						num2 = lineItemInfo.ReplaceInfo.ReplaceId;
						attributes2.Add("replaceId", num2.ToString());
					}
					htmlAnchor4.InnerText = innerText;
				}
				OrderStatus orderStatus = this.order.OrderStatus;
				DateTime finishDate = this.order.FinishDate;
				string gateway = this.order.Gateway;
				htmlAnchor2.Attributes.Add("OrderId", this.order.OrderId);
				htmlAnchor2.Attributes.Add("SkuId", text);
				htmlAnchor2.Attributes.Add("GateWay", gateway);
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				htmlAnchor2.Visible = ((this.order.LineItems.Count > 0 && orderStatus == OrderStatus.SellerAlreadySent) || (orderStatus == OrderStatus.Finished && !this.order.IsServiceOver));
				if (htmlAnchor2.Visible)
				{
					htmlAnchor2.Visible = (this.order.LineItems.Count >= 1 && (returnInfo == null || returnInfo.HandleStatus == ReturnStatus.Refused) && (replaceInfo == null || replaceInfo.HandleStatus == ReplaceStatus.Refused || replaceInfo.HandleStatus == ReplaceStatus.Replaced));
				}
				if (this.order.OrderStatus == OrderStatus.WaitBuyerPay && ((!this.order.DepositDate.HasValue && this.order.PreSaleId > 0) || this.order.PreSaleId <= 0))
				{
					this.btnOrderCancel.Visible = true;
					this.btnOrderCancel.Attributes.Add("onclick", $"closeOrder('{this.order.OrderId}')");
				}
			}
		}
	}
}
