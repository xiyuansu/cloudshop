using Hidistro.Entities.Store;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal.Store
{
	public class RolesPrivilegeDao : BaseDao
	{
		public List<int> GetRolesPrivilegeByRoleId(int roleId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT [Privilege] FROM [dbo].[aspnet_RolesPrivileges] WHERE RoleId=@RoleId");
			base.database.AddInParameter(sqlStringCommand, "RoleId", DbType.Int32, roleId);
			List<int> list = new List<int>();
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add((int)((IDataRecord)dataReader)["Privilege"]);
				}
			}
			return list;
		}

		public List<RolesPrivilegeInfo> GetRolesPrivilegeByPrivilege(int privilege)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT [Privilege],[RoleId] FROM [dbo].[aspnet_RolesPrivileges] WHERE Privilege=@Privilege");
			base.database.AddInParameter(sqlStringCommand, "Privilege", DbType.Int32, privilege);
			List<RolesPrivilegeInfo> list = new List<RolesPrivilegeInfo>();
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					RolesPrivilegeInfo item = RolesPrivilegeDao.PopulateRolesPrivilege(dataReader);
					list.Add(item);
				}
			}
			return list;
		}

		public static RolesPrivilegeInfo PopulateRolesPrivilege(IDataRecord reader)
		{
			if (reader == null)
			{
				return null;
			}
			RolesPrivilegeInfo rolesPrivilegeInfo = new RolesPrivilegeInfo();
			rolesPrivilegeInfo.RoleId = (int)reader["RoleId"];
			rolesPrivilegeInfo.Privilege = (int)reader["Privilege"];
			return rolesPrivilegeInfo;
		}
	}
}
