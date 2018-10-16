using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Hidistro.Core
{
	public sealed class Serializer
	{
		private static bool CanBinarySerialize;

		private Serializer()
		{
		}

		static Serializer()
		{
			SecurityPermission securityPermission = new SecurityPermission(SecurityPermissionFlag.SerializationFormatter);
			try
			{
				securityPermission.Demand();
				Serializer.CanBinarySerialize = true;
			}
			catch (SecurityException)
			{
				Serializer.CanBinarySerialize = false;
			}
		}

		public static byte[] ConvertToBytes(object objectToConvert)
		{
			byte[] array = null;
			if (Serializer.CanBinarySerialize)
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				using (MemoryStream memoryStream = new MemoryStream())
				{
					binaryFormatter.Serialize(memoryStream, objectToConvert);
					memoryStream.Position = 0L;
					array = new byte[memoryStream.Length];
					memoryStream.Read(array, 0, array.Length);
				}
			}
			return array;
		}

		public static bool SaveAsBinary(object objectToSave, string path)
		{
			if (objectToSave != null && Serializer.CanBinarySerialize)
			{
				byte[] array = Serializer.ConvertToBytes(objectToSave);
				if (array != null)
				{
					using (FileStream output = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
					{
						using (BinaryWriter binaryWriter = new BinaryWriter(output))
						{
							binaryWriter.Write(array);
							return true;
						}
					}
				}
			}
			return false;
		}

		public static string ConvertToString(object objectToConvert)
		{
			string result = null;
			if (objectToConvert != null)
			{
				Type type = objectToConvert.GetType();
				XmlSerializer xmlSerializer = new XmlSerializer(type);
				using (StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture))
				{
					xmlSerializer.Serialize(stringWriter, objectToConvert);
					result = stringWriter.ToString();
				}
			}
			return result;
		}

		public static string ConvertToString(object objectToConvert, params Type[] extra)
		{
			string result = null;
			if (objectToConvert != null)
			{
				Type type = objectToConvert.GetType();
				XmlSerializer xmlSerializer = new XmlSerializer(type, extra);
				using (StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture))
				{
					xmlSerializer.Serialize(stringWriter, objectToConvert);
					result = stringWriter.ToString();
				}
			}
			return result;
		}

		public static void SaveAsXML(object objectToConvert, string path)
		{
			if (objectToConvert != null)
			{
				Type type = objectToConvert.GetType();
				XmlSerializer xmlSerializer = new XmlSerializer(type);
				using (StreamWriter textWriter = new StreamWriter(path))
				{
					xmlSerializer.Serialize(textWriter, objectToConvert);
				}
			}
		}

		public static object ConvertToObject(byte[] byteArray)
		{
			object result = null;
			if (Serializer.CanBinarySerialize && byteArray != null && byteArray.Length != 0)
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				using (MemoryStream memoryStream = new MemoryStream())
				{
					memoryStream.Write(byteArray, 0, byteArray.Length);
					memoryStream.Position = 0L;
					if (byteArray.Length > 4)
					{
						result = binaryFormatter.Deserialize(memoryStream);
					}
				}
			}
			return result;
		}

		public static object ConvertFileToObject(string path, Type objectType)
		{
			object result = null;
			if (path != null && path.Length > 0)
			{
				using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
				{
					XmlSerializer xmlSerializer = new XmlSerializer(objectType);
					result = xmlSerializer.Deserialize(stream);
				}
			}
			return result;
		}

		public static object ConvertToObject(string xml, Type objectType)
		{
			object result = null;
			if (!string.IsNullOrEmpty(xml))
			{
				using (StringReader textReader = new StringReader(xml))
				{
					XmlSerializer xmlSerializer = new XmlSerializer(objectType);
					result = xmlSerializer.Deserialize(textReader);
				}
			}
			return result;
		}

		public static object ConvertToObject(XmlNode node, Type objectType)
		{
			object result = null;
			if (node != null)
			{
				using (StringReader textReader = new StringReader(node.OuterXml))
				{
					XmlSerializer xmlSerializer = new XmlSerializer(objectType);
					result = xmlSerializer.Deserialize(textReader);
				}
			}
			return result;
		}

		public static object LoadBinaryFile(string path)
		{
			if (!File.Exists(path))
			{
				return null;
			}
			byte[] array = default(byte[]);
			using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
			{
				using (BinaryReader binaryReader = new BinaryReader(fileStream))
				{
					array = new byte[fileStream.Length];
					binaryReader.Read(array, 0, (int)fileStream.Length);
				}
			}
			return Serializer.ConvertToObject(array);
		}

		public static NameValueCollection ConvertToNameValueCollection(string keys, string values)
		{
			NameValueCollection nameValueCollection = new NameValueCollection();
			if (keys != null && values != null && keys.Length > 0 && values.Length > 0)
			{
				char[] separator = new char[1]
				{
					':'
				};
				string[] array = keys.Split(separator);
				for (int i = 0; i < array.Length / 4; i++)
				{
					int num = int.Parse(array[i * 4 + 2], CultureInfo.InvariantCulture);
					int num2 = int.Parse(array[i * 4 + 3], CultureInfo.InvariantCulture);
					string name = array[i * 4];
					if (array[i * 4 + 1] == "S" && num >= 0 && num2 > 0 && values.Length >= num + num2)
					{
						nameValueCollection[name] = values.Substring(num, num2);
					}
				}
			}
			return nameValueCollection;
		}

		public static void ConvertFromNameValueCollection(NameValueCollection nvc, ref string keys, ref string values)
		{
			if (nvc != null && nvc.Count != 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				StringBuilder stringBuilder2 = new StringBuilder();
				int num = 0;
				string[] allKeys = nvc.AllKeys;
				foreach (string text in allKeys)
				{
					if (text.IndexOf(':') != -1)
					{
						throw new ArgumentException("ExtendedAttributes Key can not contain the character \":\"");
					}
					string text2 = nvc[text];
					if (!string.IsNullOrEmpty(text2))
					{
						stringBuilder.AppendFormat("{0}:S:{1}:{2}:", text, num, text2.Length);
						stringBuilder2.Append(text2);
						num += text2.Length;
					}
				}
				keys = stringBuilder.ToString();
				values = stringBuilder2.ToString();
			}
		}
	}
}
