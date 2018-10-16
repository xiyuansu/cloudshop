using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Shopping;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class Login : HtmlTemplatedWebControl
	{
		private TextBox txtUserName;

		private TextBox txtPassword;

		private IButton btnLogin;

		private DropDownList ddlPlugins;

		private CheckBox chkSaveLoginInfo;

		private HiddenField hidReturnUrl;

		private static string ReturnURL = string.Empty;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-Login.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			if (!string.IsNullOrEmpty(this.Page.Request["action"]) && this.Page.Request["action"] == "Common_UserLogin")
			{
				string text = this.UserLogin(this.Page.Request["username"], this.Page.Request["password"]);
				string text2 = string.IsNullOrEmpty(text) ? "Succes" : "Fail";
				if (text2 == "Succes")
				{
					string value = this.Page.Request["ckids"];
					HttpCookie cookie = new HttpCookie("ckids", value);
					HiContext.Current.Context.Response.Cookies.Add(cookie);
				}
				string text3 = "";
				this.Page.Response.Clear();
				this.Page.Response.ContentType = "application/json";
				text3 = "{\"Status\":\"" + text2 + "\",\"Msg\":\"" + text + "\"}";
				this.Page.Response.Write(text3);
				this.Page.Response.End();
			}
			this.hidReturnUrl = (HiddenField)this.FindControl("hidReturnUrl");
			this.txtUserName = (TextBox)this.FindControl("txtUserName");
			this.txtPassword = (TextBox)this.FindControl("txtPassword");
			this.btnLogin = ButtonManager.Create(this.FindControl("btnLogin"));
			this.ddlPlugins = (DropDownList)this.FindControl("ddlPlugins");
			this.chkSaveLoginInfo = (CheckBox)this.FindControl("chkSaveLoginInfo");
			if (this.ddlPlugins != null)
			{
				this.ddlPlugins.Items.Add(new ListItem("请选择登录方式", ""));
				IList<OpenIdSettingInfo> configedItems = MemberProcessor.GetConfigedItems();
				if (configedItems != null && configedItems.Count > 0)
				{
					foreach (OpenIdSettingInfo item in configedItems)
					{
						this.ddlPlugins.Items.Add(new ListItem(item.Name, item.OpenIdType));
					}
				}
				this.ddlPlugins.SelectedIndexChanged += this.ddlPlugins_SelectedIndexChanged;
			}
			if (this.Page.Request.UrlReferrer != (Uri)null && !string.IsNullOrEmpty(this.Page.Request.UrlReferrer.OriginalString))
			{
				Login.ReturnURL = this.Page.Request.UrlReferrer.OriginalString;
			}
			this.txtUserName.Focus();
			PageTitle.AddSiteNameTitle("用户登录");
			this.btnLogin.Click += this.btnLogin_Click;
		}

		private void ddlPlugins_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.ddlPlugins.SelectedValue.Length > 0)
			{
				this.Page.Response.Redirect("OpenId/RedirectLogin.aspx?ot=" + this.ddlPlugins.SelectedValue);
			}
		}

		protected void btnLogin_Click(object sender, EventArgs e)
		{
			if (this.Page.IsValid)
			{
				if (string.IsNullOrEmpty(this.txtUserName.Text.Trim()))
				{
					this.ShowMessage("用户名不能为空", false, "", 1);
				}
				else
				{
					string text = this.UserLogin(this.txtUserName.Text.Trim(), this.txtPassword.Text);
					if (!string.IsNullOrEmpty(text))
					{
						this.ShowMessage(text, false, "", 1);
					}
					else
					{
						string text2 = this.hidReturnUrl.Value;
						if (string.IsNullOrEmpty(text2))
						{
							text2 = base.GetParameter("ReturnUrl", false);
						}
						if (string.IsNullOrEmpty(text2) || this.IgnoreReturnUrl(text2))
						{
							text2 = "/User/UserDefault.aspx";
						}
						else if (string.IsNullOrEmpty(Login.ReturnURL))
						{
							text2 = Login.ReturnURL;
						}
						this.Page.Response.Redirect(text2);
					}
				}
			}
		}

		private bool IgnoreReturnUrl(string returnUrl)
		{
			string theUrl = returnUrl.Trim().ToLower();
			IList<string> list = new List<string>();
			list.Add("Register.aspx".ToLower().Trim());
			list.Add("ForgotPassword.aspx".ToLower().Trim());
			list.Add("RegisterAgreement.aspx".ToLower().Trim());
			return (from c in list
			where theUrl.Contains(c)
			select c).Count() == 1;
		}

		private string UserLogin(string userName, string password)
		{
			string result = string.Empty;
			userName = Globals.StripAllTags(userName);
			MemberInfo memberInfo = MemberProcessor.ValidLogin(userName, password);
			if (memberInfo != null)
			{
				Users.SetCurrentUser(memberInfo.UserId, (this.chkSaveLoginInfo != null && this.chkSaveLoginInfo.Checked) ? 7 : 0, true, true);
				HiContext.Current.User = memberInfo;
				ShoppingCartInfo cookieShoppingCart = ShoppingCartProcessor.GetCookieShoppingCart();
				if (cookieShoppingCart != null)
				{
					ShoppingCartProcessor.ConvertShoppingCartToDataBase(cookieShoppingCart);
					ShoppingCartProcessor.ClearCookieShoppingCart();
				}
			}
			else
			{
				result = "用户名或密码错误";
			}
			return result;
		}
	}
}
