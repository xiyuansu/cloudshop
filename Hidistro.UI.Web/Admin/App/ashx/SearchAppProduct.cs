using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.UI.Web.Admin.App.ashx
{
	public class SearchAppProduct : AdminBaseHandler
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
				if (action == "addproduct")
				{
					this.AddProduct(context);
					return;
				}
				throw new HidistroAshxException("错误的参数");
			}
			this.GetList(context);
		}

		private void GetList(HttpContext context)
		{
			int num = 1;
			int num2 = 10;
			int num3 = 0;
			string empty = string.Empty;
			ProductQuery productQuery = new ProductQuery();
			empty = context.Request["CategoryId"];
			if (!string.IsNullOrEmpty(empty))
			{
				num3 = empty.ToInt(0);
				productQuery.CategoryId = num3;
				productQuery.MaiCategoryPath = CatalogHelper.GetCategory(num3).Path;
			}
			empty = context.Request["BrandId"];
			if (!string.IsNullOrEmpty(context.Request["BrandId"]))
			{
				productQuery.BrandId = empty.ToInt(0);
			}
			productQuery.SaleStatus = ProductSaleStatus.OnSale;
			productQuery.IsIncludeHomeProduct = false;
			productQuery.Keywords = context.Request["ProductName"];
			productQuery.Client = 3;
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
					ProductInfo productInfo = row.ToObject<ProductInfo>();
					if (string.IsNullOrEmpty(productInfo.ThumbnailUrl40))
					{
						row["ThumbnailUrl40"] = base.CurrentSiteSetting.DefaultProductThumbnail1;
					}
				}
			}
			return dataGridViewModel;
		}

		private void AddProduct(HttpContext context)
		{
			string text = context.Request["ids"];
			IList<int> list = new List<int>();
			if (text.Length < 0)
			{
				throw new HidistroAshxException("错误的参数！");
			}
			string[] array = text.Split(',');
			foreach (string text2 in array)
			{
				if (!string.IsNullOrEmpty(text2))
				{
					list.Add(text2.ToInt(0));
				}
			}
			int num = 0;
			string[] array2 = text.Split(',');
			foreach (string obj in array2)
			{
				HomeProductInfo info = new HomeProductInfo
				{
					ProductId = obj.ToInt(0)
				};
				if (VShopHelper.AddHomeProdcut(info))
				{
					num++;
				}
			}
			if (num > 0)
			{
				base.ReturnSuccessResult(context, "AppProductSetting.aspx", 0, true);
				return;
			}
			throw new HidistroAshxException("选择首页商品失败！");
		}
	}
}
