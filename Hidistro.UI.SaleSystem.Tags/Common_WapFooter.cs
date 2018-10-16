using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Helper;
using Hidistro.Entities;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_WapFooter : WAPTemplatedWebControl
	{
		public static string UserCoordinateCookieName = "UserCoordinateCookie";

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "tags/skin-Common_Footer.html";
			}
			string text = HttpContext.Current.Request.Url.ToString().ToLower();
			if (text.Contains("/vshop/"))
			{
				base.ClientType = ClientType.VShop;
			}
			else if (text.Contains("/alioh/"))
			{
				base.ClientType = ClientType.AliOH;
			}
			else
			{
				base.ClientType = ClientType.WAP;
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			string text = HttpContext.Current.Request.Url.ToString().ToLower();
			HtmlAnchor htmlAnchor = this.FindControl("WeixinGuideAttention") as HtmlAnchor;
			htmlAnchor.Visible = false;
			if (text.Contains("/vshop/default") || text.Contains("/vshop/groupbuyproductdetails") || text.Contains("/vshop/countdownproductsdetails") || text.Contains("/vshop/productdetails"))
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				if (masterSettings.WeixinGuideAttention && (HiContext.Current.UserId <= 0 || !HiContext.Current.User.IsSubscribe))
				{
					htmlAnchor.Visible = false;
					htmlAnchor.HRef = string.Format("/vshop/AttentionYDGZ.aspx");
				}
			}
			Literal literal = this.FindControl("ltlFooter") as Literal;
			object obj = HiCache.Get($"DataCache-FooterMenuCacheKey-{0}");
			string empty = string.Empty;
			if (obj == null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				IList<ShopMenuInfo> topMenus = ShopMenuHelper.GetTopMenus(0);
				if (topMenus.Count != 0)
				{
					string str = 100 / topMenus.Count + "%";
					foreach (ShopMenuInfo item in topMenus)
					{
						if (!item.Content.StartsWith("http://") && !item.Content.StartsWith("https://"))
						{
							if (!item.Content.StartsWith("/"))
							{
								item.Content = "/" + item.Content;
							}
							item.Content = item.Content.ToLower().Replace("/wapshop/", "").Replace("/vshop/", "")
								.Replace("/alioh/", "");
							switch (base.ClientType)
							{
							case ClientType.VShop:
								item.Content = "/Vshop/" + item.Content;
								break;
							case ClientType.WAP:
								item.Content = "/WapShop/" + item.Content;
								break;
							case ClientType.AliOH:
								item.Content = "/AliOH/" + item.Content;
								break;
							}
						}
						stringBuilder.Append("<li style='width:" + str + "'>");
						stringBuilder.Append("<div class=\"navcontent\" data=\"0\">");
						if (item.Content.ToLower().Contains("/default") && base.site.OpenMultStore)
						{
							string store_PositionRouteTo = base.site.Store_PositionRouteTo;
							if (!(store_PositionRouteTo == "NearestStore"))
							{
								if (store_PositionRouteTo == "StoreList")
								{
									stringBuilder.Append("<a href=\"" + item.Content.Substring(0, item.Content.LastIndexOf("/")) + "/storeList?from\">");
								}
								else
								{
									stringBuilder.Append("<a href=\"" + item.Content + "\">");
								}
							}
							else
							{
								stringBuilder.Append("<a href=\"" + item.Content.Substring(0, item.Content.LastIndexOf("/")) + "/StoreHome\">");
							}
						}
						else if (item.Content.ToLower().Contains("/productlist") && base.site.OpenMultStore)
						{
							int num = WebHelper.GetCookie(Common_WapFooter.UserCoordinateCookieName, "StoreId").ToInt(0);
							string store_PositionRouteTo = base.site.Store_PositionRouteTo;
							if (!(store_PositionRouteTo == "NearestStore"))
							{
								if (store_PositionRouteTo == "StoreList")
								{
									stringBuilder.Append("<a href=\"" + item.Content.Substring(0, item.Content.LastIndexOf("/")) + "/ProductSearch\">");
								}
								else
								{
									stringBuilder.Append("<a href=\"" + item.Content + "\">");
								}
							}
							else
							{
								stringBuilder.Append("<a href=\"" + item.Content.Substring(0, item.Content.LastIndexOf("/")) + "/ProductSearch?StoreId=" + num + "\">");
							}
						}
						else
						{
							stringBuilder.Append("<a href=\"" + item.Content + "\">");
						}
						if (!string.IsNullOrEmpty(item.ShopMenuPic))
						{
							stringBuilder.Append("<img src=\"" + item.ShopMenuPic + "\" style=\"width:20px;height:20px;\"/>");
						}
						stringBuilder.Append(item.Name);
						stringBuilder.Append("</a></div>");
						stringBuilder.Append("</li>");
					}
					empty = stringBuilder.ToString();
					HiCache.Insert($"DataCache-FooterMenuCacheKey-{(int)base.ClientType}", stringBuilder.ToString(), 1800);
					goto IL_052e;
				}
				return;
			}
			empty = (obj as string);
			goto IL_052e;
			IL_052e:
			literal.Text = empty.ToString();
		}
	}
}
