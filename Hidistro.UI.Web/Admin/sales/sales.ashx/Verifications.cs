using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Sales;
using Hidistro.UI.Web.ashxBase;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;

namespace Hidistro.UI.Web.Admin.sales.ashx
{
	public class Verifications : AdminBaseHandler
	{
		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			if (string.IsNullOrWhiteSpace(base.action))
			{
				throw new HidistroAshxException("错误的参数");
			}
			base.action = base.action.ToLower();
			string action = base.action;
			if (action == "getlist")
			{
				this.GetList(context);
				return;
			}
			throw new HidistroAshxException("错误的参数");
		}

		public void GetList(HttpContext context)
		{
			int num = 1;
			int num2 = 10;
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
			VerificationRecordQuery verificationRecordQuery = new VerificationRecordQuery();
			verificationRecordQuery.OrderId = Globals.UrlDecode(context.Request["OrderId"]);
			verificationRecordQuery.Code = Globals.UrlDecode(context.Request["Code"]);
			verificationRecordQuery.StoreId = base.GetIntParam(context, "StoreId", true);
			verificationRecordQuery.Status = (VerificationStatus?)base.GetIntParam(context, "Status", true);
			verificationRecordQuery.StartCreateDate = base.GetDateTimeParam(context, "StartCreateDate");
			verificationRecordQuery.EndCreateDate = base.GetDateTimeParam(context, "EndCreateDate");
			verificationRecordQuery.StartVerificationDate = base.GetDateTimeParam(context, "StartVerificationDate");
			verificationRecordQuery.EndVerificationDate = base.GetDateTimeParam(context, "EndVerificationDate");
			verificationRecordQuery.PageIndex = num;
			verificationRecordQuery.PageSize = num2;
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList(verificationRecordQuery);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList(VerificationRecordQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			if (query != null)
			{
				DbQueryResult verificationRecord = OrderHelper.GetVerificationRecord(query);
				dataGridViewModel.rows = DataHelper.DataTableToDictionary(verificationRecord.Data);
				dataGridViewModel.total = verificationRecord.TotalRecords;
				foreach (Dictionary<string, object> row in dataGridViewModel.rows)
				{
					VerificationStatus verificationStatus = (VerificationStatus)row["VerificationStatus"].ToInt(0);
					row.Add("PayOrderId", OrderHelper.GetPayOrderId(row["OrderId"].ToNullString()));
					row.Add("StatusText", ((Enum)(object)verificationStatus).ToDescription());
					row["VerificationPassword"] = this.GetShowVerificationPassword(row["VerificationPassword"].ToString(), verificationStatus);
				}
			}
			return dataGridViewModel;
		}

		private string GetShowVerificationPassword(string VerificationPassword, VerificationStatus status)
		{
			string text = "";
			text = VerificationPassword;
			if (!string.IsNullOrWhiteSpace(text))
			{
				text = ((status != 0 && status != VerificationStatus.ApplyRefund) ? Regex.Replace(text, "(\\d{4})(\\d{4})(\\d+)", "$1$2$3") : Regex.Replace(text, "(\\d{4})(\\d{4})(\\d+)", "$1****$3"));
			}
			return text;
		}
	}
}
