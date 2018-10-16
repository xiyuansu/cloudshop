namespace Hidistro.Entities.Orders
{
	[TableName("Hishop_OrderGifts")]
	public class OrderGiftInfo
	{
		[FieldType(FieldType.CommonField)]
		public string OrderId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int GiftId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string GiftName
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public decimal CostPrice
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int Quantity
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ThumbnailsUrl
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int PromoteType
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

		public int NeedPoint
		{
			get;
			set;
		}
	}
}
