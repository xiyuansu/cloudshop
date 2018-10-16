using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Hishop.Alipay.OpenHome.Utility
{
	public class XmlSerialiseHelper
	{
		public static string Serialise<T>(T t)
		{
			MemoryStream memoryStream = new MemoryStream();
			StreamWriter textWriter = new StreamWriter(memoryStream, Encoding.GetEncoding("GBK"));
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
			XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
			xmlSerializerNamespaces.Add(string.Empty, string.Empty);
			xmlSerializer.Serialize(textWriter, t, xmlSerializerNamespaces);
			string text = Encoding.GetEncoding("GBK").GetString(memoryStream.ToArray()).Replace("\r", "")
				.Replace("\n", "");
			while (text.Contains(" <"))
			{
				text = text.Replace(" <", "<");
			}
			return text.Substring(text.IndexOf("?>") + 2);
		}

		public static T Deserialize<T>(string xml)
		{
			StringReader textReader = new StringReader(xml);
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
			return (T)xmlSerializer.Deserialize(textReader);
		}
	}
}
