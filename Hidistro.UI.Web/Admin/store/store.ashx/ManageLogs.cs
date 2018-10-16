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
	public class ManageLogs : AdminBaseHandler
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
			case "removeall":
				this.RemoveAll(context);
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
			OperationLogQuery operationLogQuery = new OperationLogQuery();
			operationLogQuery.OperationUserName = context.Request["OperationUserName"];
			operationLogQuery.FromDate = base.GetDateTimeParam(context, "FromDate");
			operationLogQuery.ToDate = base.GetDateTimeParam(context, "ToDate");
			if (operationLogQuery.ToDate.HasValue && operationLogQuery.ToDate.HasValue)
			{
				operationLogQuery.ToDate = (operationLogQuery.ToDate.Value.ToString("yyyy-MM-dd") + " 23:59:59").ToDateTime();
			}
			operationLogQuery.Page.SortBy = context.Request["SortBy"];
			operationLogQuery.Page.SortOrder = SortAction.Desc;
			operationLogQuery.Page.PageIndex = num;
			operationLogQuery.Page.PageSize = num2;
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList(operationLogQuery);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList(OperationLogQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			if (query != null)
			{
				DbQueryResult logs = EventLogs.GetLogs(query);
				dataGridViewModel.rows = DataHelper.DataTableToDictionary(logs.Data);
				dataGridViewModel.total = logs.TotalRecords;
				foreach (Dictionary<string, object> row in dataGridViewModel.rows)
				{
				}
			}
			return dataGridViewModel;
		}

		private void Delete(HttpContext context)
		{
			string text = context.Request["logId"];
			if (string.IsNullOrEmpty(text))
			{
				throw new HidistroAshxException("错误的参数");
			}
			long logId = long.Parse(text);
			if (EventLogs.DeleteLog(logId))
			{
				base.ReturnSuccessResult(context, "成功删除了单个操作日志", 0, true);
				return;
			}
			throw new HidistroAshxException("在删除过程中出现未知错误");
		}

		private void Deletes(HttpContext context)
		{
			string text = context.Request["logIds"];
			if (string.IsNullOrEmpty(text))
			{
				throw new HidistroAshxException("错误的参数");
			}
			int num = EventLogs.DeleteLogs(text);
			if (num > 0)
			{
				base.ReturnSuccessResult(context, $"成功删除了{num}个操作日志", 0, true);
				return;
			}
			throw new HidistroAshxException("在删除过程中出现未知错误");
		}

		private void RemoveAll(HttpContext context)
		{
			if (EventLogs.DeleteAllLogs())
			{
				base.ReturnSuccessResult(context, "成功删除了所有操作日志", 0, true);
				return;
			}
			throw new HidistroAshxException("在删除过程中出现未知错误");
		}
	}
}
