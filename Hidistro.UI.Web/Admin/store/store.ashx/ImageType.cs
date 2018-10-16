using Hidistro.Core;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace Hidistro.UI.Web.Admin.store.ashx
{
	public class ImageType : AdminBaseHandler
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
			case "edit":
				this.Edit(context);
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

		public void GetList(HttpContext context)
		{
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList();
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList()
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			DataTable photoCategories = GalleryHelper.GetPhotoCategories(0);
			DataRow dataRow = photoCategories.NewRow();
			dataRow["CategoryId"] = "0";
			dataRow["CategoryName"] = "默认分类";
			dataRow["DisplaySequence"] = "0";
			dataRow["PhotoCounts"] = GalleryHelper.GetDefaultPhotoCount().ToString();
			photoCategories.Rows.InsertAt(dataRow, 0);
			dataGridViewModel.rows = new List<Dictionary<string, object>>();
			foreach (DataRow row in photoCategories.Rows)
			{
				Dictionary<string, object> item = DataHelper.DataRowToDictionary(row);
				dataGridViewModel.rows.Add(item);
			}
			dataGridViewModel.total = dataGridViewModel.rows.Count;
			return dataGridViewModel;
		}

		public void Edit(HttpContext context)
		{
			int value = base.GetIntParam(context, "id", false).Value;
			string parameter = base.GetParameter(context, "name", true);
			bool flag = true;
			if (value > 0)
			{
				flag = false;
			}
			if (string.IsNullOrWhiteSpace(parameter))
			{
				throw new HidistroAshxException("分类名称不能为空");
			}
			if (parameter.Length > 20)
			{
				throw new HidistroAshxException("分类名称长度限在20个字符以内");
			}
			if (flag)
			{
				if (GalleryHelper.AddPhotoCategory(Globals.HtmlEncode(parameter), 0) > 0)
				{
					base.ReturnSuccessResult(context, "添加成功", 0, true);
					return;
				}
				throw new HidistroAshxException("操作失败");
			}
			Dictionary<int, string> dictionary = new Dictionary<int, string>();
			dictionary.Add(value, parameter);
			if (GalleryHelper.UpdatePhotoCategories(dictionary) > 0)
			{
				base.ReturnSuccessResult(context, "修改成功", 0, true);
				return;
			}
			throw new HidistroAshxException("操作失败");
		}

		public void Delete(HttpContext context)
		{
			int value = base.GetIntParam(context, "id", false).Value;
			if (value < 1)
			{
				throw new HidistroAshxException("错误的参数");
			}
			if (GalleryHelper.DeletePhotoCategory(value, false))
			{
				base.ReturnSuccessResult(context, "删除成功", 0, true);
				return;
			}
			throw new HidistroAshxException("操作失败");
		}

		public void Sort(HttpContext context)
		{
			int value = base.GetIntParam(context, "id", false).Value;
			int value2 = base.GetIntParam(context, "sort", false).Value;
			if (value < 1)
			{
				throw new HidistroAshxException("错误的参数");
			}
			if (GalleryHelper.SwapSequence(value, value2))
			{
				base.ReturnSuccessResult(context, "更新排序成功", 0, true);
				return;
			}
			throw new HidistroAshxException("操作失败");
		}
	}
}
