using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Depot;
using Hidistro.Entities.HOP;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.SqlDal.Commodities;
using Hidistro.SqlDal.Depot;
using Hidistro.SqlDal.Members;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Hidistro.SaleSystem.Commodities
{
	public static class ProductHelper
	{
		public static ProductInfo GetProductDetails(int productId, out Dictionary<int, IList<int>> attrs, out IList<int> tagsId)
		{
			ProductDao productDao = new ProductDao();
			attrs = productDao.GetProductAttributes(productId);
			tagsId = productDao.GetProductTags(productId);
			return productDao.GetProductDetails(productId);
		}

		public static bool SaleOffFromStore(int productId)
		{
			try
			{
				ProductDao productDao = new ProductDao();
				productDao.SaleOffFromStore(productId);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public static bool AuditProducts(string productIds, string reson, bool isPass)
		{
			if (!isPass && string.IsNullOrEmpty(reson))
			{
				return false;
			}
			ProductDao productDao = new ProductDao();
			if (isPass && !productDao.VerifyProductForAudit(productIds))
			{
				return false;
			}
			return productDao.AuditProducts(productIds, reson, isPass);
		}

		public static ProductInfo GetProductDetails(int productId, out Dictionary<int, IList<int>> attrs, out IList<int> tagsId, out DataTable skuImgs)
		{
			ProductDao productDao = new ProductDao();
			attrs = productDao.GetProductAttributes(productId);
			tagsId = productDao.GetProductTags(productId);
			skuImgs = new ProductSpecificationImageDao().GetAttrImagesByProductId(productId.ToString());
			return productDao.GetProductDetails(productId);
		}

		public static ProductInfo GetProductById(int productId)
		{
			return new ProductDao().Get<ProductInfo>(productId);
		}

		public static DataTable GetTopProductOrder(int top, string showOrder, int categroyId)
		{
			if (top < 1)
			{
				top = 6;
			}
			if (string.IsNullOrEmpty(showOrder))
			{
				showOrder = " ProductId DESC";
			}
			return new ProductDao().GetTopProductOrder(top, showOrder, categroyId);
		}

		public static DataTable GetTopProductByIds(string ids)
		{
			return new ProductDao().GetTopProductByIds(ids);
		}

		public static PageModel<StoreProductBaseModel> GetStoreNoRelationProducts(ProductQuery query)
		{
			return new ProductDao().GetStoreNoRelationProducts(query);
		}

		public static DbQueryResult GetProducts(ProductQuery query)
		{
			return new ProductDao().GetProducts(query);
		}

		public static IList<ProductInfo> GetAllProducts(ProductQuery query)
		{
			return new ProductDao().GetAllProducts(query);
		}

		public static DbQueryResult GetProducts(SupplierProductQuery query)
		{
			return new ProductDao().GetProducts(query);
		}

		public static int GetProductIsWarningStockNum()
		{
			return new ProductDao().GetProductIsWarningStockNum();
		}

		public static int GetProductIsWarningStockNum(int SupplierId)
		{
			return new ProductDao().GetProductIsWarningStockNum(SupplierId);
		}

		public static DataTable GetGroupBuyProducts(ProductQuery query)
		{
			return new ProductDao().GetGroupBuyProducts(query);
		}

		public static IList<ProductInfo> GetProducts(IList<int> productIds)
		{
			return new ProductDao().GetProducts(productIds);
		}

		public static IList<int> GetProductIds(ProductQuery query)
		{
			return new ProductDao().GetProductIds(query);
		}

		public static ProductActionStatus AddProduct(ProductInfo product, Dictionary<string, SKUItem> skus, Dictionary<int, IList<int>> attrs, IList<int> tagsId, List<ProductSpecificationImageInfo> attrImgs, bool isAPI = false, string inputItemJson = "")
		{
			if (product == null)
			{
				return ProductActionStatus.UnknowError;
			}
			Globals.EntityCoding(product, true);
			int decimalLength = HiContext.Current.SiteSettings.DecimalLength;
			if (product.MarketPrice.HasValue)
			{
				product.MarketPrice = Math.Round(product.MarketPrice.Value, decimalLength);
			}
			ProductActionStatus productActionStatus = ProductActionStatus.UnknowError;
			Database database = DatabaseFactory.CreateDatabase();
			using (DbConnection dbConnection = database.CreateConnection())
			{
				dbConnection.Open();
				DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					ProductDao productDao = new ProductDao();
					product.UpdateDate = DateTime.Now;
					product.DisplaySequence = ProductHelper.GetMaxSequence();
					int num = (int)productDao.Add(product, dbTransaction);
					if (num == 0)
					{
						dbTransaction.Rollback();
						return ProductActionStatus.DuplicateSKU;
					}
					product.ProductId = num;
					if (skus != null && skus.Count > 0 && !productDao.AddProductSKUs(num, skus, dbTransaction))
					{
						dbTransaction.Rollback();
						return ProductActionStatus.SKUError;
					}
					if (attrs != null && attrs.Count > 0 && !productDao.AddProductAttributes(num, attrs, dbTransaction))
					{
						dbTransaction.Rollback();
						return ProductActionStatus.AttributeError;
					}
					if (tagsId != null && tagsId.Count > 0 && !new TagDao().AddProductTags(num, tagsId, dbTransaction))
					{
						dbTransaction.Rollback();
						return ProductActionStatus.ProductTagEroor;
					}
					if (attrImgs != null && attrImgs.Count > 0 && !new ProductSpecificationImageDao().AddAttributeImages(num, attrImgs, dbTransaction))
					{
						dbTransaction.Rollback();
						return ProductActionStatus.ProductAttrImgsError;
					}
					if (!string.IsNullOrEmpty(inputItemJson))
					{
						List<ProductInputItemInfo> list = JsonHelper.ParseFormJson<List<ProductInputItemInfo>>(inputItemJson);
						if (list.Any())
						{
							foreach (ProductInputItemInfo item in list)
							{
								item.ProductId = num;
								productDao.Add(item, dbTransaction);
							}
						}
					}
					dbTransaction.Commit();
					productActionStatus = ProductActionStatus.Success;
				}
				catch (Exception)
				{
					dbTransaction.Rollback();
				}
				finally
				{
					dbConnection.Close();
				}
			}
			if (productActionStatus == ProductActionStatus.Success)
			{
				EventLogs.WriteOperationLog(Privilege.AddProducts, string.Format(CultureInfo.InvariantCulture, "上架了一个新商品:”{0}”", new object[1]
				{
					product.ProductName
				}), isAPI);
			}
			return productActionStatus;
		}

		public static int GetMaxSequence()
		{
			return new ProductDao().GetMaxDisplaySequence<ProductInfo>();
		}

		public static ProductActionStatus UpdateProduct(ProductInfo product, Dictionary<string, SKUItem> skus, Dictionary<int, IList<int>> attrs, IList<int> tagIds, List<ProductSpecificationImageInfo> attrImgs, string inputItemJson = "")
		{
			if (product == null)
			{
				return ProductActionStatus.UnknowError;
			}
			Globals.EntityCoding(product, true);
			int decimalLength = HiContext.Current.SiteSettings.DecimalLength;
			if (product.MarketPrice.HasValue)
			{
				product.MarketPrice = Math.Round(product.MarketPrice.Value, decimalLength);
			}
			ProductActionStatus productActionStatus = ProductActionStatus.UnknowError;
			Database database = DatabaseFactory.CreateDatabase();
			using (DbConnection dbConnection = database.CreateConnection())
			{
				dbConnection.Open();
				DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					ProductDao productDao = new ProductDao();
					if (!productDao.Update(product, dbTransaction))
					{
						dbTransaction.Rollback();
						return ProductActionStatus.DuplicateSKU;
					}
					if (!productDao.DeleteProductSKUS(product.ProductId, dbTransaction))
					{
						dbTransaction.Rollback();
						return ProductActionStatus.SKUError;
					}
					if (skus != null && skus.Count > 0 && !productDao.AddProductSKUs(product.ProductId, skus, dbTransaction))
					{
						dbTransaction.Rollback();
						return ProductActionStatus.SKUError;
					}
					if (!productDao.AddProductAttributes(product.ProductId, attrs, dbTransaction))
					{
						dbTransaction.Rollback();
						return ProductActionStatus.AttributeError;
					}
					TagDao tagDao = new TagDao();
					if (!tagDao.DeleteProductTags(product.ProductId, dbTransaction))
					{
						dbTransaction.Rollback();
						return ProductActionStatus.ProductTagEroor;
					}
					if (tagIds.Count > 0 && !tagDao.AddProductTags(product.ProductId, tagIds, dbTransaction))
					{
						dbTransaction.Rollback();
						return ProductActionStatus.ProductTagEroor;
					}
					ProductSpecificationImageDao productSpecificationImageDao = new ProductSpecificationImageDao();
					if (!productSpecificationImageDao.DeleteProductAttrImages(product.ProductId, dbTransaction))
					{
						dbTransaction.Rollback();
						return ProductActionStatus.ProductAttrImgsError;
					}
					if (attrImgs != null && attrImgs.Count > 0 && !productSpecificationImageDao.AddAttributeImages(product.ProductId, attrImgs, dbTransaction))
					{
						dbTransaction.Rollback();
						return ProductActionStatus.ProductAttrImgsError;
					}
					if (product.ProductType == 1)
					{
						productDao.DeleteInputItem(product.ProductId, dbTransaction);
					}
					if (!string.IsNullOrEmpty(inputItemJson))
					{
						List<ProductInputItemInfo> list = JsonHelper.ParseFormJson<List<ProductInputItemInfo>>(inputItemJson);
						if (list.Any())
						{
							foreach (ProductInputItemInfo item in list)
							{
								item.ProductId = product.ProductId;
								productDao.Add(item, dbTransaction);
							}
						}
					}
					dbTransaction.Commit();
					productActionStatus = ProductActionStatus.Success;
				}
				catch (Exception)
				{
					dbTransaction.Rollback();
				}
				finally
				{
					dbConnection.Close();
				}
			}
			if (productActionStatus == ProductActionStatus.Success)
			{
				ProductHelper.ClearProductCahe(product.ProductId);
				EventLogs.WriteOperationLog(Privilege.EditProducts, string.Format(CultureInfo.InvariantCulture, "修改了编号为 “{0}” 的商品", new object[1]
				{
					product.ProductId
				}), false);
			}
			return productActionStatus;
		}

		public static bool UpdateProductCategory(int productId, int newCategoryId)
		{
			ProductDao productDao = new ProductDao();
			bool flag = (newCategoryId == 0) ? productDao.UpdateProductCategory(productId, newCategoryId, null) : productDao.UpdateProductCategory(productId, newCategoryId, CatalogHelper.GetCategory(newCategoryId).Path + "|");
			if (flag)
			{
				ProductHelper.ClearProductCahe(productId);
				EventLogs.WriteOperationLog(Privilege.EditProducts, string.Format(CultureInfo.InvariantCulture, "修改编号 “{0}” 的店铺分类为 “{1}”", new object[2]
				{
					productId,
					newCategoryId
				}), false);
			}
			return flag;
		}

		public static int DeleteProduct(string productIds, bool isDeleteImage)
		{
			ManagerHelper.CheckPrivilege(Privilege.DeleteProducts);
			if (string.IsNullOrEmpty(productIds))
			{
				return 0;
			}
			string[] array = productIds.Split(',');
			IList<int> list = new List<int>();
			string[] array2 = array;
			foreach (string s in array2)
			{
				list.Add(int.Parse(s));
			}
			ProductDao productDao = new ProductDao();
			IList<ProductInfo> products = productDao.GetProducts(list);
			DataTable attrImagesByProductId = new ProductSpecificationImageDao().GetAttrImagesByProductId(productIds);
			int num = productDao.DeleteProduct(productIds);
			if (num > 0)
			{
				EventLogs.WriteOperationLog(Privilege.DeleteProducts, string.Format(CultureInfo.InvariantCulture, "删除了 “{0}” 件商品", new object[1]
				{
					list.Count
				}), false);
				if (isDeleteImage)
				{
					foreach (ProductInfo item in products)
					{
						try
						{
							ProductHelper.DeleteProductImage(item);
							ProductHelper.DeleteProductSkuImage(attrImagesByProductId);
						}
						catch
						{
						}
					}
				}
				ProductHelper.ClearProductCahe(productIds);
			}
			return num;
		}

		public static int SetFreeShip(string productIds, bool isFree)
		{
			if (string.IsNullOrEmpty(productIds))
			{
				return 0;
			}
			int num = new ProductDao().UpdateProductShipFree(productIds, isFree);
			if (num > 0)
			{
				ProductHelper.ClearProductCahe(productIds);
				EventLogs.WriteOperationLog(Privilege.OffShelfProducts, string.Format(CultureInfo.InvariantCulture, "{0}了“{1}” 件商品包邮", new object[2]
				{
					isFree ? "设置" : "取消",
					num
				}), false);
			}
			return num;
		}

		public static int UpShelf(string productIds)
		{
			ManagerHelper.CheckPrivilege(Privilege.UpShelfProducts);
			if (string.IsNullOrEmpty(productIds))
			{
				return 0;
			}
			int num = new ProductDao().UpdateProductSaleStatusOnSale(productIds);
			if (num > 0)
			{
				ProductHelper.ClearProductCahe(productIds);
				EventLogs.WriteOperationLog(Privilege.UpShelfProducts, string.Format(CultureInfo.InvariantCulture, "批量上架了 “{0}” 件商品", new object[1]
				{
					num
				}), false);
			}
			return num;
		}

		public static int OffShelf(string productIds)
		{
			ManagerHelper.CheckPrivilege(Privilege.OffShelfProducts);
			if (string.IsNullOrEmpty(productIds))
			{
				return 0;
			}
			int num = new ProductDao().UpdateProductSaleStatus(productIds, ProductSaleStatus.UnSale);
			if (num > 0)
			{
				ProductHelper.ClearProductCahe(productIds);
				EventLogs.WriteOperationLog(Privilege.OffShelfProducts, string.Format(CultureInfo.InvariantCulture, "批量下架了 “{0}” 件商品", new object[1]
				{
					num
				}), false);
			}
			return num;
		}

		public static bool ProductsIsAllOnSales(string productSkus, int storeId = 0)
		{
			string text = "";
			string[] array = productSkus.Split(',');
			string[] array2 = array;
			foreach (string text2 in array2)
			{
				text = text + text2.Split('_')[0] + ",";
			}
			text = text.TrimEnd(',');
			text = Globals.GetSafeIDList(text, ',', true);
			if (string.IsNullOrEmpty(text))
			{
				return false;
			}
			if (storeId > 0)
			{
				return new StoresDao().ProductsIsAllOnSales(text, storeId);
			}
			return new ProductDao().ProductsIsAllOnSales(text);
		}

		public static int InStock(string productIds)
		{
			ManagerHelper.CheckPrivilege(Privilege.InStockProduct);
			if (string.IsNullOrEmpty(productIds))
			{
				return 0;
			}
			int num = new ProductDao().UpdateProductSaleStatus(productIds, ProductSaleStatus.OnStock);
			if (num > 0)
			{
				ProductHelper.ClearProductCahe(productIds);
				EventLogs.WriteOperationLog(Privilege.OffShelfProducts, string.Format(CultureInfo.InvariantCulture, "批量入库了 “{0}” 件商品", new object[1]
				{
					num
				}), false);
			}
			return num;
		}

		public static int RemoveProduct(string productIds)
		{
			ManagerHelper.CheckPrivilege(Privilege.DeleteProducts);
			if (string.IsNullOrEmpty(productIds))
			{
				return 0;
			}
			int num = new ProductDao().UpdateProductSaleStatus(productIds, ProductSaleStatus.Delete);
			if (num > 0)
			{
				ProductHelper.ClearProductCahe(productIds);
				EventLogs.WriteOperationLog(Privilege.OffShelfProducts, string.Format(CultureInfo.InvariantCulture, "批量删除了 “{0}” 件商品到回收站", new object[1]
				{
					num
				}), false);
			}
			return num;
		}

		public static DataTable GetProductBaseInfo(string productIds)
		{
			return new ProductBatchDao().GetProductBaseInfo(productIds);
		}

		public static DataTable GetProductSalepriceInfo(string productIds)
		{
			return new ProductBatchDao().GetProductSalepriceInfo(productIds);
		}

		public static bool UpdateProductNames(string productIds, string prefix, string suffix)
		{
			bool flag = new ProductBatchDao().UpdateProductNames(productIds, prefix, suffix);
			if (flag)
			{
				ProductHelper.ClearProductCahe(productIds);
			}
			return flag;
		}

		public static bool ReplaceProductNames(string productIds, string oldWord, string newWord)
		{
			bool flag = new ProductBatchDao().ReplaceProductNames(productIds, oldWord, newWord);
			if (flag)
			{
				ProductHelper.ClearProductCahe(productIds);
			}
			return flag;
		}

		public static bool UpdateProductBaseInfo(string productIds, DataTable dt)
		{
			if (dt == null || dt.Rows.Count <= 0)
			{
				return false;
			}
			bool flag = new ProductBatchDao().UpdateProductBaseInfo(dt);
			if (flag)
			{
				ProductHelper.ClearProductCahe(productIds);
			}
			return flag;
		}

		public static bool UpdateProductReferralDeduct(string productIds, decimal subMemberDeduct, decimal secondLevelDeduct, decimal threeLevelDeduct)
		{
			bool flag = new ProductBatchDao().UpdateProductReferralDeduct(productIds, subMemberDeduct, secondLevelDeduct, threeLevelDeduct);
			if (flag)
			{
				ProductHelper.ClearProductCahe(productIds);
			}
			return flag;
		}

		public static bool UpdateShowSaleCounts(string productIds, int showSaleCounts)
		{
			bool flag = new ProductBatchDao().UpdateShowSaleCounts(productIds, showSaleCounts);
			if (flag)
			{
				ProductHelper.ClearProductCahe(productIds);
			}
			return flag;
		}

		public static bool UpdateShowSaleCounts(string productIds, int showSaleCounts, string operation)
		{
			bool flag = new ProductBatchDao().UpdateShowSaleCounts(productIds, showSaleCounts, operation);
			if (flag)
			{
				ProductHelper.ClearProductCahe(productIds);
			}
			return flag;
		}

		public static bool UpdateShowSaleCounts(string productIds, DataTable dt)
		{
			if (dt == null || dt.Rows.Count <= 0)
			{
				return false;
			}
			bool flag = new ProductBatchDao().UpdateShowSaleCounts(dt);
			if (flag)
			{
				ProductHelper.ClearProductCahe(productIds);
			}
			return flag;
		}

		public static DataTable GetSkuStocks(string productIds)
		{
			return new ProductBatchDao().GetSkuStocks(productIds);
		}

		public static DataTable GetStoreSkuStocks(string productIds, int storeId)
		{
			DataTable skuStocks = ProductHelper.GetSkuStocks(productIds);
			IList<StoreSKUInfo> list = null;
			list = new StoreStockDao().GetStoreStockInfosByProductIds(storeId, productIds);
			if (!skuStocks.Columns.Contains("StoreSalePrice"))
			{
				skuStocks.Columns.Add("StoreSalePrice");
			}
			foreach (DataRow row in skuStocks.Rows)
			{
				StoreSKUInfo storeSKUInfo = list.FirstOrDefault((StoreSKUInfo a) => a.ProductID == row.Field<int>("ProductId") && a.SkuId.Equals(row.Field<string>("SkuId")));
				if (storeSKUInfo == null)
				{
					row.SetField("Stock", 0);
					row.SetField("WarningStock", 0);
					row.SetField("StoreSalePrice", decimal.Zero);
				}
				else
				{
					row.SetField("Stock", storeSKUInfo.Stock);
					row.SetField("WarningStock", storeSKUInfo.WarningStock);
					row.SetField("StoreSalePrice", storeSKUInfo.StoreSalePrice);
				}
			}
			return skuStocks;
		}

		public static DataTable GetAllStoreSkuStocks(string productIds)
		{
			return new ProductBatchDao().GetStoreSkuStocks(productIds);
		}

		public static bool UpdateSkuStock(string productIds, int stock)
		{
			bool flag = new ProductBatchDao().UpdateSkuStock(productIds, stock);
			if (flag)
			{
				ProductHelper.ClearProductCahe(productIds);
			}
			return flag;
		}

		public static bool AddSkuStock(string productIds, int addStock)
		{
			bool flag = new ProductBatchDao().AddSkuStock(productIds, addStock);
			if (flag)
			{
				ProductHelper.ClearProductCahe(productIds);
			}
			return flag;
		}

		public static bool UpdateSkuStock(string productIds, Dictionary<string, int> skuStocks)
		{
			bool flag = new ProductBatchDao().UpdateSkuStock(skuStocks);
			if (flag)
			{
				ProductHelper.ClearProductCahe(productIds);
			}
			return flag;
		}

		public static bool UpdateSkuWarningStock(string productIds, int warningStock)
		{
			bool flag = new ProductBatchDao().UpdateSkuWarningStock(productIds, warningStock);
			if (flag)
			{
				ProductHelper.ClearProductCahe(productIds);
			}
			return flag;
		}

		public static bool AddSkuWarningStock(string productIds, int addWarningStock)
		{
			bool flag = new ProductBatchDao().AddSkuWarningStock(productIds, addWarningStock);
			if (flag)
			{
				ProductHelper.ClearProductCahe(productIds);
			}
			return flag;
		}

		public static bool UpdateSkuWarningStock(string productIds, Dictionary<string, int> skuWarningStocks)
		{
			bool flag = new ProductBatchDao().UpdateSkuWarningStock(skuWarningStocks);
			if (flag)
			{
				ProductHelper.ClearProductCahe(productIds);
			}
			return flag;
		}

		public static DataTable GetSkuMemberPrices(string productIds)
		{
			return new ProductBatchDao().GetSkuMemberPrices(productIds);
		}

		public static bool CheckPrice(string productIds, int baseGradeId, decimal checkPrice, bool isMember)
		{
			return new ProductBatchDao().CheckPrice(productIds, baseGradeId, checkPrice, isMember);
		}

		public static bool UpdateSkuMemberPrices(string productIds, int gradeId, decimal price)
		{
			bool flag = new ProductBatchDao().UpdateSkuMemberPrices(productIds, gradeId, price);
			if (flag)
			{
				ProductHelper.ClearProductCahe(productIds);
			}
			return flag;
		}

		public static bool UpdateSkuMemberPrices(string productIds, int gradeId, int baseGradeId, string operation, decimal price)
		{
			bool flag = new ProductBatchDao().UpdateSkuMemberPrices(productIds, gradeId, baseGradeId, operation, price);
			if (flag)
			{
				ProductHelper.ClearProductCahe(productIds);
			}
			return flag;
		}

		public static bool UpdateSkuMemberPrices(string productIds, DataSet ds)
		{
			bool flag = new ProductBatchDao().UpdateSkuMemberPrices(ds);
			if (flag)
			{
				ProductHelper.ClearProductCahe(productIds);
			}
			return flag;
		}

		public static int UpdateCrossborder(string productIds, bool crossborderstatus)
		{
			return new ProductBatchDao().UpdateCrossborder(productIds, crossborderstatus);
		}

		public static DbQueryResult GetRelatedProducts(Pagination page, int productId)
		{
			return new ProductDao().GetRelatedProducts(page, productId);
		}

		public static IEnumerable<int> GetRelatedProductsId(int productId)
		{
			return new ProductDao().GetRelatedProductsId(productId);
		}

		private static void DeleteShareImage(ShareProductInfo shareinfo)
		{
			if (shareinfo != null && !string.IsNullOrEmpty(shareinfo.ShareUrl))
			{
				ResourcesHelper.DeleteImage(shareinfo.ShareUrl);
			}
		}

		public static bool AddRelatedProduct(int productId, int relatedProductId)
		{
			bool flag = new ProductDao().AddRelatedProduct(productId, relatedProductId);
			if (flag)
			{
				ProductHelper.ClearProductCahe(productId);
			}
			return flag;
		}

		public static bool RemoveRelatedProduct(int productId, int relatedProductId)
		{
			bool flag = new ProductDao().RemoveRelatedProduct(productId, relatedProductId);
			if (flag)
			{
				ProductHelper.ClearProductCahe(productId);
			}
			return flag;
		}

		public static bool ClearRelatedProducts(int productId)
		{
			bool flag = new ProductDao().ClearRelatedProducts(productId);
			if (flag)
			{
				ProductHelper.ClearProductCahe(productId);
			}
			return flag;
		}

		public static DataSet GetTaobaoProductDetails(int productId)
		{
			return new TaobaoProductDao().GetTaobaoProductDetails(productId);
		}

		public static bool UpdateToaobProduct(TaobaoProductInfo taobaoProduct)
		{
			TaobaoProductDao taobaoProductDao = new TaobaoProductDao();
			taobaoProductDao.Delete<TaobaoProductInfo>(taobaoProduct.ProductId);
			return taobaoProductDao.Add(taobaoProduct, null) > 0;
		}

		public static bool IsExitTaobaoProduct(long taobaoProductId)
		{
			return new TaobaoProductDao().IsExitTaobaoProduct(taobaoProductId);
		}

		public static bool IsExistsProductCode(string productCode, int productId = 0)
		{
			return new ProductDao().IsExistsProductCode(productCode, productId);
		}

		public static string UploadDefaltProductImage(HttpPostedFile postedFile)
		{
			if (!ResourcesHelper.CheckPostedFile(postedFile, "image", null))
			{
				return string.Empty;
			}
			string text = HiContext.Current.GetStoragePath() + ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName));
			postedFile.SaveAs(HiContext.Current.Context.Request.MapPath(text));
			return text;
		}

		private static void DeleteProductImage(ProductInfo product)
		{
			if (product != null)
			{
				if (!string.IsNullOrEmpty(product.ImageUrl1))
				{
					ResourcesHelper.DeleteImage(product.ImageUrl1);
					ResourcesHelper.DeleteImage(product.ImageUrl1.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs40/40_"));
					ResourcesHelper.DeleteImage(product.ImageUrl1.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs60/60_"));
					ResourcesHelper.DeleteImage(product.ImageUrl1.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs100/100_"));
					ResourcesHelper.DeleteImage(product.ImageUrl1.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs160/160_"));
					ResourcesHelper.DeleteImage(product.ImageUrl1.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs180/180_"));
					ResourcesHelper.DeleteImage(product.ImageUrl1.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs220/220_"));
					ResourcesHelper.DeleteImage(product.ImageUrl1.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs310/310_"));
					ResourcesHelper.DeleteImage(product.ImageUrl1.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs410/410_"));
				}
				if (!string.IsNullOrEmpty(product.ImageUrl2))
				{
					ResourcesHelper.DeleteImage(product.ImageUrl2);
					ResourcesHelper.DeleteImage(product.ImageUrl2.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs40/40_"));
					ResourcesHelper.DeleteImage(product.ImageUrl2.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs60/60_"));
					ResourcesHelper.DeleteImage(product.ImageUrl2.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs100/100_"));
					ResourcesHelper.DeleteImage(product.ImageUrl2.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs160/160_"));
					ResourcesHelper.DeleteImage(product.ImageUrl2.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs180/180_"));
					ResourcesHelper.DeleteImage(product.ImageUrl2.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs220/220_"));
					ResourcesHelper.DeleteImage(product.ImageUrl2.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs310/310_"));
					ResourcesHelper.DeleteImage(product.ImageUrl2.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs410/410_"));
				}
				if (!string.IsNullOrEmpty(product.ImageUrl3))
				{
					ResourcesHelper.DeleteImage(product.ImageUrl3);
					ResourcesHelper.DeleteImage(product.ImageUrl3.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs40/40_"));
					ResourcesHelper.DeleteImage(product.ImageUrl3.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs60/60_"));
					ResourcesHelper.DeleteImage(product.ImageUrl3.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs100/100_"));
					ResourcesHelper.DeleteImage(product.ImageUrl3.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs160/160_"));
					ResourcesHelper.DeleteImage(product.ImageUrl3.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs180/180_"));
					ResourcesHelper.DeleteImage(product.ImageUrl3.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs220/220_"));
					ResourcesHelper.DeleteImage(product.ImageUrl3.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs310/310_"));
					ResourcesHelper.DeleteImage(product.ImageUrl3.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs410/410_"));
				}
				if (!string.IsNullOrEmpty(product.ImageUrl4))
				{
					ResourcesHelper.DeleteImage(product.ImageUrl4);
					ResourcesHelper.DeleteImage(product.ImageUrl4.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs40/40_"));
					ResourcesHelper.DeleteImage(product.ImageUrl4.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs60/60_"));
					ResourcesHelper.DeleteImage(product.ImageUrl4.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs100/100_"));
					ResourcesHelper.DeleteImage(product.ImageUrl4.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs160/160_"));
					ResourcesHelper.DeleteImage(product.ImageUrl4.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs180/180_"));
					ResourcesHelper.DeleteImage(product.ImageUrl4.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs220/220_"));
					ResourcesHelper.DeleteImage(product.ImageUrl4.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs310/310_"));
					ResourcesHelper.DeleteImage(product.ImageUrl4.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs410/410_"));
				}
				if (!string.IsNullOrEmpty(product.ImageUrl5))
				{
					ResourcesHelper.DeleteImage(product.ImageUrl5);
					ResourcesHelper.DeleteImage(product.ImageUrl5.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs40/40_"));
					ResourcesHelper.DeleteImage(product.ImageUrl5.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs60/60_"));
					ResourcesHelper.DeleteImage(product.ImageUrl5.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs100/100_"));
					ResourcesHelper.DeleteImage(product.ImageUrl5.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs160/160_"));
					ResourcesHelper.DeleteImage(product.ImageUrl5.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs180/180_"));
					ResourcesHelper.DeleteImage(product.ImageUrl5.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs220/220_"));
					ResourcesHelper.DeleteImage(product.ImageUrl5.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs310/310_"));
					ResourcesHelper.DeleteImage(product.ImageUrl5.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs410/410_"));
				}
			}
		}

		private static void DeleteProductSkuImage(DataTable dtProductAttrImage)
		{
			if (dtProductAttrImage != null)
			{
				for (int i = 0; i < dtProductAttrImage.Rows.Count; i++)
				{
					string text = dtProductAttrImage.Rows[i]["ImageUrl"].ToString();
					string text2 = dtProductAttrImage.Rows[i]["ThumbnailUrl40"].ToString();
					string text3 = dtProductAttrImage.Rows[i]["ThumbnailUrl410"].ToString();
					if (!string.IsNullOrEmpty(text))
					{
						ResourcesHelper.DeleteImage(text);
					}
					if (!string.IsNullOrEmpty(text2))
					{
						ResourcesHelper.DeleteImage(text2);
					}
					if (!string.IsNullOrEmpty(text3))
					{
						ResourcesHelper.DeleteImage(text3);
					}
				}
			}
		}

		public static DbQueryResult GetExportProducts(AdvancedProductQuery query, string removeProductIds)
		{
			return new ProductDao().GetExportProducts(query, removeProductIds);
		}

		public static DataSet GetExportProducts(AdvancedProductQuery query, bool includeCostPrice, bool includeStock, string removeProductIds)
		{
			DataSet exportProducts = new ProductDao().GetExportProducts(query, includeCostPrice, includeStock, removeProductIds);
			exportProducts.Tables[0].TableName = "types";
			exportProducts.Tables[1].TableName = "attributes";
			exportProducts.Tables[2].TableName = "values";
			exportProducts.Tables[3].TableName = "products";
			exportProducts.Tables[4].TableName = "skus";
			exportProducts.Tables[5].TableName = "skuItems";
			exportProducts.Tables[6].TableName = "productAttributes";
			exportProducts.Tables[7].TableName = "taobaosku";
			return exportProducts;
		}

		public static void EnsureMapping(DataSet mappingSet)
		{
			new ProductDao().EnsureMapping(mappingSet);
		}

		public static void ImportProducts(DataTable productData, int categoryId, int? brandId, ProductSaleStatus saleStatus, bool isImportFromTaobao)
		{
			if (productData != null && productData.Rows.Count > 0)
			{
				foreach (DataRow row in productData.Rows)
				{
					ProductInfo productInfo = new ProductInfo();
					productInfo.CategoryId = categoryId;
					productInfo.MainCategoryPath = CatalogHelper.GetCategory(categoryId).Path + "|";
					productInfo.ProductName = ((string)row["ProductName"]).Replace("\\", "");
					productInfo.ProductCode = (string)row["SKU"];
					productInfo.BrandId = brandId;
					productInfo.AuditStatus = ProductAuditStatus.Pass;
					if (row["Description"] != DBNull.Value)
					{
						productInfo.Description = (string)row["Description"];
					}
					productInfo.AddedDate = DateTime.Now;
					productInfo.SaleStatus = saleStatus;
					productInfo.HasSKU = false;
					HttpContext current = HttpContext.Current;
					if (row["ImageUrl1"] != DBNull.Value)
					{
						productInfo.ImageUrl1 = (string)row["ImageUrl1"];
					}
					if (!string.IsNullOrEmpty(productInfo.ImageUrl1) && productInfo.ImageUrl1.Length > 0)
					{
						string[] array = ProductHelper.ProcessImages(current, productInfo.ImageUrl1);
						productInfo.ThumbnailUrl40 = array[0];
						productInfo.ThumbnailUrl60 = array[1];
						productInfo.ThumbnailUrl100 = array[2];
						productInfo.ThumbnailUrl160 = array[3];
						productInfo.ThumbnailUrl180 = array[4];
						productInfo.ThumbnailUrl220 = array[5];
						productInfo.ThumbnailUrl310 = array[6];
						productInfo.ThumbnailUrl410 = array[7];
					}
					if (row["ImageUrl2"] != DBNull.Value)
					{
						productInfo.ImageUrl2 = (string)row["ImageUrl2"];
					}
					if (!string.IsNullOrEmpty(productInfo.ImageUrl2) && productInfo.ImageUrl2.Length > 0)
					{
						string[] array2 = ProductHelper.ProcessImages(current, productInfo.ImageUrl2);
					}
					if (row["ImageUrl3"] != DBNull.Value)
					{
						productInfo.ImageUrl3 = (string)row["ImageUrl3"];
					}
					if (!string.IsNullOrEmpty(productInfo.ImageUrl3) && productInfo.ImageUrl3.Length > 0)
					{
						string[] array3 = ProductHelper.ProcessImages(current, productInfo.ImageUrl3);
					}
					if (row["ImageUrl4"] != DBNull.Value)
					{
						productInfo.ImageUrl4 = (string)row["ImageUrl4"];
					}
					if (!string.IsNullOrEmpty(productInfo.ImageUrl4) && productInfo.ImageUrl4.Length > 0)
					{
						string[] array4 = ProductHelper.ProcessImages(current, productInfo.ImageUrl4);
					}
					if (row["ImageUrl5"] != DBNull.Value)
					{
						productInfo.ImageUrl5 = (string)row["ImageUrl5"];
					}
					if (!string.IsNullOrEmpty(productInfo.ImageUrl5) && productInfo.ImageUrl5.Length > 0)
					{
						string[] array5 = ProductHelper.ProcessImages(current, productInfo.ImageUrl5);
					}
					SKUItem sKUItem = new SKUItem();
					sKUItem.SkuId = "0";
					sKUItem.SKU = (string)row["SKU"];
					sKUItem.SalePrice = (decimal)row["SalePrice"];
					if (row["Stock"] != DBNull.Value)
					{
						sKUItem.Stock = (int)row["Stock"];
					}
					if (row["Weight"] != DBNull.Value)
					{
						sKUItem.Weight = (decimal)row["Weight"];
					}
					Dictionary<string, SKUItem> dictionary = new Dictionary<string, SKUItem>();
					dictionary.Add(sKUItem.SkuId, sKUItem);
					ProductActionStatus productActionStatus = ProductHelper.AddProduct(productInfo, dictionary, null, null, null, false, "");
					if (isImportFromTaobao && productActionStatus == ProductActionStatus.Success)
					{
						TaobaoProductInfo taobaoProductInfo = new TaobaoProductInfo();
						taobaoProductInfo.ProductId = productInfo.ProductId;
						taobaoProductInfo.ProTitle = productInfo.ProductName;
						taobaoProductInfo.Cid = (long)row["Cid"];
						if (row["StuffStatus"] != DBNull.Value)
						{
							taobaoProductInfo.StuffStatus = (string)row["StuffStatus"];
						}
						taobaoProductInfo.Num = (long)row["Num"];
						taobaoProductInfo.LocationState = (string)row["LocationState"];
						taobaoProductInfo.LocationCity = (string)row["LocationCity"];
						taobaoProductInfo.FreightPayer = (string)row["FreightPayer"];
						if (row["PostFee"] != DBNull.Value)
						{
							taobaoProductInfo.PostFee = (decimal)row["PostFee"];
						}
						if (row["ExpressFee"] != DBNull.Value)
						{
							taobaoProductInfo.ExpressFee = (decimal)row["ExpressFee"];
						}
						if (row["EMSFee"] != DBNull.Value)
						{
							taobaoProductInfo.EMSFee = (decimal)row["EMSFee"];
						}
						taobaoProductInfo.HasInvoice = (bool)row["HasInvoice"];
						taobaoProductInfo.HasWarranty = (bool)row["HasWarranty"];
						taobaoProductInfo.HasDiscount = (bool)row["HasDiscount"];
						taobaoProductInfo.ValidThru = (long)row["ValidThru"];
						if (row["ListTime"] != DBNull.Value)
						{
							taobaoProductInfo.ListTime = (DateTime)row["ListTime"];
						}
						else
						{
							taobaoProductInfo.ListTime = DateTime.Now;
						}
						if (row["PropertyAlias"] != DBNull.Value)
						{
							taobaoProductInfo.PropertyAlias = (string)row["PropertyAlias"];
						}
						if (row["InputPids"] != DBNull.Value)
						{
							taobaoProductInfo.InputPids = (string)row["InputPids"];
						}
						if (row["InputStr"] != DBNull.Value)
						{
							taobaoProductInfo.InputStr = (string)row["InputStr"];
						}
						if (row["SkuProperties"] != DBNull.Value)
						{
							taobaoProductInfo.SkuProperties = (string)row["SkuProperties"];
						}
						if (row["SkuQuantities"] != DBNull.Value)
						{
							taobaoProductInfo.SkuQuantities = (string)row["SkuQuantities"];
						}
						if (row["SkuPrices"] != DBNull.Value)
						{
							taobaoProductInfo.SkuPrices = (string)row["SkuPrices"];
						}
						if (row["SkuOuterIds"] != DBNull.Value)
						{
							taobaoProductInfo.SkuOuterIds = (string)row["SkuOuterIds"];
						}
						ProductHelper.UpdateToaobProduct(taobaoProductInfo);
					}
				}
			}
		}

		public static void ImportProducts(DataSet productData, int categoryId, int? bandId, ProductSaleStatus saleStatus, bool includeCostPrice, bool includeStock, bool includeImages)
		{
			foreach (DataRow row in productData.Tables["products"].Rows)
			{
				int mappedProductId = (int)row["ProductId"];
				ProductInfo product = ProductHelper.ConverToProduct(row, categoryId, bandId, saleStatus, includeImages);
				Dictionary<string, SKUItem> skus = ProductHelper.ConverToSkus(mappedProductId, productData, includeCostPrice, includeStock);
				Dictionary<int, IList<int>> attrs = ProductHelper.ConvertToAttributes(mappedProductId, productData);
				ProductActionStatus productActionStatus = ProductHelper.AddProduct(product, skus, attrs, null, null, false, "");
			}
		}

		private static Dictionary<int, IList<int>> ConvertToAttributes(int mappedProductId, DataSet productData)
		{
			DataRow[] array = productData.Tables["attributes"].Select("ProductId=" + mappedProductId.ToString(CultureInfo.InvariantCulture));
			if (array.Length == 0)
			{
				return null;
			}
			Dictionary<int, IList<int>> dictionary = new Dictionary<int, IList<int>>();
			DataRow[] array2 = array;
			foreach (DataRow dataRow in array2)
			{
				int key = (int)dataRow["SelectedAttributeId"];
				if (!dictionary.ContainsKey(key))
				{
					IList<int> value = new List<int>();
					dictionary.Add(key, value);
				}
				dictionary[key].Add((int)dataRow["SelectedValueId"]);
			}
			return dictionary;
		}

		private static Dictionary<string, SKUItem> ConverToSkus(int mappedProductId, DataSet productData, bool includeCostPrice, bool includeStock)
		{
			DataRow[] array = productData.Tables["skus"].Select("ProductId=" + mappedProductId.ToString(CultureInfo.InvariantCulture));
			if (array.Length == 0)
			{
				return null;
			}
			Dictionary<string, SKUItem> dictionary = new Dictionary<string, SKUItem>();
			DataRow[] array2 = array;
			foreach (DataRow dataRow in array2)
			{
				string text = (string)dataRow["NewSkuId"];
				SKUItem sKUItem = new SKUItem
				{
					SkuId = text,
					SKU = (string)dataRow["SKU"],
					SalePrice = (decimal)dataRow["SalePrice"]
				};
				if (dataRow["Weight"] != DBNull.Value)
				{
					sKUItem.Weight = (decimal)dataRow["Weight"];
				}
				if (includeCostPrice && dataRow["CostPrice"] != DBNull.Value)
				{
					sKUItem.CostPrice = (decimal)dataRow["CostPrice"];
				}
				if (includeStock)
				{
					sKUItem.Stock = (int)dataRow["Stock"];
				}
				DataRow[] array3 = productData.Tables["skuItems"].Select("NewSkuId='" + text + "' AND MappedProductId=" + mappedProductId.ToString(CultureInfo.InvariantCulture));
				DataRow[] array4 = array3;
				foreach (DataRow dataRow2 in array4)
				{
					sKUItem.SkuItems.Add((int)dataRow2["SelectedAttributeId"], (int)dataRow2["SelectedValueId"]);
				}
				dictionary.Add(text, sKUItem);
			}
			return dictionary;
		}

		private static ProductInfo ConverToProduct(DataRow productRow, int categoryId, int? bandId, ProductSaleStatus saleStatus, bool includeImages)
		{
			ProductInfo productInfo = new ProductInfo
			{
				CategoryId = categoryId,
				TypeId = (int)productRow["SelectedTypeId"],
				ProductName = (string)productRow["ProductName"],
				ProductCode = (string)productRow["ProductCode"],
				BrandId = bandId,
				Unit = (string)productRow["Unit"],
				ShortDescription = (string)productRow["ShortDescription"],
				Description = (string)productRow["Description"],
				Title = (string)productRow["Title"],
				Meta_Description = (string)productRow["Meta_Description"],
				Meta_Keywords = (string)productRow["Meta_Keywords"],
				AddedDate = DateTime.Now,
				SaleStatus = saleStatus,
				HasSKU = (bool)productRow["HasSKU"],
				MainCategoryPath = CatalogHelper.GetCategory(categoryId).Path + "|",
				ImageUrl1 = (string)productRow["ImageUrl1"],
				ImageUrl2 = (string)productRow["ImageUrl2"],
				ImageUrl3 = (string)productRow["ImageUrl3"],
				ImageUrl4 = (string)productRow["ImageUrl4"],
				ImageUrl5 = (string)productRow["ImageUrl5"]
			};
			if (productRow["MarketPrice"] != DBNull.Value)
			{
				productInfo.MarketPrice = (decimal)productRow["MarketPrice"];
			}
			if (includeImages)
			{
				HttpContext current = HttpContext.Current;
				if (!string.IsNullOrEmpty(productInfo.ImageUrl1) && productInfo.ImageUrl1.Length > 0)
				{
					string[] array = ProductHelper.ProcessImages(current, productInfo.ImageUrl1);
					productInfo.ThumbnailUrl40 = array[0];
					productInfo.ThumbnailUrl60 = array[1];
					productInfo.ThumbnailUrl100 = array[2];
					productInfo.ThumbnailUrl160 = array[3];
					productInfo.ThumbnailUrl180 = array[4];
					productInfo.ThumbnailUrl220 = array[5];
					productInfo.ThumbnailUrl310 = array[6];
					productInfo.ThumbnailUrl410 = array[7];
				}
				if (!string.IsNullOrEmpty(productInfo.ImageUrl2) && productInfo.ImageUrl2.Length > 0)
				{
					string[] array2 = ProductHelper.ProcessImages(current, productInfo.ImageUrl2);
				}
				if (!string.IsNullOrEmpty(productInfo.ImageUrl3) && productInfo.ImageUrl3.Length > 0)
				{
					string[] array3 = ProductHelper.ProcessImages(current, productInfo.ImageUrl3);
				}
				if (!string.IsNullOrEmpty(productInfo.ImageUrl4) && productInfo.ImageUrl4.Length > 0)
				{
					string[] array4 = ProductHelper.ProcessImages(current, productInfo.ImageUrl4);
				}
				if (!string.IsNullOrEmpty(productInfo.ImageUrl5) && productInfo.ImageUrl5.Length > 0)
				{
					string[] array5 = ProductHelper.ProcessImages(current, productInfo.ImageUrl5);
				}
			}
			return productInfo;
		}

		private static string[] ProcessImages(HttpContext context, string originalSavePath)
		{
			string fileName = Path.GetFileName(originalSavePath);
			string text = "/Storage/master/product/thumbs40/40_" + fileName;
			string text2 = "/Storage/master/product/thumbs60/60_" + fileName;
			string text3 = "/Storage/master/product/thumbs100/100_" + fileName;
			string text4 = "/Storage/master/product/thumbs160/160_" + fileName;
			string text5 = "/Storage/master/product/thumbs180/180_" + fileName;
			string text6 = "/Storage/master/product/thumbs220/220_" + fileName;
			string text7 = "/Storage/master/product/thumbs310/310_" + fileName;
			string text8 = "/Storage/master/product/thumbs410/410_" + fileName;
			string text9 = context.Request.MapPath(originalSavePath);
			if (File.Exists(text9))
			{
				try
				{
					ResourcesHelper.CreateThumbnail(text9, context.Request.MapPath(text), 40, 40);
					ResourcesHelper.CreateThumbnail(text9, context.Request.MapPath(text2), 60, 60);
					ResourcesHelper.CreateThumbnail(text9, context.Request.MapPath(text3), 100, 100);
					ResourcesHelper.CreateThumbnail(text9, context.Request.MapPath(text4), 160, 160);
					ResourcesHelper.CreateThumbnail(text9, context.Request.MapPath(text5), 180, 180);
					ResourcesHelper.CreateThumbnail(text9, context.Request.MapPath(text6), 220, 220);
					ResourcesHelper.CreateThumbnail(text9, context.Request.MapPath(text7), 310, 310);
					ResourcesHelper.CreateThumbnail(text9, context.Request.MapPath(text8), 410, 410);
				}
				catch
				{
				}
			}
			return new string[8]
			{
				text,
				text2,
				text3,
				text4,
				text5,
				text6,
				text7,
				text8
			};
		}

		public static bool AddProductTags(int productId, IList<int> tagsId, DbTransaction dbtran)
		{
			bool flag = new TagDao().AddProductTags(productId, tagsId, dbtran);
			if (flag)
			{
				ProductHelper.ClearProductCahe(productId);
			}
			return flag;
		}

		public static bool DeleteProductTags(int productId, DbTransaction tran)
		{
			bool flag = new TagDao().DeleteProductTags(productId, tran);
			if (flag)
			{
				ProductHelper.ClearProductCahe(productId);
			}
			return flag;
		}

		public static DbQueryResult GetToTaobaoProducts(ProductQuery query)
		{
			return new TaobaoProductDao().GetToTaobaoProducts(query);
		}

		public static PublishToTaobaoProductInfo GetTaobaoProduct(int productId)
		{
			return new TaobaoProductDao().GetTaobaoProduct(productId);
		}

		public static bool UpdateTaobaoProductId(int productId, long taobaoProductId)
		{
			return new TaobaoProductDao().UpdateTaobaoProductId(productId, taobaoProductId);
		}

		public static DataTable GetSkusByProductId(int productId)
		{
			return new SkuDao().GetSkusByProductId(productId);
		}

		public static string GetSkusBySkuId(string skuId, int productId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			ProductInfo productDetails = new ProductDao().GetProductDetails(productId);
			if (productDetails != null && !productDetails.HasSKU)
			{
				return string.Empty;
			}
			DataTable skusByProductId = new SkuDao().GetSkusByProductId(productId);
			if (!skusByProductId.Columns.Contains("skuId"))
			{
				return string.Empty;
			}
			DataRow[] array = skusByProductId.Select("skuid='" + skuId + "'");
			if (array.Length == 0)
			{
				return string.Empty;
			}
			DataRow dataRow = array[0];
			string text = Regex.Replace(dataRow["SkuId"].ToString(), "^" + productDetails.ProductId + "?", "");
			string[] array2 = text.Split('_');
			foreach (string text2 in array2)
			{
				if (!string.IsNullOrEmpty(text2))
				{
					AttributeValueInfo attributeValueInfo = new AttributeValueDao().Get<AttributeValueInfo>(text2.ToInt(0));
					if (attributeValueInfo != null)
					{
						AttributeInfo attribute = new AttributeDao().GetAttribute(attributeValueInfo.AttributeId);
						if (attribute != null)
						{
							stringBuilder.AppendFormat("{0}:{1},", attribute.AttributeName, attributeValueInfo.ValueStr);
						}
					}
				}
			}
			return stringBuilder.ToString().Substring(0, stringBuilder.Length - 1);
		}

		public static DataTable GetSkusByProductIdNew(int productId)
		{
			ProductInfo productDetails = new ProductDao().GetProductDetails(productId);
			if (productDetails != null && !productDetails.HasSKU)
			{
				return new DataTable();
			}
			DataTable skusByProductId = new SkuDao().GetSkusByProductId(productId);
			skusByProductId.Columns.Add(new DataColumn
			{
				ColumnName = "ValueStr",
				DataType = typeof(string)
			});
			for (int i = 0; i < skusByProductId.Rows.Count; i++)
			{
				DataRow dataRow = skusByProductId.Rows[i];
				StringBuilder stringBuilder = new StringBuilder();
				string text = Regex.Replace(dataRow["SkuId"].ToString(), "^" + productDetails.ProductId + "?", "");
				string[] array = text.Split('_');
				foreach (string text2 in array)
				{
					if (!string.IsNullOrEmpty(text2))
					{
						AttributeValueInfo attributeValueInfo = new AttributeValueDao().Get<AttributeValueInfo>(text2.ToInt(0));
						if (attributeValueInfo != null)
						{
							stringBuilder.AppendFormat("{0},", attributeValueInfo.ValueStr);
						}
					}
				}
				if (stringBuilder.Length != 0)
				{
					dataRow["ValueStr"] = stringBuilder.ToString().Substring(0, stringBuilder.Length - 1);
				}
			}
			return skusByProductId;
		}

		public static SKUItem GetSkuItemBySkuId(string skuId, int storeId = 0)
		{
			return new SkuDao().GetSkuItem(skuId, storeId);
		}

		public static List<int> GetAllEffectiveActivityProductId()
		{
			List<int> list = new List<int>();
			DataTable allEffectiveActivityProductId = new ProductDao().GetAllEffectiveActivityProductId();
			if (allEffectiveActivityProductId != null && allEffectiveActivityProductId.Rows.Count > 0)
			{
				foreach (DataRow row in allEffectiveActivityProductId.Rows)
				{
					list.Add(row[0].ToInt(0));
				}
			}
			return list;
		}

		public static List<int> GetAllEffectiveActivityProductId_PreSale()
		{
			List<int> list = new List<int>();
			DataTable allEffectiveActivityProductId_PreSale = new ProductDao().GetAllEffectiveActivityProductId_PreSale();
			if (allEffectiveActivityProductId_PreSale != null && allEffectiveActivityProductId_PreSale.Rows.Count > 0)
			{
				foreach (DataRow row in allEffectiveActivityProductId_PreSale.Rows)
				{
					list.Add(row[0].ToInt(0));
				}
			}
			return list;
		}

		public static string RemoveEffectiveActivityProductId(string productids)
		{
			try
			{
				List<int> allEffectiveActivityProductId = ProductHelper.GetAllEffectiveActivityProductId();
				string text = string.Empty;
				string[] array = productids.Split(',');
				foreach (string text2 in array)
				{
					if (!allEffectiveActivityProductId.Contains(int.Parse(text2)))
					{
						text = text + text2 + ",";
					}
				}
				if (!string.IsNullOrEmpty(text))
				{
					text = text.Substring(0, text.Length - 1);
				}
				return text;
			}
			catch (Exception)
			{
				return "";
			}
		}

		public static string RemoveEffectiveActivityProductId_PreSale(string productids)
		{
			try
			{
				List<string> source = new List<string>(productids.Split(','));
				List<int> lst = ProductHelper.GetAllEffectiveActivityProductId_PreSale();
				IEnumerable<string> source2 = from n1 in source
				where !lst.Contains(n1.ToInt(0))
				select n1;
				return string.Join(",", source2.ToArray());
			}
			catch (Exception)
			{
				return "";
			}
		}

		public static void ClearProductCahe(int productId)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			try
			{
				List<int> allMemberGrade = new MemberGradeDao().GetAllMemberGrade();
				allMemberGrade.Add(0);
				foreach (int item in allMemberGrade)
				{
					string key = $"DataCache-Product-{productId}-{item}-{masterSettings.OpenMultStore}";
					HiCache.Remove(key);
					string key2 = $"DataCache-WapProduct-{productId}-{item}-{masterSettings.OpenMultStore}";
					HiCache.Remove(key2);
					string key3 = $"DataCache-AppletProduct-{productId}-{item}";
					HiCache.Remove(key3);
				}
			}
			catch (Exception)
			{
			}
		}

		private static void ClearProductCahe(string productIds)
		{
			SiteSettings setting = SettingsManager.GetMasterSettings();
			try
			{
				string[] array = productIds.Split(',');
				if (array.Length != 0)
				{
					List<int> GradeList = new MemberGradeDao().GetAllMemberGrade();
					GradeList.Add(0);
					array.ForEach(delegate(string a)
					{
						if (!string.IsNullOrEmpty(a))
						{
							foreach (int item in GradeList)
							{
								string key = $"DataCache-Product-{a}-{item}-{setting.OpenMultStore}";
								HiCache.Remove(key);
								string key2 = $"DataCache-WapProduct-{a}-{item}-{setting.OpenMultStore}";
								HiCache.Remove(key2);
								string key3 = $"DataCache-AppletProduct-{a}-{item}";
								HiCache.Remove(key3);
							}
						}
					});
				}
			}
			catch (Exception)
			{
			}
		}

		public static IList<ShippingTemplateInfo> GetProductShippingTemplates(string productIds)
		{
			return new ProductDao().GetProductShippingTemplates(productIds);
		}

		public static bool SetProductShippingTemplates(string productIds, int shippingTemplateId)
		{
			return new ProductDao().SetProductShippingTemplates(productIds, shippingTemplateId);
		}

		public static ProductInfo GetProductBaseDetails(int productId)
		{
			return new ProductDao().GetProductBaseDetails(productId);
		}

		public static List<ProductInputItemInfo> GetProductInputItemList(int productId)
		{
			return new ProductDao().GetProductInputItemList(productId);
		}
	}
}
