using Hidistro.Core;
using Hidistro.Entities.Statistics;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Statistics;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.Admin.Report
{
	[PrivilegeCheck(Privilege.TransactionAnalysis)]
	public class TransactionAnalysisReport : AdminPage
	{
		protected HtmlInputHidden hidLastConsumeTime;

		protected HtmlInputHidden hidStartDate;

		protected HtmlInputHidden hidEndDate;

		protected void Page_Load(object sender, EventArgs e)
		{
		}

		public void ExportToExcle(object sender, EventArgs e)
		{
			string text = "TransactionAnalysis.xls";
			int num = this.hidLastConsumeTime.Value.ToInt(0);
			DateTime dateTime3;
			DateTime dateTime2;
			EnumConsumeTime timeType;
			DateTime dateTime;
			switch (num)
			{
			case 8:
				dateTime3 = Convert.ToDateTime(this.hidStartDate.Value);
				dateTime2 = Convert.ToDateTime(this.hidEndDate.Value);
				if (!(dateTime2 < dateTime3))
				{
					timeType = EnumConsumeTime.custom;
					text = "TranAnalysis" + dateTime3.ToString("yyyyMMdd") + "-" + dateTime2.ToString("yyyyMMdd") + ".xls";
					break;
				}
				return;
			case 4:
			{
				dateTime = DateTime.Now;
				dateTime2 = dateTime.Date;
				dateTime3 = dateTime2.AddDays(-30.0);
				timeType = EnumConsumeTime.inOneMonth;
				string[] obj2 = new string[5]
				{
					"TranAnalysis",
					dateTime3.ToString("yyyyMMdd"),
					"-",
					null,
					null
				};
				dateTime = dateTime2.AddDays(-1.0);
				obj2[3] = dateTime.ToString("yyyyMMdd");
				obj2[4] = ".xls";
				text = string.Concat(obj2);
				break;
			}
			case 2:
			{
				dateTime = DateTime.Now;
				dateTime2 = dateTime.Date;
				dateTime3 = dateTime2.AddDays(-7.0);
				timeType = EnumConsumeTime.inOneWeek;
				string[] obj = new string[5]
				{
					"TranAnalysis",
					dateTime3.ToString("yyyyMMdd"),
					"-",
					null,
					null
				};
				dateTime = dateTime2.AddDays(-1.0);
				obj[3] = dateTime.ToString("yyyyMMdd");
				obj[4] = ".xls";
				text = string.Concat(obj);
				break;
			}
			default:
				dateTime = DateTime.Now;
				dateTime2 = dateTime.Date;
				dateTime = DateTime.Now;
				dateTime = dateTime.Date;
				dateTime3 = dateTime.AddDays(-1.0);
				timeType = EnumConsumeTime.yesterday;
				text = "TranAnalysis" + dateTime3.ToString("yyyyMMdd") + ".xls";
				break;
			}
			IList<OrderStatisticModel> list = TransactionAnalysisHelper.GetOrderDailyStatisticsList(timeType, dateTime3, dateTime2);
			if (dateTime3 < dateTime2)
			{
				DateTime dtDay = dateTime3;
				while ((dtDay <= dateTime2 && num == 8) || (dtDay < dateTime2 && num != 8))
				{
					if (list.FirstOrDefault((OrderStatisticModel c) => c.StatisticalDate == dtDay) == null)
					{
						OrderStatisticModel orderStatisticModel = new OrderStatisticModel();
						orderStatisticModel.StatisticalDate = dtDay;
						orderStatisticModel.PV = 0;
						orderStatisticModel.UV = 0;
						orderStatisticModel.OrderAmount = decimal.Zero;
						orderStatisticModel.OrderNum = 0;
						orderStatisticModel.OrderProductQuantity = 0;
						orderStatisticModel.OrderUserNum = 0;
						orderStatisticModel.PaymentAmount = decimal.Zero;
						orderStatisticModel.PaymentOrderNum = 0;
						orderStatisticModel.PaymentProductNum = 0;
						orderStatisticModel.PaymentUserNum = 0;
						list.Add(orderStatisticModel);
					}
					dtDay = dtDay.AddDays(1.0);
				}
				list = (from c in list
				orderby c.StatisticalDate
				select c).ToList();
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
			stringBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
			stringBuilder.AppendLine("<td>时间</td>");
			stringBuilder.AppendLine("<td>浏览人数</td>");
			stringBuilder.AppendLine("<td>下单人数</td>");
			stringBuilder.AppendLine("<td>订单数</td>");
			stringBuilder.AppendLine("<td>下单件数</td>");
			stringBuilder.AppendLine("<td>下单金额</td>");
			stringBuilder.AppendLine("<td>付款人数</td>");
			stringBuilder.AppendLine("<td>付款订单</td>");
			stringBuilder.AppendLine("<td>付款件数</td>");
			stringBuilder.AppendLine("<td>付款金额</td>");
			stringBuilder.AppendLine("<td>客单价</td>");
			stringBuilder.AppendLine("<td>下单转化率</td>");
			stringBuilder.AppendLine("<td>付款转化率</td>");
			stringBuilder.AppendLine("<td>交易转化率</td>");
			stringBuilder.AppendLine("</tr>");
			foreach (OrderStatisticModel item in list)
			{
				stringBuilder.AppendLine("<tr>");
				StringBuilder stringBuilder2 = stringBuilder;
				dateTime = item.StatisticalDate;
				stringBuilder2.AppendLine("<td>" + dateTime.ToString("yyyy/MM/dd") + "</td>");
				stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + item.UV + "</td>");
				stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + item.OrderUserNum + "</td>");
				stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + item.OrderNum + "</td>");
				stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + item.OrderProductQuantity + "</td>");
				stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + item.OrderAmount.F2ToString("f2") + "</td>");
				stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + item.PaymentUserNum + "</td>");
				stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + item.PaymentOrderNum + "</td>");
				stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + item.PaymentProductNum + "</td>");
				stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + item.PaymentAmount.F2ToString("f2") + "</td>");
				string str = "0";
				if (item.PaymentUserNum > 0)
				{
					str = (item.PaymentAmount / (decimal)item.PaymentUserNum).F2ToString("f2");
				}
				stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + str + "</td>");
				stringBuilder.AppendLine("<td>" + item.ConversionRate + "%</td>");
				stringBuilder.AppendLine("<td>" + item.PaymentRate + "%</td>");
				stringBuilder.AppendLine("<td>" + item.ClinchaDealRate + "%</td>");
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
