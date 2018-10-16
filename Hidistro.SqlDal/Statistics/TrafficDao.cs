using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Statistics;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Statistics
{
	public class TrafficDao : BaseDao
	{
		public IList<Traffics> GetPageview(TrafficQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder("  select sum(PV) PV,sum(UV) UV, StatisticalDate  from  [dbo].[Hishop_DailyAccessStatistics] where ");
			stringBuilder.AppendFormat(" StatisticalDate between '{0}' and '{1}'", query.CustomConsumeStartTime, query.CustomConsumeEndTime);
			if (query.PageType >= 0)
			{
				stringBuilder.Append("AND PageType=" + query.PageType);
			}
			if (query.PageType == 1)
			{
				stringBuilder.Append(" AND StoreId = 0");
			}
			stringBuilder.Append("  group by StatisticalDate");
			IList<Traffics> result = new List<Traffics>();
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = (DataHelper.ReaderToList<Traffics>(objReader) as List<Traffics>);
			}
			return result;
		}

		public IList<TrafficSourceScope> GetPageviewSource(TrafficQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder(" PageType=" + 4);
			if (query.Type.ToLower() == "pv")
			{
				stringBuilder.Append("select SourceId,sum(pv) as scope from [Hishop_DailyAccessStatistics] where ");
			}
			else if (query.Type.ToLower() == "uv")
			{
				stringBuilder.Append("select SourceId,sum(uv) as scope from [Hishop_DailyAccessStatistics] where ");
			}
			stringBuilder2.AppendFormat(" AND StatisticalDate between '{0}' and '{1}'", query.CustomConsumeStartTime, query.CustomConsumeEndTime);
			stringBuilder.Append(stringBuilder2);
			stringBuilder.Append(" group by SourceId");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			IList<TrafficSourceScope> result = new List<TrafficSourceScope>();
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = (DataHelper.ReaderToList<TrafficSourceScope>(objReader) as List<TrafficSourceScope>);
			}
			return result;
		}
	}
}
