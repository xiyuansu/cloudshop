using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Web.ashxBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hidistro.UI.Web.Admin.store.ashx
{
	public class OnlineService : AdminBaseHandler
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
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList();
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList()
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			IList<OnlineServiceInfo> allOnlineService = OnlineServiceHelper.GetAllOnlineService(0, -1);
			dataGridViewModel.rows = new List<Dictionary<string, object>>();
			foreach (OnlineServiceInfo item in allOnlineService)
			{
				Dictionary<string, object> dictionary = item.ToDictionary();
				dictionary.Add("ServiceTypeName", EnumDescription.GetEnumDescription((Enum)(object)(OnlineServiceTypes)item.ServiceType, 0));
				dataGridViewModel.rows.Add(dictionary);
			}
			dataGridViewModel.total = dataGridViewModel.rows.Count;
			return dataGridViewModel;
		}

		public void Delete(HttpContext context)
		{
			string parameter = base.GetParameter(context, "ids", true);
			if (string.IsNullOrWhiteSpace(parameter))
			{
				throw new HidistroAshxException("错误的参数");
			}
			List<int> list = (from d in parameter.Split(',')
			where !string.IsNullOrWhiteSpace(d)
			select int.Parse(d)).ToList();
			foreach (int item in list)
			{
				OnlineServiceHelper.Delete(item);
			}
			base.ReturnSuccessResult(context, "删除成功", 0, true);
		}

		public void Sort(HttpContext context)
		{
			int value = base.GetIntParam(context, "id", false).Value;
			int value2 = base.GetIntParam(context, "sort", false).Value;
			if (value < 1)
			{
				throw new HidistroAshxException("错误的参数");
			}
			if (OnlineServiceHelper.UpdateDisplaySequence(value, value2))
			{
				base.ReturnSuccessResult(context, "更新排序成功", 0, true);
				return;
			}
			throw new HidistroAshxException("操作失败");
		}
	}
}
