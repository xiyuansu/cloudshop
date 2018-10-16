using System;

namespace Hidistro.Entities.Statistics
{
	[TableName("Hishop_DailyAccessStatistics")]
	public class SourceIpRecordsInfo
	{
		private long _id;

		private int _productid;

		private int _pagetype;

		private string _sourceip;

		private DateTime _recorddate;

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
		public string SourceIP
		{
			get
			{
				return this._sourceip;
			}
			set
			{
				this._sourceip = value;
			}
		}

		[FieldType(FieldType.CommonField)]
		public DateTime RecordDate
		{
			get
			{
				return this._recorddate;
			}
			set
			{
				this._recorddate = value;
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
	}
}
