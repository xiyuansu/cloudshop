using Hidistro.Context;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using System;
using System.Web;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class WAPLogout : WebControl
	{
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			MemberInfo user = HiContext.Current.User;
			if (user.UserId == 0)
			{
				this.Context.Response.Redirect($"/{HiContext.Current.GetClientPath}/MemberCenter?IsLogout=1", true);
			}
			Users.SetUserLogoutStatus(true);
			if (this.CurrentClientType() == ClientType.VShop && !user.IsQuickLogin)
			{
				Users.ClearWxOpenId(user.UserId);
			}
			Users.SetLoginStatus(user.UserId, false);
			HttpCookie httpCookie = HiContext.Current.Context.Request.Cookies["Token_" + HiContext.Current.UserId.ToString()];
			DateTime now;
			if (httpCookie != null && !string.IsNullOrEmpty(httpCookie.Value))
			{
				HttpCookie httpCookie2 = httpCookie;
				now = DateTime.Now;
				httpCookie2.Expires = now.AddDays(-1.0);
				HttpContext.Current.Response.Cookies.Add(httpCookie);
			}
			HttpCookie httpCookie3 = HiContext.Current.Context.Request.Cookies["PC-Member"];
			if (httpCookie3 != null && !string.IsNullOrEmpty(httpCookie3.Value))
			{
				httpCookie3.Expires = new DateTime(1911, 10, 12);
				HttpContext.Current.Response.Cookies.Add(httpCookie3);
			}
			HttpCookie httpCookie4 = HiContext.Current.Context.Request.Cookies["Shop-Member"];
			if (httpCookie4 != null && !string.IsNullOrEmpty(httpCookie4.Value))
			{
				HttpCookie httpCookie5 = httpCookie4;
				now = DateTime.Now;
				httpCookie5.Expires = now.AddDays(-1.0);
				HttpContext.Current.Response.Cookies.Add(httpCookie4);
			}
			HttpCookie httpCookie6 = HttpContext.Current.Request.Cookies["Site_ReferralUser"];
			if (httpCookie6 != null && !string.IsNullOrEmpty(httpCookie6.Value))
			{
				httpCookie6.Expires = new DateTime(1911, 10, 12);
				HttpContext.Current.Response.Cookies.Add(httpCookie6);
			}
			HttpCookie httpCookie7 = HttpContext.Current.Request.Cookies["Store_ShoppingGuider"];
			if (httpCookie7 != null && !string.IsNullOrEmpty(httpCookie7.Value))
			{
				httpCookie7.Expires = new DateTime(1911, 10, 12);
				HttpContext.Current.Response.Cookies.Add(httpCookie7);
			}
			HttpCookie httpCookie8 = HiContext.Current.Context.Request.Cookies["UserCoordinateCookie"];
			HttpCookie httpCookie9 = HiContext.Current.Context.Request.Cookies["UserCoordinateTimeCookie"];
			if (httpCookie8 != null && !string.IsNullOrEmpty(httpCookie8.Value))
			{
				httpCookie8.Expires = new DateTime(1911, 10, 12);
				HttpContext.Current.Response.Cookies.Add(httpCookie8);
			}
			if (httpCookie9 != null && !string.IsNullOrEmpty(httpCookie9.Value))
			{
				httpCookie9.Expires = new DateTime(1911, 10, 12);
				HttpContext.Current.Response.Cookies.Add(httpCookie9);
			}
			HiContext.Current.UserId = 0;
			HiContext.Current.User = null;
			this.Context.Response.Redirect($"/{HiContext.Current.GetClientPath}/MemberCenter?IsLogout=1", true);
		}

		private ClientType CurrentClientType()
		{
			string text = HttpContext.Current.Request.Url.ToString();
			ClientType result = ClientType.WAP;
			if (text.ToLower().IndexOf("/vshop/") > -1)
			{
				result = ClientType.VShop;
			}
			if (text.ToLower().IndexOf("/alioh/") > -1)
			{
				result = ClientType.AliOH;
			}
			if (text.ToLower().IndexOf("AppShop") > -1)
			{
				result = ClientType.App;
			}
			return result;
		}
	}
}
