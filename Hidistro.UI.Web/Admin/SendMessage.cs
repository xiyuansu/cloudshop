using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.AddMessage)]
	public class SendMessage : AdminPage
	{
		private int userId = 0;

		protected TextBox txtTitle;

		protected HtmlGenericControl txtTitleTip;

		protected TextBox txtContent;

		protected Button btnRefer;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnRefer.Click += this.btnRefer_Click;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["UserId"]) && !int.TryParse(this.Page.Request.QueryString["UserId"], out this.userId))
			{
				base.GotoResourceNotFound();
			}
		}

		private void btnRefer_Click(object sender, EventArgs e)
		{
			if (this.ValidateValues())
			{
				HttpCookie httpCookie = new HttpCookie("Title");
				httpCookie.HttpOnly = true;
				httpCookie.Value = Globals.UrlEncode(this.txtTitle.Text.Replace("\r\n", ""));
				HttpCookie httpCookie2 = httpCookie;
				DateTime now = DateTime.Now;
				httpCookie2.Expires = now.AddMinutes(20.0);
				HttpContext.Current.Response.Cookies.Add(httpCookie);
				HttpCookie httpCookie3 = new HttpCookie("Content");
				httpCookie3.HttpOnly = true;
				httpCookie3.Value = Globals.UrlEncode(this.txtContent.Text.Replace("\r\n", ""));
				HttpCookie httpCookie4 = httpCookie3;
				now = DateTime.Now;
				httpCookie4.Expires = now.AddMinutes(20.0);
				HttpContext.Current.Response.Cookies.Add(httpCookie3);
				if (this.userId == 0)
				{
					this.Page.Response.Redirect(Globals.GetAdminAbsolutePath("/comment/SendMessageSelectUser.aspx"));
				}
				else if (this.userId > 0)
				{
					this.Page.Response.Redirect(Globals.GetAdminAbsolutePath($"/comment/SendMessageSelectUser.aspx?UserId={this.userId}"));
				}
			}
		}

		private bool ValidateValues()
		{
			string text = string.Empty;
			if (string.IsNullOrEmpty(this.txtTitle.Text.Trim()) || this.txtTitle.Text.Trim().Length > 60)
			{
				text += Formatter.FormatErrorMessage("标题不能为空，长度限制在1-60个字符内");
			}
			if (string.IsNullOrEmpty(this.txtContent.Text.Trim()) || this.txtContent.Text.Trim().Length > 300)
			{
				text += Formatter.FormatErrorMessage("内容不能为空，长度限制在1-300个字符内");
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.ShowMsg(text, false);
				return false;
			}
			return true;
		}
	}
}
