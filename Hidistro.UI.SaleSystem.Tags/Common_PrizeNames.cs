using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Promotions;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_PrizeNames : WebControl
	{
		public LotteryActivityInfo Activity
		{
			get;
			set;
		}

		public int ActivityId
		{
			get;
			set;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (this.Activity != null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (PrizeSetting prizeSetting in this.Activity.PrizeSettingList)
				{
					stringBuilder.AppendFormat("{0}：{1} ({2}名)<br/>", prizeSetting.PrizeLevel, prizeSetting.PrizeName, prizeSetting.PrizeNum);
				}
				writer.Write(stringBuilder.ToString());
			}
			else if (this.ActivityId > 0)
			{
				List<ActivityAwardItemInfo> activityItemList = ActivityHelper.GetActivityItemList(this.ActivityId);
				StringBuilder stringBuilder2 = new StringBuilder();
				foreach (ActivityAwardItemInfo item in activityItemList)
				{
					stringBuilder2.AppendFormat("{0}：{1} <br/>", this.CapitalLetters(item.AwardGrade) + "等奖", this.GetPrizeName(item.PrizeType, item.PrizeValue));
				}
				writer.Write(stringBuilder2.ToString());
			}
		}

		public string CapitalLetters(int Num)
		{
			switch (Num)
			{
			case 1:
				return "一";
			case 2:
				return "二";
			case 3:
				return "三";
			case 4:
				return "四";
			case 5:
				return "五";
			default:
				return "六";
			}
		}

		public string GetPrizeName(int PrizeType, int PrizeValue)
		{
			switch (PrizeType)
			{
			case 2:
			{
				CouponInfo coupon = CouponHelper.GetCoupon(PrizeValue);
				if (coupon != null)
				{
					return coupon.Price.F2ToString("f2") + "元优惠券";
				}
				return PrizeValue + "元优惠券";
			}
			case 3:
			{
				GiftInfo giftDetails = GiftHelper.GetGiftDetails(PrizeValue);
				if (giftDetails != null)
				{
					return giftDetails.Name;
				}
				return "礼品";
			}
			default:
				return PrizeValue + "积分";
			}
		}
	}
}
