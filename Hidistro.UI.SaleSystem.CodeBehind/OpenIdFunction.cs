using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public static class OpenIdFunction
	{
		private const string FormFormat = "<form id=\"openidform\" name=\"openidform\" action=\"{0}\" method=\"POST\">{1}</form>";

		private const string InputFormat = "<input type=\"hidden\" id=\"{0}\" name=\"{0}\" value=\"{1}\">";

		public static string CreateField(string name, string strValue)
		{
			return string.Format(CultureInfo.InvariantCulture, "<input type=\"hidden\" id=\"{0}\" name=\"{0}\" value=\"{1}\">", new object[2]
			{
				name,
				strValue
			});
		}

		public static string CreateForm(string content, string action)
		{
			content += "<input type=\"submit\" value=\"信任登录\" style=\"display:none;\">";
			return string.Format(CultureInfo.InvariantCulture, "<form id=\"openidform\" name=\"openidform\" action=\"{0}\" method=\"POST\">{1}</form>", new object[2]
			{
				action,
				content
			});
		}

		public static void Submit(string formContent)
		{
			string s = formContent + "<script>document.forms['openidform'].submit();</script>";
			HttpContext.Current.Response.Write(s);
			HttpContext.Current.Response.End();
		}

		public static string BuildMysign(Dictionary<string, string> dicArray, string key, string sign_type, string _input_charset)
		{
			string str = OpenIdFunction.CreateLinkString(dicArray);
			str += key;
			return OpenIdFunction.Sign(str, sign_type, _input_charset);
		}

		public static Dictionary<string, string> FilterPara(SortedDictionary<string, string> dicArrayPre)
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
	}
}
