using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Member;
using Hidistro.UI.Common.Controls;
using Senparc.Weixin.MP.CommonAPIs;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_WAPGuanZhu : WAPTemplatedWebControl
	{
		private Image qRCode;

		private SiteSettings settings = HiContext.Current.SiteSettings;

		private HtmlInputHidden hidIsForceAttention;

		private bool IsSubscribe;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/Tags/Skin-Common_GuanZhu.html";
			}
			IDictionary<string, string> dictionary = new Dictionary<string, string>();
			MemberInfo user = HiContext.Current.User;
			dictionary.Add("UserId", (user == null) ? "0" : user.UserId.ToString());
			IDictionary<string, string> dictionary2 = dictionary;
			bool flag = this.settings.WeixinGuideAttention;
			dictionary2.Add("WeixinGuideAttention", flag.ToString());
			dictionary.Add("WeixinAppId", this.settings.WeixinAppId.ToString());
			dictionary.Add("WeixinAppSecret", this.settings.WeixinAppSecret.ToString());
			dictionary.Add("GetClientPath", HiContext.Current.GetClientPath.ToString());
			IDictionary<string, string> dictionary3 = dictionary;
			flag = this.settings.IsForceAttention;
			dictionary3.Add("IsForceAttention", flag.ToString());
			if ((this.settings.WeixinGuideAttention || this.settings.IsForceAttention) && !string.IsNullOrEmpty(this.settings.WeixinAppId) && !string.IsNullOrEmpty(this.settings.WeixinAppSecret) && HiContext.Current.GetClientPath == "VShop")
			{
				if (user == null || user.UserId == 0 || !user.IsSubscribe)
				{
					this.IsSubscribe = this.GetSubscribeCookie();
					dictionary.Add("CookieIsSubscribe", this.GetSubscribeCookie().ToNullString());
					if (!this.IsSubscribe || !user.IsSubscribe)
					{
						OAuthUserInfo oAuthUserInfo = base.GetOAuthUserInfo(false);
						this.IsSubscribe = oAuthUserInfo.IsAttention;
						if (user != null && user.UserId > 0 && user.IsSubscribe != oAuthUserInfo.IsAttention)
						{
							user.IsSubscribe = oAuthUserInfo.IsAttention;
							MemberProcessor.UpdateMember(user);
						}
						this.SetSubscribeCookie(this.IsSubscribe);
					}
					else
					{
						this.SetSubscribeCookie(this.IsSubscribe);
					}
				}
				else
				{
					IDictionary<string, string> dictionary4 = dictionary;
					flag = user.IsSubscribe;
					dictionary4.Add("IsSubscribe", flag.ToString());
					this.IsSubscribe = user.IsSubscribe;
					this.SetSubscribeCookie(user.IsSubscribe);
				}
			}
			if (this.settings.WeixinGuideAttention && !this.IsSubscribe && !string.IsNullOrEmpty(this.settings.WeixinAppId) && !string.IsNullOrEmpty(this.settings.WeixinAppSecret) && HiContext.Current.GetClientPath == "VShop")
			{
				base.Visible = true;
			}
			else
			{
				base.Visible = false;
			}
			base.OnInit(e);
		}

		private bool GetSubscribeCookie()
		{
			HttpCookie httpCookie = HttpContext.Current.Request.Cookies["Shop_NoLoginSubscribe"];
			return httpCookie?.Value.ToBool() ?? false;
		}

		private void SetSubscribeCookie(bool IsSubscribe)
		{
			HttpCookie httpCookie = new HttpCookie("Shop_NoLoginSubscribe");
			httpCookie.Value = IsSubscribe.ToString();
			httpCookie.Expires = DateTime.Now.AddMinutes(60.0);
			HttpContext.Current.Response.Cookies.Add(httpCookie);
		}

		protected override void AttachChildControls()
		{
			if (!this.IsSubscribe && this.settings.WeixinGuideAttention && !string.IsNullOrEmpty(this.settings.WeixinAppId) && !string.IsNullOrEmpty(this.settings.WeixinAppSecret) && HiContext.Current.GetClientPath == "VShop")
			{
				this.hidIsForceAttention = (HtmlInputHidden)this.FindControl("hidIsForceAttention");
				this.hidIsForceAttention.Value = this.settings.IsForceAttention.ToNullString().ToLower();
				this.qRCode = (Image)this.FindControl("QRCode");
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				string accesstoken = AccessTokenContainer.TryGetToken(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, false);
				string qRLIMITSTRSCENETicket = base.GetQRLIMITSTRSCENETicket(accesstoken, "referralregister", true);
				this.qRCode.ImageUrl = $"https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket={qRLIMITSTRSCENETicket}";
			}
		}
	}
}
