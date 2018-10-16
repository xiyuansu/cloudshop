using System;

namespace Hidistro.Entities
{
	[TableName("Hishop_AppLotteryDraw")]
	public class AppLotteryDraw
	{
		[FieldType(FieldType.IncrementField)]
		[FieldType(FieldType.KeyField)]
		public int Id
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
		public string Content
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime CreatTime
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int DrawType
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int DrawValue
		{
			get;
			set;
		}
	}
}
