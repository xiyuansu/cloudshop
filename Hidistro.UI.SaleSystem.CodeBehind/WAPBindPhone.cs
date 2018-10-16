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
	public class WAPBindPhone : WAPMemberTemplatedWebControl
	{
		private HtmlInputHidden hidErrorMsg;

		private HtmlInputHidden action;

		private HtmlInputHidden hidIsOpenGeetest;

		private HtmlInputControl txtFirstPhone;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-BindPhone.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.action = (HtmlInputHidden)this.FindControl("action");
			this.hidErrorMsg = (HtmlInputHidden)this.FindControl("hidErrorMsg");
			this.txtFirstPhone = (HtmlInputControl)this.FindControl("txtFirstPhone");
			this.hidIsOpenGeetest = (HtmlInputHidden)this.FindControl("hidIsOpenGeetest");
			this.hidIsOpenGeetest.Value = "0";
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
				if (!string.IsNullOrEmpty(user.CellPhone))
				{
					if (user.CellPhoneVerification)
					{
						this.action.Value = "modify";
						PageTitle.AddSiteNameTitle("修改绑定手机");
					}
					else
					{
						this.action.Value = "verify";
						PageTitle.AddSiteNameTitle("验证绑定手机");
					}
					this.txtFirstPhone.SetWhenIsNotNull(Globals.GetSafeStr(user.CellPhone, 3, 4));
				}
				else
				{
					this.action.Value = "bind";
					PageTitle.AddSiteNameTitle("绑定手机");
				}
				if (string.IsNullOrEmpty(HiContext.Current.SiteSettings.SMSSettings) || !HiContext.Current.SiteSettings.SMSEnabled)
				{
					this.hidErrorMsg.Value = "商城未开启手机短信！";
				}
			}
		}
	}
}
