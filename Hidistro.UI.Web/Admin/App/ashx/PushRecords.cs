using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Web.ashxBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hidistro.UI.Web.Admin.App.ashx
{
	public class PushRecords : AdminBaseHandler
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
			int num3 = 0;
			string empty = string.Empty;
			DateTime? nullable = null;
			DateTime? nullable2 = null;
			AppPushRecordQuery appPushRecordQuery = new AppPushRecordQuery();
			nullable = base.GetDateTimeParam(context, "StartDate");
			nullable2 = base.GetDateTimeParam(context, "EndDate");
			empty = context.Request["PushStatus"];
			if (!string.IsNullOrEmpty(context.Request["PushStatus"]))
			{
				appPushRecordQuery.PushStatus = (EnumPushStatus)empty.ToInt(0);
			}
			empty = context.Request["PushType"];
			if (!string.IsNullOrEmpty(context.Request["PushType"]))
			{
				appPushRecordQuery.PushType = (EnumPushType)empty.ToInt(0);
			}
			appPushRecordQuery.StartDate = nullable;
			appPushRecordQuery.EndDate = nullable2;
			appPushRecordQuery.PageIndex = base.CurrentPageIndex;
			appPushRecordQuery.PageSize = base.CurrentPageSize;
			DataGridViewModel<AppPushRecordInfo> dataList = this.GetDataList(appPushRecordQuery);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<AppPushRecordInfo> GetDataList(AppPushRecordQuery query)
		{
			DataGridViewModel<AppPushRecordInfo> dataGridViewModel = new DataGridViewModel<AppPushRecordInfo>();
			if (query != null)
			{
				PageModel<AppPushRecordInfo> appPushRecords = VShopHelper.GetAppPushRecords(query);
				dataGridViewModel.rows = (from c in appPushRecords.Models
				select new AppPushRecordInfo
				{
					PushRecordId = c.PushRecordId,
					PushTypeText = ((Enum)(object)(EnumPushType)c.PushType).ToDescription(),
					PushTag = c.PushTag,
					PushTagText = c.PushTagText,
					PushSendDate = c.PushSendDate,
					PushStatusText = ((Enum)(object)(EnumPushStatus)c.PushStatus).ToDescription(),
					PushTitle = c.PushTitle,
					PushRemark = c.PushRemark,
					PushStatus = c.PushStatus,
					UserId = c.UserId
				}).ToList();
				dataGridViewModel.total = appPushRecords.Total;
				foreach (AppPushRecordInfo row in dataGridViewModel.rows)
				{
				}
			}
			return dataGridViewModel;
		}

		private void Delete(HttpContext context)
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
			int num = 0;
			string[] array2 = text.Split(',');
			foreach (string obj in array2)
			{
				VShopHelper.DeleteAppPushRecord(obj.ToInt(0));
				num++;
			}
			if (num > 0)
			{
				base.ReturnSuccessResult(context, "删除成功！", 0, true);
				return;
			}
			throw new HidistroAshxException("请先选择需要删除的消息！");
		}
	}
}
