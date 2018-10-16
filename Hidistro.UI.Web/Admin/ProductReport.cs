using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities.Statistics;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Statistics;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.ProductAnalysis)]
	public class ProductReport : AdminPage
	{
		protected HtmlInputHidden hidLastConsumeTime;

		protected HtmlInputHidden hidStartDate;

		protected HtmlInputHidden hidEndDate;

		protected HtmlInputHidden hidOrderby;

		protected HtmlInputHidden hidOrderAction;

		protected void Page_Load(object sender, EventArgs e)
		{
		}

		public void ExportToExcle(object sender, EventArgs e)
		{
			string text = "ProductReport.xls";
			int num = this.hidLastConsumeTime.Value.ToInt(0);
			if (!Enum.IsDefined(typeof(EnumConsumeTime), num))
			{
				num = 1;
			}
			DateTime dateTime = default(DateTime);
			DateTime dateTime2;
			if (!DateTime.TryParse(this.hidStartDate.Value, out dateTime))
			{
				dateTime2 = DateTime.Now;
				dateTime = dateTime2.AddDays(-1.0);
			}
			DateTime dateTime3 = default(DateTime);
			if (!DateTime.TryParse(this.hidEndDate.Value, out dateTime3))
			{
				dateTime2 = DateTime.Now;
				dateTime3 = dateTime2.AddDays(-1.0);
			}
			EnumConsumeTime lastConsumeTime;
			switch (num)
			{
			case 8:
				if (dateTime3 < dateTime)
				{
					this.ShowMsg("错误的时间范围", false);
					return;
				}
				lastConsumeTime = EnumConsumeTime.custom;
				text = "ProductReport" + dateTime.ToString("yyyyMMdd") + "-" + dateTime3.ToString("yyyyMMdd") + ".xls";
				break;
			case 4:
			{
				dateTime2 = DateTime.Now;
				dateTime2 = dateTime2.Date;
				dateTime3 = dateTime2.AddDays(-1.0);
				dateTime = dateTime3.AddDays(-30.0);
				lastConsumeTime = EnumConsumeTime.inOneMonth;
				string[] obj2 = new string[5]
				{
					"ProductReport",
					dateTime.ToString("yyyyMMdd"),
					"-",
					null,
					null
				};
				dateTime2 = dateTime3.AddDays(-1.0);
				obj2[3] = dateTime2.ToString("yyyyMMdd");
				obj2[4] = ".xls";
				text = string.Concat(obj2);
				break;
			}
			case 2:
			{
				dateTime2 = DateTime.Now;
				dateTime2 = dateTime2.Date;
				dateTime3 = dateTime2.AddDays(-1.0);
				dateTime = dateTime3.AddDays(-7.0);
				lastConsumeTime = EnumConsumeTime.inOneWeek;
				string[] obj = new string[5]
				{
					"ProductReport",
					dateTime.ToString("yyyyMMdd"),
					"-",
					null,
					null
				};
				dateTime2 = dateTime3.AddDays(-1.0);
				obj[3] = dateTime2.ToString("yyyyMMdd");
				obj[4] = ".xls";
				text = string.Concat(obj);
				break;
			}
			default:
				dateTime2 = DateTime.Now;
				dateTime2 = dateTime2.Date;
				dateTime3 = dateTime2.AddDays(-1.0);
				dateTime2 = DateTime.Now;
				dateTime2 = dateTime2.Date;
				dateTime3 = dateTime2.AddDays(-1.0);
				lastConsumeTime = EnumConsumeTime.yesterday;
				text = "ProductReport" + dateTime.ToString("yyyyMMdd") + ".xls";
				break;
			}
			ProductStatisticsQuery productStatisticsQuery = new ProductStatisticsQuery();
			productStatisticsQuery.CustomConsumeEndTime = dateTime3;
			productStatisticsQuery.CustomConsumeStartTime = dateTime;
			productStatisticsQuery.LastConsumeTime = lastConsumeTime;
			string a = this.hidOrderby.Value.ToNullString().ToLower();
			string text2 = this.hidOrderAction.Value.ToNullString().ToLower();
			if (text2 != "pv" && text2 != "uv" && text2 != "paymentnum" && text2 != "salequantity" && text2 != "saleamount" && text2 != "productconversionrate")
			{
				text2 = "pv";
			}
			productStatisticsQuery.SortBy = text2;
			productStatisticsQuery.SortOrder = SortAction.Desc;
			if (a == "asc")
			{
				productStatisticsQuery.SortOrder = SortAction.Asc;
			}
			IList<ProductStatisticsInfo> productStatisticsDataNoPage = ProductStatisticsHelper.GetProductStatisticsDataNoPage(productStatisticsQuery);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
			stringBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
			stringBuilder.AppendLine("<td>商品名称</td>");
			stringBuilder.AppendLine("<td>浏览量</td>");
			stringBuilder.AppendLine("<td>浏览人数</td>");
			stringBuilder.AppendLine("<td>付款人数</td>");
			stringBuilder.AppendLine("<td>单品转化率</td>");
			stringBuilder.AppendLine("<td>销售数量</td>");
			stringBuilder.AppendLine("<td>销售金额</td>");
			stringBuilder.AppendLine("</tr>");
			foreach (ProductStatisticsInfo item in productStatisticsDataNoPage)
			{
				stringBuilder.AppendLine("<tr>");
				stringBuilder.AppendLine("<td>" + item.ProductName + "</td>");
				stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + item.PV + "</td>");
				stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + item.UV + "</td>");
				stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + item.PaymentNum + "</td>");
				stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + item.ProductConversionRate.F2ToString("f2") + "%</td>");
				stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + item.SaleQuantity + "</td>");
				stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + item.SaleAmount.F2ToString("f2") + "</td>");
				stringBuilder.AppendLine("</tr>");
			}
			stringBuilder.AppendLine("</table>");
			this.Page.Response.Clear();
			this.Page.Response.Buffer = false;
			this.Page.Response.Charset = "UTF-8";
			this.Page.Response.AppendHeader("Content-Disposition", ("attachment;filename=" + text) ?? "");
			this.Page.Response.ContentEncoding = Encoding.GetEncoding("UTF-8");
			this.Page.Response.ContentType = "application/ms-excel";
			this.Page.EnableViewState = false;
			this.Page.Response.Write(stringBuilder.ToString());
			this.Page.Response.End();
		}
	}
}
