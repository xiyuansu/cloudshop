using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.VShop;
using Hishop.Components.Validation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hidistro.Entities.Orders
{
	public class OrderInfo
	{
		private const string PAYMENT_POD_NAME = "hishop.plugins.payment.podrequest";

		private Dictionary<string, LineItemInfo> lineItems;

		private IList<OrderGiftInfo> gifts;

		private decimal adjustedFreigh;

		private IList<OrderInputItemInfo> inputItems;

		public string ErrorMessage
		{
			get;
			set;
		}

		public bool IsError
		{
			get;
			set;
		}

		public string HiPOSOrderDetails
		{
			get;
			set;
		}

		public string HiPOSUseName
		{
			get;
			set;
		}

		public Dictionary<string, LineItemInfo> LineItems
		{
			get
			{
				if (this.lineItems == null)
				{
					this.lineItems = new Dictionary<string, LineItemInfo>();
				}
				return this.lineItems;
			}
		}

		public IList<OrderGiftInfo> Gifts
		{
			get
			{
				if (this.gifts == null)
				{
					this.gifts = new List<OrderGiftInfo>();
				}
				return this.gifts;
			}
		}

		public int FightGroupId
		{
			get;
			set;
		}

		public int FightGroupActivityId
		{
			get;
			set;
		}

		public bool IsFightGroupHead
		{
			get;
			set;
		}

		public string OrderId
		{
			get;
			set;
		}

		public string PayOrderId
		{
			get
			{
				return this.OrderId + this.PayRandCode;
			}
		}

		public string OuterOrderId
		{
			get;
			set;
		}

		public string GatewayOrderId
		{
			get;
			set;
		}

		public string Remark
		{
			get;
			set;
		}

		public OrderMark? ManagerMark
		{
			get;
			set;
		}

		public string ManagerRemark
		{
			get;
			set;
		}

		public string Gateway
		{
			get;
			set;
		}

		[RangeValidator(typeof(decimal), "-10000000", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset = "ValOrder", MessageTemplate = "订单折扣不能为空，金额大小负1000万-1000万之间")]
		public decimal AdjustedDiscount
		{
			get;
			set;
		}

		public OrderStatus OrderStatus
		{
			get;
			set;
		}

		public DadaStatus DadaStatus
		{
			get;
			set;
		}

		public string CloseReason
		{
			get;
			set;
		}

		public DateTime UpdateDate
		{
			get;
			set;
		}

		public DateTime OrderDate
		{
			get;
			set;
		}

		public DateTime PayDate
		{
			get;
			set;
		}

		public DateTime ShippingDate
		{
			get;
			set;
		}

		public DateTime FinishDate
		{
			get;
			set;
		}

		public int ReferralUserId
		{
			get;
			set;
		}

		public int UserId
		{
			get;
			set;
		}

		public string Username
		{
			get;
			set;
		}

		public string EmailAddress
		{
			get;
			set;
		}

		public string RealName
		{
			get;
			set;
		}

		public string QQ
		{
			get;
			set;
		}

		public string Wangwang
		{
			get;
			set;
		}

		public string MSN
		{
			get;
			set;
		}

		public string ShippingRegion
		{
			get;
			set;
		}

		public string Address
		{
			get;
			set;
		}

		public string ZipCode
		{
			get;
			set;
		}

		public string ShipTo
		{
			get;
			set;
		}

		public string TelPhone
		{
			get;
			set;
		}

		public string CellPhone
		{
			get;
			set;
		}

		public string ShipToDate
		{
			get;
			set;
		}

		public string LatLng
		{
			get;
			set;
		}

		public int ShippingModeId
		{
			get;
			set;
		}

		public string ModeName
		{
			get;
			set;
		}

		public int RealShippingModeId
		{
			get;
			set;
		}

		public string RealModeName
		{
			get;
			set;
		}

		public int RegionId
		{
			get;
			set;
		}

		public decimal Freight
		{
			get;
			set;
		}

		[RangeValidator(typeof(decimal), "0.00", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset = "ValOrder", MessageTemplate = "运费不能为空，金额大小0-1000万之间")]
		public decimal AdjustedFreight
		{
			get
			{
				return this.adjustedFreigh;
			}
			set
			{
				this.adjustedFreigh = value;
			}
		}

		public string ShipOrderNumber
		{
			get;
			set;
		}

		public decimal Weight
		{
			get
			{
				decimal num = default(decimal);
				foreach (LineItemInfo value in this.LineItems.Values)
				{
					num += value.ItemWeight * (decimal)value.ShipmentQuantity;
				}
				return num;
			}
		}

		public string ExpressCompanyName
		{
			get;
			set;
		}

		public string ExpressCompanyAbb
		{
			get;
			set;
		}

		public int PaymentTypeId
		{
			get;
			set;
		}

		public string PaymentType
		{
			get;
			set;
		}

		public decimal RefundAmount
		{
			get;
			set;
		}

		public string RefundRemark
		{
			get;
			set;
		}

		public bool IsConfirm
		{
			get;
			set;
		}

		public string PayRandCode
		{
			get;
			set;
		}

		public int Points
		{
			get;
			set;
		}

		public int ExchangePoints
		{
			get;
			set;
		}

		public int? DeductionPoints
		{
			get;
			set;
		}

		public decimal? DeductionMoney
		{
			get;
			set;
		}

		public int ReducedPromotionId
		{
			get;
			set;
		}

		public string ReducedPromotionName
		{
			get;
			set;
		}

		public decimal ReducedPromotionAmount
		{
			get;
			set;
		}

		public bool IsReduced
		{
			get;
			set;
		}

		public int SentTimesPointPromotionId
		{
			get;
			set;
		}

		public string SentTimesPointPromotionName
		{
			get;
			set;
		}

		public decimal TimesPoint
		{
			get;
			set;
		}

		public bool IsSendTimesPoint
		{
			get;
			set;
		}

		public int FreightFreePromotionId
		{
			get;
			set;
		}

		public string FreightFreePromotionName
		{
			get;
			set;
		}

		public bool IsFreightFree
		{
			get;
			set;
		}

		public int GroupBuyId
		{
			get;
			set;
		}

		public int CountDownBuyId
		{
			get;
			set;
		}

		public int BundlingId
		{
			get;
			set;
		}

		public decimal NeedPrice
		{
			get;
			set;
		}

		public GroupBuyStatus GroupBuyStatus
		{
			get;
			set;
		}

		public FightGroupStatus FightGroupStatus
		{
			get;
			set;
		}

		public string CouponName
		{
			get;
			set;
		}

		public string CouponCode
		{
			get;
			set;
		}

		public decimal CouponAmount
		{
			get;
			set;
		}

		public decimal CouponValue
		{
			get;
			set;
		}

		public int StoreId
		{
			get;
			set;
		}

		public bool IsStoreCollect
		{
			get;
			set;
		}

		public string TakeCode
		{
			get;
			set;
		}

		public string TakeTime
		{
			get;
			set;
		}

		public decimal Tax
		{
			get;
			set;
		}

		public string InvoiceTitle
		{
			get;
			set;
		}

		public OrderSource OrderSource
		{
			get;
			set;
		}

		public string Sender
		{
			get;
			set;
		}

		public int ItemReplaceCount
		{
			get
			{
				int num = 0;
				if (this.lineItems == null)
				{
					return num;
				}
				foreach (LineItemInfo value in this.lineItems.Values)
				{
					if (value.ReplaceInfo != null && value.ReplaceInfo.HandleStatus != ReplaceStatus.Refused && value.ReplaceInfo.HandleStatus != ReplaceStatus.Replaced)
					{
						num++;
					}
				}
				return num;
			}
		}

		public int ItemReturnsCount
		{
			get
			{
				int num = 0;
				if (this.lineItems == null)
				{
					return num;
				}
				foreach (LineItemInfo value in this.lineItems.Values)
				{
					if (value.ReturnInfo != null && value.ReturnInfo.HandleStatus != ReturnStatus.Refused && value.ReturnInfo.HandleStatus != ReturnStatus.Returned)
					{
						num++;
					}
				}
				return num;
			}
		}

		public int OnlyReturnedCount
		{
			get
			{
				int num = 0;
				if (this.lineItems == null)
				{
					return num;
				}
				foreach (LineItemInfo value in this.lineItems.Values)
				{
					if (value.ReturnInfo != null && value.ReturnInfo.AfterSaleType == AfterSaleTypes.ReturnAndRefund && value.ReturnInfo.HandleStatus == ReturnStatus.Returned)
					{
						num++;
					}
				}
				return num;
			}
		}

		public int ReturnedCount
		{
			get
			{
				int num = 0;
				if (this.lineItems == null)
				{
					return num;
				}
				foreach (LineItemInfo value in this.lineItems.Values)
				{
					if (value.ReturnInfo != null && value.ReturnInfo.HandleStatus == ReturnStatus.Returned)
					{
						num++;
					}
				}
				return num;
			}
		}

		public int ReplacedCount
		{
			get
			{
				int num = 0;
				if (this.lineItems == null)
				{
					return num = 0;
				}
				foreach (LineItemInfo value in this.lineItems.Values)
				{
					if (value.ReplaceInfo != null && value.ReplaceInfo.HandleStatus == ReplaceStatus.Replaced)
					{
						num++;
					}
				}
				return num;
			}
		}

		public OrderItemStatus ItemStatus
		{
			get;
			set;
		}

		public bool IsPrinted
		{
			get;
			set;
		}

		public string FullRegionPath
		{
			get;
			set;
		}

		public int PreSaleId
		{
			get;
			set;
		}

		public decimal Deposit
		{
			get;
			set;
		}

		public decimal FinalPayment
		{
			get;
			set;
		}

		public DateTime? DepositDate
		{
			get;
			set;
		}

		public string DepositGatewayOrderId
		{
			get;
			set;
		}

		public bool IsSend
		{
			get;
			set;
		}

		public int SupplierId
		{
			get;
			set;
		}

		public bool IsBalanceOver
		{
			get;
			set;
		}

		public string ShipperName
		{
			get;
			set;
		}

		public string ParentOrderId
		{
			get;
			set;
		}

		public bool IsParentOrderPay
		{
			get;
			set;
		}

		public bool IsServiceOver
		{
			get;
			set;
		}

		public string ChildOrderIds
		{
			get;
			set;
		}

		public decimal OrderCostPrice
		{
			get;
			set;
		}

		public int UserAwardRecordsId
		{
			get;
			set;
		}

		public string IDNumber
		{
			get;
			set;
		}

		public string IDImage1
		{
			get;
			set;
		}

		public string IDImage2
		{
			get;
			set;
		}

		public int IDStatus
		{
			get;
			set;
		}

		public string IDRemark
		{
			get;
			set;
		}

		public bool IsincludeCrossBorderGoods
		{
			get;
			set;
		}

		public int ShippingId
		{
			get;
			set;
		}

		public string StoreName
		{
			get;
			set;
		}

		public OrderType OrderType
		{
			get;
			set;
		}

		public InvoiceType InvoiceType
		{
			get;
			set;
		}

		public string InvoiceTaxpayerNumber
		{
			get;
			set;
		}

		public string InvoiceData
		{
			get;
			set;
		}

		public UserInvoiceDataInfo InvoiceInfo
		{
			get
			{
				if (!string.IsNullOrEmpty(this.InvoiceData))
				{
					try
					{
						return JsonHelper.ParseFormJson<UserInvoiceDataInfo>(this.InvoiceData);
					}
					catch
					{
						return null;
					}
				}
				return null;
			}
		}

		public IList<OrderInputItemInfo> InputItems
		{
			get
			{
				if (this.inputItems == null)
				{
					this.inputItems = new List<OrderInputItemInfo>();
				}
				return this.inputItems;
			}
		}

		public decimal BalanceAmount
		{
			get;
			set;
		}

		public bool IsCanRefund
		{
			get
			{
				OrderStatus orderStatus = this.OrderStatus;
				if (this.OrderType == OrderType.ServiceOrder)
				{
					bool flag = orderStatus == OrderStatus.BuyerAlreadyPaid && this.ItemStatus == OrderItemStatus.Nomarl && this.LineItems.Count != 0;
					if (flag)
					{
						LineItemInfo value = this.LineItems.FirstOrDefault().Value;
						if (value.IsRefund)
						{
							if (value.IsOverRefund)
							{
								return true;
							}
							if (value.IsValid)
							{
								return true;
							}
							if (DateTime.Now >= value.ValidStartDate.Value && DateTime.Now <= value.ValidEndDate.Value)
							{
								return true;
							}
							return false;
						}
						return false;
					}
					return flag;
				}
				return orderStatus == OrderStatus.BuyerAlreadyPaid && this.ItemStatus == OrderItemStatus.Nomarl && this.LineItems.Count != 0;
			}
		}

		public static event EventHandler<EventArgs> Created;

		public static event EventHandler<EventArgs> Payment;

		public static event EventHandler<EventArgs> Deliver;

		public static event EventHandler<EventArgs> Refund;

		public static event EventHandler<EventArgs> Closed;

		public OrderInfo()
		{
			this.OrderStatus = OrderStatus.WaitBuyerPay;
		}

		public decimal GetTotal(bool subBalanceAmount = false)
		{
			decimal d = this.GetAmount(false);
			if (this.IsReduced)
			{
				d -= this.ReducedPromotionAmount;
			}
			d += this.Tax;
			if (!string.IsNullOrEmpty(this.CouponCode))
			{
				d -= this.CouponValue;
			}
			if (d < decimal.Zero)
			{
				d = default(decimal);
			}
			d += this.AdjustedFreight;
			if (this.DeductionMoney.HasValue)
			{
				d -= this.DeductionMoney.Value;
			}
			d += this.AdjustedDiscount;
			if (d <= decimal.Zero)
			{
				d = default(decimal);
				if (this.PreSaleId > 0)
				{
					d = this.Deposit;
				}
			}
			if (subBalanceAmount)
			{
				d -= this.BalanceAmount;
			}
			return decimal.Parse(d.F2ToString("f2"));
		}

		public decimal GetFinalPayment()
		{
			if (this.PreSaleId > 0)
			{
				decimal d = this.GetAmount(false);
				if (this.IsReduced)
				{
					d -= this.ReducedPromotionAmount;
				}
				d += this.Tax;
				if (!string.IsNullOrEmpty(this.CouponCode))
				{
					d -= this.CouponValue;
				}
				d -= this.Deposit;
				if (d < decimal.Zero)
				{
					d = default(decimal);
				}
				d += this.AdjustedFreight;
				if (this.DeductionMoney.HasValue)
				{
					d -= this.DeductionMoney.Value;
				}
				d += this.AdjustedDiscount;
				if (d <= decimal.Zero)
				{
					d = default(decimal);
				}
				return decimal.Parse(d.F2ToString("f2"));
			}
			return decimal.Zero;
		}

		public virtual decimal GetCostPrice()
		{
			decimal num = default(decimal);
			foreach (LineItemInfo value in this.LineItems.Values)
			{
				num += value.ItemCostPrice * (decimal)value.ShipmentQuantity;
			}
			if (this.SupplierId <= 0)
			{
				foreach (OrderGiftInfo gift in this.Gifts)
				{
					num += gift.CostPrice * (decimal)gift.Quantity;
				}
			}
			return num;
		}

		public virtual decimal GetProfit()
		{
			return decimal.Parse((this.GetTotal(false) - this.RefundAmount - this.GetCostPrice()).F2ToString("f2"));
		}

		public bool CanClose(bool isOpenMultStore, bool isStore = true)
		{
			bool result = false;
			if (!isOpenMultStore)
			{
				if (isStore)
				{
					return false;
				}
			}
			else if (!isStore && this.OrderStatus == OrderStatus.SellerAlreadySent && this.StoreId > 0)
			{
				return false;
			}
			if (this.OrderStatus == OrderStatus.WaitBuyerPay)
			{
				if (this.PreSaleId > 0)
				{
					result = !this.DepositDate.HasValue;
				}
				else if (this.Gateway != "hishop.plugins.payment.podrequest" && this.ShippingModeId != -2 && this.FightGroupId == 0 && (this.ParentOrderId == "0" || this.ParentOrderId == "-1"))
				{
					result = true;
				}
				if (this.ShippingModeId == -2)
				{
					result = ((this.ItemStatus == OrderItemStatus.Nomarl && !this.IsConfirm) & isOpenMultStore);
				}
			}
			if (this.Gateway == "hishop.plugins.payment.podrequest" && (this.OrderStatus == OrderStatus.WaitBuyerPay || this.OrderStatus == OrderStatus.SellerAlreadySent) && (this.ParentOrderId == "0" || this.ParentOrderId == "-1"))
			{
				result = true;
			}
			return result;
		}

		public bool CanConfirmTakeCode()
		{
			bool result = false;
			if (this.ShippingModeId == -2)
			{
				result = (this.IsConfirm && this.ItemStatus == OrderItemStatus.Nomarl && (this.OrderStatus == OrderStatus.BuyerAlreadyPaid || this.OrderStatus == OrderStatus.WaitBuyerPay));
			}
			return result;
		}

		public bool CanConfirmOrder()
		{
			bool flag = false;
			if (this.FightGroupId > 0)
			{
				return this.FightGroupStatus == FightGroupStatus.FightGroupSuccess && (this.OrderStatus == OrderStatus.BuyerAlreadyPaid || this.Gateway == "hishop.plugins.payment.payonstore") && !this.IsConfirm && this.ItemStatus == OrderItemStatus.Nomarl;
			}
			return !this.IsConfirm && this.ItemStatus == OrderItemStatus.Nomarl && this.ShippingModeId == -2 && ((this.OrderStatus == OrderStatus.WaitBuyerPay && (this.Gateway == "hishop.plugins.payment.podrequest" || this.PaymentTypeId == -3)) || this.OrderStatus == OrderStatus.BuyerAlreadyPaid);
		}

		public bool CanSendGoods(bool isOpenMultStore)
		{
			bool result = false;
			if (isOpenMultStore && this.ShippingModeId == -2)
			{
				return false;
			}
			if (this.ItemStatus == OrderItemStatus.Nomarl && (this.OrderStatus == OrderStatus.BuyerAlreadyPaid || this.OrderStatus == OrderStatus.WaitBuyerPay || (this.OrderStatus == OrderStatus.SellerAlreadySent && this.DadaStatus == DadaStatus.Expired)))
			{
				if (this.GroupBuyId > 0)
				{
					string[] source = new string[1]
					{
						"hishop.plugins.payment.podrequest"
					};
					result = ((this.OrderStatus != OrderStatus.WaitBuyerPay || source.Contains(this.Gateway)) && this.GroupBuyStatus == GroupBuyStatus.Success);
				}
				else if (this.FightGroupId > 0)
				{
					result = (this.FightGroupStatus == FightGroupStatus.FightGroupSuccess && this.OrderStatus == OrderStatus.BuyerAlreadyPaid && this.ShippingModeId != -2);
				}
				else
				{
					result = true;
					if (this.OrderStatus == OrderStatus.WaitBuyerPay && this.Gateway != "hishop.plugins.payment.podrequest")
					{
						result = false;
					}
				}
				result = (result && (this.DadaStatus == (DadaStatus)0 || this.DadaStatus > DadaStatus.Finished));
			}
			return result;
		}

		public int GetPoint(decimal pointsRate)
		{
			int result = 0;
			if (pointsRate == decimal.Zero)
			{
				return result;
			}
			decimal d = this.GetTotal(false) - this.adjustedFreigh;
			if (d < decimal.Zero)
			{
				return 0;
			}
			if (d * this.TimesPoint / pointsRate > 2147483647m)
			{
				result = 2147483647;
			}
			else if (d * this.TimesPoint >= pointsRate)
			{
				result = (int)(d * this.TimesPoint / pointsRate);
			}
			return result;
		}

		public int GetTotalNeedPoint()
		{
			int num = 0;
			if (this.Gifts == null || this.Gifts.Count == 0)
			{
				return num;
			}
			foreach (OrderGiftInfo gift in this.Gifts)
			{
				if (gift.PromoteType == 0)
				{
					num += gift.NeedPoint * gift.Quantity;
				}
			}
			return num;
		}

		public int GetGroupBuyOerderNumber()
		{
			if (this.GroupBuyId > 0)
			{
				using (Dictionary<string, LineItemInfo>.ValueCollection.Enumerator enumerator = this.LineItems.Values.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						LineItemInfo current = enumerator.Current;
						return current.Quantity;
					}
				}
			}
			return 0;
		}

		public decimal GetAmount(bool SubRefuned = false)
		{
			decimal num = default(decimal);
			decimal num2 = default(decimal);
			foreach (LineItemInfo value in this.LineItems.Values)
			{
				if ((value.Status == LineItemStatus.Refunded || value.Status == LineItemStatus.Returned) && value.ReturnInfo != null && value.Status == LineItemStatus.Returned)
				{
					num2 += value.ReturnInfo.RefundAmount;
				}
				num += value.GetSubTotal();
			}
			if (this.OrderStatus == OrderStatus.Closed)
			{
				num2 = this.RefundAmount;
			}
			if (SubRefuned)
			{
				return decimal.Parse((num - num2).F2ToString("f2"));
			}
			return decimal.Parse(num.F2ToString("f2"));
		}

		public decimal GetPayTotal()
		{
			if (this.PreSaleId > 0)
			{
				decimal num = this.Deposit + this.FinalPayment;
				if (this.GetRefunedAmount("") > decimal.Zero)
				{
					num -= this.GetRefunedAmount("");
				}
				return num;
			}
			decimal d = this.GetAmount(true);
			if (this.IsReduced)
			{
				d -= this.ReducedPromotionAmount;
			}
			d += this.Tax;
			if (!string.IsNullOrEmpty(this.CouponCode))
			{
				d -= this.CouponValue;
			}
			if (d < decimal.Zero)
			{
				d = default(decimal);
			}
			d += this.AdjustedFreight;
			if (this.DeductionMoney.HasValue)
			{
				d -= this.DeductionMoney.Value;
			}
			d += this.AdjustedDiscount;
			if (d <= decimal.Zero)
			{
				d = default(decimal);
			}
			return d;
		}

		public decimal GetAmount_Cost(bool SubRefuned = false)
		{
			decimal num = default(decimal);
			decimal num2 = default(decimal);
			decimal num3 = default(decimal);
			foreach (LineItemInfo value in this.LineItems.Values)
			{
				num3 = value.GetSubTotal_Cost();
				num += num3;
				if ((value.Status == LineItemStatus.Refunded || value.Status == LineItemStatus.Returned) && value.ReturnInfo != null && value.Status == LineItemStatus.Returned)
				{
					num2 += ((num3 > value.ReturnInfo.RefundAmount) ? value.ReturnInfo.RefundAmount : num3);
				}
			}
			if (this.OrderStatus == OrderStatus.Refunded && num2 > this.RefundAmount)
			{
				num2 = this.RefundAmount;
			}
			if (SubRefuned)
			{
				return decimal.Parse((num - num2).F2ToString("f2"));
			}
			return decimal.Parse(num.F2ToString("f2"));
		}

		public bool CheckAction(OrderActions action)
		{
			if (this.OrderStatus == OrderStatus.Finished || this.OrderStatus == OrderStatus.Closed)
			{
				return false;
			}
			switch (action)
			{
			case OrderActions.SELLER_REJECT_REFUND:
				return (this.OrderStatus == OrderStatus.BuyerAlreadyPaid || this.OrderStatus == OrderStatus.SellerAlreadySent) && this.ItemStatus == OrderItemStatus.Nomarl;
			case OrderActions.BUYER_CONFIRM_GOODS:
			case OrderActions.SELLER_FINISH_TRADE:
				return (this.OrderStatus == OrderStatus.SellerAlreadySent || this.ShippingModeId == -2) && this.ItemStatus == OrderItemStatus.Nomarl;
			case OrderActions.BUYER_PAY:
			case OrderActions.SELLER_CONFIRM_PAY:
			case OrderActions.SELLER_MODIFY_TRADE:
			case OrderActions.SELLER_CLOSE:
				return this.OrderStatus == OrderStatus.WaitBuyerPay || (this.OrderStatus == OrderStatus.SellerAlreadySent && this.Gateway.ToLower() == "hishop.plugins.payment.podrequest");
			case OrderActions.MASTER_SELLER_MODIFY_DELIVER_ADDRESS:
			case OrderActions.MASTER_SELLER_MODIFY_PAYMENT_MODE:
			case OrderActions.MASTER_SELLER_MODIFY_SHIPPING_MODE:
			case OrderActions.MASTER_SELLER_MODIFY_GIFTS:
				return this.OrderStatus == OrderStatus.WaitBuyerPay || this.OrderStatus == OrderStatus.BuyerAlreadyPaid;
			case OrderActions.SELLER_SEND_GOODS:
				return (this.OrderStatus == OrderStatus.BuyerAlreadyPaid || (this.OrderStatus == OrderStatus.WaitBuyerPay && this.Gateway == "hishop.plugins.payment.podrequest")) && this.ItemStatus == OrderItemStatus.Nomarl;
			case OrderActions.CONFIRM_TAKE_GOODS:
				return this.ShippingModeId == -2;
			default:
				return false;
			}
		}

		public static string GetOrderStatusName(OrderStatus orderStatus, OrderType orderType = OrderType.NormalOrder)
		{
			string text = "-";
			switch (orderStatus)
			{
			case OrderStatus.WaitBuyerPay:
				return "等待买家付款";
			case OrderStatus.BuyerAlreadyPaid:
				if (orderType == OrderType.ServiceOrder)
				{
					return "已付款,待核销";
				}
				return "已付款,等待发货";
			case OrderStatus.SellerAlreadySent:
				return "已发货";
			case OrderStatus.Closed:
				return "已关闭";
			case OrderStatus.Finished:
				return "订单已完成";
			case OrderStatus.History:
				return "历史订单";
			case OrderStatus.ApplyForRefund:
				return "申请退款";
			default:
				return "";
			}
		}

		public int GetAllQuantity(bool isCountAfterSale = true)
		{
			int num = 0;
			if (this.lineItems != null && this.lineItems.Count > 0)
			{
				foreach (LineItemInfo value in this.lineItems.Values)
				{
					if (isCountAfterSale || (value.Status != LineItemStatus.Refunded && value.Status != LineItemStatus.Returned))
					{
						num += value.ShipmentQuantity;
					}
				}
			}
			return num;
		}

		public int GetBuyQuantity()
		{
			int num = 0;
			if (this.lineItems != null && this.lineItems.Count > 0)
			{
				foreach (LineItemInfo value in this.lineItems.Values)
				{
					num += value.Quantity;
				}
			}
			return num;
		}

		public int GetSkuQuantity(string skuId)
		{
			int result = 0;
			if (this.lineItems != null && this.lineItems.Count > 0 && this.lineItems.ContainsKey(skuId))
			{
				LineItemInfo lineItemInfo = this.lineItems[skuId];
				result = lineItemInfo.ShipmentQuantity;
			}
			return result;
		}

		public int GetGiftQuantity()
		{
			int num = 0;
			if (this.gifts != null && this.gifts.Count > 0)
			{
				foreach (OrderGiftInfo gift in this.gifts)
				{
					num += gift.Quantity;
				}
			}
			return num;
		}

		public static void OnCreated(OrderInfo order)
		{
			if (OrderInfo.Created != null)
			{
				OrderInfo.Created(order, new EventArgs());
			}
		}

		public void OnCreated()
		{
			if (OrderInfo.Created != null)
			{
				OrderInfo.Created(this, new EventArgs());
			}
		}

		public static void OnPayment(OrderInfo order)
		{
			if (OrderInfo.Payment != null)
			{
				OrderInfo.Payment(order, new EventArgs());
			}
		}

		public void OnPayment()
		{
			if (OrderInfo.Payment != null)
			{
				OrderInfo.Payment(this, new EventArgs());
			}
		}

		public static void OnDeliver(OrderInfo order)
		{
			if (OrderInfo.Deliver != null)
			{
				OrderInfo.Deliver(order, new EventArgs());
			}
		}

		public void OnDeliver()
		{
			if (OrderInfo.Deliver != null)
			{
				OrderInfo.Deliver(this, new EventArgs());
			}
		}

		public decimal GetProductTotal()
		{
			decimal num = default(decimal);
			foreach (LineItemInfo value in this.lineItems.Values)
			{
				num += (decimal)value.Quantity * value.ItemAdjustedPrice;
			}
			return num;
		}

		public decimal GetCanRefundAmount(string SkuId = "", GroupBuyInfo groupbuy = null, int quantity = 0)
		{
			if (this.OrderStatus == OrderStatus.Refunded)
			{
				return decimal.Zero;
			}
			decimal num = this.GetTotal(false);
			if (this.PreSaleId > 0)
			{
				num = this.Deposit + this.FinalPayment;
			}
			if (this.OrderStatus == OrderStatus.SellerAlreadySent || this.OrderStatus == OrderStatus.Finished)
			{
				num -= this.AdjustedFreight;
			}
			if (string.IsNullOrEmpty(SkuId))
			{
				if (groupbuy == null)
				{
					return decimal.Parse(num.F2ToString("f2"));
				}
				decimal num2 = this.GetPayTotal();
				if (groupbuy.Status != GroupBuyStatus.Failed && (this.OrderStatus == OrderStatus.BuyerAlreadyPaid || this.OrderStatus == OrderStatus.ApplyForRefund))
				{
					num2 -= groupbuy.NeedPrice;
				}
				if (num2 < decimal.Zero)
				{
					num2 = default(decimal);
				}
				return num2.F2ToString("f2").ToDecimal(0);
			}
			if (this.lineItems.ContainsKey(SkuId))
			{
				LineItemInfo lineItemInfo = this.lineItems[SkuId];
				if (quantity <= 0 || quantity > lineItemInfo.ShipmentQuantity)
				{
					quantity = lineItemInfo.ShipmentQuantity;
				}
				decimal d = lineItemInfo.ItemAdjustedPrice * (decimal)lineItemInfo.Quantity;
				decimal productTotal = this.GetProductTotal();
				if (productTotal > decimal.Zero)
				{
					decimal num3 = d - d / productTotal * this.GetOrderDsicountAmount();
					if (lineItemInfo.ShipmentQuantity == quantity)
					{
						return (num3 > decimal.Zero) ? num3.F2ToString("f2").ToDecimal(0) : decimal.Zero;
					}
					return (num3 > decimal.Zero) ? (num3 / (decimal)lineItemInfo.ShipmentQuantity * (decimal)quantity).F2ToString("f2").ToDecimal(0) : decimal.Zero;
				}
				return decimal.Zero;
			}
			return decimal.Zero;
		}

		public decimal GetOrderDsicountAmount()
		{
			decimal d = default(decimal);
			if (this.DeductionMoney.HasValue)
			{
				d += this.DeductionMoney.Value;
			}
			d += this.CouponValue;
			d += this.ReducedPromotionAmount;
			return d - this.AdjustedDiscount;
		}

		public decimal GetRefunedAmount(string SkuId = "")
		{
			decimal num = default(decimal);
			if (this.OrderStatus == OrderStatus.Closed || this.OrderStatus == OrderStatus.ApplyForRefund)
			{
				return this.RefundAmount;
			}
			foreach (LineItemInfo value in this.lineItems.Values)
			{
				if (value.SkuId != SkuId && (value.Status == LineItemStatus.Refunded || value.Status == LineItemStatus.Returned || value.Status == LineItemStatus.DeliveryForReturn || value.Status == LineItemStatus.MerchantsAgreedForReturn || value.Status == LineItemStatus.GetGoodsForReturn || value.Status == LineItemStatus.RefundApplied || value.Status == LineItemStatus.ReturnApplied))
				{
					num += value.ReturnInfo.RefundAmount;
				}
			}
			return num;
		}

		public decimal GetProductAmount(string skuId, int quantity = 0)
		{
			decimal result = default(decimal);
			if (this.lineItems.ContainsKey(skuId))
			{
				LineItemInfo lineItemInfo = this.lineItems[skuId];
				if (quantity <= 0 || quantity > lineItemInfo.Quantity)
				{
					quantity = lineItemInfo.Quantity;
				}
				return lineItemInfo.ItemAdjustedPrice * (decimal)quantity;
			}
			return result;
		}

		public static void OnRefund(OrderInfo order)
		{
			if (OrderInfo.Refund != null)
			{
				OrderInfo.Refund(order, new EventArgs());
			}
		}

		public void OnRefund()
		{
			if (OrderInfo.Refund != null)
			{
				OrderInfo.Refund(this, new EventArgs());
			}
		}

		public static void OnClosed(OrderInfo order)
		{
			if (OrderInfo.Closed != null)
			{
				OrderInfo.Closed(order, new EventArgs());
			}
		}

		public void OnClosed()
		{
			if (OrderInfo.Closed != null)
			{
				OrderInfo.Closed(this, new EventArgs());
			}
		}
	}
}
