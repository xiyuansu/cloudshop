using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Member;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.AppShop
{
	public class AppLogin : Page
	{
		private string sessionId;

		protected HtmlForm form1;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.sessionId = this.Page.Request.QueryString["sessionId"];
			if (!string.IsNullOrEmpty(this.sessionId))
			{
				MemberInfo memberInfo = MemberProcessor.FindMemberBySessionId(this.sessionId);
				if (memberInfo == null)
				{
					base.Response.Redirect("SessionLogin?IsErrorSessionId=true");
				}
				Users.SetCurrentUser(memberInfo.UserId, 30, true, false);
			}
			string text = base.Request.QueryString["returnUrl"];
			if (!string.IsNullOrEmpty(text))
			{
				base.Response.Redirect(Globals.UrlDecode(text));
			}
		}

		public void ClearLoginStatus()
		{
			try
			{
				HttpCookie httpCookie = HiContext.Current.Context.Request.Cookies["Token_" + HiContext.Current.UserId.ToString()];
				if (httpCookie != null && !string.IsNullOrEmpty(httpCookie.Value))
				{
					httpCookie.Expires = new DateTime(1911, 10, 12);
					HttpContext.Current.Response.Cookies.Add(httpCookie);
				}
				HttpCookie httpCookie2 = HiContext.Current.Context.Request.Cookies["Shop-Member"];
				if (httpCookie2 != null && !string.IsNullOrEmpty(httpCookie2.Value))
				{
					httpCookie2.Expires = new DateTime(1911, 10, 12);
					HttpContext.Current.Response.Cookies.Add(httpCookie2);
				}
			}
			catch
			{
			}
		}
	}
}
