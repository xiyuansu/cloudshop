using System.Text;
using System.Web.Script.Serialization;

namespace HiShop.API.Setting.HttpUtility
{
	public static class Get
	{
		public static T GetJson<T>(string url, Encoding encoding = null, string appId = "", string appSecret = "", string accessToken = "")
		{
			string input = RequestUtility.HttpGet(url, null, appId, appSecret, accessToken);
			JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
			return javaScriptSerializer.Deserialize<T>(input);
		}
	}
}
