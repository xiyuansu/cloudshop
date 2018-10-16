using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Hidistro.Core
{
	public static class DictionaryExtends
	{
		public static T ToObject<T>(this Dictionary<string, object> dic) where T : new()
		{
			T val = new T();
			Type typeFromHandle = typeof(T);
			foreach (KeyValuePair<string, object> item in dic)
			{
				string name = item.Key.Replace("_", "");
				PropertyInfo property = typeFromHandle.GetProperty(name, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);
				if (property != (PropertyInfo)null && property.CanWrite)
				{
					object obj = item.Value;
					Type type = property.PropertyType;
					bool flag = false;
					if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
					{
						NullableConverter nullableConverter = new NullableConverter(type);
						type = nullableConverter.UnderlyingType;
						flag = true;
					}
					if (Convert.IsDBNull(obj))
					{
						obj = ((!type.IsPrimitive && !type.IsEnum) ? null : Activator.CreateInstance(type));
					}
					if (type.IsEnum)
					{
						object value = null;
						if (obj != null)
						{
							value = Enum.ToObject(type, obj);
						}
						property.SetValue(val, value, null);
					}
					else
					{
						object obj2 = DictionaryExtends.CheckType(obj, type);
						if (obj2 is string)
						{
							obj2 = obj2.ToNullString();
						}
						property.SetValue(val, obj2, null);
					}
				}
			}
			return val;
		}

		private static object CheckType(object value, Type conversionType)
		{
			if (value == null)
			{
				return null;
			}
			return Convert.ChangeType(value, conversionType);
		}
	}
}
