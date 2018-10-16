namespace Hidistro.Entities.Depot
{
	[TableName("Hishop_StoreMarketingImages")]
	public class StoreMarketingImagesInfo
	{
		[FieldType(FieldType.CommonField)]
		public int StoreId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int ImageId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ProductIds
		{
			get;
			set;
		}
	}
}
