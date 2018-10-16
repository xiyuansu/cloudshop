using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Sales;
using Hidistro.SqlDal.Members;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Hidistro.SqlDal.Commodities
{
	public class ProductDao : BaseDao
	{
		public IList<int> GetProductIds(ProductQuery query)
		{
			IList<int> list = new List<int>();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" AuditStatus=" + 2);
			if (query.SupplierId.HasValue)
			{
				stringBuilder.AppendFormat(" and  SupplierId={0}", query.SupplierId.Value);
			}
			if (query.IsHasStock)
			{
				stringBuilder.AppendFormat(" AND Stock > 0 ");
			}
			if (query.SaleStatus != ProductSaleStatus.All)
			{
				if (query.SaleStatus == ProductSaleStatus.Delete)
				{
					stringBuilder.AppendFormat(" AND SaleStatus = {0}", (int)query.SaleStatus);
				}
				else
				{
					stringBuilder.AppendFormat(" AND SaleStatus = {0}  ", (int)query.SaleStatus);
				}
			}
			else
			{
				stringBuilder.AppendFormat(" AND SaleStatus <> {0}", 0);
			}
			if (query.BrandId.HasValue)
			{
				stringBuilder.AppendFormat(" AND BrandId = {0}", query.BrandId.Value);
			}
			if (query.TypeId.HasValue)
			{
				stringBuilder.AppendFormat(" AND TypeId = {0}", query.TypeId.Value);
			}
			if (query.TagId.HasValue)
			{
				stringBuilder.AppendFormat(" AND ProductId IN (SELECT ProductId FROM Hishop_ProductTag WHERE TagId={0})", query.TagId);
			}
			if (query.IsFilterFightGroupProduct)
			{
				stringBuilder.AppendFormat(" AND ProductId not IN ( SELECT ProductId FROM Hishop_FightGroups WHERE getdate () BETWEEN StartTime AND EndTime OR StartTime > getdate ()UNION ALL SELECT ProductId FROM Hishop_FightGroupActivities WHERE getdate () BETWEEN StartDate AND EndDate OR StartDate > getdate () )");
			}
			if (query.IsFilterPromotionProduct)
			{
				if (!query.IsMobileExclusive)
				{
					stringBuilder.AppendFormat(" AND ProductId not IN ( SELECT DISTINCT pp.ProductId FROM Hishop_Promotions p INNER JOIN Hishop_PromotionProducts pp ON p.ActivityId = pp.ActivityId AND (   p.StartDate > getdate () OR getdate () BETWEEN p.StartDate AND p.EndDate) and p.PromoteType<>{0} )", 7);
				}
				else
				{
					stringBuilder.AppendFormat(" AND ProductId not IN ( SELECT DISTINCT pp.ProductId FROM Hishop_Promotions p INNER JOIN Hishop_PromotionProducts pp ON p.ActivityId = pp.ActivityId AND (   p.StartDate > getdate () OR getdate () BETWEEN p.StartDate AND p.EndDate) and p.PromoteType={0} )", 7);
				}
			}
			if (query.IsFilterGroupBuyProduct)
			{
				stringBuilder.AppendFormat(" AND ProductId not IN (SELECT ProductId FROM Hishop_GroupBuy WHERE Status = {0} )", 1);
			}
			if (query.IsFilterCountDownProduct)
			{
				stringBuilder.AppendFormat(" AND ProductId not IN ( SELECT ProductId FROM Hishop_CountDown WHERE getdate () BETWEEN StartDate AND EndDate OR StartDate > getdate () )");
			}
			if (query.NotInCombinationMainProduct)
			{
				stringBuilder.AppendFormat(" AND ProductId not IN ( SELECT MainProductId FROM Hishop_CombinationBuy WHERE CONVERT(varchar(100), GETDATE(), 111) <= CONVERT(varchar(100), EndDate, 111))");
			}
			if (query.NotInCombinationOtherProduct)
			{
				stringBuilder.AppendFormat(" AND not exists ( SELECT OtherProductIds FROM Hishop_CombinationBuy WHERE CONVERT(varchar(100), GETDATE(), 111) <= CONVERT(varchar(100), EndDate, 111) AND ','+OtherProductIds+',' like '%,'+ cast(p.ProductId as varchar) + ',%')");
			}
			if (query.NotInPreSaleProduct)
			{
				stringBuilder.AppendFormat(" AND ProductId not IN ( SELECT ProductId FROM Hishop_ProductPreSale WHERE PreSaleEndDate>GETDATE())");
			}
			if (!string.IsNullOrEmpty(query.Keywords))
			{
				query.Keywords = DataHelper.CleanSearchString(query.Keywords);
				string[] array = Regex.Split(query.Keywords.Trim(), "\\s+");
				stringBuilder.AppendFormat(" AND (ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[0]));
				for (int i = 1; i < array.Length && i <= 4; i++)
				{
					stringBuilder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[i]));
				}
				stringBuilder.Append(")");
			}
			if (query.IsMakeTaobao.HasValue && query.IsMakeTaobao.Value >= 0)
			{
				stringBuilder.AppendFormat(" AND IsMaketaobao={0}", query.IsMakeTaobao.Value);
			}
			if (query.IsIncludePromotionProduct.HasValue && !query.IsIncludePromotionProduct.Value)
			{
				if (!query.IsMobileExclusive)
				{
					stringBuilder.AppendFormat(" AND ProductId NOT IN (SELECT ProductId FROM Hishop_PromotionProducts WHERE ActivityId IN(SELECT ActivityId FROM dbo.Hishop_Promotions WHERE PromoteType <> {0} ))", 7);
				}
				else
				{
					stringBuilder.AppendFormat(" AND ProductId NOT IN (SELECT ProductId FROM Hishop_PromotionProducts WHERE ActivityId IN(SELECT ActivityId FROM dbo.Hishop_Promotions WHERE PromoteType = {0} ))", 7);
				}
			}
			if (query.IsIncludeHomeProduct.HasValue && !query.IsIncludeHomeProduct.Value)
			{
				stringBuilder.Append(" AND ProductId NOT IN (SELECT ProductId FROM Vshop_HomeProducts)");
			}
			if (!string.IsNullOrEmpty(query.ProductCode))
			{
				stringBuilder.AppendFormat(" AND (ProductId IN (SELECT DISTINCT(ProductId) FROM Hishop_SKUs WHERE SKU LIKE '%{0}%') OR ProductCode LIKE '%{0}%')", DataHelper.CleanSearchString(query.ProductCode));
			}
			if (query.CategoryId.HasValue)
			{
				if (query.CategoryId.Value > 0)
				{
					stringBuilder.AppendFormat(" AND (MainCategoryPath LIKE '{0}|%'  OR ExtendCategoryPath LIKE '{0}|%' OR ExtendCategoryPath1 LIKE '{0}|%' OR ExtendCategoryPath2 LIKE '{0}|%' OR ExtendCategoryPath3 LIKE '{0}|%' OR ExtendCategoryPath4 LIKE '{0}|%') ", query.MaiCategoryPath);
				}
				else
				{
					stringBuilder.Append(" AND (CategoryId = 0 OR CategoryId IS NULL)");
				}
			}
			if (query.StartDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND AddedDate >='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
			}
			if (query.EndDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND AddedDate <='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
			}
			if (query.IsWarningStock)
			{
				stringBuilder.Append(" AND  WarningStockNum>0");
			}
			if (query.IsFilterStoreProducts)
			{
				stringBuilder.AppendFormat(" and ProductId not in (select ProductId from Hishop_StoreSKUs where StoreId={0})", query.StoreId);
			}
			if (!string.IsNullOrEmpty(query.FilterProductIds))
			{
				stringBuilder.AppendFormat(" and ProductId not in ({0})", query.FilterProductIds);
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT ProductId FROM Hishop_Products WHERE " + stringBuilder.ToString());
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(((IDataRecord)dataReader)[0].ToInt(0));
				}
			}
			return list;
		}

		public DbQueryResult GetProducts(ProductQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" AuditStatus=" + 2);
			if (query.SupplierId.HasValue)
			{
				stringBuilder.AppendFormat(" and  SupplierId={0}", query.SupplierId.Value);
			}
			if (query.IsHasStock)
			{
				stringBuilder.AppendFormat(" AND Stock > 0 ");
			}
			if (query.SaleStatus != ProductSaleStatus.All)
			{
				if (query.SaleStatus == ProductSaleStatus.Delete)
				{
					stringBuilder.AppendFormat(" AND SaleStatus = {0}", (int)query.SaleStatus);
				}
				else
				{
					stringBuilder.AppendFormat(" AND SaleStatus = {0}  ", (int)query.SaleStatus);
				}
			}
			else
			{
				stringBuilder.AppendFormat(" AND SaleStatus <> {0}", 0);
			}
			if (query.BrandId.HasValue)
			{
				stringBuilder.AppendFormat(" AND BrandId = {0}", query.BrandId.Value);
			}
			if (query.TypeId.HasValue)
			{
				stringBuilder.AppendFormat(" AND TypeId = {0}", query.TypeId.Value);
			}
			if (query.TagId.HasValue)
			{
				stringBuilder.AppendFormat(" AND ProductId IN (SELECT ProductId FROM Hishop_ProductTag WHERE TagId={0})", query.TagId);
			}
			if (query.IsFilterFightGroupProduct)
			{
				stringBuilder.AppendFormat(" AND ProductId not IN ( SELECT ProductId FROM Hishop_FightGroups WHERE getdate () BETWEEN StartTime AND EndTime OR StartTime > getdate ()UNION ALL SELECT ProductId FROM Hishop_FightGroupActivities WHERE getdate () BETWEEN StartDate AND EndDate OR StartDate > getdate () )");
			}
			if (query.IsFilterPromotionProduct)
			{
				if (!query.IsMobileExclusive)
				{
					stringBuilder.AppendFormat(" AND ProductId not IN ( SELECT DISTINCT pp.ProductId FROM Hishop_Promotions p INNER JOIN Hishop_PromotionProducts pp ON p.ActivityId = pp.ActivityId AND (   p.StartDate > getdate () OR getdate () BETWEEN p.StartDate AND p.EndDate) and p.PromoteType<>{0} )", 7);
				}
				else
				{
					stringBuilder.AppendFormat(" AND ProductId not IN ( SELECT DISTINCT pp.ProductId FROM Hishop_Promotions p INNER JOIN Hishop_PromotionProducts pp ON p.ActivityId = pp.ActivityId AND (   p.StartDate > getdate () OR getdate () BETWEEN p.StartDate AND p.EndDate) and p.PromoteType={0} )", 7);
				}
			}
			if (query.IsFilterGroupBuyProduct)
			{
				stringBuilder.AppendFormat(" AND ProductId not IN (SELECT ProductId FROM Hishop_GroupBuy WHERE Status = {0} )", 1);
			}
			if (query.IsFilterCountDownProduct)
			{
				stringBuilder.AppendFormat(" AND ProductId not IN ( SELECT ProductId FROM Hishop_CountDown WHERE getdate () BETWEEN StartDate AND EndDate OR StartDate > getdate () )");
			}
			if (query.NotInCombinationMainProduct)
			{
				stringBuilder.AppendFormat(" AND ProductId not IN ( SELECT MainProductId FROM Hishop_CombinationBuy WHERE EndDate > GETDATE())");
			}
			if (query.NotInCombinationOtherProduct)
			{
				stringBuilder.AppendFormat(" AND not exists ( SELECT OtherProductIds FROM Hishop_CombinationBuy WHERE  EndDate > GETDATE() AND ','+OtherProductIds+',' like '%,'+ cast(p.ProductId as varchar) + ',%')");
			}
			if (query.NotInPreSaleProduct)
			{
				stringBuilder.AppendFormat(" AND ProductId not IN ( SELECT ProductId FROM Hishop_ProductPreSale WHERE PreSaleEndDate>GETDATE())");
			}
			if (!string.IsNullOrEmpty(query.Keywords))
			{
				query.Keywords = DataHelper.CleanSearchString(query.Keywords);
				string[] array = Regex.Split(query.Keywords.Trim(), "\\s+");
				stringBuilder.AppendFormat(" AND (ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[0]));
				for (int i = 1; i < array.Length && i <= 4; i++)
				{
					stringBuilder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[i]));
				}
				stringBuilder.Append(")");
			}
			if (query.IsMakeTaobao.HasValue && query.IsMakeTaobao.Value >= 0)
			{
				stringBuilder.AppendFormat(" AND IsMaketaobao={0}", query.IsMakeTaobao.Value);
			}
			if (query.IsIncludePromotionProduct.HasValue && !query.IsIncludePromotionProduct.Value)
			{
				if (!query.IsMobileExclusive)
				{
					stringBuilder.AppendFormat(" AND ProductId NOT IN (SELECT ProductId FROM Hishop_PromotionProducts WHERE ActivityId IN(SELECT ActivityId FROM dbo.Hishop_Promotions WHERE PromoteType <> {0} AND EndDate>=GETDATE()))", 7);
				}
				else
				{
					stringBuilder.AppendFormat(" AND ProductId NOT IN (SELECT ProductId FROM Hishop_PromotionProducts WHERE ActivityId IN(SELECT ActivityId FROM dbo.Hishop_Promotions WHERE PromoteType = {0} AND EndDate>=GETDATE()))", 7);
				}
			}
			if (query.IsIncludeHomeProduct.HasValue && !query.IsIncludeHomeProduct.Value)
			{
				stringBuilder.Append(" AND ProductId NOT IN (SELECT ProductId FROM Vshop_HomeProducts) AND ProductType <>" + 1.GetHashCode());
			}
			if (query.IsIncludeAppletProduct.HasValue && !query.IsIncludeAppletProduct.Value)
			{
				stringBuilder.Append(" AND ProductId NOT IN (SELECT ProductId FROM Hishop_AppletChoiceProducts)");
			}
			if (!string.IsNullOrEmpty(query.ProductCode))
			{
				stringBuilder.AppendFormat(" AND (ProductId IN (SELECT DISTINCT(ProductId) FROM Hishop_SKUs WHERE SKU LIKE '%{0}%') OR ProductCode LIKE '%{0}%')", DataHelper.CleanSearchString(query.ProductCode));
			}
			if (query.CategoryId.HasValue)
			{
				if (query.CategoryId.Value > 0)
				{
					stringBuilder.AppendFormat(" AND (MainCategoryPath LIKE '{0}|%'  OR ExtendCategoryPath LIKE '{0}|%' OR ExtendCategoryPath1 LIKE '{0}|%' OR ExtendCategoryPath2 LIKE '{0}|%' OR ExtendCategoryPath3 LIKE '{0}|%' OR ExtendCategoryPath4 LIKE '{0}|%') ", query.MaiCategoryPath);
				}
				else
				{
					stringBuilder.Append(" AND (CategoryId = 0 OR CategoryId IS NULL)");
				}
			}
			if (query.StartDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND AddedDate >='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
			}
			if (query.EndDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND AddedDate <='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
			}
			if (query.IsWarningStock)
			{
				stringBuilder.Append(" AND  WarningStockNum>0");
			}
			if (query.ProductType >= 0)
			{
				stringBuilder.AppendFormat(" AND  ProductType ={0}", query.ProductType);
			}
			if (query.IsFilterStoreProducts)
			{
				stringBuilder.AppendFormat(" and ProductId not in (select ProductId from Hishop_StoreProducts where StoreId={0} and SaleStatus=1 AND ProductId IN(select ProductId from Hishop_StoreSKUs where StoreId={0}))", query.StoreId);
			}
			if (!string.IsNullOrEmpty(query.FilterProductIds))
			{
				stringBuilder.AppendFormat(" and ProductId not in ({0})", query.FilterProductIds);
			}
			if (query.ShippingTemplateId.HasValue)
			{
				if (query.ShippingTemplateId.Value == 0)
				{
					stringBuilder.Append(" AND ISNULL(ShippingTemplateId ,0) = 0");
				}
				else
				{
					stringBuilder.AppendFormat(" AND ShippingTemplateId = {0}", query.ShippingTemplateId.Value);
				}
			}
			string selectFields = "CategoryId,ProductId,BrandId, HasSKU,ProductCode,IsMakeTaobao,ProductName, ThumbnailUrl40,ThumbnailUrl60,ThumbnailUrl310, MarketPrice, SalePrice,ExtendCategoryPath,ExtendCategoryPath1,ExtendCategoryPath2,ExtendCategoryPath3,ExtendCategoryPath4,CostPrice, AddedDate, Stock,WarningStock, DisplaySequence,SaleStatus,SaleCounts,ProductType";
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_BrowseProductList p", "ProductId", stringBuilder.ToString(), selectFields);
		}

		public void SaleOffFromStore(int productId)
		{
			string commandText = string.Format("update Hishop_StoreProducts set SaleStatus={0} where ProductId={1};delete from Hishop_StoreSKUs where  ProductId={1}; ", 2, productId);
			base.database.ExecuteScalar(CommandType.Text, commandText);
		}

		public PageModel<StoreProductBaseModel> GetStoreNoRelationProducts(ProductQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" supplierId=0 AND AuditStatus=" + 2);
			if (query.IsHasStock)
			{
				stringBuilder.AppendFormat(" AND Stock > 0 ");
			}
			if (query.SaleStatus != ProductSaleStatus.All)
			{
				if (query.SaleStatus == ProductSaleStatus.Delete)
				{
					stringBuilder.AppendFormat(" AND SaleStatus = {0}", (int)query.SaleStatus);
				}
				else
				{
					stringBuilder.AppendFormat(" AND SaleStatus = {0}  ", (int)query.SaleStatus);
				}
			}
			else
			{
				stringBuilder.AppendFormat(" AND SaleStatus <> {0}", 0);
			}
			if (query.BrandId.HasValue)
			{
				stringBuilder.AppendFormat(" AND BrandId = {0}", query.BrandId.Value);
			}
			if (query.TypeId.HasValue)
			{
				stringBuilder.AppendFormat(" AND TypeId = {0}", query.TypeId.Value);
			}
			if (query.TagId.HasValue)
			{
				stringBuilder.AppendFormat(" AND ProductId IN (SELECT ProductId FROM Hishop_ProductTag WHERE TagId={0})", query.TagId);
			}
			if (!string.IsNullOrEmpty(query.Keywords))
			{
				query.Keywords = DataHelper.CleanSearchString(query.Keywords);
				string[] array = Regex.Split(query.Keywords.Trim(), "\\s+");
				stringBuilder.AppendFormat(" AND (ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[0]));
				for (int i = 1; i < array.Length && i <= 4; i++)
				{
					stringBuilder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[i]));
				}
				stringBuilder.Append(")");
			}
			if (query.IsMakeTaobao.HasValue && query.IsMakeTaobao.Value >= 0)
			{
				stringBuilder.AppendFormat(" AND IsMaketaobao={0}", query.IsMakeTaobao.Value);
			}
			if (!string.IsNullOrEmpty(query.ProductCode))
			{
				stringBuilder.AppendFormat(" AND (ProductId IN (SELECT DISTINCT(ProductId) FROM Hishop_SKUs WHERE SKU LIKE '%{0}%') OR ProductCode LIKE '%{0}%')", DataHelper.CleanSearchString(query.ProductCode));
			}
			if (query.CategoryId.HasValue)
			{
				if (query.CategoryId.Value > 0)
				{
					stringBuilder.AppendFormat(" AND (MainCategoryPath LIKE '{0}|%'  OR ExtendCategoryPath LIKE '{0}|%' OR ExtendCategoryPath1 LIKE '{0}|%' OR ExtendCategoryPath2 LIKE '{0}|%' OR ExtendCategoryPath3 LIKE '{0}|%' OR ExtendCategoryPath4 LIKE '{0}|%') ", query.MaiCategoryPath);
				}
				else
				{
					stringBuilder.Append(" AND (CategoryId = 0 OR CategoryId IS NULL)");
				}
			}
			if (query.StartDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND AddedDate >='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
			}
			if (query.EndDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND AddedDate <='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
			}
			if (query.IsWarningStock)
			{
				stringBuilder.Append(" AND  WarningStockNum>0");
			}
			if (query.StoreId > 0 && query.IsFilterStoreProducts)
			{
				stringBuilder.AppendFormat(" AND  ProductId NOT IN(SELECT DISTINCT(ProductId) FROM Hishop_StoreSKUs WHERE StoreId = " + query.StoreId + ")");
			}
			if (query.ProductType >= 0)
			{
				stringBuilder.AppendFormat(" AND  ProductType ={0}", query.ProductType);
			}
			if (!string.IsNullOrEmpty(query.FilterProductIds))
			{
				stringBuilder.AppendFormat(" and ProductId not in ({0})", query.FilterProductIds);
			}
			string selectFields = "CategoryId,ProductId,BrandId, HasSKU,ProductCode,ProductName, ThumbnailUrl40,ThumbnailUrl60,ThumbnailUrl310,ThumbnailUrl410 AS ProductImage, MarketPrice, SalePrice AS Price,CostPrice, AddedDate, Stock,WarningStock, DisplaySequence,SaleStatus,SaleCounts,ProductType";
			return DataHelper.PagingByRownumber<StoreProductBaseModel>(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_BrowseProductList", "ProductId", stringBuilder.ToString(), selectFields);
		}

		public bool AuditProducts(string productIds, string reason, bool isPass)
		{
			StringBuilder stringBuilder = new StringBuilder("update Hishop_Products set AuditReson = @reason, AuditStatus = @auditStatus");
			if (isPass)
			{
				stringBuilder.AppendFormat(",SaleStatus={0}", 1);
			}
			stringBuilder.AppendFormat(" where ProductId in ({0})", productIds);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "reason", DbType.String, reason);
			base.database.AddInParameter(sqlStringCommand, "auditStatus", DbType.Int32, isPass ? 2 : 3);
			return productIds.Split(',').Length == base.database.ExecuteNonQuery(sqlStringCommand);
		}

		public bool VerifyProductForAudit(string productIds)
		{
			string commandText = $"select COUNT(*) from Hishop_SKUs where ProductId in({productIds}) and SalePrice=0 ";
			if ((int)base.database.ExecuteScalar(CommandType.Text, commandText) > 0)
			{
				return false;
			}
			commandText = $"select COUNT(*) from Hishop_Products p join  Hishop_Supplier s on p.SupplierId=s.SupplierId where p.ProductId in ({productIds}) and p.AuditStatus={1} and p.SaleStatus={3} and s.[Status]=1";
			int num = (int)base.database.ExecuteScalar(CommandType.Text, commandText);
			return productIds.Split(',').Length == num;
		}

		public Dictionary<string, SKUItem> GetProductSkuInfo(int productId, int gradeId, bool MutiStores = false)
		{
			Dictionary<string, SKUItem> dictionary = new Dictionary<string, SKUItem>();
			StringBuilder stringBuilder = new StringBuilder();
			if (gradeId > 0)
			{
				MemberGradeInfo memberGradeInfo = new MemberGradeDao().Get<MemberGradeInfo>(gradeId);
				int num = memberGradeInfo?.Discount ?? 1;
				if (!MutiStores)
				{
					stringBuilder.Append("SELECT SkuId, ProductId, SKU,Weight, Stock,WarningStock, CostPrice,FreezeStock = ISNULL(FreezeStock,0),StoreStock=Stock,");
				}
				else
				{
					stringBuilder.Append("SELECT SkuId, ProductId, s.SKU,s.Weight, Stock,WarningStock, s.CostPrice,FreezeStock = ISNULL(FreezeStock,0),StoreStock=(SELECT ISNULL(MAX(Stock),s.Stock) FROM Hishop_StoreSKUs WHERE SkuId = s.SkuID and StoreId in (select StoreId from Hishop_Stores where (CloseStatus=1 or (CloseStatus=0 and (getdate()<CloseBeginTime or CloseEndTime<GETDATE()))) and State=1)),");
				}
				stringBuilder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0}) = 1", gradeId);
				stringBuilder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Hishop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice", gradeId, num);
				stringBuilder.Append(" FROM Hishop_SKUs s WHERE ProductId = @ProductId;");
			}
			else if (!MutiStores)
			{
				stringBuilder.Append("SELECT SkuId, ProductId, SKU,Weight, Stock,WarningStock, CostPrice,FreezeStock = ISNULL(FreezeStock,0),StoreStock=Stock, SalePrice FROM Hishop_SKUs WHERE ProductId = @ProductId;");
			}
			else
			{
				stringBuilder.Append("SELECT SkuId, ProductId, SKU,Weight, Stock,WarningStock, CostPrice,FreezeStock = ISNULL(FreezeStock,0),StoreStock=(SELECT ISNULL(MAX(stock),s.Stock) FROM Hishop_StoreSKUs ss WHERE SkuId = s.SkuID and StoreId in (select StoreId from Hishop_Stores where (CloseStatus=1 or (CloseStatus=0 and (getdate()<CloseBeginTime or CloseEndTime<GETDATE()))) and State=1)), SalePrice FROM Hishop_SKUs s WHERE ProductId = @ProductId;");
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					SKUItem sKUItem = DataMapper.PopulateSKU(dataReader);
					if (sKUItem != null)
					{
						dictionary.Add((string)((IDataRecord)dataReader)["SkuId"], sKUItem);
					}
				}
			}
			return dictionary;
		}

		public Dictionary<string, SKUItem> GetProductSkuInfo(int productId, int gradeId, int storeId)
		{
			Dictionary<string, SKUItem> dictionary = new Dictionary<string, SKUItem>();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT skt.SkuId, skt.ProductId, sk.SKU,sk.[Weight], skt.Stock as StoreStock,skt.stock,skt.WarningStock, sk.CostPrice,FreezeStock = ISNULL(skt.FreezeStock,0)");
			if (gradeId > 0)
			{
				MemberGradeInfo memberGradeInfo = new MemberGradeDao().Get<MemberGradeInfo>(gradeId);
				int num = memberGradeInfo?.Discount ?? 1;
				stringBuilder.AppendFormat(",(case StoreSalePrice when 0 then sk.SalePrice else skt.StoreSalePrice end)*{0}/100  as SalePrice", num);
			}
			else
			{
				stringBuilder.Append(",(case StoreSalePrice when 0 then sk.SalePrice else skt.StoreSalePrice end) as SalePrice");
			}
			stringBuilder.Append(" FROM Hishop_StoreSKUs skt");
			stringBuilder.Append(" inner join Hishop_SKUs sk on skt.SkuId=sk.SkuId");
			stringBuilder.Append(" WHERE skt.ProductId = @ProductId and skt.StoreId=@StoreId;");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, storeId);
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					SKUItem sKUItem = DataMapper.PopulateSKU(dataReader);
					if (sKUItem != null)
					{
						dictionary.Add((string)((IDataRecord)dataReader)["SkuId"], sKUItem);
					}
				}
			}
			return dictionary;
		}

		public int GetProductIsWarningStockNum()
		{
			string commandText = $"SELECT COUNT(*) FROM vw_Hishop_BrowseProductList WHERE WarningStockNum > 0 AND SaleStatus <> ({0}) and SupplierId=0";
			return (int)base.database.ExecuteScalar(CommandType.Text, commandText);
		}

		public int GetProductIsWarningStockNum(int SupplierId)
		{
			string commandText = $"SELECT COUNT(*) FROM vw_Hishop_BrowseProductList WHERE WarningStockNum > 0 AND SaleStatus <> ({0}) and SupplierId={SupplierId}";
			return (int)base.database.ExecuteScalar(CommandType.Text, commandText);
		}

		public DataTable GetGroupBuyProducts(ProductQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" WHERE SaleStatus = {0}", 1);
			if (!string.IsNullOrEmpty(query.Keywords))
			{
				query.Keywords = DataHelper.CleanSearchString(query.Keywords);
				string[] array = Regex.Split(query.Keywords.Trim(), "\\s+");
				stringBuilder.AppendFormat(" AND (ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[0]));
				for (int i = 1; i < array.Length && i <= 4; i++)
				{
					stringBuilder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[i]));
				}
				stringBuilder.Append(")");
			}
			if (!string.IsNullOrEmpty(query.ProductCode))
			{
				stringBuilder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
			}
			if (query.CategoryId.HasValue && query.CategoryId.Value > 0)
			{
				stringBuilder.AppendFormat(" AND MainCategoryPath LIKE '{0}|%'", query.MaiCategoryPath);
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT ProductId,ProductName FROM Hishop_Products" + stringBuilder.ToString());
			DataTable result = null;
			using (IDataReader reader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(reader);
			}
			return result;
		}

		public DataTable GetTopProductOrder(int top, string showOrder, int categoryId)
		{
			string query = " SELECT TOP " + top + " *   FROM vw_Hishop_BrowseProductList WHERE SaleStatus=@SaleStatus ORDER BY " + showOrder;
			if (categoryId > 0)
			{
				CategoryInfo categoryInfo = new CategoryDao().Get<CategoryInfo>(categoryId);
				if (categoryInfo != null)
				{
					query = string.Format(" SELECT TOP " + top + " *   FROM vw_Hishop_BrowseProductList WHERE SaleStatus=@SaleStatus AND (MainCategoryPath LIKE '{0}|%' OR ExtendCategoryPath LIKE '{0}|%' OR ExtendCategoryPath1 LIKE '{0}|%' OR ExtendCategoryPath2 LIKE '{0}|%' OR ExtendCategoryPath3 LIKE '{0}|%' OR ExtendCategoryPath4 LIKE '{0}|%') ORDER BY ", categoryInfo.Path) + showOrder;
				}
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "SaleStatus", DbType.Int32, ProductSaleStatus.OnSale);
			DataTable result = null;
			using (IDataReader reader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(reader);
			}
			return result;
		}

		public DataTable GetTopProductByIds(string ids)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (ids.Length > 0)
			{
				string[] array = ids.Split(',');
				string[] array2 = array;
				foreach (string text in array2)
				{
					stringBuilder.Append("select * from vw_Hishop_BrowseProductList where SaleStatus=" + 1 + " and ProductId=" + text + " union all ");
				}
				string query = stringBuilder.ToString().Substring(0, stringBuilder.ToString().LastIndexOf(" union all "));
				DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
				DataTable result = null;
				using (IDataReader reader = base.database.ExecuteReader(sqlStringCommand))
				{
					result = DataHelper.ConverDataReaderToDataTable(reader);
				}
				return result;
			}
			return new DataTable();
		}

		public ProductInfo GetSimpleProductDetail(int productId)
		{
			ProductInfo result = null;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT CategoryId,TypeId,ProductId,ProductName,ProductCode,Unit,SaleStatus,MarketPrice FROM Hishop_Products WHERE ProductId = @ProductId;");
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToModel<ProductInfo>(objReader);
			}
			return result;
		}

		public ProductInfo GetProductDetails(int productId)
		{
			ProductInfo productInfo = this.Get<ProductInfo>(productId);
			if (productInfo != null)
			{
				DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT skus.ProductId, skus.SkuId, s.AttributeId, s.ValueId, skus.SKU, skus.SalePrice,StoreStock=(SELECT ISNULL(MAX(ss.Stock),skus.Stock) FROM Hishop_StoreSKUs ss WHERE skus.SkuId=ss.SkuID), skus.CostPrice, skus.Stock,skus.WarningStock, skus.[Weight] FROM Hishop_SKUItems s right outer join Hishop_SKUs skus on s.SkuId = skus.SkuId WHERE skus.ProductId = @ProductId ORDER BY (SELECT DisplaySequence FROM Hishop_Attributes WHERE AttributeId = s.AttributeId) DESC;SELECT s.SkuId, smp.GradeId, smp.MemberSalePrice FROM Hishop_SKUMemberPrice smp INNER JOIN Hishop_SKUs s ON smp.SkuId=s.SkuId WHERE s.ProductId=@ProductId;");
				base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
				using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
				{
					while (dataReader.Read())
					{
						string key = (string)((IDataRecord)dataReader)["SkuId"];
						if (!productInfo.Skus.ContainsKey(key))
						{
							productInfo.Skus.Add(key, DataMapper.PopulateSKU(dataReader));
						}
						if (((IDataRecord)dataReader)["AttributeId"] != DBNull.Value && ((IDataRecord)dataReader)["ValueId"] != DBNull.Value)
						{
							productInfo.Skus[key].SkuItems.Add((int)((IDataRecord)dataReader)["AttributeId"], (int)((IDataRecord)dataReader)["ValueId"]);
						}
					}
					dataReader.NextResult();
					while (dataReader.Read())
					{
						string key2 = (string)((IDataRecord)dataReader)["SkuId"];
						productInfo.Skus[key2].MemberPrices.Add((int)((IDataRecord)dataReader)["GradeId"], (decimal)((IDataRecord)dataReader)["MemberSalePrice"]);
					}
				}
			}
			return productInfo;
		}

		public ProductInfo GetProductBaseDetails(int productId)
		{
			return this.Get<ProductInfo>(productId);
		}

		public Dictionary<int, IList<int>> GetProductAttributes(int productId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT AttributeId, ValueId FROM Hishop_ProductAttributes WHERE ProductId = @ProductId");
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			Dictionary<int, IList<int>> dictionary = new Dictionary<int, IList<int>>();
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					int key = (int)((IDataRecord)dataReader)["AttributeId"];
					int item = (int)((IDataRecord)dataReader)["ValueId"];
					if (!dictionary.ContainsKey(key))
					{
						IList<int> list = new List<int>();
						list.Add(item);
						dictionary.Add(key, list);
					}
					else
					{
						dictionary[key].Add(item);
					}
				}
			}
			return dictionary;
		}

		public IList<int> GetProductTags(int productId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM Hishop_ProductTag WHERE ProductId=@ProductId");
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			IList<int> list = new List<int>();
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add((int)((IDataRecord)dataReader)["TagId"]);
				}
			}
			return list;
		}

		public IList<ProductInfo> GetProducts(IList<int> productIds)
		{
			IList<ProductInfo> list = new List<ProductInfo>();
			string text = "(";
			foreach (int productId in productIds)
			{
				text = text + productId + ",";
			}
			if (text.Length <= 1)
			{
				return list;
			}
			text = text.Substring(0, text.Length - 1) + ")";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM Hishop_Products WHERE ProductId IN " + text);
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(DataMapper.PopulateProduct(dataReader));
				}
			}
			return list;
		}

		public DataTable GetProductByIds(string productIds)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM Hishop_Products WHERE ProductId IN (" + productIds + ")");
			return base.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}

		public DbQueryResult GetExportProducts(AdvancedProductQuery query, string removeProductIds)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("(");
			if (query.IncludeOnSales)
			{
				stringBuilder.AppendFormat("SaleStatus = {0} OR ", 1);
			}
			if (query.IncludeUnSales)
			{
				stringBuilder.AppendFormat("SaleStatus = {0} OR ", 2);
			}
			if (query.IncludeInStock)
			{
				stringBuilder.AppendFormat("SaleStatus = {0} OR ", 3);
			}
			stringBuilder.Remove(stringBuilder.Length - 4, 4);
			stringBuilder.Append(")");
			if (query.BrandId.HasValue)
			{
				stringBuilder.AppendFormat(" AND BrandId = {0}", query.BrandId.Value);
			}
			if (query.IsMakeTaobao.HasValue && query.IsMakeTaobao != -1)
			{
				stringBuilder.AppendFormat(" AND IsMakeTaobao={0}  ", query.IsMakeTaobao);
			}
			if (!string.IsNullOrEmpty(query.Keywords))
			{
				query.Keywords = DataHelper.CleanSearchString(query.Keywords);
				string[] array = Regex.Split(query.Keywords.Trim(), "\\s+");
				stringBuilder.AppendFormat(" AND (ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[0]));
				for (int i = 1; i < array.Length && i <= 4; i++)
				{
					stringBuilder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[i]));
				}
				stringBuilder.Append(")");
			}
			if (!string.IsNullOrEmpty(query.ProductCode))
			{
				stringBuilder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
			}
			if (query.CategoryId.HasValue && query.CategoryId.Value > 0)
			{
				stringBuilder.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%'  OR ExtendCategoryPath LIKE '{0}|%' OR ExtendCategoryPath1 LIKE '{0}|%' OR ExtendCategoryPath2 LIKE '{0}|%' OR ExtendCategoryPath3 LIKE '{0}|%' OR ExtendCategoryPath4 LIKE '{0}|%')", query.MaiCategoryPath);
			}
			if (query.StartDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND AddedDate >='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
			}
			if (query.EndDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND AddedDate <='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
			}
			if (!string.IsNullOrEmpty(removeProductIds))
			{
				stringBuilder.AppendFormat(" AND ProductId NOT IN ({0})", removeProductIds);
			}
			string selectFields = "ProductId, ProductCode, ProductName, ThumbnailUrl40, MarketPrice, SalePrice, (SELECT CostPrice FROM Hishop_SKUs WHERE SkuId = p.SkuId) AS  CostPrice,  Stock, DisplaySequence";
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_BrowseProductList p", "ProductId", stringBuilder.ToString(), selectFields);
		}

		public DataSet GetExportProducts(AdvancedProductQuery query, bool includeCostPrice, bool includeStock, string removeProductIds)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT a.[ProductId], [TypeId], [ProductName], [ProductCode], [ShortDescription], [Unit], [Description], ").Append("[Title], [Meta_Description], [Meta_Keywords], [SaleStatus], [ImageUrl1], [ImageUrl2], [ImageUrl3], ").Append("[ImageUrl4], [ImageUrl5], [MarketPrice], [HasSKU] ")
				.Append("FROM Hishop_Products a  left join Taobao_Products b on a.productid=b.productid WHERE ");
			stringBuilder.Append("(");
			if (query.IncludeOnSales)
			{
				stringBuilder.AppendFormat("SaleStatus = {0} OR ", 1);
			}
			if (query.IncludeUnSales)
			{
				stringBuilder.AppendFormat("SaleStatus = {0} OR ", 2);
			}
			if (query.IncludeInStock)
			{
				stringBuilder.AppendFormat("SaleStatus = {0} OR ", 3);
			}
			stringBuilder.Remove(stringBuilder.Length - 4, 4);
			stringBuilder.Append(")");
			if (query.IsMakeTaobao.HasValue && query.IsMakeTaobao != -1)
			{
				if (query.IsMakeTaobao == 1)
				{
					stringBuilder.AppendFormat(" AND a.ProductId IN (SELECT ProductId FROM Taobao_Products)");
				}
				else
				{
					stringBuilder.AppendFormat(" AND a.ProductId NOT IN (SELECT ProductId FROM Taobao_Products)");
				}
			}
			if (query.BrandId.HasValue)
			{
				stringBuilder.AppendFormat(" AND BrandId = {0}", query.BrandId.Value);
			}
			if (!string.IsNullOrEmpty(query.Keywords))
			{
				query.Keywords = DataHelper.CleanSearchString(query.Keywords);
				string[] array = Regex.Split(query.Keywords.Trim(), "\\s+");
				stringBuilder.AppendFormat(" AND (ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[0]));
				for (int i = 1; i < array.Length && i <= 4; i++)
				{
					stringBuilder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[i]));
				}
				stringBuilder.Append(")");
			}
			if (!string.IsNullOrEmpty(query.ProductCode))
			{
				stringBuilder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
			}
			if (query.CategoryId.HasValue && query.CategoryId.Value > 0)
			{
				stringBuilder.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%'  OR ExtendCategoryPath LIKE '{0}|%' OR ExtendCategoryPath1 LIKE '{0}|%' OR ExtendCategoryPath2 LIKE '{0}|%' OR ExtendCategoryPath3 LIKE '{0}|%' OR ExtendCategoryPath4 LIKE '{0}|%')", query.MaiCategoryPath);
			}
			if (query.StartDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND AddedDate >='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
			}
			if (query.EndDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND AddedDate <='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
			}
			if (!string.IsNullOrEmpty(removeProductIds))
			{
				stringBuilder.AppendFormat(" AND a.ProductId NOT IN ({0})", removeProductIds);
			}
			stringBuilder.AppendFormat(" ORDER BY {0} {1}", query.SortBy, query.SortOrder);
			DbCommand storedProcCommand = base.database.GetStoredProcCommand("cp_Product_GetExportList");
			base.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, stringBuilder.ToString());
			return base.database.ExecuteDataSet(storedProcCommand);
		}

		public bool UpdateProductSalesCount(OrderInfo order, string skuId = "", int quantity = 0, DbTransaction dbTran = null)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (string.IsNullOrEmpty(skuId))
			{
				foreach (LineItemInfo value in order.LineItems.Values)
				{
					int num = (quantity > 0 && quantity < value.Quantity) ? quantity : value.Quantity;
					stringBuilder.Append("UPDATE Hishop_Products SET SaleCounts = (CASE WHEN SaleCounts - " + num + ">0 THEN SaleCounts - " + num + " ELSE 0 END),ShowSaleCounts = (CASE WHEN ShowSaleCounts - " + num + ">0 THEN ShowSaleCounts - " + num + " ELSE 0 END) WHERE ProductId=" + value.ProductId + ";");
				}
				goto IL_01cf;
			}
			if (order.LineItems.ContainsKey(skuId))
			{
				LineItemInfo lineItemInfo = order.LineItems[skuId];
				int num = (quantity > 0 && quantity < lineItemInfo.Quantity) ? quantity : lineItemInfo.Quantity;
				stringBuilder.Append("UPDATE Hishop_Products SET SaleCounts = (CASE WHEN SaleCounts - " + num + ">0 THEN SaleCounts - " + num + " ELSE 0 END),ShowSaleCounts = (CASE WHEN ShowSaleCounts - " + num + ">0 THEN ShowSaleCounts - " + num + " ELSE 0 END) WHERE ProductId=" + lineItemInfo.ProductId + ";");
				goto IL_01cf;
			}
			return false;
			IL_01cf:
			if (stringBuilder.Length > 0)
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

		public void EnsureMapping(DataSet mappingSet)
		{
			using (DbCommand command = base.database.GetSqlStringCommand("INSERT INTO  Hishop_ProductTypes (TypeName, Remark) VALUES(@TypeName, @Remark);SELECT @@IDENTITY;"))
			{
				base.database.AddInParameter(command, "TypeName", DbType.String);
				base.database.AddInParameter(command, "Remark", DbType.String);
				DataRow[] array = mappingSet.Tables["types"].Select("SelectedTypeId=0");
				DataRow[] array2 = array;
				foreach (DataRow dataRow in array2)
				{
					base.database.SetParameterValue(command, "TypeName", dataRow["TypeName"]);
					base.database.SetParameterValue(command, "Remark", dataRow["Remark"]);
					dataRow["SelectedTypeId"] = base.database.ExecuteScalar(command);
				}
			}
			using (DbCommand command2 = base.database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Hishop_Attributes; INSERT INTO Hishop_Attributes(AttributeName, DisplaySequence, TypeId, UsageMode, UseAttributeImage)  VALUES(@AttributeName, @DisplaySequence, @TypeId, @UsageMode, @UseAttributeImage);SELECT @@IDENTITY;"))
			{
				base.database.AddInParameter(command2, "AttributeName", DbType.String);
				base.database.AddInParameter(command2, "TypeId", DbType.Int32);
				base.database.AddInParameter(command2, "UsageMode", DbType.Int32);
				base.database.AddInParameter(command2, "UseAttributeImage", DbType.Boolean);
				DataRow[] array3 = mappingSet.Tables["attributes"].Select("SelectedAttributeId=0");
				DataRow[] array4 = array3;
				foreach (DataRow dataRow2 in array4)
				{
					int num = (int)mappingSet.Tables["types"].Select(string.Format("MappedTypeId={0}", dataRow2["MappedTypeId"]))[0]["SelectedTypeId"];
					base.database.SetParameterValue(command2, "AttributeName", dataRow2["AttributeName"]);
					base.database.SetParameterValue(command2, "TypeId", num);
					base.database.SetParameterValue(command2, "UsageMode", int.Parse(dataRow2["UsageMode"].ToString()));
					base.database.SetParameterValue(command2, "UseAttributeImage", bool.Parse(dataRow2["UseAttributeImage"].ToString()));
					dataRow2["SelectedAttributeId"] = base.database.ExecuteScalar(command2);
				}
			}
			using (DbCommand command3 = base.database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Hishop_AttributeValues;INSERT INTO Hishop_AttributeValues(AttributeId, DisplaySequence, ValueStr) VALUES(@AttributeId, @DisplaySequence, @ValueStr);SELECT @@IDENTITY;"))
			{
				base.database.AddInParameter(command3, "AttributeId", DbType.Int32);
				base.database.AddInParameter(command3, "ValueStr", DbType.String);
				DataRow[] array5 = mappingSet.Tables["values"].Select("SelectedValueId=0");
				DataRow[] array6 = array5;
				foreach (DataRow dataRow3 in array6)
				{
					int num2 = (int)mappingSet.Tables["attributes"].Select(string.Format("MappedAttributeId={0}", dataRow3["MappedAttributeId"]))[0]["SelectedAttributeId"];
					base.database.SetParameterValue(command3, "AttributeId", num2);
					base.database.SetParameterValue(command3, "ValueStr", dataRow3["ValueStr"]);
					dataRow3["SelectedValueId"] = base.database.ExecuteScalar(command3);
				}
			}
			mappingSet.AcceptChanges();
		}

		private decimal Opreateion(decimal opreation1, decimal opreation2, string operation)
		{
			decimal result = default(decimal);
			switch (operation)
			{
			case "+":
				return opreation1 + opreation2;
			case "-":
				return opreation1 - opreation2;
			case "*":
				return opreation1 * opreation2;
			case "/":
				return opreation1 / opreation2;
			default:
				return result;
			}
		}

		public bool AddProductSKUs(int productId, Dictionary<string, SKUItem> skus, DbTransaction dbTran)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("INSERT INTO Hishop_SKUs(SkuId, ProductId, SKU, Weight, Stock, WarningStock,CostPrice, SalePrice) VALUES(@SkuId, @ProductId, @SKU, @Weight, @Stock,@WarningStock, @CostPrice, @SalePrice)");
			base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String);
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32);
			base.database.AddInParameter(sqlStringCommand, "SKU", DbType.String);
			base.database.AddInParameter(sqlStringCommand, "Weight", DbType.Decimal);
			base.database.AddInParameter(sqlStringCommand, "Stock", DbType.Int32);
			base.database.AddInParameter(sqlStringCommand, "WarningStock", DbType.Int32);
			base.database.AddInParameter(sqlStringCommand, "CostPrice", DbType.Currency);
			base.database.AddInParameter(sqlStringCommand, "SalePrice", DbType.Currency);
			DbCommand sqlStringCommand2 = base.database.GetSqlStringCommand("INSERT INTO Hishop_SKUItems(SkuId, AttributeId, ValueId) VALUES(@SkuId, @AttributeId, @ValueId)");
			base.database.AddInParameter(sqlStringCommand2, "SkuId", DbType.String);
			base.database.AddInParameter(sqlStringCommand2, "AttributeId", DbType.Int32);
			base.database.AddInParameter(sqlStringCommand2, "ValueId", DbType.Int32);
			DbCommand sqlStringCommand3 = base.database.GetSqlStringCommand("INSERT INTO Hishop_SKUMemberPrice(SkuId, GradeId, MemberSalePrice) VALUES(@SkuId, @GradeId, @MemberSalePrice)");
			base.database.AddInParameter(sqlStringCommand3, "SkuId", DbType.String);
			base.database.AddInParameter(sqlStringCommand3, "GradeId", DbType.Int32);
			base.database.AddInParameter(sqlStringCommand3, "MemberSalePrice", DbType.Currency);
			try
			{
				base.database.SetParameterValue(sqlStringCommand, "ProductId", productId);
				foreach (SKUItem value2 in skus.Values)
				{
					string value = productId.ToString(CultureInfo.InvariantCulture) + "_" + value2.SkuId;
					base.database.SetParameterValue(sqlStringCommand, "SkuId", value);
					base.database.SetParameterValue(sqlStringCommand, "SKU", value2.SKU);
					base.database.SetParameterValue(sqlStringCommand, "Weight", value2.Weight);
					base.database.SetParameterValue(sqlStringCommand, "Stock", value2.Stock);
					base.database.SetParameterValue(sqlStringCommand, "WarningStock", value2.WarningStock);
					base.database.SetParameterValue(sqlStringCommand, "CostPrice", value2.CostPrice);
					base.database.SetParameterValue(sqlStringCommand, "SalePrice", value2.SalePrice);
					if (base.database.ExecuteNonQuery(sqlStringCommand, dbTran) == 0)
					{
						return false;
					}
					base.database.SetParameterValue(sqlStringCommand2, "SkuId", value);
					foreach (int key in value2.SkuItems.Keys)
					{
						base.database.SetParameterValue(sqlStringCommand2, "AttributeId", key);
						base.database.SetParameterValue(sqlStringCommand2, "ValueId", value2.SkuItems[key]);
						base.database.ExecuteNonQuery(sqlStringCommand2, dbTran);
					}
					base.database.SetParameterValue(sqlStringCommand3, "SkuId", value);
					foreach (int key2 in value2.MemberPrices.Keys)
					{
						base.database.SetParameterValue(sqlStringCommand3, "GradeId", key2);
						base.database.SetParameterValue(sqlStringCommand3, "MemberSalePrice", value2.MemberPrices[key2]);
						base.database.ExecuteNonQuery(sqlStringCommand3, dbTran);
					}
				}
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public bool DeleteProductSKUS(int productId, DbTransaction dbTran)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM Hishop_SKUs WHERE ProductId = @ProductId");
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			try
			{
				if (dbTran == null)
				{
					base.database.ExecuteNonQuery(sqlStringCommand);
				}
				else
				{
					base.database.ExecuteNonQuery(sqlStringCommand, dbTran);
				}
				return true;
			}
			catch
			{
				return false;
			}
		}

		public bool AddProductAttributes(int productId, Dictionary<int, IList<int>> attributes, DbTransaction dbTran)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("DELETE FROM Hishop_ProductAttributes WHERE ProductId = {0};", productId);
			int num = 0;
			if (attributes != null)
			{
				foreach (int key in attributes.Keys)
				{
					foreach (int item in attributes[key])
					{
						num++;
						stringBuilder.AppendFormat(" INSERT INTO Hishop_ProductAttributes (ProductId, AttributeId, ValueId) VALUES ({0}, {1}, {2})", productId, key, item);
					}
				}
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			if (dbTran == null)
			{
				return base.database.ExecuteNonQuery(sqlStringCommand) >= 0;
			}
			return base.database.ExecuteNonQuery(sqlStringCommand, dbTran) >= 0;
		}

		public bool UpdateProductCategory(int productId, int newCategoryId, string mainCategoryPath)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE Hishop_Products SET CategoryId = @CategoryId, MainCategoryPath = @MainCategoryPath WHERE ProductId = @ProductId");
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			base.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, newCategoryId);
			base.database.AddInParameter(sqlStringCommand, "MainCategoryPath", DbType.String, mainCategoryPath);
			return base.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}

		public int DeleteProduct(string productIds)
		{
			IList<int> list = new List<int>();
			string[] array = productIds.Split(',');
			foreach (string obj in array)
			{
				list.Add(obj.ToInt(0));
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand($"SELECT oi.ProductId FROM Hishop_Orders o,Hishop_OrderItems oi WHERE o.OrderId=oi.OrderId AND o.OrderId IN(SELECT OrderId FROM Hishop_OrderItems WHERE ProductId IN({productIds})) AND IsServiceOver = 0");
			IList<int> list2 = new List<int>();
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					if (list.Contains(((IDataRecord)dataReader)["ProductId"].ToInt(0)))
					{
						list.Remove(((IDataRecord)dataReader)["ProductId"].ToInt(0));
					}
				}
			}
			if (list.Count > 0)
			{
				DbCommand sqlStringCommand2 = base.database.GetSqlStringCommand(string.Format("DELETE FROM Hishop_Products WHERE ProductId IN ({0}); DELETE FROM Hishop_ProductTag WHERE ProductId IN ({0});DELETE FROM Hishop_Favorite where ProductId IN ({0});", string.Join(",", list)));
				return base.database.ExecuteNonQuery(sqlStringCommand2);
			}
			return 0;
		}

		public int UpdateProductSaleStatus(string productIds, ProductSaleStatus saleStatus)
		{
			string text = $"UPDATE Hishop_Products SET SaleStatus = {(int)saleStatus} WHERE ProductId IN ({productIds})";
			if (saleStatus == ProductSaleStatus.Delete)
			{
				text += $" delete from Hishop_StoreSKUs WHERE ProductId IN ({productIds})";
				text += $" delete from Hishop_StoreProducts WHERE ProductId IN ({productIds})";
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(text);
			return base.database.ExecuteNonQuery(sqlStringCommand);
		}

		public int UpdateProductSaleStatusOnSale(string productIds)
		{
			string commandText = $"select COUNT(1) from [Hishop_Products] where ProductId in({productIds}) and AuditStatus=2";
			int num = (int)base.database.ExecuteScalar(CommandType.Text, commandText);
			if (productIds.Split(',').Length == num)
			{
				DbCommand sqlStringCommand = base.database.GetSqlStringCommand($"UPDATE Hishop_Products SET SaleStatus = 1 WHERE ProductId IN ({productIds})");
				return base.database.ExecuteNonQuery(sqlStringCommand);
			}
			return 0;
		}

		public int UpdateProductShipFree(string productIds, bool isFree)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand($"UPDATE Hishop_Products SET IsfreeShipping = {Convert.ToInt32(isFree)} WHERE ProductId IN ({productIds})");
			return base.database.ExecuteNonQuery(sqlStringCommand);
		}

		public DbQueryResult GetRelatedProducts(Pagination page, int productId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" SaleStatus = {0}", 1);
			stringBuilder.AppendFormat(" AND ProductId IN (SELECT RelatedProductId FROM Hishop_RelatedProducts WHERE ProductId = {0})", productId);
			string selectFields = "ProductId, ProductCode, ProductName, ThumbnailUrl40, MarketPrice, SalePrice, Stock, DisplaySequence";
			return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "vw_Hishop_BrowseProductList p", "ProductId", stringBuilder.ToString(), selectFields);
		}

		public IEnumerable<int> GetRelatedProductsId(int productId)
		{
			string text = 1.GetHashCode().ToString();
			string query = "SELECT RelatedProductId FROM Hishop_RelatedProducts WHERE ProductId = " + productId.ToString();
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			List<int> list = new List<int>();
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					int item = (int)((IDataRecord)dataReader)[0];
					list.Add(item);
				}
			}
			return list;
		}

		public bool AddRelatedProduct(int productId, int relatedProductId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("INSERT INTO Hishop_RelatedProducts(ProductId, RelatedProductId) VALUES (@ProductId, @RelatedProductId)");
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			base.database.AddInParameter(sqlStringCommand, "RelatedProductId", DbType.Int32, relatedProductId);
			try
			{
				return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
			}
			catch
			{
				return false;
			}
		}

		public bool RemoveRelatedProduct(int productId, int relatedProductId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM Hishop_RelatedProducts WHERE ProductId = @ProductId AND RelatedProductId = @RelatedProductId");
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			base.database.AddInParameter(sqlStringCommand, "RelatedProductId", DbType.Int32, relatedProductId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool ClearRelatedProducts(int productId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM Hishop_RelatedProducts WHERE ProductId = @ProductId");
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public decimal? GetProductReferralDeduct(int productId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT ReferralDeduct FROM Hishop_Products WHERE ProductId = @ProductId");
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			object obj = base.database.ExecuteScalar(sqlStringCommand);
			if (obj == DBNull.Value)
			{
				return null;
			}
			return (decimal)obj;
		}

		public decimal? GetProductSubReferralDeduct(int productId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT SubReferralDeduct FROM Hishop_Products WHERE ProductId = @ProductId");
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			object obj = base.database.ExecuteScalar(sqlStringCommand);
			if (obj == DBNull.Value)
			{
				return null;
			}
			return (decimal)obj;
		}

		public decimal? GetProductSubMemberDeduct(int productId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT SubMemberDeduct FROM Hishop_Products WHERE ProductId = @ProductId");
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			object obj = base.database.ExecuteScalar(sqlStringCommand);
			if (obj == DBNull.Value)
			{
				return null;
			}
			return (decimal)obj;
		}

		public decimal? GetProductSecondLevelDeduct(int productId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT SecondLevelDeduct FROM Hishop_Products WHERE ProductId = @ProductId");
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			object obj = base.database.ExecuteScalar(sqlStringCommand);
			if (obj == DBNull.Value || obj == null)
			{
				return null;
			}
			return (decimal)obj;
		}

		public decimal? GetProductThreeLevelDeduct(int productId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT ThreeLevelDeduct FROM Hishop_Products WHERE ProductId = @ProductId");
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			object obj = base.database.ExecuteScalar(sqlStringCommand);
			if (obj == DBNull.Value)
			{
				return null;
			}
			return (decimal)obj;
		}

		public bool IsExistsProductCode(string productCode, int productId)
		{
			string text = "SELECT ProductId FROM Hishop_Products WHERE ProductCode = @ProductCode";
			if (productId > 0)
			{
				text += " And ProductId<>@ProductId";
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(text);
			base.database.AddInParameter(sqlStringCommand, "ProductCode", DbType.String, productCode);
			if (productId > 0)
			{
				base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			}
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0) > 0;
		}

		public bool IsExistsSkuCode(string skuCode, int productId)
		{
			string text = "SELECT COUNT(SkuId) FROM Hishop_SKUs WHERE SKU = @SkuCode";
			if (productId > 0)
			{
				text += " And ProductId <> @ProductId";
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(text);
			base.database.AddInParameter(sqlStringCommand, "SkuCode", DbType.String, skuCode);
			if (productId > 0)
			{
				base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			}
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0) > 0;
		}

		public bool ProductsIsAllOnSales(string productIds)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT COUNT(ProductId) FROM Hishop_Products WHERE ProductId IN(" + productIds + ") AND SaleStatus = " + 1);
			int num = base.database.ExecuteScalar(sqlStringCommand).ToInt(0);
			return num >= productIds.Split(',').Length;
		}

		public DataTable GetAllEffectiveActivityProductId()
		{
			string format = "SELECT ProductId FROM Hishop_FightGroupActivities WHERE EndDate > GETDATE()\r\n                            UNION \r\n                            SELECT ProductId FROM Hishop_GroupBuy WHERE [Status] = {0}\r\n                            UNION \r\n                            SELECT ProductId FROM Hishop_CountDown WHERE EndDate > GETDATE()\r\n                            UNION \r\n                            (SELECT ProductId  FROM [Hishop_CombinationBuy] AS hc \r\n                            LEFT JOIN dbo.Hishop_CombinationBuySKU AS hk  \r\n                            ON hk.CombinationId = hc.CombinationId  \r\n                            WHERE CONVERT(varchar(100), EndDate, 111) >= CONVERT(varchar(100), GETDATE(), 111))";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(string.Format(format, 1.ToString()));
			return base.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}

		public DataTable GetAllEffectiveActivityProductId_PreSale()
		{
			string format = "SELECT ProductId FROM Hishop_FightGroupActivities WHERE EndDate > GETDATE()\r\n                            UNION \r\n                            SELECT ProductId FROM Hishop_GroupBuy WHERE [Status] = {0}\r\n                            UNION \r\n                            SELECT ProductId FROM Hishop_CountDown WHERE EndDate > GETDATE() \r\n                            UNION \r\n                            SELECT ProductId from Hishop_ProductPreSale WHERE  PreSaleEndDate>GETDATE()\r\n                            UNION \r\n                            (SELECT ProductId  FROM [Hishop_CombinationBuy] AS hc \r\n                            LEFT JOIN dbo.Hishop_CombinationBuySKU AS hk  \r\n                            ON hk.CombinationId = hc.CombinationId  \r\n                            WHERE CONVERT(varchar(100), EndDate, 111) >= CONVERT(varchar(100), GETDATE(), 111))";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(string.Format(format, 1.ToString()));
			return base.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}

		public DbQueryResult GetProducts(SupplierProductQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1 ");
			if (query.IsHasStock)
			{
				stringBuilder.AppendFormat(" AND Stock > 0 ");
			}
			if (query.SaleStatus != ProductSaleStatus.All)
			{
				stringBuilder.AppendFormat(" AND SaleStatus = {0}", (int)query.SaleStatus);
			}
			else
			{
				stringBuilder.AppendFormat(" AND SaleStatus <> {0}", 0);
			}
			if (query.BrandId.HasValue)
			{
				stringBuilder.AppendFormat(" AND BrandId = {0}", query.BrandId.Value);
			}
			if (query.TypeId.HasValue)
			{
				stringBuilder.AppendFormat(" AND TypeId = {0}", query.TypeId.Value);
			}
			if (query.TagId.HasValue)
			{
				stringBuilder.AppendFormat(" AND ProductId IN (SELECT ProductId FROM Hishop_ProductTag WHERE TagId={0})", query.TagId);
			}
			if (query.IsFilterFightGroupProduct)
			{
				stringBuilder.AppendFormat(" AND ProductId not IN ( SELECT ProductId FROM Hishop_FightGroups WHERE getdate () BETWEEN StartTime AND EndTime OR StartTime > getdate ()UNION ALL SELECT ProductId FROM Hishop_FightGroupActivities WHERE getdate () BETWEEN StartDate AND EndDate OR StartDate > getdate () )");
			}
			if (query.IsFilterPromotionProduct)
			{
				if (!query.IsMobileExclusive)
				{
					stringBuilder.AppendFormat(" AND ProductId not IN ( SELECT DISTINCT pp.ProductId FROM Hishop_Promotions p INNER JOIN Hishop_PromotionProducts pp ON p.ActivityId = pp.ActivityId AND (   p.StartDate > getdate () OR getdate () BETWEEN p.StartDate AND p.EndDate) and p.PromoteType<>{0} )", 7);
				}
				else
				{
					stringBuilder.AppendFormat(" AND ProductId not IN ( SELECT DISTINCT pp.ProductId FROM Hishop_Promotions p INNER JOIN Hishop_PromotionProducts pp ON p.ActivityId = pp.ActivityId AND (   p.StartDate > getdate () OR getdate () BETWEEN p.StartDate AND p.EndDate) and p.PromoteType={0} )", 7);
				}
			}
			if (query.IsFilterGroupBuyProduct)
			{
				stringBuilder.AppendFormat(" AND ProductId not IN (SELECT ProductId FROM Hishop_GroupBuy WHERE Status = {0} )", 1);
			}
			if (query.IsFilterCountDownProduct)
			{
				stringBuilder.AppendFormat(" AND ProductId not IN ( SELECT ProductId FROM Hishop_CountDown WHERE getdate () BETWEEN StartDate AND EndDate OR StartDate > getdate () )");
			}
			if (query.NotInCombinationMainProduct)
			{
				stringBuilder.AppendFormat(" AND ProductId not IN ( SELECT MainProductId FROM Hishop_CombinationBuy WHERE CONVERT(varchar(100), GETDATE(), 111) <= CONVERT(varchar(100), EndDate, 111))");
			}
			if (query.NotInCombinationOtherProduct)
			{
				stringBuilder.AppendFormat(" AND not exists ( SELECT OtherProductIds FROM Hishop_CombinationBuy WHERE CONVERT(varchar(100), GETDATE(), 111) <= CONVERT(varchar(100), EndDate, 111) AND ','+OtherProductIds+',' like '%,'+ cast(p.ProductId as varchar) + ',%')");
			}
			if (query.NotInPreSaleProduct)
			{
				stringBuilder.AppendFormat(" AND ProductId not IN ( SELECT ProductId FROM Hishop_ProductPreSale WHERE PreSaleEndDate>GETDATE())");
			}
			if (!string.IsNullOrEmpty(query.Keywords))
			{
				query.Keywords = DataHelper.CleanSearchString(query.Keywords);
				string[] array = Regex.Split(query.Keywords.Trim(), "\\s+");
				stringBuilder.AppendFormat(" AND (ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[0]));
				for (int i = 1; i < array.Length && i <= 4; i++)
				{
					stringBuilder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[i]));
				}
				stringBuilder.Append(")");
			}
			if (query.IsMakeTaobao.HasValue && query.IsMakeTaobao.Value >= 0)
			{
				stringBuilder.AppendFormat(" AND IsMaketaobao={0}", query.IsMakeTaobao.Value);
			}
			if (query.IsIncludePromotionProduct.HasValue && !query.IsIncludePromotionProduct.Value)
			{
				if (!query.IsMobileExclusive)
				{
					stringBuilder.AppendFormat(" AND ProductId NOT IN (SELECT ProductId FROM Hishop_PromotionProducts WHERE ActivityId IN(SELECT ActivityId FROM dbo.Hishop_Promotions WHERE PromoteType <> {0} ))", 7);
				}
				else
				{
					stringBuilder.AppendFormat(" AND ProductId NOT IN (SELECT ProductId FROM Hishop_PromotionProducts WHERE ActivityId IN(SELECT ActivityId FROM dbo.Hishop_Promotions WHERE PromoteType = {0} ))", 7);
				}
			}
			if (query.IsIncludeHomeProduct.HasValue && !query.IsIncludeHomeProduct.Value)
			{
				if (!query.Client.HasValue)
				{
					stringBuilder.Append(" AND ProductId NOT IN (SELECT ProductId FROM Vshop_HomeProducts)");
				}
				else
				{
					stringBuilder.Append(" AND ProductId NOT IN (SELECT ProductId FROM Vshop_HomeProducts where Client=" + query.Client + " )");
				}
			}
			if (!string.IsNullOrEmpty(query.ProductCode))
			{
				stringBuilder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
			}
			if (query.CategoryId.HasValue)
			{
				if (query.CategoryId.Value > 0)
				{
					stringBuilder.AppendFormat(" AND (MainCategoryPath LIKE '{0}|%'  OR ExtendCategoryPath LIKE '{0}|%' OR ExtendCategoryPath1 LIKE '{0}|%' OR ExtendCategoryPath2 LIKE '{0}|%' OR ExtendCategoryPath3 LIKE '{0}|%' OR ExtendCategoryPath4 LIKE '{0}|%') ", query.MaiCategoryPath);
				}
				else
				{
					stringBuilder.Append(" AND (CategoryId = 0 OR CategoryId IS NULL)");
				}
			}
			if (query.StartDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND AddedDate >='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
			}
			if (query.EndDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND AddedDate <='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
			}
			if (query.IsWarningStock)
			{
				stringBuilder.Append(" AND  WarningStockNum>0");
			}
			if (query.IsFilterStoreProducts)
			{
				stringBuilder.AppendFormat(" and ProductId not in (select ProductId from Hishop_StoreSKUs where StoreId={0})", query.StoreId);
			}
			if (query.SupplierId == -1)
			{
				stringBuilder.Append(" AND  SupplierId!=0");
			}
			else
			{
				stringBuilder.Append(" AND  SupplierId=" + query.SupplierId);
			}
			if (query.Role == SystemRoles.SupplierAdmin)
			{
				if (query.AuditStatus == ProductAuditStatus.NotSet)
				{
					stringBuilder.AppendFormat(" AND  AuditStatus in({0},{1})", 1, 3);
				}
				else
				{
					stringBuilder.Append(" AND  AuditStatus=" + (int)query.AuditStatus);
				}
			}
			else
			{
				stringBuilder.Append(" AND  AuditStatus=" + (int)query.AuditStatus);
			}
			if (!string.IsNullOrEmpty(query.FilterProductIds))
			{
				stringBuilder.AppendFormat(" and ProductId not in ({0})", query.FilterProductIds);
			}
			string selectFields = "CategoryId,ProductId, HasSKU,ProductCode,IsMakeTaobao,ProductName, ThumbnailUrl40,ThumbnailUrl60,ThumbnailUrl310, MarketPrice, SalePrice,ExtendCategoryPath,ExtendCategoryPath1,ExtendCategoryPath2,ExtendCategoryPath3,ExtendCategoryPath4,CostPrice, AddedDate, Stock,WarningStock, DisplaySequence,SaleStatus,SupplierName,CategoryName,AuditStatus,AuditReson,SaleNum";
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_BrowseProductList p", "ProductId", stringBuilder.ToString(), selectFields);
		}

		public IList<ProductInfo> GetAllProducts(ProductQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" AuditStatus=" + 2);
			if (query.SupplierId.HasValue)
			{
				stringBuilder.AppendFormat(" and  SupplierId={0}", query.SupplierId.Value);
			}
			if (query.IsHasStock)
			{
				stringBuilder.AppendFormat(" AND Stock > 0 ");
			}
			if (query.SaleStatus != ProductSaleStatus.All)
			{
				if (query.SaleStatus == ProductSaleStatus.Delete)
				{
					stringBuilder.AppendFormat(" AND SaleStatus = {0}", (int)query.SaleStatus);
				}
				else
				{
					stringBuilder.AppendFormat(" AND SaleStatus = {0}  ", (int)query.SaleStatus);
				}
			}
			else
			{
				stringBuilder.AppendFormat(" AND SaleStatus <> {0}", 0);
			}
			if (query.BrandId.HasValue)
			{
				stringBuilder.AppendFormat(" AND BrandId = {0}", query.BrandId.Value);
			}
			if (query.TypeId.HasValue)
			{
				stringBuilder.AppendFormat(" AND TypeId = {0}", query.TypeId.Value);
			}
			if (query.TagId.HasValue)
			{
				stringBuilder.AppendFormat(" AND ProductId IN (SELECT ProductId FROM Hishop_ProductTag WHERE TagId={0})", query.TagId);
			}
			if (query.IsFilterFightGroupProduct)
			{
				stringBuilder.AppendFormat(" AND ProductId not IN ( SELECT ProductId FROM Hishop_FightGroups WHERE getdate () BETWEEN StartTime AND EndTime OR StartTime > getdate ()UNION ALL SELECT ProductId FROM Hishop_FightGroupActivities WHERE getdate () BETWEEN StartDate AND EndDate OR StartDate > getdate () )");
			}
			if (query.IsFilterPromotionProduct)
			{
				if (!query.IsMobileExclusive)
				{
					stringBuilder.AppendFormat(" AND ProductId not IN ( SELECT DISTINCT pp.ProductId FROM Hishop_Promotions p INNER JOIN Hishop_PromotionProducts pp ON p.ActivityId = pp.ActivityId AND (   p.StartDate > getdate () OR getdate () BETWEEN p.StartDate AND p.EndDate) and p.PromoteType<>{0} )", 7);
				}
				else
				{
					stringBuilder.AppendFormat(" AND ProductId not IN ( SELECT DISTINCT pp.ProductId FROM Hishop_Promotions p INNER JOIN Hishop_PromotionProducts pp ON p.ActivityId = pp.ActivityId AND (   p.StartDate > getdate () OR getdate () BETWEEN p.StartDate AND p.EndDate) and p.PromoteType={0} )", 7);
				}
			}
			if (query.IsFilterGroupBuyProduct)
			{
				stringBuilder.AppendFormat(" AND ProductId not IN (SELECT ProductId FROM Hishop_GroupBuy WHERE Status = {0} )", 1);
			}
			if (query.IsFilterCountDownProduct)
			{
				stringBuilder.AppendFormat(" AND ProductId not IN ( SELECT ProductId FROM Hishop_CountDown WHERE getdate () BETWEEN StartDate AND EndDate OR StartDate > getdate () )");
			}
			if (query.NotInCombinationMainProduct)
			{
				stringBuilder.AppendFormat(" AND ProductId not IN ( SELECT MainProductId FROM Hishop_CombinationBuy WHERE CONVERT(varchar(100), GETDATE(), 111) <= CONVERT(varchar(100), EndDate, 111))");
			}
			if (query.NotInCombinationOtherProduct)
			{
				stringBuilder.AppendFormat(" AND not exists ( SELECT OtherProductIds FROM Hishop_CombinationBuy WHERE CONVERT(varchar(100), GETDATE(), 111) <= CONVERT(varchar(100), EndDate, 111) AND ','+OtherProductIds+',' like '%,'+ cast(p.ProductId as varchar) + ',%')");
			}
			if (query.NotInPreSaleProduct)
			{
				stringBuilder.AppendFormat(" AND ProductId not IN ( SELECT ProductId FROM Hishop_ProductPreSale WHERE PreSaleEndDate>GETDATE())");
			}
			if (!string.IsNullOrEmpty(query.Keywords))
			{
				query.Keywords = DataHelper.CleanSearchString(query.Keywords);
				string[] array = Regex.Split(query.Keywords.Trim(), "\\s+");
				stringBuilder.AppendFormat(" AND (ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[0]));
				for (int i = 1; i < array.Length && i <= 4; i++)
				{
					stringBuilder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[i]));
				}
				stringBuilder.Append(")");
			}
			if (query.IsMakeTaobao.HasValue && query.IsMakeTaobao.Value >= 0)
			{
				stringBuilder.AppendFormat(" AND IsMaketaobao={0}", query.IsMakeTaobao.Value);
			}
			if (query.IsIncludePromotionProduct.HasValue && !query.IsIncludePromotionProduct.Value)
			{
				if (!query.IsMobileExclusive)
				{
					stringBuilder.AppendFormat(" AND ProductId NOT IN (SELECT ProductId FROM Hishop_PromotionProducts WHERE ActivityId IN(SELECT ActivityId FROM dbo.Hishop_Promotions WHERE PromoteType <> {0} ))", 7);
				}
				else
				{
					stringBuilder.AppendFormat(" AND ProductId NOT IN (SELECT ProductId FROM Hishop_PromotionProducts WHERE ActivityId IN(SELECT ActivityId FROM dbo.Hishop_Promotions WHERE PromoteType = {0} ))", 7);
				}
			}
			if (query.IsIncludeHomeProduct.HasValue && !query.IsIncludeHomeProduct.Value)
			{
				stringBuilder.Append(" AND ProductId NOT IN (SELECT ProductId FROM Vshop_HomeProducts)");
			}
			if (query.IsIncludeAppletProduct.HasValue && !query.IsIncludeAppletProduct.Value)
			{
				stringBuilder.Append(" AND ProductId NOT IN (SELECT ProductId FROM Hishop_AppletChoiceProducts)");
			}
			if (!string.IsNullOrEmpty(query.ProductCode))
			{
				stringBuilder.AppendFormat(" AND (ProductId IN (SELECT DISTINCT(ProductId) FROM Hishop_SKUs WHERE SKU LIKE '%{0}%') OR ProductCode LIKE '%{0}%')", DataHelper.CleanSearchString(query.ProductCode));
			}
			if (query.CategoryId.HasValue)
			{
				if (query.CategoryId.Value > 0)
				{
					stringBuilder.AppendFormat(" AND (MainCategoryPath LIKE '{0}|%'  OR ExtendCategoryPath LIKE '{0}|%' OR ExtendCategoryPath1 LIKE '{0}|%' OR ExtendCategoryPath2 LIKE '{0}|%' OR ExtendCategoryPath3 LIKE '{0}|%' OR ExtendCategoryPath4 LIKE '{0}|%') ", query.MaiCategoryPath);
				}
				else
				{
					stringBuilder.Append(" AND (CategoryId = 0 OR CategoryId IS NULL)");
				}
			}
			if (query.StartDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND AddedDate >='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
			}
			if (query.EndDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND AddedDate <='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
			}
			if (query.IsWarningStock)
			{
				stringBuilder.Append(" AND  WarningStockNum>0");
			}
			if (query.IsFilterStoreProducts)
			{
				stringBuilder.AppendFormat(" and ProductId not in (select ProductId from Hishop_StoreSKUs where StoreId={0})", query.StoreId);
			}
			if (!string.IsNullOrEmpty(query.FilterProductIds))
			{
				stringBuilder.AppendFormat(" and ProductId not in ({0})", query.FilterProductIds);
			}
			IList<ProductInfo> list = new List<ProductInfo>();
			string arg = "ProductId,ProductName,ThumbnailUrl310 ";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand($"SELECT {arg} FROM Hishop_Products WHERE {stringBuilder.ToString()} ORDER BY ProductId DESC");
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					ProductInfo productInfo = new ProductInfo();
					productInfo.ProductId = ((IDataRecord)dataReader)["ProductId"].ToInt(0);
					productInfo.ProductName = ((IDataRecord)dataReader)["ProductName"].ToNullString();
					productInfo.ThumbnailUrl310 = ((IDataRecord)dataReader)["ThumbnailUrl310"].ToNullString();
					list.Add(productInfo);
				}
			}
			return list;
		}

		public IList<ShippingTemplateInfo> GetProductShippingTemplates(string productIds)
		{
			IList<ShippingTemplateInfo> list = new List<ShippingTemplateInfo>();
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT DISTINCT(ShippingTemplateId) FROM Hishop_Products WHERE ProductId IN(" + productIds + ")");
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				IList<int> list2 = new List<int>();
				while (dataReader.Read())
				{
					list2.Add(dataReader.GetInt32(0).ToInt(0));
				}
				if (list2.Contains(0))
				{
					ShippingTemplateInfo shippingTemplateInfo = new ShippingTemplateInfo();
					shippingTemplateInfo.TemplateId = 0;
					shippingTemplateInfo.TemplateName = "未设置";
					list.Add(shippingTemplateInfo);
					list2.Remove(0);
				}
				if (list2.Count > 0)
				{
					DbCommand sqlStringCommand2 = base.database.GetSqlStringCommand(string.Format("SELECT * FROM Hishop_ShippingTemplates WHERE TemplateId IN({0})", string.Join(",", list2.ToArray())));
					using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand2))
					{
						if (list.Count > 0)
						{
							list.Concat(DataHelper.ReaderToList<ShippingTemplateInfo>(objReader));
						}
						else
						{
							list = DataHelper.ReaderToList<ShippingTemplateInfo>(objReader);
						}
					}
				}
			}
			return list;
		}

		public bool SetProductShippingTemplates(string productIds, int shippingTemplateId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE Hishop_Products SET ShippingTemplateId = @ShippingTemplateId WHERE ProductId IN(" + productIds + ")");
			base.database.AddInParameter(sqlStringCommand, "ShippingTemplateId", DbType.Int32, shippingTemplateId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool DeleteInputItem(int productId, DbTransaction dbTran)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM Hishop_ProductInputItems WHERE ProductId = @ProductId");
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			try
			{
				if (dbTran == null)
				{
					base.database.ExecuteNonQuery(sqlStringCommand);
				}
				else
				{
					base.database.ExecuteNonQuery(sqlStringCommand, dbTran);
				}
				return true;
			}
			catch
			{
				return false;
			}
		}

		public List<ProductInputItemInfo> GetProductInputItemList(int productId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("select * from Hishop_ProductInputItems Where productId = @productId; ");
			base.database.AddInParameter(sqlStringCommand, "productId", DbType.Int32, productId);
			IList<ProductInputItemInfo> source = default(IList<ProductInputItemInfo>);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				source = DataHelper.ReaderToList<ProductInputItemInfo>(objReader);
			}
			return (from r in source
			orderby r.Id
			select r).ToList();
		}
	}
}
