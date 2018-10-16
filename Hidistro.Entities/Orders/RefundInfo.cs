using System;

namespace Hidistro.Entities.Orders
{
	[TableName("Hishop_OrderRefund")]
	public class RefundInfo
	{
		[FieldType(FieldType.KeyField)]
		[FieldType(FieldType.IncrementField)]
		public int RefundId
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
		public RefundStatus HandleStatus
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
		public int? StoreId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string RefundReason
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
		public bool IsServiceProduct
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
		public string ExceptionInfo
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ValidCodes
		{
			get;
			set;
		}
	}
}
