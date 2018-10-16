using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.Admin.Report
{
	[PrivilegeCheck(Privilege.MembertAnalysis)]
	public class MemberReport : AdminPage
	{
		protected HtmlInputHidden hiJson;

		protected void Page_Load(object sender, EventArgs e)
		{
		}

		public void btnExportExcel_Click(object sender, EventArgs e)
		{
			string value = this.hiJson.Value;
			IsoDateTimeConverter isoDateTimeConverter = new IsoDateTimeConverter();
			isoDateTimeConverter.DateTimeFormat = "yyyy-MM-dd";
			List<UserStatistics> list = JsonConvert.DeserializeObject<List<UserStatistics>>(value, new JsonConverter[1]
			{
				isoDateTimeConverter
			});
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<table border='1'>");
			stringBuilder.Append("<thead><tr>");
			stringBuilder.Append("<th>时间</th>");
			stringBuilder.Append("<th>会员数</th>");
			StringBuilder stringBuilder2 = new StringBuilder();
			DateTime dateTime;
			foreach (UserStatistics item in list)
			{
				stringBuilder2.Append("<tr>");
				StringBuilder stringBuilder3 = stringBuilder2;
				dateTime = item.Time;
				stringBuilder3.Append(this.GetXLSFieldsTD(dateTime.ToString("yy-MM-dd"), true));
				stringBuilder2.Append(this.GetXLSFieldsTD(item.UserCounts, true));
				stringBuilder2.Append("</tr>");
			}
			stringBuilder.AppendFormat("<tbody>{0}</tbody></table>", stringBuilder2.ToString());
			StringWriter stringWriter = new StringWriter();
			stringWriter.Write(stringBuilder);
			HttpResponse response = base.Response;
			StringBuilder stringBuilder4 = stringWriter.GetStringBuilder();
			dateTime = DateTime.Now;
			MemberReport.DownloadFile(response, stringBuilder4, "MemberAdded" + dateTime.ToString("yyyyMMddhhmmss") + ".xls");
			stringWriter.Close();
			base.Response.End();
		}

		public static void DownloadFile(HttpResponse argResp, StringBuilder argFileStream, string strFileName)
		{
			try
			{
				string value = "attachment; filename=" + strFileName;
				argResp.AppendHeader("Content-Disposition", value);
				argResp.ContentType = "application/ms-excel";
				argResp.ContentEncoding = Encoding.GetEncoding("UTF-8");
				argResp.Write(argFileStream);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private string GetXLSFieldsTD(object argFields, bool istext)
		{
			if (argFields == null)
			{
				argFields = string.Empty;
			}
			else
			{
				string a = argFields.GetType().ToString();
				if (a == "System.DateTime")
				{
					DateTime? nullable = argFields.ToDateTime();
					argFields = ((!nullable.HasValue || nullable.Equals("0001/1/1 0:00:00")) ? "" : argFields);
				}
			}
			string arg = istext ? " style='vnd.ms-excel.numberformat:@'" : "";
			return $"<td{arg}>{argFields}</td>";
		}
	}
}
