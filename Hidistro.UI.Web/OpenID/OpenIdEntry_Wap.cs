using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Urls;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Shopping;
using Hishop.Plugins;
using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.OpenID
{
	public class OpenIdEntry_Wap : Page
	{
		private string openIdType;

		private NameValueCollection parameters;

		protected HtmlForm form1;

		protected string GetParameter(string name)
		{
			return RouteConfig.GetParameter(this.Page, name, false);
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			this.openIdType = this.GetParameter("HIGW");
			if (!string.IsNullOrEmpty(this.openIdType))
			{
				this.openIdType = this.openIdType.ToLower().Replace("_", ".");
			}
			OpenIdSettingInfo openIdSettings = MemberProcessor.GetOpenIdSettings(this.openIdType);
			if (openIdSettings == null)
			{
				base.Response.Write("登录失败，没有找到对应的插件配置信息。");
			}
			else
			{
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
				catch
				{
					this.Page.Response.Redirect("/");
				}
			}
		}

		private void Notify_Failed(object sender, FailedEventArgs e)
		{
			base.Response.Write("登录失败，" + e.Message);
		}

		private void Notify_Authenticated(object sender, AuthenticatedEventArgs e)
		{
			this.parameters.Add("CurrentOpenId", e.OpenId);
			HiContext current = HiContext.Current;
			MemberInfo memberByOpenId = MemberProcessor.GetMemberByOpenId(this.openIdType, e.OpenId);
			if (memberByOpenId != null)
			{
				Users.SetCurrentUser(memberByOpenId.UserId, 30, true, false);
				HiContext.Current.User = memberByOpenId;
				ShoppingCartInfo cookieShoppingCart = ShoppingCartProcessor.GetCookieShoppingCart();
				if (cookieShoppingCart != null)
				{
					ShoppingCartProcessor.ConvertShoppingCartToDataBase(cookieShoppingCart);
					ShoppingCartProcessor.ClearCookieShoppingCart();
				}
				if (!string.IsNullOrEmpty(this.parameters["token"]))
				{
					HttpCookie httpCookie = new HttpCookie("Token_" + memberByOpenId.UserId);
					httpCookie.HttpOnly = true;
					httpCookie.Expires = DateTime.Now.AddMinutes(30.0);
					httpCookie.Value = this.parameters["token"];
					HttpContext.Current.Response.Cookies.Add(httpCookie);
				}
			}
			else
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
				default:
					this.Page.Response.Redirect("/");
					break;
				}
			}
			string a = this.parameters["HITO"];
			if (a == "1")
			{
				this.Page.Response.Redirect("/SubmmitOrder");
			}
			else
			{
				this.Page.Response.Redirect("/");
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
			memberInfo.UserName = this.parameters["real_name"];
			if (string.IsNullOrEmpty(memberInfo.UserName))
			{
				memberInfo.UserName = "alipay" + this.parameters["user_id"];
			}
			memberInfo.Email = this.parameters["email"];
			string pass = this.GeneratePassword();
			string text = "Open";
			pass = (memberInfo.Password = Users.EncodePassword(pass, text));
			memberInfo.PasswordSalt = text;
			memberInfo.RegisteredSource = 2;
			memberInfo.CreateDate = DateTime.Now;
			memberInfo.IsLogined = true;
			int num = MemberProcessor.CreateMember(memberInfo);
			if (num <= 0)
			{
				memberInfo.UserName = "alipay" + this.GenerateUsername(8);
				num = MemberProcessor.CreateMember(memberInfo);
				if (num <= 0)
				{
					memberInfo.UserName = this.GenerateUsername();
					num = MemberProcessor.CreateMember(memberInfo);
					if (num <= 0)
					{
						base.Response.Write("为您创建随机账户时失败，请重试。");
						return;
					}
				}
			}
			memberInfo.UserId = num;
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (masterSettings.IsOpenGiftCoupons)
			{
				int num2 = 0;
				string[] array = masterSettings.GiftCouponList.Split(',');
				foreach (string obj in array)
				{
					if (obj.ToInt(0) > 0 && CouponHelper.AddCouponItemInfo(memberInfo, obj.ToInt(0)) == CouponActionStatus.Success)
					{
						num2++;
					}
				}
				if (num2 > 0)
				{
					base.Response.Write("恭喜您注册成功，" + num2 + " 张优惠券已经放入您的账户，可在会员中心我的优惠券中进行查看");
				}
			}
			this.SetLoginState(memberInfo);
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
				memberInfo.UserName = HttpUtility.UrlDecode(httpCookie.Value);
			}
			if (string.IsNullOrEmpty(memberInfo.UserName))
			{
				memberInfo.UserName = "tencent" + this.GenerateUsername(8);
			}
			string pass = this.GeneratePassword();
			string text = "Open";
			pass = (memberInfo.Password = Users.EncodePassword(pass, text));
			memberInfo.PasswordSalt = text;
			memberInfo.RegisteredSource = 2;
			memberInfo.CreateDate = DateTime.Now;
			memberInfo.IsLogined = true;
			int num = MemberProcessor.CreateMember(memberInfo);
			if (num <= 0)
			{
				memberInfo.UserName = "tencent" + this.GenerateUsername(8);
				num = MemberProcessor.CreateMember(memberInfo);
				if (num <= 0)
				{
					memberInfo.UserName = this.GenerateUsername();
					num = MemberProcessor.CreateMember(memberInfo);
					if (num <= 0)
					{
						base.Response.Write("为您创建随机账户时失败，请重试。");
						return;
					}
				}
			}
			memberInfo.UserId = num;
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (masterSettings.IsOpenGiftCoupons)
			{
				int num2 = 0;
				string[] array = masterSettings.GiftCouponList.Split(',');
				foreach (string obj in array)
				{
					if (obj.ToInt(0) > 0 && CouponHelper.AddCouponItemInfo(memberInfo, obj.ToInt(0)) == CouponActionStatus.Success)
					{
						num2++;
					}
				}
				if (num2 > 0)
				{
					base.Response.Write("恭喜您注册成功，" + num2 + " 张优惠券已经放入您的账户，可在会员中心我的优惠券中进行查看");
				}
			}
			this.SetLoginState(memberInfo);
		}

		protected void SkipTaoBaoOpenId()
		{
			MemberInfo memberInfo = new MemberInfo();
			if (HiContext.Current.ReferralUserId > 0)
			{
				memberInfo.ReferralUserId = HiContext.Current.ReferralUserId;
			}
			memberInfo.GradeId = MemberProcessor.GetDefaultMemberGrade();
			string text = this.parameters["CurrentOpenId"];
			if (!string.IsNullOrEmpty(text))
			{
				memberInfo.UserName = HttpUtility.UrlDecode(text);
			}
			if (string.IsNullOrEmpty(memberInfo.UserName))
			{
				memberInfo.UserName = "taobao" + this.GenerateUsername(8);
			}
			string pass = this.GeneratePassword();
			string text2 = "Open";
			pass = (memberInfo.Password = Users.EncodePassword(pass, text2));
			memberInfo.PasswordSalt = text2;
			memberInfo.RegisteredSource = 2;
			memberInfo.CreateDate = DateTime.Now;
			memberInfo.IsLogined = true;
			int num = MemberProcessor.CreateMember(memberInfo);
			if (num <= 0)
			{
				memberInfo.UserName = "taobao" + this.GenerateUsername(8);
				MemberInfo memberInfo2 = memberInfo;
				MemberInfo memberInfo3 = memberInfo;
				string text6 = memberInfo2.Password = (memberInfo3.TradePassword = pass);
				num = MemberProcessor.CreateMember(memberInfo);
				if (num <= 0)
				{
					memberInfo.UserName = this.GenerateUsername();
					num = MemberProcessor.CreateMember(memberInfo);
					if (num <= 0)
					{
						base.Response.Write("为您创建随机账户时失败，请重试。");
						return;
					}
				}
			}
			memberInfo.UserId = num;
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (masterSettings.IsOpenGiftCoupons)
			{
				int num2 = 0;
				string[] array = masterSettings.GiftCouponList.Split(',');
				foreach (string obj in array)
				{
					if (obj.ToInt(0) > 0 && CouponHelper.AddCouponItemInfo(memberInfo, obj.ToInt(0)) == CouponActionStatus.Success)
					{
						num2++;
					}
				}
				if (num2 > 0)
				{
					base.Response.Write("恭喜您注册成功，" + num2 + " 张优惠券已经放入您的账户，可在会员中心我的优惠券中进行查看");
				}
			}
			this.SetLoginState(memberInfo);
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
				memberInfo.UserName = HttpUtility.UrlDecode(httpCookie.Value);
			}
			if (string.IsNullOrEmpty(memberInfo.UserName))
			{
				memberInfo.UserName = "sinaweibo" + this.GenerateUsername(8);
			}
			string pass = this.GeneratePassword();
			string text = "Open";
			pass = (memberInfo.Password = Users.EncodePassword(pass, text));
			memberInfo.PasswordSalt = text;
			memberInfo.RegisteredSource = 2;
			memberInfo.CreateDate = DateTime.Now;
			memberInfo.IsLogined = true;
			int num = MemberProcessor.CreateMember(memberInfo);
			if (num <= 0)
			{
				memberInfo.UserName = "sinaweibo" + this.GenerateUsername(9);
				num = MemberProcessor.CreateMember(memberInfo);
				if (num <= 0)
				{
					memberInfo.UserName = this.GenerateUsername();
					num = MemberProcessor.CreateMember(memberInfo);
					if (num <= 0)
					{
						base.Response.Write("为您创建随机账户时失败，请重试。");
						return;
					}
				}
			}
			memberInfo.UserId = num;
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (masterSettings.IsOpenGiftCoupons)
			{
				int num2 = 0;
				string[] array = masterSettings.GiftCouponList.Split(',');
				foreach (string obj in array)
				{
					if (obj.ToInt(0) > 0 && CouponHelper.AddCouponItemInfo(memberInfo, obj.ToInt(0)) == CouponActionStatus.Success)
					{
						num2++;
					}
				}
				if (num2 > 0)
				{
					base.Response.Write("恭喜您注册成功，" + num2 + " 张优惠券已经放入您的账户，可在会员中心我的优惠券中进行查看");
				}
			}
			this.SetLoginState(memberInfo);
		}

		private void SetLoginState(MemberInfo member)
		{
			string text = this.parameters["HIGW"];
			string openId = this.parameters["CurrentOpenId"];
			MemberOpenIdInfo memberOpenId = MemberProcessor.GetMemberOpenId(text, openId);
			if (memberOpenId == null)
			{
				memberOpenId = new MemberOpenIdInfo();
				memberOpenId.UserId = member.UserId;
				memberOpenId.OpenIdType = text;
				memberOpenId.OpenId = openId;
				MemberProcessor.AddMemberOpenId(memberOpenId);
			}
			Users.SetCurrentUser(member.UserId, 30, false, false);
			HiContext.Current.User = member;
			ShoppingCartInfo cookieShoppingCart = ShoppingCartProcessor.GetCookieShoppingCart();
			if (cookieShoppingCart != null)
			{
				ShoppingCartProcessor.ConvertShoppingCartToDataBase(cookieShoppingCart);
				ShoppingCartProcessor.ClearCookieShoppingCart();
			}
			if (!string.IsNullOrEmpty(this.parameters["token"]))
			{
				HttpCookie httpCookie = new HttpCookie("Token_" + HiContext.Current.UserId.ToString());
				httpCookie.HttpOnly = true;
				httpCookie.Expires = DateTime.Now.AddMinutes(30.0);
				httpCookie.Value = this.parameters["token"];
				HttpContext.Current.Response.Cookies.Add(httpCookie);
			}
			if (!string.IsNullOrEmpty(this.parameters["target_url"]))
			{
				this.Page.Response.Redirect(this.parameters["target_url"], true);
			}
			this.Page.Response.Redirect("/");
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
