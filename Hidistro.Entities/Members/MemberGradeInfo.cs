namespace Hidistro.Entities.Members
{
	[TableName("aspnet_MemberGrades")]
	public class MemberGradeInfo
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
		public int Points
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool IsDefault
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int Discount
		{
			get;
			set;
		}
	}
}
