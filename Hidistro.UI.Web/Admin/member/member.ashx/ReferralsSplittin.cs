using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Members;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Web.ashxBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Admin.member.ashx
{
	[PrivilegeCheck(Privilege.Referrals)]
	public class ReferralsSplittin : AdminBaseHandler
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
			BalanceDetailQuery dataQuery = this.GetDataQuery(context);
			IList<CommissionDetailModel> splittinDetailsExportData = MemberHelper.GetSplittinDetailsExportData(dataQuery);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<table border='1'>");
			stringBuilder.Append("<thead><tr>");
			stringBuilder.Append("<th>分销员</th>");
			stringBuilder.Append("<th>订单编号</th>");
			stringBuilder.Append("<th>会员</th>");
			stringBuilder.Append("<th>支付时间</th>");
			stringBuilder.Append("<th>佣金结算时间</th>");
			stringBuilder.Append("<th>订单金额</th>");
			stringBuilder.Append("<th>佣金</th>");
			stringBuilder.Append("<th>佣金类型</th>");
			stringBuilder.Append("</tr></thead>");
			StringBuilder stringBuilder2 = new StringBuilder();
			foreach (CommissionDetailModel item in splittinDetailsExportData)
			{
				stringBuilder2.Append("<tr>");
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.ReferalUserName, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.OrderId, false));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.FromUserName, false));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.TradeDateStr, false));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.FinishDateStr, false));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.OrderTotal.F2ToString("f2"), false));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.Commission.F2ToString("f2"), false));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.SplittingTypeText, false));
				stringBuilder2.Append("</tr>");
			}
			stringBuilder.AppendFormat("<tbody>{0}</tbody></table>", stringBuilder2.ToString());
			StringWriter stringWriter = new StringWriter();
			stringWriter.Write(stringBuilder);
			DownloadHelper.DownloadFile(context.Response, stringWriter.GetStringBuilder(), "SplittinDetail" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls");
			stringWriter.Close();
			context.Response.End();
		}

		private void GetList(HttpContext context)
		{
			string empty = string.Empty;
			BalanceDetailQuery dataQuery = this.GetDataQuery(context);
			DataGridViewModel<Dictionary<string, object>> referralRequest = this.GetReferralRequest(dataQuery);
			string s = base.SerializeObjectToJson(referralRequest);
			context.Response.Write(s);
			context.Response.End();
		}

		public BalanceDetailQuery GetDataQuery(HttpContext context)
		{
			string empty = string.Empty;
			BalanceDetailQuery balanceDetailQuery = new BalanceDetailQuery();
			empty = context.Request["Keywords"];
			balanceDetailQuery.UserId = base.GetIntParam(context, "UserId", false).Value;
			balanceDetailQuery.OrderId = base.GetParameter(context, "OrderId", false);
			balanceDetailQuery.SplittingTypes = (SplittingTypes)Convert.ToInt32(base.GetIntParam(context, "SplittingTypes", true));
			balanceDetailQuery.UserName = empty;
			balanceDetailQuery.FromDate = base.GetDateTimeParam(context, "PayStart");
			balanceDetailQuery.ToDate = base.GetDateTimeParam(context, "PayEnd");
			balanceDetailQuery.FromDateJS = base.GetDateTimeParam(context, "SetStart");
			balanceDetailQuery.ToDateJS = base.GetDateTimeParam(context, "SetEnd");
			balanceDetailQuery.PageIndex = base.CurrentPageIndex;
			balanceDetailQuery.PageSize = base.CurrentPageSize;
			return balanceDetailQuery;
		}

		private DataGridViewModel<Dictionary<string, object>> GetReferralRequest(BalanceDetailQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			string empty = string.Empty;
			if (query != null)
			{
				DbQueryResult splittinDetails = MemberHelper.GetSplittinDetails(query);
				List<Dictionary<string, object>> list = DataHelper.DataTableToDictionary(splittinDetails.Data);
				foreach (Dictionary<string, object> item in list)
				{
					SplittinDetailInfo splittinDetailInfo = item.ToObject<SplittinDetailInfo>();
					if (splittinDetailInfo.TradeType == SplittingTypes.RegReferralDeduct)
					{
						item.Add("TradeDateStr", "");
						item.Add("FinishDateStr", splittinDetailInfo.TradeDate);
					}
					else
					{
						item.Add("TradeDateStr", splittinDetailInfo.TradeDate);
						item.Add("FinishDateStr", item["FinishDate"]);
					}
					if (splittinDetailInfo.TradeType == SplittingTypes.DrawRequest)
					{
						item.Add("Money", "-" + splittinDetailInfo.Expenses);
					}
					else
					{
						item.Add("Money", splittinDetailInfo.Income);
					}
					if (string.IsNullOrEmpty(item["FromUserName"].ToNullString()))
					{
						item.Add("UserNameStr", splittinDetailInfo.UserName);
					}
					else
					{
						item.Add("UserNameStr", item["FromUserName"].ToNullString());
					}
					item.Add("TradeTypeStr", MemberHelper.GetSplittingType((int)splittinDetailInfo.TradeType));
				}
				dataGridViewModel.rows = list;
				dataGridViewModel.total = splittinDetails.TotalRecords;
			}
			return dataGridViewModel;
		}
	}
}
