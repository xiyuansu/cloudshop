using HiShop.API.Setting.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;

namespace HiShop.API.Setting.HttpUtility
{
	public static class RequestUtility
	{
		public static string HttpGet(string url, Encoding encoding = null, string appId = "", string appSecret = "", string accessToken = "")
		{
			WebClient webClient = new WebClient();
			webClient.Encoding = (encoding ?? Encoding.UTF8);
			if (!string.IsNullOrEmpty(appId) && !string.IsNullOrEmpty(appSecret))
			{
				byte[] bytes = Encoding.UTF8.GetBytes($"{appId}:{appSecret}".ToCharArray());
				webClient.Headers.Add("Authorization", "BASIC " + Convert.ToBase64String(bytes));
			}
			if (!string.IsNullOrEmpty(accessToken))
			{
				webClient.Headers.Add("Authorization", "Bearer " + accessToken);
			}
			return webClient.DownloadString(url);
		}

		public static string HttpGet(string url, CookieContainer cookieContainer = null, Encoding encoding = null, int timeOut = 10000)
		{
			HttpWebResponse httpWebResponse = null;
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
			httpWebRequest.Method = "GET";
			httpWebRequest.Timeout = timeOut;
			if (cookieContainer != null)
			{
				httpWebRequest.CookieContainer = cookieContainer;
			}
			try
			{
				httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
			}
			catch (WebException ex)
			{
				httpWebResponse = (ex.Response as HttpWebResponse);
			}
			if (cookieContainer != null)
			{
				httpWebResponse.Cookies = cookieContainer.GetCookies(httpWebResponse.ResponseUri);
			}
			using (Stream stream = httpWebResponse.GetResponseStream())
			{
				using (StreamReader streamReader = new StreamReader(stream, encoding ?? Encoding.GetEncoding("utf-8")))
				{
					return streamReader.ReadToEnd();
				}
			}
		}

		public static string HttpPost(string url, CookieContainer cookieContainer = null, Dictionary<string, string> formData = null, Encoding encoding = null, int timeOut = 10000, string appid = "", string appSecret = "")
		{
			MemoryStream memoryStream = new MemoryStream();
			formData.FillFormDataStream(memoryStream);
			return RequestUtility.HttpPost(url, cookieContainer, memoryStream, null, null, encoding, timeOut, appid, appSecret, "", "POST", false);
		}

