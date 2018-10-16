using System;

namespace Hidistro.Entities.HOP
{
	[TableName("Taobao_Products")]
	public class TaobaoProductInfo
	{
		[FieldType(FieldType.CommonField)]
		public long Cid
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string StuffStatus
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
		public string ProTitle
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public long Num
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string LocationState
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string LocationCity
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string FreightPayer
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public decimal PostFee
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public decimal ExpressFee
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public decimal EMSFee
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool HasInvoice
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool HasWarranty
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool HasDiscount
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public long ValidThru
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime ListTime
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string PropertyAlias
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string InputPids
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string InputStr
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string SkuProperties
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string SkuQuantities
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string SkuPrices
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string SkuOuterIds
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string FoodAttributes
		{
			get;
			set;
		}
	}
}
