using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Members;
using Hidistro.UI.Web.ashxBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Admin.member.ashx
{
	public class BalanceDetails : AdminBaseHandler
	{
		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			if (!string.IsNullOrWhiteSpace(base.action))
			{
				base.action = base.action.ToLower();
			}
			string action = base.action;
			if (!(action == "getlist"))
			{
				if (action == "exporttoexcel")
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
			BalanceDetailQuery dataQuery = this.GetDataQuery(context);
			IList<BalanceDetailInfo> memberBalanceDetailsNoPage = MemberHelper.GetMemberBalanceDetailsNoPage(dataQuery);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<table border='1'>");
			stringBuilder.Append("<thead><tr>");
			stringBuilder.Append("<th>流水号</th>");
			stringBuilder.Append("<th>用户名</th>");
			stringBuilder.Append("<th>时间</th>");
			stringBuilder.Append("<th>类型</th>");
			stringBuilder.Append("<th>收入</th>");
			stringBuilder.Append("<th>支出</th>");
			stringBuilder.Append("<th>帐户余额</th>");
			stringBuilder.Append("<th>备注</th>");
			stringBuilder.Append("</tr></thead>");
			StringBuilder stringBuilder2 = new StringBuilder();
			foreach (BalanceDetailInfo item in memberBalanceDetailsNoPage)
			{
				stringBuilder2.Append("<tr>");
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.JournalNumber, false));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.UserName, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.TradeDate, false));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.TradeTypeName, false));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.Income, false));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.Expenses, false));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.Balance, false));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.Remark, false));
				stringBuilder2.Append("</tr>");
			}
			stringBuilder.AppendFormat("<tbody>{0}</tbody></table>", stringBuilder2.ToString());
			StringWriter stringWriter = new StringWriter();
			stringWriter.Write(stringBuilder);
			DownloadHelper.DownloadFile(context.Response, stringWriter.GetStringBuilder(), "UserBalanceDetail" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls");
			stringWriter.Close();
			context.Response.End();
		}

		private void GetList(HttpContext context)
		{
			BalanceDetailQuery dataQuery = this.GetDataQuery(context);
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList(dataQuery);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private BalanceDetailQuery GetDataQuery(HttpContext context)
		{
			BalanceDetailQuery balanceDetailQuery = new BalanceDetailQuery();
			balanceDetailQuery.TradeType = (TradeTypes)base.GetIntParam(context, "TradeType", false).Value;
			balanceDetailQuery.UserId = base.GetIntParam(context, "UserId", false);
			balanceDetailQuery.FromDate = base.GetDateTimeParam(context, "FromDate");
			balanceDetailQuery.ToDate = base.GetDateTimeParam(context, "ToDate");
			balanceDetailQuery.PageIndex = base.CurrentPageIndex;
			balanceDetailQuery.PageSize = base.CurrentPageSize;
			balanceDetailQuery.SortBy = "Balance";
			balanceDetailQuery.SortOrder = SortAction.Desc;
			return balanceDetailQuery;
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList(BalanceDetailQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			if (query != null)
			{
				DbQueryResult balanceDetails = MemberHelper.GetBalanceDetails(query);
				dataGridViewModel.rows = DataHelper.DataTableToDictionary(balanceDetails.Data);
				dataGridViewModel.total = balanceDetails.TotalRecords;
				foreach (Dictionary<string, object> row in dataGridViewModel.rows)
				{
					row["TradeTypeText"] = this.GetTradeType((TradeTypes)row["TradeType"], false);
				}
			}
			return dataGridViewModel;
		}

		private string GetTradeType(TradeTypes type, bool isDistributor = false)
		{
			string text = "";
			switch (type)
			{
			case TradeTypes.SelfhelpInpour:
				return "自助充值";
			case TradeTypes.RechargeGift:
				return "充值赠送";
			case TradeTypes.BackgroundAddmoney:
				return "后台加款";
			case TradeTypes.Consume:
				return "消费";
			case TradeTypes.DrawRequest:
				return "提现";
			case TradeTypes.RefundOrder:
				if (isDistributor)
				{
					return "采购单退款";
				}
				return "订单退款";
			case TradeTypes.ReturnOrder:
				return isDistributor ? "采购单退货" : "订单退货";
			case TradeTypes.Commission:
				return "分销奖励";
			default:
				return "其他";
			}
		}
	}
}
