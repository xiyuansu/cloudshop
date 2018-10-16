using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Web.ashxBase;
using System.Data;
using System.Linq;
using System.Web;

namespace Hidistro.UI.Web.Depot.product.ashx
{
	[PrivilegeCheck(Privilege.Products)]
	public class MyStoreProducts : StoreAdminBaseHandler
	{
		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			if (string.IsNullOrWhiteSpace(base.action))
			{
				throw new HidistroAshxException("错误的参数");
			}
			string action = base.action;
			if (!(action == "getlist"))
			{
				if (action == "delete")
				{
					this.Delete(context);
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
			string productName = context.Request["productName"];
			string productCode = context.Request["productCode"];
			flag = false;
			if (context.Request["isWarning"] == "1")
			{
				flag = true;
			}
			nullable = base.GetIntParam(context, "categoryId", true);
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
			storeProductsQuery.ProductType = base.GetIntParam(context, "ProductType", true).ToInt(0);
			DataGridViewModel<StoreProductsViewInfo> products = this.GetProducts(storeProductsQuery);
			string s = base.SerializeObjectToJson(products);
			context.Response.Write(s);
			context.Response.End();
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

		public void Delete(HttpContext context)
		{
			string text = context.Request.Form["productids"];
			if (string.IsNullOrEmpty(text))
			{
				throw new HidistroAshxException("请先选择要下架的商品");
			}
			int[] values = (from d in text.Split(',')
			select int.Parse(d)).ToArray();
			int storeId = HiContext.Current.Manager.StoreId;
			if (StoresHelper.DeleteProduct(storeId, string.Join("','", values)))
			{
				base.ReturnResult(context, true, "成功移出了选择的商品", 0, true);
			}
			else
			{
				base.ReturnResult(context, false, "移出商品失败，未知错误", -1, true);
			}
		}
	}
}
