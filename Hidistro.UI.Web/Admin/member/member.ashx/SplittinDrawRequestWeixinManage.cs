using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Members;
using Hidistro.UI.Web.ashxBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Admin.member.ashx
{
	public class SplittinDrawRequestWeixinManage : AdminBaseHandler
	{
		private SiteSettings siteSettings;

		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			string text = context.Request["action"];
			this.siteSettings = SettingsManager.GetMasterSettings();
			if (string.IsNullOrWhiteSpace(text))
			{
				throw new HidistroAshxException("错误的参数");
			}
			switch (text)
			{
			case "getSplittinDraws":
				this.GetSplittinDraws(context);
				break;
			case "exporttoexcel":
				this.Exporttoexcel(context);
				break;
			case "drawRequest":
				this.DrawRequest(context);
				break;
			case "moredrawRequest":
				this.MoreDrawRequest(context);
				break;
			default:
				throw new HidistroAshxException("错误的参数");
			}
		}

		private void Exporttoexcel(HttpContext context)
		{
			BalanceDrawRequestQuery balanceDrawRequestQuery = this.GetBalanceDrawRequestQuery(context);
			balanceDrawRequestQuery.PageIndex = 1;
			balanceDrawRequestQuery.PageSize = 2147483647;
			DbQueryResult splittinDraws = MemberHelper.GetSplittinDraws(balanceDrawRequestQuery, 1);
			DataTable data = splittinDraws.Data;
			string empty = string.Empty;
			empty += "用户名";
			empty += ",申请时间";
			empty += ",提现金额";
			empty += ",备注\r\n";
			foreach (DataRow row in data.Rows)
			{
				empty += row["UserName"].ToString();
				empty = empty + "," + row["RequestDate"].ToString();
				empty = empty + "," + row["Amount"].ToString();
				empty = empty + "," + row["Remark"].ToString() + "\r\n";
			}
			context.Response.Clear();
			context.Response.Buffer = false;
			context.Response.Charset = "GB2312";
			context.Response.AppendHeader("Content-Disposition", "attachment;filename=SplittinWeixinDrawRequest.csv");
			context.Response.ContentEncoding = Encoding.GetEncoding("GB2312");
			context.Response.ContentType = "application/ms-excel";
			context.Response.Write(empty);
			context.Response.End();
		}

		private void GetSplittinDraws(HttpContext context)
		{
			BalanceDrawRequestQuery balanceDrawRequestQuery = this.GetBalanceDrawRequestQuery(context);
			DataGridViewModel<Dictionary<string, object>> listSplittinDraws = this.GetListSplittinDraws(balanceDrawRequestQuery);
			string s = base.SerializeObjectToJson(listSplittinDraws);
			context.Response.Write(s);
			context.Response.End();
		}

		private BalanceDrawRequestQuery GetBalanceDrawRequestQuery(HttpContext context)
		{
			int num = 1;
			int num2 = 10;
			int drawRequestType = 0;
			string empty = string.Empty;
			DateTime? fromDate = null;
			DateTime? toDate = null;
			string empty2 = string.Empty;
			empty2 = context.Request["UserName"];
			empty = context.Request["DateStart"];
			if (!string.IsNullOrWhiteSpace(empty))
			{
				try
				{
					fromDate = DateTime.Parse(empty);
				}
				catch
				{
					fromDate = null;
				}
			}
			empty = context.Request["DateEnd"];
			if (!string.IsNullOrWhiteSpace(empty))
			{
				try
				{
					toDate = DateTime.Parse(empty);
				}
				catch
				{
					toDate = null;
				}
			}
			empty = context.Request["DrawRequestType"];
			if (!string.IsNullOrWhiteSpace(empty))
			{
				try
				{
					drawRequestType = int.Parse(empty);
				}
				catch
				{
					drawRequestType = 0;
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
			BalanceDrawRequestQuery balanceDrawRequestQuery = new BalanceDrawRequestQuery();
			balanceDrawRequestQuery.AuditStatus = base.GetIntParam(context, "AuditStatus", false).Value;
			balanceDrawRequestQuery.FromDate = fromDate;
			balanceDrawRequestQuery.ToDate = toDate;
			balanceDrawRequestQuery.UserName = empty2;
			balanceDrawRequestQuery.PageSize = num2;
			balanceDrawRequestQuery.PageIndex = num;
			balanceDrawRequestQuery.DrawRequestType = drawRequestType;
			return balanceDrawRequestQuery;
		}

		private DataGridViewModel<Dictionary<string, object>> GetListSplittinDraws(BalanceDrawRequestQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			string empty = string.Empty;
			if (query != null)
			{
				DbQueryResult splittinDraws = MemberHelper.GetSplittinDraws(query, query.AuditStatus);
				List<Dictionary<string, object>> list = DataHelper.DataTableToDictionary(splittinDraws.Data);
				foreach (Dictionary<string, object> item in list)
				{
					SplittinDrawInfo splittinDrawInfo = item.ToObject<SplittinDrawInfo>();
					item.Add("RequestStateStr", OnLinePaymentEnumHelper.GetOnLinePaymentDescription(splittinDrawInfo.RequestState));
					if (splittinDrawInfo.RequestState.ToInt(0) == 3)
					{
						item.Add("RequestStateImgStr", "<i class='glyphicon glyphicon-question-sign' data-container='body' style='cursor: pointer' data-toggle='popover' data-placement='left' title='" + splittinDrawInfo.RequestError + "' ></i>");
					}
				}
				dataGridViewModel.rows = list;
				dataGridViewModel.total = splittinDraws.TotalRecords;
			}
			return dataGridViewModel;
		}

		private void DrawRequest(HttpContext context)
		{
			string a = context.Request["ChargeType"];
			long journalNumber = 0L;
			string s = context.Request["JournalNumber"];
			string text = context.Request["Reason"];
			if (!long.TryParse(s, out journalNumber))
			{
				return;
			}
			if (a == "UnLineReCharge")
			{
				if (!this.siteSettings.EnableBulkPaymentWeixin)
				{
					throw new HidistroAshxException("系统已关闭微信批量转账功能，请拒绝转账申请");
				}
				if (MemberHelper.AccepteDraw(journalNumber))
				{
					string text2 = MemberHelper.OnLineSplittinDrawRequest_Weixin(journalNumber.ToString());
					if (text2.Contains("成功"))
					{
						base.ReturnResult(context, true, "同意申请成功", 0, true);
					}
					else
					{
						base.ReturnResult(context, false, text2, 0, true);
					}
					goto IL_00de;
				}
				throw new HidistroAshxException("操作失败");
			}
			goto IL_00de;
			IL_00de:
			if (!(a == "RefuseRequest"))
			{
				return;
			}
			if (string.IsNullOrEmpty(text))
			{
				throw new HidistroAshxException("请填写拒绝申请的原因");
			}
			if (MemberHelper.RefuseDraw(journalNumber, text))
			{
				base.ReturnResult(context, true, "拒绝申请成功", 0, true);
				return;
			}
			throw new HidistroAshxException("操作失败");
		}

		private void MoreDrawRequest(HttpContext context)
		{
			string text = context.Request["ChargeType"];
			string text2 = context.Request["JournalNumber"];
			string text3 = context.Request["Reason"];
			if (!this.siteSettings.EnableBulkPaymentWeixin)
			{
				throw new HidistroAshxException("系统已关闭微信批量转账功能，请拒绝转账申请");
			}
			string text4 = text2.Trim();
			if (!string.IsNullOrEmpty(text4))
			{
				string[] array = text4.Split(',');
				foreach (string text5 in array)
				{
					if (!string.IsNullOrEmpty(text5))
					{
						long journalNumber = 0L;
						if (long.TryParse(text5, out journalNumber))
						{
							MemberHelper.AccepteDraw(journalNumber);
						}
					}
				}
				string text6 = MemberHelper.OnLineSplittinDrawRequest_Weixin(text4);
				if (text6 == string.Empty)
				{
					throw new HidistroAshxException("付款失败");
				}
				base.ReturnResult(context, true, text6, 0, true);
				return;
			}
			throw new HidistroAshxException("请至少选择一项");
		}
	}
}
