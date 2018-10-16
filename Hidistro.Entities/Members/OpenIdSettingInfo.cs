namespace Hidistro.Entities.Members
{
	[TableName("aspnet_OpenIdSettings")]
	public class OpenIdSettingInfo
	{
		[FieldType(FieldType.KeyField)]
		public string OpenIdType
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Name
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
		public string Settings
		{
			get;
			set;
		}
	}
}
