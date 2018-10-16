using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities.Statistics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Statistics
{
	public class WXFansInteractStatisticsDao : BaseDao
	{
		public PageModel<WXFansInteractStatisticsInfo> GetWxFansInteractList(WXFansStatisticsQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder(" 1 = 1 ");
			DateTime dateTime;
			switch (query.LastConsumeTime)
			{
			case EnumConsumeTime.yesterday:
			{
				StringBuilder stringBuilder6 = stringBuilder;
				dateTime = DateTime.Now;
				dateTime = dateTime.AddDays(-1.0);
				stringBuilder6.AppendFormat(" AND DATEDIFF(dd,StatisticalDate,'{0}') = 0 ", dateTime.ToString("yyyy-MM-dd"));
				break;
			}
			case EnumConsumeTime.inOneWeek:
			{
				StringBuilder stringBuilder3 = stringBuilder;
				dateTime = DateTime.Now;
				dateTime = dateTime.AddDays(-7.0);
				string arg2 = dateTime.ToString("yyyy-MM-dd");
				dateTime = DateTime.Now;
				dateTime = dateTime.Date;
				stringBuilder3.AppendFormat(" AND (StatisticalDate BETWEEN '{0}' AND '{1}')", arg2, dateTime.ToString("yyyy-MM-dd"));
				break;
			}
			case EnumConsumeTime.inTwoWeek:
			{
				StringBuilder stringBuilder4 = stringBuilder;
				dateTime = DateTime.Now;
				dateTime = dateTime.AddDays(-14.0);
				string arg3 = dateTime.ToString("yyyy-MM-dd");
				dateTime = DateTime.Now;
				dateTime = dateTime.Date;
				stringBuilder4.AppendFormat(" AND (StatisticalDate BETWEEN '{0}' AND '{1}')", arg3, dateTime.ToString("yyyy-MM-dd"));
				break;
			}
			case EnumConsumeTime.inOneMonth:
			{
				StringBuilder stringBuilder5 = stringBuilder;
				dateTime = DateTime.Now;
				dateTime = dateTime.AddMonths(-1);
				string arg4 = dateTime.ToString("yyyy-MM-dd");
				dateTime = DateTime.Now;
				dateTime = dateTime.Date;
				stringBuilder5.AppendFormat(" AND (StatisticalDate BETWEEN '{0}' AND '{1}')", arg4, dateTime.ToString("yyyy-MM-dd"));
				break;
			}
			case EnumConsumeTime.custom:
				if (query.CustomConsumeStartTime.HasValue && query.CustomConsumeEndTime.HasValue)
				{
					StringBuilder stringBuilder2 = stringBuilder;
					dateTime = query.CustomConsumeStartTime.Value;
					string arg = dateTime.ToString("yyyy-MM-dd");
					dateTime = query.CustomConsumeEndTime.Value;
					stringBuilder2.AppendFormat(" AND (StatisticalDate BETWEEN '{0}' AND '{1}')", arg, dateTime.ToString("yyyy-MM-dd"));
				}
				break;
			}
			SortAction sortOrder = query.SortOrder;
			string sortBy = "StatisticalDate";
			if (!string.IsNullOrEmpty(query.SortBy))
			{
				sortBy = query.SortBy;
			}
			return DataHelper.PagingByRownumber<WXFansInteractStatisticsInfo>(query.PageIndex, query.PageSize, sortBy, sortOrder, true, "Hishop_WXFansInteractStatistics", "Id", stringBuilder.ToString(), "*,(MsgSendNumbers+MenuClickNumbers) as InteractNumbers,(MsgSendTimes+MenuClickTimes) as InteractTimes");
		}

		public IList<WXFansInteractStatisticsInfo> GetWxFansInteractListNoPage(WXFansStatisticsQuery query)
		{
			IList<WXFansInteractStatisticsInfo> result = new List<WXFansInteractStatisticsInfo>();
			string format = "SELECT *,(MsgSendNumbers+MenuClickNumbers) as InteractNumbers,(MsgSendTimes+MenuClickTimes) as InteractTimes FROM Hishop_WXFansInteractStatistics WHERE {0} ORDER BY {1}";
			StringBuilder stringBuilder = new StringBuilder(" 1 = 1 ");
			DateTime dateTime;
			switch (query.LastConsumeTime)
			{
			case EnumConsumeTime.yesterday:
			{
				StringBuilder stringBuilder6 = stringBuilder;
				dateTime = DateTime.Now;
				dateTime = dateTime.AddDays(-1.0);
				stringBuilder6.AppendFormat(" AND DATEDIFF(dd,StatisticalDate,'{0}') = 0 ", dateTime.ToString("yyyy-MM-dd"));
				break;
			}
			case EnumConsumeTime.inOneWeek:
			{
				StringBuilder stringBuilder3 = stringBuilder;
				dateTime = DateTime.Now;
				dateTime = dateTime.AddDays(-7.0);
				string arg2 = dateTime.ToString("yyyy-MM-dd");
				dateTime = DateTime.Now;
				dateTime = dateTime.Date;
				stringBuilder3.AppendFormat(" AND (StatisticalDate BETWEEN '{0}' AND '{1}')", arg2, dateTime.ToString("yyyy-MM-dd"));
				break;
			}
			case EnumConsumeTime.inTwoWeek:
			{
				StringBuilder stringBuilder4 = stringBuilder;
				dateTime = DateTime.Now;
				dateTime = dateTime.AddDays(-14.0);
				string arg3 = dateTime.ToString("yyyy-MM-dd");
				dateTime = DateTime.Now;
				dateTime = dateTime.Date;
				stringBuilder4.AppendFormat(" AND (StatisticalDate BETWEEN '{0}' AND '{1}')", arg3, dateTime.ToString("yyyy-MM-dd"));
				break;
			}
			case EnumConsumeTime.inOneMonth:
			{
				StringBuilder stringBuilder5 = stringBuilder;
				dateTime = DateTime.Now;
				dateTime = dateTime.AddMonths(-1);
				string arg4 = dateTime.ToString("yyyy-MM-dd");
				dateTime = DateTime.Now;
				dateTime = dateTime.Date;
				stringBuilder5.AppendFormat(" AND (StatisticalDate BETWEEN '{0}' AND '{1}')", arg4, dateTime.ToString("yyyy-MM-dd"));
				break;
			}
			case EnumConsumeTime.custom:
				if (query.CustomConsumeStartTime.HasValue && query.CustomConsumeEndTime.HasValue)
				{
					StringBuilder stringBuilder2 = stringBuilder;
					dateTime = query.CustomConsumeStartTime.Value;
					string arg = dateTime.ToString("yyyy-MM-dd");
					dateTime = query.CustomConsumeEndTime.Value;
					stringBuilder2.AppendFormat(" AND (StatisticalDate BETWEEN '{0}' AND '{1}')", arg, dateTime.ToString("yyyy-MM-dd"));
				}
				break;
			}
			SortAction sortOrder = query.SortOrder;
			string str = "StatisticalDate";
			if (!string.IsNullOrEmpty(query.SortBy))
			{
				str = query.SortBy;
			}
			string arg5 = str + " " + ((sortOrder == SortAction.Desc) ? "DESC" : "ASC");
			format = string.Format(format, stringBuilder.ToString(), arg5);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(format);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<WXFansInteractStatisticsInfo>(objReader);
			}
			return result;
		}

		public void ClearAllData()
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM Hishop_WXFansInteractStatistics");
			base.database.ExecuteNonQuery(sqlStringCommand);
		}

		public bool IsExistData(DateTime dt)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(string.Format("SELECT COUNT(*) FROM Hishop_WXFansInteractStatistics WHERE  DATEDIFF(dd,StatisticalDate,'{0}') = 0 ", dt.ToString("yyyy-MM-dd")));
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0) > 0;
		}

		public DateTime? GetLastStatisticalDate()
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT TOP 1 StatisticalDate FROM Hishop_WXFansInteractStatistics ORDER BY StatisticalDate DESC");
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					return dataReader.GetDateTime(0);
				}
				return null;
			}
		}
	}
}
