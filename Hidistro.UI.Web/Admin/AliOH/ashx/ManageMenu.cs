using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Store;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Comments;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Web.ashxBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hidistro.UI.Web.Admin.AliOH.ashx
{
	[PrivilegeCheck(Privilege.AliohManageMenu)]
	public class ManageMenu : AdminBaseHandler
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
			default:
				throw new HidistroAshxException("错误的参数");
			}
		}

		private void GetList(HttpContext context)
		{
			DataGridViewModel<MenuInfo> dataList = this.GetDataList(ClientType.AliOH);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<MenuInfo> GetDataList(ClientType type)
		{
			DataGridViewModel<MenuInfo> dataGridViewModel = new DataGridViewModel<MenuInfo>();
			bool flag = true;
			IList<MenuInfo> menus = VShopHelper.GetMenus(type);
			dataGridViewModel.rows = menus.ToList();
			dataGridViewModel.total = menus.Count;
			foreach (MenuInfo row in dataGridViewModel.rows)
			{
				string text = row.Name;
				if (row.ParentMenuId == 0)
				{
					text = "<b>" + text + "</b>";
				}
				if (row.ParentMenuId > 0)
				{
					text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + text;
				}
				row.Name = text;
				if (!row.Url.StartsWith("http"))
				{
					row.ulrs = "";
				}
				else
				{
					row.ulrs = row.Url;
				}
			}
			return dataGridViewModel;
		}

		private void Delete(HttpContext context)
		{
			int value = base.GetIntParam(context, "ids", false).Value;
			if (VShopHelper.DeleteMenu(value))
			{
				base.ReturnSuccessResult(context, "成功删除了指定的分类！", 0, true);
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

		private void SaveOrder(HttpContext context)
		{
			try
			{
				int value = base.GetIntParam(context, "MenuId", false).Value;
				int value2 = base.GetIntParam(context, "Value", false).Value;
				VShopHelper.SwapMenuSequence(value, value2);
				base.ReturnSuccessResult(context, "修改排序成功！", 0, true);
			}
			catch (Exception)
			{
				throw new HidistroAshxException("修改排序失败，未知错误！");
			}
		}
	}
}
