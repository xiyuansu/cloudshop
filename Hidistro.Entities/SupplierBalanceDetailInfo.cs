using System;

namespace Hidistro.Entities
{
	[TableName("Hishop_SupplierBalanceDetails")]
	public class SupplierBalanceDetailInfo
	{
		[FieldType(FieldType.IncrementField)]
		[FieldType(FieldType.KeyField)]
		public long JournalNumber
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
		public DateTime TradeDate
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public SupplierTradeTypes TradeType
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
		public decimal Income
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public decimal Expenses
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public decimal Balance
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
		public string ManagerUserName
		{
			get;
			set;
		}
	}
}
