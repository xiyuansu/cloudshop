using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Depot;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Depot;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Hidistro.UI.Web.Depot.home.ashx
{
	public class SearchProduct : StoreAdminBaseHandler
	{
		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			if (string.IsNullOrWhiteSpace(base.action))
			{
				throw new HidistroAshxException("错误的参数");
			}
			base.action = base.action.ToLower();
			string action = base.action;
			if (!(action == "getlist"))
			{
				if (action == "getallproducts")
				{
					this.GetAllProducts(context);
					return;
				}
				throw new HidistroAshxException("错误的参数");
			}
			this.GetList(context);
		}

		private void GetList(HttpContext context)
		{
			int? nullable = null;
			bool flag = false;
			string productName = context.Request["Keywords"];
			string productCode = "";
			flag = false;
			if (context.Request["isWarning"] == "1")
			{
				flag = true;
			}
			nullable = base.GetIntParam(context, "CategoryId", true);
			int storeId = base.CurrentManager.StoreId;
			StoreProductsQuery storeProductsQuery = new StoreProductsQuery();
			storeProductsQuery.StoreId = storeId;
			storeProductsQuery.WarningStockNum = flag;
			storeProductsQuery.SaleStatus = 1.GetHashCode();
			storeProductsQuery.productCode = productCode;
			storeProductsQuery.CategoryId = (nullable.HasValue ? nullable.Value : 0);
			storeProductsQuery.PageIndex = base.CurrentPageIndex;
			storeProductsQuery.PageSize = base.CurrentPageSize;
			storeProductsQuery.ProductName = productName;
			if (storeProductsQuery.CategoryId > 0)
			{
				storeProductsQuery.MainCategoryPath = CatalogHelper.GetCategory(nullable.Value).Path;
			}
			storeProductsQuery.FilterProductIds = Globals.UrlDecode(base.GetParameter(context, "FilterProductIds", true));
			storeProductsQuery.ProductType = base.GetIntParam(context, "ProductType", true).ToInt(0);
			DataGridViewModel<StoreProductsViewInfo> products = this.GetProducts(storeProductsQuery);
			string s = base.SerializeObjectToJson(products);
			context.Response.Write(s);
			context.Response.End();
		}

		private void GetAllProducts(HttpContext context)
		{
			ProductQuery productQuery = new ProductQuery();
			productQuery.Keywords = Globals.UrlDecode(base.GetParameter(context, "Keywords", true));
			productQuery.BrandId = base.GetIntParam(context, "BrandId", true);
			productQuery.CategoryId = base.GetIntParam(context, "CategoryId", true);
			if (productQuery.CategoryId.HasValue)
			{
				productQuery.MaiCategoryPath = CatalogHelper.GetCategory(productQuery.CategoryId.Value).Path;
			}
			productQuery.FilterProductIds = Globals.UrlDecode(base.GetParameter(context, "FilterProductIds", true));
			productQuery.ProductType = base.GetIntParam(context, "ProductType", true).ToInt(0);
			productQuery.SaleStatus = ProductSaleStatus.OnSale;
			if (context.Request["IsIncludeHomeProduct"].ToInt(0) > 0)
			{
				productQuery.IsIncludeHomeProduct = false;
			}
			if (context.Request["IsIncludeAppletProduct"].ToInt(0) > 0)
			{
				productQuery.IsIncludeAppletProduct = false;
			}
			productQuery.IsIncludeBundlingProduct = false;
			productQuery.IsFilterPromotionProduct = false;
			productQuery.PageIndex = base.CurrentPageIndex;
			productQuery.PageSize = base.CurrentPageSize;
			IList<ProductInfo> allProducts = ProductHelper.GetAllProducts(productQuery);
			var value = new
			{
				Success = true,
				Products = from p in allProducts
				select new
				{
					p.ProductId,
					p.ProductName,
					p.ThumbnailUrl310
				}
			};
			string s = base.SerializeObjectToJson(value);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList(ProductQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			if (query != null)
			{
				DbQueryResult products = ProductHelper.GetProducts(query);
				dataGridViewModel.rows = DataHelper.DataTableToDictionary(products.Data);
				dataGridViewModel.total = products.TotalRecords;
				foreach (Dictionary<string, object> row in dataGridViewModel.rows)
				{
				}
			}
			return dataGridViewModel;
		}

		private DataGridViewModel<StoreProductsViewInfo> GetProducts(StoreProductsQuery query)
		{
			DataGridViewModel<StoreProductsViewInfo> dataGridViewModel = new DataGridViewModel<StoreProductsViewInfo>();
			if (query != null)
			{
				PageModel<StoreProductsViewInfo> storeProducts = StoresHelper.GetStoreProducts(query);
				dataGridViewModel.rows = storeProducts.Models.ToList();
				dataGridViewModel.total = storeProducts.Total;
				foreach (StoreProductsViewInfo row in dataGridViewModel.rows)
				{
					row.MainStock = this.GetMainStock(row.ProductId);
					if (!row.MarketPrice.HasValue)
					{
						row.MarketPrice = 0.0m;
					}
					if (row.StoreSalePrice == decimal.Zero)
					{
						row.StoreSalePrice = row.SalePrice.Value;
					}
					if (string.IsNullOrEmpty(row.ThumbnailUrl40))
					{
						row.ThumbnailUrl40 = base.CurrentSiteSetting.DefaultProductThumbnail1;
					}
				}
			}
			return dataGridViewModel;
		}

		private int GetMainStock(int ProductId)
		{
			int result = 0;
			DataTable skuStocks = ProductHelper.GetSkuStocks(ProductId.ToString());
			if (skuStocks != null && skuStocks.Rows.Count > 0)
			{
				try
				{
					result = skuStocks.AsEnumerable().Sum((DataRow a) => a.Field<int>("Stock"));
				}
				catch
				{
					result = 0;
				}
			}
			return result;
		}
	}
}
