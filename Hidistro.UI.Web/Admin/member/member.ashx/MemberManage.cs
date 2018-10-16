using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Members;
using Hidistro.Messages;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Members;
using Hidistro.UI.Web.ashxBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Admin.member.ashx
{
	public class MemberManage : AdminBaseHandler
	{
		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			if (string.IsNullOrWhiteSpace(base.action))
			{
				throw new HidistroAshxException("错误的参数");
			}
			switch (base.action)
			{
			case "getlistReferralRequest":
				this.GetListReferralRequest(context);
				break;
			case "acceptRequest":
				this.AcceptRequest(context);
				break;
			case "refuse":
				this.Refuse(context);
				break;
			case "getlistReferral":
				this.GetlistReferral(context);
				break;
			case "getSplittinDraws":
				this.GetSplittinDraws(context);
				break;
			case "exportexcel":
				this.ExportToExcel(context);
				break;
			default:
				throw new HidistroAshxException("错误的参数");
			}
		}

		public void ExportToExcel(HttpContext context)
		{
			MemberQuery dataQuery = this.GetDataQuery(context);
			IList<Hidistro.Entities.Members.ReferralInfo> referralExportData = MemberHelper.GetReferralExportData(dataQuery);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<table border='1'>");
			stringBuilder.Append("<thead><tr>");
			stringBuilder.Append("<th>分销员</th>");
			stringBuilder.Append("<th>真实姓名</th>");
			stringBuilder.Append("<th>电话号码</th>");
			stringBuilder.Append("<th>邮箱</th>");
			stringBuilder.Append("<th>店铺名称</th>");
			stringBuilder.Append("<th>分销员等级</th>");
			stringBuilder.Append("<th>直接下级数</th>");
			stringBuilder.Append("<th>直接下级成交额</th>");
			stringBuilder.Append("<th>累计获得佣金</th>");
			stringBuilder.Append("<th>成为分销员时间</th>");
			stringBuilder.Append("</tr></thead>");
			StringBuilder stringBuilder2 = new StringBuilder();
			DateTime dateTime;
			foreach (Hidistro.Entities.Members.ReferralInfo item in referralExportData)
			{
				ReferralExtInfo referralExtInfo = MemberProcessor.GetReferralExtInfo(item.RequetReason);
				stringBuilder2.Append("<tr>");
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.UserName, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(string.IsNullOrEmpty(referralExtInfo.RealName) ? item.RealName : referralExtInfo.RealName, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(string.IsNullOrEmpty(referralExtInfo.CellPhone) ? item.CellPhone : referralExtInfo.CellPhone, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(string.IsNullOrEmpty(referralExtInfo.Email) ? item.Email : referralExtInfo.Email, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.ShopName, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.GradeName, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.SubNumber, false));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.LowerSaleTotal.F2ToString("f2"), false));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.UserAllSplittin.F2ToString("f2"), false));
				StringBuilder stringBuilder3 = stringBuilder2;
				dateTime = item.AuditDate.Value;
				stringBuilder3.Append(ExcelHelper.GetXLSFieldsTD(dateTime.ToString("yyyy-MM-dd HH:mm:ss"), false));
				stringBuilder2.Append("</tr>");
			}
			stringBuilder.AppendFormat("<tbody>{0}</tbody></table>", stringBuilder2.ToString());
			StringWriter stringWriter = new StringWriter();
			stringWriter.Write(stringBuilder);
			HttpResponse response = context.Response;
			StringBuilder stringBuilder4 = stringWriter.GetStringBuilder();
			dateTime = DateTime.Now;
			DownloadHelper.DownloadFile(response, stringBuilder4, "ReferralList_" + dateTime.ToString("yyyyMMddhhmmss") + ".xls");
			stringWriter.Close();
			context.Response.End();
		}

		private void GetSplittinDraws(HttpContext context)
		{
		}

		private void AcceptRequest(HttpContext context)
		{
			int userId = 0;
			string empty = string.Empty;
			empty = context.Request["userId"];
			if (!string.IsNullOrWhiteSpace(empty))
			{
				try
				{
					userId = int.Parse(empty);
				}
				catch
				{
					throw new HidistroAshxException("异常的参数：用户ID");
				}
			}
			if (MemberHelper.AccepteRerralRequest(userId))
			{
				MemberInfo user = Users.GetUser(userId);
				if (user != null)
				{
					Users.ClearUserCache(userId, user.SessionId);
				}
				string text = HiContext.Current.SiteSettings.SiteUrl.ToLower();
				text = (text.StartsWith("http://") ? text : ("http://" + text));
				text += "/vshop/SplittinRule";
				string first = "您的分销员申请已通过审核";
				string remark = "点击查看详情，跳转到分销员的分销海报页面";
				string keyword = "审核通过";
				string keyword2 = DateTime.Now.ToString("yyyy年MM月dd日 HH:mm");
				Messenger.ExtensionAudit(user, text, keyword, keyword2, first, remark);
				base.ReturnResult(context, true, "分销员的申请已经审核通过", 0, true);
				return;
			}
			throw new HidistroAshxException("审核通过失败");
		}

		private void Refuse(HttpContext context)
		{
			int userId = 0;
			string empty = string.Empty;
			empty = context.Request["userId"];
			if (!string.IsNullOrWhiteSpace(empty))
			{
				try
				{
					userId = int.Parse(empty);
				}
				catch
				{
					throw new HidistroAshxException("异常的参数：用户ID");
				}
			}
			string text = context.Request["refusalReason"];
			if (string.IsNullOrWhiteSpace(text) || text == "拒绝时填写拒绝理由")
			{
				throw new HidistroAshxException("请输入拒绝理由！");
			}
			if (MemberHelper.RefuseRerralRequest(userId, text))
			{
				MemberInfo user = Users.GetUser(userId);
				if (user != null)
				{
					Users.ClearUserCache(userId, user.SessionId);
				}
				base.ReturnResult(context, true, "拒绝了分销员的申请", 0, true);
				return;
			}
			throw new HidistroAshxException("拒绝通过失败");
		}

		private void GetlistReferral(HttpContext context)
		{
			MemberQuery dataQuery = this.GetDataQuery(context);
			PageModel<Hidistro.Entities.Members.ReferralInfo> referralList = MemberHelper.GetReferralList(dataQuery);
			if (referralList.Models != null && referralList.Models.Count() > 0)
			{
				foreach (Hidistro.Entities.Members.ReferralInfo model in referralList.Models)
				{
					model.LowerSaleTotal = model.LowerSaleTotal.F2ToString("f2").ToDecimal(0);
					model.UserAllSplittin = model.UserAllSplittin.F2ToString("f2").ToDecimal(0);
					model.RequetReason = MemberProcessor.GetReferralExtShowInfo(model.RefusalReason);
				}
			}
			string s = base.SerializeObjectToJson(referralList);
			context.Response.Write(s);
			context.Response.End();
		}

		private MemberQuery GetDataQuery(HttpContext context)
		{
			int num = 1;
			int num2 = 10;
			string empty = string.Empty;
			int num3 = 0;
			string text = "";
			string text2 = "";
			SortAction sortOrder = SortAction.Desc;
			DateTime? startTime = null;
			DateTime? endTime = null;
			string empty2 = string.Empty;
			MemberQuery memberQuery = new MemberQuery();
			bool isRepeled = false;
			empty2 = context.Request["UserName"];
			empty = context.Request["DateStart"];
			if (!string.IsNullOrWhiteSpace(empty))
			{
				try
				{
					startTime = DateTime.Parse(empty);
				}
				catch
				{
					startTime = null;
				}
			}
			empty = context.Request["DateEnd"];
			if (!string.IsNullOrWhiteSpace(empty))
			{
				try
				{
					endTime = DateTime.Parse(empty);
				}
				catch
				{
					endTime = null;
				}
			}
			empty = context.Request["IsRepeled"];
			if (!string.IsNullOrWhiteSpace(empty))
			{
				try
				{
					isRepeled = bool.Parse(empty);
				}
				catch
				{
					isRepeled = false;
				}
			}
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
			text = Globals.StripAllTags(context.Request["ShopName"].ToNullString());
			num3 = context.Request["GradeId"].ToInt(0);
			text2 = context.Request["SortBy"].ToNullString();
			if (!string.IsNullOrEmpty(text2))
			{
				string[] array = text2.Split('_');
				if (array.Length >= 2)
				{
					text2 = array[0].ToLower();
					switch (text2)
					{
					case "time":
						text2 = "AuditDate";
						break;
					case "lowerusers":
						text2 = "SubNumber";
						break;
					case "tradetotal":
						text2 = "LowerSaleTotal";
						break;
					case "commissiontotal":
						text2 = "UserAllSplittin";
						break;
					default:
						text2 = "";
						break;
					}
					sortOrder = ((!(array[1].ToLower() == "d")) ? SortAction.Asc : SortAction.Desc);
				}
			}
			memberQuery.StartTime = startTime;
			memberQuery.EndTime = endTime;
			memberQuery.UserName = empty2;
			memberQuery.PageIndex = num;
			memberQuery.PageSize = num2;
			memberQuery.ReferralStatus = 2.GetHashCode();
			if (string.IsNullOrEmpty(text2))
			{
				memberQuery.SortBy = "AuditDate";
				memberQuery.SortOrder = SortAction.Desc;
			}
			else
			{
				memberQuery.SortBy = text2;
				memberQuery.SortOrder = sortOrder;
			}
			memberQuery.IsRepeled = isRepeled;
			memberQuery.ReferralGradeId = num3;
			memberQuery.ShopName = text;
			return memberQuery;
		}

		private DataGridViewModel<Dictionary<string, object>> GetReferrals(MemberQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			string empty = string.Empty;
			if (query != null)
			{
				DbQueryResult referrals = MemberHelper.GetReferrals(query, 0, false);
				List<Dictionary<string, object>> list = DataHelper.DataTableToDictionary(referrals.Data);
				foreach (Dictionary<string, object> item in list)
				{
					MemberInfo memberInfo = item.ToObject<MemberInfo>();
					item.Add("LowerSaleTotalStr", MemberProcessor.GetLowerSaleTotalByUserId(memberInfo.UserId).F2ToString("f2"));
					item.Add("UserAllSplittinStr", MemberProcessor.GetUserAllSplittin(memberInfo.UserId).F2ToString("f2"));
				}
				dataGridViewModel.rows = list;
				dataGridViewModel.total = referrals.TotalRecords;
			}
			return dataGridViewModel;
		}

		private void GetListReferralRequest(HttpContext context)
		{
			int num = 1;
			int num2 = 10;
			string empty = string.Empty;
			DateTime? startTime = null;
			DateTime? endTime = null;
			string empty2 = string.Empty;
			MemberQuery memberQuery = new MemberQuery();
			empty2 = context.Request["UserName"];
			empty = context.Request["DateStart"];
			if (!string.IsNullOrWhiteSpace(empty))
			{
				try
				{
					startTime = DateTime.Parse(empty);
				}
				catch
				{
					startTime = null;
				}
			}
			empty = context.Request["DateEnd"];
			if (!string.IsNullOrWhiteSpace(empty))
			{
				try
				{
					endTime = DateTime.Parse(empty);
				}
				catch
				{
					endTime = null;
				}
			}
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
			memberQuery.StartTime = startTime;
			memberQuery.EndTime = endTime;
			memberQuery.UserName = empty2;
			memberQuery.PageIndex = num;
			memberQuery.PageSize = num2;
			memberQuery.ReferralStatus = 1;
			DataGridViewModel<Dictionary<string, object>> referralRequest = this.GetReferralRequest(memberQuery);
			string s = base.SerializeObjectToJson(referralRequest);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetReferralRequest(MemberQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			string empty = string.Empty;
			if (query != null)
			{
				DbQueryResult spreadMembers = MemberHelper.GetSpreadMembers(query);
				List<Dictionary<string, object>> list = DataHelper.DataTableToDictionary(spreadMembers.Data);
				foreach (Dictionary<string, object> item in list)
				{
					MemberInfo memberInfo = item.ToObject<MemberInfo>();
					MemberInfo user = Users.GetUser(memberInfo.UserId);
					item.Add("SaleTotalStr", (user?.Expenditure ?? decimal.Zero).ToNullString().ToDecimal(0).F2ToString("f2"));
					item["RequetReason"] = MemberProcessor.GetReferralExtShowInfo(item["RequetReason"].ToNullString());
				}
				dataGridViewModel.rows = list;
				dataGridViewModel.total = spreadMembers.TotalRecords;
			}
			return dataGridViewModel;
		}
	}
}
