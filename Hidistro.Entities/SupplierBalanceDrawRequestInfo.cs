using System;

namespace Hidistro.Entities
{
	[TableName("Hishop_SupplierBalanceDrawRequest")]
	public class SupplierBalanceDrawRequestInfo
	{
		[FieldType(FieldType.IncrementField)]
		[FieldType(FieldType.KeyField)]
		public int ID
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int SupplierId
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
		public DateTime RequestTime
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public decimal Amount
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string AccountName
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
		public string MerchantCode
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
		public bool IsWeixin
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool IsAlipay
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string AlipayRealName
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string AlipayCode
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string RequestState
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string RequestError
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool? IsPass
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ManagerRemark
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime? AccountDate
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ManagerUserName
		{
			get;
			set;
		}

		public string DrawType
		{
			get;
			set;
		}

		public string ReceiverName
		{
			get;
			set;
		}

		public string ReceiverID
		{
			get;
			set;
		}

		public string StateStr
		{
			get;
			set;
		}

		public string AccountDateStr
		{
			get;
			set;
		}

		public string SupplierName
		{
			get;
			set;
		}
	}
}
