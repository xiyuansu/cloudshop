using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Web;

namespace Hishop.Weixin.MP.Util
{
	public sealed class WebUtils
	{
		public string DoPost(string url, IDictionary<string, string> parameters)
		{
			try
			{
				HttpWebRequest webRequest = this.GetWebRequest(url, "POST");
				webRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
				byte[] bytes = Encoding.UTF8.GetBytes(WebUtils.BuildQuery(parameters));
				Stream requestStream = webRequest.GetRequestStream();
				requestStream.Write(bytes, 0, bytes.Length);
				requestStream.Close();
				HttpWebResponse rsp = (HttpWebResponse)webRequest.GetResponse();
				return this.GetResponseAsString(rsp, Encoding.UTF8);
			}
			catch (Exception ex)
			{
				parameters.Add("url", url);
				WebUtils.AppendLog(parameters, ex, "GetWebRequest");
				return "";
			}
		}

		public string DoPost(string url, string value)
		{
			try
			{
				HttpWebRequest webRequest = this.GetWebRequest(url, "POST");
				webRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
				byte[] bytes = Encoding.UTF8.GetBytes(value);
				Stream requestStream = webRequest.GetRequestStream();
				requestStream.Write(bytes, 0, bytes.Length);
				requestStream.Close();
				HttpWebResponse rsp = (HttpWebResponse)webRequest.GetResponse();
				return this.GetResponseAsString(rsp, Encoding.UTF8);
			}
			catch (Exception ex)
			{
				IDictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("url", url);
				dictionary.Add("value", value);
				WebUtils.AppendLog(dictionary, ex, "GetWebRequest");
				return "";
			}
		}

		public string DoGet(string url, IDictionary<string, string> parameters)
		{
			try
			{
				if (parameters != null && parameters.Count > 0)
				{
					url = ((!url.Contains("?")) ? (url + "?" + WebUtils.BuildQuery(parameters)) : (url + "&" + WebUtils.BuildQuery(parameters)));
				}
				HttpWebRequest webRequest = this.GetWebRequest(url, "GET");
				webRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
				HttpWebResponse rsp = (HttpWebResponse)webRequest.GetResponse();
				return this.GetResponseAsString(rsp, Encoding.UTF8);
			}
			catch (Exception ex)
			{
				parameters.Add("url", url);
				WebUtils.AppendLog(parameters, ex, "GetWebRequest");
				return "";
			}
		}

		public bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
		{
			return true;
		}

		public HttpWebRequest GetWebRequest(string url, string method)
		{
			try
			{
				HttpWebRequest httpWebRequest = null;
				if (url.Contains("https"))
				{
					ServicePointManager.ServerCertificateValidationCallback = this.CheckValidationResult;
					httpWebRequest = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
				}
				else
				{
					httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
				}
				httpWebRequest.ServicePoint.Expect100Continue = false;
				httpWebRequest.Method = method;
				httpWebRequest.KeepAlive = true;
				httpWebRequest.UserAgent = "Hishop";
				return httpWebRequest;
			}
			catch (Exception ex)
			{
				IDictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("url", url);
				dictionary.Add("method", method);
				WebUtils.AppendLog(dictionary, ex, "GetWebRequest");
				return null;
			}
		}

		public string GetResponseAsString(HttpWebResponse rsp, Encoding encoding)
		{
			Stream stream = null;
			StreamReader streamReader = null;
			try
			{
				stream = rsp.GetResponseStream();
				streamReader = new StreamReader(stream, encoding);
				return streamReader.ReadToEnd();
			}
			finally
			{
				streamReader?.Close();
				stream?.Close();
				rsp?.Close();
			}
		}

		public string BuildGetUrl(string url, IDictionary<string, string> parameters)
		{
			if (parameters != null && parameters.Count > 0)
			{
				url = ((!url.Contains("?")) ? (url + "?" + WebUtils.BuildQuery(parameters)) : (url + "&" + WebUtils.BuildQuery(parameters)));
			}
			return url;
		}

		public static string BuildQuery(IDictionary<string, string> parameters)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			IEnumerator<KeyValuePair<string, string>> enumerator = parameters.GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<string, string> current = enumerator.Current;
				string key = current.Key;
				current = enumerator.Current;
				string value = current.Value;
				if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
				{
					if (flag)
					{
						stringBuilder.Append("&");
					}
					stringBuilder.Append(key);
					stringBuilder.Append("=");
					stringBuilder.Append(HttpUtility.UrlEncode(value, Encoding.UTF8));
					flag = true;
				}
			}
			return stringBuilder.ToString();
		}

		public static void AppendLog(IDictionary<string, string> param, Exception ex = null, string logPath = "")
		{
			if (param == null)
			{
				param = new Dictionary<string, string>();
			}
			if (ex != null)
			{
				if (ex is ThreadAbortException)
				{
					return;
				}
				param.Add("ErrorMessage", ex.Message);
				param.Add("StackTrace", ex.StackTrace);
				if (ex.InnerException != null)
				{
					param.Add("InnerException", ex.InnerException.ToString());
				}
				if (ex.GetBaseException() != null)
				{
					param.Add("BaseException", ex.GetBaseException().Message);
				}
				if (ex.TargetSite != null)
				{
					param.Add("TargetSite", ex.TargetSite.ToString());
				}
				param.Add("ExSource", ex.Source);
			}
			object obj = new object();
			lock (obj)
			{
				DateTime now;
				if (string.IsNullOrEmpty(logPath))
				{
					now = DateTime.Now;
					logPath = "/log/error_" + now.ToString("yyyyMMddHHmm") + ".txt";
				}
				else if (logPath.IndexOf('/') == -1 && logPath.IndexOf('.') == -1)
				{
					string[] obj2 = new string[5]
					{
						"/log/",
						logPath,
						"_",
						null,
						null
					};
					now = DateTime.Now;
					obj2[3] = now.ToString("yyyyMMddHHmm");
					obj2[4] = ".txt";
					logPath = string.Concat(obj2);
				}
				string path = WebUtils.GetphysicsPath(logPath);
				WebUtils.CreatePath(path);
				using (StreamWriter streamWriter = File.AppendText(path))
				{
					StreamWriter streamWriter2 = streamWriter;
					now = DateTime.Now;
					streamWriter2.WriteLine("时间：" + now.ToString());
					if (param != null)
					{
						foreach (KeyValuePair<string, string> item in param)
						{
							streamWriter.WriteLine(item.Key + ":" + item.Value);
						}
					}
					streamWriter.WriteLine("");
					streamWriter.WriteLine("");
				}
			}
		}

		public static bool CreatePath(string path)
		{
			bool result = true;
			path = WebUtils.GetphysicsPath(path).ToLower();
			string text = AppDomain.CurrentDomain.BaseDirectory.ToLower();
			path = path.Replace(text, "");
			string[] array = path.Split(Path.DirectorySeparatorChar);
			string str = array[0];
			str = text + str;
			try
			{
				for (int i = 1; i < array.Length - 1; i++)
				{
					str = str + Path.DirectorySeparatorChar.ToString() + array[i];
					if (!Directory.Exists(str))
					{
						Directory.CreateDirectory(str);
					}
				}
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		public static string GetphysicsPath(string path)
		{
			string text = "";
			try
			{
				if (HttpContext.Current == null)
				{
					string text2 = path.Replace("/", "\\");
					if (text2.StartsWith("\\"))
					{
						text2 = text2.TrimStart('\\');
					}
					return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, text2);
				}
				return HttpContext.Current.Request.MapPath(path);
			}
			catch
			{
				return path;
			}
		}
	}
}
