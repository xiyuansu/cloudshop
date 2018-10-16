using Hishop.Components.Validation.Properties;
using Hishop.Components.Validation.Validators;
using System;
using System.Globalization;
using System.Reflection;

namespace Hishop.Components.Validation
{
	internal static class ValidationReflectionHelper
	{
		public static PropertyInfo GetProperty(Type type, string propertyName, bool throwIfInvalid)
		{
			if (string.IsNullOrEmpty(propertyName))
			{
				throw new ArgumentNullException("propertyName");
			}
			PropertyInfo property = type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public);
			if (!ValidationReflectionHelper.IsValidProperty(property))
			{
				if (throwIfInvalid)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.ExceptionInvalidProperty, propertyName, type.FullName));
				}
				return null;
			}
			return property;
		}

		public static bool IsValidProperty(PropertyInfo propertyInfo)
		{
			if (propertyInfo != null && propertyInfo.CanRead)
			{
				return propertyInfo.GetIndexParameters().Length == 0;
			}
			return false;
		}

		public static FieldInfo GetField(Type type, string fieldName, bool throwIfInvalid)
		{
			if (string.IsNullOrEmpty(fieldName))
			{
				throw new ArgumentNullException("fieldName");
			}
			FieldInfo field = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public);
			if (!ValidationReflectionHelper.IsValidField(field))
			{
				if (throwIfInvalid)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.ExceptionInvalidField, fieldName, type.FullName));
				}
				return null;
			}
			return field;
		}

		public static bool IsValidField(FieldInfo fieldInfo)
		{
			return null != fieldInfo;
		}

		public static MethodInfo GetMethod(Type type, string methodName, bool throwIfInvalid)
		{
			if (string.IsNullOrEmpty(methodName))
			{
				throw new ArgumentNullException("methodName");
			}
			MethodInfo method = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public, null, Type.EmptyTypes, null);
			if (!ValidationReflectionHelper.IsValidMethod(method))
			{
				if (throwIfInvalid)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.ExceptionInvalidMethod, methodName, type.FullName));
				}
				return null;
			}
			return method;
		}

		public static bool IsValidMethod(MethodInfo methodInfo)
		{
			if (methodInfo != null && typeof(void) != methodInfo.ReturnType)
			{
				return methodInfo.GetParameters().Length == 0;
			}
			return false;
		}

		public static T ExtractValidationAttribute<T>(ICustomAttributeProvider attributeProvider, string ruleset) where T : BaseValidationAttribute
		{
			if (attributeProvider != null)
			{
				object[] customAttributes = attributeProvider.GetCustomAttributes(typeof(T), false);
				for (int i = 0; i < customAttributes.Length; i++)
				{
					T result = (T)customAttributes[i];
					if (ruleset.Equals(result.Ruleset))
					{
						return result;
					}
				}
			}
			return null;
		}
	}
}
