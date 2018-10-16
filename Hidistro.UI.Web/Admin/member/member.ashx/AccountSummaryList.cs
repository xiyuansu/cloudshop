using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Members;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Web.ashxBase;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Admin.member.ashx
{
	public class AccountSummaryList : AdminBaseHandler
	{
		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			if (!string.IsNullOrWhiteSpace(base.action))
			{
				base.action = base.action.ToLower();
			}
			switch (base.action)
			{
			case "getlist":
				this.GetList(context);
				break;
			case "addmoney":
				this.AddMoney(context);
				break;
			case "getadvancestatistics":
				this.GetAdvanceStatistics(context);
				break;
			case "exporttoexcel":
				this.ExportToExcel(context);
				break;
			default:
				throw new HidistroAshxException("错误的参数");
			}
		}

		public void ExportToExcel(HttpContext context)
		{
			MemberQuery dataQuery = this.GetDataQuery(context);
			IList<MemberInfo> memberBlanceListNoPage = MemberHelper.GetMemberBlanceListNoPage(dataQuery);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<table border='1'>");
			stringBuilder.Append("<thead><tr>");
			stringBuilder.Append("<th>会员名</th>");
			stringBuilder.Append("<th>姓名</th>");
			stringBuilder.Append("<th>昵称</th>");
			stringBuilder.Append("<th>帐号总额</th>");
			stringBuilder.Append("<th>冻结金额</th>");
			stringBuilder.Append("<th>可用余额</th>");
			stringBuilder.Append("</tr></thead>");
			StringBuilder stringBuilder2 = new StringBuilder();
			foreach (MemberInfo item in memberBlanceListNoPage)
			{
				stringBuilder2.Append("<tr>");
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.UserName, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.RealName, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.NickName, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.Balance, false));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.RequestBalance, false));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.UseableBalance, false));
				stringBuilder2.Append("</tr>");
			}
			stringBuilder.AppendFormat("<tbody>{0}</tbody></table>", stringBuilder2.ToString());
			StringWriter stringWriter = new StringWriter();
			stringWriter.Write(stringBuilder);
			DownloadHelper.DownloadFile(context.Response, stringWriter.GetStringBuilder(), "BalanceList" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls");
			stringWriter.Close();
			context.Response.End();
		}

		private void GetAdvanceStatistics(HttpContext context)
		{
			try
			{
				IDictionary<string, decimal> advanceStatistics = MemberHelper.GetAdvanceStatistics();
				string str = base.SerializeObjectToJson(advanceStatistics);
				context.Response.Write("{\"Status\":\"Success\",\"Data\":" + str + "}");
			}
			catch (Exception ex)
			{
				context.Response.Write("{\"Status\":\"Failure\",\"Msg\":\"" + ex.Message + "\"}");
				context.Response.End();
			}
			context.Response.End();
		}

		private void GetList(HttpContext context)
		{
			MemberQuery dataQuery = this.GetDataQuery(context);
			DataGridViewModel<Dictionary<string, object>> listSplittinDraws = this.GetListSplittinDraws(dataQuery);
			string s = base.SerializeObjectToJson(listSplittinDraws);
			context.Response.Write(s);
			context.Response.End();
		}

		private MemberQuery GetDataQuery(HttpContext context)
		{
			MemberQuery memberQuery = new MemberQuery();
			memberQuery.UserName = base.GetParameter(context, "UserName", true);
			memberQuery.RealName = base.GetParameter(context, "RealName", true);
			memberQuery.PageIndex = base.CurrentPageIndex;
			memberQuery.PageSize = base.CurrentPageSize;
			memberQuery.SortBy = "Balance";
			memberQuery.SortOrder = SortAction.Desc;
			return memberQuery;
		}

		private DataGridViewModel<Dictionary<string, object>> GetListSplittinDraws(MemberQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			if (query != null)
			{
				DbQueryResult memberBlanceList = MemberHelper.GetMemberBlanceList(query);
				dataGridViewModel.rows = DataHelper.DataTableToDictionary(memberBlanceList.Data);
				dataGridViewModel.total = memberBlanceList.TotalRecords;
				foreach (Dictionary<string, object> row in dataGridViewModel.rows)
				{
					row.Add("UseableBalance", row["Balance"].ToDecimal(0) - row["RequestBalance"].ToDecimal(0));
				}
			}
			return dataGridViewModel;
		}

		private void AddMoney(HttpContext context)
		{
			ManagerHelper.CheckPrivilege(Privilege.MemberReCharge);
			decimal parameter = base.GetParameter(context, "income", default(decimal));
			int parameter2 = base.GetParameter(context, "userid", 0);
			string parameter3 = base.GetParameter(context, "remark", true);
			decimal d = decimal.Parse(parameter.F2ToString("f2"));
			if (parameter > d)
			{
				throw new HidistroAshxException("金额不能超过2位小数");
			}
			if (parameter < -10000000m || parameter > 10000000m)
			{
				throw new HidistroAshxException("金额大小必须在正负1000万之间");
			}
			MemberInfo user = Users.GetUser(parameter2);
			if (user == null)
			{
				throw new HidistroAshxException("该用户不存在");
			}
			decimal balance = parameter + user.Balance;
			BalanceDetailInfo balanceDetailInfo = new BalanceDetailInfo();
			balanceDetailInfo.UserId = parameter2;
			balanceDetailInfo.UserName = user.UserName;
			balanceDetailInfo.TradeDate = DateTime.Now;
			balanceDetailInfo.TradeType = TradeTypes.BackgroundAddmoney;
			balanceDetailInfo.Income = parameter;
			balanceDetailInfo.Balance = balance;
			balanceDetailInfo.Remark = parameter3;
			ValidationResults validationResults = Validation.Validate(balanceDetailInfo, "ValBalanceDetail");
			string text = string.Empty;
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult item in (IEnumerable<ValidationResult>)validationResults)
				{
					text += Formatter.FormatErrorMessage(item.Message);
				}
				throw new HidistroAshxException(text);
			}
			if (MemberHelper.AddBalance(balanceDetailInfo, parameter))
			{
				base.ReturnSuccessResult(context, "操作成功", 0, true);
			}
		}
	}
}
