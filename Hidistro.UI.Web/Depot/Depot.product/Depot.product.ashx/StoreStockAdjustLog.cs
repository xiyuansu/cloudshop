using Hidistro.Core;
using Hidistro.Entities.Depot;
using Hidistro.SaleSystem.Depot;
using Hidistro.UI.Web.ashxBase;
using System.Linq;
using System.Web;

namespace Hidistro.UI.Web.Depot.product.ashx
{
	public class StoreStockAdjustLog : StoreAdminBaseHandler
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
			StoreStockLogQuery storeStockLogQuery = new StoreStockLogQuery();
			storeStockLogQuery.StoreId = base.CurrentManager.StoreId;
			storeStockLogQuery.ProductId = base.GetIntParam(context, "ProductId", true);
			storeStockLogQuery.StartTime = base.GetDateTimeParam(context, "StartTime");
			storeStockLogQuery.EndTime = base.GetDateTimeParam(context, "EndTime");
			storeStockLogQuery.PageIndex = base.CurrentPageIndex;
			storeStockLogQuery.PageSize = base.CurrentPageSize;
			DataGridViewModel<StoreStockLogInfo> dataList = this.GetDataList(storeStockLogQuery);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<StoreStockLogInfo> GetDataList(StoreStockLogQuery query)
		{
			DataGridViewModel<StoreStockLogInfo> dataGridViewModel = new DataGridViewModel<StoreStockLogInfo>();
			if (query != null)
			{
				PageModel<StoreStockLogInfo> storeStockLog = StoresHelper.GetStoreStockLog(query);
				dataGridViewModel.rows = storeStockLog.Models.ToList();
				dataGridViewModel.total = storeStockLog.Total;
			}
			return dataGridViewModel;
		}
	}
}
