using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Hidistro.Entities
{
	public static class EnumHelper
	{
		private static Hashtable enumDesciption = EnumHelper.GetDescriptionContainer();

		public static string ToDescription(this Enum value)
		{
			if (value == null)
			{
				return "";
			}
			Type type = value.GetType();
			string name = Enum.GetName(type, value);
			return EnumHelper.GetDescription(type, name);
		}

		public static Dictionary<int, string> ToDescriptionDictionary<TEnum>()
		{
			Type typeFromHandle = typeof(TEnum);
			Array values = Enum.GetValues(typeFromHandle);
			Dictionary<int, string> dictionary = new Dictionary<int, string>();
			foreach (Enum item in values)
			{
				dictionary.Add(Convert.ToInt32(item), item.ToDescription());
			}
			return dictionary;
		}

		public static Dictionary<int, string> ToDictionary<TEnum>()
		{
			Type typeFromHandle = typeof(TEnum);
			Array values = Enum.GetValues(typeFromHandle);
			Dictionary<int, string> dictionary = new Dictionary<int, string>();
			foreach (Enum item in values)
			{
				dictionary.Add(Convert.ToInt32(item), item.ToString());
			}
			return dictionary;
		}

		private static bool IsIntType(double d)
		{
			return (double)(int)d != d;
		}

		private static Hashtable GetDescriptionContainer()
		{
			EnumHelper.enumDesciption = new Hashtable();
			return EnumHelper.enumDesciption;
		}

		private static void AddToEnumDescription(Type enumType)
		{
			EnumHelper.enumDesciption.Add(enumType, EnumHelper.GetEnumDic(enumType));
		}

		private static Dictionary<string, string> GetEnumDic(Type enumType)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			FieldInfo[] fields = enumType.GetFields();
			FieldInfo[] array = fields;
			foreach (FieldInfo fieldInfo in array)
			{
				if (fieldInfo.FieldType.IsEnum)
				{
					object[] customAttributes = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
					dictionary.Add(fieldInfo.Name, ((DescriptionAttribute)customAttributes[0]).Description);
				}
			}
			return dictionary;
		}

		private static string GetDescription(Type enumType, string enumText)
		{
			if (string.IsNullOrEmpty(enumText))
			{
				return null;
			}
			if (!EnumHelper.enumDesciption.ContainsKey(enumType))
			{
				EnumHelper.AddToEnumDescription(enumType);
			}
			object obj = EnumHelper.enumDesciption[enumType];
			if (obj != null && !string.IsNullOrEmpty(enumText))
			{
				Dictionary<string, string> dictionary = (Dictionary<string, string>)obj;
				return dictionary[enumText].Split('|')[0];
			}
			throw new ApplicationException("不存在枚举的描述");
		}
	}
}
