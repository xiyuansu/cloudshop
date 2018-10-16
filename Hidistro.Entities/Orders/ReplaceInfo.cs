using System;

namespace Hidistro.Entities.Orders
{
	[TableName("Hishop_OrderReplace")]
	public class ReplaceInfo
	{
		[FieldType(FieldType.KeyField)]
		[FieldType(FieldType.IncrementField)]
		public int ReplaceId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string OrderId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime ApplyForTime
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string UserRemark
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public ReplaceStatus HandleStatus
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime? UserConfirmGoodsTime
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string AdminRemark
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
		public int Quantity
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string UserExpressCompanyName
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string UserExpressCompanyAbb
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string UserShipOrderNumber
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ExpressCompanyName
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ExpressCompanyAbb
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ShipOrderNumber
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int? StoreId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string UserCredentials
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ReplaceReason
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string AdminShipAddress
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string AdminShipTo
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string AdminCellPhone
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime? AgreedOrRefusedTime
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime? UserSendGoodsTime
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime? MerchantsConfirmGoodsTime
		{
			get;
			set;
		}

		public string ShopName
		{
			get;
			set;
		}

		public int UserId
		{
			get;
			set;
		}

		public string UserName
		{
			get;
			set;
		}

		public string OrderTotal
		{
			get;
			set;
		}

		public OrderStatus OrderStatus
		{
			get;
			set;
		}

		public int SupplierId
		{
			get;
			set;
		}

		public string ShipperName
		{
			get;
			set;
		}

		public string ReplaceStatusStr
		{
			get;
			set;
		}

		public string handleTimeStr
		{
			get;
			set;
		}

		public string OperText
		{
			get;
			set;
		}

		public string PayOrderId
		{
			get;
			set;
		}
	}
}
