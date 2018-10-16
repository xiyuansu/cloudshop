using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Members;
using Hidistro.UI.Web.ashxBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Admin.member.ashx
{
	public class MemberPointDetails : AdminBaseHandler
	{
		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			if (!string.IsNullOrWhiteSpace(base.action))
			{
				base.action = base.action.ToLower();
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

		public void ExportToExcel(HttpContext context)
		{
			int value = base.GetIntParam(context, "UserId", false).Value;
			IList<PointDetailInfo> userPointsNoPage = MemberHelper.GetUserPointsNoPage(value, base.GetIntParam(context, "TradeType", true).ToString());
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<table border='1'>");
			stringBuilder.Append("<thead><tr>");
			stringBuilder.Append("<th>会员名</th>");
			stringBuilder.Append("<th>积分来源</th>");
			stringBuilder.Append("<th>积分变化</th>");
			stringBuilder.Append("<th>可用积分余额</th>");
			stringBuilder.Append("<th>备注</th>");
			stringBuilder.Append("</tr></thead>");
			StringBuilder stringBuilder2 = new StringBuilder();
			foreach (PointDetailInfo item in userPointsNoPage)
			{
				stringBuilder2.Append("<tr>");
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.UserName, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.TradeTypeName, false));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD((item.Increased > 0) ? ("+" + item.Increased) : ("-" + item.Reduced), false));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.Points, false));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.Remark, true));
				stringBuilder2.Append("</tr>");
			}
			stringBuilder.AppendFormat("<tbody>{0}</tbody></table>", stringBuilder2.ToString());
			StringWriter stringWriter = new StringWriter();
			stringWriter.Write(stringBuilder);
			DownloadHelper.DownloadFile(context.Response, stringWriter.GetStringBuilder(), "PointDetail" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls");
			stringWriter.Close();
			context.Response.End();
		}

		private void GetList(HttpContext context)
		{
			int value = base.GetIntParam(context, "UserId", false).Value;
			int? intParam = base.GetIntParam(context, "TradeType", true);
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList(value, intParam, base.CurrentPageIndex);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList(int userId, int? typeId, int pageindex = 1)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			if (userId > 0)
			{
				PointQuery pointQuery = new PointQuery();
				pointQuery.PageIndex = pageindex;
				pointQuery.PageSize = 10;
				pointQuery.UserId = userId;
				if (typeId.HasValue && typeId.Value > 0)
				{
					pointQuery.TradeType = (PointTradeType)typeId.Value;
				}
				PageModel<PointDetailInfo> userPoints = MemberHelper.GetUserPoints(pointQuery);
				dataGridViewModel.rows = DataHelper.ListToDictionary(userPoints.Models);
				dataGridViewModel.total = userPoints.Total;
			}
			return dataGridViewModel;
		}
	}
}
