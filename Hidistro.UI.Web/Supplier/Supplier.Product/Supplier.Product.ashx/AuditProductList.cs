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
	public class AuditProductList : SupplierAdminHandler
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
			if (!(a == "getlist"))
			{
				if (a == "delete")
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
			int? typeId = null;
			string empty = string.Empty;
			string empty2 = string.Empty;
			ProductSaleStatus productSaleStatus = ProductSaleStatus.All;
			string keywords = context.Request["productName"];
			string productCode = context.Request["productCode"];
			empty2 = context.Request["AuditStatus"];
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
			SupplierProductQuery supplierProductQuery = new SupplierProductQuery
			{
				Keywords = keywords,
				ProductCode = productCode,
				CategoryId = categoryId,
				PageSize = base.CurrentPageSize,
				PageIndex = base.CurrentPageIndex,
				SortOrder = SortAction.Desc,
				SortBy = "UpdateDate",
				TypeId = typeId,
				SaleStatus = ProductSaleStatus.OnStock,
				SupplierId = HiContext.Current.Manager.StoreId,
				Role = SystemRoles.SupplierAdmin
			};
			if (!string.IsNullOrEmpty(empty2))
			{
				supplierProductQuery.AuditStatus = (ProductAuditStatus)Enum.Parse(typeof(ProductAuditStatus), empty2);
			}
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
				Globals.EntityCoding(query, true);
				DbQueryResult products = ProductHelper.GetProducts(query);
				List<Dictionary<string, object>> list = DataHelper.DataTableToDictionary(products.Data);
				List<Dictionary<string, object>> list2 = new List<Dictionary<string, object>>();
				foreach (Dictionary<string, object> item in list)
				{
					ProductInfo productInfo = item.ToObject<ProductInfo>();
					item.Add("AuditStatusStr", this.ParseAuditStatus(string.Concat((int)productInfo.AuditStatus)));
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

		public string ParseAuditStatus(string status)
		{
			if (status == "1")
			{
				return "审核中";
			}
			if (status == "3")
			{
				return "未通过";
			}
			return status;
		}

		private void Delete(HttpContext context)
		{
			string text = context.Request["ids"];
			if (text.Length < 0)
			{
				throw new HidistroAshxException("错误的参数！");
			}
			text = ProductHelper.RemoveEffectiveActivityProductId(text);
			if (string.IsNullOrEmpty(text))
			{
				throw new HidistroAshxException("选中的商品都在参加活动不能被删除！");
			}
			int num = ProductHelper.RemoveProduct(text);
			if (num > 0)
			{
				base.ReturnSuccessResult(context, "成功删除了选择的商品！", 0, true);
				return;
			}
			throw new HidistroAshxException("删除商品失败，未知错误！");
		}
	}
}
