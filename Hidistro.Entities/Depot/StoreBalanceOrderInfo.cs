using Hidistro.Core;
using Hidistro.Entities.Orders;
using System;

namespace Hidistro.Entities.Depot
{
	public class StoreBalanceOrderInfo
	{
		public string OrderId
		{
			get;
			set;
		}

		public DateTime OrderDate
		{
			get;
			set;
		}

		public DateTime OverBalanceDate
		{
			get;
			set;
		}

		public decimal OrderTotal
		{
			get;
			set;
		}

		public decimal Freight
		{
			get;
			set;
		}

		public decimal DeductionMoney
		{
			get;
			set;
		}

		public decimal CouponValue
		{
			get;
			set;
		}

		public decimal RefundAmount
		{
			get;
			set;
		}

		public bool IsStoreCollect
		{
			get;
			set;
		}

		public decimal PlatCommission
		{
			get;
			set;
		}

		public decimal OverBalance
		{
			get;
			set;
		}

		public decimal ProductTotal
		{
			get;
			set;
		}

		public int StoreId
		{
			get;
			set;
		}

		public decimal Tax
		{
			get;
			set;
		}

		public OrderType OrderType
		{
			get;
			set;
		}

		public decimal GetShouldOverBalance(decimal commissionRate)
		{
			if (!this.IsStoreCollect)
			{
				return this.OrderTotal + this.DeductionMoney + this.CouponValue - this.Tax.F2ToString("f2").ToDecimal(0) - this.RefundAmount - this.GetRefundCouponValue() - this.GetPlatCommission(commissionRate);
			}
			return this.DeductionMoney + this.CouponValue - this.GetRefundCouponValue() - this.Tax.F2ToString("f2").ToDecimal(0) - this.GetPlatCommission(commissionRate);
		}

		public decimal GetRefundCouponValue()
		{
			decimal result = default(decimal);
			if (this.RefundAmount > decimal.Zero)
			{
				decimal d = this.RefundAmount / (this.OrderTotal - this.Freight - this.Tax.F2ToString("f2").ToDecimal(0));
				if (d < decimal.One)
				{
					return (d * this.CouponValue).F2ToString("f2").ToDecimal(0);
				}
				return this.CouponValue;
			}
			return result;
		}

		public decimal GetPlatCommission(decimal commissionRate)
		{
			return (this.OrderTotal + this.CouponValue + this.DeductionMoney - this.GetRefundCouponValue() - this.Tax - this.RefundAmount - this.Freight) * (commissionRate / 100m).F2ToString("f2").ToDecimal(0);
		}
	}
}
