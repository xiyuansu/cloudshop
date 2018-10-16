using Hidistro.Context;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	public class AccessDenied : AdminPage
	{
		protected Literal litMessage;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(base.Request.QueryString["errormsg"]))
			{
				this.litMessage.Text = base.Request.QueryString["errormsg"];
			}
			else
			{
				this.litMessage.Text = $"您登录的管理员帐号 “{HiContext.Current.Manager.UserName}” 没有权限访问当前页面或进行当前操作";
			}
		}
	}
}
