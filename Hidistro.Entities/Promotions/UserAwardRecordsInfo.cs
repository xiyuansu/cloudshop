using System;

namespace Hidistro.Entities.Promotions
{
	[TableName("Hishop_UserAwardRecords")]
	public class UserAwardRecordsInfo
	{
		[FieldType(FieldType.KeyField)]
		[FieldType(FieldType.IncrementField)]
		public int Id
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int ActivityId
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
		public int AwardId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int AwardGrade
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int PrizeType
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int PrizeValue
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string AwardName
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string AwardPic
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
		public DateTime? AwardDate
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime CreateDate
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool IsDel
		{
			get;
			set;
		}
	}
}
