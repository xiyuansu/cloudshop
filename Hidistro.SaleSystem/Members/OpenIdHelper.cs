using Hidistro.Entities.Members;
using Hidistro.SqlDal.Members;
using Hishop.Plugins;
using System.Collections.Generic;

namespace Hidistro.SaleSystem.Members
{
	public static class OpenIdHelper
	{
		public static void SaveSettings(OpenIdSettingInfo settings)
		{
			new OpenIdSettingDao().SaveSettings(settings);
		}

		public static bool IsExistOpenIdSetting(string openIdType)
		{
			return new OpenIdSettingDao().IsExistOpenIdSetting(openIdType);
		}

		public static void DeleteSettings(string openIdType)
		{
			new OpenIdSettingDao().DeleteOpenIdSetting(openIdType);
		}

		public static OpenIdSettingInfo GetOpenIdSettings(string openIdType)
		{
			return new OpenIdSettingDao().GetOpenIdSetting(openIdType);
		}

		public static PluginItemCollection GetConfigedItems()
		{
			IList<string> configedTypes = new OpenIdSettingDao().GetConfigedTypes();
			if (configedTypes == null || configedTypes.Count == 0)
			{
				return null;
			}
			PluginItemCollection plugins = OpenIdPlugins.Instance().GetPlugins();
			if (plugins != null && plugins.Count > 0)
			{
				PluginItem[] items = plugins.Items;
				PluginItem[] array = items;
				foreach (PluginItem pluginItem in array)
				{
					if (!configedTypes.Contains(pluginItem.FullName.ToLower()))
					{
						plugins.Remove(pluginItem.FullName.ToLower());
					}
				}
			}
			return plugins;
		}

		public static PluginItemCollection GetEmptyItems()
		{
			PluginItemCollection plugins = OpenIdPlugins.Instance().GetPlugins();
			if (plugins == null || plugins.Count == 0)
			{
				return null;
			}
			IList<string> configedTypes = new OpenIdSettingDao().GetConfigedTypes();
			if (configedTypes != null && configedTypes.Count > 0)
			{
				foreach (string item in configedTypes)
				{
					if (plugins.ContainsKey(item.ToLower()))
					{
						plugins.Remove(item.ToLower());
					}
				}
			}
			return plugins;
		}
	}
}
