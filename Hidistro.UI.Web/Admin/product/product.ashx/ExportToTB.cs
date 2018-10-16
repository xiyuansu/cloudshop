using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Web.ashxBase;
using Hishop.TransferManager;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace Hidistro.UI.Web.Admin.product.ashx
{
	[PrivilegeCheck(Privilege.ProductBatchExport)]
	public class ExportToTB : AdminBaseHandler
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
				if (action == "export")
				{
					this.Export(context);
					return;
				}
				throw new HidistroAshxException("错误的参数");
			}
			this.GetList(context);
		}

		private void GetList(HttpContext context)
		{
			AdvancedProductQuery query = this.GetQuery(context);
			DataGridViewModel<Dictionary<string, object>> products = this.GetProducts(query, context);
			string s = base.SerializeObjectToJson(products);
			context.Response.Write(s);
			context.Response.End();
		}

		private AdvancedProductQuery GetQuery(HttpContext context)
		{
			AdvancedProductQuery advancedProductQuery = new AdvancedProductQuery();
			int num = 1;
			int num2 = 10;
			int num3 = 0;
			string empty = string.Empty;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			advancedProductQuery.Keywords = context.Request["productName"];
			advancedProductQuery.ProductCode = context.Request["productCode"];
			advancedProductQuery.CategoryId = base.GetIntParam(context, "categoryId", true);
			advancedProductQuery.SaleStatus = ProductSaleStatus.OnSale;
			if (advancedProductQuery.CategoryId.HasValue)
			{
				advancedProductQuery.MaiCategoryPath = CatalogHelper.GetCategory(advancedProductQuery.CategoryId.Value).Path;
			}
			advancedProductQuery.IncludeInStock = base.GetBoolParam(context, "IncludeInStock", false).Value;
			advancedProductQuery.IncludeOnSales = base.GetBoolParam(context, "IncludeOnSales", false).Value;
			advancedProductQuery.IncludeUnSales = base.GetBoolParam(context, "IncludeUnSales", false).Value;
			if (!advancedProductQuery.IncludeInStock && !advancedProductQuery.IncludeOnSales && !advancedProductQuery.IncludeUnSales)
			{
				throw new HidistroAshxException("至少要选择包含一个商品状态");
			}
			advancedProductQuery.IsMakeTaobao = base.GetIntParam(context, "IsMakeTaobao", true);
			advancedProductQuery.StartDate = base.GetDateTimeParam(context, "StartDate");
			advancedProductQuery.EndDate = base.GetDateTimeParam(context, "EndDate");
			advancedProductQuery.PageSize = base.CurrentPageSize;
			advancedProductQuery.PageIndex = base.CurrentPageIndex;
			advancedProductQuery.SortOrder = SortAction.Desc;
			advancedProductQuery.SortBy = "DisplaySequence";
			Globals.EntityCoding(advancedProductQuery, true);
			return advancedProductQuery;
		}

		private DataGridViewModel<Dictionary<string, object>> GetProducts(AdvancedProductQuery query, HttpContext context)
		{
			string empty = string.Empty;
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			if (query != null)
			{
				empty = (string.IsNullOrEmpty(context.Request["RemoveProductId"]) ? "" : context.Request["RemoveProductId"]);
				DbQueryResult exportProducts = ProductHelper.GetExportProducts(query, empty);
				dataGridViewModel.rows = DataHelper.DataTableToDictionary(exportProducts.Data);
				dataGridViewModel.total = exportProducts.TotalRecords;
				foreach (Dictionary<string, object> row in dataGridViewModel.rows)
				{
					if (string.IsNullOrEmpty(row["ThumbnailUrl40"].ToNullString()))
					{
						row["ThumbnailUrl40"] = HiContext.Current.SiteSettings.DefaultProductImage;
					}
				}
			}
			return dataGridViewModel;
		}

		private void Export(HttpContext context)
		{
			string text = context.Request["adapterName"];
			if (string.IsNullOrEmpty(text) || text.Length == 0)
			{
				throw new HidistroAshxException("请选择一个导出版本！");
			}
			bool flag = false;
			bool value = base.GetBoolParam(context, "exportStock", true).Value;
			bool flag2 = true;
			string empty = string.Empty;
			string text2 = "http://" + HttpContext.Current.Request.Url.Host;
			string empty2 = string.Empty;
			empty = (string.IsNullOrEmpty(context.Request["RemoveProductId"]) ? "" : context.Request["RemoveProductId"]);
			DataSet exportProducts = ProductHelper.GetExportProducts(this.GetQuery(context), flag, value, empty);
			ExportAdapter exporter = TransferHelper.GetExporter(text, exportProducts, flag, value, flag2, text2, empty2);
			exporter.DoExport();
		}
	}
}
