using System;

namespace Hidistro.Core.Extends
{
	public static class DateTimeExtends
	{
		public static long DateTimeToUnixTimestamp(this DateTime dateTime)
		{
			DateTime d = new DateTime(1970, 1, 1, 0, 0, 0, dateTime.Kind);
			return Convert.ToInt64((dateTime - d).TotalSeconds);
		}

		public static DateTime UnixTimestampToDateTime(this DateTime target, long timestamp)
		{
			return new DateTime(1970, 1, 1, 0, 0, 0, target.Kind).AddSeconds((double)timestamp);
		}
	}
}
