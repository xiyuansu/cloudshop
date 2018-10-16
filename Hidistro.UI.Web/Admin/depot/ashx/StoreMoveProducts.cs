using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Depot;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hidistro.UI.Web.Admin.depot.ashx
{
	public class StoreMoveProducts : AdminBaseHandler
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
			int? categoryId = null;
			bool flag = false;
			int pageIndex = 1;
			int pageSize = 10;
			int num = 0;
			string empty = string.Empty;
			string keywords = context.Request["productName"];
			empty = context.Request["categoryId"];
			if (!string.IsNullOrWhiteSpace(empty))
			{
				try
				{
					categoryId = int.Parse(empty);
				}
				catch
				{
					categoryId = null;
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
			ProductQuery productQuery = new ProductQuery();
			productQuery.StoreId = base.GetIntParam(context, "StoreId", false).Value;
			productQuery.CategoryId = categoryId;
			productQuery.PageIndex = pageIndex;
			productQuery.PageSize = pageSize;
			productQuery.Keywords = keywords;
			productQuery.SaleStatus = ProductSaleStatus.OnSale;
			productQuery.SupplierId = 0;
			productQuery.IsFilterStoreProducts = true;
			if (!string.IsNullOrEmpty(context.Request["BrandId"]))
			{
				productQuery.BrandId = base.GetIntParam(context, "BrandId", false).Value;
			}
			productQuery.SortBy = "DisplaySequence";
			productQuery.SortOrder = SortAction.Desc;
			productQuery.ProductType = (-1).GetHashCode();
			if (categoryId.HasValue)
			{
				productQuery.MaiCategoryPath = CatalogHelper.GetCategory(categoryId.Value).Path;
			}
			Globals.EntityCoding(productQuery, true);
			DataGridViewModel<Dictionary<string, object>> products = this.GetProducts(productQuery);
			string s = base.SerializeObjectToJson(products);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetProducts(ProductQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			if (query != null)
			{
				DbQueryResult products = ProductHelper.GetProducts(query);
				dataGridViewModel.rows = DataHelper.DataTableToDictionary(products.Data);
				dataGridViewModel.total = products.TotalRecords;
				foreach (Dictionary<string, object> row in dataGridViewModel.rows)
				{
					ProductInfo productInfo = row.ToObject<ProductInfo>();
					if (productInfo.CategoryId > 0)
					{
						CategoryInfo category = CatalogHelper.GetCategory(productInfo.CategoryId);
						row.Add("categoryName", category.Name);
					}
					else
					{
						row.Add("categoryName", "");
					}
					if (string.IsNullOrEmpty(productInfo.ThumbnailUrl40.ToNullString()))
					{
						row["ThumbnailUrl40"] = HiContext.Current.SiteSettings.DefaultProductImage;
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
