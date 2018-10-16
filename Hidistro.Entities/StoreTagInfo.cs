namespace Hidistro.Entities
{
	[TableName("Hishop_StoreTags")]
	public class StoreTagInfo
	{
		[FieldType(FieldType.KeyField)]
		[FieldType(FieldType.IncrementField)]
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
		public string TagImgSrc
		{
			get;
			set;
		}

		public int RelationStore
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

		public int StoreId
		{
			get;
			set;
		}
	}
}
