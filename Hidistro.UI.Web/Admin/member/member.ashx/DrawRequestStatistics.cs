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
	public class DrawRequestStatistics : AdminBaseHandler
	{
		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			string text = context.Request["action"];
			if (string.IsNullOrWhiteSpace(text))
			{
				throw new HidistroAshxException("错误的参数");
			}
			string a = text;
			if (!(a == "getlist"))
			{
				if (a == "exportexcel")
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
			balanceDetailQuery.TradeType = TradeTypes.DrawRequest;
			balanceDetailQuery.SortBy = "TradeDate";
			balanceDetailQuery.SortOrder = SortAction.Desc;
			balanceDetailQuery.PageIndex = base.CurrentPageIndex;
			balanceDetailQuery.PageSize = base.CurrentPageSize;
			return balanceDetailQuery;
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
					row["TradeTypeText"] = this.GetTradeTypeText((TradeTypes)row["TradeType"], false);
					row.Add("IsWeiXin", MemberHelper.GetBalanceIsWeixin(row["InpourId"].ToInt(0)));
				}
			}
			return dataGridViewModel;
		}

		private string GetTradeTypeText(TradeTypes type, bool isDistributor = false)
		{
			string empty = string.Empty;
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

		private void ExportExcel(HttpContext context)
		{
			BalanceDetailQuery dataQuery = this.GetDataQuery(context);
			DbQueryResult balanceDetailsNoPage = MemberHelper.GetBalanceDetailsNoPage(dataQuery);
			string empty = string.Empty;
			empty += "用户名";
			empty += ",交易时间";
			empty += ",提现帐户";
			empty += ",业务摘要";
			empty += ",转出金额";
			empty += ",当前余额";
			empty += ",操作人\r\n";
			foreach (DataRow row in balanceDetailsNoPage.Data.Rows)
			{
				int requestId = row["InpourId"].ToInt(0);
				BalanceDrawRequestInfo balanceDrawRequestInfo = MemberHelper.GetBalanceDrawRequestInfo(requestId);
				empty += row["UserName"];
				empty = empty + "," + row["TradeDate"];
				if (balanceDrawRequestInfo != null)
				{
					string text = "";
					text = ((!balanceDrawRequestInfo.IsWeixin.HasValue || !balanceDrawRequestInfo.IsWeixin.Value) ? ((!balanceDrawRequestInfo.IsAlipay.HasValue || !balanceDrawRequestInfo.IsAlipay.Value) ? $"提现到银行卡(开户银行:{balanceDrawRequestInfo.BankName}，银行开户名:{balanceDrawRequestInfo.AccountName}，银行卡帐号:{balanceDrawRequestInfo.MerchantCode})" : ("提现到支付宝(支付宝帐号:" + balanceDrawRequestInfo.AlipayCode + "，支付宝姓名:" + balanceDrawRequestInfo.AlipayRealName + ")")) : "提现到微信");
					empty = empty + "," + text;
				}
				else
				{
					empty += ",";
				}
				empty += ",提现";
				empty = empty + "," + row["Expenses"];
				empty = empty + "," + row["Balance"];
				empty = empty + "," + row["ManagerUserName"] + "\r\n";
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
