namespace Hidistro.Entities
{
	[TableName("Hishop_AppPushRecordUserRead")]
	public class AppPushRecordUserReadInfo
	{
		[FieldType(FieldType.IncrementField)]
		[FieldType(FieldType.KeyField)]
		public int PushRecordUserReadId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int PushRecordId
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
	}
}
