using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Depot;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Hidistro.SqlDal.Depot
{
	public class StoreStockDao : BaseDao
	{
		public bool DeleteProduct(int StoreId, string IDList)
		{
			string text = "DELETE FROM Hishop_StoreSKUs WHERE StoreID=@StoreID AND ProductId in('" + IDList + "')";
			text = text + " Update Hishop_StoreProducts Set SaleStatus = " + 2 + " WHERE ProductId in('" + IDList + "') And StoreId = @StoreId";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(text);
			base.database.AddInParameter(sqlStringCommand, "StoreID", DbType.Int32, StoreId);
			return base.database.ExecuteNonQuery(sqlStringCommand) >= 1;
		}

		public bool DeleteProduct(int StoreId, int ProductID)
		{
			string text = "DELETE FROM Hishop_StoreSKUs WHERE ProductId = @ProductId And StoreId = @StoreId";
			text = text + " Update Hishop_StoreProducts Set SaleStatus = " + 2 + " WHERE ProductId = @ProductId And StoreId = @StoreId";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(text);
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, ProductID);
			base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, StoreId);
			return base.database.ExecuteNonQuery(sqlStringCommand) >= 1;
		}

		public bool StoreHasProduct(int StoreID, int ProductId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("Select Count(ProductID) FROM Hishop_StoreSKUs WHERE ProductId = @ProductId And StoreId = @StoreId ");
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, ProductId);
			base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, StoreID);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0) > 0;
		}

		public bool ProductHasStores(int ProductId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("Select Count(StoreId) FROM Hishop_storeProducts WHERE ProductId=@ProductId AND SaleStatus=1");
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, ProductId);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0) > 0;
		}

		public bool ProductInStoreAndIsAboveSelf(int productId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("select COUNT(StoreId) from Hishop_Stores WHERE StoreId IN( SELECT  StoreId FROM Hishop_storeProducts WHERE ProductId=@ProductId AND SaleStatus=1 ) AND IsAboveSelf = 1");
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0) > 0;
		}

		public bool StoreHasProductSku(int StoreID, string SkuId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("Select COUNT(ProductID) FROM Hishop_StoreSKUs WHERE SkuId = @SkuId And StoreId = @StoreId ");
			base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, SkuId);
			base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, StoreID);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0) > 0;
		}

		public int GetStoreStock(int StoreID, string SkuId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("Select ISNULL(Stock,0) FROM Hishop_StoreSKUs WHERE StoreId = @StoreId AND SkuId = @SkuId ");
			base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, SkuId);
			base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, StoreID);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0);
		}

		public int GetStoreStock(string SkuId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("Select top 1 ISNULL(Stock,0) FROM Hishop_StoreSKUs WHERE  SkuId = @SkuId ");
			base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, SkuId);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0);
		}

		public IList<StoreProductModel> GetStorePrducts(int StoreID)
		{
			string query = $"SELECT distinct(ProductId) FROM Hishop_StoreSKUs WHERE StoreID = {StoreID}";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand);
			IList<StoreProductModel> list = new List<StoreProductModel>();
			while (dataReader.Read())
			{
				list.Add(this.GetStoreProductInfo(StoreID, dataReader.GetInt32(0)));
			}
			return list;
		}

		public StoreProductModel GetStoreProductInfo(int StoreID, int productId)
		{
			StoreProductModel storeProductModel = null;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM Hishop_Products WHERE ProductId = @ProductId;SELECT skus.ProductId, skus.SkuId, s.AttributeId, s.ValueId, skus.SKU, skus.SalePrice, skus.CostPrice, (select Stock from Hishop_StoreSKUs where SkuId = skus.SkuId And StoreId = @StoreId) as Stock,StoreStock=stock,(select WarningStock from Hishop_StoreSKUs where SkuId = skus.SkuId And StoreID = @StoreID) as WarningStock, skus.[Weight] FROM Hishop_SKUItems s right outer join Hishop_SKUs skus on s.SkuId = skus.SkuId WHERE skus.ProductId = @ProductId ORDER BY (SELECT DisplaySequence FROM Hishop_Attributes WHERE AttributeId = s.AttributeId) DESC;");
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			base.database.AddInParameter(sqlStringCommand, "StoreID", DbType.Int32, StoreID);
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				storeProductModel = DataHelper.ReaderToModel<StoreProductModel>(dataReader);
				if (storeProductModel != null)
				{
					dataReader.NextResult();
					while (dataReader.Read())
					{
						string key = (string)((IDataRecord)dataReader)["SkuId"];
						if (!storeProductModel.Skus.ContainsKey(key))
						{
							storeProductModel.Skus.Add(key, DataMapper.PopulateSKU(dataReader));
						}
						if (((IDataRecord)dataReader)["AttributeId"] != DBNull.Value && ((IDataRecord)dataReader)["ValueId"] != DBNull.Value)
						{
							storeProductModel.Skus[key].SkuItems.Add((int)((IDataRecord)dataReader)["AttributeId"], (int)((IDataRecord)dataReader)["ValueId"]);
						}
					}
				}
			}
			return storeProductModel;
		}

		public StoreProductModel GetStoreProductInfoForStoreApp(int StoreID, int productId)
		{
			StoreProductModel storeProductModel = null;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT ProductId,ProductName,ProductCode,ThumbnailUrl410 FROM Hishop_Products WHERE ProductId = @ProductId;SELECT skus.ProductId,\r\n\t            skus.SkuId,\t\r\n\t            skus.SKU,\r\n\t            skus.SalePrice, \r\n                skus.CostPrice,\r\n                skus.[Weight],\r\n                skus.Stock,\r\n                ssku.Stock as StoreStock,\r\n                ssku.WarningStock,\r\n                case when StoreSalePrice=0 then SalePrice else StoreSalePrice end storeSalePrice\r\n                FROM Hishop_StoreSKUs AS ssku\r\n\t            inner JOIN   Hishop_SKUs AS skus  ON skus.SkuId = ssku.SkuId \t \r\n\t            WHERE   ssku.ProductId = @ProductId AND ssku.StoreId = @StoreID");
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			base.database.AddInParameter(sqlStringCommand, "StoreID", DbType.Int32, StoreID);
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				storeProductModel = DataHelper.ReaderToModel<StoreProductModel>(dataReader);
				if (storeProductModel != null)
				{
					dataReader.NextResult();
					while (dataReader.Read())
					{
						string key = (string)((IDataRecord)dataReader)["SkuId"];
						if (!storeProductModel.Skus.ContainsKey(key))
						{
							SKUItem sKUItem = DataMapper.PopulateSKU(dataReader);
							sKUItem.StoreSalePrice = ((IDataRecord)dataReader)["StoreSalePrice"].ToDecimal(0);
							storeProductModel.Skus.Add(key, sKUItem);
						}
					}
				}
			}
			return storeProductModel;
		}

		public IList<StoreProductModel> GetNoStockStoreProductList(int storeId)
		{
			IList<StoreProductModel> list = new List<StoreProductModel>();
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT distinct(ProductID) from Hishop_StoreSKUs where StoreId = @StoreId AND (Stock <= WarningStock OR Stock <= 0)");
			base.database.AddInParameter(sqlStringCommand, "StoreID", DbType.Int32, storeId);
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					StoreProductModel storeProductInfo = this.GetStoreProductInfo(storeId, ((IDataRecord)dataReader)["ProductId"].ToInt(0));
					if (storeProductInfo != null)
					{
						list.Add(storeProductInfo);
					}
				}
			}
			return list;
		}

		public int GetNoStockStoreProductCount(int storeId)
		{
			IList<StoreProductModel> list = new List<StoreProductModel>();
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT COUNT(distinct(ProductID)) from Hishop_StoreSKUs where StoreId = @StoreId AND (Stock < WarningStock OR Stock = 0)");
			base.database.AddInParameter(sqlStringCommand, "StoreID", DbType.Int32, storeId);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0);
		}

		public DbQueryResult GetStorePrducts(StoreStockQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (query.IsRelation)
			{
				stringBuilder.Append($" ProductID in(select distinct(ProductID) FROM Hishop_StoreSKUs where  StoreID =  {query.StoreID}");
			}
			else
			{
				stringBuilder.Append($" ProductID not in(select distinct(ProductID) FROM Hishop_StoreSKUs where  StoreID = {query.StoreID}");
			}
			if (query.SaleStatus != ProductSaleStatus.All)
			{
				stringBuilder.AppendFormat(" AND SaleStatus = {0}", (int)query.SaleStatus);
			}
			else
			{
				stringBuilder.AppendFormat(" AND SaleStatus <> ({0})", 0);
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
			if (query.IsWarningStock)
			{
				stringBuilder.Append(" AND  WarningStockNum>0");
			}
			string selectFields = string.Format("CategoryId,ProductId, ProductCode,IsMakeTaobao,ProductName, ThumbnailUrl40, MarketPrice, SalePrice,ExtendCategoryPath,ExtendCategoryPath1,ExtendCategoryPath2,ExtendCategoryPath3,ExtendCategoryPath4, (SELECT CostPrice FROM Hishop_SKUs WHERE SkuId = p.SkuId) AS  CostPrice,  (SELECT SUM(Stock) FROM Hishop_StoreSKUs WHERE ProductId = p.ProductId And StoreID={0}) AS Stock,(SELECT TOP 1 [WarningStock] FROM Hishop_StoreSKUs WHERE ProductId = p.ProductId And StoreID={0}) AS WarningStock, DisplaySequence,SaleStatus", query.StoreID);
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_BrowseProductList p", "ProductId", stringBuilder.ToString(), selectFields);
		}

		public bool SaveStoreStock(IList<StoreSKUInfo> list, int UpdateType)
		{
			bool result = false;
			int num = 0;
			StringBuilder stringBuilder = new StringBuilder();
			foreach (StoreSKUInfo item in list)
			{
				if (!this.StoreHasProductSku(item.StoreId, item.SkuId))
				{
					DbCommand sqlStringCommand = base.database.GetSqlStringCommand("insert into Hishop_StoreSKUs (StoreId,ProductID,SkuId,Stock,WarningStock,StoreSalePrice) values(@StoreId,@ProductID,@SkuId,@Stock,@WarningStock,@StoreSalePrice)");
					base.database.AddInParameter(sqlStringCommand, "ProductID", DbType.Int32, item.ProductID);
					base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, item.SkuId);
					base.database.AddInParameter(sqlStringCommand, "Stock", DbType.Int32, item.Stock);
					base.database.AddInParameter(sqlStringCommand, "WarningStock", DbType.Int32, item.WarningStock);
					base.database.AddInParameter(sqlStringCommand, "StoreSalePrice", DbType.Decimal, item.StoreSalePrice);
					base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, item.StoreId);
					num += base.database.ExecuteNonQuery(sqlStringCommand);
				}
				else
				{
					string str = "UPDATE Hishop_StoreSKUs SET";
					switch (UpdateType)
					{
					case 1:
						str += " Stock = @Stock";
						break;
					case 2:
						str += " WarningStock = @WarningStock";
						break;
					case 3:
						str += " StoreSalePrice=@StoreSalePrice";
						break;
					case 4:
						str += " Stock = @Stock,WarningStock = @WarningStock";
						break;
					default:
						str += " Stock = @Stock,WarningStock = @WarningStock,StoreSalePrice=@StoreSalePrice";
						break;
					}
					str += " where StoreId = @StoreId AND SkuID = @SkuID";
					DbCommand sqlStringCommand2 = base.database.GetSqlStringCommand(str);
					base.database.AddInParameter(sqlStringCommand2, "StoreId", DbType.Int32, item.StoreId);
					base.database.AddInParameter(sqlStringCommand2, "SkuId", DbType.String, item.SkuId);
					switch (UpdateType)
					{
					case 1:
						base.database.AddInParameter(sqlStringCommand2, "Stock", DbType.Int32, item.Stock);
						break;
					case 2:
						base.database.AddInParameter(sqlStringCommand2, "WarningStock", DbType.Int32, item.WarningStock);
						break;
					case 3:
						base.database.AddInParameter(sqlStringCommand2, "StoreSalePrice", DbType.Decimal, item.StoreSalePrice);
						break;
					case 4:
						base.database.AddInParameter(sqlStringCommand2, "Stock", DbType.Int32, item.Stock);
						base.database.AddInParameter(sqlStringCommand2, "WarningStock", DbType.Int32, item.WarningStock);
						break;
					default:
						base.database.AddInParameter(sqlStringCommand2, "Stock", DbType.Int32, item.Stock);
						base.database.AddInParameter(sqlStringCommand2, "WarningStock", DbType.Int32, item.WarningStock);
						base.database.AddInParameter(sqlStringCommand2, "StoreSalePrice", DbType.Decimal, item.StoreSalePrice);
						break;
					}
					num += base.database.ExecuteNonQuery(sqlStringCommand2);
				}
			}
			if (num > 0)
			{
				result = true;
			}
			return result;
		}

		public bool EditStoreProduct(IList<StoreSKUInfo> list)
		{
			bool result = false;
			int num = 0;
			foreach (StoreSKUInfo item in list)
			{
				DbCommand sqlStringCommand = base.database.GetSqlStringCommand("if not exists (select * from Hishop_StoreSKUs where StoreId = @StoreId AND SkuID = @SkuID) begin insert into Hishop_StoreSKUs(ProductID, SkuId, StoreId, Stock, WarningStock, FreezeStock, StoreSalePrice) values(@ProductID,@SkuId,@StoreId, @Stock, @WarningStock, @FreezeStock, @StoreSalePrice) end else begin UPDATE Hishop_StoreSKUs SET Stock = @Stock, WarningStock = @WarningStock, StoreSalePrice = @StoreSalePrice where StoreId = @StoreId AND SkuID = @SkuID end");
				base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, item.StoreId);
				base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, item.SkuId);
				base.database.AddInParameter(sqlStringCommand, "Stock", DbType.Int32, item.Stock);
				base.database.AddInParameter(sqlStringCommand, "WarningStock", DbType.Int32, item.WarningStock);
				base.database.AddInParameter(sqlStringCommand, "StoreSalePrice", DbType.Decimal, item.StoreSalePrice);
				base.database.AddInParameter(sqlStringCommand, "ProductID", DbType.Int32, item.ProductID);
				base.database.AddInParameter(sqlStringCommand, "FreezeStock", DbType.Int32, item.FreezeStock.ToInt(0));
				num += base.database.ExecuteNonQuery(sqlStringCommand);
			}
			if (num > 0)
			{
				result = true;
			}
			return result;
		}

		public StoreProductInfo GetStoreProductModel(int ProductId, int StoreId)
		{
			StoreProductInfo result = null;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM [Hishop_StoreProducts] WHERE ProductID = @ProductId AND StoreId = @StoreId");
			base.database.AddInParameter(sqlStringCommand, "ProductID", DbType.Int32, ProductId);
			base.database.AddInParameter(sqlStringCommand, "storeId", DbType.Int32, StoreId);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToModel<StoreProductInfo>(objReader);
			}
			return result;
		}

		public bool AddStoreProduct(DataTable dtStoreStock, DataTable dtlog, List<int> lstProductId, int StoreId)
		{
			using (SqlConnection sqlConnection = new SqlConnection(base.database.ConnectionString))
			{
				sqlConnection.Open();
				SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
				try
				{
					foreach (int item in lstProductId)
					{
						StoreProductInfo storeProductModel = this.GetStoreProductModel(item, StoreId);
						if (storeProductModel != null)
						{
							storeProductModel.SaleStatus = 1;
							storeProductModel.UpdateTime = DateTime.Now;
							if (!this.Update(storeProductModel, sqlTransaction))
							{
								sqlTransaction.Rollback();
								return false;
							}
						}
						else
						{
							storeProductModel = new StoreProductInfo();
							storeProductModel.ProductId = item;
							storeProductModel.StoreId = StoreId;
							storeProductModel.SaleCounts = 0;
							storeProductModel.SaleStatus = 1;
							storeProductModel.UpdateTime = DateTime.Now;
							if (this.Add(storeProductModel, sqlTransaction) <= 0)
							{
								sqlTransaction.Rollback();
								return false;
							}
						}
					}
					SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(sqlConnection, SqlBulkCopyOptions.CheckConstraints, sqlTransaction);
					sqlBulkCopy.DestinationTableName = dtStoreStock.TableName;
					for (int i = 0; i < dtStoreStock.Columns.Count; i++)
					{
						sqlBulkCopy.ColumnMappings.Add(dtStoreStock.Columns[i].ColumnName, dtStoreStock.Columns[i].ColumnName);
					}
					sqlBulkCopy.WriteToServer(dtStoreStock);
					SqlBulkCopy sqlBulkCopy2 = new SqlBulkCopy(sqlConnection, SqlBulkCopyOptions.CheckConstraints, sqlTransaction);
					sqlBulkCopy2.DestinationTableName = dtlog.TableName;
					for (int j = 0; j < dtlog.Columns.Count; j++)
					{
						sqlBulkCopy2.ColumnMappings.Add(dtlog.Columns[j].ColumnName, dtlog.Columns[j].ColumnName);
					}
					sqlBulkCopy2.WriteToServer(dtlog);
					sqlTransaction.Commit();
					return true;
				}
				catch (Exception)
				{
					sqlTransaction.Rollback();
					return false;
				}
				finally
				{
					sqlConnection.Close();
				}
			}
		}

		public IList<StoreSKUInfo> GetStoreStockInfosByProduct(int storeId, int productId)
		{
			IList<StoreSKUInfo> result = null;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM [Hishop_StoreSKUs] WHERE ProductID = @ProductId AND StoreId = @StoreId");
			base.database.AddInParameter(sqlStringCommand, "ProductID", DbType.Int32, productId);
			base.database.AddInParameter(sqlStringCommand, "storeId", DbType.Int32, storeId);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<StoreSKUInfo>(objReader);
			}
			return result;
		}

		public IList<StoreSKUInfo> GetStoreStockInfosBySkuIds(int storeId, string skuIds)
		{
			IList<StoreSKUInfo> result = null;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM [Hishop_StoreSKUs] WHERE StoreId = @StoreId AND SkuId in (" + skuIds + ")");
			base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, storeId);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<StoreSKUInfo>(objReader);
			}
			return result;
		}

		public IList<StoreSKUInfo> GetStoreStockInfosByProductIds(int storeId, string ProductIds)
		{
			IList<StoreSKUInfo> result = null;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM [Hishop_StoreSKUs] WHERE StoreId = @StoreId AND ProductID in (" + ProductIds + ")");
			base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, storeId);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<StoreSKUInfo>(objReader);
			}
			return result;
		}

		public bool UpdateStoreStockForEditProduct(int ProductID, string SkuidList, string ManagerUserName)
		{
			bool result = false;
			if (string.IsNullOrEmpty(SkuidList))
			{
				return result;
			}
			int num = 0;
			string text = SkuidList.Split(',').Aggregate(string.Empty, (string a, string current) => a + "'" + current + "',").TrimEnd(',');
			if (string.IsNullOrEmpty(text))
			{
				return true;
			}
			string query = "INSERT INTO HISHOP_STORESTOCKLOG (STOREID, CHANGETIME, PRODUCTID, SKUID, CONTENT, REMARK, OPERATOR) \r\n                                            SELECT STORESTOCK.STOREID,GETDATE() CHANGETIME,STORESTOCK.PRODUCTID,STORESTOCK.SKUID,PRO.PRODUCTNAME+ ';' + ISNULL(ATTR.ATTRV,'') + '库存由【'+ CAST(STORESTOCK.STOCK AS VARCHAR(20)) +'】修改为【0】' CONTENT,'由平台删除规格' REMARK,'ADMIN' OPERATOR \r\n                                            FROM Hishop_StoreSKUs STORESTOCK \r\n                                            LEFT JOIN HISHOP_PRODUCTS PRO ON PRO.PRODUCTID = STORESTOCK.PRODUCTID \r\n                                            LEFT JOIN \r\n                                            (SELECT SKUID,ATTRV=STUFF( (SELECT ';' + NVALUE FROM (SELECT SKUID, ATTRIBUTENAME+':'+VALUESTR NVALUE FROM HISHOP_SKUITEMS SI JOIN HISHOP_ATTRIBUTES A ON SI.ATTRIBUTEID = A.ATTRIBUTEID JOIN HISHOP_ATTRIBUTEVALUES AV ON SI.VALUEID = AV.VALUEID) AS B    WHERE B.SKUID = A.SKUID FOR XML PATH('')) , 1 , 1 , '' )  FROM \r\n                                            (SELECT SKUID, ATTRIBUTENAME+':'+VALUESTR NVALUE FROM HISHOP_SKUITEMS SI JOIN HISHOP_ATTRIBUTES A ON SI.ATTRIBUTEID = A.ATTRIBUTEID JOIN HISHOP_ATTRIBUTEVALUES AV ON SI.VALUEID = AV.VALUEID) A GROUP BY SKUID ) ATTR \r\n                                            ON ATTR.SKUID=STORESTOCK.SKUID \r\n                                            WHERE STORESTOCK.PRODUCTID = @PRODUCTID AND STORESTOCK.SKUID NOT IN (" + text + ")";
			string query2 = "DELETE Hishop_StoreSKUs WHERE PRODUCTID = @PRODUCTID AND SKUID NOT IN(" + text + ");update dbo.Hishop_StoreProducts  set SaleStatus=0 where productid=@PRODUCTID and  (not exists(select * from [Hishop_StoreSKUs] where ProductID=Hishop_StoreProducts.ProductId ))";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "ProductID", DbType.Int32, ProductID);
			num += base.database.ExecuteNonQuery(sqlStringCommand);
			if (num > 0)
			{
				sqlStringCommand = base.database.GetSqlStringCommand(query2);
				base.database.AddInParameter(sqlStringCommand, "ProductID", DbType.Int32, ProductID);
				num += base.database.ExecuteNonQuery(sqlStringCommand);
			}
			if (num > 0)
			{
				result = true;
			}
			return result;
		}

		public DbQueryResult GetStoreViewPrducts(StoreStockQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append($" ProductID in (select distinct(ProductID) FROM Hishop_StoreSKUs WHERE StoreID = {query.StoreID})");
			stringBuilder.AppendFormat(" AND SaleStatus = {0}", 1);
			string str = " ProductId, ProductCode,ProductName,ProductType,ThumbnailUrl40,ThumbnailUrl220, MarketPrice, ";
			str += string.Format("case when (SELECT MIN(StoreSalePrice) FROM dbo.Hishop_StoreSKUs WHERE   ProductId = p.ProductId and StoreId='{0}')=0 then (SELECT MIN(SalePrice) FROM dbo.Hishop_SKUs WHERE   ProductId = p.ProductId ) else (SELECT MIN(StoreSalePrice) FROM dbo.Hishop_StoreSKUs WHERE   ProductId = p.ProductId and StoreId='{0}') end AS SalePrice", query.StoreID);
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_BrowseProductList p", "ProductId", stringBuilder.ToString(), str);
		}

		public bool StoreStockIsEnough(int storeId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT COUNT(StoreId) from Hishop_StoreSKUs where StoreId = @StoreId AND (Stock < WarningStock OR Stock = 0)");
			base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, storeId);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0) <= 0;
		}

		public int GetStoreProductStock(int storeId, int productId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT SUM(Stock) from Hishop_StoreSKUs where StoreId = @StoreId AND ProductId = @ProductId");
			base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, storeId);
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0);
		}

		public int ImportAllProductToAllStores(int storeId)
		{
			string text = "DELETE Hishop_StoreSKUs where StoreId=" + storeId + ";";
			text = text + "insert into Hishop_StoreSKUs select " + storeId + ",ProductId,SKuId,Stock,WarningStock,0 from Hishop_SKUs where ProductId in (select productId from Hishop_Products where SaleStatus=1)";
			text = text + "update Hishop_StoreProducts set SaleStatus = " + 1 + ",UpdateTime = getdate() where StoreId=" + storeId + " and  SaleStatus = " + 2 + " and productId in(select productId from Hishop_Products where SaleStatus=" + 1 + ");";
			text = text + "insert into Hishop_StoreProducts select " + storeId + ",ProductId,0,SaleStatus,getdate() from dbo.Hishop_Products where SaleStatus =" + 1 + " and productId not in(select productId from Hishop_StoreProducts where storeId = " + storeId + ");";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(text);
			return base.database.ExecuteNonQuery(sqlStringCommand).ToInt(0);
		}
	}
}
