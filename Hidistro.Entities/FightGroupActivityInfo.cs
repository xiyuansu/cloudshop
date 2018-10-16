using System;

namespace Hidistro.Entities
{
	[TableName("Hishop_FightGroupActivities")]
	public class FightGroupActivityInfo
	{
		[FieldType(FieldType.IncrementField)]
		[FieldType(FieldType.KeyField)]
		public int FightGroupActivityId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int ProductId
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
		public int JoinNumber
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int MaxCount
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Icon
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int LimitedHour
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ProductName
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ShareTitle
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ShareContent
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int DisplaySequence
		{
			get;
			set;
		}
	}
}
