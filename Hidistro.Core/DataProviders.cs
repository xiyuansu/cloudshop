using Hidistro.Core.Configuration;
using System;

namespace Hidistro.Core
{
	public sealed class DataProviders
	{
		private DataProviders()
		{
		}

		public static object CreateInstance(Provider dataProvider)
		{
			if (dataProvider == null)
			{
				return null;
			}
			Type type = Type.GetType(dataProvider.Type);
			object result = null;
			if (type != (Type)null)
			{
				result = Activator.CreateInstance(type);
			}
			return result;
		}

		public static object CreateInstance(string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr))
			{
				return null;
			}
			try
			{
				return Activator.CreateInstance(Type.GetType(typeStr));
			}
			catch
			{
				return null;
			}
		}
	}
}
