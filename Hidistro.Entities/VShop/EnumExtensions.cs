using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Web.UI.WebControls;

namespace Hidistro.Entities.VShop
{
	public static class EnumExtensions
	{
		private static Dictionary<string, string> _cacheEnumShowTextdDic = new Dictionary<string, string>();

		public static string ToShowText(this Enum en)
		{
			return en.ToShowText(false, ",");
		}

		public static string ToShowText(this Enum en, bool exceptionIfFail, string flagsSeparator)
		{
			string enumFullName = EnumExtensions.GetEnumFullName(en);
			string showText = default(string);
			if (!EnumExtensions._cacheEnumShowTextdDic.TryGetValue(enumFullName, out showText))
			{
				Type type = en.GetType();
				object[] customAttributes = type.GetCustomAttributes(typeof(FlagsAttribute), false);
				if (customAttributes != null && customAttributes.Length != 0)
				{
					long num = Convert.ToInt64(en);
					StringBuilder stringBuilder = new StringBuilder();
					string[] names = Enum.GetNames(type);
					string result = "";
					string[] array = names;
					foreach (string text in array)
					{
						long num2 = Convert.ToInt64(Enum.Parse(type, text));
						if (num2 == 0)
						{
							object[] customAttributes2 = type.GetField(text).GetCustomAttributes(typeof(EnumShowTextAttribute), false);
							if (customAttributes2.Length != 0)
							{
								result = ((EnumShowTextAttribute)customAttributes2[0]).ShowText;
							}
						}
						else if ((num2 & num) == num2)
						{
							if (stringBuilder.Length != 0)
							{
								stringBuilder.Append(flagsSeparator);
							}
							object[] customAttributes3 = type.GetField(text).GetCustomAttributes(typeof(EnumShowTextAttribute), false);
							if (customAttributes3.Length != 0)
							{
								stringBuilder.Append(((EnumShowTextAttribute)customAttributes3[0]).ShowText);
							}
							else
							{
								if (exceptionIfFail)
								{
									throw new InvalidOperationException($"此枚举{enumFullName}未定义EnumShowTextAttribute");
								}
								stringBuilder.Append(text);
							}
						}
					}
					if (stringBuilder.Length > 0)
					{
						return stringBuilder.ToString();
					}
					return result;
				}
				FieldInfo field = type.GetField(en.ToString());
				if (field == (FieldInfo)null)
				{
					throw new InvalidOperationException($"非完整枚举{enumFullName}");
				}
				object[] customAttributes4 = field.GetCustomAttributes(typeof(EnumShowTextAttribute), false);
				if (customAttributes4.Length != 0)
				{
					showText = ((EnumShowTextAttribute)customAttributes4[0]).ShowText;
					lock (EnumExtensions._cacheEnumShowTextdDic)
					{
						EnumExtensions._cacheEnumShowTextdDic[enumFullName] = showText;
					}
					goto IL_0265;
				}
				if (exceptionIfFail)
				{
					throw new InvalidOperationException($"此枚举{enumFullName}未定义EnumShowTextAttribute");
				}
				return en.ToString();
			}
			goto IL_0265;
			IL_0265:
			return showText;
		}

		private static string GetEnumFullName(Enum en)
		{
			return en.GetType().FullName + "." + en.ToString();
		}

		public static void BindEnum<T>(this ListControl listControl, string Unbindkey = "") where T : struct
		{
			Type typeFromHandle = typeof(T);
			if (!typeFromHandle.IsEnum)
			{
				throw new InvalidOperationException("类型必须是枚举:" + typeFromHandle.FullName);
			}
			Array values = Enum.GetValues(typeFromHandle);
			if (!listControl.AppendDataBoundItems)
			{
				listControl.Items.Clear();
			}
			foreach (Enum item in values)
			{
				if (item.ToString() != Unbindkey)
				{
					listControl.Items.Add(new ListItem(item.ToShowText(), item.ToString()));
				}
			}
		}
	}
}
