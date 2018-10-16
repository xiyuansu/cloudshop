using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Orders
{
	public class ReturnDao : BaseDao
	{
		public bool RefundToBalance(OrderInfo order, MemberInfo user, decimal refundMoney)
		{
			if (user != null)
			{
				string orderId = order.OrderId;
				decimal num = default(decimal);
				decimal num2 = default(decimal);
				num2 = user.Balance + refundMoney;
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("INSERT INTO Hishop_BalanceDetails(UserId,UserName,TradeDate,TradeType,Income,Balance,Remark)");
				stringBuilder.AppendFormat("values({0},'{1}',{2},{3},{4},{5},'{6}')", user.UserId, user.UserName, "getdate()", 5, refundMoney, num2, "订单" + order.OrderId + "退款");
				DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
				return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
			}
			return false;
		}

		public bool AgreedReturns(int returnsId, decimal refundMoney, string adminRemark, string OrderId, string skuId = "", string AdminShipAddress = "", string adminShipTo = "", string adminCellPhone = "", bool isRefund = false)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("UPDATE Hishop_OrderItems SET Status = @ItemStatus,RefundAmount = @RefundMoney WHERE OrderId = @OrderId AND SkuId = @SkuId;");
			if (!isRefund)
			{
				stringBuilder.Append("UPDATE Hishop_OrderReturns SET HandleStatus = @HandleStatus,AgreedOrRefusedTime = @HandleTime ,AdminRemark = @AdminRemark,RefundAmount = @RefundMoney,AdminShipAddress = @AdminShipAddress,AdminShipTo=@AdminShipTo,AdminCellPhone=@AdminCellPhone WHERE ReturnId = @ReturnId;");
			}
			else
			{
				stringBuilder.Append("UPDATE Hishop_OrderReturns SET HandleStatus = @HandleStatus,AgreedOrRefusedTime = @HandleTime,FinishTime = @HandleTime ,AdminRemark = @AdminRemark,RefundAmount = @RefundMoney WHERE ReturnId = @ReturnId;");
			}
			ReturnInfo returnInfo = this.Get<ReturnInfo>(returnsId);
			if (returnInfo == null)
			{
				return false;
			}
			if (returnInfo.AfterSaleType == AfterSaleTypes.ReturnAndRefund)
			{
				decimal d = base.database.ExecuteScalar(CommandType.Text, $"select CostPrice from Hishop_OrderItems where OrderId='{returnInfo.OrderId}' and SkuId='{returnInfo.SkuId}'").ToDecimal(0);
				decimal num = d * (decimal)returnInfo.Quantity;
				stringBuilder.Append(" update Hishop_Orders SET OrderCostPrice = OrderCostPrice - " + num + ",RefundAmount=isnull(RefundAmount,0)+@RefundMoney where  OrderId = @OrderId; ");
			}
			else
			{
				stringBuilder.Append(" update Hishop_Orders SET RefundAmount=isnull(RefundAmount,0)+@RefundMoney where  OrderId = @OrderId; ");
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "HandleTime", DbType.DateTime, DateTime.Now);
			base.database.AddInParameter(sqlStringCommand, "AdminRemark", DbType.String, adminRemark);
			base.database.AddInParameter(sqlStringCommand, "RefundMoney", DbType.Decimal, refundMoney);
			base.database.AddInParameter(sqlStringCommand, "ReturnId", DbType.Int32, returnsId);
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, OrderId);
			base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
			if (!isRefund)
			{
				base.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 3);
				base.database.AddInParameter(sqlStringCommand, "AdminShipAddress", DbType.String, AdminShipAddress);
				base.database.AddInParameter(sqlStringCommand, "AdminShipTo", DbType.String, adminShipTo);
				base.database.AddInParameter(sqlStringCommand, "AdminCellPhone", DbType.String, adminCellPhone);
				base.database.AddInParameter(sqlStringCommand, "ItemStatus", DbType.Int32, 21);
			}
			else
			{
				base.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 1);
				if (returnInfo.AfterSaleType == AfterSaleTypes.ReturnAndRefund || returnInfo.AfterSaleType == AfterSaleTypes.Replace)
				{
					base.database.AddInParameter(sqlStringCommand, "ItemStatus", DbType.Int32, 24);
				}
				else
				{
					base.database.AddInParameter(sqlStringCommand, "ItemStatus", DbType.Int32, 11);
				}
			}
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool FinishGetGoods(int ReturnsId, string AdminRemark, string OrderId, string skuId, decimal refundAmount)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("UPDATE Hishop_OrderItems SET Status = @ItemStatus WHERE OrderId = @OrderId AND SkuId = @SkuId;");
			stringBuilder.Append("UPDATE Hishop_OrderReturns SET HandleStatus = @HandleStatus,RefundAmount = @RefundAmount,ConfirmGoodsTime = @HandleTime ,AdminRemark = @AdminRemark WHERE ReturnId = @ReturnId");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 5);
			base.database.AddInParameter(sqlStringCommand, "HandleTime", DbType.DateTime, DateTime.Now);
			base.database.AddInParameter(sqlStringCommand, "AdminRemark", DbType.String, AdminRemark);
			base.database.AddInParameter(sqlStringCommand, "ReturnId", DbType.Int32, ReturnsId);
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, OrderId);
			base.database.AddInParameter(sqlStringCommand, "ItemStatus", DbType.Int32, 23);
			base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
			base.database.AddInParameter(sqlStringCommand, "RefundAmount", DbType.Decimal, refundAmount);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool FinishReturn(ReturnInfo returns, decimal RefundMoney, decimal PointsRate)
		{
			if (returns == null)
			{
				return false;
			}
			return this.FinishReturn(returns.ReturnId, returns.AdminRemark, returns.OrderId, returns.SkuId, RefundMoney);
		}

		public bool FinishReturn(int ReturnsId, string AdminRemark, string OrderId, string skuId, decimal refundAmount)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("UPDATE Hishop_OrderItems SET Status = @ItemStatus WHERE OrderId = @OrderId AND SkuId = @SkuId;");
			stringBuilder.AppendFormat("UPDATE Hishop_Orders SET FinishDate = CASE WHEN FinishDate IS NULL THEN GETDATE() ELSE FinishDate END WHERE OrderId = @OrderId AND OrderStatus = {0};", 5);
			stringBuilder.Append("UPDATE Hishop_OrderReturns SET RefundAmount = @RefundAmount,HandleStatus = @HandleStatus,ConfirmGoodsTime = @HandleTime,FinishTime = @HandleTime ,AdminRemark = @AdminRemark WHERE ReturnId = @ReturnId;");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 1);
			base.database.AddInParameter(sqlStringCommand, "HandleTime", DbType.DateTime, DateTime.Now);
			base.database.AddInParameter(sqlStringCommand, "AdminRemark", DbType.String, AdminRemark);
			base.database.AddInParameter(sqlStringCommand, "ReturnId", DbType.Int32, ReturnsId);
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, OrderId);
			base.database.AddInParameter(sqlStringCommand, "ItemStatus", DbType.Int32, 24);
			base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
			base.database.AddInParameter(sqlStringCommand, "RefundAmount", DbType.Decimal, refundAmount);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UserSendGoods(int ReturnsId, string ExpressCompanyAbb, string ExpressCompanyName, string ShipOrderNumber, string OrderId, string skuId = "")
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("UPDATE Hishop_OrderItems SET Status = @ItemStatus WHERE OrderId = @OrderId AND SkuId = @SkuId;");
			stringBuilder.Append("UPDATE Hishop_OrderReturns SET HandleStatus = @HandleStatus,UserSendGoodsTime = @HandleTime ,ExpressCompanyAbb = @ExpressCompanyAbb,ExpressCompanyName = @ExpressCompanyName,ShipOrderNumber = @ShipOrderNumber WHERE ReturnId = @ReturnId");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 4);
			base.database.AddInParameter(sqlStringCommand, "HandleTime", DbType.DateTime, DateTime.Now);
			base.database.AddInParameter(sqlStringCommand, "ExpressCompanyAbb", DbType.String, ExpressCompanyAbb);
			base.database.AddInParameter(sqlStringCommand, "ExpressCompanyName", DbType.String, ExpressCompanyName);
			base.database.AddInParameter(sqlStringCommand, "ShipOrderNumber", DbType.String, ShipOrderNumber);
			base.database.AddInParameter(sqlStringCommand, "ReturnId", DbType.Int32, ReturnsId);
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, OrderId);
			base.database.AddInParameter(sqlStringCommand, "ItemStatus", DbType.Int32, 22);
			base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool CheckReturn(ReturnInfo returns, OrderInfo order, string operation, string adminReamrk, bool accept, decimal PointsRate = default(decimal))
		{
			decimal refundAmount = returns.RefundAmount;
			decimal num = (int)(returns.RefundAmount / PointsRate);
			decimal num2 = refundAmount;
			StringBuilder stringBuilder = new StringBuilder();
			if (!accept)
			{
				if (string.IsNullOrEmpty(returns.SkuId))
				{
					stringBuilder.Append("UPDATE Hishop_Orders SET OrderStatus = @OrderStatus  WHERE OrderId = @OrderId;");
				}
				else
				{
					stringBuilder.Append("UPDATE Hishop_OrderItems SET Status = @ItemStatus WHERE OrderId = @OrderId AND SkuId = @SkuId;");
				}
				stringBuilder.Append("UPDATE Hishop_OrderReturns SET Operator = @Operator, AdminRemark = @AdminRemark,HandleStatus  =@HandleStatus,AgreedOrRefusedTime = @AgreedOrRefusedTime,RefundAmount = @RefundMoney WHERE  ReturnId = @ReturnId;");
			}
			else
			{
				stringBuilder.AppendFormat("UPDATE Hishop_Orders SET FinishDate = CASE WHEN FinishDate IS NULL THEN GETDATE() ELSE FinishDate END WHERE OrderId = @OrderId AND OrderStatus = {0};", 5);
				foreach (LineItemInfo value in order.LineItems.Values)
				{
					if ((value.Status == LineItemStatus.Refunded || value.Status == LineItemStatus.Returned) && value.ReturnInfo != null && value.Status == LineItemStatus.Returned)
					{
						num2 += value.ReturnInfo.RefundAmount;
					}
				}
				stringBuilder.Append("UPDATE Hishop_Orders SET RefundAmount = @OrderRefundMoney WHERE OrderId = @OrderId;");
				stringBuilder.Append("UPDATE Hishop_OrderItems SET RefundAmount = @RefundMoney, Status = @ItemStatus, RealTotalPrice = (ItemAdjustedPrice * Quantity - @RefundMoney) WHERE OrderId = @OrderId AND SkuId = @SkuId;");
				stringBuilder.Append(" UPDATE Hishop_OrderReturns SET Operator = @Operator, AdminRemark = @AdminRemark,HandleStatus = @HandleStatus,ConfirmGoodsTime = @ConfirmGoodsTime,FinishTime = @FinishTime,RefundAmount = @RefundMoney WHERE ReturnId = @ReturnId;");
				if (returns.RefundType == RefundTypes.InBalance & accept)
				{
					string arg = returns.OrderId;
					if (!string.IsNullOrEmpty(returns.SkuId))
					{
						if (!order.LineItems.ContainsKey(returns.SkuId))
						{
							return false;
						}
						arg = "订单 " + returns.OrderId + " 有商品退货 ";
					}
					stringBuilder.Append(" INSERT INTO Hishop_BalanceDetails(UserId,UserName,TradeDate,TradeType,Income");
					stringBuilder.AppendFormat(",Balance,Remark) select UserId,Username,getdate() as TradeDate,{0} as TradeType,", 7);
					stringBuilder.Append("@RefundMoney as Income,ISNULL((SELECT TOP 1 Balance from Hishop_BalanceDetails b");
					stringBuilder.AppendFormat(" WHERE b.UserId=a.UserId order by JournalNumber desc),0)+@RefundMoney as Balance,'{0}' as Remark ", arg);
					stringBuilder.Append("FROM Hishop_Orders a WHERE OrderId = @OrderId;");
				}
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			if (accept)
			{
				base.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 1);
				base.database.AddInParameter(sqlStringCommand, "ItemStatus", DbType.Int32, 24);
				base.database.AddInParameter(sqlStringCommand, "ConfirmGoodsTime", DbType.DateTime, DateTime.Now);
				base.database.AddInParameter(sqlStringCommand, "FinishTime", DbType.DateTime, DateTime.Now);
			}
			else
			{
				base.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 2);
				base.database.AddInParameter(sqlStringCommand, "ItemStatus", DbType.Int32, 25);
				base.database.AddInParameter(sqlStringCommand, "AgreedOrRefusedTime", DbType.DateTime, DateTime.Now);
			}
			base.database.AddInParameter(sqlStringCommand, "OrderRefundMoney", DbType.Decimal, num2);
			base.database.AddInParameter(sqlStringCommand, "RefundPoint", DbType.Int32, num);
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, returns.OrderId);
			base.database.AddInParameter(sqlStringCommand, "Operator", DbType.String, returns.Operator);
			base.database.AddInParameter(sqlStringCommand, "AdminRemark", DbType.String, adminReamrk);
			base.database.AddInParameter(sqlStringCommand, "RefundMoney", DbType.Decimal, refundAmount);
			base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, returns.SkuId);
			base.database.AddInParameter(sqlStringCommand, "ReturnId", DbType.Int32, returns.ReturnId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public void GetRefundTypeFromReturn(string orderId, out int refundType, out string remark, string SkuId = "")
		{
			refundType = 0;
			remark = "";
			string str = "SELECT RefundType,UserRemark FROM Hishop_OrderReturns WHERE HandleStatus=0 AND OrderId = @OrderId";
			str = (string.IsNullOrEmpty(SkuId) ? (str + " AND (SkuId='' OR SkuId IS NULL)") : (str + " AND SkuId = @SkuId"));
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(str);
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, SkuId);
			IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand);
			if (dataReader.Read())
			{
				refundType = (int)((IDataRecord)dataReader)["RefundType"];
				remark = (string)((IDataRecord)dataReader)["UserRemark"];
			}
		}

		public IList<ReturnInfo> GetReturnApplysNoPage(ReturnsApplyQuery query)
		{
			query.OrderId = base.GetTrueOrderId(query.OrderId);
			IList<ReturnInfo> result = new List<ReturnInfo>();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1");
			if (!string.IsNullOrEmpty(query.OrderId))
			{
				stringBuilder.AppendFormat(" AND OrderId = '{0}'", query.OrderId);
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
			if (query.HandleStatus.HasValue)
			{
				stringBuilder.AppendFormat(" AND HandleStatus = {0}", query.HandleStatus);
			}
			if (query.UserId.HasValue)
			{
				stringBuilder.AppendFormat(" AND UserId = {0}", query.UserId.Value);
			}
			if (query.StoreId.HasValue && query.StoreId > 0)
			{
				stringBuilder.AppendFormat(" AND StoreId = {0}", query.StoreId.Value);
			}
			if (query.IsNoCompleted.HasValue && query.IsNoCompleted.Value)
			{
				stringBuilder.AppendFormat(" AND HandleStatus <> {0} AND HandleStatus <> {1}", 2, 1);
			}
			if (query.SupplierNoCompleted.HasValue && query.SupplierNoCompleted.Value)
			{
				stringBuilder.AppendFormat(" AND (HandleStatus = {0} OR HandleStatus = {1})", 0, 5);
			}
			if (query.ReturnId.HasValue)
			{
				stringBuilder.AppendFormat(" AND ReturnId = {0}", query.ReturnId.Value);
			}
			if (!string.IsNullOrEmpty(query.ProductName))
			{
				stringBuilder.AppendFormat(" AND ShopName LIKE '%{0}%'", query.ProductName);
			}
			if (!string.IsNullOrEmpty(query.ReturnIds))
			{
				stringBuilder.Append($" AND ReturnId IN({query.ReturnIds}) ");
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM vw_Hishop_OrderReturns WHERE " + stringBuilder.ToString());
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<ReturnInfo>(objReader);
			}
			return result;
		}

		public PageModel<ReturnInfo> GetReturnsApplys(ReturnsApplyQuery query)
		{
			query.OrderId = base.GetTrueOrderId(query.OrderId);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1");
			if (!string.IsNullOrEmpty(query.OrderId))
			{
				stringBuilder.AppendFormat(" AND OrderId = '{0}'", query.OrderId);
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
			if (query.HandleStatus.HasValue)
			{
				if (query.HandleStatus.Value != 6)
				{
					stringBuilder.AppendFormat(" AND HandleStatus = {0}", query.HandleStatus);
				}
				else
				{
					stringBuilder.AppendFormat(" AND (HandleStatus = {0} OR HandleStatus = {1} OR (HandleStatus = {2} AND AfterSaleType = {3})) AND ExceptionInfo IS NOT NULL AND ExceptionInfo <>''", 4, 5, 0, 3);
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
			if (query.IsNoCompleted.HasValue && query.IsNoCompleted.Value)
			{
				stringBuilder.AppendFormat(" AND HandleStatus <> {0} AND HandleStatus <> {1}", 2, 1);
			}
			if (query.SupplierNoCompleted.HasValue && query.SupplierNoCompleted.Value)
			{
				stringBuilder.AppendFormat(" AND (HandleStatus = {0} OR HandleStatus = {1})", 0, 5);
			}
			if (query.ReturnId.HasValue)
			{
				stringBuilder.AppendFormat(" AND ReturnId = {0}", query.ReturnId.Value);
			}
			if (!string.IsNullOrEmpty(query.ProductName))
			{
				stringBuilder.AppendFormat(" AND ShopName LIKE '%{0}%'", query.ProductName);
			}
			return DataHelper.PagingByRownumber<ReturnInfo>(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_OrderReturns", "ReturnId", stringBuilder.ToString(), "*");
		}

		public bool DelReturnsApply(int returnsId)
		{
			string query = $"DELETE FROM Hishop_OrderReturns WHERE ReturnId={returnsId} and (HandleStatus ={2} OR HandleStatus = {1}) ";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool ApplyForReturn(ReturnInfo returnInfo)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("UPDATE Hishop_OrderItems SET RefundAmount = @RefundAmount,RealTotalPrice = (ItemAdjustedPrice * Quantity - @RefundAmount),Status = @ItemStatus WHERE OrderId = @OrderId AND SkuId = @SkuId;");
			stringBuilder.Append("DELETE FROM Hishop_OrderReturns WHERE OrderId = @OrderId AND SkuId = @SkuId;");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "RefundAmount", DbType.Decimal, returnInfo.RefundAmount);
			base.database.AddInParameter(sqlStringCommand, "ItemStatus", DbType.Int32, 20);
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, returnInfo.OrderId);
			base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, returnInfo.SkuId);
			if (base.database.ExecuteNonQuery(sqlStringCommand) > 0)
			{
				return this.Add(returnInfo, null) > 0;
			}
			return false;
		}

		public bool StoreApplyForReturn(ReturnInfo returnInfo)
		{
			LineItemStatus lineItemStatus = LineItemStatus.Returned;
			returnInfo.HandleStatus = ReturnStatus.Returned;
			returnInfo.ApplyForTime = DateTime.Now;
			if (returnInfo.RefundType == RefundTypes.BackReturn)
			{
				lineItemStatus = LineItemStatus.GetGoodsForReturn;
				returnInfo.HandleStatus = ReturnStatus.GetGoods;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("UPDATE Hishop_OrderItems SET RefundAmount = @RefundAmount,RealTotalPrice = (ItemAdjustedPrice * Quantity - @RefundAmount),Status = @ItemStatus WHERE OrderId = @OrderId AND SkuId = @SkuId;");
			base.database.ExecuteNonQuery(CommandType.Text, string.Format("DELETE FROM Hishop_OrderReturns WHERE OrderId = @OrderId AND SkuId = @SkuId;", returnInfo.OrderId, returnInfo.SkuId));
			int num = this.Add(returnInfo, null).ToInt(0);
			if (num > 0)
			{
				returnInfo.ReturnId = num;
				DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
				base.database.AddInParameter(sqlStringCommand, "RefundAmount", DbType.Decimal, returnInfo.RefundAmount);
				base.database.AddInParameter(sqlStringCommand, "ItemStatus", DbType.Int32, (int)lineItemStatus);
				base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, returnInfo.OrderId);
				base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, returnInfo.SkuId);
				return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
			}
			return false;
		}

		public bool UpdateRefundOrderId(string oldRefundOrderId, string newRefundOrderId, string orderId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE Hishop_OrderReturns SET RefundOrderId = @newRefundOrderId WHERE RefundOrderId = @oldRefundOrderId AND OrderId = @OrderId");
			base.database.AddInParameter(sqlStringCommand, "newRefundOrderId", DbType.String, newRefundOrderId);
			base.database.AddInParameter(sqlStringCommand, "oldRefundOrderId", DbType.String, oldRefundOrderId);
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public decimal GetRefundMoney(OrderInfo order, out decimal refundMoney, string SkuId = "")
		{
			decimal num = default(decimal);
			string text = "SELECT RefundAmount FROM dbo.Hishop_OrderReturns WHERE HandleStatus=1 AND OrderId = @OrderId";
			if (!string.IsNullOrEmpty(SkuId))
			{
				text += " AND SkuId = @SkuId";
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(text);
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, order.OrderId);
			base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, SkuId);
			num = Convert.ToDecimal(base.database.ExecuteScalar(sqlStringCommand));
			return refundMoney = num;
		}

		public ReturnInfo GetReturnInfo(string OrderId, string SkuId = "")
		{
			OrderId = base.GetTrueOrderId(OrderId);
			ReturnInfo result = null;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT TOP 1 * FROM Hishop_OrderReturns WHERE OrderId = @OrderId");
			if (!string.IsNullOrEmpty(SkuId))
			{
				stringBuilder.Append(" AND SkuId = @SkuID");
			}
			else
			{
				stringBuilder.Append(" AND (SkuId IS NULL OR SkuId='')");
			}
			stringBuilder.Append(" ORDER BY ReturnId DESC");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, OrderId);
			base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, SkuId);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToModel<ReturnInfo>(objReader);
			}
			return result;
		}

		public int GetReturnGoodsNum(string OrderId, string SkuId = "")
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT ISNULL(Sum(Quantity),0) FROM Hishop_OrderReturns WHERE OrderId = @OrderId");
			if (!string.IsNullOrEmpty(SkuId))
			{
				stringBuilder.Append(" AND SkuId = @SkuID");
			}
			stringBuilder.Append(" AND HandleStatus = " + 1);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, OrderId);
			base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, SkuId);
			return Convert.ToInt32(base.database.ExecuteScalar(sqlStringCommand));
		}

		public ReturnInfo GetReturnInfoOfRefundOrderId(string RefundOrderId)
		{
			ReturnInfo result = null;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT TOP 1 * FROM Hishop_OrderReturns WHERE RefundOrderId = @RefundOrderId");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "RefundOrderId", DbType.String, RefundOrderId);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToModel<ReturnInfo>(objReader);
			}
			return result;
		}

		public int GetReturnId(string orderId, string skuId = "")
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT TOP 1 ReturnId FROM Hishop_OrderReturns WHERE OrderId = @OrderId");
			if (!string.IsNullOrEmpty(skuId))
			{
				stringBuilder.Append(" AND SkuId = @SkuID");
			}
			else
			{
				stringBuilder.Append(" AND (SkuId IS NULL OR SkuId='')");
			}
			stringBuilder.Append(" ORDER BY ReturnId DESC");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0);
		}

		public bool UpdateReturnsApply_Receipt(int ReturnsId, string OrderId, string skuId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("UPDATE Hishop_OrderItems SET Status = @ItemStatus WHERE OrderId = @OrderId AND SkuId = @SkuId;");
			stringBuilder.Append("UPDATE Hishop_OrderReturns SET HandleStatus = @HandleStatus,ConfirmGoodsTime = @ConfirmGoodsTime  WHERE ReturnId = @ReturnId AND OrderId=@OrderId");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "ItemStatus", DbType.Int32, 23);
			base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
			base.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 5);
			base.database.AddInParameter(sqlStringCommand, "ConfirmGoodsTime", DbType.DateTime, DateTime.Now);
			base.database.AddInParameter(sqlStringCommand, "ReturnId", DbType.Int32, ReturnsId);
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, OrderId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool SaveRefundErr(int refundId, string msg)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE Hishop_OrderReturns SET ExceptionInfo = @ExceptionInfo WHERE ReturnId = @RefundId");
			base.database.AddInParameter(sqlStringCommand, "RefundId", DbType.Int32, refundId);
			base.database.AddInParameter(sqlStringCommand, "ExceptionInfo", DbType.String, msg);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public IList<ReturnInfo> GetReturnListOfReturnIds(string refundIds)
		{
			IList<ReturnInfo> result = new List<ReturnInfo>();
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM vw_Hishop_OrderReturns WHERE ReturnId IN(" + refundIds + ") AND RefundType = 3;");
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<ReturnInfo>(objReader);
			}
			return result;
		}
	}
}
