using Hishop.API.HiPOS;
using HiShop.API.HiPOS.AdvancedAPIs.Auth.AuthJson;
using HiShop.API.Setting.HttpUtility;
using System.Collections.Generic;
using System.Text;

namespace HiShop.API.HiPOS.AdvancedAPIs.Auth
{
	public static class AuthApi
	{
		public static AuthResult GetAuth(string hostname, string notify_url)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("hostname", hostname);
			dictionary.Add("notify_url", notify_url);
			return Post.PostGetJson<AuthResult>(HiPOSParameter.GETAUTH, null, dictionary, Encoding.UTF8, "", "", 10000);
		}
	}
}
