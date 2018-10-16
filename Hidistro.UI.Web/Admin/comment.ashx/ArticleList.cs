using Hidistro.Context;
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
	public class ArticleList : AdminBaseHandler
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
			case "release":
				this.Release(context);
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
			ArticleQuery articleQuery = new ArticleQuery();
			articleQuery.Keywords = context.Request["Keywords"];
			if (!string.IsNullOrEmpty(context.Request["CategoryId"]))
			{
				articleQuery.CategoryId = base.GetIntParam(context, "CategoryId", false).Value;
			}
			articleQuery.StartArticleTime = base.GetDateTimeParam(context, "FromDate");
			articleQuery.EndArticleTime = base.GetDateTimeParam(context, "ToDate");
			articleQuery.SortOrder = SortAction.Desc;
			articleQuery.PageIndex = num;
			articleQuery.PageSize = num2;
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList(articleQuery);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList(ArticleQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			if (query != null)
			{
				DbQueryResult articleList = ArticleHelper.GetArticleList(query);
				dataGridViewModel.rows = DataHelper.DataTableToDictionary(articleList.Data);
				dataGridViewModel.total = articleList.TotalRecords;
				foreach (Dictionary<string, object> row in dataGridViewModel.rows)
				{
					if (string.IsNullOrEmpty(row["IconUrl"].ToString()))
					{
						row.Add("IconUrls", HiContext.Current.SiteSettings.DefaultProductImage);
					}
					else
					{
						row.Add("IconUrls", row["IconUrl"]);
					}
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
			if (ArticleHelper.DeleteArticle(value))
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
			int num = ArticleHelper.DeleteArticles(list);
			if (num > 0)
			{
				base.ReturnSuccessResult(context, $"成功删除{num}篇文章", 0, true);
				return;
			}
			throw new HidistroAshxException("删除失败！");
		}

		private void Release(HttpContext context)
		{
			bool value = base.GetBoolParam(context, "IsRelease", false).Value;
			int value2 = base.GetIntParam(context, "ArticleId", false).Value;
			bool isRelease = false;
			string arg = "取消";
			if (!value)
			{
				isRelease = true;
				arg = "发布";
			}
			if (ArticleHelper.UpdateRelease(value2, isRelease))
			{
				base.ReturnSuccessResult(context, $"{arg}当前文章成功", 0, true);
				return;
			}
			throw new HidistroAshxException($"{arg}当前文章失败");
		}
	}
}
