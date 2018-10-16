using System;

namespace Hidistro.Entities.Orders
{
	[TableName("Hishop_OrderReturns")]
	public class ReturnInfo
	{
		[FieldType(FieldType.KeyField)]
		[FieldType(FieldType.IncrementField)]
		public int ReturnId
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
		public RefundTypes RefundType
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public decimal RefundAmount
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
		public ReturnStatus HandleStatus
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime? FinishTime
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
		public string Operator
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string RefundOrderId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string RefundGateWay
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
		public string ReturnReason
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
		public string BankName
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string BankAccountName
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string BankAccountNo
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
		public DateTime? ConfirmGoodsTime
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public AfterSaleTypes AfterSaleType
		{
			get;
			set;
		}

		public string ReturnStatusStr
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

		[FieldType(FieldType.CommonField)]
		public string ExceptionInfo
		{
			get;
			set;
		}

		public string GateWay
		{
			get;
			set;
		}
	}
}
