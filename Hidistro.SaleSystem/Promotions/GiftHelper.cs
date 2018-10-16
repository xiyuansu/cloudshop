using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Promotions;
using Hidistro.SqlDal.Promotions;
using System.Collections.Generic;

namespace Hidistro.SaleSystem.Promotions
{
	public static class GiftHelper
	{
		public static bool AddGift(GiftInfo gift)
		{
			Globals.EntityCoding(gift, true);
			return new GiftDao().Add(gift, null) > 0;
		}

		public static bool UpdateGift(GiftInfo gift)
		{
			Globals.EntityCoding(gift, true);
			return new GiftDao().Update(gift, null);
		}

		public static bool DeleteGift(int giftId)
		{
			bool result = new GiftDao().Delete<GiftInfo>(giftId);
			ActivityDao activityDao = new ActivityDao();
			if (activityDao.ExistGiftNoReceive(giftId))
			{
				activityDao.DeleteGiftNoReceives(giftId);
			}
			return result;
		}

		public static DbQueryResult GetGifts(GiftQuery query)
		{
			return new GiftDao().GetGifts(query);
		}

		public static GiftInfo GetGiftDetails(int giftId)
		{
			return new GiftDao().Get<GiftInfo>(giftId);
		}

		public static IList<GiftInfo> GetGiftDetailsByGiftIds(string giftIds)
		{
			return new GiftDao().GetGiftDetailsByGiftIds(giftIds);
		}

		public static List<GiftInfo> GetAllGift()
		{
			return new GiftDao().GetAllGift();
		}
	}
}
