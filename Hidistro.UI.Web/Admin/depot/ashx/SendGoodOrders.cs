using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Sales;
using Hidistro.UI.Web.Admin.depot.Models;
using Hidistro.UI.Web.ashxBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Admin.depot.ashx
{
	public class SendGoodOrders : AdminBaseHandler
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
			case "delete":
				this.Delete(context);
				break;
			case "exportrtoexcel":
				this.ExportToExcel(context);
				break;
			default:
				throw new HidistroAshxException("错误的参数");
			}
		}

		private void GetList(HttpContext context)
		{
			SendGoodOrderQuery query = this.getQuery(context);
			SendGoodOrders<Dictionary<string, object>> dataList = this.GetDataList(query);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private SendGoodOrderQuery getQuery(HttpContext context)
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
			if (!string.IsNullOrEmpty(context.Request["StoreId"]))
			{
				storeId = context.Request["StoreId"].ToInt(0);
			}
			SendGoodOrderQuery sendGoodOrderQuery = new SendGoodOrderQuery();
			if (!string.IsNullOrEmpty(context.Request["StartDate"]))
			{
				sendGoodOrderQuery.ShippingStartDate = base.GetDateTimeParam(context, "StartDate").Value;
			}
			if (!string.IsNullOrEmpty(context.Request["EndDate"]))
			{
				sendGoodOrderQuery.ShippingEndDate = base.GetDateTimeParam(context, "EndDate").Value;
			}
			sendGoodOrderQuery.StoreId = storeId;
			sendGoodOrderQuery.PageIndex = num;
			sendGoodOrderQuery.PageSize = num2;
			sendGoodOrderQuery.SortOrder = SortAction.Desc;
			return sendGoodOrderQuery;
		}

		private SendGoodOrders<Dictionary<string, object>> GetDataList(SendGoodOrderQuery query)
		{
			this.setting = SettingsManager.GetMasterSettings();
			SendGoodOrders<Dictionary<string, object>> sendGoodOrders = new SendGoodOrders<Dictionary<string, object>>();
			if (query != null)
			{
				DbQueryResult storeSendGoodOrders = StoresHelper.GetStoreSendGoodOrders(query);
				List<Dictionary<string, object>> list = DataHelper.DataTableToDictionary(storeSendGoodOrders.Data);
				foreach (Dictionary<string, object> item in list)
				{
					OrderInfo orderInfo = OrderHelper.GetOrderInfo(item["OrderId"].ToString());
					Dictionary<string, object> dictionary = item;
					DateTime dateTime = orderInfo.ShippingDate;
					dictionary.Add("ShippingDateStr", dateTime.ToString("yyyy-MM-dd HH:mm:ss"));
					Dictionary<string, object> dictionary2 = item;
					dateTime = orderInfo.OrderDate;
					dictionary2.Add("OrderDateStr", dateTime.ToString("yyyy-MM-dd HH:mm:ss"));
					item.Add("OrderTotalStr", orderInfo.GetPayTotal());
					item.Add("OrderProfitStr", orderInfo.GetProfit());
				}
				decimal orderSummaryTotal = default(decimal);
				decimal orderSummaryProfit = default(decimal);
				StoresHelper.GetStoreSendGoodTotalAmount(query, out orderSummaryTotal, out orderSummaryProfit);
				sendGoodOrders.OrderSummaryTotal = orderSummaryTotal;
				sendGoodOrders.OrderSummaryProfit = orderSummaryProfit;
				sendGoodOrders.rows = list;
				sendGoodOrders.total = storeSendGoodOrders.TotalRecords;
			}
			return sendGoodOrders;
		}

		public void Delete(HttpContext context)
		{
			string text = context.Request.Form["ids"];
			if (string.IsNullOrEmpty(text))
			{
				throw new HidistroAshxException("请选择要删除的数据");
			}
			string[] array = text.Split(',');
			foreach (string obj in array)
			{
				MarketingImagesInfo marketingImagesInfo = MarketingImagesHelper.GetMarketingImagesInfo(obj.ToInt(0));
				if (marketingImagesInfo != null)
				{
					string text2 = marketingImagesInfo.ImageUrl.ToNullString().ToLower();
					if (!text2.StartsWith("http://") && !text2.StartsWith("https://") && File.Exists(context.Server.MapPath(marketingImagesInfo.ImageUrl)))
					{
						File.Delete(context.Server.MapPath(marketingImagesInfo.ImageUrl));
					}
					MarketingImagesHelper.DeleteMarketingImages(obj.ToInt(0));
				}
			}
			base.ReturnSuccessResult(context, "删除成功！", 0, true);
		}

		private void ExportToExcel(HttpContext context)
		{
			SendGoodOrderQuery query = this.getQuery(context);
			DbQueryResult storeSendGoodOrdersNoPage = StoresHelper.GetStoreSendGoodOrdersNoPage(query);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
			stringBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
			stringBuilder.AppendLine("<td>发货时间</td>");
			stringBuilder.AppendLine("<td>下单时间</td>");
			stringBuilder.AppendLine("<td>订单编号</td>");
			stringBuilder.AppendLine("<td>用户名</td>");
			stringBuilder.AppendLine("<td>收货人</td>");
			stringBuilder.AppendLine("<td>订单金额</td>");
			stringBuilder.AppendLine("<td>利润</td>");
			stringBuilder.AppendLine("</tr>");
			decimal d = default(decimal);
			decimal d2 = default(decimal);
			DataTable data = storeSendGoodOrdersNoPage.Data;
			foreach (DataRow row in data.Rows)
			{
				OrderInfo orderInfo = OrderHelper.GetOrderInfo(row["OrderId"].ToNullString());
				d += orderInfo.GetTotal(false);
				d2 += orderInfo.GetProfit();
				stringBuilder.AppendLine("<tr>");
				StringBuilder stringBuilder2 = stringBuilder;
				DateTime value = row["ShippingDate"].ToDateTime().Value;
				stringBuilder2.AppendLine("<td>" + value.ToString("yyyy-MM-dd HH:mm:ss") + "</td>");
				StringBuilder stringBuilder3 = stringBuilder;
				value = row["OrderDate"].ToDateTime().Value;
				stringBuilder3.AppendLine("<td>" + value.ToString("yyyy-MM-dd HH:mm:ss") + "</td>");
				stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + row["OrderId"].ToNullString() + "</td>");
				stringBuilder.AppendLine("<td>" + row["Username"].ToNullString() + "</td>");
				stringBuilder.AppendLine("<td>" + row["ShipTo"].ToNullString() + "</td>");
				StringBuilder stringBuilder4 = stringBuilder;
				decimal num = orderInfo.GetTotal(false);
				stringBuilder4.AppendLine("<td>" + num.ToString("N2") + "</td>");
				StringBuilder stringBuilder5 = stringBuilder;
				num = orderInfo.GetProfit();
				stringBuilder5.AppendLine("<td>" + num.ToString("N2") + "</td>");
				stringBuilder.AppendLine("</tr>");
			}
			stringBuilder.AppendLine("<tr>");
			stringBuilder.AppendLine("<td>订单总金额：" + d.ToString("N2") + "</td>");
			stringBuilder.AppendLine("<td>订单总利润：" + d2.ToString("N2") + "</td>");
			stringBuilder.AppendLine("<td></td>");
			stringBuilder.AppendLine("</tr>");
			stringBuilder.AppendLine("</table>");
			context.Response.Clear();
			context.Response.Buffer = false;
			context.Response.Charset = "GB2312";
			context.Response.AppendHeader("Content-Disposition", "attachment;filename=发货统计.xls");
			context.Response.ContentEncoding = Encoding.GetEncoding("GB2312");
			context.Response.ContentType = "application/ms-excel";
			context.Response.Write(stringBuilder.ToString());
			context.Response.End();
		}
	}
}
