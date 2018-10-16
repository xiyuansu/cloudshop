using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.UI.Web.Admin.vshop.ashx
{
	public class ManageMenu : AdminBaseHandler
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
			case "sort":
				this.Sort(context);
				break;
			default:
				throw new HidistroAshxException("错误的参数");
			}
		}

		public void GetList(HttpContext context)
		{
			ClientType value = (ClientType)base.GetIntParam(context, "ClientType", false).Value;
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList(value);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList(ClientType clientType)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			IList<MenuInfo> menus = VShopHelper.GetMenus(clientType);
			dataGridViewModel.rows = new List<Dictionary<string, object>>();
			dataGridViewModel.total = menus.Count;
			foreach (MenuInfo item in menus)
			{
				Dictionary<string, object> dictionary = item.ToDictionary();
				dictionary.Add("IsHttpStart", item.Url.StartsWith("http"));
				dataGridViewModel.rows.Add(dictionary);
			}
			return dataGridViewModel;
		}

		public void Delete(HttpContext context)
		{
			int? intParam = base.GetIntParam(context, "id", false);
			if (!intParam.HasValue)
			{
				throw new HidistroAshxException("错误的编号");
			}
			if (base.CurrentSiteSetting.IsDemoSite)
			{
				throw new HidistroAshxException("演示站点不允许删除微信自定义菜单");
			}
			if (VShopHelper.DeleteMenu(intParam.Value))
			{
				base.ReturnSuccessResult(context, "成功删除了指定的菜单", 0, true);
				return;
			}
			throw new HidistroAshxException("删除失败!");
		}

		public void Sort(HttpContext context)
		{
			int? intParam = base.GetIntParam(context, "id", true);
			if (!intParam.HasValue)
			{
				throw new HidistroAshxException("错误的编号");
			}
			int value = base.GetIntParam(context, "sort", false).Value;
			VShopHelper.SwapMenuSequence(intParam.Value, value);
			base.ReturnSuccessResult(context, "操作成功", 0, true);
		}
	}
}
