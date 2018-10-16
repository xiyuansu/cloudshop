using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Promotions;
using Hidistro.UI.Web.ashxBase;
using System;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.UI.Web.Admin.promotion.ashx
{
	public class CountDownsDetails : AdminBaseHandler
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
			CountDownQuery countDownQuery = new CountDownQuery();
			countDownQuery.CountDownId = base.GetIntParam(context, "CountDownId", false).Value;
			countDownQuery.PageIndex = num;
			countDownQuery.PageSize = num2;
			if (!string.IsNullOrEmpty(context.Request.QueryString["StoreId"]))
			{
				countDownQuery.StoreId = base.GetIntParam(context, "StoreId", false).Value;
			}
			if (!string.IsNullOrEmpty(context.Request.QueryString["OrderStatus"]))
			{
				countDownQuery.OrderState = (OrderStatus)base.GetIntParam(context, "OrderStatus", false).Value;
			}
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList(countDownQuery);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList(CountDownQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			if (query != null)
			{
				DbQueryResult countDownTotalList = PromoteHelper.GetCountDownTotalList(query);
				dataGridViewModel.rows = DataHelper.DataTableToDictionary(countDownTotalList.Data);
				dataGridViewModel.total = countDownTotalList.TotalRecords;
				foreach (Dictionary<string, object> row in dataGridViewModel.rows)
				{
					row.Add("OrderStatusText", EnumDescription.GetEnumDescription((Enum)(object)(OrderStatus)row["OrderStatus"], 0));
				}
			}
			return dataGridViewModel;
		}
	}
}
