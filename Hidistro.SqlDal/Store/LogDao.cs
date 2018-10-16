using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Store;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Store
{
	public class LogDao : BaseDao
	{
		public bool DeleteAllLogs()
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("TRUNCATE TABLE Hishop_Logs");
			try
			{
				base.database.ExecuteNonQuery(sqlStringCommand);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public int DeleteLogs(string strIds)
		{
			if (strIds.Length <= 0)
			{
				return 0;
			}
			string query = $"DELETE FROM Hishop_Logs WHERE LogId IN ({strIds})";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			return base.database.ExecuteNonQuery(sqlStringCommand);
		}

		public DbQueryResult GetLogs(OperationLogQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			Pagination page = query.Page;
			if (query.FromDate.HasValue)
			{
				stringBuilder.AppendFormat("AddedTime >= '{0}'", DataHelper.GetSafeDateTimeFormat(query.FromDate.Value));
			}
			if (query.ToDate.HasValue)
			{
				if (!string.IsNullOrEmpty(stringBuilder.ToString()))
				{
					stringBuilder.Append(" AND");
				}
				stringBuilder.AppendFormat(" AddedTime <= '{0}'", DataHelper.GetSafeDateTimeFormat(query.ToDate.Value));
			}
			if (!string.IsNullOrEmpty(query.OperationUserName))
			{
				if (!string.IsNullOrEmpty(stringBuilder.ToString()))
				{
					stringBuilder.Append(" AND");
				}
				stringBuilder.AppendFormat(" UserName = '{0}'", DataHelper.CleanSearchString(query.OperationUserName));
			}
			return DataHelper.PagingByTopsort(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "Hishop_Logs", "LogId", stringBuilder.ToString(), "*");
		}

		public IList<string> GetOperationUserNames()
		{
			IList<string> list = new List<string>();
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT DISTINCT UserName FROM Hishop_Logs");
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(((IDataRecord)dataReader)["UserName"].ToString());
				}
			}
			return list;
		}
	}
}
