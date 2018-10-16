using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.Entities.WeChatApplet;
using Hidistro.SaleSystem.Store;
using Hidistro.SaleSystem.WeChartApplet;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.UI.Web.Admin.Applet.ashx
{
	[PrivilegeCheck(Privilege.AppletProductSetting)]
	public class AppletSetting : AdminBaseHandler
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
			DbQueryResult showProductList = WeChartAppletHelper.GetShowProductList(0, num, num2, 0, ProductType.PhysicalProduct);
			dataGridViewModel.rows = DataHelper.DataTableToDictionary(showProductList.Data);
			dataGridViewModel.total = showProductList.TotalRecords;
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
				if (string.IsNullOrEmpty(productInfo.ThumbnailUrl160))
				{
					row["ThumbnailUrl160"] = base.CurrentSiteSetting.DefaultProductThumbnail1;
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
			if (WeChartAppletHelper.RemoveChoiceProduct(text, 0))
			{
				base.ReturnSuccessResult(context, "删除成功！", 0, true);
				return;
			}
			throw new HidistroAshxException("请选择要删除的商品！");
		}

		private void SaveOrder(HttpContext context)
		{
			int value = base.GetIntParam(context, "Value", false).Value;
			if (value >= 0)
			{
				int value2 = base.GetIntParam(context, "ProductId", false).Value;
				AppletChoiceProductInfo info = new AppletChoiceProductInfo
				{
					ProductId = value2,
					StoreId = 0,
					DisplaySequence = value
				};
				if (WeChartAppletHelper.UpdateChoiceProductSequence(info))
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
			if (text.Length < 0)
			{
				throw new HidistroAshxException("错误的参数！");
			}
			if (WeChartAppletHelper.AddChoiceProdcut(text, 0))
			{
				base.ReturnSuccessResult(context, "添加成功！", 0, true);
				return;
			}
			throw new HidistroAshxException("选择首页商品失败！");
		}
	}
}
