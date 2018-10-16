using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Sales;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.UI.Web.Admin.member.ashx
{
	public class MemberDetails : AdminBaseHandler
	{
		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			if (!string.IsNullOrWhiteSpace(base.action))
			{
				base.action = base.action.ToLower();
			}
			string action = base.action;
			if (action == "getlist")
			{
				this.GetList(context);
				return;
			}
			throw new HidistroAshxException("错误的参数");
		}

		private void GetList(HttpContext context)
		{
			OrderQuery dataQuery = this.GetDataQuery(context);
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList(dataQuery);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private OrderQuery GetDataQuery(HttpContext context)
		{
			OrderQuery orderQuery = new OrderQuery();
			orderQuery.IsPay = true;
			orderQuery.IsAllOrder = true;
			orderQuery.UserId = base.GetIntParam(context, "UserId", false);
			orderQuery.PageIndex = base.CurrentPageIndex;
			orderQuery.PageSize = base.CurrentPageSize;
			orderQuery.SortBy = "OrderDate";
			orderQuery.SortOrder = SortAction.Desc;
			return orderQuery;
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList(OrderQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			if (query != null)
			{
				DbQueryResult orders = OrderHelper.GetOrders(query);
				dataGridViewModel.rows = DataHelper.DataTableToDictionary(orders.Data);
				dataGridViewModel.total = orders.TotalRecords;
				foreach (Dictionary<string, object> row in dataGridViewModel.rows)
				{
					decimal d = row["OrderTotal"].ToDecimal(0);
					decimal d2 = row["RefundAmount"].ToDecimal(0);
					row.Add("ConsumptionAmount", d - d2);
				}
			}
			return dataGridViewModel;
		}
	}
}
