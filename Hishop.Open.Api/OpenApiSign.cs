using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Hishop.Open.Api
{
	public static class OpenApiSign
	{
		public static Dictionary<string, string> Parameterfilter(SortedDictionary<string, string> dicArrayPre)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			foreach (KeyValuePair<string, string> item in dicArrayPre)
			{
				if (item.Key.ToLower() != "sign" && item.Key.ToLower() != "sign_type" && item.Value != "" && item.Value != null)
				{
					dictionary.Add(item.Key.ToLower(), item.Value);
				}
			}
			return dictionary;
		}

		public static string BuildSign(Dictionary<string, string> dicArray, string appSecret, string sign_type, string _input_charset)
		{
			return OpenApiSign.Sign(OpenApiSign.CreateLinkstring(dicArray) + appSecret, sign_type, _input_charset);
		}

		public static string CreateLinkstring(Dictionary<string, string> dicArray)
		{
			SortedDictionary<string, string> sortedDictionary = new SortedDictionary<string, string>(dicArray);
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<string, string> item in sortedDictionary)
			{
				if (!string.IsNullOrEmpty(item.Key) && !string.IsNullOrEmpty(item.Value))
				{
					stringBuilder.Append(item.Key + item.Value);
				}
			}
			return stringBuilder.ToString();
		}

		public static string Sign(string prestr, string sign_type, string _input_charset)
		{
			StringBuilder stringBuilder = new StringBuilder(32);
			if (sign_type.ToUpper() == "MD5")
			{
				byte[] array = new MD5CryptoServiceProvider().ComputeHash(Encoding.GetEncoding(_input_charset).GetBytes(prestr));
				for (int i = 0; i < array.Length; i++)
				{
					stringBuilder.Append(array[i].ToString("x").PadLeft(2, '0'));
				}
			}
			return stringBuilder.ToString().ToUpper();
		}

		public static string PostData(string url, string postData)
		{
			string empty = string.Empty;
			try
			{
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));
				byte[] bytes = Encoding.UTF8.GetBytes(postData);
				httpWebRequest.Method = "POST";
				httpWebRequest.ContentType = "application/x-www-form-urlencoded";
				httpWebRequest.ContentLength = bytes.Length;
				using (Stream stream = httpWebRequest.GetRequestStream())
				{
					stream.Write(bytes, 0, bytes.Length);
				}
				using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
				{
					using (Stream stream2 = httpWebResponse.GetResponseStream())
					{
						Encoding uTF = Encoding.UTF8;
						Stream stream3 = stream2;
						if (httpWebResponse.ContentEncoding.ToLower() == "gzip")
						{
							stream3 = new GZipStream(stream2, CompressionMode.Decompress);
						}
						else if (httpWebResponse.ContentEncoding.ToLower() == "deflate")
						{
							stream3 = new DeflateStream(stream2, CompressionMode.Decompress);
						}
						using (StreamReader streamReader = new StreamReader(stream3, uTF))
						{
							return streamReader.ReadToEnd();
						}
					}
				}
			}
			catch (Exception ex)
			{
				return $"获取信息错误：{ex.Message}";
			}
		}

		public static string GetData(string url, string method = "GET")
		{
			HttpWebRequest obj = (HttpWebRequest)WebRequest.Create(url);
			obj.Method = method;
			StreamReader streamReader = new StreamReader(((HttpWebResponse)obj.GetResponse()).GetResponseStream(), Encoding.UTF8);
			string result = streamReader.ReadToEnd();
			streamReader.Close();
			return result;
		}

		public static bool CheckSign(SortedDictionary<string, string> tmpParas, string appSecret, ref string message)
		{
			bool flag = OpenApiSign.BuildSign(OpenApiSign.Parameterfilter(tmpParas), appSecret, "MD5", "utf-8") == tmpParas["sign"];
			message = (flag ? "" : OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Invalid_Signature, "sign"));
			return flag;
		}

		public static string GetSign(SortedDictionary<string, string> tmpParas, string keycode)
		{
			return OpenApiSign.BuildSign(OpenApiSign.Parameterfilter(tmpParas), keycode, "MD5", "utf-8");
		}

		public static bool CheckTimeStamp(string timestamp)
		{
			DateTime d = DateTime.Parse(timestamp);
			return (DateTime.Now - d).TotalMinutes <= 10.0;
		}
	}
}
