using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Hidistro.SqlDal.Promotions
{
	public class CountDownDao : BaseDao
	{
		public decimal GetCountDownSalePrice(int countDownId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT min(SalePrice)  FROM Hishop_CountDownSku WHERE CountDownId = @CountDownId");
			base.database.AddInParameter(sqlStringCommand, "CountDownId", DbType.Int32, countDownId);
			return base.database.ExecuteScalar(sqlStringCommand).ToDecimal(0);
		}

		public int GetCountDownSurplusNumber(int countDownId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT sum (TotalCount) - sum (BoughtCount)  FROM Hishop_CountDownSku WHERE CountDownId = @CountDownId");
			base.database.AddInParameter(sqlStringCommand, "CountDownId", DbType.Int32, countDownId);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0);
		}

		public CountDownSkuInfo GetCountDownSkus(int countDownId, string skuId)
		{
			CountDownSkuInfo result = null;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT CountDownSkuId,SkuId,SalePrice,TotalCount AS ActivityTotal,TotalCount,CountDownId,BoughtCount FROM Hishop_CountDownSku WHERE SkuId = @SkuId AND CountDownId = @CountDownId");
			base.database.AddInParameter(sqlStringCommand, "CountDownId", DbType.Int32, countDownId);
			base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateCountDownSku(dataReader);
				}
			}
			return result;
		}

		public DataTable GetCountDownSkusByProdctId(int countDownId)
		{
			return this.GetCountDownSkus(countDownId, 0, false);
		}

		public DataTable GetCountDownSkus(int countDownId, int storeId = 0, bool isOpenStore = false)
		{
			DataTable result = null;
			StringBuilder stringBuilder = new StringBuilder();
			if (storeId == 0)
			{
				if (isOpenStore)
				{
					stringBuilder.Append("SELECT cds.CountDownSkuId,cds.SkuId,cds.SalePrice,(case when cds.TotalCount<=sku.Stock then cds.TotalCount else sku.Stock end) TotalCount,cds.CountDownId,(select isnull(SUM(Quantity),0) from Hishop_OrderItems where SkuId=cds.SkuId AND OrderId in(select OrderId from Hishop_Orders where ParentOrderId<>'-1' AND CountDownBuyId=@CountDownId AND StoreId=0 and OrderStatus<>4) and productId = (select ProductId from Hishop_CountDown where CountDownId=@CountDownId)) as  BoughtCount,sku.SalePrice OldSalePrice FROM Hishop_CountDownSku cds LEFT JOIN Hishop_SKUs sku ON cds.SkuId = sku.SkuId WHERE CountDownId=@CountDownId");
				}
				else
				{
					stringBuilder.Append("SELECT cds.CountDownSkuId,cds.SkuId,cds.SalePrice,cds.TotalCount,cds.CountDownId,cds.BoughtCount,sku.SalePrice OldSalePrice FROM Hishop_CountDownSku cds LEFT JOIN Hishop_SKUs sku ON cds.SkuId = sku.SkuId WHERE CountDownId=@CountDownId");
				}
			}
			else
			{
				stringBuilder.Append("SELECT cds.CountDownSkuId,cds.SkuId,cds.SalePrice,cds.CountDownId,(case when cds.TotalCount<=ssk.Stock then cds.TotalCount else ssk.Stock end) TotalCount,ssk.Stock");
				stringBuilder.AppendFormat(",(select isnull(SUM(Quantity),0) from Hishop_OrderItems where SkuId=cds.SkuId AND OrderId in(select OrderId from Hishop_Orders where ParentOrderId<>'-1' AND CountDownBuyId={0} AND StoreId={1} and OrderStatus<>{2}) and productId = (select ProductId from Hishop_CountDown where CountDownId={0})) as  BoughtCount", countDownId, storeId, 4);
				stringBuilder.AppendFormat(",ssk.StoreSalePrice OldSalePrice FROM Hishop_CountDownSku cds inner join Hishop_StoreSKUs ssk on cds.SkuId=ssk.SkuId");
				stringBuilder.AppendFormat(" WHERE CountDownId=@CountDownId and ssk.StoreId={0}", storeId);
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "CountDownId", DbType.Int32, countDownId);
			using (IDataReader reader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(reader);
			}
			return result;
		}

		public DbQueryResult GetCountDownTotalList(CountDownQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder(" 1=1 ");
			if (query.CountDownId > 0)
			{
				stringBuilder.AppendFormat(" AND CountDownBuyId={0}", query.CountDownId);
			}
			if (query.StoreId >= 0)
			{
				stringBuilder.AppendFormat(" AND o.StoreId={0}", query.StoreId);
			}
			if (query.OrderState != 0)
			{
				stringBuilder.AppendFormat(" AND o.OrderStatus={0}", (int)query.OrderState);
			}
			string selectFields = "o.*,s.StoreName,(SELECT top 1  quantity FROM Hishop_OrderItems WHERE OrderId =o.OrderId ) as Quantity";
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_Orders o inner join vw_Hishop_StoreForPromotion s on s.StoreId=o.storeId", "OrderId", stringBuilder.ToString(), selectFields);
		}

		public DbQueryResult GetCountDownList(CountDownQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1 ");
			if (!string.IsNullOrEmpty(query.ProductName))
			{
				stringBuilder.AppendFormat(" AND ProductName Like '%{0}%'", DataHelper.CleanSearchString(query.ProductName));
			}
			switch (query.State)
			{
			case 0:
				stringBuilder.Append(" AND getdate() BETWEEN StartDate AND Enddate");
				break;
			case 1:
				stringBuilder.Append(" AND StartDate > getdate()");
				break;
			case 2:
				stringBuilder.Append(" AND Enddate < getdate()");
				break;
			}
			string selectFields = "*";
			return DataHelper.PagingByTopnotin(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_CountDown", "CountDownId", stringBuilder.ToString(), selectFields);
		}

		public bool DeleteCountDown(int countDownId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM Hishop_CountDown WHERE CountDownId=@CountDownId");
			base.database.AddInParameter(sqlStringCommand, "CountDownId", DbType.Int32, countDownId);
			base.database.ExecuteNonQuery(sqlStringCommand);
			sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM [Hishop_StoreActivitys] WHERE ActivityId = @CountDownId and ActivityType=2");
			base.database.AddInParameter(sqlStringCommand, "CountDownId", DbType.Int32, countDownId);
			base.database.ExecuteNonQuery(sqlStringCommand);
			sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM Hishop_CountDownSku WHERE CountDownId = @CountDownId");
			base.database.AddInParameter(sqlStringCommand, "CountDownId", DbType.Int32, countDownId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public void SetOverCountDown(int countDownId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE Hishop_CountDown SET EndDate = getdate() WHERE CountDownId = @CountDownId");
			base.database.AddInParameter(sqlStringCommand, "CountDownId", DbType.Int32, countDownId);
			base.database.ExecuteNonQuery(sqlStringCommand);
		}

		public void EditCountDown(CountDownInfo countDownInfo, IEnumerable<CountDownSkuInfo> countDownSkus)
		{
			if (this.Update(countDownInfo, null) && countDownSkus.Count() != 0)
			{
				DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM Hishop_CountDownSku WHERE CountDownId=@CountDownId");
				base.database.AddInParameter(sqlStringCommand, "CountDownId", DbType.Int32, countDownInfo.CountDownId);
				base.database.ExecuteNonQuery(sqlStringCommand);
				foreach (CountDownSkuInfo countDownSku in countDownSkus)
				{
					this.Add(countDownSku, null);
				}
			}
		}

		public int AddCountDown(CountDownInfo countDownInfo, IEnumerable<CountDownSkuInfo> countDownSkus)
		{
			int countDownId = (int)this.Add(countDownInfo, null);
			if (countDownId == 0)
			{
				return 0;
			}
			countDownSkus.ForEach(delegate(CountDownSkuInfo item)
			{
				item.CountDownId = countDownId;
				this.Add(item, null);
			});
			return countDownId;
		}

		public bool ProductCountDownExist(int productId, DateTime startDate, DateTime endDate)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT COUNT (1) FROM Hishop_CountDown WHERE ProductId = @ProductId AND EndDate > @EndDate");
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			base.database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, endDate);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0) > 0;
		}

		public bool ProductCountDownExist(int productId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT TOP 1 CASE WHEN EndDate > getdate() THEN 1 ELSE 0 END dateFlag  FROM Hishop_CountDown WHERE ProductId = @ProductId ORDER BY EndDate DESC");
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0) > 0;
		}

		public bool ProductCountDownExist(int productId, DateTime startDate, int countDownId, DateTime endDate)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT COUNT (1) FROM Hishop_CountDown WHERE CountDownId<>@CountDownId And ProductId = @ProductId AND EndDate > @EndDate");
			base.database.AddInParameter(sqlStringCommand, "CountDownId", DbType.Int32, countDownId);
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			base.database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, endDate);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0) > 0;
		}

		public string GetCountDownActiveProducts()
		{
			List<string> list = new List<string>();
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT ProductId FROM Hishop_CountDown WHERE EndDate > getdate ()");
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(((IDataRecord)dataReader)[0].ToString());
				}
			}
			return string.Join(",", list.ToArray());
		}

		public CountDownInfo ProductExistsCountDown(int productId, string skuId = "", int storeId = 0, bool isOpenMultStore = false)
		{
			CountDownInfo countDownInfo = null;
			string text = "SELECT * FROM Hishop_CountDown WHERE ProductId = @ProductId AND getdate () BETWEEN startDate AND EndDate";
			if (isOpenMultStore)
			{
				text = ((storeId <= 0) ? (text + "  AND  (storeType=0 OR storeType=1 OR CountDownId IN (SELECT ActivityId FROM Hishop_StoreActivitys WHERE ActivityId=Hishop_CountDown.CountDownId and ActivityType=2 and StoreId=0))  ") : (text + " AND  (storeType=1 OR CountDownId IN (SELECT ActivityId FROM Hishop_StoreActivitys where ActivityId=Hishop_CountDown.CountDownId and ActivityType=2 and StoreId=@StoreId))"));
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(text);
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, storeId);
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					countDownInfo = DataMapper.PopulateCountDown(dataReader);
				}
			}
			if (!string.IsNullOrEmpty(skuId) && countDownInfo != null && storeId == 0 && !isOpenMultStore)
			{
				sqlStringCommand = base.database.GetSqlStringCommand("select CountDownSkuId, SkuId, SalePrice, TotalCount, CountDownId, BoughtCount  From  Hishop_CountDownSku where  CountDownId=@CountDownId and SkuId=@SkuId");
				base.database.AddInParameter(sqlStringCommand, "CountDownId", DbType.Int32, countDownInfo.CountDownId);
				base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
				using (IDataReader dataReader2 = base.database.ExecuteReader(sqlStringCommand))
				{
					if (dataReader2.Read())
					{
						CountDownSkuInfo countDownSkuInfo = DataMapper.PopulateCountDownSku(dataReader2);
						return (countDownSkuInfo.TotalCount > countDownSkuInfo.BoughtCount) ? countDownInfo : null;
					}
				}
			}
			return countDownInfo;
		}

		public bool CheckDuplicateBuyCountDown(int productId, int userId)
		{
			string query = "SELECT COUNT (1)  FROM Hishop_Orders O INNER JOIN Hishop_CountDown CD ON O.CountDownBuyId = CD.CountDownId WHERE  O.OrderStatus <> @OrderStatus AND  O.UserId = @UserId  AND O.ParentOrderId<>'-1' AND CD.ProductId = @ProductId AND getdate () BETWEEN CD.startdate AND CD.enddate";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			base.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 4);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0) > 0;
		}

		public bool CheckDuplicateBuyCountDown(int maxCount, int countDownId, int userId, string orderId, int buyAmount, int storeId = 0)
		{
			orderId = base.GetTrueOrderId(orderId);
			string query = "select SUM(Quantity) from Hishop_OrderItems where OrderId in(select OrderId from Hishop_Orders where UserId=@UserId and ParentOrderId<>'-1' AND CountDownBuyId=@CountDownId AND StoreId=@StoreId and OrderStatus<>@OrderStatus) and productId = (select ProductId from Hishop_CountDown where CountDownId=@CountDownId)";
			if (!string.IsNullOrEmpty(orderId))
			{
				query = "select SUM(Quantity) from Hishop_OrderItems where OrderId in(select OrderId from Hishop_Orders where UserId=@UserId and ParentOrderId<>'-1' AND CountDownBuyId=@CountDownId AND StoreId=@StoreId and OrderStatus<>@OrderStatus and OrderId<>@OrderId) and productId = (select ProductId from Hishop_CountDown where CountDownId=@CountDownId)";
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "CountDownId", DbType.Int32, countDownId);
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			base.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 4);
			base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, storeId);
			if (!string.IsNullOrEmpty(orderId))
			{
				base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			}
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0) + buyAmount > maxCount;
		}

		public int GetCountDownOrderCount(int countDownId, string skuId, int storeId = 0)
		{
			string query = "select SUM(Quantity) from Hishop_OrderItems where SkuId=@SkuId AND OrderId in(select OrderId from Hishop_Orders where ParentOrderId<>'-1' AND CountDownBuyId=@CountDownId AND StoreId=@StoreId and OrderStatus<>@OrderStatus) and productId = (select ProductId from Hishop_CountDown where CountDownId=@CountDownId)";
			if (storeId == -1)
			{
				query = "select SUM(Quantity) from Hishop_OrderItems where SkuId=@SkuId AND OrderId in(select OrderId from Hishop_Orders where ParentOrderId<>'-1' AND CountDownBuyId=@CountDownId and OrderStatus<>@OrderStatus) and productId = (select ProductId from Hishop_CountDown where CountDownId=@CountDownId)";
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
			base.database.AddInParameter(sqlStringCommand, "CountDownId", DbType.Int32, countDownId);
			base.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 4);
			base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, storeId);
			base.database.AddInParameter(sqlStringCommand, "ReturnedStatus", DbType.Int32, 24.GetHashCode());
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0);
		}

		public bool CountDownInfoIsOver(int productId, int countDownId, bool isOpenMultileStore, int storeId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT COUNT(1) FROM Hishop_CountDown WHERE ProductId=@ProductId AND CountDownId=@CountDownId AND getdate() BETWEEN  StartDate AND EndDate");
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			base.database.AddInParameter(sqlStringCommand, "CountDownId", DbType.Int32, countDownId);
			bool flag = base.database.ExecuteScalar(sqlStringCommand).ToInt(0) > 0;
			if (!flag)
			{
				return flag;
			}
			sqlStringCommand = (isOpenMultileStore ? base.database.GetSqlStringCommand("SELECT count (1) FROM Hishop_CountDownSku WHERE CountDownId = @CountDownId AND TotalCount > BoughtCount") : base.database.GetSqlStringCommand("SELECT count (1) FROM Hishop_CountDownSku WHERE CountDownId = @CountDownId AND TotalCount > BoughtCount"));
			base.database.AddInParameter(sqlStringCommand, "CountDownId", DbType.Int32, countDownId);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0) > 0;
		}

		public bool IsActiveCountDownByProductId(int productId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT COUNT(1) FROM Hishop_CountDown WHERE getdate() BETWEEN StartDate AND EndDate  AND ProductId=@ProductId ");
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0) > 0;
		}

		public CountDownInfo ActiveCountDownByProductId(int productId)
		{
			CountDownInfo result = null;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM Hishop_CountDown WHERE getdate() BETWEEN StartDate AND EndDate  AND ProductId=@ProductId ");
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateCountDown(dataReader);
				}
			}
			return result;
		}

		public CountDownInfo ActiveCountDownByProductId(int productId, int storeId)
		{
			CountDownInfo result = null;
			StringBuilder stringBuilder = new StringBuilder("SELECT * FROM Hishop_CountDown c ");
			stringBuilder.AppendFormat("WHERE EndDate>=getdate() AND ProductId ={0} ", productId);
			stringBuilder.AppendFormat(" and(StoreType = 1 or(select COUNT(1) from dbo.Hishop_StoreActivitys where StoreId ={0} and ActivityId = c.CountDownId and ActivityType = 2) > 0)", storeId);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateCountDown(dataReader);
				}
			}
			return result;
		}

		public bool IsActiveCountDownByProductId(int productId, string skuId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT HasSKU FROM Hishop_products WHERE ProductId = @ProductId");
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			bool flag = base.database.ExecuteScalar(sqlStringCommand).ToBool();
			sqlStringCommand = base.database.GetSqlStringCommand("SELECT top 1 CountDownId FROM Hishop_CountDown WHERE getdate() BETWEEN StartDate AND EndDate  AND ProductId=@ProductId ");
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			int num = base.database.ExecuteScalar(sqlStringCommand).ToInt(0);
			if (!flag)
			{
				return num > 0;
			}
			sqlStringCommand = base.database.GetSqlStringCommand("SELECT count (1) FROM Hishop_CountDownSku where CountDownId=@CountDownId AND SkuId=@SkuId ");
			base.database.AddInParameter(sqlStringCommand, "CountDownId", DbType.Int32, num);
			base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0) > 0;
		}

		public bool IsActiveGroupBuyByProductId(int productId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT COUNT(1) FROM Hishop_GroupBuy WHERE getdate() BETWEEN StartDate AND EndDate  AND ProductId=@ProductId AND Status = 1");
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0) > 0;
		}

		public CountDownInfo GetCountDownInfoNoLimit(int productId)
		{
			CountDownInfo result = null;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM Hishop_CountDown WHERE datediff(hh,EndDate,getdate())<=0 AND ProductId=@ProductId ");
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateCountDown(dataReader);
				}
			}
			return result;
		}

		public byte ProductIsJoinCountDown(int productId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT isnull(max(CountDownId),0)  FROM [Hishop_CountDown] where ProductId=@ProductId and EndDate>GETDATE() and (StoreType in(0,1) or (StoreType=2 and CountDownId in (select ActivityId from Hishop_StoreActivitys where StoreId=0 and ActivityType=2 and [Hishop_CountDown].CountDownId=ActivityId)))");
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			int num = base.database.ExecuteScalar(sqlStringCommand).ToInt(0);
			if (num > 0)
			{
				return 1;
			}
			sqlStringCommand.CommandText = $"SELECT isnull(max(CountDownId),0)  FROM [Hishop_CountDown] where ProductId={productId} and EndDate>GETDATE() and (StoreType=2 and CountDownId in (select ActivityId from Hishop_StoreActivitys where StoreId>0 and ActivityType=2 and [Hishop_CountDown].CountDownId=ActivityId))";
			num = base.database.ExecuteScalar(sqlStringCommand).ToInt(0);
			if (num > 0)
			{
				return 2;
			}
			return 0;
		}

		public DbQueryResult GetCountDownProductList(ProductBrowseQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder($"  ProductId IN(SELECT ProductId FROM Hishop_Products WHERE SaleStatus={1}) AND EndDate >= getdate()");
			if (query.StoreId == 0)
			{
				stringBuilder.Append(" and  (storeType=0 or storeType=1 or CountDownId in (select ActivityId from Hishop_StoreActivitys where ActivityId=vw_Hishop_CountDown.CountDownId and ActivityType=2 and StoreId=0) )");
			}
			else
			{
				stringBuilder.AppendFormat(" and  (storeType=1 or CountDownId in (select ActivityId from Hishop_StoreActivitys where ActivityId=vw_Hishop_CountDown.CountDownId and ActivityType=2 and StoreId={0}) )", query.StoreId);
			}
			return DataHelper.PagingByTopsort(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_CountDown", "CountDownId", stringBuilder.ToString(), "*");
		}

		public DataTable GetCountDownProductList(CategoryInfo category, string keyWord, int page, int size, int storeid, out int total, bool onlyUnFinished = true)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("a.CountDownId,a.ProductId,a.ProductName,b.ProductCode,b.ShortDescription,a.CountDownPrice,a.TotalCount,a.BoughtCount,a.StartDate,a.EndDate,a.DisplaySequence,");
			stringBuilder.Append(" a.ThumbnailUrl60,a.ThumbnailUrl100,a.ThumbnailUrl160,a.ThumbnailUrl180,a.ThumbnailUrl220,a.ThumbnailUrl310,a.ThumbnailUrl410,a.MarketPrice,b.SalePrice,a.MaxCount,");
			StringBuilder stringBuilder2 = new StringBuilder();
			stringBuilder2.Append(" vw_Hishop_CountDown a left join vw_Hishop_BrowseProductList b on a.ProductId = b.ProductId  ");
			StringBuilder stringBuilder3 = new StringBuilder(" SaleStatus=1 AND a.EndDate >= getdate()");
			if (onlyUnFinished)
			{
				string arg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
				stringBuilder3.AppendFormat(" AND ( a.StartDate <= '{0}' ) AND ( a.EndDate >= '{0}') ", arg);
			}
			if (category != null)
			{
				stringBuilder3.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%' OR ExtendCategoryPath LIKE '{0}|%' OR ExtendCategoryPath1 LIKE '{0}|%' OR ExtendCategoryPath2 LIKE '{0}|%' OR ExtendCategoryPath3 LIKE '{0}|%' OR ExtendCategoryPath4 LIKE '{0}|%') ", category.Path);
			}
			if (!string.IsNullOrEmpty(keyWord))
			{
				stringBuilder3.AppendFormat(" AND (ProductName LIKE '%{0}%' OR ProductCode LIKE '%{0}%')", keyWord);
			}
			if (storeid == 0)
			{
				stringBuilder.Append(" 0 as StoreId ");
				stringBuilder3.Append(" and  (storeType=0 or storeType=1 or CountDownId in (select ActivityId from Hishop_StoreActivitys where ActivityId=a.CountDownId and ActivityType=2 and StoreId=0) )");
			}
			else
			{
				stringBuilder.AppendFormat(" '{0}' as StoreId ", storeid);
				stringBuilder3.AppendFormat(" and  (storeType=1 or CountDownId in (select ActivityId from Hishop_StoreActivitys where ActivityId=a.CountDownId and ActivityType=2 and StoreId={0}) )", storeid);
			}
			string sortBy = "a.DisplaySequence";
			DbQueryResult dbQueryResult = DataHelper.PagingByRownumber(page, size, sortBy, SortAction.Desc, true, stringBuilder2.ToString(), "CountDownId", stringBuilder3.ToString(), stringBuilder.ToString());
			DataTable data = dbQueryResult.Data;
			total = dbQueryResult.TotalRecords;
			return data;
		}

		public DataTable GetCounDownProducList(int maxnum)
		{
			DataTable result = new DataTable();
			string query = string.Format("select top " + maxnum + " CountDownId,ProductId,ProductName,SalePrice,CountDownPrice,StartDate,EndDate,MarketPrice, ThumbnailUrl60,ThumbnailUrl100, ThumbnailUrl160,ThumbnailUrl180, ThumbnailUrl220,ThumbnailUrl310 from vw_Hishop_CountDown where getdate () BETWEEN startDate AND enddate  AND ProductId IN(SELECT ProductId FROM Hishop_Products WHERE SaleStatus={0}) order by DisplaySequence desc", 1);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			using (IDataReader reader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(reader);
			}
			return result;
		}

		public List<CountDownSkuInfo> GetCountDownSkus(int countDownId, int productId, int storeId, bool openMultStore)
		{
			List<CountDownSkuInfo> result = new List<CountDownSkuInfo>();
			StringBuilder stringBuilder = new StringBuilder();
			if (openMultStore)
			{
				LineItemStatus lineItemStatus;
				if (storeId == 0)
				{
					stringBuilder.Append("SELECT cds.CountDownSkuId,cds.SkuId,cds.SalePrice,cds.TotalCount AS ActivityTotal,(case when cds.TotalCount<=sku.Stock then cds.TotalCount else sku.Stock end) TotalCount,cds.CountDownId");
					StringBuilder stringBuilder2 = stringBuilder;
					object[] obj = new object[4]
					{
						countDownId,
						storeId,
						4,
						null
					};
					lineItemStatus = LineItemStatus.Returned;
					obj[3] = lineItemStatus.GetHashCode();
					stringBuilder2.AppendFormat(",(select isnull(SUM(Quantity),0) from Hishop_OrderItems where SkuId=cds.SkuId AND  Status NOT IN({3}) AND OrderId in(select OrderId from Hishop_Orders where ParentOrderId<>'-1' AND CountDownBuyId={0} AND StoreId={1} and OrderStatus<>{2}) and productId = (select ProductId from Hishop_CountDown where CountDownId={0})) as  BoughtCount", obj);
					stringBuilder.AppendFormat(",sku.SalePrice OldSalePrice  FROM Hishop_CountDownSku cds LEFT JOIN Hishop_SKUs sku ON cds.SkuId = sku.SkuId ");
					stringBuilder.AppendFormat(" WHERE CountDownId=@CountDownId ");
				}
				else
				{
					stringBuilder.Append("SELECT cds.CountDownSkuId,cds.SkuId,cds.SalePrice,cds.TotalCount AS ActivityTotal,cds.CountDownId,(case when cds.TotalCount<=ssk.Stock then cds.TotalCount else ssk.Stock end) TotalCount,ssk.Stock");
					StringBuilder stringBuilder3 = stringBuilder;
					object[] obj2 = new object[4]
					{
						countDownId,
						storeId,
						4,
						null
					};
					lineItemStatus = LineItemStatus.Returned;
					obj2[3] = lineItemStatus.GetHashCode();
					stringBuilder3.AppendFormat(",(select isnull(SUM(Quantity),0) from Hishop_OrderItems where SkuId=cds.SkuId AND  Status NOT IN({3}) AND OrderId in(select OrderId from Hishop_Orders where ParentOrderId<>'-1' AND CountDownBuyId={0} AND StoreId={1} and OrderStatus<>{2}) and productId = (select ProductId from Hishop_CountDown where CountDownId={0})) as  BoughtCount", obj2);
					stringBuilder.AppendFormat(",ssk.StoreSalePrice OldSalePrice FROM Hishop_CountDownSku cds inner join Hishop_StoreSKUs ssk on cds.SkuId=ssk.SkuId");
					stringBuilder.AppendFormat(" WHERE CountDownId=@CountDownId and ssk.StoreId={0}", storeId);
				}
			}
			else
			{
				stringBuilder.Append("SELECT cds.CountDownSkuId,cds.SkuId,cds.SalePrice,cds.TotalCount AS ActivityTotal,cds.TotalCount,cds.CountDownId,cds.BoughtCount,sku.SalePrice OldSalePrice ");
				stringBuilder.Append(" FROM Hishop_CountDownSku cds LEFT JOIN Hishop_SKUs sku ON cds.SkuId = sku.SkuId ");
				stringBuilder.AppendFormat(" WHERE CountDownId=@CountDownId ");
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "CountDownId", DbType.Int32, countDownId);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<CountDownSkuInfo>(objReader).ToList();
			}
			return result;
		}

		public DataTable GetCounDownProducListNew(int maxnum)
		{
			DataTable result = new DataTable();
			string query = $"SELECT TOP {maxnum}\r\n       c.*,\r\n       p.ProductName,\r\n       p.MarketPrice,\r\n       p.ThumbnailUrl40,\r\n       p.ThumbnailUrl60,\r\n       p.ThumbnailUrl100,\r\n       p.ThumbnailUrl160,\r\n       p.ThumbnailUrl180,\r\n       p.ThumbnailUrl220,\r\n       p.ThumbnailUrl310,\r\n       p.ThumbnailUrl410,\r\n       bp.SalePrice,\r\n       ( SELECT min (SalePrice) FROM Hishop_CountDownSku cds WHERE cds.CountDownId = c.CountDownId ) CountDownPrice\r\n\r\n  FROM Hishop_CountDown c\r\n       INNER JOIN\r\n       Hishop_Products p\r\n          ON     c.ProductId = p.ProductId\r\n             AND p.SaleStatus = {1}\r\n             AND GETDATE () BETWEEN c.StartDate AND c.EndDate\r\n  INNER JOIN vw_Hishop_BrowseProductList bp ON c.ProductId = bp.ProductId\r\nORDER BY c.DisplaySequence DESC";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			using (IDataReader reader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(reader);
			}
			return result;
		}

		public DataTable GetAppletCounDownProducList()
		{
			DataTable result = new DataTable();
			string query = $"SELECT \r\n       c.*,\r\n       p.ProductName,\r\n       p.ThumbnailUrl160,\r\n       bp.SalePrice,\r\n       ( SELECT min (SalePrice) FROM Hishop_CountDownSku cds WHERE cds.CountDownId = c.CountDownId ) CountDownPrice\r\n\r\n  FROM Hishop_CountDown c\r\n       INNER JOIN\r\n       Hishop_Products p\r\n          ON     c.ProductId = p.ProductId\r\n             AND p.SaleStatus = {1}\r\n             AND GETDATE ()< c.EndDate\r\n  INNER JOIN vw_Hishop_BrowseProductList bp ON c.ProductId = bp.ProductId\r\nORDER BY c.DisplaySequence DESC";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			using (IDataReader reader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(reader);
			}
			return result;
		}

		public int GetActivityStartsImmediatelyAboutCountDown(int productId)
		{
			string query = string.Format("SELECT TOP 1 CountDownId FROM Hishop_CountDown WHERE StartDate > getdate () AND ProductId = @ProductId ORDER BY StartDate");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0);
		}

		public int NearCountDown(int productId)
		{
			string query = string.Format("SELECT TOP 1 CountDownId FROM Hishop_CountDown WHERE  ProductId = @ProductId ORDER BY StartDate desc");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0);
		}

		public int GetActivityStartsImmediatelyAboutGroupBuy(int productId)
		{
			string query = string.Format("SELECT TOP 1 GroupBuyId FROM Hishop_GroupBuy WHERE StartDate > getdate () AND ProductId = @ProductId AND Status = 1 ORDER BY StartDate");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0);
		}

		public int ReductionCountDownBoughtCount(int countDownId, string skuId, int boughtCount, DbTransaction dbTransaction)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE Hishop_CountDownSku SET BoughtCount= CASE WHEN BoughtCount - @BoughtCount < 0 THEN 0 ELSE BoughtCount - @BoughtCount END WHERE CountDownId =@CountDownId AND SkuId=@SkuId ");
			base.database.AddInParameter(sqlStringCommand, "CountDownId", DbType.Int32, countDownId);
			base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
			base.database.AddInParameter(sqlStringCommand, "BoughtCount", DbType.Int32, boughtCount);
			if (dbTransaction != null)
			{
				return base.database.ExecuteNonQuery(sqlStringCommand, dbTransaction);
			}
			return base.database.ExecuteNonQuery(sqlStringCommand);
		}
	}
}
