using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Web.Admin.depot.Models;
using Hidistro.UI.Web.ashxBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Admin.depot.ashx
{
	public class SalesclerkList : AdminBaseHandler
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

		private void ExportToExcel(HttpContext context)
		{
			try
			{
				ManagerQuery query = this.GetQuery(context);
				query.PageIndex = 1;
				query.PageSize = 2147483647;
				DbQueryResult managersExpand = ManagerHelper.GetManagersExpand(query);
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
				stringBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
				stringBuilder.AppendLine("<td>店员</td>");
				stringBuilder.AppendLine("<td>累计发展会员数</td>");
				stringBuilder.AppendLine("<td>累计会员消费</td>");
				stringBuilder.AppendLine("</tr>");
				DataTable data = managersExpand.Data;
				foreach (DataRow row in data.Rows)
				{
					stringBuilder.AppendLine("<tr>");
					stringBuilder.AppendLine("<td>" + row.Field<string>("UserName") + ((row.Field<int>("RoleId") == -1) ? "(门店管理员)" : "(导购)") + "</td>");
					stringBuilder.AppendLine("<td>" + row.Field<int>("MemberCount") + "</td>");
					stringBuilder.AppendLine("<td>" + row.Field<decimal>("ConsumeTotals").F2ToString("f2") + "</td>");
					stringBuilder.AppendLine("</tr>");
				}
				stringBuilder.AppendLine("<tr>");
				stringBuilder.AppendLine("<td></td>");
				stringBuilder.AppendLine("</tr>");
				stringBuilder.AppendLine("</table>");
				context.Response.Clear();
				context.Response.Buffer = false;
				context.Response.Charset = "GB2312";
				context.Response.AppendHeader("Content-Disposition", "attachment;filename=StoresExpand_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
				context.Response.ContentEncoding = Encoding.GetEncoding("GB2312");
				context.Response.ContentType = "application/ms-excel";
				context.Response.Write(stringBuilder.ToString());
				context.Response.End();
			}
			catch (Exception)
			{
				throw new HidistroAshxException("导出数据错误");
			}
		}

		public ManagerQuery GetQuery(HttpContext context)
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
			ManagerQuery managerQuery = new ManagerQuery();
			managerQuery.UserName = context.Request["UserName"];
			managerQuery.UserName = managerQuery.UserName.Trim();
			managerQuery.StoreId = base.GetIntParam(context, "storeId", false).Value;
			managerQuery.SortOrder = SortAction.Desc;
			managerQuery.PageIndex = num;
			managerQuery.PageSize = num2;
			return managerQuery;
		}

		private void GetList(HttpContext context)
		{
			ManagerQuery query = this.GetQuery(context);
			MemberDetailsModel<Dictionary<string, object>> dataList = this.GetDataList(query);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private MemberDetailsModel<Dictionary<string, object>> GetDataList(ManagerQuery query)
		{
			MemberDetailsModel<Dictionary<string, object>> memberDetailsModel = new MemberDetailsModel<Dictionary<string, object>>();
			if (query != null)
			{
				DbQueryResult managersExpand = ManagerHelper.GetManagersExpand(query);
				memberDetailsModel.rows = new List<Dictionary<string, object>>();
				memberDetailsModel.rows = DataHelper.DataTableToDictionary(managersExpand.Data);
				memberDetailsModel.total = managersExpand.TotalRecords;
				if (memberDetailsModel.rows.Count > 0)
				{
					memberDetailsModel.rows = (from a in memberDetailsModel.rows
					orderby Math.Abs(a["RoleId"].ToInt(0) + 1)
					select a).ToList();
				}
			}
			return memberDetailsModel;
		}
	}
}
