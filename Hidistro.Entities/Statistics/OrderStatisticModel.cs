using Hidistro.Core;
using System;

namespace Hidistro.Entities.Statistics
{
	public class OrderStatisticModel
	{
		private DateTime _statisticaldate;

		private int _orderusernum;

		private int _ordernum;

		private int _orderproductquantity;

		private decimal _orderamount;

		private int _paymentusernum;

		private int _paymentordernum;

		private int _paymentproductnum;

		private decimal _paymentamount;

		private decimal _refundamount;

		private int _pv;

		private int _uv;

		public DateTime StatisticalDate
		{
			get
			{
				return this._statisticaldate;
			}
			set
			{
				this._statisticaldate = value;
			}
		}

		public int OrderUserNum
		{
			get
			{
				return this._orderusernum;
			}
			set
			{
				this._orderusernum = value;
			}
		}

		public int OrderNum
		{
			get
			{
				return this._ordernum;
			}
			set
			{
				this._ordernum = value;
			}
		}

		public int OrderProductQuantity
		{
			get
			{
				return this._orderproductquantity;
			}
			set
			{
				this._orderproductquantity = value;
			}
		}

		public decimal OrderAmount
		{
			get
			{
				return this._orderamount;
			}
			set
			{
				this._orderamount = value;
			}
		}

		public int PaymentUserNum
		{
			get
			{
				return this._paymentusernum;
			}
			set
			{
				this._paymentusernum = value;
			}
		}

		public int PaymentOrderNum
		{
			get
			{
				return this._paymentordernum;
			}
			set
			{
				this._paymentordernum = value;
			}
		}

		public int PaymentProductNum
		{
			get
			{
				return this._paymentproductnum;
			}
			set
			{
				this._paymentproductnum = value;
			}
		}

		public decimal PaymentAmount
		{
			get
			{
				return this._paymentamount;
			}
			set
			{
				this._paymentamount = value;
			}
		}

		public int PV
		{
			get
			{
				return this._pv;
			}
			set
			{
				this._pv = value;
			}
		}

		public int UV
		{
			get
			{
				return this._uv;
			}
			set
			{
				this._uv = value;
			}
		}

		public string ConversionRate
		{
			get
			{
				decimal num = default(decimal);
				if (this.PV > 0)
				{
					num = (decimal)this.OrderUserNum * 100m / (decimal)this.UV;
				}
				return num.F2ToString("f2");
			}
		}

		public string PaymentRate
		{
			get
			{
				decimal num = default(decimal);
				if (this.OrderUserNum > 0)
				{
					num = (decimal)this.PaymentUserNum * 100m / (decimal)this.OrderUserNum;
				}
				return num.F2ToString("f2");
			}
		}

		public string ClinchaDealRate
		{
			get
			{
				decimal num = default(decimal);
				if (this.PV > 0)
				{
					num = (decimal)this.PaymentUserNum * 100m / (decimal)this.UV;
				}
				return num.F2ToString("f2");
			}
		}

		public decimal RefundAmount
		{
			get
			{
				return this._refundamount;
			}
			set
			{
				this._refundamount = value;
			}
		}
	}
}
