namespace Hidistro.Entities
{
	[TableName("aspnet_MemberTags")]
	public class MemberTagInfo
	{
		[FieldType(FieldType.IncrementField)]
		[FieldType(FieldType.KeyField)]
		public int TagId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string TagName
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int OrderCount
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public decimal OrderTotalAmount
		{
			get;
			set;
		}
	}
}
