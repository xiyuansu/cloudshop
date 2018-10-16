using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Supplier;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Shopping;
using Hidistro.SaleSystem.Supplier;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class APPPreSaleproductdetail : AppshopTemplatedWebControl
	{
		private int presaleId;

		private int productId;

		private SiteSettings sitesettings = SettingsManager.GetMasterSettings();

		private AppshopTemplatedRepeater rptProductImages;

		private AppshopTemplatedRepeater rptCouponList;

		private AppshopTemplatedRepeater rptProductConsultations;

		private AppshopTemplatedRepeater rp_guest;

		private Literal litProdcutName;

		private Literal litSoldCount;

		private Literal litShortDescription;

		private Literal litDescription;

		private Literal litConsultationsCount;

		private Literal litReviewsCount;

		private Literal ltlcombinamaininfo;

		private Common_ExpandAttributes expandAttr;

		private HtmlInputHidden litHasCollected;

		private HtmlInputHidden hidden_skus;

		private UserProductReferLabel lbUserProductRefer;

		private HtmlButton buyButton;

		private HtmlInputHidden hidden_skuItem;

		private HtmlInputHidden hidCartQuantity;

		private HtmlGenericControl divshiptoregion;

		private HtmlGenericControl divwaplocateaddress;

		private HtmlGenericControl divPodrequest;

		private HtmlGenericControl divGuest;

		private StockLabel lblStock;

		private Literal litUnit;

		private ProductPromote promote;

		private HtmlInputHidden hdAppId;

		private HtmlInputHidden hdTitle;

		private HtmlInputHidden hdDesc;

		private HtmlInputHidden hdImgUrl;

		private HtmlInputHidden hdLink;

		private HtmlInputHidden hidRegionId;

		private HtmlInputHidden hidCombinaid;

		private HtmlGenericControl divProductReferral;

		private HtmlGenericControl liOrderPromotions;

		private HtmlGenericControl liOrderPromotions2;

		private HtmlGenericControl liProductSendGifts;

		private HtmlGenericControl liProductSendGifts2;

		private HtmlGenericControl liOrderPromotions_free2;

		private HtmlGenericControl liOrderPromotions_free;

		private HtmlInputHidden hidden_productId;

		private Literal ltlOrderPromotion;

		private Literal ltlOrderPromotion2;

		private Literal ltlProductSendGifts2;

		private Literal ltlProductSendGifts;

		private Literal ltlOrderPromotion_free2;

		private Literal ltlOrderPromotion_free;

		private HtmlInputHidden hidCouponCount;

		private HtmlInputHidden hidHasStores;

		private HtmlInputHidden hidEndDate;

		private HtmlInputHidden hidNowDate;

		private Common_SKUSubmitOrder skuSubmitOrder;

		private HtmlGenericControl divConsultationEmpty;

		private HtmlGenericControl ulConsultations;

		private HtmlGenericControl divShortDescription;

		private HtmlGenericControl divcombina;

		private ProductFreightLiteral productFreight;

		private HtmlInputHidden hidUnOnSale;

		private HtmlInputHidden hidSupplier;

		private HtmlGenericControl divPhonePrice;

		private Literal litPhonePrice;

		private Literal litpresaleprice;

		private Literal litsaleprice;

		private Literal litRetainage;

		private Literal litDeliverGood;

		private Literal litSupplierName;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-vpreSaleproductdetails.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (!int.TryParse(this.Page.Request.QueryString["PreSaleId"], out this.presaleId))
			{
				this.ShowWapMessage("错误的活动ID", "");
				return;
			}
			ProductPreSaleInfo productPreSaleInfo = ProductPreSaleHelper.GetProductPreSaleInfo(this.presaleId);
			if (productPreSaleInfo == null)
			{
				this.ShowWapMessage("错误的活动ID", "");
				return;
			}
			if (productPreSaleInfo.PreSaleEndDate < DateTime.Now)
			{
				this.ShowWapMessage("活动已结束", "");
				return;
			}
			this.productId = productPreSaleInfo.ProductId;
			this.hidSupplier = (HtmlInputHidden)this.FindControl("hidSupplier");
			this.litSupplierName = (Literal)this.FindControl("litSupplierName");
			this.rptProductConsultations = (AppshopTemplatedRepeater)this.FindControl("rptProductConsultations");
			this.rptProductImages = (AppshopTemplatedRepeater)this.FindControl("rptProductImages");
			this.rptCouponList = (AppshopTemplatedRepeater)this.FindControl("rptCouponList");
			this.rp_guest = (AppshopTemplatedRepeater)this.FindControl("rp_guest");
			this.litProdcutName = (Literal)this.FindControl("litProdcutName");
			this.litShortDescription = (Literal)this.FindControl("litShortDescription");
			this.litDescription = (Literal)this.FindControl("litDescription");
			this.ltlcombinamaininfo = (Literal)this.FindControl("ltlcombinamaininfo");
			this.skuSubmitOrder = (Common_SKUSubmitOrder)this.FindControl("skuSubmitOrder");
			this.expandAttr = (Common_ExpandAttributes)this.FindControl("ExpandAttributes");
			this.litSoldCount = (Literal)this.FindControl("litSoldCount");
			this.litConsultationsCount = (Literal)this.FindControl("litConsultationsCount");
			this.litReviewsCount = (Literal)this.FindControl("litReviewsCount");
			this.litHasCollected = (HtmlInputHidden)this.FindControl("litHasCollected");
			this.hidden_skus = (HtmlInputHidden)this.FindControl("hidden_skus");
			this.ltlOrderPromotion = (Literal)this.FindControl("ltlOrderPromotion");
			this.ltlOrderPromotion2 = (Literal)this.FindControl("ltlOrderPromotion2");
			this.ltlProductSendGifts = (Literal)this.FindControl("ltlProductSendGifts");
			this.ltlProductSendGifts2 = (Literal)this.FindControl("ltlProductSendGifts2");
			this.liOrderPromotions = (HtmlGenericControl)this.FindControl("liOrderPromotions");
			this.liOrderPromotions2 = (HtmlGenericControl)this.FindControl("liOrderPromotions2");
			this.liProductSendGifts = (HtmlGenericControl)this.FindControl("liProductSendGifts");
			this.liProductSendGifts2 = (HtmlGenericControl)this.FindControl("liProductSendGifts2");
			this.liOrderPromotions_free2 = (HtmlGenericControl)this.FindControl("liOrderPromotions_free2");
			this.liOrderPromotions_free = (HtmlGenericControl)this.FindControl("liOrderPromotions_free");
			this.ltlOrderPromotion_free2 = (Literal)this.FindControl("ltlOrderPromotion_free2");
			this.ltlOrderPromotion_free = (Literal)this.FindControl("ltlOrderPromotion_free");
			this.lbUserProductRefer = (UserProductReferLabel)this.FindControl("lbUserProductRefer");
			this.divshiptoregion = (HtmlGenericControl)this.FindControl("divshiptoregion");
			this.divwaplocateaddress = (HtmlGenericControl)this.FindControl("divwaplocateaddress");
			this.productFreight = (ProductFreightLiteral)this.FindControl("productFreight");
			this.promote = (ProductPromote)this.FindControl("ProductPromote");
			this.hdAppId = (HtmlInputHidden)this.FindControl("hdAppId");
			this.hdTitle = (HtmlInputHidden)this.FindControl("hdTitle");
			this.hdDesc = (HtmlInputHidden)this.FindControl("hdDesc");
			this.hdImgUrl = (HtmlInputHidden)this.FindControl("hdImgUrl");
			this.hdLink = (HtmlInputHidden)this.FindControl("hdLink");
			this.hidCombinaid = (HtmlInputHidden)this.FindControl("hidCombinaid");
			this.divConsultationEmpty = (HtmlGenericControl)this.FindControl("divConsultationEmpty");
			this.ulConsultations = (HtmlGenericControl)this.FindControl("ulConsultations");
			this.divShortDescription = (HtmlGenericControl)this.FindControl("divShortDescription");
			this.hidRegionId = (HtmlInputHidden)this.FindControl("hidRegionId");
			this.divProductReferral = (HtmlGenericControl)this.FindControl("divProductReferral");
			this.hidden_productId = (HtmlInputHidden)this.FindControl("hidden_productId");
			this.hidCouponCount = (HtmlInputHidden)this.FindControl("hidCouponCount");
			this.hidHasStores = (HtmlInputHidden)this.FindControl("hidHasStores");
			this.divPodrequest = (HtmlGenericControl)this.FindControl("divPodrequest");
			this.divGuest = (HtmlGenericControl)this.FindControl("divGuest");
			this.divcombina = (HtmlGenericControl)this.FindControl("divcombina");
			this.hidUnOnSale = (HtmlInputHidden)this.FindControl("hidUnOnSale");
			this.divPhonePrice = (HtmlGenericControl)this.FindControl("divPhonePrice");
			this.litPhonePrice = (Literal)this.FindControl("litPhonePrice");
			this.litpresaleprice = (Literal)this.FindControl("litpresaleprice");
			this.litsaleprice = (Literal)this.FindControl("litsaleprice");
			this.litRetainage = (Literal)this.FindControl("litRetainage");
			this.litDeliverGood = (Literal)this.FindControl("litDeliverGood");
			this.hidEndDate = (HtmlInputHidden)this.FindControl("hidEndDate");
			this.hidNowDate = (HtmlInputHidden)this.FindControl("hidNowDate");
			this.hdAppId.Value = masterSettings.WeixinAppId;
			HtmlInputHidden htmlInputHidden = this.hidRegionId;
			int num = HiContext.Current.DeliveryScopRegionId;
			htmlInputHidden.Value = num.ToString();
			this.hidden_skuItem = (HtmlInputHidden)this.FindControl("hidden_skuItem");
			this.hidCartQuantity = (HtmlInputHidden)this.FindControl("txCartQuantity");
			this.lblStock = (StockLabel)this.FindControl("lblStock");
			this.litUnit = (Literal)this.FindControl("litUnit");
			ProductBrowseInfo productPreSaleBrowseInfo = ProductBrowser.GetProductPreSaleBrowseInfo(this.productId, true);
			if (productPreSaleBrowseInfo.Product == null || productPreSaleBrowseInfo.Product.SaleStatus == ProductSaleStatus.Delete)
			{
				this.Page.Response.Redirect("ProductDelete.aspx");
				return;
			}
			if (productPreSaleBrowseInfo.Product.SaleStatus == ProductSaleStatus.UnSale)
			{
				this.hidUnOnSale.Value = "1";
			}
			this.litpresaleprice.Text = ((productPreSaleInfo.DepositPercent == 0) ? productPreSaleInfo.Deposit.F2ToString("f2") : ((decimal)productPreSaleInfo.DepositPercent * productPreSaleBrowseInfo.Product.MinSalePrice / 100m).F2ToString("f2"));
			if (productPreSaleBrowseInfo.Product.MinSalePrice < productPreSaleBrowseInfo.Product.MaxSalePrice)
			{
				this.litsaleprice.Text = "￥" + productPreSaleBrowseInfo.Product.MinSalePrice.F2ToString("f2") + "～ ￥" + productPreSaleBrowseInfo.Product.MaxSalePrice.F2ToString("f2");
			}
			else
			{
				this.litsaleprice.Text = "￥" + productPreSaleBrowseInfo.Product.MinSalePrice.F2ToString("f2");
			}
			Literal literal = this.litRetainage;
			DateTime dateTime = productPreSaleInfo.PaymentStartDate;
			string str = dateTime.ToString("yyyy/MM/dd");
			dateTime = productPreSaleInfo.PaymentEndDate;
			literal.Text = str + "～" + dateTime.ToString("yyyy/MM/dd");
			Literal literal2 = this.litDeliverGood;
			object text;
			if (!productPreSaleInfo.DeliveryDate.HasValue)
			{
				text = $"尾款支付后{productPreSaleInfo.DeliveryDays}天内发货";
			}
			else
			{
				dateTime = productPreSaleInfo.DeliveryDate.Value;
				text = dateTime.ToString("yyyy年MM月dd日") + "发货";
			}
			literal2.Text = (string)text;
			HtmlInputHidden htmlInputHidden2 = this.hidNowDate;
			dateTime = DateTime.Now;
			htmlInputHidden2.Value = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
			HtmlInputHidden htmlInputHidden3 = this.hidEndDate;
			dateTime = productPreSaleInfo.PreSaleEndDate;
			htmlInputHidden3.Value = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
			this.skuSubmitOrder.ProductInfo = productPreSaleBrowseInfo.Product;
			this.skuSubmitOrder.OrderBusiness = 4;
			this.skuSubmitOrder.PreSaleId = this.presaleId;
			this.skuSubmitOrder.productPreSaleInfo = productPreSaleInfo;
			this.lbUserProductRefer.product = productPreSaleBrowseInfo.Product;
			this.productFreight.ShippingTemplateId = productPreSaleBrowseInfo.Product.ShippingTemplateId;
			this.productFreight.Volume = productPreSaleBrowseInfo.Product.Weight;
			this.productFreight.Weight = productPreSaleBrowseInfo.Product.Weight;
			this.hdTitle.Value = Globals.StripAllTags(string.IsNullOrEmpty(productPreSaleBrowseInfo.Product.Title) ? productPreSaleBrowseInfo.Product.ProductName : productPreSaleBrowseInfo.Product.Title);
			this.hdDesc.Value = Globals.StripAllTags(string.IsNullOrEmpty(productPreSaleBrowseInfo.Product.ShortDescription) ? this.hdTitle.Value : productPreSaleBrowseInfo.Product.ShortDescription);
			string local = string.IsNullOrEmpty(productPreSaleBrowseInfo.Product.ImageUrl1) ? SettingsManager.GetMasterSettings().DefaultProductImage : productPreSaleBrowseInfo.Product.ImageUrl1;
			this.hdImgUrl.Value = Globals.FullPath(local);
			this.hdLink.Value = Globals.FullPath(HttpContext.Current.Request.Url.ToString());
			IEnumerable enumerable = from item in productPreSaleBrowseInfo.Product.Skus
			select item.Value;
			if (this.hidCartQuantity != null)
			{
				this.hidCartQuantity.Value = ShoppingCartProcessor.GetQuantity_Product(this.productId);
			}
			IEnumerable value = from item in productPreSaleBrowseInfo.Product.Skus
			select item.Value;
			if (JsonConvert.SerializeObject(productPreSaleBrowseInfo.DbSKUs) != null)
			{
				this.hidden_skuItem.Value = JsonConvert.SerializeObject(productPreSaleBrowseInfo.DbSKUs);
			}
			if (this.hidden_skus != null)
			{
				this.hidden_skus.Value = JsonConvert.SerializeObject(value);
			}
			if (this.hidden_productId != null)
			{
				this.hidden_productId.Value = this.productId.ToString();
			}
			if (this.promote != null)
			{
				this.promote.ProductId = this.productId;
			}
			MemberInfo user = HiContext.Current.User;
			if (user != null && user.IsReferral() && (!(this.sitesettings.SubMemberDeduct <= decimal.Zero) || productPreSaleBrowseInfo.Product.SubMemberDeduct.HasValue))
			{
				if (!productPreSaleBrowseInfo.Product.SubMemberDeduct.HasValue)
				{
					goto IL_0bcf;
				}
				decimal? subMemberDeduct = productPreSaleBrowseInfo.Product.SubMemberDeduct;
				if (!(subMemberDeduct.GetValueOrDefault() <= default(decimal)) || !subMemberDeduct.HasValue)
				{
					goto IL_0bcf;
				}
			}
			goto IL_0bf5;
			IL_0bf6:
			int num2;
			if (num2 != 0)
			{
				this.divProductReferral.Visible = false;
			}
			if (this.rptProductImages != null)
			{
				string locationUrl = "javascript:;";
				if (string.IsNullOrEmpty(productPreSaleBrowseInfo.Product.ImageUrl1) && string.IsNullOrEmpty(productPreSaleBrowseInfo.Product.ImageUrl2) && string.IsNullOrEmpty(productPreSaleBrowseInfo.Product.ImageUrl3) && string.IsNullOrEmpty(productPreSaleBrowseInfo.Product.ImageUrl4) && string.IsNullOrEmpty(productPreSaleBrowseInfo.Product.ImageUrl5))
				{
					productPreSaleBrowseInfo.Product.ImageUrl1 = masterSettings.DefaultProductImage;
				}
				List<SlideImage> list = new List<SlideImage>();
				list.Add(new SlideImage(productPreSaleBrowseInfo.Product.ImageUrl1, locationUrl));
				list.Add(new SlideImage(productPreSaleBrowseInfo.Product.ImageUrl2, locationUrl));
				list.Add(new SlideImage(productPreSaleBrowseInfo.Product.ImageUrl3, locationUrl));
				list.Add(new SlideImage(productPreSaleBrowseInfo.Product.ImageUrl4, locationUrl));
				list.Add(new SlideImage(productPreSaleBrowseInfo.Product.ImageUrl5, locationUrl));
				this.rptProductImages.DataSource = from item in list
				where !string.IsNullOrWhiteSpace(item.ImageUrl)
				select item;
				this.rptProductImages.DataBind();
			}
			this.litProdcutName.Text = productPreSaleBrowseInfo.Product.ProductName;
			this.litShortDescription.Text = productPreSaleBrowseInfo.Product.ShortDescription;
			this.divShortDescription.Visible = !string.IsNullOrEmpty(productPreSaleBrowseInfo.Product.ShortDescription);
			int supplierId = productPreSaleBrowseInfo.Product.SupplierId;
			if (supplierId > 0)
			{
				SupplierInfo supplierById = SupplierHelper.GetSupplierById(supplierId);
				if (supplierById != null)
				{
					this.hidSupplier.Value = "true";
					this.litSupplierName.Text = supplierById.SupplierName;
				}
			}
			else
			{
				this.hidSupplier.Value = "false";
			}
			if (this.litDescription != null)
			{
				string text2 = "";
				Regex regex = new Regex("<script[^>]*?>.*?</script>", RegexOptions.IgnoreCase);
				if (!string.IsNullOrWhiteSpace(productPreSaleBrowseInfo.Product.MobbileDescription))
				{
					text2 = regex.Replace(productPreSaleBrowseInfo.Product.MobbileDescription, "");
				}
				else if (!string.IsNullOrWhiteSpace(productPreSaleBrowseInfo.Product.Description))
				{
					text2 = regex.Replace(productPreSaleBrowseInfo.Product.Description, "");
				}
				text2 = text2.Replace("src", "data-url");
				this.litDescription.Text = text2;
			}
			Literal control = this.litSoldCount;
			num = productPreSaleBrowseInfo.Product.ShowSaleCounts;
			control.SetWhenIsNotNull(num.ToString());
			if (this.expandAttr != null)
			{
				this.expandAttr.ProductId = this.productId;
			}
			Literal control2 = this.litConsultationsCount;
			num = productPreSaleBrowseInfo.ConsultationCount;
			control2.SetWhenIsNotNull(num.ToString());
			Literal control3 = this.litReviewsCount;
			num = productPreSaleBrowseInfo.ReviewCount;
			control3.SetWhenIsNotNull(num.ToString());
			MemberInfo user2 = HiContext.Current.User;
			bool flag = false;
			if (user2 != null)
			{
				flag = ProductBrowser.CheckHasCollect(user2.UserId, this.productId);
			}
			this.litHasCollected.SetWhenIsNotNull(flag ? "1" : "0");
			this.BindCouponList();
			PageTitle.AddSiteNameTitle(productPreSaleBrowseInfo.Product.ProductName);
			this.BindPromotionInfo();
			this.BindProductSendGifts();
			this.BindGuestProducts();
			DataTable dBConsultations = productPreSaleBrowseInfo.DBConsultations;
			for (int i = 0; i < dBConsultations.Rows.Count; i++)
			{
				dBConsultations.Rows[i]["UserName"] = DataHelper.GetHiddenUsername(dBConsultations.Rows[i]["UserName"].ToNullString());
			}
			this.rptProductConsultations.DataSource = dBConsultations;
			this.rptProductConsultations.DataBind();
			this.divConsultationEmpty.Visible = dBConsultations.IsNullOrEmpty();
			this.ulConsultations.Visible = !dBConsultations.IsNullOrEmpty();
			PageTitle.AddSiteNameTitle("预售商品详情");
			return;
			IL_0bcf:
			if (HiContext.Current.SiteSettings.OpenReferral == 1)
			{
				num2 = ((!HiContext.Current.SiteSettings.ShowDeductInProductPage) ? 1 : 0);
				goto IL_0bf6;
			}
			goto IL_0bf5;
			IL_0bf5:
			num2 = 1;
			goto IL_0bf6;
		}

		private void BindGuestProducts()
		{
			IList<AppProductYouLikeModel> newProductYouLikeModel = ProductBrowser.GetNewProductYouLikeModel(this.productId, 0, 0, null, false);
			if (newProductYouLikeModel != null && newProductYouLikeModel.Count > 0)
			{
				this.divGuest.Visible = true;
				this.rp_guest.DataSource = newProductYouLikeModel;
				this.rp_guest.DataBind();
			}
		}

		public void BindCouponList()
		{
			int userId = 0;
			MemberInfo user = HiContext.Current.User;
			if (user != null)
			{
				userId = user.UserId;
			}
			int num = 0;
			DataTable couponList = CouponHelper.GetCouponList(this.productId, userId, false, false, false);
			this.hidCouponCount.Value = couponList.Rows.Count.ToString();
			this.rptCouponList.DataSource = couponList;
			this.rptCouponList.DataBind();
		}

		private void BindPromotionInfo()
		{
			StoreActivityEntityList storeActivityEntity = PromoteHelper.GetStoreActivityEntity(0, 0);
			if (storeActivityEntity.FullAmountReduceList.Count > 0)
			{
				HtmlGenericControl htmlGenericControl = this.liOrderPromotions;
				HtmlGenericControl htmlGenericControl2 = this.liOrderPromotions2;
				bool visible = htmlGenericControl2.Visible = true;
				htmlGenericControl.Visible = visible;
				string empty = string.Empty;
				empty = (from t in storeActivityEntity.FullAmountReduceList
				select t.ActivityName).Aggregate((string t, string n) => t + "，" + n);
				Literal literal = this.ltlOrderPromotion2;
				Literal literal2 = this.ltlOrderPromotion;
				string text3 = literal.Text = (literal2.Text = empty);
			}
			if (storeActivityEntity.FullAmountSentFreightList.Count > 0)
			{
				HtmlGenericControl htmlGenericControl3 = this.liOrderPromotions_free2;
				HtmlGenericControl htmlGenericControl4 = this.liOrderPromotions_free;
				bool visible = htmlGenericControl4.Visible = true;
				htmlGenericControl3.Visible = visible;
				string empty2 = string.Empty;
				empty2 += (from t in storeActivityEntity.FullAmountSentFreightList
				select t.ActivityName).Aggregate((string t, string n) => t + "，" + n);
				Literal literal3 = this.ltlOrderPromotion_free2;
				Literal literal4 = this.ltlOrderPromotion_free;
				string text3 = literal3.Text = (literal4.Text = empty2);
			}
			string text6 = this.BindProductSendGifts();
			if (storeActivityEntity.FullAmountSentGiftList.Count > 0 || !string.IsNullOrEmpty(text6))
			{
				if (storeActivityEntity.FullAmountSentGiftList.Count > 0)
				{
					string text7 = (from t in storeActivityEntity.FullAmountSentGiftList
					select t.ActivityName).Aggregate((string t, string n) => t + "， " + n);
					HtmlGenericControl htmlGenericControl5 = this.liProductSendGifts;
					HtmlGenericControl htmlGenericControl6 = this.liProductSendGifts2;
					bool visible = htmlGenericControl6.Visible = true;
					htmlGenericControl5.Visible = visible;
					Literal literal5 = this.ltlProductSendGifts;
					Literal literal6 = this.ltlProductSendGifts2;
					string text3 = literal5.Text = (literal6.Text = text7);
				}
				if (!string.IsNullOrEmpty(text6))
				{
					HtmlGenericControl htmlGenericControl7 = this.liProductSendGifts;
					HtmlGenericControl htmlGenericControl8 = this.liProductSendGifts2;
					bool visible = htmlGenericControl8.Visible = true;
					htmlGenericControl7.Visible = visible;
					Literal literal7 = this.ltlProductSendGifts;
					literal7.Text = literal7.Text + "</em>" + text6;
					Literal literal8 = this.ltlProductSendGifts2;
					literal8.Text = literal8.Text + "</em>" + text6;
				}
			}
		}

		private string BindProductSendGifts()
		{
			string promsg = string.Empty;
			PromotionInfo promotion = ProductBrowser.GetProductPromotionInfo(this.productId);
			if (promotion == null || (promotion.PromoteType != PromoteType.SentGift && promotion.PromoteType != PromoteType.SentProduct))
			{
				return string.Empty;
			}
			if (promotion.PromoteType == PromoteType.SentGift)
			{
				IList<GiftInfo> giftDetailsByGiftIds = ProductBrowser.GetGiftDetailsByGiftIds(promotion.GiftIds);
				if (giftDetailsByGiftIds.Count > 0)
				{
					giftDetailsByGiftIds.ForEach(delegate(GiftInfo giftinfo)
					{
						promsg += string.Format("<em><a href=\"{0}\"><img src=\"{1}\" title=\"{2}\"></a></em><b> ×1 </b>", base.GetRouteUrl("FavourableDetails", new
						{
							activityId = promotion.ActivityId
						}), Globals.GetImageServerUrl("http://", giftinfo.ThumbnailUrl40), giftinfo.Name);
					});
				}
			}
			else if (promotion.PromoteType == PromoteType.SentProduct)
			{
				promsg += string.Format("<b>{0}</b>", (promotion.Name.Length > 40) ? (promotion.Name.Substring(0, 40) + "...") : promotion.Name);
			}
			return promsg;
		}
	}
}
