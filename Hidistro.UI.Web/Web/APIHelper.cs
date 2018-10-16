using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Hidistro.UI.Web
{
	public class APIHelper
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

		public static string BuildSign(Dictionary<string, string> dicArray, string key, string sign_type, string _input_charset)
		{
			string str = APIHelper.CreateLinkstring(dicArray);
			str += key;
			return APIHelper.Sign(str, sign_type, _input_charset);
		}

		public static string CreateLinkstring(Dictionary<string, string> dicArray)
		{
			SortedDictionary<string, string> sortedDictionary = new SortedDictionary<string, string>(dicArray);
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<string, string> item in sortedDictionary)
			{
				if (!string.IsNullOrEmpty(item.Key) && !string.IsNullOrEmpty(item.Value))
				{
					stringBuilder.Append(item.Key + "=" + item.Value + "&");
				}
			}
			int length = stringBuilder.Length;
			stringBuilder.Remove(length - 1, 1);
			return stringBuilder.ToString();
		}

		public static string Sign(string prestr, string sign_type, string _input_charset)
		{
			StringBuilder stringBuilder = new StringBuilder(32);
			if (sign_type.ToUpper() == "MD5")
			{
				MD5 mD = new MD5CryptoServiceProvider();
				byte[] array = mD.ComputeHash(Encoding.GetEncoding(_input_charset).GetBytes(prestr));
				for (int i = 0; i < array.Length; i++)
				{
					stringBuilder.Append(array[i].ToString("x").PadLeft(2, '0'));
				}
			}
			return stringBuilder.ToString();
		}

		public static string PostData(string url, string postData)
		{
			string result = string.Empty;
			try
			{
				Uri requestUri = new Uri(url);
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUri);
				Encoding uTF = Encoding.UTF8;
				byte[] bytes = uTF.GetBytes(postData);
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
						Encoding uTF2 = Encoding.UTF8;
						Stream stream3 = stream2;
						if (httpWebResponse.ContentEncoding.ToLower() == "gzip")
						{
							stream3 = new GZipStream(stream2, CompressionMode.Decompress);
						}
						else if (httpWebResponse.ContentEncoding.ToLower() == "deflate")
						{
							stream3 = new DeflateStream(stream2, CompressionMode.Decompress);
						}
						using (StreamReader streamReader = new StreamReader(stream3, uTF2))
						{
							result = streamReader.ReadToEnd();
						}
					}
				}
			}
			catch (Exception ex)
			{
				result = $"获取信息错误：{ex.Message}";
			}
			return result;
		}

		public static string GetData(string url, string method = "GET")
		{
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
			httpWebRequest.Method = method;
			HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
			StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.UTF8);
			string result = streamReader.ReadToEnd();
			streamReader.Close();
			return result;
		}

		public static bool CheckSign(SortedDictionary<string, string> tmpParas, string keycode, string sign)
		{
			Dictionary<string, string> dicArray = APIHelper.Parameterfilter(tmpParas);
			string text = APIHelper.BuildSign(dicArray, keycode, "MD5", "utf-8");
			return APIHelper.BuildSign(dicArray, keycode, "MD5", "utf-8") == sign;
		}

		public static string GetSign(SortedDictionary<string, string> tmpParas, string keycode)
		{
			Dictionary<string, string> dicArray = APIHelper.Parameterfilter(tmpParas);
			return APIHelper.BuildSign(dicArray, keycode, "MD5", "utf-8");
		}
	}
}
