using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace HiShop.API.Setting.XmlUtility
{
	public static class XmlUtility
	{
		public static object Deserialize<T>(string xml)
		{
			try
			{
				using (StringReader textReader = new StringReader(xml))
				{
					XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
					return xmlSerializer.Deserialize(textReader);
				}
			}
			catch (Exception)
			{
				return null;
			}
		}

		public static object Deserialize<T>(Stream stream)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
			return xmlSerializer.Deserialize(stream);
		}

		public static string Serializer<T>(T obj)
		{
			MemoryStream memoryStream = new MemoryStream();
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
			try
			{
				xmlSerializer.Serialize(memoryStream, obj);
			}
			catch (InvalidOperationException)
			{
				throw;
			}
			memoryStream.Position = 0L;
			StreamReader streamReader = new StreamReader(memoryStream);
			string result = streamReader.ReadToEnd();
			streamReader.Dispose();
			memoryStream.Dispose();
			return result;
		}

		public static XDocument Convert(Stream stream)
		{
			stream.Seek(0L, SeekOrigin.Begin);
			using (XmlReader reader = XmlReader.Create(stream))
			{
				return XDocument.Load(reader);
			}
		}
	}
}
