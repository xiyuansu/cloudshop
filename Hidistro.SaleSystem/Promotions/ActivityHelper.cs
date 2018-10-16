using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.VShop;
using Hidistro.SqlDal.Members;
using Hidistro.SqlDal.Promotions;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Hidistro.SaleSystem.Promotions
{
	public static class ActivityHelper
	{
		private static object oWinning = new object();

		public static List<ActivityAwardItemInfo> GetActivityItemList(int ActivityId)
		{
			return new ActivityDao().GetActivityItemList(ActivityId);
		}

		public static ActivityAwardItemInfo GetActivityItem(int ActivityId, int AwardGrade)
		{
			return new ActivityDao().GetActivityItem(ActivityId, AwardGrade, null);
		}

		public static ActivityInfo GetActivityInfo(int ActivityId)
		{
			return new ActivityDao().Get<ActivityInfo>(ActivityId);
		}

		public static long AddActivityInfo(ActivityInfo Info, DbTransaction dbtran)
		{
			return new ActivityDao().Add(Info, dbtran);
		}

		public static ActivityJoinStatisticsInfo GetCurrUserActivityStatisticsInfo(int UserId, int ActivityId)
		{
			return new ActivityDao().GetCurrUserActivityStatisticsInfo(UserId, ActivityId, null);
		}

		public static bool ExistValueInActivity(int ValueId, ActivityEnumPrizeType PrizeType)
		{
			return new ActivityDao().ExistValueInActivity(ValueId, PrizeType);
		}

		public static bool ExistGiftNoReceive(int GiftId)
		{
			return new ActivityDao().ExistGiftNoReceive(GiftId);
		}

		public static bool ManageWinningResult(int UserId, ActivityInfo Info, int AwardGrade, ref int FreeTimes)
		{
			lock (ActivityHelper.oWinning)
			{
				Database database = DatabaseFactory.CreateDatabase();
				ActivityDao activityDao = new ActivityDao();
				using (DbConnection dbConnection = database.CreateConnection())
				{
					dbConnection.Open();
					DbTransaction dbTransaction = dbConnection.BeginTransaction();
					try
					{
						bool flag = false;
						bool flag2 = false;
						ActivityJoinStatisticsInfo activityJoinStatisticsInfo = activityDao.GetCurrUserActivityStatisticsInfo(UserId, Info.ActivityId, dbTransaction);
						MemberInfo memberInfo = new MemberDao().Get<MemberInfo>(UserId);
						if (activityJoinStatisticsInfo != null)
						{
							if (Info.ResetType == 2)
							{
								DateTime lastJoinDate = activityJoinStatisticsInfo.LastJoinDate;
								if (DateTime.Now.Date == lastJoinDate.Date)
								{
									if (activityJoinStatisticsInfo.FreeNum < Info.FreeTimes)
									{
										flag = true;
										activityJoinStatisticsInfo.FreeNum++;
										FreeTimes = Info.FreeTimes - activityJoinStatisticsInfo.FreeNum;
									}
									else
									{
										activityJoinStatisticsInfo.IntegralTotal += Info.ConsumptionIntegral;
										activityJoinStatisticsInfo.IntegralNum++;
										FreeTimes = 0;
									}
								}
								else
								{
									flag = true;
									activityJoinStatisticsInfo.FreeNum = 1;
									FreeTimes = Info.FreeTimes - 1;
								}
							}
							else if (activityJoinStatisticsInfo.FreeNum < Info.FreeTimes)
							{
								flag = true;
								activityJoinStatisticsInfo.FreeNum++;
								FreeTimes = Info.FreeTimes - activityJoinStatisticsInfo.FreeNum;
							}
							else
							{
								activityJoinStatisticsInfo.IntegralTotal += Info.ConsumptionIntegral;
								activityJoinStatisticsInfo.IntegralNum++;
								FreeTimes = 0;
							}
						}
						else
						{
							flag2 = true;
							activityJoinStatisticsInfo = new ActivityJoinStatisticsInfo();
							activityJoinStatisticsInfo.ActivityId = Info.ActivityId;
							activityJoinStatisticsInfo.UserId = UserId;
							if (Info.FreeTimes > 0)
							{
								flag = true;
								activityJoinStatisticsInfo.FreeNum = 1;
								activityJoinStatisticsInfo.IntegralTotal = 0;
								activityJoinStatisticsInfo.IntegralNum = 0;
								FreeTimes = Info.FreeTimes - 1;
							}
							else
							{
								activityJoinStatisticsInfo.IntegralTotal = Info.ConsumptionIntegral;
								activityJoinStatisticsInfo.IntegralNum = 1;
								activityJoinStatisticsInfo.FreeNum = 0;
								FreeTimes = 0;
							}
						}
						activityJoinStatisticsInfo.JoinNum++;
						activityJoinStatisticsInfo.LastJoinDate = DateTime.Now;
						bool flag3 = false;
						CouponInfo couponInfo = null;
						GiftInfo giftInfo = null;
						ActivityAwardItemInfo activityAwardItemInfo = null;
						if (AwardGrade > 0)
						{
							activityAwardItemInfo = activityDao.GetActivityItem(Info.ActivityId, AwardGrade, dbTransaction);
							if (activityAwardItemInfo.WinningNum < activityAwardItemInfo.AwardNum)
							{
								if (activityAwardItemInfo.PrizeType == 2)
								{
									couponInfo = new CouponDao().Get<CouponInfo>(activityAwardItemInfo.PrizeValue);
									if (couponInfo != null)
									{
										int couponSurplus = new CouponDao().GetCouponSurplus(activityAwardItemInfo.PrizeValue);
										if (couponSurplus > 0 && couponInfo.ClosingTime > DateTime.Now)
										{
											flag3 = true;
										}
									}
								}
								else if (activityAwardItemInfo.PrizeType == 3)
								{
									giftInfo = new GiftDao().Get<GiftInfo>(activityAwardItemInfo.PrizeValue);
									if (giftInfo != null)
									{
										flag3 = true;
									}
								}
								else
								{
									flag3 = true;
								}
							}
						}
						else
						{
							flag3 = false;
						}
						if (!flag)
						{
							PointDetailInfo pointDetailInfo = new PointDetailInfo();
							pointDetailInfo.Increased = 0;
							pointDetailInfo.OrderId = "";
							pointDetailInfo.Points = memberInfo.Points - Info.ConsumptionIntegral;
							pointDetailInfo.Reduced = Info.ConsumptionIntegral;
							pointDetailInfo.Remark = "抽奖消耗积分";
							pointDetailInfo.SignInSource = 0;
							pointDetailInfo.TradeDate = DateTime.Now;
							if (Info.ActivityType == 1)
							{
								pointDetailInfo.TradeType = PointTradeType.JoinRotaryTable;
							}
							else if (Info.ActivityType == 3)
							{
								pointDetailInfo.TradeType = PointTradeType.JoinSmashingGoldenEgg;
							}
							else
							{
								pointDetailInfo.TradeType = PointTradeType.JoinScratchCard;
							}
							pointDetailInfo.UserId = UserId;
							if (new PointDetailDao().Add(pointDetailInfo, dbTransaction) <= 0)
							{
								dbTransaction.Rollback();
								return false;
							}
						}
						if (!flag3)
						{
							if (flag2)
							{
								if (activityDao.Add(activityJoinStatisticsInfo, dbTransaction) <= 0)
								{
									dbTransaction.Rollback();
								}
							}
							else if (!activityDao.UpdateActivityStatisticsInfo(activityJoinStatisticsInfo, dbTransaction))
							{
								dbTransaction.Rollback();
							}
							dbTransaction.Commit();
							return false;
						}
						activityJoinStatisticsInfo.WinningNum++;
						if (flag2)
						{
							if (activityDao.Add(activityJoinStatisticsInfo, dbTransaction) <= 0)
							{
								dbTransaction.Rollback();
							}
						}
						else if (!activityDao.UpdateActivityStatisticsInfo(activityJoinStatisticsInfo, dbTransaction))
						{
							dbTransaction.Rollback();
						}
						activityAwardItemInfo.WinningNum++;
						if (!activityDao.Update(activityAwardItemInfo, dbTransaction))
						{
							dbTransaction.Rollback();
							return false;
						}
						UserAwardRecordsInfo userAwardRecordsInfo = new UserAwardRecordsInfo();
						userAwardRecordsInfo.ActivityId = Info.ActivityId;
						if (activityAwardItemInfo.PrizeType == 2)
						{
							userAwardRecordsInfo.AwardName = couponInfo.CouponName;
							userAwardRecordsInfo.AwardDate = DateTime.Now;
							userAwardRecordsInfo.Status = 2;
							CouponItemInfo couponItemInfo = new CouponItemInfo();
							couponItemInfo.UserId = UserId;
							couponItemInfo.UserName = memberInfo.UserName;
							couponItemInfo.CanUseProducts = couponInfo.CanUseProducts;
							couponItemInfo.ClosingTime = couponInfo.ClosingTime;
							couponItemInfo.CouponId = couponInfo.CouponId;
							couponItemInfo.CouponName = couponInfo.CouponName;
							couponItemInfo.OrderUseLimit = couponInfo.OrderUseLimit;
							couponItemInfo.Price = couponInfo.Price;
							couponItemInfo.StartTime = couponInfo.StartTime;
							couponItemInfo.UseWithGroup = couponInfo.UseWithGroup;
							couponItemInfo.UseWithPanicBuying = couponInfo.UseWithPanicBuying;
							couponItemInfo.UseWithFireGroup = couponInfo.UseWithFireGroup;
							couponItemInfo.GetDate = DateTime.Now;
							couponItemInfo.ClaimCode = Guid.NewGuid().ToString();
							if (new CouponDao().Add(couponItemInfo, dbTransaction) <= 0)
							{
								dbTransaction.Rollback();
								return false;
							}
						}
						else if (activityAwardItemInfo.PrizeType == 3)
						{
							userAwardRecordsInfo.AwardDate = null;
							userAwardRecordsInfo.AwardName = giftInfo.Name;
							userAwardRecordsInfo.AwardPic = giftInfo.ThumbnailUrl160;
							userAwardRecordsInfo.Status = 1;
						}
						else
						{
							userAwardRecordsInfo.AwardName = activityAwardItemInfo.PrizeValue + "积分";
							userAwardRecordsInfo.AwardDate = DateTime.Now;
							userAwardRecordsInfo.Status = 2;
							PointDetailInfo pointDetailInfo2 = new PointDetailInfo();
							pointDetailInfo2.Increased = activityAwardItemInfo.PrizeValue;
							pointDetailInfo2.OrderId = "";
							if (flag)
							{
								pointDetailInfo2.Points = memberInfo.Points + activityAwardItemInfo.PrizeValue;
							}
							else
							{
								pointDetailInfo2.Points = memberInfo.Points - Info.ConsumptionIntegral + activityAwardItemInfo.PrizeValue;
							}
							pointDetailInfo2.Reduced = 0;
							pointDetailInfo2.Remark = "抽奖获得积分";
							pointDetailInfo2.SignInSource = 0;
							pointDetailInfo2.TradeDate = DateTime.Now;
							if (Info.ActivityType == 1)
							{
								pointDetailInfo2.TradeType = PointTradeType.JoinRotaryTable;
							}
							else if (Info.ActivityType == 3)
							{
								pointDetailInfo2.TradeType = PointTradeType.JoinSmashingGoldenEgg;
							}
							else
							{
								pointDetailInfo2.TradeType = PointTradeType.JoinScratchCard;
							}
							pointDetailInfo2.UserId = UserId;
							if (new PointDetailDao().Add(pointDetailInfo2, dbTransaction) <= 0)
							{
								dbTransaction.Rollback();
								return false;
							}
						}
						userAwardRecordsInfo.AwardGrade = AwardGrade;
						userAwardRecordsInfo.AwardId = activityAwardItemInfo.AwardId;
						userAwardRecordsInfo.CreateDate = DateTime.Now;
						userAwardRecordsInfo.PrizeType = activityAwardItemInfo.PrizeType;
						userAwardRecordsInfo.PrizeValue = activityAwardItemInfo.PrizeValue;
						userAwardRecordsInfo.UserId = UserId;
						if (activityDao.Add(userAwardRecordsInfo, dbTransaction) <= 0)
						{
							dbTransaction.Rollback();
							return false;
						}
						dbTransaction.Commit();
						return true;
					}
					catch (Exception ex)
					{
						dbTransaction.Rollback();
						Globals.WriteLog("ActivityLog.txt", "Methed:ManageWinningResult , Id：" + Info.ActivityId + " , Msg：" + ex.Message);
						return false;
					}
					finally
					{
						dbConnection.Close();
					}
				}
			}
		}

		public static IList<UserAwardRecordsInfo> GetCurrUserNoReceiveAwardRecordsId(int UserId)
		{
			return new ActivityDao().GetCurrUserNoReceiveAwardRecordsId(UserId);
		}

		public static int CountCurrUserNoReceiveAward(int UserId)
		{
			return new ActivityDao().CountCurrUserNoReceiveAward(UserId);
		}

		public static PageModel<UserAwardRecordsInfo> GetCurrUserReceiveAwardRecordsId(int UserId, int PageIndex, int PageSize)
		{
			return new ActivityDao().GetCurrUserReceiveAwardRecordsId(UserId, PageIndex, PageSize);
		}

		public static int AddActivityAwardItemInfo(ActivityAwardItemInfo info, DbTransaction dbTran)
		{
			return (int)new ActivityDao().Add(info, dbTran);
		}

		public static bool UpdateActivityInfo(ActivityInfo info, DbTransaction dbTran)
		{
			return new ActivityDao().Update(info, dbTran);
		}

		public static bool DeleteActivityAwardItemByActivityId(int ActivityId, DbTransaction dbTran)
		{
			return new ActivityDao().DeleteActivityAwardItemByActivityId(ActivityId, dbTran);
		}

		public static bool DeleteActivityInfo(int ActivityId)
		{
			return new ActivityDao().Delete<ActivityInfo>(ActivityId);
		}

		public static int UpdateEndDate(DateTime EndDate, int id)
		{
			return new ActivityDao().UpdateEndDate(EndDate, id);
		}

		public static PageModel<ViewUserAwardRecordsInfo> GetAllAwardRecordsByActityId(PrizeQuery query)
		{
			return new ActivityDao().GetAllAwardRecordsByActityId(query);
		}

		public static UserAwardRecordsInfo GetUserAwardRecordsInfo(int RcordId)
		{
			return new ActivityDao().Get<UserAwardRecordsInfo>(RcordId);
		}

		public static DataSet ActivityStatistics(int ActivityId)
		{
			return new ActivityDao().ActivityStatistics(ActivityId);
		}

		public static PageModel<ActivityInfo> GetActivityList(LotteryActivityQuery page)
		{
			return new ActivityDao().GetActivityList(page);
		}

		public static PageModel<ActivityInfo> GetNotEndActivityList(EffectiveActivityQuery page)
		{
			return new ActivityDao().GetNotEndActivityList(page);
		}

		public static ActivityAwardItemInfo GetAwardItemInfoById(int AwardId)
		{
			return new ActivityDao().Get<ActivityAwardItemInfo>(AwardId);
		}

		public static bool DeleteEidAwardItem(int ActivityId, string AwardIds, DbTransaction dbTran)
		{
			return new ActivityDao().DeleteEidAwardItem(ActivityId, AwardIds, dbTran);
		}

		public static bool UpdateActivityAwardItemInfo(ActivityAwardItemInfo model, DbTransaction dbTran)
		{
			return new ActivityDao().Update(model, dbTran);
		}
	}
}
