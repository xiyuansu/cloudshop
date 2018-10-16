using Hishop.Components.Validation.Properties;
using System;
using System.Collections;

namespace Hishop.Components.Validation.Validators
{
	public class ObjectCollectionValidator : Validator
	{
		private Type targetType;

		private string targetRuleset;

		private Validator targetTypeValidator;

		protected override string DefaultMessageTemplate
		{
			get
			{
				return null;
			}
		}

		public ObjectCollectionValidator(Type targetType)
			: this(targetType, string.Empty)
		{
		}

		public ObjectCollectionValidator(Type targetType, string targetRuleset)
			: base(null, null)
		{
			if (targetType == null)
			{
				throw new ArgumentNullException("targetType");
			}
			if (targetRuleset == null)
			{
				throw new ArgumentNullException("targetRuleset");
			}
			this.targetType = targetType;
			this.targetRuleset = targetRuleset;
			this.targetTypeValidator = ValidationFactory.CreateValidator(this.targetType, this.targetRuleset);
		}

		protected internal override void DoValidate(object objectToValidate, object currentTarget, string key, ValidationResults validationResults)
		{
			if (objectToValidate != null)
			{
				IEnumerable enumerable = objectToValidate as IEnumerable;
				if (enumerable != null)
				{
					foreach (object item in enumerable)
					{
						if (item != null)
						{
							if (this.targetType.IsAssignableFrom(item.GetType()))
							{
								this.targetTypeValidator.DoValidate(item, item, null, validationResults);
							}
							else
							{
								base.LogValidationResult(validationResults, Resources.ObjectCollectionValidatorIncompatibleElementInTargetCollection, item, null);
							}
						}
					}
				}
				else
				{
					base.LogValidationResult(validationResults, Resources.ObjectCollectionValidatorTargetNotCollection, currentTarget, key);
				}
			}
		}
	}
}
