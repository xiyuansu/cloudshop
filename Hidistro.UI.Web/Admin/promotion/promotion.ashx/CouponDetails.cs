using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Promotions;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.UI.Web.Admin.promotion.ashx
{
	public class CouponDetails : AdminBaseHandler
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
			CouponItemInfoQuery couponItemInfoQuery = new CouponItemInfoQuery();
			couponItemInfoQuery.CouponId = base.GetIntParam(context, "CouponId", false).Value;
			couponItemInfoQuery.OrderId = base.GetParameter(context, "OrderId", false);
			couponItemInfoQuery.CouponStatus = base.GetIntParam(context, "CouponStatus", true);
			couponItemInfoQuery.PageIndex = num;
			couponItemInfoQuery.PageSize = num2;
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList(couponItemInfoQuery);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList(CouponItemInfoQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			if (query != null)
			{
				DbQueryResult couponsUseList = CouponHelper.GetCouponsUseList(query);
				dataGridViewModel.rows = DataHelper.DataTableToDictionary(couponsUseList.Data);
				dataGridViewModel.total = couponsUseList.TotalRecords;
			}
			return dataGridViewModel;
		}
	}
}
