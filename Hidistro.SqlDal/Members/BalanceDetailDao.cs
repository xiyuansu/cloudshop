using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Members
{
	public class BalanceDetailDao : BaseDao
	{
		public DbQueryResult GetBalanceDetails(BalanceDetailQuery query)
		{
			if (query == null)
			{
				return new DbQueryResult();
			}
			DbQueryResult dbQueryResult = new DbQueryResult();
			StringBuilder stringBuilder = new StringBuilder();
			string text = this.BuildBalanceDetailsQuery(query);
			stringBuilder.AppendFormat("SELECT TOP {0} *, isnull (Income, 0) + isnull (Expenses, 0) Amount ,CASE WHEN TradeType = 1 THEN '自助充值' WHEN TradeType = 2 THEN '后台加款' WHEN TradeType = 3 THEN '消费' WHEN TradeType = 4 THEN '提现' WHEN TradeType = 5 THEN '退款' WHEN TradeType = 7 THEN '退货' WHEN TradeType = 8 THEN '分销奖励' WHEN TradeType = 9 THEN '充值赠送' ELSE '其他' END TradeTypeText ", query.PageSize);
			stringBuilder.Append(" FROM Hishop_BalanceDetails B where 0=0 ");
			if (query.PageIndex == 1)
			{
				stringBuilder.AppendFormat("{0} ORDER BY JournalNumber DESC", text);
			}
			else
			{
				stringBuilder.AppendFormat(" and JournalNumber < (select min(JournalNumber) from (select top {0} JournalNumber from Hishop_BalanceDetails where 0=0 {1} ORDER BY JournalNumber DESC ) as tbltemp) {1} ORDER BY JournalNumber DESC", (query.PageIndex - 1) * query.PageSize, text);
			}
			if (query.IsCount)
			{
				stringBuilder.AppendFormat(";select count(JournalNumber) as Total from Hishop_BalanceDetails where 0=0 {0}", text);
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				dbQueryResult.Data = DataHelper.ConverDataReaderToDataTable(dataReader);
				if (query.IsCount && dataReader.NextResult())
				{
					dataReader.Read();
					dbQueryResult.TotalRecords = dataReader.GetInt32(0);
				}
			}
			return dbQueryResult;
		}

		public DateTime? GetUserBalanceLastActivityTime(int userId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT TOP 1 [TradeDate] FROM [dbo].[Hishop_BalanceDetails] WHERE Expenses IS NOT NULL AND [UserId]=@UserId AND TradeType=@TradeType ORDER BY [TradeDate] DESC");
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			base.database.AddInParameter(sqlStringCommand, "TradeType", DbType.Int32, 4);
			return (DateTime?)(object)(base.database.ExecuteScalar(sqlStringCommand) as DateTime?);
		}

		public DbQueryResult GetMemberBlanceList(MemberQuery query)
		{
			string text = " 1 = 1 ";
			if (!string.IsNullOrEmpty(query.UserName))
			{
				text += $"AND UserId IN (SELECT UserId FROM aspnet_Members WHERE UserName LIKE '%{DataHelper.CleanSearchString(query.UserName)}%')";
			}
			if (!string.IsNullOrEmpty(query.RealName))
			{
				text += string.Format(" AND (REPLACE(RealName,' ','') LIKE '%{0}%' OR REPLACE(NickName,' ','') LIKE '%{0}%')", DataHelper.CleanSearchString(query.RealName));
			}
			if (string.IsNullOrEmpty(query.SortBy))
			{
				query.SortBy = "Balance";
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "aspnet_Members", "UserId", text.ToString(), "*");
		}

		public IList<MemberInfo> GetMemberBlanceListNoPage(MemberQuery query)
		{
			IList<MemberInfo> result = new List<MemberInfo>();
			string text = " 1 = 1 ";
			if (!string.IsNullOrEmpty(query.UserName))
			{
				text += $"AND UserId IN (SELECT UserId FROM aspnet_Members WHERE UserName LIKE '%{DataHelper.CleanSearchString(query.UserName)}%')";
			}
			if (!string.IsNullOrEmpty(query.RealName))
			{
				text += string.Format(" AND (REPLACE(RealName,' ','') LIKE '%{0}%' OR REPLACE(NickName,' ','') LIKE '%{0}%')", DataHelper.CleanSearchString(query.RealName));
			}
			if (string.IsNullOrEmpty(query.SortBy))
			{
				query.SortBy = "Balance";
			}
			string query2 = "SELECT UserName,RealName,NickName,Balance,RequestBalance FROM aspnet_Members WHERE " + text + " ORDER BY " + query.SortBy + " DESC";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query2);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<MemberInfo>(objReader);
			}
			return result;
		}

		public DbQueryResult GetBalanceDetailsNoPage(BalanceDetailQuery query)
		{
			DbQueryResult dbQueryResult = new DbQueryResult();
			StringBuilder stringBuilder = new StringBuilder();
			string arg = this.BuildBalanceDetailsQuery(query);
			stringBuilder.Append("SELECT * FROM Hishop_BalanceDetails WHERE 0=0 ");
			stringBuilder.AppendFormat("{0} ORDER BY JournalNumber DESC", arg);
			if (query.IsCount)
			{
				stringBuilder.AppendFormat(";select count(JournalNumber) as Total from Hishop_BalanceDetails where 0=0 {0}", arg);
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				dbQueryResult.Data = DataHelper.ConverDataReaderToDataTable(dataReader);
				if (query.IsCount && dataReader.NextResult())
				{
					dataReader.Read();
					dbQueryResult.TotalRecords = dataReader.GetInt32(0);
				}
			}
			return dbQueryResult;
		}

		public IList<BalanceDetailInfo> GetMemberBalanceDetailsNoPage(BalanceDetailQuery query)
		{
			IList<BalanceDetailInfo> result = new List<BalanceDetailInfo>();
			StringBuilder stringBuilder = new StringBuilder();
			string arg = this.BuildBalanceDetailsQuery(query);
			stringBuilder.Append("SELECT * FROM Hishop_BalanceDetails WHERE 0=0 ");
			stringBuilder.AppendFormat("{0} ORDER BY JournalNumber DESC", arg);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<BalanceDetailInfo>(objReader);
			}
			return result;
		}

		public DbQueryResult GetBalanceDrawRequests(BalanceDrawRequestQuery query, bool IsAdmin = true)
		{
			DbQueryResult dbQueryResult = new DbQueryResult();
			StringBuilder stringBuilder = new StringBuilder();
			string text = this.BuildBalanceDrawRequestQuery(query);
			stringBuilder.AppendFormat("select top {0} *", query.PageSize);
			stringBuilder.Append(" from Hishop_BalanceDrawRequest B where 0=0 ");
			if (IsAdmin)
			{
				text += " AND IsPass IS NULL ";
			}
			switch (query.DrawRequestType)
			{
			case 1:
				text += " AND IsWeixin <> 1 AND IsAlipay <> 1";
				break;
			case 2:
				text += " AND IsWeixin = 1 ";
				break;
			case 3:
				text += " AND IsAlipay = 1";
				break;
			}
			if (query.PageIndex == 1)
			{
				stringBuilder.AppendFormat("{0} ORDER BY RequestTime DESC", text);
			}
			else
			{
				stringBuilder.AppendFormat(" and RequestTime < (select min(RequestTime) from (select top {0} RequestTime from Hishop_BalanceDrawRequest where 0=0 {1} ORDER BY RequestTime DESC ) as tbltemp) {1} ORDER BY RequestTime DESC", (query.PageIndex - 1) * query.PageSize, text);
			}
			if (query.IsCount)
			{
				stringBuilder.AppendFormat(";select count(*) as Total from Hishop_BalanceDrawRequest where 0=0 {0}", text);
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				dbQueryResult.Data = DataHelper.ConverDataReaderToDataTable(dataReader);
				if (query.IsCount && dataReader.NextResult())
				{
					dataReader.Read();
					dbQueryResult.TotalRecords = dataReader.GetInt32(0);
				}
			}
			return dbQueryResult;
		}

		public DbQueryResult GetBalanceDrawRequestsNoPage(BalanceDrawRequestQuery query, bool IsAdmin = true)
		{
			if (query == null)
			{
				return new DbQueryResult();
			}
			DbQueryResult dbQueryResult = new DbQueryResult();
			StringBuilder stringBuilder = new StringBuilder();
			string text = this.BuildBalanceDrawRequestQuery(query);
			stringBuilder.Append("SELECT *");
			stringBuilder.Append(" FROM Hishop_BalanceDrawRequest B WHERE 0=0 ");
			if (IsAdmin)
			{
				text += " AND IsPass IS NULL ";
			}
			switch (query.DrawRequestType)
			{
			case 1:
				text += " AND IsWeixin <> 1 AND IsAlipay <> 1";
				break;
			case 2:
				text += " AND IsWeixin = 1 ";
				break;
			case 3:
				text += " AND IsAlipay = 1";
				break;
			}
			stringBuilder.AppendFormat("{0} ORDER BY RequestTime DESC", text);
			if (query.IsCount)
			{
				stringBuilder.AppendFormat(";SELECT COUNT(*) AS TOTAL FROM Hishop_BalanceDrawRequest WHERE 0=0 {0}", text);
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				dbQueryResult.Data = DataHelper.ConverDataReaderToDataTable(dataReader);
				if (query.IsCount && dataReader.NextResult())
				{
					dataReader.Read();
					dbQueryResult.TotalRecords = dataReader.GetInt32(0);
				}
			}
			return dbQueryResult;
		}

		public bool UpdateBalanceDrawRequest(int id, string RequestState, string RequestError, string ManagerUserName)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE Hishop_BalanceDrawRequest SET RequestState = @RequestState, RequestError = @RequestError, AccountDate = @AccountDate, ManagerUserName = @ManagerUserName WHERE ID = @ID");
			base.database.AddInParameter(sqlStringCommand, "ID", DbType.Int32, id);
			base.database.AddInParameter(sqlStringCommand, "RequestState", DbType.String, RequestState);
			base.database.AddInParameter(sqlStringCommand, "RequestError", DbType.String, RequestError);
			base.database.AddInParameter(sqlStringCommand, "AccountDate", DbType.DateTime, DateTime.Now);
			base.database.AddInParameter(sqlStringCommand, "ManagerUserName", DbType.String, ManagerUserName);
			return base.database.ExecuteNonQuery(sqlStringCommand) >= 1;
		}

		public void OnLineBalanceDrawRequest_Alipay_AllError(string requestIds, string Error)
		{
			Database database = base.database;
			OnLinePayment onLinePayment = OnLinePayment.Paying;
			int hashCode = onLinePayment.GetHashCode();
			DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_BalanceDrawRequest SET RequestState = @RequestState, RequestError = @RequestError, AccountDate = @AccountDate WHERE RequestState = '" + hashCode.ToString() + "' AND IsAlipay = 1  AND Id IN(@RequestIds)");
			Database database2 = base.database;
			DbCommand command = sqlStringCommand;
			onLinePayment = OnLinePayment.PayFail;
			hashCode = onLinePayment.GetHashCode();
			database2.AddInParameter(command, "RequestState", DbType.String, hashCode.ToString());
			base.database.AddInParameter(sqlStringCommand, "RequestError", DbType.String, Error);
			base.database.AddInParameter(sqlStringCommand, "AccountDate", DbType.DateTime, DateTime.Now);
			base.database.AddInParameter(sqlStringCommand, "RequestIds", DbType.String, requestIds);
			base.database.ExecuteNonQuery(sqlStringCommand);
		}

		public void OnLineBalanceDrawRequest_Weixin_AllError(string Error)
		{
			Database database = base.database;
			OnLinePayment onLinePayment = OnLinePayment.Paying;
			int hashCode = onLinePayment.GetHashCode();
			DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_BalanceDrawRequest SET RequestState = @RequestState, RequestError = @RequestError, AccountDate = @AccountDate WHERE RequestState = '" + hashCode.ToString() + "' AND IsWeixin = 1 ");
			Database database2 = base.database;
			DbCommand command = sqlStringCommand;
			onLinePayment = OnLinePayment.PayFail;
			hashCode = onLinePayment.GetHashCode();
			database2.AddInParameter(command, "RequestState", DbType.String, hashCode.ToString());
			base.database.AddInParameter(sqlStringCommand, "RequestError", DbType.String, Error);
			base.database.AddInParameter(sqlStringCommand, "AccountDate", DbType.DateTime, DateTime.Now);
			base.database.ExecuteNonQuery(sqlStringCommand);
		}

		public bool DeleteBalanceDrawRequestById(int Id, bool IsPass, DbTransaction dbTran = null, string sReason = "")
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE [DBO].[HISHOP_BALANCEDRAWREQUEST] SET ISPASS = @ISPASS, AccountDate = @AccountDate, ManagerRemark = @ManagerRemark WHERE ID=@ID;UPDATE aspnet_Members SET RequestBalance = 0 WHERE UserId IN (SELECT USERID FROM [DBO].[HISHOP_BALANCEDRAWREQUEST] WHERE ID=@ID)");
			base.database.AddInParameter(sqlStringCommand, "ID", DbType.Int32, Id);
			base.database.AddInParameter(sqlStringCommand, "ISPASS", DbType.Boolean, IsPass);
			base.database.AddInParameter(sqlStringCommand, "AccountDate", DbType.DateTime, DateTime.Now);
			base.database.AddInParameter(sqlStringCommand, "ManagerRemark", DbType.String, sReason);
			if (dbTran != null)
			{
				return base.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0;
			}
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool DeleteBalanceDrawRequest(int userId, bool IsPass, DbTransaction dbTran = null, string sReason = "")
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE [DBO].[HISHOP_BALANCEDRAWREQUEST] SET ISPASS = @ISPASS, AccountDate = @AccountDate, ManagerRemark = @ManagerRemark WHERE UserId=@UserId AND IsPass IS NULL ;UPDATE aspnet_Members SET RequestBalance = 0 WHERE UserId = @UserId");
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			base.database.AddInParameter(sqlStringCommand, "ISPASS", DbType.Boolean, IsPass);
			base.database.AddInParameter(sqlStringCommand, "AccountDate", DbType.DateTime, DateTime.Now);
			base.database.AddInParameter(sqlStringCommand, "ManagerRemark", DbType.String, sReason);
			if (dbTran != null)
			{
				return base.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0;
			}
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		private string BuildBalanceDrawRequestQuery(BalanceDrawRequestQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (query.Id.HasValue)
			{
				stringBuilder.AppendFormat(" AND Id = {0}", query.Id.Value);
			}
			if (query.UserId.HasValue)
			{
				stringBuilder.AppendFormat(" AND UserId = {0}", query.UserId.Value);
			}
			if (!string.IsNullOrEmpty(query.UserName))
			{
				stringBuilder.AppendFormat(" AND UserName='{0}'", DataHelper.CleanSearchString(query.UserName));
			}
			if (query.FromDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND RequestTime >= '{0}'", DataHelper.GetSafeDateTimeFormat(query.FromDate.Value));
			}
			if (query.ToDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND RequestTime <= '{0}'", query.ToDate.Value.ToString("yyyy-MM-dd") + " 23:59:59");
			}
			return stringBuilder.ToString();
		}

		public DataTable GetWaittingPayingWeixin(string IdList)
		{
			DataTable result = new DataTable();
			string text = "SELECT BDR.USERID, BDR.AMOUNT, BDR.ID, M.REALNAME, M.CELLPHONE, MO.OPENID FROM  HISHOP_BALANCEDRAWREQUEST BDR LEFT JOIN ASPNET_MEMBERS M ON BDR.USERID = M.USERID LEFT JOIN ASPNET_MEMBEROPENIDS MO ON BDR.USERID = MO.USERID AND  LOWER(MO.OPENIDTYPE) = 'hishop.plugins.openid.weixin' WHERE BDR.RequestState =  " + 2.GetHashCode() + " AND BDR.ISWEIXIN = 1  AND BDR.IsPass IS NULL AND BDR.ID IN ('" + IdList.Replace(",", "','") + "')";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(text.ToString());
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
				dataReader.Close();
			}
			return result;
		}

		public DataTable GetWaittingPayingAlipay(string IdList)
		{
			DataTable result = new DataTable();
			string text = "SELECT ID, USERID, AMOUNT, AlipayRealName, AlipayCode FROM  HISHOP_BALANCEDRAWREQUEST  WHERE RequestState <> " + 1.GetHashCode() + " AND ISALIPAY = 1  AND IsPass IS NULL AND ID IN ('" + IdList.Replace(",", "','") + "')";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(text.ToString());
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
				dataReader.Close();
			}
			return result;
		}

		public int DeleteBalanceDetails(int userId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM [dbo].[Hishop_BalanceDetails] WHERE UserId=@UserId");
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			return base.database.ExecuteNonQuery(sqlStringCommand);
		}

		public BalanceDetailInfo GetBalanceDetailInfoOfInpurId(string inpourId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM [dbo].[Hishop_BalanceDetails] WHERE InpourId=@InpourId");
			base.database.AddInParameter(sqlStringCommand, "InpourId", DbType.String, inpourId);
			BalanceDetailInfo result = null;
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToModel<BalanceDetailInfo>(objReader);
			}
			return result;
		}

		private string BuildBalanceDetailsQuery(BalanceDetailQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (query.UserId.HasValue)
			{
				stringBuilder.AppendFormat(" AND UserId = {0}", query.UserId.Value);
			}
			if (!string.IsNullOrEmpty(query.UserName))
			{
				stringBuilder.AppendFormat(" AND UserName='{0}'", DataHelper.CleanSearchString(query.UserName));
			}
			if (query.FromDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND TradeDate >= '{0}'", DataHelper.GetSafeDateTimeFormat(query.FromDate.Value));
			}
			if (query.ToDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND TradeDate < '{0}'", DataHelper.GetSafeDateTimeFormat(query.ToDate.Value.AddDays(1.0)));
			}
			if (query.TradeType != 0)
			{
				stringBuilder.AppendFormat(" AND TradeType = {0}", (int)query.TradeType);
			}
			return stringBuilder.ToString();
		}

		public int DeleteBalanceDrawRequestsByMemberId(int memberId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM [dbo].[Hishop_BalanceDrawRequest] WHERE UserId=@UserId");
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, memberId);
			return base.database.ExecuteNonQuery(sqlStringCommand);
		}

		public bool GetBalanceIsWeixin(int id)
		{
			if (id == 0)
			{
				return false;
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT IsWeixin FROM Hishop_BalanceDrawRequest WHERE Id=@Id");
			base.database.AddInParameter(sqlStringCommand, "Id", DbType.Int32, id);
			return base.database.ExecuteScalar(sqlStringCommand).ToBool();
		}

		public bool GetIsRechargeGift()
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT count(*) FROM Hishop_BalanceDetails WHERE TradeType=9 ");
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0) > 0;
		}

		public BalanceDrawRequestInfo GetBalanceDrawRequestInfo(int requestId)
		{
			BalanceDrawRequestInfo result = null;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM Hishop_BalanceDrawRequest WHERE ID = @RequestId ");
			base.database.AddInParameter(sqlStringCommand, "RequestId", DbType.Int32, requestId);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToModel<BalanceDrawRequestInfo>(objReader);
			}
			return result;
		}
	}
}
