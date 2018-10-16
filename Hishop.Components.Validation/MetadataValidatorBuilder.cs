using System;
using System.Reflection;

namespace Hishop.Components.Validation
{
	internal class MetadataValidatorBuilder : ValidatorBuilderBase
	{
		public MetadataValidatorBuilder()
		{
		}

		public MetadataValidatorBuilder(MemberAccessValidatorBuilderFactory memberAccessValidatorFactory)
			: base(memberAccessValidatorFactory)
		{
		}

		public Validator CreateValidator(Type type, string ruleset)
		{
			return base.CreateValidator(new MetadataValidatedType(type, ruleset));
		}

		internal Validator CreateValidatorForType(Type type, string ruleset)
		{
			return base.CreateValidatorForValidatedElement(new MetadataValidatedType(type, ruleset), base.GetCompositeValidatorBuilderForType);
		}

		internal Validator CreateValidatorForProperty(PropertyInfo propertyInfo, string ruleset)
		{
			return base.CreateValidatorForValidatedElement(new MetadataValidatedElement(propertyInfo, ruleset), base.GetCompositeValidatorBuilderForProperty);
		}

		internal Validator CreateValidatorForField(FieldInfo fieldInfo, string ruleset)
		{
			return base.CreateValidatorForValidatedElement(new MetadataValidatedElement(fieldInfo, ruleset), base.GetCompositeValidatorBuilderForField);
		}

		internal Validator CreateValidatorForMethod(MethodInfo methodInfo, string ruleset)
		{
			return base.CreateValidatorForValidatedElement(new MetadataValidatedElement(methodInfo, ruleset), base.GetCompositeValidatorBuilderForMethod);
		}
	}
}
