using System;

namespace Hidistro.Entities.Statistics
{
	[TableName("Hishop_ProductDailyAccessStatistics")]
	public class ProductDailyAccessStatisticsInfo
	{
		private int _id;

		private DateTime _statisticaldate;

		private int _year;

		private int _month;

		private int _day;

		private int _activitytype;

		private int _productid;

		private int _sourceid;

		private int _pv;

		private int _uv;

		private int _paymentnum;

		private int _salequantity;

		private decimal _saleamount;

		[FieldType(FieldType.KeyField)]
		[FieldType(FieldType.IncrementField)]
		public int Id
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
		public int ActivityType
		{
			get
			{
				return this._activitytype;
			}
			set
			{
				this._activitytype = value;
			}
		}

		[FieldType(FieldType.CommonField)]
		public int ProductId
		{
			get
			{
				return this._productid;
			}
			set
			{
				this._productid = value;
			}
		}

		[FieldType(FieldType.CommonField)]
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

		[FieldType(FieldType.CommonField)]
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

		[FieldType(FieldType.CommonField)]
		public int PaymentNum
		{
			get
			{
				return this._paymentnum;
			}
			set
			{
				this._paymentnum = value;
			}
		}

		[FieldType(FieldType.CommonField)]
		public int SaleQuantity
		{
			get
			{
				return this._salequantity;
			}
			set
			{
				this._salequantity = value;
			}
		}

		[FieldType(FieldType.CommonField)]
		public decimal SaleAmount
		{
			get
			{
				return this._saleamount;
			}
			set
			{
				this._saleamount = value;
			}
		}
	}
}
