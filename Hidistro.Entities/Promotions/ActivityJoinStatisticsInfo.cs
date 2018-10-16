using System;

namespace Hidistro.Entities.Promotions
{
	[TableName("Hishop_ActivityJoinStatistics")]
	public class ActivityJoinStatisticsInfo
	{
		[FieldType(FieldType.CommonField)]
		[FieldType(FieldType.IncrementField)]
		public int Id
		{
			get;
			set;
		}

		[FieldType(FieldType.KeyField)]
		public int ActivityId
		{
			get;
			set;
		}

		[FieldType(FieldType.KeyField)]
		public int UserId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int JoinNum
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int IntegralTotal
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int IntegralNum
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int WinningNum
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime LastJoinDate
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int FreeNum
		{
			get;
			set;
		}
	}
}
