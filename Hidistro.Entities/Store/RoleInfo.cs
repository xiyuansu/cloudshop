namespace Hidistro.Entities.Store
{
	[TableName("aspnet_Roles")]
	public class RoleInfo
	{
		[FieldType(FieldType.IncrementField)]
		[FieldType(FieldType.KeyField)]
		public int RoleId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string RoleName
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
	}
}
