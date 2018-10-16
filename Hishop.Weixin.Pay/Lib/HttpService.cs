using Hishop.Weixin.Pay.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Web;

namespace Hishop.Weixin.Pay.Lib
{
	public class HttpService
	{
		public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
		{
			return true;
		}

		public static string Post(string xml, string url, bool isUseCert, PayConfig config, int timeout)
		{
			GC.Collect();
			string text = "";
			HttpWebRequest httpWebRequest = null;
			HttpWebResponse httpWebResponse = null;
			Stream stream = null;
			IDictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("xml", xml);
			dictionary.Add("url", url);
			dictionary.Add("isUseCert", isUseCert.ToString());
			dictionary.Add("timeout", timeout.ToString());
			dictionary.Add("SSLCERT_PATH", config.SSLCERT_PATH);
			dictionary.Add("SSLCERT_PASSWORD", config.SSLCERT_PASSWORD);
			dictionary.Add("PROXY_URL", config.PROXY_URL);
			long num = 0L;
			try
			{
				ServicePointManager.DefaultConnectionLimit = 200;
				if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
				{
					ServicePointManager.ServerCertificateValidationCallback = HttpService.CheckValidationResult;
				}
				httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
				httpWebRequest.Method = "POST";
				httpWebRequest.Timeout = -1;
				httpWebRequest.ContentType = "text/xml";
				byte[] bytes = Encoding.UTF8.GetBytes(xml);
				httpWebRequest.ContentLength = bytes.Length;
				if (isUseCert)
				{
					string physicalApplicationPath = HttpContext.Current.Request.PhysicalApplicationPath;
					FileStream fileStream = new FileStream(physicalApplicationPath + config.SSLCERT_PATH, FileMode.Open, FileAccess.Read, FileShare.Read);
					byte[] array = new byte[fileStream.Length];
					fileStream.Read(array, 0, array.Length);
					fileStream.Close();
					X509Certificate2 value = new X509Certificate2(array, config.SSLCERT_PASSWORD, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);
					httpWebRequest.ClientCertificates.Add(value);
				}
				stream = httpWebRequest.GetRequestStream();
				stream.Write(bytes, 0, bytes.Length);
				stream.Close();
				ServicePointManager.ServerCertificateValidationCallback = HttpService.CheckValidationResult;
				httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
				StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.UTF8);
				text = streamReader.ReadToEnd().Trim();
				WxPayLog.AppendLog(dictionary, "", url + "---0", text, LogType.Refund);
				streamReader.Close();
			}
			catch (ThreadAbortException)
			{
				WxPayLog.AppendLog(dictionary, xml, url + "---1", text, LogType.Refund);
				Thread.ResetAbort();
				return "";
			}
			catch (WebException ex2)
			{
				if (ex2.Status == WebExceptionStatus.ProtocolError)
				{
					dictionary.Add("StatusCode", ((HttpWebResponse)ex2.Response).StatusCode.ToString());
					dictionary.Add("StatusCode", ((HttpWebResponse)ex2.Response).StatusDescription);
				}
				WxPayLog.AppendLog(dictionary, xml, url + "---2---" + num + "---" + ex2.Message, text, LogType.Refund);
				return "";
			}
			catch (Exception ex3)
			{
				dictionary.Add("HttpService", ex3.ToString());
				WxPayLog.AppendLog(dictionary, xml, url + "---3", text, LogType.Refund);
				return "";
			}
			finally
			{
				httpWebResponse?.Close();
				httpWebRequest?.Abort();
			}
			return text;
		}

		public static string Get(string url, string PROXY_URL = "")
		{
			IDictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("PROXY_URL", PROXY_URL);
			GC.Collect();
			string result = "";
			HttpWebRequest httpWebRequest = null;
			HttpWebResponse httpWebResponse = null;
			try
			{
				ServicePointManager.DefaultConnectionLimit = 200;
				if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
				{
					ServicePointManager.ServerCertificateValidationCallback = HttpService.CheckValidationResult;
				}
				httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
				httpWebRequest.Method = "GET";
				if (!string.IsNullOrEmpty(PROXY_URL))
				{
					WebProxy webProxy = new WebProxy();
					webProxy.Address = new Uri(PROXY_URL);
					httpWebRequest.Proxy = webProxy;
				}
				httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
				StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.UTF8);
				result = streamReader.ReadToEnd().Trim();
				streamReader.Close();
			}
			catch (ThreadAbortException ex)
			{
				dictionary.Add("HttpService", "Thread - caught ThreadAbortException - resetting.");
				dictionary.Add("Exception message: {0}", ex.Message);
				WxPayLog.writeLog(dictionary, "", url, "", LogType.Error);
				Thread.ResetAbort();
			}
			catch (WebException ex2)
			{
				dictionary.Add("HttpService", ex2.ToString());
				if (ex2.Status == WebExceptionStatus.ProtocolError)
				{
					dictionary.Add("HttpService", "StatusCode : " + ((HttpWebResponse)ex2.Response).StatusCode);
					dictionary.Add("HttpService", "StatusDescription : " + ((HttpWebResponse)ex2.Response).StatusDescription);
				}
				WxPayLog.writeLog(dictionary, "", url, "", LogType.Error);
				return "";
			}
			catch (Exception ex3)
			{
				dictionary.Add("HttpService", ex3.ToString());
				WxPayLog.writeLog(dictionary, "", url, "", LogType.Error);
				return "";
			}
			finally
			{
				httpWebResponse?.Close();
				httpWebRequest?.Abort();
			}
			return result;
		}
	}
}
