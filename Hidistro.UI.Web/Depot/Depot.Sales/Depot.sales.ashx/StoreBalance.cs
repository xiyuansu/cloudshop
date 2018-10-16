using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Depot;
using Hidistro.SaleSystem.Depot;
using Hidistro.UI.Web.ashxBase;
using Hidistro.UI.Web.Depot.sales.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Depot.sales.ashx
{
	public class StoreBalance : StoreAdminBaseHandler
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
				if (action == "exporttoexcel")
				{
					this.ExportToExcel(context);
					return;
				}
				throw new HidistroAshxException("错误的参数");
			}
			this.GetList(context);
		}

		private StoreBalanceQuery GetQuery(HttpContext context)
		{
			StoreBalanceQuery storeBalanceQuery = new StoreBalanceQuery();
			int pageIndex = 1;
			int pageSize = 10;
			string empty = string.Empty;
			if (string.IsNullOrEmpty(context.Request["StartDate"]))
			{
				throw new HidistroAshxException("错误的开始时间");
			}
			DateTime? dateTimeParam = base.GetDateTimeParam(context, "StartDate");
			if (!dateTimeParam.HasValue)
			{
				throw new HidistroAshxException("错误的开始时间");
			}
			storeBalanceQuery.StartDate = dateTimeParam.Value;
			if (string.IsNullOrEmpty(context.Request["EndDate"]))
			{
				throw new HidistroAshxException("错误的结束时间");
			}
			DateTime? dateTimeParam2 = base.GetDateTimeParam(context, "EndDate");
			if (!dateTimeParam2.HasValue)
			{
				throw new HidistroAshxException("错误的结束时间");
			}
			storeBalanceQuery.EndDate = dateTimeParam2.Value;
			empty = context.Request["IsStoreCollect"];
			if (!string.IsNullOrWhiteSpace(empty))
			{
				int value = base.GetIntParam(context, "IsStoreCollect", false).Value;
				storeBalanceQuery.IsStoreCollect = (value == 1);
			}
			empty = context.Request["page"];
			if (!string.IsNullOrWhiteSpace(empty))
			{
				try
				{
					pageIndex = int.Parse(empty);
				}
				catch
				{
					pageIndex = 1;
				}
			}
			empty = context.Request["rows"];
			if (!string.IsNullOrWhiteSpace(empty))
			{
				try
				{
					pageSize = int.Parse(empty);
				}
				catch
				{
					pageSize = 10;
				}
			}
			storeBalanceQuery.PageIndex = pageIndex;
			storeBalanceQuery.PageSize = pageSize;
			storeBalanceQuery.StoreId = base.CurrentManager.StoreId;
			storeBalanceQuery.IsCount = true;
			storeBalanceQuery.SortBy = "FinishDate";
			storeBalanceQuery.SortOrder = SortAction.Desc;
			return storeBalanceQuery;
		}

		private void GetList(HttpContext context)
		{
			StoreBalanceQuery query = this.GetQuery(context);
			StoerBalanceModel<Dictionary<string, object>> dataList = this.GetDataList(query);
			IsoDateTimeConverter isoDateTimeConverter = new IsoDateTimeConverter();
			isoDateTimeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
			string s = JsonConvert.SerializeObject(dataList, Formatting.Indented, isoDateTimeConverter);
			context.Response.Write(s);
			context.Response.End();
		}

		private StoerBalanceModel<Dictionary<string, object>> GetDataList(StoreBalanceQuery query)
		{
			StoerBalanceModel<Dictionary<string, object>> stoerBalanceModel = new StoerBalanceModel<Dictionary<string, object>>();
			DbQueryResult storeBalanceOverOrders = StoresHelper.GetStoreBalanceOverOrders(query);
			stoerBalanceModel.rows = DataHelper.DataTableToDictionary(storeBalanceOverOrders.Data);
			List<Dictionary<string, object>> rows = DataHelper.DataTableToDictionary(storeBalanceOverOrders.Data);
			int storeId = query.StoreId;
			DateTime value = query.StartDate.Value;
			DateTime? endDate = query.EndDate;
			decimal num = stoerBalanceModel.totalAmount = StoresHelper.GetStoreBalanceOrderTotal(storeId, value, endDate.Value, query.IsStoreCollect);
			stoerBalanceModel.rows = rows;
			stoerBalanceModel.total = storeBalanceOverOrders.TotalRecords;
			return stoerBalanceModel;
		}

		private void ExportToExcel(HttpContext context)
		{
			StoreBalanceQuery query = this.GetQuery(context);
			query.PageIndex = 1;
			query.PageSize = 2147483647;
			DbQueryResult storeBalanceOverOrders = StoresHelper.GetStoreBalanceOverOrders(query);
			decimal storeBalanceOrderTotal = StoresHelper.GetStoreBalanceOrderTotal(query.StoreId, query.StartDate.Value, query.EndDate.Value, query.IsStoreCollect);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
			stringBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
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
