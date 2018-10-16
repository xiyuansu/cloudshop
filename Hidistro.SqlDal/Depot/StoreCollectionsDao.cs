using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Depot;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Depot
{
	public class StoreCollectionsDao : BaseDao
	{
		public bool DeleteStoreCollection(string SerialNumber)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("delete from Hishop_StoreCollections where SerialNumber = @SerialNumber");
			base.database.AddInParameter(sqlStringCommand, "SerialNumber", DbType.String, SerialNumber);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public StoreCollectionInfo GetStoreCollectionInfo(string serialNumber)
		{
			StoreCollectionInfo result = new StoreCollectionInfo();
			string query = "SELECT * FROM dbo.Hishop_StoreCollections WHERE SerialNumber= @SerialNumber";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "SerialNumber", DbType.String, serialNumber);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToModel<StoreCollectionInfo>(objReader);
			}
			return result;
		}

		public PageModel<StoreCollectionInfo> GetStoreCollectionInfos(StoreCollectionsQuery query, out decimal amountTotal, out decimal storeTotal, out decimal platTotal)
		{
			string enumDescription = EnumDescription.GetEnumDescription((Enum)(object)EnumPaymentType.CashPay, 1);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" Status <> " + 0);
			if (query.StoreId.HasValue && query.StoreId > 0)
			{
				stringBuilder.Append(" and StoreId = " + query.StoreId);
			}
			if (query.CollectionType.HasValue)
			{
				if (query.CollectionType.Value == 1)
				{
					stringBuilder.Append(" AND GateWay = '" + enumDescription + "'");
				}
				else
				{
					stringBuilder.Append(" AND GateWay != '" + enumDescription + "'");
				}
			}
			if (query.PaymentTypeId.HasValue)
			{
				stringBuilder.Append(" and GateWay = '" + EnumDescription.GetEnumDescription((Enum)(object)(EnumPaymentType)query.PaymentTypeId.Value, 1) + "'");
			}
			if (query.StartPayTime.HasValue)
			{
				stringBuilder.Append(" and PayTime >= '" + query.StartPayTime.Value + "'");
			}
			if (query.EndPayTime.HasValue)
			{
				stringBuilder.Append(" and PayTime <= '" + query.EndPayTime.Value + "'");
			}
			if (query.OrderType.HasValue)
			{
				stringBuilder.Append((" and OrderType = " + query.OrderType.Value) ?? "");
			}
			if (query.OrderIds != null && query.OrderIds.Count > 0)
			{
				string text = string.Empty;
				foreach (string orderId in query.OrderIds)
				{
					text = text + "'" + orderId + "',";
				}
				text = text.Substring(0, text.Length - 1);
				stringBuilder.Append(" and (OrderId is null or OrderId = '' or OrderId in(" + text + "))");
			}
			int num = 0;
			storeTotal = default(decimal);
			amountTotal = default(decimal);
			platTotal = default(decimal);
			try
			{
				this.GetCashCollectionTotal(stringBuilder.ToString(), enumDescription, out amountTotal, out storeTotal, out platTotal);
				PageModel<StoreCollectionInfo> pageModel = new PageModel<StoreCollectionInfo>();
				return DataHelper.PagingByRownumber<StoreCollectionInfo>(query.PageIndex, query.PageSize, "PayTime", SortAction.Desc, true, "Hishop_StoreCollections", "SerialNumber", stringBuilder.ToString(), "*");
			}
			catch (Exception ex)
			{
				IDictionary<string, string> iParam = new Dictionary<string, string>();
				Globals.WriteExceptionLog(ex, iParam, "StoreCollection");
				return null;
			}
		}

		public StoreCollectionInfo GetStoreCollectionInfoByOrderId(string OrderId)
		{
			StoreCollectionInfo result = new StoreCollectionInfo();
			string query = "SELECT top 1 * FROM dbo.Hishop_StoreCollections where OrderId = @OrderId";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, OrderId);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToModel<StoreCollectionInfo>(objReader);
			}
			return result;
		}

		public void GetCashCollectionTotal(string strWhere, string gateway, out decimal amountTotal, out decimal storeTotal, out decimal platTotal)
		{
			string query = "SELECT  SUM(ISNULL(PayAmount,0)) - SUM(ISNULL(RefundAmount,0)) FROM dbo.Hishop_StoreCollections where " + strWhere;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			amountTotal = base.database.ExecuteScalar(sqlStringCommand).ToDecimal(0);
			storeTotal = this.GetNotCashCollectionTotal(strWhere, gateway);
			platTotal = amountTotal - storeTotal;
		}

		public decimal GetNotCashCollectionTotal(string strWhere, string gateway)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(strWhere.ToString());
			stringBuilder.Append(" and GateWay = '" + gateway + "'");
			StoreCollectionInfo storeCollectionInfo = new StoreCollectionInfo();
			string query = "SELECT  SUM(ISNULL(PayAmount,0)) - SUM(ISNULL(RefundAmount,0)) FROM dbo.Hishop_StoreCollections where " + stringBuilder.ToString();
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			return base.database.ExecuteScalar(sqlStringCommand).ToDecimal(0);
		}

		public IList<StoreCollectionInfo> GetStoreCollectionList(int StoreId, DateTime startTime, DateTime endTime)
		{
			string query = "SELECT * FROM dbo.Hishop_StoreCollections where StoreId = @StoreId and PayTime >= @StartTime and PayTime<= @EndTime and Status <> @Status";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.String, StoreId);
			base.database.AddInParameter(sqlStringCommand, "StartTime", DbType.DateTime, startTime);
			base.database.AddInParameter(sqlStringCommand, "EndTime", DbType.DateTime, endTime);
			base.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, 0);
			IList<StoreCollectionInfo> result = new List<StoreCollectionInfo>();
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<StoreCollectionInfo>(objReader);
			}
			return result;
		}
	}
}
