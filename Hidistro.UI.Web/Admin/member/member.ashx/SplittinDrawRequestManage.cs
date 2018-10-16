using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Members;
using Hidistro.UI.Web.ashxBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Admin.member.ashx
{
	public class SplittinDrawRequestManage : AdminBaseHandler
	{
		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			string text = context.Request["action"];
			if (string.IsNullOrWhiteSpace(text))
			{
				throw new HidistroAshxException("错误的参数");
			}
			switch (text)
			{
			case "getSplittinDraws":
				this.GetSplittinDraws(context);
				break;
			case "exporttoexcel":
				this.Exporttoexcel(context);
				break;
			case "drawRequest":
				this.DrawRequest(context);
				break;
			case "historyexportexcel":
				this.HistoryExportToExcel(context);
				break;
			default:
				throw new HidistroAshxException("错误的参数");
			}
		}

		public void HistoryExportToExcel(HttpContext context)
		{
			int value = base.GetIntParam(context, "UserId", false).Value;
			BalanceDrawRequestQuery balanceDrawRequestQuery = this.GetBalanceDrawRequestQuery(context);
			IList<CommissionRequestModel> splittinDrawsExportData = MemberHelper.GetSplittinDrawsExportData(balanceDrawRequestQuery, balanceDrawRequestQuery.AuditStatus);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<table border='1'>");
			stringBuilder.Append("<thead><tr>");
			stringBuilder.Append("<th>分销员</th>");
			stringBuilder.Append("<th>申请时间</th>");
			stringBuilder.Append("<th>提现金额</th>");
			stringBuilder.Append("<th>操作人</th>");
			stringBuilder.Append("<th>备注</th>");
			stringBuilder.Append("<th>提现账号信息</th>");
			stringBuilder.Append("</tr></thead>");
			StringBuilder stringBuilder2 = new StringBuilder();
			DateTime dateTime;
			foreach (CommissionRequestModel item in splittinDrawsExportData)
			{
				stringBuilder2.Append("<tr>");
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.ReferralUserName, true));
				StringBuilder stringBuilder3 = stringBuilder2;
				dateTime = item.RequestDate;
				stringBuilder3.Append(ExcelHelper.GetXLSFieldsTD(dateTime.ToString("yyyy-MM-dd HH:mm:ss"), false));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.Amount.F2ToString("f2"), false));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.ManagerUserName, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.Remark, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.AccountInfo, true));
				stringBuilder2.Append("</tr>");
			}
			stringBuilder.AppendFormat("<tbody>{0}</tbody></table>", stringBuilder2.ToString());
			StringWriter stringWriter = new StringWriter();
			stringWriter.Write(stringBuilder);
			HttpResponse response = context.Response;
			StringBuilder stringBuilder4 = stringWriter.GetStringBuilder();
			dateTime = DateTime.Now;
			DownloadHelper.DownloadFile(response, stringBuilder4, "SplittinDraws" + dateTime.ToString("yyyyMMddhhmmss") + ".xls");
			stringWriter.Close();
			context.Response.End();
		}

		private void GetSplittinDraws(HttpContext context)
		{
			BalanceDrawRequestQuery balanceDrawRequestQuery = this.GetBalanceDrawRequestQuery(context);
			DataGridViewModel<Dictionary<string, object>> listSplittinDraws = this.GetListSplittinDraws(balanceDrawRequestQuery);
			string s = base.SerializeObjectToJson(listSplittinDraws);
			context.Response.Write(s);
			context.Response.End();
		}

		private BalanceDrawRequestQuery GetBalanceDrawRequestQuery(HttpContext context)
		{
			int num = 1;
			int num2 = 10;
			int drawRequestType = 0;
			string empty = string.Empty;
			DateTime? fromDate = null;
			DateTime? toDate = null;
			string empty2 = string.Empty;
			empty2 = context.Request["UserName"];
			empty = context.Request["DateStart"];
			if (!string.IsNullOrWhiteSpace(empty))
			{
				try
				{
					fromDate = DateTime.Parse(empty);
				}
				catch
				{
					fromDate = null;
				}
			}
			empty = context.Request["DateEnd"];
			if (!string.IsNullOrWhiteSpace(empty))
			{
				try
				{
					toDate = DateTime.Parse(empty);
				}
				catch
				{
					toDate = null;
				}
			}
			empty = context.Request["DrawRequestType"];
			if (!string.IsNullOrWhiteSpace(empty))
			{
				try
				{
					drawRequestType = int.Parse(empty);
				}
				catch
				{
					drawRequestType = 0;
				}
			}
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
			BalanceDrawRequestQuery balanceDrawRequestQuery = new BalanceDrawRequestQuery();
			balanceDrawRequestQuery.AuditStatus = base.GetIntParam(context, "AuditStatus", false).Value;
			balanceDrawRequestQuery.FromDate = fromDate;
			balanceDrawRequestQuery.ToDate = toDate;
			balanceDrawRequestQuery.UserName = empty2;
			balanceDrawRequestQuery.PageSize = num2;
			balanceDrawRequestQuery.PageIndex = num;
			balanceDrawRequestQuery.DrawRequestType = drawRequestType;
			return balanceDrawRequestQuery;
		}

		private DataGridViewModel<Dictionary<string, object>> GetListSplittinDraws(BalanceDrawRequestQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			string empty = string.Empty;
			if (query != null)
			{
				DbQueryResult splittinDraws = MemberHelper.GetSplittinDraws(query, query.AuditStatus);
				List<Dictionary<string, object>> list2 = dataGridViewModel.rows = DataHelper.DataTableToDictionary(splittinDraws.Data);
				dataGridViewModel.total = splittinDraws.TotalRecords;
			}
			return dataGridViewModel;
		}

		private void Exporttoexcel(HttpContext context)
		{
			BalanceDrawRequestQuery balanceDrawRequestQuery = this.GetBalanceDrawRequestQuery(context);
			balanceDrawRequestQuery.PageIndex = 1;
			balanceDrawRequestQuery.PageSize = 2147483647;
			DbQueryResult splittinDraws = MemberHelper.GetSplittinDraws(balanceDrawRequestQuery, 1);
			DataTable data = splittinDraws.Data;
			string empty = string.Empty;
			empty += "用户名";
			empty += ",申请时间";
			empty += ",提现金额";
			empty += ",开户银行";
			empty += ",开户人姓名";
			empty += ",银行账号";
			empty += ",备注\r\n";
			foreach (DataRow row in data.Rows)
			{
				empty += row["UserName"].ToString();
				empty = empty + "," + row["RequestDate"].ToString();
				empty = empty + "," + row["Amount"].ToString();
				empty = empty + "," + row["BankName"].ToString();
				empty = empty + "," + row["AccountName"].ToString();
				empty = empty + ",`" + row["MerchantCode"].ToString();
				empty = empty + "," + row["Remark"].ToString() + "\r\n";
			}
			context.Response.Clear();
			context.Response.Buffer = false;
			context.Response.Charset = "GB2312";
			context.Response.AppendHeader("Content-Disposition", "attachment;filename=SplittinDrawRequest.csv");
			context.Response.ContentEncoding = Encoding.GetEncoding("GB2312");
			context.Response.ContentType = "application/ms-excel";
			context.Response.Write(empty);
			context.Response.End();
		}

		private void DrawRequest(HttpContext context)
		{
			string a = context.Request["ChargeType"];
			long journalNumber = 0L;
			string s = context.Request["JournalNumber"];
			string text = context.Request["Reason"];
			if (!long.TryParse(s, out journalNumber))
			{
				return;
			}
			if (a == "UnLineReCharge")
			{
				if (!MemberHelper.AccepteDraw(journalNumber))
				{
					throw new HidistroAshxException("操作失败");
				}
				base.ReturnResult(context, true, "同意申请成功", 0, true);
			}
			if (!(a == "RefuseRequest"))
			{
				return;
			}
			if (string.IsNullOrEmpty(text))
			{
				throw new HidistroAshxException("请填写拒绝申请的原因");
			}
			if (MemberHelper.RefuseDraw(journalNumber, text))
			{
				base.ReturnResult(context, true, "拒绝申请成功", 0, true);
				return;
			}
			throw new HidistroAshxException("操作失败");
		}
	}
}
