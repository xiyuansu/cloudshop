using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Commodities;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.UI.Web.Admin.Supplier.Product.ashx
{
	public class AuditProductList : AdminBaseHandler
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

		public void GetList(HttpContext context)
		{
			SupplierProductQuery dataQuery = this.GetDataQuery(context);
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList(dataQuery);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private SupplierProductQuery GetDataQuery(HttpContext context)
		{
			SupplierProductQuery supplierProductQuery = new SupplierProductQuery();
			supplierProductQuery.Keywords = base.GetParameter(context, "Keywords", true);
			supplierProductQuery.CategoryId = base.GetIntParam(context, "CategoryId", true);
			if (supplierProductQuery.CategoryId.HasValue)
			{
				supplierProductQuery.MaiCategoryPath = CatalogHelper.GetCategory(supplierProductQuery.CategoryId.Value).Path;
			}
			supplierProductQuery.SaleStatus = ProductSaleStatus.OnStock;
			supplierProductQuery.AuditStatus = ProductAuditStatus.Apply;
			int? intParam = base.GetIntParam(context, "SupplierId", true);
			if (intParam.HasValue)
			{
				supplierProductQuery.SupplierId = intParam.Value;
			}
			else
			{
				supplierProductQuery.SupplierId = -1;
			}
			supplierProductQuery.Role = SystemRoles.SystemAdministrator;
			supplierProductQuery.PageIndex = base.CurrentPageIndex;
			supplierProductQuery.PageSize = base.CurrentPageSize;
			supplierProductQuery.SortBy = "UpdateDate";
			supplierProductQuery.SortOrder = SortAction.Desc;
			return supplierProductQuery;
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList(SupplierProductQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			if (query != null)
			{
				DbQueryResult products = ProductHelper.GetProducts(query);
				dataGridViewModel.rows = DataHelper.DataTableToDictionary(products.Data);
				foreach (Dictionary<string, object> row in dataGridViewModel.rows)
				{
					if (string.IsNullOrEmpty(row["ThumbnailUrl40"].ToNullString()))
					{
						row["ThumbnailUrl40"] = HiContext.Current.SiteSettings.DefaultProductImage;
					}
				}
				dataGridViewModel.total = products.TotalRecords;
			}
			return dataGridViewModel;
		}
	}
}
