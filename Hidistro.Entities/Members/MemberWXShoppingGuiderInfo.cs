namespace Hidistro.Entities.Members
{
	[TableName("aspnet_MemberWXShoppingGuider")]
	public class MemberWXShoppingGuiderInfo
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
		public int ShoppingGuiderId
		{
			get;
			set;
		}
	}
}
