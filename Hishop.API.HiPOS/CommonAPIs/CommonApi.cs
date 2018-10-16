using HiShop.API.HiPOS.Entities;
using HiShop.API.Setting.HttpUtility;
using System.Collections.Generic;

namespace Hishop.API.HiPOS.CommonAPIs
{
	public static class CommonApi
	{
		public static AccessTokenResult GetToken(string appid, string appSecret, string grant_type = "client_credentials")
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("grant_type", grant_type);
			return Post.PostGetJson<AccessTokenResult>(HiPOSParameter.GETTOKEN, null, dictionary, null, appid, appSecret, 10000);
		}
	}
}
