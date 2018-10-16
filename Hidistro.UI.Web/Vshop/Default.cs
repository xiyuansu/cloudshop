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

namespace Hidistro.UI.Web.Vshop
{
	public class Default : Page
	{
		protected HtmlGenericControl divReferralInfo;

		protected HtmlImage Referral_Logo;

		protected Literal Referral_ShopName;

		protected Literal litImageServerUrl;

		protected HtmlGenericControl topemptydiv;

		protected HtmlInputHidden Hidden1;

		protected HomePage H_Page;

		protected HtmlInputHidden hidPageTitle;

		protected HtmlInputHidden hdAppId;

		protected HtmlInputHidden hdLink;

		protected HtmlInputHidden hdQQMapKey;

		protected HtmlInputHidden hidIsOpenMultStore;

		protected HtmlInputHidden hidIsToPlatform;

		protected HtmlInputHidden hidHasPosition;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.litImageServerUrl.Text = $"<script language=\"javascript\" type=\"text/javascript\"> \r\n                                var ImageServerUrl=\"{Globals.GetImageServerUrl()}\";\r\n                            </script>";
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (base.IsPostBack)
			{
				return;
			}
			MemberInfo user = HiContext.Current.User;
			string a = this.Page.Request.QueryString["fromSource"];
			string store_PositionRouteTo;
			int num;
			if (masterSettings.OpenMultStore && a != "1" && a != "2")
			{
				store_PositionRouteTo = masterSettings.Store_PositionRouteTo;
				if (!store_PositionRouteTo.Equals("NearestStore"))
				{
					if ((store_PositionRouteTo.Equals("NearestStore") || store_PositionRouteTo.Equals("Platform")) && user.StoreId > 0)
					{
						num = (masterSettings.Store_IsMemberVisitBelongStore ? 1 : 0);
						goto IL_00cd;
					}
					num = 0;
				}
				else
				{
					num = 1;
				}
				goto IL_00cd;
			}
			goto IL_0118;
			IL_0118:
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
					string text = user2.Referral.BannerUrl;
					if (string.IsNullOrEmpty(text))
					{
						text = masterSettings.LogoUrl;
					}
					this.Referral_Logo.Src = text;
					this.Referral_ShopName.Text = user2.Referral.ShopName;
					this.hidPageTitle.Value = user2.Referral.ShopName;
				}
			}
			this.hidIsToPlatform.Value = masterSettings.Store_PositionRouteTo;
			string cookie = WebHelper.GetCookie("UserCoordinateCookie");
			string cookie2 = WebHelper.GetCookie("UserCoordinateTimeCookie");
			if (!string.IsNullOrEmpty(cookie) && !string.IsNullOrEmpty(cookie2))
			{
				this.hidHasPosition.Value = "1";
			}
			this.topemptydiv.Visible = masterSettings.IsOpenAppPromoteCoupons;
			this.hdQQMapKey.Value = (string.IsNullOrEmpty(masterSettings.QQMapAPIKey) ? "SYJBZ-DSLR3-IWX3Q-3XNTM-ELURH-23FTP" : masterSettings.QQMapAPIKey);
			this.hidIsOpenMultStore.Value = (masterSettings.OpenMultStore ? "1" : "0");
			if (user.UserId != 0 && user.IsReferral() && string.IsNullOrEmpty(this.Page.Request.QueryString["ReferralUserId"]))
			{
				string text2 = this.Page.Request.Url.ToString().ToLower();
				text2 = text2.Split('?')[0];
				string text3 = "";
				string text4 = "";
				string[] array = this.Page.Request.Url.Query.Split('&');
				foreach (string text5 in array)
				{
					if (!text5.ToLower().Contains("referraluserid"))
					{
						if (text5.ToLower().Contains("returnurl="))
						{
							text4 = text5;
						}
						else
						{
							text3 = text3 + text5.Replace("?", "") + "&";
						}
					}
				}
				if (text3 != "")
				{
					text3 = text3.TrimEnd('&');
					text2 = text2 + "?" + text3;
				}
				text2 = ((text2.IndexOf("?") > -1) ? (text2 + "&ReferralUserId=" + HiContext.Current.User.UserId) : (text2 + "?ReferralUserId=" + HiContext.Current.User.UserId));
				if (text4 != "")
				{
					text2 = text2 + "&" + text4;
				}
				this.Page.Response.Redirect(text2);
			}
			this.hdAppId.Value = masterSettings.WeixinAppId;
			string value = Globals.FullPath(HttpContext.Current.Request.Url.ToString());
			this.hdLink.Value = value;
			return;
			IL_00cd:
			if (num != 0)
			{
				this.Page.Response.Redirect("StoreHome.aspx");
			}
			else if (store_PositionRouteTo.Equals("StoreList"))
			{
				this.Page.Response.Redirect("StoreList?from");
			}
			goto IL_0118;
		}
	}
}
