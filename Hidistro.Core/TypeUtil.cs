using System;
using System.ComponentModel;

namespace Hidistro.Core
{
	public class TypeUtil
	{
		public static Type GetUnNullableType(Type conversionType)
		{
			if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
			{
				NullableConverter nullableConverter = new NullableConverter(conversionType);
				conversionType = nullableConverter.UnderlyingType;
			}
			return conversionType;
		}
	}
}
