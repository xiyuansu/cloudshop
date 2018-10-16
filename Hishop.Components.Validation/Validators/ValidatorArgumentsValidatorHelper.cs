using Hishop.Components.Validation.Properties;
using System;

namespace Hishop.Components.Validation.Validators
{
	internal static class ValidatorArgumentsValidatorHelper
	{
		internal static void ValidateContainsCharacterValidator(string characterSet)
		{
			if (characterSet != null)
			{
				return;
			}
			throw new ArgumentNullException("characterSet");
		}

		internal static void ValidateDomainValidator(object domain)
		{
			if (domain != null)
			{
				return;
			}
			throw new ArgumentNullException("domain");
		}

		internal static void ValidateEnumConversionValidator(Type enumType)
		{
			if (enumType != null)
			{
				return;
			}
			throw new ArgumentNullException("enumType");
		}

		internal static void ValidateRangeValidator(IComparable lowerBound, RangeBoundaryType lowerBoundaryType, IComparable upperBound, RangeBoundaryType upperBoundaryType)
		{
			if (lowerBoundaryType != 0 && lowerBound == null)
			{
				throw new ArgumentNullException("lowerBound");
			}
			if (upperBoundaryType != 0 && upperBound == null)
			{
				throw new ArgumentNullException("upperBound");
			}
			if (lowerBoundaryType == RangeBoundaryType.Ignore && upperBoundaryType == RangeBoundaryType.Ignore)
			{
				throw new ArgumentException(Resources.ExceptionCannotIgnoreBothBoundariesInRange, "lowerBound");
			}
			if (lowerBound == null)
			{
				return;
			}
			if (upperBound == null)
			{
				return;
			}
			if (lowerBound.GetType() == upperBound.GetType())
			{
				return;
			}
			throw new ArgumentException(Resources.ExceptionTypeOfBoundsMustMatch, "upperBound");
		}

		internal static void ValidateRegexValidator(string pattern, string patternResourceName, Type patternResourceType)
		{
			if (pattern == null && (patternResourceName == null || patternResourceType == null))
			{
				throw new ArgumentNullException("pattern");
			}
			if (pattern == null && patternResourceName == null)
			{
				throw new ArgumentNullException("patternResourceName");
			}
			if (pattern != null)
			{
				return;
			}
			if (patternResourceType != null)
			{
				return;
			}
			throw new ArgumentNullException("patternResourceType");
		}

		internal static void ValidateRelativeDatimeValidator(int lowerBound, DateTimeUnit lowerUnit, RangeBoundaryType lowerBoundType, int upperBound, DateTimeUnit upperUnit, RangeBoundaryType upperBoundType)
		{
			if (lowerBound == 0 || lowerUnit != 0 || lowerBoundType == RangeBoundaryType.Ignore)
			{
				if (upperBound == 0)
				{
					return;
				}
				if (upperUnit != 0)
				{
					return;
				}
				if (upperBoundType == RangeBoundaryType.Ignore)
				{
					return;
				}
			}
			throw new ArgumentException(Resources.RelativeDateTimeValidatorNotValidDateTimeUnit);
		}

		internal static void ValidateTypeConversionValidator(Type targetType)
		{
			if (targetType != null)
			{
				return;
			}
			throw new ArgumentNullException("targetType");
		}
	}
}
