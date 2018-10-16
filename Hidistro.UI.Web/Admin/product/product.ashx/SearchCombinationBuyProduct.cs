using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Commodities;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.UI.Web.Admin.product.ashx
{
	public class SearchCombinationBuyProduct : AdminBaseHandler
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
			if (action == "getlist")
			{
				this.GetList(context);
				return;
			}
			throw new HidistroAshxException("错误的参数");
		}

		private void GetList(HttpContext context)
		{
			int num = context.Request["IsSingle"].ToInt(0);
			ProductQuery productQuery = new ProductQuery();
			productQuery.Keywords = base.GetParameter(context, "Keywords", true);
			productQuery.BrandId = base.GetIntParam(context, "BrandId", true);
			productQuery.CategoryId = base.GetIntParam(context, "CategoryId", true);
			productQuery.TagId = base.GetIntParam(context, "TagId", true);
			if (productQuery.CategoryId.HasValue)
			{
				productQuery.MaiCategoryPath = CatalogHelper.GetCategory(productQuery.CategoryId.Value).Path;
			}
			productQuery.FilterProductIds = base.GetParameter(context, "FilterProductIds", true);
			productQuery.SaleStatus = ProductSaleStatus.OnSale;
			productQuery.IsHasStock = true;
			productQuery.IsFilterGroupBuyProduct = true;
			productQuery.IsFilterFightGroupProduct = true;
			productQuery.IsFilterCountDownProduct = true;
			productQuery.NotInPreSaleProduct = true;
			if (num == 1)
			{
				productQuery.NotInCombinationMainProduct = true;
			}
			productQuery.PageIndex = base.CurrentPageIndex;
			productQuery.PageSize = base.CurrentPageSize;
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList(productQuery);
			string s = base.SerializeObjectToJson(dataList);
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
