using Hidistro.Core;
using Hidistro.Core.Jobs;
using Hidistro.Entities;
using Hidistro.SaleSystem.Statistics;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;

namespace Hidistro.Jobs
{
	public class WXFansJob : IJob
	{
		public void Execute(XmlNode node)
		{
			try
			{
				DateTime now = DateTime.Now;
				List<WXMenuClickInfo> list = (List<WXMenuClickInfo>)HiCache.Get("DataCache-WXMenuClickRecords");
				if (list != null && list.Count > 0)
				{
					HiCache.Remove("DataCache-WXMenuClickRecords");
					Database database = DatabaseFactory.CreateDatabase();
					StringBuilder stringBuilder = new StringBuilder();
					foreach (WXMenuClickInfo item in list)
					{
						stringBuilder.AppendFormat("INSERT INTO [Hishop_MenuClickRecords] (MenuId,WXOpenId,ClickDate) VALUES({0},'{1}','{2}');", item.MenuId, item.WXOpenId, item.ClickDate.ToString("yyyy-MM-dd"));
					}
					if (!string.IsNullOrEmpty(stringBuilder.ToString()))
					{
						database.ExecuteNonQuery(CommandType.Text, stringBuilder.ToString());
					}
				}
				WXFansJob.SynchroWXFansData(now);
			}
			catch (Exception ex)
			{
				Globals.WriteExceptionLog(ex, null, "WXFansJob");
			}
		}

		public static void SynchroWXFansData(DateTime dt)
		{
			if (dt.Hour == 2 || dt.Hour == 4 || dt.Hour == 6 || dt.Hour == 8)
			{
				object obj = HiCache.Get("DataCache-WXDataSynchroDateKey");
				if (obj == null)
				{
					WXFansJob.SynchroWXFansDataAction();
				}
				else if (Convert.ToDateTime(obj).Date < dt.Date)
				{
					WXFansJob.SynchroWXFansDataAction();
				}
			}
		}

		public static void SynchroWXFansDataAction()
		{
			bool flag = false;
			DateTime now = DateTime.Now;
			DateTime? wxFansLastStatisticalDate = WXFansHelper.GetWxFansLastStatisticalDate();
			DateTime dateTime = DateTime.Now;
			dateTime = dateTime.AddDays(-1.0);
			DateTime startDate = dateTime.Date;
			dateTime = DateTime.Now;
			dateTime = dateTime.AddDays(-1.0);
			DateTime date = dateTime.Date;
			int num;
			if (wxFansLastStatisticalDate.HasValue)
			{
				if (wxFansLastStatisticalDate.HasValue)
				{
					DateTime value = wxFansLastStatisticalDate.Value;
					dateTime = DateTime.Now;
					dateTime = dateTime.AddDays(-1.0);
					num = ((value < dateTime.Date) ? 1 : 0);
				}
				else
				{
					num = 0;
				}
			}
			else
			{
				num = 1;
			}
			if (num != 0)
			{
				if (wxFansLastStatisticalDate.HasValue)
				{
					dateTime = wxFansLastStatisticalDate.Value;
					startDate = dateTime.AddDays(1.0);
				}
				WXFansHelper.SynchroWXFansData(startDate, date, out flag);
			}
			wxFansLastStatisticalDate = WXFansHelper.GetWxFansInteractLastStatisticalDate();
			dateTime = DateTime.Now;
			dateTime = dateTime.AddDays(-1.0);
			startDate = dateTime.Date;
			dateTime = DateTime.Now;
			dateTime = dateTime.AddDays(-1.0);
			date = dateTime.Date;
			int num2;
			if (wxFansLastStatisticalDate.HasValue)
			{
				if (wxFansLastStatisticalDate.HasValue)
				{
					DateTime value2 = wxFansLastStatisticalDate.Value;
					dateTime = DateTime.Now;
					dateTime = dateTime.AddDays(-1.0);
					num2 = ((value2 < dateTime.Date) ? 1 : 0);
				}
				else
				{
					num2 = 0;
				}
			}
			else
			{
				num2 = 1;
			}
			if (num2 != 0)
			{
				if (wxFansLastStatisticalDate.HasValue)
				{
					dateTime = wxFansLastStatisticalDate.Value;
					startDate = dateTime.AddDays(1.0);
				}
				WXFansHelper.SynchroWXFansInteractData(startDate, date, out flag);
			}
			HiCache.Insert("DataCache-WXDataSynchroDateKey", now.Date, 5400);
		}
	}
}
