using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Member;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class VLogin : WAPTemplatedWebControl
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

		private string unionId;

		private NameValueCollection parameters;

		private bool IsAuth = false;

		private string AuthMsg = "";

		private HtmlGenericControl fastlogin;

		private HtmlGenericControl fastlogin0;

		private HiddenField smsenable;

		private HiddenField emailenable;

		private HtmlInputText txtUserName;

		private HtmlInputText txtRegisterUserName;

		private HtmlAnchor forgetPassword;

		private HtmlInputHidden hidUnionId;

		private HtmlInputHidden hid_WxOpenIdBindUsers;

		private HtmlGenericControl divRealName;

		private HtmlGenericControl divBirthday;

		private HtmlGenericControl divSex;

		private HiddenField hidIsNeedValidateEmail;

		private HiddenField hidIsOpenGeetest;

		private HtmlInputHidden hid_IsValidationService;

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
			this.hidIsAuth = (HtmlInputHidden)this.FindControl("hid_IsAuth");
			this.hidAuthMsg = (HtmlInputHidden)this.FindControl("hid_AuthMsg");
			this.hidOpenIdType = (HtmlInputHidden)this.FindControl("hid_OpenIdType");
			this.hidOpenId = (HtmlInputHidden)this.FindControl("hid_OpenId");
			this.hidToken = (HtmlInputHidden)this.FindControl("hid_Token");
			this.hidRealName = (HtmlInputHidden)this.FindControl("hid_RealName");
			this.hidUserId = (HtmlInputHidden)this.FindControl("hid_UserId");
			this.hidEmail = (HtmlInputHidden)this.FindControl("hid_Email");
			this.openId = this.Page.Request.QueryString["openId"].ToNullString();
			this.hidOpenIdType.Value = "hishop.plugins.openid.weixin";
			this.client = (HtmlInputHidden)this.FindControl("client");
			this.hid_OpenIdINUsers = (HtmlInputHidden)this.FindControl("hid_OpenIdINUsers");
			this.client.Value = base.ClientType.ToString().ToLower();
			this.smsenable = (HiddenField)this.FindControl("smsenable");
			this.emailenable = (HiddenField)this.FindControl("emailenable");
			this.txtUserName = (HtmlInputText)this.FindControl("txtUserName");
			this.txtRegisterUserName = (HtmlInputText)this.FindControl("txtRegisterUserName");
			this.forgetPassword = (HtmlAnchor)this.FindControl("forgetPassword");
			this.hidUnionId = (HtmlInputHidden)this.FindControl("hid_UnionId");
			this.hid_WxOpenIdBindUsers = (HtmlInputHidden)this.FindControl("hid_WxOpenIdBindUsers");
			this.divRealName = (HtmlGenericControl)this.FindControl("divRealName");
			this.divBirthday = (HtmlGenericControl)this.FindControl("divBirthday");
			this.hidIsNeedValidateEmail = (HiddenField)this.FindControl("hidIsNeedValidateEmail");
			this.divSex = (HtmlGenericControl)this.FindControl("divSex");
			this.labNickName = (Label)this.FindControl("labNickName");
			this.labNickName1 = (Label)this.FindControl("labNickName1");
			this.labNickName2 = (Label)this.FindControl("labNickName2");
			this.userPicture = (HtmlImage)this.FindControl("userPicture");
			this.hid_QuickLoginIsForceBindingMobbile = (HtmlInputHidden)this.FindControl("hid_QuickLoginIsForceBindingMobbile");
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			this.hid_QuickLoginIsForceBindingMobbile.Value = (masterSettings.QuickLoginIsForceBindingMobbile ? "true" : "false");
			this.hidIsNeedValidateEmail.Value = (masterSettings.IsNeedValidEmail ? "1" : "0");
			this.hidIsOpenGeetest = (HiddenField)this.FindControl("hidIsOpenGeetest");
			this.hidUnionId.Value = this.Page.Request.QueryString["UnionId"].ToNullString();
			this.hidIsOpenGeetest.Value = (masterSettings.IsOpenGeetest ? "1" : "0");
			this.hid_IsValidationService = (HtmlInputHidden)this.FindControl("hid_IsValidationService");
			this.hidRealName.Value = this.Page.Request.QueryString["nickName"].ToNullString();
			this.labNickName.Text = this.hidRealName.Value;
			this.labNickName1.Text = this.hidRealName.Value;
			this.labNickName2.Text = this.hidRealName.Value;
			string text = this.Page.Request.QueryString["headimage"].ToNullString();
			if (string.IsNullOrEmpty(text))
			{
				text = "/Templates/common/images/headerimg.png";
			}
			this.userPicture.Src = text;
			this.hid_IsValidationService.Value = masterSettings.IsValidationService.ToNullString();
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
			if (string.IsNullOrEmpty(this.openId))
			{
				this.openId = this.Page.Request.QueryString["OpenID"];
			}
			MemberInfo memberInfo = null;
			this.fastlogin = (HtmlGenericControl)this.FindControl("fastlogin");
			if (this.fastlogin != null)
			{
				this.fastlogin.Visible = false;
			}
			this.fastlogin0 = (HtmlGenericControl)this.FindControl("fastlogin0");
			if (this.fastlogin0 != null)
			{
				this.fastlogin0.Visible = false;
			}
			if (!string.IsNullOrEmpty(this.openId))
			{
				memberInfo = MemberProcessor.GetMemberByOpenId("hishop.plugins.openid.weixin", this.openId);
				if (memberInfo?.IsQuickLogin ?? false)
				{
					this.hid_OpenIdINUsers.Value = "true";
				}
				if (memberInfo != null)
				{
					this.hid_WxOpenIdBindUsers.Value = "true";
				}
				if (memberInfo == null || (memberInfo.IsQuickLogin && !memberInfo.IsLogined))
				{
					memberInfo = null;
					this.hidIsAuth.Value = "false";
					this.hidOpenId.Value = this.openId;
					this.hidRealName.Value = this.Page.Request.QueryString["nickName"].ToNullString();
				}
			}
			MemberInfo user = HiContext.Current.User;
			if (user.UserId != 0)
			{
				if ((from item in user.MemberOpenIds
				where item.OpenIdType.ToLower() == "hishop.plugins.openid.weixin"
				select item).Count() == 0 && memberInfo == null && !string.IsNullOrEmpty(this.openId))
				{
					MemberOpenIdInfo memberOpenIdInfo = new MemberOpenIdInfo();
					memberOpenIdInfo.UserId = user.UserId;
					memberOpenIdInfo.OpenIdType = "hishop.plugins.openid.weixin";
					memberOpenIdInfo.OpenId = this.openId;
					MemberProcessor.AddMemberOpenId(memberOpenIdInfo);
				}
				memberInfo = user;
			}
			if (memberInfo != null)
			{
				Users.SetCurrentUser(memberInfo.UserId, 365, true, false);
				this.Page.Response.Redirect("/Vshop/MemberCenter.aspx");
			}
			PageTitle.AddSiteNameTitle("登录");
		}
	}
}
