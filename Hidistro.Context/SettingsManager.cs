using Hidistro.Core;
using Hidistro.Core.Attributes;
using Hidistro.SqlDal.Store;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Hidistro.Context
{
	public static class SettingsManager
	{
		public static void Save(SiteSettings settings)
		{
			SettingsManager.SaveMasterSettings(settings);
			HiCache.Remove("FileCache-MasterSettings");
		}

		public static SiteSettings GetMasterSettings()
		{
			SiteSettings siteSettings = HiCache.Get<SiteSettings>("FileCache-MasterSettings");
			if (siteSettings == null)
			{
				siteSettings = SettingsManager.DecrySettings(DataHelper.ConvertDictionaryToEntity<SiteSettings>(new SiteSettingsDao().GetSiteSettings()));
				HiCache.Insert("FileCache-MasterSettings", siteSettings, 1800);
			}
			return siteSettings;
		}

		public static IDictionary<string, object> GetEncrySettings()
		{
			return new SiteSettingsDao().GetSiteSettings();
		}

		public static SiteSettings EncrySettings(SiteSettings settings)
		{
			PropertyInfo[] properties = settings.GetType().GetProperties();
			Type type = settings.GetType();
			PropertyInfo[] array = properties;
			foreach (PropertyInfo propertyInfo in array)
			{
				MethodInfo getMethod = propertyInfo.GetGetMethod();
				if (getMethod != (MethodInfo)null && getMethod.IsPublic && propertyInfo.CanWrite)
				{
					object[] customAttributes = propertyInfo.GetCustomAttributes(false);
					if (customAttributes != null && customAttributes.Length != 0)
					{
						for (int j = 0; j < customAttributes.Length; j++)
						{
							if (customAttributes[j].GetType() == typeof(GlobalCodeAttribute))
							{
								GlobalCodeAttribute globalCodeAttribute = (GlobalCodeAttribute)customAttributes[j];
								if (globalCodeAttribute != null && propertyInfo.PropertyType == typeof(string))
								{
									if (globalCodeAttribute.IsEncryption)
									{
										propertyInfo.SetValue(settings, HiCryptographer.Encrypt(getMethod.Invoke(settings, new object[0]).ToNullString()), null);
									}
									else if (globalCodeAttribute.IsHtmlCode)
									{
										propertyInfo.SetValue(settings, Globals.HtmlEncode(getMethod.Invoke(settings, new object[0]).ToNullString()), null);
									}
									else if (globalCodeAttribute.IsUrlEncode)
									{
										propertyInfo.SetValue(settings, Globals.UrlEncode(getMethod.Invoke(settings, new object[0]).ToNullString()), null);
									}
								}
							}
						}
					}
					else
					{
						propertyInfo.SetValue(settings, getMethod.Invoke(settings, new object[0]), null);
					}
				}
			}
			return settings;
		}

		public static SiteSettings DecrySettings(SiteSettings settings)
		{
			PropertyInfo[] properties = settings.GetType().GetProperties();
			Type type = settings.GetType();
			PropertyInfo[] array = properties;
			foreach (PropertyInfo propertyInfo in array)
			{
				MethodInfo getMethod = propertyInfo.GetGetMethod();
				if (getMethod != (MethodInfo)null && getMethod.IsPublic && propertyInfo.CanWrite)
				{
					object[] customAttributes = propertyInfo.GetCustomAttributes(false);
					if (customAttributes != null && customAttributes.Length != 0)
					{
						for (int j = 0; j < customAttributes.Length; j++)
						{
							if (customAttributes[j].GetType() == typeof(GlobalCodeAttribute))
							{
								GlobalCodeAttribute globalCodeAttribute = (GlobalCodeAttribute)customAttributes[j];
								if (globalCodeAttribute != null && propertyInfo.PropertyType == typeof(string))
								{
									if (globalCodeAttribute.IsEncryption)
									{
										propertyInfo.SetValue(settings, HiCryptographer.TryDecypt(getMethod.Invoke(settings, new object[0]).ToNullString()), null);
									}
									else if (globalCodeAttribute.IsHtmlCode)
									{
										propertyInfo.SetValue(settings, Globals.HtmlDecode(getMethod.Invoke(settings, new object[0]).ToNullString()), null);
									}
									else if (globalCodeAttribute.IsUrlEncode)
									{
										propertyInfo.SetValue(settings, Globals.UrlDecode(getMethod.Invoke(settings, new object[0]).ToNullString()), null);
									}
								}
							}
						}
					}
					else
					{
						propertyInfo.SetValue(settings, getMethod.Invoke(settings, new object[0]), null);
					}
				}
			}
			return settings;
		}

		private static void SaveMasterSettings(SiteSettings settings)
		{
			settings = SettingsManager.EncrySettings(settings);
			IDictionary<string, object> items = DataHelper.ConvertEntityToObjDictionary(settings);
			IDictionary<string, object> encrySettings = SettingsManager.GetEncrySettings();
			new SiteSettingsDao().SaveSiteSettings(items, encrySettings);
		}
	}
}
