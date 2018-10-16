using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Depot;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.UI.Web.Admin.depot.ashx
{
	public class BindHiPOS : AdminBaseHandler
	{
		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			if (string.IsNullOrWhiteSpace(base.action))
			{
				throw new HidistroAshxException("错误的参数");
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
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList(context);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList(HttpContext context)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			string storeName = string.Empty;
			string empty = string.Empty;
			empty = context.Request["StoreName"];
			if (!string.IsNullOrEmpty(empty))
			{
				storeName = empty;
			}
			StoresQuery storesQuery = new StoresQuery();
			storesQuery.StoreName = storeName;
			storesQuery.PageIndex = base.CurrentPageIndex;
			storesQuery.PageSize = base.CurrentPageSize;
			DbQueryResult storeManagersHiPOS = ManagerHelper.GetStoreManagersHiPOS(storesQuery);
			dataGridViewModel.rows = DataHelper.DataTableToDictionary(storeManagersHiPOS.Data);
			dataGridViewModel.total = storeManagersHiPOS.TotalRecords;
			return dataGridViewModel;
		}
	}
}
