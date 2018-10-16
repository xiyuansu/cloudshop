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
	public class MemberPointList : AdminBaseHandler
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
			MemberQuery dataQuery = this.GetDataQuery(context);
			string safeIDList = Globals.GetSafeIDList(base.GetParameter(context, "Ids", false), ',', true);
			IList<PointMemberModel> pointMembersNoPage = MemberHelper.GetPointMembersNoPage(dataQuery, safeIDList);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<table border='1'>");
			stringBuilder.Append("<thead><tr>");
			stringBuilder.Append("<th>会员名</th>");
			stringBuilder.Append("<th>用户积分</th>");
			stringBuilder.Append("<th>历史积分</th>");
			stringBuilder.Append("<th>会员等级</th>");
			stringBuilder.Append("</tr></thead>");
			StringBuilder stringBuilder2 = new StringBuilder();
			foreach (PointMemberModel item in pointMembersNoPage)
			{
				stringBuilder2.Append("<tr>");
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.UserName, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.Points, false));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.HistoryPoint, false));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.GradeName, true));
				stringBuilder2.Append("</tr>");
			}
			stringBuilder.AppendFormat("<tbody>{0}</tbody></table>", stringBuilder2.ToString());
			StringWriter stringWriter = new StringWriter();
			stringWriter.Write(stringBuilder);
			DownloadHelper.DownloadFile(context.Response, stringWriter.GetStringBuilder(), "MemberPointList" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls");
			stringWriter.Close();
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
			memberQuery.GradeId = base.GetIntParam(context, "GradeId", true);
			memberQuery.PageIndex = base.CurrentPageIndex;
			memberQuery.PageSize = base.CurrentPageSize;
			memberQuery.SortBy = "points";
			memberQuery.SortOrder = SortAction.Desc;
			return memberQuery;
		}

		private DataGridViewModel<Dictionary<string, object>> GetListSplittinDraws(MemberQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			if (query != null)
			{
				DbQueryResult pointMembers = MemberHelper.GetPointMembers(query);
				dataGridViewModel.rows = DataHelper.DataTableToDictionary(pointMembers.Data);
				dataGridViewModel.total = pointMembers.TotalRecords;
				foreach (Dictionary<string, object> row in dataGridViewModel.rows)
				{
					row.Add("UseableBalance", row["Balance"].ToDecimal(0) - row["RequestBalance"].ToDecimal(0));
				}
			}
			return dataGridViewModel;
		}
	}
}
