using Hidistro.Core;
using Hidistro.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Orders
{
	public class ReplaceDao : BaseDao
	{
		public bool AgreedReplace(int ReplaceId, string AdminRemark, string OrderId, string skuId, string AdminShipAddress = "", string adminShipTo = "", string adminCellPhone = "")
		{
			OrderId = base.GetTrueOrderId(OrderId);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("UPDATE Hishop_OrderItems SET Status = @ItemStatus WHERE OrderId = @OrderId AND SkuId = @SkuId;");
			stringBuilder.Append("UPDATE Hishop_OrderReplace SET AdminRemark = @AdminRemark,HandleStatus = @HandleStatus,AgreedOrRefusedTime = @HandleTime,AdminShipAddress = @AdminShipAddress,AdminShipTo=@AdminShipTo,AdminCellPhone=@AdminCellPhone WHERE ReplaceId = @ReplaceId ");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 3);
			base.database.AddInParameter(sqlStringCommand, "HandleTime", DbType.DateTime, DateTime.Now);
			base.database.AddInParameter(sqlStringCommand, "AdminRemark", DbType.String, AdminRemark);
			base.database.AddInParameter(sqlStringCommand, "ReplaceId", DbType.Int32, ReplaceId);
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, OrderId);
			base.database.AddInParameter(sqlStringCommand, "AdminShipAddress", DbType.String, AdminShipAddress);
			base.database.AddInParameter(sqlStringCommand, "AdminShipTo", DbType.String, adminShipTo);
			base.database.AddInParameter(sqlStringCommand, "AdminCellPhone", DbType.String, adminCellPhone);
			base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
			base.database.AddInParameter(sqlStringCommand, "ItemStatus", DbType.Int32, 31);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool CheckReplace(string orderId, string adminRemark, bool accept, string SkuId = "", string AdminShipAddress = "", string adminShipTo = "", string adminCellPhone = "")
		{
			orderId = base.GetTrueOrderId(orderId);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("UPDATE Hishop_OrderItems SET Status = @ItemStatus WHERE OrderId = @OrderId AND SkuId = @SkuId;");
			stringBuilder.Append("UPDATE Hishop_OrderReplace SET AdminRemark = @AdminRemark,HandleStatus = @HandleStatus,AgreedOrRefusedTime = @HandleTime,AdminShipAddress = @AdminShipAddress,AdminShipTo = @AdminShipTo,AdminCellPhone = @AdminCellPhone WHERE HandleStatus = 0 AND OrderId = @OrderId ");
			stringBuilder.Append(" AND SkuId = @SkuId");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			if (accept)
			{
				stringBuilder.AppendFormat("UPDATE Hishop_Orders SET FinishDate = CASE WHEN FinishDate IS NULL THEN GETDATE() ELSE FinishDate END WHERE OrderId = @OrderId AND OrderStatus = {0};", 5);
				base.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 3);
				base.database.AddInParameter(sqlStringCommand, "ItemStatus", DbType.Int32, 31);
			}
			else
			{
				base.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 2);
				base.database.AddInParameter(sqlStringCommand, "ItemStatus", DbType.Int32, 35);
			}
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			base.database.AddInParameter(sqlStringCommand, "AdminRemark", DbType.String, adminRemark);
			base.database.AddInParameter(sqlStringCommand, "HandleTime", DbType.DateTime, DateTime.Now);
			base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, SkuId);
			base.database.AddInParameter(sqlStringCommand, "AdminShipAddress", DbType.String, AdminShipAddress);
			base.database.AddInParameter(sqlStringCommand, "AdminShipTo", DbType.String, adminShipTo);
			base.database.AddInParameter(sqlStringCommand, "AdminCellPhone", DbType.String, adminCellPhone);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public string GetReplaceComments(string orderId, string SkuId = "")
		{
			orderId = base.GetTrueOrderId(orderId);
			string text = "SELECT Comments FROM Hishop_OrderReplace WHERE HandleStatus=0 AND OrderId = @OrderId";
			if (!string.IsNullOrEmpty(SkuId))
			{
				text += " AND SkuId = @SkuId";
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(text);
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, SkuId);
			object obj = base.database.ExecuteScalar(sqlStringCommand);
			if (obj == null || obj is DBNull)
			{
				return "";
			}
			return obj.ToString();
		}

		public IList<ReplaceInfo> GetReplaceApplysNoPage(ReplaceApplyQuery query)
		{
			query.OrderId = base.GetTrueOrderId(query.OrderId);
			IList<ReplaceInfo> result = new List<ReplaceInfo>();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1");
			if (!string.IsNullOrEmpty(query.OrderId))
			{
				stringBuilder.AppendFormat(" and OrderId = '{0}'", query.OrderId);
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
			if (query.ReplaceId.HasValue)
			{
				stringBuilder.AppendFormat(" AND ReplaceId = {0}", query.ReplaceId.Value);
			}
			if (!string.IsNullOrEmpty(query.ProductName))
			{
				stringBuilder.AppendFormat(" AND ShopName LIKE '%{0}%'", query.ProductName);
			}
			if (!string.IsNullOrEmpty(query.ReplaceIds))
			{
				stringBuilder.Append($" AND ReplaceId IN({query.ReplaceIds}) ");
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM vw_Hishop_OrderReplace WHERE " + stringBuilder.ToString());
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<ReplaceInfo>(objReader);
			}
			return result;
		}

		public PageModel<ReplaceInfo> GetReplaceApplys(ReplaceApplyQuery query)
		{
			query.OrderId = base.GetTrueOrderId(query.OrderId);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1");
			if (!string.IsNullOrEmpty(query.OrderId))
			{
				stringBuilder.AppendFormat(" and OrderId = '{0}'", query.OrderId);
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
			if (query.ReplaceId.HasValue)
			{
				stringBuilder.AppendFormat(" AND ReplaceId = {0}", query.ReplaceId.Value);
			}
			if (!string.IsNullOrEmpty(query.ProductName))
			{
				stringBuilder.AppendFormat(" AND ShopName LIKE '%{0}%'", query.ProductName);
			}
			return DataHelper.PagingByRownumber<ReplaceInfo>(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_OrderReplace", "ReplaceId", stringBuilder.ToString(), "*");
		}

		public bool DelReplaceApply(int replaceId)
		{
			string query = $"DELETE FROM Hishop_OrderReplace WHERE ReplaceId={replaceId} AND (HandleStatus = {2} OR HandleStatus = {1}) ";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool ApplyForReplace(ReplaceInfo replace)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("UPDATE Hishop_OrderItems SET Status = @ItemStatus WHERE OrderId = @OrderId AND SkuId = @SkuId;");
			stringBuilder.Append("DELETE FROM Hishop_OrderReplace WHERE OrderId = @OrderId AND SkuId = @SkuId;");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, replace.OrderId);
			base.database.AddInParameter(sqlStringCommand, "ItemStatus", DbType.Int32, 30);
			base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, replace.SkuId);
			if (base.database.ExecuteNonQuery(sqlStringCommand) > 0)
			{
				return this.Add(replace, null) > 0;
			}
			return false;
		}

		public ReplaceInfo GetReplaceInfo(string OrderId, string SkuId = "")
		{
			OrderId = base.GetTrueOrderId(OrderId);
			ReplaceInfo result = null;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT TOP 1 * FROM Hishop_OrderReplace WHERE OrderId = @OrderId");
			if (!string.IsNullOrEmpty(SkuId))
			{
				stringBuilder.Append(" AND SkuId = @SkuID");
			}
			else
			{
				stringBuilder.Append(" AND (SkuId IS NULL OR SkuId='')");
			}
			stringBuilder.Append(" ORDER BY ReplaceId DESC");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, OrderId);
			base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, SkuId);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToModel<ReplaceInfo>(objReader);
			}
			return result;
		}

		public bool FinishReplace(int ReplaceId, string AdminRemark, string OrderId, string skuId = "")
		{
			OrderId = base.GetTrueOrderId(OrderId);
			OrderInfo orderInfo = new OrderDao().GetOrderInfo(OrderId);
			if (orderInfo == null)
			{
				return false;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("UPDATE Hishop_OrderItems SET Status = @ItemStatus WHERE OrderId = @OrderId AND SkuId = @SkuId;");
			stringBuilder.Append("UPDATE Hishop_OrderReplace SET HandleStatus = @HandleStatus,UserConfirmGoodsTime = @HandleTime ,AdminRemark = @AdminRemark WHERE ReplaceId = @ReplaceId;");
			stringBuilder.AppendFormat("UPDATE Hishop_Orders SET FinishDate = CASE WHEN FinishDate IS NULL THEN GETDATE() ELSE FinishDate END WHERE OrderId = @OrderId AND OrderStatus = {0};", 5);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 1);
			base.database.AddInParameter(sqlStringCommand, "HandleTime", DbType.DateTime, DateTime.Now);
			base.database.AddInParameter(sqlStringCommand, "AdminRemark", DbType.String, AdminRemark);
			base.database.AddInParameter(sqlStringCommand, "ReplaceId", DbType.Int32, ReplaceId);
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, OrderId);
			base.database.AddInParameter(sqlStringCommand, "ItemStatus", DbType.Int32, 34);
			base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UserSendGoods(int ReplaceId, string ExpressCompanyAbb, string ExpressCompanyName, string ShipOrderNumber, string OrderId, string SkuId = "")
		{
			OrderId = base.GetTrueOrderId(OrderId);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("UPDATE Hishop_OrderItems SET Status = @ItemStatus WHERE OrderId = @OrderId AND SkuId = @SkuId;");
			stringBuilder.Append("UPDATE Hishop_OrderReplace SET HandleStatus = @HandleStatus,UserSendGoodsTime = @HandleTime ,UserExpressCompanyAbb = @UserExpressCompanyAbb,UserExpressCompanyName = @UserExpressCompanyName,UserShipOrderNumber = @UserShipOrderNumber WHERE ReplaceId = @ReplaceId");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 4);
			base.database.AddInParameter(sqlStringCommand, "HandleTime", DbType.DateTime, DateTime.Now);
			base.database.AddInParameter(sqlStringCommand, "UserExpressCompanyAbb", DbType.String, ExpressCompanyAbb);
			base.database.AddInParameter(sqlStringCommand, "UserExpressCompanyName", DbType.String, ExpressCompanyName);
			base.database.AddInParameter(sqlStringCommand, "UserShipOrderNumber", DbType.String, ShipOrderNumber);
			base.database.AddInParameter(sqlStringCommand, "ReplaceId", DbType.Int32, ReplaceId);
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, OrderId);
			base.database.AddInParameter(sqlStringCommand, "ItemStatus", DbType.Int32, 32);
			base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, SkuId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool ShopSendGoods(int ReplaceId, string ExpressCompanyAbb, string ExpressCompanyName, string ShipOrderNumber, string OrderId, string SkuId = "")
		{
			OrderId = base.GetTrueOrderId(OrderId);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("UPDATE Hishop_OrderItems SET Status = @ItemStatus WHERE OrderId = @OrderId AND SkuId = @SkuId;");
			stringBuilder.Append("UPDATE Hishop_OrderReplace SET HandleStatus = @HandleStatus,MerchantsConfirmGoodsTime = @HandleTime ,ExpressCompanyAbb = @ExpressCompanyAbb,ExpressCompanyName = @ExpressCompanyName,ShipOrderNumber = @ShipOrderNumber WHERE ReplaceId = @ReplaceId");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 6);
			base.database.AddInParameter(sqlStringCommand, "HandleTime", DbType.DateTime, DateTime.Now);
			base.database.AddInParameter(sqlStringCommand, "ExpressCompanyAbb", DbType.String, ExpressCompanyAbb);
			base.database.AddInParameter(sqlStringCommand, "ExpressCompanyName", DbType.String, ExpressCompanyName);
			base.database.AddInParameter(sqlStringCommand, "ShipOrderNumber", DbType.String, ShipOrderNumber);
			base.database.AddInParameter(sqlStringCommand, "ReplaceId", DbType.Int32, ReplaceId);
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, OrderId);
			base.database.AddInParameter(sqlStringCommand, "ItemStatus", DbType.Int32, 33);
			base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, SkuId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public int GetReplaceId(string orderId, string skuId = "")
		{
			orderId = base.GetTrueOrderId(orderId);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT TOP 1 ReplaceId FROM Hishop_OrderReplace WHERE OrderId = @OrderId");
			if (!string.IsNullOrEmpty(skuId))
			{
				stringBuilder.Append(" AND SkuId = @SkuID");
			}
			else
			{
				stringBuilder.Append(" AND (SkuId IS NULL OR SkuId='')");
			}
			stringBuilder.Append(" ORDER BY ReplaceId DESC");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0);
		}
	}
}
