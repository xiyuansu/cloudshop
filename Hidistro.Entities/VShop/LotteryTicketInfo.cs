using System;

namespace Hidistro.Entities.VShop
{
	[TableName("Vshop_LotteryActivity")]
	public class LotteryTicketInfo : LotteryActivityInfo
	{
		[FieldType(FieldType.CommonField)]
		public string GradeIds
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int MinValue
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string InvitationCode
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime OpenTime
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool IsOpened
		{
			get;
			set;
		}
	}
}
