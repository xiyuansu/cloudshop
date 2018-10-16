using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Depot;
using Hidistro.SaleSystem.Depot;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Admin.depot.ashx
{
	public class DrawRequest : AdminBaseHandler
	{
		private string sError = string.Empty;

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
			case "confirm":
				this.Confirm(context);
				break;
			case "refuse":
				this.Refuse(context);
				break;
			case "exportexcel":
				this.ExportExcel(context);
				break;
			default:
				throw new HidistroAshxException("错误的参数");
			}
		}

		private StoreBalanceDrawRequestQuery GetDataQuery(HttpContext context)
		{
			StoreBalanceDrawRequestQuery storeBalanceDrawRequestQuery = new StoreBalanceDrawRequestQuery();
			storeBalanceDrawRequestQuery.UserName = base.GetParameter(context, "UserName", true);
			storeBalanceDrawRequestQuery.StoreName = base.GetParameter(context, "StoreName", true);
			storeBalanceDrawRequestQuery.StoreId = base.GetIntParam(context, "StoreId", true);
			storeBalanceDrawRequestQuery.FromDate = base.GetDateTimeParam(context, "FromDate");
			storeBalanceDrawRequestQuery.ToDate = base.GetDateTimeParam(context, "ToDate");
			storeBalanceDrawRequestQuery.AuditStatus = 1;
			storeBalanceDrawRequestQuery.DrawRequestType = 1;
			storeBalanceDrawRequestQuery.PageIndex = base.CurrentPageIndex;
			storeBalanceDrawRequestQuery.PageSize = base.CurrentPageSize;
			if (storeBalanceDrawRequestQuery.StoreId.HasValue && storeBalanceDrawRequestQuery.StoreId.Value < 0)
			{
				storeBalanceDrawRequestQuery.StoreId = 0;
			}
			return storeBalanceDrawRequestQuery;
		}

		public void GetList(HttpContext context)
		{
			StoreBalanceDrawRequestQuery dataQuery = this.GetDataQuery(context);
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList(dataQuery);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList(StoreBalanceDrawRequestQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			if (query != null)
			{
				PageModel<StoreBalanceDrawRequestInfo> balanceDrawRequests = StoreBalanceHelper.GetBalanceDrawRequests(query, true);
				dataGridViewModel.rows = new List<Dictionary<string, object>>();
				dataGridViewModel.total = balanceDrawRequests.Total;
				foreach (StoreBalanceDrawRequestInfo model in balanceDrawRequests.Models)
				{
					Dictionary<string, object> item = model.ToDictionary();
					dataGridViewModel.rows.Add(item);
				}
			}
			return dataGridViewModel;
		}

		public void Confirm(HttpContext context)
		{
			int? intParam = base.GetIntParam(context, "id", true);
			if (!intParam.HasValue || intParam < 1)
			{
				throw new HidistroAshxException("错误的数据编号");
			}
			if (StoreBalanceHelper.DealBalanceDrawRequestById(intParam.Value, true, ref this.sError, ""))
			{
				base.ReturnSuccessResult(context, "预付款提现成功", 0, true);
				return;
			}
			throw new HidistroAshxException(this.sError);
		}

		public void Refuse(HttpContext context)
		{
			int? intParam = base.GetIntParam(context, "id", true);
			if (!intParam.HasValue || intParam < 1)
			{
				throw new HidistroAshxException("错误的数据编号");
			}
			string parameter = base.GetParameter(context, "Reason", true);
			if (string.IsNullOrWhiteSpace(parameter))
			{
				throw new HidistroAshxException("请填写拒绝申请的原因");
			}
			if (parameter.Length > 50)
			{
				throw new HidistroAshxException("拒绝原因不能超过50字符");
			}
			if (StoreBalanceHelper.DealBalanceDrawRequestById(intParam.Value, false, ref this.sError, parameter))
			{
				base.ReturnSuccessResult(context, "拒绝申请成功", 0, true);
				return;
			}
			throw new HidistroAshxException("提现申请操作失败");
		}

		public void ExportExcel(HttpContext context)
		{
			StoreBalanceDrawRequestQuery dataQuery = this.GetDataQuery(context);
			DbQueryResult balanceDrawRequest4Report = StoreBalanceHelper.GetBalanceDrawRequest4Report(dataQuery, true);
			string empty = string.Empty;
			empty += "用户名";
			empty += ",门店";
			empty += ",申请时间";
			empty += ",业务摘要";
			empty += ",提现金额";
			empty += ",备注\r\n";
			foreach (DataRow row in balanceDrawRequest4Report.Data.Rows)
			{
				empty += row["UserName"];
				empty = empty + "," + row["StoreName"];
				empty = empty + "," + row["RequestTime"];
				empty += ",银行卡提现";
				empty = empty + "," + row["Amount"];
				empty = empty + "," + row["Remark"] + "\r\n";
			}
			context.Response.Clear();
			context.Response.Buffer = false;
			context.Response.Charset = "GB2312";
			context.Response.AppendHeader("Content-Disposition", "attachment;filename=DrawRequest.csv");
			context.Response.ContentEncoding = Encoding.GetEncoding("GB2312");
			context.Response.ContentType = "application/octet-stream";
			context.Response.Write(empty);
			context.Response.End();
		}
	}
}
