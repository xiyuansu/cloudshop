using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Depot;
using Hidistro.SqlDal.Depot;
using System.Collections.Generic;

namespace Hidistro.SaleSystem
{
	public static class StoreActivityHelper
	{
		internal static void SaveStoreActivity(string storeIds, int acivityId, int type)
		{
			string[] array = storeIds.Split(',');
			List<StoreActivityInfo> list = new List<StoreActivityInfo>();
			string[] array2 = array;
			foreach (string obj in array2)
			{
				list.Add(new StoreActivityInfo
				{
					ActivityId = acivityId,
					ActivityType = type,
					StoreId = obj.ToInt(0)
				});
			}
			StoreActivityDao storeActivityDao = new StoreActivityDao();
			storeActivityDao.Save(list);
		}

		public static List<StoreBase> GetActivityStores(int activityId, int type, int storeType)
		{
			if (storeType == 0)
			{
				return new List<StoreBase>
				{
					new StoreBase
					{
						StoreId = 0,
						StoreName = "平台店"
					}
				};
			}
			StoreActivityDao storeActivityDao = new StoreActivityDao();
			return storeActivityDao.GetActivityStores(activityId, type, storeType == 2);
		}

		public static bool JoinActivity(int activityId, int type, int storeId, int storeType)
		{
			bool flag = false;
			if (storeId == 0)
			{
				if (storeType == 2)
				{
					return new StoreActivityDao().CheckStoreJoin(activityId, type, storeId);
				}
				return true;
			}
			switch (storeType)
			{
			case 0:
				return false;
			case 1:
				return true;
			default:
				return new StoreActivityDao().CheckStoreJoin(activityId, type, storeId);
			}
		}
	}
}
