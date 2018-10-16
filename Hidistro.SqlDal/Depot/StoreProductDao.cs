using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Depot;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Hidistro.SqlDal.Depot
{
	public class StoreProductDao : BaseDao
	{
		public List<StoreProductEntity> GetProductRecommend(int storeId)
		{
			List<StoreProductEntity> result = new List<StoreProductEntity>();
			StringBuilder stringBuilder = new StringBuilder("select top 3  s.ProductId ,p.ProductName,p.ProductType ,p.ThumbnailUrl220,");
			stringBuilder.Append(" (SELECT MIN(SalePrice) FROM dbo.Hishop_SKUs WHERE   ProductId = s.ProductId) AS SalePrice");
			stringBuilder.Append(" from Hishop_StoreProducts   s");
			stringBuilder.Append("  inner join dbo.Hishop_Products p on s.ProductId=p.ProductId");
			stringBuilder.AppendFormat(" where StoreId={0} and s.SaleStatus=1", storeId);
			stringBuilder.Append(" order by s.SaleCounts desc,s.UpdateTime desc");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<StoreProductEntity>(objReader).ToList();
			}
			return result;
		}

		public List<StoreProductEntity> GetProductRecommend(string storeIdList, ProductType productType)
		{
			List<StoreProductEntity> result = new List<StoreProductEntity>();
			StringBuilder stringBuilder = new StringBuilder("select s.StoreId, s.ProductId ,p.ProductName,p.ProductType  ,p.ThumbnailUrl220,case when (SELECT MIN(StoreSalePrice) FROM dbo.Hishop_StoreSKUs WHERE   ProductId = s.ProductId and StoreId=s.StoreId)=0 then (SELECT MIN(SalePrice) FROM dbo.Hishop_SKUs WHERE   ProductId = s.ProductId ) else (SELECT MIN(StoreSalePrice) FROM dbo.Hishop_StoreSKUs WHERE   ProductId = s.ProductId and StoreId=s.StoreId) end AS SalePrice");
			stringBuilder.Append("  from Hishop_StoreProducts s ");
			stringBuilder.Append("  inner join dbo.Hishop_Products p on s.ProductId=p.ProductId");
			stringBuilder.AppendFormat(" where StoreId in (" + storeIdList + ") AND s.SaleStatus = 1 ");
			stringBuilder.Append(" and s.ProductId in (select pdid from GetStoresTop3Products('" + storeIdList + "','" + productType.GetHashCode() + "') as t where t.storeid=s.StoreId )");
			if (productType > ProductType.All)
			{
				stringBuilder.Append(" and p.ProductType=" + productType.GetHashCode());
			}
			stringBuilder.Append(" order by s.SaleCounts desc,s.UpdateTime desc");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<StoreProductEntity>(objReader).ToList();
			}
			return result;
		}

		public DbQueryResult GetStoresForCountDowns(StoreEntityQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("  1=1", query.ProductId);
			if (!string.IsNullOrEmpty(query.Key))
			{
				stringBuilder.AppendFormat(" and StoreName like '%{0}%'", query.Key);
			}
			if (query.TagId > 0)
			{
				stringBuilder.AppendFormat(" and StoreId in(select StoreId from  Hishop_StoreTagRelations  where TagId ={0})", query.TagId);
			}
			if (query.RegionId > 0)
			{
				stringBuilder.AppendFormat(" and [FullRegionPath] like '%,{0}%'", query.RegionId);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, $"dbo.PromotionStatus(vs.StoreId,{query.ProductId},{query.ActivityId})", SortAction.Asc, query.IsCount, "vw_Hishop_StoreForPromotion vs", "StoreId", stringBuilder.ToString(), $"StoreId,StoreName,[address],dbo.PromotionStatus(vs.StoreId,{query.ProductId},{query.ActivityId}) as PromotionStatus ");
		}

		public List<int> GetAllStoresForOrderPromotions(StoreEntityQuery query)
		{
			List<int> list = new List<int>();
			StringBuilder stringBuilder = new StringBuilder("select StoreId from vw_Hishop_StoreForPromotion vs");
			stringBuilder.AppendFormat(" where 0=0");
			if (!string.IsNullOrEmpty(query.Key))
			{
				stringBuilder.AppendFormat(" and StoreName like '%{0}%'", query.Key);
			}
			if (query.TagId > 0)
			{
				stringBuilder.AppendFormat(" and StoreId in(select StoreId from  Hishop_StoreTagRelations  where TagId ={0})", query.TagId);
			}
			if (query.RegionId > 0)
			{
				stringBuilder.AppendFormat(" and [FullRegionPath] like '%,{0}%'", query.RegionId);
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add((int)((IDataRecord)dataReader)["StoreId"]);
				}
			}
			return list;
		}

		public DbQueryResult GetStoresForOrderPromotions(StoreEntityQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("  1=1", query.ProductId);
			if (!string.IsNullOrEmpty(query.Key))
			{
				stringBuilder.AppendFormat(" and StoreName like '%{0}%'", query.Key);
			}
			if (query.TagId > 0)
			{
				stringBuilder.AppendFormat(" and StoreId in(select StoreId from  Hishop_StoreTagRelations  where TagId ={0})", query.TagId);
			}
			if (query.RegionId > 0)
			{
				stringBuilder.AppendFormat(" and [FullRegionPath] like '%,{0}%'", query.RegionId);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, "StoreId", SortAction.Asc, query.IsCount, "vw_Hishop_StoreForPromotion vs", "StoreId", stringBuilder.ToString(), "StoreId,StoreName,[address],0 as PromotionStatus ");
		}

		public List<StoreBaseEntity> GetRecomStoreByCountdownProductId(StoreEntityQuery query)
		{
			List<StoreBaseEntity> list = new List<StoreBaseEntity>();
			StringBuilder stringBuilder = new StringBuilder("select top 10  s.StoreId,s.StoreName");
			stringBuilder.AppendFormat(",dbo.GetDistance({0},{1},Latitude,Longitude) as Distance", query.Position.Latitude, query.Position.Longitude);
			stringBuilder.AppendFormat(" from Hishop_Stores s  where  (CloseStatus=1 or (CloseStatus=0 and (getdate()<CloseBeginTime or CloseEndTime<GETDATE())))");
			stringBuilder.AppendFormat(" AND [State]=1 and s.TopRegionId={0} and s.StoreId in (select distinct StoreId from  dbo.Hishop_StoreSKUs where ProductId={1} and Stock>0)", query.RegionId, query.ProductId);
			stringBuilder.AppendFormat(" and exists(select * from Hishop_CountDownSku cds inner join Hishop_StoreSKUs ssk on cds.SkuId=ssk.SkuId where cds.CountDownId={0}  and ssk.StoreId=s.StoreId and (case when cds.TotalCount<=ssk.Stock then cds.TotalCount else ssk.Stock end)>(select isnull(SUM(Quantity),0) from Hishop_OrderItems where SkuId=cds.SkuId AND OrderId in(select OrderId from Hishop_Orders where ParentOrderId<>'-1' AND CountDownBuyId={0} AND StoreId=s.StoreId and OrderStatus<>4)))", query.ActivityId);
			stringBuilder.AppendFormat(" and (IsAboveSelf=1 or IsSupportExpress=1 or (IsStoreDelive=1 and (dbo.GetDistance({0},{1},Latitude,Longitude)<(ServeRadius*1000) or dbo.IsInDeliveryScope(StoreId,'{2}')=1)))", query.Position.Latitude, query.Position.Longitude, query.AreaId);
			stringBuilder.AppendFormat(" and StoreId<>{0} and StoreId in ({1})", query.StoreId, query.Key);
			stringBuilder.AppendFormat("  order by dbo.GetDistance({0},{1},Latitude,Longitude)", query.Position.Latitude, query.Position.Longitude);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(new StoreBaseEntity
					{
						StoreId = ((IDataRecord)dataReader)["StoreId"].ToInt(0),
						StoreName = ((IDataRecord)dataReader)["StoreName"].ToString(),
						Distance = ((IDataRecord)dataReader)["Distance"].ToString()
					});
				}
			}
			return list;
		}

		public List<int> GetAllStoresForCountDowns(StoreEntityQuery query)
		{
			List<int> list = new List<int>();
			StringBuilder stringBuilder = new StringBuilder("select StoreId from vw_Hishop_StoreForPromotion vs");
			stringBuilder.AppendFormat(" where dbo.PromotionStatus(vs.StoreId,{0},{1})=0", query.ProductId, query.ActivityId);
			if (!string.IsNullOrEmpty(query.Key))
			{
				stringBuilder.AppendFormat(" and StoreName like '%{0}%'", query.Key);
			}
			if (query.TagId > 0)
			{
				stringBuilder.AppendFormat(" and StoreId in(select StoreId from  Hishop_StoreTagRelations  where TagId ={0})", query.TagId);
			}
			if (query.RegionId > 0)
			{
				stringBuilder.AppendFormat(" and [FullRegionPath] like '%,{0}%'", query.RegionId);
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add((int)((IDataRecord)dataReader)["StoreId"]);
				}
			}
			return list;
		}

		public DbQueryResult GetStoreRecommend(StoreEntityQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("  TopRegionId={0} and (CloseStatus=1 or (CloseStatus=0 and (getdate()<CloseBeginTime or CloseEndTime<GETDATE()))) AND [State]=1", query.RegionId);
			if (query.TagId > 0)
			{
				stringBuilder.AppendFormat(" and s.StoreId in(SELECT[StoreId]  FROM[dbo].[Hishop_StoreTagRelations] where TagId = {0})", query.TagId);
			}
			stringBuilder.AppendFormat(" and (IsAboveSelf=1 or IsSupportExpress=1 or (IsStoreDelive=1 and (dbo.GetDistance({0},{1},Latitude,Longitude)<(ServeRadius*1000) or dbo.IsInDeliveryScope(StoreId,'{2}')=1)))", query.Position.Latitude, query.Position.Longitude, query.FullAreaPath);
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, $"dbo.GetDistance({query.Position.Latitude},{query.Position.Longitude},Latitude,Longitude)", SortAction.Asc, query.IsCount, "Hishop_Stores s", "StoreId", stringBuilder.ToString(), string.Format("StoreId,StoreName,StoreImages,Latitude,Longitude,FullRegionPath,[address],dbo.GetDistance({0},{1},Latitude,Longitude)as Distance,IsAboveSelf,IsSupportExpress,IsStoreDelive,isnull(MinOrderPrice,-1) MinOrderPrice,isnull(StoreFreight,0) as StoreFreight,(select COUNT(1) from Hishop_StoreProducts sp inner join Hishop_Products ps on sp.ProductId=ps.ProductId where storeid=s.StoreId and sp.SaleStatus=1 {2}) as OnSaleNum ", query.Position.Latitude, query.Position.Longitude, (query.ProductType > ProductType.All) ? ("and ProductType=" + query.ProductType.GetHashCode()) : ""));
		}

		public List<StoreBaseEntity> GetStoreRecommendByProductId(StoreEntityQuery query)
		{
			List<StoreBaseEntity> list = new List<StoreBaseEntity>();
			StringBuilder stringBuilder = new StringBuilder("select top 10  s.StoreId,s.StoreName");
			stringBuilder.AppendFormat(",dbo.GetDistance({0},{1},Latitude,Longitude) as Distance", query.Position.Latitude, query.Position.Longitude);
			stringBuilder.AppendFormat(" from Hishop_Stores s  where  (CloseStatus=1 or (CloseStatus=0 and (getdate()<CloseBeginTime or CloseEndTime<GETDATE())))");
			stringBuilder.AppendFormat(" AND [State]=1 and s.TopRegionId={0} and s.StoreId in (select distinct StoreId from  dbo.Hishop_StoreSKUs where ProductId={1} and Stock>0)", query.RegionId, query.ProductId);
			if (query.StoreId > 0)
			{
				stringBuilder.AppendFormat(" and s.StoreId <>{0}", query.StoreId);
			}
			stringBuilder.AppendFormat(" and (IsAboveSelf=1 or IsSupportExpress=1 or (IsStoreDelive=1 and (dbo.GetDistance({0},{1},Latitude,Longitude)<(ServeRadius*1000) or dbo.IsInDeliveryScope(StoreId,'{2}')=1)))", query.Position.Latitude, query.Position.Longitude, query.AreaId);
			stringBuilder.AppendFormat("  order by dbo.GetDistance({0},{1},Latitude,Longitude)", query.Position.Latitude, query.Position.Longitude);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(new StoreBaseEntity
					{
						StoreId = ((IDataRecord)dataReader)["StoreId"].ToInt(0),
						StoreName = ((IDataRecord)dataReader)["StoreName"].ToString(),
						Distance = ((IDataRecord)dataReader)["Distance"].ToString()
					});
				}
			}
			return list;
		}

		public List<StoreProductEntity> GetProduct4Search(string storeIds, string key, int categoryId, string mainCategoryPath, ProductType productType)
		{
			List<StoreProductEntity> result = new List<StoreProductEntity>();
			if (string.IsNullOrEmpty(key) && categoryId == 0)
			{
				return result;
			}
			StringBuilder stringBuilder = new StringBuilder("select s.StoreId, s.ProductId ,p.ProductName,p.ProductType ,p.ThumbnailUrl220,case when (SELECT MIN(StoreSalePrice) FROM dbo.Hishop_StoreSKUs WHERE   ProductId = s.ProductId and StoreId=s.StoreId)=0 then (SELECT MIN(SalePrice) FROM dbo.Hishop_SKUs WHERE   ProductId = s.ProductId ) else (SELECT MIN(StoreSalePrice) FROM dbo.Hishop_StoreSKUs WHERE   ProductId = s.ProductId and StoreId=s.StoreId) end AS SalePrice");
			stringBuilder.Append("  from Hishop_StoreProducts s ");
			stringBuilder.Append("  inner join dbo.Hishop_Products p on s.ProductId=p.ProductId");
			stringBuilder.AppendFormat(" where s.SaleStatus=1 and  StoreId in (" + storeIds + ") ");
			if (!string.IsNullOrWhiteSpace(key))
			{
				string[] array = Regex.Split(key.Trim(), "\\s+");
				stringBuilder.AppendFormat(" AND (p.ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[0]));
				for (int i = 1; i < array.Length && i <= 4; i++)
				{
					stringBuilder.AppendFormat(" OR p.ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[i]));
				}
				stringBuilder.Append(")");
			}
			if (categoryId > 0 && !string.IsNullOrEmpty(mainCategoryPath))
			{
				string text = $"{mainCategoryPath}|";
				stringBuilder.Append(" and (MainCategoryPath like '" + text + "%' or ExtendCategoryPath like '" + text + "%' or ExtendCategoryPath1 like '" + text + "%'");
				stringBuilder.Append(" or ExtendCategoryPath2 like '" + text + "%' or ExtendCategoryPath3 like '" + text + "%'or ExtendCategoryPath4 like '" + text + "%')");
			}
			if (productType > ProductType.All)
			{
				stringBuilder.Append(" and ProductType=" + productType.GetHashCode());
			}
			stringBuilder.Append(" order by s.SaleCounts desc,s.UpdateTime desc");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<StoreProductEntity>(objReader).ToList();
			}
			return result;
		}

		public DbQueryResult SearchPdInStoreList(StoreEntityQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("   TopRegionId={0} and StoreId in(", query.RegionId);
			stringBuilder.Append("SELECT distinct (sp.StoreId) FROM [Hishop_Products] p");
			stringBuilder.Append(" inner join Hishop_StoreProducts sp on p.ProductId=sp.ProductId");
			stringBuilder.Append(" inner join Hishop_Stores s on s.StoreId=sp.StoreId");
			stringBuilder.Append(" where  s.StoreId IN(SELECT StoreId FROM Hishop_StoreSKUs where ProductID = p.ProductId) ");
			if (!string.IsNullOrWhiteSpace(query.Key))
			{
				string[] array = Regex.Split(query.Key.Trim(), "\\s+");
				stringBuilder.AppendFormat(" AND (p.ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[0]));
				for (int i = 1; i < array.Length && i <= 4; i++)
				{
					stringBuilder.AppendFormat(" AND p.ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[i]));
				}
				stringBuilder.Append(")");
			}
			if (query.CategoryId > 0 && !string.IsNullOrEmpty(query.MainCategoryPath))
			{
				string text = $"{query.MainCategoryPath}|";
				stringBuilder.Append(" and (MainCategoryPath like '" + text + "%' or ExtendCategoryPath like '" + text + "%' or ExtendCategoryPath1 like '" + text + "%'");
				stringBuilder.Append(" or ExtendCategoryPath2 like '" + text + "%' or ExtendCategoryPath3 like '" + text + "%'or ExtendCategoryPath4 like '" + text + "%')");
			}
			if (query.ProductType > ProductType.All)
			{
				stringBuilder.Append(" and ProductType=" + query.ProductType.GetHashCode());
			}
			stringBuilder.Append(" and (s.CloseStatus=1 or (s.CloseStatus=0 and (getdate()<s.CloseBeginTime or s.CloseEndTime<GETDATE()))) and s.[State]=1 and sp.SaleStatus=1");
			stringBuilder.AppendFormat(" and (IsAboveSelf=1 or IsSupportExpress=1 or (IsStoreDelive=1 and (dbo.GetDistance({0},{1},Latitude,Longitude)<(ServeRadius*1000) or dbo.IsInDeliveryScope(s.StoreId,'{2}')=1)))", query.Position.Latitude, query.Position.Longitude, query.AreaId);
			stringBuilder.AppendFormat(" )");
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, $"dbo.GetDistance({query.Position.Latitude},{query.Position.Longitude},Latitude,Longitude)", SortAction.Asc, query.IsCount, "Hishop_Stores s", "s.StoreId", stringBuilder.ToString(), $"StoreId,StoreImages,StoreName,Latitude,Longitude,FullRegionPath,[address],dbo.GetDistance({query.Position.Latitude},{query.Position.Longitude},Latitude,Longitude)as Distance,IsAboveSelf,IsSupportExpress,IsStoreDelive,isnull(MinOrderPrice,-1) MinOrderPrice,isnull(StoreFreight,0) as StoreFreight,(select COUNT(1) from Hishop_StoreProducts where storeid=s.StoreId and SaleStatus=1) as OnSaleNum ");
		}

		public bool UpdateStoreProductSaleCount(int productId, int quantity, int storeId, DbTransaction dbTran = null)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("Update Hishop_StoreProducts SET SaleCounts=SaleCounts+@Quantity WHERE StoreId=@StoreId AND ProductId=@ProductId;");
			base.database.AddInParameter(sqlStringCommand, "Quantity", DbType.Int32, quantity);
			base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, storeId);
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			if (dbTran != null)
			{
				return base.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0;
			}
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool MinusStoreProductSaleCount(int productId, int quantity, int storeId, DbTransaction dbTran = null)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("Update Hishop_StoreProducts SET SaleCounts=SaleCounts-@Quantity WHERE StoreId=@StoreId AND ProductId=@ProductId;");
			base.database.AddInParameter(sqlStringCommand, "Quantity", DbType.Int32, quantity);
			base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, storeId);
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			if (dbTran != null)
			{
				return base.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0;
			}
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
	}
}
