using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Members;
using Hidistro.SqlDal.Members;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Text.RegularExpressions;

namespace Hidistro.SqlDal.Commodities
{
	public class ProductBrowseDao : BaseDao
	{
		public DataTable GetSaleProductRanking(CategoryInfo category, int maxNum)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT TOP {0} ProductId, ProductName, ProductCode, ShowSaleCounts AS SaleCounts, ThumbnailUrl40, ThumbnailUrl60, ThumbnailUrl100,", maxNum);
			stringBuilder.AppendFormat("  ThumbnailUrl160, ThumbnailUrl180, ThumbnailUrl220, SalePrice, MarketPrice FROM vw_Hishop_BrowseProductList WHERE SaleStatus = {0}", 1);
			if (category != null)
			{
				stringBuilder.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%' OR ExtendCategoryPath LIKE '{0}|%' OR ExtendCategoryPath1 LIKE '{0}|%' OR ExtendCategoryPath2 LIKE '{0}|%' OR ExtendCategoryPath3 LIKE '{0}|%' OR ExtendCategoryPath4 LIKE '{0}|%') ", category.Path);
			}
			stringBuilder.Append("ORDER BY ShowSaleCounts DESC");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			using (IDataReader reader = base.database.ExecuteReader(sqlStringCommand))
			{
				return DataHelper.ConverDataReaderToDataTable(reader);
			}
		}

