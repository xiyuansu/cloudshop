using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Store;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Store
{
	public class OnlineServiceDao : BaseDao
	{
		public DbQueryResult GetOnlineService(OnlineServiceQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1 ");
			if (query.ServiceType.HasValue)
			{
				stringBuilder.AppendFormat(" and ServiceType = " + query.ServiceType);
			}
			if (query.ServiceId.HasValue)
			{
				stringBuilder.AppendFormat(" and Id = " + query.ServiceId);
			}
			if (query.Status.HasValue)
			{
				stringBuilder.AppendFormat(" Status = " + query.Status);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_Service", "ID", stringBuilder.ToString(), "*");
		}

		public override bool SaveSequence<T>(int keyId, int sequence, string keyName = null)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("update Hishop_Service set OrderId=" + sequence + " WHERE  Id=" + keyId);
			return base.database.ExecuteNonQuery(sqlStringCommand) >= 1;
		}
	}
}
