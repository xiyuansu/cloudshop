using System;

namespace Hidistro.Entities.Depot
{
	[TableName("Hishop_StoreStockLog")]
	public class StoreStockLogInfo
	{
		[FieldType(FieldType.IncrementField)]
		[FieldType(FieldType.KeyField)]
		public int Id
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
		public DateTime ChangeTime
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int ProductId
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
		public string Content
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Remark
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Operator
		{
			get;
			set;
		}
	}
}
