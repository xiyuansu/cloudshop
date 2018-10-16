namespace Hidistro.Entities.Promotions
{
	[TableName("Hishop_ActivityAwardItem")]
	public class ActivityAwardItemInfo
	{
		[FieldType(FieldType.KeyField)]
		[FieldType(FieldType.IncrementField)]
		public int AwardId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int ActivityId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int AwardGrade
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int PrizeType
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int PrizeValue
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public decimal HitRate
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int AwardNum
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
	}
}
