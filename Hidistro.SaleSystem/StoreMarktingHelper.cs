using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.SqlDal;
using System.Collections.Generic;
using System.Linq;

namespace Hidistro.SaleSystem
{
	public static class StoreMarktingHelper
	{
		public static List<StoreMarktingInfo> GetStoreMarktingInfoList()
		{
			return new StoreMarktingDao().Gets<StoreMarktingInfo>("DisplaySequence", SortAction.Desc, null).ToList();
		}

		public static int AddInfo(StoreMarktingInfo info)
		{
			StoreMarktingDao storeMarktingDao = new StoreMarktingDao();
			if (!storeMarktingDao.TypeValidate(info.Id, info.MarktingType))
			{
				return -1;
			}
			info.DisplaySequence = storeMarktingDao.GetMaxDisplaySequence<StoreMarktingInfo>();
			info.RedirectTo = info.RedirectUrl;
			return (storeMarktingDao.Add(info, null) > 0) ? 1 : 2;
		}

		public static bool Delete(int id)
		{
			return new StoreMarktingDao().Delete<StoreMarktingInfo>(id);
		}

		public static StoreMarktingInfo GetStoreMarktingInfo(int id)
		{
			return new StoreMarktingDao().Get<StoreMarktingInfo>(id);
		}

		public static int Edit(StoreMarktingInfo info)
		{
			StoreMarktingDao storeMarktingDao = new StoreMarktingDao();
			if (!storeMarktingDao.TypeValidate(info.Id, info.MarktingType))
			{
				return -1;
			}
			info.RedirectTo = info.RedirectUrl;
			return storeMarktingDao.Update(info, null) ? 1 : 2;
		}

		public static bool UpdateDisplaySequence(int id, int displaySequence)
		{
			return new StoreMarktingDao().SaveSequence<StoreMarktingInfo>(id, displaySequence, null);
		}
	}
}
