using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Store;
using Hidistro.SqlDal.Store;
using System.Collections.Generic;
using System.Linq;

namespace Hidistro.SaleSystem.Store
{
	public class OnlineServiceHelper
	{
		public static DbQueryResult GetOnelineService(OnlineServiceQuery query)
		{
			return new OnlineServiceDao().GetOnlineService(query);
		}

		public static OnlineServiceInfo GetOnlineServiceInfo(int ServiceId)
		{
			IList<OnlineServiceInfo> source = new OnlineServiceDao().Gets<OnlineServiceInfo>("OrderId", SortAction.Asc, null);
			if (ServiceId > 0)
			{
				return source.FirstOrDefault((OnlineServiceInfo t) => t.Id == ServiceId);
			}
			return (from t in source
			orderby t.OrderId, t.Id
			select t).FirstOrDefault();
		}

		public static IList<OnlineServiceInfo> GetAllOnlineService(int ServiceType = 0, int Status = -1)
		{
			IQueryable<OnlineServiceInfo> source = new OnlineServiceDao().Gets<OnlineServiceInfo>("OrderId", SortAction.Desc, null).AsQueryable();
			if (ServiceType > 0)
			{
				source = from t in source
				where t.ServiceType == ServiceType
				select t;
			}
			if (Status >= 0)
			{
				source = from t in source
				where t.Status == Status
				select t;
			}
			return source.ToList();
		}

		public static bool SaveOnlineService(OnlineServiceInfo info)
		{
			return new OnlineServiceDao().Add(info, null) > 0;
		}

		public static bool Delete(int ServiceId)
		{
			ManagerHelper.CheckPrivilege(Privilege.SiteSettings);
			return new OnlineServiceDao().Delete<OnlineServiceInfo>(ServiceId);
		}

		public static bool UpdateDisplaySequence(int serviceId, int displaySequence)
		{
			return new OnlineServiceDao().SaveSequence<OnlineServiceInfo>(serviceId, displaySequence, null);
		}

		public static bool Update(OnlineServiceInfo ServiceInfo)
		{
			return new OnlineServiceDao().Update(ServiceInfo, null);
		}
	}
}
