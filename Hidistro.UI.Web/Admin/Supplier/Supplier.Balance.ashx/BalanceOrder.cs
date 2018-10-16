using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Supplier;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Supplier;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Admin.Supplier.Balance.ashx
{
	public class BalanceOrder : AdminBaseHandler
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
			BalanceOrderQuery dataQuery = this.GetDataQuery(context);
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList(dataQuery);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private BalanceOrderQuery GetDataQuery(HttpContext context)
		{
			BalanceOrderQuery balanceOrderQuery = new BalanceOrderQuery();
			balanceOrderQuery.SupplierId = base.GetIntParam(context, "SupplierId", false).Value;
			balanceOrderQuery.StartDate = base.GetDateTimeParam(context, "StartDate");
			balanceOrderQuery.EndDate = base.GetDateTimeParam(context, "EndDate");
			balanceOrderQuery.OrderId = base.GetParameter(context, "OrderId", true);
			balanceOrderQuery.IsBalanceOver = (base.GetIntParam(context, "BalanceOver", false) != 0);
			balanceOrderQuery.PageIndex = base.CurrentPageIndex;
			balanceOrderQuery.PageSize = base.CurrentPageSize;
			return balanceOrderQuery;
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList(BalanceOrderQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			if (query != null)
			{
				DbQueryResult orders = BalanceOrderHelper.GetOrders(query);
				dataGridViewModel.rows = new List<Dictionary<string, object>>();
				dataGridViewModel.total = orders.TotalRecords;
				foreach (DataRow row in orders.Data.Rows)
				{
					Dictionary<string, object> dictionary = DataHelper.DataRowToDictionary(row);
					dictionary.Add("SettlementAmount", dictionary["OrderCostPrice"].ToDecimal(0) + dictionary["Freight"].ToDecimal(0));
					dictionary.Add("OrderStatusText", OrderHelper.GetOrderStatusText((OrderStatus)dictionary["OrderStatus"], dictionary["ShippingModeId"].ToInt(0), dictionary["IsConfirm"].ToBool(), dictionary["Gateway"].ToString(), dictionary["PaymentTypeId"].ToInt(0), dictionary["PreSaleId"].ToInt(0), dictionary["DepositDate"].ToDateTime(), false, OrderItemStatus.Nomarl, OrderType.NormalOrder));
					dataGridViewModel.rows.Add(dictionary);
				}
			}
			return dataGridViewModel;
		}

		public void ExportExcel(HttpContext context)
		{
			BalanceOrderQuery dataQuery = this.GetDataQuery(context);
			DbQueryResult orders4Report = BalanceOrderHelper.GetOrders4Report(dataQuery);
			string empty = string.Empty;
			empty += "下单时间";
			empty += ",订单编号";
			empty += ",支付方式";
			empty += ",供货总价";
			empty += ",运费";
			empty += ",结算金额\r\n";
			foreach (DataRow row in orders4Report.Data.Rows)
			{
				empty += row["OrderDate"];
				empty = empty + "," + row["OrderId"];
				empty = empty + "," + row["PaymentType"];
				empty = empty + "," + row["OrderCostPrice"];
				empty = empty + "," + row["Freight"];
				empty = empty + "," + (row["Freight"].ToDecimal(0) + row["OrderCostPrice"].ToDecimal(0)) + "\r\n";
			}
			context.Response.Clear();
			context.Response.Buffer = false;
			context.Response.Charset = "GB2312";
			context.Response.AppendHeader("Content-Disposition", "attachment;filename=OrderBalance" + (dataQuery.IsBalanceOver ? "Over" : "") + ".csv");
			context.Response.ContentEncoding = Encoding.GetEncoding("GB2312");
			context.Response.ContentType = "application/octet-stream";
			context.Response.Write(empty);
			context.Response.End();
		}
	}
}
