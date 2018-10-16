using System;
using System.ComponentModel;
using System.Reflection;

namespace Hidistro.Entities
{
	public class EnumDescription
	{
		public static string GetEnumDescription(Enum enumSubitem, int index = 0)
		{
			string text = enumSubitem.ToString();
			FieldInfo field = enumSubitem.GetType().GetField(text);
			if (field == (FieldInfo)null)
			{
				return string.Empty;
			}
			object[] customAttributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
			if (customAttributes == null || customAttributes.Length == 0)
			{
				return text;
			}
			DescriptionAttribute descriptionAttribute = (DescriptionAttribute)customAttributes[0];
			return descriptionAttribute.Description.Split('|')[index];
		}

		public static bool GetEnumValue<TEnum>(string enumDescription, ref TEnum currentfiled)
		{
			bool result = false;
			Type typeFromHandle = typeof(TEnum);
			FieldInfo[] fields = typeFromHandle.GetFields();
			for (int i = 1; i < fields.Length - 1; i++)
			{
				DescriptionAttribute descriptionAttribute = fields[i].GetCustomAttributes(typeof(DescriptionAttribute), false)[0] as DescriptionAttribute;
				if (descriptionAttribute.Description.Contains(enumDescription))
				{
					currentfiled = (TEnum)fields[i].GetValue(typeof(TEnum));
					result = true;
					break;
				}
			}
			return result;
		}
	}
}
