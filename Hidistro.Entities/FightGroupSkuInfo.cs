namespace Hidistro.Entities
{
	[TableName("Hishop_FightGroupSkus")]
	public class FightGroupSkuInfo
	{
		[FieldType(FieldType.IncrementField)]
		[FieldType(FieldType.KeyField)]
		public int FightGroupSkuId
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

		[FieldType(FieldType.CommonField)]
		public long FightGroupActivityId
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

		public int Stock
		{
			get;
			set;
		}
	}
}
