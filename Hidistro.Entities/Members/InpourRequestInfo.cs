using System;

namespace Hidistro.Entities.Members
{
	[TableName("Hishop_InpourRequest")]
	public class InpourRequestInfo
	{
		[FieldType(FieldType.KeyField)]
		public string InpourId
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
		public decimal InpourBlance
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int UserId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int PaymentId
		{
			get;
			set;
		}
	}
}
