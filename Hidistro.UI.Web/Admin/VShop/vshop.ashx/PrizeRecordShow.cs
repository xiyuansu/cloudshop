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
	public class PrizeRecordShow : AdminBaseHandler
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
			PrizeQuery prizeQuery = new PrizeQuery();
			prizeQuery.UserName = context.Request["UserName"];
			prizeQuery.ActivityId = base.GetIntParam(context, "ActivityId", false).Value;
			prizeQuery.Status = base.GetIntParam(context, "Status", false).Value;
			prizeQuery.AwardGrade = base.GetIntParam(context, "AwardGrade", false).Value;
			prizeQuery.PageIndex = num;
			prizeQuery.PageSize = num2;
			prizeQuery.SortBy = "CreateDate";
			prizeQuery.SortOrder = SortAction.Desc;
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList(prizeQuery);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList(PrizeQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			if (query != null)
			{
				PageModel<ViewUserAwardRecordsInfo> allAwardRecordsByActityId = ActivityHelper.GetAllAwardRecordsByActityId(query);
				dataGridViewModel.rows = new List<Dictionary<string, object>>();
				dataGridViewModel.total = allAwardRecordsByActityId.Total;
				foreach (ViewUserAwardRecordsInfo model in allAwardRecordsByActityId.Models)
				{
					Dictionary<string, object> dictionary = model.ToDictionary();
					dictionary.Add("AwardGradeText", this.GetWeekCN(model.AwardGrade));
					dictionary.Add("StatusText", (model.Status == 2) ? "已领奖" : "未领奖");
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
	}
}
