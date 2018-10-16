using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Promotions;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal.Promotions
{
	public class GiftDao : BaseDao
	{
		public DbQueryResult GetGifts(GiftQuery query)
		{
			string text = $"[Name] LIKE '%{DataHelper.CleanSearchString(query.Name)}%'";
			if (query.IsPromotion)
			{
				text += " AND IsPromotion = 1";
			}
			if (query.IsOnline)
			{
				text += " AND NeedPoint > 0";
			}
			if (!string.IsNullOrEmpty(query.FilterGiftIds))
			{
				text += $" and GiftId not in ({query.FilterGiftIds})";
			}
			Pagination page = query.Page;
			if (query.IsOnline)
			{
				text += " AND IsPointExchange = 1";
				page.SortBy = " NeedPoint ASC,GiftId";
			}
			return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "Hishop_Gifts", "GiftId", text, "*");
		}

		public IList<GiftInfo> GetGiftDetailsByGiftIds(string giftIds)
		{
			IList<GiftInfo> result = new List<GiftInfo>();
			if (string.IsNullOrEmpty(giftIds))
			{
				return result;
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM Hishop_Gifts WHERE GiftId in (" + giftIds + ")");
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<GiftInfo>(objReader);
			}
			return result;
		}

		public IList<GiftInfo> GetOnlinePromotionGifts()
		{
			IList<GiftInfo> result = null;
			string query = "SELECT * FROM Hishop_Gifts WHERE IsPromotion=1";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<GiftInfo>(objReader);
			}
			return result;
		}

		public List<GiftInfo> GetAllGift()
		{
			string query = "select * from Hishop_Gifts ";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			List<GiftInfo> result = null;
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = (DataHelper.ReaderToList<GiftInfo>(objReader) as List<GiftInfo>);
			}
			return result;
		}
	}
}
