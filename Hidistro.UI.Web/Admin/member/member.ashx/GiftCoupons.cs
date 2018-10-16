using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Promotions;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.UI.Web.Admin.member.ashx
{
	public class GiftCoupons : AdminBaseHandler
	{
		private int UserNum;

		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			if (!string.IsNullOrWhiteSpace(base.action))
			{
				base.action = base.action.ToLower();
			}
			this.UserNum = base.GetIntParam(context, "UserNum", false).Value;
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
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList(couponsSearch);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList(CouponsSearch query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			if (query != null)
			{
				DbQueryResult couponInfos = CouponHelper.GetCouponInfos(query, "");
				dataGridViewModel.rows = DataHelper.DataTableToDictionary(couponInfos.Data);
				dataGridViewModel.total = couponInfos.TotalRecords;
				foreach (Dictionary<string, object> row in dataGridViewModel.rows)
				{
					row.Add("CouponSurplus", this.GetCouponSurplus(row["CouponId"].ToInt(0)));
				}
			}
			return dataGridViewModel;
		}

		private string GetCouponSurplus(int CouponId)
		{
			int couponSurplus = CouponHelper.GetCouponSurplus(CouponId);
			int cantObtainUserNum = CouponHelper.GetCantObtainUserNum(CouponId);
			if (couponSurplus >= this.UserNum - cantObtainUserNum)
			{
				return couponSurplus.ToString();
			}
			return "<span>" + couponSurplus + "</span><span style='color:red;'>(数量不足)</span>";
		}
	}
}
