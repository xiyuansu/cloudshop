using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Helper;
using Hidistro.Entities.Members;
using Hidistro.UI.SaleSystem.CodeBehind;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web
{
	public class Default : Page
	{
		public SiteSettings siteSettings = SettingsManager.GetMasterSettings();

		protected HtmlGenericControl divReferralInfo;

		protected HtmlImage Referral_Logo;

		protected Literal Referral_ShopName;

		protected Literal litImageServerUrl;

		protected HtmlInputHidden hidPageTitle;

		protected HtmlInputHidden hdQQMapKey;

		protected HtmlInputHidden hidIsOpenMultStore;

		protected HtmlInputHidden hidIsToPlatform;

		protected HtmlInputHidden hidHasPosition;

		protected HtmlGenericControl topemptydiv;

		protected HomePage H_Page;

		protected void Page_Load(object sender, EventArgs e)
		{
			string text = HttpContext.Current.Request.Url.ToString().ToLower();
			string text2 = HttpContext.Current.Request.UserAgent;
			this.topemptydiv.Visible = this.siteSettings.IsOpenAppPromoteCoupons;
			if (string.IsNullOrEmpty(text2))
			{
				text2 = "";
			}
			bool flag = this.Page.Request.QueryString["source"].ToNullString() != "";
			if (this.siteSettings.AutoRedirectClient && text.IndexOf("/wapshop/") != -1 && text2.ToLower().IndexOf("micromessenger") > -1 && this.siteSettings.OpenVstore == 1)
			{
				string text3 = HttpContext.Current.Request.Url.ToString().ToLower().Replace("/wapshop/", "/vShop/");
				if (!flag)
				{
					text3 = text3 + (string.IsNullOrEmpty(HttpContext.Current.Request.Url.Query) ? "?" : "&") + "source=wap";
				}
				this.Page.Response.Redirect(text3, true);
				return;
			}
			this.litImageServerUrl.Text = $"<script language=\"javascript\" type=\"text/javascript\"> \r\n                                var ImageServerUrl=\"{Globals.GetImageServerUrl()}\";\r\n                            </script>";
			if (base.IsPostBack)
			{
				return;
			}
			MemberInfo user = HiContext.Current.User;
			string a = this.Page.Request.QueryString["fromSource"];
			string store_PositionRouteTo;
			int num;
			if (this.siteSettings.OpenMultStore && a != "1" && a != "2")
			{
				store_PositionRouteTo = this.siteSettings.Store_PositionRouteTo;
				if (!store_PositionRouteTo.Equals("NearestStore"))
				{
					if ((store_PositionRouteTo.Equals("NearestStore") || store_PositionRouteTo.Equals("Platform")) && user.StoreId > 0)
					{
						num = (this.siteSettings.Store_IsMemberVisitBelongStore ? 1 : 0);
						goto IL_022c;
					}
					num = 0;
				}
				else
				{
					num = 1;
				}
				goto IL_022c;
			}
			goto IL_0277;
			IL_022c:
			if (num != 0)
			{
				this.Page.Response.Redirect("StoreHome.aspx");
			}
			else if (store_PositionRouteTo.Equals("StoreList"))
			{
				this.Page.Response.Redirect("StoreList?from");
			}
			goto IL_0277;
			IL_0277:
			int num2 = base.Request.QueryString["ReferralUserId"].ToInt(0);
			if (num2 == 0)
			{
				num2 = HiContext.Current.ReferralUserId;
			}
			if (num2 > 0)
			{
				MemberInfo user2 = Users.GetUser(num2);
				if (user2?.IsReferral() ?? false)
				{
					this.divReferralInfo.Visible = true;
					string text4 = user2.Referral.BannerUrl;
					if (string.IsNullOrEmpty(text4))
					{
						text4 = this.siteSettings.LogoUrl;
					}
					this.Referral_Logo.Src = text4;
					this.Referral_ShopName.Text = user2.Referral.ShopName;
					this.hidPageTitle.Value = user2.Referral.ShopName;
				}
			}
			this.hidIsToPlatform.Value = this.siteSettings.Store_PositionRouteTo;
			string cookie = WebHelper.GetCookie("UserCoordinateCookie");
			string cookie2 = WebHelper.GetCookie("UserCoordinateTimeCookie");
			if (!string.IsNullOrEmpty(cookie) && !string.IsNullOrEmpty(cookie2))
			{
				this.hidHasPosition.Value = "1";
			}
			this.hdQQMapKey.Value = (string.IsNullOrEmpty(this.siteSettings.QQMapAPIKey) ? "SYJBZ-DSLR3-IWX3Q-3XNTM-ELURH-23FTP" : this.siteSettings.QQMapAPIKey);
			this.hidIsOpenMultStore.Value = (this.siteSettings.OpenMultStore ? "1" : "0");
			if (user.UserId != 0 && user.IsReferral() && string.IsNullOrEmpty(this.Page.Request.QueryString["ReferralUserId"]))
			{
				string text5 = this.Page.Request.Url.ToString().ToLower();
				text5 = text5.Split('?')[0];
				string text6 = "";
				string text7 = "";
				string[] array = this.Page.Request.Url.Query.Split('&');
				foreach (string text8 in array)
				{
					if (!text8.ToLower().Contains("referraluserid"))
					{
						if (text8.ToLower().Contains("returnurl="))
						{
							text7 = text8;
						}
						else
						{
							text6 = text6 + text8.Replace("?", "") + "&";
						}
					}
				}
				if (text6 != "")
				{
					text6 = text6.TrimEnd('&');
					text5 = text5 + "?" + text6;
				}
				text5 = ((text5.IndexOf("?") > -1) ? (text5 + "&ReferralUserId=" + HiContext.Current.User.UserId) : (text5 + "?ReferralUserId=" + HiContext.Current.User.UserId));
				if (text7 != "")
				{
					text5 = text5 + "&" + text7;
				}
				this.Page.Response.Redirect(text5);
			}
		}
	}
}
