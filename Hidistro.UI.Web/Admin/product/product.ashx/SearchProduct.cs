using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Commodities;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hidistro.UI.Web.Admin.product.ashx
{
	public class SearchProduct : AdminBaseHandler
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
			ProductQuery productQuery = new ProductQuery();
			productQuery.Keywords = Globals.UrlDecode(base.GetParameter(context, "Keywords", true));
			productQuery.BrandId = base.GetIntParam(context, "BrandId", true);
			productQuery.CategoryId = base.GetIntParam(context, "CategoryId", true);
			if (productQuery.CategoryId.HasValue)
			{
				productQuery.MaiCategoryPath = CatalogHelper.GetCategory(productQuery.CategoryId.Value).Path;
			}
			productQuery.FilterProductIds = Globals.UrlDecode(base.GetParameter(context, "FilterProductIds", true));
			productQuery.SaleStatus = ProductSaleStatus.OnSale;
			if (context.Request["IsIncludeHomeProduct"].ToInt(0) > 0)
			{
				productQuery.IsIncludeHomeProduct = false;
			}
			if (context.Request["IsIncludeAppletProduct"].ToInt(0) > 0)
			{
				productQuery.IsIncludeAppletProduct = false;
			}
			productQuery.ProductType = context.Request["ProductType"].ToInt(0);
			productQuery.IsIncludeBundlingProduct = false;
			productQuery.IsFilterPromotionProduct = false;
			productQuery.PageIndex = base.CurrentPageIndex;
			productQuery.PageSize = base.CurrentPageSize;
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList(productQuery);
			string s = base.SerializeObjectToJson(dataList);
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
	}
}
