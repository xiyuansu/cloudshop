using Hidistro.Context;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WAPSetPassword : WAPMemberTemplatedWebControl
	{
		private HtmlInputHidden hidErrorMsg;

		private HtmlInputHidden hidHasPassword;

		private HtmlGenericControl setpwdPanel;

		private HtmlGenericControl changepwdPanel;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-SetPassword.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.hidErrorMsg = (HtmlInputHidden)this.FindControl("hidErrorMsg");
			this.setpwdPanel = (HtmlGenericControl)this.FindControl("setpwdPanel");
			this.changepwdPanel = (HtmlGenericControl)this.FindControl("changepwdPanel");
			this.hidHasPassword = (HtmlInputHidden)this.FindControl("hidHasPassword");
			MemberInfo user = Users.GetUser(HiContext.Current.UserId);
			if (user == null)
			{
				if (base.ClientType == ClientType.VShop)
				{
					HttpContext.Current.Response.Redirect("/VShop/MemberCenter.aspx");
				}
				else
				{
					HttpContext.Current.Response.Redirect("Login.aspx");
				}
			}
			else if (user.PasswordSalt == "Open")
			{
				this.setpwdPanel.Visible = true;
				this.changepwdPanel.Visible = false;
				this.hidHasPassword.Value = "0";
			}
			else
			{
				this.changepwdPanel.Visible = true;
				this.setpwdPanel.Visible = false;
				this.hidHasPassword.Value = "1";
			}
		}
	}
}
