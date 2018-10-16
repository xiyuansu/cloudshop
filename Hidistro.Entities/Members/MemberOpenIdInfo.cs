namespace Hidistro.Entities.Members
{
	[TableName("aspnet_MemberOpenIds")]
	public class MemberOpenIdInfo
	{
		[FieldType(FieldType.KeyField)]
		public int UserId
		{
			get;
			set;
		}

		[FieldType(FieldType.KeyField)]
		public string OpenIdType
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string OpenId
		{
			get;
			set;
		}
	}
}
