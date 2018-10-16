using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Depot;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Depot;
using Hidistro.UI.Web.ashxBase;
using System.Linq;
using System.Web;

namespace Hidistro.UI.Web.Admin.depot.ashx
{
	public class StoreProducts : AdminBaseHandler
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
			int pageIndex = 1;
			int pageSize = 10;
			int num = 0;
			string empty = string.Empty;
			string productName = context.Request["productName"];
			empty = context.Request["categoryId"];
			if (!string.IsNullOrWhiteSpace(empty))
			{
				try
				{
					nullable = int.Parse(empty);
				}
				catch
				{
					nullable = null;
				}
			}
			empty = context.Request["page"];
			if (!string.IsNullOrWhiteSpace(empty))
			{
				try
				{
					pageIndex = int.Parse(empty);
				}
				catch
				{
					pageIndex = 1;
				}
			}
			empty = context.Request["rows"];
			if (!string.IsNullOrWhiteSpace(empty))
			{
				try
				{
					pageSize = int.Parse(empty);
				}
				catch
				{
					pageSize = 10;
				}
			}
			StoreProductsQuery storeProductsQuery = new StoreProductsQuery();
			storeProductsQuery.StoreId = base.GetIntParam(context, "StoreId", false).Value;
			storeProductsQuery.CategoryId = (nullable.HasValue ? nullable.Value : 0);
			storeProductsQuery.PageIndex = pageIndex;
			storeProductsQuery.PageSize = pageSize;
			storeProductsQuery.ProductName = productName;
			storeProductsQuery.SaleStatus = 1;
			if (storeProductsQuery.CategoryId > 0)
			{
				storeProductsQuery.MainCategoryPath = CatalogHelper.GetCategory(nullable.Value).Path;
			}
			storeProductsQuery.ProductType = -1;
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
					if (string.IsNullOrEmpty(row.ThumbnailUrl40.ToNullString()))
					{
						row.ThumbnailUrl40 = HiContext.Current.SiteSettings.DefaultProductImage;
					}
				}
			}
			return dataGridViewModel;
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
			int storeID = context.Request["StoreId"].ToInt(0);
			if (StoresHelper.DeleteProduct(storeID, string.Join("','", values)))
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
