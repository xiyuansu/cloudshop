using Hishop.Weixin.MP.Domain;
using Hishop.Weixin.MP.Util;
using System.Configuration;
using System.Web.Script.Serialization;

namespace Hishop.Weixin.MP.Api
{
	public class TokenApi
	{
		public string AppId
		{
			get
			{
				return ConfigurationManager.AppSettings.Get("AppId");
			}
		}

		public string AppSecret
		{
			get
			{
				return ConfigurationManager.AppSettings.Get("AppSecret");
			}
		}

		public static string GetToken_Message(string appid, string secret)
		{
			string url = $"https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={appid}&secret={secret}";
			string text = new WebUtils().DoGet(url, null);
			if (text.Contains("access_token"))
			{
				text = new JavaScriptSerializer().Deserialize<Token>(text).access_token;
			}
			return text;
		}

		public static string GetToken(string appid, string secret)
		{
			string url = $"https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={appid}&secret={secret}";
			return new WebUtils().DoGet(url, null);
		}
	}
}
