using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Statistics;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Hidistro.SqlDal.Statistics
{
	public class ProductStatisticsDao : BaseDao
	{
		public bool ProudctTodayHasPayment(int userId, int productId, string orderId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT COUNT(DISTINCT(OrderId)) FROM Hishop_OrderItems WHERE ProductId = @ProductId AND OrderId IN(SELECT OrderId FROM Hishop_Orders WHERE UserId = @UserId AND PayDate IS NOT NULL AND DATEDIFF(dd,GETDATE(),PayDate) = 0 AND OrderId <> @OrderId)");
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			base.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 2);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0) > 0;
		}

		public bool UpdateOrderSaleStatistics(OrderInfo order, EnumTrafficActivityType activityType)
		{
			bool flag = false;
			StringBuilder stringBuilder = new StringBuilder();
			decimal amount = order.GetAmount(false);
			decimal d = amount - (order.GetTotal(false) - order.AdjustedFreight);
			decimal num = d / amount;
			if (num < decimal.Zero)
			{
				num = default(decimal);
			}
			foreach (LineItemInfo item in from it in order.LineItems.Values
			group it by it.ProductId into g
			select g.First())
			{
				IList<LineItemInfo> source = order.LineItems.Values.Where((LineItemInfo li) => li.ProductId == item.ProductId).ToList();
				bool flag2 = this.ProudctTodayHasPayment(order.UserId, item.ProductId, order.OrderId);
				int productStatisticId = this.GetProductStatisticId(item.ProductId, activityType);
				int num2 = source.Sum((LineItemInfo li) => li.Quantity);
				decimal d2 = source.Sum((LineItemInfo li) => li.ItemAdjustedPrice * (decimal)li.Quantity);
				decimal num3 = d2 - d2 * num;
				if (productStatisticId > 0)
				{
					stringBuilder.AppendFormat("UPDATE Hishop_ProductDailyAccessStatistics SET PaymentNum = PaymentNum + " + ((!flag2) ? 1 : 0) + ",SaleQuantity = SaleQuantity + {0},SaleAmount = SaleAmount + {1} WHERE [Id] = {2};", num2, num3, productStatisticId);
				}
				else
				{
					StringBuilder stringBuilder2 = stringBuilder;
					object[] obj = new object[8];
					DateTime dateTime = DateTime.Now;
					dateTime = dateTime.Date;
					obj[0] = dateTime.ToString("yyyy-MM-dd");
					dateTime = DateTime.Now;
					obj[1] = dateTime.Year;
					dateTime = DateTime.Now;
					obj[2] = dateTime.Month;
					dateTime = DateTime.Now;
					obj[3] = dateTime.Day;
					obj[4] = (int)activityType;
					obj[5] = item.ProductId;
					obj[6] = num2;
					obj[7] = num3;
					stringBuilder2.AppendFormat("INSERT INTO Hishop_ProductDailyAccessStatistics(StatisticalDate,Year,Month,Day,ActivityType,ProductId,PV,UV,PaymentNum,SaleQuantity,SaleAmount) Values('{0}',{1},{2},{3},{4},{5},0,0,1,{6},{7})", obj);
				}
			}
			if (stringBuilder.ToString() == "")
			{
				return false;
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public int GetProductStatisticId(int productId, EnumTrafficActivityType activityType)
		{
			int result = 0;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT TOP 1 [Id] FROM Hishop_ProductDailyAccessStatistics WHERE ProductId = @ProductId AND ActivityType = @ActivityType AND Year = @Year AND Month = @Month AND Day = @Day");
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			base.database.AddInParameter(sqlStringCommand, "ActivityType", DbType.Int32, (int)activityType);
			Database database = base.database;
			DbCommand command = sqlStringCommand;
			DateTime now = DateTime.Now;
			database.AddInParameter(command, "Year", DbType.Int32, now.Year);
			Database database2 = base.database;
			DbCommand command2 = sqlStringCommand;
			now = DateTime.Now;
			database2.AddInParameter(command2, "Month", DbType.Int32, now.Month);
			Database database3 = base.database;
			DbCommand command3 = sqlStringCommand;
			now = DateTime.Now;
			database3.AddInParameter(command3, "Day", DbType.Int32, now.Day);
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = dataReader.GetInt32(0);
				}
			}
			return result;
		}

		public bool ClearProudctStatistic(int productId, int quantity, decimal amount, DateTime statisticDate, EnumTrafficActivityType activityType)
		{
			string format = "UPDATE Hishop_ProductDailyAccessStatistics SET SaleQuantity = SaleQuantity - {0},SaleAmount = SaleAmount - {1} WHERE ProductId ={2} AND StatisticalDate = '{3}' AND ActivityType = {4}";
			format = string.Format(format, quantity, amount, productId, statisticDate.ToString("yyyy-MM-dd"), (int)activityType);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(format);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool ClearProudctStatisticOfOrder(OrderInfo order, EnumTrafficActivityType activityType, int quantity, decimal refundAmount)
		{
			if (order.LineItems.Count <= 0)
			{
				return false;
			}
			string text = order.PayDate.ToString("yyyy-MM-dd");
			string format = "UPDATE Hishop_ProductDailyAccessStatistics SET SaleQuantity = SaleQuantity - {0},SaleAmount = SaleAmount - {1} WHERE ProductId ={2} AND StatisticalDate = '{3}' AND ActivityType = {4};";
			StringBuilder stringBuilder = new StringBuilder();
			decimal amount = order.GetAmount(false);
			decimal d = amount - (order.GetTotal(false) - order.AdjustedFreight);
			decimal num = default(decimal);
			if (amount > decimal.Zero)
			{
				num = d / amount;
			}
			if (num < decimal.Zero)
			{
				num = default(decimal);
			}
			foreach (LineItemInfo value in order.LineItems.Values)
			{
				int num2 = quantity;
				if (num2 <= 0)
				{
					num2 = value.Quantity;
				}
				decimal num3 = value.GetSubTotal() - value.GetSubTotal() * num;
				if (refundAmount > decimal.Zero)
				{
					num3 = refundAmount - refundAmount * num;
				}
				if (num3 < decimal.Zero)
				{
					num3 = default(decimal);
				}
				stringBuilder.AppendLine(string.Format(format, num2, num3, value.ProductId, text, (int)activityType));
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public PageModel<ProductStatisticsInfo> GetProductStatisticsData(ProductStatisticsQuery query)
		{
			string format = "SELECT * FROM(SELECT ROW_NUMBER() OVER(ORDER BY {2} {3}) AS RowNumber, p.ProductId, (SELECT ProductName FROM Hishop_Products where ProductId = p.ProductId) AS ProductName, SUM(PV) AS PV,SUM(UV) AS UV,SUM(PaymentNum) AS PaymentNum,SUM(SaleQuantity) AS SaleQuantity,SUM(SaleAmount) AS SaleAmount,(CASE WHEN SUM(UV) <=0 THEN 0 ELSE CONVERT(money,SUM(PaymentNum))/SUM(UV)*100.0 END) AS ProductConversionRate  FROM Hishop_ProductDailyAccessStatistics p right join Hishop_Products b on b.ProductId= p.ProductId WHERE  1 = 1  AND ({0})  GROUP BY p.ProductId) AS STable WHERE {1}";
			string format2 = "SELECT COUNT(*) AS Total FROM(SELECT ROW_NUMBER() OVER(ORDER BY SUM(PV) DESC) AS RowNumber FROM Hishop_ProductDailyAccessStatistics p right join Hishop_Products b on b.ProductId= p.ProductId WHERE  1 = 1  AND ({0})  GROUP BY p.ProductId) AS STable;";
			string text = "RowNumber BETWEEN 1 AND " + query.PageSize;
			if (query.PageIndex > 1)
			{
				text = $"RowNumber BETWEEN {(query.PageIndex - 1) * query.PageSize + 1} AND {query.PageIndex * query.PageSize}";
			}
			StringBuilder stringBuilder = new StringBuilder(" 1 = 1 ");
			DateTime dateTime;
			switch (query.LastConsumeTime)
			{
			case EnumConsumeTime.yesterday:
			{
				StringBuilder stringBuilder6 = stringBuilder;
				dateTime = DateTime.Now;
				dateTime = dateTime.AddDays(-1.0);
				stringBuilder6.AppendFormat(" AND DATEDIFF(dd,StatisticalDate,'{0}') = 0 ", dateTime.ToString("yyyy-MM-dd"));
				break;
			}
			case EnumConsumeTime.inOneWeek:
			{
				StringBuilder stringBuilder3 = stringBuilder;
				dateTime = DateTime.Now;
				dateTime = dateTime.AddDays(-7.0);
				string arg2 = dateTime.ToString("yyyy-MM-dd");
				dateTime = DateTime.Now;
				dateTime = dateTime.AddDays(-1.0);
				dateTime = dateTime.Date;
				stringBuilder3.AppendFormat(" AND (StatisticalDate BETWEEN '{0}' AND '{1}')", arg2, dateTime.ToString("yyyy-MM-dd"));
				break;
			}
			case EnumConsumeTime.inTwoWeek:
			{
				StringBuilder stringBuilder4 = stringBuilder;
				dateTime = DateTime.Now;
				dateTime = dateTime.AddDays(-14.0);
				string arg3 = dateTime.ToString("yyyy-MM-dd");
				dateTime = DateTime.Now;
				dateTime = dateTime.AddDays(-1.0);
				dateTime = dateTime.Date;
				stringBuilder4.AppendFormat(" AND (StatisticalDate BETWEEN '{0}' AND '{1}')", arg3, dateTime.ToString("yyyy-MM-dd"));
				break;
			}
			case EnumConsumeTime.inOneMonth:
			{
				StringBuilder stringBuilder5 = stringBuilder;
				dateTime = DateTime.Now;
				dateTime = dateTime.AddMonths(-1);
				string arg4 = dateTime.ToString("yyyy-MM-dd");
				dateTime = DateTime.Now;
				dateTime = dateTime.AddDays(-1.0);
				dateTime = dateTime.Date;
				stringBuilder5.AppendFormat(" AND (StatisticalDate BETWEEN '{0}' AND '{1}')", arg4, dateTime.ToString("yyyy-MM-dd"));
				break;
			}
			case EnumConsumeTime.custom:
				if (query.CustomConsumeStartTime.HasValue && query.CustomConsumeEndTime.HasValue)
				{
					StringBuilder stringBuilder2 = stringBuilder;
					dateTime = query.CustomConsumeStartTime.Value;
					string arg = dateTime.ToString("yyyy-MM-dd");
					dateTime = query.CustomConsumeEndTime.Value;
					stringBuilder2.AppendFormat(" AND (StatisticalDate BETWEEN '{0}' AND '{1}')", arg, dateTime.ToString("yyyy-MM-dd"));
				}
				break;
			}
			SortAction sortOrder = query.SortOrder;
			string text2 = "pv";
			if (!string.IsNullOrEmpty(query.SortBy))
			{
				text2 = ((!(query.SortBy != "productconversionrate")) ? "(CASE WHEN SUM(UV) <=0 THEN 0 ELSE CONVERT(money,SUM(PaymentNum))/SUM(UV)*100 END)" : ("SUM(" + query.SortBy + ")"));
			}
			format = string.Format(format, stringBuilder, text, text2, (sortOrder == SortAction.Desc) ? "DESC" : "ASC");
			format2 = string.Format(format2, stringBuilder);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(format + format2);
			PageModel<ProductStatisticsInfo> pageModel = new PageModel<ProductStatisticsInfo>();
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				pageModel.Models = DataHelper.ReaderToList<ProductStatisticsInfo>(dataReader);
				if (dataReader.NextResult() && dataReader.Read())
				{
					pageModel.Total = ((IDataRecord)dataReader)["Total"].ToInt(0);
				}
			}
			return pageModel;
		}

		public IList<ProductStatisticsInfo> GetProductStatisticsDataNoPage(ProductStatisticsQuery query)
		{
			string format = "SELECT * FROM(SELECT ROW_NUMBER() OVER(ORDER BY {1} {2}) AS RowNumber, ProductId, (SELECT ProductName FROM Hishop_Products where ProductId = p.ProductId) AS ProductName, SUM(PV) AS PV,SUM(UV) AS UV,SUM(PaymentNum) AS PaymentNum,SUM(SaleQuantity) AS SaleQuantity,SUM(SaleAmount) AS SaleAmount,(CASE WHEN SUM(UV) <=0 THEN 0 ELSE CONVERT(money,SUM(PaymentNum))/SUM(UV)*100.0 END) AS ProductConversionRate  FROM Hishop_ProductDailyAccessStatistics p WHERE  1 = 1  AND ({0})  GROUP BY ProductId) AS STable";
			StringBuilder stringBuilder = new StringBuilder(" 1 = 1 ");
			DateTime dateTime;
			switch (query.LastConsumeTime)
			{
			case EnumConsumeTime.yesterday:
			{
				StringBuilder stringBuilder6 = stringBuilder;
				dateTime = DateTime.Now;
				dateTime = dateTime.AddDays(-1.0);
				stringBuilder6.AppendFormat(" AND DATEDIFF(dd,StatisticalDate,'{0}') = 0 ", dateTime.ToString("yyyy-MM-dd"));
				break;
			}
			case EnumConsumeTime.inOneWeek:
			{
				StringBuilder stringBuilder3 = stringBuilder;
				dateTime = DateTime.Now;
				dateTime = dateTime.AddDays(-7.0);
				string arg2 = dateTime.ToString("yyyy-MM-dd");
				dateTime = DateTime.Now;
				dateTime = dateTime.AddDays(-1.0);
				dateTime = dateTime.Date;
				stringBuilder3.AppendFormat(" AND (StatisticalDate BETWEEN '{0}' AND '{1}')", arg2, dateTime.ToString("yyyy-MM-dd"));
				break;
			}
			case EnumConsumeTime.inTwoWeek:
			{
				StringBuilder stringBuilder4 = stringBuilder;
				dateTime = DateTime.Now;
				dateTime = dateTime.AddDays(-14.0);
				string arg3 = dateTime.ToString("yyyy-MM-dd");
				dateTime = DateTime.Now;
				dateTime = dateTime.AddDays(-1.0);
				dateTime = dateTime.Date;
				stringBuilder4.AppendFormat(" AND (StatisticalDate BETWEEN '{0}' AND '{1}')", arg3, dateTime.ToString("yyyy-MM-dd"));
				break;
			}
			case EnumConsumeTime.inOneMonth:
			{
				StringBuilder stringBuilder5 = stringBuilder;
				dateTime = DateTime.Now;
				dateTime = dateTime.AddMonths(-1);
				string arg4 = dateTime.ToString("yyyy-MM-dd");
				dateTime = DateTime.Now;
				dateTime = dateTime.AddDays(-1.0);
				dateTime = dateTime.Date;
				stringBuilder5.AppendFormat(" AND (StatisticalDate BETWEEN '{0}' AND '{1}')", arg4, dateTime.ToString("yyyy-MM-dd"));
				break;
			}
			case EnumConsumeTime.custom:
				if (query.CustomConsumeStartTime.HasValue && query.CustomConsumeEndTime.HasValue)
				{
					StringBuilder stringBuilder2 = stringBuilder;
					dateTime = query.CustomConsumeStartTime.Value;
					string arg = dateTime.ToString("yyyy-MM-dd");
					dateTime = query.CustomConsumeEndTime.Value;
					stringBuilder2.AppendFormat(" AND (StatisticalDate BETWEEN '{0}' AND '{1}')", arg, dateTime.ToString("yyyy-MM-dd"));
				}
				break;
			}
			SortAction sortOrder = query.SortOrder;
			string arg5 = "pv";
			if (!string.IsNullOrEmpty(query.SortBy))
			{
				arg5 = ((!(query.SortBy != "productconversionrate")) ? "(CASE WHEN SUM(UV) <=0 THEN 0 ELSE CONVERT(money,SUM(PaymentNum))/SUM(UV)*100 END)" : ("SUM(" + query.SortBy + ")"));
			}
			format = string.Format(format, stringBuilder, arg5, (sortOrder == SortAction.Desc) ? "DESC" : "ASC");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(format);
			IList<ProductStatisticsInfo> result = new List<ProductStatisticsInfo>();
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<ProductStatisticsInfo>(objReader);
			}
			return result;
		}

		public IList<ProductCategoryStatisticsInfo> GetProductCategoryStatisticsData(ProductStatisticsQuery query)
		{
			IList<ProductCategoryStatisticsInfo> result = new List<ProductCategoryStatisticsInfo>();
			string arg = " 1 = 1 ";
			string format = "SELECT CategoryId,Name AS CategoryName,ISNULL((SELECT SUM(SaleQuantity) FROM Hishop_ProductDailyAccessStatistics WHERE ProductId IN(SELECT ProductId FROM Hishop_Products WHERE (MainCategoryPath LIKE CONVERT(varchar,c.CategoryId)+'|%') OR CategoryId = c.CategoryId) AND {0}),0) AS SaleCounts,ISNULL((SELECT SUM(SaleAmount) FROM Hishop_ProductDailyAccessStatistics WHERE ProductId IN(SELECT ProductId FROM Hishop_Products WHERE (MainCategoryPath LIKE CONVERT(varchar,c.CategoryId)+'|%')  OR CategoryId = c.CategoryId) AND {0}),0) AS SaleAmounts FROM Hishop_Categories c WHERE Depth = 1";
			DateTime dateTime;
			switch (query.LastConsumeTime)
			{
			case EnumConsumeTime.yesterday:
				dateTime = DateTime.Now;
				dateTime = dateTime.AddDays(-1.0);
				arg = string.Format(" DATEDIFF(dd,StatisticalDate,'{0}') = 0 ", dateTime.ToString("yyyy-MM-dd"));
				break;
			case EnumConsumeTime.inOneWeek:
			{
				dateTime = DateTime.Now;
				dateTime = dateTime.AddDays(-7.0);
				string arg3 = dateTime.ToString("yyyy-MM-dd");
				dateTime = DateTime.Now;
				dateTime = dateTime.AddDays(-1.0);
				dateTime = dateTime.Date;
				arg = string.Format(" (StatisticalDate BETWEEN '{0}' AND '{1}')", arg3, dateTime.ToString("yyyy-MM-dd"));
				break;
			}
			case EnumConsumeTime.inTwoWeek:
			{
				dateTime = DateTime.Now;
				dateTime = dateTime.AddDays(-14.0);
				string arg4 = dateTime.ToString("yyyy-MM-dd");
				dateTime = DateTime.Now;
				dateTime = dateTime.AddDays(-1.0);
				dateTime = dateTime.Date;
				arg = string.Format(" (StatisticalDate BETWEEN '{0}' AND '{1}')", arg4, dateTime.ToString("yyyy-MM-dd"));
				break;
			}
			case EnumConsumeTime.inOneMonth:
			{
				dateTime = DateTime.Now;
				dateTime = dateTime.AddMonths(-1);
				string arg5 = dateTime.ToString("yyyy-MM-dd");
				dateTime = DateTime.Now;
				dateTime = dateTime.AddDays(-1.0);
				dateTime = dateTime.Date;
				arg = string.Format(" (StatisticalDate BETWEEN '{0}' AND '{1}')", arg5, dateTime.ToString("yyyy-MM-dd"));
				break;
			}
			case EnumConsumeTime.custom:
				if (query.CustomConsumeStartTime.HasValue && query.CustomConsumeEndTime.HasValue)
				{
					dateTime = query.CustomConsumeStartTime.Value;
					string arg2 = dateTime.ToString("yyyy-MM-dd");
					dateTime = query.CustomConsumeEndTime.Value;
					arg = string.Format(" (StatisticalDate BETWEEN '{0}' AND '{1}')", arg2, dateTime.ToString("yyyy-MM-dd"));
				}
				break;
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(string.Format(format, arg));
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<ProductCategoryStatisticsInfo>(objReader);
			}
			return result;
		}
	}
}
