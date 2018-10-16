using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.UI.Web.Admin.store.ashx
{
	public class Managers : AdminBaseHandler
	{
		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			if (string.IsNullOrWhiteSpace(base.action))
			{
				throw new HidistroAshxException("错误的参数");
			}
			string action = base.action;
			if (!(action == "getlist"))
			{
				if (action == "delete")
				{
					this.Delete(context);
					return;
				}
				throw new HidistroAshxException("错误的参数");
			}
			this.GetList(context);
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
			ManagerQuery managerQuery = new ManagerQuery();
			managerQuery.UserName = context.Request["UserName"];
			if (!string.IsNullOrEmpty(context.Request["RoleId"]))
			{
				managerQuery.RoleId = base.GetIntParam(context, "RoleId", false).Value;
			}
			managerQuery.SortBy = "CreateDate";
			managerQuery.SortOrder = SortAction.Desc;
			managerQuery.PageIndex = num;
			managerQuery.PageSize = num2;
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList(managerQuery);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList(ManagerQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			if (query != null)
			{
				DbQueryResult managers = ManagerHelper.GetManagers(query);
				dataGridViewModel.rows = DataHelper.DataTableToDictionary(managers.Data);
				dataGridViewModel.total = managers.TotalRecords;
				foreach (Dictionary<string, object> row in dataGridViewModel.rows)
				{
				}
			}
			return dataGridViewModel;
		}

		private void Delete(HttpContext context)
		{
			int value = base.GetIntParam(context, "managerId", false).Value;
			if (value <= 0)
			{
				throw new HidistroAshxException("错误的参数");
			}
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (HiContext.Current.Manager.ManagerId == value)
			{
				throw new HidistroAshxException("不能删除自己");
			}
			ManagerInfo manager = Users.GetManager(value);
			if (manager != null && manager.UserName.ToLower() == "admin" && masterSettings.IsDemoSite)
			{
				throw new HidistroAshxException("演示站点，不能删除超级管理员账号");
			}
			if (!ManagerHelper.Delete(manager.ManagerId))
			{
				throw new HidistroAshxException("未知错误");
			}
			base.ReturnSuccessResult(context, "成功删除了一个管理员", 0, true);
		}
	}
}
