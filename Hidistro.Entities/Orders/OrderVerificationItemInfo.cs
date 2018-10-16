using System;

namespace Hidistro.Entities.Orders
{
	[TableName("Hishop_OrderVerificationItems")]
	public class OrderVerificationItemInfo
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
		public string OrderId
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
		public string VerificationPassword
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int VerificationStatus
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int? ManagerId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string UserName
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime? VerificationDate
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime? RefundDate
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime? CreateDate
		{
			get;
			set;
		}
	}
}
