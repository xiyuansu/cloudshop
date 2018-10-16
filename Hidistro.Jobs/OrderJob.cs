using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Jobs;
using Hidistro.Entities;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Member;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text;
using System.Xml;

namespace Hidistro.Jobs
{
	public class OrderJob : IJob
	{
		private Database database = DatabaseFactory.CreateDatabase();

		private SiteSettings setting = null;

		public void Execute(XmlNode node)
		{
			this.setting = SettingsManager.GetMasterSettings();
			this.ManagementCountDownOrders();
			this.ManagementFightGroupOrders();
			this.ManagePreSaleOrders();
			this.ManagementOrders();
			this.ProcessorSplittin();
			this.ProcessOrderServiceOver();
			this.DeleteNoPayedStoreOrder();
			this.ProcessorEvaluate(this.setting);
		}

		private void UpdateMemberPoints()
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT OrderId,UserId,OrderPoint,DeductionPoints,OrderStatus,TimesPoint,(SELECT SUM(ISNULL(RefundAmount,0)) FROM Hishop_OrderItems oi WHERE o.OrderId = oi.OrderId AND oi.Status = " + 24 + ") AS RefundAmount,AdjustedFreight,OrderTotal FROM Hishop_Orders o WHERE OrderStatus = " + 5 + " AND UserId <> 0 AND DATEDIFF(HH,FinishDate,getdate()) >= @OrderEndDays AND (ItemStatus = 0 OR ItemStatus = 6)  AND IsServiceOver = 0 ");
			this.database.AddInParameter(sqlStringCommand, "OrderEndDays", DbType.Int32, this.setting.EndOrderDays * 24);
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					int points = ((IDataRecord)dataReader)["OrderPoint"].ToInt(0);
					decimal num = ((IDataRecord)dataReader)["RefundAmount"].ToDecimal(0);
					decimal d = ((IDataRecord)dataReader)["AdjustedFreight"].ToDecimal(0);
					decimal d2 = ((IDataRecord)dataReader)["OrderTotal"].ToDecimal(0);
					decimal d3 = ((IDataRecord)dataReader)["TimesPoint"].ToDecimal(0);
					if (num > decimal.Zero)
					{
						d2 = d2 - d - num;
						points = (d2 * d3 / this.setting.PointsRate).ToInt(0);
					}
					this.UpdateUserPoint(((IDataRecord)dataReader)["OrderId"].ToNullString(), points, 0, ((IDataRecord)dataReader)["UserId"].ToInt(0), null);
				}
			}
		}

		private void UpdateUserPoint(string orderId, int points, int? deductionPoints, int userId, DbTransaction dbTran = null)
		{
			Hidistro.Entities.Members.MemberInfo user = Users.GetUser(userId);
			if (user != null)
			{
				this.AddPointDetail(user, orderId, points, deductionPoints, PointTradeType.Bounty, dbTran);
				int historyPoint = this.GetHistoryPoint(user.UserId, dbTran);
				this.ChangeMemberGrade(user.UserId, user.GradeId, historyPoint, dbTran);
			}
		}

		private int GetHistoryPoint(int userId, DbTransaction tran = null)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT SUM(Increased) FROM Hishop_PointDetails WHERE UserId = @UserId AND TradeType <> " + 3);
			this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			if (tran != null)
			{
				return this.database.ExecuteScalar(sqlStringCommand, tran).ToInt(0);
			}
			return this.database.ExecuteScalar(sqlStringCommand).ToInt(0);
		}

		private bool ChangeMemberGrade(int userId, int gradId, int points, DbTransaction dbTran = null)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ISNULL(Points, 0) AS Point, GradeId FROM aspnet_MemberGrades Order by Point Desc ");
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read() && (int)((IDataRecord)dataReader)["GradeId"] != gradId)
				{
					if ((int)((IDataRecord)dataReader)["Point"] <= points)
					{
						return this.UpdateUserRank(userId, (int)((IDataRecord)dataReader)["GradeId"], dbTran);
					}
				}
				return true;
			}
		}

		private bool UpdateUserRank(int userId, int gradeId, DbTransaction dbTran = null)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Members SET GradeId = @GradeId WHERE UserId = @UserId");
			this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, gradeId);
			this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			if (dbTran != null)
			{
				return this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0;
			}
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		private void AddPointDetail(Hidistro.Entities.Members.MemberInfo member, string orderId, int points, int? deductionPoints, PointTradeType pType, DbTransaction dbTran)
		{
			PointDetailInfo pointDetailInfo = new PointDetailInfo();
			pointDetailInfo.OrderId = orderId;
			pointDetailInfo.UserId = member.UserId;
			pointDetailInfo.TradeDate = DateTime.Now;
			pointDetailInfo.TradeType = pType;
			switch (pType)
			{
			case PointTradeType.Bounty:
				pointDetailInfo.Increased = points;
				pointDetailInfo.Points = points + member.Points;
				break;
			case PointTradeType.ShoppingDeduction:
				pointDetailInfo.Reduced = (deductionPoints.HasValue ? deductionPoints.Value : 0);
				pointDetailInfo.Points = member.Points - (deductionPoints.HasValue ? deductionPoints.Value : 0);
				break;
			}
			if (pointDetailInfo.Points > 2147483647)
			{
				pointDetailInfo.Points = 2147483647;
			}
			if (pointDetailInfo.Points < 0)
			{
				pointDetailInfo.Points = 0;
			}
			if (pointDetailInfo.Increased > 0 || pointDetailInfo.Reduced > 0)
			{
				BaseDao baseDao = new BaseDao();
				baseDao.Add(pointDetailInfo, dbTran);
				member.Points = pointDetailInfo.Points;
			}
		}

		private void WriteLog(Exception ex, DateTime startTime, DateTime endTime)
		{
			TimeSpan timeSpan = endTime - startTime;
			if (timeSpan.TotalMilliseconds == 0.0)
			{
				endTime = DateTime.Now;
			}
			timeSpan = endTime - startTime;
			double totalMilliseconds = timeSpan.TotalMilliseconds;
			IDictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("ErrorMessage", ex.Message);
			dictionary.Add("StackTrace", ex.StackTrace);
			if (ex.InnerException != null)
			{
				dictionary.Add("InnerException", ex.InnerException.ToString());
			}
			if (ex.GetBaseException() != null)
			{
				dictionary.Add("BaseException", ex.GetBaseException().Message);
			}
			if (ex.TargetSite != (MethodBase)null)
			{
				dictionary.Add("TargetSite", ex.TargetSite.ToString());
			}
			dictionary.Add("ExSource", ex.Source);
			dictionary.Add("CostTime", totalMilliseconds.ToString());
			Globals.WriteLog(dictionary, "", "", "", "JobError");
		}

		private void DeleteNoPayedStoreOrder()
		{
			DateTime now = DateTime.Now;
			try
			{
				DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_StoreCollections WHERE Status = " + 0 + " AND CreateTime < @CreateTime");
				this.database.AddInParameter(sqlStringCommand, "CreateTime", DbType.DateTime, DateTime.Now.AddHours(-2.0));
				this.database.ExecuteNonQuery(sqlStringCommand);
			}
			catch (Exception ex)
			{
				this.WriteLog(ex, now, DateTime.Now);
			}
		}

		private void ManagementFightGroupOrders()
		{
			DateTime now = DateTime.Now;
			try
			{
				DbCommand sqlStringCommand = this.database.GetSqlStringCommand("\r\nSELECT fg.FightGroupId, oi.SkuId, oi.Quantity,o.IsFightGroupHead,o.BalanceAmount,o.OrderId,o.UserId\r\n  FROM Hishop_Orders o\r\n       INNER JOIN Hishop_OrderItems oi ON o.OrderId = oi.OrderId\r\n       INNER JOIN Hishop_FightGroups fg ON o.FightGroupId = fg.FightGroupId\r\n       INNER JOIN Hishop_FightGroupActivities fga\r\n          ON fg.FightGroupActivityId = fga.FightGroupActivityId\r\n WHERE (o.OrderStatus = 1 and (o.IsConfirm = 0 or o.IsConfirm is null)) AND o.OrderDate <= @OrderDate and (o.CountDownBuyId is null or o.CountDownBuyId = 0)\r\n");
				this.database.AddInParameter(sqlStringCommand, "OrderDate", DbType.DateTime, DateTime.Now.AddDays((double)(-this.setting.CloseOrderDays)));
				IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand);
				while (dataReader.Read())
				{
					int fightGroupId = ((IDataRecord)dataReader)["FightGroupId"].ToInt(0);
					string skuId = ((IDataRecord)dataReader)["SkuId"].ToNullString();
					int quantity = ((IDataRecord)dataReader)["Quantity"].ToInt(0);
					int userId = ((IDataRecord)dataReader)["UserId"].ToInt(0);
					decimal balanceAmount = ((IDataRecord)dataReader)["BalanceAmount"].ToDecimal(0);
					string orderId = ((IDataRecord)dataReader)["OrderId"].ToNullString();
					this.CloseOrderToReduceFightGroup(fightGroupId, skuId, quantity);
					this.CloseReturnBalance(userId, orderId, balanceAmount);
				}
				dataReader.Close();
				dataReader.Dispose();
			}
			catch (Exception ex)
			{
				this.WriteLog(ex, now, DateTime.Now);
			}
		}

		private void CloseOrderToReduceFightGroup(int fightGroupId, string skuId, int quantity)
		{
			BaseDao baseDao = new BaseDao();
			FightGroupInfo fightGroupInfo = baseDao.Get<FightGroupInfo>(fightGroupId);
			if (fightGroupInfo != null)
			{
				FightGroupSkuInfo groupSkuInfoByActivityIdSkuId = this.GetGroupSkuInfoByActivityIdSkuId(fightGroupInfo.FightGroupActivityId, skuId);
				if (groupSkuInfoByActivityIdSkuId != null)
				{
					groupSkuInfoByActivityIdSkuId.BoughtCount -= quantity;
					baseDao.Update(groupSkuInfoByActivityIdSkuId, null);
				}
			}
		}

		private FightGroupSkuInfo GetGroupSkuInfoByActivityIdSkuId(int fightGroupActivityId, string skuId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT top 1 * FROM Hishop_FightGroupSkus WHERE FightGroupActivityId=@FightGroupActivityId AND SkuId=@SkuId");
			this.database.AddInParameter(sqlStringCommand, "FightGroupActivityId", DbType.Int32, fightGroupActivityId);
			this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
			using (IDataReader objReader = this.database.ExecuteReader(sqlStringCommand))
			{
				return DataHelper.ReaderToModel<FightGroupSkuInfo>(objReader);
			}
		}

		private void NormalOrderCloseReturnBalance()
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT BalanceAmount,OrderId,UserId FROM Hishop_Orders WHERE BalanceAmount > 0 AND LOWER(Gateway) <> 'hishop.plugins.payment.podrequest' and (OrderStatus = 1 AND(IsConfirm = 0 or IsConfirm is null)) AND BalanceAmount > 0 AND BalanceAmount IS NOT NULL AND OrderDate <= @OrderDate AND EXISTS(SELECT UserId FROM aspnet_Members WHERE UserId = Hishop_Orders.UserId) AND(CountDownBuyId is null or CountDownBuyId = 0) AND(PreSaleId IS NULL OR PreSaleId = 0) AND(ParentOrderId = '0' OR ParentOrderId = '-1')");
			this.database.AddInParameter(sqlStringCommand, "OrderDate", DbType.DateTime, DateTime.Now.AddDays((double)(-this.setting.CloseOrderDays)));
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					decimal balanceAmount = ((IDataRecord)dataReader)["BalanceAmount"].ToDecimal(0);
					int userId = ((IDataRecord)dataReader)["userId"].ToInt(0);
					string orderId = ((IDataRecord)dataReader)["OrderId"].ToNullString();
					this.CloseReturnBalance(userId, orderId, balanceAmount);
				}
			}
		}

		private void PreSaleOrderCloseReturnBalance()
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT UserId, OrderId, BalanceAmount FROM Hishop_Orders WHERE BalanceAmount > 0 AND OrderStatus = 1 AND(((IsConfirm = 0 or IsConfirm is null) AND DepositDate IS NULL AND OrderDate <= @OrderDate and PreSaleId > 0) OR(DepositDate is NULL AND PreSaleId IN(SELECT PreSaleId FROM Hishop_ProductPreSale WHERE PreSaleEndDate < @CurrDate))OR(PayDate is NULL AND PreSaleId IN(SELECT PreSaleId FROM Hishop_ProductPreSale WHERE CONVERT(varchar(100), @CurrDate, 111) > CONVERT(varchar(100), PaymentEndDate, 111))))");
			this.database.AddInParameter(sqlStringCommand, "OrderDate", DbType.DateTime, DateTime.Now.AddDays((double)(-this.setting.CloseOrderDays)));
			this.database.AddInParameter(sqlStringCommand, "CurrDate", DbType.DateTime, DateTime.Now);
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					decimal balanceAmount = ((IDataRecord)dataReader)["BalanceAmount"].ToDecimal(0);
					int userId = ((IDataRecord)dataReader)["userId"].ToInt(0);
					string orderId = ((IDataRecord)dataReader)["OrderId"].ToNullString();
					this.CloseReturnBalance(userId, orderId, balanceAmount);
				}
			}
		}

		private void CloseReturnBalance(int userId, string orderId, decimal balanceAmount)
		{
			if (balanceAmount > decimal.Zero)
			{
				Users.ClearUserCache(userId, "");
				Hidistro.Entities.Members.MemberInfo user = Users.GetUser(userId);
				BalanceDetailInfo balanceDetailInfo = new BalanceDetailInfo();
				balanceDetailInfo.TradeDate = DateTime.Now;
				balanceDetailInfo.TradeType = TradeTypes.RefundOrder;
				balanceDetailInfo.UserId = userId;
				balanceDetailInfo.UserName = user.UserName;
				balanceDetailInfo.Balance = user.Balance + balanceAmount;
				balanceDetailInfo.Expenses = null;
				balanceDetailInfo.Income = balanceAmount;
				balanceDetailInfo.InpourId = "";
				balanceDetailInfo.ManagerUserName = "";
				balanceDetailInfo.Remark = "订单" + orderId + "自动关闭，还原抵扣的余额";
				if (new BaseDao().Add(balanceDetailInfo, null) > 0)
				{
					user.Balance += balanceAmount;
					MemberProcessor.UpdateMember(user);
					Users.ClearUserCache(user.UserId, user.SessionId);
				}
			}
		}

		private void ManagementOrders()
		{
			DateTime now = DateTime.Now;
			try
			{
				this.NormalOrderCloseReturnBalance();
				DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_PointDetails(OrderId,UserId,TradeDate,TradeType,Increased,Reduced,Points,Remark) SELECT OrderId,UserId,GETDATE(),3,DeductionPoints,0,ISNULL((SELECT Points FROM aspnet_Members WHERE UserId=Hishop_Orders.UserId),0) + DeductionPoints,'订单'+OrderId+'自动关闭，还原抵扣的积分' FROM Hishop_Orders WHERE LOWER(Gateway)<>'hishop.plugins.payment.podrequest' and (OrderStatus = 1 AND (IsConfirm = 0 or IsConfirm is null)) AND DeductionPoints > 0 AND DeductionPoints IS NOT NULL AND OrderDate <= @OrderDate AND EXISTS(SELECT UserId FROM aspnet_Members WHERE UserId = Hishop_Orders.UserId) AND (CountDownBuyId is null or CountDownBuyId = 0) AND (PreSaleId IS NULL OR PreSaleId = 0) AND (ParentOrderId='0' OR ParentOrderId='-1');INSERT INTO Hishop_PointDetails(OrderId,UserId,TradeDate,TradeType,Increased,Reduced,Points,Remark) SELECT OrderId,UserId,GETDATE(),3,ExchangePoints,0,ISNULL((SELECT Points FROM aspnet_Members WHERE UserId=Hishop_Orders.UserId),0) + ExchangePoints,'订单'+OrderId+'自动关闭，退回礼品兑换的积分' FROM Hishop_Orders WHERE LOWER(Gateway)<>'hishop.plugins.payment.podrequest' and  (OrderStatus = 1 AND (IsConfirm = 0 or IsConfirm is null)) AND ExchangePoints  >0 AND ExchangePoints IS NOT NULL AND OrderDate <= @OrderDate AND EXISTS(SELECT UserId FROM aspnet_Members WHERE UserId = Hishop_Orders.UserId) AND (ParentOrderId='0' OR ParentOrderId='-1'); UPDATE Hishop_Orders SET OrderStatus=4,CloseReason='过期没付款，自动关闭' WHERE LOWER(Gateway)<>'hishop.plugins.payment.podrequest' and (OrderStatus = 1 and (IsConfirm = 0 or IsConfirm is null)) AND OrderDate <= @OrderDate and (CountDownBuyId is null or CountDownBuyId = 0) AND (PreSaleId IS NULL OR PreSaleId = 0); UPDATE Hishop_Orders SET FinishDate = getdate(), OrderStatus = " + 5 + " WHERE OrderStatus=" + 3 + " AND ShippingDate <= @ShippingDate AND (ItemStatus = 0 or ItemStatus=6); UPDATE Hishop_OrderReplace SET UserConfirmGoodsTime = getdate(),HandleStatus = " + 1 + " WHERE HandleStatus = " + 6 + " AND MerchantsConfirmGoodsTime <= @ShippingDate;UPDATE Hishop_CouponItems SET UsedTime = NULL,OrderId = NULL WHERE OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE CouponCode=Hishop_CouponItems.ClaimCode AND OrderStatus=4 AND CloseReason='过期没付款，自动关闭');DELETE FROM Hishop_Orders WHERE (OrderStatus=4 OR IsServiceOver=1) AND ParentOrderId='-1';");
				string commandText = sqlStringCommand.CommandText;
				IDictionary<string, string> dictionary = new Dictionary<string, string>();
				IDictionary<string, string> dictionary2 = dictionary;
				DateTime dateTime = DateTime.Now;
				dateTime = dateTime.AddDays((double)(-this.setting.CloseOrderDays));
				dictionary2.Add("OrderDate", dateTime.ToString());
				IDictionary<string, string> dictionary3 = dictionary;
				dateTime = DateTime.Now;
				dateTime = dateTime.AddDays((double)(-this.setting.FinishOrderDays));
				dictionary3.Add("ShippingDate", dateTime.ToString());
				dictionary.Add("OrderEndDays", this.setting.EndOrderDays.ToString());
				Database obj = this.database;
				DbCommand command = sqlStringCommand;
				dateTime = DateTime.Now;
				obj.AddInParameter(command, "OrderDate", DbType.DateTime, dateTime.AddDays((double)(-this.setting.CloseOrderDays)));
				Database obj2 = this.database;
				DbCommand command2 = sqlStringCommand;
				dateTime = DateTime.Now;
				obj2.AddInParameter(command2, "ShippingDate", DbType.DateTime, dateTime.AddDays((double)(-this.setting.FinishOrderDays)));
				this.database.AddInParameter(sqlStringCommand, "OrderEndDays", DbType.Int32, this.setting.EndOrderDays);
				this.database.ExecuteNonQuery(sqlStringCommand);
			}
			catch (Exception ex)
			{
				this.WriteLog(ex, now, DateTime.Now);
			}
		}

		private void ProcessOrderServiceOver()
		{
			DateTime now = DateTime.Now;
			try
			{
				this.UpdateMemberPoints();
				StringBuilder stringBuilder = new StringBuilder();
				StringBuilder stringBuilder2 = stringBuilder;
				string format = " UPDATE Hishop_Orders SET IsServiceOver = 1 WHERE ((OrderStatus = " + 5 + " AND DATEDIFF(HH,FinishDate,getdate()) >= {0} AND  ItemStatus = 0 ) OR (OrderStatus = " + 4 + " AND ShippingDate IS NOT NULL)) AND IsServiceOver = 0 AND OrderType <> {1};";
				object arg = this.setting.EndOrderDays * 24;
				OrderType orderType = OrderType.ServiceOrder;
				stringBuilder2.AppendFormat(format, arg, orderType.GetHashCode());
				StringBuilder stringBuilder3 = stringBuilder;
				string format2 = " UPDATE Hishop_Orders SET IsServiceOver = 1 WHERE OrderStatus = " + 5 + " AND ItemStatus = 0 AND IsServiceOver = 0 AND OrderType = {0};";
				orderType = OrderType.ServiceOrder;
				stringBuilder3.AppendFormat(format2, orderType.GetHashCode());
				DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
				this.database.ExecuteNonQuery(sqlStringCommand);
				this.BalanceOrder();
			}
			catch (Exception ex)
			{
				this.WriteLog(ex, now, DateTime.Now);
			}
		}

		private void BalanceOrder()
		{
			this.BalanceSupplierOrder();
			this.BalanceStoreOrder();
		}

		private void BalanceStoreOrder()
		{
			if (this.setting.OpenMultStore)
			{
				try
				{
					Dictionary<int, decimal> storeCommissionRate = this.GetStoreCommissionRate();
					if (storeCommissionRate.Count != 0)
					{
						DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT OrderId,IsStoreCollect,OrderStatus,ShippingDate FROM Hishop_Orders o WHERE o.IsBalanceOver = 0 AND o.IsServiceOver = 1 AND o.StoreId > 0  ORDER BY FinishDate");
						using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
						{
							while (dataReader.Read())
							{
								string text = ((IDataRecord)dataReader)["OrderId"].ToNullString();
								OrderStatus orderStatus = (OrderStatus)((IDataRecord)dataReader)["OrderStatus"].ToInt(0);
								DateTime? nullable = null;
								if (((IDataRecord)dataReader)["ShippingDate"] != DBNull.Value)
								{
									nullable = ((IDataRecord)dataReader)["ShippingDate"].ToDateTime();
								}
								StoreBalanceOrderInfo storeBalanceOrderInfo = this.GetStoreBalanceOrderInfo(text);
								if (storeBalanceOrderInfo != null)
								{
									StringBuilder stringBuilder = new StringBuilder();
									decimal num = storeBalanceOrderInfo.GetShouldOverBalance(storeCommissionRate[storeBalanceOrderInfo.StoreId]);
									decimal platCommission = storeBalanceOrderInfo.GetPlatCommission(storeCommissionRate[storeBalanceOrderInfo.StoreId]);
									if ((storeBalanceOrderInfo.OrderType == OrderType.ServiceOrder && orderStatus == OrderStatus.Closed) || (!nullable.HasValue && orderStatus == OrderStatus.Closed && storeBalanceOrderInfo.OrderType != OrderType.ServiceOrder))
									{
										stringBuilder.Append(" UPDATE Hishop_Orders SET IsBalanceOver= 1 WHERE  OrderId='" + text + "' AND IsBalanceOver = 0 AND IsServiceOver = 1 ;");
									}
									else
									{
										if (orderStatus == OrderStatus.Closed)
										{
											platCommission = default(decimal);
											num = storeBalanceOrderInfo.Freight;
										}
										stringBuilder.Append("INSERT INTO[dbo].[Hishop_StoreBalanceDetails]([StoreId],[TradeDate],[TradeType],[TradeNo],[Income],[Expenses],[Balance],CreateTime,PlatCommission)");
										stringBuilder.AppendFormat("SELECT {0},'{1}',2,'{2}',{3},0,Balance,GETDATE(),{4} FROM Hishop_Stores s WHERE s.StoreId={0};", storeBalanceOrderInfo.StoreId, storeBalanceOrderInfo.OrderDate, storeBalanceOrderInfo.OrderId, num, storeBalanceOrderInfo.GetPlatCommission(storeCommissionRate[storeBalanceOrderInfo.StoreId]));
										stringBuilder.Append(" UPDATE s SET s.Balance = s.Balance + " + num + " FROM dbo.Hishop_Stores s WHERE s.storeId = " + storeBalanceOrderInfo.StoreId + ";");
										stringBuilder.Append(" UPDATE Hishop_Orders SET IsBalanceOver= 1 WHERE  OrderId='" + text + "' AND IsBalanceOver = 0 AND IsServiceOver = 1 ;");
									}
									sqlStringCommand.CommandText = stringBuilder.ToString();
									this.database.ExecuteNonQuery(sqlStringCommand);
								}
							}
						}
					}
				}
				catch (Exception ex)
				{
					this.WriteLog(ex, DateTime.Now, DateTime.Now);
				}
			}
		}

		private StoreBalanceOrderInfo GetStoreBalanceOrderInfo(string orderId)
		{
			string query = "SELECT StoreId,OrderId,OrderDate,OrderTotal,AdjustedFreight AS Freight,ISNULL(DeductionMoney,0) AS DeductionMoney,ISNULL(CouponValue,0) AS CouponValue,ISNULL(RefundAmount,0) AS RefundAmount,IsStoreCollect, OrderDate AS OverBalanceDate,0 AS OverBalance,Tax,OrderType FROM Hishop_Orders WHERE OrderId = @OrderId";
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			using (IDataReader objReader = this.database.ExecuteReader(sqlStringCommand))
			{
				return DataHelper.ReaderToModel<StoreBalanceOrderInfo>(objReader);
			}
		}

		public Dictionary<int, decimal> GetStoreCommissionRate()
		{
			Dictionary<int, decimal> dictionary = new Dictionary<int, decimal>();
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select StoreId, CommissionRate from Hishop_Stores where StoreId in(select StoreId from Hishop_Orders o where o.IsBalanceOver = 0 and o.IsServiceOver = 1 and o.StoreId > 0)");
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					dictionary[((IDataRecord)dataReader)["StoreId"].ToInt(0)] = ((IDataRecord)dataReader)["CommissionRate"].ToDecimal(0);
				}
			}
			return dictionary;
		}

		private void BalanceSupplierOrder()
		{
			DateTime now = DateTime.Now;
			try
			{
				DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select OrderId from Hishop_Orders o where o.IsBalanceOver=0 and o.IsServiceOver= 1 and o.SupplierId>0  order by FinishDate");
				using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
				{
					while (dataReader.Read())
					{
						string str = ((IDataRecord)dataReader)["OrderId"].ToNullString();
						StringBuilder stringBuilder = new StringBuilder();
						stringBuilder.Append("INSERT INTO[dbo].[Hishop_SupplierBalanceDetails]([SupplierId],[UserName],[TradeDate],[TradeType],[OrderId],[Income],[Expenses],[Balance],CreateTime)");
						stringBuilder.Append("select o.SupplierId, m.UserName, o.OrderDate,2, o.OrderId, o.OrderCostPrice+Freight,0,s.Balance+(o.OrderCostPrice+Freight),getdate()");
						stringBuilder.Append("from dbo.Hishop_Orders o join dbo.aspnet_Managers m on o.SupplierId= m.StoreId join dbo.Hishop_Supplier s on o.SupplierId= s.SupplierId ");
						stringBuilder.Append(" where o.OrderId='" + str + "' and m.RoleId= -2;");
						stringBuilder.Append(" update s set s.Balance=s.Balance+ (o.OrderCostPrice + isnull(Freight, 0) ) from dbo.Hishop_Supplier s join Hishop_Orders o  on o.SupplierId = s.SupplierId where o.OrderId = '" + str + "' ");
						stringBuilder.Append(" update dbo.Hishop_Orders set IsBalanceOver= 1 where  OrderId='" + str + "' and IsBalanceOver = 0 and IsServiceOver = 1 and SupplierId>0");
						sqlStringCommand.CommandText = stringBuilder.ToString();
						this.database.ExecuteNonQuery(sqlStringCommand);
					}
				}
			}
			catch (Exception ex)
			{
				this.WriteLog(ex, now, DateTime.Now);
			}
		}

		private void ManagementCountDownOrders()
		{
			DateTime now = DateTime.Now;
			try
			{
				int num = 40;
				int num2 = (this.setting.CountDownTime == 0) ? num : this.setting.CountDownTime;
				DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT O.OrderId,O.CountDownBuyId,OI.Quantity,OI.ProductId,OI.SkuId,O.BalanceAmount,O.UserId FROM Hishop_Orders O INNER JOIN Hishop_OrderItems OI ON O.OrderId=OI.OrderId WHERE O.CountDownBuyId > 0 AND O.OrderStatus = 1 AND dateadd (Minute, @CountDownTime, O.OrderDate) <= getdate ()");
				this.database.AddInParameter(sqlStringCommand, "CountDownTime", DbType.Int32, num2);
				DataSet dataSet = this.database.ExecuteDataSet(sqlStringCommand);
				if (dataSet.Tables.Count != 0)
				{
					DataTable dataTable = dataSet.Tables[0];
					foreach (DataRow row in dataTable.Rows)
					{
						string value = row["OrderId"].ToString();
						string value2 = row["SkuId"].ToString();
						int num3 = row["CountDownBuyId"].ToInt(0);
						int num4 = row["ProductId"].ToInt(0);
						int num5 = row["Quantity"].ToInt(0);
						sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Orders SET OrderStatus=4,CloseReason='限时购订单过期没付款，自动关闭' WHERE OrderId = @OrderId");
						this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, value);
						this.database.ExecuteNonQuery(sqlStringCommand);
						this.CloseReturnBalance(row["UserId"].ToInt(0), row["OrderId"].ToNullString(), row["BalanceAmount"].ToDecimal(0));
						sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_CountDownSku SET BoughtCount= CASE WHEN BoughtCount - @BoughtCount < 0 THEN 0 ELSE BoughtCount - @BoughtCount END WHERE CountDownId =@CountDownId AND SkuId=@SkuId ");
						this.database.AddInParameter(sqlStringCommand, "CountDownId", DbType.Int32, num3);
						this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, value2);
						this.database.AddInParameter(sqlStringCommand, "BoughtCount", DbType.Int32, num5);
						this.database.ExecuteNonQuery(sqlStringCommand);
					}
				}
			}
			catch (Exception ex)
			{
				this.WriteLog(ex, now, DateTime.Now);
			}
		}

		private void ProcessorSplittin()
		{
			DateTime now = DateTime.Now;
			try
			{
				DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_SplittinDetails WHERE IsUse = 'false' AND OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE OrderStatus = " + 5 + " AND IsServiceOver = 1)");
				this.database.AddInParameter(sqlStringCommand, "FinishDate", DbType.DateTime, DateTime.Now.AddDays((double)(-this.setting.EndOrderDays)));
				IList<SplittinDetailInfo> list = null;
				using (IDataReader objReader = this.database.ExecuteReader(sqlStringCommand))
				{
					list = DataHelper.ReaderToList<SplittinDetailInfo>(objReader);
				}
				IList<int> list2 = new List<int>();
				if (list != null)
				{
					foreach (SplittinDetailInfo item in list)
					{
						item.IsUse = true;
						item.Balance = item.Income.Value + this.GetUserUseSplittin(item.UserId);
						if (new BaseDao().Add(item, null) > 0)
						{
							this.RemoveNoUseSplittin(item.OrderId);
						}
						if (!list2.Contains(item.UserId))
						{
							list2.Add(item.UserId);
						}
					}
					MemberProcessor.UpdateUserReferralGrade(list2);
				}
			}
			catch (Exception ex)
			{
				this.WriteLog(ex, now, DateTime.Now);
			}
		}

		private decimal GetUserUseSplittin(int userId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT TOP 1 Balance FROM Hishop_SplittinDetails WHERE IsUse = 'true' AND UserId =  @UserId ORDER BY JournalNumber DESC");
			this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			if (obj != null && obj != DBNull.Value)
			{
				return (decimal)obj;
			}
			return decimal.Zero;
		}

		public bool RemoveNoUseSplittin(string orderId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM  Hishop_SplittinDetails WHERE IsUse = 'false' AND OrderId = @OrderId");
			this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		private void ManagePreSaleOrders()
		{
			DateTime now = DateTime.Now;
			try
			{
				DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_PointDetails(OrderId,UserId,TradeDate,TradeType,Increased,Reduced,Points,Remark) SELECT OrderId,UserId,GETDATE(),3,DeductionPoints,0,ISNULL((SELECT Points FROM aspnet_Members WHERE UserId=o.UserId),0) + DeductionPoints,'预售订单'+OrderId+'过期未付尾款,自动关闭，还原抵扣的积分' FROM Hishop_Orders o WHERE  (OrderStatus = 1 AND o.PreSaleId > 0 AND DeductionPoints > 0 AND DeductionPoints IS NOT NULL AND DepositDate IS NOT NULL AND GETDATE() >= ISNULL((SELECT PaymentEndDate FROM Hishop_ProductPreSale WHERE PreSaleId = o.PreSaleId),GetDATE()));");
				this.database.ExecuteNonQuery(sqlStringCommand);
				DbCommand sqlStringCommand2 = this.database.GetSqlStringCommand("INSERT INTO Hishop_PointDetails(OrderId,UserId,TradeDate,TradeType,Increased,Reduced,Points,Remark) SELECT OrderId,UserId,GETDATE(),3,DeductionPoints,0,ISNULL((SELECT Points FROM aspnet_Members WHERE UserId=o.UserId),0) + DeductionPoints,'预售订单'+OrderId+'过期未付尾款,自动关闭，还原抵扣的积分' FROM Hishop_Orders o WHERE  (OrderStatus = 1 AND o.PreSaleId > 0 AND DeductionPoints > 0 AND DeductionPoints IS NOT NULL AND DepositDate IS NULL AND GETDATE() >= ISNULL((SELECT PreSaleEndDate FROM Hishop_ProductPreSale WHERE PreSaleId = o.PreSaleId),GetDATE()));");
				this.database.ExecuteNonQuery(sqlStringCommand2);
				DbCommand sqlStringCommand3 = this.database.GetSqlStringCommand("\r\n                UPDATE Hishop_Products SET SaleStatus=2  WHERE SaleStatus=1 AND  ProductId IN ( SELECT ProductId FROM Hishop_ProductPreSale WHERE  ExecutMark = 0 AND PreSaleEndDate < @CurrDate);\r\n                UPDATE Hishop_ProductPreSale SET ExecutMark = 1 WHERE ExecutMark = 0 AND PreSaleEndDate < @CurrDate;\r\n                UPDATE Hishop_Orders SET OrderStatus=4,CloseReason='过期没付款，自动关闭' WHERE (OrderStatus=1 and (IsConfirm=0 or IsConfirm is null)) AND DepositDate IS NULL AND OrderDate <= @OrderDate and PreSaleId > 0;\r\n                UPDATE Hishop_Orders SET OrderStatus=4,CloseReason='预售结束未付款，自动关闭' WHERE OrderStatus=1 AND DepositDate is NULL AND PreSaleId IN (SELECT PreSaleId FROM Hishop_ProductPreSale WHERE PreSaleEndDate < @CurrDate);\r\n                UPDATE Hishop_Orders SET OrderStatus=4,CloseReason='预售尾款支付时间已结束未付尾款，自动关闭' WHERE OrderStatus=1 AND DepositDate IS NOT NULL AND PayDate IS NULL AND PreSaleId IN (SELECT PreSaleId FROM Hishop_ProductPreSale WHERE CONVERT(varchar(100), @CurrDate, 111) > CONVERT(varchar(100), PaymentEndDate, 111));");
				this.database.AddInParameter(sqlStringCommand3, "OrderDate", DbType.DateTime, DateTime.Now.AddDays((double)(-this.setting.CloseOrderDays)));
				this.database.AddInParameter(sqlStringCommand3, "CurrDate", DbType.DateTime, DateTime.Now);
				this.database.ExecuteNonQuery(sqlStringCommand3);
			}
			catch (Exception ex)
			{
				this.WriteLog(ex, now, DateTime.Now);
			}
		}

		private void ProcessorEvaluate(SiteSettings siteSettings)
		{
			DateTime now = DateTime.Now;
			try
			{
				DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_ProductReviews (ProductId,[UserId],[OrderId]   ,[UserEmail],UserName ,[SkuId],[Score] ,[SKUContent],[ReviewText],[ReviewDate]) SELECT  OI.ProductId,O.UserId,O.OrderId,ISNULL(O.EmailAddress,''),O.Username,OI.SkuId,5,OI.SKUContent,'好评！', Getdate() FROM Hishop_Orders O INNER JOIN  Hishop_OrderItems OI ON O.OrderId=OI.OrderId WHERE   O.OrderStatus = 5 AND DATEADD(day, @EvaluateTime, O.FinishDate) <= GetDate() AND o.OrderId NOT IN (SELECT OrderId FROM Hishop_ProductReviews) AND ISNULL((SELECT ProductId FROM Hishop_Products WHERE ProductId = OI.ProductId),0) > 0");
				this.database.AddInParameter(sqlStringCommand, "EvaluateTime", DbType.Int32, siteSettings.EndOrderDaysEvaluate);
				this.database.ExecuteNonQuery(sqlStringCommand);
			}
			catch (Exception ex)
			{
				this.WriteLog(ex, now, DateTime.Now);
			}
		}
	}
}
