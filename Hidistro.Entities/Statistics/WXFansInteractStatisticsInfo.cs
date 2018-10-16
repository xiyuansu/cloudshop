using System;

namespace Hidistro.Entities.Statistics
{
	[TableName("Hishop_WXFansInteractStatistics")]
	public class WXFansInteractStatisticsInfo
	{
		[FieldType(FieldType.KeyField)]
		[FieldType(FieldType.IncrementField)]
		public long Id
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int MsgSendNumbers
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int MenuClickTimes
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int MsgSendTimes
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int MenuClickNumbers
		{
			get;
			set;
		}

		public int InteractNumbers
		{
			get;
			set;
		}

		public int InteractTimes
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
