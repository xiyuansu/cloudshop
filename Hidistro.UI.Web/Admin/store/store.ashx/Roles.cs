using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hidistro.UI.Web.Admin.store.ashx
{
	public class Roles : AdminBaseHandler
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
			case "addandupdate":
				this.AddAndUpdate(context);
				break;
			default:
				throw new HidistroAshxException("错误的参数");
			}
		}

		private void GetList(HttpContext context)
		{
			DataGridViewModel<RoleInfo> dataList = this.GetDataList();
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<RoleInfo> GetDataList()
		{
			DataGridViewModel<RoleInfo> dataGridViewModel = new DataGridViewModel<RoleInfo>();
			IEnumerable<RoleInfo> source = from item in ManagerHelper.GetRoles()
			where item.RoleId != -1 && item.RoleId != 0 && item.RoleId != -2 && item.RoleId != -3
			select item;
			dataGridViewModel.rows = source.ToList();
			dataGridViewModel.total = source.Count();
			return dataGridViewModel;
		}

		private void Delete(HttpContext context)
		{
			int value = base.GetIntParam(context, "roleId", false).Value;
			if (value <= 0)
			{
				throw new HidistroAshxException("错误的参数");
			}
			try
			{
				if (ManagerHelper.GetManagers(new ManagerQuery
				{
					RoleId = value
				}).TotalRecords > 0)
				{
					throw new HidistroAshxException("删除失败，该部门下已有管理员！");
				}
				ManagerHelper.DeleteRole(value);
				base.ReturnSuccessResult(context, "成功删除了选择的部门！", 0, true);
			}
			catch
			{
				throw new HidistroAshxException("删除失败，该部门下已有管理员！");
			}
		}

		private void AddAndUpdate(HttpContext context)
		{
			int value = base.GetIntParam(context, "RoleId", false).Value;
			string parameter = base.GetParameter(context, "RoleName", false);
			string parameter2 = base.GetParameter(context, "RoleDesc", false);
			if (string.IsNullOrEmpty(parameter) || parameter.Length > 60)
			{
				throw new HidistroAshxException("部门名称不能为空，长度限制在60个字符以内！");
			}
			RoleInfo roleInfo = new RoleInfo();
			if (value > 0)
			{
				roleInfo.RoleId = value;
				roleInfo.RoleName = Globals.HtmlEncode(parameter).Replace(",", "");
				roleInfo.Description = Globals.HtmlEncode(parameter2);
				ManagerHelper.UpdateRole(roleInfo);
				base.ReturnSuccessResult(context, "修改部门成功！", 0, true);
			}
			else
			{
				roleInfo.RoleName = Globals.HtmlEncode(parameter).Replace(",", "");
				roleInfo.Description = Globals.HtmlEncode(parameter2);
				ManagerHelper.AddRole(roleInfo);
				base.ReturnSuccessResult(context, "成功添加了一个部门！", 0, true);
			}
		}
	}
}
