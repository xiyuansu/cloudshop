using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace HiShop.API.HiPOS
{
	public class CheckSignature
	{
		public const string Token = "weixin";

		public static bool Check(string signature, string timestamp, string nonce, string token = null)
		{
			return signature == CheckSignature.GetSignature(timestamp, nonce, token);
		}

		public static string GetSignature(string timestamp, string nonce, string token = null)
		{
			token = (token ?? "weixin");
			string[] value = (from z in new string[3]
			{
				token,
				timestamp,
				nonce
			}
			orderby z
			select z).ToArray();
			string s = string.Join("", value);
			SHA1 sHA = SHA1.Create();
			byte[] array = sHA.ComputeHash(Encoding.UTF8.GetBytes(s));
			StringBuilder stringBuilder = new StringBuilder();
			byte[] array2 = array;
			foreach (byte b in array2)
			{
				stringBuilder.AppendFormat("{0:x2}", b);
			}
			return stringBuilder.ToString();
		}
	}
}
