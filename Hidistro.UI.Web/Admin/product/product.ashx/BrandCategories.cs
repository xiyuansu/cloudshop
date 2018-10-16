using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Web.ashxBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace Hidistro.UI.Web.Admin.product.ashx
{
	[PrivilegeCheck(Privilege.BrandCategories)]
	public class BrandCategories : AdminBaseHandler
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
			case "sort":
				this.Sort(context);
				break;
			default:
				throw new HidistroAshxException("错误的参数");
			}
		}

		private void GetList(HttpContext context)
		{
			string brandname = context.Request["Keywords"];
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList(brandname);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList(string brandname)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			DataTable brandCategories = CatalogHelper.GetBrandCategories(brandname);
			int num = (base.CurrentPageIndex - 1) * base.CurrentPageSize;
			int num2 = num + base.CurrentPageSize;
			DataTable dataTable = brandCategories.Copy();
			dataTable.Clear();
			for (int i = 0; i < brandCategories.Rows.Count; i++)
			{
				if (i >= num && i < num2)
				{
					DataRow dataRow = dataTable.NewRow();
					dataRow.ItemArray = brandCategories.Rows[i].ItemArray;
					dataTable.Rows.Add(dataRow);
				}
			}
			dataGridViewModel.rows = DataHelper.DataTableToDictionary(dataTable);
			dataGridViewModel.total = brandCategories.Rows.Count;
			foreach (Dictionary<string, object> row in dataGridViewModel.rows)
			{
			}
			return dataGridViewModel;
		}

		private void Delete(HttpContext context)
		{
			int value = base.GetIntParam(context, "BrandId", false).Value;
			if (CatalogHelper.BrandHvaeProducts(value))
			{
				throw new HidistroAshxException("选择的品牌分类下还有商品，删除失败");
			}
			if (CatalogHelper.DeleteBrandCategory(value))
			{
				base.ReturnSuccessResult(context, "成功删除品牌分类！", 0, true);
				return;
			}
			throw new HidistroAshxException("删除品牌分类失败");
		}

		private void Sort(HttpContext context)
		{
			int value = base.GetIntParam(context, "id", false).Value;
			int value2 = base.GetIntParam(context, "sort", false).Value;
			if (value < 1)
			{
				throw new HidistroAshxException("错误的参数");
			}
			try
			{
				if (CatalogHelper.UpdateBrandCategoryDisplaySequence(value, value2))
				{
					base.ReturnSuccessResult(context, "批量更新排序成功！", 0, true);
					goto end_IL_0042;
				}
				throw new HidistroAshxException("批量更新排序失败！");
				end_IL_0042:;
			}
			catch (Exception ex)
			{
				throw new HidistroAshxException("批量更新排序失败！" + ex.Message);
			}
		}
	}
}
