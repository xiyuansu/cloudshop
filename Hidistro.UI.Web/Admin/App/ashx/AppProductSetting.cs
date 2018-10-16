using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.UI.Web.Admin.App.ashx
{
	[PrivilegeCheck(Privilege.AppProductSetting)]
	public class AppProductSetting : AdminBaseHandler
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
			case "saveorder":
				this.SaveOrder(context);
				break;
			case "addproduct":
				this.AddProduct(context);
				break;
			default:
				throw new HidistroAshxException("错误的参数");
			}
		}

		private void GetList(HttpContext context)
		{
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList(context);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList(HttpContext context)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			ProductQuery productQuery = new ProductQuery();
			int num = 1;
			int num2 = 10;
			num = context.Request["page"].ToInt(0);
			if (num < 1)
			{
				num = 1;
			}
			num2 = context.Request["rows"].ToInt(0);
			if (num2 < 1)
			{
				num2 = 10;
			}
			productQuery.PageSize = num2;
			productQuery.PageIndex = num;
			productQuery.SortOrder = SortAction.Desc;
			productQuery.SortBy = "t.DisplaySequence";
			productQuery.StoreId = 0;
			productQuery.IsFilterStoreProducts = true;
			productQuery.SupplierId = 0;
			DbQueryResult homeProducts = VShopHelper.GetHomeProducts(productQuery);
			dataGridViewModel.rows = DataHelper.DataTableToDictionary(homeProducts.Data);
			dataGridViewModel.total = homeProducts.TotalRecords;
			foreach (Dictionary<string, object> row in dataGridViewModel.rows)
			{
				ProductInfo productInfo = row.ToObject<ProductInfo>();
				if (productInfo.ProductName.Length >= 26)
				{
					row.Add("SubProductName", productInfo.ProductName.Substring(0, 26) + "...");
				}
				else
				{
					row.Add("SubProductName", productInfo.ProductName);
				}
				if (string.IsNullOrEmpty(productInfo.ThumbnailUrl40))
				{
					row["ThumbnailUrl40"] = base.CurrentSiteSetting.DefaultProductThumbnail1;
				}
			}
			return dataGridViewModel;
		}

		private void Delete(HttpContext context)
		{
			string text = context.Request["ids"];
			IList<int> list = new List<int>();
			bool flag = false;
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
					flag = true;
				}
			}
			if (!flag)
			{
				throw new HidistroAshxException("请选择要删除的商品！");
			}
			list.ForEach(delegate(int c)
			{
				VShopHelper.RemoveHomeProduct(c);
			});
			base.ReturnSuccessResult(context, "删除成功！", 0, true);
		}

		private void SaveOrder(HttpContext context)
		{
			int value = base.GetIntParam(context, "Value", false).Value;
			if (value >= 0)
			{
				int value2 = base.GetIntParam(context, "ProductId", false).Value;
				HomeProductInfo info = new HomeProductInfo
				{
					ProductId = value2,
					DisplaySequence = value
				};
				if (VShopHelper.UpdateHomeProductSequence(info))
				{
					base.ReturnSuccessResult(context, "保存排序成功！", 0, true);
					return;
				}
				throw new HidistroAshxException("修改排序失败！未知错误！");
			}
			throw new HidistroAshxException("错误的参数");
		}

		private void AddProduct(HttpContext context)
		{
			string text = context.Request["ids"];
			int num = 0;
			string[] array = text.Split(',');
			foreach (string text2 in array)
			{
				if (!string.IsNullOrEmpty(text2))
				{
					HomeProductInfo info = new HomeProductInfo
					{
						ProductId = text2.ToInt(0)
					};
					if (VShopHelper.AddHomeProdcut(info))
					{
						num++;
					}
				}
			}
			if (num > 0)
			{
				base.ReturnSuccessResult(context, "添加成功！", 0, true);
				return;
			}
			throw new HidistroAshxException("选择首页商品失败！");
		}
	}
}
