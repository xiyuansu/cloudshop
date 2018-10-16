using Hidistro.Context;
using Hidistro.Entities.Members;
using System;
using System.Web;
using System.Web.UI;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class Logout : Page
	{
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			MemberInfo user = HiContext.Current.User;
			if (HiContext.Current.UserId != 0)
			{
				HttpCookie httpCookie = HiContext.Current.Context.Request.Cookies["Token_" + HiContext.Current.UserId.ToString()];
				if (httpCookie != null && !string.IsNullOrEmpty(httpCookie.Value))
				{
					httpCookie.Expires = new DateTime(1911, 10, 12);
					HttpContext.Current.Response.Cookies.Add(httpCookie);
				}
			}
			Users.SetUserLogoutStatus(true);
			HttpCookie httpCookie2 = HiContext.Current.Context.Request.Cookies["PC-Member"];
			if (httpCookie2 != null && !string.IsNullOrEmpty(httpCookie2.Value))
			{
				httpCookie2.Expires = new DateTime(1911, 10, 12);
				HttpContext.Current.Response.Cookies.Add(httpCookie2);
			}
			HttpCookie httpCookie3 = HiContext.Current.Context.Request.Cookies["Shop-Member"];
			if (httpCookie3 != null && !string.IsNullOrEmpty(httpCookie3.Value))
			{
				httpCookie3.Expires = DateTime.Now.AddDays(-1.0);
				HttpContext.Current.Response.Cookies.Add(httpCookie3);
			}
			HiContext.Current.User = null;
			HiContext.Current.UserId = 0;
			HttpCookie httpCookie4 = HttpContext.Current.Request.Cookies["Site_ReferralUser"];
			if (httpCookie4 != null && !string.IsNullOrEmpty(httpCookie4.Value))
			{
				httpCookie4.Expires = new DateTime(1911, 10, 12);
				HttpContext.Current.Response.Cookies.Add(httpCookie4);
			}
			HttpCookie httpCookie5 = HiContext.Current.Context.Request.Cookies["UserCoordinateCookie"];
			HttpCookie httpCookie6 = HiContext.Current.Context.Request.Cookies["UserCoordinateTimeCookie"];
			if (httpCookie5 != null && !string.IsNullOrEmpty(httpCookie5.Value))
			{
				httpCookie5.Expires = new DateTime(1911, 10, 12);
				HttpContext.Current.Response.Cookies.Add(httpCookie5);
			}
			if (httpCookie6 != null && !string.IsNullOrEmpty(httpCookie6.Value))
			{
				httpCookie6.Expires = new DateTime(1911, 10, 12);
				HttpContext.Current.Response.Cookies.Add(httpCookie6);
			}
			this.Context.Response.Redirect("/", true);
		}
	}
}
