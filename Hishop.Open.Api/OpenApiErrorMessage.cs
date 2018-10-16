using System;
using System.ComponentModel;

namespace Hishop.Open.Api
{
	public static class OpenApiErrorMessage
	{
		public static string GetEnumDescription(Enum enumSubitem)
		{
			string text = enumSubitem.ToString();
			object[] customAttributes = enumSubitem.GetType().GetField(text).GetCustomAttributes(typeof(DescriptionAttribute), false);
			if (customAttributes != null && customAttributes.Length != 0)
			{
				return ((DescriptionAttribute)customAttributes[0]).Description;
			}
			return text;
		}

		public static string ShowErrorMsg(Enum enumSubitem, string fields)
		{
			string text = OpenApiErrorMessage.GetEnumDescription(enumSubitem).Replace("_", " ");
			return string.Format("{{\"error_response\":{{\"code\":\"{0}\",\"msg\":\"{1}:{2}\",\"sub_msg\":\"{3}\"}}}}", Convert.ToInt16(enumSubitem).ToString(), enumSubitem.ToString().Replace("_", " "), fields, text);
		}
	}
}
