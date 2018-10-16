using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Web.ashxBase;
using System;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.UI.Web.Depot.product.ashx
{
	[PrivilegeCheck(Privilege.Products)]
	public class ProductOnSales : StoreAdminBaseHandler
	{
		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			if (string.IsNullOrWhiteSpace(base.action))
			{
				throw new HidistroAshxException("错误的参数");
			}
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
			ProductQuery query = this.GetQuery(context);
			DataGridViewModel<Dictionary<string, object>> products = this.GetProducts(query);
			string s = base.SerializeObjectToJson(products);
			context.Response.Write(s);
			context.Response.End();
		}

		private ProductQuery GetQuery(HttpContext context)
		{
			ProductQuery productQuery = new ProductQuery();
			int num = 1;
			int num2 = 10;
			string empty = string.Empty;
			productQuery.Keywords = context.Request["productName"];
			productQuery.ProductCode = context.Request["productCode"];
			productQuery.CategoryId = base.GetIntParam(context, "categoryId", true);
			if (productQuery.CategoryId.HasValue)
			{
				productQuery.MaiCategoryPath = CatalogHelper.GetCategory(productQuery.CategoryId.Value).Path;
			}
			if (context.Request["isWarning"] == "1")
			{
				productQuery.IsWarningStock = true;
			}
			productQuery.StartDate = base.GetDateTimeParam(context, "StartDate");
			productQuery.EndDate = base.GetDateTimeParam(context, "EndDate");
			productQuery.BrandId = base.GetIntParam(context, "BrandId", true);
			productQuery.TagId = base.GetIntParam(context, "TagId", true);
			productQuery.TypeId = base.GetIntParam(context, "TypeId", true);
			int? intParam = base.GetIntParam(context, "SaleStatus", true);
			if (intParam.HasValue)
			{
				productQuery.SaleStatus = (ProductSaleStatus)intParam.Value;
			}
			else
			{
				productQuery.SaleStatus = ProductSaleStatus.OnSale;
			}
			num = base.GetIntParam(context, "page", false).Value;
			if (num < 1)
			{
				num = 1;
			}
			num2 = base.GetIntParam(context, "rows", false).Value;
			if (num2 < 1)
			{
				num2 = 10;
			}
			productQuery.PageSize = num2;
			productQuery.PageIndex = num;
			productQuery.SortOrder = SortAction.Desc;
			productQuery.SortBy = "DisplaySequence";
			productQuery.StoreId = HiContext.Current.Manager.StoreId;
			productQuery.IsFilterStoreProducts = true;
			productQuery.SupplierId = 0;
			productQuery.ProductType = base.GetIntParam(context, "ProductType", true).ToInt(0);
			Globals.EntityCoding(productQuery, true);
			return productQuery;
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
					switch ((ProductSaleStatus)row["SaleStatus"])
					{
					case ProductSaleStatus.OnSale:
						row["SaleStatus"] = "出售中";
						break;
					case ProductSaleStatus.UnSale:
						row["SaleStatus"] = "下架区";
						break;
					default:
						row["SaleStatus"] = "仓库中";
						break;
					}
					if (Convert.IsDBNull(row["MarketPrice"]))
					{
						row["MarketPrice"] = null;
					}
					decimal? nullable = (decimal?)row["MarketPrice"];
					if (nullable.HasValue)
					{
						row["MarketPrice"] = nullable.Value.F2ToString("f2");
					}
					else
					{
						row["MarketPrice"] = "-";
					}
				}
			}
			return dataGridViewModel;
		}
	}
}
