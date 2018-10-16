using System;

namespace Hidistro.Entities.Depot
{
	[TableName("Hishop_StoreCollections")]
	public class StoreCollectionInfo
	{
		[FieldType(FieldType.KeyField)]
		public string SerialNumber
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public decimal PayAmount
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime PayTime
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int? PaymentTypeId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string PaymentTypeName
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string GateWayOrderId
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
		public int Status
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
		public int? UserId
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
		public DateTime CreateTime
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
		public int OrderType
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
		public DateTime? RefundTime
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string GateWay
		{
			get;
			set;
		}
	}
}
