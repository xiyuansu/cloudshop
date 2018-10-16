using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.Messages;
using Hidistro.SaleSystem.Members;
using Hidistro.SqlDal.Members;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Admin.member.ashx
{
	public class BalanceDrawRequestWeixin : AdminBaseHandler
	{
		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			if (!string.IsNullOrWhiteSpace(base.action))
			{
				base.action = base.action.ToLower();
			}
			switch (base.action)
			{
			case "getlist":
				this.GetList(context);
				break;
			case "confirm":
				this.Confirm(context);
				break;
			case "refuse":
				this.Refuse(context);
				break;
			case "exportexcel":
				this.ExportExcel(context);
				break;
			default:
				throw new HidistroAshxException("错误的参数");
			}
		}

		public void ExportExcel(HttpContext context)
		{
			BalanceDrawRequestQuery dataQuery = this.GetDataQuery(context);
			dataQuery.PageIndex = 1;
			dataQuery.PageSize = 2147483647;
			DbQueryResult balanceDrawRequests = MemberHelper.GetBalanceDrawRequests(dataQuery, true);
			DataTable data = balanceDrawRequests.Data;
			string empty = string.Empty;
			empty += "用户名";
			empty += ",申请时间";
			empty += ",提现金额";
			empty += ",备注\r\n";
			foreach (DataRow row in data.Rows)
			{
				empty += row["UserName"].ToString();
				empty = empty + "," + row["RequestTime"].ToString();
				empty = empty + "," + row["Amount"].ToString();
				empty = empty + "," + row["Remark"].ToString() + "\r\n";
			}
			context.Response.Clear();
			context.Response.Buffer = false;
			context.Response.Charset = "GB2312";
			context.Response.AppendHeader("Content-Disposition", "attachment;filename=BalanceWeiXinDrawRequest.csv");
			context.Response.ContentEncoding = Encoding.GetEncoding("GB2312");
			context.Response.ContentType = "application/octet-stream";
			context.Response.Write(empty);
			context.Response.End();
		}

		private BalanceDrawRequestQuery GetDataQuery(HttpContext context)
		{
			BalanceDrawRequestQuery balanceDrawRequestQuery = new BalanceDrawRequestQuery();
			balanceDrawRequestQuery.FromDate = base.GetDateTimeParam(context, "FromDate");
			balanceDrawRequestQuery.ToDate = base.GetDateTimeParam(context, "ToDate");
			balanceDrawRequestQuery.UserName = base.GetParameter(context, "UserName", true);
			balanceDrawRequestQuery.PageIndex = base.CurrentPageIndex;
			balanceDrawRequestQuery.PageSize = base.CurrentPageSize;
			balanceDrawRequestQuery.DrawRequestType = 2;
			return balanceDrawRequestQuery;
		}

		private void GetList(HttpContext context)
		{
			BalanceDrawRequestQuery dataQuery = this.GetDataQuery(context);
			DataGridViewModel<Dictionary<string, object>> listSplittinDraws = this.GetListSplittinDraws(dataQuery);
			string s = base.SerializeObjectToJson(listSplittinDraws);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetListSplittinDraws(BalanceDrawRequestQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			if (query != null)
			{
				DbQueryResult balanceDrawRequests = MemberHelper.GetBalanceDrawRequests(query, true);
				dataGridViewModel.rows = DataHelper.DataTableToDictionary(balanceDrawRequests.Data);
				dataGridViewModel.total = balanceDrawRequests.TotalRecords;
				foreach (Dictionary<string, object> row in dataGridViewModel.rows)
				{
					row.Add("RequestStateText", OnLinePaymentEnumHelper.GetOnLinePaymentDescription(row["RequestState"]));
				}
			}
			return dataGridViewModel;
		}

		public void Confirm(HttpContext context)
		{
			if (base.CurrentSiteSetting.IsDemoSite)
			{
				throw new HidistroAshxException("演示站点,无法真实付款");
			}
			string text = string.Empty;
			string parameter = base.GetParameter(context, "ids", true);
			if (string.IsNullOrWhiteSpace(parameter))
			{
				throw new HidistroAshxException("请至少选择一项");
			}
			if (!base.CurrentSiteSetting.EnableBulkPaymentWeixin)
			{
				throw new HidistroAshxException("系统已关闭微信批量转账功能，请拒绝转账申请");
			}
			int[] array = (from d in parameter.Split(',')
			where !string.IsNullOrWhiteSpace(d)
			select int.Parse(d)).ToArray();
			int[] array2 = array;
			foreach (int id in array2)
			{
				MemberHelper.DealBalanceDrawRequestById(id, true, ref text, "");
			}
			text = MemberHelper.OnLineBalanceDrawRequest_Weixin(string.Join(",", array));
			if (!string.IsNullOrEmpty(text))
			{
				base.ReturnSuccessResult(context, text, 0, true);
				return;
			}
			throw new HidistroAshxException("付款失败");
		}

		public void Refuse(HttpContext context)
		{
			if (base.CurrentSiteSetting.IsDemoSite)
			{
				throw new HidistroAshxException("演示站点,无法真实付款");
			}
			string empty = string.Empty;
			int? intParam = base.GetIntParam(context, "id", true);
			if (!intParam.HasValue || intParam < 1)
			{
				throw new HidistroAshxException("错误的数据编号");
			}
			string parameter = base.GetParameter(context, "Reason", true);
			if (string.IsNullOrWhiteSpace(parameter))
			{
				throw new HidistroAshxException("请填写拒绝申请的原因");
			}
			if (parameter.Length > 50)
			{
				throw new HidistroAshxException("拒绝原因不能超过50字符");
			}
			if (MemberHelper.DealBalanceDrawRequestById(intParam.Value, false, ref empty, parameter))
			{
				BalanceDrawRequestInfo balanceDrawRequestInfo = new ReferralDao().Get<BalanceDrawRequestInfo>(intParam.Value.ToInt(0));
				string text = "";
				text = ((!balanceDrawRequestInfo.IsWeixin.HasValue || !balanceDrawRequestInfo.IsWeixin.Value) ? ((!balanceDrawRequestInfo.IsAlipay.HasValue || !balanceDrawRequestInfo.IsAlipay.Value) ? ("银行卡：" + balanceDrawRequestInfo.BankName + "(" + balanceDrawRequestInfo.AccountName + ")") : ("支付宝：" + balanceDrawRequestInfo.AlipayCode)) : "微信");
				MemberInfo user = Users.GetUser(balanceDrawRequestInfo.UserId);
				Messenger.DrawResultMessager(user, null, balanceDrawRequestInfo.Amount, text, balanceDrawRequestInfo.RequestTime, false, parameter);
				base.ReturnSuccessResult(context, "拒绝申请成功", 0, true);
				return;
			}
			throw new HidistroAshxException("提现申请操作失败");
		}
	}
}
