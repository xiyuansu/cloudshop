using System;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;

namespace Hidistro.Core.Helper
{
	public class WebHelper
	{
		private static string[] _browserlist = new string[7]
		{
			"ie",
			"chrome",
			"mozilla",
			"netscape",
			"firefox",
			"opera",
			"konqueror"
		};

		private static string[] _searchenginelist = new string[12]
		{
			"baidu",
			"google",
			"360",
			"sogou",
			"bing",
			"msn",
			"sohu",
			"soso",
			"sina",
			"163",
			"yahoo",
			"jikeu"
		};

		private static Regex _metaregex = new Regex("<meta([^<]*)charset=([^<]*)[\"']", RegexOptions.IgnoreCase | RegexOptions.Multiline);

		public static string HtmlDecode(string s)
		{
			return HttpUtility.HtmlDecode(s);
		}

		public static string HtmlEncode(string s)
		{
			return HttpUtility.HtmlEncode(s);
		}

		public static string UrlDecode(string s)
		{
			return HttpUtility.UrlDecode(s);
		}

		public static string UrlEncode(string s)
		{
			return HttpUtility.UrlEncode(s);
		}

		public static void DeleteCookie(string name)
		{
			HttpCookie httpCookie = new HttpCookie(name);
			httpCookie.Expires = DateTime.Now.AddYears(-1);
			HttpContext.Current.Response.AppendCookie(httpCookie);
		}

		public static string GetCookie(string name)
		{
			HttpCookie httpCookie = HttpContext.Current.Request.Cookies[name];
			if (httpCookie != null)
			{
				return httpCookie.Value;
			}
			return string.Empty;
		}

		public static string GetCookie(string name, string key)
		{
			HttpCookie httpCookie = HttpContext.Current.Request.Cookies[name];
			if (httpCookie?.HasKeys ?? false)
			{
				string text = httpCookie[key];
				if (text != null)
				{
					return text;
				}
			}
			return string.Empty;
		}

		public static void SetCookie(string name, string key, string value, DateTime? dt = default(DateTime?))
		{
			WebHelper.SetCookie(name, value, dt, key, true);
		}

		public static void SetCookie(string name, string value, DateTime? dt = default(DateTime?), string key = null, bool httponly = true)
		{
			HttpCookie httpCookie = HttpContext.Current.Request.Cookies[name];
			if (httpCookie == null)
			{
				httpCookie = new HttpCookie(name);
			}
			httpCookie.HttpOnly = httponly;
			if (string.IsNullOrEmpty(key))
			{
				httpCookie.Value = value;
			}
			else
			{
				httpCookie[key] = value;
			}
			if (dt.HasValue)
			{
				httpCookie.Expires = dt.Value;
			}
			HttpContext.Current.Response.AppendCookie(httpCookie);
		}

		public static bool IsGet()
		{
			return HttpContext.Current.Request.HttpMethod == "GET";
		}

		public static bool IsPost()
		{
			return HttpContext.Current.Request.HttpMethod == "POST";
		}

		public static bool IsAjax()
		{
			return HttpContext.Current.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
		}

		public static string GetQueryString(string key, string defaultValue)
		{
			string text = HttpContext.Current.Request.QueryString[key];
			if (!string.IsNullOrWhiteSpace(text))
			{
				return text;
			}
			return defaultValue;
		}

		public static string GetQueryString(string key)
		{
			return WebHelper.GetQueryString(key, "");
		}

		public static string GetFormString(string key, string defaultValue)
		{
			string text = HttpContext.Current.Request.Form[key];
			if (!string.IsNullOrWhiteSpace(text))
			{
				return text;
			}
			return defaultValue;
		}

		public static string GetFormString(string key)
		{
			return WebHelper.GetFormString(key, "");
		}

		public static string GetUrlReferrer()
		{
			Uri urlReferrer = HttpContext.Current.Request.UrlReferrer;
			if (urlReferrer == (Uri)null)
			{
				return string.Empty;
			}
			return urlReferrer.ToString();
		}

		public static string GetHost()
		{
			return HttpContext.Current.Request.Url.Host;
		}

		public static string GetPort()
		{
			return HttpContext.Current.Request.Url.Port.ToString();
		}

		public static string GetUrl()
		{
			return HttpContext.Current.Request.Url.ToString();
		}

		public static string GetRawUrl()
		{
			return HttpContext.Current.Request.RawUrl;
		}

