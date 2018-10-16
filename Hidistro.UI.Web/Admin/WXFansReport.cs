using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Statistics;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Statistics;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	public class WXFansReport : AdminPage
	{
		protected HtmlInputHidden hidLastConsumeTime;

		protected HtmlInputHidden hidStartDate;

		protected HtmlInputHidden hidEndDate;

		protected HtmlGenericControl ReportDataPanel;

		protected HtmlGenericControl InitDataPanel;

		protected Literal litMsg;

		protected HtmlInputButton btnSynchroData;

		[PrivilegeCheck(Privilege.WachaWeChatFanGrowthAnalysis)]
		protected void Page_Load(object sender, EventArgs e)
		{
			if (HiContext.Current.SiteSettings.IsInitWXFansData)
			{
				this.ReportDataPanel.Visible = true;
				this.InitDataPanel.Visible = false;
			}
			else
			{
				if (string.IsNullOrEmpty(HiContext.Current.SiteSettings.WeixinAppId) || string.IsNullOrEmpty(HiContext.Current.SiteSettings.WeixinAppSecret))
				{
					this.litMsg.Text = "请先配置好微信AppId和AppSecret才能进行同步操作。";
					this.btnSynchroData.Visible = false;
				}
				this.ReportDataPanel.Visible = false;
				this.InitDataPanel.Visible = true;
			}
		}

		public void ExportToExcle(object sender, EventArgs e)
		{
			string text = "WXFansReport.xls";
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
				text = "WXFansReport" + dateTime.ToString("yyyyMMdd") + "-" + dateTime3.ToString("yyyyMMdd") + ".xls";
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
					"WXFansReport",
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
					"WXFansReport",
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
				text = "WXFansReport" + dateTime.ToString("yyyyMMdd") + ".xls";
				break;
			}
			WXFansStatisticsQuery wXFansStatisticsQuery = new WXFansStatisticsQuery();
			wXFansStatisticsQuery.CustomConsumeEndTime = dateTime3;
			wXFansStatisticsQuery.CustomConsumeStartTime = dateTime;
			wXFansStatisticsQuery.LastConsumeTime = lastConsumeTime;
			IList<WXFansStatisticsInfo> wxFansListNoPage = WXFansHelper.GetWxFansListNoPage(wXFansStatisticsQuery);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
			stringBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
			stringBuilder.AppendLine("<td>日期</td>");
			stringBuilder.AppendLine("<td>新关注人数</td>");
			stringBuilder.AppendLine("<td>取消关注人数</td>");
			stringBuilder.AppendLine("<td>净增长人数</td>");
			stringBuilder.AppendLine("<td>累计关注人数</td>");
			stringBuilder.AppendLine("</tr>");
			foreach (WXFansStatisticsInfo item in wxFansListNoPage)
			{
				stringBuilder.AppendLine("<tr>");
				StringBuilder stringBuilder2 = stringBuilder;
				dateTime2 = item.StatisticalDate;
				stringBuilder2.AppendLine("<td>" + dateTime2.ToString("yyyy-MM-dd") + "</td>");
				stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + item.NewUser + "</td>");
				stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + item.CancelUser + "</td>");
				stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + item.NetGrowthUser + "</td>");
				stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + item.CumulateUser + "</td>");
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
