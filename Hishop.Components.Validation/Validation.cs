using System;
using System.Collections.Generic;

namespace Hishop.Components.Validation
{
	public static class Validation
	{
		public static ValidationResults Validate<T>(T target)
		{
			Validator<T> validator = ValidationFactory.CreateValidator<T>();
			return validator.Validate(target);
		}

		public static ValidationResults Validate<T>(T target, params string[] rulesets)
		{
			if (rulesets == null)
			{
				throw new ArgumentNullException("rulesets");
			}
			ValidationResults validationResults = new ValidationResults();
			foreach (string ruleset in rulesets)
			{
				Validator<T> validator = ValidationFactory.CreateValidator<T>(ruleset);
				foreach (ValidationResult item in (IEnumerable<ValidationResult>)validator.Validate(target))
				{
					validationResults.AddResult(item);
				}
			}
			return validationResults;
		}

		public static ValidationResults ValidateFromAttributes<T>(T target)
		{
			Validator<T> validator = ValidationFactory.CreateValidatorFromAttributes<T>();
			return validator.Validate(target);
		}

		public static ValidationResults ValidateFromAttributes<T>(T target, params string[] rulesets)
		{
			if (rulesets == null)
			{
				throw new ArgumentNullException("rulesets");
			}
			ValidationResults validationResults = new ValidationResults();
			foreach (string ruleset in rulesets)
			{
				Validator<T> validator = ValidationFactory.CreateValidatorFromAttributes<T>(ruleset);
				ValidationResults validationResults2 = validator.Validate(target);
				foreach (ValidationResult item in (IEnumerable<ValidationResult>)validationResults2)
				{
					validationResults.AddResult(item);
				}
			}
			return validationResults;
		}
	}
}