		public static string GetIP()
		{
			string empty = string.Empty;
			empty = ((HttpContext.Current.Request.ServerVariables["HTTP_VIA"] == null) ? HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString() : HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString());
			if (string.IsNullOrEmpty(empty) || empty.Equals("::1"))
			{
				empty = "127.0.0.1";
			}
			return empty;
		}

		public static string GetBrowserType()
		{
			string type = HttpContext.Current.Request.Browser.Type;
			if (string.IsNullOrEmpty(type))
			{
				return "未知";
			}
			return type.ToLower();
		}

		public static string GetBrowserName()
		{
			string browser = HttpContext.Current.Request.Browser.Browser;
			if (string.IsNullOrEmpty(browser))
			{
				return "未知";
			}
			return browser.ToLower();
		}

		public static string GetBrowserVersion()
		{
			string version = HttpContext.Current.Request.Browser.Version;
			if (string.IsNullOrEmpty(version))
			{
				return "未知";
			}
			return version;
		}

		public static string GetOSType()
		{
			string result = "未知";
			string userAgent = HttpContext.Current.Request.UserAgent;
			if (userAgent.Contains("NT 6.1"))
			{
				result = "Windows 7";
			}
			else if (userAgent.Contains("NT 5.1"))
			{
				result = "Windows XP";
			}
			else if (userAgent.Contains("NT 6.2"))
			{
				result = "Windows 8";
			}
			else if (userAgent.Contains("android"))
			{
				result = "Android";
			}
			else if (userAgent.Contains("iphone"))
			{
				result = "IPhone";
			}
			else if (userAgent.Contains("Mac"))
			{
				result = "Mac";
			}
			else if (userAgent.Contains("NT 6.0"))
			{
				result = "Windows Vista";
			}
			else if (userAgent.Contains("NT 5.2"))
			{
				result = "Windows 2003";
			}
			else if (userAgent.Contains("NT 5.0"))
			{
				result = "Windows 2000";
			}
			else if (userAgent.Contains("98"))
			{
				result = "Windows 98";
			}
			else if (userAgent.Contains("95"))
			{
				result = "Windows 95";
			}
			else if (userAgent.Contains("Me"))
			{
				result = "Windows Me";
			}
			else if (userAgent.Contains("NT 4"))
			{
				result = "Windows NT4";
			}
			else if (userAgent.Contains("Unix"))
			{
				result = "UNIX";
			}
			else if (userAgent.Contains("Linux"))
			{
				result = "Linux";
			}
			else if (userAgent.Contains("SunOS"))
			{
				result = "SunOS";
			}
			return result;
		}

		public static string GetOSName()
		{
			string platform = HttpContext.Current.Request.Browser.Platform;
			if (string.IsNullOrEmpty(platform))
			{
				return "未知";
			}
			return platform;
		}

		public static bool IsBrowser()
		{
			string browserName = WebHelper.GetBrowserName();
			string[] browserlist = WebHelper._browserlist;
			foreach (string value in browserlist)
			{
				if (browserName.Contains(value))
				{
					return true;
				}
			}
			return false;
		}

		public static bool IsMobile()
		{
			if (HttpContext.Current.Request.Browser.IsMobileDevice)
			{
				return true;
			}
			bool flag = false;
			if (bool.TryParse(((HttpCapabilitiesBase)HttpContext.Current.Request.Browser)["IsTablet"], out flag) & flag)
			{
				return true;
			}
			return false;
		}

		public static bool IsCrawler()
		{
			bool crawler = HttpContext.Current.Request.Browser.Crawler;
			if (!crawler)
			{
				string urlReferrer = WebHelper.GetUrlReferrer();
				if (urlReferrer.Length > 0)
				{
					string[] searchenginelist = WebHelper._searchenginelist;
					foreach (string value in searchenginelist)
					{
						if (urlReferrer.Contains(value))
						{
							return true;
						}
					}
				}
			}
			return crawler;
		}

		public static NameValueCollection GetParmList(string data)
		{
			NameValueCollection nameValueCollection = new NameValueCollection(StringComparer.OrdinalIgnoreCase);
			if (!string.IsNullOrEmpty(data))
			{
				int length = data.Length;
				for (int i = 0; i < length; i++)
				{
					int num = i;
					int num2 = -1;
					while (i < length)
					{
						switch (data[i])
						{
						case '=':
							if (num2 < 0)
							{
								num2 = i;
							}
							goto default;
						default:
							i++;
							continue;
						case '&':
							break;
						}
						break;
					}
					string name;
					string value;
					if (num2 >= 0)
					{
						name = data.Substring(num, num2 - num);
						value = data.Substring(num2 + 1, i - num2 - 1);
					}
					else
					{
						name = data.Substring(num, i - num);
						value = string.Empty;
					}
					nameValueCollection[name] = value;
					if (i == length - 1 && data[i] == '&')
					{
						nameValueCollection[name] = string.Empty;
					}
				}
			}
			return nameValueCollection;
		}

