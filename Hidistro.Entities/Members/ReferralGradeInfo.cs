namespace Hidistro.Entities.Members
{
	[TableName("aspnet_ReferralGrades")]
	public class ReferralGradeInfo
	{
		[FieldType(FieldType.KeyField)]
		[FieldType(FieldType.IncrementField)]
		public int GradeId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Name
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
		public decimal CommissionThreshold
		{
			get;
			set;
		}

		public int ReferralCount
		{
			get;
			set;
		}
	}
}
