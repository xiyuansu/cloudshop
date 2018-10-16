using Hidistro.Core;
using Hidistro.Entities.Commodities;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Commodities
{
	public class ProductBatchDao : BaseDao
	{
		public DataTable GetProductBaseInfo(string productIds)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand($"SELECT ProductId, ProductName, ProductCode, MarketPrice, ThumbnailUrl40, SaleCounts, ShowSaleCounts,DisplaySequence FROM Hishop_Products WHERE ProductId IN ({DataHelper.CleanSearchString(productIds)}) ORDER BY DisplaySequence DESC");
			DataTable result = null;
			using (IDataReader reader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(reader);
			}
			return result;
		}

		public DataTable GetProductSalepriceInfo(string productIds)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand($"SELECT MIN(SalePrice) AS MinSalePrice,ProductId  FROM Hishop_SKUs GROUP BY ProductId HAVING ProductId IN ({DataHelper.CleanSearchString(productIds)})");
			DataTable result = null;
			using (IDataReader reader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(reader);
			}
			return result;
		}

		public ProductInfo GetProductBaseInfo(int productId)
		{
			ProductInfo result = null;
			IList<ProductInfo> productBaseInfo = this.GetProductBaseInfo(new int[1]
			{
				productId
			});
			if (productBaseInfo.Count > 0)
			{
				result = productBaseInfo[0];
			}
			return result;
		}

		public IList<ProductInfo> GetProductBaseInfo(IEnumerable<int> productIds)
		{
			string productBaseInfoSelectSQL = this.getProductBaseInfoSelectSQL(productIds);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(productBaseInfoSelectSQL);
			IList<ProductInfo> result = null;
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<ProductInfo>(objReader);
			}
			return result;
		}

		private string getProductBaseInfoSelectSQL(IEnumerable<int> productIds)
		{
			return string.Format("SELECT ProductId, ProductName, ProductCode, MarketPrice, ThumbnailUrl40, SaleCounts, ShowSaleCounts FROM Hishop_Products WHERE ProductId IN ({0})", string.Join(",", productIds));
		}

		public bool UpdateProductReferralDeduct(string productIds, decimal subMemberDeduct, decimal secondLevelDeduct, decimal threeLevelDeduct)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand($"UPDATE Hishop_Products SET SubMemberDeduct = {subMemberDeduct}, SecondLevelDeduct = {secondLevelDeduct}, ThreeLevelDeduct = {threeLevelDeduct} WHERE ProductId IN ({productIds})");
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateProductNames(string productIds, string prefix, string suffix)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand($"UPDATE Hishop_Products SET ProductName = '{DataHelper.CleanSearchString(prefix)}'+ProductName+'{DataHelper.CleanSearchString(suffix)}' WHERE ProductId IN ({productIds})");
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool ReplaceProductNames(string productIds, string oldWord, string newWord)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand($"UPDATE Hishop_Products SET ProductName = REPLACE(ProductName, '{DataHelper.CleanSearchString(oldWord)}', '{DataHelper.CleanSearchString(newWord)}') WHERE ProductId IN ({productIds})");
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateProductBaseInfo(DataTable dt)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(" ");
			foreach (DataRow row in dt.Rows)
			{
				num++;
				string text = num.ToString();
				stringBuilder.AppendFormat(" UPDATE Hishop_Products SET ProductName = @ProductName{0}, ProductCode = @ProductCode{0}, MarketPrice = @MarketPrice{0},DisplaySequence = @DisplaySequence{0}", text);
				stringBuilder.AppendFormat(" WHERE ProductId = {0}", row["ProductId"]);
				base.database.AddInParameter(sqlStringCommand, "ProductName" + text, DbType.String, row["ProductName"]);
				base.database.AddInParameter(sqlStringCommand, "ProductCode" + text, DbType.String, row["ProductCode"]);
				base.database.AddInParameter(sqlStringCommand, "MarketPrice" + text, DbType.String, row["MarketPrice"]);
				base.database.AddInParameter(sqlStringCommand, "DisplaySequence" + text, DbType.Int32, row["DisplaySequence"]);
			}
			sqlStringCommand.CommandText = stringBuilder.ToString();
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateShowSaleCounts(string productIds, int showSaleCounts)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand($"UPDATE Hishop_Products SET ShowSaleCounts = {showSaleCounts} WHERE ProductId IN ({productIds})");
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateVisitCounts(int productId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand($"UPDATE Hishop_Products SET VistiCounts = VistiCounts + 1 WHERE ProductId = {productId}");
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateShowSaleCounts(string productIds, int showSaleCounts, string operation)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand($"UPDATE Hishop_Products SET ShowSaleCounts = SaleCounts {operation} {showSaleCounts} WHERE ProductId IN ({productIds})");
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateShowSaleCounts(DataTable dt)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (DataRow row in dt.Rows)
			{
				stringBuilder.AppendFormat(" UPDATE Hishop_Products SET ShowSaleCounts = {0} WHERE ProductId = {1}", row["ShowSaleCounts"], row["ProductId"]);
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public DataTable GetSkuStocks(string productIds)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT p.ProductId,ProductName,p.ProductCode, SkuId, SKU,SalePrice, Stock,WarningStock, ThumbnailUrl40 FROM Hishop_Products p JOIN Hishop_SKUs s ON p.ProductId = s.ProductId WHERE p.ProductId IN ({0})", DataHelper.CleanSearchString(productIds));
			stringBuilder.Append(" SELECT SkuId, AttributeName, ValueStr FROM Hishop_SKUItems si JOIN Hishop_Attributes a ON si.AttributeId = a.AttributeId JOIN Hishop_AttributeValues av ON si.ValueId = av.ValueId");
			stringBuilder.AppendFormat(" WHERE si.SkuId IN(SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({0}))", DataHelper.CleanSearchString(productIds));
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			DataTable dataTable = null;
			DataTable dataTable2 = null;
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
				dataReader.NextResult();
				dataTable2 = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			dataTable.Columns.Add("SKUContent");
			if (dataTable != null && dataTable.Rows.Count > 0 && dataTable2 != null && dataTable2.Rows.Count > 0)
			{
				foreach (DataRow row in dataTable.Rows)
				{
					string text = string.Empty;
					foreach (DataRow row2 in dataTable2.Rows)
					{
						if ((string)row["SkuId"] == (string)row2["SkuId"])
						{
							text = text + row2["AttributeName"] + "：" + row2["ValueStr"] + "; ";
						}
					}
					row["SKUContent"] = text;
				}
			}
			return dataTable;
		}

		public DataTable GetStoreSkuStocks(string productIds)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("select s.ProductId,p.ProductName,SkuId,Stock,ThumbnailUrl40,s.StoreId,t.StoreName from Hishop_StoreSKUs s left join Hishop_Products p on s.ProductID=p.ProductId\r\n  left join Hishop_Stores t on s.StoreId=t.StoreId\r\n   where p.ProductId IN ({0})", DataHelper.CleanSearchString(productIds));
			stringBuilder.Append(" SELECT SkuId, AttributeName, ValueStr FROM Hishop_SKUItems si JOIN Hishop_Attributes a ON si.AttributeId = a.AttributeId JOIN Hishop_AttributeValues av ON si.ValueId = av.ValueId");
			stringBuilder.AppendFormat(" WHERE si.SkuId IN(SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({0}))", DataHelper.CleanSearchString(productIds));
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			DataTable dataTable = null;
			DataTable dataTable2 = null;
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
				dataReader.NextResult();
				dataTable2 = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			dataTable.Columns.Add("SKUContent");
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				foreach (DataRow row in dataTable.Rows)
				{
					string text = string.Empty;
					if (dataTable2 != null && dataTable2.Rows.Count > 0)
					{
						foreach (DataRow row2 in dataTable2.Rows)
						{
							if ((string)row["SkuId"] == (string)row2["SkuId"])
							{
								text = text + row2["AttributeName"] + "：" + row2["ValueStr"] + "; ";
							}
						}
					}
					if (text == string.Empty)
					{
						row["SKUContent"] = row["ProductName"].ToNullString();
					}
					else
					{
						row["SKUContent"] = text;
					}
				}
			}
			return dataTable;
		}

		public bool HasStoreSkuStocks(string sSKUId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT COUNT(SkuId) FROM Hishop_StoreSKUs WHERE ProductID = '{0}'", sSKUId.ToNullString().Split('_')[0]);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			return (int)base.database.ExecuteScalar(sqlStringCommand) > 0;
		}

		public DataTable HasStoreByProducts(string productIds)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT distinct(ProductID) FROM Hishop_StoreSKUs WHERE ProductID IN ({0})", productIds);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			return base.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}

		public int CanGetGoodsOnStore(string skuId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT COUNT(distinct(SkuId)) FROM Hishop_StoreSKUs WHERE SkuId IN ({0})", skuId);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			return int.Parse(base.database.ExecuteScalar(sqlStringCommand).ToString());
		}

		public bool UpdateSkuStock(string productIds, int stock)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand($"UPDATE Hishop_SKUs SET Stock = {stock} WHERE ProductId IN ({DataHelper.CleanSearchString(productIds)})");
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool AddSkuStock(string productIds, int addStock)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(string.Format("UPDATE Hishop_SKUs SET Stock = CASE WHEN Stock + ({0}) < 0 THEN 0 ELSE Stock + ({0}) END WHERE ProductId IN ({1})", addStock, DataHelper.CleanSearchString(productIds)));
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateSkuStock(Dictionary<string, int> skuStocks)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string key in skuStocks.Keys)
			{
				stringBuilder.AppendFormat(" UPDATE Hishop_SKUs SET Stock = {0} WHERE SkuId = '{1}'", skuStocks[key], DataHelper.CleanSearchString(key));
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateSkuWarningStock(string productIds, int Warningstock)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand($"UPDATE Hishop_SKUs SET WarningStock = {Warningstock} WHERE ProductId IN ({DataHelper.CleanSearchString(productIds)})");
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool AddSkuWarningStock(string productIds, int addWarningStock)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(string.Format("UPDATE Hishop_SKUs SET WarningStock = CASE WHEN WarningStock + ({0}) < 0 THEN 0 ELSE WarningStock + ({0}) END WHERE ProductId IN ({1})", addWarningStock, DataHelper.CleanSearchString(productIds)));
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateSkuWarningStock(Dictionary<string, int> skuWarningStocks)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string key in skuWarningStocks.Keys)
			{
				stringBuilder.AppendFormat(" UPDATE Hishop_SKUs SET WarningStock = {0} WHERE SkuId = '{1}'", skuWarningStocks[key], DataHelper.CleanSearchString(key));
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public DataTable GetSkuMemberPrices(string productIds)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT SkuId, ProductName, SKU, CostPrice, MarketPrice, SalePrice,SupplierId FROM Hishop_Products p JOIN Hishop_SKUs s ON p.ProductId = s.ProductId WHERE p.ProductId IN ({0})", DataHelper.CleanSearchString(productIds));
			stringBuilder.Append(" SELECT SkuId, AttributeName, ValueStr FROM Hishop_SKUItems si JOIN Hishop_Attributes a ON si.AttributeId = a.AttributeId JOIN Hishop_AttributeValues av ON si.ValueId = av.ValueId");
			stringBuilder.AppendFormat(" WHERE si.SkuId IN(SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({0}))", DataHelper.CleanSearchString(productIds));
			stringBuilder.AppendLine(" SELECT CAST(GradeId AS NVARCHAR) + '_' + [Name] AS MemberGradeName,Discount FROM aspnet_MemberGrades");
			stringBuilder.AppendLine(" SELECT SkuId, (SELECT CAST(GradeId AS NVARCHAR) + '_' + [Name] FROM aspnet_MemberGrades WHERE GradeId = sm.GradeId) AS MemberGradeName,MemberSalePrice");
			stringBuilder.AppendFormat(" FROM Hishop_SKUMemberPrice sm WHERE SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({0}))", DataHelper.CleanSearchString(productIds));
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			DataTable dataTable = null;
			DataTable dataTable2 = null;
			DataTable dataTable3 = null;
			DataTable dataTable4 = null;
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
				if (dataTable != null && dataTable.Rows.Count > 0)
				{
					dataTable.Columns.Add("SKUContent");
					dataReader.NextResult();
					dataTable2 = DataHelper.ConverDataReaderToDataTable(dataReader);
					dataReader.NextResult();
					dataTable4 = DataHelper.ConverDataReaderToDataTable(dataReader);
					if (dataTable4 != null && dataTable4.Rows.Count > 0)
					{
						foreach (DataRow row in dataTable4.Rows)
						{
							dataTable.Columns.Add((string)row["MemberGradeName"]);
						}
					}
					dataReader.NextResult();
					dataTable3 = DataHelper.ConverDataReaderToDataTable(dataReader);
				}
			}
			if (dataTable2 != null && dataTable2.Rows.Count > 0)
			{
				foreach (DataRow row2 in dataTable.Rows)
				{
					string text = string.Empty;
					foreach (DataRow row3 in dataTable2.Rows)
					{
						if ((string)row2["SkuId"] == (string)row3["SkuId"])
						{
							text = text + row3["AttributeName"] + "：" + row3["ValueStr"] + "; ";
						}
					}
					row2["SKUContent"] = text;
				}
			}
			if (dataTable3 != null && dataTable3.Rows.Count > 0)
			{
				foreach (DataRow row4 in dataTable.Rows)
				{
					foreach (DataRow row5 in dataTable3.Rows)
					{
						if ((string)row4["SkuId"] == (string)row5["SkuId"])
						{
							row4[(string)row5["MemberGradeName"]] = row5["MemberSalePrice"];
						}
					}
				}
			}
			if (dataTable4 != null && dataTable4.Rows.Count > 0)
			{
				foreach (DataRow row6 in dataTable.Rows)
				{
					decimal d = decimal.Parse(row6["SalePrice"].ToString());
					foreach (DataRow row7 in dataTable4.Rows)
					{
						decimal d2 = decimal.Parse(row7["Discount"].ToString());
						string arg = (d * (d2 / 100m)).F2ToString("f2");
						row6[(string)row7["MemberGradeName"]] = row6[(string)row7["MemberGradeName"]] + "|" + arg;
					}
				}
			}
			return dataTable;
		}

		public bool CheckPrice(string productIds, int baseGradeId, decimal checkPrice, bool isMember)
		{
			StringBuilder stringBuilder = new StringBuilder(" ");
			switch (baseGradeId)
			{
			case -2:
				stringBuilder.AppendFormat("SELECT COUNT(*) FROM Hishop_SKUs WHERE ProductId IN ({0}) AND CostPrice - {1} < 0", productIds, checkPrice);
				break;
			case -3:
				stringBuilder.AppendFormat("SELECT COUNT(*) FROM Hishop_SKUs WHERE ProductId IN ({0}) AND SalePrice - {1} < 0", productIds, checkPrice);
				break;
			default:
				if (isMember)
				{
					stringBuilder.AppendFormat("SELECT COUNT(*) FROM Hishop_SKUMemberPrice WHERE MemberSalePrice - {0} < 0 AND GradeId = {1}", checkPrice, baseGradeId);
					stringBuilder.AppendFormat(" AND SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({0})) ", productIds);
				}
				break;
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			return (int)base.database.ExecuteScalar(sqlStringCommand) > 0;
		}

		public bool UpdateSkuMemberPrices(string productIds, int gradeId, decimal price)
		{
			StringBuilder stringBuilder = new StringBuilder();
			switch (gradeId)
			{
			case -2:
				stringBuilder.AppendFormat("UPDATE Hishop_SKUs SET CostPrice = {0} WHERE ProductId IN ({1})", price, DataHelper.CleanSearchString(productIds));
				break;
			case -3:
				stringBuilder.AppendFormat("UPDATE Hishop_SKUs SET SalePrice = {0} WHERE ProductId IN ({1})", price, DataHelper.CleanSearchString(productIds));
				break;
			default:
				stringBuilder.AppendFormat("DELETE FROM Hishop_SKUMemberPrice WHERE GradeId = {0} AND SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({1}))", gradeId, DataHelper.CleanSearchString(productIds));
				stringBuilder.AppendFormat(" INSERT INTO Hishop_SKUMemberPrice (SkuId,GradeId,MemberSalePrice) SELECT SkuId, {0} AS GradeId, {1} AS MemberSalePrice FROM Hishop_SKUs WHERE ProductId IN ({2})", gradeId, price, DataHelper.CleanSearchString(productIds));
				break;
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateSkuMemberPrices(string productIds, int gradeId, int baseGradeId, string operation, decimal price)
		{
			StringBuilder stringBuilder = new StringBuilder(" ");
			switch (gradeId)
			{
			case -2:
				switch (baseGradeId)
				{
				case -2:
					stringBuilder.AppendFormat("UPDATE Hishop_SKUs SET CostPrice = CostPrice {0} ({1}) WHERE ProductId IN ({2})", operation, price, DataHelper.CleanSearchString(productIds));
					break;
				case -3:
					stringBuilder.AppendFormat("UPDATE Hishop_SKUs SET CostPrice = SalePrice {0} ({1}) WHERE ProductId IN ({2})", operation, price, DataHelper.CleanSearchString(productIds));
					break;
				}
				break;
			case -3:
				switch (baseGradeId)
				{
				case -2:
					stringBuilder.AppendFormat("UPDATE Hishop_SKUs SET SalePrice = CostPrice {0} ({1}) WHERE ProductId IN ({2})", operation, price, DataHelper.CleanSearchString(productIds));
					break;
				case -3:
					stringBuilder.AppendFormat("UPDATE Hishop_SKUs SET SalePrice = SalePrice {0} ({1}) WHERE ProductId IN ({2})", operation, price, DataHelper.CleanSearchString(productIds));
					break;
				}
				break;
			default:
				stringBuilder.AppendFormat("DELETE FROM Hishop_SKUMemberPrice WHERE GradeId = {0} AND SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({1}))", gradeId, DataHelper.CleanSearchString(productIds));
				switch (baseGradeId)
				{
				case -2:
					stringBuilder.AppendFormat(" INSERT INTO Hishop_SKUMemberPrice (SkuId,GradeId,MemberSalePrice) SELECT SkuId, {0} AS GradeId, CostPrice {1} ({2}) AS MemberSalePrice FROM Hishop_SKUs WHERE ProductId IN ({3})", gradeId, operation, price, DataHelper.CleanSearchString(productIds));
					break;
				case -3:
					stringBuilder.AppendFormat(" INSERT INTO Hishop_SKUMemberPrice (SkuId,GradeId,MemberSalePrice) SELECT SkuId, {0} AS GradeId, SalePrice {1} ({2}) AS MemberSalePrice FROM Hishop_SKUs WHERE ProductId IN ({3})", gradeId, operation, price, DataHelper.CleanSearchString(productIds));
					break;
				default:
					stringBuilder.AppendFormat(" INSERT INTO Hishop_SKUMemberPrice (SkuId,GradeId,MemberSalePrice) SELECT SkuId, {0} AS GradeId, MemberSalePrice {1} ({2}) AS MemberSalePrice", gradeId, operation, price);
					stringBuilder.AppendFormat(" FROM Hishop_SKUMemberPrice WHERE GradeId = {0} AND SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({1}))", baseGradeId, DataHelper.CleanSearchString(productIds));
					break;
				}
				break;
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateSkuMemberPrices(DataSet ds)
		{
			StringBuilder stringBuilder = new StringBuilder();
			DataTable dataTable = ds.Tables["skuPriceTable"];
			DataTable dataTable2 = ds.Tables["skuMemberPriceTable"];
			string text = string.Empty;
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				foreach (DataRow row in dataTable.Rows)
				{
					text = text + "'" + row["skuId"] + "',";
					stringBuilder.AppendFormat(" UPDATE Hishop_SKUs SET CostPrice = {0}, SalePrice = {1} WHERE SkuId = '{2}'", row["costPrice"], row["salePrice"], row["skuId"]);
				}
			}
			if (text.Length > 1)
			{
				stringBuilder.AppendFormat(" DELETE FROM Hishop_SKUMemberPrice WHERE SkuId IN ({0}) ", text.Remove(text.Length - 1));
			}
			if (dataTable2 != null && dataTable2.Rows.Count > 0)
			{
				foreach (DataRow row2 in dataTable2.Rows)
				{
					stringBuilder.AppendFormat(" INSERT INTO Hishop_SKUMemberPrice (SkuId, GradeId, MemberSalePrice) VALUES ('{0}', {1}, {2})", row2["skuId"], row2["gradeId"], row2["memberPrice"]);
				}
			}
			if (stringBuilder.Length <= 0)
			{
				return false;
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public int UpdateCrossborder(string productIds, bool crossborderstatus)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand($"UPDATE Hishop_Products SET IsCrossborder={(crossborderstatus ? 1 : 0)} WHERE ProductId IN ({productIds})");
			return base.database.ExecuteNonQuery(sqlStringCommand);
		}
	}
}
