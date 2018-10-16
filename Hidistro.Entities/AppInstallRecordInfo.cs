namespace Hidistro.Entities
{
	[TableName("Hishop_AppInstallRecords")]
	public class AppInstallRecordInfo
	{
		[FieldType(FieldType.KeyField)]
		public string VID
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
	}
}
