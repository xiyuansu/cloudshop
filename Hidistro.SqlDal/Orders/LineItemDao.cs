using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Orders;
using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Orders
{
	public class LineItemDao : BaseDao
	{
		public bool AddOrderLineItems(string orderId, ICollection lineItems, DbTransaction dbTran)
		{
			if (lineItems == null || lineItems.Count == 0)
			{
				return false;
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(" ");
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			int num = 0;
			StringBuilder stringBuilder = new StringBuilder();
			foreach (LineItemInfo lineItem in lineItems)
			{
				string text = num.ToString();
				stringBuilder.Append("INSERT INTO Hishop_OrderItems(OrderId, SkuId, ProductId, SKU, Quantity, ShipmentQuantity, CostPrice").Append(",ItemListPrice, ItemAdjustedPrice, ItemDescription, ThumbnailsUrl, Weight, SKUContent, PromotionId, PromotionName").Append(",IsValid,ValidStartDate,ValidEndDate,IsRefund,IsOverRefund) VALUES( @OrderId")
					.Append(",@SkuId")
					.Append(text)
					.Append(",@ProductId")
					.Append(text)
					.Append(",@SKU")
					.Append(text)
					.Append(",@Quantity")
					.Append(text)
					.Append(",@ShipmentQuantity")
					.Append(text)
					.Append(",@CostPrice")
					.Append(text)
					.Append(",@ItemListPrice")
					.Append(text)
					.Append(",@ItemAdjustedPrice")
					.Append(text)
					.Append(",@ItemDescription")
					.Append(text)
					.Append(",@ThumbnailsUrl")
					.Append(text)
					.Append(",@Weight")
					.Append(text)
					.Append(",@SKUContent")
					.Append(text)
					.Append(",@PromotionId")
					.Append(text)
					.Append(",@PromotionName")
					.Append(text)
					.Append(",@IsValid")
					.Append(text)
					.Append(",@ValidStartDate")
					.Append(text)
					.Append(",@ValidEndDate")
					.Append(text)
					.Append(",@IsRefund")
					.Append(text)
					.Append(",@IsOverRefund")
					.Append(text)
					.Append(");");
				base.database.AddInParameter(sqlStringCommand, "SkuId" + text, DbType.String, lineItem.SkuId);
				base.database.AddInParameter(sqlStringCommand, "ProductId" + text, DbType.Int32, lineItem.ProductId);
				base.database.AddInParameter(sqlStringCommand, "SKU" + text, DbType.String, lineItem.SKU);
				base.database.AddInParameter(sqlStringCommand, "Quantity" + text, DbType.Int32, lineItem.Quantity);
				base.database.AddInParameter(sqlStringCommand, "ShipmentQuantity" + text, DbType.Int32, lineItem.ShipmentQuantity);
				base.database.AddInParameter(sqlStringCommand, "CostPrice" + text, DbType.Currency, lineItem.ItemCostPrice);
				base.database.AddInParameter(sqlStringCommand, "ItemListPrice" + text, DbType.Currency, lineItem.ItemListPrice);
				base.database.AddInParameter(sqlStringCommand, "ItemAdjustedPrice" + text, DbType.Currency, lineItem.ItemAdjustedPrice);
				base.database.AddInParameter(sqlStringCommand, "ItemDescription" + text, DbType.String, lineItem.ItemDescription);
				base.database.AddInParameter(sqlStringCommand, "ThumbnailsUrl" + text, DbType.String, lineItem.ThumbnailsUrl);
				base.database.AddInParameter(sqlStringCommand, "Weight" + text, DbType.Decimal, lineItem.ItemWeight);
				base.database.AddInParameter(sqlStringCommand, "SKUContent" + text, DbType.String, lineItem.SKUContent);
				base.database.AddInParameter(sqlStringCommand, "PromotionId" + text, DbType.Int32, lineItem.PromotionId);
				base.database.AddInParameter(sqlStringCommand, "PromotionName" + text, DbType.String, lineItem.PromotionName);
				base.database.AddInParameter(sqlStringCommand, "IsValid" + text, DbType.Int32, lineItem.IsValid);
				base.database.AddInParameter(sqlStringCommand, "ValidStartDate" + text, DbType.DateTime, lineItem.ValidStartDate);
				base.database.AddInParameter(sqlStringCommand, "ValidEndDate" + text, DbType.DateTime, lineItem.ValidEndDate);
				base.database.AddInParameter(sqlStringCommand, "IsRefund" + text, DbType.Int32, lineItem.IsRefund);
				base.database.AddInParameter(sqlStringCommand, "IsOverRefund" + text, DbType.Int32, lineItem.IsOverRefund);
				num++;
				if (num == 50)
				{
					sqlStringCommand.CommandText = stringBuilder.ToString();
					int num2 = (dbTran == null) ? base.database.ExecuteNonQuery(sqlStringCommand) : base.database.ExecuteNonQuery(sqlStringCommand, dbTran);
					if (num2 <= 0)
					{
						return false;
					}
					stringBuilder.Remove(0, stringBuilder.Length);
					sqlStringCommand.Parameters.Clear();
					base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
					num = 0;
				}
			}
			if (stringBuilder.ToString().Length > 0)
			{
				sqlStringCommand.CommandText = stringBuilder.ToString();
				if (dbTran != null)
				{
					return base.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0;
				}
				return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
			}
			return true;
		}

		public bool DeleteLineItem(string skuId, string orderId, DbTransaction dbTran)
		{
			orderId = base.GetTrueOrderId(orderId);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM Hishop_OrderItems WHERE OrderId=@OrderId AND SkuId=@SkuId ");
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
			if (dbTran != null)
			{
				return base.database.ExecuteNonQuery(sqlStringCommand, dbTran) == 1;
			}
			return base.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}

		public bool UpdateLineItem(string orderId, LineItemInfo lineItem, DbTransaction dbTran)
		{
			orderId = base.GetTrueOrderId(orderId);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE Hishop_OrderItems SET ShipmentQuantity=@ShipmentQuantity,ItemAdjustedPrice=@ItemAdjustedPrice,Quantity=@Quantity, PromotionId = NULL, PromotionName = NULL WHERE OrderId=@OrderId AND SkuId=@SkuId;  update o  set OrderCostPrice = (select isnull(sum(oi.ShipmentQuantity * oi.CostPrice), 0) from Hishop_OrderItems oi where o.OrderId = oi.OrderId)  from[Hishop_Orders]  o where OrderId =@OrderId;");
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, lineItem.SkuId);
			base.database.AddInParameter(sqlStringCommand, "ShipmentQuantity", DbType.Int32, lineItem.ShipmentQuantity);
			base.database.AddInParameter(sqlStringCommand, "ItemAdjustedPrice", DbType.Currency, lineItem.ItemAdjustedPrice);
			base.database.AddInParameter(sqlStringCommand, "Quantity", DbType.Int32, lineItem.Quantity);
			if (dbTran != null)
			{
				return base.database.ExecuteNonQuery(sqlStringCommand, dbTran) == 2;
			}
			return base.database.ExecuteNonQuery(sqlStringCommand) == 2;
		}

		public int GetLineItemNumber(int productId)
		{
			string query = $"select count(*) from dbo.Hishop_OrderItems as items left join Hishop_Orders orders on items.OrderId=orders.OrderId where orders.OrderStatus!={1} and orders.OrderStatus!={4} and items.ProductId=@ProductId";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			return (int)base.database.ExecuteScalar(sqlStringCommand);
		}

		public DbQueryResult GetLineItems(Pagination page, int productId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("ProductId = {0} ", productId);
			return DataHelper.PagingByTopsort(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "vw_Hishop_OrderItem", "OrderId", stringBuilder.ToString(), "*");
		}

		public DataTable GetLineItems(int productId, int maxNum)
		{
			DataTable result = new DataTable();
			string query = string.Format("select top " + maxNum + " items.*,orders.PayDate,orders.Username,orders.ShipTo from dbo.Hishop_OrderItems as items left join Hishop_Orders orders on items.OrderId=orders.OrderId where orders.OrderStatus!={0} and orders.OrderStatus!={1} and items.ProductId=@ProductId  order by orders.PayDate desc", 1, 4);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			using (IDataReader reader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(reader);
			}
			return result;
		}

		public DbQueryResult GetLineItems(int productId, int currentPage, int pageSize)
		{
			string table = $"(select items.*,orders.PayDate,orders.Username,orders.ShipTo,M.Picture from dbo.Hishop_OrderItems as items left join Hishop_Orders orders on items.OrderId=orders.OrderId left join aspnet_Members as M on orders.UserId=M.UserId where orders.OrderStatus!={1} and orders.OrderStatus!={4} and items.ProductId={productId} ) as DataTable";
			return DataHelper.PagingByRownumber(currentPage, pageSize, "PayDate", SortAction.Desc, true, table, "OrderId", "", "*");
		}

		public bool IsBuyProduct(int productId, int userId)
		{
			bool result = false;
			try
			{
				string query = "SELECT TOP 1 orders.OrderId FROM Hishop_OrderItems AS items LEFT JOIN Hishop_Orders orders ON items.OrderId = orders.OrderId WHERE ProductId = @ProductId AND orders.UserId = @UserId AND orders.OrderStatus = @OrderStatus";
				DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
				base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
				base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
				base.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 5);
				using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
				{
					if (dataReader.Read())
					{
						result = true;
					}
				}
			}
			catch (Exception)
			{
				result = true;
			}
			return result;
		}

		public int CountDownOrderCount(int productId, int userId, int countDownId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT ISNULL(SUM(quantity),0) AS Quanity FROM Hishop_OrderItems WHERE ProductID=@ProductID AND  OrderId in(SELECT orderid FROM Hishop_Orders WHERE UserID=@UserID AND orderstatus!=@OrderClosed AND orderstatus!=@OrderRefuned AND ISNULL(CountDownBuyId, 0) = @CountDownId)");
			base.database.AddInParameter(sqlStringCommand, "ProductID", DbType.Int32, productId);
			base.database.AddInParameter(sqlStringCommand, "UserID", DbType.Int32, userId);
			base.database.AddInParameter(sqlStringCommand, "OrderClosed", DbType.Int32, 4);
			base.database.AddInParameter(sqlStringCommand, "OrderRefuned", DbType.Int32, 9);
			base.database.AddInParameter(sqlStringCommand, "CountDownId", DbType.Int32, countDownId);
			return (int)base.database.ExecuteScalar(sqlStringCommand);
		}
	}
}
