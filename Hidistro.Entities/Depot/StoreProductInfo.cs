using System;

namespace Hidistro.Entities.Depot
{
	[TableName("Hishop_StoreProducts")]
	public class StoreProductInfo
	{
		[FieldType(FieldType.KeyField)]
		public int StoreId
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

		[FieldType(FieldType.CommonField)]
		public int SaleCounts
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int SaleStatus
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime UpdateTime
		{
			get;
			set;
		}
	}
}
