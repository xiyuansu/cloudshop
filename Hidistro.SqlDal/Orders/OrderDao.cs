using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.SqlDal.Depot;
using Hidistro.SqlDal.Promotions;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Hidistro.SqlDal.Orders
{
	public class OrderDao : BaseDao
	{
		public bool ExistsOrder(string orderId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("Select Count(1) FROM Hishop_Orders WHERE OrderId = @OrderId");
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			object obj = base.database.ExecuteScalar(sqlStringCommand);
			return (int)obj > 0;
		}

		public int GetJoinGroupCount(int fightGroupId, string orderStatuses)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT Count(1) FROM Hishop_Orders WHERE FightGroupId = @FightGroupId AND OrderStatus NOT IN (" + orderStatuses + ")");
			base.database.AddInParameter(sqlStringCommand, "FightGroupId", DbType.Int32, fightGroupId);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0);
		}

		public string GetOrderIdByUserAwardRecordsId(int UserAwardRecordsId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT top 1 OrderId FROM Hishop_Orders WHERE UserAwardRecordsId = @UserAwardRecordsId AND OrderStatus = @OrderStatus");
			base.database.AddInParameter(sqlStringCommand, "UserAwardRecordsId", DbType.Int32, UserAwardRecordsId);
			base.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, OrderStatus.WaitBuyerPay);
			return base.database.ExecuteScalar(sqlStringCommand).ToNullString();
		}

		public List<OrderInfo> GetFightGroupOrders(int fightGroupId, bool justShowPay = false)
		{
			string text = "SELECT * FROM Hishop_Orders WHERE FightGroupId = @FightGroupId";
			if (justShowPay)
			{
				text = text + " AND OrderStatus<>" + 1 + " AND OrderStatus<>" + 4;
			}
			text += " order by PayDate";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(text);
			base.database.AddInParameter(sqlStringCommand, "FightGroupId", DbType.Int32, fightGroupId);
			List<OrderInfo> list = new List<OrderInfo>();
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					OrderInfo orderInfo = DataMapper.PopulateOrder(dataReader);
					string query = " SELECT * FROM Hishop_OrderItems WHERE OrderId = '" + orderInfo.OrderId + "'";
					DbCommand sqlStringCommand2 = base.database.GetSqlStringCommand(query);
					IDataReader dataReader2 = base.database.ExecuteReader(sqlStringCommand2);
					while (dataReader2.Read())
					{
						LineItemInfo value = DataMapper.PopulateLineItem(dataReader2);
						orderInfo.LineItems.Add((string)((IDataRecord)dataReader2)["SkuId"], value);
					}
					list.Add(orderInfo);
				}
				dataReader.Close();
			}
			return list;
		}

		public void UpdateOrderIsStoreCollect(string orderId, bool isStoreCollect)
		{
			orderId = base.GetTrueOrderId(orderId);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE Hishop_Orders SET IsStoreCollect = @IsStoreCollect WHERE OrderId = @OrderId");
			base.database.AddInParameter(sqlStringCommand, "IsStoreCollect", DbType.Boolean, isStoreCollect);
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			base.database.ExecuteNonQuery(sqlStringCommand);
		}

		public bool CheckSameStoreForHiPOS(string hiPOSDeviceId, int storeId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT Count (1) FROM Hishop_StoreHiPOS WHERE StoreId = @StoreId AND HiPOSDeviceId = @HiPOSDeviceId");
			base.database.AddInParameter(sqlStringCommand, "HiPOSDeviceId", DbType.String, hiPOSDeviceId);
			base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, storeId);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0) > 0;
		}

		public void UpdateOrderPaymentType(string orderId, string paymentType)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE Hishop_Orders SET PaymentType = @PaymentType WHERE OrderId = @OrderId");
			base.database.AddInParameter(sqlStringCommand, "PaymentType", DbType.String, paymentType);
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			base.database.ExecuteNonQuery(sqlStringCommand);
		}

		public bool UpdateOrderItemStatus(string OrderId, OrderItemStatus status, bool isAllReturned)
		{
			OrderId = base.GetTrueOrderId(OrderId);
			string text = "UPDATE Hishop_Orders SET ItemStatus = @ItemStatus WHERE OrderId = @OrderId;";
			if (isAllReturned)
			{
				text = text + "UPDATE Hishop_Orders SET OrderStatus = " + 4 + ",CloseReason = '订单全部退货完成' WHERE OrderId = @OrderId;";
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(text);
			base.database.AddInParameter(sqlStringCommand, "ItemStatus", DbType.Int32, (int)status);
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, OrderId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateOrderStatus(OrderInfo order, OrderStatus status)
		{
			string text = "";
			DateTime now;
			if (status == OrderStatus.ApplyForRefund)
			{
				object arg = status.GetHashCode();
				string orderId = order.OrderId;
				now = DateTime.Now;
				text = string.Format("UPDATE Hishop_Orders SET OrderStatus = {0},UpdateDate ='{2}' WHERE OrderId = '{1}'", arg, orderId, now.ToString("yyyy-MM-dd HH:mm:ss"));
			}
			if (status == OrderStatus.Closed)
			{
				object[] obj = new object[4]
				{
					status.GetHashCode(),
					order.OrderId,
					DataHelper.CleanSearchString(order.CloseReason),
					null
				};
				now = DateTime.Now;
				obj[3] = now.ToString("yyyy-MM-dd HH:mm:ss");
				text = string.Format("UPDATE Hishop_Orders SET OrderStatus = {0},CloseReason = '{2}',UpdateDate ='{3}' WHERE OrderId = '{1}'", obj);
			}
			if (status == OrderStatus.Finished)
			{
				object[] obj2 = new object[4]
				{
					status.GetHashCode(),
					order.OrderId,
					null,
					null
				};
				now = DateTime.Now;
				obj2[2] = now.ToString("yyyy-MM-dd HH:mm:ss");
				now = DateTime.Now;
				obj2[3] = now.ToString("yyyy-MM-dd HH:mm:ss");
				text = string.Format("UPDATE Hishop_Orders SET OrderStatus = {0},FinishDate = '{2}',UpdateDate ='{3}' WHERE OrderId = '{1}'", obj2);
			}
			if (status == OrderStatus.BuyerAlreadyPaid)
			{
				if (order.PayDate == DateTime.MinValue)
				{
					object[] obj3 = new object[4]
					{
						status.GetHashCode(),
						order.OrderId,
						null,
						null
					};
					now = DateTime.Now;
					obj3[2] = now.ToString("yyyy-MM-dd HH:mm:ss");
					now = DateTime.Now;
					obj3[3] = now.ToString("yyyy-MM-dd HH:mm:ss");
					text = string.Format("UPDATE Hishop_Orders SET OrderStatus = {0},PayDate = '{2}',UpdateDate ='{3}' WHERE OrderId = '{1}'", obj3);
				}
				else
				{
					object arg2 = status.GetHashCode();
					string orderId2 = order.OrderId;
					now = DateTime.Now;
					text = string.Format("UPDATE Hishop_Orders SET OrderStatus = {0},UpdateDate ='{2}' WHERE OrderId = '{1}'", arg2, orderId2, now.ToString("yyyy-MM-dd HH:mm:ss"));
				}
			}
			if (status == OrderStatus.SellerAlreadySent)
			{
				object[] obj4 = new object[7]
				{
					status.GetHashCode(),
					order.OrderId,
					null,
					null,
					null,
					null,
					null
				};
				now = DateTime.Now;
				obj4[2] = now.ToString("yyyy-MM-dd HH:mm:ss");
				now = DateTime.Now;
				obj4[3] = now.ToString("yyyy-MM-dd HH:mm:ss");
				obj4[4] = order.ShipOrderNumber;
				obj4[5] = order.ExpressCompanyAbb;
				obj4[6] = order.ExpressCompanyName;
				text = string.Format("UPDATE Hishop_Orders SET OrderStatus = {0},ShippingDate = '{2}',UpdateDate ='{3}',ShipOrderNumber = {4},ExpressCompanyName = '{5}',ExpressCompanyAbb = {6} WHERE OrderId = '{1}'", obj4);
			}
			if (status == OrderStatus.Refunded)
			{
				object[] obj5 = new object[7]
				{
					status.GetHashCode(),
					order.OrderId,
					null,
					null,
					null,
					null,
					null
				};
				now = DateTime.Now;
				obj5[2] = now.ToString("yyyy-MM-dd HH:mm:ss");
				now = DateTime.Now;
				obj5[3] = now.ToString("yyyy-MM-dd HH:mm:ss");
				obj5[4] = order.ShipOrderNumber;
				obj5[5] = order.ExpressCompanyAbb;
				obj5[6] = order.ExpressCompanyName;
				text = string.Format("UPDATE Hishop_Orders SET OrderStatus = {0},UpdateDate ='{2}' WHERE OrderId = '{1}'", obj5);
			}
			if (status == OrderStatus.RefundRefused)
			{
				object[] obj6 = new object[7]
				{
					status.GetHashCode(),
					order.OrderId,
					null,
					null,
					null,
					null,
					null
				};
				now = DateTime.Now;
				obj6[2] = now.ToString("yyyy-MM-dd HH:mm:ss");
				now = DateTime.Now;
				obj6[3] = now.ToString("yyyy-MM-dd HH:mm:ss");
				obj6[4] = order.ShipOrderNumber;
				obj6[5] = order.ExpressCompanyAbb;
				obj6[6] = order.ExpressCompanyName;
				text = string.Format("UPDATE Hishop_Orders SET OrderStatus = {0},UpdateDate ='{2}' WHERE OrderId = '{1}'", obj6);
			}
			if (!string.IsNullOrEmpty(text))
			{
				DbCommand sqlStringCommand = base.database.GetSqlStringCommand(text);
				return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
			}
			return false;
		}

		public DataSet GetUserOrder(int userId, OrderQuery query)
		{
			string text = string.Empty;
			if (query.TakeOnStore.HasValue && query.TakeOnStore.Value)
			{
				text += $" AND (( ShippingModeId=-2 AND (OrderStatus={2} or (OrderStatus={1} AND PaymentTypeId=-3)) AND ItemStatus=0 ) or (OrderType =6 and OrderStatus=2))";
			}
			else if (query.Status == OrderStatus.WaitBuyerPay)
			{
				text = text + " AND (ParentOrderId='-1' OR ParentOrderId='0') AND OrderStatus = " + 1 + " AND Gateway <> 'hishop.plugins.payment.podrequest'";
			}
			if (query.Status == OrderStatus.SellerAlreadySent)
			{
				text = text + " AND (OrderStatus = " + 3 + " and ShippingModeId!=-2) AND ItemStatus = " + 0;
			}
			else if (query.Status == OrderStatus.BuyerAlreadyPaid)
			{
				text = text + " AND (OrderStatus = " + 2 + " OR  (Gateway = 'hishop.plugins.payment.podrequest' AND OrderStatus = " + 1 + ")) AND ParentOrderId<>'-1' AND ShippingModeId!=-2 AND ItemStatus = " + 0;
			}
			else if (query.Status == OrderStatus.Finished)
			{
				text = text + " AND (OrderStatus = " + 5 + " OR  (Gateway = 'hishop.plugins.payment.podrequest' AND OrderStatus = " + 5 + ")) AND ShippingModeId!=-2 AND ItemStatus = " + 0;
			}
			else if (query.Status == OrderStatus.WaitReview)
			{
				text += " AND OrderId IN (SELECT DISTINCT OI.OrderId FROM Hishop_OrderItems OI LEFT JOIN Hishop_ProductReviews PR ON OI.OrderId = PR.OrderId AND OI.SkuId = PR.SkuId  WHERE PR.ReviewId IS NULL) AND (OrderStatus = " + 5 + " OR  (OrderStatus = " + 4 + " AND CloseReason = '订单全部退货完成')) AND UserId = " + userId;
			}
			else if (query.ItemStatus.HasValue && query.ItemStatus.Value > 0)
			{
				text += $" AND ((ItemStatus = {query.ItemStatus.Value}) OR OrderStatus = {6}  OR ( StoreId > 0 AND IsConfirm = 1) )  ";
			}
			string str = "SELECT OrderId,(OrderId+ISNULL(PayRandCode,'')) AS PayOrderId, OrderDate, OrderStatus,PaymentTypeId,ShippingModeId, OrderTotal,IsConfirm,IsError, (SELECT SUM(Quantity) FROM Hishop_OrderItems WHERE OrderId = o.OrderId) as ProductSum,GateWay,ISNULL(ItemStatus,0) AS ItemStatus,TakeCode,FinishDate FROM Hishop_Orders as o WHERE UserId  = @UserId AND ((ParentOrderId<>'-1' AND OrderStatus<>" + 1 + ") OR (OrderStatus=" + 1 + " AND (ParentOrderId='-1' OR ParentOrderId='0'))) ";
			str += text;
			if (!query.ShowGiftOrder)
			{
				str += " And (select count(OrderID) from Hishop_OrderItems where OrderID=o.OrderId OR OrderID=o.ParentOrderId)>0";
			}
			str += " ORDER BY OrderDate DESC";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(str);
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			return base.database.ExecuteDataSet(sqlStringCommand);
		}

		public List<OrderInfo> GetListUserOrder(int userId, OrderQuery query)
		{
			string text = string.Empty;
			if (query.TakeOnStore.HasValue && query.TakeOnStore.Value)
			{
				text += $" AND ShippingModeId=-2 AND (OrderStatus={2} or (OrderStatus={1} AND PaymentTypeId=-3)) AND ItemStatus=0";
			}
			else if (query.Status == OrderStatus.WaitBuyerPay)
			{
				text = text + "   AND OrderStatus = " + 1;
			}
			if (query.Status == OrderStatus.SellerAlreadySent)
			{
				text = text + " AND (OrderStatus = " + 3 + " and ShippingModeId!=-2) AND ItemStatus = " + 0;
			}
			else if (query.Status == OrderStatus.BuyerAlreadyPaid)
			{
				text = text + " AND (OrderStatus = " + 2 + " OR  (Gateway = 'hishop.plugins.payment.podrequest' AND OrderStatus = " + 1 + ")) AND ParentOrderId<>'-1' AND ShippingModeId!=-2 AND ItemStatus = " + 0;
			}
			else if (query.Status == OrderStatus.Finished)
			{
				text = text + " AND (OrderStatus = " + 5 + " OR  (Gateway = 'hishop.plugins.payment.podrequest' AND OrderStatus = " + 5 + ")) AND ShippingModeId!=-2 AND ItemStatus = " + 0;
			}
			else if (query.Status == OrderStatus.WaitReview)
			{
				text += " AND OrderId IN (SELECT DISTINCT OI.OrderId FROM Hishop_OrderItems OI LEFT JOIN Hishop_ProductReviews PR ON OI.OrderId = PR.OrderId AND OI.SkuId = PR.SkuId  WHERE PR.ReviewId IS NULL) AND (OrderStatus = " + 5 + " OR  (OrderStatus = " + 4 + " AND CloseReason = '订单全部退货完成')) AND UserId = " + userId;
			}
			else if (query.ItemStatus > 0)
			{
				text += $" AND ((ItemStatus = {query.ItemStatus}) OR OrderStatus = {6}  OR ( StoreId > 0 AND IsConfirm = 1) )  ";
			}
			if (!string.IsNullOrEmpty(query.ParentOrderId))
			{
				text += $" AND ParentOrderId = '{DataHelper.CleanSearchString(query.ParentOrderId)}'";
			}
			if (query.IsServiceOrder.HasValue && !query.IsServiceOrder.Value)
			{
				text += $" And OrderType <>'{6.GetHashCode()}'";
			}
			string empty = string.Empty;
			empty = "SELECT s.StoreName,*,ChildOrderIds=stuff(( select ',' + OrderId from Hishop_Orders where ParentOrderId=o.OrderId for xml path( '')),1 ,1, '') FROM Hishop_Orders as o  LEFT JOIN Hishop_Stores as s ON o.StoreId=s.StoreId WHERE UserId  = @UserId  AND ((ParentOrderId<>'-1' AND OrderStatus<>" + 1 + ") OR (OrderStatus=" + 1 + " AND (ParentOrderId='-1' OR ParentOrderId='0'))) ";
			empty += text;
			if (!query.ShowGiftOrder)
			{
				empty += " And (select count(OrderID) from Hishop_OrderItems where OrderID=o.OrderId OR OrderID=o.ParentOrderId)>0";
			}
			empty += " ORDER BY OrderDate DESC";
			empty = empty + " SELECT * FROM Hishop_OrderItems  WHERE OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE UserId = @UserId AND ParentOrderId<>'-1' " + text + ")";
			if (query.ShowGiftOrder)
			{
				empty = empty + "select * from Hishop_OrderGifts where OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE UserId = @UserId AND ParentOrderId<>'-1' " + text + ")";
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(empty);
			PromotionDao promotionDao = new PromotionDao();
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			List<OrderInfo> list = new List<OrderInfo>();
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					OrderInfo orderInfo = DataMapper.PopulateOrder(dataReader);
					if (DBNull.Value != ((IDataRecord)dataReader)["ChildOrderIds"])
					{
						orderInfo.ChildOrderIds = ((IDataRecord)dataReader)["ChildOrderIds"].ToNullString();
					}
					if (DBNull.Value != ((IDataRecord)dataReader)["StoreName"])
					{
						orderInfo.StoreName = (string)((IDataRecord)dataReader)["StoreName"];
					}
					list.Add(orderInfo);
				}
				dataReader.NextResult();
				while (dataReader.Read())
				{
					LineItemInfo item = DataMapper.PopulateLineItem(dataReader);
					OrderInfo orderInfo2 = list.FirstOrDefault((OrderInfo a) => a.OrderId == item.OrderId || ("," + a.ChildOrderIds + ",").Contains("," + item.OrderId + ","));
					if (orderInfo2 != null)
					{
						if (item.PromotionId > 0)
						{
							PromotionInfo promotion = promotionDao.GetPromotion(item.PromotionId);
							if (promotion != null)
							{
								item.PromoteType = promotion.PromoteType;
							}
						}
						orderInfo2.LineItems.Add((string)((IDataRecord)dataReader)["SkuId"], item);
						if (item.Status == LineItemStatus.Normal)
						{
							item.ReplaceInfo = null;
							item.ReturnInfo = null;
						}
						else if (item.Status == LineItemStatus.ReplaceApplied || item.Status == LineItemStatus.Replaced || item.Status == LineItemStatus.ReplaceRefused || item.Status == LineItemStatus.UserDeliveryForReplace || item.Status == LineItemStatus.MerchantsDeliveryForRepalce || item.Status == LineItemStatus.MerchantsAgreedForReplace)
						{
							ReplaceInfo replaceInfo = new ReplaceDao().GetReplaceInfo(orderInfo2.OrderId, item.SkuId);
							item.ReplaceInfo = replaceInfo;
						}
						else
						{
							ReturnInfo returnInfo = new ReturnDao().GetReturnInfo(orderInfo2.OrderId, item.SkuId);
							item.ReturnInfo = returnInfo;
						}
					}
				}
				dataReader.NextResult();
				while (dataReader.Read())
				{
					OrderGiftInfo gitem = DataMapper.PopulateOrderGift(dataReader);
					OrderInfo orderInfo3 = list.FirstOrDefault((OrderInfo a) => a.OrderId == gitem.OrderId || ("," + a.ChildOrderIds + ",").Contains("," + gitem.OrderId + ","));
					orderInfo3?.Gifts.Add(gitem);
				}
			}
			return list;
		}

		public List<OrderInfo> GetBuilderQueryListUserOrderId(int userId, OrderQuery query)
		{
			string text = string.Empty;
			if (query.TakeOnStore.HasValue && query.TakeOnStore.Value)
			{
				text += $" AND ((ShippingModeId=-2 AND (OrderStatus={2} or (OrderStatus={1} AND PaymentTypeId=-3)) AND ItemStatus=0) or (OrderType = 6 and OrderStatus = 2))";
			}
			else if (query.Status == OrderStatus.WaitBuyerPay)
			{
				text = text + " AND (ParentOrderId='-1' OR ParentOrderId='0')  AND OrderStatus = " + 1 + " AND Gateway<>'hishop.plugins.payment.podrequest'";
			}
			if (query.Status == OrderStatus.SellerAlreadySent)
			{
				text = text + " AND (OrderStatus = " + 3 + " and ShippingModeId!=-2) AND ItemStatus = " + 0;
			}
			else if (query.Status == OrderStatus.BuyerAlreadyPaid)
			{
				text = text + " AND (OrderStatus = " + 2 + " OR  (Gateway = 'hishop.plugins.payment.podrequest' AND OrderStatus = " + 1 + ")) AND ParentOrderId<>'-1' AND ShippingModeId!=-2 AND ItemStatus = " + 0;
			}
			else if (query.Status == OrderStatus.Finished)
			{
				text = text + " AND (OrderStatus = " + 5 + " OR  (Gateway = 'hishop.plugins.payment.podrequest' AND OrderStatus = " + 5 + ")) AND ShippingModeId!=-2 AND ItemStatus = " + 0;
			}
			else if (query.Status == OrderStatus.WaitReview)
			{
				text += " AND OrderId IN (SELECT DISTINCT OI.OrderId FROM Hishop_OrderItems OI LEFT JOIN Hishop_ProductReviews PR ON OI.OrderId = PR.OrderId AND OI.SkuId = PR.SkuId  WHERE PR.ReviewId IS NULL) AND (OrderStatus = " + 5 + " OR  (OrderStatus = " + 4 + " AND CloseReason = '订单全部退货完成')) AND UserId = " + userId;
			}
			else if (query.ItemStatus > 0)
			{
				text += $" AND ((ItemStatus = {query.ItemStatus}) OR OrderStatus = {6}  OR ( StoreId > 0 AND IsConfirm = 1) )  ";
			}
			if (!string.IsNullOrEmpty(query.ParentOrderId))
			{
				text += $" AND ParentOrderId = '{DataHelper.CleanSearchString(query.ParentOrderId)}'";
			}
			if (!query.ShowGiftOrder)
			{
				text += " AND (SELECT COUNT(OrderID) FROM Hishop_OrderItems WHERE OrderID = o.OrderId OR OrderId IN(SELECT OrderId FROM Hishop_Orders WHERE ParentOrderId = o.OrderID))>0";
			}
			OrderType? type = query.Type;
			if (type.HasValue)
			{
				string str = text;
				type = query.Type;
				text = str + $" AND OrderType = {type.GetHashCode()} ";
			}
			if (query.IsIncludePreSaleOrder.HasValue && query.IsIncludePreSaleOrder == false)
			{
				text += " AND PreSaleId = 0 ";
			}
			if (query.IsServiceOrder.HasValue && !query.IsServiceOrder.Value)
			{
				text += $" And OrderType <>'{6.GetHashCode()}'";
			}
			if (query.SupplierId.HasValue)
			{
				text = ((query.SupplierId != -1) ? (text + " AND SupplierId=" + query.SupplierId) : (text + " AND SupplierId>0"));
			}
			if (query.StoreId.HasValue)
			{
				text = ((!(query.StoreId < 0)) ? (text + " AND StoreId = " + query.StoreId) : (text + " AND StoreId > 0"));
			}
			List<OrderInfo> list = new List<OrderInfo>();
			string query2 = "select top " + query.PageSize + " *,ChildOrderIds=stuff(( select ',' + OrderId from Hishop_Orders where ParentOrderId=o.OrderId for xml path( '')),1 ,1, '')  FROM Hishop_Orders o WHERE UserId  = @UserId  AND ((ParentOrderId<>'-1' AND OrderStatus<>" + 1 + ") OR (OrderStatus=" + 1 + " AND (ParentOrderId='-1' OR ParentOrderId='0'))) AND OrderId NOT IN (SELECT TOP (" + query.PageSize + "*" + (query.PageIndex - 1) + ") OrderId FROM Hishop_Orders WHERE UserId  = @UserId  AND ((ParentOrderId<>'-1' AND OrderStatus<>" + 1 + ") OR (OrderStatus=" + 1 + " AND (ParentOrderId='-1' OR ParentOrderId='0'))) " + text + " ORDER BY OrderDate DESC) " + text + " ORDER BY OrderDate DESC";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query2);
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			PromotionDao promotionDao = new PromotionDao();
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					OrderInfo orderInfo = DataMapper.PopulateOrder(dataReader);
					orderInfo.ChildOrderIds = ((IDataRecord)dataReader)["ChildOrderIds"].ToNullString();
					list.Add(orderInfo);
				}
			}
			return list;
		}

		public List<OrderInfo> GetPageListUserOrder(int userId, OrderQuery query)
		{
			string empty = string.Empty;
			List<OrderInfo> list = new List<OrderInfo>();
			list = this.GetBuilderQueryListUserOrderId(userId, query);
			if (list == null || list.Count <= 0)
			{
				return list;
			}
			string text = string.Join("','", (from s in list
			select s.OrderId).ToArray());
			empty = "SELECT * FROM Hishop_OrderItems  WHERE OrderId IN ('" + text + "') OR OrderId IN(SELECT OrderId FROM Hishop_Orders WHERE ParentOrderId IN('" + text + "'));";
			empty = empty + "SELECT StoreId,StoreName FROM Hishop_Stores WHERE  StoreId IN (SELECT StoreId FROM Hishop_Orders WHERE OrderId IN ('" + text + "'));";
			if (query.ShowGiftOrder)
			{
				empty = empty + "SELECT * from Hishop_OrderGifts where OrderId IN ('" + text + "')";
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(empty);
			PromotionDao promotionDao = new PromotionDao();
			IDataReader reader = base.database.ExecuteReader(sqlStringCommand);
			try
			{
				while (reader.Read())
				{
					LineItemInfo item = DataMapper.PopulateLineItem(reader);
					OrderInfo orderInfo = list.FirstOrDefault((OrderInfo a) => a.OrderId == item.OrderId || ("," + a.ChildOrderIds + ",").Contains("," + item.OrderId + ","));
					if (orderInfo != null)
					{
						if (item.PromotionId > 0)
						{
							PromotionInfo promotion = promotionDao.GetPromotion(item.PromotionId);
							if (promotion != null)
							{
								item.PromoteType = promotion.PromoteType;
							}
						}
						orderInfo.LineItems.Add((string)((IDataRecord)reader)["SkuId"], item);
						if (item.Status == LineItemStatus.Normal)
						{
							item.ReplaceInfo = null;
							item.ReturnInfo = null;
						}
						else if (item.Status == LineItemStatus.ReplaceApplied || item.Status == LineItemStatus.Replaced || item.Status == LineItemStatus.ReplaceRefused || item.Status == LineItemStatus.UserDeliveryForReplace || item.Status == LineItemStatus.MerchantsDeliveryForRepalce || item.Status == LineItemStatus.MerchantsAgreedForReplace)
						{
							ReplaceInfo replaceInfo = new ReplaceDao().GetReplaceInfo(orderInfo.OrderId, item.SkuId);
							item.ReplaceInfo = replaceInfo;
						}
						else
						{
							ReturnInfo returnInfo = new ReturnDao().GetReturnInfo(orderInfo.OrderId, item.SkuId);
							item.ReturnInfo = returnInfo;
						}
					}
				}
				reader.NextResult();
				while (reader.Read())
				{
					if (((IDataRecord)reader)["StoreName"] != DBNull.Value)
					{
						List<OrderInfo> list2 = (from s in list
						where s.StoreId == Convert.ToInt32(((IDataRecord)reader)["StoreId"].ToString())
						select s).ToList();
						foreach (OrderInfo item2 in list2)
						{
							item2.StoreName = (string)((IDataRecord)reader)["StoreName"];
						}
					}
				}
				reader.NextResult();
				while (reader.Read())
				{
					OrderGiftInfo gitem = DataMapper.PopulateOrderGift(reader);
					OrderInfo orderInfo2 = list.FirstOrDefault((OrderInfo a) => a.OrderId == gitem.OrderId || ("," + a.ChildOrderIds + ",").Contains("," + gitem.OrderId + ","));
					orderInfo2?.Gifts.Add(gitem);
				}
			}
			finally
			{
				if (reader != null)
				{
					reader.Dispose();
				}
			}
			return list;
		}

		public PageModel<AfterSaleRecordModel> GetUserAfterOrders(int userId, AfterSalesQuery query)
		{
			PageModel<AfterSaleRecordModel> pageModel = new PageModel<AfterSaleRecordModel>();
			string text = " OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE   UserId = " + userId + " AND ParentOrderId<>'-1') AND AfterSaleType <> 2";
			if (query.AfterSaleType.HasValue)
			{
				text = text + " AND AfterSaleType = " + query.AfterSaleType.Value;
			}
			if (query.ProductType != ProductType.All)
			{
				text = text + " AND IsServiceProduct = " + ((query.ProductType == ProductType.ServiceProduct) ? "1" : "0");
			}
			return DataHelper.PagingByRownumber<AfterSaleRecordModel>(query.PageIndex, query.PageSize, "ApplyForTime", SortAction.Desc, true, "vw_Hishop_AfterSaleRecords", "KeyId", text, "*");
		}

		public int GetUserAfterSaleCount(int userId, bool isCountReplace = false, ProductType? productType = default(ProductType?))
		{
			int num = 0;
			string text = " OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE UserId = " + userId + " AND ParentOrderId<>'-1')";
			if (!isCountReplace)
			{
				text += " AND AfterSaleType <> 2 ";
			}
			text = text + " AND (HandleStatus = " + 0 + " AND AfterSaleType = " + 0;
			text = text + " OR (HandleStatus NOT IN(" + 2 + "," + 1 + ") AND AfterSaleType = " + 1 + ")";
			text = text + " OR (HandleStatus = " + 0 + " AND AfterSaleType = " + 3 + ")";
			if (isCountReplace)
			{
				text = text + " OR (HandleStatus NOT IN(" + 2 + "," + 1 + ") AND AfterSaleType = " + 2 + ")";
			}
			text += ")";
			if (productType.HasValue && productType != ProductType.All)
			{
				text = text + " AND IsServiceProduct = " + ((productType == ProductType.ServiceProduct) ? "1" : "0");
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT COUNT(*) FROM vw_Hishop_AfterSaleRecords WHERE" + text);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0);
		}

		public DbQueryResult GetMyUserOrder(int userId, OrderQuery query)
		{
			if (string.IsNullOrEmpty(query.SortBy))
			{
				query.SortBy = "OrderDate";
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("UserId = {0}  AND ((ParentOrderId<>'-1' AND OrderStatus<>" + 1 + ") OR (OrderStatus=" + 1 + " AND (ParentOrderId='-1' OR ParentOrderId='0'))) ", userId);
			if (!string.IsNullOrEmpty(query.OrderId))
			{
				if (query.OrderId.Contains("P"))
				{
					stringBuilder.AppendFormat(" AND ParentOrderId = '{0}'", DataHelper.CleanSearchString(query.OrderId));
				}
				else
				{
					stringBuilder.AppendFormat(" AND OrderId = '{0}'", DataHelper.CleanSearchString(query.OrderId));
				}
			}
			if (!string.IsNullOrEmpty(query.ParentOrderId))
			{
				stringBuilder.AppendFormat(" AND ParentOrderId = '{0}'", DataHelper.CleanSearchString(query.ParentOrderId));
			}
			if (!string.IsNullOrEmpty(query.ShipId))
			{
				stringBuilder.AppendFormat(" AND ShipOrderNumber = '{0}'", DataHelper.CleanSearchString(query.ShipId));
			}
			if (!string.IsNullOrEmpty(query.ShipTo))
			{
				stringBuilder.AppendFormat(" AND ShipTo LIKE '%{0}%'", DataHelper.CleanSearchString(query.ShipTo));
			}
			if (query.Status == OrderStatus.History)
			{
				stringBuilder.AppendFormat(" AND OrderStatus = {0} AND OrderDate < '{1}'", 5, DateTime.Now.AddMonths(-3));
			}
			else if (query.Status == OrderStatus.SellerAlreadySent)
			{
				stringBuilder.Append(" AND (OrderStatus = " + 3 + " and ShippingModeId!=-2 AND ItemStatus = " + 0 + ")");
			}
			else if (query.Status == OrderStatus.WaitReview)
			{
				stringBuilder.Append((" AND OrderId IN (SELECT DISTINCT OI.OrderId FROM Hishop_OrderItems OI LEFT JOIN Hishop_ProductReviews PR ON OI.OrderId = PR.OrderId AND OI.SkuId = PR.SkuId  WHERE PR.ReviewId IS NULL) AND (OrderStatus = " + 5 + " OR  (OrderStatus = " + 4 + " AND CloseReason = '订单全部退货完成')) AND UserId = " + userId) ?? "");
			}
			else if (query.Status == OrderStatus.BuyerAlreadyPaid)
			{
				stringBuilder.Append(" AND (OrderStatus = " + 2 + " OR  (Gateway = 'hishop.plugins.payment.podrequest' AND OrderStatus = " + 1 + ")) AND ParentOrderId<>'-1' AND ShippingModeId!=-2   AND ItemStatus = " + 0);
			}
			else if (query.Status != 0)
			{
				stringBuilder.AppendFormat(" AND OrderStatus = {0}", (int)query.Status);
			}
			if (query.StartDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND OrderDate > '{0}'", query.StartDate);
			}
			if (!string.IsNullOrEmpty(query.ProductName))
			{
				stringBuilder.AppendFormat(" And OrderID In (select OrderID from Hishop_OrderItems where ItemDescription like '%" + DataHelper.CleanSearchString(query.ProductName) + "%')");
			}
			if (query.EndDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND OrderDate < '{0}'", query.EndDate);
			}
			if (query.IsServiceOrder.HasValue && !query.IsServiceOrder.Value)
			{
				stringBuilder.Append(" And OrderType <>" + 6.GetHashCode() + " ");
			}
			return DataHelper.PagingByTopsort(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, true, "Hishop_Orders", null, stringBuilder.ToString(), "*,(OrderId+ISNULL(PayRandCode,'')) AS PayOrderId");
		}

		public int GetUserOrderCount(int userId, OrderQuery query)
		{
			string text = string.Empty;
			if (query.TakeOnStore.HasValue && query.TakeOnStore.Value)
			{
				text += $" AND ((ShippingModeId=-2 AND (OrderStatus={2} or (OrderStatus={1} AND PaymentTypeId=-3)) AND ItemStatus=0) or (OrderType = 6 and OrderStatus = 2))";
			}
			else if (query.Status == OrderStatus.WaitBuyerPay)
			{
				text = text + " AND (ParentOrderId='-1' OR ParentOrderId='0') AND OrderStatus = " + 1 + " AND Gateway <> 'hishop.plugins.payment.podrequest'";
			}
			else if (query.Status == OrderStatus.SellerAlreadySent)
			{
				text = text + " AND (OrderStatus = " + 3 + " and ShippingModeId!=-2 AND ItemStatus = " + 0 + ")";
			}
			else if (query.Status == OrderStatus.BuyerAlreadyPaid)
			{
				text = text + " AND (OrderStatus = " + 2 + " OR  (Gateway = 'hishop.plugins.payment.podrequest' AND OrderStatus = " + 1 + ")) AND ParentOrderId<>'-1' AND ShippingModeId!=-2   AND ItemStatus = " + 0;
			}
			else if (query.Status == OrderStatus.Finished)
			{
				text = text + " AND (OrderStatus = " + 5 + " OR  (Gateway = 'hishop.plugins.payment.podrequest' AND OrderStatus = " + 5 + ")) AND ShippingModeId!=-2 AND ItemStatus = " + 0;
			}
			else if (query.Status == OrderStatus.WaitReview)
			{
				text = text + " AND OrderId IN (SELECT DISTINCT OI.OrderId FROM Hishop_OrderItems OI LEFT JOIN Hishop_ProductReviews PR ON OI.OrderId = PR.OrderId AND OI.SkuId = PR.SkuId  WHERE PR.ReviewId IS NULL) AND (OrderStatus = " + 5 + " OR  (OrderStatus = " + 4 + " AND CloseReason = '订单全部退货完成'))";
			}
			if (!query.ShowGiftOrder)
			{
				text += " AND (SELECT COUNT(OrderID) FROM Hishop_OrderItems WHERE OrderID = o.OrderId OR OrderId IN(SELECT OrderId FROM Hishop_Orders WHERE ParentOrderId = o.OrderID))>0";
			}
			if (query.ItemStatus > 0)
			{
				text += $" AND ((ItemStatus = {query.ItemStatus}) OR OrderStatus = {6} OR ( StoreId > 0 AND IsConfirm = 1 ) )  ";
			}
			OrderType? type = query.Type;
			if (type.HasValue)
			{
				object[] obj = new object[4]
				{
					text,
					" AND OrderType=",
					null,
					null
				};
				type = query.Type;
				obj[2] = type.GetHashCode();
				obj[3] = " ";
				text = string.Concat(obj);
			}
			if (query.IsServiceOrder.HasValue && !query.IsServiceOrder.Value)
			{
				text += $" And OrderType <>'{6.GetHashCode()}'";
			}
			if (query.IsIncludePreSaleOrder.HasValue && query.IsIncludePreSaleOrder == false)
			{
				text += " AND PreSaleId = 0 ";
			}
			if (query.IsAfterSales.HasValue && query.IsAfterSales.Value)
			{
				string text2 = $"({6})";
				text = text + " AND (OrderStatus IN " + text2 + " OR ItemStatus <> " + 0 + ")";
			}
			if (query.SupplierId.HasValue)
			{
				text = ((query.SupplierId != -1) ? (text + " AND SupplierId=" + query.SupplierId) : (text + " AND SupplierId>0"));
			}
			if (query.StoreId.HasValue)
			{
				text = ((!(query.StoreId < 0)) ? (text + " AND StoreId = " + query.StoreId) : (text + " AND StoreId > 0"));
			}
			string str = "SELECT COUNT(1)  FROM Hishop_Orders o WHERE UserId = @UserId  AND ((ParentOrderId<>'-1' AND OrderStatus<>" + 1 + ") OR (OrderStatus=" + 1 + " AND (ParentOrderId='-1' OR ParentOrderId='0')))";
			str += text;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(str);
			sqlStringCommand.CommandType = CommandType.Text;
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			return (int)base.database.ExecuteScalar(sqlStringCommand);
		}

		public bool EditOrderShipNumber(string orderId, string shipNumber)
		{
			orderId = base.GetTrueOrderId(orderId);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE Hishop_Orders SET ShipOrderNumber=@ShipOrderNumber WHERE OrderId = @OrderId");
			base.database.AddInParameter(sqlStringCommand, "ShipOrderNumber", DbType.String, shipNumber);
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public DbQueryResult GetOrders(OrderQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder("1=1 ");
			int num;
			if (!query.IsAllOrder)
			{
				int? supplierId = query.SupplierId;
				num = -1;
				if (supplierId == num)
				{
					stringBuilder.AppendFormat(" AND SupplierId>0");
				}
				else
				{
					stringBuilder.AppendFormat(" AND SupplierId={0}", query.SupplierId);
				}
			}
			if (!string.IsNullOrEmpty(query.OrderId))
			{
				if (query.OrderId.Contains("P"))
				{
					stringBuilder.AppendFormat(" AND (ParentOrderId = '{0}' OR (ParentOrderId + PayRandCode) = '{0}')", DataHelper.CleanSearchString(query.OrderId));
				}
				else
				{
					stringBuilder.AppendFormat(" AND (OrderId = '{0}' OR (OrderId + PayRandCode) = '{0}')", DataHelper.CleanSearchString(query.OrderId));
				}
				if (query.StoreId.HasValue)
				{
					if (query.StoreId.Value == 0)
					{
						stringBuilder.Append(" AND (StoreId = 0 or StoreId is null or StoreId='')");
					}
					else
					{
						stringBuilder.AppendFormat(" AND StoreId = {0}", query.StoreId.Value);
					}
				}
			}
			else
			{
				OrderType? type = query.Type;
				if (type.HasValue)
				{
					type = query.Type;
					if (type.Value == OrderType.GroupOrder)
					{
						stringBuilder.Append(" And GroupBuyId > 0 ");
					}
					else
					{
						stringBuilder.Append(" And GroupBuyId is null ");
					}
					StringBuilder stringBuilder2 = stringBuilder;
					type = query.Type;
					stringBuilder2.Append(" And OrderType =" + type.GetHashCode() + " ");
				}
				OrderType orderType;
				if (query.IsServiceOrder.HasValue && !query.IsServiceOrder.Value)
				{
					StringBuilder stringBuilder3 = stringBuilder;
					orderType = OrderType.ServiceOrder;
					stringBuilder3.Append(" And OrderType <>" + orderType.GetHashCode() + " ");
				}
				if (query.UserId.HasValue)
				{
					stringBuilder.AppendFormat(" AND UserId = {0}", query.UserId.Value);
				}
				if (query.PaymentType.HasValue)
				{
					stringBuilder.AppendFormat(" AND PaymentTypeId = '{0}'", query.PaymentType.Value);
				}
				if (query.GroupBuyId.HasValue)
				{
					stringBuilder.AppendFormat(" AND GroupBuyId = {0}", query.GroupBuyId.Value);
				}
				if (!string.IsNullOrEmpty(query.ProductName))
				{
					if (query.ProductName.Trim().Contains(" "))
					{
						StringBuilder stringBuilder4 = new StringBuilder();
						string[] array = query.ProductName.Trim().Split(' ');
						int num2 = 0;
						string[] array2 = array;
						foreach (string text in array2)
						{
							if (!string.IsNullOrEmpty(text.Trim()))
							{
								if (num2 == 0)
								{
									stringBuilder4.Append($" ItemDescription LIKE '%{DataHelper.CleanSearchString(text)}%'");
								}
								else
								{
									stringBuilder4.Append($" OR ItemDescription LIKE '%{DataHelper.CleanSearchString(text)}%'");
								}
								num2++;
							}
						}
						stringBuilder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM Hishop_OrderItems WHERE ({0}))", stringBuilder4.ToString());
					}
					else
					{
						stringBuilder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM Hishop_OrderItems WHERE ItemDescription LIKE '%{0}%')", DataHelper.CleanSearchString(query.ProductName));
					}
				}
				if (!string.IsNullOrEmpty(query.ShipTo))
				{
					stringBuilder.AppendFormat(" AND ShipTo LIKE '%{0}%'", DataHelper.CleanSearchString(query.ShipTo));
				}
				if (query.RegionId.HasValue)
				{
					stringBuilder.AppendFormat(" AND (ShippingRegion like '%{0}%' OR ','+FullRegionPath+',' like '%{1}%')", DataHelper.CleanSearchString(query.FullRegionName), "," + query.RegionId + ",");
				}
				if (!string.IsNullOrEmpty(query.UserName))
				{
					stringBuilder.AppendFormat(" AND  UserName like '%{0}%' ", DataHelper.CleanSearchString(query.UserName));
				}
				if (query.Status == OrderStatus.History)
				{
					stringBuilder.AppendFormat(" AND OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2} AND OrderDate < '{3}'", 1, 4, 9, DateTime.Now.AddMonths(-3));
				}
				else if (query.Status == OrderStatus.BuyerAlreadyPaid && query.TakeOnStore.HasValue && !query.TakeOnStore.Value)
				{
					int? supplierId = query.StoreId;
					num = 0;
					if (supplierId > num)
					{
						stringBuilder.AppendFormat(" AND (OrderStatus = {0} OR (OrderStatus = 1 AND Gateway = 'hishop.plugins.payment.podrequest')) AND StoreId={1} and ShippingModeId!=-2", (int)query.Status, query.StoreId);
					}
					else
					{
						stringBuilder.AppendFormat(" AND (OrderStatus = {0} OR (OrderStatus = 1 AND Gateway = 'hishop.plugins.payment.podrequest')) and ShippingModeId!=-2", (int)query.Status);
					}
				}
				else if (query.Status == OrderStatus.BuyerAlreadyPaid && query.TakeOnStore.HasValue && query.TakeOnStore.Value)
				{
					int? supplierId = query.StoreId;
					num = 0;
					if (supplierId > num)
					{
						stringBuilder.AppendFormat(" AND ((OrderStatus = {0} OR (OrderStatus = 1 AND Gateway = 'hishop.plugins.payment.podrequest')) or (PaymentTypeId=-3 AND OrderStatus = 1)) AND StoreId={1} and (ShippingModeId=-2 or OrderType=6)", (int)query.Status, query.StoreId);
					}
					else
					{
						stringBuilder.AppendFormat(" AND ((OrderStatus = {0} OR (OrderStatus = 1 AND Gateway = 'hishop.plugins.payment.podrequest')) or (PaymentTypeId=-3 AND OrderStatus = 1)) and (ShippingModeId=-2 or OrderType=6)", (int)query.Status);
					}
				}
				else if (query.Status == OrderStatus.ApplyForRefund)
				{
					stringBuilder.AppendFormat(" AND OrderStatus = {0}", (int)query.Status);
				}
				else if (query.Status != 0)
				{
					stringBuilder.AppendFormat(" AND OrderStatus = {0}", (int)query.Status);
					if (query.ItemStatus.HasValue)
					{
						stringBuilder.AppendFormat(" And ItemStatus={0}", query.ItemStatus.Value);
					}
				}
				if (query.TakeOnStore.HasValue && query.Status != OrderStatus.BuyerAlreadyPaid)
				{
					stringBuilder.AppendFormat(" and ShippingModeId {0}", query.TakeOnStore.Value ? " = -2" : " <> -2");
				}
				if (query.IsWaitTakeOnStore.HasValue && query.IsWaitTakeOnStore.Value)
				{
					stringBuilder.Append(" And (");
					stringBuilder.AppendFormat("(IsConfirm = {0} AND ShippingModeId = {1} AND (OrderStatus = {2} OR OrderStatus = {3}))", 1, -2, 1, 2);
					StringBuilder stringBuilder5 = stringBuilder;
					object[] obj = new object[5]
					{
						" OR ( OrderType =",
						null,
						null,
						null,
						null
					};
					orderType = OrderType.ServiceOrder;
					obj[1] = orderType.GetHashCode();
					obj[2] = " and  OrderStatus = ";
					obj[3] = 2;
					obj[4] = ") ";
					stringBuilder5.Append(string.Concat(obj));
					stringBuilder.Append(")");
				}
				if (query.IsConfirm.HasValue)
				{
					if (query.IsConfirm.Value)
					{
						stringBuilder.AppendFormat(" And IsConfirm = {0}", 1);
					}
					else
					{
						stringBuilder.AppendFormat(" And IsConfirm = {0} AND (ShippingModeId = -2 AND (OrderStatus = {1}  OR (OrderStatus = " + 1 + " AND PaymentTypeId = -3))) ", 0, 2);
					}
				}
				if (query.IsAfterSales.HasValue && query.IsAfterSales.Value)
				{
					num = 6;
					string arg = num.ToString();
					stringBuilder.AppendFormat(" And (OrderStatus IN ({0}) OR ItemStatus <> 0)", arg);
				}
				if (query.IsAfterSaleRefused.HasValue && query.IsAfterSaleRefused.Value)
				{
					num = 18;
					string arg2 = num.ToString();
					stringBuilder.AppendFormat(" And (OrderStatus in ({0}) AND ItemStatus = 0)", arg2);
				}
				if (query.IsAfterSaleCompleted.HasValue && query.IsAfterSaleCompleted.Value)
				{
					num = 9;
					string arg3 = num.ToString();
					stringBuilder.AppendFormat(" And (OrderStatus in ({0}) OR ItemStatus != " + 0 + ")", arg3);
				}
				if (query.IsReturning.HasValue && query.IsReturning.Value)
				{
					stringBuilder.AppendFormat(" And (ItemStatus = " + 2 + ")");
				}
				if (query.IsTakeOnStoreCompleted.HasValue && query.IsTakeOnStoreCompleted.Value)
				{
					StringBuilder stringBuilder6 = stringBuilder;
					object[] obj2 = new object[5]
					{
						" And (OrderStatus = ",
						5,
						" AND (ShippingModeId = -2 or OrderType =",
						null,
						null
					};
					orderType = OrderType.ServiceOrder;
					obj2[3] = orderType.GetHashCode();
					obj2[4] = ")) ";
					stringBuilder6.Append(string.Concat(obj2));
				}
				if (query.IsAllTakeOnStore.HasValue && query.IsAllTakeOnStore.Value)
				{
					StringBuilder stringBuilder7 = stringBuilder;
					orderType = OrderType.ServiceOrder;
					stringBuilder7.Append(" AND (ShippingModeId = -2 or OrderType =" + orderType.GetHashCode() + ")");
				}
				if (query.IsAllAfterSale.HasValue && query.IsAllAfterSale.Value)
				{
					num = 6;
					string arg4 = num.ToString();
					arg4 = arg4 + "," + 18;
					arg4 = arg4 + "," + 9;
					stringBuilder.AppendFormat(" And (OrderStatus IN ({0}) OR ItemStatus <> 0)", arg4);
				}
				if (query.IsStoreCollection.HasValue)
				{
					if (query.IsStoreCollection.Value)
					{
						stringBuilder.Append(" AND IsStoreCollect = 1 ");
					}
					else
					{
						stringBuilder.Append(" AND IsStoreCollect = 0 ");
					}
				}
				if (query.StartDate.HasValue)
				{
					stringBuilder.AppendFormat(" AND datediff(dd,'{0}',OrderDate)>=0", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
				}
				if (query.EndDate.HasValue)
				{
					stringBuilder.AppendFormat(" AND datediff(dd,'{0}',OrderDate)<=0", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
				}
				if (query.ShippingModeId.HasValue)
				{
					if (query.ShippingModeId.Value == -2)
					{
						StringBuilder stringBuilder8 = stringBuilder;
						orderType = OrderType.ServiceOrder;
						stringBuilder8.Append(" AND (ShippingModeId = -2 or OrderType =" + orderType.GetHashCode() + ")");
					}
					else
					{
						stringBuilder.AppendFormat(" AND (ShippingModeId = {0} )", query.ShippingModeId.Value);
					}
				}
				if (query.IsPrinted.HasValue)
				{
					stringBuilder.AppendFormat(" AND ISNULL(IsPrinted, 0)={0}", query.IsPrinted.Value);
				}
				if (query.SourceOrder.HasValue)
				{
					stringBuilder.AppendFormat(" And SourceOrder = {0}", query.SourceOrder.Value);
				}
				if (query.StoreId.HasValue)
				{
					if (query.StoreId.Value == 0)
					{
						stringBuilder.Append(" AND (StoreId = 0 or StoreId is null or StoreId='')");
					}
					else
					{
						stringBuilder.AppendFormat(" AND StoreId = {0}", query.StoreId.Value);
					}
				}
				if (query.IsTickit.HasValue)
				{
					stringBuilder.AppendFormat(" and (Tax {0}", query.IsTickit.Value ? ">0)" : " is null or Tax = '')");
				}
				if (!string.IsNullOrEmpty(query.TakeCode))
				{
					stringBuilder.AppendFormat(" and TakeCode = '{0}'", DataHelper.CleanSearchString(query.TakeCode));
				}
				if (query.IsAllotStore.HasValue)
				{
					int? supplierId = query.IsAllotStore;
					num = 1;
					if (supplierId == num)
					{
						stringBuilder.Append(" AND (StoreId = 0 or StoreId is null or StoreId='') AND OrderStatus=2 ");
					}
					else
					{
						supplierId = query.IsAllotStore;
						num = 2;
						if (supplierId == num)
						{
							stringBuilder.Append(" and StoreId > 0 ");
						}
					}
				}
				if (query.IsPay)
				{
					stringBuilder.Append(" AND PayDate IS NOT NULL");
				}
				if (!string.IsNullOrEmpty(query.InvoiceTypes))
				{
					stringBuilder.Append(" AND InvoiceTitle IS NOT NULL AND InvoiceTitle <> '' AND InvoiceType IN(" + query.InvoiceTypes + ")");
				}
			}
			stringBuilder.Append(" AND ParentOrderId <> '-1' ");
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_Orders", "OrderId", stringBuilder.ToString(), "*,(OrderId+ISNULL(PayRandCode,'')) AS PayOrderId");
		}

		public bool SetExceptionOrder(string orderId, string errorMsg)
		{
			orderId = base.GetTrueOrderId(orderId);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE Hishop_Orders SET ErrorMessage = @ErrorMessage, IsError = @IsError WHERE OrderId = @OrderId");
			base.database.AddInParameter(sqlStringCommand, "IsError", DbType.Boolean, true);
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			base.database.AddInParameter(sqlStringCommand, "ErrorMessage", DbType.String, errorMsg);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public List<OrderInfo> GetExportOrders(OrderQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT * FROM Hishop_Orders WHERE 1=1 ");
			if (query.SupplierId == -1)
			{
				stringBuilder.AppendFormat(" AND SupplierId>0");
			}
			else if (query.SupplierId >= 0)
			{
				stringBuilder.AppendFormat(" AND SupplierId={0}", query.SupplierId);
			}
			if (!string.IsNullOrEmpty(query.OrderIds))
			{
				stringBuilder.AppendFormat(" AND OrderId IN ({0})", query.OrderIds);
			}
			else if (!string.IsNullOrEmpty(query.OrderId))
			{
				if (query.OrderId.Contains("P"))
				{
					stringBuilder.AppendFormat(" AND ParentOrderId = '{0}'", DataHelper.CleanSearchString(query.OrderId));
				}
				else
				{
					stringBuilder.AppendFormat(" AND OrderId = '{0}'", DataHelper.CleanSearchString(query.OrderId));
				}
				if (query.StoreId.HasValue)
				{
					if (query.StoreId.Value == 0)
					{
						stringBuilder.Append(" AND (StoreId = 0 or StoreId is null or StoreId='')");
					}
					else
					{
						stringBuilder.AppendFormat(" AND StoreId = {0}", query.StoreId.Value);
					}
				}
			}
			else
			{
				OrderType? type = query.Type;
				if (type.HasValue)
				{
					type = query.Type;
					if (type.Value == OrderType.GroupOrder)
					{
						stringBuilder.Append(" And GroupBuyId > 0 ");
					}
					else
					{
						stringBuilder.Append(" And GroupBuyId is null ");
					}
					StringBuilder stringBuilder2 = stringBuilder;
					type = query.Type;
					stringBuilder2.Append(" And OrderType =" + type.GetHashCode() + " ");
				}
				if (query.UserId.HasValue)
				{
					stringBuilder.AppendFormat(" AND UserId = '{0}'", query.UserId.Value);
				}
				if (query.PaymentType.HasValue)
				{
					stringBuilder.AppendFormat(" AND PaymentTypeId = '{0}'", query.PaymentType.Value);
				}
				if (query.GroupBuyId.HasValue)
				{
					stringBuilder.AppendFormat(" AND GroupBuyId = {0}", query.GroupBuyId.Value);
				}
				if (!string.IsNullOrEmpty(query.ProductName))
				{
					stringBuilder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM Hishop_OrderItems WHERE ItemDescription LIKE '%{0}%')", DataHelper.CleanSearchString(query.ProductName));
				}
				if (!string.IsNullOrEmpty(query.ShipTo))
				{
					stringBuilder.AppendFormat(" AND ShipTo LIKE '%{0}%'", DataHelper.CleanSearchString(query.ShipTo));
				}
				if (query.RegionId.HasValue)
				{
					stringBuilder.AppendFormat(" AND ShippingRegion like '%{0}%'", DataHelper.CleanSearchString(query.FullRegionName));
				}
				if (!string.IsNullOrEmpty(query.UserName))
				{
					stringBuilder.AppendFormat(" AND  UserName  = '{0}' ", DataHelper.CleanSearchString(query.UserName));
				}
				if (query.Status == OrderStatus.History)
				{
					stringBuilder.AppendFormat(" AND OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2} AND OrderDate < '{3}'", 1, 4, 9, DateTime.Now.AddMonths(-3));
				}
				else if (query.Status == OrderStatus.BuyerAlreadyPaid)
				{
					stringBuilder.AppendFormat(" AND (OrderStatus = {0} OR (OrderStatus = 1 AND Gateway = 'hishop.plugins.payment.podrequest'))", (int)query.Status);
				}
				else if (query.Status != 0)
				{
					stringBuilder.AppendFormat(" AND OrderStatus = {0}", (int)query.Status);
					if (query.ItemStatus.HasValue)
					{
						stringBuilder.AppendFormat(" And ItemStatus={0}", query.ItemStatus.Value);
					}
				}
				if (query.StartDate.HasValue)
				{
					stringBuilder.AppendFormat(" AND datediff(dd,'{0}',OrderDate)>=0", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
				}
				if (query.EndDate.HasValue)
				{
					stringBuilder.AppendFormat(" AND datediff(dd,'{0}',OrderDate)<=0", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
				}
				if (query.ShippingModeId.HasValue)
				{
					stringBuilder.AppendFormat(" AND ShippingModeId = {0}", query.ShippingModeId.Value);
				}
				if (query.IsPrinted.HasValue)
				{
					stringBuilder.AppendFormat(" AND ISNULL(IsPrinted, 0)={0}", query.IsPrinted.Value);
				}
				if (query.SourceOrder.HasValue)
				{
					stringBuilder.AppendFormat(" And SourceOrder = {0}", query.SourceOrder.Value);
				}
				if (query.StoreId.HasValue)
				{
					if (query.StoreId.Value == 0)
					{
						stringBuilder.Append(" AND (StoreId = 0 or StoreId is null or StoreId='')");
					}
					else
					{
						stringBuilder.AppendFormat(" AND StoreId = {0}", query.StoreId.Value);
					}
				}
				if (query.IsTickit.HasValue)
				{
					stringBuilder.AppendFormat(" and (Tax {0}", query.IsTickit.Value ? ">0)" : " is null or Tax = '')");
				}
				if (query.TakeOnStore.HasValue)
				{
					stringBuilder.AppendFormat(" and ShippingModeId {0}", query.TakeOnStore.Value ? " = -2" : " <> -2");
				}
				if (!string.IsNullOrEmpty(query.TakeCode))
				{
					stringBuilder.AppendFormat(" and TakeCode = '{0}'", DataHelper.CleanSearchString(query.TakeCode));
				}
				if (!string.IsNullOrEmpty(query.InvoiceTypes))
				{
					stringBuilder.Append(" AND InvoiceTitle IS NOT NULL AND InvoiceTitle <> '' AND InvoiceType IN(" + query.InvoiceTypes + ")");
				}
			}
			stringBuilder.Append(" AND ParentOrderId <> '-1'");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			List<OrderInfo> list = new List<OrderInfo>();
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					OrderInfo orderInfo = new OrderInfo();
					orderInfo.OrderId = ((IDataRecord)dataReader)["OrderId"].ToNullString();
					if (!string.IsNullOrEmpty(((IDataRecord)dataReader)["OrderDate"].ToNullString()))
					{
						orderInfo.OrderDate = DateTime.Parse(((IDataRecord)dataReader)["OrderDate"].ToNullString());
					}
					if (!string.IsNullOrEmpty(((IDataRecord)dataReader)["PayDate"].ToNullString()))
					{
						orderInfo.PayDate = DateTime.Parse(((IDataRecord)dataReader)["PayDate"].ToNullString());
					}
					if (!string.IsNullOrEmpty(((IDataRecord)dataReader)["FinishDate"].ToNullString()))
					{
						orderInfo.FinishDate = DateTime.Parse(((IDataRecord)dataReader)["FinishDate"].ToNullString());
					}
					orderInfo.UserId = ((IDataRecord)dataReader)["UserId"].ToInt(0);
					orderInfo.Username = ((IDataRecord)dataReader)["Username"].ToNullString();
					orderInfo.RealName = ((IDataRecord)dataReader)["RealName"].ToNullString();
					orderInfo.ShippingRegion = ((IDataRecord)dataReader)["ShippingRegion"].ToNullString();
					orderInfo.Address = ((IDataRecord)dataReader)["Address"].ToNullString();
					orderInfo.ZipCode = ((IDataRecord)dataReader)["ZipCode"].ToNullString();
					orderInfo.ShipTo = ((IDataRecord)dataReader)["ShipTo"].ToNullString();
					orderInfo.TelPhone = ((IDataRecord)dataReader)["TelPhone"].ToNullString();
					orderInfo.CellPhone = ((IDataRecord)dataReader)["CellPhone"].ToNullString();
					orderInfo.ShipToDate = ((IDataRecord)dataReader)["ShipToDate"].ToNullString();
					orderInfo.RealModeName = ((IDataRecord)dataReader)["RealModeName"].ToNullString();
					orderInfo.ModeName = ((IDataRecord)dataReader)["ModeName"].ToNullString();
					orderInfo.Freight = ((((IDataRecord)dataReader)["ActualFreight"] == DBNull.Value) ? ((IDataRecord)dataReader)["Freight"].ToDecimal(0) : ((IDataRecord)dataReader)["ActualFreight"].ToDecimal(0));
					orderInfo.ShipOrderNumber = ((IDataRecord)dataReader)["ShipOrderNumber"].ToString();
					orderInfo.Points = (int)((IDataRecord)dataReader)["OrderPoint"];
					if (!string.IsNullOrEmpty(((IDataRecord)dataReader)["SupplierId"].ToString()))
					{
						orderInfo.SupplierId = ((IDataRecord)dataReader)["SupplierId"].ToInt(0);
					}
					if (!string.IsNullOrEmpty(((IDataRecord)dataReader)["ShipperName"].ToString()))
					{
						orderInfo.ShipperName = ((IDataRecord)dataReader)["ShipperName"].ToString();
					}
					orderInfo.OrderCostPrice = ((IDataRecord)dataReader)["OrderCostPrice"].ToDecimal(0);
					if (DBNull.Value != ((IDataRecord)dataReader)["RefundAmount"])
					{
						orderInfo.RefundAmount = (decimal)((IDataRecord)dataReader)["RefundAmount"];
					}
					if (DBNull.Value != ((IDataRecord)dataReader)["IsincludeCrossBorderGoods"])
					{
						orderInfo.IsincludeCrossBorderGoods = (bool)((IDataRecord)dataReader)["IsincludeCrossBorderGoods"];
					}
					orderInfo.IDNumber = ((((IDataRecord)dataReader)["IDNumber"] == null) ? string.Empty : ((IDataRecord)dataReader)["IDNumber"].ToString());
					if (DBNull.Value != ((IDataRecord)dataReader)["IsReduced"])
					{
						orderInfo.IsReduced = (bool)((IDataRecord)dataReader)["IsReduced"];
					}
					if (DBNull.Value != ((IDataRecord)dataReader)["ReducedPromotionAmount"])
					{
						orderInfo.ReducedPromotionAmount = (decimal)((IDataRecord)dataReader)["ReducedPromotionAmount"];
					}
					if (DBNull.Value != ((IDataRecord)dataReader)["Tax"])
					{
						orderInfo.Tax = (decimal)((IDataRecord)dataReader)["Tax"];
					}
					if (DBNull.Value != ((IDataRecord)dataReader)["CouponCode"])
					{
						orderInfo.CouponCode = (string)((IDataRecord)dataReader)["CouponCode"];
					}
					if (DBNull.Value != ((IDataRecord)dataReader)["CouponValue"])
					{
						orderInfo.CouponValue = (decimal)((IDataRecord)dataReader)["CouponValue"];
					}
					if (DBNull.Value != ((IDataRecord)dataReader)["AdjustedFreight"])
					{
						orderInfo.AdjustedFreight = (decimal)((IDataRecord)dataReader)["AdjustedFreight"];
					}
					if (DBNull.Value != ((IDataRecord)dataReader)["DeductionMoney"])
					{
						orderInfo.DeductionMoney = (decimal)((IDataRecord)dataReader)["DeductionMoney"];
					}
					if (DBNull.Value != ((IDataRecord)dataReader)["AdjustedDiscount"])
					{
						orderInfo.AdjustedDiscount = (decimal)((IDataRecord)dataReader)["AdjustedDiscount"];
					}
					if (DBNull.Value != ((IDataRecord)dataReader)["Deposit"])
					{
						orderInfo.Deposit = (decimal)((IDataRecord)dataReader)["Deposit"];
					}
					if (DBNull.Value != ((IDataRecord)dataReader)["PreSaleId"])
					{
						orderInfo.PreSaleId = (int)((IDataRecord)dataReader)["PreSaleId"];
					}
					if (DBNull.Value != ((IDataRecord)dataReader)["ExpressCompanyName"])
					{
						orderInfo.ExpressCompanyName = ((IDataRecord)dataReader)["ExpressCompanyName"].ToNullString();
					}
					orderInfo.Remark = ((IDataRecord)dataReader)["Remark"].ToNullString();
					orderInfo.OrderStatus = (OrderStatus)((IDataRecord)dataReader)["OrderStatus"].ToInt(0);
					orderInfo.PaymentType = ((IDataRecord)dataReader)["PaymentType"].ToNullString();
					orderInfo.StoreId = ((IDataRecord)dataReader)["StoreId"].ToInt(0);
					if (orderInfo.StoreId > 0)
					{
						orderInfo.StoreName = new StoresDao().GetStoreNameByStoreId(orderInfo.StoreId);
					}
					orderInfo.ReducedPromotionName = ((IDataRecord)dataReader)["ReducedPromotionName"].ToNullString();
					orderInfo.SentTimesPointPromotionName = ((IDataRecord)dataReader)["SentTimesPointPromotionName"].ToNullString();
					orderInfo.FreightFreePromotionName = ((IDataRecord)dataReader)["FreightFreePromotionName"].ToNullString();
					orderInfo.Tax = ((IDataRecord)dataReader)["Tax"].ToDecimal(0);
					orderInfo.InvoiceTitle = ((IDataRecord)dataReader)["InvoiceTitle"].ToNullString();
					orderInfo.InvoiceTaxpayerNumber = ((IDataRecord)dataReader)["InvoiceTaxpayerNumber"].ToNullString();
					orderInfo.InvoiceType = (InvoiceType)((IDataRecord)dataReader)["InvoiceType"].ToInt(0);
					orderInfo.InvoiceData = ((IDataRecord)dataReader)["InvoiceData"].ToNullString();
					DbCommand sqlStringCommand2 = base.database.GetSqlStringCommand(" SELECT * FROM Hishop_OrderItems Where OrderId = @OrderId ;SELECT * FROM Hishop_OrderGifts Where OrderId = @OrderId;");
					base.database.AddInParameter(sqlStringCommand2, "OrderId", DbType.String, ((IDataRecord)dataReader)["OrderId"].ToString());
					using (IDataReader dataReader2 = base.database.ExecuteReader(sqlStringCommand2))
					{
						PromotionDao promotionDao = new PromotionDao();
						while (dataReader2.Read())
						{
							LineItemInfo lineItemInfo = DataMapper.PopulateLineItem(dataReader2);
							if (lineItemInfo.PromotionId > 0)
							{
								PromotionInfo promotion = promotionDao.GetPromotion(lineItemInfo.PromotionId);
								if (promotion != null)
								{
									lineItemInfo.PromoteType = promotion.PromoteType;
								}
							}
							orderInfo.LineItems.Add((string)((IDataRecord)dataReader2)["SkuId"], lineItemInfo);
							if (lineItemInfo.Status == LineItemStatus.Normal)
							{
								lineItemInfo.ReplaceInfo = null;
								lineItemInfo.ReturnInfo = null;
							}
							else if (lineItemInfo.Status == LineItemStatus.ReplaceApplied || lineItemInfo.Status == LineItemStatus.Replaced || lineItemInfo.Status == LineItemStatus.ReplaceRefused || lineItemInfo.Status == LineItemStatus.UserDeliveryForReplace || lineItemInfo.Status == LineItemStatus.MerchantsDeliveryForRepalce || lineItemInfo.Status == LineItemStatus.MerchantsAgreedForReplace)
							{
								ReplaceInfo replaceInfo2 = lineItemInfo.ReplaceInfo = new ReplaceDao().GetReplaceInfo(orderInfo.OrderId, lineItemInfo.SkuId);
							}
							else
							{
								ReturnInfo returnInfo2 = lineItemInfo.ReturnInfo = new ReturnDao().GetReturnInfo(orderInfo.OrderId, lineItemInfo.SkuId);
							}
						}
						dataReader2.NextResult();
						while (dataReader2.Read())
						{
							OrderGiftInfo item = DataMapper.PopulateOrderGift(dataReader2);
							orderInfo.Gifts.Add(item);
						}
						dataReader2.Close();
					}
					list.Add(orderInfo);
				}
				dataReader.Close();
			}
			return list;
		}

		public bool UpdateOrderWhenBankRequestInsert(int paymentId, string paymentName)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE Hishop_Orders SET PaymentTypeId = @PaymentTypeId,PaymentType = @PaymentType WHERE PaymentTypeId = 0 AND Gateway = 'hishop.plugins.payment.bankrequest'");
			base.database.AddInParameter(sqlStringCommand, "PaymentTypeId", DbType.Int32, paymentId);
			base.database.AddInParameter(sqlStringCommand, "PaymentType", DbType.String, paymentName);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public DataTable GetSendGoodsOrders(string orderIds, bool openMultStore, bool isStoreSend = false)
		{
			DataTable result = null;
			string text = "";
			text = ((!isStoreSend) ? string.Format("SELECT *,(OrderId+ISNULL(PayRandCode,'')) AS PayOrderId FROM Hishop_Orders WHERE (OrderStatus = 2 OR (OrderStatus = 1 AND Gateway = 'hishop.plugins.payment.podrequest')) AND ItemStatus = 0 AND {0} AND OrderId IN ({1}) AND (GroupBuyId is null OR (GroupBuyId>0 AND GroupBuyStatus=3))", (!openMultStore) ? "1=1" : "(StoreId is null or StoreId <= 0 or StoreId = '')", orderIds) : string.Format("SELECT *,(OrderId+ISNULL(PayRandCode,'')) AS PayOrderId FROM Hishop_Orders WHERE (OrderStatus = 2 OR (OrderStatus = 1 AND Gateway = 'hishop.plugins.payment.podrequest')) AND ItemStatus = 0 AND {0} AND OrderId IN ({1}) AND (GroupBuyId is null OR (GroupBuyId>0 AND GroupBuyStatus=3))", "ShippingModeId!=-2", orderIds));
			text += $" and OrderType<>'{6.GetHashCode()}'";
			text += " and (FightGroupId not in (select FightGroupId from Hishop_FightGroups where FightGroupId=Hishop_Orders.FightGroupId AND Status<>1))  order by OrderDate desc";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(text);
			using (IDataReader reader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(reader);
			}
			return result;
		}

		public DataSet GetOrdersAndLines(string orderIds)
		{
			base.database = DatabaseFactory.CreateDatabase();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT *,(OrderId+ISNULL(PayRandCode,'')) AS PayOrderId FROM Hishop_Orders WHERE OrderStatus > 0 AND OrderStatus < 4 AND OrderId IN ({0}) order by OrderDate desc ", orderIds);
			stringBuilder.AppendFormat(" SELECT * FROM Hishop_OrderItems WHERE OrderId IN ({0});", orderIds);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			return base.database.ExecuteDataSet(sqlStringCommand);
		}

		public DataSet GetOrderGoods(string orderIds)
		{
			base.database = DatabaseFactory.CreateDatabase();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT OrderId, ItemDescription AS ProductName, SKU, SKUContent, ShipmentQuantity,");
			stringBuilder.Append(" (SELECT Stock FROM Hishop_SKUs WHERE SkuId = oi.SkuId) + oi.ShipmentQuantity AS Stock, (SELECT Remark FROM Hishop_Orders WHERE OrderId = oi.OrderId) AS Remark");
			stringBuilder.Append(" FROM Hishop_OrderItems oi WHERE OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE (OrderStatus = 2 or (OrderStatus = 1 AND Gateway='hishop.plugins.payment.podrequest') AND ItemStatus = 0))");
			stringBuilder.AppendFormat(" AND OrderId IN ({0}) AND ISNULL(oi.Status,0) <> {1} AND ISNULL(oi.Status,0) <> {2} ORDER BY OrderId;", orderIds, 11, 24);
			stringBuilder.AppendFormat("SELECT OrderId AS GiftOrderId,GiftName,Quantity FROM dbo.Hishop_OrderGifts WHERE OrderId IN({0}) AND OrderId IN(SELECT OrderId FROM Hishop_Orders WHERE (OrderStatus = 2 or (OrderStatus = 1 AND Gateway='hishop.plugins.payment.podrequest')) AND ItemStatus = 0)", orderIds);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			return base.database.ExecuteDataSet(sqlStringCommand);
		}

		public DataSet GetStoreOrderGoods(string orderIds, int storeId)
		{
			base.database = DatabaseFactory.CreateDatabase();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT OrderId, ItemDescription AS ProductName, SKU, SKUContent, ShipmentQuantity,");
			stringBuilder.Append(" (SELECT Stock FROM Hishop_StoreSKUs WHERE SkuId = oi.SkuId and StoreId=" + storeId + ") + oi.ShipmentQuantity AS Stock, (SELECT Remark FROM Hishop_Orders WHERE OrderId = oi.OrderId) AS Remark");
			stringBuilder.Append(" FROM Hishop_OrderItems oi WHERE OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE OrderStatus = 2 or (OrderStatus = 1 AND Gateway='hishop.plugins.payment.podrequest'))");
			stringBuilder.AppendFormat(" AND OrderId IN ({0}) ORDER BY OrderId;", orderIds);
			stringBuilder.AppendFormat("SELECT OrderId AS GiftOrderId,GiftName,Quantity FROM dbo.Hishop_OrderGifts WHERE OrderId IN({0}) AND OrderId IN(SELECT OrderId FROM Hishop_Orders WHERE OrderStatus = 2 or (OrderStatus = 1 AND Gateway='hishop.plugins.payment.podrequest'))", orderIds);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			return base.database.ExecuteDataSet(sqlStringCommand);
		}

		public DataSet GetProductGoods(string orderIds)
		{
			base.database = DatabaseFactory.CreateDatabase();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT ItemDescription AS ProductName, SKU, SKUContent, sum(ShipmentQuantity) as ShipmentQuantity,");
			stringBuilder.Append(" (SELECT Stock FROM Hishop_SKUs WHERE SkuId = oi.SkuId) + sum(ShipmentQuantity) AS Stock FROM Hishop_OrderItems oi");
			stringBuilder.Append(" WHERE OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE OrderStatus = 2 or (OrderStatus = 1 AND Gateway='hishop.plugins.payment.podrequest'))");
			stringBuilder.AppendFormat(" AND OrderId in ({0})  AND ISNULL(oi.Status,0) <> {1} AND ISNULL(oi.Status,0) <> {2}  GROUP BY ItemDescription, SkuId, SKU, SKUContent;", orderIds, 11, 24);
			stringBuilder.AppendFormat("SELECT OrderId AS GiftOrderId,GiftName,Quantity FROM dbo.Hishop_OrderGifts WHERE OrderId IN({0}) AND OrderId IN(SELECT OrderId FROM Hishop_Orders WHERE (OrderStatus = 2 or (OrderStatus = 1 AND Gateway='hishop.plugins.payment.podrequest')) AND ItemStatus = 0)", orderIds);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			return base.database.ExecuteDataSet(sqlStringCommand);
		}

		public DataSet GetStoreProductGoods(string orderIds, int storeId)
		{
			base.database = DatabaseFactory.CreateDatabase();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT ItemDescription AS ProductName, SKU, SKUContent, sum(ShipmentQuantity) as ShipmentQuantity,");
			stringBuilder.Append(" (SELECT Stock FROM Hishop_StoreSKUs WHERE SkuId = oi.SkuId and StoreId=" + storeId + ") + sum(ShipmentQuantity) AS Stock FROM Hishop_OrderItems oi");
			stringBuilder.Append(" WHERE OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE OrderStatus = 2 or (OrderStatus = 1 AND Gateway='hishop.plugins.payment.podrequest'))");
			stringBuilder.AppendFormat(" AND OrderId in ({0}) AND ISNULL(oi.Status,0) <> {1} AND ISNULL(oi.Status,0) <> {2}  GROUP BY ItemDescription, SkuId, SKU, SKUContent;", orderIds, 11, 24);
			stringBuilder.AppendFormat("SELECT OrderId AS GiftOrderId,GiftName,Quantity FROM dbo.Hishop_OrderGifts WHERE OrderId IN({0}) AND OrderId IN(SELECT OrderId FROM Hishop_Orders WHERE OrderStatus = 2 or (OrderStatus = 1 AND Gateway='hishop.plugins.payment.podrequest') AND ItemStatus = 0)", orderIds);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			return base.database.ExecuteDataSet(sqlStringCommand);
		}

		public OrderInfo GetOrderInfo(string orderId)
		{
			orderId = base.GetTrueOrderId(orderId);
			OrderInfo singleOrderInfo = this.GetSingleOrderInfo(orderId);
			string query = " SELECT item.*,o.SupplierId,o.ShipperName FROM Hishop_OrderItems as item left join Hishop_Orders as o on item.OrderId=o.OrderId WHERE item.OrderId = @OrderId;SELECT * FROM Hishop_OrderGifts WHERE OrderID = @OrderID;Select * from Hishop_OrderInputItems where OrderID=@OrderID order by InputFieldGroup;";
			if (singleOrderInfo != null && singleOrderInfo.ParentOrderId == "-1")
			{
				query = "SELECT item.*,o.SupplierId,o.ShipperName FROM Hishop_OrderItems as item left join Hishop_Orders as o on item.OrderId=o.OrderId WHERE o.ParentOrderId = @OrderId;SELECT * FROM Hishop_OrderGifts WHERE OrderID in (select OrderId from Hishop_Orders where ParentOrderId = @OrderID);";
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			PromotionDao promotionDao = new PromotionDao();
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					LineItemInfo lineItemInfo = DataMapper.PopulateLineItem(dataReader);
					if (lineItemInfo.PromotionId > 0)
					{
						PromotionInfo promotion = promotionDao.GetPromotion(lineItemInfo.PromotionId);
						if (promotion != null)
						{
							lineItemInfo.PromoteType = promotion.PromoteType;
						}
					}
					lineItemInfo.SupplierId = (int)((IDataRecord)dataReader)["SupplierId"];
					lineItemInfo.SupplierName = ((IDataRecord)dataReader)["ShipperName"].ToNullString();
					singleOrderInfo.LineItems.Add((string)((IDataRecord)dataReader)["SkuId"], lineItemInfo);
					if (lineItemInfo.Status == LineItemStatus.Normal)
					{
						lineItemInfo.ReplaceInfo = null;
						lineItemInfo.ReturnInfo = null;
					}
					else if (lineItemInfo.Status == LineItemStatus.ReplaceApplied || lineItemInfo.Status == LineItemStatus.Replaced || lineItemInfo.Status == LineItemStatus.ReplaceRefused || lineItemInfo.Status == LineItemStatus.UserDeliveryForReplace || lineItemInfo.Status == LineItemStatus.MerchantsDeliveryForRepalce || lineItemInfo.Status == LineItemStatus.MerchantsAgreedForReplace)
					{
						ReplaceInfo replaceInfo2 = lineItemInfo.ReplaceInfo = new ReplaceDao().GetReplaceInfo(singleOrderInfo.OrderId, lineItemInfo.SkuId);
					}
					else
					{
						ReturnInfo returnInfo2 = lineItemInfo.ReturnInfo = new ReturnDao().GetReturnInfo(singleOrderInfo.OrderId, lineItemInfo.SkuId);
					}
				}
				dataReader.NextResult();
				while (dataReader.Read())
				{
					OrderGiftInfo item = DataMapper.PopulateOrderGift(dataReader);
					singleOrderInfo.Gifts.Add(item);
				}
				dataReader.NextResult();
				while (dataReader.Read())
				{
					OrderInputItemInfo item2 = DataMapper.PopulateInputItem(dataReader);
					singleOrderInfo.InputItems.Add(item2);
				}
			}
			return singleOrderInfo;
		}

		public OrderInfo GetSingleOrderInfo(string orderId)
		{
			orderId = base.GetTrueOrderId(orderId);
			OrderInfo orderInfo = null;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT s.StoreName,o.*,(CASE ISNULL(o.GroupBuyId,0) WHEN 0 THEN 0 ELSE (SELECT STATUS FROM Hishop_GroupBuy g WHERE g.GroupBuyId= o.GroupBuyId) END) AS GroupBuyStatus,(CASE ISNULL(o.FightGroupId,0) WHEN 0 THEN 0 ELSE (SELECT STATUS FROM Hishop_FightGroups fg WHERE fg.FightGroupId= o.FightGroupId) END) AS FightGroupStatus FROM Hishop_Orders as o left join Hishop_Stores as s on o.StoreId = s.StoreId WHERE OrderId = @OrderId;");
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			PromotionDao promotionDao = new PromotionDao();
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					orderInfo = DataMapper.PopulateOrder(dataReader);
					if (DBNull.Value != ((IDataRecord)dataReader)["StoreName"])
					{
						orderInfo.StoreName = (string)((IDataRecord)dataReader)["StoreName"];
					}
				}
			}
			return orderInfo;
		}

		public OrderInfo GetOrderInfoByTakeCode(string takeCode)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT  top 1 OrderId FROM Hishop_Orders WHERE TakeCode=@takeCode");
			base.database.AddInParameter(sqlStringCommand, "takeCode", DbType.String, takeCode);
			string value = base.database.ExecuteScalar(sqlStringCommand).ToNullString();
			if (string.IsNullOrEmpty(value))
			{
				return null;
			}
			OrderInfo orderInfo = null;
			DbCommand sqlStringCommand2 = base.database.GetSqlStringCommand("SELECT * FROM Hishop_Orders Where OrderId = @OrderId;  SELECT * FROM Hishop_OrderItems Where OrderId = @OrderId;Select * from Hishop_OrderGifts where OrderID=@OrderID");
			base.database.AddInParameter(sqlStringCommand2, "OrderId", DbType.String, value);
			PromotionDao promotionDao = new PromotionDao();
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand2))
			{
				if (dataReader.Read())
				{
					orderInfo = DataMapper.PopulateOrder(dataReader);
				}
				dataReader.NextResult();
				while (dataReader.Read())
				{
					LineItemInfo lineItemInfo = DataMapper.PopulateLineItem(dataReader);
					if (lineItemInfo.PromotionId > 0)
					{
						PromotionInfo promotion = promotionDao.GetPromotion(lineItemInfo.PromotionId);
						if (promotion != null)
						{
							lineItemInfo.PromoteType = promotion.PromoteType;
						}
					}
					orderInfo.LineItems.Add((string)((IDataRecord)dataReader)["SkuId"], lineItemInfo);
				}
				dataReader.NextResult();
				while (dataReader.Read())
				{
					OrderGiftInfo item = DataMapper.PopulateOrderGift(dataReader);
					orderInfo.Gifts.Add(item);
				}
			}
			return orderInfo;
		}

		public int GetOrderStatus(string orderId)
		{
			orderId = base.GetTrueOrderId(orderId);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT OrderStatus FROM Hishop_Orders WHERE OrderId = @orderId");
			base.database.AddInParameter(sqlStringCommand, "orderId", DbType.String, orderId);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0);
		}

		public int ValidateTakeCode(string takeCode, int storeId, string orderId, bool needTakeCode = true)
		{
			orderId = base.GetTrueOrderId(orderId);
			string text = "SELECT top 1  OrderId,OrderStatus,StoreId FROM Hishop_Orders WHERE 1=1 " + (string.IsNullOrEmpty(orderId) ? "" : " and OrderId = @orderId");
			if (needTakeCode)
			{
				text += " and TakeCode = @takeCode ";
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(text);
			if (needTakeCode)
			{
				base.database.AddInParameter(sqlStringCommand, "takeCode", DbType.String, takeCode);
			}
			if (!string.IsNullOrEmpty(orderId))
			{
				base.database.AddInParameter(sqlStringCommand, "orderId", DbType.String, orderId);
			}
			DataTable dataTable = base.database.ExecuteDataSet(sqlStringCommand).Tables[0];
			if (dataTable.IsNullOrEmpty())
			{
				return -1;
			}
			if (dataTable.Rows[0]["StoreId"].ToInt(0) != storeId)
			{
				return -3;
			}
			int num = dataTable.Rows[0]["OrderStatus"].ToInt(0);
			if (num != 1 && num != 2)
			{
				return -2;
			}
			return 1;
		}

		public OrderInfo ValidateTakeCode(string takeCode)
		{
			OrderInfo orderInfo = null;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM Hishop_Orders WHERE TakeCode = @TakeCode;");
			base.database.AddInParameter(sqlStringCommand, "TakeCode", DbType.String, takeCode);
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					orderInfo = DataMapper.PopulateOrder(dataReader);
				}
			}
			if (orderInfo != null)
			{
				sqlStringCommand = base.database.GetSqlStringCommand("\r\nWITH HiPOSOrderDetails\r\n     AS (SELECT DISTINCT p.ProductName, p.ProductId, oi.OrderId\r\n           FROM Hishop_OrderItems oi\r\n                INNER JOIN Hishop_Products p\r\n                   ON oi.ProductId = p.ProductId AND oi.OrderId = @OrderId)\r\nSELECT CASE\r\n          WHEN len (stuff ( (SELECT '' + ProductName\r\n                               FROM HiPOSOrderDetails\r\n                             FOR XML PATH ( '' )),\r\n                           1,\r\n                           0,\r\n                           '')) > 32\r\n          THEN\r\n               left (stuff ( (SELECT '' + ProductName\r\n                                FROM HiPOSOrderDetails\r\n                              FOR XML PATH ( '' )),\r\n                            1,\r\n                            0,\r\n                            ''),\r\n                     30)\r\n             + '等等'\r\n          ELSE\r\n             stuff ( (SELECT '' + ProductName\r\n                        FROM HiPOSOrderDetails\r\n                      FOR XML PATH ( '' )),\r\n                    1,\r\n                    0,\r\n                    '')\r\n       END\r\n          ProductNames\r\n");
				base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderInfo.OrderId);
				orderInfo.HiPOSOrderDetails = base.database.ExecuteScalar(sqlStringCommand).ToString();
			}
			return orderInfo;
		}

		public OrderInfo GetOrderInfoFromGateWayOrderId(string GatewayOrderId)
		{
			OrderInfo orderInfo = null;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM Hishop_Orders Where GatewayOrderId = @GatewayOrderId;  SELECT * FROM Hishop_OrderItems Where OrderId in (select(OrderId) FROM Hishop_Orders where GatewayOrderId = @GatewayOrderId)Select * from Hishop_OrderGifts where OrderId in (select(OrderId) FROM Hishop_Orders where GatewayOrderId = @GatewayOrderId)");
			base.database.AddInParameter(sqlStringCommand, "GatewayOrderId", DbType.String, GatewayOrderId);
			PromotionDao promotionDao = new PromotionDao();
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					orderInfo = DataMapper.PopulateOrder(dataReader);
				}
				dataReader.NextResult();
				while (dataReader.Read())
				{
					LineItemInfo lineItemInfo = DataMapper.PopulateLineItem(dataReader);
					if (lineItemInfo.PromotionId > 0)
					{
						PromotionInfo promotion = promotionDao.GetPromotion(lineItemInfo.PromotionId);
						if (promotion != null)
						{
							lineItemInfo.PromoteType = promotion.PromoteType;
						}
					}
					orderInfo.LineItems.Add((string)((IDataRecord)dataReader)["SkuId"], lineItemInfo);
				}
				dataReader.NextResult();
				while (dataReader.Read())
				{
					OrderGiftInfo item = DataMapper.PopulateOrderGift(dataReader);
					orderInfo.Gifts.Add(item);
				}
			}
			return orderInfo;
		}

		public DataTable GetOrderItemThumbnailsUrl(string orderId)
		{
			orderId = base.GetTrueOrderId(orderId);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT ThumbnailsUrl,ItemDescription,ProductId FROM Hishop_OrderItems WHERE OrderId=@OrderId");
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			DataSet dataSet = base.database.ExecuteDataSet(sqlStringCommand);
			return dataSet.Tables[0];
		}

		public DataTable GetOrderGiftsThumbnailsUrl(string orderId)
		{
			orderId = base.GetTrueOrderId(orderId);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT ThumbnailsUrl,GiftName ,GiftId FROM Hishop_OrderGifts WHERE OrderId=@OrderId");
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			DataSet dataSet = base.database.ExecuteDataSet(sqlStringCommand);
			return dataSet.Tables[0];
		}

		public int DeleteOrders(string orderIds)
		{
			string[] array = orderIds.Split(',');
			string text = "";
			int num = 0;
			int num2 = 0;
			string[] array2 = array;
			foreach (string text2 in array2)
			{
				OrderInfo orderInfo = new OrderDao().GetOrderInfo(text2.TrimStart('\'').TrimEnd('\''));
				int num3;
				if (orderInfo != null)
				{
					if (orderInfo.CountDownBuyId > 0)
					{
						foreach (LineItemInfo value in orderInfo.LineItems.Values)
						{
							new CountDownDao().ReductionCountDownBoughtCount(orderInfo.CountDownBuyId, value.SkuId, value.Quantity, null);
						}
					}
					num2++;
					if ((orderInfo.OrderStatus != OrderStatus.WaitBuyerPay || orderInfo.IsConfirm) && orderInfo.OrderStatus != OrderStatus.Closed && orderInfo.OrderStatus != OrderStatus.Refunded)
					{
						num3 = ((orderInfo.OrderStatus != OrderStatus.Closed && orderInfo.OrderStatus != OrderStatus.Refunded && orderInfo.OrderStatus != OrderStatus.WaitBuyerPay && orderInfo.OrderDate < DateTime.Now.AddMonths(-3)) ? 1 : 0);
						goto IL_0149;
					}
					num3 = 1;
					goto IL_0149;
				}
				continue;
				IL_0149:
				if (num3 != 0)
				{
					num++;
					text += ((text == "") ? ("'" + orderInfo.OrderId + "'") : (",'" + orderInfo.OrderId + "'"));
				}
			}
			if (text != "")
			{
				DbCommand sqlStringCommand = base.database.GetSqlStringCommand($"DELETE FROM Hishop_Orders WHERE OrderId IN({text})");
				return base.database.ExecuteNonQuery(sqlStringCommand);
			}
			return 0;
		}

		public bool CreatOrder(OrderInfo orderInfo, DbTransaction dbTran)
		{
			try
			{
				bool flag = false;
				DbCommand sqlStringCommand = base.database.GetSqlStringCommand("IF NOT EXISTS (SELECT OrderId  FROM Hishop_Orders WHERE OrderId = @OrderId)INSERT INTO Hishop_Orders(OrderId, OrderDate,PayDate, ReferralUserId, UserId, Username,RealName, EmailAddress, Remark, AdjustedDiscount, OrderStatus,ShippingRegion, Address, ZipCode, ShipTo, TelPhone, CellPhone, ShipToDate,LatLng, ShippingModeId, ModeName, RegionId, Freight, AdjustedFreight, ShipOrderNumber, [Weight], PaymentTypeId,PaymentType, OrderTotal, OrderPoint, OrderCostPrice, OrderProfit, Amount, ReducedPromotionId,ReducedPromotionName,ReducedPromotionAmount,IsReduced,SentTimesPointPromotionId,SentTimesPointPromotionName,TimesPoint,IsSendTimesPoint,FreightFreePromotionId,FreightFreePromotionName,IsFreightFree,CouponName, CouponCode, CouponAmount, CouponValue,GroupBuyId,NeedPrice,GroupBuyStatus,CountDownBuyId,ExpressCompanyName,ExpressCompanyAbb,OuterOrderId,Tax,SourceOrder,InvoiceTitle,Gateway,StoreId,TakeCode,DeductionPoints,DeductionMoney,FullRegionPath,PreSaleId,Deposit,DepositDate,DepositGatewayOrderId,FinalPayment,SupplierId,ShipperName,ParentOrderId,ExchangePoints,IsBalanceOver,IsServiceOver,UserAwardRecordsId,FightGroupId,IsFightGroupHead,IDNumber,IDImage1,IDImage2,IDStatus,IDRemark,IsincludeCrossBorderGoods,ShippingId,OrderType,InvoiceType,InvoiceTaxpayerNumber,BalanceAmount,InvoiceData)VALUES (@OrderId, @OrderDate,@PayDate, @ReferralUserId, @UserId, @Username,@RealName, @EmailAddress, @Remark, @AdjustedDiscount, @OrderStatus,@ShippingRegion, @Address, @ZipCode, @ShipTo, @TelPhone, @CellPhone, @ShipToDate,@LatLng, @ShippingModeId, @ModeName, @RegionId, @Freight, @AdjustedFreight, @ShipOrderNumber, @Weight, @PaymentTypeId,@PaymentType, @OrderTotal, @OrderPoint, @OrderCostPrice, @OrderProfit, @Amount, @ReducedPromotionId,@ReducedPromotionName,@ReducedPromotionAmount,@IsReduced,@SentTimesPointPromotionId,@SentTimesPointPromotionName,@TimesPoint,@IsSendTimesPoint,@FreightFreePromotionId,@FreightFreePromotionName,@IsFreightFree,@CouponName, @CouponCode, @CouponAmount, @CouponValue,@GroupBuyId,@NeedPrice,@GroupBuyStatus,@CountDownBuyId,@ExpressCompanyName,@ExpressCompanyAbb,@OuterOrderId,@Tax,@SourceOrder,@InvoiceTitle,@Gateway,@StoreId,@TakeCode,@DeductionPoints,@DeductionMoney,@FullRegionPath,@PreSaleId,@Deposit,@DepositDate,@DepositGatewayOrderId,@FinalPayment,@SupplierId,@ShipperName,@ParentOrderId,@ExchangePoints,0,0,@UserAwardRecordsId,@FightGroupId,@IsFightGroupHead,@IDNumber,@IDImage1,@IDImage2,@IDStatus,@IDRemark,@IsincludeCrossBorderGoods,@ShippingId,@OrderType,@InvoiceType,@InvoiceTaxpayerNumber,@BalanceAmount,@InvoiceData);");
				base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderInfo.OrderId);
				base.database.AddInParameter(sqlStringCommand, "OrderDate", DbType.DateTime, orderInfo.OrderDate);
				if (orderInfo.PayDate != DateTime.MinValue)
				{
					base.database.AddInParameter(sqlStringCommand, "PayDate", DbType.DateTime, orderInfo.PayDate);
				}
				else
				{
					base.database.AddInParameter(sqlStringCommand, "PayDate", DbType.DateTime, null);
				}
				if (orderInfo.DepositDate != (DateTime?)DateTime.MinValue)
				{
					base.database.AddInParameter(sqlStringCommand, "DepositDate", DbType.DateTime, orderInfo.DepositDate);
				}
				else
				{
					base.database.AddInParameter(sqlStringCommand, "DepositDate", DbType.DateTime, null);
				}
				base.database.AddInParameter(sqlStringCommand, "DepositGatewayOrderId", DbType.String, orderInfo.DepositGatewayOrderId);
				base.database.AddInParameter(sqlStringCommand, "ReferralUserId", DbType.Int32, orderInfo.ReferralUserId);
				base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, orderInfo.UserId);
				base.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, orderInfo.Username);
				base.database.AddInParameter(sqlStringCommand, "Wangwang", DbType.String, orderInfo.Wangwang);
				base.database.AddInParameter(sqlStringCommand, "RealName", DbType.String, orderInfo.RealName);
				base.database.AddInParameter(sqlStringCommand, "EmailAddress", DbType.String, orderInfo.EmailAddress);
				base.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, orderInfo.Remark);
				base.database.AddInParameter(sqlStringCommand, "AdjustedDiscount", DbType.Currency, orderInfo.AdjustedDiscount);
				base.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, (int)orderInfo.OrderStatus);
				base.database.AddInParameter(sqlStringCommand, "ShippingRegion", DbType.String, orderInfo.ShippingRegion);
				base.database.AddInParameter(sqlStringCommand, "Address", DbType.String, orderInfo.Address);
				base.database.AddInParameter(sqlStringCommand, "ZipCode", DbType.String, orderInfo.ZipCode);
				base.database.AddInParameter(sqlStringCommand, "ShipTo", DbType.String, orderInfo.ShipTo);
				base.database.AddInParameter(sqlStringCommand, "TelPhone", DbType.String, orderInfo.TelPhone);
				base.database.AddInParameter(sqlStringCommand, "CellPhone", DbType.String, orderInfo.CellPhone);
				base.database.AddInParameter(sqlStringCommand, "ShipToDate", DbType.String, orderInfo.ShipToDate);
				base.database.AddInParameter(sqlStringCommand, "LatLng", DbType.String, orderInfo.LatLng);
				base.database.AddInParameter(sqlStringCommand, "ShippingModeId", DbType.Int32, orderInfo.ShippingModeId);
				base.database.AddInParameter(sqlStringCommand, "ModeName", DbType.String, orderInfo.ModeName);
				base.database.AddInParameter(sqlStringCommand, "RegionId", DbType.Int32, orderInfo.RegionId);
				base.database.AddInParameter(sqlStringCommand, "Freight", DbType.Currency, orderInfo.Freight);
				base.database.AddInParameter(sqlStringCommand, "AdjustedFreight", DbType.Currency, orderInfo.AdjustedFreight);
				base.database.AddInParameter(sqlStringCommand, "ShipOrderNumber", DbType.String, orderInfo.ShipOrderNumber);
				base.database.AddInParameter(sqlStringCommand, "Weight", DbType.Int32, orderInfo.Weight);
				base.database.AddInParameter(sqlStringCommand, "ExpressCompanyName", DbType.String, orderInfo.ExpressCompanyName);
				base.database.AddInParameter(sqlStringCommand, "ExpressCompanyAbb", DbType.String, orderInfo.ExpressCompanyAbb);
				base.database.AddInParameter(sqlStringCommand, "TakeCode", DbType.String, orderInfo.TakeCode);
				base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, orderInfo.StoreId);
				base.database.AddInParameter(sqlStringCommand, "PaymentTypeId", DbType.Int32, orderInfo.PaymentTypeId);
				base.database.AddInParameter(sqlStringCommand, "PaymentType", DbType.String, orderInfo.PaymentType);
				base.database.AddInParameter(sqlStringCommand, "Gateway", DbType.String, orderInfo.Gateway);
				base.database.AddInParameter(sqlStringCommand, "OrderTotal", DbType.Currency, orderInfo.GetTotal(false));
				base.database.AddInParameter(sqlStringCommand, "OrderPoint", DbType.Int32, orderInfo.Points);
				base.database.AddInParameter(sqlStringCommand, "OrderCostPrice", DbType.Currency, orderInfo.GetCostPrice());
				base.database.AddInParameter(sqlStringCommand, "OrderProfit", DbType.Currency, orderInfo.GetProfit());
				base.database.AddInParameter(sqlStringCommand, "Amount", DbType.Currency, orderInfo.GetAmount(false));
				base.database.AddInParameter(sqlStringCommand, "ReducedPromotionId", DbType.Int32, orderInfo.ReducedPromotionId);
				base.database.AddInParameter(sqlStringCommand, "ReducedPromotionName", DbType.String, orderInfo.ReducedPromotionName);
				base.database.AddInParameter(sqlStringCommand, "ReducedPromotionAmount", DbType.Currency, orderInfo.ReducedPromotionAmount);
				base.database.AddInParameter(sqlStringCommand, "IsReduced", DbType.Boolean, orderInfo.IsReduced);
				base.database.AddInParameter(sqlStringCommand, "SentTimesPointPromotionId", DbType.Int32, orderInfo.SentTimesPointPromotionId);
				base.database.AddInParameter(sqlStringCommand, "SentTimesPointPromotionName", DbType.String, orderInfo.SentTimesPointPromotionName);
				base.database.AddInParameter(sqlStringCommand, "TimesPoint", DbType.Currency, orderInfo.TimesPoint);
				base.database.AddInParameter(sqlStringCommand, "IsSendTimesPoint", DbType.Boolean, orderInfo.IsSendTimesPoint);
				base.database.AddInParameter(sqlStringCommand, "FreightFreePromotionId", DbType.Int32, orderInfo.FreightFreePromotionId);
				base.database.AddInParameter(sqlStringCommand, "FreightFreePromotionName", DbType.String, orderInfo.FreightFreePromotionName);
				base.database.AddInParameter(sqlStringCommand, "IsFreightFree", DbType.Boolean, orderInfo.IsFreightFree);
				base.database.AddInParameter(sqlStringCommand, "CouponName", DbType.String, orderInfo.CouponName);
				base.database.AddInParameter(sqlStringCommand, "CouponCode", DbType.String, orderInfo.CouponCode);
				base.database.AddInParameter(sqlStringCommand, "CouponAmount", DbType.Currency, orderInfo.CouponAmount);
				base.database.AddInParameter(sqlStringCommand, "CouponValue", DbType.Currency, orderInfo.CouponValue);
				if (orderInfo.DeductionPoints.HasValue && orderInfo.DeductionPoints > 0)
				{
					base.database.AddInParameter(sqlStringCommand, "DeductionPoints", DbType.Int32, orderInfo.DeductionPoints);
					base.database.AddInParameter(sqlStringCommand, "DeductionMoney", DbType.Currency, orderInfo.DeductionMoney);
				}
				else
				{
					base.database.AddInParameter(sqlStringCommand, "DeductionPoints", DbType.Int32, DBNull.Value);
					base.database.AddInParameter(sqlStringCommand, "DeductionMoney", DbType.Currency, DBNull.Value);
				}
				if (!string.IsNullOrEmpty(orderInfo.OuterOrderId))
				{
					base.database.AddInParameter(sqlStringCommand, "OuterOrderId", DbType.String, orderInfo.OuterOrderId);
				}
				else
				{
					base.database.AddInParameter(sqlStringCommand, "OuterOrderId", DbType.String, DBNull.Value);
				}
				if (orderInfo.GroupBuyId > 0)
				{
					base.database.AddInParameter(sqlStringCommand, "GroupBuyId", DbType.Int32, orderInfo.GroupBuyId);
					base.database.AddInParameter(sqlStringCommand, "NeedPrice", DbType.Currency, orderInfo.NeedPrice);
					base.database.AddInParameter(sqlStringCommand, "GroupBuyStatus", DbType.Int32, 1);
				}
				else
				{
					base.database.AddInParameter(sqlStringCommand, "GroupBuyId", DbType.Int32, DBNull.Value);
					base.database.AddInParameter(sqlStringCommand, "NeedPrice", DbType.Currency, DBNull.Value);
					base.database.AddInParameter(sqlStringCommand, "GroupBuyStatus", DbType.Int32, DBNull.Value);
				}
				if (orderInfo.FightGroupActivityId > 0)
				{
					base.database.AddInParameter(sqlStringCommand, "FightGroupId", DbType.Int32, orderInfo.FightGroupId);
					base.database.AddInParameter(sqlStringCommand, "IsFightGroupHead", DbType.Boolean, orderInfo.IsFightGroupHead);
				}
				else
				{
					base.database.AddInParameter(sqlStringCommand, "FightGroupId", DbType.Int32, 0);
					base.database.AddInParameter(sqlStringCommand, "IsFightGroupHead", DbType.Boolean, false);
				}
				if (orderInfo.CountDownBuyId > 0)
				{
					base.database.AddInParameter(sqlStringCommand, "CountDownBuyId ", DbType.Int32, orderInfo.CountDownBuyId);
				}
				else
				{
					base.database.AddInParameter(sqlStringCommand, "CountDownBuyId ", DbType.Int32, DBNull.Value);
				}
				if (orderInfo.UserAwardRecordsId > 0)
				{
					base.database.AddInParameter(sqlStringCommand, "UserAwardRecordsId", DbType.Int32, orderInfo.UserAwardRecordsId);
				}
				else
				{
					base.database.AddInParameter(sqlStringCommand, "UserAwardRecordsId", DbType.Int32, 0);
				}
				if (orderInfo.PreSaleId > 0)
				{
					base.database.AddInParameter(sqlStringCommand, "PreSaleId", DbType.Int32, orderInfo.PreSaleId);
					base.database.AddInParameter(sqlStringCommand, "Deposit", DbType.Decimal, orderInfo.Deposit);
					base.database.AddInParameter(sqlStringCommand, "FinalPayment", DbType.Decimal, orderInfo.FinalPayment);
				}
				else
				{
					base.database.AddInParameter(sqlStringCommand, "PreSaleId", DbType.Int32, 0);
					base.database.AddInParameter(sqlStringCommand, "Deposit", DbType.Decimal, 0);
					base.database.AddInParameter(sqlStringCommand, "FinalPayment", DbType.Decimal, 0);
				}
				base.database.AddInParameter(sqlStringCommand, "Tax", DbType.Currency, orderInfo.Tax);
				base.database.AddInParameter(sqlStringCommand, "SourceOrder", DbType.Int32, (int)orderInfo.OrderSource);
				base.database.AddInParameter(sqlStringCommand, "FullRegionPath", DbType.String, orderInfo.FullRegionPath);
				base.database.AddInParameter(sqlStringCommand, "SupplierId", DbType.Int32, orderInfo.SupplierId);
				base.database.AddInParameter(sqlStringCommand, "ShipperName", DbType.String, orderInfo.ShipperName);
				base.database.AddInParameter(sqlStringCommand, "ParentOrderId", DbType.String, orderInfo.ParentOrderId);
				base.database.AddInParameter(sqlStringCommand, "ExchangePoints", DbType.Int32, orderInfo.ExchangePoints);
				base.database.AddInParameter(sqlStringCommand, "IDNumber", DbType.String, orderInfo.IDNumber);
				base.database.AddInParameter(sqlStringCommand, "IDImage1", DbType.String, orderInfo.IDImage1);
				base.database.AddInParameter(sqlStringCommand, "IDImage2", DbType.String, orderInfo.IDImage2);
				base.database.AddInParameter(sqlStringCommand, "IDStatus", DbType.Int32, orderInfo.IDStatus);
				base.database.AddInParameter(sqlStringCommand, "IDRemark", DbType.String, orderInfo.IDRemark);
				base.database.AddInParameter(sqlStringCommand, "IsincludeCrossBorderGoods", DbType.Boolean, orderInfo.IsincludeCrossBorderGoods);
				base.database.AddInParameter(sqlStringCommand, "ShippingId", DbType.Int32, orderInfo.ShippingId);
				base.database.AddInParameter(sqlStringCommand, "OrderType", DbType.Int32, (int)orderInfo.OrderType);
				base.database.AddInParameter(sqlStringCommand, "InvoiceTitle", DbType.String, orderInfo.InvoiceTitle);
				base.database.AddInParameter(sqlStringCommand, "InvoiceType", DbType.Int32, (int)orderInfo.InvoiceType);
				base.database.AddInParameter(sqlStringCommand, "InvoiceTaxpayerNumber", DbType.String, orderInfo.InvoiceTaxpayerNumber);
				base.database.AddInParameter(sqlStringCommand, "BalanceAmount", DbType.Decimal, orderInfo.BalanceAmount);
				base.database.AddInParameter(sqlStringCommand, "InvoiceData", DbType.String, orderInfo.InvoiceData);
				int num = base.database.ExecuteNonQuery(sqlStringCommand, dbTran);
				return num >= 1;
			}
			catch (Exception ex)
			{
				Globals.WriteExceptionLog(ex, null, "CreatOrder");
				return false;
			}
		}

		public bool ExistCountDownOverbBought(OrderInfo orderInfo, DbTransaction dbTran)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("select case when BoughtCount + @quantity >TotalCount then 1 else 0 end as ret from hishop_countdownSku where countDownid=@countDownId and skuid=@skuid");
			base.database.AddInParameter(sqlStringCommand, "quantity", DbType.Int32, orderInfo.LineItems.Values.First().Quantity);
			base.database.AddInParameter(sqlStringCommand, "countDownId", DbType.Int32, orderInfo.CountDownBuyId);
			base.database.AddInParameter(sqlStringCommand, "skuid", DbType.String, orderInfo.LineItems.Values.First().SkuId);
			int num = (dbTran == null) ? ((int)base.database.ExecuteScalar(sqlStringCommand)) : ((int)base.database.ExecuteScalar(sqlStringCommand, dbTran));
			return num > 0;
		}

		public bool ExistCountDownOverbBought(int quantity, int countDownId, string skuId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("select case when BoughtCount + @quantity >TotalCount then 1 else 0 end as ret from hishop_countdownSku where countDownid=@countDownId and skuid=@skuid");
			base.database.AddInParameter(sqlStringCommand, "quantity", DbType.Int32, quantity);
			base.database.AddInParameter(sqlStringCommand, "countDownId", DbType.Int32, countDownId);
			base.database.AddInParameter(sqlStringCommand, "skuid", DbType.String, skuId);
			int num = (int)base.database.ExecuteScalar(sqlStringCommand);
			return num > 0;
		}

		public bool UpdateCountDownBuyNum(OrderInfo orderInfo, DbTransaction dbTran)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE Hishop_CountDownSku SET BoughtCount = BoughtCount + @quantity WHERE countDownid = @countDownId AND skuid = @skuid AND (BoughtCount + @quantity <= TotalCount);");
			base.database.AddInParameter(sqlStringCommand, "quantity", DbType.Int32, orderInfo.LineItems.Values.First().Quantity);
			base.database.AddInParameter(sqlStringCommand, "countDownId", DbType.Int32, orderInfo.CountDownBuyId);
			base.database.AddInParameter(sqlStringCommand, "skuid", DbType.String, orderInfo.LineItems.Values.First().SkuId);
			bool flag = false;
			if (dbTran != null)
			{
				return base.database.ExecuteNonQuery(sqlStringCommand, dbTran) == 1;
			}
			return base.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}

		public bool UpdateOrderOfTakeGoods(OrderInfo order, DbTransaction dbTran = null)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE Hishop_Orders SET  OrderStatus = @OrderStatus, UpdateDate = getdate(), PayDate = @PayDate, ShippingDate = @ShippingDate, FinishDate = @FinishDate,TakeTime = @TakeTime,IsStoreCollect = @IsStoreCollect WHERE OrderId = @OrderId");
			base.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, (int)order.OrderStatus);
			DateTime payDate = order.PayDate;
			if (order.PayDate != DateTime.MinValue)
			{
				base.database.AddInParameter(sqlStringCommand, "PayDate", DbType.DateTime, order.PayDate);
			}
			else
			{
				base.database.AddInParameter(sqlStringCommand, "PayDate", DbType.DateTime, DateTime.Now);
			}
			DateTime shippingDate = order.ShippingDate;
			if (order.ShippingDate != DateTime.MinValue)
			{
				base.database.AddInParameter(sqlStringCommand, "ShippingDate", DbType.DateTime, order.ShippingDate);
			}
			else
			{
				base.database.AddInParameter(sqlStringCommand, "ShippingDate", DbType.DateTime, DateTime.Now);
			}
			base.database.AddInParameter(sqlStringCommand, "FinishDate", DbType.DateTime, DateTime.Now);
			base.database.AddInParameter(sqlStringCommand, "TakeTime", DbType.DateTime, DateTime.Now);
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, order.OrderId);
			base.database.AddInParameter(sqlStringCommand, "IsStoreCollect", DbType.Boolean, order.IsStoreCollect);
			if (dbTran != null)
			{
				return base.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0;
			}
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateOrder(OrderInfo order, DbTransaction dbTran = null)
		{
			string text = "UPDATE Hishop_Orders SET  OrderStatus = @OrderStatus,DadaStatus=@DadaStatus, CloseReason=@CloseReason,UpdateDate=getdate(), PayDate = @PayDate, ShippingDate = @ShippingDate, FinishDate = @FinishDate, RegionId = @RegionId, ShippingRegion = @ShippingRegion, Address = @Address, ZipCode = @ZipCode,ShipTo = @ShipTo, TelPhone = @TelPhone, CellPhone = @CellPhone,LatLng=@LatLng, ShippingModeId=@ShippingModeId ,ModeName=@ModeName, RealShippingModeId = @RealShippingModeId, RealModeName = @RealModeName, ShipOrderNumber = @ShipOrderNumber,  ExpressCompanyName = @ExpressCompanyName,ExpressCompanyAbb = @ExpressCompanyAbb, PaymentTypeId = @PaymentTypeId,PaymentType = @PaymentType, Gateway = @Gateway, ManagerMark = @ManagerMark,ManagerRemark = @ManagerRemark,IsPrinted = @IsPrinted, OrderTotal = @OrderTotal, OrderProfit=@OrderProfit,Amount=@Amount, AdjustedFreight = @AdjustedFreight, AdjustedDiscount = @AdjustedDiscount,OrderPoint = @OrderPoint,GatewayOrderId = @GatewayOrderId,DepositGatewayOrderId = @DepositGatewayOrderId,StoreId=@StoreId,IsStoreCollect=@IsStoreCollect,IsConfirm = @IsConfirm,TakeCode = @TakeCode,FullRegionPath=@FullRegionPath,DepositDate=@DepositDate,Deposit=@Deposit,FinalPayment=@FinalPayment,IDNumber=@IDNumber,IDImage1=@IDImage1,IDImage2=@IDImage2,IDStatus=@IDStatus,IDRemark=@IDRemark,IsincludeCrossBorderGoods=@IsincludeCrossBorderGoods,ShippingId=@ShippingId WHERE OrderId = @OrderId;";
			if (order.ParentOrderId == "-1" && order.PreSaleId <= 0)
			{
				text += "UPDATE Hishop_Orders SET  OrderStatus = @OrderStatus, CloseReason=@CloseReason,UpdateDate=getdate(), PayDate = @PayDate, FinishDate = @FinishDate, PaymentTypeId=@PaymentTypeId,PaymentType = @PaymentType, Gateway = @Gateway, IsConfirm = @IsConfirm,DepositDate=@DepositDate WHERE ParentOrderId = @OrderId";
			}
			if (order.ParentOrderId == "-1" && order.PreSaleId > 0)
			{
				text += "UPDATE Hishop_Orders SET  OrderStatus = @OrderStatus, CloseReason=@CloseReason,UpdateDate=getdate(), PayDate = @PayDate, FinishDate = @FinishDate, PaymentTypeId=@PaymentTypeId,PaymentType = @PaymentType, Gateway = @Gateway, IsConfirm = @IsConfirm,DepositDate = @DepositDate,Deposit= @Deposit,FinalPayment = @FinalPayment WHERE ParentOrderId = @OrderId AND EXISTS (SELECT OrderId FROM Hishop_OrderItems WHERE OrderId=Hishop_Orders.OrderId);";
				if (order.Gifts.Count > 0)
				{
					text += "UPDATE Hishop_Orders SET  OrderStatus = @OrderStatus, CloseReason = @CloseReason,UpdateDate = getdate(), PayDate = @PayDate, FinishDate = @FinishDate, PaymentTypeId = @PaymentTypeId,PaymentType = @PaymentType, Gateway = @Gateway, IsConfirm = @IsConfirm,DepositDate = @DepositDate WHERE ParentOrderId = @OrderId AND NOT EXISTS (SELECT OrderId FROM Hishop_OrderItems WHERE OrderId=Hishop_Orders.OrderId);";
				}
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(text);
			base.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, (int)order.OrderStatus);
			base.database.AddInParameter(sqlStringCommand, "DadaStatus", DbType.Int32, (int)order.DadaStatus);
			base.database.AddInParameter(sqlStringCommand, "CloseReason", DbType.String, order.CloseReason);
			DateTime payDate = order.PayDate;
			if (order.PayDate != DateTime.MinValue)
			{
				base.database.AddInParameter(sqlStringCommand, "PayDate", DbType.DateTime, order.PayDate);
			}
			else
			{
				base.database.AddInParameter(sqlStringCommand, "PayDate", DbType.DateTime, DBNull.Value);
			}
			DateTime shippingDate = order.ShippingDate;
			if (order.ShippingDate != DateTime.MinValue)
			{
				base.database.AddInParameter(sqlStringCommand, "ShippingDate", DbType.DateTime, order.ShippingDate);
			}
			else
			{
				base.database.AddInParameter(sqlStringCommand, "ShippingDate", DbType.DateTime, DBNull.Value);
			}
			DateTime finishDate = order.FinishDate;
			if (order.FinishDate != DateTime.MinValue)
			{
				base.database.AddInParameter(sqlStringCommand, "FinishDate", DbType.DateTime, order.FinishDate);
			}
			else
			{
				base.database.AddInParameter(sqlStringCommand, "FinishDate", DbType.DateTime, DBNull.Value);
			}
			base.database.AddInParameter(sqlStringCommand, "RegionId", DbType.String, order.RegionId);
			base.database.AddInParameter(sqlStringCommand, "ShippingRegion", DbType.String, order.ShippingRegion);
			base.database.AddInParameter(sqlStringCommand, "Address", DbType.String, order.Address);
			base.database.AddInParameter(sqlStringCommand, "ZipCode", DbType.String, order.ZipCode);
			base.database.AddInParameter(sqlStringCommand, "ShipTo", DbType.String, order.ShipTo);
			base.database.AddInParameter(sqlStringCommand, "TelPhone", DbType.String, order.TelPhone);
			base.database.AddInParameter(sqlStringCommand, "CellPhone", DbType.String, order.CellPhone);
			base.database.AddInParameter(sqlStringCommand, "LatLng", DbType.String, order.LatLng);
			base.database.AddInParameter(sqlStringCommand, "ShippingModeId", DbType.Int32, order.ShippingModeId);
			base.database.AddInParameter(sqlStringCommand, "ModeName", DbType.String, order.ModeName);
			base.database.AddInParameter(sqlStringCommand, "RealShippingModeId", DbType.Int32, order.RealShippingModeId);
			base.database.AddInParameter(sqlStringCommand, "RealModeName", DbType.String, order.RealModeName);
			base.database.AddInParameter(sqlStringCommand, "ShipOrderNumber", DbType.String, order.ShipOrderNumber);
			base.database.AddInParameter(sqlStringCommand, "ExpressCompanyName", DbType.String, order.ExpressCompanyName);
			base.database.AddInParameter(sqlStringCommand, "ExpressCompanyAbb", DbType.String, order.ExpressCompanyAbb);
			base.database.AddInParameter(sqlStringCommand, "PaymentTypeId", DbType.Int32, order.PaymentTypeId);
			base.database.AddInParameter(sqlStringCommand, "PaymentType", DbType.String, order.PaymentType);
			base.database.AddInParameter(sqlStringCommand, "Gateway", DbType.String, order.Gateway);
			base.database.AddInParameter(sqlStringCommand, "ManagerMark", DbType.Int32, order.ManagerMark);
			base.database.AddInParameter(sqlStringCommand, "ManagerRemark", DbType.String, order.ManagerRemark);
			base.database.AddInParameter(sqlStringCommand, "IsPrinted", DbType.Boolean, order.IsPrinted);
			base.database.AddInParameter(sqlStringCommand, "OrderTotal", DbType.Currency, order.GetTotal(false));
			base.database.AddInParameter(sqlStringCommand, "OrderProfit", DbType.Currency, order.GetProfit());
			base.database.AddInParameter(sqlStringCommand, "Amount", DbType.Currency, order.GetAmount(false));
			base.database.AddInParameter(sqlStringCommand, "AdjustedFreight", DbType.Currency, order.AdjustedFreight);
			base.database.AddInParameter(sqlStringCommand, "AdjustedDiscount", DbType.Currency, order.AdjustedDiscount);
			base.database.AddInParameter(sqlStringCommand, "OrderPoint", DbType.Int32, order.Points);
			base.database.AddInParameter(sqlStringCommand, "GatewayOrderId", DbType.String, order.GatewayOrderId);
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, order.OrderId);
			base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, order.StoreId);
			base.database.AddInParameter(sqlStringCommand, "IsStoreCollect", DbType.Boolean, order.IsStoreCollect);
			base.database.AddInParameter(sqlStringCommand, "IsConfirm", DbType.Boolean, order.IsConfirm);
			base.database.AddInParameter(sqlStringCommand, "TakeCode", DbType.String, order.TakeCode);
			base.database.AddInParameter(sqlStringCommand, "FullRegionPath", DbType.String, order.FullRegionPath);
			base.database.AddInParameter(sqlStringCommand, "DepositGatewayOrderId", DbType.String, order.DepositGatewayOrderId);
			if (order.DepositDate.HasValue && order.DepositDate != (DateTime?)DateTime.MinValue)
			{
				base.database.AddInParameter(sqlStringCommand, "DepositDate", DbType.DateTime, order.DepositDate);
			}
			else
			{
				base.database.AddInParameter(sqlStringCommand, "DepositDate", DbType.DateTime, DBNull.Value);
			}
			base.database.AddInParameter(sqlStringCommand, "Deposit", DbType.Currency, order.Deposit);
			base.database.AddInParameter(sqlStringCommand, "FinalPayment", DbType.Currency, order.FinalPayment);
			base.database.AddInParameter(sqlStringCommand, "IDNumber", DbType.String, order.IDNumber);
			base.database.AddInParameter(sqlStringCommand, "IDImage1", DbType.String, order.IDImage1);
			base.database.AddInParameter(sqlStringCommand, "IDImage2", DbType.String, order.IDImage2);
			base.database.AddInParameter(sqlStringCommand, "IDStatus", DbType.Int32, order.IDStatus);
			base.database.AddInParameter(sqlStringCommand, "IDRemark", DbType.String, order.IDRemark);
			base.database.AddInParameter(sqlStringCommand, "IsincludeCrossBorderGoods", DbType.Boolean, order.IsincludeCrossBorderGoods);
			base.database.AddInParameter(sqlStringCommand, "ShippingId", DbType.Int32, order.ShippingId);
			if (dbTran != null)
			{
				return base.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0;
			}
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool SetOrderIsStoreCollect(string orderId)
		{
			orderId = base.GetTrueOrderId(orderId);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE Hishop_Orders SET IsStoreCollect = 1 where OrderId = @orderId");
			base.database.AddInParameter(sqlStringCommand, "orderId", DbType.String, orderId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool SetOrderShippingMode(string orderIds, int realShippingModeId, string realModeName)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand($"UPDATE Hishop_Orders SET RealShippingModeId=@RealShippingModeId,RealModeName=@RealModeName WHERE (OrderStatus = 2 OR (OrderStatus = 1 AND Gateway='hishop.plugins.payment.podrequest')) AND OrderId IN ({orderIds})");
			base.database.AddInParameter(sqlStringCommand, "RealShippingModeId", DbType.Int32, realShippingModeId);
			base.database.AddInParameter(sqlStringCommand, "RealModeName", DbType.String, realModeName);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool SetOrderExpressComputerpe(string orderIds, string expressCompanyName, string expressCompanyAbb)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand($"UPDATE Hishop_Orders SET ExpressCompanyName=@ExpressCompanyName,ExpressCompanyAbb=@ExpressCompanyAbb WHERE (OrderStatus = 2 OR (OrderStatus = 1 AND Gateway='hishop.plugins.payment.podrequest')) AND OrderId IN ({orderIds})");
			base.database.AddInParameter(sqlStringCommand, "ExpressCompanyName", DbType.String, expressCompanyName);
			base.database.AddInParameter(sqlStringCommand, "ExpressCompanyAbb", DbType.String, expressCompanyAbb);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public void AddCountDownBoughtCount(OrderInfo orderInfo)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(" ");
			StringBuilder stringBuilder = new StringBuilder();
			foreach (LineItemInfo value in orderInfo.LineItems.Values)
			{
				stringBuilder.Append("UPDATE Hishop_CountDownSku SET BoughtCount = BoughtCount + @BoughtCount WHERE CountDownId = @CountDownBuyId AND SkuId=@SkuId;");
				base.database.AddInParameter(sqlStringCommand, "BoughtCount", DbType.String, value.Quantity);
				base.database.AddInParameter(sqlStringCommand, "CountDownBuyId", DbType.Int32, orderInfo.CountDownBuyId);
				base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, value.SkuId);
			}
			sqlStringCommand.CommandText = stringBuilder.ToString();
			base.database.ExecuteNonQuery(sqlStringCommand);
		}

		public bool UpdatePayOrderStock(string orderId, int storeId = 0, string parentOrderId = null, DbTransaction dbTran = null)
		{
			orderId = base.GetTrueOrderId(orderId);
			string query = "Update Hishop_SKUs Set Stock = CASE WHEN (Stock - (SELECT SUM(oi.ShipmentQuantity) FROM Hishop_OrderItems oi Where oi.SkuId =Hishop_SKUs.SkuId AND OrderId =@OrderId))<=0 Then 0 ELSE Stock - (SELECT SUM(oi.ShipmentQuantity) FROM Hishop_OrderItems oi  Where oi.SkuId = Hishop_SKUs.SkuId AND OrderId =@OrderId) END WHERE Hishop_SKUs.SkuId IN (Select SkuId FROM Hishop_OrderItems Where OrderId =@OrderId)";
			if (parentOrderId != null && parentOrderId == "-1")
			{
				query = "Update Hishop_SKUs Set Stock = CASE WHEN (Stock - (SELECT SUM(oi.ShipmentQuantity) FROM Hishop_OrderItems oi Where oi.SkuId =Hishop_SKUs.SkuId AND OrderId IN(SELECT OrderId FROM Hishop_Orders WHERE ParentOrderId = @OrderId)))<=0 Then 0 ELSE Stock - (SELECT SUM(oi.ShipmentQuantity) FROM Hishop_OrderItems oi  Where oi.SkuId = Hishop_SKUs.SkuId AND OrderId IN(SELECT OrderId FROM Hishop_Orders WHERE ParentOrderId = @OrderId)) END WHERE Hishop_SKUs.SkuId IN (Select SkuId FROM Hishop_OrderItems Where OrderId IN(SELECT OrderId FROM Hishop_Orders WHERE ParentOrderId = @OrderId))";
			}
			else if (storeId > 0)
			{
				query = "Update Hishop_StoreSKUs  Set Stock = CASE WHEN (Stock - (SELECT SUM(oi.ShipmentQuantity) FROM Hishop_OrderItems oi Where oi.SkuId = Hishop_StoreSKUs.SkuId AND OrderId =@OrderId))<=0 Then 0 ELSE Stock - (SELECT SUM(oi.ShipmentQuantity) FROM Hishop_OrderItems oi  Where oi.SkuId = Hishop_StoreSKUs.SkuId AND OrderId =@OrderId) END WHERE Hishop_StoreSKUs.SkuId  IN (Select SkuId FROM Hishop_OrderItems Where OrderId =@OrderId) AND StoreID=@StoreID";
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			if (storeId > 0)
			{
				base.database.AddInParameter(sqlStringCommand, "StoreID", DbType.Int32, storeId);
			}
			if (dbTran != null)
			{
				return base.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0;
			}
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateSendGoodsOrderStock(OrderInfo order, DbTransaction dbTran = null)
		{
			List<StoreStockLogInfo> list = new List<StoreStockLogInfo>();
			if (order == null)
			{
				return false;
			}
			if ((order.ItemStatus != 0 || order.OrderStatus != OrderStatus.SellerAlreadySent) && order.ShippingModeId != -2)
			{
				return false;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (LineItemInfo value in order.LineItems.Values)
			{
				if (value.Status != LineItemStatus.Refunded && value.Status != LineItemStatus.Returned && value.ShipmentQuantity > 0)
				{
					if (order.StoreId > 0)
					{
						stringBuilder.Append(string.Format("UPDATE Hishop_StoreSKUs SET Stock = CASE WHEN (Stock - {0}) < 0 THEN 0 ELSE (Stock - {0}) END WHERE SkuId = '{1}' AND StoreId = {2};", value.ShipmentQuantity, value.SkuId, order.StoreId));
					}
					else
					{
						stringBuilder.Append(string.Format("UPDATE Hishop_Skus SET Stock = CASE WHEN (Stock - {0}) < 0 THEN 0 ELSE (Stock - {0}) END WHERE SkuId = '{1}';", value.ShipmentQuantity, value.SkuId));
					}
				}
			}
			if (!string.IsNullOrEmpty(stringBuilder.ToString()))
			{
				DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
				if (dbTran != null)
				{
					return base.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0;
				}
				return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
			}
			return false;
		}

		public bool UpdateTakeGoodsOrderStock(OrderInfo order, DbTransaction dbTran = null)
		{
			List<StoreStockLogInfo> list = new List<StoreStockLogInfo>();
			if (order == null)
			{
				return false;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (LineItemInfo value in order.LineItems.Values)
			{
				if (value.Status != LineItemStatus.Refunded && value.Status != LineItemStatus.Returned && value.ShipmentQuantity > 0)
				{
					if (order.StoreId > 0)
					{
						stringBuilder.Append(string.Format("UPDATE Hishop_StoreSKUs SET Stock = CASE WHEN (Stock - {0}) < 0 THEN 0 ELSE (Stock - {0}) END WHERE SkuId = '{1}' AND StoreId = {2};", value.ShipmentQuantity, value.SkuId, order.StoreId));
					}
					else
					{
						stringBuilder.Append(string.Format("UPDATE Hishop_Skus SET Stock = CASE WHEN (Stock - {0}) < 0 THEN 0 ELSE (Stock - {0}) END WHERE SkuId = '{1}';", value.ShipmentQuantity, value.SkuId));
					}
				}
			}
			if (!string.IsNullOrEmpty(stringBuilder.ToString()))
			{
				DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
				if (dbTran != null)
				{
					return base.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0;
				}
				return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
			}
			return false;
		}

		public bool UpdateRefundOrderStock(string orderId, int storeId = 0, string SkuId = "", DbTransaction dbTran = null)
		{
			orderId = base.GetTrueOrderId(orderId);
			string str = "Update Hishop_SKUs Set Stock = Stock + (SELECT SUM(oi.ShipmentQuantity) FROM Hishop_OrderItems oi Where oi.SkuId =Hishop_SKUs.SkuId AND OrderId =@OrderId) WHERE ";
			str = (string.IsNullOrEmpty(SkuId) ? (str + "Hishop_SKUs.SkuId IN (Select SkuId FROM Hishop_OrderItems Where OrderId = @OrderId)") : (str + "Hishop_Skus.SkuId = @SkuId"));
			if (storeId > 0)
			{
				str = "Update Hishop_StoreSKUs Set Stock = Stock + (SELECT SUM(oi.ShipmentQuantity) FROM Hishop_OrderItems oi Where oi.SkuId =Hishop_StoreSKUs.SkuId AND OrderId =@OrderId) WHERE ";
				str = (string.IsNullOrEmpty(SkuId) ? (str + "Hishop_StoreSKUs.SkuId IN (Select SkuId FROM Hishop_OrderItems Where OrderId =@OrderId) AND StoreId=@StoreId") : (str + "Hishop_StoreSKUs.SkuId = @SkuId AND StoreId=@StoreId"));
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(str);
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			if (!string.IsNullOrEmpty(SkuId))
			{
				base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, SkuId);
			}
			if (storeId > 0)
			{
				base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, storeId);
			}
			if (dbTran != null)
			{
				return base.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0;
			}
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool CheckRefund(string orderId, string Operator, string adminRemark, int refundType, bool accept)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("UPDATE Hishop_Orders SET OrderStatus = @OrderStatus WHERE OrderId = @OrderId;");
			stringBuilder.Append(" UPDATE Hishop_OrderRefund SET Operator = @Operator,AdminRemark = @AdminRemark,HandleStatus = @HandleStatus,AgreedOrRefusedTime = @HandleTime WHERE HandleStatus = 0 and OrderId = @OrderId;");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			if (accept)
			{
				base.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 9);
				base.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 1);
			}
			else
			{
				base.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 2);
				base.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 2);
			}
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			base.database.AddInParameter(sqlStringCommand, "Operator", DbType.String, Operator);
			base.database.AddInParameter(sqlStringCommand, "AdminRemark", DbType.String, adminRemark);
			base.database.AddInParameter(sqlStringCommand, "HandleTime", DbType.DateTime, DateTime.Now);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public string GetReplaceComments(string orderId)
		{
			orderId = base.GetTrueOrderId(orderId);
			string query = "SELECT Comments FROM Hishop_OrderReplace WHERE HandleStatus = 0 AND OrderId='" + orderId + "'";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			object obj = base.database.ExecuteScalar(sqlStringCommand);
			if (obj == null || obj is DBNull)
			{
				return "";
			}
			return obj.ToString();
		}

		public DataSet GetTradeOrders(OrderQuery query, out int records, bool IsAPI = false)
		{
			DataSet dataSet = new DataSet();
			DbCommand storedProcCommand = base.database.GetStoredProcCommand("cp_API_Orders_Get");
			base.database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, query.PageIndex);
			base.database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, query.PageSize);
			base.database.AddInParameter(storedProcCommand, "IsCount", DbType.Boolean, query.IsCount);
			base.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, this.BuildOrdersQuery(query, IsAPI));
			base.database.AddOutParameter(storedProcCommand, "TotalOrders", DbType.Int32, 4);
			using (dataSet = base.database.ExecuteDataSet(storedProcCommand))
			{
				dataSet.Relations.Add("OrderRelation", dataSet.Tables[0].Columns["OrderId"], dataSet.Tables[1].Columns["OrderId"]);
			}
			records = (int)base.database.GetParameterValue(storedProcCommand, "TotalOrders");
			return dataSet;
		}

		private string BuildOrdersQuery(OrderQuery query, bool IsAPI = false)
		{
			if (query == null)
			{
				throw new ArgumentNullException("query");
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT OrderId FROM Hishop_Orders WHERE 1=1 ");
			if (!string.IsNullOrEmpty(query.OrderId))
			{
				if (query.OrderId.Contains("P"))
				{
					stringBuilder.AppendFormat(" AND ParentOrderId = '{0}'", DataHelper.CleanSearchString(query.OrderId));
				}
				else
				{
					stringBuilder.AppendFormat(" AND OrderId = '{0}'", DataHelper.CleanSearchString(query.OrderId));
				}
			}
			else
			{
				if (query.PaymentType.HasValue)
				{
					stringBuilder.AppendFormat(" AND PaymentTypeId = '{0}'", query.PaymentType.Value);
				}
				if (query.GroupBuyId.HasValue)
				{
					stringBuilder.AppendFormat(" AND GroupBuyId = {0}", query.GroupBuyId.Value);
				}
				if (!string.IsNullOrEmpty(query.ProductName))
				{
					stringBuilder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM Hishop_OrderItems WHERE ItemDescription LIKE '%{0}%')", DataHelper.CleanSearchString(query.ProductName));
				}
				if (!string.IsNullOrEmpty(query.ShipTo))
				{
					stringBuilder.AppendFormat(" AND ShipTo LIKE '%{0}%'", DataHelper.CleanSearchString(query.ShipTo));
				}
				if (query.RegionId.HasValue)
				{
					stringBuilder.AppendFormat(" AND ShippingRegion like '%{0}%'", DataHelper.CleanSearchString(query.FullRegionName));
				}
				if (query.SourceOrder.HasValue)
				{
					stringBuilder.AppendFormat(" AND SourceOrder={0}", query.SourceOrder.Value);
				}
				if (!string.IsNullOrEmpty(query.UserName))
				{
					stringBuilder.AppendFormat(" AND  UserName  = '{0}' ", DataHelper.CleanSearchString(query.UserName));
				}
				if (query.Status == OrderStatus.History)
				{
					stringBuilder.AppendFormat(" AND OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2} AND OrderDate < '{3}'", 1, 4, 9, DateTime.Now.AddMonths(-3));
				}
				else if (query.Status == OrderStatus.BuyerAlreadyPaid)
				{
					stringBuilder.AppendFormat(" AND (OrderStatus = {0} OR (OrderStatus = 1 AND Gateway = 'hishop.plugins.payment.podrequest'))", (int)query.Status);
				}
				else if (query.Status != 0)
				{
					stringBuilder.AppendFormat(" AND OrderStatus = {0}", (int)query.Status);
					if (query.ItemStatus.HasValue)
					{
						stringBuilder.AppendFormat(" And ItemStatus={0}", query.ItemStatus.Value);
					}
				}
				if (IsAPI)
				{
					stringBuilder.AppendFormat(" AND (SupplierId=0 OR SupplierId is NULL)");
				}
				if (query.DataType == 1)
				{
					if (query.StartDate.HasValue)
					{
						stringBuilder.AppendFormat(" AND datediff(ss,'{0}',OrderDate)>=0", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
					}
					if (query.EndDate.HasValue)
					{
						stringBuilder.AppendFormat(" AND datediff(ss,'{0}',OrderDate)<=0", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
					}
				}
				else
				{
					if (query.StartDate.HasValue)
					{
						stringBuilder.AppendFormat(" AND datediff(ss,'{0}',UpdateDate)>=0", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
					}
					if (query.EndDate.HasValue)
					{
						stringBuilder.AppendFormat(" AND datediff(ss,'{0}',UpdateDate)<=0", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
					}
				}
				if (query.ShippingModeId.HasValue)
				{
					stringBuilder.AppendFormat(" AND ShippingModeId = {0}", query.ShippingModeId.Value);
				}
				if (query.IsPrinted.HasValue)
				{
					stringBuilder.AppendFormat(" AND ISNULL(IsPrinted, 0)={0}", query.IsPrinted.Value);
				}
			}
			stringBuilder.Append(" AND ParentOrderId<>'-1'");
			if (!string.IsNullOrEmpty(query.SortBy))
			{
				stringBuilder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
			}
			return stringBuilder.ToString();
		}

		public bool CheckStock(OrderInfo order)
		{
			if (order.LineItems.Count == 0 && order.Gifts.Count != 0)
			{
				return true;
			}
			if (order == null || order.LineItems.Count == 0)
			{
				return false;
			}
			string text = "";
			foreach (LineItemInfo value in order.LineItems.Values)
			{
				if (value.Status != LineItemStatus.Refunded && value.ShipmentQuantity > 0)
				{
					DbCommand sqlStringCommand = base.database.GetSqlStringCommand("select count(*) FROM Hishop_Skus WHERE SkuId = @SkuId");
					base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, value.SkuId);
					if ((int)base.database.ExecuteScalar(sqlStringCommand) > 0)
					{
						bool num = order.StoreId > 0 || order.ShippingModeId == -1;
						text = (sqlStringCommand.CommandText = ((!num) ? $"SELECT (ISNULL(SUM(Stock),0) - {value.ShipmentQuantity}) AS stock  FROM Hishop_Skus WHERE SkuId = @SkuId AND ProductId = @ProductId" : ((order.StoreId <= 0) ? $"SELECT (ISNULL(SUM(Stock),0) - {value.ShipmentQuantity}) AS stock FROM Hishop_StoreSKUs WHERE SkuId = @SkuId  AND ProductId = @ProductId" : $"SELECT (ISNULL(SUM(Stock),0) - {value.ShipmentQuantity}) AS stock FROM Hishop_StoreSKUs WHERE SkuId = @SkuId AND StoreId = @StoreId AND ProductId = @ProductId")));
						sqlStringCommand.Parameters.Clear();
						base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, value.ProductId);
						base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, value.SkuId);
						base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, order.StoreId);
						if ((int)base.database.ExecuteScalar(sqlStringCommand) < 0)
						{
							return false;
						}
					}
				}
			}
			return true;
		}

		public IList<LineItemInfo> GetNoStockItems(OrderInfo order)
		{
			string text = "";
			IList<LineItemInfo> list = null;
			foreach (LineItemInfo value in order.LineItems.Values)
			{
				if (value.Status != LineItemStatus.Refunded && value.ShipmentQuantity > 0)
				{
					text = ((order.StoreId <= 0 && order.ShippingModeId != -1) ? $"SELECT (ISNULL(SUM(Stock),0) - {value.ShipmentQuantity}) AS stock  FROM Hishop_Skus WHERE SkuId = @SkuId AND ProductId = @ProductId" : ((order.StoreId <= 0) ? $"SELECT (ISNULL(SUM(Stock),0) - {value.ShipmentQuantity}) AS stock FROM Hishop_StoreSKUs WHERE SkuId = @SkuId  AND ProductId = @ProductId" : $"SELECT (ISNULL(SUM(Stock),0) - {value.ShipmentQuantity}) AS stock FROM Hishop_StoreSKUs WHERE SkuId = @SkuId AND StoreId = @StoreId AND ProductId = @ProductId"));
					DbCommand sqlStringCommand = base.database.GetSqlStringCommand(text);
					base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, value.ProductId);
					base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, value.SkuId);
					base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, order.StoreId);
					if ((int)base.database.ExecuteScalar(sqlStringCommand) < 0)
					{
						if (list == null)
						{
							list = new List<LineItemInfo>();
						}
						list.Add(value);
					}
				}
			}
			return list;
		}

		public string GetAdminShipAddres()
		{
			string result = "";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT TOP 1 AdminShipAddress FROM Hishop_OrderReturns WHERE StoreId = 0 AND AdminShipAddress IS NOT NULL AND AdminShipAddress<>'' ORDER BY ReturnId DESC  ");
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = (string)((IDataRecord)dataReader)["AdminShipAddress"];
				}
			}
			sqlStringCommand = base.database.GetSqlStringCommand("SELECT TOP 1 AdminShipAddress FROM Hishop_OrderReplace WHERE StoreId = 0 AND AdminShipAddress IS NOT NULL AND AdminShipAddress<>'' ORDER BY ReplaceId DESC  ");
			using (IDataReader dataReader2 = base.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader2.Read())
				{
					result = (string)((IDataRecord)dataReader2)["AdminShipAddress"];
				}
			}
			return result;
		}

		public bool IsExistOuterOrder(string outerOrderId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT COUNT(OrderId) FROM Hishop_Orders WHERE OuterOrderId = @OuterOrderId  ");
			base.database.AddInParameter(sqlStringCommand, "OuterOrderId", DbType.String, outerOrderId);
			return int.Parse(base.database.ExecuteScalar(sqlStringCommand).ToNullString()) > 0;
		}

		public DataSet GetWaitReviewOrderIds(int userId = 0, string orderId = "")
		{
			orderId = base.GetTrueOrderId(orderId);
			base.database = DatabaseFactory.CreateDatabase();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT O.OrderId FROM Hishop_Orders O LEFT JOIN Hishop_OrderItems OI ON OI.OrderId = O.OrderId LEFT JOIN Hishop_ProductReviews PR ON OI.OrderId = PR.OrderId AND OI.SkuId = PR.SkuId  WHERE O.ParentOrderId<>'-1' AND (O.OrderStatus = '" + 5 + "' OR O.OrderStatus = '" + 4 + "')");
			if (userId > 0)
			{
				stringBuilder.Append(" AND O.UserId = '" + userId + "' ");
			}
			if (!string.IsNullOrEmpty(orderId))
			{
				stringBuilder.Append(" AND O.OrderId = '" + orderId + "' ");
			}
			stringBuilder.Append(" AND PR.ReviewId IS NULL GROUP BY O.OrderId");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			return base.database.ExecuteDataSet(sqlStringCommand);
		}

		public List<string> GetCommentServiceOrderIds(IEnumerable<string> orderIds, bool canComment)
		{
			List<string> list = new List<string>();
			if (orderIds != null && orderIds.Count() > 0)
			{
				base.database = DatabaseFactory.CreateDatabase();
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("SELECT O.OrderId FROM Hishop_Orders O ").Append(" INNER JOIN Hishop_OrderVerificationItems as OIV On OIV.OrderId = O.OrderId ").Append(" AND OIV.VerificationStatus = " + 1.GetHashCode() + " ")
					.Append(" AND OIV.OrderId in(" + string.Join(",", orderIds.ToArray()) + ") ")
					.Append(" INNER JOIN Hishop_OrderItems OI ON OI.OrderId = O.OrderId ")
					.Append(" LEFT JOIN Hishop_ProductReviews PR ON OI.OrderId = PR.OrderId AND OI.SkuId = PR.SkuId  ")
					.Append(" WHERE O.OrderType=" + 6.GetHashCode() + " ");
				if (canComment)
				{
					stringBuilder.Append(" AND PR.ReviewId IS NULL ");
				}
				else
				{
					stringBuilder.Append(" AND PR.ReviewId IS NOT NULL ");
				}
				stringBuilder.Append("GROUP BY O.OrderId");
				DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
				DataSet dataSet = base.database.ExecuteDataSet(sqlStringCommand);
				if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
				{
					foreach (DataRow row in dataSet.Tables[0].Rows)
					{
						list.Add(row[0].ToNullString());
					}
				}
			}
			return (from d in list
			where !string.IsNullOrWhiteSpace(d)
			select d).ToList();
		}

		public IList<string> GetStoreFinishOrderIds(int storeId, int endOrderDays)
		{
			IList<string> list = new List<string>();
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT OrderId FROM Hishop_Orders WHERE OrderStatus = @OrderStatus AND StoreId = @StoreId AND ParentOrderId<>'-1' AND FinishDate <= '" + DateTime.Now.AddDays((double)(-endOrderDays)) + "'");
			base.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 5);
			base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, storeId);
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(((IDataRecord)dataReader)[0].ToNullString());
				}
			}
			return list;
		}

		public bool UpdateOrderPayInfo(OrderInfo order)
		{
			string str = "";
			str = ((!(order.ParentOrderId == "-1")) ? (str + "UPDATE Hishop_Orders SET Gateway = @Gateway,PayRandCode = @PayRandCode,PaymentTypeId = @PaymentTypeId,PaymentType = @PaymentType,IsParentOrderPay=0 WHERE OrderId = @OrderId;") : (str + "UPDATE Hishop_Orders SET Gateway = @Gateway,PayRandCode = @PayRandCode,PaymentTypeId = @PaymentTypeId,PaymentType = @PaymentType,IsParentOrderPay=1 WHERE OrderId = @OrderId OR ParentOrderId = @OrderId;"));
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(str);
			base.database.AddInParameter(sqlStringCommand, "Gateway", DbType.String, order.Gateway);
			base.database.AddInParameter(sqlStringCommand, "PayRandCode", DbType.String, order.PayRandCode);
			base.database.AddInParameter(sqlStringCommand, "PaymentTypeId", DbType.String, order.PaymentTypeId);
			base.database.AddInParameter(sqlStringCommand, "PaymentType", DbType.String, order.PaymentType);
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, order.OrderId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public string GetOrderIdsByParent(string parentOrderId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("Select OrderId FROM Hishop_Orders WHERE ParentOrderId = @ParentOrderId");
			base.database.AddInParameter(sqlStringCommand, "ParentOrderId", DbType.String, parentOrderId);
			string text = "";
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					text = text + ((IDataRecord)dataReader)["OrderId"].ToString() + ",";
				}
			}
			return text.Substring(0, text.Length - 1);
		}

		public PageModel<AfterSaleRecordModel> GetAfterSalesList(AfterSalesQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder(" 1 = 1 ");
			if (query.AfterSaleType.HasValue)
			{
				stringBuilder.AppendFormat(" AND AfterSaleType = {0}", query.AfterSaleType.Value);
			}
			else if (query.MoreAfterSaleType != null && query.MoreAfterSaleType.Count() > 0)
			{
				stringBuilder.AppendFormat(" AND AfterSaleType  IN(" + string.Join(",", query.MoreAfterSaleType.ToArray()) + ")");
			}
			if (query.Status.Count > 0)
			{
				stringBuilder.AppendFormat(" AND HandleStatus  IN(" + string.Join(",", query.Status.ToArray()) + ")");
			}
			if (query.StoreId > 0)
			{
				stringBuilder.AppendFormat(" AND StoreId = {0}", query.StoreId);
			}
			if (query.UserId > 0)
			{
				stringBuilder.AppendFormat(" AND UserId = {0}", query.StoreId);
			}
			string sortBy = "HandleTime";
			if (!string.IsNullOrEmpty(query.SortBy))
			{
				sortBy = query.SortBy;
			}
			SortAction sortOrder = SortAction.Desc;
			return DataHelper.PagingByRownumber<AfterSaleRecordModel>(query.PageIndex, query.PageSize, sortBy, sortOrder, true, "vw_Hishop_AfterSaleRecords", "KeyId", stringBuilder.ToString(), "*");
		}

		public IList<LineItemInfo> GetOrderItems(string orderId, string skuId = "")
		{
			orderId = base.GetTrueOrderId(orderId);
			string query = "SELECT * FROM Hishop_OrderItems WHERE OrderId = @OrderId;";
			if (!string.IsNullOrEmpty(skuId))
			{
				query = "SELECT * FROM Hishop_OrderItems WHERE OrderId = @OrderId AND SkuId = @SkuId;";
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			if (!string.IsNullOrEmpty(skuId))
			{
				base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
			}
			IList<LineItemInfo> list = new List<LineItemInfo>();
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					LineItemInfo item = DataMapper.PopulateLineItem(dataReader);
					list.Add(item);
				}
			}
			return list;
		}

		public IList<OrderVerificationItemInfo> GetOrderVerificationItems(string orderId)
		{
			orderId = base.GetTrueOrderId(orderId);
			string query = "SELECT * FROM Hishop_OrderVerificationItems WHERE OrderId = @OrderId;";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			IList<OrderVerificationItemInfo> result = new List<OrderVerificationItemInfo>();
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<OrderVerificationItemInfo>(objReader);
			}
			return result;
		}

		public StoreSalesStatisticsModel GetStoreSaleStatistics(int storeId, DateTime startTime, DateTime endTime)
		{
			StoreSalesStatisticsModel storeSalesStatisticsModel = new StoreSalesStatisticsModel();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT ISNULL(SUM(OrderTotal - ISNULL(RefundAmount,0)),0) AS SaleAmount FROM Hishop_Orders WHERE StoreId = {0} AND (OrderStatus NOT IN(1,4) OR (OrderStatus = 4 AND PayDate IS NOT NULL)) AND OrderDate BETWEEN '{1}' AND '{2}';", storeId, startTime, endTime);
			stringBuilder.AppendFormat("SELECT COUNT(OrderId) AS OrderCount FROM Hishop_Orders WHERE StoreId = {0} AND OrderDate BETWEEN '{1}' AND '{2}';", storeId, startTime, endTime);
			stringBuilder.AppendFormat("SELECT COUNT(OrderId) AS PayOrderCount FROM Hishop_Orders WHERE StoreId = {0} AND (OrderStatus NOT IN(1,4) OR (OrderStatus = 4 AND PayDate IS NOT NULL)) AND OrderDate BETWEEN '{1}' AND '{2}';", storeId, startTime, endTime);
			using (IDataReader dataReader = base.database.ExecuteReader(CommandType.Text, stringBuilder.ToString()))
			{
				if (dataReader.Read())
				{
					storeSalesStatisticsModel.SaleAmount = ((IDataRecord)dataReader)["SaleAmount"].ToDecimal(0);
				}
				dataReader.NextResult();
				if (dataReader.Read())
				{
					storeSalesStatisticsModel.OrderCount = ((IDataRecord)dataReader)["OrderCount"].ToInt(0);
				}
				dataReader.NextResult();
				if (dataReader.Read())
				{
					storeSalesStatisticsModel.PayOrderCount = ((IDataRecord)dataReader)["PayOrderCount"].ToInt(0);
				}
			}
			return storeSalesStatisticsModel;
		}

		public IList<StoreDaySaleAmountModel> GetStoreSaleAmountOfDay(int storeId, DateTime startTime, DateTime endTime)
		{
			IList<StoreDaySaleAmountModel> list = new List<StoreDaySaleAmountModel>();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT (OrderTotal - ISNULL(RefundAmount,0)) AS SaleAmount,OrderDate FROM Hishop_Orders WHERE StoreId = {0} AND (OrderStatus NOT IN(1,4) OR (OrderStatus = 4 AND PayDate IS NOT NULL)) AND OrderDate BETWEEN '{1}' AND '{2}' ORDER BY OrderDate ASC;", storeId, startTime, endTime);
			IList<StoreDaySaleAmountModel> list2 = new List<StoreDaySaleAmountModel>();
			using (IDataReader objReader = base.database.ExecuteReader(CommandType.Text, stringBuilder.ToString()))
			{
				list2 = DataHelper.ReaderToList<StoreDaySaleAmountModel>(objReader);
			}
			IList<StoreDaySaleAmountModel> list3 = new List<StoreDaySaleAmountModel>();
			if (list2 == null)
			{
				list2 = new List<StoreDaySaleAmountModel>();
			}
			for (int i = 0; i <= (endTime - startTime).Days; i++)
			{
				StoreDaySaleAmountModel storeDaySaleAmountModel = new StoreDaySaleAmountModel();
				DateTime currDate = startTime.AddDays((double)i).Date;
				storeDaySaleAmountModel.OrderDate = currDate;
				list3 = (from sa in list2
				where sa.OrderDate >= currDate && sa.OrderDate < currDate.AddDays(1.0)
				select sa).ToList();
				if (list3 != null && list3.Count > 0)
				{
					storeDaySaleAmountModel.SaleAmount = (from sa in list2
					where sa.OrderDate >= currDate && sa.OrderDate < currDate.AddDays(1.0)
					select sa).Sum((StoreDaySaleAmountModel sa) => sa.SaleAmount);
				}
				else
				{
					storeDaySaleAmountModel.SaleAmount = decimal.Zero;
				}
				list.Add(storeDaySaleAmountModel);
			}
			return list;
		}

		public OrderVerificationItemInfo GetVerificationInfoByPassword(string verificationPassword)
		{
			OrderVerificationItemInfo result = null;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM Hishop_OrderVerificationItems Where VerificationPassword = @verificationPassword; ");
			base.database.AddInParameter(sqlStringCommand, "verificationPassword", DbType.String, verificationPassword);
			PromotionDao promotionDao = new PromotionDao();
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToModel<OrderVerificationItemInfo>(objReader);
			}
			return result;
		}

		public DataTable GetOrderInputItem(string orderId)
		{
			orderId = base.GetTrueOrderId(orderId);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM Hishop_OrderInputItems Where OrderId = @OrderId");
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			DataSet dataSet = base.database.ExecuteDataSet(sqlStringCommand);
			return dataSet.Tables[0];
		}

		public DataTable GetVerificationItem(string orderId)
		{
			orderId = base.GetTrueOrderId(orderId);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM Hishop_OrderVerificationItems v\r\n            inner join Hishop_OrderItems o on v.OrderId=o.OrderId and v.SkuId=o.SkuId \r\n            left join Hishop_Stores s on v.StoreId=s.StoreId\r\n            Where v.OrderId = @OrderId");
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			DataSet dataSet = base.database.ExecuteDataSet(sqlStringCommand);
			return dataSet.Tables[0];
		}

		private IList<OrderVerificationItemInfo> GetVerificationItem(string orderId, string skuId)
		{
			orderId = base.GetTrueOrderId(orderId);
			IList<OrderVerificationItemInfo> result = new List<OrderVerificationItemInfo>();
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM Hishop_OrderVerificationItems Where OrderId = @OrderId and SkuId=@SkuId; ");
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
			PromotionDao promotionDao = new PromotionDao();
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<OrderVerificationItemInfo>(objReader);
			}
			return result;
		}

		public OrderInfo GetServiceProductOrderInfo(string orderId)
		{
			if (string.IsNullOrEmpty(orderId))
			{
				return null;
			}
			orderId = base.GetTrueOrderId(orderId);
			OrderInfo orderInfo = null;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT o.*,isnull(StoreName,'平台') as StoreName FROM Hishop_Orders o left join Hishop_Stores s on o.StoreId=s.StoreId Where OrderId = @OrderId;  SELECT * FROM Hishop_OrderItems Where OrderId = @OrderId;Select * from Hishop_OrderGifts where OrderID=@OrderID;Select * from Hishop_OrderInputItems where OrderID=@OrderID order by InputFieldGroup;");
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			PromotionDao promotionDao = new PromotionDao();
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					orderInfo = DataMapper.PopulateOrder(dataReader);
					if (DBNull.Value != ((IDataRecord)dataReader)["StoreName"])
					{
						orderInfo.StoreName = ((IDataRecord)dataReader)["StoreName"].ToNullString();
					}
				}
				dataReader.NextResult();
				while (dataReader.Read())
				{
					LineItemInfo lineItemInfo = DataMapper.PopulateLineItem(dataReader);
					if (lineItemInfo.PromotionId > 0)
					{
						PromotionInfo promotion = promotionDao.GetPromotion(lineItemInfo.PromotionId);
						if (promotion != null)
						{
							lineItemInfo.PromoteType = promotion.PromoteType;
						}
					}
					IList<OrderVerificationItemInfo> verificationItem = this.GetVerificationItem(lineItemInfo.OrderId, lineItemInfo.SkuId);
					if (verificationItem != null)
					{
						for (int i = 0; i < verificationItem.Count; i++)
						{
							lineItemInfo.VerificationItems.Add(verificationItem[i]);
						}
					}
					orderInfo.LineItems.Add((string)((IDataRecord)dataReader)["SkuId"], lineItemInfo);
				}
				dataReader.NextResult();
				while (dataReader.Read())
				{
					OrderGiftInfo item = DataMapper.PopulateOrderGift(dataReader);
					orderInfo.Gifts.Add(item);
				}
				dataReader.NextResult();
				while (dataReader.Read())
				{
					OrderInputItemInfo item2 = DataMapper.PopulateInputItem(dataReader);
					orderInfo.InputItems.Add(item2);
				}
			}
			return orderInfo;
		}

		public int GetCanRefundQuantity(string orderId, bool canOverRefund)
		{
			orderId = base.GetTrueOrderId(orderId);
			string text = "";
			text = ((!canOverRefund) ? $"SELECT COUNT(Id) FROM Hishop_OrderVerificationItems WHERE OrderId = @OrderId AND (VerificationStatus = {0})" : $"SELECT COUNT(Id) FROM Hishop_OrderVerificationItems WHERE OrderId = @OrderId AND (VerificationStatus = {0} OR  VerificationStatus = {3})");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(text);
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0);
		}

		public DbQueryResult GetFinishedVerificationRecord(Pagination page, int storeId, string keyword, int managerId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("VerificationDate is not null ");
			stringBuilder.AppendFormat(" and storeId= " + storeId);
			if (!string.IsNullOrEmpty(keyword))
			{
				stringBuilder.AppendFormat(" and ( t.OrderId='{0}' or charindex(','+ltrim('{0}')+',',','+VerificationPasswords+',' )>0) ", keyword);
			}
			if (managerId > 0)
			{
				stringBuilder.AppendFormat(" and ManagerId= " + managerId);
			}
			string table = "Hishop_OrderItems i inner join (SELECT StoreId,OrderId,SkuId,ManagerId,UserName,VerificationDate,COUNT(*) num,(select VerificationPassword+',' from  Hishop_OrderVerificationItems s where s.OrderId=t.OrderId and s.VerificationDate=t.VerificationDate FOR XML PATH('')) as VerificationPasswords FROM Hishop_OrderVerificationItems t group by StoreId,OrderId,SkuId,ManagerId,UserName,VerificationDate) t on t.OrderId=i.OrderId and t.SkuId=i.SkuId";
			string selectFields = "ItemDescription,ThumbnailsUrl,SKUContent,ItemAdjustedPrice,StoreId,t.OrderId,t.SkuId,ManagerId,UserName,VerificationDate,num,VerificationPasswords ";
			return DataHelper.PagingByTopsort(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, table, "t.OrderId", stringBuilder.ToString(), selectFields);
		}

		public bool SetOrderRefuned(string orderId)
		{
			orderId = base.GetTrueOrderId(orderId);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE Hishop_Orders SET OrderStatus = @OrderStatus WHERE OrderId = @OrderId;UPDATE Hishop_OrderItems SET Status = @Status WHERE OrderId = @OrderId;");
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			base.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 4);
			base.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, 11);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public int CheckValidCodeForRefund(string orderId, string validCode)
		{
			orderId = base.GetTrueOrderId(orderId);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT COUNT(Id) FROM  Hishop_OrderVerificationItems WHERE OrderId = @OrderId AND VerificationPassword IN(" + validCode + ");");
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0);
		}

		public bool SetOrderVerificationItemStatus(string orderId, string validCodes, VerificationStatus status)
		{
			orderId = base.GetTrueOrderId(orderId);
			string text = "";
			switch (status)
			{
			case VerificationStatus.Finished:
				text = "UPDATE Hishop_OrderVerificationItems SET VerificationStatus = @VerificationStatus,VerificationDate = @VerificationDate WHERE OrderId = @OrderId AND VerificationPassword IN(" + validCodes + ");";
				break;
			case VerificationStatus.Refunded:
				text = "UPDATE Hishop_OrderVerificationItems SET VerificationStatus = @VerificationStatus,RefundDate = @RefundDate WHERE OrderId = @OrderId AND VerificationPassword IN(" + validCodes + ");";
				break;
			default:
				text = "UPDATE Hishop_OrderVerificationItems SET VerificationStatus = @VerificationStatus WHERE OrderId = @OrderId AND VerificationPassword IN(" + validCodes + ");";
				break;
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(text);
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			base.database.AddInParameter(sqlStringCommand, "VerificationStatus", DbType.Int32, (int)status);
			base.database.AddInParameter(sqlStringCommand, "VerificationDate", DbType.DateTime, DateTime.Now);
			base.database.AddInParameter(sqlStringCommand, "RefundDate", DbType.DateTime, DateTime.Now);
			int num = base.database.ExecuteNonQuery(sqlStringCommand);
			return num > 0;
		}

		public bool IsVerificationFinished(string orderId)
		{
			orderId = base.GetTrueOrderId(orderId);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT COUNT(*) FROM Hishop_OrderVerificationItems WHERE OrderId = @OrderId AND (VerificationStatus = @Applied  OR VerificationStatus = @ApplyRefund)");
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			base.database.AddInParameter(sqlStringCommand, "Applied", DbType.Int32, 0);
			base.database.AddInParameter(sqlStringCommand, "ApplyRefund", DbType.Int32, 4);
			int num = (int)base.database.ExecuteScalar(sqlStringCommand);
			return num == 0;
		}

		public bool IsExistVerificationPassword(string password)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT COUNT(*) FROM Hishop_OrderVerificationItems WHERE VerificationPassword=@VerificationPassword");
			base.database.AddInParameter(sqlStringCommand, "VerificationPassword", DbType.String, password);
			int num = (int)base.database.ExecuteScalar(sqlStringCommand);
			return num > 0;
		}

		public string GetVerificationPasswordsOfOrderId(string orderId)
		{
			orderId = base.GetTrueOrderId(orderId);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT VerificationPassword FROM Hishop_OrderVerificationItems WHERE OrderId=@OrderId");
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			IList<string> list = new List<string>();
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(((IDataRecord)dataReader)[0].ToNullString());
				}
			}
			return string.Join(",", list);
		}

		public DbQueryResult GetVerificationRecord(VerificationRecordQuery query)
		{
			query.OrderId = base.GetTrueOrderId(query.OrderId);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1 ");
			if (query.StoreId.HasValue && query.StoreId > -1)
			{
				stringBuilder.Append(" AND Vi.StoreId=" + query.StoreId.Value + " ");
			}
			if (query.ManagerId.HasValue)
			{
				stringBuilder.Append(" AND ManagerId=" + query.ManagerId.Value + " ");
			}
			if (query.Status.HasValue)
			{
				stringBuilder.Append(" AND VerificationStatus=" + query.Status.Value.GetHashCode() + " ");
			}
			if (!string.IsNullOrWhiteSpace(query.Code))
			{
				stringBuilder.Append(" AND VerificationPassword like '%" + query.Code + "%' ");
			}
			if (!string.IsNullOrWhiteSpace(query.OrderId))
			{
				stringBuilder.Append(" AND OrderId='" + query.OrderId + "' ");
			}
			DateTime value;
			if (query.StartVerificationDate.HasValue)
			{
				StringBuilder stringBuilder2 = stringBuilder;
				value = query.StartVerificationDate.Value;
				stringBuilder2.Append(" AND DATEDIFF(dd,VerificationDate,'" + value.ToString("yyyy-MM-dd") + "')<= 0 ");
			}
			if (query.EndVerificationDate.HasValue)
			{
				StringBuilder stringBuilder3 = stringBuilder;
				value = query.EndVerificationDate.Value;
				stringBuilder3.Append(" AND DATEDIFF(dd,VerificationDate,'" + value.ToString("yyyy-MM-dd") + "')>= 0 ");
			}
			if (query.StartCreateDate.HasValue)
			{
				StringBuilder stringBuilder4 = stringBuilder;
				value = query.StartCreateDate.Value;
				stringBuilder4.Append(" AND DATEDIFF(dd,vi.CreateDate,'" + value.ToString("yyyy-MM-dd") + "')<= 0 ");
			}
			if (query.EndCreateDate.HasValue)
			{
				StringBuilder stringBuilder5 = stringBuilder;
				value = query.EndCreateDate.Value;
				stringBuilder5.Append(" AND DATEDIFF(dd,vi.CreateDate,'" + value.ToString("yyyy-MM-dd") + "')>= 0 ");
			}
			if (string.IsNullOrWhiteSpace(query.SortBy))
			{
				query.SortBy = " Id";
			}
			string table = "Hishop_OrderVerificationItems as vi left join Hishop_Stores as s on vi.StoreId= s.StoreId left JOIN aspnet_Managers as m on m.ManagerId= vi.ManagerId";
			string selectFields = "vi.Id,vi.OrderId,vi.SkuId,vi.StoreId,vi.VerificationPassword,vi.VerificationDate,vi.VerificationStatus,vi.ManagerId,vi.UserName,vi.RefundDate,vi.CreateDate,ISNULL(s.StoreName,'平台') as StoreName,m.UserName as ManagerName";
			return DataHelper.PagingByTopsort(query.PageIndex, query.PageSize, query.SortBy, SortAction.Desc, query.IsCount, table, "Id", stringBuilder.ToString(), selectFields);
		}

		public string GetPayOrderId(string orderId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT (OrderId + ISNULL(PayRandCode,'')) AS PayOrderId FROM Hishop_Orders WHERE OrderId = @OrderId");
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					return ((IDataRecord)dataReader)[0].ToNullString();
				}
				return "";
			}
		}

		public InvoiceInfo GetLastInvoiceInfo(int userId)
		{
			InvoiceInfo invoiceInfo = null;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT TOP 1 InvoiceTitle,InvoiceType,InvoiceTaxpayerNumber FROM Hishop_Orders WHERE len(InvoiceTitle) > 0 AND UserId = @UserId order by OrderDate desc");
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					invoiceInfo = new InvoiceInfo();
					invoiceInfo.InvoiceTitle = ((IDataRecord)dataReader)["InvoiceTitle"].ToNullString();
					invoiceInfo.InvoiceType = (InvoiceType)((IDataRecord)dataReader)["InvoiceType"].ToInt(0);
					invoiceInfo.InvoiceTaxpayerNumber = ((IDataRecord)dataReader)["InvoiceTaxpayerNumber"].ToNullString();
				}
			}
			return invoiceInfo;
		}

		public int GetVerificationQuantityOfServiceOrder(string orderId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT COUNT(Id) FROM Hishop_OrderVerificationItems WHERE OrderId = @OrderId AND (VerificationStatus = @VerificationStatus OR VerificationStatus = @VerificationStatus1)");
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			Database database = base.database;
			DbCommand command = sqlStringCommand;
			VerificationStatus verificationStatus = VerificationStatus.Finished;
			database.AddInParameter(command, "VerificationStatus", DbType.Int32, verificationStatus.GetHashCode());
			Database database2 = base.database;
			DbCommand command2 = sqlStringCommand;
			verificationStatus = VerificationStatus.Expired;
			database2.AddInParameter(command2, "VerificationStatus1", DbType.Int32, verificationStatus.GetHashCode());
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0);
		}

		public bool UpdateOrderGatewayOrderId(string orderId, string gatewayOrderId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE Hishop_Orders SET GatewayOrderId = @GatewayOrderId WHERE OrderId = @OrderId; ");
			base.database.AddInParameter(sqlStringCommand, "GatewayOrderId", DbType.String, gatewayOrderId);
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public PageModel<OrderModel> GetOrderList(OrderQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder("1=1 ");
			int num;
			if (!query.IsAllOrder)
			{
				int? supplierId = query.SupplierId;
				num = -1;
				if (supplierId == num)
				{
					stringBuilder.AppendFormat(" AND SupplierId>0");
				}
				else
				{
					stringBuilder.AppendFormat(" AND SupplierId={0}", query.SupplierId);
				}
			}
			if (!string.IsNullOrEmpty(query.OrderId))
			{
				if (query.OrderId.Contains("P"))
				{
					stringBuilder.AppendFormat(" AND (ParentOrderId = '{0}' OR (ParentOrderId + PayRandCode) = '{0}')", DataHelper.CleanSearchString(query.OrderId));
				}
				else
				{
					stringBuilder.AppendFormat(" AND (OrderId = '{0}' OR (OrderId + PayRandCode) = '{0}')", DataHelper.CleanSearchString(query.OrderId));
				}
				if (query.StoreId.HasValue)
				{
					if (query.StoreId.Value == 0)
					{
						stringBuilder.Append(" AND (StoreId = 0 or StoreId is null or StoreId='')");
					}
					else
					{
						stringBuilder.AppendFormat(" AND StoreId = {0}", query.StoreId.Value);
					}
				}
			}
			else
			{
				OrderType? type = query.Type;
				if (type.HasValue)
				{
					type = query.Type;
					if (type.Value == OrderType.GroupOrder)
					{
						stringBuilder.Append(" And GroupBuyId > 0 ");
					}
					else
					{
						stringBuilder.Append(" And GroupBuyId is null ");
					}
					StringBuilder stringBuilder2 = stringBuilder;
					type = query.Type;
					stringBuilder2.Append(" And OrderType =" + type.GetHashCode() + " ");
				}
				OrderType orderType;
				if (query.IsServiceOrder.HasValue && !query.IsServiceOrder.Value)
				{
					StringBuilder stringBuilder3 = stringBuilder;
					orderType = OrderType.ServiceOrder;
					stringBuilder3.Append(" And OrderType <>" + orderType.GetHashCode() + " ");
				}
				if (query.UserId.HasValue)
				{
					stringBuilder.AppendFormat(" AND UserId = {0}", query.UserId.Value);
				}
				if (query.PaymentType.HasValue)
				{
					stringBuilder.AppendFormat(" AND PaymentTypeId = '{0}'", query.PaymentType.Value);
				}
				if (query.GroupBuyId.HasValue)
				{
					stringBuilder.AppendFormat(" AND GroupBuyId = {0}", query.GroupBuyId.Value);
				}
				if (!string.IsNullOrEmpty(query.ProductName))
				{
					if (query.ProductName.Trim().Contains(" "))
					{
						StringBuilder stringBuilder4 = new StringBuilder();
						string[] array = query.ProductName.Trim().Split(' ');
						int num2 = 0;
						string[] array2 = array;
						foreach (string text in array2)
						{
							if (!string.IsNullOrEmpty(text.Trim()))
							{
								if (num2 == 0)
								{
									stringBuilder4.Append($" ItemDescription LIKE '%{DataHelper.CleanSearchString(text)}%'");
								}
								else
								{
									stringBuilder4.Append($" OR ItemDescription LIKE '%{DataHelper.CleanSearchString(text)}%'");
								}
								num2++;
							}
						}
						stringBuilder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM Hishop_OrderItems WHERE ({0}))", stringBuilder4.ToString());
					}
					else
					{
						stringBuilder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM Hishop_OrderItems WHERE ItemDescription LIKE '%{0}%')", DataHelper.CleanSearchString(query.ProductName));
					}
				}
				if (!string.IsNullOrEmpty(query.ShipTo))
				{
					stringBuilder.AppendFormat(" AND ShipTo LIKE '%{0}%'", DataHelper.CleanSearchString(query.ShipTo));
				}
				if (query.RegionId.HasValue)
				{
					stringBuilder.AppendFormat(" AND (ShippingRegion like '%{0}%' OR ','+FullRegionPath+',' like '%{1}%')", DataHelper.CleanSearchString(query.FullRegionName), "," + query.RegionId + ",");
				}
				if (!string.IsNullOrEmpty(query.UserName))
				{
					stringBuilder.AppendFormat(" AND  UserName like '%{0}%' ", DataHelper.CleanSearchString(query.UserName));
				}
				if (query.Status == OrderStatus.History)
				{
					stringBuilder.AppendFormat(" AND OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2} AND OrderDate < '{3}'", 1, 4, 9, DateTime.Now.AddMonths(-3));
				}
				else if (query.Status == OrderStatus.BuyerAlreadyPaid && query.TakeOnStore.HasValue && !query.TakeOnStore.Value)
				{
					int? supplierId = query.StoreId;
					num = 0;
					if (supplierId > num)
					{
						stringBuilder.AppendFormat(" AND (OrderStatus = {0} OR (OrderStatus = 1 AND Gateway = 'hishop.plugins.payment.podrequest')) AND StoreId={1} and ShippingModeId!=-2", (int)query.Status, query.StoreId);
					}
					else
					{
						stringBuilder.AppendFormat(" AND (OrderStatus = {0} OR (OrderStatus = 1 AND Gateway = 'hishop.plugins.payment.podrequest')) and ShippingModeId!=-2", (int)query.Status);
					}
				}
				else if (query.Status == OrderStatus.BuyerAlreadyPaid && query.TakeOnStore.HasValue && query.TakeOnStore.Value)
				{
					int? supplierId = query.StoreId;
					num = 0;
					if (supplierId > num)
					{
						stringBuilder.AppendFormat(" AND ((OrderStatus = {0} OR (OrderStatus = 1 AND Gateway = 'hishop.plugins.payment.podrequest')) or (PaymentTypeId=-3 AND OrderStatus = 1)) AND StoreId={1} and (ShippingModeId=-2 or OrderType=6)", (int)query.Status, query.StoreId);
					}
					else
					{
						stringBuilder.AppendFormat(" AND ((OrderStatus = {0} OR (OrderStatus = 1 AND Gateway = 'hishop.plugins.payment.podrequest')) or (PaymentTypeId=-3 AND OrderStatus = 1)) and (ShippingModeId=-2 or OrderType=6)", (int)query.Status);
					}
				}
				else if (query.Status == OrderStatus.ApplyForRefund)
				{
					stringBuilder.AppendFormat(" AND OrderStatus = {0}", (int)query.Status);
				}
				else if (query.Status != 0)
				{
					stringBuilder.AppendFormat(" AND OrderStatus = {0}", (int)query.Status);
					if (query.ItemStatus.HasValue)
					{
						stringBuilder.AppendFormat(" And ItemStatus={0}", query.ItemStatus.Value);
					}
				}
				if (query.TakeOnStore.HasValue && query.Status != OrderStatus.BuyerAlreadyPaid)
				{
					stringBuilder.AppendFormat(" and ShippingModeId {0}", query.TakeOnStore.Value ? " = -2" : " <> -2");
				}
				if (query.IsWaitTakeOnStore.HasValue && query.IsWaitTakeOnStore.Value)
				{
					stringBuilder.Append(" And (");
					stringBuilder.AppendFormat("(IsConfirm = {0} AND ShippingModeId = {1} AND (OrderStatus = {2} OR OrderStatus = {3}))", 1, -2, 1, 2);
					StringBuilder stringBuilder5 = stringBuilder;
					object[] obj = new object[5]
					{
						" OR ( OrderType =",
						null,
						null,
						null,
						null
					};
					orderType = OrderType.ServiceOrder;
					obj[1] = orderType.GetHashCode();
					obj[2] = " and  OrderStatus = ";
					obj[3] = 2;
					obj[4] = ") ";
					stringBuilder5.Append(string.Concat(obj));
					stringBuilder.Append(")");
				}
				if (query.IsConfirm.HasValue)
				{
					if (query.IsConfirm.Value)
					{
						stringBuilder.AppendFormat(" And IsConfirm = {0}", 1);
					}
					else
					{
						stringBuilder.AppendFormat(" And IsConfirm = {0} AND (ShippingModeId = -2 AND (OrderStatus = {1}  OR (OrderStatus = " + 1 + " AND PaymentTypeId = -3))) ", 0, 2);
					}
				}
				if (query.IsAfterSales.HasValue && query.IsAfterSales.Value)
				{
					num = 6;
					string arg = num.ToString();
					stringBuilder.AppendFormat(" And (OrderStatus IN ({0}) OR ItemStatus <> 0)", arg);
				}
				if (query.IsAfterSaleRefused.HasValue && query.IsAfterSaleRefused.Value)
				{
					num = 18;
					string arg2 = num.ToString();
					stringBuilder.AppendFormat(" And (OrderStatus in ({0}) AND ItemStatus = 0)", arg2);
				}
				if (query.IsAfterSaleCompleted.HasValue && query.IsAfterSaleCompleted.Value)
				{
					num = 9;
					string arg3 = num.ToString();
					stringBuilder.AppendFormat(" And (OrderStatus in ({0}) OR ItemStatus != " + 0 + ")", arg3);
				}
				if (query.IsReturning.HasValue && query.IsReturning.Value)
				{
					stringBuilder.AppendFormat(" And (ItemStatus = " + 2 + ")");
				}
				if (query.IsTakeOnStoreCompleted.HasValue && query.IsTakeOnStoreCompleted.Value)
				{
					StringBuilder stringBuilder6 = stringBuilder;
					object[] obj2 = new object[5]
					{
						" And (OrderStatus = ",
						5,
						" AND (ShippingModeId = -2 or OrderType =",
						null,
						null
					};
					orderType = OrderType.ServiceOrder;
					obj2[3] = orderType.GetHashCode();
					obj2[4] = ")) ";
					stringBuilder6.Append(string.Concat(obj2));
				}
				if (query.IsAllTakeOnStore.HasValue && query.IsAllTakeOnStore.Value)
				{
					StringBuilder stringBuilder7 = stringBuilder;
					orderType = OrderType.ServiceOrder;
					stringBuilder7.Append(" AND (ShippingModeId = -2 or OrderType =" + orderType.GetHashCode() + ")");
				}
				if (query.IsAllAfterSale.HasValue && query.IsAllAfterSale.Value)
				{
					num = 6;
					string arg4 = num.ToString();
					arg4 = arg4 + "," + 18;
					arg4 = arg4 + "," + 9;
					stringBuilder.AppendFormat(" And (OrderStatus IN ({0}) OR ItemStatus <> 0)", arg4);
				}
				if (query.IsStoreCollection.HasValue)
				{
					if (query.IsStoreCollection.Value)
					{
						stringBuilder.Append(" AND IsStoreCollect = 1 ");
					}
					else
					{
						stringBuilder.Append(" AND IsStoreCollect = 0 ");
					}
				}
				if (query.StartDate.HasValue)
				{
					stringBuilder.AppendFormat(" AND datediff(dd,'{0}',OrderDate)>=0", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
				}
				if (query.EndDate.HasValue)
				{
					stringBuilder.AppendFormat(" AND datediff(dd,'{0}',OrderDate)<=0", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
				}
				if (query.ShippingModeId.HasValue)
				{
					if (query.ShippingModeId.Value == -2)
					{
						StringBuilder stringBuilder8 = stringBuilder;
						orderType = OrderType.ServiceOrder;
						stringBuilder8.Append(" AND (ShippingModeId = -2 or OrderType =" + orderType.GetHashCode() + ")");
					}
					else
					{
						stringBuilder.AppendFormat(" AND (ShippingModeId = {0} )", query.ShippingModeId.Value);
					}
				}
				if (query.IsPrinted.HasValue)
				{
					stringBuilder.AppendFormat(" AND ISNULL(IsPrinted, 0)={0}", query.IsPrinted.Value);
				}
				if (query.SourceOrder.HasValue)
				{
					stringBuilder.AppendFormat(" And SourceOrder = {0}", query.SourceOrder.Value);
				}
				if (query.StoreId.HasValue)
				{
					if (query.StoreId.Value == 0)
					{
						stringBuilder.Append(" AND (StoreId = 0 or StoreId is null or StoreId='')");
					}
					else
					{
						stringBuilder.AppendFormat(" AND StoreId = {0}", query.StoreId.Value);
					}
				}
				if (query.IsTickit.HasValue)
				{
					stringBuilder.AppendFormat(" and (Tax {0}", query.IsTickit.Value ? ">0)" : " is null or Tax = '')");
				}
				if (!string.IsNullOrEmpty(query.TakeCode))
				{
					stringBuilder.AppendFormat(" and TakeCode = '{0}'", DataHelper.CleanSearchString(query.TakeCode));
				}
				if (query.IsAllotStore.HasValue)
				{
					int? supplierId = query.IsAllotStore;
					num = 1;
					if (supplierId == num)
					{
						stringBuilder.Append(" AND (StoreId = 0 or StoreId is null or StoreId='') AND OrderStatus=2 ");
					}
					else
					{
						supplierId = query.IsAllotStore;
						num = 2;
						if (supplierId == num)
						{
							stringBuilder.Append(" and StoreId > 0 ");
						}
					}
				}
				if (query.IsPay)
				{
					stringBuilder.Append(" AND PayDate IS NOT NULL");
				}
				if (!string.IsNullOrEmpty(query.InvoiceTypes))
				{
					stringBuilder.Append(" AND InvoiceTitle IS NOT NULL AND InvoiceTitle <> '' AND InvoiceType IN(" + query.InvoiceTypes + ")");
				}
			}
			stringBuilder.Append(" AND ParentOrderId <> '-1' ");
			StringBuilder stringBuilder9 = new StringBuilder();
			stringBuilder9.Append(",(ISNULL((SELECT SUM(ItemAdjustedPrice * Quantity) FROM Hishop_OrderItems WHERE OrderId = o.OrderId),0)) AS ProductAmount");
			stringBuilder9.Append(",(ISNULL((SELECT SUM(ShipmentQuantity) FROM Hishop_OrderItems WHERE OrderId = o.OrderId),0)) AS AllQuantity");
			stringBuilder9.Append(",(ISNULL((SELECT SUM(Quantity) FROM Hishop_OrderItems WHERE OrderId = o.OrderId),0)) AS BuyQuantity");
			stringBuilder9.Append(",(ISNULL((SELECT SUM(Quantity) FROM Hishop_OrderGifts WHERE OrderId = o.OrderId),0)) AS GiftQuantity");
			StringBuilder stringBuilder10 = stringBuilder9;
			object[] obj3 = new object[5]
			{
				",(ISNULL((SELECT SUM(Quantity) FROM Hishop_OrderReturns WHERE OrderId = o.OrderId AND AfterSaleType = ",
				1.GetHashCode(),
				" AND HandleStatus = ",
				null,
				null
			};
			ReturnStatus returnStatus = ReturnStatus.Returned;
			obj3[3] = returnStatus.GetHashCode();
			obj3[4] = "),0)) AS AfterSaleQuantity";
			stringBuilder10.Append(string.Concat(obj3));
			StringBuilder stringBuilder11 = stringBuilder9;
			object[] obj4 = new object[9]
			{
				",(SELECT COUNT(OrderId) FROM Hishop_OrderItems WHERE OrderId = o.OrderId AND Status NOT IN (",
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null
			};
			LineItemStatus lineItemStatus = LineItemStatus.Returned;
			obj4[1] = lineItemStatus.GetHashCode();
			obj4[2] = ",";
			lineItemStatus = LineItemStatus.ReturnsRefused;
			obj4[3] = lineItemStatus.GetHashCode();
			obj4[4] = ",";
			lineItemStatus = LineItemStatus.Replaced;
			obj4[5] = lineItemStatus.GetHashCode();
			obj4[6] = ",";
			lineItemStatus = LineItemStatus.ReplaceRefused;
			obj4[7] = lineItemStatus.GetHashCode();
			obj4[8] = ")) AS AfterSaleCount";
			stringBuilder11.Append(string.Concat(obj4));
			StringBuilder stringBuilder12 = stringBuilder9;
			object[] obj5 = new object[5]
			{
				",(ISNULL((SELECT TOP 1 ReturnId FROM Hishop_OrderReturns WHERE OrderId = o.OrderId AND HandleStatus NOT IN (",
				null,
				null,
				null,
				null
			};
			returnStatus = ReturnStatus.Returned;
			obj5[1] = returnStatus.GetHashCode();
			obj5[2] = ",";
			returnStatus = ReturnStatus.Refused;
			obj5[3] = returnStatus.GetHashCode();
			obj5[4] = ")),0)) AS ReturnId";
			stringBuilder12.Append(string.Concat(obj5));
			StringBuilder stringBuilder13 = stringBuilder9;
			object[] obj6 = new object[5]
			{
				",(ISNULL((SELECT TOP 1 ReplaceId FROM Hishop_OrderReplace WHERE OrderId = o.OrderId AND HandleStatus NOT IN(",
				null,
				null,
				null,
				null
			};
			ReplaceStatus replaceStatus = ReplaceStatus.Replaced;
			obj6[1] = replaceStatus.GetHashCode();
			obj6[2] = ", ";
			replaceStatus = ReplaceStatus.Refused;
			obj6[3] = replaceStatus.GetHashCode();
			obj6[4] = ")), 0)) AS ReplaceId";
			stringBuilder13.Append(string.Concat(obj6));
			stringBuilder9.Append(",(SELECT TOP 1 StoreName FROM Hishop_Stores WHERE StoreId = o.StoreId AND o.StoreId > 0) AS StoreName");
			return DataHelper.PagingByRownumber<OrderModel>(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_Orders o", "OrderId", stringBuilder.ToString(), "*,(OrderId+ISNULL(PayRandCode,'')) AS PayOrderId" + stringBuilder9.ToString());
		}
	}
}
