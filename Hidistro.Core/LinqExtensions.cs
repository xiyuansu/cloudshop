using System;
using System.Collections;
using System.Collections.Generic;

namespace Hidistro.Core
{
	public static class LinqExtensions
	{
		public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
		{
			if (enumeration != null)
			{
				foreach (T item in enumeration)
				{
					action(item);
				}
			}
		}

		public static void ForEach(this IEnumerable enumeration, Action<object> action)
		{
			if (enumeration != null)
			{
				foreach (object item in enumeration)
				{
					action(item);
				}
			}
		}
	}
}
