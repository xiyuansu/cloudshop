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

namespace Hidistro.UI.Web.Admin.product.ashx
{
	[PrivilegeCheck(Privilege.Products)]
	public class ProductOnDeleted : AdminBaseHandler
	{
		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			if (string.IsNullOrWhiteSpace(base.action))
			{
				throw new HidistroAshxException("错误的参数");
			}
			switch (base.action)
			{
			case "getlist":
				this.GetList(context);
				break;
			case "delete":
				this.Delete(context);
				break;
			case "chagestatus":
				this.ChageStatus(context);
				break;
			default:
				throw new HidistroAshxException("错误的参数");
			}
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
			productQuery.SaleStatus = ProductSaleStatus.Delete;
			if (productQuery.CategoryId.HasValue)
			{
				productQuery.MaiCategoryPath = CatalogHelper.GetCategory(productQuery.CategoryId.Value).Path;
			}
			productQuery.BrandId = base.GetIntParam(context, "BrandId", true);
			productQuery.TagId = base.GetIntParam(context, "TagId", true);
			productQuery.TypeId = base.GetIntParam(context, "TypeId", true);
			productQuery.StartDate = base.GetDateTimeParam(context, "StartDate");
			productQuery.EndDate = base.GetDateTimeParam(context, "EndDate");
			productQuery.PageSize = base.CurrentPageSize;
			productQuery.PageIndex = base.CurrentPageIndex;
			productQuery.SortOrder = SortAction.Desc;
			productQuery.SortBy = "DisplaySequence";
			productQuery.IsFilterStoreProducts = true;
			Globals.EntityCoding(productQuery, true);
			return productQuery;
		}

		private DataGridViewModel<Dictionary<string, object>> GetProducts(ProductQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			if (query != null)
			{
				query.ProductType = (-1).GetHashCode();
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
					if (string.IsNullOrEmpty(row["ThumbnailUrl40"].ToNullString()))
					{
						row["ThumbnailUrl40"] = HiContext.Current.SiteSettings.DefaultProductImage;
					}
				}
			}
			return dataGridViewModel;
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
			string text2 = context.Request["PenetrationStatus"];
			int num = ProductHelper.DeleteProduct(text, text2.Equals("1") && true);
			if (num > 0)
			{
				int num2 = text.Split(',').Length;
				if (num2 == num)
				{
					base.ReturnSuccessResult(context, "成功删除了选择的商品！", 0, true);
				}
				else
				{
					base.ReturnSuccessResult(context, "成功删除了部分选择的商品,存在于未过售后期的订单中的商品无法删除！", 0, true);
				}
				return;
			}
			throw new HidistroAshxException("删除商品失败，如果商品存在于未过售后期的订单中将无法删除！");
		}

		private void ChageStatus(HttpContext context)
		{
			string text = context.Request["ids"];
			if (text.Length < 0)
			{
				throw new HidistroAshxException("错误的参数！");
			}
			string text2 = context.Request["SaleStatus"];
			string a = text2;
			if (!(a == "OffShelf"))
			{
				if (!(a == "UpShelf"))
				{
					if (a == "InStock")
					{
						int num = ProductHelper.InStock(text);
						if (num > 0)
						{
							base.ReturnSuccessResult(context, "操作成功，您可以在仓库区的商品里面找到入库以后的商品！", 0, true);
							return;
						}
						throw new HidistroAshxException("入库商品失败，未知错误！");
					}
					return;
				}
				text = ProductHelper.RemoveEffectiveActivityProductId(text);
				if (string.IsNullOrEmpty(text))
				{
					throw new HidistroAshxException("选中的商品都在参加活动不能被下架！");
				}
				int num2 = ProductHelper.UpShelf(text);
				if (num2 > 0)
				{
					base.ReturnSuccessResult(context, "操作成功，您可以在出售中的商品里面找到上架以后的商品", 0, true);
					return;
				}
				throw new HidistroAshxException("上架商品失败，未知错误");
			}
			int num3 = ProductHelper.OffShelf(text);
			if (num3 > 0)
			{
				base.ReturnSuccessResult(context, "操作成功，您可以在下架区的商品里面找到下架以后的商品！", 0, true);
				return;
			}
			throw new HidistroAshxException("下架商品失败，未知错误");
		}
	}
}
