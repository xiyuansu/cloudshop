using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Hidistro.Core
{
	public static class ObjectExtends
	{
		public static string F2ToString(this object obj, string format = "f2")
		{
			if (obj == null)
			{
				return "";
			}
			if ((obj.IsDecimal() || obj.IsDouble() || obj.IsInt() || obj.IsFloat()) && (format == "f2" || format == "F2"))
			{
				string text = obj.ToNullString();
				int length = obj.ToNullString().Length;
				int num = obj.ToNullString().LastIndexOf(".");
				string text2 = "";
				if (num > -1)
				{
					switch (length - num)
					{
					case 1:
						text2 = text + "00";
						break;
					case 2:
						text2 = text + "0";
						break;
					case 3:
						text2 = text;
						break;
					default:
						text2 = text.Substring(0, num + 3);
						break;
					}
				}
				else
				{
					text2 = text + ".00";
				}
				return text2;
			}
			return obj.ToString();
		}

		public static string ToNullString(this object obj)
		{
			return (obj == null && obj != DBNull.Value) ? string.Empty : obj.ToString().Trim();
		}

		public static int ToInt(this object obj, int defaultValue = 0)
		{
			if (obj == null || obj == DBNull.Value)
			{
				return defaultValue;
			}
			if (typeof(decimal) == obj.GetType())
			{
				obj = Math.Floor((decimal)obj);
			}
			else if (typeof(double) == obj.GetType())
			{
				obj = Math.Floor((double)obj);
			}
			else if (typeof(string) == obj.GetType() && obj.ToNullString().IndexOf('.') > -1)
			{
				obj = obj.ToNullString().Substring(0, obj.ToNullString().IndexOf('.'));
			}
			string s = obj.ToNullString();
			int result = defaultValue;
			int.TryParse(s, out result);
			return result;
		}

		public static long ToLong(this object obj, int defaultValue = 0)
		{
			string s = obj.ToNullString();
			long result = defaultValue;
			long.TryParse(s, out result);
			return result;
		}

		public static bool ToBool(this object obj)
		{
			bool result = false;
			string text = obj.ToNullString();
			if (text == "1")
			{
				return true;
			}
			bool.TryParse(text, out result);
			return result;
		}

		public static decimal ToDecimal(this object obj, int defaultValue = 0)
		{
			string s = obj.ToNullString();
			decimal result = defaultValue;
			decimal.TryParse(s, out result);
			return result;
		}

		public static decimal ToDecimal_MoreDot(this object obj, int defaultValue = 0)
		{
			string text = obj.ToNullString();
			int num = 0;
			for (int i = 0; i < text.Length; i++)
			{
				if (text.Substring(i, 1) == ".")
				{
					num++;
				}
			}
			if (num > 1)
			{
				num = 0;
				string text2 = "";
				for (int j = 0; j < text.Length; j++)
				{
					if (text.Substring(j, 1) == ".")
					{
						num++;
						if (num <= 1)
						{
							goto IL_0087;
						}
						continue;
					}
					goto IL_0087;
					IL_0087:
					text2 += text.Substring(j, 1);
				}
				text = text2;
			}
			decimal result = defaultValue;
			decimal.TryParse(text, out result);
			return result;
		}

		public static double ToDouble(this object obj, int defaultValue = 0)
		{
			string s = obj.ToNullString();
			double result = (double)defaultValue;
			double.TryParse(s, out result);
			return result;
		}

		public static DateTime? ToDateTime(this object obj)
		{
			string s = obj.ToNullString();
			DateTime minValue = DateTime.MinValue;
			DateTime.TryParse(s, out minValue);
			if (minValue == DateTime.MinValue)
			{
				return null;
			}
			return minValue;
		}

		public static bool IsDecimal(this object obj)
		{
			decimal num = default(decimal);
			return decimal.TryParse(obj.ToNullString(), out num);
		}

		public static bool IsDouble(this object obj)
		{
			double num = 0.0;
			return double.TryParse(obj.ToNullString(), out num);
		}

		public static bool IsFloat(this object obj)
		{
			float num = 0f;
			return float.TryParse(obj.ToNullString(), out num);
		}

		public static bool IsInt(this object obj)
		{
			int num = 0;
			return int.TryParse(obj.ToNullString(), out num);
		}

		public static bool IsPositiveInteger(this object obj)
		{
			return Regex.IsMatch(obj.ToNullString(), "[0-9]\\d*");
		}

		public static Dictionary<string, object> ToDictionary(this object o)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			Type type = o.GetType();
			PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
			PropertyInfo[] array = properties;
			foreach (PropertyInfo propertyInfo in array)
			{
				MethodInfo getMethod = propertyInfo.GetGetMethod();
				if (getMethod != (MethodInfo)null && getMethod.IsPublic)
				{
					dictionary.Add(propertyInfo.Name, getMethod.Invoke(o, new object[0]));
				}
			}
			return dictionary;
		}
	}
}
