using System;
using System.Web;

namespace Hishop.Weixin.Pay.Util
{
	internal class UrlHelper
	{
		public static string GetStringUrlParam(string key)
		{
			return UrlHelper.GetStringUrlParam(key, string.Empty);
		}

		public static string GetStringUrlParam(string key, string defaultValue)
		{
			return HttpContext.Current.Request.QueryString[key] ?? defaultValue;
		}

		public static int GetIntUrlParam(string key)
		{
			return UrlHelper.GetIntUrlParam(key, 0);
		}

		public static int GetIntUrlParam(string key, int defaultValue)
		{
			string text = HttpContext.Current.Request.QueryString[key];
			if (text == null)
			{
				return defaultValue;
			}
			try
			{
				return Convert.ToInt32(text);
			}
			catch (FormatException)
			{
				return defaultValue;
			}
		}
	}
}
