using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Shopping;
using Hidistro.UI.Common.Controls;
using Hishop.Plugins;
using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WAPLogin : WAPTemplatedWebControl
	{
		private HtmlInputControl hidIsAuth;

		private HtmlInputControl hidAuthMsg;

		private HtmlInputControl hidOpenIdType;

		private HtmlInputControl hidOpenId;

		private HtmlInputControl hidToken;

		private HtmlInputControl hidRealName;

		private HtmlInputControl hidUserId;

		private HtmlInputControl hidEmail;

		private HtmlInputControl client;

		private HtmlInputControl hid_OpenIdINUsers;

		private string openIdType;

		private string openId;

		private NameValueCollection parameters;

		private bool IsAuth = false;

		private string AuthMsg = string.Empty;

		private HiddenField smsenable;

		private HiddenField emailenable;

		private HtmlInputText txtUserName;

		private HtmlInputText txtRegisterUserName;

		private HtmlAnchor forgetPassword;

		private HtmlGenericControl divRealName;

		private HtmlGenericControl divBirthday;

		private HtmlGenericControl divSex;

		private HiddenField hidIsNeedValidateEmail;

		private HiddenField hidIsOpenGeetest;

		private Label labNickName1;

		private Label labNickName2;

		private Label labNickName;

		private HtmlImage userPicture;

		private HtmlInputHidden hid_QuickLoginIsForceBindingMobbile;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-VLogin.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			string a = this.Page.Request["action"].ToNullString().ToLower();
			if (a == "register")
			{
				PageTitle.AddSiteNameTitle("登录");
			}
			else
			{
				PageTitle.AddSiteNameTitle("注册");
			}
			this.hidIsAuth = (HtmlInputHidden)this.FindControl("hid_IsAuth");
			this.hidAuthMsg = (HtmlInputHidden)this.FindControl("hid_AuthMsg");
			this.hidOpenIdType = (HtmlInputHidden)this.FindControl("hid_OpenIdType");
			this.hidOpenId = (HtmlInputHidden)this.FindControl("hid_OpenId");
			this.hidToken = (HtmlInputHidden)this.FindControl("hid_Token");
			this.hidRealName = (HtmlInputHidden)this.FindControl("hid_RealName");
			this.hidUserId = (HtmlInputHidden)this.FindControl("hid_UserId");
			this.hidEmail = (HtmlInputHidden)this.FindControl("hid_Email");
			this.hidIsNeedValidateEmail = (HiddenField)this.FindControl("hidIsNeedValidateEmail");
			this.hidIsOpenGeetest = (HiddenField)this.FindControl("hidIsOpenGeetest");
			this.client = (HtmlInputHidden)this.FindControl("client");
			this.client.Value = base.ClientType.ToString().ToLower();
			this.smsenable = (HiddenField)this.FindControl("smsenable");
			this.emailenable = (HiddenField)this.FindControl("emailenable");
			this.txtUserName = (HtmlInputText)this.FindControl("txtUserName");
			this.txtRegisterUserName = (HtmlInputText)this.FindControl("txtRegisterUserName");
			this.forgetPassword = (HtmlAnchor)this.FindControl("forgetPassword");
			this.divRealName = (HtmlGenericControl)this.FindControl("divRealName");
			this.divBirthday = (HtmlGenericControl)this.FindControl("divBirthday");
			this.divSex = (HtmlGenericControl)this.FindControl("divSex");
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			this.hidIsNeedValidateEmail.Value = (masterSettings.IsNeedValidEmail ? "1" : "0");
			this.hidIsOpenGeetest.Value = (masterSettings.IsOpenGeetest ? "1" : "0");
			this.labNickName = (Label)this.FindControl("labNickName");
			this.labNickName1 = (Label)this.FindControl("labNickName1");
			this.labNickName2 = (Label)this.FindControl("labNickName2");
			this.userPicture = (HtmlImage)this.FindControl("userPicture");
			this.hid_QuickLoginIsForceBindingMobbile = (HtmlInputHidden)this.FindControl("hid_QuickLoginIsForceBindingMobbile");
			this.hid_QuickLoginIsForceBindingMobbile.Value = (masterSettings.QuickLoginIsForceBindingMobbile ? "true" : "false");
			if (masterSettings.RegistExtendInfo.Contains("RealName"))
			{
				this.divRealName.Visible = true;
			}
			if (masterSettings.RegistExtendInfo.Contains("Sex"))
			{
				this.divSex.Visible = true;
			}
			if (masterSettings.RegistExtendInfo.Contains("Birthday"))
			{
				this.divBirthday.Visible = true;
			}
			if (masterSettings.IsSurportEmail && masterSettings.IsSurportPhone && masterSettings.SMSEnabled && !string.IsNullOrEmpty(masterSettings.SMSSettings))
			{
				this.smsenable.Value = "true";
				this.emailenable.Value = "true";
				this.txtUserName.Attributes.Add("placeholder", "请输入用户名/邮箱/手机");
				this.txtRegisterUserName.Attributes.Add("placeholder", "邮箱/手机");
				this.forgetPassword.Visible = true;
			}
			else if (!masterSettings.IsSurportEmail && (!masterSettings.IsSurportPhone || !masterSettings.SMSEnabled || string.IsNullOrEmpty(masterSettings.SMSSettings)))
			{
				this.smsenable.Value = "false";
				this.emailenable.Value = "true";
				this.txtUserName.Attributes.Add("placeholder", "请输入用户名/邮箱");
				this.txtRegisterUserName.Attributes.Add("placeholder", "邮箱");
			}
			else if (masterSettings.IsSurportEmail && (!masterSettings.IsSurportPhone || !masterSettings.SMSEnabled || string.IsNullOrEmpty(masterSettings.SMSSettings)))
			{
				this.smsenable.Value = "false";
				this.emailenable.Value = "true";
				this.txtUserName.Attributes.Add("placeholder", "请输入用户名/邮箱");
				this.txtRegisterUserName.Attributes.Add("placeholder", "邮箱");
			}
			else if (!masterSettings.IsSurportEmail && masterSettings.IsSurportPhone && masterSettings.SMSEnabled && !string.IsNullOrEmpty(masterSettings.SMSSettings))
			{
				this.smsenable.Value = "true";
				this.emailenable.Value = "false";
				this.txtUserName.Attributes.Add("placeholder", "请输入用户名/手机");
				this.txtRegisterUserName.Attributes.Add("placeholder", "手机");
			}
			this.openIdType = base.GetParameter("HIGW");
			if (!string.IsNullOrEmpty(this.openIdType))
			{
				this.openIdType = this.openIdType.ToLower().Replace("_", ".");
				OpenIdSettingInfo openIdSettings = MemberProcessor.GetOpenIdSettings(this.openIdType);
				if (openIdSettings == null)
				{
					this.AuthMsg = "没有找到对应的插件配置信息。";
				}
				else
				{
					this.IsAuth = true;
					this.parameters = new NameValueCollection
					{
						this.Page.Request.Form,
						this.Page.Request.QueryString
					};
					OpenIdNotify openIdNotify = OpenIdNotify.CreateInstance(this.openIdType, this.parameters);
					openIdNotify.Authenticated += this.Notify_Authenticated;
					openIdNotify.Failed += this.Notify_Failed;
					try
					{
						openIdNotify.Verify(30000, HiCryptographer.Decrypt(openIdSettings.Settings));
					}
					catch (Exception ex)
					{
						this.IsAuth = false;
						this.AuthMsg = ex.Message;
					}
					if (this.hidIsAuth != null)
					{
						this.hidIsAuth.Value = this.IsAuth.ToString();
					}
					if (this.hidAuthMsg != null)
					{
						this.hidAuthMsg.Value = this.AuthMsg;
					}
					if (this.hidOpenId != null)
					{
						this.hidOpenId.Value = this.openId;
					}
					if (this.hidOpenIdType != null)
					{
						this.hidOpenIdType.Value = this.openIdType;
					}
				}
			}
		}

		private void Notify_Failed(object sender, FailedEventArgs e)
		{
			this.AuthMsg = "登录失败，" + e.Message;
		}

		private void Notify_Authenticated(object sender, AuthenticatedEventArgs e)
		{
			bool flag = false;
			if (this.hidToken != null)
			{
				this.hidToken.Value = Globals.StripAllTags(this.parameters["token"]);
			}
			if (this.hidUserId != null)
			{
				this.hidUserId.Value = Globals.StripAllTags(this.parameters["user_id"]);
			}
			if (this.hidEmail != null)
			{
				this.hidEmail.Value = Globals.StripAllTags(this.parameters["email"]);
			}
			string text = "";
			switch (this.openIdType.ToLower())
			{
			case "hishop.plugins.openid.alipay.alipayservice":
				text = (string.IsNullOrEmpty(this.parameters["real_name"]) ? string.Empty : this.parameters["real_name"].ToNullString().Trim());
				break;
			case "hishop.plugins.openid.qq.qqservice":
			{
				HttpCookie httpCookie3 = HttpContext.Current.Request.Cookies["NickName"];
				if (httpCookie3 != null)
				{
					text = HttpUtility.UrlDecode(httpCookie3.Value).Trim();
				}
				break;
			}
			case "hishop.plugins.openid.taobao.taobaoservice":
			{
				HttpCookie httpCookie4 = HttpContext.Current.Request.Cookies["NickName"];
				if (httpCookie4 != null)
				{
					text = HttpUtility.UrlDecode(httpCookie4.Value).Trim();
				}
				break;
			}
			case "hishop.plugins.openid.sina.sinaservice":
			{
				HttpCookie httpCookie2 = HttpContext.Current.Request.Cookies["SinaNickName"];
				if (httpCookie2 != null)
				{
					text = HttpUtility.UrlDecode(httpCookie2.Value).Trim();
				}
				break;
			}
			case "hishop.plugins.openid.weixin.weixinservice":
			{
				HttpCookie httpCookie = HttpContext.Current.Request.Cookies["NickName"];
				if (text != null)
				{
					text = HttpUtility.UrlDecode(httpCookie.Value).Trim();
				}
				break;
			}
			default:
				this.Page.Response.Redirect("/", true);
				break;
			}
			this.hidRealName.Value = text;
			this.labNickName.Text = text;
			this.labNickName1.Text = text;
			this.labNickName2.Text = text;
			string text2 = this.Page.Request.QueryString["headimage"].ToNullString();
			if (string.IsNullOrEmpty(text2))
			{
				text2 = "/Templates/common/images/headerimg.png";
			}
			this.userPicture.Src = text2;
			this.parameters.Add("CurrentOpenId", e.OpenId);
			HiContext current = HiContext.Current;
			this.openId = e.OpenId;
			if (!string.IsNullOrEmpty(this.openId))
			{
				HttpCookie httpCookie5 = new HttpCookie("openId");
				httpCookie5.HttpOnly = true;
				httpCookie5.Value = this.openId;
				httpCookie5.Expires = DateTime.MaxValue;
				HttpContext.Current.Response.Cookies.Add(httpCookie5);
			}
			MemberInfo memberByOpenId = MemberProcessor.GetMemberByOpenId(this.openIdType, this.openId);
			if (memberByOpenId != null)
			{
				Users.SetCurrentUser(memberByOpenId.UserId, 1, true, false);
				ShoppingCartInfo cookieShoppingCart = ShoppingCartProcessor.GetCookieShoppingCart();
				current.User = memberByOpenId;
				if (cookieShoppingCart != null)
				{
					ShoppingCartProcessor.ConvertShoppingCartToDataBase(cookieShoppingCart);
					ShoppingCartProcessor.ClearCookieShoppingCart();
				}
				if (!string.IsNullOrEmpty(this.parameters["token"]))
				{
					HttpCookie httpCookie6 = new HttpCookie("Token_" + HiContext.Current.UserId.ToString());
					httpCookie6.HttpOnly = true;
					httpCookie6.Expires = DateTime.Now.AddMinutes(30.0);
					httpCookie6.Value = Globals.StripAllTags(this.parameters["token"]);
					HttpContext.Current.Response.Cookies.Add(httpCookie6);
				}
			}
			else
			{
				this.AuthMsg = "Auth_Sucess";
			}
		}
	}
}
