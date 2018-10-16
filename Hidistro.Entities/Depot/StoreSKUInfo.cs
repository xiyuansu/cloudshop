namespace Hidistro.Entities.Depot
{
	[TableName("Hishop_StoreSKUs")]
	public class StoreSKUInfo
	{
		[FieldType(FieldType.CommonField)]
		public int ProductID
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string SkuId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int StoreId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int Stock
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int WarningStock
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int? FreezeStock
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public decimal StoreSalePrice
		{
			get;
			set;
		}
	}
}
