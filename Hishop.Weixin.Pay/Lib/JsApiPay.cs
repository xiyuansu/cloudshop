using Hishop.Weixin.Pay.Domain;
using LitJson;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;

namespace Hishop.Weixin.Pay.Lib
{
	public class JsApiPay
	{
		private string appSecret;

		private string appId;

		private string openId;

		private string access_token;

		private string Key;

		private string PROXY_URL;

		private static IDictionary<string, string> paramDict = new Dictionary<string, string>();

		private static Page page
		{
			get;
			set;
		}

		public static WxPayData unifiedOrderResult
		{
			get;
			set;
		}

		public static IDictionary<string, string> InitParamDict(NameValueCollection nv)
		{
			IDictionary<string, string> dictionary = new Dictionary<string, string>();
			foreach (KeyValuePair<string, string> item in nv)
			{
				dictionary.Add(item.Key, item.Value);
			}
			return dictionary;
		}

		public static void GetCode(Page page, string appId, bool appendLog = false)
		{
			string text = HttpUtility.UrlEncode(page.Request.Url.ToString());
			WxPayData wxPayData = new WxPayData();
			wxPayData.SetValue("appid", appId);
			wxPayData.SetValue("redirect_uri", text);
			wxPayData.SetValue("response_type", "code");
			wxPayData.SetValue("scope", "snsapi_base");
			wxPayData.SetValue("state", "STATE#wechat_redirect");
			string text2 = "https://open.weixin.qq.com/connect/oauth2/authorize?" + wxPayData.ToUrl();
			if (appendLog)
			{
				WxPayLog.AppendLog(JsApiPay.paramDict, text2, text, "跳转去获取code页面", LogType.GetTokenOrOpenID);
			}
			page.Response.Redirect(text2);
		}

		public static NameValueCollection GetOpenidAndAccessToken(Page page, string appId, string appSecret, bool appendLog = false)
		{
			if (!string.IsNullOrEmpty(page.Request.QueryString["code"]))
			{
				string code = page.Request.QueryString["code"];
				return JsApiPay.GetOpenidAndAccessTokenFromCode(code, page, appId, appSecret, false);
			}
			string host = page.Request.Url.Host;
			string path = page.Request.Path;
			string value = HttpUtility.UrlEncode(page.Request.Url.ToString());
			WxPayData wxPayData = new WxPayData();
			wxPayData.SetValue("appid", appId);
			wxPayData.SetValue("redirect_uri", value);
			wxPayData.SetValue("response_type", "code");
			wxPayData.SetValue("scope", "snsapi_base");
			wxPayData.SetValue("state", "STATE#wechat_redirect");
			string text = "https://open.weixin.qq.com/connect/oauth2/authorize?" + wxPayData.ToUrl();
			if (appendLog)
			{
				WxPayLog.AppendLog(JsApiPay.paramDict, text, page.Request.Url.ToString(), "未获取到code,跳转去获取code页面", LogType.GetTokenOrOpenID);
			}
			try
			{
				page.Response.Redirect(text);
			}
			catch (ThreadAbortException)
			{
			}
			return null;
		}

