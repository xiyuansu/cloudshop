using Hidistro.Core;
using System;
using System.Web.UI;

namespace Hidistro.UI.Web
{
	public class loginEntry : Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			string text = this.Page.Request.QueryString["returnUrl"].ToNullString().ToLower();
			if (!string.IsNullOrEmpty(text) && (text.StartsWith("/admin") || text.StartsWith("/depot") || text.StartsWith("/supplier")))
			{
				base.Response.Redirect(Globals.GetAdminAbsolutePath("login?returnUrl=" + text), true);
			}
			else
			{
				if (string.IsNullOrWhiteSpace(text))
				{
					text = base.Request.RawUrl;
				}
				base.Response.Redirect($"/User/Login?ReturnUrl={text}", true);
			}
		}
	}
}
