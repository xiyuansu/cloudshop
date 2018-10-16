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
	public class ProductOnSales : AdminBaseHandler
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
			case "updateCrossborder":
				this.UpdateCrossborder(context);
				break;
			case "dowdPdInStore":
				this.DowdPdInStore(context);
				break;
			default:
				throw new HidistroAshxException("错误的参数");
			}
		}

		private void DowdPdInStore(HttpContext context)
		{
			int num = context.Request["id"].ToInt(0);
			if (num > 0)
			{
				base.ReturnSuccessResult(context, ProductHelper.SaleOffFromStore(num) ? "成功从门店下架！" : "操作失败，请重试！", 0, true);
			}
			else
			{
				base.ReturnSuccessResult(context, "参数错误", 0, true);
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
			productQuery.Keywords = base.GetParameter(context, "productName", true);
			productQuery.ProductCode = base.GetParameter(context, "productCode", true);
			productQuery.CategoryId = base.GetIntParam(context, "categoryId", true);
			productQuery.ShippingTemplateId = base.GetIntParam(context, "ShippingTemplateId", true);
			productQuery.ProductType = base.GetIntParam(context, "ProductType", true).ToInt(0);
			empty = context.Request["SaleStatus"];
			if (!string.IsNullOrEmpty(empty))
			{
				productQuery.SaleStatus = (ProductSaleStatus)Enum.Parse(typeof(ProductSaleStatus), empty);
			}
			else
			{
				productQuery.SaleStatus = ProductSaleStatus.All;
			}
			if (productQuery.CategoryId.HasValue)
			{
				productQuery.MaiCategoryPath = CatalogHelper.GetCategory(productQuery.CategoryId.Value).Path;
			}
			if (context.Request["isWarning"] == "1")
			{
				productQuery.IsWarningStock = true;
			}
			productQuery.BrandId = base.GetIntParam(context, "BrandId", true);
			productQuery.TagId = base.GetIntParam(context, "TagId", true);
			productQuery.TypeId = base.GetIntParam(context, "TypeId", true);
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
			productQuery.IsFilterStoreProducts = true;
			productQuery.SupplierId = 0;
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
					string text = row["ProductName"].ToString();
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
			int num = ProductHelper.RemoveProduct(text);
			if (num > 0)
			{
				base.ReturnSuccessResult(context, "成功删除了选择的商品！", 0, true);
				return;
			}
			throw new HidistroAshxException("错误的参数！");
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
			if (!(a == "OnSale"))
			{
				if (!(a == "UnSale"))
				{
					if (a == "OnStock")
					{
						text = ProductHelper.RemoveEffectiveActivityProductId(text);
						if (string.IsNullOrEmpty(text))
						{
							throw new HidistroAshxException("选中的商品都在参加活动不能被下架！");
						}
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
				int num2 = ProductHelper.OffShelf(text);
				if (num2 > 0)
				{
					base.ReturnSuccessResult(context, "操作成功，您可以在下架区的商品里面找到下架以后的商品！", 0, true);
					return;
				}
				throw new HidistroAshxException("下架商品失败，未知错误");
			}
			int num3 = ProductHelper.UpShelf(text);
			if (num3 > 0)
			{
				base.ReturnSuccessResult(context, "操作成功，您可以在出售中的商品里面找到上架以后的商品！", 0, true);
				return;
			}
			throw new HidistroAshxException("上架商品失败，未知错误");
		}

		private void UpdateCrossborder(HttpContext context)
		{
			string text = context.Request["ids"];
			bool flag = false;
			bool.TryParse(context.Request["Crossborderstatus"], out flag);
			if (text.Length < 0)
			{
				throw new HidistroAshxException("错误的参数！");
			}
			if (HiContext.Current.SiteSettings.IsOpenCertification)
			{
				int num = ProductHelper.UpdateCrossborder(text, flag);
				if (num > 0)
				{
					base.ReturnSuccessResult(context, flag ? "成功设置选择的商品为跨境！" : "成功取消了选择的商品跨境", 0, true);
				}
				else
				{
					base.ReturnFailResult(context, flag ? "设置商品跨境失败！" : "取消商品跨境失败", -1, true);
				}
			}
			else
			{
				base.ReturnFailResult(context, "请先到订单设置中开启跨境商品实名认证", -1, true);
			}
		}
	}
}
