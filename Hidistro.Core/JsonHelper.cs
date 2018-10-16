using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Hidistro.Core
{
	public class JsonHelper
	{
		public static string GetJson<T>(T obj)
		{
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(T));
			using (MemoryStream memoryStream = new MemoryStream())
			{
				dataContractJsonSerializer.WriteObject(memoryStream, obj);
				return Encoding.UTF8.GetString(memoryStream.ToArray());
			}
		}

		public static T ParseFormJson<T>(string szJson)
		{
			T val = Activator.CreateInstance<T>();
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(szJson)))
			{
				DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(T));
				return (T)dataContractJsonSerializer.ReadObject(stream);
			}
		}
	}
}
