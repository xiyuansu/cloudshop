using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
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
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WAPServiceMemberOrderDetails : WAPMemberTemplatedWebControl
	{
		private const string VERIFICATION_CODE_QRCODE_SAVE_RELATIVE_PATH = "/Storage/master/ServiceQRCode/";

		private string orderId;

		private HtmlGenericControl liFullReduction;

		private Literal litBuildPrice;

		private Literal litShipToDate;

		private Literal litTax;

		private Literal litDisCountPrice;

		private Literal litPointsPrice;

		private Literal litOrderId;

		private Literal litTakeCode;

		private Literal litCounponPrice;

		private Literal litCloseReason;

		private Literal litOrderDate;

		private Literal litRemark;

		private Literal litOrderStatus;

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

		private HtmlGenericControl divstoreinfo;

		private HtmlGenericControl liInvoiceTitle;

		private HtmlGenericControl liInvoiceTaxpayerNumber;

		private HtmlGenericControl liCounponPrice;

		private HtmlGenericControl liDiscountPrice;

		private HtmlGenericControl liPointPrice;

		private HtmlGenericControl liTax;

		private HtmlControl dvClose;

		private HtmlControl btnOrderClose;

		private HtmlAnchor ensureRecieved;

		private HtmlAnchor lookupQRCode;

		private HtmlAnchor btnOrderRefund;

		private HtmlAnchor btnOrderCancel;

		private HtmlAnchor btnToPay;

		private HtmlAnchor lnkProductReview;

		private HtmlGenericControl spandemo;

		private HtmlGenericControl liBalanceAmount;

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

		private HtmlInputHidden hidHasTradePassword;

		private Literal lit_ValidDate;

		private Repeater rptVerCode;

		private HtmlGenericControl divOrderInputItemInfo;

		private HtmlGenericControl divOrderBtn;

		private Literal litOrderInputItemInfo;

		private Literal litBalanceAmount;

		private Repeater rptOrderInputItemInfo;

		private int currentGroup = 1;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VServiceMemberOrderDetails.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.hidHasTradePassword = (HtmlInputHidden)this.FindControl("hidHasTradePassword");
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			this.orderId = this.Page.Request.QueryString["orderId"];
			this.litBalanceAmount = (Literal)this.FindControl("litBalanceAmount");
			this.liBalanceAmount = (HtmlGenericControl)this.FindControl("liBalanceAmount");
			this.spandemo = (HtmlGenericControl)this.FindControl("spandemo");
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
			this.BindOrderInfo();
			this.paymenttypeselect.ClientType = base.ClientType;
			if (this.order.CountDownBuyId > 0)
			{
				this.paymenttypeselect.OrderSalesPromotion = Common_WAPPaymentTypeSelect.EnumOrderSalesPromotion.CountDownBuy;
			}
			this.paymenttypeselect.IsFireGroup = (this.order.FightGroupId > 0);
			this.litFightGroupStatusLabel.Order = this.order;
			this.paymenttypeselect.IsServiceProduct = true;
			this.SetOperatorsStatus();
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
			IEnumerable<OrderGiftInfo> enumerable = from a in this.order.Gifts
			where a.PromoteType == 0 || a.PromoteType == 15
			select a;
			if (enumerable.Count() > 0)
			{
				this.rptCartGifts.DataSource = enumerable;
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
			if (this.order.InputItems.Count > 0)
			{
				this.divOrderInputItemInfo.Visible = true;
				this.rptOrderInputItemInfo.DataSource = this.order.InputItems;
				this.rptOrderInputItemInfo.ItemDataBound += this.rptOrderInputItemInfo_ItemDataBound;
				this.rptOrderInputItemInfo.DataBind();
			}
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
				this.litPointsPrice.Text = this.order.DeductionMoney.Value.F2ToString("f2");
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
				this.btnOrderRefund.Visible = (this.order.OrderStatus == OrderStatus.BuyerAlreadyPaid && this.order.ItemStatus == OrderItemStatus.Nomarl && this.order.LineItems.Count != 0);
				if (this.btnOrderRefund.Visible && this.order.FightGroupId > 0)
				{
					FightGroupInfo fightGroup = VShopHelper.GetFightGroup(this.order.FightGroupId);
					if (fightGroup != null)
					{
						this.btnOrderRefund.Visible = (fightGroup.Status != FightGroupStatus.FightGroupIn);
					}
				}
				if (this.order.OrderType == OrderType.ServiceOrder && this.btnOrderRefund.Visible)
				{
					LineItemInfo value = this.order.LineItems.FirstOrDefault().Value;
					if (value.IsRefund)
					{
						if (value.IsOverRefund)
						{
							this.btnOrderRefund.Visible = true;
						}
						else if (value.IsValid)
						{
							this.btnOrderRefund.Visible = true;
						}
						else if (DateTime.Now >= value.ValidStartDate.Value && DateTime.Now <= value.ValidEndDate.Value)
						{
							this.btnOrderRefund.Visible = true;
						}
						else
						{
							this.btnOrderRefund.Visible = false;
						}
					}
					else
					{
						this.btnOrderRefund.Visible = false;
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
				this.litBalanceAmount.Text = this.order.BalanceAmount.F2ToString("f2");
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
			if (this.order.OrderStatus == OrderStatus.SellerAlreadySent && this.order.ItemStatus == OrderItemStatus.Nomarl)
			{
				this.ensureRecieved.Visible = true;
			}
			else
			{
				this.ensureRecieved.Visible = false;
			}
			if (this.order.OrderStatus == OrderStatus.SellerAlreadySent || this.order.OrderStatus == OrderStatus.Finished)
			{
				this.divOrderBtn.Visible = false;
			}
			if (string.IsNullOrEmpty(this.order.TakeCode) || string.IsNullOrEmpty(masterSettings.HiPOSAppId) || this.order.OrderStatus == OrderStatus.Finished || this.order.OrderStatus == OrderStatus.Closed)
			{
				this.lookupQRCode.Visible = false;
			}
			ProductPreSaleInfo productPreSaleInfo = null;
			int paymentTypeId;
			if (this.order.OrderStatus == OrderStatus.WaitBuyerPay && this.order.Gateway != EnumDescription.GetEnumDescription((Enum)(object)EnumPaymentType.CashOnDelivery, 1) && this.order.PaymentTypeId != -3)
			{
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
			LineItemInfo value2 = this.order.LineItems.FirstOrDefault().Value;
			string text = "长期有效";
			int num;
			if (!value2.IsValid)
			{
				nullable = value2.ValidStartDate;
				if (nullable.HasValue)
				{
					nullable = value2.ValidEndDate;
					num = (nullable.HasValue ? 1 : 0);
					goto IL_06a5;
				}
			}
			num = 0;
			goto IL_06a5;
			IL_06a5:
			if (num != 0)
			{
				nullable = value2.ValidStartDate;
				dateTime = nullable.Value;
				string arg = dateTime.ToString("yyyy-MM-dd");
				nullable = value2.ValidEndDate;
				dateTime = nullable.Value;
				text = string.Format("有效期&nbsp;&nbsp;{0} ~ {1}", arg, dateTime.ToString("yyyy-MM-dd"));
			}
			this.lit_ValidDate.Text = text;
			IList<OrderVerificationItemInfo> orderVerificationItems = TradeHelper.GetOrderVerificationItems(this.order.OrderId);
			ServiceOrderStatus serviceOrderStatus = this.GetOrderStatus(this.order, orderVerificationItems);
			this.litOrderStatus.Text = ((Enum)(object)serviceOrderStatus).ToDescription();
			this.CreateVerificationCodeQRCode(orderVerificationItems);
			this.rptVerCode.DataSource = orderVerificationItems;
			this.rptVerCode.ItemDataBound += this.rptVerCode_ItemDataBound;
			this.rptVerCode.DataBind();
			this.litOrderId.Text = this.orderId;
			Literal literal3 = this.litOrderDate;
			dateTime = this.order.OrderDate;
			literal3.Text = dateTime.ToString();
			this.litTotalPrice.SetWhenIsNotNull(this.order.GetAmount(false).F2ToString("f2"));
			Literal control = this.litPayTime;
			object value3;
			if (!(this.order.PayDate != DateTime.MinValue))
			{
				value3 = "";
			}
			else
			{
				dateTime = this.order.PayDate;
				value3 = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
			}
			control.SetWhenIsNotNull((string)value3);
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

		private void rptVerCode_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				OrderVerificationItemInfo orderVerificationItemInfo = e.Item.DataItem as OrderVerificationItemInfo;
				Literal literal = e.Item.FindControl("lit_ver_img") as Literal;
				Literal literal2 = e.Item.FindControl("lit_ver_pass") as Literal;
				Literal literal3 = e.Item.FindControl("lit_ver_status") as Literal;
				literal.Text = $"<img class=\"v-code-img\" src=\"{this.GetVerificationCodeQRCodePath(orderVerificationItemInfo)}\" />";
				literal2.Text = orderVerificationItemInfo.VerificationPassword;
				literal3.Text = EnumDescription.GetEnumDescription((Enum)(object)(VerificationStatus)orderVerificationItemInfo.VerificationStatus, 0);
			}
		}

		private void rptOrderInputItemInfo_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				OrderInputItemInfo orderInputItemInfo = e.Item.DataItem as OrderInputItemInfo;
				HtmlGenericControl htmlGenericControl;
				if (orderInputItemInfo.InputFieldType == 6)
				{
					htmlGenericControl = (e.Item.FindControl("divOrderInputItemTwo") as HtmlGenericControl);
					htmlGenericControl.Visible = true;
					Literal literal = e.Item.FindControl("litOrderInputItemTwoTtile") as Literal;
					Literal literal2 = e.Item.FindControl("litOrderInputItemTwoValue") as Literal;
					literal.Text = orderInputItemInfo.InputFieldTitle;
					StringBuilder stringBuilder = new StringBuilder();
					string[] array = orderInputItemInfo.InputFieldValue.Split(new string[1]
					{
						","
					}, StringSplitOptions.RemoveEmptyEntries);
					string[] array2 = array;
					foreach (string arg in array2)
					{
						stringBuilder.Append($"<img src=\"{arg}\" />");
					}
					literal2.Text = stringBuilder.ToString();
				}
				else
				{
					htmlGenericControl = (e.Item.FindControl("divOrderInputItemOne") as HtmlGenericControl);
					htmlGenericControl.Visible = true;
					Literal literal3 = e.Item.FindControl("litOrderInputItemOneTtile") as Literal;
					Literal literal4 = e.Item.FindControl("litOrderInputItemOneValue") as Literal;
					literal3.Text = orderInputItemInfo.InputFieldTitle;
					literal4.Text = orderInputItemInfo.InputFieldValue;
				}
				if (this.currentGroup != orderInputItemInfo.InputFieldGroup)
				{
					htmlGenericControl.Style.Add("border-top", "1px dashed #ddd");
					htmlGenericControl.Style.Add("padding-top", "10px");
					this.currentGroup = orderInputItemInfo.InputFieldGroup;
				}
			}
		}

		private void CreateVerificationCodeQRCode(IList<OrderVerificationItemInfo> orderVerCodes)
		{
			string format = "/Storage/master/ServiceQRCode/{0}_{1}.png";
			foreach (OrderVerificationItemInfo orderVerCode in orderVerCodes)
			{
				if (orderVerCode != null && !string.IsNullOrWhiteSpace(orderVerCode.VerificationPassword))
				{
					string qrCodeUrl = string.Format(format, orderVerCode.Id, orderVerCode.VerificationPassword);
					Globals.CreateQRCode(orderVerCode.VerificationPassword, qrCodeUrl, false, ImageFormats.Png);
				}
			}
		}

		private string GetVerificationCodeQRCodePath(OrderVerificationItemInfo data)
		{
			string result = "";
			if (data != null && !string.IsNullOrWhiteSpace(data.VerificationPassword))
			{
				string format = Globals.FullPath("/Storage/master/ServiceQRCode/{0}_{1}.png");
				result = string.Format(format, data.Id, data.VerificationPassword);
			}
			return result;
		}

		private void LoadAllControls()
		{
			this.liFullReduction = (HtmlGenericControl)this.FindControl("liFullReduction");
			this.litFightGroupStatusLabel = (FightGroupStatusLabel)this.FindControl("litFightGroupStatusLabel");
			this.ensureRecieved = (HtmlAnchor)this.FindControl("ensureRecieved");
			this.btnOrderRefund = (HtmlAnchor)this.FindControl("btnOrderRefund");
			this.btnOrderClose = (HtmlAnchor)this.FindControl("btnOrderClose");
			this.lookupQRCode = (HtmlAnchor)this.FindControl("lookupQRCode");
			this.litOrderId = (Literal)this.FindControl("litOrderId");
			this.litOrderDate = (Literal)this.FindControl("litOrderDate");
			this.litOrderStatus = (Literal)this.FindControl("litOrderStatus");
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
			this.litCloseReason = (Literal)this.FindControl("litCloseReason");
			this.litCounponPrice = (Literal)this.FindControl("litCounponPrice");
			this.litPointsPrice = (Literal)this.FindControl("litPointsPrice");
			this.liPointPrice = (HtmlGenericControl)this.FindControl("liPointPrice");
			this.litBuildPrice = (Literal)this.FindControl("litBuildPrice");
			this.litDisCountPrice = (Literal)this.FindControl("litDisCountPrice");
			this.litFreight = (Literal)this.FindControl("litFreight");
			this.litFreight2 = (Literal)this.FindControl("litFreight2");
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
			this.lit_ValidDate = (Literal)this.FindControl("lit_ValidDate");
			this.rptVerCode = (Repeater)this.FindControl("rptVerCode");
			this.divOrderBtn = (HtmlGenericControl)this.FindControl("divOrderBtn");
			this.rptOrderInputItemInfo = (Repeater)this.FindControl("rptOrderInputItemInfo");
			this.divOrderInputItemInfo = (HtmlGenericControl)this.FindControl("divOrderInputItemInfo");
			this.litOrderInputItemInfo = (Literal)this.FindControl("litOrderInputItemInfo");
		}

		private void BindStoreInfo()
		{
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
					string text4 = htmlAnchor5.HRef = (htmlAnchor6.HRef = $"ServiceProductDetails.aspx?ProductId={num}&StoreId={this.order.StoreId}");
				}
				else
				{
					HtmlAnchor htmlAnchor7 = htmlAnchor;
					HtmlAnchor htmlAnchor8 = htmlAnchor3;
					string text4 = htmlAnchor7.HRef = (htmlAnchor8.HRef = $"ServiceProductDetails.aspx?ProductId={num}");
				}
				htmlAnchor2.HRef = "ApplyReturn?OrderId=" + this.order.OrderId + "&SkuId=" + lineItemInfo.SkuId;
				literal3.Text = lineItemInfo.SKUContent;
				LineItemStatus lineItemStatus = (LineItemStatus)DataBinder.Eval(e.Item.DataItem, "Status");
				string text7 = (string)DataBinder.Eval(e.Item.DataItem, "StatusText");
				ReplaceInfo replaceInfo = lineItemInfo.ReplaceInfo;
				ReturnInfo returnInfo = lineItemInfo.ReturnInfo;
				if (lineItemStatus == LineItemStatus.Normal)
				{
					text7 = "";
				}
				else if (returnInfo != null)
				{
					text7 = ((returnInfo.AfterSaleType != AfterSaleTypes.OnlyRefund) ? EnumDescription.GetEnumDescription((Enum)(object)returnInfo.HandleStatus, 1) : EnumDescription.GetEnumDescription((Enum)(object)returnInfo.HandleStatus, 3));
				}
				else if (replaceInfo != null)
				{
					text7 = ((replaceInfo.HandleStatus == ReplaceStatus.Replaced) ? "" : EnumDescription.GetEnumDescription((Enum)(object)replaceInfo.HandleStatus, 1));
				}
				Literal literal5 = (Literal)e.Item.FindControl("litStatusText");
				if (literal5 != null)
				{
					literal5.Text = text7;
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
				htmlAnchor2.Visible = false;
				if (this.order.OrderStatus == OrderStatus.WaitBuyerPay && ((!this.order.DepositDate.HasValue && this.order.PreSaleId > 0) || this.order.PreSaleId <= 0))
				{
					this.btnOrderCancel.Visible = true;
					this.btnOrderCancel.Attributes.Add("onclick", $"closeOrder('{this.order.OrderId}')");
				}
			}
		}

		private ServiceOrderStatus GetOrderStatus(OrderInfo order, IList<OrderVerificationItemInfo> orderVerificationItems = null)
		{
			ServiceOrderStatus result = ServiceOrderStatus.Finished;
			if (order.OrderStatus == OrderStatus.WaitBuyerPay)
			{
				result = ServiceOrderStatus.WaitBuyerPay;
			}
			else if (order.OrderStatus == OrderStatus.Closed)
			{
				result = ServiceOrderStatus.Closed;
			}
			else if (order.OrderStatus == OrderStatus.Finished)
			{
				result = ServiceOrderStatus.Finished;
			}
			else
			{
				IList<OrderVerificationItemInfo> source = orderVerificationItems;
				if (orderVerificationItems == null)
				{
					source = TradeHelper.GetOrderVerificationItems(order.OrderId);
				}
				if (source.Any((OrderVerificationItemInfo d) => d.VerificationStatus == 0.GetHashCode()))
				{
					result = ServiceOrderStatus.WaitConsumption;
				}
				else if (source.Any((OrderVerificationItemInfo d) => d.VerificationStatus == 3.GetHashCode()))
				{
					result = ServiceOrderStatus.Expired;
				}
				else if (source.Count() > 0 && source.Count(delegate(OrderVerificationItemInfo d)
				{
					int verificationStatus = d.VerificationStatus;
					VerificationStatus verificationStatus2 = VerificationStatus.Refunded;
					int result2;
					if (verificationStatus != verificationStatus2.GetHashCode())
					{
						int verificationStatus3 = d.VerificationStatus;
						verificationStatus2 = VerificationStatus.ApplyRefund;
						result2 = ((verificationStatus3 != verificationStatus2.GetHashCode()) ? 1 : 0);
					}
					else
					{
						result2 = 0;
					}
					return (byte)result2 != 0;
				}) == 0)
				{
					result = ServiceOrderStatus.Refunding;
				}
			}
			return result;
		}
	}
}
