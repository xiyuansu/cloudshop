using System;
using System.Web.Security;

namespace Hishop.Weixin.MP.Util
{
	public class CheckSignature
	{
		public static readonly string Token = "weixin_test";

		public static bool Check(string signature, string timestamp, string nonce, string token)
		{
			token = (token ?? CheckSignature.Token);
			string[] array = new string[3]
			{
				timestamp,
				nonce,
				token
			};
			Array.Sort(array);
			string password = string.Join("", array);
			password = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "SHA1");
			return signature == password.ToLower();
		}
	}
}
