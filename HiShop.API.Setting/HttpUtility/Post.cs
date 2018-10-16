using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

namespace HiShop.API.Setting.HttpUtility
{
	public static class Post
	{
		public static T GetResult<T>(string returnText)
		{
			JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
			return javaScriptSerializer.Deserialize<T>(returnText);
		}

		public static T PostFileGetJson<T>(string url, CookieContainer cookieContainer = null, Dictionary<string, string> fileDictionary = null, Dictionary<string, string> postDataDictionary = null, Encoding encoding = null, int timeOut = 10000, string appId = "", string appSecret = "", string accessToken = "", string method = "POST")
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				postDataDictionary.FillFormDataStream(memoryStream);
				string returnText = RequestUtility.HttpPost(url, cookieContainer, memoryStream, fileDictionary, null, encoding, timeOut, appId, appSecret, accessToken, method, false);
				return Post.GetResult<T>(returnText);
			}
		}

		public static T PostGetJson<T>(string url, CookieContainer cookieContainer = null, Dictionary<string, string> formData = null, Encoding encoding = null, string appId = "", string appSecret = "", int timeOut = 10000)
		{
			string returnText = RequestUtility.HttpPost(url, cookieContainer, formData, encoding, timeOut, appId, appSecret);
			return Post.GetResult<T>(returnText);
		}

		public static void Download(string url, string data, Stream stream)
		{
			WebClient webClient = new WebClient();
			byte[] array = webClient.UploadData(url, "POST", Encoding.UTF8.GetBytes(string.IsNullOrEmpty(data) ? "" : data));
			byte[] array2 = array;
			foreach (byte value in array2)
			{
				stream.WriteByte(value);
			}
		}
	}
}
