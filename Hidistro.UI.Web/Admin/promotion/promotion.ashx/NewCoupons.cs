using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Promotions;
using Hidistro.UI.Web.ashxBase;
using System;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.UI.Web.Admin.promotion.ashx
{
	public class NewCoupons : AdminBaseHandler
	{
		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			if (string.IsNullOrWhiteSpace(base.action))
			{
				throw new HidistroAshxException("错误的参数");
			}
			base.action = base.action.ToLower();
			switch (base.action)
			{
			case "getlist":
				this.GetList(context);
				break;
			case "delete":
				this.Delete(context);
				break;
			case "setover":
				this.SetOver(context);
				break;
			default:
				throw new HidistroAshxException("错误的参数");
			}
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
			CouponsSearch couponsSearch = new CouponsSearch();
			couponsSearch.CouponName = context.Request["CouponName"];
			couponsSearch.State = base.GetIntParam(context, "State", true);
			couponsSearch.ObtainWay = base.GetIntParam(context, "ObtainWay", true);
			couponsSearch.PageIndex = num;
			couponsSearch.PageSize = num2;
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
					CouponInfo couponInfo = row.ToObject<CouponInfo>();
					int couponSurplus = CouponHelper.GetCouponSurplus(couponInfo.CouponId);
					row.Add("LastCount", couponSurplus);
					int couponObtainUserNum = CouponHelper.GetCouponObtainUserNum(couponInfo.CouponId);
					int couponObtainNum = CouponHelper.GetCouponObtainNum(couponInfo.CouponId, 0);
					row.Add("UserCount", couponObtainUserNum);
					row.Add("UserGetCount", couponObtainNum);
					int couponUsedNum = CouponHelper.GetCouponUsedNum(couponInfo.CouponId);
					row.Add("UseCount", couponUsedNum);
					row.Add("IsCouponEnd", this.IsCouponEnd(couponInfo.ClosingTime));
				}
			}
			return dataGridViewModel;
		}

		private bool IsCouponEnd(DateTime dt)
		{
			if (dt.CompareTo(DateTime.Now) > 0)
			{
				return true;
			}
			return false;
		}

		public void Delete(HttpContext context)
		{
			int value = base.GetIntParam(context, "CouponId", false).Value;
			if (CouponHelper.DeleteCoupon(value))
			{
				base.ReturnSuccessResult(context, "删除成功", 0, true);
				return;
			}
			throw new HidistroAshxException("删除优惠券失败!");
		}

		private void SetOver(HttpContext context)
		{
			int? intParam = base.GetIntParam(context, "CouponId", false);
			if (!intParam.HasValue)
			{
				throw new HidistroAshxException("错误的编号");
			}
			if (CouponHelper.LetInvalidCoupon(intParam.Value))
			{
				base.ReturnSuccessResult(context, "操作优惠券失效成功", 0, true);
				return;
			}
			throw new HidistroAshxException("操作优惠券失效失败");
		}
	}
}
