using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Members;
using Hidistro.UI.Web.Admin.depot.Models;
using Hidistro.UI.Web.ashxBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Admin.depot.ashx
{
	public class MemberDetails : AdminBaseHandler
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
				if (action == "exportrtoexcel")
				{
					this.ExportToExcel(context);
					return;
				}
				throw new HidistroAshxException("错误的参数");
			}
			this.GetList(context);
		}

		private MemberQuery getQuery(HttpContext context)
		{
			int num = 1;
			int num2 = 10;
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
			MemberQuery memberQuery = new MemberQuery();
			memberQuery.UserName = context.Request["StoreName"];
			memberQuery.UserName = memberQuery.UserName.Trim();
			memberQuery.StoreId = base.GetIntParam(context, "storeId", false).Value;
			memberQuery.ShoppingGuiderId = base.GetIntParam(context, "ManagerId", false).Value;
			memberQuery.StartTime = base.GetDateTimeParam(context, "startDate");
			memberQuery.EndTime = base.GetDateTimeParam(context, "endDate");
			memberQuery.SortOrder = SortAction.Desc;
			memberQuery.PageIndex = num;
			memberQuery.PageSize = num2;
			return memberQuery;
		}

		private void GetList(HttpContext context)
		{
			MemberQuery query = this.getQuery(context);
			MemberDetailsModel<Dictionary<string, object>> dataList = this.GetDataList(query);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private MemberDetailsModel<Dictionary<string, object>> GetDataList(MemberQuery query)
		{
			MemberDetailsModel<Dictionary<string, object>> memberDetailsModel = new MemberDetailsModel<Dictionary<string, object>>();
			if (query != null)
			{
				DbQueryResult membersClerkExpand = MemberHelper.GetMembersClerkExpand(query);
				memberDetailsModel.rows = DataHelper.DataTableToDictionary(membersClerkExpand.Data);
				memberDetailsModel.total = membersClerkExpand.TotalRecords;
				foreach (Dictionary<string, object> row in memberDetailsModel.rows)
				{
					DateTime dateTime = default(DateTime);
					DateTime.TryParse((row["CreateDate"] == null) ? "" : row["CreateDate"].ToString(), out dateTime);
					row["CreateDate"] = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
				}
			}
			return memberDetailsModel;
		}

		private void ExportToExcel(HttpContext context)
		{
			MemberQuery query = this.getQuery(context);
			query.PageIndex = 1;
			query.PageSize = 2147483647;
			DbQueryResult membersClerkExpand = MemberHelper.GetMembersClerkExpand(query);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
			stringBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
			stringBuilder.AppendLine("<td>会员账号</td>");
			stringBuilder.AppendLine("<td>店员</td>");
			stringBuilder.AppendLine("<td>创建时间</td>");
			stringBuilder.AppendLine("<td>订单金额</td>");
			stringBuilder.AppendLine("</tr>");
			DataTable data = membersClerkExpand.Data;
			DateTime dateTime;
			foreach (DataRow row in data.Rows)
			{
				stringBuilder.AppendLine("<tr>");
				stringBuilder.AppendLine("<td>" + row.Field<string>("UserName") + "</td>");
				stringBuilder.AppendLine("<td>" + row.Field<string>("ManagersName") + "</td>");
				StringBuilder stringBuilder2 = stringBuilder;
				dateTime = row.Field<DateTime>("CreateDate");
				stringBuilder2.AppendLine("<td>" + dateTime.ToString("yyyy-MM-dd HH:mm:ss") + "</td>");
				stringBuilder.AppendLine("<td>" + row["Expenditure"].ToDecimal(0).F2ToString("f2") + "</td>");
				stringBuilder.AppendLine("</tr>");
			}
			stringBuilder.AppendLine("<tr>");
			stringBuilder.AppendLine("<td></td>");
			stringBuilder.AppendLine("</tr>");
			stringBuilder.AppendLine("</table>");
			context.Response.Clear();
			context.Response.Buffer = false;
			context.Response.Charset = "GB2312";
			HttpResponse response = context.Response;
			dateTime = DateTime.Now;
			response.AppendHeader("Content-Disposition", "attachment;filename=MemberDetails_" + dateTime.ToString("yyyyMMddHHmmss") + ".xls");
			context.Response.ContentEncoding = Encoding.GetEncoding("GB2312");
			context.Response.ContentType = "application/ms-excel";
			context.Response.Write(stringBuilder.ToString());
			context.Response.End();
		}
	}
}
