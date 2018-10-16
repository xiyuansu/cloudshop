using System.Web;

namespace HiShop.API.Setting.BrowserUtility
{
	public static class BroswerUtility
	{
		public static bool SideInWeixinBroswer(HttpContextBase httpContext)
		{
			string userAgent = httpContext.Request.UserAgent;
			if (string.IsNullOrEmpty(userAgent) || (!userAgent.Contains("MicroMessenger") && !userAgent.Contains("Windows Phone")))
			{
				return false;
			}
			return true;
		}
	}
}
