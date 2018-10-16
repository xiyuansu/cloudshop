using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Hidistro.Core
{
	public static class ModelCopier
	{
		public static void CopyCollection<T>(IEnumerable<T> from, ICollection<T> to)
		{
			if (from != null && to != null && !to.IsReadOnly)
			{
				to.Clear();
				foreach (T item in from)
				{
					to.Add(item);
				}
			}
		}

		public static void CopyModel(object from, object to, params string[] exceptProperties)
		{
			if (from != null && to != null)
			{
				if (exceptProperties == null)
				{
					exceptProperties = new string[0];
				}
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(from);
				PropertyDescriptorCollection properties2 = TypeDescriptor.GetProperties(to);
				foreach (PropertyDescriptor item in properties)
				{
					if (!exceptProperties.Contains(item.Name))
					{
						PropertyDescriptor propertyDescriptor2 = properties2.Find(item.Name, true);
						if (propertyDescriptor2 != null && !propertyDescriptor2.IsReadOnly)
						{
							bool flag = propertyDescriptor2.PropertyType.IsAssignableFrom(item.PropertyType);
							bool flag2 = !flag && Nullable.GetUnderlyingType(item.PropertyType) == propertyDescriptor2.PropertyType;
							if (flag | flag2)
							{
								object value = item.GetValue(from);
								object value2 = propertyDescriptor2.GetValue(to);
								if ((flag || (value != null & flag2)) && value != null && !value.Equals(value2) && (!(propertyDescriptor2.PropertyType == typeof(DateTime)) || !value.Equals(DateTime.MinValue)))
								{
									propertyDescriptor2.SetValue(to, value);
								}
							}
						}
					}
				}
			}
		}

		public static TEntity CreateInstanceFrom<TEntity>(object from)
		{
			TEntity val = (TEntity)Activator.CreateInstance(typeof(TEntity));
			ModelCopier.CopyModel(from, val);
			return val;
		}
	}
}
