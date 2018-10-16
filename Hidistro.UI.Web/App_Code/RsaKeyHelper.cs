using System.Diagnostics;
using System.IO;

namespace Hidistro.UI.Web.App_Code
{
	public class RsaKeyHelper
	{
		public static string CreateRSAKeyFile(string generatorPath, string keyDirectory)
		{
			Process process = new Process();
			ProcessStartInfo processStartInfo = new ProcessStartInfo();
			processStartInfo.FileName = generatorPath;
			processStartInfo.UseShellExecute = false;
			processStartInfo.RedirectStandardInput = true;
			processStartInfo.RedirectStandardOutput = true;
			processStartInfo.Arguments = "\"" + keyDirectory + "\"";
			process.StartInfo = processStartInfo;
			process.Start();
			process.WaitForExit();
			return RsaKeyHelper.GetRSAKeyContent(keyDirectory + "/rsa_public_key.pem", true);
		}

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
