using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Store;
using Hidistro.Entities.Supplier;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Store;
using Hidistro.SaleSystem.Supplier;
using Hidistro.UI.Web.ashxBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Supplier.Balance.ashx
{
	[AdministerCheck(true)]
	public class BalanceManage : SupplierAdminHandler
	{
		private new ManagerInfo CurrentManager;

		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			this.CurrentManager = HiContext.Current.Manager;
			string text = context.Request["action"];
			if (string.IsNullOrWhiteSpace(text))
			{
				throw new HidistroAshxException("错误的参数");
			}
			switch (text)
			{
			case "getlistBalance":
				this.GetListBalance(context);
				break;
			case "getBalanceOrder":
				this.GetBalanceOrder(context);
				break;
			case "getlistBalanceDetail":
				this.GetlistBalanceDetail(context);
				break;
			case "exporttoexcelBalance":
				this.ExportToExcelBalance(context);
				break;
			case "exportToExcelBalanceOrder":
				this.ExportToExcelBalanceOrder(context);
				break;
			case "ExportToExcelBalancedetail":
				this.ExportToExcelBalancedetail(context);
				break;
			default:
				throw new HidistroAshxException("错误的参数");
			}
		}

		private void ExportToExcelBalance(HttpContext context)
		{
			BalanceDrawRequestSupplierQuery listBalanceqQuery = this.getListBalanceqQuery(context);
			listBalanceqQuery.PageIndex = 1;
			listBalanceqQuery.PageSize = 2147483647;
			DbQueryResult balanceDrawRequest4Report = BalanceHelper.GetBalanceDrawRequest4Report(listBalanceqQuery, false);
			string empty = string.Empty;
			empty += "申请时间";
			empty += ",提现金额";
			empty += ",提现方式";
			empty += ",收款人";
			empty += ",收款账号";
			empty += ",状态";
			empty += ",拒绝理由";
			empty += ",放款时间\r\n";
			foreach (DataRow row in balanceDrawRequest4Report.Data.Rows)
			{
				empty += row["RequestTime"];
				empty = empty + "," + row["Amount"];
				empty = empty + "," + ((row["IsAlipay"].ToString() == "False") ? "银行卡转账" : "支付宝支付");
				empty = empty + "," + ((row["IsAlipay"].ToString() == "False") ? row["AccountName"] : row["AlipayRealName"]);
				empty = empty + "," + ((row["IsAlipay"].ToString() == "False") ? row["MerchantCode"] : row["AlipayCode"]);
				empty = empty + "," + this.ParseDrawStatus(row["IsPass"]);
				empty = empty + "," + row["ManagerRemark"];
				empty = empty + "," + ((row["IsPass"] != null && row["IsPass"].ToString() == "True") ? row["AccountDate"].ToString() : "--") + "\r\n";
			}
			context.Response.Clear();
			context.Response.Buffer = false;
			context.Response.Charset = "GB2312";
			context.Response.AppendHeader("Content-Disposition", "attachment;filename=MyBalance.csv");
			context.Response.ContentEncoding = Encoding.GetEncoding("GB2312");
			context.Response.ContentType = "application/ms-excel";
			context.Response.Write(empty);
			context.Response.End();
		}

		private void ExportToExcelBalanceOrder(HttpContext context)
		{
			BalanceOrderQuery balanceOrderQuery = this.GetBalanceOrderQuery(context);
			balanceOrderQuery.PageIndex = 1;
			balanceOrderQuery.PageSize = 2147483647;
			DbQueryResult orders4Report = BalanceOrderHelper.GetOrders4Report(balanceOrderQuery);
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
				empty = empty + "," + row["OrderId"] + "\t";
				empty = empty + "," + row["PaymentType"];
				empty = empty + "," + row["OrderCostPrice"];
				empty = empty + "," + row["Freight"];
				empty = empty + "," + (row["Freight"].ToDecimal(0) + row["OrderCostPrice"].ToDecimal(0)) + "\r\n";
			}
			context.Response.Clear();
			context.Response.Buffer = false;
			context.Response.Charset = "GB2312";
			context.Response.AppendHeader("Content-Disposition", "attachment;filename=OrderBalance.csv");
			context.Response.ContentEncoding = Encoding.GetEncoding("GB2312");
			context.Response.ContentType = "application/ms-excel";
			context.Response.Write(empty);
			context.Response.End();
		}

		private void ExportToExcelBalancedetail(HttpContext context)
		{
			BalanceDetailSupplierQuery balanceDetailSupplierQuery = this.GetBalanceDetailSupplierQuery(context);
			DbQueryResult balanceDetails4Report = BalanceHelper.GetBalanceDetails4Report(balanceDetailSupplierQuery);
			string empty = string.Empty;
			empty += "时间";
			empty += ",类型";
			empty += ",订单号";
			empty += ",收入";
			empty += ",支出";
			empty += ",账户余额";
			empty += ",备注\r\n";
			foreach (DataRow row in balanceDetails4Report.Data.Rows)
			{
				empty += row["TradeDate"];
				empty = empty + "," + row["TradeTypeText"];
				empty = empty + "," + row["OrderId"];
				empty = empty + "," + row["Income"];
				empty = empty + "," + row["Expenses"];
				empty = empty + "," + row["Balance"];
				empty = empty + "," + row["Remark"] + "\r\n";
			}
			context.Response.Clear();
			context.Response.Buffer = false;
			context.Response.Charset = "GB2312";
			context.Response.AppendHeader("Content-Disposition", "attachment;filename=BalanceDetail.csv");
			context.Response.ContentEncoding = Encoding.GetEncoding("GB2312");
			context.Response.ContentType = "application/ms-excel";
			context.Response.Write(empty);
			context.Response.End();
		}

		private void GetlistBalanceDetail(HttpContext context)
		{
			BalanceDetailSupplierQuery balanceDetailSupplierQuery = this.GetBalanceDetailSupplierQuery(context);
			DataGridViewModel<Dictionary<string, object>> balanceDetails = this.GetBalanceDetails(balanceDetailSupplierQuery);
			string s = base.SerializeObjectToJson(balanceDetails);
			context.Response.Write(s);
			context.Response.End();
		}

		private BalanceDetailSupplierQuery GetBalanceDetailSupplierQuery(HttpContext context)
		{
			int num = 1;
			int num2 = 10;
			string empty = string.Empty;
			DateTime? fromDate = null;
			DateTime? toDate = null;
			int tradeType = 0;
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
			empty = context.Request["TradeType"];
			if (!string.IsNullOrWhiteSpace(empty))
			{
				try
				{
					tradeType = int.Parse(empty);
				}
				catch
				{
					tradeType = 0;
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
			BalanceDetailSupplierQuery balanceDetailSupplierQuery = new BalanceDetailSupplierQuery();
			balanceDetailSupplierQuery.SupplierId = this.CurrentManager.StoreId;
			balanceDetailSupplierQuery.FromDate = fromDate;
			balanceDetailSupplierQuery.ToDate = toDate;
			balanceDetailSupplierQuery.TradeType = tradeType;
			balanceDetailSupplierQuery.PageSize = num2;
			balanceDetailSupplierQuery.PageIndex = num;
			return balanceDetailSupplierQuery;
		}

		private void GetBalanceOrder(HttpContext context)
		{
			BalanceOrderQuery balanceOrderQuery = this.GetBalanceOrderQuery(context);
			DataGridViewModel<Dictionary<string, object>> balanceOrders = this.GetBalanceOrders(balanceOrderQuery);
			string s = base.SerializeObjectToJson(balanceOrders);
			context.Response.Write(s);
			context.Response.End();
		}

		private BalanceOrderQuery GetBalanceOrderQuery(HttpContext context)
		{
			int num = 1;
			int num2 = 10;
			string empty = string.Empty;
			DateTime? startDate = null;
			DateTime? endDate = null;
			int num3 = 0;
			BalanceOrderQuery balanceOrderQuery = new BalanceOrderQuery();
			empty = context.Request["BalanceOver"];
			if (!string.IsNullOrWhiteSpace(empty))
			{
				try
				{
					num3 = int.Parse(empty);
				}
				catch
				{
					num3 = 0;
				}
			}
			empty = context.Request["DateStart"];
			if (!string.IsNullOrWhiteSpace(empty))
			{
				try
				{
					startDate = DateTime.Parse(empty);
				}
				catch
				{
					startDate = null;
				}
			}
			empty = context.Request["EndDate"];
			if (!string.IsNullOrWhiteSpace(empty))
			{
				try
				{
					endDate = DateTime.Parse(empty);
				}
				catch
				{
					endDate = null;
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
			balanceOrderQuery.IsBalanceOver = (num3 != 0);
			balanceOrderQuery.OrderId = context.Request["OrderId"];
			balanceOrderQuery.StartDate = startDate;
			balanceOrderQuery.EndDate = endDate;
			balanceOrderQuery.PageSize = num2;
			balanceOrderQuery.PageIndex = num;
			balanceOrderQuery.SupplierId = this.CurrentManager.StoreId;
			return balanceOrderQuery;
		}

		private DataGridViewModel<Dictionary<string, object>> GetBalanceOrders(BalanceOrderQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			string empty = string.Empty;
			if (query != null)
			{
				DbQueryResult orders = BalanceOrderHelper.GetOrders(query);
				List<Dictionary<string, object>> list = DataHelper.DataTableToDictionary(orders.Data);
				foreach (Dictionary<string, object> item in list)
				{
					OrderInfo orderInfo = item.ToObject<OrderInfo>();
					item.Add("OrderStatusText", OrderHelper.GetOrderStatusText(orderInfo.OrderStatus, orderInfo.ShippingModeId, orderInfo.IsConfirm, orderInfo.Gateway, 0, orderInfo.PreSaleId, orderInfo.DepositDate, false, orderInfo.ItemStatus, OrderType.NormalOrder));
				}
				dataGridViewModel.rows = list;
				dataGridViewModel.total = orders.TotalRecords;
			}
			return dataGridViewModel;
		}

		private DataGridViewModel<Dictionary<string, object>> GetBalanceDetails(BalanceDetailSupplierQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			string empty = string.Empty;
			if (query != null)
			{
				DbQueryResult balanceDetails = BalanceHelper.GetBalanceDetails(query);
				List<Dictionary<string, object>> list = DataHelper.DataTableToDictionary(balanceDetails.Data);
				foreach (Dictionary<string, object> item in list)
				{
					SupplierBalanceDetailInfo supplierBalanceDetailInfo = item.ToObject<SupplierBalanceDetailInfo>();
					if (supplierBalanceDetailInfo.TradeType == SupplierTradeTypes.DrawRequest)
					{
						item.Add("TradeTypeStr", "提现");
					}
					else
					{
						item.Add("TradeTypeStr", "商品交易");
					}
					if (string.IsNullOrEmpty(supplierBalanceDetailInfo.OrderId))
					{
						item.Add("OrderIdStr", "--");
					}
					else
					{
						item.Add("OrderIdStr", supplierBalanceDetailInfo.OrderId);
					}
					if (supplierBalanceDetailInfo.Income == decimal.Zero)
					{
						item.Add("IncomeStr", "--");
					}
					else
					{
						item.Add("IncomeStr", supplierBalanceDetailInfo.Income.F2ToString("f2"));
					}
					if (supplierBalanceDetailInfo.Expenses == decimal.Zero)
					{
						item.Add("ExpensesStr", "--");
					}
					else
					{
						item.Add("ExpensesStr", supplierBalanceDetailInfo.Expenses.F2ToString("f2"));
					}
				}
				dataGridViewModel.rows = list;
				dataGridViewModel.total = balanceDetails.TotalRecords;
			}
			return dataGridViewModel;
		}

		private void GetListBalance(HttpContext context)
		{
			BalanceDrawRequestSupplierQuery listBalanceqQuery = this.getListBalanceqQuery(context);
			DataGridViewModel<SupplierBalanceDrawRequestInfo> balance = this.GetBalance(listBalanceqQuery);
			string s = base.SerializeObjectToJson(balance);
			context.Response.Write(s);
			context.Response.End();
		}

		private BalanceDrawRequestSupplierQuery getListBalanceqQuery(HttpContext context)
		{
			int num = 1;
			int num2 = 10;
			string empty = string.Empty;
			DateTime? fromDate = null;
			DateTime? toDate = null;
			int auditState = 0;
			BalanceDrawRequestSupplierQuery balanceDrawRequestSupplierQuery = new BalanceDrawRequestSupplierQuery();
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
			empty = context.Request["TradeType"];
			if (!string.IsNullOrWhiteSpace(empty))
			{
				try
				{
					auditState = int.Parse(empty);
				}
				catch
				{
					auditState = 0;
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
			balanceDrawRequestSupplierQuery.UserId = this.CurrentManager.StoreId;
			balanceDrawRequestSupplierQuery.FromDate = fromDate;
			balanceDrawRequestSupplierQuery.ToDate = toDate;
			balanceDrawRequestSupplierQuery.AuditState = auditState;
			balanceDrawRequestSupplierQuery.PageSize = num2;
			balanceDrawRequestSupplierQuery.PageIndex = num;
			return balanceDrawRequestSupplierQuery;
		}

		public DataGridViewModel<SupplierBalanceDrawRequestInfo> GetBalance(BalanceDrawRequestSupplierQuery query)
		{
			DataGridViewModel<SupplierBalanceDrawRequestInfo> dataGridViewModel = new DataGridViewModel<SupplierBalanceDrawRequestInfo>();
			if (query != null)
			{
				PageModel<SupplierBalanceDrawRequestInfo> balanceDrawRequests = BalanceHelper.GetBalanceDrawRequests(query, false);
				List<SupplierBalanceDrawRequestInfo> list = new List<SupplierBalanceDrawRequestInfo>();
				foreach (SupplierBalanceDrawRequestInfo model in balanceDrawRequests.Models)
				{
					if (model.IsAlipay)
					{
						model.DrawType = "支付宝支付";
						model.ReceiverName = model.AlipayRealName;
						model.ReceiverID = model.AlipayCode;
					}
					else
					{
						model.DrawType = "银行卡转账";
						model.ReceiverName = model.AccountName;
						model.ReceiverID = model.MerchantCode;
					}
					model.StateStr = this.ParseDrawStatus(model.IsPass);
					if (model.IsPass == true && model.IsPass.HasValue)
					{
						model.AccountDateStr = model.AccountDate.ToString();
					}
					else
					{
						model.AccountDateStr = "--";
					}
					list.Add(model);
				}
				dataGridViewModel.rows = balanceDrawRequests.Models.ToList();
				dataGridViewModel.total = balanceDrawRequests.Total;
			}
			return dataGridViewModel;
		}

		public string ParseDrawStatus(object status)
		{
			if (status == null || string.IsNullOrEmpty(status.ToString()))
			{
				return "审核中";
			}
			if (status.ToString() == "True")
			{
				return "已通过审核";
			}
			return "拒绝";
		}
	}
}
