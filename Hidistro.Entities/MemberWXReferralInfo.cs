namespace Hidistro.Entities
{
	[TableName("aspnet_MemberWXReferral")]
	public class MemberWXReferralInfo
	{
		[FieldType(FieldType.IncrementField)]
		[FieldType(FieldType.KeyField)]
		public int Id
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

		[FieldType(FieldType.CommonField)]
		public int ReferralUserId
		{
			get;
			set;
		}
	}
}
