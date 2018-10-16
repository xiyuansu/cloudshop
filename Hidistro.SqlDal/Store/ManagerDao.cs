using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Store
{
	public class ManagerDao : BaseDao
	{
		public ManagerInfo GetManagerBySessionId(string sessionId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT [ManagerId],[RoleId],[UserName],[Password],[PasswordSalt],[CreateDate],[StoreId],[SessionId],[Status],[ContactInfo],[HeadImage] FROM [dbo].[aspnet_Managers] WHERE SessionId = @SessionId");
			base.database.AddInParameter(sqlStringCommand, "SessionId", DbType.String, sessionId);
			ManagerInfo result = null;
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToModel<ManagerInfo>(objReader);
			}
			return result;
		}

		public DbQueryResult GetManagers(ManagerQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("UserName LIKE '%{0}%' AND RoleId !={1} AND RoleId !={2} AND RoleId !={3}", DataHelper.CleanSearchString(query.UserName), -1, -3, -2);
			if (query.RoleId.HasValue)
			{
				stringBuilder.AppendFormat(" AND  RoleId = {0}", query.RoleId.Value);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_Manager", "ManagerId", stringBuilder.ToString(), "*");
		}

		public IList<int> GetManagerIdsByRoleId(int roleId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand($"SELECT ManagerId FROM aspnet_Managers WHERE RoleId = {roleId}");
			IList<int> list = new List<int>();
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(((IDataRecord)dataReader)["ManagerId"].ToInt(0));
				}
			}
			return list;
		}

		public ManagerInfo FindManagerByUsername(string userName)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM [dbo].[aspnet_Managers] WHERE UserName=@UserName");
			base.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, userName);
			ManagerInfo result = null;
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToModel<ManagerInfo>(objReader);
			}
			return result;
		}

		public ManagerInfo FindManagerByStoreId(int storeId, SystemRoles role)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM [dbo].[aspnet_Managers] WHERE StoreId = @StoreId AND RoleId = @RoleId");
			base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, storeId);
			base.database.AddInParameter(sqlStringCommand, "RoleId", DbType.Int32, (int)role);
			ManagerInfo result = null;
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToModel<ManagerInfo>(objReader);
			}
			return result;
		}

		public ManagerInfo FindManagerByStoreIdAndRoleId(int storeId, int roleId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM [dbo].[aspnet_Managers] WHERE StoreId=@StoreId and RoleId=@RoleId");
			base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, storeId);
			base.database.AddInParameter(sqlStringCommand, "RoleId", DbType.Int32, roleId);
			ManagerInfo result = null;
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToModel<ManagerInfo>(objReader);
			}
			return result;
		}

		public bool UpdateAdminSessionId(int managerId, string sessionId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE [dbo].[aspnet_Managers] SET SessionId = @SessionId WHERE ManagerId=@ManagerId");
			base.database.AddInParameter(sqlStringCommand, "ManagerId", DbType.Int32, managerId);
			base.database.AddInParameter(sqlStringCommand, "SessionId", DbType.String, sessionId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public StoresInfo GetStoreInfoBySessionId(string sessionId)
		{
			StoresInfo result = null;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM [dbo].[Hishop_Stores] WHERE StoreId = (SELECT TOP 1 StoreId FROM aspnet_Managers WHERE SessionId = @SessionId)");
			base.database.AddInParameter(sqlStringCommand, "SessionId", DbType.String, sessionId);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToModel<StoresInfo>(objReader);
			}
			return result;
		}

		public DbQueryResult GetManagerList(ManagerQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string table = "(SELECT A.StoreId,a.RoleId, A.ManagerId,A.UserName,COUNT(B.UserId) AS MemberCount,ISNULL((SELECT SUM(Expenditure) FROM aspnet_Members AS ce WHERE ce.StoreId = A.StoreId and A.ManagerId = ce.ShoppingGuiderId),0) AS ConsumeTotals FROM aspnet_Managers AS A LEFT JOIN aspnet_Members AS B ON A.ManagerId = B.ShoppingGuiderId GROUP BY A.ManagerId,A.StoreId,a.RoleId, A.UserName) AS Managers_List";
			stringBuilder.Append(" 1=1 AND (RoleId=-3 OR RoleId=-1) ");
			if (!string.IsNullOrWhiteSpace(query.UserName))
			{
				stringBuilder.AppendFormat(" AND UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.UserName));
			}
			if (query.StoreId.HasValue && query.StoreId > 0)
			{
				stringBuilder.AppendFormat(" AND StoreId = {0}", query.StoreId.Value);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, table, "Managers_List.ManagerId", stringBuilder.ToString(), "*");
		}

		public IList<ManagerInfo> GetManagersIdAndNameByRoleId(int storeId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand($"SELECT ManagerId,UserName FROM aspnet_Managers WHERE StoreId={storeId}  ");
			IList<ManagerInfo> list = new List<ManagerInfo>();
			IDataReader objReader = base.database.ExecuteReader(sqlStringCommand);
			return DataHelper.ReaderToList<ManagerInfo>(objReader);
		}

		public IList<MemberClientTokenInfo> GetClientIdAndTokenByStoreId(int storeId, string storeIds)
		{
			string text = "SELECT ClientId,Token FROM aspnet_Managers";
			if (storeId > 0)
			{
				text = text + " WHERE StoreId = " + storeId;
			}
			else if (!string.IsNullOrWhiteSpace(storeIds))
			{
				text = text + " WHERE StoreId in (" + storeIds + ")";
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(text);
			IList<MemberClientTokenInfo> result = new List<MemberClientTokenInfo>();
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<MemberClientTokenInfo>(objReader);
			}
			return result;
		}

		public bool SaveClientIdAndToken(string clientId, string token, int managerId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("Update aspnet_Managers SET ClientId=@ClientId,Token=@Token WHERE ManagerId=@ManagerId");
			base.database.AddInParameter(sqlStringCommand, "ClientId", DbType.String, clientId);
			base.database.AddInParameter(sqlStringCommand, "Token", DbType.String, token);
			base.database.AddInParameter(sqlStringCommand, "ManagerId", DbType.Int32, managerId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
	}
}
