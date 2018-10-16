using Hidistro.Context;
using Hidistro.Core;
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
	public class WAPBindEmail : WAPMemberTemplatedWebControl
	{
		private HtmlInputHidden hidErrorMsg;

		private HtmlInputHidden action;

		private HtmlInputControl txtFirstEmail;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-BindEmail.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.hidErrorMsg = (HtmlInputHidden)this.FindControl("hidErrorMsg");
			this.action = (HtmlInputHidden)this.FindControl("action");
			this.txtFirstEmail = (HtmlInputControl)this.FindControl("txtFirstEmail");
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
			else
			{
				if (!string.IsNullOrEmpty(user.Email))
				{
					this.txtFirstEmail.SetWhenIsNotNull(Globals.GetEmailSafeStr(user.Email, 2, 3));
					if (user.EmailVerification)
					{
						this.action.Value = "modify";
						PageTitle.AddSiteNameTitle("修改绑定邮箱");
					}
					else
					{
						this.action.Value = "verify";
						PageTitle.AddSiteNameTitle("验证绑定邮箱");
					}
				}
				else
				{
					this.action.Value = "bind";
					PageTitle.AddSiteNameTitle("绑定邮箱");
				}
				if (string.IsNullOrEmpty(HiContext.Current.SiteSettings.EmailSettings) || !HiContext.Current.SiteSettings.EmailEnabled)
				{
					this.ShowWapMessage("商城未开启邮件发送功能！", "MemberCenter");
				}
			}
		}
	}
}
