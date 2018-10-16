using System.IO;

namespace Hishop.Alipay.OpenHome.Utility
{
	internal class RsaFileHelper
	{
		public static string GetRSAKeyContent(string path, bool isPubKey)
		{
			string text = string.Empty;
			string arg = isPubKey ? "PUBLIC KEY" : "RSA PRIVATE KEY";
			using (StreamReader streamReader = new StreamReader(path))
			{
				text = streamReader.ReadToEnd();
				streamReader.Close();
			}
			string text2 = $"-----BEGIN {arg}-----\\n";
			string value = $"-----END {arg}-----";
			int num = text.IndexOf(text2) + text2.Length;
			int num2 = text.IndexOf(value, num);
			text = text.Substring(num, num2 - num);
			return text.Replace("\r", "").Replace("\n", "");
		}
	}
}
