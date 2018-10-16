using System;

namespace Hishop.Components.Validation.Validators
{
	internal class RelativeDateTimeGenerator
	{
		internal DateTime GenerateBoundDateTime(int bound, DateTimeUnit unit, DateTime referenceDateTime)
		{
			switch (unit)
			{
			case DateTimeUnit.Day:
				return referenceDateTime.AddDays((double)bound);
			case DateTimeUnit.Hour:
				return referenceDateTime.AddHours((double)bound);
			case DateTimeUnit.Minute:
				return referenceDateTime.AddMinutes((double)bound);
			case DateTimeUnit.Month:
				return referenceDateTime.AddMonths(bound);
			case DateTimeUnit.Second:
				return referenceDateTime.AddSeconds((double)bound);
			case DateTimeUnit.Year:
				return referenceDateTime.AddYears(bound);
			default:
				return referenceDateTime;
			}
		}
	}
}
