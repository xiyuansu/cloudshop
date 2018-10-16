using System;
using System.Collections;
using System.Collections.Generic;

namespace Hishop.Components.Validation
{
	[Serializable]
	public class ValidationResults : IEnumerable<ValidationResult>, IEnumerable
	{
		private List<ValidationResult> validationResults;

		public bool IsValid
		{
			get
			{
				return this.validationResults.Count == 0;
			}
		}

		public int Count
		{
			get
			{
				return this.validationResults.Count;
			}
		}

		public ValidationResults()
		{
			this.validationResults = new List<ValidationResult>();
		}

		public void AddResult(ValidationResult validationResult)
		{
			this.validationResults.Add(validationResult);
		}

		public void AddAllResults(IEnumerable<ValidationResult> sourceValidationResults)
		{
			this.validationResults.AddRange(sourceValidationResults);
		}

		public ValidationResults FindAll(TagFilter tagFilter, params string[] tags)
		{
			if (tags == null)
			{
				string[] array = new string[1];
				tags = array;
			}
			ValidationResults validationResults = new ValidationResults();
			foreach (ValidationResult item in (IEnumerable<ValidationResult>)this)
			{
				bool flag = false;
				string[] array2 = tags;
				foreach (string text in array2)
				{
					if (text == null && item.Tag == null)
					{
						goto IL_0053;
					}
					if (text != null && text.Equals(item.Tag))
					{
						goto IL_0053;
					}
					continue;
					IL_0053:
					flag = true;
					break;
				}
				if (flag ^ tagFilter == TagFilter.Ignore)
				{
					validationResults.AddResult(item);
				}
			}
			return validationResults;
		}

		IEnumerator<ValidationResult> IEnumerable<ValidationResult>.GetEnumerator()
		{
			return (IEnumerator<ValidationResult>)(object)this.validationResults.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return (IEnumerator)(object)this.validationResults.GetEnumerator();
		}
	}
}
