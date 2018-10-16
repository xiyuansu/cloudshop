namespace Hidistro.Entities.Supplier
{
	[TableName("Hishop_Supplier")]
	public class SupplierInfo
	{
		[FieldType(FieldType.KeyField)]
		[FieldType(FieldType.IncrementField)]
		public int SupplierId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string SupplierName
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Picture
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int Status
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string TradePassword
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string TradePasswordSalt
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public decimal Balance
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ContactMan
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Tel
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int RegionId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Address
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string FullRegionPath
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string WXOpenId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Introduce
		{
			get;
			set;
		}
	}
}
