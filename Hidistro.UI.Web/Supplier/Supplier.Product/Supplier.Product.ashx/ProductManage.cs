using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Web.ashxBase;
using System;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.UI.Web.Supplier.Product.ashx
{
	[PrivilegeCheck(Privilege.Products)]
	public class ProductManage : SupplierAdminHandler
	{
		private new ManagerInfo CurrentManager;

		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			this.CurrentManager = HiContext.Current.Manager;
			string text = context.Request["action"];
			if (string.IsNullOrWhiteSpace(text))
			{
				throw new HidistroAshxException("错误的参数");
			}
			string a = text;
			if (a == "getlist")
			{
				this.GetList(context);
				return;
			}
			throw new HidistroAshxException("错误的参数");
		}

		private void GetList(HttpContext context)
		{
			int? categoryId = null;
			int? typeId = null;
			bool flag = false;
			int pageIndex = 1;
			int pageSize = 10;
			string empty = string.Empty;
			ProductSaleStatus saleStatus = ProductSaleStatus.All;
			string keywords = context.Request["productName"];
			string productCode = context.Request["productCode"];
			flag = false;
			if (context.Request["isWarning"] == "true")
			{
				flag = true;
			}
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
			empty = context.Request["typeId"];
			if (!string.IsNullOrWhiteSpace(empty))
			{
				try
				{
					typeId = int.Parse(empty);
				}
				catch
				{
					typeId = null;
				}
			}
			empty = context.Request["saleStatus"];
			if (!string.IsNullOrWhiteSpace(empty))
			{
				try
				{
					saleStatus = (ProductSaleStatus)Enum.Parse(typeof(ProductSaleStatus), empty);
				}
				catch
				{
					saleStatus = ProductSaleStatus.All;
				}
			}
			SupplierProductQuery supplierProductQuery = new SupplierProductQuery
			{
				Keywords = keywords,
				ProductCode = productCode,
				CategoryId = categoryId,
				PageSize = pageSize,
				PageIndex = pageIndex,
				SortOrder = SortAction.Desc,
				SortBy = "DisplaySequence",
				TypeId = typeId,
				SaleStatus = saleStatus,
				AuditStatus = ProductAuditStatus.Pass,
				SupplierId = HiContext.Current.Manager.StoreId,
				IsWarningStock = flag
			};
			if (supplierProductQuery.CategoryId > 0)
			{
				supplierProductQuery.MaiCategoryPath = CatalogHelper.GetCategory(categoryId.Value).Path;
			}
			string s = base.SerializeObjectToJson(this.GetProducts(supplierProductQuery));
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetProducts(SupplierProductQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			string empty = string.Empty;
			if (query != null)
			{
				DbQueryResult products = ProductHelper.GetProducts(query);
				List<Dictionary<string, object>> list = DataHelper.DataTableToDictionary(products.Data);
				foreach (Dictionary<string, object> item in list)
				{
					if (string.IsNullOrEmpty(item["ThumbnailUrl40"].ToNullString()))
					{
						item["ThumbnailUrl40"] = HiContext.Current.SiteSettings.DefaultProductImage;
					}
				}
				dataGridViewModel.rows = list;
				dataGridViewModel.total = products.TotalRecords;
			}
			return dataGridViewModel;
		}
	}
}
