using System;
using System.Collections.Generic;

namespace Hishop.Weixin.Pay.Util
{
	internal class PayDictionary : Dictionary<string, string>
	{
		public void Add(string key, object value)
		{
			string value2 = (value != null) ? ((!(value is string)) ? ((!(value is DateTime)) ? ((!(value is DateTime?)) ? ((!(value is decimal)) ? ((!(value is decimal?)) ? value.ToString() : $"{((decimal?)(object)(value as decimal?)).Value:F0}") : $"{value:F2}") : ((DateTime?)(object)(value as DateTime?)).Value.ToString("yyyyMMddHHmmss")) : ((DateTime)value).ToString("yyyyMMddHHmmss")) : ((string)value)) : null;
			this.Add(key, value2);
		}

		public new void Add(string key, string value)
		{
			if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
			{
				base.Add(key, value);
			}
		}
	}
}
