using System;

namespace Hidistro.Entities.Statistics
{
	[TableName("Hishop_DailyAccessStatistics")]
	public class DailyAccessStatisticsInfo
	{
		private long _id;

		private DateTime _statisticaldate;

		private int _year;

		private int _month;

		private int _day;

		private int _pagetype;

		private int _sourceid;

		private int _pv;

		private int _uv;

		private int _storeId;

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
		public int PageType
		{
			get
			{
				return this._pagetype;
			}
			set
			{
				this._pagetype = value;
			}
		}

		[FieldType(FieldType.CommonField)]
		public int SourceId
		{
			get
			{
				return this._sourceid;
			}
			set
			{
				this._sourceid = value;
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
		public int StoreId
		{
			get
			{
				return this._storeId;
			}
			set
			{
				this._storeId = value;
			}
		}
	}
}
