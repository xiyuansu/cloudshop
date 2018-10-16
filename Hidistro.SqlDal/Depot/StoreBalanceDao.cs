using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Depot;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Hidistro.SqlDal.Depot
{
	public class StoreBalanceDao : BaseDao
	{
		public StoreBalanceInfo StatisticsBalance(int storeId, decimal commsionRate)
		{
			StringBuilder stringBuilder = new StringBuilder("SELECT Balance");
			stringBuilder.Append(",(SELECT ISNULL(SUM(Amount), 0) FROM dbo.Hishop_StoreBalanceDrawRequest r WHERE r.StoreId = s.StoreId AND IsPass IS NULL) AS BalanceForzen");
			stringBuilder.Append(",(SELECT ISNULL(SUM(Amount), 0) FROM dbo.Hishop_StoreBalanceDrawRequest r WHERE r.StoreId = s.StoreId AND IsPass = 1) AS BalanceOut");
			stringBuilder.AppendFormat(",(SELECT SUM(Income-Expenses) FROM Hishop_StoreBalanceDetails WHERE TradeType = 2 AND StoreId = {0}) AS FinishOrderBalance", storeId);
			stringBuilder.AppendFormat(",(SELECT SUM(OrderTotal  + ISNULL(DeductionMoney,0) + ISNULL(CouponValue,0) - Tax - ISNULL(RefundAmount,0) - ROUND((OrderTotal + ISNULL(DeductionMoney,0) + ISNULL(CouponValue,0) - Tax - ISNULL(RefundAmount,0) - AdjustedFreight) * {0},2)) FROM Hishop_Orders o WHERE IsStoreCollect = 0 AND (OrderStatus = {2} OR (OrderStatus = {1} AND ShippingDate IS NOT NULL)) AND IsBalanceOver = 0 AND o.StoreId = s.StoreId) AS PlatCollectBalance", commsionRate / 100m, 4, 5);
			stringBuilder.AppendFormat(",(SELECT SUM(ISNULL(DeductionMoney,0) + ISNULL(CouponValue,0) - Tax -  ROUND((OrderTotal + ISNULL(DeductionMoney,0) + ISNULL(CouponValue,0) - Tax - ISNULL(RefundAmount,0) - AdjustedFreight) * {0},2)) FROM Hishop_Orders o WHERE IsStoreCollect = 1 AND (OrderStatus = {2} OR (OrderStatus = {1} AND ShippingDate IS NOT NULL)) AND IsBalanceOver = 0 AND o.StoreId = s.StoreId) AS StoreCollectBalance", commsionRate / 100m, 4, 5);
			stringBuilder.AppendFormat(" FROM Hishop_Stores s WHERE StoreId = {0}", storeId);
			StoreBalanceInfo storeBalanceInfo = new StoreBalanceInfo();
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			try
			{
				using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
				{
					if (dataReader.Read())
					{
						storeBalanceInfo.Balance = ((IDataRecord)dataReader)["Balance"].ToDecimal(0);
						storeBalanceInfo.BalanceForzen = ((IDataRecord)dataReader)["BalanceForzen"].ToDecimal(0);
						storeBalanceInfo.BalanceOut = ((IDataRecord)dataReader)["BalanceOut"].ToDecimal(0);
						storeBalanceInfo.FinishOrderBalance = ((IDataRecord)dataReader)["FinishOrderBalance"].ToDecimal(0);
						storeBalanceInfo.NoFinishOrderBalance = ((IDataRecord)dataReader)["PlatCollectBalance"].ToDecimal(0) + ((IDataRecord)dataReader)["StoreCollectBalance"].ToDecimal(0);
					}
				}
			}
			catch (Exception ex)
			{
				IDictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("sql", stringBuilder.ToString());
				Globals.WriteExceptionLog(ex, dictionary, "StatisticsBalance");
			}
			return storeBalanceInfo;
		}

		public StoreBalanceOffLineOrderInfo GetBalanceOffOrderDetails(int balanceId)
		{
			StringBuilder stringBuilder = new StringBuilder("SELECT  od.PayTime,sd.CreateTime as OverBalanceDate,od.PaymentTypeName,od.PayAmount as OrderTotal ");
			stringBuilder.Append(" from dbo.Hishop_StoreBalanceDetails sd");
			stringBuilder.Append(" inner join  Hishop_StoreCollections od on sd.TradeNo=od.SerialNumber");
			stringBuilder.AppendFormat("  where sd.JournalNumber={0}", balanceId);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				return DataHelper.ReaderToModel<StoreBalanceOffLineOrderInfo>(objReader);
			}
		}

		public StoreBalanceOrderInfo GetBalanceDetails(int balanceId)
		{
			StringBuilder stringBuilder = new StringBuilder("SELECT OrderId,OrderDate,od.AdjustedFreight as Freight,sd.CreateTime as OverBalanceDate,OrderTotal,ISNULL(DeductionMoney,0) as DeductionMoney,ISNULL(CouponValue,0) CouponValue,ISNULL(RefundAmount,0) RefundAmount,IsStoreCollect, PlatCommission ,Income as  OverBalance,Tax ");
			stringBuilder.Append(" from dbo.Hishop_StoreBalanceDetails sd");
			stringBuilder.Append(" inner join  Hishop_Orders od on sd.TradeNo=od.OrderId");
			stringBuilder.AppendFormat("  where sd.JournalNumber={0}", balanceId);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				return DataHelper.ReaderToModel<StoreBalanceOrderInfo>(objReader);
			}
		}

		public DataTable GetWaittingPayingAlipay(string IdList)
		{
			DataTable result = new DataTable();
			string text = "SELECT Id, StoreId as USERID, AMOUNT, AlipayRealName, AlipayCode FROM  Hishop_StoreBalanceDrawRequest  WHERE RequestState <> " + 1.GetHashCode() + " AND IsAlipay = 1  AND IsPass IS NULL AND Id IN ('" + IdList.Replace(",", "','") + "')";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(text.ToString());
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
				dataReader.Close();
			}
			return result;
		}

		public PageModel<StoreBalanceDetailInfo> GetBalanceDetails(StoreBalanceDetailQuery query)
		{
			PageModel<StoreBalanceDetailInfo> pageModel = new PageModel<StoreBalanceDetailInfo>();
			pageModel.Total = 0;
			pageModel.Models = new List<StoreBalanceDetailInfo>();
			try
			{
				if (query == null)
				{
					return pageModel;
				}
				DbQueryResult dbQueryResult = new DbQueryResult();
				StringBuilder stringBuilder = new StringBuilder();
				string text = this.BuildBalanceDetailsQuery(query);
				stringBuilder.AppendFormat("SELECT TOP {0} * ", query.PageSize);
				stringBuilder.Append(" FROM Hishop_StoreBalanceDetails B WHERE 1 = 1");
				if (query.PageIndex == 1)
				{
					stringBuilder.AppendFormat("{0} ORDER BY TradeDate DESC", text);
				}
				else
				{
					stringBuilder.AppendFormat(" AND TradeDate < (SELECT MIN(TradeDate) FROM (SELECT TOP {0} JournalNumber FROM Hishop_StoreBalanceDetails WHERE 1 = 1 {1} ORDER BY TradeDate DESC ) as tbltemp) {1} ORDER BY JournalNumber DESC", (query.PageIndex - 1) * query.PageSize, text);
				}
				if (query.IsCount)
				{
					stringBuilder.AppendFormat(";SELECT COUNT(TradeDate) AS Total FROM Hishop_StoreBalanceDetails WHERE 1 = 1 {0}", text);
				}
				DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
				using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
				{
					pageModel.Models = DataHelper.ReaderToList<StoreBalanceDetailInfo>(dataReader);
					if (query.IsCount && dataReader.NextResult())
					{
						dataReader.Read();
						pageModel.Total = dataReader.GetInt32(0);
					}
				}
				return pageModel;
			}
			catch (Exception)
			{
				return pageModel;
			}
		}

		public IList<StoreBalanceDetailInfo> GetBalanceDetails4Report(StoreBalanceDetailQuery query)
		{
			IList<StoreBalanceDetailInfo> result = new List<StoreBalanceDetailInfo>();
			try
			{
				if (query == null)
				{
					return result;
				}
				DbQueryResult dbQueryResult = new DbQueryResult();
				StringBuilder stringBuilder = new StringBuilder();
				string arg = this.BuildBalanceDetailsQuery(query);
				stringBuilder.Append("SELECT  * ");
				stringBuilder.Append(" FROM Hishop_StoreBalanceDetails B WHERE 0 = 0 ");
				stringBuilder.AppendFormat("{0} ORDER BY JournalNumber DESC", arg);
				DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
				using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
				{
					result = DataHelper.ReaderToList<StoreBalanceDetailInfo>(objReader);
				}
				return result;
			}
			catch (Exception)
			{
				return result;
			}
		}

		private string BuildBalanceDetailsQuery(StoreBalanceDetailQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (query.StoreId.HasValue && query.StoreId.Value > 0)
			{
				stringBuilder.AppendFormat(" AND StoreId = {0}", query.StoreId.Value);
			}
			if (!string.IsNullOrEmpty(query.StoreName))
			{
				stringBuilder.AppendFormat(" AND StoreId IN(SELECT StoreId FROM Hishop_Stores WHERE StoreName LIKE '%{0}%')", DataHelper.CleanSearchString(query.StoreName));
			}
			if (query.FromDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND TradeDate >= '{0}'", DataHelper.GetSafeDateTimeFormat(query.FromDate.Value));
			}
			if (query.ToDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND TradeDate <= '{0}'", DataHelper.GetSafeDateTimeFormat(query.ToDate.Value.AddDays(1.0)));
			}
			if (query.TradeType != 0)
			{
				stringBuilder.AppendFormat(" AND TradeType = {0}", (int)query.TradeType);
			}
			return stringBuilder.ToString();
		}

		public bool UpdateBalanceDrawRequest(int id, string RequestState, string RequestError, string ManagerUserName)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE Hishop_StoreBalanceDrawRequest SET RequestState = @RequestState, RequestError = @RequestError, AccountDate = @AccountDate, ManagerUserName = @ManagerUserName WHERE Id = @Id");
			base.database.AddInParameter(sqlStringCommand, "Id", DbType.Int32, id);
			base.database.AddInParameter(sqlStringCommand, "RequestState", DbType.String, RequestState);
			base.database.AddInParameter(sqlStringCommand, "RequestError", DbType.String, RequestError);
			base.database.AddInParameter(sqlStringCommand, "AccountDate", DbType.DateTime, DateTime.Now);
			base.database.AddInParameter(sqlStringCommand, "ManagerUserName", DbType.String, ManagerUserName);
			return base.database.ExecuteNonQuery(sqlStringCommand) >= 1;
		}

		public bool DeleteBalanceDrawRequestById(bool IsPass, decimal balance, StoreBalanceDrawRequestInfo balanceDrawRequest, string manageName, DbTransaction dbTran, string sReason = "")
		{
			StringBuilder stringBuilder = new StringBuilder("UPDATE [Hishop_StoreBalanceDrawRequest] SET IsPass = @IsPass, AccountDate = @AccountDate, ManagerRemark = @ManagerRemark, ManagerUserName = @ManagerUserName WHERE Id  = @Id;");
			if (IsPass)
			{
				StoreBalanceDetailInfo storeBalanceDetailInfo = new StoreBalanceDetailInfo();
				stringBuilder.Append("UPDATE Hishop_Stores SET Balance = Balance - @Balance WHERE StoreId = @StoreId");
				storeBalanceDetailInfo.StoreId = balanceDrawRequest.StoreId;
				storeBalanceDetailInfo.TradeDate = DateTime.Now;
				storeBalanceDetailInfo.TradeType = StoreTradeTypes.DrawRequest;
				storeBalanceDetailInfo.Expenses = balanceDrawRequest.Amount;
				storeBalanceDetailInfo.Balance = balance - balanceDrawRequest.Amount;
				storeBalanceDetailInfo.Remark = balanceDrawRequest.Remark;
				storeBalanceDetailInfo.ManagerUserName = manageName;
				storeBalanceDetailInfo.TradeNo = balanceDrawRequest.Id.ToString();
				storeBalanceDetailInfo.CreateTime = DateTime.Now;
				storeBalanceDetailInfo.Income = default(decimal);
				storeBalanceDetailInfo.PlatCommission = decimal.Zero;
				if (this.Add(storeBalanceDetailInfo, dbTran) <= 0)
				{
					return false;
				}
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "Id", DbType.Int32, balanceDrawRequest.Id);
			base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, balanceDrawRequest.StoreId);
			base.database.AddInParameter(sqlStringCommand, "IsPass", DbType.Boolean, IsPass);
			base.database.AddInParameter(sqlStringCommand, "AccountDate", DbType.DateTime, DateTime.Now);
			base.database.AddInParameter(sqlStringCommand, "ManagerRemark", DbType.String, sReason);
			base.database.AddInParameter(sqlStringCommand, "Balance", DbType.Decimal, balanceDrawRequest.Amount);
			base.database.AddInParameter(sqlStringCommand, "ManagerUserName", DbType.String, manageName);
			return base.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0;
		}

		public StoreBalanceDrawRequestInfo GetLastDrawRequest(int storeId)
		{
			StoreBalanceDrawRequestInfo storeBalanceDrawRequestInfo = new StoreBalanceDrawRequestInfo();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT TOP 1 [AccountName],[BankName],[MerchantCode] FROM [dbo].[Hishop_StoreBalanceDrawRequest] WHERE StoreId = {0} AND IsAlipay = 0 ORDER BY Id desc;", storeId);
			stringBuilder.AppendFormat("SELECT TOP 1 [AlipayRealName] ,[AlipayCode] FROM  [dbo].[Hishop_StoreBalanceDrawRequest] WHERE StoreId = {0} AND IsAlipay = 1  ORDER BY Id desc", storeId);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			try
			{
				using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
				{
					if (dataReader.Read())
					{
						storeBalanceDrawRequestInfo.AccountName = ((IDataRecord)dataReader)["AccountName"].ToString();
						storeBalanceDrawRequestInfo.BankName = ((IDataRecord)dataReader)["BankName"].ToString();
						storeBalanceDrawRequestInfo.MerchantCode = ((IDataRecord)dataReader)["MerchantCode"].ToString();
					}
					if (dataReader.NextResult() && dataReader.Read())
					{
						storeBalanceDrawRequestInfo.AlipayRealName = ((IDataRecord)dataReader)["AlipayRealName"].ToString();
						storeBalanceDrawRequestInfo.AlipayCode = ((IDataRecord)dataReader)["AlipayCode"].ToString();
					}
				}
			}
			catch (Exception)
			{
				storeBalanceDrawRequestInfo = null;
			}
			return storeBalanceDrawRequestInfo;
		}

		public void OnLineBalanceDrawRequest_Alipay_AllError(string requestIds, string errorMessage)
		{
			Database database = base.database;
			OnLinePayment onLinePayment = OnLinePayment.Paying;
			int hashCode = onLinePayment.GetHashCode();
			DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_StoreBalanceDrawRequest SET RequestState = @RequestState, RequestError = @RequestError, AccountDate = @AccountDate WHERE RequestState = '" + hashCode.ToString() + "' AND IsAlipay = 1 AND Id IN(@RequestIds)");
			Database database2 = base.database;
			DbCommand command = sqlStringCommand;
			onLinePayment = OnLinePayment.PayFail;
			hashCode = onLinePayment.GetHashCode();
			database2.AddInParameter(command, "RequestState", DbType.String, hashCode.ToString());
			base.database.AddInParameter(sqlStringCommand, "RequestError", DbType.String, errorMessage);
			base.database.AddInParameter(sqlStringCommand, "AccountDate", DbType.DateTime, DateTime.Now);
			base.database.AddInParameter(sqlStringCommand, "RequestIds", DbType.String, requestIds);
			base.database.ExecuteNonQuery(sqlStringCommand);
		}

		public bool AddRequest(StoreBalanceDrawRequestInfo balanceDrawRequest)
		{
			bool result = false;
			using (DbConnection dbConnection = base.database.CreateConnection())
			{
				try
				{
					dbConnection.Open();
					using (DbTransaction dbTransaction = dbConnection.BeginTransaction())
					{
						try
						{
							if (this.Add(balanceDrawRequest, dbTransaction) > 0)
							{
								dbTransaction.Commit();
								result = true;
							}
						}
						catch (Exception)
						{
							dbTransaction.Rollback();
						}
					}
				}
				catch (Exception)
				{
				}
				finally
				{
					dbConnection.Close();
				}
			}
			return result;
		}

		public decimal GetBalanceLeft(int storeId)
		{
			decimal result = default(decimal);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT Balance -(SELECT ISNULL(SUM(Amount), 0) FROM Hishop_StoreBalanceDrawRequest r WHERE r.storeId = s.storeId AND IsPass IS NULL) FROM dbo.Hishop_Stores s WHERE StoreId =" + storeId);
			try
			{
				result = (decimal)base.database.ExecuteScalar(sqlStringCommand);
				return result;
			}
			catch (Exception)
			{
			}
			return result;
		}

		private decimal GetBalanceForzen(int storeId)
		{
			decimal result = default(decimal);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT ISNULL(SUM(Amount), 0) FROM dbo.Hishop_StoreBalanceDrawRequest r WHERE  IsPass IS NULL AND StoreId =" + storeId);
			try
			{
				result = (decimal)base.database.ExecuteScalar(sqlStringCommand);
				return result;
			}
			catch (Exception)
			{
			}
			return result;
		}

		public PageModel<StoreBalanceDrawRequestInfo> GetBalanceDrawRequests(StoreBalanceDrawRequestQuery query, bool IsAdmin = true)
		{
			PageModel<StoreBalanceDrawRequestInfo> pageModel = new PageModel<StoreBalanceDrawRequestInfo>();
			StringBuilder stringBuilder = new StringBuilder();
			int num = (query.PageIndex - 1) * query.PageSize + 1;
			int num2 = query.PageIndex * query.PageSize;
			string text = this.BuildBalanceDrawRequestQuery(query);
			stringBuilder.AppendFormat("SELECT * FROM(SELECT ROW_NUMBER() OVER(ORDER  BY B.id) RowId,B.*,(SELECT Top 1 StoreName FROM Hishop_Stores WHERE StoreId = b.StoreId) AS StoreName,m.UserName");
			stringBuilder.Append(" FROM Hishop_StoreBalanceDrawRequest b JOIN aspnet_Managers as m on m.StoreId = b.StoreId WHERE 1 = 1 and m.RoleId = -1");
			stringBuilder.Append(text);
			if (IsAdmin)
			{
				text += " AND IsPass IS NULL ";
			}
			switch (query.DrawRequestType)
			{
			case 3:
				text += " AND IsAlipay = 1";
				stringBuilder.Append(" AND IsAlipay = 1");
				break;
			case 1:
				text += " AND (IsAlipay <> 1 OR IsAlipay IS NULL)";
				stringBuilder.Append(" AND (IsAlipay <> 1 OR IsAlipay IS NULL)");
				break;
			}
			stringBuilder.AppendFormat(") AS nTable WHERE RowId BETWEEN {0} AND {1}", num, num2);
			if (query.IsCount)
			{
				stringBuilder.AppendFormat(";SELECT COUNT(1) AS Total  FROM Hishop_StoreBalanceDrawRequest b join aspnet_Managers as m on m.StoreId = b.StoreId WHERE 1 = 1 and m.RoleId = -1 {0}", text);
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				pageModel.Models = DataHelper.ReaderToList<StoreBalanceDrawRequestInfo>(dataReader).ToList();
				if (query.IsCount && dataReader.NextResult())
				{
					dataReader.Read();
					pageModel.Total = dataReader.GetInt32(0);
				}
			}
			return pageModel;
		}

		public DbQueryResult GetBalanceDrawRequest4Report(StoreBalanceDrawRequestQuery query, bool isAdmin)
		{
			DbQueryResult dbQueryResult = new DbQueryResult();
			StringBuilder stringBuilder = new StringBuilder();
			string str = this.BuildBalanceDrawRequestQuery(query);
			stringBuilder.AppendFormat("SELECT B.*,(SELECT Top 1 StoreName FROM Hishop_Stores WHERE StoreId = b.StoreId) AS StoreName,m.UserName");
			stringBuilder.Append(" FROM Hishop_StoreBalanceDrawRequest b join aspnet_Managers as m on m.StoreId = b.StoreId WHERE 1 = 1 and m.RoleId = -1");
			if (isAdmin)
			{
				str += " AND IsPass IS NULL ";
			}
			switch (query.DrawRequestType)
			{
			case 3:
				str += " AND IsAlipay = 1";
				stringBuilder.Append(" AND IsAlipay = 1");
				break;
			case 1:
				str += " AND (IsAlipay <> 1 OR IsAlipay IS NULL)";
				stringBuilder.Append(" AND (IsAlipay <> 1 OR IsAlipay IS NULL)");
				break;
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			using (IDataReader reader = base.database.ExecuteReader(sqlStringCommand))
			{
				dbQueryResult.Data = DataHelper.ConverDataReaderToDataTable(reader);
			}
			return dbQueryResult;
		}

		private string BuildBalanceDrawRequestQuery(StoreBalanceDrawRequestQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (query.Id.HasValue && query.Id.Value > 0)
			{
				stringBuilder.AppendFormat(" AND Id = {0} ", query.Id.Value);
			}
			if (query.StoreId.HasValue && query.StoreId.Value > 0)
			{
				stringBuilder.AppendFormat(" AND B.StoreId = {0}", query.StoreId.Value);
			}
			if (!string.IsNullOrEmpty(query.StoreName))
			{
				stringBuilder.AppendFormat(" AND B.StoreId IN(SELECT StoreId FROM Hishop_Stores WHERE StoreName LIKE '%{0}%')", DataHelper.CleanSearchString(query.StoreName));
			}
			if (!string.IsNullOrEmpty(query.UserName))
			{
				stringBuilder.AppendFormat(" AND m.UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.UserName));
			}
			if (query.FromDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND RequestTime >= '{0}'", DataHelper.GetSafeDateTimeFormat(query.FromDate.Value));
			}
			if (query.ToDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND RequestTime <= '{0}'", query.ToDate.Value.ToString("yyyy-MM-dd") + " 23:59:59");
			}
			switch (query.AuditStatus)
			{
			case 1:
				stringBuilder.Append(" AND IsPass IS NULL ");
				break;
			case 2:
				stringBuilder.Append(" AND AccountDate IS NOT NULL AND IsPass = 1 ");
				break;
			case 3:
				stringBuilder.Append(" AND AccountDate IS NOT NULL AND IsPass = 0 ");
				break;
			}
			return stringBuilder.ToString();
		}

		public DbQueryResult GetBalanceStatisticsList(StoreBalanceDetailQuery query)
		{
			DbQueryResult dbQueryResult = new DbQueryResult();
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			stringBuilder.AppendFormat("SELECT TOP {0} StoreId,StoreName,Tel,Balance,", query.PageSize);
			stringBuilder.Append("(SELECT ISNULL(SUM(amount),0) FROM Hishop_StoreBalanceDrawRequest r WHERE r.StoreId = s.StoreId AND IsPass IS NULL) as BalanceFozen,");
			stringBuilder.Append("(SELECT ISNULL(SUM(amount),0) FROM Hishop_StoreBalanceDrawRequest r WHERE r.StoreId = s.StoreId AND IsPass=  1) as BalanceOut");
			stringBuilder.Append(" FROM  dbo.Hishop_Stores s WHERE 1 = 1");
			if (!string.IsNullOrEmpty(query.StoreName))
			{
				stringBuilder2.AppendFormat(" AND StoreName LIKE '%{0}%'", query.StoreName);
			}
			if (query.PageIndex == 1)
			{
				stringBuilder.AppendFormat("{0} ORDER BY StoreId DESC", stringBuilder2);
			}
			else
			{
				stringBuilder.AppendFormat(" AND StoreId < (SELECT MIN(StoreId) FROM (SELECT TOP {0} StoreId FROM Hishop_Stores WHERE 1 = 1 {1} ORDER BY StoreId DESC ) AS tbltemp) {1} ORDER BY StoreId DESC", (query.PageIndex - 1) * query.PageSize, stringBuilder2);
			}
			if (query.IsCount)
			{
				stringBuilder.AppendFormat(";SELECT COUNT(1) AS Total FROM Hishop_Stores WHERE 1 = 1 {0}", stringBuilder2);
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				DataTable dataTable2 = dbQueryResult.Data = DataHelper.ConverDataReaderToDataTable(dataReader);
				if (query.IsCount && dataReader.NextResult())
				{
					dataReader.Read();
					dbQueryResult.TotalRecords = dataReader.GetInt32(0);
				}
			}
			return dbQueryResult;
		}

		public decimal GetOverBalanceOrdersTotal(StoreBalanceOrderQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT ISNULL(SUM(ISNULL(InCome,0)),0) - ISNULL(SUM(ISNULL(Expenses,0)),0) AS BalanceTotal FROM Hishop_StoreBalanceDetails WHERE TradeType = " + 2 + " AND StoreId = " + query.StoreId);
			if (query.StartDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND TradeNo IN(SELECT OrderId FROM Hishop_Orders WHERE DATEDIFF(dd,'{0}',OrderDate) >= 0)", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
			}
			if (query.EndDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND TradeNo IN(SELECT OrderId FROM Hishop_Orders WHERE DATEDIFF(dd,'{0}',OrderDate) <= 0)", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
			}
			if (query.OverStartDate.HasValue && query.OverEndDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND (CreateTime BETWEEN '{0}' AND '{1}')", DataHelper.GetSafeDateTimeFormat(query.OverStartDate.Value), DataHelper.GetSafeDateTimeFormat(query.OverEndDate.Value));
			}
			else if (query.OverStartDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND DATEDIFF(dd,'{0}',CreateTime) >= 0", DataHelper.GetSafeDateTimeFormat(query.OverStartDate.Value));
			}
			else if (query.OverEndDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND DATEDIFF(dd,'{0}',CreateTime) <= 0", DataHelper.GetSafeDateTimeFormat(query.OverEndDate.Value));
			}
			if (!string.IsNullOrEmpty(query.OrderId))
			{
				stringBuilder.AppendFormat(" AND TradeNo = '{0}'", DataHelper.CleanSearchString(query.OrderId));
			}
			return base.database.ExecuteScalar(CommandType.Text, stringBuilder.ToString()).ToDecimal(0);
		}

		public decimal GetNotOverBalanceOrdersTotal(StoreBalanceOrderQuery query, decimal commsionRate)
		{
			decimal num = default(decimal);
			string text = $"SELECT SUM(OrderTotal  + ISNULL(DeductionMoney,0) + ISNULL(CouponValue,0) - ROUND(Tax,2) - ISNULL(RefundAmount,0) - ROUND((OrderTotal + ISNULL(DeductionMoney,0) + ISNULL(CouponValue,0) - Tax - ISNULL(RefundAmount,0) - AdjustedFreight) * {commsionRate / 100m},2)) AS PlatTotal FROM Hishop_Orders WHERE IsStoreCollect = 0 ";
			string text2 = $"SELECT SUM(ISNULL(DeductionMoney,0) + ISNULL(CouponValue,0) - ROUND(Tax,2) -  ROUND((OrderTotal + ISNULL(DeductionMoney,0) + ISNULL(CouponValue,0) - Tax - ISNULL(RefundAmount,0) - AdjustedFreight) * {commsionRate / 100m},2)) AS StoreTotal FROM Hishop_Orders WHERE  IsStoreCollect = 1 ";
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("AND StoreId = {0} and IsBalanceOver = 0 ", query.StoreId);
			stringBuilder.AppendFormat(" AND (OrderStatus = {0} OR (OrderStatus={1} AND ShippingDate IS NOT NULL))", 5, 4);
			if (query.StartDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND DATEDIFF(dd,'{0}',OrderDate) >= 0", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
			}
			if (query.EndDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND DATEDIFF(dd,'{0}',OrderDate) <= 0", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
			}
			if (!string.IsNullOrEmpty(query.OrderId))
			{
				stringBuilder.AppendFormat(" AND OrderId = '{0}'", DataHelper.CleanSearchString(query.OrderId));
			}
			string commandText = text + stringBuilder.ToString() + ";" + text2 + stringBuilder.ToString() + ";";
			using (IDataReader dataReader = base.database.ExecuteReader(CommandType.Text, commandText))
			{
				if (dataReader.Read())
				{
					num = ((IDataRecord)dataReader)["PlatTotal"].ToDecimal(0);
				}
				if (dataReader.NextResult() && dataReader.Read())
				{
					num += ((IDataRecord)dataReader)["StoreTotal"].ToDecimal(0);
				}
			}
			return num;
		}

		public PageModel<StoreBalanceOrderInfo> GetBalanceOrders(StoreBalanceOrderQuery query)
		{
			if (query.IsBalanceOver)
			{
				return new StoreBalanceDao().GetOverBalanceOrders(query);
			}
			StringBuilder stringBuilder = new StringBuilder($"StoreId = {query.StoreId} and IsBalanceOver = {0} ");
			stringBuilder.AppendFormat(" AND (OrderStatus={0} OR (OrderStatus={1} AND ShippingDate IS NOT NULL))", 5, 4);
			if (query.StartDate.HasValue && query.EndDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND OrderDate BETWEEN '{0}' AND '{1}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value), DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
			}
			else if (query.StartDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND DATEDIFF(dd,'{0}',OrderDate) >= 0", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
			}
			else if (query.EndDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND DATEDIFF(dd,'{0}',OrderDate) <= 0", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
			}
			if (!string.IsNullOrEmpty(query.OrderId))
			{
				stringBuilder.AppendFormat(" AND OrderId = '{0}'", DataHelper.CleanSearchString(query.OrderId));
			}
			return DataHelper.PagingByRownumber<StoreBalanceOrderInfo>(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_Orders", "OrderId", stringBuilder.ToString(), "OrderId,OrderDate,OrderTotal,AdjustedFreight AS Freight,DeductionMoney,CouponValue,RefundAmount,IsStoreCollect,Tax,OrderType");
		}

		public PageModel<StoreBalanceOrderInfo> GetOverBalanceOrders(StoreBalanceOrderQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder($"StoreId = {query.StoreId} AND  TradeType = {2} ");
			if (query.StartDate.HasValue && query.EndDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND TradeNo IN(SELECT OrderId FROM Hishop_Orders WHERE OrderDate BETWEEN '{0}' AND '{1}')", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value), DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
			}
			else if (query.StartDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND TradeNo IN(SELECT OrderId FROM Hishop_Orders WHERE DATEDIFF(dd,'{0}',OrderDate) >= 0)", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
			}
			else if (query.EndDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND TradeNo IN(SELECT OrderId FROM Hishop_Orders WHERE DATEDIFF(dd,'{0}',OrderDate) <= 0)", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
			}
			if (query.EndDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND TradeNo IN(SELECT OrderId FROM Hishop_Orders WHERE DATEDIFF(dd,'{0}',OrderDate) <= 0)", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
			}
			if (query.OverStartDate.HasValue && query.OverEndDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND CreateTime BETWEEN '{0}' AND '{1}'", DataHelper.GetSafeDateTimeFormat(query.OverStartDate.Value), DataHelper.GetSafeDateTimeFormat(query.OverEndDate.Value));
			}
			else if (query.OverStartDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND DATEDIFF(dd,'{0}',CreateTime) >= 0)", DataHelper.GetSafeDateTimeFormat(query.OverStartDate.Value));
			}
			else if (query.OverEndDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND DATEDIFF(dd,'{0}',CreateTime) <= 0)", DataHelper.GetSafeDateTimeFormat(query.OverEndDate.Value));
			}
			if (!string.IsNullOrEmpty(query.OrderId))
			{
				stringBuilder.AppendFormat(" AND TradeNo = '{0}'", DataHelper.CleanSearchString(query.OrderId));
			}
			query.SortBy = "TradeDate";
			return DataHelper.PagingByRownumber<StoreBalanceOrderInfo>(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_StoreBalanceDetails", "JournalNumber", stringBuilder.ToString(), "TradeNo AS OrderId,TradeDate AS OrderDate,PlatCommission,Income AS OverBalance");
		}

		public StoreBalanceOrderInfo GetBalanceOrderDetail(string orderId, bool isOverBalance = true)
		{
			StoreBalanceOrderInfo result = new StoreBalanceOrderInfo();
			string text = "";
			text = ((!isOverBalance) ? "SELECT OrderId,OrderDate,OrderTotal,AdjustedFreight AS Freight,ISNULL(DeductionMoney,0) AS DeductionMoney,ISNULL(CouponValue,0) AS CouponValue,ISNULL(RefundAmount,0) AS RefundAmount,IsStoreCollect, OrderDate AS OverBalanceDate,0 AS OverBalance,Tax FROM Hishop_Orders WHERE OrderId = @OrderId" : "SELECT OrderId,OrderDate,OrderTotal,AdjustedFreight AS Freight,ISNULL(DeductionMoney,0) AS DeductionMoney,ISNULL(CouponValue,0) AS CouponValue,ISNULL(RefundAmount,0) AS RefundAmount,IsStoreCollect,d.CreateTime AS OverBalanceDate,ISNULL(InCome,0) AS OverBalance,0 AS Tax,d.PlatCommission FROM Hishop_Orders o,Hishop_StoreBalanceDetails d WHERE d.TradeNO = o.OrderId AND o.OrderId = @OrderId");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(text);
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToModel<StoreBalanceOrderInfo>(objReader);
			}
			return result;
		}
	}
}
