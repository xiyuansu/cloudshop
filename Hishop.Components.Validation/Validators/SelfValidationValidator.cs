using Hishop.Components.Validation.Properties;
using System;
using System.Reflection;

namespace Hishop.Components.Validation.Validators
{
	public class SelfValidationValidator : Validator
	{
		private MethodInfo methodInfo;

		protected override string DefaultMessageTemplate
		{
			get
			{
				return null;
			}
		}

		public SelfValidationValidator(MethodInfo methodInfo)
			: base(null, null)
		{
			if (methodInfo == null)
			{
				throw new ArgumentNullException("methodInfo");
			}
			if (typeof(void) != methodInfo.ReturnType)
			{
				throw new ArgumentException(Resources.ExceptionSelfValidationMethodWithInvalidSignature, "methodInfo");
			}
			ParameterInfo[] parameters = methodInfo.GetParameters();
			if (1 == parameters.Length && typeof(ValidationResults) == parameters[0].ParameterType)
			{
				this.methodInfo = methodInfo;
				return;
			}
			throw new ArgumentException(Resources.ExceptionSelfValidationMethodWithInvalidSignature, "methodInfo");
		}

		protected internal override void DoValidate(object objectToValidate, object currentTarget, string key, ValidationResults validationResults)
		{
			if (objectToValidate == null)
			{
				base.LogValidationResult(validationResults, Resources.SelfValidationValidatorMessage, currentTarget, key);
			}
			else if (!this.methodInfo.DeclaringType.IsAssignableFrom(objectToValidate.GetType()))
			{
				base.LogValidationResult(validationResults, Resources.SelfValidationValidatorMessage, currentTarget, key);
			}
			else
			{
				try
				{
					this.methodInfo.Invoke(objectToValidate, new object[1]
					{
						validationResults
					});
				}
				catch (Exception)
				{
					base.LogValidationResult(validationResults, Resources.SelfValidationMethodThrownMessage, currentTarget, key);
				}
			}
		}
	}
}
