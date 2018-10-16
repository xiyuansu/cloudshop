using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.pay
{
	public class Core
	{
		public static Dictionary<string, string> FilterPara(SortedDictionary<string, string> dicArrayPre)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			foreach (KeyValuePair<string, string> item in dicArrayPre)
			{
				if (item.Key.ToLower() != "sign" && item.Key.ToLower() != "sign_type" && item.Value != "" && item.Value != null)
				{
					dictionary.Add(item.Key, item.Value);
				}
			}
			return dictionary;
		}

		public static string CreateLinkString(Dictionary<string, string> dicArray)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<string, string> item in dicArray)
			{
				stringBuilder.Append(item.Key + "=" + item.Value + "&");
			}
			int length = stringBuilder.Length;
			stringBuilder.Remove(length - 1, 1);
			return stringBuilder.ToString();
		}

		public static string CreateLinkStringUrlencode(Dictionary<string, string> dicArray, Encoding code)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<string, string> item in dicArray)
			{
				stringBuilder.Append(item.Key + "=" + HttpUtility.UrlEncode(item.Value, code) + "&");
			}
			int length = stringBuilder.Length;
			stringBuilder.Remove(length - 1, 1);
			return stringBuilder.ToString();
		}

		public static void LogResult(string sWord)
		{
			string str = HttpContext.Current.Server.MapPath("log");
			str = str + "\\" + DateTime.Now.ToString().Replace(":", "") + ".txt";
			StreamWriter streamWriter = new StreamWriter(str, false, Encoding.Default);
			streamWriter.Write(sWord);
			streamWriter.Close();
		}

		public static string GetAbstractToMD5(Stream sFile)
		{
			MD5 mD = new MD5CryptoServiceProvider();
			byte[] array = mD.ComputeHash(sFile);
			StringBuilder stringBuilder = new StringBuilder(32);
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append(array[i].ToString("x").PadLeft(2, '0'));
			}
			return stringBuilder.ToString();
		}

		public static string GetAbstractToMD5(byte[] dataFile)
		{
			MD5 mD = new MD5CryptoServiceProvider();
			byte[] array = mD.ComputeHash(dataFile);
			StringBuilder stringBuilder = new StringBuilder(32);
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append(array[i].ToString("x").PadLeft(2, '0'));
			}
			return stringBuilder.ToString();
		}
	}
}
