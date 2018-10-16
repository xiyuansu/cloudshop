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
	[PrivilegeCheck(Privilege.ProductUnclassified)]
	public class ProductUnclassified : AdminBaseHandler
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
			case "addtocategorie":
				this.AddToCategorie(context);
				break;
			case "movecategory":
				this.MoveCategory(context);
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
			productQuery.SaleStatus = ProductSaleStatus.OnSale;
			if (productQuery.CategoryId.HasValue && productQuery.CategoryId.Value > 0)
			{
				CategoryInfo category = CatalogHelper.GetCategory(productQuery.CategoryId.Value);
				productQuery.MaiCategoryPath = ((category == null) ? "" : category.Path);
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
				DbQueryResult products = ProductHelper.GetProducts(query);
				dataGridViewModel.rows = DataHelper.DataTableToDictionary(products.Data);
				dataGridViewModel.total = products.TotalRecords;
				string text = "";
				string text2 = "<samp class=\"extend nocontent\" id=\"extend_{productId}_{index}\" productId=\"{productId}\" index=\"{index}\">{key}{del}</samp>";
				foreach (Dictionary<string, object> row in dataGridViewModel.rows)
				{
					int num = (int)row["ProductId"];
					object extendCategoryPath = row["ExtendCategoryPath"];
					object obj = row["CategoryId"];
					int num2 = 0;
					if (obj != null && obj != DBNull.Value)
					{
						row.Add("CategoryStr", CatalogHelper.GetFullCategory((int)obj));
					}
					text = this.GetExtendCategoryPath(extendCategoryPath);
					if (!string.IsNullOrEmpty(text))
					{
						row.Add("ExtendCategoryPathStr", text2.Replace("{key}", text).Replace("nocontent", "").Replace("{index}", "1")
							.Replace("{del}", "<i></i>")
							.Replace("{productId}", num.ToString()));
					}
					else
					{
						num2 = 1;
					}
					object extendCategoryPath2 = row["ExtendCategoryPath1"];
					text = this.GetExtendCategoryPath(extendCategoryPath2);
					if (!string.IsNullOrEmpty(text))
					{
						row.Add("ExtendCategoryPathStr1", text2.Replace("{key}", text).Replace("nocontent", "").Replace("{index}", "2")
							.Replace("{del}", "<i></i>")
							.Replace("{productId}", num.ToString()));
					}
					else if (num2 == 0)
					{
						num2 = 2;
					}
					object extendCategoryPath3 = row["ExtendCategoryPath2"];
					text = this.GetExtendCategoryPath(extendCategoryPath3);
					if (!string.IsNullOrEmpty(text))
					{
						row.Add("ExtendCategoryPathStr2", text2.Replace("{key}", text).Replace("nocontent", "").Replace("{index}", "3")
							.Replace("{del}", "<i></i>")
							.Replace("{productId}", num.ToString()));
					}
					else if (num2 == 0)
					{
						num2 = 3;
					}
					object extendCategoryPath4 = row["ExtendCategoryPath3"];
					text = this.GetExtendCategoryPath(extendCategoryPath4);
					if (!string.IsNullOrEmpty(text))
					{
						row.Add("ExtendCategoryPathStr3", text2.Replace("{key}", text).Replace("nocontent", "").Replace("{index}", "4")
							.Replace("{del}", "<i></i>")
							.Replace("{productId}", num.ToString()));
					}
					else if (num2 == 0)
					{
						num2 = 4;
					}
					object extendCategoryPath5 = row["ExtendCategoryPath4"];
					text = this.GetExtendCategoryPath(extendCategoryPath5);
					if (!string.IsNullOrEmpty(text))
					{
						row.Add("ExtendCategoryPathStr4", text2.Replace("{key}", text).Replace("nocontent", "").Replace("{index}", "5")
							.Replace("{del}", "<i></i>")
							.Replace("{productId}", num.ToString()));
					}
					else if (num2 == 0)
					{
						num2 = 5;
					}
					if (string.IsNullOrEmpty(row["ThumbnailUrl40"].ToNullString()))
					{
						row["ThumbnailUrl40"] = HiContext.Current.SiteSettings.DefaultProductImage;
					}
					row.Add("selectedExtIndex", num2);
				}
			}
			return dataGridViewModel;
		}

		private string GetExtendCategoryPath(object extendCategoryPath)
		{
			string result = "";
			if (extendCategoryPath != null && extendCategoryPath != DBNull.Value)
			{
				string text = (string)extendCategoryPath;
				if (text.Length > 0)
				{
					text = text.Substring(0, text.Length - 1);
					if (text.Contains("|"))
					{
						text = text.Substring(text.LastIndexOf('|') + 1);
					}
					result = CatalogHelper.GetFullCategory(int.Parse(text));
				}
			}
			return result;
		}

		private void Delete(HttpContext context)
		{
			string text = context.Request["ids"];
			if (text.Length < 0)
			{
				throw new HidistroAshxException("错误的参数！");
			}
			text = ProductHelper.RemoveEffectiveActivityProductId_PreSale(text);
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

		private void AddToCategorie(HttpContext context)
		{
			string text = context.Request["ids"];
			if (text.Length < 0)
			{
				throw new HidistroAshxException("错误的参数！");
			}
			int num = base.GetIntParam(context, "dropIndex", false).Value;
			int value = base.GetIntParam(context, "dropCategories", false).Value;
			if (num < 1 || num > 5)
			{
				num = 1;
			}
			string[] array = text.Contains(",") ? text.Split(',') : new string[1]
			{
				text
			};
			bool flag = false;
			string[] array2 = array;
			foreach (string obj in array2)
			{
				if (value > 0)
				{
					CatalogHelper.SetProductExtendCategory(obj.ToInt(0), CatalogHelper.GetCategory(value).Path + "|", num);
					flag = true;
				}
				else
				{
					CatalogHelper.SetProductExtendCategory(obj.ToInt(0), null, num);
					flag = false;
				}
			}
			if (flag)
			{
				base.ReturnSuccessResult(context, "添加成功！", 0, true);
				return;
			}
			throw new HidistroAshxException("错误的参数");
		}

		private void MoveCategory(HttpContext context)
		{
			int value = base.GetIntParam(context, "Categories", false).Value;
			string text = context.Request["ids"];
			if (text.Length < 0)
			{
				throw new HidistroAshxException("错误的参数！");
			}
			string[] array = text.Contains(",") ? text.Split(',') : new string[1]
			{
				text
			};
			string[] array2 = array;
			foreach (string value2 in array2)
			{
				bool flag = ProductHelper.UpdateProductCategory(Convert.ToInt32(value2), value);
			}
			base.ReturnSuccessResult(context, "转移商品类型成功", value, true);
		}
	}
}
