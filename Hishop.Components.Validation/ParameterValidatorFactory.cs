using System.Reflection;

namespace Hishop.Components.Validation
{
	public static class ParameterValidatorFactory
	{
		public static Validator CreateValidator(ParameterInfo paramInfo)
		{
			MetadataValidatedParameterElement metadataValidatedParameterElement = new MetadataValidatedParameterElement();
			metadataValidatedParameterElement.UpdateFlyweight(paramInfo);
			CompositeValidatorBuilder compositeValidatorBuilder = new CompositeValidatorBuilder(metadataValidatedParameterElement);
			foreach (IValidatorDescriptor validatorDescriptor in metadataValidatedParameterElement.GetValidatorDescriptors())
			{
				compositeValidatorBuilder.AddValueValidator(validatorDescriptor.CreateValidator(paramInfo.ParameterType, null, null));
			}
			return compositeValidatorBuilder.GetValidator();
		}
	}
}
