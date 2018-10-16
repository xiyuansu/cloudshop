using System;
using System.IO;

namespace HiShop.API.Setting.StreamUtility
{
	public static class StreamUtility
	{
		public static string GetBase64String(Stream stream)
		{
			byte[] array = new byte[stream.Length];
			stream.Position = 0L;
			stream.Read(array, 0, (int)stream.Length);
			return Convert.ToBase64String(array, Base64FormattingOptions.None);
		}

		public static Stream GetStreamFromBase64String(string base64String, string savePath)
		{
			byte[] array = Convert.FromBase64String(base64String);
			MemoryStream memoryStream = new MemoryStream(array, 0, array.Length);
			memoryStream.Write(array, 0, array.Length);
			if (!string.IsNullOrEmpty(savePath))
			{
				StreamUtility.SaveFileFromStream(memoryStream, savePath);
			}
			memoryStream.Seek(0L, SeekOrigin.Begin);
			return memoryStream;
		}

		public static void SaveFileFromStream(MemoryStream memoryStream, string savePath)
		{
			memoryStream.Seek(0L, SeekOrigin.Begin);
			using (FileStream fileStream = new FileStream(savePath, FileMode.OpenOrCreate))
			{
				fileStream.Write(memoryStream.ToArray(), 0, (int)memoryStream.Length);
			}
		}
	}
}
