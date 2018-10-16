using Hidistro.Core.Attributes;
using System;
using System.Reflection;

namespace Hidistro.Core
{
	public static class EnumExtensions
	{
		public static string GetGlobalCode(this Enum e)
		{
			GlobalCodeAttribute globalCodeAttribute = EnumExtensions.GetGlobalCodeAttribute(e);
			return (globalCodeAttribute != null) ? globalCodeAttribute.Description : e.ToString();
		}

		public static string GetExtName(this Enum e)
		{
			GlobalCodeAttribute globalCodeAttribute = EnumExtensions.GetGlobalCodeAttribute(e);
			if (globalCodeAttribute != null)
			{
				return globalCodeAttribute.ExtName;
			}
			return string.Empty;
		}

		public static GlobalCodeAttribute GetGlobalCodeAttribute(Enum e)
		{
			Type type = e.GetType();
			MemberInfo[] member = type.GetMember(e.ToString());
			if (member.Length != 0)
			{
				object[] customAttributes = member[0].GetCustomAttributes(typeof(GlobalCodeAttribute), false);
				if (customAttributes.Length != 0)
				{
					return (GlobalCodeAttribute)customAttributes[0];
				}
			}
			return null;
		}
	}
}