		public static NameValueCollection GetOpenidAndAccessTokenFromCode(string code, Page page, string appId, string appSecret, bool appendLog = false)
		{
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection.Add("OpenID", "");
			nameValueCollection.Add("Access_Token", "");
			string value = "";
			string value2 = "";
			try
			{
				WxPayData wxPayData = new WxPayData();
				wxPayData.SetValue("appid", appId);
				wxPayData.SetValue("secret", appSecret);
				wxPayData.SetValue("code", code);
				wxPayData.SetValue("grant_type", "authorization_code");
				string text = "https://api.weixin.qq.com/sns/oauth2/access_token?" + wxPayData.ToUrl();
				string json = HttpService.Get(text, "");
				JsonData jsonData = JsonMapper.ToObject(json);
				value2 = (string)jsonData["access_token"];
				value = (string)jsonData["openid"];
				nameValueCollection.Set("OpenID", value);
				nameValueCollection.Set("Access_Token", value2);
				if (!JsApiPay.paramDict.ContainsKey("appid"))
				{
					JsApiPay.paramDict.Add("appid", appId);
				}
				if (!JsApiPay.paramDict.ContainsKey("secret"))
				{
					JsApiPay.paramDict.Add("secret", appSecret);
				}
				if (!JsApiPay.paramDict.ContainsKey("code"))
				{
					JsApiPay.paramDict.Add("code", code);
				}
				if (!JsApiPay.paramDict.ContainsKey("grant_type"))
				{
					JsApiPay.paramDict.Add("grant_type", "authorization_code");
				}
				if (!JsApiPay.paramDict.ContainsKey("RequestUrl"))
				{
					JsApiPay.paramDict.Add("RequestUrl", text);
				}
				if (!JsApiPay.paramDict.ContainsKey("OpenID"))
				{
					JsApiPay.paramDict.Add("OpenID", value);
				}
				if (!JsApiPay.paramDict.ContainsKey("Access_Token"))
				{
					JsApiPay.paramDict.Add("Access_Token", value2);
				}
				if (appendLog)
				{
					WxPayLog.AppendLog(JsApiPay.paramDict, "", page.Request.Url.ToString(), "根据code获取OpenId和AccessToken", LogType.GetTokenOrOpenID);
				}
				return nameValueCollection;
			}
			catch (Exception ex)
			{
				if (!JsApiPay.paramDict.ContainsKey("appid"))
				{
					JsApiPay.paramDict.Add("appid", appId);
				}
				if (!JsApiPay.paramDict.ContainsKey("secret"))
				{
					JsApiPay.paramDict.Add("secret", appSecret);
				}
				if (!JsApiPay.paramDict.ContainsKey("code"))
				{
					JsApiPay.paramDict.Add("code", code);
				}
				if (!JsApiPay.paramDict.ContainsKey("grant_type"))
				{
					JsApiPay.paramDict.Add("grant_type", "authorization_code");
				}
				if (!JsApiPay.paramDict.ContainsKey("OpenID"))
				{
					JsApiPay.paramDict.Add("OpenID", value);
				}
				if (!JsApiPay.paramDict.ContainsKey("Access_Token"))
				{
					JsApiPay.paramDict.Add("Access_Token", value2);
				}
				WxPayLog.AppendLog(JsApiPay.paramDict, "", page.Request.Url.ToString(), "根据code获取OpenId和AccessToken" + ex.Message, LogType.GetTokenOrOpenID);
				throw new WxPayException(ex.ToString());
			}
		}

		public static string GetJsApiParameters(PayConfig config)
		{
			WxPayData wxPayData = new WxPayData();
			wxPayData.SetValue("appId", JsApiPay.unifiedOrderResult.GetValue("appid"));
			wxPayData.SetValue("timeStamp", WxPayApi.GenerateTimeStamp());
			wxPayData.SetValue("nonceStr", WxPayApi.GenerateNonceStr());
			wxPayData.SetValue("package", "prepay_id=" + JsApiPay.unifiedOrderResult.GetValue("prepay_id"));
			wxPayData.SetValue("signType", "MD5");
			wxPayData.SetValue("paySign", wxPayData.MakeSign(config.Key));
			return wxPayData.ToJson();
		}

		public static string GetEditAddressParameters(string appId, string access_token, bool appendLog = false)
		{
			string text = "";
			try
			{
				string host = JsApiPay.page.Request.Url.Host;
				string path = JsApiPay.page.Request.Path;
				string query = JsApiPay.page.Request.Url.Query;
				string value = "http://" + host + path + query;
				WxPayData wxPayData = new WxPayData();
				wxPayData.SetValue("appid", appId);
				wxPayData.SetValue("url", value);
				wxPayData.SetValue("timestamp", WxPayApi.GenerateTimeStamp());
				wxPayData.SetValue("noncestr", WxPayApi.GenerateNonceStr());
				wxPayData.SetValue("accesstoken", access_token);
				string password = wxPayData.ToUrl();
				string text2 = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "SHA1");
				WxPayData wxPayData2 = new WxPayData();
				wxPayData2.SetValue("appId", appId);
				wxPayData2.SetValue("scope", "jsapi_address");
				wxPayData2.SetValue("signType", "sha1");
				wxPayData2.SetValue("addrSign", text2);
				wxPayData2.SetValue("timeStamp", wxPayData.GetValue("timestamp"));
				wxPayData2.SetValue("nonceStr", wxPayData.GetValue("noncestr"));
				text = wxPayData2.ToJson();
				JsApiPay.paramDict.Add("paramJson", text);
				if (appendLog)
				{
					WxPayLog.AppendLog(JsApiPay.paramDict, text2, JsApiPay.page.Request.Url.ToString(), "获取收货地址js函数入口参数", LogType.GetOrEditAddress);
				}
			}
			catch (Exception ex)
			{
				WxPayLog.AppendLog(JsApiPay.paramDict, "", JsApiPay.page.Request.Url.ToString(), "获取收货地址js函数入口参数:" + ex.Message, LogType.GetOrEditAddress);
				throw new WxPayException(ex.ToString());
			}
			return text;
		}
	}
}
