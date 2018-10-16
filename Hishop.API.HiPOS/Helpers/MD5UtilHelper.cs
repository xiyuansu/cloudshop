using System;
using System.Security.Cryptography;
using System.Text;

namespace HiShop.API.HiPOS.Helpers
{
	public class MD5UtilHelper
	{
		public static string GetMD5(string encypStr, string charset)
		{
			MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
			byte[] bytes;
			try
			{
				bytes = Encoding.GetEncoding(charset).GetBytes(encypStr);
			}
			catch (Exception)
			{
				bytes = Encoding.GetEncoding("GB2312").GetBytes(encypStr);
			}
			byte[] value = mD5CryptoServiceProvider.ComputeHash(bytes);
			string text = BitConverter.ToString(value);
			return text.Replace("-", "").ToUpper();
		}
	}
}
