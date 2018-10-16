namespace Hidistro.Entities.Promotions
{
	[TableName("Hishop_CombinationBuySKU")]
	public class CombinationBuySKUInfo
	{
		[FieldType(FieldType.KeyField)]
		public int CombinationId
		{
			get;
			set;
		}

		[FieldType(FieldType.KeyField)]
		public int ProductId
		{
			get;
			set;
		}

		[FieldType(FieldType.KeyField)]
		public string SkuId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public decimal CombinationPrice
		{
			get;
			set;
		}
	}
}
