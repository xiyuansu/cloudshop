using Hishop.Components.Validation.Validators;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Hishop.Components.Validation
{
	internal class MetadataValidatedType : MetadataValidatedElement, IValidatedType, IValidatedElement
	{
		public MetadataValidatedType(Type targetType, string ruleset)
			: base(targetType, ruleset)
		{
		}

		IEnumerable<IValidatedElement> IValidatedType.GetValidatedProperties()
		{
			MetadataValidatedElement flyweight = new MetadataValidatedElement(base.Ruleset);
			try
			{
				PropertyInfo[] properties = base.TargetType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
				foreach (PropertyInfo propertyInfo in properties)
				{
					if (ValidationReflectionHelper.IsValidProperty(propertyInfo))
					{
						flyweight.UpdateFlyweight(propertyInfo);
						yield return (IValidatedElement)flyweight;
					}
				}
			}
			finally
			{
			}
		}

		IEnumerable<IValidatedElement> IValidatedType.GetValidatedFields()
		{
			MetadataValidatedElement flyweight = new MetadataValidatedElement(base.Ruleset);
			try
			{
				FieldInfo[] fields = base.TargetType.GetFields(BindingFlags.Instance | BindingFlags.Public);
				foreach (FieldInfo fieldInfo in fields)
				{
					flyweight.UpdateFlyweight(fieldInfo);
					yield return (IValidatedElement)flyweight;
				}
			}
			finally
			{
			}
		}

		IEnumerable<IValidatedElement> IValidatedType.GetValidatedMethods()
		{
			MetadataValidatedElement flyweight = new MetadataValidatedElement(base.Ruleset);
			try
			{
				MethodInfo[] methods = base.TargetType.GetMethods(BindingFlags.Instance | BindingFlags.Public);
				foreach (MethodInfo methodInfo in methods)
				{
					methodInfo.GetParameters();
					if (ValidationReflectionHelper.IsValidMethod(methodInfo))
					{
						flyweight.UpdateFlyweight(methodInfo);
						yield return (IValidatedElement)flyweight;
					}
				}
			}
			finally
			{
			}
		}

		IEnumerable<MethodInfo> IValidatedType.GetSelfValidationMethods()
		{
			Type type = base.TargetType;
			if (type.GetCustomAttributes(typeof(HasSelfValidationAttribute), false).Length != 0)
			{
				try
				{
					MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					foreach (MethodInfo methodInfo in methods)
					{
						bool hasReturnType = methodInfo.ReturnType != typeof(void);
						ParameterInfo[] parameters = methodInfo.GetParameters();
						if (!hasReturnType && parameters.Length == 1 && parameters[0].ParameterType == typeof(ValidationResults))
						{
							try
							{
								object[] customAttributes = methodInfo.GetCustomAttributes(typeof(SelfValidationAttribute), false);
								for (int j = 0; j < customAttributes.Length; j++)
								{
									SelfValidationAttribute attribute = (SelfValidationAttribute)customAttributes[j];
									if (base.Ruleset.Equals(attribute.Ruleset))
									{
										yield return methodInfo;
									}
								}
							}
							finally
							{
							}
						}
					}
				}
				finally
				{
				}
			}
		}
	}
}
