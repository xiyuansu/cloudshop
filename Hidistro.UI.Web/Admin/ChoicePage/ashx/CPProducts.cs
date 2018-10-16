using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Commodities;
using Hidistro.UI.Web.ashxBase;
using System;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.UI.Web.Admin.ChoicePage.ashx
{
	public class CPProducts : AdminBaseHandler
	{
		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			if (!string.IsNullOrWhiteSpace(base.action))
			{
				base.action = base.action.ToLower();
			}
			string action = base.action;
			if (action == "getlist")
			{
				this.GetList(context);
				return;
			}
			throw new HidistroAshxException("错误的参数");
		}

		private ProductQuery GetDataQuery(HttpContext context)
		{
			ProductQuery productQuery = new ProductQuery();
			productQuery.Keywords = base.GetParameter(context, "Keywords", true);
			productQuery.ProductCode = base.GetParameter(context, "ProductCode", true);
			productQuery.CategoryId = base.GetIntParam(context, "CategoryId", true);
			productQuery.BrandId = base.GetIntParam(context, "BrandId", true);
			productQuery.TypeId = base.GetIntParam(context, "TypeId", true);
			productQuery.TagId = base.GetIntParam(context, "TagId", true);
			ProductSaleStatus? nullable = base.GetParameter(context, "SaleStatus", (ProductSaleStatus?)null);
			if (!nullable.HasValue)
			{
				nullable = ProductSaleStatus.All;
			}
			productQuery.SaleStatus = nullable.Value;
			productQuery.StartDate = base.GetDateTimeParam(context, "StartDate");
			productQuery.EndDate = base.GetDateTimeParam(context, "EndDate");
			productQuery.IsWarningStock = base.GetBoolParam(context, "IsWarningStock", false).Value;
			productQuery.IsFilterFightGroupProduct = base.GetBoolParam(context, "IsFilterFightGroupProduct", false).Value;
			productQuery.IsFilterBundlingProduct = base.GetBoolParam(context, "IsFilterBundlingProduct", false).Value;
			productQuery.IsFilterPromotionProduct = base.GetBoolParam(context, "IsFilterPromotionProduct", false).Value;
			productQuery.IsFilterCountDownProduct = base.GetBoolParam(context, "IsFilterCountDownProduct", false).Value;
			productQuery.IsFilterGroupBuyProduct = base.GetBoolParam(context, "IsFilterGroupBuyProduct", false).Value;
			productQuery.IsHasStock = base.GetBoolParam(context, "IsHasStock", false).Value;
			productQuery.NotInCombinationMainProduct = base.GetBoolParam(context, "NotInCombinationMainProduct", false).Value;
			productQuery.NotInPreSaleProduct = base.GetBoolParam(context, "NotInPreSaleProduct", false).Value;
			productQuery.NotInCombinationOtherProduct = base.GetBoolParam(context, "NotInCombinationOtherProduct", false).Value;
			productQuery.FilterProductIds = base.GetParameter(context, "FilterProductIds", true);
			if (productQuery.CategoryId.HasValue)
			{
				productQuery.MaiCategoryPath = CatalogHelper.GetCategory(productQuery.CategoryId.Value).Path;
			}
			Globals.EntityCoding(productQuery, true);
			productQuery.PageSize = base.CurrentPageSize;
			productQuery.PageIndex = base.CurrentPageIndex;
			productQuery.SortBy = "DisplaySequence";
			productQuery.SortOrder = SortAction.Desc;
			return productQuery;
		}

		private void GetList(HttpContext context)
		{
			ProductQuery dataQuery = this.GetDataQuery(context);
			DataGridViewModel<Dictionary<string, object>> listProducts = this.GetListProducts(dataQuery);
			string s = base.SerializeObjectToJson(listProducts);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetListProducts(ProductQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			if (query != null)
			{
				DbQueryResult products = ProductHelper.GetProducts(query);
				dataGridViewModel.rows = DataHelper.DataTableToDictionary(products.Data);
				dataGridViewModel.total = products.TotalRecords;
				foreach (Dictionary<string, object> row in dataGridViewModel.rows)
				{
					row["ThumbnailUrl40"] = base.GetImageOrDefaultImage(row["ThumbnailUrl40"], base.CurrentSiteSetting.DefaultProductImage);
					if (row["CostPrice"] == null || Convert.IsDBNull(row["CostPrice"]))
					{
						row["CostPrice"] = 0;
					}
				}
			}
			return dataGridViewModel;
		}
	}
}
