using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Members;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Shopping;
using Hidistro.UI.Common.Controls;
using Hishop.Plugins;
using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class OpenIdEntry : HtmlTemplatedWebControl
	{
		private SmallStatusMessage Statuses;

		private TextBox txtUserName;

		private TextBox txtPassword;

		private TextBox txtPassword2;

		private TextBox txtNumber;

		private string verifyCodeKey = "VerifyCode";

		private TextBox txtBindUserName;

		private TextBox txtBindPassword;

		private LinkButton btnLogin;

		private IButton btnBindingLogin;

		private DropDownList ddlPlugins;

		private CheckBox chkSaveLoginInfo;

		private static string ReturnURL = string.Empty;

		private string openIdType;

		private string openId;

		private HiddenField openIdHF;

		private HiddenField HIGW;

		private HiddenField hidNickName;

		private HiddenField hidUnionId;

		private HiddenField hidHeadImage;

		private Literal nameTitle;

		private HiddenField hidIsValidateEmail;

		private HiddenField hidIsOpenGeetest;

		private HtmlGenericControl divRealName;

		private HtmlGenericControl divBirthday;

		private HtmlGenericControl divSex;

		private HtmlGenericControl divGeetest;

		private HtmlGenericControl divGeetest1;

		private HtmlGenericControl divimgcode;

		private HtmlGenericControl divimgcode1;

		private HtmlGenericControl divRealNametip;

		private HtmlGenericControl divBirthdaytip;

		private Label labNickName;

		private HtmlGenericControl userBindPanel;

		private HtmlGenericControl phoneBindPanel;

		private OpenIdNotify notify;

		private OpenIdSettingInfo settings;

		public NameValueCollection Parameters
		{
			get
			{
				return this.ViewState["parameters"] as NameValueCollection;
			}
			set
			{
				this.ViewState["parameters"] = value;
			}
		}

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-OpenIdEntry.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.Statuses = (SmallStatusMessage)this.FindControl("Statuses");
			this.txtBindUserName = (TextBox)this.FindControl("txtBindUserName");
			this.txtBindPassword = (TextBox)this.FindControl("txtBindPassword");
			this.btnLogin = (LinkButton)this.FindControl("btnLogin");
			this.btnBindingLogin = ButtonManager.Create(this.FindControl("btnBindingLogin"));
			this.ddlPlugins = (DropDownList)this.FindControl("ddlPlugins");
			this.chkSaveLoginInfo = (CheckBox)this.FindControl("chkSaveLoginInfo");
			this.openIdHF = (HiddenField)this.FindControl("openId");
			this.HIGW = (HiddenField)this.FindControl("HIGW");
			this.hidNickName = (HiddenField)this.FindControl("hidNickName");
			this.hidUnionId = (HiddenField)this.FindControl("hidUnionId");
			this.nameTitle = (Literal)this.FindControl("NameTitle");
			this.hidIsOpenGeetest = (HiddenField)this.FindControl("hidIsOpenGeetest");
			this.hidIsValidateEmail = (HiddenField)this.FindControl("hidIsValidateEmail");
			SiteSettings siteSettings = HiContext.Current.SiteSettings;
			this.hidIsValidateEmail.Value = (siteSettings.IsNeedValidEmail ? "1" : "0");
			this.hidIsOpenGeetest.Value = (siteSettings.IsOpenGeetest ? "1" : "0");
			this.divRealName = (HtmlGenericControl)this.FindControl("divRealName");
			this.divBirthday = (HtmlGenericControl)this.FindControl("divBirthday");
			this.divSex = (HtmlGenericControl)this.FindControl("divSex");
			this.divGeetest = (HtmlGenericControl)this.FindControl("divGeetest");
			this.divGeetest1 = (HtmlGenericControl)this.FindControl("divGeetest1");
			this.divimgcode = (HtmlGenericControl)this.FindControl("divimgcode");
			this.divimgcode1 = (HtmlGenericControl)this.FindControl("divimgcode1");
			this.divRealNametip = (HtmlGenericControl)this.FindControl("divRealNametip");
			this.divBirthdaytip = (HtmlGenericControl)this.FindControl("divBirthdaytip");
			this.labNickName = (Label)this.FindControl("labNickName");
			this.hidHeadImage = (HiddenField)this.FindControl("hidHeadImage");
			this.phoneBindPanel = (HtmlGenericControl)this.FindControl("phoneBindPanel");
			this.userBindPanel = (HtmlGenericControl)this.FindControl("userBindPanel");
			if (siteSettings.RegistExtendInfo.Contains("RealName"))
			{
				this.divRealName.Visible = true;
				this.divRealNametip.Visible = true;
			}
			else
			{
				this.divRealName.Visible = false;
				this.divRealNametip.Visible = false;
			}
			if (siteSettings.RegistExtendInfo.Contains("Sex"))
			{
				this.divSex.Visible = true;
			}
			if (siteSettings.RegistExtendInfo.Contains("Birthday"))
			{
				this.divBirthday.Visible = true;
				this.divBirthdaytip.Visible = true;
			}
			else
			{
				this.divBirthday.Visible = false;
				this.divBirthdaytip.Visible = false;
			}
			if (siteSettings.IsSurportEmail && siteSettings.IsSurportPhone && siteSettings.SMSEnabled && !string.IsNullOrEmpty(siteSettings.SMSSettings))
			{
				this.nameTitle.Text = "邮箱/手机：";
			}
			else if (!siteSettings.IsSurportEmail && (!siteSettings.IsSurportPhone || !siteSettings.SMSEnabled || string.IsNullOrEmpty(siteSettings.SMSSettings)))
			{
				this.nameTitle.Text = "邮箱：";
			}
			else if (siteSettings.IsSurportEmail && (!siteSettings.IsSurportPhone || !siteSettings.SMSEnabled || string.IsNullOrEmpty(siteSettings.SMSSettings)))
			{
				this.nameTitle.Text = "邮箱：";
			}
			else if (!siteSettings.IsSurportEmail && siteSettings.IsSurportPhone && siteSettings.SMSEnabled && !string.IsNullOrEmpty(siteSettings.SMSSettings))
			{
				this.nameTitle.Text = "手机：";
			}
			if (this.Page.Request.UrlReferrer != (Uri)null && !string.IsNullOrEmpty(this.Page.Request.UrlReferrer.OriginalString))
			{
				OpenIdEntry.ReturnURL = this.Page.Request.UrlReferrer.OriginalString;
			}
			this.txtBindUserName.Focus();
			this.openIdType = Globals.StripAllTags(base.GetParameter("HIGW", false));
			if (!string.IsNullOrEmpty(this.openIdType))
			{
				this.openIdType = this.openIdType.ToLower().Replace("_", ".");
			}
			PageTitle.AddSiteNameTitle("信任登录");
			this.btnLogin.Click += this.btnLogin_Click;
			this.btnBindingLogin.Click += this.btnBindingLogin_Click;
			if (!siteSettings.IsOpenGeetest)
			{
				this.divGeetest.Style.Add("display", "none");
				this.divimgcode.Style.Add("display", "block");
				this.divGeetest1.Style.Add("display", "none");
				this.divimgcode1.Style.Add("display", "block");
			}
			else
			{
				this.divGeetest.Style.Add("display", "block");
				this.divimgcode.Style.Add("display", "none");
				this.divGeetest1.Style.Add("display", "block");
				this.divimgcode1.Style.Add("display", "none");
			}
			if (!this.Page.IsPostBack)
			{
				if (siteSettings.QuickLoginIsForceBindingMobbile)
				{
					this.userBindPanel.Visible = false;
					this.phoneBindPanel.Visible = true;
				}
				else
				{
					this.phoneBindPanel.Visible = false;
					this.userBindPanel.Visible = true;
				}
				this.OpenIdEntryInit();
			}
		}

		private void btnBindingLogin_Click(object sender, EventArgs e)
		{
			string text = Globals.StripAllTags(this.Parameters["CurrentOpenId"].ToNullString());
			string text2 = Globals.StripAllTags(base.GetParameter("HIGW", false).ToNullString().Replace("_", "."));
			if (this.Page.IsValid)
			{
				string text3 = Globals.StripAllTags(this.txtBindUserName.Text.Trim());
				if (string.IsNullOrEmpty(text3))
				{
					this.ShowMessage("用户名不能为空", false, "", 1);
				}
				else
				{
					MemberInfo memberInfo = MemberProcessor.ValidLogin(text3, this.txtBindPassword.Text);
					if (memberInfo != null)
					{
						if (!string.IsNullOrEmpty(memberInfo.UnionId) && text2.ToLower() == "hishop.plugins.openid.weixin.weixinservice" && memberInfo.UnionId != text)
						{
							this.ShowMessage("该用户已存在信任登录绑定关系,请选择其它帐号", false, "", 1);
						}
						else
						{
							MemberInfo memberByOpenId = MemberProcessor.GetMemberByOpenId(text2, text);
							if (memberByOpenId != null && memberByOpenId.UserId != memberInfo.UserId)
							{
								this.ShowMessage("该用户已存在信任登录绑定关系,请选择其它帐号", false, "", 1);
							}
							else
							{
								if (string.IsNullOrEmpty(memberInfo.NickName))
								{
									memberInfo.NickName = this.hidNickName.Value;
									MemberProcessor.UpdateMember(memberInfo);
								}
								Users.SetCurrentUser(memberInfo.UserId, 0, true, true);
								HiContext.Current.User = memberInfo;
								ShoppingCartInfo cookieShoppingCart = ShoppingCartProcessor.GetCookieShoppingCart();
								if (cookieShoppingCart != null)
								{
									ShoppingCartProcessor.ConvertShoppingCartToDataBase(cookieShoppingCart);
									ShoppingCartProcessor.ClearCookieShoppingCart();
								}
								if (!string.IsNullOrEmpty(text2) && !string.IsNullOrEmpty(text))
								{
									if (text2.ToLower() == "hishop.plugins.openid.weixin.weixinservice" && MemberProcessor.GetMemberByUnionId(text) == null)
									{
										memberInfo.UnionId = text;
										MemberProcessor.UpdateMember(memberInfo);
									}
									MemberOpenIdInfo memberOpenIdInfo = new MemberOpenIdInfo();
									memberOpenIdInfo.UserId = memberInfo.UserId;
									memberOpenIdInfo.OpenIdType = text2;
									memberOpenIdInfo.OpenId = text;
									if (MemberProcessor.GetMemberByOpenId(memberOpenIdInfo.OpenIdType, memberOpenIdInfo.OpenId) == null)
									{
										MemberProcessor.AddMemberOpenId(memberOpenIdInfo);
									}
								}
								this.SetLoginState(memberInfo, 0);
							}
						}
					}
					else
					{
						this.ShowMessage("用户名或密码不正确", false, "", 1);
					}
				}
			}
		}

		protected void btnLogin_Click(object sender, EventArgs e)
		{
			switch (this.openIdType.ToLower())
			{
			case "hishop.plugins.openid.alipay.alipayservice":
				this.SkipAlipayOpenId();
				break;
			case "hishop.plugins.openid.qq.qqservice":
				this.SkipQQOpenId();
				break;
			case "hishop.plugins.openid.taobao.taobaoservice":
				this.SkipTaoBaoOpenId();
				break;
			case "hishop.plugins.openid.sina.sinaservice":
				this.SkipSinaOpenId();
				break;
			case "hishop.plugins.openid.weixin.weixinservice":
				this.SkipWeiXinOpenId();
				break;
			default:
				this.Page.Response.Redirect("/", true);
				break;
			}
		}

		private void SetLoginState(MemberInfo member, int iSendCouponCount = 0)
		{
			string text = Globals.StripAllTags(this.Parameters["CurrentOpenId"]);
			if (!string.IsNullOrEmpty(text))
			{
				MemberOpenIdInfo memberOpenIdInfo = MemberProcessor.GetMemberOpenIdInfo(member.UserId, this.openIdType);
				if (memberOpenIdInfo != null && memberOpenIdInfo.OpenId != text)
				{
					this.ShowMessage("该账号已被绑定,请绑定其他帐号。", false, "", 1);
					return;
				}
				MemberOpenIdInfo memberOpenId = MemberProcessor.GetMemberOpenId(this.openIdType, text);
				if (memberOpenId == null)
				{
					memberOpenId = new MemberOpenIdInfo();
					memberOpenId.UserId = member.UserId;
					memberOpenId.OpenIdType = this.openIdType;
					memberOpenId.OpenId = text;
					try
					{
						MemberProcessor.AddMemberOpenId(memberOpenId);
					}
					catch (Exception ex)
					{
						NameValueCollection param = new NameValueCollection
						{
							this.Page.Request.QueryString,
							this.Page.Request.Form
						};
						Globals.WriteExceptionLog_Page(ex, param, "AddMemberOpenId");
					}
				}
			}
			Users.SetCurrentUser(member.UserId, 0, true, true);
			HiContext.Current.User = member;
			ShoppingCartInfo cookieShoppingCart = ShoppingCartProcessor.GetCookieShoppingCart();
			if (cookieShoppingCart != null)
			{
				ShoppingCartProcessor.ConvertShoppingCartToDataBase(cookieShoppingCart);
				ShoppingCartProcessor.ClearCookieShoppingCart();
			}
			if (!string.IsNullOrEmpty(this.Parameters["token"]))
			{
				HttpCookie httpCookie = new HttpCookie("Token_" + member.UserId);
				httpCookie.HttpOnly = true;
				httpCookie.Expires = DateTime.Now.AddMinutes(30.0);
				httpCookie.Value = this.Parameters["token"];
				HttpContext.Current.Response.Cookies.Add(httpCookie);
			}
			if (!string.IsNullOrEmpty(this.Parameters["target_url"]))
			{
				this.Page.Response.Redirect(this.Parameters["target_url"], true);
			}
			string text2 = this.Parameters["HITO"];
			string text3 = this.Parameters["target_url"].ToNullString();
			if (string.IsNullOrEmpty(text3))
			{
				text3 = "/Default";
			}
			if (iSendCouponCount > 0)
			{
				this.ShowMessage("恭喜您注册成功，" + iSendCouponCount + " 张优惠券已经放入您的账户，可在会员中心我的优惠券中进行查看", true, text3, 1);
			}
			else
			{
				this.ShowMessage("恭喜您注册/登录成功", true, text3, 1);
			}
		}

		private void OpenIdEntryInit()
		{
			this.settings = MemberProcessor.GetOpenIdSettings(this.openIdType);
			if (this.settings == null)
			{
				this.ShowMessage("登录失败，没有找到对应的插件配置信息。", false, "/User/Login", 2);
			}
			else
			{
				this.Parameters = new NameValueCollection
				{
					this.Page.Request.Form,
					this.Page.Request.QueryString
				};
				this.notify = OpenIdNotify.CreateInstance(this.openIdType, this.Parameters);
				this.notify.Authenticated += this.Notify_Authenticated;
				this.notify.Failed += this.Notify_Failed;
				this.notify.Verify(30000, HiCryptographer.Decrypt(this.settings.Settings));
			}
		}

		private void Notify_Failed(object sender, FailedEventArgs e)
		{
			this.ShowMessage("登录失败，" + e.Message, false, "/User/Login", 2);
		}

		private void Notify_Authenticated(object sender, AuthenticatedEventArgs e)
		{
			this.openId = e.OpenId;
			this.Parameters.Add("CurrentOpenId", e.OpenId);
			this.openIdHF.Value = Globals.StripAllTags(this.Parameters["CurrentOpenId"]);
			this.HIGW.Value = this.openIdType;
			HiContext current = HiContext.Current;
			MemberInfo memberInfo = null;
			if (!string.IsNullOrEmpty(e.OpenId))
			{
				memberInfo = MemberProcessor.GetMemberByOpenId(this.openIdType, e.OpenId);
				if (memberInfo == null && this.openIdType.ToLower() == "hishop.plugins.openid.weixin.weixinservice")
				{
					memberInfo = MemberProcessor.GetMemberByUnionId(this.openId);
					this.hidUnionId.Value = this.openId;
				}
			}
			string text = string.Empty;
			switch (this.openIdType.ToLower())
			{
			case "hishop.plugins.openid.alipay.alipayservice":
				text = (string.IsNullOrEmpty(this.Parameters["real_name"]) ? string.Empty : this.Parameters["real_name"].Trim());
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
				if (httpCookie != null)
				{
					text = HttpUtility.UrlDecode(httpCookie.Value).Trim();
				}
				break;
			}
			default:
				this.Page.Response.Redirect("/", true);
				break;
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.btnLogin.Text = text + "," + this.btnLogin.Text;
				this.labNickName.Text = text;
				this.hidNickName.Value = text;
			}
			if (memberInfo != null)
			{
				this.SetLoginState(memberInfo, 0);
			}
		}

		private void GoTo()
		{
			string a = this.Parameters["HITO"];
			if (a == "1")
			{
				this.Page.Response.Redirect("/SubmmitOrder", true);
			}
			else
			{
				this.Page.Response.Redirect("/", true);
			}
		}

		protected void SkipAlipayOpenId()
		{
			MemberInfo memberInfo = new MemberInfo();
			if (HiContext.Current.ReferralUserId > 0)
			{
				memberInfo.ReferralUserId = HiContext.Current.ReferralUserId;
			}
			memberInfo.GradeId = MemberProcessor.GetDefaultMemberGrade();
			memberInfo.UserName = (string.IsNullOrEmpty(Globals.StripAllTags(this.Parameters["real_name"])) ? string.Empty : Globals.StripAllTags(this.Parameters["real_name"]).Trim());
			memberInfo.NickName = this.hidNickName.Value;
			if (string.IsNullOrEmpty(memberInfo.UserName))
			{
				memberInfo.UserName = "alipay" + Globals.StripAllTags(this.Parameters["user_id"]);
			}
			if (MemberProcessor.FindMemberByUsername(memberInfo.UserName) != null)
			{
				memberInfo.UserName = this.GenerateUsername(8);
				if (MemberProcessor.FindMemberByUsername(memberInfo.UserName) != null)
				{
					memberInfo.UserName = this.GenerateUsername();
					if (MemberProcessor.FindMemberByUsername(memberInfo.UserName) != null)
					{
						this.ShowMessage("为您创建随机账户时失败，请重试。", false, "", 1);
						return;
					}
				}
			}
			memberInfo.Email = Globals.StripAllTags(this.Parameters["email"]);
			string pass = this.GeneratePassword();
			string text = "Open";
			pass = (memberInfo.Password = Users.EncodePassword(pass, text));
			memberInfo.PasswordSalt = text;
			memberInfo.RegisteredSource = 1;
			memberInfo.CreateDate = DateTime.Now;
			int num = MemberProcessor.CreateMember(memberInfo);
			if (num <= 0)
			{
				this.ShowMessage("为您创建账户时失败，请重试。", false, "", 1);
			}
			else
			{
				int num2 = 0;
				memberInfo.UserId = num;
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				if (masterSettings.IsOpenGiftCoupons)
				{
					string[] array = masterSettings.GiftCouponList.Split(',');
					foreach (string obj in array)
					{
						if (obj.ToInt(0) > 0 && CouponHelper.AddCouponItemInfo(memberInfo, obj.ToInt(0)) == CouponActionStatus.Success)
						{
							num2++;
						}
					}
				}
				memberInfo.UserName = MemberHelper.GetUserName(memberInfo.UserId);
				MemberHelper.Update(memberInfo, true);
				HiContext.Current.User = memberInfo;
				this.SetLoginState(memberInfo, num2);
			}
		}

		protected void SkipQQOpenId()
		{
			MemberInfo memberInfo = new MemberInfo();
			if (HiContext.Current.ReferralUserId > 0)
			{
				memberInfo.ReferralUserId = HiContext.Current.ReferralUserId;
			}
			memberInfo.GradeId = MemberProcessor.GetDefaultMemberGrade();
			HttpCookie httpCookie = HttpContext.Current.Request.Cookies["NickName"];
			if (httpCookie != null)
			{
				memberInfo.UserName = Globals.StripAllTags(HttpUtility.UrlDecode(httpCookie.Value).Trim());
			}
			memberInfo.NickName = this.hidNickName.Value;
			if (string.IsNullOrEmpty(memberInfo.UserName))
			{
				memberInfo.UserName = "tencent" + this.GenerateUsername(8);
			}
			if (MemberProcessor.FindMemberByUsername(memberInfo.UserName) != null)
			{
				memberInfo.UserName = "tencent" + this.GenerateUsername(8);
				if (MemberProcessor.FindMemberByUsername(memberInfo.UserName) != null)
				{
					memberInfo.UserName = this.GenerateUsername();
					if (MemberProcessor.FindMemberByUsername(memberInfo.UserName) != null)
					{
						this.ShowMessage("为您创建随机账户时失败，请重试。", false, "", 1);
						return;
					}
				}
			}
			string pass = this.GeneratePassword();
			string text = "Open";
			pass = (memberInfo.Password = Users.EncodePassword(pass, text));
			memberInfo.PasswordSalt = text;
			memberInfo.RegisteredSource = 1;
			memberInfo.CreateDate = DateTime.Now;
			int num = MemberProcessor.CreateMember(memberInfo);
			if (num <= 0)
			{
				this.ShowMessage("为您创建随机账户时失败，请重试。", false, "", 1);
			}
			else
			{
				memberInfo.UserId = num;
				int num2 = 0;
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				if (masterSettings.IsOpenGiftCoupons)
				{
					string[] array = masterSettings.GiftCouponList.Split(',');
					foreach (string obj in array)
					{
						if (obj.ToInt(0) > 0 && CouponHelper.AddCouponItemInfo(memberInfo, obj.ToInt(0)) == CouponActionStatus.Success)
						{
							num2++;
						}
					}
				}
				memberInfo.UserName = MemberHelper.GetUserName(memberInfo.UserId);
				MemberHelper.Update(memberInfo, true);
				HiContext.Current.User = memberInfo;
				this.SetLoginState(memberInfo, num2);
			}
		}

		protected void SkipTaoBaoOpenId()
		{
			MemberInfo memberInfo = new MemberInfo();
			if (HiContext.Current.ReferralUserId > 0)
			{
				memberInfo.ReferralUserId = HiContext.Current.ReferralUserId;
			}
			memberInfo.GradeId = MemberProcessor.GetDefaultMemberGrade();
			HttpCookie httpCookie = HttpContext.Current.Request.Cookies["NickName"];
			if (httpCookie != null)
			{
				memberInfo.UserName = Globals.StripAllTags(HttpUtility.UrlDecode(httpCookie.Value).Trim());
			}
			memberInfo.NickName = this.hidNickName.Value;
			if (string.IsNullOrEmpty(memberInfo.UserName))
			{
				memberInfo.UserName = "taobao" + this.GenerateUsername(8);
			}
			if (MemberProcessor.FindMemberByUsername(memberInfo.UserName) != null)
			{
				memberInfo.UserName = "taobao" + this.GenerateUsername(8);
				if (MemberProcessor.FindMemberByUsername(memberInfo.UserName) != null)
				{
					memberInfo.UserName = this.GenerateUsername();
					if (MemberProcessor.FindMemberByUsername(memberInfo.UserName) != null)
					{
						this.ShowMessage("为您创建随机账户时失败，请重试。", false, "", 1);
						return;
					}
				}
			}
			string pass = this.GeneratePassword();
			string text = "Open";
			pass = (memberInfo.Password = Users.EncodePassword(pass, text));
			memberInfo.PasswordSalt = text;
			memberInfo.RegisteredSource = 1;
			memberInfo.CreateDate = DateTime.Now;
			int num = MemberProcessor.CreateMember(memberInfo);
			if (num <= 0)
			{
				this.ShowMessage("为您创建账户时失败，请重试。", false, "", 1);
			}
			int num2 = 0;
			memberInfo.UserId = num;
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (masterSettings.IsOpenGiftCoupons)
			{
				string[] array = masterSettings.GiftCouponList.Split(',');
				foreach (string obj in array)
				{
					if (obj.ToInt(0) > 0 && CouponHelper.AddCouponItemInfo(memberInfo, obj.ToInt(0)) == CouponActionStatus.Success)
					{
						num2++;
					}
				}
			}
			memberInfo.UserName = MemberHelper.GetUserName(memberInfo.UserId);
			MemberHelper.Update(memberInfo, true);
			HiContext.Current.User = memberInfo;
			this.SetLoginState(memberInfo, num2);
		}

		protected void SkipSinaOpenId()
		{
			MemberInfo memberInfo = new MemberInfo();
			if (HiContext.Current.ReferralUserId > 0)
			{
				memberInfo.ReferralUserId = HiContext.Current.ReferralUserId;
			}
			memberInfo.GradeId = MemberProcessor.GetDefaultMemberGrade();
			HttpCookie httpCookie = HttpContext.Current.Request.Cookies["SinaNickName"];
			if (httpCookie != null)
			{
				memberInfo.UserName = Globals.StripAllTags(HttpUtility.UrlDecode(httpCookie.Value).Trim());
			}
			memberInfo.NickName = this.hidNickName.Value;
			if (string.IsNullOrEmpty(memberInfo.UserName))
			{
				memberInfo.UserName = "sinaweibo" + this.GenerateUsername(8);
			}
			if (MemberProcessor.FindMemberByUsername(memberInfo.UserName) != null)
			{
				memberInfo.UserName = "sinaweibo" + this.GenerateUsername(8);
				if (MemberProcessor.FindMemberByUsername(memberInfo.UserName) != null)
				{
					memberInfo.UserName = this.GenerateUsername();
					if (MemberProcessor.FindMemberByUsername(memberInfo.UserName) != null)
					{
						this.ShowMessage("为您创建随机账户时失败，请重试。", false, "", 1);
						return;
					}
				}
			}
			string pass = this.GeneratePassword();
			string text = "Open";
			pass = (memberInfo.Password = Users.EncodePassword(pass, text));
			memberInfo.PasswordSalt = text;
			memberInfo.RegisteredSource = 1;
			memberInfo.CreateDate = DateTime.Now;
			int num = MemberProcessor.CreateMember(memberInfo);
			if (num <= 0)
			{
				this.ShowMessage("为您创建账户时失败，请重试。", false, "", 1);
			}
			else
			{
				int num2 = 0;
				memberInfo.UserId = num;
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				if (masterSettings.IsOpenGiftCoupons)
				{
					string[] array = masterSettings.GiftCouponList.Split(',');
					foreach (string obj in array)
					{
						if (obj.ToInt(0) > 0 && CouponHelper.AddCouponItemInfo(memberInfo, obj.ToInt(0)) == CouponActionStatus.Success)
						{
							num2++;
						}
					}
				}
				memberInfo.UserName = MemberHelper.GetUserName(memberInfo.UserId);
				MemberHelper.Update(memberInfo, true);
				HiContext.Current.User = memberInfo;
				this.SetLoginState(memberInfo, num2);
			}
		}

		protected void SkipWeiXinOpenId()
		{
			MemberInfo memberInfo = new MemberInfo();
			if (HiContext.Current.ReferralUserId > 0)
			{
				memberInfo.ReferralUserId = HiContext.Current.ReferralUserId;
			}
			memberInfo.UnionId = this.Parameters["CurrentOpenId"].ToNullString();
			memberInfo.GradeId = MemberProcessor.GetDefaultMemberGrade();
			HttpCookie httpCookie = HttpContext.Current.Request.Cookies["NickName"];
			if (httpCookie != null)
			{
				memberInfo.UserName = Globals.StripAllTags(HttpUtility.UrlDecode(httpCookie.Value).Trim());
			}
			memberInfo.NickName = this.hidNickName.Value;
			if (string.IsNullOrEmpty(memberInfo.UserName))
			{
				memberInfo.UserName = "weixin" + this.GenerateUsername(8);
			}
			if (MemberProcessor.FindMemberByUsername(memberInfo.UserName) != null)
			{
				memberInfo.UserName = "weixin" + this.GenerateUsername(8);
				if (MemberProcessor.FindMemberByUsername(memberInfo.UserName) != null)
				{
					memberInfo.UserName = this.GenerateUsername();
					if (MemberProcessor.FindMemberByUsername(memberInfo.UserName) != null)
					{
						this.ShowMessage("为您创建随机账户时失败，请重试。", false, "", 1);
						return;
					}
				}
			}
			string pass = this.GeneratePassword();
			string text = "Open";
			pass = (memberInfo.Password = Users.EncodePassword(pass, text));
			memberInfo.PasswordSalt = text;
			memberInfo.RegisteredSource = 1;
			memberInfo.CreateDate = DateTime.Now;
			memberInfo.IsLogined = false;
			memberInfo.IsQuickLogin = false;
			int num = MemberProcessor.CreateMember(memberInfo);
			if (num <= 0)
			{
				this.ShowMessage("为您创建账户时失败，请重试。", false, "", 1);
			}
			else
			{
				int num2 = 0;
				memberInfo.UserId = num;
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				if (masterSettings.IsOpenGiftCoupons)
				{
					string[] array = masterSettings.GiftCouponList.Split(',');
					foreach (string obj in array)
					{
						if (obj.ToInt(0) > 0 && CouponHelper.AddCouponItemInfo(memberInfo, obj.ToInt(0)) == CouponActionStatus.Success)
						{
							num2++;
						}
					}
				}
				memberInfo.UserName = MemberHelper.GetUserName(memberInfo.UserId);
				MemberHelper.Update(memberInfo, true);
				HiContext.Current.User = memberInfo;
				this.SetLoginState(memberInfo, num2);
			}
		}

		private void UserLogin(string userName, string password)
		{
			userName = Globals.StripAllTags(userName);
			MemberInfo memberInfo = MemberProcessor.ValidLogin(userName, password);
			if (memberInfo != null)
			{
				Users.SetCurrentUser(memberInfo.UserId, 0, true, true);
				ShoppingCartInfo cookieShoppingCart = ShoppingCartProcessor.GetCookieShoppingCart();
				HiContext.Current.User = memberInfo;
				if (cookieShoppingCart != null)
				{
					ShoppingCartProcessor.ConvertShoppingCartToDataBase(cookieShoppingCart);
					ShoppingCartProcessor.ClearCookieShoppingCart();
				}
			}
		}

		private string GenerateUsername(int length)
		{
			return this.GenerateRndString(length, "h");
		}

		private string GenerateUsername()
		{
			return this.GenerateRndString(10, "h");
		}

		private string GeneratePassword()
		{
			return this.GenerateRndString(8, "");
		}

		private string GenerateRndString(int length, string prefix)
		{
			string text = string.Empty;
			Random random = new Random();
			while (text.Length < 10)
			{
				int num = random.Next();
				text += ((char)((num % 3 != 0) ? ((ushort)(48 + (ushort)(num % 10))) : ((ushort)(97 + (ushort)(num % 26))))).ToString();
			}
			return prefix + text;
		}
	}
}
