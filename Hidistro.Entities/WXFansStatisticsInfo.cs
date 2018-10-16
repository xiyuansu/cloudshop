using System;

namespace Hidistro.Entities
{
	[TableName("Hishop_WXFansStatistics")]
	public class WXFansStatisticsInfo
	{
		[FieldType(FieldType.KeyField)]
		[FieldType(FieldType.IncrementField)]
		public long Id
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int NewUser
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int CancelUser
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int CumulateUser
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int NetGrowthUser
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime StatisticalDate
		{
			get;
			set;
		}
	}
}
