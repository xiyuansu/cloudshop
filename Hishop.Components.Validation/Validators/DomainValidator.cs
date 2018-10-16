using Hishop.Components.Validation.Properties;
using System.Collections.Generic;

namespace Hishop.Components.Validation.Validators
{
	public class DomainValidator<T> : ValueValidator<T>
	{
		private IEnumerable<T> domain;

		protected override string DefaultNonNegatedMessageTemplate
		{
			get
			{
				return Resources.DomainNonNegatedDefaultMessageTemplate;
			}
		}

		protected override string DefaultNegatedMessageTemplate
		{
			get
			{
				return Resources.DomainNegatedDefaultMessageTemplate;
			}
		}

		public DomainValidator(IEnumerable<T> domain)
			: this(domain, false)
		{
		}

		public DomainValidator(bool negated, params T[] domain)
			: this((IEnumerable<T>)new List<T>(domain), (string)null, negated)
		{
		}

		public DomainValidator(string messageTemplate, params T[] domain)
			: this((IEnumerable<T>)new List<T>(domain), messageTemplate, false)
		{
		}

		public DomainValidator(IEnumerable<T> domain, bool negated)
			: this(domain, (string)null, negated)
		{
		}

		public DomainValidator(IEnumerable<T> domain, string messageTemplate, bool negated)
			: base(messageTemplate, (string)null, negated)
		{
			ValidatorArgumentsValidatorHelper.ValidateDomainValidator(domain);
			this.domain = domain;
		}

		protected override void DoValidate(T objectToValidate, object currentTarget, string key, ValidationResults validationResults)
		{
			bool flag = false;
			bool flag2 = objectToValidate == null;
			if (!flag2)
			{
				flag = true;
				foreach (T item in this.domain)
				{
					if (!item.Equals(objectToValidate))
					{
						continue;
					}
					flag = false;
					break;
				}
			}
			if (!flag2 && flag == base.Negated)
			{
				return;
			}
			base.LogValidationResult(validationResults, this.GetMessage(objectToValidate, key), currentTarget, key);
		}
	}
}
