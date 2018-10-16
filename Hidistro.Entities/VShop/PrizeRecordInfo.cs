using System;

namespace Hidistro.Entities.VShop
{
	[TableName("Vshop_PrizeRecord")]
	public class PrizeRecordInfo
	{
		[FieldType(FieldType.IncrementField)]
		[FieldType(FieldType.KeyField)]
		public int RecordId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int ActivityID
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime? PrizeTime
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int UserID
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string PrizeName
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Prizelevel
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool IsPrize
		{
			get;
			set;
		}

		public string ActivityName
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string RealName
		{
			get;
			set;
		}

		public string UserName
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string CellPhone
		{
			get;
			set;
		}
	}
}
