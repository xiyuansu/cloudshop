namespace Hidistro.Entities.Commodities
{
	[TableName("Hishop_ProductSpecificationImages")]
	public class ProductSpecificationImageInfo
	{
		[FieldType(FieldType.KeyField)]
		public int ProductId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int AttributeId
		{
			get;
			set;
		}

		[FieldType(FieldType.KeyField)]
		public int ValueId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ImageUrl
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ThumbnailUrl40
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ThumbnailUrl410
		{
			get;
			set;
		}
	}
}
