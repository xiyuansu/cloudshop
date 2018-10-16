using System;

namespace Hidistro.Entities.Promotions
{
	[TableName("Hishop_CombinationBuy")]
	public class CombinationBuyInfo
	{
		[FieldType(FieldType.KeyField)]
		[FieldType(FieldType.IncrementField)]
		public int CombinationId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int MainProductId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string OtherProductIds
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime StartDate
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime EndDate
		{
			get;
			set;
		}
	}
}
