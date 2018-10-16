using Hishop.Components.Validation.Properties;
using System.Collections.Generic;
using System.Globalization;

namespace Hishop.Components.Validation.Validators
{
	public class ContainsCharactersValidator : ValueValidator<string>
	{
		private string characterSet;

		private ContainsCharacters containsCharacters;

		protected override string DefaultNonNegatedMessageTemplate
		{
			get
			{
				return Resources.ContainsCharactersNonNegatedDefaultMessageTemplate;
			}
		}

		protected override string DefaultNegatedMessageTemplate
		{
			get
			{
				return Resources.ContainsCharactersNegatedDefaultMessageTemplate;
			}
		}

		internal string CharacterSet
		{
			get
			{
				return this.characterSet;
			}
		}

		internal ContainsCharacters ContainsCharacters
		{
			get
			{
				return this.containsCharacters;
			}
		}

		public ContainsCharactersValidator(string characterSet)
			: this(characterSet, ContainsCharacters.Any)
		{
		}

		public ContainsCharactersValidator(string characterSet, ContainsCharacters containsCharacters)
			: this(characterSet, containsCharacters, false)
		{
		}

		public ContainsCharactersValidator(string characterSet, ContainsCharacters containsCharacters, string messageTemplate)
			: this(characterSet, containsCharacters, messageTemplate, false)
		{
		}

		public ContainsCharactersValidator(string characterSet, ContainsCharacters containsCharacters, bool negated)
			: this(characterSet, containsCharacters, null, negated)
		{
		}

		public ContainsCharactersValidator(string characterSet, ContainsCharacters containsCharacters, string messageTemplate, bool negated)
			: base(messageTemplate, (string)null, negated)
		{
			ValidatorArgumentsValidatorHelper.ValidateContainsCharacterValidator(characterSet);
			this.characterSet = characterSet;
			this.containsCharacters = containsCharacters;
		}

		protected override void DoValidate(string objectToValidate, object currentTarget, string key, ValidationResults validationResults)
		{
			bool flag = false;
			bool flag2 = objectToValidate == null;
			if (!flag2)
			{
				if (this.ContainsCharacters == ContainsCharacters.Any)
				{
					List<char> list = new List<char>(this.characterSet);
					bool flag3 = false;
					foreach (char item in objectToValidate)
					{
						if (list.Contains(item))
						{
							flag3 = true;
							break;
						}
					}
					flag = !flag3;
				}
				else if (ContainsCharacters.All == this.ContainsCharacters)
				{
					List<char> list2 = new List<char>(objectToValidate);
					bool flag4 = true;
					string text = this.CharacterSet;
					foreach (char item2 in text)
					{
						if (!list2.Contains(item2))
						{
							flag4 = false;
							break;
						}
					}
					flag = !flag4;
				}
			}
			if (!flag2 && flag == base.Negated)
			{
				return;
			}
			base.LogValidationResult(validationResults, this.GetMessage(objectToValidate, key), currentTarget, key);
		}

		protected override string GetMessage(object objectToValidate, string key)
		{
			return string.Format(CultureInfo.CurrentCulture, base.MessageTemplate, objectToValidate, key, base.Tag, this.CharacterSet, this.ContainsCharacters);
		}
	}
}
