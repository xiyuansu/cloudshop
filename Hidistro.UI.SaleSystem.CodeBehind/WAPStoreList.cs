using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Helper;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Depot;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WAPStoreList : WAPTemplatedWebControl
	{
		private WapTemplatedRepeater rptTags;

		private Literal literlitNoMatchSwitchal;

		private Literal litBanner;

		private Literal litAddr;

		private HtmlInputHidden hdQQMapKey;

		private HtmlInputHidden hidIsReloadPosition;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-StoreList.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			base.CheckOpenMultStore();
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			MemberInfo user = HiContext.Current.User;
			if (user.UserId != 0 && user.IsReferral() && string.IsNullOrEmpty(this.Page.Request.QueryString["ReferralUserId"]))
			{
				string text = HttpContext.Current.Request.Url.ToString();
				text = ((text.IndexOf("?") <= -1) ? (text + "?ReferralUserId=" + HiContext.Current.UserId) : (text + "&ReferralUserId=" + HiContext.Current.UserId));
				this.Page.Response.Redirect(text);
			}
			else
			{
				this.literlitNoMatchSwitchal = (Literal)this.FindControl("litNoMatchSwitch");
				this.litAddr = (Literal)this.FindControl("litAddr");
				this.hdQQMapKey = (HtmlInputHidden)this.FindControl("hdQQMapKey");
				this.hidIsReloadPosition = (HtmlInputHidden)this.FindControl("hidIsReloadPosition");
				this.hdQQMapKey.Value = (string.IsNullOrEmpty(masterSettings.QQMapAPIKey) ? "SYJBZ-DSLR3-IWX3Q-3XNTM-ELURH-23FTP" : masterSettings.QQMapAPIKey);
				string cookie = WebHelper.GetCookie("UserCoordinateCookie");
				string cookie2 = WebHelper.GetCookie("UserCoordinateTimeCookie");
				this.litAddr.Text = HttpUtility.UrlDecode(WebHelper.GetCookie("UserCoordinateCookie", "Address").ToNullString());
				if (string.IsNullOrEmpty(cookie) || string.IsNullOrEmpty(cookie2))
				{
					this.litAddr.Text = "<img src=\"/templates/common/images/icon/ani_loading.png\" class=\"iconzhuan\" />";
					this.hidIsReloadPosition.Value = "1";
				}
				this.litBanner = (Literal)this.FindControl("litBanner");
				this.literlitNoMatchSwitchal.Text = masterSettings.Store_PositionNoMatchTo;
				if (this.Page.Request.Url.Query.ToLower().Contains("from"))
				{
					Dictionary<string, string> bannerList = StoreListHelper.GetBannerList();
					foreach (string key in bannerList.Keys)
					{
						Literal literal = this.litBanner;
						literal.Text += string.Format("<a href='{1}'><img src='{0}'  style=\"width:750px;height:280px\"></a>", key, bannerList[key]);
					}
					this.rptTags = (WapTemplatedRepeater)this.FindControl("rptTags");
					this.rptTags.DataSource = StoreListHelper.GetTagsList();
					this.rptTags.DataBind();
				}
			}
		}
	}
}
