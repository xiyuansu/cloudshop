using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hishop.Open.Api;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Hidistro.SqlDal.Commodities
{
	public class ProductApiDao : BaseDao
	{
		public DbQueryResult GetProductsApiByQuery(ProductQuery query)
		{
			DataSet dataSet = new DataSet();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1 ");
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
				stringBuilder.AppendFormat(" AND UpdateDate >='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
			}
			if (query.EndDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND UpdateDate <='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
			}
			string selectFields = "CategoryId,(SELECT Name FROM Hishop_Categories WHERE CategoryId = p.CategoryId) AS CategoryName,BrandId, (SELECT BrandName FROM Hishop_BrandCategories WHERE BrandId = p.BrandId) AS BrandName,TypeId, (SELECT TypeName FROM Hishop_ProductTypes WHERE TypeId = p.TypeId) AS TypeName,ProductId,ProductName,ProductCode, ImageUrl1,ImageUrl2,ImageUrl3,ImageUrl4,ImageUrl5,AddedDate,UpdateDate,SaleStatus,SaleCounts,Stock,SalePrice,DisplaySequence";
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_BrowseProductList p", "ProductId", stringBuilder.ToString(), selectFields);
		}

		public product_item_model GetProduct(int num_iid)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT CategoryId, (SELECT Name FROM Hishop_Categories WHERE CategoryId = p.CategoryId) AS CategoryName, BrandId, (SELECT BrandName FROM Hishop_BrandCategories WHERE BrandId = p.BrandId) AS BrandName, TypeId, (SELECT TypeName FROM Hishop_ProductTypes WHERE TypeId = p.TypeId) AS TypeName, ProductId, ProductCode, ProductName, ImageUrl1,ImageUrl2, ImageUrl3, ImageUrl4, ImageUrl5, Description, MobbileDescription, AddedDate, UpdateDate, DisplaySequence, SaleStatus,SaleCounts FROM Hishop_Products p WHERE p.ProductId  = @ProductId");
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, num_iid);
			product_item_model product_item_model = null;
			string text = Globals.GetImageServerUrl();
			if (string.IsNullOrWhiteSpace(text))
			{
				text = "http://" + HttpContext.Current.Request.Url.Host;
			}
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					product_item_model = new product_item_model();
					product_item_model.cid = (int)((IDataRecord)dataReader)["CategoryId"];
					if (((IDataRecord)dataReader)["CategoryName"] != DBNull.Value)
					{
						product_item_model.cat_name = (string)((IDataRecord)dataReader)["CategoryName"];
					}
					if (((IDataRecord)dataReader)["BrandId"] != DBNull.Value)
					{
						product_item_model.brand_id = (int)((IDataRecord)dataReader)["BrandId"];
					}
					if (((IDataRecord)dataReader)["BrandName"] != DBNull.Value)
					{
						product_item_model.brand_name = (string)((IDataRecord)dataReader)["BrandName"];
					}
					if (((IDataRecord)dataReader)["TypeId"] != DBNull.Value)
					{
						product_item_model.type_id = (int)((IDataRecord)dataReader)["TypeId"];
					}
					if (((IDataRecord)dataReader)["TypeName"] != DBNull.Value)
					{
						product_item_model.type_name = (string)((IDataRecord)dataReader)["TypeName"];
					}
					product_item_model.num_iid = (int)((IDataRecord)dataReader)["ProductId"];
					if (((IDataRecord)dataReader)["ProductCode"] != DBNull.Value)
					{
						product_item_model.outer_id = (string)((IDataRecord)dataReader)["ProductCode"];
					}
					product_item_model.title = (string)((IDataRecord)dataReader)["ProductName"];
					if (((IDataRecord)dataReader)["ImageUrl1"] != DBNull.Value)
					{
						product_item_model.pic_url.Add(text + (string)((IDataRecord)dataReader)["ImageUrl1"]);
					}
					if (((IDataRecord)dataReader)["ImageUrl2"] != DBNull.Value)
					{
						product_item_model.pic_url.Add(text + (string)((IDataRecord)dataReader)["ImageUrl2"]);
					}
					if (((IDataRecord)dataReader)["ImageUrl3"] != DBNull.Value)
					{
						product_item_model.pic_url.Add(text + (string)((IDataRecord)dataReader)["ImageUrl3"]);
					}
					if (((IDataRecord)dataReader)["ImageUrl4"] != DBNull.Value)
					{
						product_item_model.pic_url.Add(text + (string)((IDataRecord)dataReader)["ImageUrl4"]);
					}
					if (((IDataRecord)dataReader)["ImageUrl5"] != DBNull.Value)
					{
						product_item_model.pic_url.Add(text + (string)((IDataRecord)dataReader)["ImageUrl5"]);
					}
					if (((IDataRecord)dataReader)["Description"] != DBNull.Value)
					{
						product_item_model.desc = Globals.UrlEncode(((string)((IDataRecord)dataReader)["Description"]).Replace("src=\"/Storage/master/gallery", $"src=\"{text}/Storage/master/gallery").Replace('"', '“'));
					}
					if (((IDataRecord)dataReader)["MobbileDescription"] != DBNull.Value)
					{
						product_item_model.wap_desc = Globals.UrlEncode(((string)((IDataRecord)dataReader)["MobbileDescription"]).Replace("src=\"/Storage/master/gallery", $"src=\"{text}/Storage/master/gallery").Replace('"', '“'));
					}
					product_item_model.list_time = (DateTime)((IDataRecord)dataReader)["AddedDate"];
					if (((IDataRecord)dataReader)["UpdateDate"] != DBNull.Value)
					{
						product_item_model.modified = (DateTime)((IDataRecord)dataReader)["UpdateDate"];
					}
					product_item_model.display_sequence = (int)((IDataRecord)dataReader)["DisplaySequence"];
					switch ((ProductSaleStatus)((IDataRecord)dataReader)["SaleStatus"])
					{
					case ProductSaleStatus.OnSale:
						product_item_model.approve_status = "On_Sale";
						break;
					case ProductSaleStatus.UnSale:
						product_item_model.approve_status = "Un_Sale";
						break;
					default:
						product_item_model.approve_status = "In_Stock";
						break;
					}
					product_item_model.sold_quantity = (int)((IDataRecord)dataReader)["SaleCounts"];
				}
			}
			return product_item_model;
		}

		public string GetProps(int num_iid)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT a.AttributeId, AttributeName, ValueStr FROM Hishop_ProductAttributes pa JOIN Hishop_Attributes a ON pa.AttributeId = a.AttributeId JOIN Hishop_AttributeValues v ON a.AttributeId = v.AttributeId AND pa.ValueId = v.ValueId  WHERE ProductId = @ProductId ORDER BY a.DisplaySequence DESC, v.DisplaySequence DESC");
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, num_iid);
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					if (dictionary.Keys.Contains((string)((IDataRecord)dataReader)["AttributeName"]))
					{
						Dictionary<string, string> dictionary2 = dictionary;
						string key = (string)((IDataRecord)dataReader)["AttributeName"];
						dictionary2[key] = dictionary2[key] + "," + (string)((IDataRecord)dataReader)["ValueStr"];
					}
					else
					{
						dictionary.Add((string)((IDataRecord)dataReader)["AttributeName"], (string)((IDataRecord)dataReader)["ValueStr"]);
					}
				}
			}
			string text = string.Empty;
			foreach (string key2 in dictionary.Keys)
			{
				text = text + key2 + ":" + dictionary[key2] + ";";
			}
			return text;
		}

		public IList<product_sku_model> GetSkus(int num_iid)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT s.SkuId, s.SKU, s.ProductId, s.Stock, s.SalePrice, AttributeName, ValueStr FROM Hishop_SKUs s left join Hishop_SKUItems si on s.SkuId = si.SkuId left join Hishop_Attributes a on si.AttributeId = a.AttributeId left join Hishop_AttributeValues av on si.ValueId = av.ValueId WHERE s.SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId = @ProductId)");
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, num_iid);
			IList<product_sku_model> list = new List<product_sku_model>();
			IDataReader reader = base.database.ExecuteReader(sqlStringCommand);
			try
			{
				while (reader.Read())
				{
					product_sku_model product_sku_model = list.FirstOrDefault((product_sku_model item) => item.sku_id == (string)((IDataRecord)reader)["SkuId"]);
					if (product_sku_model == null)
					{
						product_sku_model = new product_sku_model();
						product_sku_model.sku_id = (string)((IDataRecord)reader)["SkuId"];
						if (((IDataRecord)reader)["SKU"] != DBNull.Value)
						{
							product_sku_model.outer_sku_id = (string)((IDataRecord)reader)["SKU"];
						}
						product_sku_model.quantity = (int)((IDataRecord)reader)["Stock"];
						product_sku_model.price = (decimal)((IDataRecord)reader)["SalePrice"];
						if (((IDataRecord)reader)["AttributeName"] != DBNull.Value && ((IDataRecord)reader)["ValueStr"] != DBNull.Value)
						{
							product_sku_model.sku_properties_name = (string)((IDataRecord)reader)["AttributeName"] + ":" + (string)((IDataRecord)reader)["ValueStr"];
						}
						list.Add(product_sku_model);
					}
					else if (((IDataRecord)reader)["AttributeName"] != DBNull.Value && ((IDataRecord)reader)["ValueStr"] != DBNull.Value)
					{
						product_sku_model product_sku_model2 = product_sku_model;
						product_sku_model2.sku_properties_name = product_sku_model2.sku_properties_name + ";" + (string)((IDataRecord)reader)["AttributeName"] + ":" + (string)((IDataRecord)reader)["ValueStr"];
					}
				}
			}
			finally
			{
				if (reader != null)
				{
					reader.Dispose();
				}
			}
			return list;
		}

		public int UpdateProductQuantity(int num_iid, string sku_id, int quantity, int type)
		{
			string str = "UPDATE Hishop_SKUs  SET Stock = ";
			if (type == 2)
			{
				str += " Stock + ";
			}
			str += "@Stock WHERE ProductId = @ProductId";
			if (!string.IsNullOrWhiteSpace(sku_id))
			{
				str += " AND SkuId = @SkuId";
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(str);
			base.database.AddInParameter(sqlStringCommand, "Stock", DbType.Int32, quantity);
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, num_iid);
			base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, sku_id);
			return base.database.ExecuteNonQuery(sqlStringCommand);
		}

		public int UpdateProductApproveStatus(int num_iid, string approve_status)
		{
			ProductSaleStatus productSaleStatus = ProductSaleStatus.OnStock;
			if (approve_status.Equals("On_Sale", StringComparison.OrdinalIgnoreCase))
			{
				productSaleStatus = ProductSaleStatus.OnSale;
				goto IL_0046;
			}
			if (approve_status.Equals("Un_Sale", StringComparison.OrdinalIgnoreCase))
			{
				productSaleStatus = ProductSaleStatus.UnSale;
				goto IL_0046;
			}
			if (approve_status.Equals("In_Stock", StringComparison.OrdinalIgnoreCase))
			{
				productSaleStatus = ProductSaleStatus.OnStock;
				goto IL_0046;
			}
			return 0;
			IL_0046:
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE Hishop_Products SET SaleStatus = @SaleStatus  WHERE ProductId = @ProductId ");
			base.database.AddInParameter(sqlStringCommand, "SaleStatus", DbType.Int32, (int)productSaleStatus);
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, num_iid);
			return base.database.ExecuteNonQuery(sqlStringCommand);
		}
	}
}
