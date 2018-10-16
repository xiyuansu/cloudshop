using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Promotions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace Hidistro.UI.Web.API
{
	public class ActivitysHandler : IHttpHandler, IRequiresSessionState
	{
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public void ProcessRequest(HttpContext context)
		{
			string text = context.Request["action"];
			string text2 = text;
			switch (text2)
			{
			default:
				if (text2 == "GetCurrUserAcceptPrize")
				{
					this.GetCurrUserAcceptPrize(context);
				}
				break;
			case "ActivityDraw":
				this.ActivityDraw(context);
				break;
			case "GetActivityInfo":
				this.GetActivityInfo(context);
				break;
			case "BeforeDelCoupon":
				this.BeforeDelCoupon(context);
				break;
			case "BeforeDelGift":
				this.BeforeDelGift(context);
				break;
			case "GetCurrUserNoAcceptPrize":
				this.GetCurrUserNoAcceptPrize(context);
				break;
			}
		}

		public void GetCurrUserNoAcceptPrize(HttpContext context)
		{
			MemberInfo user = HiContext.Current.User;
			if (user.UserId == 0)
			{
				this.ReturnError(context, 1001, "请先登入");
			}
			else
			{
				IList<UserAwardRecordsInfo> currUserNoReceiveAwardRecordsId = ActivityHelper.GetCurrUserNoReceiveAwardRecordsId(user.UserId);
				if (currUserNoReceiveAwardRecordsId == null)
				{
					this.ReturnError(context, 1009, "无数据");
				}
				else
				{
					currUserNoReceiveAwardRecordsId = (from c in currUserNoReceiveAwardRecordsId
					orderby c.CreateDate descending
					select c).ToList();
					string str = "{";
					str += "\"Code\":\"0\",";
					str += "\"Records\":[";
					foreach (UserAwardRecordsInfo item in currUserNoReceiveAwardRecordsId)
					{
						str = str + "{\"CreateDate\":\"" + item.CreateDate + "\",";
						str = ((item.AwardName.Length <= 13) ? (str + "\"AwardName\":\"" + item.AwardName + "\",") : (str + "\"AwardName\":\"" + item.AwardName.Substring(0, 13) + "...\","));
						str = str + "\"GiftId\":\"" + item.PrizeValue + "\",";
						str = str + "\"RecordId\":\"" + item.Id + "\",";
						str = str + "\"AwardPic\":\"" + item.AwardPic + "\"},";
					}
					str = str.Substring(0, str.Length - 1);
					str += "]";
					str += "}";
					context.Response.ContentType = "application/json";
					context.Response.Write(str);
					context.Response.End();
				}
			}
		}

		public void GetCurrUserAcceptPrize(HttpContext context)
		{
			MemberInfo user = HiContext.Current.User;
			if (user.UserId == 0)
			{
				this.ReturnError(context, 1001, "请先登入");
			}
			else
			{
				int pageIndex = context.Request["pageIndex"].ToInt(0);
				int pageSize = context.Request["pageSize"].ToInt(0);
				PageModel<UserAwardRecordsInfo> currUserReceiveAwardRecordsId = ActivityHelper.GetCurrUserReceiveAwardRecordsId(user.UserId, pageIndex, pageSize);
				if (currUserReceiveAwardRecordsId.Total == 0)
				{
					this.ReturnError(context, 1009, "无数据");
				}
				else
				{
					string str = "{";
					if (currUserReceiveAwardRecordsId.Models.Count() > 0)
					{
						str += "\"Code\":\"0\",";
						str += "\"Records\":[";
						foreach (UserAwardRecordsInfo model in currUserReceiveAwardRecordsId.Models)
						{
							str = str + "{\"CreateDate\":\"" + model.CreateDate + "\",";
							str = ((model.AwardName.Length <= 14) ? (str + "\"AwardName\":\"" + model.AwardName + "\",") : (str + "\"AwardName\":\"" + model.AwardName.Substring(0, 13) + "...\","));
							str = str + "\"GiftId\":\"" + model.PrizeValue + "\",";
							if (model.PrizeType == 3)
							{
								str = str + "\"AwardPic\":\"" + model.AwardPic + "\",";
								str = str + "\"Link\":\"MyGiftDetailInfo.aspx?RecordId=" + model.Id + "\"},";
							}
							else if (model.PrizeType == 2)
							{
								str += "\"AwardPic\":\"/Templates/common/images/ic_coupons.jpg\",";
								str += "\"Link\":\"MemberCoupons.aspx?usedType=1\"},";
							}
							else
							{
								str += "\"AwardPic\":\"/Templates/common/images/ic_point.jpg\",";
								str += "\"Link\":\"Point.aspx\"},";
							}
						}
						str = str.Substring(0, str.Length - 1);
						str += "]";
					}
					else
					{
						str += "\"Code\":\"1\"";
					}
					str += "}";
					context.Response.ContentType = "application/json";
					context.Response.Write(str);
					context.Response.End();
				}
			}
		}

		public void BeforeDelCoupon(HttpContext context)
		{
			int num = context.Request["CouponId"].ToInt(0);
			if (num > 0)
			{
				string s = "{\"Code\":\"0\"}";
				if (ActivityHelper.ExistValueInActivity(num, ActivityEnumPrizeType.Coupon))
				{
					s = "{\"Code\":\"1\",\"Msg\":\"有活动中正在使用此优惠券确定要进行此操作么?\"}";
				}
				context.Response.ContentType = "application/json";
				context.Response.Write(s);
				context.Response.End();
			}
		}

		public void BeforeDelGift(HttpContext context)
		{
			int num = context.Request["GiftId"].ToInt(0);
			if (num > 0)
			{
				string s = "{\"Code\":\"0\"}";
				if (ActivityHelper.ExistGiftNoReceive(num))
				{
					s = "{\"Code\":\"1\",\"Msg\":\"有活动存在此礼品未领取,确定要删除此礼品?\"}";
				}
				else if (ActivityHelper.ExistValueInActivity(num, ActivityEnumPrizeType.Gift))
				{
					s = "{\"Code\":\"1\",\"Msg\":\"有活动中正在使用此礼品确定要删除么?\"}";
				}
				context.Response.ContentType = "application/json";
				context.Response.Write(s);
				context.Response.End();
			}
		}

		public void ActivityDraw(HttpContext context)
		{
			int activityId = context.Request["ActivityId"].ToInt(0);
			MemberInfo user = HiContext.Current.User;
			if (user.UserId == 0)
			{
				this.ReturnError(context, 1001, "请先登入");
			}
			else
			{
				ActivityInfo activityInfo = ActivityHelper.GetActivityInfo(activityId);
				if (activityInfo == null)
				{
					this.ReturnError(context, 1007, "活动不存在!");
				}
				else if (DateTime.Now < activityInfo.StartDate)
				{
					this.ReturnError(context, 1002, "未开始");
				}
				else if (DateTime.Now > activityInfo.EndDate)
				{
					this.ReturnError(context, 1003, "已结束");
				}
				else
				{
					bool flag = false;
					ActivityJoinStatisticsInfo currUserActivityStatisticsInfo = ActivityHelper.GetCurrUserActivityStatisticsInfo(user.UserId, activityInfo.ActivityId);
					if (currUserActivityStatisticsInfo != null)
					{
						if (activityInfo.ResetType == 2)
						{
							DateTime lastJoinDate = currUserActivityStatisticsInfo.LastJoinDate;
							if (DateTime.Now.Date == lastJoinDate.Date)
							{
								if (currUserActivityStatisticsInfo.FreeNum < activityInfo.FreeTimes)
								{
									flag = true;
								}
							}
							else
							{
								flag = true;
							}
						}
						else if (currUserActivityStatisticsInfo.FreeNum < activityInfo.FreeTimes)
						{
							flag = true;
						}
					}
					else if (activityInfo.FreeTimes > 0)
					{
						flag = true;
					}
					if (!flag && user.Points < activityInfo.ConsumptionIntegral)
					{
						this.ReturnError(context, 1006, "积分不足");
					}
					else
					{
						List<ActivityAwardItemInfo> activityItemList = ActivityHelper.GetActivityItemList(activityId);
						List<double> list = new List<double>();
						decimal num = 100m;
						foreach (ActivityAwardItemInfo item in activityItemList)
						{
							num -= item.HitRate;
							list.Add((double)item.HitRate / 100.0);
						}
						if (num > decimal.Zero)
						{
							list.Add((double)num / 100.0);
						}
						AliasMethod aliasMethod = new AliasMethod(list);
						int num2 = aliasMethod.next();
						int num3 = 1;
						string empty = string.Empty;
						int awardGrade = 0;
						if (num2 < activityItemList.Count)
						{
							awardGrade = activityItemList[num2].AwardGrade;
						}
						bool flag2 = ActivityHelper.ManageWinningResult(user.UserId, activityInfo, awardGrade, ref num3);
						empty = ((!flag2) ? ("{\"Code\":\"" + 1004 + "\",\"Msg\":\"未中奖\",\"AwardGrade\":\"0\",\"FreeTime\":\"" + num3 + "\",\"Points\":\"" + user.Points + "\"}") : ("{\"Code\":\"" + 1005 + "\",\"Msg\":\"恭喜你中奖了\",\"AwardGrade\":\"" + activityItemList[num2].AwardGrade + "\",\"FreeTime\":\"" + num3 + "\",\"Points\":\"" + user.Points + "\"}"));
						if (!flag || (flag2 && activityItemList[num2].PrizeType == 1))
						{
							Users.ClearUserCache(user.UserId, "");
						}
						context.Response.ContentType = "application/json";
						context.Response.Write(empty);
						context.Response.End();
					}
				}
			}
		}

		public void GetActivityInfo(HttpContext context)
		{
			int activityId = context.Request["ActivityId"].ToInt(0);
			ActivityInfo activityInfo = ActivityHelper.GetActivityInfo(activityId);
			if (activityInfo == null)
			{
				this.ReturnError(context, 1007, "活动不存在!");
			}
			else
			{
				ActivityJsonModel activityJsonModel = new ActivityJsonModel();
				MemberInfo user = HiContext.Current.User;
				DateTime dateTime;
				if (user.UserId == 0)
				{
					activityJsonModel.FreeTimes = activityInfo.FreeTimes;
				}
				else
				{
					activityJsonModel.FreeTimes = activityInfo.FreeTimes;
					ActivityJoinStatisticsInfo currUserActivityStatisticsInfo = ActivityHelper.GetCurrUserActivityStatisticsInfo(user.UserId, activityId);
					if (currUserActivityStatisticsInfo == null)
					{
						activityJsonModel.FreeTimes = activityInfo.FreeTimes;
					}
					else if (activityInfo.ResetType == 2)
					{
						dateTime = DateTime.Now;
						DateTime date = dateTime.Date;
						dateTime = currUserActivityStatisticsInfo.LastJoinDate;
						if (date == dateTime.Date)
						{
							activityJsonModel.FreeTimes = activityInfo.FreeTimes - currUserActivityStatisticsInfo.FreeNum;
						}
						else
						{
							activityJsonModel.FreeTimes = activityInfo.FreeTimes;
						}
					}
					else
					{
						Globals.AppendLog("5", "", "", "");
						activityJsonModel.FreeTimes = activityInfo.FreeTimes - currUserActivityStatisticsInfo.FreeNum;
					}
				}
				activityJsonModel.ResetType = activityInfo.ResetType;
				activityJsonModel.ActivityId = activityInfo.ActivityId;
				activityJsonModel.ActivityName = activityInfo.ActivityName;
				activityJsonModel.ActivityType = activityInfo.ActivityType;
				activityJsonModel.ConsumptionIntegral = activityInfo.ConsumptionIntegral;
				activityJsonModel.Description = activityInfo.Description;
				ActivityJsonModel activityJsonModel2 = activityJsonModel;
				dateTime = activityInfo.EndDate;
				activityJsonModel2.EndDate = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
				ActivityJsonModel activityJsonModel3 = activityJsonModel;
				dateTime = activityInfo.StartDate;
				activityJsonModel3.StartDate = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
				if (DateTime.Now < activityInfo.StartDate)
				{
					activityJsonModel.Statu = 1;
				}
				else if (DateTime.Now > activityInfo.EndDate)
				{
					activityJsonModel.Statu = 2;
				}
				else
				{
					activityJsonModel.Statu = 0;
				}
				activityJsonModel.AwardList = new List<AwardItemInfo>();
				List<ActivityAwardItemInfo> activityItemList = ActivityHelper.GetActivityItemList(activityId);
				foreach (ActivityAwardItemInfo item in activityItemList)
				{
					AwardItemInfo awardItemInfo = new AwardItemInfo();
					awardItemInfo.ActivityId = item.ActivityId;
					awardItemInfo.AwardGrade = this.CapitalLetters(item.AwardGrade);
					awardItemInfo.AwardId = item.AwardId;
					awardItemInfo.PrizeType = item.PrizeType;
					if (item.PrizeType == 2)
					{
						CouponInfo coupon = CouponHelper.GetCoupon(item.PrizeValue);
						if (coupon != null)
						{
							awardItemInfo.AwardName = coupon.Price.F2ToString("f2") + "元";
						}
						else
						{
							awardItemInfo.AwardName = "";
						}
						awardItemInfo.AwardPic = "";
					}
					else if (item.PrizeType == 3)
					{
						GiftInfo giftDetails = GiftHelper.GetGiftDetails(item.PrizeValue);
						if (giftDetails != null)
						{
							awardItemInfo.AwardName = giftDetails.Name;
							awardItemInfo.AwardPic = giftDetails.ThumbnailUrl60;
						}
						else
						{
							awardItemInfo.AwardName = "礼品";
							awardItemInfo.AwardPic = "";
						}
					}
					else
					{
						awardItemInfo.AwardPic = "";
						awardItemInfo.AwardName = item.PrizeValue.ToString();
					}
					activityJsonModel.AwardList.Add(awardItemInfo);
				}
				string s = JsonConvert.SerializeObject(new
				{
					Result = activityJsonModel
				});
				context.Response.Write(s);
				context.Response.End();
			}
		}

		public void ReturnError(HttpContext context, int ErrorCode, string Msg)
		{
			context.Response.ContentType = "application/json";
			context.Response.Write("{\"Code\":\"" + ErrorCode + "\",\"Msg\":\"" + Msg + "\"}");
			context.Response.End();
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
	}
}
