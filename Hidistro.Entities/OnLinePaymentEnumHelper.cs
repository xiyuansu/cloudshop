using System;
using System.ComponentModel;
using System.Reflection;

namespace Hidistro.Entities
{
	public class OnLinePaymentEnumHelper
	{
		public static string GetOnLinePaymentDescription(object v)
		{
			return OnLinePaymentEnumHelper.GetDescription(typeof(OnLinePayment), v);
		}

		private static string GetDescription(Type t, object v)
		{
			try
			{
				FieldInfo field = t.GetField(OnLinePaymentEnumHelper.GetName(t, v));
				DescriptionAttribute[] array = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);
				return (array.Length != 0) ? array[0].Description : OnLinePaymentEnumHelper.GetName(t, v);
			}
			catch
			{
				return "未知";
			}
		}

		private static string GetName(Type t, object v)
		{
			try
			{
				return Enum.GetName(t, Convert.ToInt32(v));
			}
			catch
			{
				return "未知";
			}
		}
	}
}
