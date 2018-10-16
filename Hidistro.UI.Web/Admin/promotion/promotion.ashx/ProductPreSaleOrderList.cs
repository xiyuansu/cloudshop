using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Promotions;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.UI.Web.Admin.promotion.ashx
{
	public class ProductPreSaleOrderList : AdminBaseHandler
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
			int value = base.GetIntParam(context, "PreSaleId", false).Value;
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList(value, num, num2);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList(int preSaleId, int pageIndex, int pageSize)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			if (preSaleId > 0)
			{
				PageModel<ProductPreSaleOrderInfo> preSaleOrderList = ProductPreSaleHelper.GetPreSaleOrderList(preSaleId, pageIndex, pageSize);
				dataGridViewModel.rows = new List<Dictionary<string, object>>();
				dataGridViewModel.total = preSaleOrderList.Total;
				foreach (ProductPreSaleOrderInfo model in preSaleOrderList.Models)
				{
					Dictionary<string, object> item = model.ToDictionary();
					dataGridViewModel.rows.Add(item);
				}
			}
			return dataGridViewModel;
		}
	}
}
