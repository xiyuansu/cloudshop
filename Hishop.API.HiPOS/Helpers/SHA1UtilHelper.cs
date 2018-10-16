using System;
using System.Security.Cryptography;
using System.Text;

namespace HiShop.API.HiPOS.Helpers
{
	public class SHA1UtilHelper
	{
		public static string GetSha1(string str)
		{
			SHA1 sHA = new SHA1CryptoServiceProvider();
			ASCIIEncoding aSCIIEncoding = new ASCIIEncoding();
			byte[] bytes = aSCIIEncoding.GetBytes(str);
			byte[] value = sHA.ComputeHash(bytes);
			return BitConverter.ToString(value).Replace("-", "");
		}
	}
}
