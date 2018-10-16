using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal.Store
{
	public class RoleDao : BaseDao
	{
		public bool DeleteRolesPrivileges(int roleId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM [dbo].[aspnet_RolesPrivileges] WHERE RoleId=@RoleId;");
			base.database.AddInParameter(sqlStringCommand, "RoleId", DbType.Int32, roleId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool DeleteRole(int roleId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM [dbo].[aspnet_RolesPrivileges] WHERE RoleId=@RoleId;DELETE FROM [dbo].[aspnet_Roles] WHERE RoleId=@RoleId;");
			base.database.AddInParameter(sqlStringCommand, "RoleId", DbType.Int32, roleId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
	}
}
