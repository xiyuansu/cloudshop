using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Helper;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Promotions;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WAPStoreProductDetails : WAPTemplatedWebControl
	{
		private int productId;

		private int storeId = 0;

		private SiteSettings sitesettings = SettingsManager.GetMasterSettings();

		private WapTemplatedRepeater rptCouponList;

		private WapTemplatedRepeater rp_guest;

		private WapTemplatedRepeater rp_com;

		private Literal litProdcutName;

		private Literal litPdImgSlides;

		private Literal litSalePrice;

		private Literal litSoldCount;

		private Literal litMarketPrice;

		private Literal litReferral;

		private Literal litShortDescription;

		private Literal litDescription;

		private Literal ltlBottomStatus;

		private Literal litConsultationsCount;

		private Literal litReviewsCount;

		private HtmlInputHidden litStoreId;

		private HtmlInputHidden litHasCollected;

		private HtmlInputHidden hidLngLat;

		private HtmlInputHidden hidStoreId;

		private HtmlInputHidden hdQQMapKey;

		private HtmlInputHidden hidStoreName;

		private StockLabel lblStock;

		private Literal litUnit;

		private HyperLink aCountDownUrl;

		private Literal ltlStoreName;

		private Literal ltlStoreAddress;

		private Literal ltlDistance;

		private Literal ltlDelivery;

		private Literal ltlDeliveryDetail;

		private HtmlInputHidden hdAppId;

		private HtmlInputHidden hdTitle;

		private HtmlInputHidden hdDesc;

		private HtmlInputHidden hdImgUrl;

		private HtmlInputHidden hdLink;

		private HtmlGenericControl divProductReferral;

		private HtmlGenericControl liProductSendGifts;

		private HtmlGenericControl liProductSendGifts2;

		private Literal ltlProductSendGifts;

		private Literal ltlProductSendGifts2;

		private Common_SKUSubmitStoreOrder skuSubmitOrder;

		private HtmlGenericControl divShortDescription;

		private HtmlGenericControl divCountDownUrl;

		private HtmlInputHidden hidUnOnSale;

		private HtmlInputHidden hidRecommend;

		private HtmlInputHidden hidNoData;

		private Literal ltlMessage;

		private HtmlGenericControl spdiscount;

		private HtmlGenericControl divGouMai;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-vstoreproductdetails.html";
			}
			if (!int.TryParse(this.Page.Request.QueryString["storeId"], out this.storeId) || this.storeId == 0)
			{
				this.Page.Response.Redirect("ProductDetails.aspx?productId=" + this.productId);
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			base.CheckOpenMultStore();
			if (!int.TryParse(this.Page.Request.QueryString["productId"], out this.productId))
			{
				this.ShowWapMessage("错误的商品ID", "Default.aspx");
			}
			this.aCountDownUrl = (HyperLink)this.FindControl("aCountDownUrl");
			this.divCountDownUrl = (HtmlGenericControl)this.FindControl("divCountDownUrl");
			this.aCountDownUrl.Visible = false;
			this.HasActivitiesToJumpUrl();
			MemberInfo user = HiContext.Current.User;
			if (user.UserId != 0 && user.IsReferral() && string.IsNullOrEmpty(this.Page.Request.QueryString["ReferralUserId"]))
			{
				string text = HttpContext.Current.Request.Url.ToString();
				text = ((text.IndexOf("?") <= -1) ? (text + "?ReferralUserId=" + HiContext.Current.UserId) : (text + "&ReferralUserId=" + HiContext.Current.UserId));
				this.Page.Response.Redirect(text);
				return;
			}
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			this.litPdImgSlides = (Literal)this.FindControl("litPdImgSlides");
			this.rptCouponList = (WapTemplatedRepeater)this.FindControl("rptCouponList");
			this.rp_guest = (WapTemplatedRepeater)this.FindControl("rp_guest");
			this.rp_com = (WapTemplatedRepeater)this.FindControl("rp_com");
			this.litProdcutName = (Literal)this.FindControl("litProdcutName");
			this.litSalePrice = (Literal)this.FindControl("litSalePrice");
			this.litMarketPrice = (Literal)this.FindControl("litMarketPrice");
			this.litReferral = (Literal)this.FindControl("litReferral");
			this.litShortDescription = (Literal)this.FindControl("litShortDescription");
			this.litDescription = (Literal)this.FindControl("litDescription");
			this.ltlBottomStatus = (Literal)this.FindControl("ltlBottomStatus");
			this.skuSubmitOrder = (Common_SKUSubmitStoreOrder)this.FindControl("skuSubmitOrder");
			this.litSoldCount = (Literal)this.FindControl("litSoldCount");
			this.litConsultationsCount = (Literal)this.FindControl("litConsultationsCount");
			this.litReviewsCount = (Literal)this.FindControl("litReviewsCount");
			this.litStoreId = (HtmlInputHidden)this.FindControl("litStoreId");
			this.litStoreId.Value = this.storeId.ToString();
			this.litHasCollected = (HtmlInputHidden)this.FindControl("litHasCollected");
			this.hidLngLat = (HtmlInputHidden)this.FindControl("hidLngLat");
			this.hdQQMapKey = (HtmlInputHidden)this.FindControl("hdQQMapKey");
			this.hidStoreName = (HtmlInputHidden)this.FindControl("hidStoreName");
			this.divShortDescription = (HtmlGenericControl)this.FindControl("divShortDescription");
			this.divProductReferral = (HtmlGenericControl)this.FindControl("divProductReferral");
			this.hidUnOnSale = (HtmlInputHidden)this.FindControl("hidUnOnSale");
			this.hidRecommend = (HtmlInputHidden)this.FindControl("hidRecommend");
			this.hidNoData = (HtmlInputHidden)this.FindControl("hidNoData");
			this.divGouMai = (HtmlGenericControl)this.FindControl("divGouMai");
			this.hidStoreId = (HtmlInputHidden)this.FindControl("hidStoreId");
			this.ltlMessage = (Literal)this.FindControl("ltlMessage");
			this.spdiscount = (HtmlGenericControl)this.FindControl("spdiscount");
			this.hidRecommend.Value = (masterSettings.Store_IsRecommend ? "1" : "0");
			this.lblStock = (StockLabel)this.FindControl("lblStock");
			this.litUnit = (Literal)this.FindControl("litUnit");
			this.hdAppId = (HtmlInputHidden)this.FindControl("hdAppId");
			this.hdTitle = (HtmlInputHidden)this.FindControl("hdTitle");
			this.hdDesc = (HtmlInputHidden)this.FindControl("hdDesc");
			this.hdImgUrl = (HtmlInputHidden)this.FindControl("hdImgUrl");
			this.hdLink = (HtmlInputHidden)this.FindControl("hdLink");
			this.hdAppId.Value = masterSettings.WeixinAppId;
			StoreProductQuery storeProductQuery = new StoreProductQuery
			{
				ProductId = this.productId,
				StoreId = this.storeId
			};
			string cookie = WebHelper.GetCookie("UserCoordinateCookie", "NewCoordinate");
			if (!string.IsNullOrEmpty(cookie))
			{
				string[] array = cookie.Split(',');
				storeProductQuery.Position = new PositionInfo(array[0].ToDouble(0), array[1].ToDouble(0));
				storeProductQuery.Position.CityId = WebHelper.GetCookie("UserCoordinateCookie", "CityRegionId").ToInt(0);
				storeProductQuery.Position.AreaId = WebHelper.GetCookie("UserCoordinateCookie", "RegionId").ToInt(0);
			}
			else
			{
				storeProductQuery.Position = new PositionInfo(0.0, 0.0);
				storeProductQuery.Position.CityId = 0;
				storeProductQuery.Position.AreaId = 0;
			}
			this.hidStoreId.Value = this.storeId.ToString();
			ProductModel storeProduct = ProductBrowser.GetStoreProduct(storeProductQuery);
			this.skuSubmitOrder.ProductInfo = storeProduct;
			if (storeProduct == null || storeProduct.SaleStatus == ProductSaleStatus.Delete)
			{
				this.Page.Response.Redirect("ProductDelete.aspx");
				return;
			}
			if (storeProduct.ProductType == 1.GetHashCode())
			{
				HttpContext.Current.Response.Redirect("ServiceProductDetails?productId=" + this.productId + "&StoreID=" + this.storeId);
			}
			if (storeProduct.SaleStatus != ProductSaleStatus.OnSale)
			{
				this.hidUnOnSale.Value = "1";
			}
			if (this.spdiscount != null && HiContext.Current.User.UserId > 0)
			{
				MemberGradeInfo memberGrade = MemberProcessor.GetMemberGrade(HiContext.Current.User.GradeId);
				this.spdiscount.Visible = true;
				this.spdiscount.InnerHtml = "<strong class='vip_price'><img src='/templates/pccommon/images/vip_price.png' />" + memberGrade.Name + "价</strong>";
			}
			MemberInfo user2 = HiContext.Current.User;
			if (user2 != null && user2.IsReferral() && (!(this.sitesettings.SubMemberDeduct <= decimal.Zero) || storeProduct.SubMemberDeduct.HasValue))
			{
				if (!storeProduct.SubMemberDeduct.HasValue)
				{
					goto IL_078d;
				}
				decimal? subMemberDeduct = storeProduct.SubMemberDeduct;
				if (!(subMemberDeduct.GetValueOrDefault() <= default(decimal)) || !subMemberDeduct.HasValue)
				{
					goto IL_078d;
				}
			}
			goto IL_07c7;
			IL_078d:
			int num;
			if (HiContext.Current.SiteSettings.OpenReferral == 1 && HiContext.Current.SiteSettings.ShowDeductInProductPage && user2.Referral != null)
			{
				num = (user2.Referral.IsRepeled ? 1 : 0);
				goto IL_07c8;
			}
			goto IL_07c7;
			IL_07c8:
			if (num != 0)
			{
				this.divProductReferral.Visible = false;
			}
			else
			{
				this.litReferral.Text = storeProduct.ProductReduce;
			}
			storeProduct.ImgUrlList.ForEach(delegate(string i)
			{
				Literal literal5 = this.litPdImgSlides;
				literal5.Text += $"<img src=\"{i}\" />";
			});
			this.litProdcutName.Text = storeProduct.ProductName;
			this.litSalePrice.Text = ((storeProduct.MinSalePrice == storeProduct.MaxSalePrice) ? storeProduct.MinSalePrice.F2ToString("f2") : (storeProduct.MinSalePrice.F2ToString("f2") + "~" + storeProduct.MaxSalePrice.F2ToString("f2")));
			this.litMarketPrice.Text = storeProduct.MarketPrice.F2ToString("f2");
			this.litShortDescription.Text = storeProduct.ShortDescription;
			this.divShortDescription.Visible = !string.IsNullOrEmpty(storeProduct.ShortDescription);
			if (!string.IsNullOrWhiteSpace(storeProduct.Description))
			{
				Regex regex = new Regex("<script[^>]*?>.*?</script>", RegexOptions.IgnoreCase);
				this.litDescription.Text = regex.Replace(storeProduct.Description, "").Replace("src", "data-url");
			}
			Literal literal = this.litSoldCount;
			int num2 = storeProduct.ShowSaleCounts;
			literal.Text = num2.ToString();
			Literal control = this.litConsultationsCount;
			num2 = storeProduct.ConsultationCount;
			control.SetWhenIsNotNull(num2.ToString());
			Literal control2 = this.litReviewsCount;
			num2 = storeProduct.ReviewCount;
			control2.SetWhenIsNotNull(num2.ToString());
			if (storeProduct.StoreInfo.IsInServiceArea)
			{
				this.ltlStoreName = (Literal)this.FindControl("ltlStoreName");
				this.ltlStoreName.Text = storeProduct.StoreInfo.StoreName;
				this.ltlDistance = (Literal)this.FindControl("ltlDistance");
				this.ltlDistance.Text = storeProduct.StoreInfo.Distance;
				this.ltlDelivery = (Literal)this.FindControl("ltlDelivery");
				this.ltlDelivery.Text = storeProduct.StoreInfo.Delivery.DeliveryList.Aggregate(string.Empty, (string t, string n) => "<li>" + t + "</li><li>" + n + "</li>");
				this.ltlDeliveryDetail = (Literal)this.FindControl("ltlDeliveryDetail");
				if (storeProduct.StoreInfo.Delivery.IsStoreDelive)
				{
					if (storeProduct.StoreInfo.Delivery.MinOrderPrice > decimal.Zero)
					{
						Literal literal2 = this.ltlDeliveryDetail;
						num2 = storeProduct.StoreInfo.Delivery.MinOrderPrice.ToInt(0);
						literal2.Text = $"￥<em>{num2.ToString()}</em>起送";
						if (storeProduct.StoreInfo.Delivery.StoreFreight > decimal.Zero)
						{
							Literal literal3 = this.ltlDeliveryDetail;
							string text2 = literal3.Text;
							num2 = storeProduct.StoreInfo.Delivery.StoreFreight.ToInt(0);
							literal3.Text = text2 + $",配送费￥<em>{num2.ToString()}</em>";
						}
						else
						{
							Literal literal4 = this.ltlDeliveryDetail;
							literal4.Text += string.Format(",免配送费");
						}
					}
					else if (storeProduct.StoreInfo.Delivery.StoreFreight > decimal.Zero)
					{
						this.ltlDeliveryDetail.Text = $"配送费￥<em>{storeProduct.StoreInfo.Delivery.StoreFreight.ToInt(0)}</em>";
					}
					else
					{
						this.ltlDeliveryDetail.Text = string.Format("免配送费");
					}
				}
				this.ltlStoreAddress = (Literal)this.FindControl("ltlStoreAddress");
				this.ltlStoreAddress.Text = storeProduct.StoreInfo.AddressSimply;
				this.hidLngLat.Value = $"{storeProduct.StoreInfo.Position.Longitude},{storeProduct.StoreInfo.Position.Latitude}";
				this.hidStoreName.Value = storeProduct.StoreInfo.StoreName;
				this.hdQQMapKey.Value = masterSettings.QQMapAPIKey;
				this.hdTitle.Value = Globals.StripAllTags(string.IsNullOrEmpty(storeProduct.Title) ? storeProduct.ProductName : storeProduct.Title);
				this.hdDesc.Value = Globals.StripAllTags(string.IsNullOrEmpty(storeProduct.ShortDescription) ? this.hdTitle.Value : storeProduct.ShortDescription);
				string oldValue = "/storage/master/product/images/";
				string newValue = "/storage/master/product/thumbs410/410_";
				string text3 = storeProduct.ImgUrlList[0];
				if (!string.IsNullOrEmpty(text3))
				{
					text3 = text3.ToLower().Replace(oldValue, newValue);
				}
				string local = string.IsNullOrEmpty(text3) ? SettingsManager.GetMasterSettings().DefaultProductImage : text3;
				this.hdImgUrl.Value = Globals.FullPath(local);
				this.hdLink.Value = Globals.FullPath(HttpContext.Current.Request.Url.ToString());
			}
			this.litHasCollected.SetWhenIsNotNull(storeProduct.IsFavorite ? "1" : "0");
			PageTitle.AddSiteNameTitle(storeProduct.ProductName);
			this.ProcessException(storeProduct);
			return;
			IL_07c7:
			num = 1;
			goto IL_07c8;
		}

		private void ProcessException(ProductModel pd)
		{
			string str = "<div id=\"addcartButton\" style='float: left;' type=\"shoppingBtn\" class=\"add_cart\">加入购物车</div>";
			switch (pd.ExStatus)
			{
			case DetailException.StopService:
				this.ltlBottomStatus.Text = string.Format("<div class=\"xieye\"><h4>歇业中</h4><p>营业时间：{0}</p> </div>", pd.StoreInfo.CloseEndTime.Value.ToString("yyyy年MM月dd号 HH:mm"));
				break;
			case DetailException.NoStock:
				this.ltlBottomStatus.Text = "<div class=\"notpro\">已售罄</div>";
				this.ltlMessage.Text = "商品已售罄啦，不过以下门店还有货哦";
				break;
			case DetailException.OverServiceArea:
				this.ltlBottomStatus.Text = str + "<div class=\"chaoqu\">服务范围超区</div>";
				break;
			case DetailException.IsNotWorkTime:
				this.ltlBottomStatus.Text = str + "<div class=\"nottheTime\">非营业时间</div>";
				break;
			}
			if (pd.ExStatus == DetailException.Nomal)
			{
				this.hidNoData.Value = "0";
			}
			else
			{
				this.hidNoData.Value = "1";
			}
			this.divGouMai.Visible = false;
			if (pd.Stock > 0 && pd.ExStatus == DetailException.Nomal)
			{
				this.divGouMai.Visible = true;
			}
		}

		private void BindGuestProducts()
		{
			IList<AppProductYouLikeModel> newProductYouLikeModel = ProductBrowser.GetNewProductYouLikeModel(this.productId, 0, 0, null, false);
			if (newProductYouLikeModel != null && newProductYouLikeModel.Count > 0)
			{
				this.rp_guest.DataSource = newProductYouLikeModel;
				this.rp_guest.DataBind();
			}
		}

		private void BindProductSendGifts()
		{
			PromotionInfo productPromotionInfo = ProductBrowser.GetProductPromotionInfo(this.productId);
			string promsg = string.Empty;
			if (productPromotionInfo != null)
			{
				if (productPromotionInfo.PromoteType == PromoteType.SentGift && !string.IsNullOrEmpty(productPromotionInfo.GiftIds))
				{
					IList<GiftInfo> giftDetailsByGiftIds = ProductBrowser.GetGiftDetailsByGiftIds(productPromotionInfo.GiftIds);
					if (giftDetailsByGiftIds.Count > 0)
					{
						giftDetailsByGiftIds.ForEach(delegate(GiftInfo giftinfo)
						{
							promsg = promsg + giftinfo.Name + " × 1,";
						});
						promsg = promsg.TrimEnd(',');
					}
				}
				else if (productPromotionInfo.PromoteType == PromoteType.SentProduct)
				{
					promsg += productPromotionInfo.Name;
				}
				HtmlGenericControl htmlGenericControl = this.liProductSendGifts;
				HtmlGenericControl htmlGenericControl2 = this.liProductSendGifts2;
				bool visible = htmlGenericControl2.Visible = (promsg.Length > 0);
				htmlGenericControl.Visible = visible;
				Literal literal = this.ltlProductSendGifts2;
				Literal literal2 = this.ltlProductSendGifts;
				string text3 = literal.Text = (literal2.Text = promsg);
			}
		}

		private void HasActivitiesToJumpUrl()
		{
			CountDownInfo countDownInfo = PromoteHelper.ActiveCountDownByProductId(this.productId, this.storeId);
			if (countDownInfo != null && countDownInfo.CountDownId > 0)
			{
				if (countDownInfo.StartDate <= DateTime.Now)
				{
					this.Page.Response.Redirect("CountDownStoreProductsDetails.aspx?countDownId=" + countDownInfo.CountDownId + "&StoreId=" + this.storeId);
				}
				else
				{
					this.aCountDownUrl.Text = "该商品即将参与抢购活动，     去看看";
					this.aCountDownUrl.NavigateUrl = "CountDownStoreProductsDetails.aspx?countDownId=" + countDownInfo.CountDownId + "&StoreId=" + this.storeId;
					this.aCountDownUrl.Style.Add("color", "red");
					this.aCountDownUrl.Visible = true;
					this.divCountDownUrl.Visible = true;
				}
			}
		}
	}
}
