using Hishop.Weixin.MP.Util;

namespace Hishop.Weixin.MP.Api
{
	public class MenuApi
	{
		public static string DeleteMenus(string accessToken)
		{
			string url = $"https://api.weixin.qq.com/cgi-bin/menu/delete?access_token={accessToken}";
			return new WebUtils().DoGet(url, null);
		}

		public static string CreateMenus(string accessToken, string json)
		{
			string url = $"https://api.weixin.qq.com/cgi-bin/menu/create?access_token={accessToken}";
			return new WebUtils().DoPost(url, json);
		}

		public static string GetMenus(string accessToken)
		{
			string url = $"https://api.weixin.qq.com/cgi-bin/menu/get?access_token={accessToken}";
			return new WebUtils().DoGet(url, null);
		}
	}
}
