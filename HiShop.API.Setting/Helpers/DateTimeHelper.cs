using System;

namespace HiShop.API.Setting.Helpers
{
	public static class DateTimeHelper
	{
		public static DateTime BaseTime = new DateTime(1970, 1, 1);

		public static DateTime GetDateTimeFromXml(long dateTimeFromXml)
		{
			return DateTimeHelper.BaseTime.AddTicks((dateTimeFromXml + 28800) * 10000000);
		}

		public static DateTime GetDateTimeFromXml(string dateTimeFromXml)
		{
			return DateTimeHelper.GetDateTimeFromXml(long.Parse(dateTimeFromXml));
		}

		public static long GetWeixinDateTime(DateTime dateTime)
		{
			return (dateTime.Ticks - DateTimeHelper.BaseTime.Ticks) / 10000000 - 28800;
		}
	}
}
