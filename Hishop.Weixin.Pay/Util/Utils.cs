using Hishop.Weixin.Pay.Notify;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace Hishop.Weixin.Pay.Util
{
	internal class Utils
	{
		private static readonly DateTime BaseTime = new DateTime(1970, 1, 1);

		private static Dictionary<string, XmlSerializer> parsers = new Dictionary<string, XmlSerializer>();

		public static DateTime ConvertSecondsToDateTime(long seconds)
		{
			DateTime dateTime = Utils.BaseTime;
			dateTime = dateTime.AddSeconds((double)seconds);
			return dateTime.AddHours(8.0);
		}

		public static long GetCurrentTimeSeconds()
		{
			return (long)(DateTime.UtcNow - Utils.BaseTime).TotalSeconds;
		}

		public static long GetTimeSeconds(DateTime dt)
		{
			return (long)(dt.ToUniversalTime() - Utils.BaseTime).TotalSeconds;
		}

		public static string CreateNoncestr()
		{
			return DateTime.Now.ToString("fffffff");
		}

		public static T GetNotifyObject<T>(string xml) where T : NotifyObject
		{
			Type typeFromHandle = typeof(T);
			string fullName = typeFromHandle.FullName;
			XmlSerializer xmlSerializer = null;
			if (!Utils.parsers.TryGetValue(fullName, out xmlSerializer) || xmlSerializer == null)
			{
				XmlAttributes xmlAttributes = new XmlAttributes();
				xmlAttributes.XmlRoot = new XmlRootAttribute("xml");
				XmlAttributeOverrides xmlAttributeOverrides = new XmlAttributeOverrides();
				xmlAttributeOverrides.Add(typeFromHandle, xmlAttributes);
				xmlSerializer = new XmlSerializer(typeFromHandle, xmlAttributeOverrides);
				Utils.parsers[fullName] = xmlSerializer;
			}
			object obj = null;
			try
			{
				using (Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
				{
					obj = xmlSerializer.Deserialize(stream);
				}
			}
			catch (Exception)
			{
				return null;
			}
			return (T)(obj as T);
		}

		public static string GetToken(string appid, string secret)
		{
			string url = $"https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={appid}&secret={secret}";
			string value = new WebUtils().DoGet(url);
			if (string.IsNullOrEmpty(value))
			{
				return string.Empty;
			}
			Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(value);
			if (dictionary != null && dictionary.ContainsKey("access_token"))
			{
				return dictionary["access_token"];
			}
			return string.Empty;
		}

		public static PayDictionary GetPayDictionary(object obj)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			Type type = obj.GetType();
			PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
			PropertyInfo[] array = properties;
			foreach (PropertyInfo propertyInfo in array)
			{
				MethodInfo getMethod = propertyInfo.GetGetMethod();
				if (getMethod != (MethodInfo)null && getMethod.IsPublic && propertyInfo.CanRead)
				{
					dictionary.Add(propertyInfo.Name, getMethod.Invoke(obj, new object[0]));
				}
			}
			PayDictionary payDictionary = new PayDictionary();
			foreach (KeyValuePair<string, object> item in dictionary)
			{
				payDictionary.Add(item.Key, item.Value);
			}
			return payDictionary;
		}
	}
}
