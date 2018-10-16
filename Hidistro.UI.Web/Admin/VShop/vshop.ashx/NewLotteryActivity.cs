using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Promotions;
using Hidistro.UI.Web.ashxBase;
using System;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.UI.Web.Admin.vshop.ashx
{
	public class NewLotteryActivity : AdminBaseHandler
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
			LotteryActivityQuery lotteryActivityQuery = new LotteryActivityQuery();
			lotteryActivityQuery.ActivityName = context.Request["ActivityName"];
			lotteryActivityQuery.Stateus = (ActivityTypeStateus)base.GetIntParam(context, "Stateus", false).Value;
			lotteryActivityQuery.ActivityType = (LotteryActivityType)base.GetIntParam(context, "ActivityType", false).Value;
			lotteryActivityQuery.PageIndex = num;
			lotteryActivityQuery.PageSize = num2;
			lotteryActivityQuery.SortBy = "ActivityId";
			lotteryActivityQuery.SortOrder = SortAction.Desc;
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList(lotteryActivityQuery);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList(LotteryActivityQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			if (query != null)
			{
				PageModel<ActivityInfo> activityList = ActivityHelper.GetActivityList(query);
				dataGridViewModel.rows = new List<Dictionary<string, object>>();
				dataGridViewModel.total = activityList.Total;
				foreach (ActivityInfo model in activityList.Models)
				{
					List<ActivityAwardItemInfo> activityItemList = ActivityHelper.GetActivityItemList(model.ActivityId);
					Dictionary<string, object> dictionary = model.ToDictionary();
					dictionary.Add("StatusText", this.GetAcitivityStateus(model.StartDate, model.EndDate));
					List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
					foreach (ActivityAwardItemInfo item in activityItemList)
					{
						Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
						dictionary2.Add("AwardGradeText", this.GetWeekCN(item.AwardGrade));
						dictionary2.Add("PrizeType", this.GetPrizeType((PrizeTypes)item.PrizeType, item.PrizeValue));
						list.Add(dictionary2);
					}
					dictionary.Add("AwardItem", list);
					dataGridViewModel.rows.Add(dictionary);
				}
			}
			return dataGridViewModel;
		}

		public string GetAcitivityStateus(DateTime? startDate, DateTime? endDate)
		{
			DateTime now = DateTime.Now;
			if ((DateTime?)now >= startDate && (DateTime?)now <= endDate)
			{
				return "进行中";
			}
			if ((DateTime?)now > startDate && (DateTime?)now > endDate)
			{
				return "已结束";
			}
			if ((DateTime?)now < startDate && (DateTime?)now < endDate)
			{
				return "未开始";
			}
			return "";
		}

		public string GetWeekCN(int n)
		{
			string[] array = new string[7]
			{
				"",
				"一等奖",
				"二等奖",
				"三等奖",
				"四等奖",
				"五等奖",
				"六等奖"
			};
			return array[n];
		}

		public string GetPrizeType(PrizeTypes PrizeType, int PrizeValue)
		{
			string result = string.Empty;
			switch (PrizeType)
			{
			case PrizeTypes.Integral:
				result = PrizeValue + "积分; ";
				break;
			case PrizeTypes.Coupou:
			{
				CouponInfo coupon = CouponHelper.GetCoupon(PrizeValue);
				result = ((coupon == null) ? " " : ((!(coupon.ClosingTime > coupon.StartTime)) ? (coupon.Price + "元优惠券<font style='color:red;'>(已失效)</font>; ") : (coupon.Price + "元优惠券; ")));
				break;
			}
			case PrizeTypes.Gift:
			{
				GiftInfo giftDetails = GiftHelper.GetGiftDetails(PrizeValue);
				result = ((giftDetails == null) ? " " : (giftDetails.Name + "; "));
				break;
			}
			}
			return result;
		}

		public void Delete(HttpContext context)
		{
			int? intParam = base.GetIntParam(context, "id", false);
			if (!intParam.HasValue)
			{
				throw new HidistroAshxException("错误的编号");
			}
			if (ActivityHelper.DeleteActivityInfo(intParam.Value))
			{
				base.ReturnSuccessResult(context, "删除成功", 0, true);
				return;
			}
			throw new HidistroAshxException("删除失败!");
		}

		private void SetOver(HttpContext context)
		{
			int? intParam = base.GetIntParam(context, "id", false);
			if (!intParam.HasValue)
			{
				throw new HidistroAshxException("错误的编号");
			}
			ActivityInfo activityInfo = ActivityHelper.GetActivityInfo(intParam.Value);
			if (ActivityHelper.UpdateEndDate(DateTime.Now, intParam.Value) > 0)
			{
				base.ReturnSuccessResult(context, "操作成功", 0, true);
				return;
			}
			throw new HidistroAshxException("操作失败");
		}
	}
}
