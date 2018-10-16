namespace Hidistro.Entities.Store
{
	[TableName("aspnet_RolesPrivileges")]
	public class RolesPrivilegeInfo
	{
		[FieldType(FieldType.KeyField)]
		public int Privilege
		{
			get;
			set;
		}

		[FieldType(FieldType.KeyField)]
		public int RoleId
		{
			get;
			set;
		}
	}
}
