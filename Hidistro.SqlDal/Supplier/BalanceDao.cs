using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Supplier;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Hidistro.SqlDal.Supplier
{
	public class BalanceDao : BaseDao
	{
		public BalanceInfo StatisticsBalance(int supplierId)
		{
			StringBuilder stringBuilder = new StringBuilder("select Balance");
			stringBuilder.Append(",(select isnull(SUM(Amount), 0) from dbo.Hishop_SupplierBalanceDrawRequest r where r.SupplierId = s.SupplierId and IsPass is null) as BalanceForzen");
			stringBuilder.Append(",(select isnull(SUM(Amount), 0) from dbo.Hishop_SupplierBalanceDrawRequest r where r.SupplierId = s.SupplierId and IsPass = 1) as BalanceOut");
			stringBuilder.AppendFormat(" from dbo.Hishop_Supplier s where SupplierId = {0}", supplierId);
			BalanceInfo balanceInfo = new BalanceInfo();
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			try
			{
				using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
				{
					if (dataReader.Read())
					{
						balanceInfo.Balance = (decimal)((IDataRecord)dataReader)["Balance"];
						balanceInfo.BalanceForzen = (decimal)((IDataRecord)dataReader)["BalanceForzen"];
						balanceInfo.BalanceOut = (decimal)((IDataRecord)dataReader)["BalanceOut"];
					}
				}
			}
			catch (Exception)
			{
			}
			return balanceInfo;
		}

		public DataTable GetWaittingPayingAlipay(string IdList)
		{
			DataTable result = new DataTable();
			string text = "SELECT ID, SupplierId as USERID, AMOUNT, AlipayRealName, AlipayCode FROM  Hishop_SupplierBalanceDrawRequest  WHERE RequestState <> " + 1.GetHashCode() + " AND ISALIPAY = 1  AND IsPass IS NULL AND ID IN ('" + IdList.Replace(",", "','") + "')";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(text.ToString());
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
				dataReader.Close();
			}
			return result;
		}

		public DbQueryResult GetBalanceDetails(BalanceDetailSupplierQuery query)
		{
			try
			{
				if (query == null)
				{
					return new DbQueryResult();
				}
				DbQueryResult dbQueryResult = new DbQueryResult();
				StringBuilder stringBuilder = new StringBuilder();
				string text = this.BuildBalanceDetailsQuery(query);
				stringBuilder.AppendFormat("SELECT TOP {0} *, isnull (Income, 0) + isnull (Expenses, 0) Amount ,CASE WHEN TradeType = 1 THEN '提现'  ELSE '商品交易' END TradeTypeText ", query.PageSize);
				stringBuilder.Append(" FROM Hishop_SupplierBalanceDetails B where 0=0 ");
				if (query.PageIndex == 1)
				{
					stringBuilder.AppendFormat("{0} ORDER BY JournalNumber DESC", text);
				}
				else
				{
					stringBuilder.AppendFormat(" and JournalNumber < (select min(JournalNumber) from (select top {0} JournalNumber from Hishop_SupplierBalanceDetails where 0=0 {1} ORDER BY JournalNumber DESC ) as tbltemp) {1} ORDER BY JournalNumber DESC", (query.PageIndex - 1) * query.PageSize, text);
				}
				if (query.IsCount)
				{
					stringBuilder.AppendFormat(";select count(JournalNumber) as Total from Hishop_SupplierBalanceDetails where 0=0 {0}", text);
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
			catch (Exception)
			{
				return new DbQueryResult();
			}
		}

		public DbQueryResult GetBalanceDetails4Report(BalanceDetailSupplierQuery query)
		{
			try
			{
				if (query == null)
				{
					return new DbQueryResult();
				}
				DbQueryResult dbQueryResult = new DbQueryResult();
				StringBuilder stringBuilder = new StringBuilder();
				string arg = this.BuildBalanceDetailsQuery(query);
				stringBuilder.Append("SELECT  *, isnull (Income, 0) + isnull (Expenses, 0) Amount ,CASE WHEN TradeType = 1 THEN '提现'  ELSE '商品交易' END TradeTypeText ");
				stringBuilder.Append(" FROM Hishop_SupplierBalanceDetails B where 0=0 ");
				stringBuilder.AppendFormat("{0} ORDER BY JournalNumber DESC", arg);
				DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
				using (IDataReader reader = base.database.ExecuteReader(sqlStringCommand))
				{
					dbQueryResult.Data = DataHelper.ConverDataReaderToDataTable(reader);
				}
				return dbQueryResult;
			}
			catch (Exception)
			{
				return new DbQueryResult();
			}
		}

		private string BuildBalanceDetailsQuery(BalanceDetailSupplierQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (query.SupplierId.HasValue && query.SupplierId.Value > 0)
			{
				stringBuilder.AppendFormat(" AND SupplierId = {0}", query.SupplierId.Value);
			}
			if (!string.IsNullOrEmpty(query.Key))
			{
				stringBuilder.AppendFormat(" AND UserName like '%{0}%'", DataHelper.CleanSearchString(query.Key));
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
				stringBuilder.AppendFormat(" AND TradeType = {0}", query.TradeType);
			}
			return stringBuilder.ToString();
		}

		public bool UpdateBalanceDrawRequest(int id, string RequestState, string RequestError, string ManagerUserName)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE Hishop_SupplierBalanceDrawRequest SET RequestState = @RequestState, RequestError = @RequestError, AccountDate = @AccountDate, ManagerUserName = @ManagerUserName WHERE ID = @ID");
			base.database.AddInParameter(sqlStringCommand, "ID", DbType.Int32, id);
			base.database.AddInParameter(sqlStringCommand, "RequestState", DbType.String, RequestState);
			base.database.AddInParameter(sqlStringCommand, "RequestError", DbType.String, RequestError);
			base.database.AddInParameter(sqlStringCommand, "AccountDate", DbType.DateTime, DateTime.Now);
			base.database.AddInParameter(sqlStringCommand, "ManagerUserName", DbType.String, ManagerUserName);
			return base.database.ExecuteNonQuery(sqlStringCommand) >= 1;
		}

		public bool DeleteBalanceDrawRequestById(bool IsPass, decimal balance, SupplierBalanceDrawRequestInfo balanceDrawRequest, string manageName, DbTransaction dbTran, string sReason = "")
		{
			StringBuilder stringBuilder = new StringBuilder("UPDATE Hishop_SupplierBalanceDrawRequest SET ISPASS = @ISPASS, AccountDate = @AccountDate, ManagerRemark = @ManagerRemark, ManagerUserName = @ManagerUserName WHERE ID=@ID;");
			if (IsPass)
			{
				SupplierBalanceDetailInfo supplierBalanceDetailInfo = new SupplierBalanceDetailInfo();
				stringBuilder.Append("UPDATE Hishop_Supplier SET Balance = Balance - @Balance WHERE SupplierId = @SupplierId");
				supplierBalanceDetailInfo.SupplierId = balanceDrawRequest.SupplierId;
				supplierBalanceDetailInfo.UserName = balanceDrawRequest.UserName;
				supplierBalanceDetailInfo.TradeDate = DateTime.Now;
				supplierBalanceDetailInfo.TradeType = SupplierTradeTypes.DrawRequest;
				supplierBalanceDetailInfo.Expenses = balanceDrawRequest.Amount;
				supplierBalanceDetailInfo.Balance = balance - balanceDrawRequest.Amount;
				supplierBalanceDetailInfo.Remark = balanceDrawRequest.Remark;
				supplierBalanceDetailInfo.ManagerUserName = manageName;
				supplierBalanceDetailInfo.OrderId = balanceDrawRequest.ID.ToString();
				if (this.Add(supplierBalanceDetailInfo, dbTran) <= 0)
				{
					return false;
				}
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "ID", DbType.Int32, balanceDrawRequest.ID);
			base.database.AddInParameter(sqlStringCommand, "SupplierId", DbType.Int32, balanceDrawRequest.SupplierId);
			base.database.AddInParameter(sqlStringCommand, "ISPASS", DbType.Boolean, IsPass);
			base.database.AddInParameter(sqlStringCommand, "AccountDate", DbType.DateTime, DateTime.Now);
			base.database.AddInParameter(sqlStringCommand, "ManagerRemark", DbType.String, sReason);
			base.database.AddInParameter(sqlStringCommand, "Balance", DbType.Decimal, balanceDrawRequest.Amount);
			base.database.AddInParameter(sqlStringCommand, "ManagerUserName", DbType.String, manageName);
			return base.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0;
		}

		public SupplierBalanceDrawRequestInfo GetLastDrawRequest(int supplierId)
		{
			SupplierBalanceDrawRequestInfo supplierBalanceDrawRequestInfo = new SupplierBalanceDrawRequestInfo();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT TOP 1 [AccountName],[BankName],[MerchantCode] FROM[dbo].[Hishop_SupplierBalanceDrawRequest] where SupplierId = {0} and IsAlipay = 0  and IsWeixin = 0 order by ID desc;", supplierId);
			stringBuilder.AppendFormat("SELECT TOP 1 [AlipayRealName] ,[AlipayCode] FROM  [dbo].[Hishop_SupplierBalanceDrawRequest] where SupplierId={0} and IsAlipay=1 order by ID desc", supplierId);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			try
			{
				using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
				{
					if (dataReader.Read())
					{
						supplierBalanceDrawRequestInfo.AccountName = ((IDataRecord)dataReader)["AccountName"].ToString();
						supplierBalanceDrawRequestInfo.BankName = ((IDataRecord)dataReader)["BankName"].ToString();
						supplierBalanceDrawRequestInfo.MerchantCode = ((IDataRecord)dataReader)["MerchantCode"].ToString();
					}
					if (dataReader.NextResult() && dataReader.Read())
					{
						supplierBalanceDrawRequestInfo.AlipayRealName = ((IDataRecord)dataReader)["AlipayRealName"].ToString();
						supplierBalanceDrawRequestInfo.AlipayCode = ((IDataRecord)dataReader)["AlipayCode"].ToString();
					}
				}
			}
			catch (Exception)
			{
				supplierBalanceDrawRequestInfo = null;
			}
			return supplierBalanceDrawRequestInfo;
		}

		public void OnLineBalanceDrawRequest_Alipay_AllError(string requestIds, string errorMessage)
		{
			Database database = base.database;
			OnLinePayment onLinePayment = OnLinePayment.Paying;
			int hashCode = onLinePayment.GetHashCode();
			DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_SupplierBalanceDrawRequest SET RequestState = @RequestState, RequestError = @RequestError, AccountDate = @AccountDate WHERE RequestState = '" + hashCode.ToString() + "' AND IsAlipay = 1 AND Id IN(@RequestIds)");
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

		public bool AddRequest(SupplierBalanceDrawRequestInfo balanceDrawRequest)
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

		public decimal GetBalanceLeft(int supplierId)
		{
			decimal result = default(decimal);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("select Balance-(select isnull(SUM(Amount), 0) from dbo.Hishop_SupplierBalanceDrawRequest r where r.SupplierId = s.SupplierId and IsPass is null) from dbo.Hishop_Supplier s where SupplierId =" + supplierId);
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

		private decimal GetBalanceForzen(int supplierId)
		{
			decimal result = default(decimal);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("select isnull(SUM(Amount), 0) from dbo.Hishop_SupplierBalanceDrawRequest r where  IsPass is null and SupplierId =" + supplierId);
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

		public PageModel<SupplierBalanceDrawRequestInfo> GetBalanceDrawRequests(BalanceDrawRequestSupplierQuery query, bool IsAdmin = true)
		{
			PageModel<SupplierBalanceDrawRequestInfo> pageModel = new PageModel<SupplierBalanceDrawRequestInfo>();
			StringBuilder stringBuilder = new StringBuilder();
			string text = this.BuildBalanceDrawRequestQuery(query);
			stringBuilder.AppendFormat("select top {0} B.*,S.SupplierName", query.PageSize);
			stringBuilder.Append(" from Hishop_SupplierBalanceDrawRequest B join Hishop_Supplier S on b.SupplierId=S.SupplierId where 0=0 ");
			if (IsAdmin)
			{
				text += " AND IsPass IS NULL ";
			}
			switch (query.DrawRequestType)
			{
			case 3:
				text += " AND IsAlipay = 1";
				break;
			case 1:
				text += " AND IsWeixin <> 1 AND IsAlipay <> 1";
				break;
			}
			if (query.PageIndex == 1)
			{
				stringBuilder.AppendFormat("{0} ORDER BY RequestTime DESC", text);
			}
			else
			{
				stringBuilder.AppendFormat(" and RequestTime < (select min(RequestTime) from (select top {0} RequestTime from Hishop_SupplierBalanceDrawRequest where 0=0 {1} ORDER BY RequestTime DESC ) as tbltemp) {1} ORDER BY RequestTime DESC", (query.PageIndex - 1) * query.PageSize, text);
			}
			if (query.IsCount)
			{
				stringBuilder.AppendFormat(";select count(1) as Total from Hishop_SupplierBalanceDrawRequest B where 0=0 {0}", text);
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				pageModel.Models = DataHelper.ReaderToList<SupplierBalanceDrawRequestInfo>(objReader).ToList();
				pageModel.Total = pageModel.Models.Count();
			}
			return pageModel;
		}

		public DbQueryResult GetBalanceDrawRequest4Report(BalanceDrawRequestSupplierQuery query, bool isAdmin)
		{
			DbQueryResult dbQueryResult = new DbQueryResult();
			StringBuilder stringBuilder = new StringBuilder();
			string text = this.BuildBalanceDrawRequestQuery(query);
			stringBuilder.AppendFormat("select B.*,S.SupplierName");
			stringBuilder.Append(" from Hishop_SupplierBalanceDrawRequest B join Hishop_Supplier S on b.SupplierId=S.SupplierId where 0=0 ");
			if (isAdmin)
			{
				text += " AND IsPass IS NULL ";
			}
			switch (query.DrawRequestType)
			{
			case 3:
				text += " AND IsAlipay = 1";
				break;
			case 1:
				text += " AND IsWeixin <> 1 AND IsAlipay <> 1";
				break;
			}
			stringBuilder.AppendFormat("{0} ORDER BY RequestTime DESC", text);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			using (IDataReader reader = base.database.ExecuteReader(sqlStringCommand))
			{
				dbQueryResult.Data = DataHelper.ConverDataReaderToDataTable(reader);
			}
			return dbQueryResult;
		}

		private string BuildBalanceDrawRequestQuery(BalanceDrawRequestSupplierQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (query.Id.HasValue && query.Id.Value > 0)
			{
				stringBuilder.AppendFormat(" AND Id={0}", query.Id.Value);
			}
			if (query.UserId.HasValue && query.UserId.Value > 0)
			{
				stringBuilder.AppendFormat(" AND B.SupplierId = {0}", query.UserId.Value);
			}
			if (!string.IsNullOrEmpty(query.UserName))
			{
				stringBuilder.AppendFormat(" AND UserName like '%{0}%'", DataHelper.CleanSearchString(query.UserName));
			}
			if (query.FromDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND RequestTime >= '{0}'", DataHelper.GetSafeDateTimeFormat(query.FromDate.Value));
			}
			if (query.ToDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND RequestTime <= '{0}'", query.ToDate.Value.ToString("yyyy-MM-dd") + " 23:59:59");
			}
			switch (query.AuditState)
			{
			case 1:
				stringBuilder.Append(" and  IsPass is null ");
				break;
			case 2:
				stringBuilder.Append(" AND AccountDate is not null and  IsPass=1 ");
				break;
			case 3:
				stringBuilder.Append(" AND AccountDate is not null and  IsPass=0 ");
				break;
			}
			return stringBuilder.ToString();
		}

		public IList<SupplierSettlementModel> GetBalanceStatisticsExportData(BalanceStatisticsQuery query)
		{
			IList<SupplierSettlementModel> result = new List<SupplierSettlementModel>();
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			stringBuilder.Append("SELECT SupplierId,SupplierName,Tel,Balance,");
			stringBuilder.Append("(SELECT ISNULL(SUM(amount),0) FROM Hishop_SupplierBalanceDrawRequest r WHERE r.SupplierId=s.SupplierId AND IsPass IS null) AS BalanceFozen,");
			stringBuilder.Append("(SELECT ISNULL(SUM(amount),0) FROM Hishop_SupplierBalanceDrawRequest r WHERE r.SupplierId=s.SupplierId AND IsPass=1) AS BalanceOut");
			stringBuilder.Append(" FROM  dbo.Hishop_Supplier s WHERE 0=0");
			if (!string.IsNullOrEmpty(query.Key))
			{
				stringBuilder2.AppendFormat(" AND SupplierName like '%{0}%'", query.Key);
			}
			stringBuilder.AppendFormat("{0} ORDER BY SupplierId DESC", stringBuilder2);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<SupplierSettlementModel>(objReader);
			}
			return result;
		}

		public DbQueryResult GetBalanceStatisticsList(BalanceStatisticsQuery query)
		{
			DbQueryResult dbQueryResult = new DbQueryResult();
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			stringBuilder.AppendFormat("select top {0} SupplierId,SupplierName,Tel,Balance,", query.PageSize);
			stringBuilder.Append("(select isNull(SUM(amount),0) from Hishop_SupplierBalanceDrawRequest r where r.SupplierId=s.SupplierId and IsPass is null) as BalanceFozen,");
			stringBuilder.Append("(select isnull(SUM(amount),0) from Hishop_SupplierBalanceDrawRequest r where r.SupplierId=s.SupplierId and IsPass=1) as BalanceOut");
			stringBuilder.Append(" from  dbo.Hishop_Supplier s where 0=0");
			if (!string.IsNullOrEmpty(query.Key))
			{
				stringBuilder2.AppendFormat(" and SupplierName like '%{0}%'", query.Key);
			}
			if (query.PageIndex == 1)
			{
				stringBuilder.AppendFormat("{0} ORDER BY SupplierId DESC", stringBuilder2);
			}
			else
			{
				stringBuilder.AppendFormat(" and SupplierId < (select min(SupplierId) from (select top {0} SupplierId from Hishop_Supplier where 0=0 {1} ORDER BY SupplierId DESC ) as tbltemp) {1} ORDER BY SupplierId DESC", (query.PageIndex - 1) * query.PageSize, stringBuilder2);
			}
			if (query.IsCount)
			{
				stringBuilder.AppendFormat(";select count(1) as Total from Hishop_Supplier where 0=0 {0}", stringBuilder2);
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

		public SupplierBalanceDrawRequestInfo GetBalanceDrawRequestInfo(int requestId)
		{
			SupplierBalanceDrawRequestInfo result = null;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM Hishop_SupplierBalanceDrawRequest WHERE ID = @RequestId ");
			base.database.AddInParameter(sqlStringCommand, "RequestId", DbType.Int32, requestId);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToModel<SupplierBalanceDrawRequestInfo>(objReader);
			}
			return result;
		}
	}
}