		public static string HttpPost(string url, CookieContainer cookieContainer = null, Stream postStream = null, Dictionary<string, string> fileDictionary = null, string refererUrl = null, Encoding encoding = null, int timeOut = 10000, string appId = "", string appSecret = "", string accessToken = "", string method = "POST", bool checkValidationResult = false)
		{
			HttpWebResponse httpWebResponse = null;
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
			httpWebRequest.Method = method;
			httpWebRequest.Timeout = timeOut;
			if (!string.IsNullOrEmpty(appId) && !string.IsNullOrEmpty(appSecret))
			{
				byte[] bytes = Encoding.UTF8.GetBytes($"{appId}:{appSecret}".ToCharArray());
				httpWebRequest.Headers.Add("Authorization", "BASIC " + Convert.ToBase64String(bytes));
			}
			if (!string.IsNullOrEmpty(accessToken))
			{
				httpWebRequest.Headers.Add("Authorization", "Bearer " + accessToken);
			}
			if (checkValidationResult)
			{
				ServicePointManager.ServerCertificateValidationCallback = RequestUtility.CheckValidationResult;
			}
			if (fileDictionary != null && fileDictionary.Count > 0)
			{
				postStream = (postStream ?? new MemoryStream());
				string text = "----" + DateTime.Now.Ticks.ToString("x");
				string format = "\r\n--" + text + "\r\nContent-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: application/octet-stream\r\n\r\n";
				string format2 = "\r\n--" + text + "\r\nContent-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
				foreach (KeyValuePair<string, string> item in fileDictionary)
				{
					try
					{
						string value = item.Value;
						using (FileStream fileStream = FileHelper.GetFileStream(value))
						{
							string text2 = null;
							text2 = ((fileStream == null) ? string.Format(format2, item.Key, item.Value) : string.Format(format, item.Key, Path.GetFileName(value)));
							byte[] bytes2 = Encoding.UTF8.GetBytes((postStream.Length == 0) ? text2.Substring(2, text2.Length - 2) : text2);
							postStream.Write(bytes2, 0, bytes2.Length);
							if (fileStream != null)
							{
								byte[] array = new byte[1024];
								int num = 0;
								while ((num = fileStream.Read(array, 0, array.Length)) != 0)
								{
									postStream.Write(array, 0, num);
								}
							}
						}
					}
					catch (Exception ex)
					{
						throw ex;
					}
				}
				byte[] bytes3 = Encoding.UTF8.GetBytes("\r\n--" + text + "--\r\n");
				postStream.Write(bytes3, 0, bytes3.Length);
				httpWebRequest.ContentType = $"multipart/form-data; boundary={text}";
			}
			else
			{
				httpWebRequest.ContentType = "application/x-www-form-urlencoded";
			}
			httpWebRequest.ContentLength = (postStream?.Length ?? 0);
			httpWebRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
			httpWebRequest.KeepAlive = true;
			if (!string.IsNullOrEmpty(refererUrl))
			{
				httpWebRequest.Referer = refererUrl;
			}
			httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.57 Safari/537.36";
			if (cookieContainer != null)
			{
				httpWebRequest.CookieContainer = cookieContainer;
			}
			if (postStream != null)
			{
				postStream.Position = 0L;
				Stream requestStream = httpWebRequest.GetRequestStream();
				byte[] array = new byte[1024];
				int num = 0;
				while ((num = postStream.Read(array, 0, array.Length)) != 0)
				{
					requestStream.Write(array, 0, num);
				}
				postStream.Seek(0L, SeekOrigin.Begin);
				StreamReader streamReader = new StreamReader(postStream);
				string text3 = streamReader.ReadToEnd();
				postStream.Close();
			}
			try
			{
				httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
			}
			catch (WebException ex2)
			{
				httpWebResponse = (ex2.Response as HttpWebResponse);
			}
			if (cookieContainer != null)
			{
				httpWebResponse.Cookies = cookieContainer.GetCookies(httpWebResponse.ResponseUri);
			}
			using (Stream stream = httpWebResponse.GetResponseStream())
			{
				using (StreamReader streamReader2 = new StreamReader(stream, encoding ?? Encoding.GetEncoding("utf-8")))
				{
					return streamReader2.ReadToEnd();
				}
			}
		}

		private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
		{
			return true;
		}

		public static bool IsWeixinClientRequest(this HttpContext httpContext)
		{
			return !string.IsNullOrEmpty(httpContext.Request.UserAgent) && httpContext.Request.UserAgent.Contains("MicroMessenger");
		}

		public static string GetQueryString(this Dictionary<string, string> formData)
		{
			if (formData == null || formData.Count == 0)
			{
				return "";
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (KeyValuePair<string, string> formDatum in formData)
			{
				num++;
				stringBuilder.AppendFormat("{0}={1}", formDatum.Key, formDatum.Value);
				if (num < formData.Count)
				{
					stringBuilder.Append("&");
				}
			}
			return stringBuilder.ToString();
		}

		public static void FillFormDataStream(this Dictionary<string, string> formData, Stream stream)
		{
			string queryString = formData.GetQueryString();
			byte[] array = (formData == null) ? new byte[0] : Encoding.UTF8.GetBytes(queryString);
			stream.Write(array, 0, array.Length);
			stream.Seek(0L, SeekOrigin.Begin);
		}

		public static string HtmlEncode(this string html)
		{
			return System.Web.HttpUtility.HtmlEncode(html);
		}

		public static string HtmlDecode(this string html)
		{
			return System.Web.HttpUtility.HtmlDecode(html);
		}

		public static string UrlEncode(this string url)
		{
			return System.Web.HttpUtility.UrlEncode(url);
		}

		public static string UrlDecode(this string url)
		{
			return System.Web.HttpUtility.UrlDecode(url);
		}
	}
}
