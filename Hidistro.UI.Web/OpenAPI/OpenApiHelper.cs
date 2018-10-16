using Hidistro.Context;
using Hidistro.Core;
using Hishop.Open.Api;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Hidistro.UI.Web.OpenAPI
{
	public class OpenApiHelper
	{
		public static bool CheckSystemParameters(SortedDictionary<string, string> parameters, string app_key, out string result)
		{
			result = string.Empty;
			if (!parameters.ContainsKey("app_key") || string.IsNullOrEmpty(DataHelper.CleanSearchString(parameters["app_key"])))
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Missing_App_Key, "app_key");
				return false;
			}
			if (app_key != parameters["app_key"])
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Invalid_App_Key, "app_key");
				return false;
			}
			if (!parameters.Keys.Contains("timestamp") || string.IsNullOrEmpty(DataHelper.CleanSearchString(parameters["timestamp"])))
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Missing_Timestamp, "timestamp");
				return false;
			}
			if (!OpenApiHelper.IsDate(parameters["timestamp"]) || !OpenApiSign.CheckTimeStamp(parameters["timestamp"]))
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Invalid_Timestamp, "timestamp");
				return false;
			}
			if (!parameters.ContainsKey("sign") || string.IsNullOrEmpty(DataHelper.CleanSearchString(parameters["sign"])))
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Missing_Signature, "sign");
				return false;
			}
			return true;
		}

		public static bool CheckSystemParameters(string in_app_key, string in_timestamp, string in_sign, out string result)
		{
			result = string.Empty;
			SiteSettings siteSettings = HiContext.Current.SiteSettings;
			if (string.IsNullOrEmpty(DataHelper.CleanSearchString(in_app_key)))
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Missing_App_Key, "app_key");
				return false;
			}
			if (!siteSettings.AppKey.Equals(in_app_key))
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Invalid_App_Key, "app_key");
				return false;
			}
			if (string.IsNullOrEmpty(DataHelper.CleanSearchString(in_timestamp)))
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Missing_Timestamp, "timestamp");
				return false;
			}
			if (!OpenApiHelper.IsDate(in_timestamp) || !OpenApiSign.CheckTimeStamp(in_timestamp))
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Invalid_Timestamp, "timestamp");
				return false;
			}
			if (string.IsNullOrEmpty(DataHelper.CleanSearchString(in_sign)))
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Missing_Signature, "sign");
				return false;
			}
			return true;
		}

		public static SortedDictionary<string, string> GetSortedParams(HttpContext context)
		{
			SortedDictionary<string, string> sortedDictionary = new SortedDictionary<string, string>();
			NameValueCollection nameValueCollection = new NameValueCollection
			{
				context.Request.Form,
				context.Request.QueryString
			};
			string[] allKeys = nameValueCollection.AllKeys;
			for (int i = 0; i < allKeys.Length; i++)
			{
				sortedDictionary.Add(allKeys[i], nameValueCollection[allKeys[i]]);
			}
			sortedDictionary.Remove("HIGW");
			return sortedDictionary;
		}

		public static bool IsDate(string s)
		{
			DateTime dateTime = default(DateTime);
			return DateTime.TryParseExact(s, "yyyy-MM-dd HH:mm:ss", (IFormatProvider)null, DateTimeStyles.None, out dateTime);
		}
	}
}
