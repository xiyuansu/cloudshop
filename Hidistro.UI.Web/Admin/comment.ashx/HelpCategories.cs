using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Comments;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Web.ashxBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hidistro.UI.Web.Admin.comment.ashx
{
	[PrivilegeCheck(Privilege.ArticleCategories)]
	public class HelpCategories : AdminBaseHandler
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
			case "isshow":
				this.IsShow(context);
				break;
			default:
				throw new HidistroAshxException("错误的参数");
			}
		}

		private void GetList(HttpContext context)
		{
			DataGridViewModel<HelpCategoryInfo> dataList = this.GetDataList();
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<HelpCategoryInfo> GetDataList()
		{
			DataGridViewModel<HelpCategoryInfo> dataGridViewModel = new DataGridViewModel<HelpCategoryInfo>();
			IList<HelpCategoryInfo> helpCategorys = ArticleHelper.GetHelpCategorys();
			dataGridViewModel.rows = helpCategorys.ToList();
			dataGridViewModel.total = helpCategorys.Count;
			foreach (HelpCategoryInfo row in dataGridViewModel.rows)
			{
				if (string.IsNullOrEmpty(row.IconUrl))
				{
					row.IconUrl = HiContext.Current.SiteSettings.DefaultProductImage;
				}
			}
			return dataGridViewModel;
		}

		private void Delete(HttpContext context)
		{
			int categoryId = context.Request["ids"].ToInt(0);
			if (ArticleHelper.DeleteHelpCategory(categoryId))
			{
				base.ReturnSuccessResult(context, "成功删除分类！", 0, true);
			}
		}

		private void SaveOrder(HttpContext context)
		{
			int value = base.GetIntParam(context, "Value", false).Value;
			if (value > 0)
			{
				int value2 = base.GetIntParam(context, "CategroyId", false).Value;
				try
				{
					ArticleHelper.SwapHelpCategorySequence(value2, value);
					base.ReturnSuccessResult(context, "保存排序成功！", 0, true);
				}
				catch (Exception)
				{
					throw new HidistroAshxException("修改排序失败！未知错误！");
				}
				return;
			}
			throw new HidistroAshxException("错误的参数");
		}

		private void IsShow(HttpContext context)
		{
			int value = base.GetIntParam(context, "CategoryId", false).Value;
			HelpCategoryInfo helpCategory = ArticleHelper.GetHelpCategory(value);
			if (helpCategory.IsShowFooter)
			{
				helpCategory.IsShowFooter = false;
			}
			else
			{
				helpCategory.IsShowFooter = true;
			}
			ArticleHelper.UpdateHelpCategory(helpCategory);
			base.ReturnSuccessResult(context, "", 0, true);
		}
	}
}
