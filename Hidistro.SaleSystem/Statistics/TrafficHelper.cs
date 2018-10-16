using Hidistro.Entities.Sales;
using Hidistro.Entities.Statistics;
using Hidistro.SqlDal.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hidistro.SaleSystem.Statistics
{
	public static class TrafficHelper
	{
		public static IList<Traffics> GetPageview(TrafficQuery query)
		{
			DateTime dateTime;
			if (query.LastConsumeTime == EnumConsumeTime.custom)
			{
				if (query.CustomConsumeStartTime > query.CustomConsumeEndTime)
				{
					return null;
				}
			}
			else if (query.LastConsumeTime == EnumConsumeTime.inOneMonth)
			{
				dateTime = DateTime.Now;
				dateTime = dateTime.Date;
				query.CustomConsumeEndTime = dateTime.AddDays(-1.0);
				dateTime = query.CustomConsumeEndTime;
				query.CustomConsumeStartTime = dateTime.AddDays(-29.0);
			}
			else if (query.LastConsumeTime == EnumConsumeTime.inOneWeek)
			{
				dateTime = DateTime.Now;
				dateTime = dateTime.Date;
				query.CustomConsumeEndTime = dateTime.AddDays(-1.0);
				dateTime = query.CustomConsumeEndTime;
				query.CustomConsumeStartTime = dateTime.AddDays(-6.0);
			}
			else if (query.LastConsumeTime == EnumConsumeTime.preThreeMonth)
			{
				dateTime = DateTime.Now;
				dateTime = dateTime.Date;
				query.CustomConsumeEndTime = dateTime.AddDays(-1.0);
				dateTime = query.CustomConsumeEndTime;
				query.CustomConsumeStartTime = dateTime.AddDays(-89.0);
			}
			else
			{
				dateTime = DateTime.Now;
				dateTime = dateTime.Date;
				query.CustomConsumeEndTime = dateTime.AddDays(-1.0);
				query.CustomConsumeStartTime = query.CustomConsumeEndTime;
			}
			IList<Traffics> list = new TrafficDao().GetPageview(query);
			if (query.CustomConsumeStartTime < query.CustomConsumeEndTime)
			{
				DateTime dtDay = query.CustomConsumeStartTime;
				while (dtDay <= query.CustomConsumeEndTime)
				{
					if (list.FirstOrDefault((Traffics c) => c.StatisticalDate == dtDay) == null)
					{
						Traffics traffics = new Traffics();
						traffics.StatisticalDate = dtDay;
						list.Add(traffics);
					}
					dtDay = dtDay.AddDays(1.0);
				}
				list = (from c in list
				orderby c.StatisticalDate
				select c).ToList();
			}
			return list;
		}

		public static IList<TrafficSourceScope> GetPageviewSource(TrafficQuery query)
		{
			DateTime dateTime;
			if (query.LastConsumeTime == EnumConsumeTime.custom)
			{
				if (query.CustomConsumeStartTime > query.CustomConsumeEndTime)
				{
					return null;
				}
			}
			else if (query.LastConsumeTime == EnumConsumeTime.inOneMonth)
			{
				dateTime = DateTime.Now;
				dateTime = dateTime.Date;
				query.CustomConsumeEndTime = dateTime.AddDays(-1.0);
				dateTime = query.CustomConsumeEndTime;
				query.CustomConsumeStartTime = dateTime.AddDays(-29.0);
			}
			else if (query.LastConsumeTime == EnumConsumeTime.inOneWeek)
			{
				dateTime = DateTime.Now;
				dateTime = dateTime.Date;
				query.CustomConsumeEndTime = dateTime.AddDays(-1.0);
				dateTime = query.CustomConsumeEndTime;
				query.CustomConsumeStartTime = dateTime.AddDays(-6.0);
			}
			else if (query.LastConsumeTime == EnumConsumeTime.preThreeMonth)
			{
				dateTime = DateTime.Now;
				dateTime = dateTime.Date;
				query.CustomConsumeEndTime = dateTime.AddDays(-1.0);
				dateTime = query.CustomConsumeEndTime;
				query.CustomConsumeStartTime = dateTime.AddDays(-89.0);
			}
			else
			{
				dateTime = DateTime.Now;
				dateTime = dateTime.Date;
				query.CustomConsumeEndTime = dateTime.AddDays(-1.0);
				query.CustomConsumeStartTime = query.CustomConsumeEndTime;
			}
			return new TrafficDao().GetPageviewSource(query);
		}
	}
}
