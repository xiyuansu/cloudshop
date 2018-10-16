using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Supplier;
using Hidistro.SaleSystem.Supplier;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Admin.Supplier.Balance.ashx
{
	public class BalanceDetail : AdminBaseHandler
	{
		private string sError = string.Empty;

		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			if (string.IsNullOrWhiteSpace(base.action))
			{
				throw new HidistroAshxException("错误的参数");
			}
			base.action = base.action.ToLower();
			string action = base.action;
			if (!(action == "getlist"))
			{
				if (action == "exportexcel")
				{
					this.ExportExcel(context);
					return;
				}
				throw new HidistroAshxException("错误的参数");
			}
			this.GetList(context);
		}

		public void GetList(HttpContext context)
		{
			BalanceDetailSupplierQuery dataQuery = this.GetDataQuery(context);
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList(dataQuery);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private BalanceDetailSupplierQuery GetDataQuery(HttpContext context)
		{
			BalanceDetailSupplierQuery balanceDetailSupplierQuery = new BalanceDetailSupplierQuery();
			balanceDetailSupplierQuery.SupplierId = base.GetIntParam(context, "SupplierId", false);
			balanceDetailSupplierQuery.FromDate = base.GetDateTimeParam(context, "FromDate");
			balanceDetailSupplierQuery.ToDate = base.GetDateTimeParam(context, "ToDate");
			balanceDetailSupplierQuery.TradeType = base.GetIntParam(context, "TradeType", false).Value;
			balanceDetailSupplierQuery.PageIndex = base.CurrentPageIndex;
			balanceDetailSupplierQuery.PageSize = base.CurrentPageSize;
			return balanceDetailSupplierQuery;
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList(BalanceDetailSupplierQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			if (query != null)
			{
				DbQueryResult balanceDetails = BalanceHelper.GetBalanceDetails(query);
				dataGridViewModel.rows = new List<Dictionary<string, object>>();
				dataGridViewModel.total = balanceDetails.TotalRecords;
				foreach (DataRow row in balanceDetails.Data.Rows)
				{
					Dictionary<string, object> item = DataHelper.DataRowToDictionary(row);
					dataGridViewModel.rows.Add(item);
				}
			}
			return dataGridViewModel;
		}

		public void ExportExcel(HttpContext context)
		{
			BalanceDetailSupplierQuery dataQuery = this.GetDataQuery(context);
			SupplierInfo supplierById = SupplierHelper.GetSupplierById(dataQuery.SupplierId.Value);
			DbQueryResult balanceDetails4Report = BalanceHelper.GetBalanceDetails4Report(dataQuery);
			StringBuilder stringBuilder = new StringBuilder(300);
			stringBuilder.Append("供应商");
			stringBuilder.Append(",时间");
			stringBuilder.Append(",提现帐户");
			stringBuilder.Append(",类型");
			stringBuilder.Append(",订单号");
			stringBuilder.Append(",收入");
			stringBuilder.Append(",支出");
			stringBuilder.Append(",账户余额");
			stringBuilder.Append(",备注\r\n");
			foreach (DataRow row in balanceDetails4Report.Data.Rows)
			{
				int requestId = row["OrderId"].ToInt(0);
				SupplierBalanceDrawRequestInfo balanceDrawRequestInfo = BalanceHelper.GetBalanceDrawRequestInfo(requestId);
				stringBuilder.Append(supplierById.SupplierName);
				stringBuilder.Append("," + row["TradeDate"]);
				if (balanceDrawRequestInfo != null)
				{
					string text = "";
					text = ((!balanceDrawRequestInfo.IsWeixin) ? ((!balanceDrawRequestInfo.IsAlipay) ? $"提现到银行卡(开户银行:{balanceDrawRequestInfo.BankName}，银行开户名:{balanceDrawRequestInfo.AccountName}，银行卡帐号:{balanceDrawRequestInfo.MerchantCode})" : ("提现到支付宝(支付宝帐号:" + balanceDrawRequestInfo.AlipayCode + "，支付宝姓名:" + balanceDrawRequestInfo.AlipayRealName + ")")) : "提现到微信");
					stringBuilder.Append("," + text);
				}
				else
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append("," + row["TradeTypeText"]);
				SupplierTradeTypes supplierTradeTypes = (SupplierTradeTypes)row["TradeType"].ToInt(0);
				if (supplierTradeTypes == SupplierTradeTypes.OrderBalance)
				{
					stringBuilder.Append("," + row["OrderId"]);
				}
				else
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append("," + row["Income"]);
				stringBuilder.Append("," + row["Expenses"]);
				stringBuilder.Append("," + row["Balance"]);
				stringBuilder.Append("," + row["Remark"] + "\r\n");
			}
			context.Response.Clear();
			context.Response.Buffer = false;
			context.Response.Charset = "GB2312";
			context.Response.AppendHeader("Content-Disposition", "attachment;filename=BalanceDetail.csv");
			context.Response.ContentEncoding = Encoding.GetEncoding("GB2312");
			context.Response.ContentType = "application/octet-stream";
			context.Response.Write(stringBuilder.ToString());
			context.Response.End();
		}
	}
}
