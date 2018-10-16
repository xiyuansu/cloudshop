using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.SqlDal.Members;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Commodities
{
	public class SkuDao : BaseDao
	{
		public SKUItem GetSkuItem(string skuId, int storeId = 0)
		{
			string empty = string.Empty;
			empty = ((storeId <= 0) ? "SELECT *,0 as StoreStock FROM Hishop_SKUs s WHERE SkuId=@SkuId;" : "select k.SalePrice,s.ProductID,s.SkuId,StoreId,s.Stock,s.WarningStock,s.FreezeStock,case when StoreSalePrice=0 then SalePrice else StoreSalePrice end storeSalePrice from  Hishop_StoreSKUs s inner join Hishop_SKUs k on s.SkuId=k.SkuId WHERE StoreId=@StoreId AND s.SkuId=@SkuId;");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(empty);
			base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
			base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, storeId);
			SKUItem result = null;
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToModel<SKUItem>(objReader);
			}
			return result;
		}

		public int GetSkuStock(string skuId, int storeId)
		{
			string empty = string.Empty;
			empty = ((storeId <= 0) ? "SELECT Stock FROM Hishop_SKUs WHERE SkuId = @SkuId;" : "SELECT Stock FROM Hishop_StoreSKUs where SkuId=@SkuId and StoreId = @StoreId;");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(empty);
			base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
			base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, storeId);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0);
		}

		public DataTable GetProductInfoBySku(string skuId)
		{
			DataTable result = null;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(" SELECT s.SkuId, s.SKU, s.ProductId, s.Stock, AttributeName, ValueStr FROM Hishop_SKUs s left join Hishop_SKUItems si on s.SkuId = si.SkuId left join Hishop_Attributes a on si.AttributeId = a.AttributeId left join Hishop_AttributeValues av on si.ValueId = av.ValueId WHERE s.SkuId = @SkuId AND s.ProductId IN (SELECT ProductId FROM Hishop_Products WHERE SaleStatus=1)");
			base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
			using (IDataReader reader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(reader);
			}
			return result;
		}

		public SKUItem GetProductAndSku(int gradeId, int productId, string options)
		{
			if (string.IsNullOrEmpty(options))
			{
				return null;
			}
			string[] array = options.Split(',');
			if (array == null || array.Length == 0)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (gradeId > 0)
			{
				int discount = new MemberGradeDao().Get<MemberGradeInfo>(gradeId).Discount;
				stringBuilder.Append("SELECT SkuId, ProductId, SKU,Weight, Stock,WarningStock, CostPrice,StoreStock=(SELECT ISNULL(MAX(ss.Stock),s.Stock) FROM Hishop_StoreSKUs ss WHERE s.SkuId=ss.SkuID),");
				stringBuilder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0}) = 1", gradeId);
				stringBuilder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Hishop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice", gradeId, discount);
				stringBuilder.Append(" FROM Hishop_SKUs s WHERE ProductId = @ProductId");
			}
			else
			{
				stringBuilder.Append("SELECT SkuId, ProductId, SKU,Weight, Stock,WarningStock, CostPrice,StoreStock=(SELECT ISNULL(MAX(ss.Stock),s.Stock) FROM Hishop_StoreSKUs ss WHERE s.SkuId=ss.SkuID), SalePrice FROM Hishop_SKUs s WHERE ProductId = @ProductId");
			}
			string[] array2 = array;
			foreach (string text in array2)
			{
				string[] array3 = text.Split(':');
				int num = 0;
				int num2 = 0;
				int.TryParse(array3[0], out num);
				if (array3.Length >= 2)
				{
					int.TryParse(array3[1], out num2);
				}
				stringBuilder.AppendFormat(" AND SkuId IN (SELECT SkuId FROM Hishop_SKUItems WHERE AttributeId = {0} AND ValueId = {1}) ", num, num2);
			}
			SKUItem result = null;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToModel<SKUItem>(objReader);
			}
			return result;
		}

		public DataTable GetSkus(int productId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT SkuId, a.AttributeId, AttributeName, UseAttributeImage, av.ValueId, ValueStr,pi.ImageUrl,pi.ThumbnailUrl410 FROM Hishop_SKUItems s join Hishop_Attributes a on s.AttributeId = a.AttributeId join Hishop_AttributeValues av on s.ValueId = av.ValueId  LEFT JOIN (select * from Hishop_ProductSpecificationImages where ProductId=@ProductId) pi ON s.ValueId=pi.ValueId WHERE SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId = @ProductId) ORDER BY a.DisplaySequence DESC,av.DisplaySequence DESC");
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			using (IDataReader reader = base.database.ExecuteReader(sqlStringCommand))
			{
				return DataHelper.ConverDataReaderToDataTable(reader);
			}
		}

		public DataTable GetSkus(int productId, int storeId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT SkuId, a.AttributeId, AttributeName, UseAttributeImage, av.ValueId, ValueStr,pi.ImageUrl,pi.ThumbnailUrl410 FROM Hishop_SKUItems s join Hishop_Attributes a on s.AttributeId = a.AttributeId join Hishop_AttributeValues av on s.ValueId = av.ValueId  LEFT JOIN (select * from Hishop_ProductSpecificationImages where ProductId=@ProductId) pi ON s.ValueId=pi.ValueId WHERE SkuId IN (SELECT SkuId FROM Hishop_StoreSKUs WHERE ProductId = @ProductId and StoreId=@StoreId) ORDER BY a.DisplaySequence DESC,av.DisplaySequence DESC");
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, storeId);
			using (IDataReader reader = base.database.ExecuteReader(sqlStringCommand))
			{
				return DataHelper.ConverDataReaderToDataTable(reader);
			}
		}

		public DataTable GetSkusByProductId(int productId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT SkuId, ProductId, SKU,Weight, Stock, CostPrice,saleprice");
			stringBuilder.Append(" FROM Hishop_SKUs s WHERE ProductId = @ProductId");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			using (IDataReader reader = base.database.ExecuteReader(sqlStringCommand))
			{
				return DataHelper.ConverDataReaderToDataTable(reader);
			}
		}

		public DataTable GetExpandAttributes(int productId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" SELECT a.AttributeId, AttributeName, ValueStr FROM Hishop_ProductAttributes pa JOIN Hishop_Attributes a ON pa.AttributeId = a.AttributeId");
			stringBuilder.Append(" JOIN Hishop_AttributeValues v ON a.AttributeId = v.AttributeId AND pa.ValueId = v.ValueId  WHERE ProductId = @ProductId ORDER BY a.DisplaySequence DESC, v.DisplaySequence DESC");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			DataTable dataTable = default(DataTable);
			using (IDataReader reader = base.database.ExecuteReader(sqlStringCommand))
			{
				dataTable = DataHelper.ConverDataReaderToDataTable(reader);
			}
			DataTable dataTable2 = new DataTable();
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				dataTable2 = dataTable.Clone();
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
			}
			return dataTable2;
		}

		public IList<string> GetSkuIdsBysku(string sku)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT SkuId FROM Hishop_SKUs WHERE SKU = @SKU");
			base.database.AddInParameter(sqlStringCommand, "SKU", DbType.String, sku);
			IList<string> list = new List<string>();
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add((string)((IDataRecord)dataReader)["SkuId"]);
				}
			}
			return list;
		}

		public DataTable GetUnUpUnUpsellingSkus(int productId, int attributeId, int valueId)
		{
			string text = "SELECT A.*,(SELECT Stock FROM Hishop_SKUs WHERE SkuId=A.SKuId) AS Stock FROM Hishop_SKUItems as A  WHERE A.SkuId IN(SELECT SkuId FROM Hishop_SKUs WHERE ProductId = @ProductId AND AttributeId = @AttributeId)";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT A.*,B.Stock FROM Hishop_SKUItems as A left join Hishop_SKUs as B on A.SkuId=B.SkuId  WHERE B.ProductId = @ProductId\r\n        AND (A.SkuId in (SELECT SKUId FROM Hishop_SKUItems WHERE AttributeId = @AttributeId AND ValueId=@ValueId)\r\n\t\t\t\t  OR AttributeId = @AttributeId)");
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			base.database.AddInParameter(sqlStringCommand, "AttributeId", DbType.Int32, attributeId);
			base.database.AddInParameter(sqlStringCommand, "ValueId", DbType.Int32, valueId);
			DataTable result = null;
			using (IDataReader reader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(reader);
			}
			return result;
		}

		public Dictionary<string, decimal> GetCostPriceForItems(string skuIds)
		{
			Dictionary<string, decimal> dictionary = new Dictionary<string, decimal>();
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand($"SELECT s.SkuId, s.CostPrice FROM Hishop_SKUs s WHERE SkuId IN ({skuIds})");
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					decimal value = (((IDataRecord)dataReader)["CostPrice"] == DBNull.Value) ? decimal.Zero : ((decimal)((IDataRecord)dataReader)["CostPrice"]);
					dictionary.Add((string)((IDataRecord)dataReader)["SkuId"], value);
				}
			}
			return dictionary;
		}

		public DataTable GetTheSku(string skuId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * ,(SELECT MarketPrice FROM Hishop_Products WHERE productid = Hishop_SKUs.ProductId) MarketPrice FROM Hishop_SKUs WHERE SkuId = @SkuId");
			base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
			return base.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}
	}
}
