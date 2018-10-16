using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hidistro.UI.Web.Admin.Supplier.Product.ashx
{
	public class ProductList : AdminBaseHandler
	{
		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			if (string.IsNullOrWhiteSpace(base.action))
			{
				throw new HidistroAshxException("错误的参数");
			}
			base.action = base.action.ToLower();
			switch (base.action)
			{
			case "getlist":
				this.GetList(context);
				break;
			case "delete":
				this.Delete(context);
				break;
			case "changesalestatus":
				this.ChangeSaleStatus(context);
				break;
			default:
				throw new HidistroAshxException("错误的参数");
			}
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
			supplierProductQuery.ProductCode = base.GetParameter(context, "ProductCode", true);
			supplierProductQuery.CategoryId = base.GetIntParam(context, "CategoryId", true);
			supplierProductQuery.BrandId = base.GetIntParam(context, "BrandId", true);
			supplierProductQuery.TagId = base.GetIntParam(context, "TagId", true);
			supplierProductQuery.TypeId = base.GetIntParam(context, "TypeId", true);
			int? intParam = base.GetIntParam(context, "SaleStatus", true);
			if (intParam.HasValue)
			{
				supplierProductQuery.SaleStatus = (ProductSaleStatus)intParam.Value;
			}
			else
			{
				supplierProductQuery.SaleStatus = ProductSaleStatus.All;
			}
			supplierProductQuery.IsWarningStock = base.GetBoolParam(context, "IsWarningStock", false).Value;
			if (supplierProductQuery.CategoryId.HasValue)
			{
				supplierProductQuery.MaiCategoryPath = CatalogHelper.GetCategory(supplierProductQuery.CategoryId.Value).Path;
			}
			int? intParam2 = base.GetIntParam(context, "SupplierId", true);
			if (intParam2.HasValue)
			{
				supplierProductQuery.SupplierId = intParam2.Value;
			}
			else
			{
				supplierProductQuery.SupplierId = -1;
			}
			supplierProductQuery.AuditStatus = ProductAuditStatus.Pass;
			supplierProductQuery.PageIndex = base.CurrentPageIndex;
			supplierProductQuery.PageSize = base.CurrentPageSize;
			supplierProductQuery.SortBy = "DisplaySequence";
			supplierProductQuery.SortOrder = SortAction.Desc;
			return supplierProductQuery;
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList(SupplierProductQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			if (query != null)
			{
				Globals.EntityCoding(query, true);
				DbQueryResult products = ProductHelper.GetProducts(query);
				dataGridViewModel.rows = DataHelper.DataTableToDictionary(products.Data);
				dataGridViewModel.total = products.TotalRecords;
				foreach (Dictionary<string, object> row in dataGridViewModel.rows)
				{
					row.Add("SaleStatusText", this.ParseSaleStatus((int)row["SaleStatus"]));
					if (string.IsNullOrEmpty(row["ThumbnailUrl40"].ToNullString()))
					{
						row["ThumbnailUrl40"] = HiContext.Current.SiteSettings.DefaultProductImage;
					}
				}
			}
			return dataGridViewModel;
		}

		private string ParseSaleStatus(int status)
		{
			string text = "";
			switch (status)
			{
			case 1:
				return "出售中";
			case 2:
				return "下架中";
			default:
				return "仓库中";
			}
		}

		public void Delete(HttpContext context)
		{
			string text = context.Request["ids"];
			if (text.IndexOf(",") < 0)
			{
				int value = base.GetIntParam(context, "ids", false).Value;
				if (value < 0)
				{
					throw new HidistroAshxException("错误的商品编号");
				}
				this.CheckProductCanDelete(value);
			}
			else
			{
				text = ProductHelper.RemoveEffectiveActivityProductId(text);
			}
			if (string.IsNullOrWhiteSpace(text))
			{
				throw new HidistroAshxException("未选择商品，或选择的商品在活动中，不可删除");
			}
			string[] source = text.Split(',');
			int num = 0;
			int num2 = source.Count();
			num = ProductHelper.RemoveProduct(text);
			if (num > 0)
			{
				if (num2 == 1)
				{
					base.ReturnSuccessResult(context, "删除商品成功", 0, true);
				}
				else
				{
					base.ReturnSuccessResult(context, $"成功删除了{num}个商品,未删除成功的商品可能正参与活动中", 0, true);
				}
				return;
			}
			throw new HidistroAshxException("删除商品失败");
		}

		private void CheckProductCanDelete(int ProductId)
		{
			if (PromoteHelper.ProductCountDownExist(ProductId))
			{
				throw new HidistroAshxException("商品正在参加限时购不能编辑规格、价格、库存以及销售状态");
			}
			if (PromoteHelper.ProductGroupBuyExist(ProductId))
			{
				throw new HidistroAshxException("商品正在参加团购不能编辑规格、价格、库存以及销售状态等");
			}
			if (VShopHelper.ExistEffectiveFightGroupInfo(ProductId))
			{
				throw new HidistroAshxException("商品正在参加火拼团不能编辑规格、价格、库存以及销售状态等");
			}
			if (!CombinationBuyHelper.ExistEffectiveCombinationBuyInfo(ProductId))
			{
				return;
			}
			throw new HidistroAshxException("商品正在参加组合购不能编辑规格、价格、库存以及销售状态等");
		}

		public void ChangeSaleStatus(HttpContext context)
		{
			string text = context.Request["ids"];
			int? intParam = base.GetIntParam(context, "status", true);
			if (!intParam.HasValue)
			{
				throw new HidistroAshxException("错误的状态参数");
			}
			ProductSaleStatus value = (ProductSaleStatus)intParam.Value;
			if (string.IsNullOrWhiteSpace(text))
			{
				throw new HidistroAshxException("请选择要操作的商品");
			}
			string[] source = text.Split(',');
			int num = source.Count();
			bool flag = false;
			int num2 = 0;
			switch (value)
			{
			case ProductSaleStatus.OnSale:
				num2 = ProductHelper.UpShelf(text);
				break;
			case ProductSaleStatus.UnSale:
				text = ProductHelper.RemoveEffectiveActivityProductId(text);
				if (string.IsNullOrWhiteSpace(text))
				{
					throw new HidistroAshxException("选中的商品都在参加活动不能被下架");
				}
				num2 = ProductHelper.OffShelf(text);
				break;
			case ProductSaleStatus.OnStock:
				text = ProductHelper.RemoveEffectiveActivityProductId(text);
				if (string.IsNullOrWhiteSpace(text))
				{
					throw new HidistroAshxException("选中的商品都在参加活动不能被入库");
				}
				num2 = ProductHelper.InStock(text);
				break;
			default:
				throw new HidistroAshxException("错误的状态参数");
			}
			if (num2 > 0)
			{
				base.ReturnSuccessResult(context, "操作商品成功", 0, true);
				return;
			}
			throw new HidistroAshxException("操作商品失败");
		}
	}
}
