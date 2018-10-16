using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Depot;
using Hidistro.SaleSystem.Depot;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace Hidistro.UI.Web.Admin.depot.ashx
{
	public class RquestStatistics : AdminBaseHandler
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
			if (action == "getlist")
			{
				this.GetList(context);
				return;
			}
			throw new HidistroAshxException("错误的参数");
		}

		public void GetList(HttpContext context)
		{
			StoreBalanceDetailQuery dataQuery = this.GetDataQuery(context);
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList(dataQuery);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private StoreBalanceDetailQuery GetDataQuery(HttpContext context)
		{
			StoreBalanceDetailQuery storeBalanceDetailQuery = new StoreBalanceDetailQuery();
			storeBalanceDetailQuery.StoreName = base.GetParameter(context, "Key", true);
			storeBalanceDetailQuery.PageIndex = base.CurrentPageIndex;
			storeBalanceDetailQuery.PageSize = base.CurrentPageSize;
			return storeBalanceDetailQuery;
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList(StoreBalanceDetailQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			if (query != null)
			{
				DbQueryResult balanceStatisticsList = StoreBalanceHelper.GetBalanceStatisticsList(query);
				dataGridViewModel.rows = new List<Dictionary<string, object>>();
				dataGridViewModel.total = balanceStatisticsList.TotalRecords;
				foreach (DataRow row in balanceStatisticsList.Data.Rows)
				{
					Dictionary<string, object> dictionary = DataHelper.DataRowToDictionary(row);
					dictionary.Add("AvailableBalance", dictionary["Balance"].ToDecimal(0) - dictionary["BalanceFozen"].ToDecimal(0));
					dataGridViewModel.rows.Add(dictionary);
				}
			}
			return dataGridViewModel;
		}
	}
}
