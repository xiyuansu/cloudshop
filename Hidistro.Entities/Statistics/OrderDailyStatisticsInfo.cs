using System;

namespace Hidistro.Entities.Statistics
{
	[TableName("Hishop_OrderDailyStatistics")]
	public class OrderDailyStatisticsInfo
	{
		private long _id;

		private DateTime _statisticaldate;

		private int _year;

		private int _month;

		private int _day;

		private int _orderusernum;

		private int _ordernum;

		private int _orderproductquantity;

		private decimal _orderamount;

		private int _paymentusernum;

		private int _paymentordernum;

		private int _paymentproductnum;

		private decimal _paymentamount;

		private decimal _refundamount;

		[FieldType(FieldType.KeyField)]
		[FieldType(FieldType.IncrementField)]
		public long Id
		{
			get
			{
				return this._id;
			}
			set
			{
				this._id = value;
			}
		}

		[FieldType(FieldType.CommonField)]
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

		[FieldType(FieldType.CommonField)]
		public int Year
		{
			get
			{
				return this._year;
			}
			set
			{
				this._year = value;
			}
		}

		[FieldType(FieldType.CommonField)]
		public int Month
		{
			get
			{
				return this._month;
			}
			set
			{
				this._month = value;
			}
		}

		[FieldType(FieldType.CommonField)]
		public int Day
		{
			get
			{
				return this._day;
			}
			set
			{
				this._day = value;
			}
		}

		[FieldType(FieldType.CommonField)]
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

		[FieldType(FieldType.CommonField)]
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

		[FieldType(FieldType.CommonField)]
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

		[FieldType(FieldType.CommonField)]
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

		[FieldType(FieldType.CommonField)]
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

		[FieldType(FieldType.CommonField)]
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

		[FieldType(FieldType.CommonField)]
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

		[FieldType(FieldType.CommonField)]
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

		[FieldType(FieldType.CommonField)]
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
