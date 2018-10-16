using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Promotions;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.UI.Web.Admin.promotion.ashx
{
	public class ChooseCoupons : AdminBaseHandler
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
			CouponsSearch couponsSearch = new CouponsSearch();
			couponsSearch.CouponName = base.GetParameter(context, "CouponName", true);
			couponsSearch.ObtainWay = 1;
			couponsSearch.IsValid = true;
			couponsSearch.PageIndex = base.CurrentPageIndex;
			couponsSearch.PageSize = base.CurrentPageSize;
			string parameter = base.GetParameter(context, "NotInCouponIds", true);
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList(couponsSearch, parameter);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList(CouponsSearch query, string NotInCouponIds = "")
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			if (query != null)
			{
				string sWhere = "";
				if (!string.IsNullOrWhiteSpace(NotInCouponIds))
				{
					sWhere = "and COUPONID NOT IN ('" + NotInCouponIds.Replace(",", "','") + "')";
				}
				DbQueryResult couponInfos = CouponHelper.GetCouponInfos(query, sWhere);
				dataGridViewModel.rows = DataHelper.DataTableToDictionary(couponInfos.Data);
				dataGridViewModel.total = couponInfos.TotalRecords;
				foreach (Dictionary<string, object> row in dataGridViewModel.rows)
				{
					row.Add("CouponSurplus", this.GetCouponSurplus(row["CouponId"].ToInt(0)));
				}
			}
			return dataGridViewModel;
		}

		private int GetCouponSurplus(int CouponId)
		{
			return CouponHelper.GetCouponSurplus(CouponId);
		}
	}
}
