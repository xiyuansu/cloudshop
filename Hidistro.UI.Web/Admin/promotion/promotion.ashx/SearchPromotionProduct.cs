using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Promotions;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.UI.Web.Admin.promotion.ashx
{
	public class SearchPromotionProduct : AdminBaseHandler
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
				if (action == "add")
				{
					this.Add(context);
					return;
				}
				throw new HidistroAshxException("错误的参数");
			}
			this.GetList(context);
		}

		public void GetList(HttpContext context)
		{
			ProductQuery productQuery = new ProductQuery();
			productQuery.IsMobileExclusive = base.GetBoolParam(context, "IsMobileExclusive", false).Value;
			productQuery.Keywords = base.GetParameter(context, "Keywords", true);
			productQuery.BrandId = base.GetIntParam(context, "BrandId", true);
			productQuery.CategoryId = base.GetIntParam(context, "CategoryId", true);
			productQuery.TagId = base.GetIntParam(context, "TagId", true);
			if (productQuery.CategoryId.HasValue)
			{
				productQuery.MaiCategoryPath = CatalogHelper.GetCategory(productQuery.CategoryId.Value).Path;
			}
			productQuery.SaleStatus = ProductSaleStatus.OnSale;
			productQuery.IsIncludePromotionProduct = false;
			productQuery.IsIncludeBundlingProduct = false;
			productQuery.IsFilterPromotionProduct = false;
			productQuery.IsFilterFightGroupProduct = true;
			productQuery.NotInPreSaleProduct = true;
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
					row["MarketPrice"] = row["MarketPrice"].ToDecimal(0);
				}
			}
			return dataGridViewModel;
		}

		public void Add(HttpContext context)
		{
			int? intParam = base.GetIntParam(context, "CombinationId", false);
			if (intParam < 1)
			{
				throw new HidistroAshxException("错误的活动编号");
			}
			if (CombinationBuyHelper.DeleteCombinationBuy(intParam.Value))
			{
				base.ReturnSuccessResult(context, "删除成功!", 0, true);
				return;
			}
			throw new HidistroAshxException("删除失败!");
		}
	}
}
