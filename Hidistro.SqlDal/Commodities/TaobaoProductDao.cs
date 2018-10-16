using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.HOP;
using System;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Commodities
{
	public class TaobaoProductDao : BaseDao
	{
		public DataSet GetTaobaoProductDetails(int productId)
		{
			DataSet dataSet = new DataSet();
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT ProductId, HasSKU, ProductName, ProductCode, MarketPrice, (SELECT [Name] FROM Hishop_Categories WHERE CategoryId = p.CategoryId) AS CategoryName, (SELECT BrandName FROM Hishop_BrandCategories WHERE BrandId = p.BrandId) AS BrandName, (SELECT MIN(SalePrice) FROM Hishop_SKUs WHERE ProductId = p.ProductId) AS SalePrice, (SELECT MIN(CostPrice) FROM Hishop_SKUs WHERE ProductId = p.ProductId) AS CostPrice, (SELECT SUM(Stock) FROM Hishop_SKUs WHERE ProductId = p.ProductId) AS Stock FROM Hishop_Products p WHERE ProductId = @ProductId SELECT AttributeName, ValueStr FROM Hishop_ProductAttributes pa join Hishop_Attributes a ON pa.AttributeId = a.AttributeId JOIN Hishop_AttributeValues v ON a.AttributeId = v.AttributeId AND pa.ValueId = v.ValueId WHERE ProductId = @ProductId ORDER BY a.DisplaySequence DESC, v.DisplaySequence DESC SELECT Weight AS '重量', Stock AS '库存', CostPrice AS '成本价', SalePrice AS '一口价', SkuId AS '商家编码' FROM Hishop_SKUs s WHERE ProductId = @ProductId; SELECT SkuId AS '商家编码',AttributeName,UseAttributeImage,ValueStr FROM Hishop_SKUItems s join Hishop_Attributes a on s.AttributeId = a.AttributeId join Hishop_AttributeValues av on s.ValueId = av.ValueId WHERE SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId = @ProductId) ORDER BY a.DisplaySequence DESC SELECT * FROM Taobao_Products WHERE ProductId = @ProductId");
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			DataTable table = default(DataTable);
			DataTable table2 = default(DataTable);
			DataTable dataTable = default(DataTable);
			DataTable dataTable2 = default(DataTable);
			DataTable table3 = default(DataTable);
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				table = DataHelper.ConverDataReaderToDataTable(dataReader);
				dataReader.NextResult();
				table2 = DataHelper.ConverDataReaderToDataTable(dataReader);
				dataReader.NextResult();
				dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
				dataReader.NextResult();
				dataTable2 = DataHelper.ConverDataReaderToDataTable(dataReader);
				dataReader.NextResult();
				table3 = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			if (dataTable != null && dataTable.Rows.Count > 0 && dataTable2 != null && dataTable2.Rows.Count > 0)
			{
				foreach (DataRow row in dataTable2.Rows)
				{
					DataColumn dataColumn = new DataColumn();
					dataColumn.ColumnName = (string)row["AttributeName"];
					if (!dataTable.Columns.Contains(dataColumn.ColumnName))
					{
						dataTable.Columns.Add(dataColumn);
					}
				}
				foreach (DataRow row2 in dataTable.Rows)
				{
					foreach (DataRow row3 in dataTable2.Rows)
					{
						if (string.Compare((string)row2["商家编码"], (string)row3["商家编码"]) == 0)
						{
							row2[(string)row3["AttributeName"]] = row3["ValueStr"];
						}
					}
				}
			}
			dataSet.Tables.Add(table);
			dataSet.Tables.Add(table2);
			dataSet.Tables.Add(dataTable);
			dataSet.Tables.Add(table3);
			return dataSet;
		}

		public bool IsExitTaobaoProduct(long taobaoProductId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand($"SELECT COUNT(*) FROM Hishop_Products WHERE TaobaoProductId = {taobaoProductId}");
			return (int)base.database.ExecuteScalar(sqlStringCommand) > 0;
		}

		public DbQueryResult GetToTaobaoProducts(ProductQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SaleStatus<>{0}", 0);
			if (!string.IsNullOrEmpty(query.ProductCode))
			{
				stringBuilder.AppendFormat(" AND ProductCode = '{0}'", DataHelper.CleanSearchString(query.ProductCode));
			}
			if (!string.IsNullOrEmpty(query.Keywords))
			{
				stringBuilder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Keywords));
			}
			if (query.CategoryId.HasValue && query.CategoryId.Value > 0)
			{
				stringBuilder.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%' OR ExtendCategoryPath LIKE '{0}|%' OR ExtendCategoryPath1 LIKE '{0}|%' OR ExtendCategoryPath2 LIKE '{0}|%' OR ExtendCategoryPath3 LIKE '{0}|%' OR ExtendCategoryPath4 LIKE '{0}|%')", query.MaiCategoryPath);
			}
			if (query.StartDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND AddedDate >='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
			}
			if (query.EndDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND AddedDate <='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
			}
			if (query.PublishStatus == PublishStatus.Already)
			{
				stringBuilder.Append(" AND TaobaoProductId <> 0");
			}
			else if (query.PublishStatus == PublishStatus.Notyet)
			{
				stringBuilder.Append(" AND TaobaoProductId = 0");
			}
			if (query.IsMakeTaobao.HasValue)
			{
				if (query.IsMakeTaobao.Value == 1)
				{
					stringBuilder.Append(" AND ProductId IN (SELECT ProductId FROM Taobao_Products where ProductId = p.ProductId)");
				}
				else if (query.IsMakeTaobao.Value == 0)
				{
					stringBuilder.Append(" AND ProductId NOT IN (SELECT ProductId FROM Taobao_Products where ProductId = p.ProductId)");
				}
			}
			StringBuilder stringBuilder2 = new StringBuilder();
			stringBuilder2.Append("ProductId, ProductCode, ProductName, ThumbnailUrl40, MarketPrice, SalePrice, Stock, DisplaySequence,TaobaoProductId");
			stringBuilder2.Append(",(SELECT COUNT(*) FROM Taobao_Products WHERE ProductId = p.ProductId) AS IsMakeTaobao");
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_BrowseProductList p", "ProductId", stringBuilder.ToString(), stringBuilder2.ToString());
		}

		public PublishToTaobaoProductInfo GetTaobaoProduct(int productId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT tp.*, p.TaobaoProductId,p.ProductCode, p.Description, p.ImageUrl1, p.ImageUrl2, p.ImageUrl3, p.ImageUrl4, p.ImageUrl5,");
			stringBuilder.Append(" (SELECT MIN(SalePrice) FROM Hishop_SKUs WHERE ProductId = p.ProductId) AS SalePrice,");
			stringBuilder.Append(" (SELECT MIN(Weight) FROM Hishop_SKUs WHERE ProductId = p.ProductId) AS Weight");
			stringBuilder.AppendFormat(" FROM Hishop_Products p JOIN Taobao_Products tp ON p.ProductId = tp.ProductId WHERE p.ProductId = {0}", productId);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			PublishToTaobaoProductInfo publishToTaobaoProductInfo = null;
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					publishToTaobaoProductInfo = new PublishToTaobaoProductInfo();
					publishToTaobaoProductInfo.Cid = (long)((IDataRecord)dataReader)["Cid"];
					if (((IDataRecord)dataReader)["StuffStatus"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.StuffStatus = (string)((IDataRecord)dataReader)["StuffStatus"];
					}
					publishToTaobaoProductInfo.ProductId = (int)((IDataRecord)dataReader)["ProductId"];
					publishToTaobaoProductInfo.ProTitle = (string)((IDataRecord)dataReader)["ProTitle"];
					publishToTaobaoProductInfo.Num = (long)((IDataRecord)dataReader)["Num"];
					publishToTaobaoProductInfo.LocationState = (string)((IDataRecord)dataReader)["LocationState"];
					publishToTaobaoProductInfo.LocationCity = (string)((IDataRecord)dataReader)["LocationCity"];
					publishToTaobaoProductInfo.FreightPayer = (string)((IDataRecord)dataReader)["FreightPayer"];
					if (((IDataRecord)dataReader)["PostFee"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.PostFee = (decimal)((IDataRecord)dataReader)["PostFee"];
					}
					if (((IDataRecord)dataReader)["ExpressFee"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.ExpressFee = (decimal)((IDataRecord)dataReader)["ExpressFee"];
					}
					if (((IDataRecord)dataReader)["EMSFee"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.EMSFee = (decimal)((IDataRecord)dataReader)["EMSFee"];
					}
					publishToTaobaoProductInfo.HasInvoice = (bool)((IDataRecord)dataReader)["HasInvoice"];
					publishToTaobaoProductInfo.HasWarranty = (bool)((IDataRecord)dataReader)["HasWarranty"];
					publishToTaobaoProductInfo.HasDiscount = (bool)((IDataRecord)dataReader)["HasDiscount"];
					publishToTaobaoProductInfo.ValidThru = (long)((IDataRecord)dataReader)["ValidThru"];
					if (((IDataRecord)dataReader)["ListTime"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.ListTime = (DateTime)((IDataRecord)dataReader)["ListTime"];
					}
					if (((IDataRecord)dataReader)["PropertyAlias"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.PropertyAlias = (string)((IDataRecord)dataReader)["PropertyAlias"];
					}
					if (((IDataRecord)dataReader)["InputPids"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.InputPids = (string)((IDataRecord)dataReader)["InputPids"];
					}
					if (((IDataRecord)dataReader)["InputStr"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.InputStr = (string)((IDataRecord)dataReader)["InputStr"];
					}
					if (((IDataRecord)dataReader)["SkuProperties"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.SkuProperties = (string)((IDataRecord)dataReader)["SkuProperties"];
					}
					if (((IDataRecord)dataReader)["SkuQuantities"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.SkuQuantities = (string)((IDataRecord)dataReader)["SkuQuantities"];
					}
					if (((IDataRecord)dataReader)["SkuPrices"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.SkuPrices = (string)((IDataRecord)dataReader)["SkuPrices"];
					}
					if (((IDataRecord)dataReader)["SkuOuterIds"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.SkuOuterIds = (string)((IDataRecord)dataReader)["SkuOuterIds"];
					}
					if (((IDataRecord)dataReader)["FoodAttributes"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.FoodAttributes = (string)((IDataRecord)dataReader)["FoodAttributes"];
					}
					if (((IDataRecord)dataReader)["TaobaoProductId"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.TaobaoProductId = (long)((IDataRecord)dataReader)["TaobaoProductId"];
					}
					if (((IDataRecord)dataReader)["ProductCode"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.ProductCode = (string)((IDataRecord)dataReader)["ProductCode"];
					}
					if (((IDataRecord)dataReader)["Description"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.Description = (string)((IDataRecord)dataReader)["Description"];
					}
					if (((IDataRecord)dataReader)["ImageUrl1"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.ImageUrl1 = (string)((IDataRecord)dataReader)["ImageUrl1"];
					}
					if (((IDataRecord)dataReader)["ImageUrl2"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.ImageUrl2 = (string)((IDataRecord)dataReader)["ImageUrl2"];
					}
					if (((IDataRecord)dataReader)["ImageUrl3"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.ImageUrl3 = (string)((IDataRecord)dataReader)["ImageUrl3"];
					}
					if (((IDataRecord)dataReader)["ImageUrl4"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.ImageUrl4 = (string)((IDataRecord)dataReader)["ImageUrl4"];
					}
					if (((IDataRecord)dataReader)["ImageUrl5"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.ImageUrl5 = (string)((IDataRecord)dataReader)["ImageUrl5"];
					}
					publishToTaobaoProductInfo.SalePrice = (decimal)((IDataRecord)dataReader)["SalePrice"];
					if (((IDataRecord)dataReader)["Weight"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.Weight = (decimal)((IDataRecord)dataReader)["Weight"];
					}
				}
			}
			return publishToTaobaoProductInfo;
		}

		public bool UpdateTaobaoProductId(int productId, long taobaoProductId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand($"UPDATE Hishop_Products SET TaobaoProductId = {taobaoProductId} WHERE ProductId = {productId}");
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
	}
}
