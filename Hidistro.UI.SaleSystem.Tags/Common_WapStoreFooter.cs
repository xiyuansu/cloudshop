using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.UI.Common.Controls;
using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_WapStoreFooter : WAPTemplatedWebControl
	{
		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "tags/skin-Store_Footer.html";
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
			Literal literal = this.FindControl("ltlFooter") as Literal;
			if (literal != null)
			{
				string currentUrl = HttpContext.Current.Request.Url.ToString().ToLower();
				HtmlAnchor htmlAnchor = this.FindControl("WeixinGuideAttention") as HtmlAnchor;
				htmlAnchor.Visible = false;
				if (currentUrl.Contains("/vshop/default") || currentUrl.Contains("/vshop/groupbuyproductdetails") || currentUrl.Contains("/vshop/countdownproductsdetails") || currentUrl.Contains("/vshop/productdetails"))
				{
					SiteSettings masterSettings = SettingsManager.GetMasterSettings();
					if (masterSettings.WeixinGuideAttention && (HiContext.Current.UserId <= 0 || !HiContext.Current.User.IsSubscribe))
					{
						htmlAnchor.Visible = false;
						htmlAnchor.HRef = string.Format("/vshop/AttentionYDGZ.aspx");
					}
				}
				string empty = string.Empty;
				StringBuilder stringBuilder = new StringBuilder();
				string[] source = new string[2]
				{
					"/storehome",
					"/storemarketing"
				};
				string[] source2 = new string[3]
				{
					"/storelist",
					"/storelistquery",
					"/storelistsearch"
				};
				string text = 25 + "%";
				string store_PositionRouteTo = SettingsManager.GetMasterSettings().Store_PositionRouteTo;
				int num = this.Page.Request.QueryString["storeId"].ToInt(0);
				EnumStore_PositionRouteTo enumStore_PositionRouteTo;
				if (source.Any((string t) => currentUrl.Contains(t)))
				{
					if (currentUrl.Contains("/storehome"))
					{
						string a = store_PositionRouteTo;
						enumStore_PositionRouteTo = EnumStore_PositionRouteTo.StoreList;
						if (a == enumStore_PositionRouteTo.ToString())
						{
							text = 33 + "%";
							stringBuilder.Append("<li style='width:" + text + "'><div class=\"navcontent\" data=\"0\">");
							stringBuilder.Append("<a href=\"StoreList?from=\"><img src=\"/Storage/master/menu/icon_home_default.png\" style=\"width:20px;height:20px;\"/>首页</a>");
							stringBuilder.Append("</div></li>");
							Common_WapStoreFooter.BuildMenu(stringBuilder, text, false, num);
						}
						else
						{
							string a2 = store_PositionRouteTo;
							enumStore_PositionRouteTo = EnumStore_PositionRouteTo.NearestStore;
							if (a2 == enumStore_PositionRouteTo.ToString())
							{
								stringBuilder.Append("<li style='width:" + text + "'><div class=\"navcontent\" data=\"0\">");
								stringBuilder.Append("<a href=\"storehome.aspx?storeId=" + num + "\"><img src=\"/Storage/master/menu/icon_home_default.png\" style=\"width:20px;height:20px;\"/>首页</a>");
								stringBuilder.Append("</div></li>");
								Common_WapStoreFooter.BuildMenu(stringBuilder, text, true, num);
							}
							else
							{
								text = 33 + "%";
								stringBuilder.Append("<li style='width:" + text + "'><div class=\"navcontent\" data=\"0\">");
								stringBuilder.Append("<a href=\"default.aspx\"><img src=\"/Storage/master/menu/icon_home_default.png\" style=\"width:20px;height:20px;\"/>首页</a>");
								stringBuilder.Append("</div></li>");
								Common_WapStoreFooter.BuildMenu(stringBuilder, text, false, num);
							}
						}
					}
					else
					{
						stringBuilder.Append("<li style='width:" + text + "'><div class=\"navcontent\" data=\"0\">");
						stringBuilder.Append("<a href=\"storehome.aspx?storeId=" + num + "\"><img src=\"/Storage/master/menu/icon_home_default.png\" style=\"width:20px;height:20px;\"/>首页</a>");
						stringBuilder.Append("</div></li>");
						Common_WapStoreFooter.BuildMenu(stringBuilder, text, true, num);
					}
				}
				else if (source2.Any((string t) => currentUrl.Contains(t)))
				{
					string a3 = store_PositionRouteTo;
					enumStore_PositionRouteTo = EnumStore_PositionRouteTo.StoreList;
					if (a3 == enumStore_PositionRouteTo.ToString())
					{
						stringBuilder.Append("<li style='width:" + text + "'><div class=\"navcontent\" data=\"0\">");
						stringBuilder.Append("<a href=\"StoreList?from\"><img src=\"/Storage/master/menu/icon_home_default.png\" style=\"width:20px;height:20px;\"/>首页</a>");
						stringBuilder.Append("</div></li>");
					}
					else
					{
						stringBuilder.Append("<li style='width:" + text + "'><div class=\"navcontent\" data=\"0\">");
						stringBuilder.Append("<a href=\"Default.aspx\"><img src=\"/Storage/master/menu/icon_home_default.png\" style=\"width:20px;height:20px;\"/>首页</a>");
						stringBuilder.Append("</div></li>");
					}
					Common_WapStoreFooter.BuildMenu(stringBuilder, text, true, num);
				}
				else
				{
					stringBuilder.Append("<li style='width:" + text + "'><div class=\"navcontent\" data=\"0\">");
					stringBuilder.Append("<a href=\"Default.aspx\"><img src=\"/Storage/master/menu/icon_home_default.png\" style=\"width:20px;height:20px;\"/>首页</a>");
					stringBuilder.Append("</div></li>");
					Common_WapStoreFooter.BuildMenu(stringBuilder, text, true, num);
				}
				empty = stringBuilder.ToString();
				literal.Text = empty.ToString();
			}
		}

		private static void BuildMenu(StringBuilder listrs, string liwidth, bool addMyCenter, int storeid)
		{
			listrs.Append("<li style='width:" + liwidth + "'><div class=\"navcontent\" data=\"0\">");
			listrs.Append("<a href=\"ProductSearch.aspx?StoreId=" + storeid + "\"><img src=\"/Storage/master/menu/icon_searchcate_default.png\" style=\"width:20px;height:20px;\"/>分类</a>");
			listrs.Append("</div></li>");
			listrs.Append("<li style='width:" + liwidth + "'><div class=\"navcontent\" data=\"0\">");
			listrs.Append("<a href=\"ShoppingCart.aspx\"><img src=\"/Storage/master/menu/icon_cart_default.png\" style=\"width:20px;height:20px;\"/>购物车</a>");
			listrs.Append("</div></li>");
			if (addMyCenter)
			{
				listrs.Append("<li style='width:" + liwidth + "'><div class=\"navcontent\" data=\"0\">");
				listrs.Append("<a href=\"MemberCenter.aspx\"><img src=\"/Storage/master/menu/icon-member_default.png\" style=\"width:20px;height:20px;\"/>用户中心</a>");
				listrs.Append("</div></li>");
			}
		}
	}
}
