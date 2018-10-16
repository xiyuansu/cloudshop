using System;

namespace Hidistro.Entities.Promotions
{
	[TableName("Hishop_Activity")]
	public class ActivityInfo
	{
		[FieldType(FieldType.KeyField)]
		[FieldType(FieldType.IncrementField)]
		public int ActivityId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ActivityName
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int ActivityType
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Description
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime StartDate
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime EndDate
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ShareDetail
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string SharePic
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int ResetType
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int FreeTimes
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int ConsumptionIntegral
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime CreateDate
		{
			get;
			set;
		}
	}
}
