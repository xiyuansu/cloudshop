using System;

namespace Hishop.Plugins
{
	public abstract class IPlibaryRequest : IPlugin
	{
		public static IPlibaryRequest CreateInstance(string name, string IPAddress, string DataUrl)
		{
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}
			object[] args = new object[2]
			{
				IPAddress,
				DataUrl
			};
			Type plugin = IPlibaryPlugins.Instance().GetPlugin("IPlibaryRequest", name);
			if (plugin == null)
			{
				return null;
			}
			return Activator.CreateInstance(plugin, args) as IPlibaryRequest;
		}

		public static IPlibaryRequest CreateInstance(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}
			Type plugin = IPlibaryPlugins.Instance().GetPlugin("IPlibaryRequest", name);
			if (plugin == null)
			{
				return null;
			}
			return Activator.CreateInstance(plugin) as IPlibaryRequest;
		}

		public abstract IPData IPLocation();
	}
}
