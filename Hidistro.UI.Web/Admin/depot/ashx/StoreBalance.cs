using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Depot;
using Hidistro.SaleSystem.Depot;
using Hidistro.UI.Web.Admin.depot.Models;
using Hidistro.UI.Web.ashxBase;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Admin.depot.ashx
{
	public class StoreBalance : AdminBaseHandler
	{
		private SiteSettings setting;

		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			if (string.IsNullOrWhiteSpace(base.action))
			{
				throw new HidistroAshxException("错误的参数");
			}
			switch (base.action)
			{
			case "getlist":
				this.GetList(context);
				break;
			case "exportrtoexcel":
				this.ExportToExcel(context);
				break;
			case "GetBalanceOnLineDetailById":
				this.GetBalanceOnLineDetailById(context);
				break;
			case "GetBalanceOffLineDetailById":
				this.GetBalanceOffLineDetailById(context);
				break;
			default:
				throw new HidistroAshxException("错误的参数");
			}
		}

		private void GetBalanceOffLineDetailById(HttpContext context)
		{
			StoreBalanceOffLineOrderInfo balanceOffOrderDetails = StoreBalanceHelper.GetBalanceOffOrderDetails(context.Request.QueryString["id"].ToInt(0));
			IsoDateTimeConverter isoDateTimeConverter = new IsoDateTimeConverter();
			isoDateTimeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
			string s = JsonConvert.SerializeObject(balanceOffOrderDetails, Formatting.Indented, isoDateTimeConverter);
			context.Response.Write(s);
			context.Response.End();
		}

		private void GetBalanceOnLineDetailById(HttpContext context)
		{
			StoreBalanceOrderInfo balanceDetails = StoreBalanceHelper.GetBalanceDetails(context.Request.QueryString["id"].ToInt(0));
			IsoDateTimeConverter isoDateTimeConverter = new IsoDateTimeConverter();
			isoDateTimeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
			string s = JsonConvert.SerializeObject(balanceDetails, Formatting.Indented, isoDateTimeConverter);
			context.Response.Write(s);
			context.Response.End();
		}

		private void GetList(HttpContext context)
		{
			StoreBalanceQuery query = this.getQuery(context);
			StoerBalanceModel<Dictionary<string, object>> dataList = this.GetDataList(query);
			IsoDateTimeConverter isoDateTimeConverter = new IsoDateTimeConverter();
			isoDateTimeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
			string s = JsonConvert.SerializeObject(dataList, Formatting.Indented, isoDateTimeConverter);
			context.Response.Write(s);
			context.Response.End();
		}

		private StoreBalanceQuery getQuery(HttpContext context)
		{
			int num = 1;
			int num2 = 10;
			int num3 = 0;
			int storeId = 0;
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
			if (!string.IsNullOrEmpty(context.Request["IsStoreCollect"]))
			{
				num3 = context.Request["IsStoreCollect"].ToInt(0);
			}
			if (!string.IsNullOrEmpty(context.Request["StoreId"]))
			{
				storeId = context.Request["StoreId"].ToInt(0);
			}
			StoreBalanceQuery storeBalanceQuery = new StoreBalanceQuery();
			if (!string.IsNullOrEmpty(context.Request["StartDate"]) && !string.IsNullOrEmpty(context.Request["EndDate"]))
			{
				storeBalanceQuery.StartDate = base.GetDateTimeParam(context, "StartDate").Value;
				storeBalanceQuery.EndDate = base.GetDateTimeParam(context, "EndDate").Value;
			}
			else
			{
				base.ReturnFailResult(context, "开始和结束日期不能为空！", -1, true);
			}
			storeBalanceQuery.StoreId = storeId;
			storeBalanceQuery.PageIndex = num;
			storeBalanceQuery.PageSize = num2;
			storeBalanceQuery.SortBy = "FinishDate";
			storeBalanceQuery.SortOrder = SortAction.Desc;
			return storeBalanceQuery;
		}

		private StoerBalanceModel<Dictionary<string, object>> GetDataList(StoreBalanceQuery query)
		{
			StoerBalanceModel<Dictionary<string, object>> stoerBalanceModel = new StoerBalanceModel<Dictionary<string, object>>();
			if (query != null)
			{
				DbQueryResult storeBalanceOverOrders = StoresHelper.GetStoreBalanceOverOrders(query);
				List<Dictionary<string, object>> rows = DataHelper.DataTableToDictionary(storeBalanceOverOrders.Data);
				int storeId = query.StoreId;
				DateTime value = query.StartDate.Value;
				DateTime? endDate = query.EndDate;
				decimal num = stoerBalanceModel.totalAmount = StoresHelper.GetStoreBalanceOrderTotal(storeId, value, endDate.Value);
				stoerBalanceModel.rows = rows;
				stoerBalanceModel.total = storeBalanceOverOrders.TotalRecords;
			}
			return stoerBalanceModel;
		}

		private void ExportToExcel(HttpContext context)
		{
			StoreBalanceQuery query = this.getQuery(context);
			query.PageIndex = 1;
			query.PageSize = 2147483647;
			DbQueryResult storeBalanceOverOrders = StoresHelper.GetStoreBalanceOverOrders(query);
			decimal storeBalanceOrderTotal = StoresHelper.GetStoreBalanceOrderTotal(query.StoreId, query.StartDate.Value, query.EndDate.Value);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
			stringBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
			stringBuilder.AppendLine("<td>门店</td>");
			stringBuilder.AppendLine("<td>结算时间</td>");
			stringBuilder.AppendLine("<td>订单编号</td>");
			stringBuilder.AppendLine("<td>订单实付</td>");
			stringBuilder.AppendLine("<td>退款金额</td>");
			stringBuilder.AppendLine("<td>平台佣金</td>");
			stringBuilder.AppendLine("<td>结算金额</td>");
			stringBuilder.AppendLine("<td>收款方</td>");
			stringBuilder.AppendLine("<td>运费</td>");
			stringBuilder.AppendLine("<td>积分抵扣</td>");
			stringBuilder.AppendLine("<td>优惠券抵扣</td>");
			stringBuilder.AppendLine("</tr>");
			DataTable data = storeBalanceOverOrders.Data;
			DateTime dateTime;
			foreach (DataRow row in data.Rows)
			{
				stringBuilder.AppendLine("<tr>");
				stringBuilder.AppendLine("<td>" + row.Field<string>("StoreName") + " </td>");
				StringBuilder stringBuilder2 = stringBuilder;
				dateTime = row.Field<DateTime>("CreateTime");
				stringBuilder2.AppendLine("<td>" + dateTime.ToString("yyyy-MM-dd HH:mm:ss") + "</td>");
				stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + row.Field<string>("TradeNo") + "</td>");
				stringBuilder.AppendLine("<td>" + row["OrderTotal"].ToDecimal(0).F2ToString("f2") + "</td>");
				stringBuilder.AppendLine("<td>" + row["RefundAmount"].ToDecimal(0).F2ToString("f2") + "</td>");
				stringBuilder.AppendLine("<td>" + row["PlatCommission"].ToDecimal(0).F2ToString("f2") + "</td>");
				stringBuilder.AppendLine("<td>" + row["Income"].ToDecimal(0).F2ToString("f2") + "</td>");
				if (row["CollectByStore"].ToString() == "1")
				{
					stringBuilder.AppendLine("<td>门店</td>");
				}
				else
				{
					stringBuilder.AppendLine("<td>平台</td>");
				}
				stringBuilder.AppendLine("<td>" + row["Freight"].ToDecimal(0).F2ToString("f2") + "</td>");
				stringBuilder.AppendLine("<td>" + row["DeductionMoney"].ToDecimal(0).F2ToString("f2") + "</td>");
				stringBuilder.AppendLine("<td>" + row["CouponValue"].ToDecimal(0).F2ToString("f2") + "</td>");
				stringBuilder.AppendLine("</tr>");
			}
			stringBuilder.AppendLine("<tr>");
			stringBuilder.AppendLine("<td>总结算金额：" + storeBalanceOrderTotal.F2ToString("f2") + "</td>");
			stringBuilder.AppendLine("<td></td>");
			stringBuilder.AppendLine("</tr>");
			stringBuilder.AppendLine("</table>");
			context.Response.Clear();
			context.Response.Buffer = false;
			context.Response.Charset = "GB2312";
			HttpResponse response = context.Response;
			dateTime = DateTime.Now;
			response.AppendHeader("Content-Disposition", "attachment;filename=StoreBalance_" + dateTime.ToString("yyyyMMddHHmmss") + ".xls");
			context.Response.ContentEncoding = Encoding.GetEncoding("GB2312");
			context.Response.ContentType = "application/ms-excel";
			context.Response.Write(stringBuilder.ToString());
			context.Response.End();
		}
	}
}
