namespace Hidistro.Entities
{
	[TableName("Hishop_AppVersionRecords")]
	public class AppVersionRecordInfo
	{
		[FieldType(FieldType.KeyField)]
		[FieldType(FieldType.IncrementField)]
		public int Id
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Device
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Version
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Description
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool IsForcibleUpgrade
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string UpgradeUrl
		{
			get;
			set;
		}
	}
}
