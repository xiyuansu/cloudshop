namespace Hidistro.Entities.Promotions
{
	[TableName("Hishop_CountDownSku")]
	public class CountDownSkuInfo
	{
		[FieldType(FieldType.CommonField)]
		public int CountDownId
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
		public decimal SalePrice
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int TotalCount
		{
			get;
			set;
		}

		[FieldType(FieldType.KeyField)]
		[FieldType(FieldType.IncrementField)]
		public int CountDownSkuId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int BoughtCount
		{
			get;
			set;
		}

		public int ActivityTotal
		{
			get;
			set;
		}

		public decimal OldSalePrice
		{
			get;
			set;
		}
	}
}
