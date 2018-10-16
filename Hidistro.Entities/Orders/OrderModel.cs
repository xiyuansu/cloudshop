using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.VShop;
using System;

namespace Hidistro.Entities.Orders
{
	public class OrderModel
	{
		private const string PAYMENT_POD_NAME = "hishop.plugins.payment.podrequest";

		private decimal adjustedFreigh;

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

		public string OrderId
		{
			get;
			set;
		}

		public string PayOrderId
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

		public int UserId
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

		public decimal GetProfit
		{
			get
			{
				return decimal.Parse((this.OrderTotal - this.RefundAmount - this.OrderCostPrice).F2ToString("f2"));
			}
		}

		public decimal OrderTotal
		{
			get;
			set;
		}

		public bool CanConfirmOrder
		{
			get
			{
				bool flag = false;
				if (this.FightGroupId > 0)
				{
					return this.FightGroupStatus == FightGroupStatus.FightGroupSuccess && (this.OrderStatus == OrderStatus.BuyerAlreadyPaid || this.Gateway == "hishop.plugins.payment.payonstore") && !this.IsConfirm && this.ItemStatus == OrderItemStatus.Nomarl;
				}
				return !this.IsConfirm && this.ItemStatus == OrderItemStatus.Nomarl && this.ShippingModeId == -2 && ((this.OrderStatus == OrderStatus.WaitBuyerPay && (this.Gateway == "hishop.plugins.payment.podrequest" || this.PaymentTypeId == -3)) || this.OrderStatus == OrderStatus.BuyerAlreadyPaid);
			}
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

		public decimal ProductAmount
		{
			get;
			set;
		}

		public decimal GetSubRefundProductAmount
		{
			get
			{
				return this.ProductAmount - this.RefundAmount;
			}
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

		public string CouponCode
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
			get;
			set;
		}

		public int ItemReturnsCount
		{
			get;
			set;
		}

		public int OnlyReturnedCount
		{
			get;
			set;
		}

		public int ReturnedCount
		{
			get;
			set;
		}

		public int ReplacedCount
		{
			get;
			set;
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

		public int AllQuantity
		{
			get;
			set;
		}

		public int AfterSaleQuantity
		{
			get;
			set;
		}

		public int BuyQuantity
		{
			get;
			set;
		}

		public int GiftQuantity
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

		public int SupplierId
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

		public decimal BalanceAmount
		{
			get;
			set;
		}

		public int AfterSaleCount
		{
			get;
			set;
		}

		public int ReturnId
		{
			get;
			set;
		}

		public int ReplaceId
		{
			get;
			set;
		}

		public string RealName
		{
			get;
			set;
		}

		public string Username
		{
			get;
			set;
		}

		public OrderModel()
		{
			this.OrderStatus = OrderStatus.WaitBuyerPay;
		}

		public decimal GetTotal(bool subBalanceAmount = false)
		{
			decimal d = this.ProductAmount;
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
				decimal d = this.ProductAmount;
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

		public decimal GetPayTotal()
		{
			if (this.PreSaleId > 0)
			{
				decimal num = this.Deposit + this.FinalPayment;
				if (this.RefundAmount > decimal.Zero)
				{
					num -= this.RefundAmount;
				}
				return num;
			}
			decimal d = this.GetSubRefundProductAmount;
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
	}
}
