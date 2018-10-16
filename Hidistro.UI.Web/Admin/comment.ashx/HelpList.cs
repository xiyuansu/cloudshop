using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Comments;
using Hidistro.SaleSystem.Comments;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.UI.Web.Admin.comment.ashx
{
	public class HelpList : AdminBaseHandler
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
			case "deletes":
				this.Deletes(context);
				break;
			default:
				throw new HidistroAshxException("错误的参数");
			}
		}

		private void GetList(HttpContext context)
		{
			int num = 1;
			int num2 = 10;
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
			HelpQuery helpQuery = new HelpQuery();
			helpQuery.Keywords = context.Request["Keywords"];
			if (!string.IsNullOrEmpty(context.Request["CategoryId"]))
			{
				helpQuery.CategoryId = base.GetIntParam(context, "CategoryId", false).Value;
			}
			helpQuery.StartArticleTime = base.GetDateTimeParam(context, "FromDate");
			helpQuery.EndArticleTime = base.GetDateTimeParam(context, "ToDate");
			helpQuery.SortBy = "AddedDate";
			helpQuery.SortOrder = SortAction.Desc;
			helpQuery.PageIndex = num;
			helpQuery.PageSize = num2;
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList(helpQuery);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList(HelpQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			if (query != null)
			{
				DbQueryResult helpList = ArticleHelper.GetHelpList(query);
				dataGridViewModel.rows = DataHelper.DataTableToDictionary(helpList.Data);
				dataGridViewModel.total = helpList.TotalRecords;
				foreach (Dictionary<string, object> row in dataGridViewModel.rows)
				{
				}
			}
			return dataGridViewModel;
		}

		private void Delete(HttpContext context)
		{
			int value = base.GetIntParam(context, "articleId", false).Value;
			if (value <= 0)
			{
				throw new HidistroAshxException("错误的参数");
			}
			if (ArticleHelper.DeleteHelp(value))
			{
				base.ReturnSuccessResult(context, "删除成功！", 0, true);
				return;
			}
			throw new HidistroAshxException("删除失败！");
		}

		private void Deletes(HttpContext context)
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
			int num = ArticleHelper.DeleteHelps(list);
			if (num > 0)
			{
				base.ReturnSuccessResult(context, $"成功删除{num}篇帮助", 0, true);
				return;
			}
			throw new HidistroAshxException("删除失败！");
		}
	}
}