		public DataTable GetSubjectList(SubjectListQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT TOP {0} ProductId,ProductName,ProductCode,ShowSaleCounts AS SaleCounts,ShortDescription,", query.MaxNum);
			stringBuilder.Append(" ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160,ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice,SalePrice");
			stringBuilder.Append(" FROM vw_Hishop_BrowseProductList p WHERE ");
			stringBuilder.Append(this.BuildProductSubjectQuerySearch(query));
			if (!string.IsNullOrEmpty(query.SortBy))
			{
				stringBuilder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), DataHelper.CleanSearchString(query.SortOrder.ToString()));
			}
			DataTable result = null;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			using (IDataReader reader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(reader);
			}
			return result;
		}

		public DataTable GetHotProductList()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("select top 5 * from (select top 10 ShowSaleCounts,ProductId,ProductName,ThumbnailUrl40,ThumbnailUrl220,MarketPrice,ImageUrl2,ImageUrl3,ImageUrl4,ImageUrl5,\r\n            (SELECT MIN(SalePrice) AS Expr1 FROM dbo.Hishop_SKUs WHERE ProductId = p.ProductId) AS SalePrice from Hishop_Products as p\r\n            Where SaleStatus=1  order by ShowSaleCounts desc) as A order by newid()");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			return base.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}

		public DataTable GetSkusByProductId(int productId, int gradeId)
		{
			int num = 100;
			MemberGradeInfo memberGradeInfo = this.Get<MemberGradeInfo>(gradeId);
			if (memberGradeInfo != null)
			{
				num = memberGradeInfo.Discount;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT SkuId, ProductId, SKU,Weight, Stock, CostPrice,");
			stringBuilder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0}) = 1", gradeId);
			stringBuilder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Hishop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0})", gradeId);
			stringBuilder.AppendFormat(" ELSE SalePrice * {0} /100 END) AS SalePrice", num);
			stringBuilder.Append(" FROM Hishop_SKUs s WHERE ProductId = @ProductId");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			using (IDataReader reader = base.database.ExecuteReader(sqlStringCommand))
			{
				return DataHelper.ConverDataReaderToDataTable(reader);
			}
		}

		public ProductInfo GetProductDescription(int productId)
		{
			ProductInfo productInfo = new ProductInfo();
			StringBuilder stringBuilder = new StringBuilder(" select  ProductId,ProductName,Description,MobbileDescription  FROM Hishop_Products  where productId=" + productId);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					productInfo.Description = ((IDataRecord)dataReader)["Description"].ToNullString();
					productInfo.MobbileDescription = ((IDataRecord)dataReader)["MobbileDescription"].ToNullString();
				}
			}
			return productInfo;
		}

		public ProductModel GetProductModel(StoreProductQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			ProductModel productModel = new ProductModel();
			stringBuilder.Append(" select  p.ProductId,p.ProductName,p.ShortDescription,isnull(p.MarketPrice,0) as MarketPrice,p.Title,p.Meta_Description,p.Meta_Keywords,p.VistiCounts,p.ProductType,p.ShippingTemplateId");
			stringBuilder.Append(",p.IsValid,p.ValidStartDate,p.ValidEndDate,p.IsRefund,p.IsOverRefund,p.Unit");
			stringBuilder.Append(",( case when exists(select * FROM dbo.[Hishop_StoreSKUs] s  where s.ProductID=p.ProductId and s.StoreId=sp.StoreId) then (select top 1 (case when [StoreSalePrice]=0 then (select saleprice FROM dbo.Hishop_SKUs WHERE SkuId= s.SkuId) else [StoreSalePrice] end ) FROM dbo.[Hishop_StoreSKUs] s where s.ProductID=p.ProductId and s.StoreId=sp.StoreId  order by 1 asc) else (select min(saleprice) FROM dbo.Hishop_SKUs WHERE ProductID=p.ProductId) end)as MinSalePrice");
			stringBuilder.Append(",( case when exists(select * FROM dbo.[Hishop_StoreSKUs] s  where s.ProductID=p.ProductId and s.StoreId=sp.StoreId) then (select top 1 (case when [StoreSalePrice]=0 then (select saleprice FROM dbo.Hishop_SKUs WHERE SkuId= s.SkuId) else [StoreSalePrice] end ) FROM dbo.[Hishop_StoreSKUs] s where s.ProductID=p.ProductId and s.StoreId=sp.StoreId  order by 1 desc) else (select max(saleprice) FROM dbo.Hishop_SKUs WHERE ProductID=p.ProductId) end)as MaxSalePrice");
			stringBuilder.Append(",p.ImageUrl1,p.ImageUrl2,p.ImageUrl3,p.ImageUrl4,p.ImageUrl5,isnull(p.ThumbnailUrl160,'') as ThumbnailUrl160,p.SubMemberDeduct,p.ShowSaleCounts,(case when  p.MobbileDescription is null then p.[Description]  else p.MobbileDescription end) as [Description]");
			stringBuilder.Append(",( SELECT Count(1) FROM Hishop_ProductConsultations where ProductId=p.ProductId AND ReplyUserId IS NOT NULL) as ConsultationCount");
			stringBuilder.Append(",(SELECT Count(1) FROM Hishop_ProductReviews where ProductId=p.ProductId) as ReviewCount,isnull(sp.StoreId,0) StoreId,isnull(sp.SaleStatus,2) SaleStatus ");
			stringBuilder.Append(",StoreName,Latitude,Longitude,FullRegionPath,[address],IsAboveSelf,IsSupportExpress,IsStoreDelive,isnull(MinOrderPrice,-1) MinOrderPrice,StoreFreight,CloseStatus,CloseEndTime,CloseBeginTime,isnull([OpenStartDate],'2017-1-1 00:00:01')as [OpenStartTime],isnull([OpenEndDate],'2017-1-1 23:59:59')as [OpenEndTime] ");
			if (query.Position.Latitude > 0.0)
			{
				stringBuilder.AppendFormat(",dbo.GetDistance({0},{1},Latitude,Longitude)as Distance", query.Position.Latitude, query.Position.Longitude);
				stringBuilder.AppendFormat(",(case when IsSupportExpress=1 or (st.TopRegionId={0} and(IsAboveSelf=1 or  (IsStoreDelive=1 and (dbo.GetDistance({1},{2},Latitude,Longitude)<(ServeRadius*1000) or dbo.IsInDeliveryScope(st.StoreId,'{3}')=1)))) then 1 else 0 end) as IsInServiceArea", query.Position.CityId, query.Position.Latitude, query.Position.Longitude, query.Position.AreaId);
			}
			else
			{
				stringBuilder.Append(",0 as Distance,1 as IsInServiceArea");
			}
			stringBuilder.Append(",(SELECT ISNULL(SUM([Stock]),0)  FROM [dbo].[Hishop_StoreSKUs] WHERE ProductId=p.ProductId and StoreId=st.StoreId) as Stock");
			stringBuilder.Append("  FROM Hishop_Products p ");
			stringBuilder.Append(" LEFT JOIN Hishop_StoreProducts sp on p.ProductId=sp.ProductId");
			stringBuilder.AppendFormat(" INNER JOIN Hishop_Stores st on sp.StoreId=st.StoreId  and sp.StoreId={0}", query.StoreId);
			stringBuilder.AppendFormat(" where p.ProductId={0};", query.ProductId);
			stringBuilder.Append("SELECT hs.SkuId, hs.ProductId, SKU,[Weight], ss.Stock,ss.WarningStock, CostPrice,FreezeStock = ISNULL(ss.FreezeStock,0),StoreStock=ss.Stock,");
			stringBuilder.Append("(case when ss.StoreSalePrice>0 then ss.StoreSalePrice else hs.SalePrice end)  as SalePrice");
			stringBuilder.Append(" FROM Hishop_SKUs hs    inner join[Hishop_StoreSKUs] ss on ss.SkuId=hs.SkuId");
			stringBuilder.AppendFormat(" WHERE hs.ProductId ={0} and ss.StoreId={1};", query.ProductId, query.StoreId);
			stringBuilder.AppendFormat(" SELECT SkuId, a.AttributeId, AttributeName, UseAttributeImage, av.ValueId, ValueStr, pi.ImageUrl,pi.ThumbnailUrl40,pi.ThumbnailUrl410  FROM Hishop_SKUItems s JOIN Hishop_Attributes a ON s.AttributeId = a.AttributeId JOIN Hishop_AttributeValues av ON s.ValueId = av.ValueId LEFT JOIN (SELECT * FROM Hishop_ProductSpecificationImages WHERE ProductId = {0}) pi ON s.ValueId = pi.ValueId WHERE SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId = {0}) ORDER BY a.DisplaySequence DESC,av.DisplaySequence DESC;", query.ProductId);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					productModel.VistiCounts = (int)((IDataRecord)dataReader)["VistiCounts"];
					productModel.ConsultationCount = (int)((IDataRecord)dataReader)["ConsultationCount"];
					productModel.Description = ((IDataRecord)dataReader)["Description"].ToString();
					productModel.ImgUrlList = new List<string>();
					productModel.ImgUrlList.Add(((IDataRecord)dataReader)["ImageUrl1"].ToNullString());
					productModel.ImgUrlList.Add(((IDataRecord)dataReader)["ImageUrl2"].ToNullString());
					productModel.ImgUrlList.Add(((IDataRecord)dataReader)["ImageUrl3"].ToNullString());
					productModel.ImgUrlList.Add(((IDataRecord)dataReader)["ImageUrl4"].ToNullString());
					productModel.ImgUrlList.Add(((IDataRecord)dataReader)["ImageUrl5"].ToNullString());
					productModel.SubmitOrderImg = ((IDataRecord)dataReader)["ThumbnailUrl160"].ToNullString();
					productModel.MarketPrice = ((IDataRecord)dataReader)["MarketPrice"].ToDecimal(0);
					productModel.Title = ((IDataRecord)dataReader)["Title"].ToNullString();
					productModel.Meta_Description = ((IDataRecord)dataReader)["Meta_Description"].ToNullString();
					productModel.Meta_Keywords = ((IDataRecord)dataReader)["Meta_Keywords"].ToNullString();
					productModel.MinSalePrice = ((IDataRecord)dataReader)["MinSalePrice"].ToDecimal(0);
					productModel.MaxSalePrice = ((IDataRecord)dataReader)["MaxSalePrice"].ToDecimal(0);
					productModel.ProductId = (int)((IDataRecord)dataReader)["ProductId"];
					productModel.ProductName = ((IDataRecord)dataReader)["ProductName"].ToString();
					productModel.ReviewCount = (int)((IDataRecord)dataReader)["ReviewCount"];
					productModel.SaleStatus = (ProductSaleStatus)((IDataRecord)dataReader)["SaleStatus"];
					productModel.ShortDescription = ((IDataRecord)dataReader)["ShortDescription"].ToString();
					productModel.ShowSaleCounts = (int)((IDataRecord)dataReader)["ShowSaleCounts"];
					productModel.StoreId = (int)((IDataRecord)dataReader)["StoreId"];
					productModel.Stock = (int)((IDataRecord)dataReader)["Stock"];
					productModel.ShippingTemplateId = ((IDataRecord)dataReader)["ShippingTemplateId"].ToInt(0);
					if (((IDataRecord)dataReader)["IsValid"] != DBNull.Value)
					{
						productModel.IsValid = (bool)((IDataRecord)dataReader)["IsValid"];
						if (!productModel.IsValid)
						{
							if (((IDataRecord)dataReader)["ValidStartDate"] != DBNull.Value)
							{
								productModel.ValidStartDate = (DateTime)((IDataRecord)dataReader)["ValidStartDate"];
							}
							if (((IDataRecord)dataReader)["ValidEndDate"] != DBNull.Value)
							{
								productModel.ValidEndDate = (DateTime)((IDataRecord)dataReader)["ValidEndDate"];
							}
						}
					}
					if (((IDataRecord)dataReader)["IsRefund"] != DBNull.Value)
					{
						productModel.IsRefund = (bool)((IDataRecord)dataReader)["IsRefund"];
					}
					if (((IDataRecord)dataReader)["IsOverRefund"] != DBNull.Value)
					{
						productModel.IsOverRefund = (bool)((IDataRecord)dataReader)["IsOverRefund"];
					}
					if (((IDataRecord)dataReader)["SubMemberDeduct"] != null && !string.IsNullOrEmpty(((IDataRecord)dataReader)["SubMemberDeduct"].ToString()))
					{
						productModel.SubMemberDeduct = ((IDataRecord)dataReader)["SubMemberDeduct"].ToDecimal(0);
					}
					productModel.ExStatus = DetailException.Nomal;
					productModel.ProductType = ((IDataRecord)dataReader)["ProductType"].ToInt(0);
					productModel.StoreInfo = new StoreBaseEntity
					{
						StoreId = ((IDataRecord)dataReader)["StoreId"].ToInt(0),
						StoreName = ((IDataRecord)dataReader)["StoreName"].ToString(),
						FullRegionPath = ((IDataRecord)dataReader)["FullRegionPath"].ToString(),
						Address = ((IDataRecord)dataReader)["address"].ToString(),
						Delivery = new StoreDeliveryInfo
						{
							IsStoreDelive = ((IDataRecord)dataReader)["IsStoreDelive"].ToBool(),
							IsPickeupInStore = ((IDataRecord)dataReader)["IsAboveSelf"].ToBool(),
							IsSupportExpress = ((IDataRecord)dataReader)["IsSupportExpress"].ToBool(),
							MinOrderPrice = ((((IDataRecord)dataReader)["IsStoreDelive"].ToString() == "True") ? ((IDataRecord)dataReader)["MinOrderPrice"].ToDecimal(0) : decimal.MinusOne),
							StoreFreight = ((IDataRecord)dataReader)["StoreFreight"].ToDecimal(0)
						},
						Position = new PositionInfo(((IDataRecord)dataReader)["Latitude"].ToDouble(0), ((IDataRecord)dataReader)["Longitude"].ToDouble(0)),
						IsOpen = ((IDataRecord)dataReader)["CloseStatus"].ToBool(),
						CloseEndTime = ((IDataRecord)dataReader)["CloseEndTime"].ToDateTime(),
						CloseStartTime = ((IDataRecord)dataReader)["CloseBeginTime"].ToDateTime(),
						Distance = ((IDataRecord)dataReader)["Distance"].ToInt(0).ToString(),
						IsInServiceArea = ((IDataRecord)dataReader)["IsInServiceArea"].ToBool(),
						OpenEndTime = ((IDataRecord)dataReader)["OpenEndTime"].ToDateTime().Value,
						OpenStartTime = ((IDataRecord)dataReader)["OpenStartTime"].ToDateTime().Value
					};
				}
				if (dataReader.NextResult())
				{
					productModel.Skus = new List<SKUItem>();
					while (dataReader.Read())
					{
						SKUItem sKUItem = DataMapper.PopulateSKU(dataReader);
						if (sKUItem != null)
						{
							productModel.Skus.Add(sKUItem);
						}
					}
				}
				if (dataReader.NextResult())
				{
					productModel.SkuTable = DataHelper.ConverDataReaderToDataTable(dataReader);
				}
			}
			return productModel;
		}

		protected string BuildProductSubjectQuerySearch(SubjectListQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" SaleStatus = {0}", 1);
			stringBuilder.AppendFormat(" AND ProductType ='{0}'", 0.GetHashCode());
			if (query.TagId != 0)
			{
				stringBuilder.AppendFormat(" AND ProductId IN(SELECT ProductId FROM Hishop_ProductTag WHERE TagId = {0})", query.TagId);
			}
			if (query.Category != null)
			{
				stringBuilder.AppendFormat(" AND (MainCategoryPath LIKE '{0}|%' OR ExtendCategoryPath LIKE '{0}|%' OR ExtendCategoryPath1 LIKE '{0}|%' OR ExtendCategoryPath2 LIKE '{0}|%' OR ExtendCategoryPath3 LIKE '{0}|%' OR ExtendCategoryPath4 LIKE '{0}|%') ", query.Category.Path);
			}
			if (query.BrandCategoryId.HasValue)
			{
				stringBuilder.AppendFormat(" AND BrandId = {0}", query.BrandCategoryId.Value);
			}
			if (query.ProductTypeId.HasValue)
			{
				stringBuilder.AppendFormat(" AND TypeId = {0}", query.ProductTypeId.Value);
			}
			if (query.AttributeValues.Count > 0)
			{
				foreach (AttributeValueInfo attributeValue in query.AttributeValues)
				{
					stringBuilder.AppendFormat(" AND (ProductId IN ( SELECT ProductId FROM Hishop_ProductAttributes WHERE AttributeId={0} And ValueId={1})) ", attributeValue.AttributeId, attributeValue.ValueId);
				}
			}
			if (query.MinPrice.HasValue)
			{
				stringBuilder.AppendFormat(" AND SalePrice >= {0}", query.MinPrice.Value);
			}
			if (query.MaxPrice.HasValue)
			{
				stringBuilder.AppendFormat(" AND SalePrice <= {0}", query.MaxPrice.Value);
			}
			if (!string.IsNullOrEmpty(query.Keywords) && query.Keywords.Trim().Length > 0)
			{
				query.Keywords = DataHelper.CleanSearchString(query.Keywords);
				string[] array = Regex.Split(query.Keywords.Trim(), "\\s+");
				stringBuilder.AppendFormat(" AND (ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[0]));
				for (int i = 1; i < array.Length && i <= 5; i++)
				{
					stringBuilder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[i]));
				}
				stringBuilder.Append(")");
			}
			return stringBuilder.ToString();
		}

		public void AddVistiCounts(int productId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE Hishop_Products SET VistiCounts = VistiCounts + 1 WHERE ProductId =" + productId);
			base.database.ExecuteNonQuery(sqlStringCommand);
		}

		public ProductBrowseInfo GetProductBrowseInfo(int productId, int gradeId, int? maxConsultationNum, bool MutiStores = false)
		{
			int num = 100;
			ProductBrowseInfo productBrowseInfo = new ProductBrowseInfo();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" SELECT * ,CASE WHEN BrandId IS NULL THEN NULL ELSE (SELECT BrandName FROM Hishop_BrandCategories WHERE BrandId= p.BrandId) END AS BrandName");
			stringBuilder.Append(" FROM Hishop_Products p where ProductId=@ProductId;");
			if (gradeId > 0)
			{
				MemberGradeInfo memberGradeInfo = new MemberGradeDao().Get<MemberGradeInfo>(gradeId);
				num = (memberGradeInfo?.Discount ?? 1);
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
			stringBuilder.Append(" SELECT Count(*) FROM Hishop_ProductReviews where ProductId=@ProductId; ");
			stringBuilder.Append($"select count(*) from dbo.Hishop_OrderItems as items left join Hishop_Orders orders on items.OrderId=orders.OrderId where orders.OrderStatus!={1} and orders.OrderStatus!={4} and items.ProductId=@ProductId");
			if (maxConsultationNum.HasValue)
			{
				stringBuilder.AppendFormat(" SELECT TOP {0} * FROM Hishop_ProductConsultations where ProductId=@ProductId AND ReplyUserId IS NOT NULL ORDER BY ConsultationId DESC ;", maxConsultationNum);
				stringBuilder.Append(" SELECT Count(*) FROM Hishop_ProductConsultations where ProductId=@ProductId AND ReplyUserId IS NOT NULL ; ");
			}
			else
			{
				stringBuilder.Append(" SELECT * FROM Hishop_ProductConsultations where ProductId=@ProductId AND ReplyUserId IS NOT NULL ORDER BY ConsultationId DESC ;");
				stringBuilder.Append(" SELECT Count(*) FROM Hishop_ProductConsultations where ProductId=@ProductId AND ReplyUserId IS NOT NULL ; ");
			}
			stringBuilder.Append(" SELECT a.AttributeId, AttributeName, ValueStr FROM Hishop_ProductAttributes pa JOIN Hishop_Attributes a ON pa.AttributeId = a.AttributeId");
			stringBuilder.Append(" JOIN Hishop_AttributeValues v ON a.AttributeId = v.AttributeId AND pa.ValueId = v.ValueId  WHERE ProductId = @ProductId ORDER BY a.DisplaySequence DESC, v.DisplaySequence DESC");
			stringBuilder.Append(" SELECT SkuId, a.AttributeId, AttributeName, UseAttributeImage, av.ValueId, ValueStr, pi.ImageUrl,pi.ThumbnailUrl40,pi.ThumbnailUrl410 FROM Hishop_SKUItems s join Hishop_Attributes a on s.AttributeId = a.AttributeId join Hishop_AttributeValues av on s.ValueId = av.ValueId LEFT JOIN (select * from Hishop_ProductSpecificationImages where ProductId=@ProductId) pi ON s.ValueId=pi.ValueId WHERE SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId = @ProductId)  ORDER BY a.DisplaySequence DESC,av.DisplaySequence DESC;");
			stringBuilder.Append(" SELECT TOP 20 ProductId,ProductName,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice,SalePrice FROM vw_Hishop_BrowseProductList");
			stringBuilder.AppendFormat(" WHERE SaleStatus = {0} AND ProductId IN (SELECT RelatedProductId FROM Hishop_RelatedProducts WHERE ProductId = {1}) ORDER BY DisplaySequence DESC;", 1, productId);
			stringBuilder.Append("SELECT TOP 20 ProductId,ProductName,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice,SalePrice FROM vw_Hishop_BrowseProductList");
			stringBuilder.AppendFormat(" WHERE SaleStatus = {0} AND ProductId<>{1}  AND CategoryId = (SELECT CategoryId FROM Hishop_Products WHERE ProductId={1})", 1, productId);
			stringBuilder.AppendFormat(" AND ProductId NOT IN (SELECT RelatedProductId FROM Hishop_RelatedProducts WHERE ProductId = {0})", productId);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					productBrowseInfo.Product = DataMapper.PopulateProduct(dataReader);
					if (((IDataRecord)dataReader)["BrandName"] != DBNull.Value)
					{
						productBrowseInfo.BrandName = (string)((IDataRecord)dataReader)["BrandName"];
					}
				}
				if (dataReader.NextResult())
				{
					while (dataReader.Read())
					{
						SKUItem sKUItem = DataMapper.PopulateSKU(dataReader);
						if (sKUItem != null)
						{
							productBrowseInfo.Product.Skus.Add((string)((IDataRecord)dataReader)["SkuId"], sKUItem);
						}
					}
				}
				if (dataReader.NextResult() && dataReader.Read())
				{
					productBrowseInfo.ReviewCount = (int)((IDataRecord)dataReader)[0];
				}
				if (dataReader.NextResult() && dataReader.Read())
				{
					productBrowseInfo.SaleCount = (int)((IDataRecord)dataReader)[0];
				}
				if (dataReader.NextResult())
				{
					productBrowseInfo.DBConsultations = DataHelper.ConverDataReaderToDataTable(dataReader);
				}
				if (dataReader.NextResult() && dataReader.Read())
				{
					productBrowseInfo.ConsultationCount = (int)((IDataRecord)dataReader)[0];
				}
				if (dataReader.NextResult())
				{
					DataTable dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
					if (dataTable != null && dataTable.Rows.Count > 0)
					{
						DataTable dataTable2 = dataTable.Clone();
						foreach (DataRow row in dataTable.Rows)
						{
							bool flag = false;
							if (dataTable2.Rows.Count > 0)
							{
								foreach (DataRow row2 in dataTable2.Rows)
								{
									if ((int)row2["AttributeId"] == (int)row["AttributeId"])
									{
										flag = true;
										DataRow dataRow3 = row2;
										dataRow3["ValueStr"] = dataRow3["ValueStr"] + ", " + row["ValueStr"];
									}
								}
							}
							if (!flag)
							{
								DataRow dataRow4 = dataTable2.NewRow();
								dataRow4["AttributeId"] = row["AttributeId"];
								dataRow4["AttributeName"] = row["AttributeName"];
								dataRow4["ValueStr"] = row["ValueStr"];
								dataTable2.Rows.Add(dataRow4);
							}
						}
						productBrowseInfo.DbAttribute = dataTable2;
					}
				}
				if (dataReader.NextResult())
				{
					productBrowseInfo.DbSKUs = DataHelper.ConverDataReaderToDataTable(dataReader);
					productBrowseInfo.ListSKUs = DataHelper.ReaderToList<SKUItem>(dataReader);
					for (int i = 0; i < productBrowseInfo.ListSKUs.Count; i++)
					{
						SKUItem sKUItem2 = productBrowseInfo.ListSKUs[i];
						productBrowseInfo.ListSKUs[i].SalePrice = sKUItem2.SalePrice.F2ToString("f2").ToDecimal(0);
						productBrowseInfo.ListSKUs[i].OldSalePrice = sKUItem2.OldSalePrice.F2ToString("f2").ToDecimal(0);
						productBrowseInfo.ListSKUs[i].CostPrice = sKUItem2.CostPrice.F2ToString("f2").ToDecimal(0);
						productBrowseInfo.ListSKUs[i].StoreSalePrice = sKUItem2.StoreSalePrice.F2ToString("f2").ToDecimal(0);
						if (sKUItem2.MemberPrices != null && sKUItem2.MemberPrices.Count > 0)
						{
							foreach (int key in sKUItem2.MemberPrices.Keys)
							{
								productBrowseInfo.ListSKUs[i].MemberPrices[key] = sKUItem2.MemberPrices[key].F2ToString("f2").ToDecimal(0);
							}
						}
					}
				}
				if (dataReader.NextResult())
				{
					productBrowseInfo.DbCorrelatives = DataHelper.ConverDataReaderToDataTable(dataReader);
					productBrowseInfo.ListCorrelatives = DataHelper.ReaderToList<ProductInfo>(dataReader);
				}
				if (dataReader.NextResult())
				{
					productBrowseInfo.DbCorrelatives.Merge(DataHelper.ConverDataReaderToDataTable(dataReader), true);
					IList<ProductInfo> list = DataHelper.ReaderToList<ProductInfo>(dataReader);
					if (list != null)
					{
						foreach (ProductInfo item in list)
						{
							productBrowseInfo.ListCorrelatives.Add(item);
						}
					}
				}
			}
			return productBrowseInfo;
		}

		public ProductBrowseInfo GetWAPProductBrowseInfo(int productId, int gradeId, int? maxConsultationNum, bool MutiStores = false)
		{
			int num = 100;
			ProductBrowseInfo productBrowseInfo = new ProductBrowseInfo();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" SELECT *  FROM Hishop_Products p where ProductId=@ProductId;");
			if (gradeId > 0)
			{
				MemberGradeInfo memberGradeInfo = new MemberGradeDao().Get<MemberGradeInfo>(gradeId);
				num = (memberGradeInfo?.Discount ?? 1);
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
			stringBuilder.Append(" SELECT Count(*) FROM Hishop_ProductReviews where ProductId=@ProductId; ");
			if (maxConsultationNum.HasValue)
			{
				stringBuilder.AppendFormat(" SELECT TOP {0} * FROM Hishop_ProductConsultations where ProductId=@ProductId AND ReplyUserId IS NOT NULL ORDER BY ConsultationId DESC ;", maxConsultationNum);
				stringBuilder.Append(" SELECT Count(*) FROM Hishop_ProductConsultations where ProductId=@ProductId AND ReplyUserId IS NOT NULL ; ");
			}
			else
			{
				stringBuilder.Append(" SELECT * FROM Hishop_ProductConsultations where ProductId=@ProductId AND ReplyUserId IS NOT NULL ORDER BY ConsultationId DESC ;");
				stringBuilder.Append(" SELECT Count(*) FROM Hishop_ProductConsultations where ProductId=@ProductId AND ReplyUserId IS NOT NULL ; ");
			}
			stringBuilder.Append(" SELECT SkuId, a.AttributeId, AttributeName, UseAttributeImage, av.ValueId, ValueStr, pi.ImageUrl,pi.ThumbnailUrl40,pi.ThumbnailUrl410 FROM Hishop_SKUItems s join Hishop_Attributes a on s.AttributeId = a.AttributeId join Hishop_AttributeValues av on s.ValueId = av.ValueId LEFT JOIN (select * from Hishop_ProductSpecificationImages where ProductId=@ProductId) pi ON s.ValueId=pi.ValueId WHERE SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId = @ProductId)  ORDER BY a.DisplaySequence DESC,av.DisplaySequence DESC;");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					productBrowseInfo.Product = DataMapper.PopulateProduct(dataReader);
				}
				if (dataReader.NextResult())
				{
					while (dataReader.Read())
					{
						SKUItem sKUItem = DataMapper.PopulateSKU(dataReader);
						if (sKUItem != null)
						{
							productBrowseInfo.Product.Skus.Add((string)((IDataRecord)dataReader)["SkuId"], sKUItem);
						}
					}
				}
				if (dataReader.NextResult() && dataReader.Read())
				{
					productBrowseInfo.ReviewCount = (int)((IDataRecord)dataReader)[0];
				}
				if (dataReader.NextResult())
				{
					productBrowseInfo.DBConsultations = DataHelper.ConverDataReaderToDataTable(dataReader);
				}
				if (dataReader.NextResult() && dataReader.Read())
				{
					productBrowseInfo.ConsultationCount = (int)((IDataRecord)dataReader)[0];
				}
				if (dataReader.NextResult())
				{
					productBrowseInfo.DbSKUs = DataHelper.ConverDataReaderToDataTable(dataReader);
				}
			}
			return productBrowseInfo;
		}

		public ProductBrowseInfo GetAppletProductBrowseInfo(int productId, int gradeId)
		{
			int num = 100;
			ProductBrowseInfo productBrowseInfo = new ProductBrowseInfo();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" SELECT *  FROM Hishop_Products p where ProductId=@ProductId;");
			if (gradeId > 0)
			{
				MemberGradeInfo memberGradeInfo = new MemberGradeDao().Get<MemberGradeInfo>(gradeId);
				num = (memberGradeInfo?.Discount ?? 1);
				stringBuilder.Append("SELECT SkuId, ProductId, SKU,Weight, Stock,WarningStock, CostPrice,FreezeStock = ISNULL(FreezeStock,0),StoreStock=Stock,");
				stringBuilder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0}) = 1", gradeId);
				stringBuilder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Hishop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice", gradeId, num);
				stringBuilder.Append(" FROM Hishop_SKUs s WHERE ProductId = @ProductId;");
			}
			else
			{
				stringBuilder.Append("SELECT SkuId, ProductId, SKU,Weight, Stock,WarningStock, CostPrice,FreezeStock = ISNULL(FreezeStock,0),StoreStock=Stock, SalePrice FROM Hishop_SKUs WHERE ProductId = @ProductId;");
			}
			stringBuilder.Append(" SELECT SkuId, a.AttributeId, AttributeName, UseAttributeImage, av.ValueId, ValueStr, pi.ImageUrl,pi.ThumbnailUrl40,pi.ThumbnailUrl410 FROM Hishop_SKUItems s join Hishop_Attributes a on s.AttributeId = a.AttributeId join Hishop_AttributeValues av on s.ValueId = av.ValueId LEFT JOIN (select * from Hishop_ProductSpecificationImages where ProductId=@ProductId) pi ON s.ValueId=pi.ValueId WHERE SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId = @ProductId)  ORDER BY a.DisplaySequence DESC,av.DisplaySequence DESC;");
			stringBuilder.Append(" SELECT Count(*) FROM Hishop_ProductReviews where ProductId=@ProductId; ");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					productBrowseInfo.Product = DataMapper.PopulateProduct(dataReader);
				}
				if (dataReader.NextResult())
				{
					while (dataReader.Read())
					{
						SKUItem sKUItem = DataMapper.PopulateSKU(dataReader);
						if (sKUItem != null)
						{
							productBrowseInfo.Product.Skus.Add((string)((IDataRecord)dataReader)["SkuId"], sKUItem);
						}
					}
				}
				if (dataReader.NextResult())
				{
					productBrowseInfo.DbSKUs = DataHelper.ConverDataReaderToDataTable(dataReader);
				}
				if (dataReader.NextResult() && dataReader.Read())
				{
					productBrowseInfo.ReviewCount = (int)((IDataRecord)dataReader)[0];
				}
			}
			return productBrowseInfo;
		}

		public ProductBrowseInfo GetProductPreSaleBrowseInfo(int productId)
		{
			ProductBrowseInfo productBrowseInfo = new ProductBrowseInfo();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" SELECT * ,CASE WHEN BrandId IS NULL THEN NULL ELSE (SELECT BrandName FROM Hishop_BrandCategories WHERE BrandId= p.BrandId) END AS BrandName");
			stringBuilder.Append(" FROM Hishop_Products p where ProductId=@ProductId;");
			stringBuilder.Append("SELECT SkuId, ProductId, SKU,Weight, Stock,WarningStock, CostPrice,FreezeStock = ISNULL(FreezeStock,0),StoreStock=Stock, SalePrice FROM Hishop_SKUs WHERE ProductId = @ProductId;");
			stringBuilder.Append(" SELECT Count(*) FROM Hishop_ProductReviews where ProductId=@ProductId; ");
			stringBuilder.Append($"select count(*) from dbo.Hishop_OrderItems as items left join Hishop_Orders orders on items.OrderId=orders.OrderId where orders.OrderStatus!={1} and orders.OrderStatus!={4} and items.ProductId=@ProductId");
			stringBuilder.Append(" SELECT Count(*) FROM Hishop_ProductConsultations where ProductId=@ProductId AND ReplyUserId IS NOT NULL ; ");
			stringBuilder.Append(" SELECT a.AttributeId, AttributeName, ValueStr FROM Hishop_ProductAttributes pa JOIN Hishop_Attributes a ON pa.AttributeId = a.AttributeId");
			stringBuilder.Append(" JOIN Hishop_AttributeValues v ON a.AttributeId = v.AttributeId AND pa.ValueId = v.ValueId  WHERE ProductId = @ProductId ORDER BY a.DisplaySequence DESC, v.DisplaySequence DESC");
			stringBuilder.Append(" SELECT SkuId, a.AttributeId, AttributeName, UseAttributeImage, av.ValueId, ValueStr, pi.ImageUrl,pi.ThumbnailUrl40,pi.ThumbnailUrl410 FROM Hishop_SKUItems s join Hishop_Attributes a on s.AttributeId = a.AttributeId join Hishop_AttributeValues av on s.ValueId = av.ValueId LEFT JOIN (select * from Hishop_ProductSpecificationImages where ProductId=@ProductId) pi ON s.ValueId=pi.ValueId WHERE SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId = @ProductId)  ORDER BY a.DisplaySequence DESC,av.DisplaySequence DESC;");
			stringBuilder.Append(" SELECT TOP 20 ProductId,ProductName,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice,SalePrice FROM vw_Hishop_BrowseProductList");
			stringBuilder.AppendFormat(" WHERE SaleStatus = {0} AND ProductId IN (SELECT RelatedProductId FROM Hishop_RelatedProducts WHERE ProductId = {1}) ORDER BY DisplaySequence DESC;", 1, productId);
			stringBuilder.Append("SELECT TOP 20 ProductId,ProductName,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice,SalePrice FROM vw_Hishop_BrowseProductList");
			stringBuilder.AppendFormat(" WHERE SaleStatus = {0} AND ProductId<>{1}  AND CategoryId = (SELECT CategoryId FROM Hishop_Products WHERE ProductId={1} AND SaleStatus = {0})", 1, productId);
			stringBuilder.AppendFormat(" AND ProductId NOT IN (SELECT RelatedProductId FROM Hishop_RelatedProducts WHERE ProductId = {0})", productId);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					productBrowseInfo.Product = DataMapper.PopulateProduct(dataReader);
					if (((IDataRecord)dataReader)["BrandName"] != DBNull.Value)
					{
						productBrowseInfo.BrandName = (string)((IDataRecord)dataReader)["BrandName"];
					}
				}
				if (dataReader.NextResult())
				{
					while (dataReader.Read())
					{
						SKUItem sKUItem = DataMapper.PopulateSKU(dataReader);
						if (sKUItem != null)
						{
							productBrowseInfo.Product.Skus.Add((string)((IDataRecord)dataReader)["SkuId"], sKUItem);
						}
					}
				}
				if (dataReader.NextResult() && dataReader.Read())
				{
					productBrowseInfo.ReviewCount = (int)((IDataRecord)dataReader)[0];
				}
				if (dataReader.NextResult() && dataReader.Read())
				{
					productBrowseInfo.SaleCount = (int)((IDataRecord)dataReader)[0];
				}
				if (dataReader.NextResult() && dataReader.Read())
				{
					productBrowseInfo.ConsultationCount = (int)((IDataRecord)dataReader)[0];
				}
				if (dataReader.NextResult())
				{
					DataTable dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
					if (dataTable != null && dataTable.Rows.Count > 0)
					{
						DataTable dataTable2 = dataTable.Clone();
						foreach (DataRow row in dataTable.Rows)
						{
							bool flag = false;
							if (dataTable2.Rows.Count > 0)
							{
								foreach (DataRow row2 in dataTable2.Rows)
								{
									if ((int)row2["AttributeId"] == (int)row["AttributeId"])
									{
										flag = true;
										DataRow dataRow3 = row2;
										dataRow3["ValueStr"] = dataRow3["ValueStr"] + ", " + row["ValueStr"];
									}
								}
							}
							if (!flag)
							{
								DataRow dataRow4 = dataTable2.NewRow();
								dataRow4["AttributeId"] = row["AttributeId"];
								dataRow4["AttributeName"] = row["AttributeName"];
								dataRow4["ValueStr"] = row["ValueStr"];
								dataTable2.Rows.Add(dataRow4);
							}
						}
						productBrowseInfo.DbAttribute = dataTable2;
					}
				}
				if (dataReader.NextResult())
				{
					productBrowseInfo.DbSKUs = DataHelper.ConverDataReaderToDataTable(dataReader);
					productBrowseInfo.ListSKUs = DataHelper.ReaderToList<SKUItem>(dataReader);
				}
				if (dataReader.NextResult())
				{
					productBrowseInfo.DbCorrelatives = DataHelper.ConverDataReaderToDataTable(dataReader);
					productBrowseInfo.ListCorrelatives = DataHelper.ReaderToList<ProductInfo>(dataReader);
				}
				if (dataReader.NextResult())
				{
					productBrowseInfo.DbCorrelatives.Merge(DataHelper.ConverDataReaderToDataTable(dataReader), true);
					IList<ProductInfo> list = DataHelper.ReaderToList<ProductInfo>(dataReader);
					if (list != null)
					{
						foreach (ProductInfo item in list)
						{
							productBrowseInfo.ListCorrelatives.Add(item);
						}
					}
				}
			}
			return productBrowseInfo;
		}

		public DbQueryResult GetBrowseProductList(ProductBrowseQuery query)
		{
			string filter = this.BuildProductBrowseQuerySearch(query);
			string selectFields = "ProductId,ProductName,ProductCode, ShowSaleCounts AS SaleCounts, ShortDescription, ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice, SalePrice,Stock";
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_BrowseProductList p", "ProductId", filter, selectFields);
		}

		public DataTable GetVistiedProducts(IList<int> productIds)
		{
			if (productIds.Count == 0)
			{
				return null;
			}
			string text = string.Empty;
			for (int i = 0; i < productIds.Count; i++)
			{
				text = text + productIds[i] + ",";
			}
			text = text.Remove(text.Length - 1);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand($"SELECT ProductId,ProductName,ProductCode,ShowSaleCounts AS SaleCounts, ShortDescription,ThumbnailUrl40, ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160,ThumbnailUrl180,MarketPrice,SalePrice  FROM  vw_Hishop_BrowseProductList WHERE ProductId IN({text}) and SaleStatus={1}");
			DataTable result = default(DataTable);
			using (IDataReader reader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(reader);
			}
			return result;
		}

		public DbQueryResult GetBrandProducts(int? brandId, int pageNumber, int maxNum)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("ProductId,ProductName,ProductCode,ShowSaleCounts AS SaleCounts,ShortDescription,ShowSaleCounts,");
			stringBuilder.Append(" ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160,ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice,SalePrice");
			StringBuilder stringBuilder2 = new StringBuilder();
			stringBuilder2.Append(" SaleStatus=1");
			if (brandId.HasValue)
			{
				stringBuilder2.AppendFormat(" AND BrandId = {0}", brandId);
			}
			return DataHelper.PagingByRownumber(pageNumber, maxNum, "DisplaySequence", SortAction.Desc, true, "vw_Hishop_BrowseProductList s", "ProductId", stringBuilder2.ToString(), stringBuilder.ToString());
		}

		public DbQueryResult GetProducts(CategoryInfo category, string keyWord, string productIds, int pageNumber, int maxNum, string sort, bool isAsc = false)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("ProductId,ProductName,ProductCode,ShowSaleCounts AS SaleCounts,ShortDescription,", maxNum);
			stringBuilder.Append(" ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160,ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice,VistiCounts,SalePrice");
			StringBuilder stringBuilder2 = new StringBuilder();
			stringBuilder2.Append(" SaleStatus=1");
			if (category != null)
			{
				stringBuilder2.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%' OR ExtendCategoryPath LIKE '{0}|%' OR ExtendCategoryPath1 LIKE '{0}|%' OR ExtendCategoryPath2 LIKE '{0}|%' OR ExtendCategoryPath3 LIKE '{0}|%' OR ExtendCategoryPath4 LIKE '{0}|%') ", category.Path);
			}
			if (!string.IsNullOrEmpty(keyWord))
			{
				stringBuilder2.AppendFormat(" AND (ProductName LIKE '%{0}%' OR ProductCode LIKE '%{0}%')", keyWord);
			}
			if (!string.IsNullOrEmpty(productIds))
			{
				stringBuilder2.AppendFormat(" AND CHARINDEX(','+cast(ProductId as varchar)+',',','+'{0}'+',')>0 ", productIds);
			}
			if (string.IsNullOrWhiteSpace(sort))
			{
				sort = "ProductId";
			}
			return DataHelper.PagingByRownumber(pageNumber, maxNum, sort, isAsc ? SortAction.Asc : SortAction.Desc, true, "vw_Hishop_BrowseProductList", "ProductId", stringBuilder2.ToString(), stringBuilder.ToString());
		}

		public DbQueryResult GetBatchBuyProducts(ProductQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" SaleStatus = 1 ");
			if (query.BrandId.HasValue)
			{
				stringBuilder.AppendFormat(" AND BrandId = {0}", query.BrandId.Value);
			}
			if (query.TypeId.HasValue)
			{
				stringBuilder.AppendFormat(" AND TypeId = {0}", query.TypeId.Value);
			}
			if (query.TagId.HasValue && query.TagId.Value > 0)
			{
				stringBuilder.AppendFormat(" AND ProductId IN (SELECT ProductId FROM Hishop_ProductTag WHERE TagId={0})", query.TagId);
			}
			if (!string.IsNullOrEmpty(query.Keywords))
			{
				query.Keywords = DataHelper.CleanSearchString(query.Keywords);
				string[] array = Regex.Split(query.Keywords.Trim(), "\\s+");
				stringBuilder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[0]));
				for (int i = 1; i < array.Length && i <= 4; i++)
				{
					stringBuilder.AppendFormat("AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[i]));
				}
			}
			if (!string.IsNullOrEmpty(query.ProductCode))
			{
				stringBuilder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
			}
			if (query.CategoryId.HasValue && query.CategoryId.Value > 0)
			{
				stringBuilder.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%'  OR ExtendCategoryPath LIKE '{0}|%' OR ExtendCategoryPath1 LIKE '{0}|%' OR ExtendCategoryPath2 LIKE '{0}|%' OR ExtendCategoryPath3 LIKE '{0}|%' OR ExtendCategoryPath4 LIKE '{0}|%')", query.MaiCategoryPath);
			}
			string selectFields = "ProductId, ProductCode,ProductName,ThumbnailUrl100";
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_BrowseProductList p", "ProductId", stringBuilder.ToString(), selectFields);
		}

		protected string BuildProductBrowseQuerySearch(ProductBrowseQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SaleStatus = {0}", (int)query.ProductSaleStatus);
			if (!query.IsPrecise)
			{
				if (!string.IsNullOrEmpty(query.ProductCode))
				{
					stringBuilder.AppendFormat(" AND ProductCode LIKE  '%{0}%'", DataHelper.CleanSearchString(query.ProductCode).ToLower());
				}
			}
			else if (!string.IsNullOrEmpty(query.ProductCode))
			{
				stringBuilder.AppendFormat(" AND ProductCode = '{0}'", DataHelper.CleanSearchString(query.ProductCode).ToLower());
			}
			if (query.AttributeValues.Count > 0)
			{
				foreach (AttributeValueInfo attributeValue in query.AttributeValues)
				{
					stringBuilder.AppendFormat(" AND ProductId IN ( SELECT ProductId FROM Hishop_ProductAttributes WHERE AttributeId={0} And ValueId={1}) ", attributeValue.AttributeId, attributeValue.ValueId);
				}
			}
			if (query.BrandId.HasValue)
			{
				if (query.BrandId.Value == 0)
				{
					stringBuilder.Append(" AND BrandId IS NOT NULL");
				}
				else
				{
					stringBuilder.AppendFormat(" AND BrandId = {0}", query.BrandId.Value);
				}
			}
			ProductType? productType = query.ProductType;
			if (productType > ProductType.All)
			{
				StringBuilder stringBuilder2 = stringBuilder;
				productType = query.ProductType;
				stringBuilder2.Append(" AND ProductType =" + productType.GetHashCode());
			}
			if (query.MinSalePrice.HasValue)
			{
				stringBuilder.AppendFormat(" AND SalePrice >= {0}", query.MinSalePrice.Value);
			}
			if (query.MaxSalePrice.HasValue)
			{
				stringBuilder.AppendFormat(" AND SalePrice <= {0}", query.MaxSalePrice.Value);
			}
			if (!string.IsNullOrEmpty(query.Keywords) && query.Keywords.Trim().Length > 0)
			{
				if (!query.IsPrecise)
				{
					query.Keywords = DataHelper.CleanSearchString(query.Keywords);
					string[] array = Regex.Split(query.Keywords.Trim(), "\\s+");
					List<string> list = new List<string>();
					list.Add(string.Format("(ProductName LIKE '%{0}%' OR ProductCode LIKE '%{0}%')", DataHelper.CleanSearchString(array[0])));
					for (int i = 1; i < array.Length && i <= 4; i++)
					{
						list.Add(string.Format("(ProductName LIKE '%{0}%' OR ProductCode LIKE '%{0}%')", DataHelper.CleanSearchString(array[i])));
					}
					stringBuilder.Append(" AND (" + string.Join(" AND ", list.ToArray()) + ")");
				}
				else
				{
					stringBuilder.AppendFormat(" AND ProductName = '{0}'", DataHelper.CleanSearchString(query.Keywords));
				}
			}
			if (query.Category != null)
			{
				stringBuilder.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%' OR ExtendCategoryPath LIKE '{0}|%' OR ExtendCategoryPath1 LIKE '{0}|%' OR ExtendCategoryPath2 LIKE '{0}|%' OR ExtendCategoryPath3 LIKE '{0}|%' OR ExtendCategoryPath4 LIKE '{0}|%') ", query.Category.Path);
			}
			if (!string.IsNullOrEmpty(query.TagIds))
			{
				string[] array2 = query.TagIds.Split('_');
				string[] array3 = array2;
				foreach (string text in array3)
				{
					int num = 0;
					if (int.TryParse(text, out num) && !string.IsNullOrEmpty(text) && num > 0)
					{
						stringBuilder.AppendFormat(" AND ProductId IN(SELECT ProductId FROM Hishop_ProductTag WHERE TagId = {0})", num.ToString());
					}
				}
			}
			if (!string.IsNullOrEmpty(query.CanUseProducts) && query.CanUseProducts.Trim().Length > 0)
			{
				stringBuilder.AppendFormat(" AND ProductId IN ('{0}')", query.CanUseProducts.Replace(",", "','"));
			}
			return stringBuilder.ToString();
		}

		protected string BuildAPIProductBrowseQuerySearch(ProductQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder(" 1=1 ");
			if (query.SaleStatus != ProductSaleStatus.All)
			{
				stringBuilder.AppendFormat(" AND SaleStatus = {0}", (int)query.SaleStatus);
			}
			else
			{
				stringBuilder.AppendFormat(" AND SaleStatus not in ({0})", 0);
			}
			if (query.BrandId.HasValue)
			{
				stringBuilder.AppendFormat(" AND BrandId = {0}", query.BrandId.Value);
			}
			if (query.TagId.HasValue)
			{
				stringBuilder.AppendFormat("AND ProductId IN (SELECT ProductId FROM Hishop_ProductTag WHERE TagId={0})", query.TagId);
			}
			if (!string.IsNullOrEmpty(query.Keywords))
			{
				query.Keywords = DataHelper.CleanSearchString(query.Keywords);
				string[] array = Regex.Split(query.Keywords.Trim(), "\\s+");
				stringBuilder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[0]));
				for (int i = 1; i < array.Length && i <= 4; i++)
				{
					stringBuilder.AppendFormat("AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[i]));
				}
			}
			if (!string.IsNullOrEmpty(query.ProductCode))
			{
				stringBuilder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
			}
			if (query.CategoryId.HasValue && query.CategoryId.Value > 0)
			{
				stringBuilder.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%'  OR ExtendCategoryPath LIKE '{0}|%' OR ExtendCategoryPath1 LIKE '{0}|%' OR ExtendCategoryPath2 LIKE '{0}|%' OR ExtendCategoryPath3 LIKE '{0}|%' OR ExtendCategoryPath4 LIKE '{0}|%')", query.MaiCategoryPath);
			}
			if (query.IsMakeTaobao.HasValue && query.IsMakeTaobao.Value >= 0)
			{
				stringBuilder.AppendFormat(" AND IsMaketaobao={0}", query.IsMakeTaobao.Value);
			}
			if (query.PublishStatus != 0)
			{
				if (query.PublishStatus == PublishStatus.Notyet)
				{
					stringBuilder.Append(" AND TaobaoProductId = 0");
				}
				else
				{
					stringBuilder.Append(" AND TaobaoProductId <> 0");
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
			return stringBuilder.ToString();
		}

		public DataTable ApiGetSkusByProductId(int pid)
		{
			string query = $"select skuId,SKU,Stock,SalePrice,(select distinct(select  valuestr+' ' from (select valuestr from Hishop_AttributeValues where valueid in(select ValueId from Hishop_SKUItems where skuId =hishop_skus.Skuid)) a for xml  path(''))as valuestr)as valuestr from hishop_skus where productid={pid}";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			return base.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}

		public ProductModel GetProductSkus(int productId, int gradeId, bool MutiStores = false, int storeId = 0)
		{
			int num = 100;
			ProductModel productModel = new ProductModel();
			StringBuilder stringBuilder = new StringBuilder("SELECT ThumbnailUrl160,SaleStatus FROM Hishop_Products WHERE ProductId = @ProductId;");
			if (gradeId > 0)
			{
				MemberGradeInfo memberGradeInfo = new MemberGradeDao().Get<MemberGradeInfo>(gradeId);
				num = (memberGradeInfo?.Discount ?? 1);
			}
			if (storeId == 0)
			{
				if (gradeId > 0)
				{
					if (!MutiStores)
					{
						stringBuilder.Append("SELECT SkuId, ProductId, SKU,Weight, Stock,WarningStock, CostPrice,FreezeStock = ISNULL(FreezeStock,0),StoreStock=Stock,");
					}
					else
					{
						stringBuilder.Append("SELECT SkuId, ProductId, s.SKU,s.Weight, Stock,WarningStock, s.CostPrice,FreezeStock = ISNULL(FreezeStock,0),StoreStock=(SELECT ISNULL(MAX(Stock),s.Stock) FROM Hishop_StoreSKUs WHERE SkuId = s.SkuID AND StoreId IN (SELECT StoreId FROM Hishop_Stores WHERE CloseStatus = 1 AND State = 1)),");
					}
					stringBuilder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0}) = 1", gradeId);
					stringBuilder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Hishop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice", gradeId, num);
					stringBuilder.Append(" FROM Hishop_SKUs s WHERE ProductId = @ProductId;");
				}
				else if (!MutiStores)
				{
					stringBuilder.Append("SELECT SkuId, ProductId, SKU,Weight, Stock,WarningStock, CostPrice,FreezeStock = ISNULL(FreezeStock,0),StoreStock = Stock, SalePrice FROM Hishop_SKUs WHERE ProductId = @ProductId;");
				}
				else
				{
					stringBuilder.Append("SELECT SkuId, ProductId, SKU,Weight, Stock,WarningStock, CostPrice,FreezeStock = ISNULL(FreezeStock,0),StoreStock = (SELECT ISNULL(MAX(stock),s.Stock) FROM Hishop_StoreSKUs ss WHERE SkuId = s.SkuID AND StoreId IN (SELECT StoreId FROM Hishop_Stores WHERE CloseStatus = 1 AND State = 1)), SalePrice FROM Hishop_SKUs s WHERE ProductId = @ProductId;");
				}
			}
			else
			{
				stringBuilder.Append("SELECT hs.SkuId, hs.ProductId, SKU,[Weight], ss.Stock,ss.WarningStock, CostPrice,FreezeStock = ISNULL(ss.FreezeStock,0),StoreStock = ss.Stock,");
				stringBuilder.AppendFormat("(CASE WHEN ss.StoreSalePrice > 0 THEN ss.StoreSalePrice ELSE hs.SalePrice END)*{0}/100 AS SalePrice", num);
				stringBuilder.Append(" FROM Hishop_SKUs hs INNER JOIN [Hishop_StoreSKUs] ss ON ss.SkuId = hs.SkuId");
				stringBuilder.Append(" WHERE hs.ProductId = @ProductId AND ss.StoreId = @StoreId;");
			}
			stringBuilder.Append(" SELECT SkuId, a.AttributeId, AttributeName, UseAttributeImage, av.ValueId, ValueStr, pi.ImageUrl FROM Hishop_SKUItems s JOIN Hishop_Attributes a ON s.AttributeId = a.AttributeId JOIN Hishop_AttributeValues av ON s.ValueId = av.ValueId LEFT JOIN (SELECT * FROM Hishop_ProductSpecificationImages WHERE ProductId = @ProductId) pi ON s.ValueId = pi.ValueId WHERE SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId = @ProductId) ORDER BY a.DisplaySequence DESC,av.DisplaySequence DESC;");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, storeId);
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					productModel.SubmitOrderImg = ((IDataRecord)dataReader)["ThumbnailUrl160"].ToNullString();
					productModel.SaleStatus = (ProductSaleStatus)((IDataRecord)dataReader)["SaleStatus"].ToInt(0);
				}
				if (dataReader.NextResult())
				{
					while (dataReader.Read())
					{
						SKUItem sKUItem = DataMapper.PopulateSKU(dataReader);
						if (sKUItem != null)
						{
							if (productModel.Skus == null)
							{
								productModel.Skus = new List<SKUItem>();
							}
							productModel.Skus.Add(sKUItem);
						}
					}
				}
				if (dataReader.NextResult())
				{
					productModel.SkuTable = DataHelper.ConverDataReaderToDataTable(dataReader);
				}
			}
			return productModel;
		}
	}
}
