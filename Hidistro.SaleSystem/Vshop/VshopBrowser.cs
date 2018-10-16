using Hidistro.Context;
using Hidistro.Entities.VShop;
using Hidistro.SqlDal.Promotions;
using Hidistro.SqlDal.Store;
using Hidistro.SqlDal.VShop;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;

namespace Hidistro.SaleSystem.Vshop
{
	public static class VshopBrowser
	{
		public static TopicInfo GetTopic(int topicId)
		{
			return new TopicDao().GetTopic(topicId);
		}

		public static MessageInfo GetMessage(int messageId)
		{
			return new ReplyDao().Get<MessageInfo>(messageId);
		}

		public static LotteryActivityInfo GetLotteryActivity(int activityid)
		{
			LotteryActivityInfo lotteryActivityInfo = new LotteryActivityDao().Get<LotteryActivityInfo>(activityid);
			if (lotteryActivityInfo != null)
			{
				lotteryActivityInfo.PrizeSettingList = JsonConvert.DeserializeObject<List<PrizeSetting>>(lotteryActivityInfo.PrizeSetting);
			}
			return lotteryActivityInfo;
		}

		public static int GetActivityCount(int activityId)
		{
			return new ActivitySignUpDao().GetActivityCount(activityId);
		}

		public static VActivityInfo GetActivity(int activityId)
		{
			return new ActivityDao().GetActivity(activityId);
		}

		public static bool SaveActivitySignUp(ActivitySignUpInfo info)
		{
			return new ActivitySignUpDao().SaveActivitySignUp(info);
		}

		public static LotteryTicketInfo GetLotteryTicket(int activityid)
		{
			LotteryTicketInfo lotteryTicketInfo = new LotteryActivityDao().Get<LotteryTicketInfo>(activityid);
			if (lotteryTicketInfo != null)
			{
				lotteryTicketInfo.PrizeSettingList = JsonConvert.DeserializeObject<List<PrizeSetting>>(lotteryTicketInfo.PrizeSetting);
			}
			return lotteryTicketInfo;
		}

		public static DataSet GetVoteByIsShowWX()
		{
			return new VoteDao().GetShowWXVotes();
		}

		public static DataTable GetVote(int voteId, out string voteName, out int checkNum, out int voteNum)
		{
			return new VoteDao().VishopLoadVote(voteId, out voteName, out checkNum, out voteNum);
		}

		public static bool Vote(int voteId, string itemIds)
		{
			return new VoteDao().Vote(voteId, itemIds, HiContext.Current.UserId);
		}

		public static bool IsVote(int voteId)
		{
			return new VoteDao().VishopIsVote(voteId, HiContext.Current.UserId);
		}

		public static bool HasSignUp(int activityId, int userId)
		{
			return new PrizeRecordDao().HasSignUp(activityId, userId);
		}

		public static int GetCountBySignUp(int activityId)
		{
			return new PrizeRecordDao().GetCountBySignUp(activityId);
		}

		public static bool OpenTicket(int ticketId)
		{
			LotteryTicketInfo lotteryTicket = VshopBrowser.GetLotteryTicket(ticketId);
			if (new PrizeRecordDao().OpenTicket(ticketId, lotteryTicket.PrizeSettingList))
			{
				lotteryTicket.IsOpened = true;
				return new LotteryActivityDao().UpdateLotteryTicket(lotteryTicket);
			}
			return false;
		}

		public static bool UpdatePrizeRecord(PrizeRecordInfo model)
		{
			return new PrizeRecordDao().UpdatePrizeRecord(model);
		}

		public static int AddPrizeRecord(PrizeRecordInfo model)
		{
			return (int)new PrizeRecordDao().Add(model, null);
		}

		public static bool UpdatePrizeRecord(int activityId, int userId, string realName, string cellPhone)
		{
			PrizeRecordDao prizeRecordDao = new PrizeRecordDao();
			PrizeRecordInfo userPrizeRecord = prizeRecordDao.GetUserPrizeRecord(activityId, HiContext.Current.UserId);
			userPrizeRecord.UserID = userId;
			userPrizeRecord.RealName = realName;
			userPrizeRecord.CellPhone = cellPhone;
			return prizeRecordDao.UpdatePrizeRecord(userPrizeRecord);
		}

		public static int GetUserPrizeCount(int activityid)
		{
			return new PrizeRecordDao().GetUserPrizeCount(activityid, HiContext.Current.UserId);
		}

		public static PrizeRecordInfo LastPrizeRecord(int activityid)
		{
			return new PrizeRecordDao().LastPrizeRecord(activityid, HiContext.Current.UserId);
		}

		public static PrizeRecordInfo GetUserPrizeRecord(int activityid)
		{
			return new PrizeRecordDao().GetUserPrizeRecord(activityid, HiContext.Current.UserId);
		}

		public static List<PrizeRecordInfo> GetPrizeList(PrizeQuery page)
		{
			return new PrizeRecordDao().GetPrizeList(page);
		}
	}
}
