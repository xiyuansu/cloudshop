using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Comments;
using Hidistro.SaleSystem.Comments;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hidistro.UI.Web.Admin.comment.ashx
{
	public class SendedMessages : AdminBaseHandler
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
			MessageBoxQuery messageBoxQuery = new MessageBoxQuery();
			messageBoxQuery.Sernder = "admin";
			messageBoxQuery.PageIndex = base.CurrentPageIndex;
			messageBoxQuery.PageSize = base.CurrentPageSize;
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList(messageBoxQuery);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList(MessageBoxQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			if (query != null)
			{
				DbQueryResult managerSendedMessages = NoticeHelper.GetManagerSendedMessages(query);
				dataGridViewModel.rows = DataHelper.DataTableToDictionary(managerSendedMessages.Data);
				dataGridViewModel.total = managerSendedMessages.TotalRecords;
				foreach (Dictionary<string, object> row in dataGridViewModel.rows)
				{
				}
			}
			return dataGridViewModel;
		}

		private void Delete(HttpContext context)
		{
			string parameter = base.GetParameter(context, "ids", false);
			if (string.IsNullOrWhiteSpace(parameter))
			{
				throw new HidistroAshxException("请选要删除的消息");
			}
			List<long> list = (from d in parameter.Split(',')
			where !string.IsNullOrWhiteSpace(d)
			select long.Parse(d)).ToList();
			int num = list.Count();
			int num2 = 0;
			if (list.Count < 1)
			{
				throw new HidistroAshxException("请选要删除的消息");
			}
			if (list.Count > 0)
			{
				num2 = NoticeHelper.DeleteManagerMessages(list);
				base.ReturnSuccessResult(context, "成功删除了选择的消息！", 0, true);
				return;
			}
			throw new HidistroAshxException("请选择需要删除的消息");
		}
	}
}
