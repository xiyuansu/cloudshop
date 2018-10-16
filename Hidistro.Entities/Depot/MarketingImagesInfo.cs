namespace Hidistro.Entities.Depot
{
	[TableName("Hishop_MarketingImages")]
	public class MarketingImagesInfo
	{
		[FieldType(FieldType.KeyField)]
		[FieldType(FieldType.IncrementField)]
		public int ImageId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ImageName
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
		public string Description
		{
			get;
			set;
		}
	}
}
