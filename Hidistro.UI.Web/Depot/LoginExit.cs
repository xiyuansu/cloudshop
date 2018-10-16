using Hidistro.Context;
using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;

namespace Hidistro.UI.Web.Depot
{
	public class LoginExit : Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			FormsAuthentication.SignOut();
			HttpCookie authCookie = FormsAuthentication.GetAuthCookie(HiContext.Current.ManagerId.ToString(), true);
			if (authCookie != null)
			{
				authCookie.Expires = new DateTime(1911, 10, 12);
				HiContext.Current.Context.Response.Cookies.Add(authCookie);
			}
			HttpCookie httpCookie = HiContext.Current.Context.Request.Cookies["UserCoordinateCookie"];
			HttpCookie httpCookie2 = HiContext.Current.Context.Request.Cookies["UserCoordinateTimeCookie"];
			if (httpCookie != null && !string.IsNullOrEmpty(httpCookie.Value))
			{
				httpCookie.Expires = new DateTime(1911, 10, 12);
				HttpContext.Current.Response.Cookies.Add(httpCookie);
			}
			if (httpCookie2 != null && !string.IsNullOrEmpty(httpCookie2.Value))
			{
				httpCookie2.Expires = new DateTime(1911, 10, 12);
				HttpContext.Current.Response.Cookies.Add(httpCookie2);
			}
			base.Response.Redirect("~/Admin/Login.aspx", true);
		}
	}
}
