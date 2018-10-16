using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Supplier;
using Hidistro.SaleSystem.Supplier;
using Hidistro.UI.Web.ashxBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Admin.Supplier.Balance.ashx
{
	public class RquestStatistics : AdminBaseHandler
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
			string action = base.action;
			if (!(action == "getlist"))
			{
				if (action == "exportexcel")
				{
					this.ExportToExcel(context);
					return;
				}
				throw new HidistroAshxException("错误的参数");
			}
			this.GetList(context);
		}

		public void ExportToExcel(HttpContext context)
		{
			BalanceStatisticsQuery dataQuery = this.GetDataQuery(context);
			IList<SupplierSettlementModel> balanceStatisticsExportData = BalanceHelper.GetBalanceStatisticsExportData(dataQuery);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<table border='1'>");
			stringBuilder.Append("<thead><tr>");
			stringBuilder.Append("<th>供应商</th>");
			stringBuilder.Append("<th>联系电话</th>");
			stringBuilder.Append("<th>可提现金额</th>");
			stringBuilder.Append("<th>已冻结提现</th>");
			stringBuilder.Append("<th>已提现总额</th>");
			stringBuilder.Append("</tr></thead>");
			StringBuilder stringBuilder2 = new StringBuilder();
			foreach (SupplierSettlementModel item in balanceStatisticsExportData)
			{
				stringBuilder2.Append("<tr>");
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.SupplierName, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.Tel, false));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.CanDrawRequestBalance.F2ToString("f2"), false));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.FrozenBalance.F2ToString("f2"), false));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.BalanceOut.F2ToString("f2"), false));
				stringBuilder2.Append("</tr>");
			}
			stringBuilder.AppendFormat("<tbody>{0}</tbody></table>", stringBuilder2.ToString());
			StringWriter stringWriter = new StringWriter();
			stringWriter.Write(stringBuilder);
			DownloadHelper.DownloadFile(context.Response, stringWriter.GetStringBuilder(), "SupplierSettlement" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls");
			stringWriter.Close();
			context.Response.End();
		}

		public void GetList(HttpContext context)
		{
			BalanceStatisticsQuery dataQuery = this.GetDataQuery(context);
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList(dataQuery);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private BalanceStatisticsQuery GetDataQuery(HttpContext context)
		{
			BalanceStatisticsQuery balanceStatisticsQuery = new BalanceStatisticsQuery();
			balanceStatisticsQuery.Key = base.GetParameter(context, "Key", true);
			balanceStatisticsQuery.PageIndex = base.CurrentPageIndex;
			balanceStatisticsQuery.PageSize = base.CurrentPageSize;
			return balanceStatisticsQuery;
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList(BalanceStatisticsQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			if (query != null)
			{
				DbQueryResult balanceStatisticsList = BalanceHelper.GetBalanceStatisticsList(query);
				dataGridViewModel.rows = new List<Dictionary<string, object>>();
				dataGridViewModel.total = balanceStatisticsList.TotalRecords;
				foreach (DataRow row in balanceStatisticsList.Data.Rows)
				{
					Dictionary<string, object> dictionary = DataHelper.DataRowToDictionary(row);
					dictionary.Add("AvailableBalance", dictionary["Balance"].ToDecimal(0) - dictionary["BalanceFozen"].ToDecimal(0));
					dataGridViewModel.rows.Add(dictionary);
				}
			}
			return dataGridViewModel;
		}
	}
}
