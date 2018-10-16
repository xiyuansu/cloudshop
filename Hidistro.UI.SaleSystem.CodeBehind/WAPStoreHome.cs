using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Helper;
using Hidistro.Entities;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem;
using Hidistro.SaleSystem.Depot;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WAPStoreHome : WAPTemplatedWebControl
	{
		private int storeId = 0;

		private HtmlImage storeLogo;

		private Literal litAddress;

		private Literal litStoreName;

		private Literal litStoreName2;

		private Literal litOpenDate;

		private Literal litStoreDelive;

		private Literal litActivityList;

		private Literal litActivityCount;

		private HtmlAnchor aTel;

		private Literal litStoreTel;

		private HtmlInputHidden hidLatitude;

		private HtmlInputHidden hidLongitude;

		private HtmlInputHidden hidIsOpenData;

		private HtmlInputHidden hdQQMapKey;

		private HtmlInputHidden hidStoreName;

		private HtmlInputHidden hidIsReloadPosition;

		private HtmlInputHidden hdAppId;

		private HtmlInputHidden hdTitle;

		private HtmlInputHidden hdDesc;

		private HtmlInputHidden hdImgUrl;

		private HtmlInputHidden hdLink;

		private Repeater rp_markting;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-StoreHome.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			base.CheckOpenMultStore();
			this.storeLogo = (HtmlImage)this.FindControl("storeLogo");
			this.litAddress = (Literal)this.FindControl("litAddress");
			this.litStoreName = (Literal)this.FindControl("litStoreName");
			this.litStoreName2 = (Literal)this.FindControl("litStoreName2");
			this.litOpenDate = (Literal)this.FindControl("litOpenDate");
			this.litStoreDelive = (Literal)this.FindControl("litStoreDelive");
			this.litActivityList = (Literal)this.FindControl("litActivityList");
			this.litActivityCount = (Literal)this.FindControl("litActivityCount");
			this.hidLatitude = (HtmlInputHidden)this.FindControl("hidLatitude");
			this.hidLongitude = (HtmlInputHidden)this.FindControl("hidLongitude");
			this.hidStoreName = (HtmlInputHidden)this.FindControl("hidStoreName");
			this.hidIsOpenData = (HtmlInputHidden)this.FindControl("hidIsOpenData");
			this.hdQQMapKey = (HtmlInputHidden)this.FindControl("hdQQMapKey");
			this.rp_markting = (Repeater)this.FindControl("rp_markting");
			int.TryParse(this.Page.Request.QueryString["storeId"], out this.storeId);
			int num = this.Page.Request.QueryString["storeSource"].ToInt(0);
			string cookie = WebHelper.GetCookie("UserCoordinateCookie", "Coordinate");
			this.hidIsReloadPosition = (HtmlInputHidden)this.FindControl("hidIsReloadPosition");
			this.aTel = (HtmlAnchor)this.FindControl("aTel");
			this.litStoreTel = (Literal)this.FindControl("litStoreTel");
			this.hdAppId = (HtmlInputHidden)this.FindControl("hdAppId");
			this.hdTitle = (HtmlInputHidden)this.FindControl("hdTitle");
			this.hdDesc = (HtmlInputHidden)this.FindControl("hdDesc");
			this.hdImgUrl = (HtmlInputHidden)this.FindControl("hdImgUrl");
			this.hdLink = (HtmlInputHidden)this.FindControl("hdLink");
			this.hdAppId.Value = HiContext.Current.SiteSettings.WeixinAppId;
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (!masterSettings.OpenMultStore)
			{
				this.Page.Response.Redirect("Default.aspx");
			}
			else if (masterSettings.Store_PositionRouteTo == 2.ToString() && num != 3 && num != 2 && num != 1 && num != 4)
			{
				this.Page.Response.Redirect("StoreList?from");
			}
			this.hdQQMapKey.Value = (string.IsNullOrEmpty(masterSettings.QQMapAPIKey) ? "SYJBZ-DSLR3-IWX3Q-3XNTM-ELURH-23FTP" : masterSettings.QQMapAPIKey);
			string cookie2 = WebHelper.GetCookie("UserCoordinateTimeCookie");
			if (this.storeId > 0 && !string.IsNullOrWhiteSpace(cookie) && !string.IsNullOrEmpty(cookie2))
			{
				StoresInfo storeById = StoresHelper.GetStoreById(this.storeId);
				if (storeById != null && storeById.StoreId > 0)
				{
					this.hdTitle.Value = storeById.StoreName;
					this.hdDesc.Value = storeById.StoreName;
					string storeImages = storeById.StoreImages;
					string local = string.IsNullOrEmpty(storeImages) ? SettingsManager.GetMasterSettings().LogoUrl : storeImages;
					this.hdImgUrl.Value = Globals.FullPath(local);
					this.hdLink.Value = Globals.FullPath(this.Page.Request.Url.ToString());
					MemberInfo user = HiContext.Current.User;
					if (user.UserId != 0 && user.IsReferral() && string.IsNullOrEmpty(this.Page.Request.QueryString["ReferralUserId"]))
					{
						string text = HttpContext.Current.Request.Url.ToString();
						text = ((text.IndexOf("?") <= -1) ? (text + "?ReferralUserId=" + HiContext.Current.UserId) : (text + "&ReferralUserId=" + HiContext.Current.UserId));
						this.Page.Response.Redirect(text);
					}
					else
					{
						string cookie3 = WebHelper.GetCookie("UserCoordinateCookie");
						this.litAddress.Text = HttpUtility.UrlDecode(WebHelper.GetCookie("UserCoordinateCookie", "Address"));
						this.storeLogo.Src = storeById.StoreImages;
						this.litStoreName.SetWhenIsNotNull("<a href=\"StoreAbout?StoreId=" + storeById.StoreId + "\">" + storeById.StoreName + "</a>");
						this.litStoreName2.SetWhenIsNotNull("<a href=\"StoreAbout?StoreId=" + storeById.StoreId + "\">" + storeById.StoreName + "</a>");
						string text2 = (storeById.OpenEndDate < storeById.OpenStartDate) ? "次日" : "";
						Literal literal = this.litOpenDate;
						DateTime dateTime = storeById.OpenStartDate;
						string arg = dateTime.ToString("HH:mm");
						string arg2 = text2;
						dateTime = storeById.OpenEndDate;
						literal.Text = string.Format("{0} 至 {1}{2}", arg, arg2, dateTime.ToString("HH:mm"));
						HtmlInputHidden htmlInputHidden = this.hidLatitude;
						double? nullable = storeById.Latitude;
						htmlInputHidden.Value = nullable.ToString();
						HtmlInputHidden htmlInputHidden2 = this.hidLongitude;
						nullable = storeById.Longitude;
						htmlInputHidden2.Value = nullable.ToString();
						this.hidStoreName.Value = storeById.StoreName.ToString();
						this.aTel.HRef = "tel://" + storeById.Tel;
						this.litStoreTel.Text = storeById.Tel;
						if (!base.site.Store_IsOrderInClosingTime)
						{
							dateTime = DateTime.Now;
							string str = dateTime.ToString("yyyy-MM-dd");
							dateTime = storeById.OpenStartDate;
							DateTime value = (str + " " + dateTime.ToString("HH:mm")).ToDateTime().Value;
							dateTime = DateTime.Now;
							string str2 = dateTime.ToString("yyyy-MM-dd");
							dateTime = storeById.OpenEndDate;
							DateTime dateTime2 = (str2 + " " + dateTime.ToString("HH:mm")).ToDateTime().Value;
							if (dateTime2 <= value)
							{
								dateTime2 = dateTime2.AddDays(1.0);
							}
							this.hidIsOpenData.Value = "true";
							if (DateTime.Now < value || DateTime.Now > dateTime2)
							{
								this.hidIsOpenData.Value = "false";
							}
						}
						if (storeById.IsStoreDelive)
						{
							decimal? minOrderPrice = storeById.MinOrderPrice;
							int num2;
							if (minOrderPrice.GetValueOrDefault() > default(decimal) && minOrderPrice.HasValue)
							{
								Literal literal2 = this.litStoreDelive;
								num2 = storeById.MinOrderPrice.ToInt(0);
								literal2.Text = $"￥{num2.ToString()}起送，";
							}
							minOrderPrice = storeById.StoreFreight;
							if (minOrderPrice.GetValueOrDefault() > default(decimal) && minOrderPrice.HasValue)
							{
								Literal literal3 = this.litStoreDelive;
								string text3 = literal3.Text;
								num2 = storeById.StoreFreight.ToInt(0);
								literal3.Text = text3 + $"配送费￥{num2.ToString()}";
							}
							else
							{
								Literal literal4 = this.litStoreDelive;
								literal4.Text += "免配送费";
							}
						}
						StoreActivityEntityList storeActivity = StoresHelper.GetStoreActivity(this.storeId);
						if (storeActivity != null)
						{
							StringBuilder stringBuilder = new StringBuilder();
							if (storeActivity.FullAmountReduceList.Count > 0)
							{
								stringBuilder.AppendFormat("<div class=\"jian\"><i class=\"tag tag_green\">减</i><span>");
								int num3 = 0;
								while (num3 < storeActivity.FullAmountReduceList.Count)
								{
									if (num3 < 2)
									{
										stringBuilder.AppendFormat("{0}；", storeActivity.FullAmountReduceList[num3].ActivityName);
										num3++;
										continue;
									}
									stringBuilder.AppendFormat("{0}{1}", storeActivity.FullAmountReduceList[num3].ActivityName, (storeActivity.FullAmountReduceList.Count > 3) ? "等" : "");
									break;
								}
								if (stringBuilder.ToString().EndsWith("；"))
								{
									stringBuilder.Remove(stringBuilder.ToString().LastIndexOf('；'), 1);
								}
								stringBuilder.AppendFormat("</span></div>");
							}
							if (storeActivity.FullAmountSentFreightList.Count > 0)
							{
								stringBuilder.AppendFormat("<div class=\"mian\"><i class=\"tag tag_yellow\">免</i><span>");
								int num4 = 0;
								while (num4 < storeActivity.FullAmountSentFreightList.Count)
								{
									if (num4 < 2)
									{
										stringBuilder.AppendFormat("{0}；", storeActivity.FullAmountSentFreightList[num4].ActivityName);
										num4++;
										continue;
									}
									stringBuilder.AppendFormat("{0}{1}", storeActivity.FullAmountSentFreightList[num4].ActivityName, (storeActivity.FullAmountSentFreightList.Count > 3) ? "等" : "");
									break;
								}
								if (stringBuilder.ToString().EndsWith("；"))
								{
									stringBuilder.Remove(stringBuilder.ToString().LastIndexOf('；'), 1);
								}
								stringBuilder.AppendFormat("</span></div>");
							}
							if (storeActivity.FullAmountSentGiftList.Count > 0)
							{
								stringBuilder.AppendFormat("<div class=\"song\"><i class=\"tag tag_blue\">送</i><span>");
								int num5 = 0;
								while (num5 < storeActivity.FullAmountSentGiftList.Count)
								{
									if (num5 < 2)
									{
										stringBuilder.AppendFormat("{0}；", storeActivity.FullAmountSentGiftList[num5].ActivityName);
										num5++;
										continue;
									}
									stringBuilder.AppendFormat("{0}{1}", storeActivity.FullAmountSentGiftList[num5].ActivityName, (storeActivity.FullAmountSentGiftList.Count > 3) ? "等" : "");
									break;
								}
								if (stringBuilder.ToString().EndsWith("；"))
								{
									stringBuilder.Remove(stringBuilder.ToString().LastIndexOf('；'), 1);
								}
								stringBuilder.AppendFormat("</span></div>");
							}
							this.litActivityList.Text = stringBuilder.ToString();
							if (storeActivity.ActivityCount > 1)
							{
								this.litActivityCount.Text = $"<div id=\"huod-b\"><a href=\"javascript:;\">{storeActivity.ActivityCount}个活动</a><i></i></div>";
							}
						}
						if (this.rp_markting != null)
						{
							List<StoreMarktingInfo> storeMarktingInfoList = StoreMarktingHelper.GetStoreMarktingInfoList();
							foreach (StoreMarktingInfo item in storeMarktingInfoList)
							{
								item.StoreId = this.storeId;
								item.RedirectTo = this.RedirectToFullPath(item.RedirectTo);
							}
							this.rp_markting.DataSource = storeMarktingInfoList;
							this.rp_markting.DataBind();
						}
						PageTitle.AddSiteNameTitle(storeById.StoreName);
					}
				}
			}
			else
			{
				this.hidIsReloadPosition.Value = "1";
			}
		}

		private string RedirectToFullPath(string url)
		{
			string str = "/vshop/";
			switch (base.ClientType)
			{
			case ClientType.AliOH:
				str = "/AliOH/";
				break;
			case ClientType.WAP:
				str = "/WapShop/";
				break;
			}
			if (url.Contains("FightGroupActivities"))
			{
				str = "/vshop/";
			}
			if (url.IndexOf('/') == -1)
			{
				url = str + url;
			}
			return url;
		}

		private string GetImageFullPath(string imageUrl)
		{
			if (string.IsNullOrEmpty(imageUrl))
			{
				return Globals.FullPath(HiContext.Current.SiteSettings.DefaultProductThumbnail8);
			}
			if (imageUrl.StartsWith("http://"))
			{
				return imageUrl;
			}
			return Globals.FullPath(imageUrl);
		}
	}
}
