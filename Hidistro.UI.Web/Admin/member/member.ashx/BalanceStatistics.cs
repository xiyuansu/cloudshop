using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Members;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Admin.member.ashx
{
	public class BalanceStatistics : AdminBaseHandler
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
				if (action == "exportexcel")
				{
					this.ExportExcel(context);
					return;
				}
				throw new HidistroAshxException("错误的参数");
			}
			this.GetList(context);
		}

		private BalanceDetailQuery GetDataQuery(HttpContext context)
		{
			BalanceDetailQuery balanceDetailQuery = new BalanceDetailQuery();
			balanceDetailQuery.FromDate = base.GetDateTimeParam(context, "FromDate");
			balanceDetailQuery.ToDate = base.GetDateTimeParam(context, "ToDate");
			balanceDetailQuery.UserName = base.GetParameter(context, "UserName", true);
			balanceDetailQuery.PageIndex = base.CurrentPageIndex;
			balanceDetailQuery.PageSize = base.CurrentPageSize;
			balanceDetailQuery.SortBy = "TradeDate";
			balanceDetailQuery.SortOrder = SortAction.Desc;
			return balanceDetailQuery;
		}

		private string GetTradeType(TradeTypes type, bool isDistributor = false)
		{
			string text = "";
			switch (type)
			{
			case TradeTypes.SelfhelpInpour:
				return "自助充值";
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

		private void GetList(HttpContext context)
		{
			BalanceDetailQuery dataQuery = this.GetDataQuery(context);
			DataGridViewModel<Dictionary<string, object>> listSplittinDraws = this.GetListSplittinDraws(dataQuery);
			string s = base.SerializeObjectToJson(listSplittinDraws);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetListSplittinDraws(BalanceDetailQuery query)
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

		public void ExportExcel(HttpContext context)
		{
			BalanceDetailQuery dataQuery = this.GetDataQuery(context);
			DbQueryResult balanceDetailsNoPage = MemberHelper.GetBalanceDetailsNoPage(dataQuery);
			string empty = string.Empty;
			empty += "用户名";
			empty += ",交易时间";
			empty += ",业务摘要";
			empty += ",转入金额";
			empty += ",转出金额";
			empty += ",当前余额\r\n";
			foreach (DataRow row in balanceDetailsNoPage.Data.Rows)
			{
				string tradeType = this.GetTradeType((TradeTypes)row["TradeType"], false);
				empty += row["UserName"];
				empty = empty + "," + row["TradeDate"];
				empty = empty + "," + tradeType;
				empty = empty + "," + row["Income"];
				empty = empty + "," + row["Expenses"];
				empty = empty + "," + row["Balance"] + "\r\n";
			}
			context.Response.Clear();
			context.Response.Buffer = false;
			context.Response.Charset = "GB2312";
			context.Response.AppendHeader("Content-Disposition", "attachment;filename=BalanceDetailsStatistics.csv");
			context.Response.ContentEncoding = Encoding.GetEncoding("GB2312");
			context.Response.ContentType = "application/octet-stream";
			context.Response.Write(empty);
			context.Response.End();
		}
	}
}
