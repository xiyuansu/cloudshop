using Hishop.Components.Validation.Properties;
using System;

namespace Hishop.Components.Validation.Validators
{
	public class ValueAccessValidator : Validator
	{
		private ValueAccess valueAccess;

		private Validator valueValidator;

		protected override string DefaultMessageTemplate
		{
			get
			{
				return Resources.ValueValidatorDefaultMessageTemplate;
			}
		}

		public ValueAccessValidator(ValueAccess valueAccess, Validator valueValidator)
			: base(null, null)
		{
			if (valueAccess == null)
			{
				throw new ArgumentNullException("valueAccess");
			}
			if (valueValidator == null)
			{
				throw new ArgumentNullException("valueValidator");
			}
			this.valueAccess = valueAccess;
			this.valueValidator = valueValidator;
		}

		protected internal override void DoValidate(object objectToValidate, object currentTarget, string key, ValidationResults validationResults)
		{
			if (objectToValidate != null)
			{
				object objectToValidate2 = default(object);
				string message = default(string);
				if (this.valueAccess.GetValue(objectToValidate, out objectToValidate2, out message))
				{
					this.valueValidator.DoValidate(objectToValidate2, objectToValidate, this.valueAccess.Key, validationResults);
				}
				else
				{
					base.LogValidationResult(validationResults, message, currentTarget, this.valueAccess.Key);
				}
			}
			else
			{
				string messageTemplate = base.MessageTemplate;
				base.LogValidationResult(validationResults, messageTemplate, currentTarget, key);
			}
		}
	}
}
