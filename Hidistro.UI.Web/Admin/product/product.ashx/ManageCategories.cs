using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.UI.Web.Admin.product.ashx
{
	[PrivilegeCheck(Privilege.ProductCategory)]
	public class ManageCategories : AdminBaseHandler
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
			case "setorder":
				this.SetOrder(context);
				break;
			default:
				throw new HidistroAshxException("错误的参数");
			}
		}

		private void GetList(HttpContext context)
		{
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList();
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList()
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			IList<CategoryInfo> sequenceCategories = CatalogHelper.GetSequenceCategories("");
			dataGridViewModel.rows = new List<Dictionary<string, object>>();
			dataGridViewModel.total = sequenceCategories.Count;
			foreach (CategoryInfo item in sequenceCategories)
			{
				Dictionary<string, object> dictionary = item.ToDictionary();
				int depth = item.Depth;
				string text = (string)(dictionary["Name"] = item.Name);
				dataGridViewModel.rows.Add(dictionary);
			}
			return dataGridViewModel;
		}

		private void Delete(HttpContext context)
		{
			int value = base.GetIntParam(context, "ids", false).Value;
			if (CatalogHelper.DeleteCategory(value))
			{
				base.ReturnSuccessResult(context, "删除成功！", 0, true);
				return;
			}
			throw new HidistroAshxException("分类删除失败，未知错误！");
		}

		private void SetOrder(HttpContext context)
		{
			int value = base.GetIntParam(context, "CategoryId", false).Value;
			int value2 = base.GetIntParam(context, "DisplaySequence", false).Value;
			CategoryInfo category = CatalogHelper.GetCategory(value);
			if (category.DisplaySequence == value2)
			{
				return;
			}
			if (CatalogHelper.SwapCategorySequence(value, value2))
			{
				HiCache.Remove("DataCache-Categories");
				base.ReturnSuccessResult(context, "", 0, true);
				return;
			}
			throw new HidistroAshxException("参数错误！");
		}
	}
}
