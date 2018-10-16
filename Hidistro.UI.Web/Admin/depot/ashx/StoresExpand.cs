using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Depot;
using Hidistro.SaleSystem.Depot;
using Hidistro.UI.Web.Admin.depot.Models;
using Hidistro.UI.Web.ashxBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Admin.depot.ashx
{
	public class StoresExpand : AdminBaseHandler
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

		private StoresQuery getQuery(HttpContext context)
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
			StoresQuery storesQuery = new StoresQuery();
			storesQuery.UserName = context.Request["UserName"];
			storesQuery.StoreName = context.Request["StoreName"];
			storesQuery.StoreName = storesQuery.StoreName.Trim();
			storesQuery.SortOrder = SortAction.Desc;
			storesQuery.PageIndex = num;
			storesQuery.PageSize = num2;
			return storesQuery;
		}

		private void GetList(HttpContext context)
		{
			StoresQuery query = this.getQuery(context);
			MemberDetailsModel<Dictionary<string, object>> dataList = this.GetDataList(query);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private MemberDetailsModel<Dictionary<string, object>> GetDataList(StoresQuery query)
		{
			MemberDetailsModel<Dictionary<string, object>> memberDetailsModel = new MemberDetailsModel<Dictionary<string, object>>();
			if (query != null)
			{
				DbQueryResult storeExpand = StoresHelper.GetStoreExpand(query);
				memberDetailsModel.rows = DataHelper.DataTableToDictionary(storeExpand.Data);
				memberDetailsModel.total = storeExpand.TotalRecords;
			}
			return memberDetailsModel;
		}

		private void ExportToExcel(HttpContext context)
		{
			try
			{
				StoresQuery query = this.getQuery(context);
				query.PageIndex = 1;
				query.PageSize = 2147483647;
				DbQueryResult storeExpand = StoresHelper.GetStoreExpand(query);
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
				stringBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
				stringBuilder.AppendLine("<td>门店</td>");
				stringBuilder.AppendLine("<td>店员数</td>");
				stringBuilder.AppendLine("<td>累计发展会员数</td>");
				stringBuilder.AppendLine("<td>累计会员消费</td>");
				stringBuilder.AppendLine("<td>累计发展会员订单数</td>");
				stringBuilder.AppendLine("</tr>");
				DataTable data = storeExpand.Data;
				foreach (DataRow row in data.Rows)
				{
					stringBuilder.AppendLine("<tr>");
					stringBuilder.AppendLine("<td>" + row.Field<string>("StoreName") + "</td>");
					stringBuilder.AppendLine("<td>" + row.Field<int>("ManagerCount") + "</td>");
					stringBuilder.AppendLine("<td>" + row.Field<int>("MemberCount") + "</td>");
					stringBuilder.AppendLine("<td>" + row.Field<decimal>("ConsumeTotals").F2ToString("f2") + "</td>");
					stringBuilder.AppendLine("<td>" + row.Field<int>("OrderNumbers") + "</td>");
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
	}
}
