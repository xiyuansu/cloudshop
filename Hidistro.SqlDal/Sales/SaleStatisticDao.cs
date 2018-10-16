using Hidistro.Core;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Sales;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Sales
{
	public class SaleStatisticDao : BaseDao
	{
		public RecentlyOrderStatic GetNewlyOrdersCountAndPayCount(DateTime dt, int StoreId = 0, int SupplierId = 0)
		{
			RecentlyOrderStatic recentlyOrderStatic = new RecentlyOrderStatic();
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(" ");
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT Count(*) as OrderCount ");
			if (StoreId > 0)
			{
				stringBuilder.AppendFormat("FROM Hishop_Orders WHERE OrderStatus= {0} AND ParentOrderId<>'-1' AND DATEDIFF(s,OrderDate,'{1}')<=0 AND StoreId = {2};", 1, dt.ToString(), StoreId);
				stringBuilder.AppendFormat("SELECT  Count(*) as PayOrderCount FROM Hishop_Orders WHERE OrderStatus = {0} AND ParentOrderId<>'-1' AND DATEDIFF(s,payDate,'{1}') <= 0 AND StoreId = {2};", 2, dt.ToString(), StoreId);
				stringBuilder.AppendFormat("SELECT  Count(*) as RefundOrderCount FROM Hishop_OrderRefund WHERE HandleStatus = 0 AND DATEDIFF(s,ApplyForTime,'{0}') <= 0 AND StoreId = {1};", dt.ToString(), StoreId);
				stringBuilder.AppendFormat("SELECT  Count(*) as ReplacementOrderCount FROM Hishop_OrderReplace WHERE HandleStatus = 0 AND DATEDIFF(s,ApplyForTime,'{0}') <= 0 AND StoreId = {1};", dt.ToString(), StoreId);
				stringBuilder.AppendFormat("SELECT  Count(*) as ReturnsOrderCount FROM Hishop_OrderReturns WHERE HandleStatus = 0 AND DATEDIFF(s,ApplyForTime,'{0}') <= 0 AND StoreId = {1};", dt.ToString(), StoreId);
			}
			else if (SupplierId > 0)
			{
				stringBuilder.AppendFormat("FROM Hishop_Orders WHERE OrderStatus= {0} AND ParentOrderId<>'-1' AND DATEDIFF(s,OrderDate,'{1}')<=0 AND SupplierId = {2};", 1, dt.ToString(), SupplierId);
				stringBuilder.AppendFormat("SELECT  Count(*) as PayOrderCount FROM Hishop_Orders WHERE OrderStatus = {0} AND ParentOrderId<>'-1' AND DATEDIFF(s,payDate,'{1}') <= 0 AND SupplierId = {2};", 2, dt.ToString(), SupplierId);
				stringBuilder.AppendFormat("SELECT  Count(*) as RefundOrderCount FROM Hishop_OrderRefund  orf join Hishop_Orders o on orf.OrderId=o.OrderId WHERE  orf.HandleStatus = 0 AND DATEDIFF(s,ApplyForTime,'{0}') <= 0 AND SupplierId = {1};", dt.ToString(), SupplierId);
				stringBuilder.AppendFormat("SELECT  Count(*) as ReplacementOrderCount FROM Hishop_OrderReplace orp join Hishop_Orders o on orp.OrderId=o.OrderId WHERE HandleStatus = 0 AND DATEDIFF(s,ApplyForTime,'{0}') <= 0 AND SupplierId = {1};", dt.ToString(), SupplierId);
				stringBuilder.AppendFormat("SELECT  Count(*) as ReturnsOrderCount FROM Hishop_OrderReturns ort join Hishop_Orders o on ort.OrderId=o.OrderId WHERE HandleStatus = 0 AND DATEDIFF(s,ApplyForTime,'{0}') <= 0 AND SupplierId = {1};", dt.ToString(), SupplierId);
			}
			else
			{
				stringBuilder.AppendFormat("FROM Hishop_Orders WHERE OrderStatus= {0} AND ParentOrderId<>'-1' AND DATEDIFF(s,OrderDate,'{1}')<=0 AND SupplierId = 0 AND StoreId=0;", 1, dt.ToString());
				stringBuilder.AppendFormat("SELECT  Count(*) as PayOrderCount FROM Hishop_Orders WHERE OrderStatus = {0} AND ParentOrderId<>'-1' AND DATEDIFF(s,payDate,'{1}') <= 0 AND SupplierId = 0 AND StoreId=0;", 2, dt.ToString());
				stringBuilder.AppendFormat("SELECT  Count(*) as RefundOrderCount FROM Hishop_OrderRefund  orf join Hishop_Orders o on orf.OrderId=o.OrderId WHERE  orf.HandleStatus = 0 AND DATEDIFF(s,ApplyForTime,'{0}') <= 0 AND o.SupplierId = 0 AND o.StoreId=0;", dt.ToString());
				stringBuilder.AppendFormat("SELECT  Count(*) as ReplacementOrderCount FROM Hishop_OrderReplace orp join Hishop_Orders o on orp.OrderId=o.OrderId WHERE HandleStatus = 0 AND DATEDIFF(s,ApplyForTime,'{0}') <= 0 AND o.SupplierId = 0 AND o.StoreId=0;", dt.ToString());
				stringBuilder.AppendFormat("SELECT  Count(*) as ReturnsOrderCount FROM Hishop_OrderReturns ort join Hishop_Orders o on ort.OrderId=o.OrderId WHERE HandleStatus = 0 AND DATEDIFF(s,ApplyForTime,'{0}') <= 0 AND o.SupplierId = 0 AND o.StoreId=0;", dt.ToString());
			}
			sqlStringCommand.CommandText = stringBuilder.ToString();
			recentlyOrderStatic.OrdersCount = 0;
			recentlyOrderStatic.PayCount = 0;
			recentlyOrderStatic.RefundOrderCount = 0;
			recentlyOrderStatic.ReplacementOrderCount = 0;
			recentlyOrderStatic.ReturnsOrderCount = 0;
			try
			{
				using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
				{
					if (dataReader.Read())
					{
						recentlyOrderStatic.OrdersCount = dataReader.GetInt32(0);
					}
					if (dataReader.NextResult() && dataReader.Read())
					{
						recentlyOrderStatic.PayCount = dataReader.GetInt32(0);
					}
					if (dataReader.NextResult() && dataReader.Read())
					{
						recentlyOrderStatic.RefundOrderCount = dataReader.GetInt32(0);
					}
					if (dataReader.NextResult() && dataReader.Read())
					{
						recentlyOrderStatic.ReplacementOrderCount = dataReader.GetInt32(0);
					}
					if (dataReader.NextResult() && dataReader.Read())
					{
						recentlyOrderStatic.ReturnsOrderCount = dataReader.GetInt32(0);
					}
				}
			}
			catch (Exception ex)
			{
				Globals.WriteLog("/static.txt", ex.Message + "-" + dt.ToString());
			}
			if (recentlyOrderStatic.OrdersCount > 0 || recentlyOrderStatic.PayCount > 0 || recentlyOrderStatic.RefundOrderCount > 0 || recentlyOrderStatic.ReplacementOrderCount > 0 || recentlyOrderStatic.ReturnsOrderCount > 0)
			{
				recentlyOrderStatic.HasOrderSatic = true;
			}
			else
			{
				recentlyOrderStatic.HasOrderSatic = false;
			}
			return recentlyOrderStatic;
		}

		public AdminStatisticsInfo GetStatistics(int memberBrithDaySetting = 0)
		{
			Database database = base.database;
			object[] obj = new object[9]
			{
				"SELECT  (SELECT COUNT(OrderId) FROM Hishop_Orders WHERE (OrderStatus = 2  OR (OrderStatus = 1 AND Gateway = 'hishop.plugins.payment.podrequest')) and ShippingModeId!=-2 and ParentOrderId<>'-1' AND SupplierId=0 AND OrderType<>6) AS orderNumbWaitConsignment,(select Count(ConsultationId) from Hishop_ProductConsultations where ReplyUserId is null) as productConsultations,(select Count(*) from Hishop_ManagerMessageBox where IsRead=0 and Accepter='admin' and Sernder in (select UserName from aspnet_Members)) as messages, isnull((select sum(PaymentAmount-RefundAmount) from Hishop_OrderDailyStatistics where StatisticalDate='",
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null
			};
			DateTime now = DateTime.Now;
			obj[1] = DataHelper.GetSafeDateTimeFormat(now.Date);
			obj[2] = "'),0) as orderPriceToday, isnull((select sum(OrderProfit) from Hishop_Orders where  (OrderStatus<>1 AND OrderStatus<>4 AND OrderStatus<>9 AND ParentOrderId<>'-1')  and OrderDate>='";
			now = DateTime.Now;
			obj[3] = DataHelper.GetSafeDateTimeFormat(now.Date);
			obj[4] = "'),0) as orderProfitToday, (select count(*) from aspnet_Members where CreateDate>='";
			now = DateTime.Now;
			obj[5] = DataHelper.GetSafeDateTimeFormat(now.Date);
			obj[6] = "' ) as userNewAddToday, isnull((select sum(balance) from aspnet_Members),0) as memberBalance,(select count(*) from Hishop_BalanceDrawRequest WHERE ISPASS IS NULL) as memberBlancedraw,(select count(*) from Hishop_Orders where datediff(dd,getdate(),OrderDate)=0 and ParentOrderId<>'-1' AND (OrderStatus<>1 AND OrderStatus<>4 AND OrderStatus<>9)) as todayFinishOrder,(select count(*) from Hishop_Orders where datediff(dd,getdate()-1,OrderDate)=0 and ParentOrderId<>'-1' AND (OrderStatus<>1 AND OrderStatus<>4 AND OrderStatus<>9)) as yesterdayFinishOrder, isnull((select sum(OrderTotal)-isnull(sum(RefundAmount),0) from hishop_orders where (OrderStatus<>1 AND OrderStatus<>4 AND OrderStatus<>9 AND ParentOrderId<>'-1')   and datediff(dd,getdate()-1,OrderDate)=0),0) as orderPriceYesterDay,(select count(*) from aspnet_Members where datediff(dd,getdate()-1,CreateDate)=0) as userNewAddYesterToday,(select count(*) from aspnet_Members) as TotalMembers,(select count(*) from Hishop_Products where SaleStatus!=0) as TotalProducts, isnull((select sum(PaymentAmount-RefundAmount) from Hishop_OrderDailyStatistics where datediff(dd,StatisticalDate,getdate())<=30),0) as orderPriceMonth,(select count(*) from Hishop_Products where SupplierId>0 and AuditStatus=1 and SaleStatus=3) as SupplierProducts4Audit,(select count(*) from Hishop_Products where SupplierId>0 and AuditStatus=2) as SupplierTotalProducts,(select count(*) from Hishop_SupplierBalanceDrawRequest WHERE ISPASS IS NULL) as SupplierBlancedrawRequestNum,(select isnull(sum(Expenses),0) from Hishop_SupplierBalanceDetails) as SupplierBlancedrawTotal,(select count(*)  from aspnet_Members where dateadd(year,year(getdate())-year(BirthDate),BirthDate) between DATEADD(DD,-1,getdate()) and dateadd(day,";
			obj[7] = memberBrithDaySetting;
			obj[8] = ",getdate())) as MemberBirthdayNum";
			DbCommand sqlStringCommand = database.GetSqlStringCommand(string.Concat(obj));
			AdminStatisticsInfo adminStatisticsInfo = new AdminStatisticsInfo();
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					adminStatisticsInfo.OrderNumbWaitConsignment = (int)((IDataRecord)dataReader)["orderNumbWaitConsignment"];
					adminStatisticsInfo.ProductConsultations = (int)((IDataRecord)dataReader)["ProductConsultations"];
					adminStatisticsInfo.Messages = (int)((IDataRecord)dataReader)["Messages"];
					adminStatisticsInfo.OrderProfitToday = (decimal)((IDataRecord)dataReader)["orderProfitToday"];
					adminStatisticsInfo.UserNewAddToday = (int)((IDataRecord)dataReader)["userNewAddToday"];
					adminStatisticsInfo.MembersBalance = (decimal)((IDataRecord)dataReader)["memberBalance"];
					adminStatisticsInfo.OrderPriceToday = (decimal)((IDataRecord)dataReader)["orderPriceToday"];
					adminStatisticsInfo.MemberBlancedrawRequest = (int)((IDataRecord)dataReader)["memberBlancedraw"];
					adminStatisticsInfo.TodayFinishOrder = (int)((IDataRecord)dataReader)["todayFinishOrder"];
					adminStatisticsInfo.YesterdayFinishOrder = (int)((IDataRecord)dataReader)["yesterdayFinishOrder"];
					adminStatisticsInfo.OrderPriceYesterDay = (decimal)((IDataRecord)dataReader)["orderPriceYesterDay"];
					adminStatisticsInfo.UserNewAddYesterToday = (int)((IDataRecord)dataReader)["userNewAddYesterToday"];
					adminStatisticsInfo.TotalMembers = (int)((IDataRecord)dataReader)["TotalMembers"];
					adminStatisticsInfo.TotalProducts = (int)((IDataRecord)dataReader)["TotalProducts"];
					adminStatisticsInfo.OrderPriceMonth = (decimal)((IDataRecord)dataReader)["OrderPriceMonth"];
					adminStatisticsInfo.SupplierProducts4Audit = (int)((IDataRecord)dataReader)["SupplierProducts4Audit"];
					adminStatisticsInfo.SupplierBlancedrawRequestNum = (int)((IDataRecord)dataReader)["SupplierBlancedrawRequestNum"];
					adminStatisticsInfo.SupplierTotalProducts = (int)((IDataRecord)dataReader)["SupplierTotalProducts"];
					adminStatisticsInfo.SupplierBlancedrawTotal = (decimal)((IDataRecord)dataReader)["SupplierBlancedrawTotal"];
					adminStatisticsInfo.MemberBirthdayNum = (int)((IDataRecord)dataReader)["MemberBirthdayNum"];
				}
			}
			return adminStatisticsInfo;
		}

		public StoreStatisticsInfo GetStoreOrderStatistics(int storeId)
		{
			StringBuilder stringBuilder = new StringBuilder("SELECT ");
			stringBuilder.Append("(SELECT COUNT(OrderId) FROM Hishop_Orders WHERE IsConfirm = 0 AND ItemStatus = 0 AND StoreId = {0} AND ShippingModeId = -2 AND ParentOrderId<>'-1' AND (OrderStatus = " + 2 + " OR (OrderStatus = " + 1 + " AND PaymentTypeId = -3))) AS WaitConfirmTotal,");
			stringBuilder.Append("(SELECT COUNT(OrderId) FROM Hishop_Orders WHERE (OrderStatus = " + 2 + "  OR (OrderStatus = " + 1 + " AND Gateway = 'hishop.plugins.payment.podrequest')) AND ItemStatus = 0 AND ShippingModeId <> -2 AND ParentOrderId<>'-1' AND OrderType <>" + 6 + " AND StoreId = {0}) AS WaitSendGoodsTotal,");
			stringBuilder.Append("(SELECT COUNT(OrderId) FROM Hishop_Orders WHERE IsConfirm = 1 AND (ShippingModeId = -2) AND StoreId = {0} AND ItemStatus = 0 AND ParentOrderId<>'-1' AND (OrderStatus = " + 2 + " OR OrderStatus = " + 1 + ")) AS WaitPickGoodsTotal,");
			stringBuilder.Append("(SELECT COUNT(OrderId) FROM Hishop_Orders WHERE OrderStatus = " + 3 + " AND StoreId = {0} AND ParentOrderId<>'-1' AND ShippingModeId  <> -2) AS WaitGetGoodsTotal,");
			string text = " StoreId = {0} ";
			text = text + " AND (HandleStatus = " + 0 + " AND AfterSaleType = " + 0;
			text = text + " OR (HandleStatus NOT IN(" + 2 + "," + 1 + ") AND AfterSaleType = " + 1 + ")";
			text = text + " OR (HandleStatus = " + 0 + " AND AfterSaleType = " + 3 + ")";
			text = text + " OR (HandleStatus NOT IN(" + 2 + "," + 1 + ") AND AfterSaleType = " + 2 + ")";
			text += ")";
			stringBuilder.Append("(SELECT COUNT(*) FROM vw_Hishop_AfterSaleRecords WHERE " + text + ") AS WaitDealAfterSaleTotal,");
			stringBuilder.Append("(SELECT COUNT(*) FROM vw_Hishop_AfterSaleRecords WHERE StoreId = {0} and HandleStatus in (0,4) and AfterSaleType in(1,2)) AS WaitReceiptAfterSaleTotal,");
			stringBuilder.Append("(SELECT COUNT(OrderId) FROM Hishop_Orders WHERE OrderDate >= '" + DataHelper.GetSafeDateTimeFormat(DateTime.Now.Date) + "' AND ParentOrderId<>'-1' ) AS TodayOrderTotal,");
			stringBuilder.Append("(SELECT COUNT(DISTINCT(ProductId)) FROM Hishop_StoreSKUs WHERE (Stock <= 0 OR Stock <= WarningStock)  AND StoreId = {0}) AS StockWarningTotal");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(string.Format(stringBuilder.ToString(), storeId));
			StoreStatisticsInfo storeStatisticsInfo = new StoreStatisticsInfo();
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					storeStatisticsInfo.WaitConfirmTotal = ((IDataRecord)dataReader)["WaitConfirmTotal"].ToInt(0);
					storeStatisticsInfo.WaitSendGoodsTotal = ((IDataRecord)dataReader)["WaitSendGoodsTotal"].ToInt(0);
					storeStatisticsInfo.WaitPickGoodsTotal = ((IDataRecord)dataReader)["WaitPickGoodsTotal"].ToInt(0);
					storeStatisticsInfo.WaitGetGoodsTotal = ((IDataRecord)dataReader)["WaitGetGoodsTotal"].ToInt(0);
					storeStatisticsInfo.WaitDealAfterSaleTotal = ((IDataRecord)dataReader)["WaitDealAfterSaleTotal"].ToInt(0);
					storeStatisticsInfo.TodayOrderTotal = ((IDataRecord)dataReader)["TodayOrderTotal"].ToInt(0);
					storeStatisticsInfo.StockWarningTotal = ((IDataRecord)dataReader)["StockWarningTotal"].ToInt(0);
					storeStatisticsInfo.WaitReceiptAfterSaleTotal = ((IDataRecord)dataReader)["WaitReceiptAfterSaleTotal"].ToInt(0);
				}
			}
			return storeStatisticsInfo;
		}

		public int GetStoreOrderStatistics(int storeId, int status)
		{
			StringBuilder stringBuilder = new StringBuilder();
			DbCommand sqlStringCommand;
			switch (status)
			{
			case 2:
				stringBuilder.Append("SELECT COUNT(OrderId) FROM Hishop_Orders WHERE IsConfirm = 0 AND ItemStatus = 0 AND StoreId = {0} AND ShippingModeId = -2 AND ParentOrderId<>'-1' AND (OrderStatus = " + 2 + " OR (OrderStatus = " + 1 + " AND PaymentTypeId = -3))");
				goto IL_00af;
			case 30:
				stringBuilder.Append("SELECT COUNT(OrderId) FROM Hishop_Orders WHERE (OrderStatus = " + 2 + "  OR (OrderStatus = " + 1 + " AND Gateway = 'hishop.plugins.payment.podrequest')) AND ItemStatus = 0 AND ShippingModeId <> -2 AND ParentOrderId<>'-1' AND OrderType <>" + 6 + " AND StoreId = {0}");
				goto IL_00af;
			default:
				{
					return 0;
				}
				IL_00af:
				sqlStringCommand = base.database.GetSqlStringCommand(string.Format(stringBuilder.ToString(), storeId));
				return base.database.ExecuteScalar(sqlStringCommand).ToInt(0);
			}
		}

		public StoreStatisticsInfo GetStoreCashierStatistics(int storeId, DateTime? startTime, DateTime? endTime)
		{
			if (!startTime.HasValue)
			{
				startTime = DateTime.Now.Date;
			}
			string text = " AND {0} >= '" + DataHelper.GetSafeDateTimeFormat(startTime.Value) + "'";
			if (endTime.HasValue)
			{
				text = text + " AND {0} <= '" + DataHelper.GetSafeDateTimeFormat(endTime.Value) + "'";
			}
			string text2 = "hishop.plugins.payment.cashreceipts";
			StringBuilder stringBuilder = new StringBuilder("SELECT ");
			stringBuilder.Append("(SELECT SUM(OrderTotal) FROM Hishop_Orders WHERE StoreId = {0} " + string.Format(text, "PayDate") + " AND ParentOrderId<>'-1' ) AS TodayCashierTotal,");
			stringBuilder.Append("(SELECT SUM(OrderTotal) FROM Hishop_Orders WHERE StoreId = {0} AND LOWER(GateWay) = '" + text2 + "'" + string.Format(text, "PayDate") + " AND ParentOrderId<>'-1' ) AS CashCashierTotal,");
			stringBuilder.Append("(SELECT SUM(OrderTotal) FROM Hishop_Orders WHERE StoreId = {0} AND LOWER(GateWay) <> '" + text2 + "'" + string.Format(text, "PayDate") + " AND ParentOrderId<>'-1' ) AS OnlinePayCashierTotal,");
			stringBuilder.Append("(SELECT SUM(OrderTotal) FROM Hishop_Orders WHERE StoreId = {0} AND ShippingModeId = -2" + string.Format(text, "PayDate") + " AND ParentOrderId<>'-1' ) AS OnDoorCashierTotal,");
			stringBuilder.Append("(SELECT SUM(OrderTotal) FROM Hishop_Orders WHERE StoreId = {0} AND IsStoreCollect = 1" + string.Format(text, "PayDate") + " AND ParentOrderId<>'-1' ) AS OfflineCashierTotal,");
			stringBuilder.Append("(SELECT SUM(OrderTotal) FROM Hishop_Orders WHERE StoreId = {0} AND IsStoreCollect = 0" + string.Format(text, "PayDate") + " AND ParentOrderId<>'-1' ) AS PlatCashierTotal");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			StoreStatisticsInfo storeStatisticsInfo = new StoreStatisticsInfo();
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					storeStatisticsInfo.TodayCashierTotal = ((IDataRecord)dataReader)["TodayCashierTotal"].ToDecimal(0);
					storeStatisticsInfo.CashCashierTotal = ((IDataRecord)dataReader)["CashCashierTotal"].ToDecimal(0);
					storeStatisticsInfo.OnlinePayCashierTotal = ((IDataRecord)dataReader)["OnlinePayCashierTotal"].ToDecimal(0);
					storeStatisticsInfo.OnDoorCashierTotal = ((IDataRecord)dataReader)["OnDoorCashierTotal"].ToDecimal(0);
					storeStatisticsInfo.OfflineCashierTotal = ((IDataRecord)dataReader)["OfflineCashierTotal"].ToDecimal(0);
					storeStatisticsInfo.PlatCashierTotal = ((IDataRecord)dataReader)["PlatCashierTotal"].ToDecimal(0);
				}
			}
			return storeStatisticsInfo;
		}

		public decimal GetSaleTotalByUserId(int userId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT ISNULL(SUM(ISNULL(OrderTotal,0)- ISNULL(RefundAmount,0)),0) FROM Hishop_Orders WHERE OrderStatus != {0} AND OrderStatus != {1} AND ParentOrderId<>'-1' ", 1, 4);
			stringBuilder.AppendFormat(" AND userId = {0}", userId);
			return decimal.Parse(base.database.ExecuteScalar(CommandType.Text, stringBuilder.ToString()).ToString());
		}
	}
}
