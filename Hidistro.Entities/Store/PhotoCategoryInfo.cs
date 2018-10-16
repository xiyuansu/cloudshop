namespace Hidistro.Entities.Store
{
	[TableName("Hishop_PhotoCategories")]
	public class PhotoCategoryInfo
	{
		[FieldType(FieldType.IncrementField)]
		[FieldType(FieldType.KeyField)]
		public int CategoryId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string CategoryName
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

		[FieldType(FieldType.CommonField)]
		public int SupplierId
		{
			get;
			set;
		}

		public int PhotoCounts
		{
			get;
			set;
		}
	}
}
