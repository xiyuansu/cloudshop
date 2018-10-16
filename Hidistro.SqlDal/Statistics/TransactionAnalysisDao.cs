using Hidistro.Core;
using Hidistro.Entities.Statistics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Statistics
{
	public class TransactionAnalysisDao : BaseDao
	{
		public OrderDailyStatisticsInfo GetOrderDailyStatisticsInfoByDay(DateTime dtNow)
		{
			string text = "select top 1 * from Hishop_OrderDailyStatistics where StatisticalDate = @StatisticalDate";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(text.ToString());
			base.database.AddInParameter(sqlStringCommand, "StatisticalDate", DbType.DateTime, dtNow.Date);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				return DataHelper.ReaderToModel<OrderDailyStatisticsInfo>(objReader);
			}
		}

		public int ExistOrderInDateByUserId(DateTime dtNow, int UserId)
		{
			string text = "select Count(*) from Hishop_Orders where UserId = @UserId and OrderDate >=@StartDate and OrderDate< @EndDate and (ParentOrderId = '0' or ParentOrderId = '-1')";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(text.ToString());
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, UserId);
			base.database.AddInParameter(sqlStringCommand, "StartDate", DbType.DateTime, dtNow.Date);
			base.database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, dtNow.Date.AddDays(1.0));
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0);
		}

		public int ExistPayOrderInDateByUserId(DateTime dtNow, int UserId)
		{
			dtNow = dtNow.Date;
			DateTime dateTime = dtNow.Date.AddDays(1.0);
			string text = " ((paydate >='" + dtNow + "' and paydate <'" + dateTime + "' and Gateway != 'hishop.plugins.payment.podrequest')";
			text = text + " or (Gateway = 'hishop.plugins.payment.podrequest' and ShippingDate>='" + dtNow + "' and ShippingDate <'" + dateTime + "'))";
			text += " and (ParentOrderId = '0' or ParentOrderId = '-1')";
			string text2 = $"select Count(*) from Hishop_Orders where UserId = {UserId} and {text}";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(text2.ToString());
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0);
		}

		public IList<OrderStatisticModel> GetOrderDailyStatisticsList(EnumConsumeTime TimeType, DateTime StartDate, DateTime EndDate)
		{
			string format = "SELECT a.StatisticalDate, IsNull(b.PV,0) AS PV,IsNull(b.UV,0) AS UV,[OrderUserNum] ,[OrderNum],[OrderProductQuantity],[OrderAmount],[PaymentUserNum] ,[PaymentOrderNum]\r\n                      ,[PaymentProductNum],[PaymentAmount],[RefundAmount] FROM Hishop_OrderDailyStatistics AS a\r\n                LEFT JOIN (SELECT SUM(PV) AS PV ,SUM(UV) AS UV,StatisticalDate,PageType  FROM dbo.Hishop_DailyAccessStatistics WHERE StoreId = 0\r\n                GROUP BY StatisticalDate,PageType HAVING PageType = {0}) AS b\r\n                ON b.StatisticalDate = a.StatisticalDate";
			format = string.Format(format, 4);
			string text = " where ";
			int num;
			switch (TimeType)
			{
			case EnumConsumeTime.custom:
				text = text + "a.StatisticalDate >= '" + StartDate + "' and a.StatisticalDate <= '" + EndDate + "'";
				break;
			default:
				num = ((TimeType == EnumConsumeTime.inOneWeek) ? 1 : 0);
				goto IL_0070;
			case EnumConsumeTime.inOneMonth:
				{
					num = 1;
					goto IL_0070;
				}
				IL_0070:
				text = ((num == 0) ? ((TimeType != EnumConsumeTime.yesterday) ? (text + "a.StatisticalDate = '" + DateTime.Now.Date + "'") : (text + "a.StatisticalDate = '" + StartDate.Date + "'")) : (text + "a.StatisticalDate >= '" + StartDate + "' and a.StatisticalDate < '" + EndDate + "'"));
				break;
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(format + text);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				return DataHelper.ReaderToList<OrderStatisticModel>(objReader);
			}
		}

		public int GetAccessStatisticsModelList(EnumConsumeTime TimeType, DateTime StartDate, DateTime EndDate)
		{
			string format = "SELECT IsNull(SUM(UV),null) AS UV FROM dbo.Hishop_DailyAccessStatistics";
			format = string.Format(format, 4);
			string text = $" where StoreId = 0 AND  PageType = {4} and ";
			int num;
			switch (TimeType)
			{
			case EnumConsumeTime.custom:
				text = text + "StatisticalDate >= '" + StartDate + "' and StatisticalDate <= '" + EndDate + "'";
				break;
			default:
				num = ((TimeType == EnumConsumeTime.inOneWeek) ? 1 : 0);
				goto IL_0079;
			case EnumConsumeTime.inOneMonth:
				{
					num = 1;
					goto IL_0079;
				}
				IL_0079:
				text = ((num == 0) ? ((TimeType != EnumConsumeTime.yesterday) ? (text + "StatisticalDate = '" + DateTime.Now.Date + "'") : (text + "StatisticalDate = '" + StartDate.Date + "'")) : (text + "StatisticalDate >= '" + StartDate + "' and StatisticalDate < '" + EndDate + "'"));
				break;
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(format + text);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0);
		}

		public CustomerTradingModel GetCustomerTrading(DateTime StartDate, DateTime EndDate)
		{
			CustomerTradingModel customerTradingModel = new CustomerTradingModel();
			string text = " ((paydate >='" + StartDate + "' and paydate <'" + EndDate + "' and Gateway != 'hishop.plugins.payment.podrequest')";
			text = text + " or (Gateway = 'hishop.plugins.payment.podrequest' and ShippingDate>='" + StartDate + "' and ShippingDate <'" + EndDate + "'))";
			text += " and (ParentOrderId = '0' or ParentOrderId = '-1')";
			string text2 = " ((paydate <'" + EndDate + "' and Gateway != 'hishop.plugins.payment.podrequest')";
			text2 = text2 + " or (Gateway = 'hishop.plugins.payment.podrequest' and  ShippingDate <'" + EndDate + "'))";
			text2 += " and (ParentOrderId = '0' or ParentOrderId = '-1')";
			string text3 = " ((paydate <'" + StartDate + "' and Gateway != 'hishop.plugins.payment.podrequest')";
			text3 = text3 + " or (Gateway = 'hishop.plugins.payment.podrequest' and  ShippingDate <'" + StartDate + "'))";
			text3 += " and (ParentOrderId = '0' or ParentOrderId = '-1')";
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT COUNT(DISTINCT userId) as  AllCustomerNum FROM aspnet_Members;");
			stringBuilder.AppendFormat(" SELECT SUM((CASE PreSaleId WHEN 0 THEN  IsNull(OrderTotal,0) else  IsNull(Deposit,0) +IsNull(FinalPayment,0) END)) as AllCustomerAmount  FROM dbo.Hishop_Orders where {0} ;", text2);
			stringBuilder.AppendFormat(" SELECT COUNT(DISTINCT userId) as OldCustomerNum FROM dbo.Hishop_Orders where {0};", text3);
			stringBuilder.AppendFormat(" SELECT COUNT(DISTINCT userId) as OldCustomerPayNum FROM dbo.Hishop_Orders where UserId IN(SELECT DISTINCT UserId FROM Hishop_Orders WHERE {0}) AND {1};", text3, text);
			stringBuilder.AppendFormat(" SELECT SUM((CASE PreSaleId WHEN 0 THEN  IsNull(OrderTotal,0) else  IsNull(Deposit,0) +IsNull(FinalPayment,0) END)) as OldCustomerAmount  FROM dbo.Hishop_Orders where {0} and UserId in (select DISTINCT userId from dbo.Hishop_Orders where {1});", text, text3);
			stringBuilder.AppendFormat("SELECT COUNT(DISTINCT userId) as  NewCustomerPayNum FROM dbo.Hishop_Orders where {0} and userId not in (select DISTINCT userId from dbo.Hishop_Orders where {1});", text, text3);
			Globals.AppendLog(stringBuilder.ToString(), "", "", "GetCustomerTrading");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					customerTradingModel.AllCustomerNum = ((IDataRecord)dataReader)["AllCustomerNum"].ToInt(0);
				}
				if (dataReader.NextResult() && dataReader.Read() && ((IDataRecord)dataReader)["AllCustomerAmount"] != DBNull.Value)
				{
					customerTradingModel.AllCustomerAmount = ((IDataRecord)dataReader)["AllCustomerAmount"].ToDecimal(0);
				}
				if (dataReader.NextResult() && dataReader.Read())
				{
					customerTradingModel.OldCustomerNum = ((IDataRecord)dataReader)["OldCustomerNum"].ToInt(0);
				}
				if (dataReader.NextResult() && dataReader.Read())
				{
					customerTradingModel.OldCustomerPayNum = ((IDataRecord)dataReader)["OldCustomerPayNum"].ToInt(0);
				}
				if (dataReader.NextResult() && dataReader.Read())
				{
					if (((IDataRecord)dataReader)["OldCustomerAmount"] != DBNull.Value)
					{
						customerTradingModel.OldCustomerAmount = ((IDataRecord)dataReader)["OldCustomerAmount"].ToDecimal(0);
					}
					customerTradingModel.NewCustomerAmount = customerTradingModel.AllCustomerAmount - customerTradingModel.OldCustomerAmount;
				}
				customerTradingModel.NewCustomerNum = customerTradingModel.AllCustomerNum - customerTradingModel.OldCustomerNum;
				if (dataReader.NextResult() && dataReader.Read() && ((IDataRecord)dataReader)["NewCustomerPayNum"] != DBNull.Value)
				{
					customerTradingModel.NewCustomerPayNum = ((IDataRecord)dataReader)["NewCustomerPayNum"].ToInt(0);
				}
			}
			return customerTradingModel;
		}

		public OrderAmountDistributionModel GetOrderAmountDistribution(DateTime StartDate, DateTime EndDate)
		{
			OrderAmountDistributionModel result = new OrderAmountDistributionModel();
			string text = " ((paydate >='" + StartDate + "' and paydate < '" + EndDate + "' and Gateway != 'hishop.plugins.payment.podrequest')";
			text = text + " or (Gateway = 'hishop.plugins.payment.podrequest' and ShippingDate>='" + StartDate + "' and ShippingDate < '" + EndDate + "'))";
			text += " and (ParentOrderId = '0' or ParentOrderId = '-1')";
			StringBuilder stringBuilder = new StringBuilder();
			string arg = "(CASE  PreSaleId WHEN 0 THEN  OrderTotal else Deposit +FinalPayment END)";
			stringBuilder.AppendFormat("SELECT SUM(CASE WHEN {0}<= {1} THEN 1 ELSE 0 END) AS OneCount", arg, "50");
			stringBuilder.AppendFormat(", SUM(CASE WHEN {0} BETWEEN {1} AND {2} THEN 1 ELSE 0 END) AS TwoCount", arg, "50.01", "100.00");
			stringBuilder.AppendFormat(", SUM(CASE WHEN {0} BETWEEN {1} AND {2} THEN 1 ELSE 0 END) AS ThreeCount", arg, "100.01", "200.00");
			stringBuilder.AppendFormat(", SUM(CASE WHEN {0} BETWEEN {1} AND {2} THEN 1 ELSE 0 END) AS FourCount", arg, "200.01", "500.00");
			stringBuilder.AppendFormat(", SUM(CASE WHEN {0} BETWEEN {1} AND {2} THEN 1 ELSE 0 END) AS FiveCount", arg, "500.01", "1000.00");
			stringBuilder.AppendFormat(", SUM(CASE WHEN {0} BETWEEN {1} AND {2} THEN 1 ELSE 0 END) AS SixCount", arg, "1000.01", "5000.00");
			stringBuilder.AppendFormat(", SUM(CASE WHEN {0} BETWEEN {1} AND {2} THEN 1 ELSE 0 END) AS SevenCount", arg, "5000.01", "10000.00");
			stringBuilder.AppendFormat(", SUM(CASE WHEN {0} > {1} THEN 1 ELSE 0 END) AS EightCount", arg, "10000");
			stringBuilder.AppendFormat(" FROM dbo.Hishop_Orders WHERE {0}", text);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToModel<OrderAmountDistributionModel>(objReader);
			}
			return result;
		}

		public OrderSourceDistributionModel GetOrderSourceDistribution(DateTime StartDate, DateTime EndDate)
		{
			OrderSourceDistributionModel orderSourceDistributionModel = new OrderSourceDistributionModel();
			string text = " ((paydate >='" + StartDate + "' and paydate < '" + EndDate + "' and Gateway != 'hishop.plugins.payment.podrequest')";
			text = text + " or (Gateway = 'hishop.plugins.payment.podrequest' and ShippingDate>='" + StartDate + "' and ShippingDate < '" + EndDate + "'))";
			text += " and (ParentOrderId = '0' or ParentOrderId = '-1')";
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT SourceOrder, SUM(CASE  PreSaleId WHEN 0 THEN  OrderTotal else Deposit +FinalPayment END) as Amount,COUNT(orderId) AS OrderCount ");
			stringBuilder.AppendFormat(" From (Select PreSaleId,OrderId, OrderTotal,Deposit,FinalPayment,SourceOrder From dbo.Hishop_Orders Where {0}) as a", text);
			stringBuilder.Append(" GROUP BY SourceOrder");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					switch (((IDataRecord)dataReader)["SourceOrder"].ToInt(0))
					{
					case 1:
						orderSourceDistributionModel.PCAmount = ((IDataRecord)dataReader)["Amount"].ToDecimal(0);
						orderSourceDistributionModel.PCCount = ((IDataRecord)dataReader)["OrderCount"].ToInt(0);
						break;
					case 3:
						orderSourceDistributionModel.WeiXinAmount = ((IDataRecord)dataReader)["Amount"].ToDecimal(0);
						orderSourceDistributionModel.WeiXinCount = ((IDataRecord)dataReader)["OrderCount"].ToInt(0);
						break;
					case 6:
						orderSourceDistributionModel.AppAmount = ((IDataRecord)dataReader)["Amount"].ToDecimal(0);
						orderSourceDistributionModel.AppCount = ((IDataRecord)dataReader)["OrderCount"].ToInt(0);
						break;
					case 8:
						orderSourceDistributionModel.AppletAmount = ((IDataRecord)dataReader)["Amount"].ToDecimal(0);
						orderSourceDistributionModel.AppletCount = ((IDataRecord)dataReader)["OrderCount"].ToInt(0);
						break;
					default:
					{
						OrderSourceDistributionModel orderSourceDistributionModel2 = orderSourceDistributionModel;
						orderSourceDistributionModel2.OtherAmount += ((IDataRecord)dataReader)["Amount"].ToDecimal(0);
						orderSourceDistributionModel.OtherCount += ((IDataRecord)dataReader)["OrderCount"].ToInt(0);
						break;
					}
					}
				}
			}
			return orderSourceDistributionModel;
		}
	}
}
