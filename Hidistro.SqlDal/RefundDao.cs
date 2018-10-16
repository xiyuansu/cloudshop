using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.SqlDal.Orders;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Hidistro.SqlDal
{
	public class RefundDao : BaseDao
	{
		public void GetRefundType(string orderId, out int refundType, out string remark)
		{
			orderId = base.GetTrueOrderId(orderId);
			refundType = 0;
			remark = "";
			string query = "SELECT RefundType,UserRemark FROM Hishop_OrderRefund WHERE OrderId= @OrderId";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					refundType = ((((IDataRecord)dataReader)["RefundType"] != DBNull.Value) ? ((int)((IDataRecord)dataReader)["RefundType"]) : 0);
					remark = (string)((IDataRecord)dataReader)["UserRemark"];
				}
			}
		}

		public bool FinishRefund(RefundInfo refund, decimal RefundMoney, decimal PointsRate = default(decimal), MemberInfo user = null)
		{
			if (refund == null)
			{
				return false;
			}
			OrderInfo orderInfo = new OrderDao().GetOrderInfo(refund.OrderId);
			if (orderInfo.OrderType == OrderType.ServiceOrder)
			{
				return this.CheckRefundForServiceProduct(orderInfo, refund, refund.Operator, refund.AdminRemark, true, RefundMoney, user, true);
			}
			return this.CheckRefund(orderInfo, refund.Operator, refund.AdminRemark, (int)refund.RefundType, true, RefundMoney, PointsRate, user, true);
		}

		public bool FailedRefund(RefundInfo refund, decimal PointsRate = default(decimal))
		{
			if (refund == null)
			{
				return false;
			}
			OrderInfo orderInfo = new OrderDao().GetOrderInfo(refund.OrderId);
			if (orderInfo.OrderType != OrderType.ServiceOrder)
			{
				return this.CheckRefundForServiceProduct(orderInfo, refund, refund.Operator, refund.AdminRemark, false, refund.RefundAmount, null, true);
			}
			return this.CheckRefund(orderInfo, refund.Operator, refund.AdminRemark, (int)refund.RefundType, false, refund.RefundAmount, PointsRate, null, true);
		}

		public bool CheckRefundForServiceProduct(OrderInfo order, RefundInfo refund, string Operator, string adminRemark, bool accept, decimal refundMoney, MemberInfo user, bool isRefundNotify = false)
		{
			if (refundMoney <= decimal.Zero)
			{
				refundMoney = refund.RefundAmount;
			}
			int refundType = (int)refund.RefundType;
			StringBuilder stringBuilder = new StringBuilder();
			if (!accept)
			{
				stringBuilder.Append(" UPDATE Hishop_OrderRefund set Operator = @Operator,AdminRemark = @AdminRemark,HandleStatus = @HandleStatus,AgreedOrRefusedTime = @HandleTime WHERE  RefundId = @RefundId");
				stringBuilder.Append(";");
			}
			else
			{
				stringBuilder.Append("UPDATE Hishop_Orders SET RefundAmount = ISNULL(RefundAmount,0) + @OrderRefundMoney,OrderCostPrice = OrderCostPrice - @OrderRefundMoney WHERE OrderId = @OrderId;");
				if (isRefundNotify)
				{
					stringBuilder.Append(" UPDATE Hishop_OrderRefund SET Operator = @Operator,AdminRemark = @AdminRemark,HandleStatus = @HandleStatus,FinishTime = @HandleTime,RefundAmount = @RefundAmount WHERE HandleStatus=0 AND RefundId = @RefundId;");
				}
				else
				{
					stringBuilder.Append(" UPDATE Hishop_OrderRefund SET Operator = @Operator,AdminRemark = @AdminRemark,HandleStatus = @HandleStatus,AgreedOrRefusedTime = @HandleTime,FinishTime = @HandleTime,RefundAmount = @RefundAmount WHERE HandleStatus = 0 AND RefundId = @RefundId;");
				}
				if (refund == null || order == null)
				{
					return false;
				}
				if ((refundType == 1 & accept) && user != null)
				{
					string orderId = order.OrderId;
					decimal num = default(decimal);
					decimal num2 = default(decimal);
					num = refundMoney;
					num2 = user.Balance + num;
					stringBuilder.Append("INSERT INTO Hishop_BalanceDetails(UserId,UserName,TradeDate,TradeType,Income,Balance,Remark)");
					stringBuilder.AppendFormat("VALUES({0},'{1}',{2},{3},{4},{5},'{6}')", user.UserId, user.UserName, "getdate()", 5, num, num2, "订单" + order.OrderId + "退款");
				}
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			if (accept)
			{
				base.database.AddInParameter(sqlStringCommand, "ItemStatus", DbType.Int32, 11);
				base.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 1);
			}
			else
			{
				base.database.AddInParameter(sqlStringCommand, "ItemStatus", DbType.Int32, 12);
				base.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 2);
			}
			base.database.AddInParameter(sqlStringCommand, "OrderRefundMoney", DbType.Decimal, refundMoney);
			base.database.AddInParameter(sqlStringCommand, "RefundAmount", DbType.Decimal, refundMoney);
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, order.OrderId);
			base.database.AddInParameter(sqlStringCommand, "RefundId", DbType.Int32, refund.RefundId);
			base.database.AddInParameter(sqlStringCommand, "Operator", DbType.String, Operator);
			base.database.AddInParameter(sqlStringCommand, "AdminRemark", DbType.String, adminRemark);
			base.database.AddInParameter(sqlStringCommand, "HandleTime", DbType.DateTime, DateTime.Now);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool CheckRefund(OrderInfo order, string Operator, string adminRemark, int refundType, bool accept, decimal RefundMoney = default(decimal), decimal PointsRate = default(decimal), MemberInfo user = null, bool isRefundNotify = false)
		{
			decimal num = RefundMoney;
			StringBuilder stringBuilder = new StringBuilder();
			if (!accept)
			{
				stringBuilder.Append("UPDATE Hishop_Orders SET RefundAmount = 0, OrderStatus = @OrderStatus WHERE OrderId = @OrderId;");
				stringBuilder.Append(" UPDATE Hishop_OrderRefund set Operator = @Operator,AdminRemark = @AdminRemark,HandleStatus = @HandleStatus,AgreedOrRefusedTime = @HandleTime WHERE  OrderId = @OrderId");
				stringBuilder.Append(";");
			}
			else
			{
				if (order.SupplierId > 0)
				{
					if (!string.IsNullOrEmpty(order.ShipOrderNumber))
					{
						stringBuilder.Append("UPDATE Hishop_Orders SET OrderStatus = @OrderStatus,CloseReason = '订单已退款完成',RefundAmount = @OrderRefundMoney,OrderCostPrice = OrderCostPrice - @OrderRefundMoney WHERE OrderId = @OrderId;");
					}
					else
					{
						stringBuilder.Append("UPDATE Hishop_Orders SET OrderStatus = @OrderStatus,OrderCostPrice=0,CloseReason = '订单已退款完成',RefundAmount = @OrderRefundMoney WHERE OrderId = @OrderId;");
					}
				}
				else
				{
					stringBuilder.Append("UPDATE Hishop_Orders SET OrderStatus = @OrderStatus,CloseReason = '订单已退款完成',RefundAmount = @OrderRefundMoney,OrderCostPrice = OrderCostPrice - @OrderRefundMoney WHERE OrderId = @OrderId;");
				}
				if (isRefundNotify)
				{
					stringBuilder.Append(" UPDATE Hishop_OrderRefund SET Operator = @Operator,AdminRemark = @AdminRemark,HandleStatus=@HandleStatus,FinishTime = @HandleTime,RefundAmount = @RefundAmount WHERE HandleStatus=0 AND OrderId = @OrderId;");
				}
				else
				{
					stringBuilder.Append(" UPDATE Hishop_OrderRefund SET Operator = @Operator,AdminRemark = @AdminRemark,HandleStatus = @HandleStatus,AgreedOrRefusedTime = @HandleTime,FinishTime = @HandleTime,RefundAmount = @RefundAmount WHERE HandleStatus = 0 AND OrderId = @OrderId;");
				}
				RefundInfo refundInfo = this.GetRefundInfo(order.OrderId);
				if (refundInfo == null || order == null)
				{
					return false;
				}
				if ((refundType == 1 & accept) && user != null)
				{
					string orderId = order.OrderId;
					decimal num2 = default(decimal);
					decimal num3 = default(decimal);
					num2 = RefundMoney;
					num3 = user.Balance + num2;
					stringBuilder.Append("INSERT INTO Hishop_BalanceDetails(UserId,UserName,TradeDate,TradeType,Income,Balance,Remark)");
					stringBuilder.AppendFormat("values({0},'{1}',{2},{3},{4},{5},'{6}')", user.UserId, user.UserName, "getdate()", 5, num2, num3, "订单" + order.OrderId + "退款");
				}
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			if (accept)
			{
				base.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 4);
				base.database.AddInParameter(sqlStringCommand, "ItemStatus", DbType.Int32, 11);
				base.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 1);
			}
			else
			{
				base.database.AddInParameter(sqlStringCommand, "ItemStatus", DbType.Int32, 12);
				base.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 2);
				base.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 2);
			}
			if (RefundMoney == decimal.Zero)
			{
				RefundMoney = order.GetTotal(false);
			}
			base.database.AddInParameter(sqlStringCommand, "OrderRefundMoney", DbType.Decimal, num);
			base.database.AddInParameter(sqlStringCommand, "RefundAmount", DbType.Decimal, RefundMoney);
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, order.OrderId);
			base.database.AddInParameter(sqlStringCommand, "Operator", DbType.String, Operator);
			base.database.AddInParameter(sqlStringCommand, "AdminRemark", DbType.String, adminRemark);
			base.database.AddInParameter(sqlStringCommand, "HandleTime", DbType.DateTime, DateTime.Now);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateRefundOrderStock(string orderId, int StoreId = 0, string skuId = "", int Quantity = 0)
		{
			orderId = base.GetTrueOrderId(orderId);
			string text = "";
			text = ((Quantity <= 0) ? ((StoreId <= 0) ? "(SELECT SUM(oi.ShipmentQuantity) FROM Hishop_OrderItems oi  Where oi.SkuId = Hishop_SKUs.SkuId AND OrderId = @OrderId)" : "(SELECT SUM(oi.ShipmentQuantity) FROM Hishop_OrderItems oi  Where oi.SkuId = Hishop_StoreSKUs.SkuId AND OrderId =@OrderId)") : Quantity.ToString());
			string text2 = $"Update Hishop_SKUs Set Stock = Stock + {text} WHERE Hishop_SKUs.SkuId  IN (Select SkuId FROM Hishop_OrderItems Where OrderId = @OrderId)";
			if (!string.IsNullOrEmpty(skuId))
			{
				text2 += " AND Hishop_SKUs.SkuId = @SkuId";
			}
			if (StoreId > 0)
			{
				text2 = $"UPDATE Hishop_StoreSKUs Set Stock = Stock + {text} WHERE Hishop_StoreSKUs.SkuId  IN (Select SkuId FROM Hishop_OrderItems Where OrderId =@OrderId) AND StoreId = @StoreID";
				if (!string.IsNullOrEmpty(skuId))
				{
					text2 += " AND Hishop_StoreSKUs.SkuId = @SkuId";
				}
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(text2);
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			if (StoreId > 0)
			{
				base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, StoreId);
			}
			base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
			return base.database.ExecuteNonQuery(sqlStringCommand) >= 1;
		}

		public IList<RefundModel> GetRefundApplysNoPage(RefundApplyQuery query)
		{
			query.OrderId = base.GetTrueOrderId(query.OrderId);
			IList<RefundModel> result = new List<RefundModel>();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1");
			if (!string.IsNullOrEmpty(query.OrderId))
			{
				stringBuilder.AppendFormat(" AND OrderId = '{0}'", query.OrderId);
			}
			if (query.HandleStatus.HasValue)
			{
				if (query.HandleStatus.Value != 3)
				{
					stringBuilder.AppendFormat(" AND HandleStatus = {0}", query.HandleStatus);
				}
				else
				{
					stringBuilder.AppendFormat(" AND HandleStatus = {0} AND ExceptionInfo IS NOT NULL AND ExceptionInfo <>''", 0);
				}
			}
			if (query.UserId.HasValue)
			{
				stringBuilder.AppendFormat(" AND UserId = {0}", query.UserId.Value);
			}
			if (query.StoreId.HasValue && query.StoreId > 0)
			{
				stringBuilder.AppendFormat(" AND StoreId = {0}", query.StoreId.Value);
			}
			if (query.RefundId.HasValue)
			{
				stringBuilder.AppendFormat(" AND RefundId = {0}", query.RefundId.Value);
			}
			if (query.SupplierId.HasValue)
			{
				if (query.SupplierId == -1)
				{
					stringBuilder.Append(" AND SupplierId > 0 ");
				}
				else
				{
					stringBuilder.AppendFormat(" AND SupplierId = {0} ", query.SupplierId.Value);
				}
			}
			else if (query.UserId.ToInt(0) <= 0)
			{
				stringBuilder.Append(" AND ISNULL(SupplierId,0) = 0 ");
			}
			if (!string.IsNullOrEmpty(query.RefundIds))
			{
				stringBuilder.Append($" AND RefundId IN({query.RefundIds}) ");
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM vw_Hishop_OrderRefund WHERE " + stringBuilder.ToString());
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<RefundModel>(objReader);
			}
			return result;
		}

		public PageModel<RefundModel> GetRefundApplys(RefundApplyQuery query)
		{
			query.OrderId = base.GetTrueOrderId(query.OrderId);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1");
			if (!string.IsNullOrEmpty(query.OrderId))
			{
				stringBuilder.AppendFormat(" AND OrderId = '{0}'", query.OrderId);
			}
			if (query.HandleStatus.HasValue)
			{
				if (query.HandleStatus.Value != 3)
				{
					stringBuilder.AppendFormat(" AND HandleStatus = {0}", query.HandleStatus);
				}
				else
				{
					stringBuilder.AppendFormat(" AND HandleStatus = {0} AND ExceptionInfo IS NOT NULL AND ExceptionInfo <>''", 0);
				}
			}
			if (query.UserId.HasValue)
			{
				stringBuilder.AppendFormat(" AND UserId = {0}", query.UserId.Value);
			}
			if (query.StoreId.HasValue && query.StoreId > 0)
			{
				stringBuilder.AppendFormat(" AND StoreId = {0}", query.StoreId.Value);
			}
			if (query.RefundId.HasValue)
			{
				stringBuilder.AppendFormat(" AND RefundId = {0}", query.RefundId.Value);
			}
			if (query.SupplierId.HasValue)
			{
				if (query.SupplierId == -1)
				{
					stringBuilder.Append(" AND SupplierId > 0 ");
				}
				else
				{
					stringBuilder.AppendFormat(" AND SupplierId = {0} ", query.SupplierId.Value);
				}
			}
			else if (query.UserId.ToInt(0) <= 0)
			{
				stringBuilder.Append(" and ISNULL(SupplierId,0)=0 ");
			}
			return DataHelper.PagingByRownumber<RefundModel>(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_OrderRefund", "RefundId", stringBuilder.ToString(), "*");
		}

		public bool DelRefundApply(int refundId)
		{
			string query = $"DELETE FROM Hishop_OrderRefund WHERE RefundId={refundId} and (HandleStatus = {1} OR HandleStatus = {2}) ";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateRefundOrderId(string oldRefundOrderId, string newRefundOrderId, string orderId)
		{
			orderId = base.GetTrueOrderId(orderId);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE Hishop_OrderRefund SET RefundOrderId = @newRefundOrderId WHERE RefundOrderId = @oldRefundOrderId AND OrderId = @OrderId");
			base.database.AddInParameter(sqlStringCommand, "newRefundOrderId", DbType.String, newRefundOrderId);
			base.database.AddInParameter(sqlStringCommand, "oldRefundOrderId", DbType.String, oldRefundOrderId);
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool ApplyForRefund(RefundInfo refund)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("UPDATE Hishop_Orders SET OrderStatus = @OrderStatus,RefundAmount = @RefundMoney WHERE OrderId = @OrderId;");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 6);
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, refund.OrderId);
			base.database.AddInParameter(sqlStringCommand, "RefundMoney", DbType.Decimal, refund.RefundAmount);
			if (base.database.ExecuteNonQuery(sqlStringCommand) > 0)
			{
				return this.Add(refund, null) > 0;
			}
			return false;
		}

		public int ServiceOrderApplyForRefund(RefundInfo refund)
		{
			StringBuilder stringBuilder = new StringBuilder();
			return this.Add(refund, null).ToInt(0);
		}

		public RefundInfo GetRefundInfo(string OrderId)
		{
			OrderId = base.GetTrueOrderId(OrderId);
			RefundInfo result = null;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT TOP 1 * FROM Hishop_OrderRefund WHERE OrderId = @OrderId");
			stringBuilder.Append(" ORDER BY RefundId DESC");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, OrderId);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToModel<RefundInfo>(objReader);
			}
			return result;
		}

		public List<RefundInfo> GetRefundInfos(string[] OrderIds)
		{
			List<RefundInfo> result = new List<RefundInfo>();
			if (OrderIds.Length != 0)
			{
				OrderIds = (from d in OrderIds
				select "'" + d + "'").ToArray();
				string str = string.Join(",", OrderIds);
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("SELECT * FROM Hishop_OrderRefund WHERE OrderId in (" + str + ")");
				stringBuilder.Append(" ORDER BY RefundId DESC");
				DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
				using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
				{
					result = DataHelper.ReaderToList<RefundInfo>(objReader).ToList();
				}
			}
			return result;
		}

		public int GetRefundId(string orderId, string skuId = "")
		{
			orderId = base.GetTrueOrderId(orderId);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT TOP 1 RefundId FROM Hishop_OrderRefund WHERE OrderId = @OrderId");
			if (!string.IsNullOrEmpty(skuId))
			{
				stringBuilder.Append(" AND SkuId = @SkuID");
			}
			else
			{
				stringBuilder.Append(" AND (SkuId IS NULL OR SkuId='')");
			}
			stringBuilder.Append(" ORDER BY RefundId DESC");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0);
		}

		public RefundInfo GetRefundInfoOfRefundOrderId(string RefundOrderId)
		{
			RefundInfo result = null;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT TOP 1 * FROM Hishop_OrderRefund WHERE RefundOrderId = @RefundOrderId");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "RefundOrderId", DbType.String, RefundOrderId);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToModel<RefundInfo>(objReader);
			}
			return result;
		}

		public int GetRefundQuantityOfServiceOrder(string orderId)
		{
			orderId = base.GetTrueOrderId(orderId);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT SUM(Quantity) FROM Hishop_OrderRefund WHERE OrderId = @OrderId AND HandleStatus = @HandleStatus");
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			base.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 1);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0);
		}

		public bool SaveRefundErr(int refundId, string msg)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE Hishop_OrderRefund SET ExceptionInfo = @ExceptionInfo WHERE RefundId = @RefundId");
			base.database.AddInParameter(sqlStringCommand, "RefundId", DbType.Int32, refundId);
			base.database.AddInParameter(sqlStringCommand, "ExceptionInfo", DbType.String, msg);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public IList<RefundInfo> GetRefundListOfRefundIds(string refundIds)
		{
			IList<RefundInfo> result = new List<RefundInfo>();
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM Hishop_OrderRefund WHERE RefundId IN(" + refundIds + ") AND RefundType = 3;");
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<RefundInfo>(objReader);
			}
			return result;
		}
	}
}
