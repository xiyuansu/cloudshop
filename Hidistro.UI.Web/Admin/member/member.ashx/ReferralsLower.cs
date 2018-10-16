using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Members;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Web.ashxBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Admin.member.ashx
{
	[PrivilegeCheck(Privilege.Referrals)]
	public class ReferralsLower : AdminBaseHandler
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
				if (action == "exportexcel")
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
			MemberQuery dataQuery = this.GetDataQuery(context);
			IList<SubMemberModel> subMemberExportData = MemberHelper.GetSubMemberExportData(dataQuery, value);
			StringBuilder stringBuilder = new StringBuilder();
			MemberInfo user = Users.GetUser(value);
			if (user != null)
			{
				stringBuilder.Append("<table border='1'>");
				stringBuilder.Append("<thead><tr>");
				stringBuilder.Append("<th>分销员</th>");
				stringBuilder.Append("<th>下级会员</th>");
				stringBuilder.Append("<th>注册时间</th>");
				stringBuilder.Append("<th>累计消费金额</th>");
				stringBuilder.Append("<th>获得佣金(含未结)</th>");
				stringBuilder.Append("</tr></thead>");
				StringBuilder stringBuilder2 = new StringBuilder();
				DateTime dateTime;
				foreach (SubMemberModel item in subMemberExportData)
				{
					stringBuilder2.Append("<tr>");
					stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(user.UserName, false));
					stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.SubUserName, false));
					StringBuilder stringBuilder3 = stringBuilder2;
					dateTime = item.RegisterTime;
					stringBuilder3.Append(ExcelHelper.GetXLSFieldsTD(dateTime.ToString("yyyy-MM-dd HH:mm:ss"), false));
					stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.ConsumeTotal.F2ToString("f2"), false));
					stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.CommissionTotal.F2ToString("f2"), false));
					stringBuilder2.Append("</tr>");
				}
				stringBuilder.AppendFormat("<tbody>{0}</tbody></table>", stringBuilder2.ToString());
				StringWriter stringWriter = new StringWriter();
				stringWriter.Write(stringBuilder);
				HttpResponse response = context.Response;
				StringBuilder stringBuilder4 = stringWriter.GetStringBuilder();
				dateTime = DateTime.Now;
				DownloadHelper.DownloadFile(response, stringBuilder4, "ReferralLowers" + dateTime.ToString("yyyyMMddhhmmss") + ".xls");
				stringWriter.Close();
				context.Response.End();
			}
		}

		private void GetList(HttpContext context)
		{
			string empty = string.Empty;
			MemberQuery dataQuery = this.GetDataQuery(context);
			int value = base.GetIntParam(context, "UserId", false).Value;
			DataGridViewModel<Dictionary<string, object>> referralRequest = this.GetReferralRequest(dataQuery, value);
			string s = base.SerializeObjectToJson(referralRequest);
			context.Response.Write(s);
			context.Response.End();
		}

		public MemberQuery GetDataQuery(HttpContext context)
		{
			string empty = string.Empty;
			MemberQuery memberQuery = new MemberQuery();
			empty = context.Request["Keywords"];
			int value = base.GetIntParam(context, "UserId", false).Value;
			memberQuery.UserName = empty;
			memberQuery.PageIndex = base.CurrentPageIndex;
			memberQuery.PageSize = base.CurrentPageSize;
			return memberQuery;
		}

		private DataGridViewModel<Dictionary<string, object>> GetReferralRequest(MemberQuery query, int UserId)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			string empty = string.Empty;
			if (query != null)
			{
				DbQueryResult referrals = MemberHelper.GetReferrals(query, UserId, true);
				List<Dictionary<string, object>> list = DataHelper.DataTableToDictionary(referrals.Data);
				foreach (Dictionary<string, object> item in list)
				{
					MemberInfo memberInfo = item.ToObject<MemberInfo>();
					item.Add("UserAllSplittinStr", MemberProcessor.GetUserAllSplittin(memberInfo.UserId).F2ToString("f2"));
				}
				dataGridViewModel.rows = list;
				dataGridViewModel.total = referrals.TotalRecords;
			}
			return dataGridViewModel;
		}
	}
}