		public static string GetPostData(string url, string postData)
		{
			return WebHelper.GetRequestData(url, "post", postData);
		}

		public static string GetRequestData(string url, string method, string parameterData)
		{
			return WebHelper.GetRequestData(url, method, parameterData, Encoding.UTF8, 20000);
		}

		public static string GetRequestData(string url, string method, string parameterData, Encoding encoding, int timeout = 20000)
		{
			using (HttpWebResponse httpWebResponse = WebHelper.GetURLResponse(url, method, parameterData, encoding, timeout))
			{
				if (httpWebResponse == null)
				{
					throw new Exception("Generate Response Error");
				}
				if (encoding == null)
				{
					MemoryStream memoryStream = new MemoryStream();
					Stream responseStream = httpWebResponse.GetResponseStream();
					if (httpWebResponse.ContentEncoding != null && httpWebResponse.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase))
					{
						new GZipStream(responseStream, CompressionMode.Decompress).CopyTo(memoryStream, 10240);
					}
					else
					{
						responseStream.CopyTo(memoryStream, 10240);
					}
					byte[] array = memoryStream.ToArray();
					string @string = Encoding.Default.GetString(array, 0, array.Length);
					Match match = WebHelper._metaregex.Match(@string);
					string text = (match.Groups.Count > 2) ? match.Groups[2].Value : string.Empty;
					text = text.Replace("\"", string.Empty).Replace("'", string.Empty).Replace(";", string.Empty);
					if (text.Length > 0)
					{
						text = text.ToLower().Replace("iso-8859-1", "gbk");
						encoding = Encoding.GetEncoding(text);
					}
					else
					{
						encoding = ((!(httpWebResponse.CharacterSet.ToLower().Trim() == "iso-8859-1")) ? ((!string.IsNullOrEmpty(httpWebResponse.CharacterSet.Trim())) ? Encoding.GetEncoding(httpWebResponse.CharacterSet) : Encoding.UTF8) : Encoding.GetEncoding("gbk"));
					}
					return encoding.GetString(array);
				}
				StreamReader streamReader = null;
				if (httpWebResponse.ContentEncoding != null && httpWebResponse.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase))
				{
					using (streamReader = new StreamReader(new GZipStream(httpWebResponse.GetResponseStream(), CompressionMode.Decompress), encoding))
					{
						return streamReader.ReadToEnd();
					}
				}
				using (streamReader = new StreamReader(httpWebResponse.GetResponseStream(), encoding))
				{
					return streamReader.ReadToEnd();
				}
			}
		}

		public static HttpWebResponse GetURLResponse(string url, string method = "get", string parameterData = "", Encoding encoding = null, int timeout = 20000)
		{
			if (encoding == null)
			{
				encoding = Encoding.UTF8;
			}
			if (!url.Contains("http://") && !url.Contains("https://"))
			{
				url = "http://" + url;
			}
			if (!string.IsNullOrWhiteSpace(parameterData) && method.ToLower() == "get")
			{
				bool flag = false;
				if (parameterData.IndexOf("?") == 0 || parameterData.IndexOf("&") == 0)
				{
					flag = true;
				}
				if (!flag)
				{
					url = ((url.IndexOf("?") >= 0) ? (url + "&") : (url + "?"));
				}
				url += parameterData;
			}
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
			httpWebRequest.Method = method.Trim().ToLower();
			httpWebRequest.Timeout = timeout;
			httpWebRequest.AllowAutoRedirect = true;
			httpWebRequest.ContentType = "text/html";
			httpWebRequest.Accept = "text/html, application/xhtml+xml, */*,zh-CN";
			httpWebRequest.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)";
			httpWebRequest.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
			if (!string.IsNullOrEmpty(parameterData) && httpWebRequest.Method == "post")
			{
				httpWebRequest.ContentType = "application/x-www-form-urlencoded";
				byte[] bytes = encoding.GetBytes(parameterData);
				httpWebRequest.ContentLength = bytes.Length;
				httpWebRequest.GetRequestStream().Write(bytes, 0, bytes.Length);
			}
			return (HttpWebResponse)httpWebRequest.GetResponse();
		}
	}
}
