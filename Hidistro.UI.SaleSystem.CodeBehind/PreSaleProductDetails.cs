using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Store;
using Hidistro.Entities.Supplier;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Shopping;
using Hidistro.SaleSystem.Store;
using Hidistro.SaleSystem.Supplier;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class PreSaleProductDetails : HtmlTemplatedWebControl
	{
		private int preSaleId;

		private int productId;

		private SiteSettings sitesettings = SettingsManager.GetMasterSettings();

		private Common_Location common_Location;

		private Literal litProductName;

		private StockLabel lblStock;

		private Literal litUnit;

		private Literal litSaleCounts;

		private Label lblBuyPrice;

		private TotalLabel lblTotalPrice;

		private Literal litDescription;

		private Literal litShortDescription;

		private BuyButton btnBuy;

		private HyperLink hpkProductConsultations;

		private Literal ltlSaleCount;

		private Literal ltlConsultation;

		private Literal ltlReviewCount;

		private Literal litReviewCount;

		private Common_ProductImages images;

		private ThemedTemplatedRepeater rptExpandAttributes;

		private ThemedTemplatedRepeater rptOnlineService;

		private SKUSelector skuSelector;

		private Common_ProductConsultations consultations;

		private Common_GoodsList_Correlative correlative;

		private HtmlInputHidden hiddenpid;

		private HtmlInputHidden hidden_skus;

		private HtmlInputHidden hidden_skuItem;

		private HtmlInputHidden hidCartQuantity;

		private Literal ltlProductSendGifts;

		private Literal ltlPromotionSendGifts;

		private UserProductReferLabel lbUserProductRefer;

		private HtmlInputHidden hidIsOpenMultiStore;

		private HyperLink aCountDownUrl;

		private Image imgQrCode;

		private HtmlGenericControl divGift;

		private HtmlInputHidden hidden_productId;

		private HtmlInputHidden hidHasStores;

		private HiImage imgBrand;

		private Literal ltlGiftName;

		private Literal ltlGiftNum;

		private HtmlAnchor aBrand;

		private SkuLabel lblSku;

		private HtmlImage imgCustomerService;

		private Literal ltlOrderPromotion;

		private HtmlGenericControl divOrderPromotions;

		private HtmlGenericControl divCuxiao;

		private HtmlGenericControl divPhonePrice;

		private Literal litPhonePrice;

		private Literal litPhonePriceEndDate;

		private Literal ltlOrderPromotion_free;

		private Literal ltlUnit;

		private HtmlGenericControl divProductReferral;

		private HtmlGenericControl divqq;

		private Common_SetDeliveryRegion setDeliverRegion;

		private Label lblPaymentStartDate;

		private Label lblPaymentEndDate;

		private Label lblDelivery;

		private Label lblDepositPercent;

		private Label lblDeposit;

		private Label lblFinalPaymentPercent;

		private Label lblFinalPayment;

		private HtmlInputHidden hidEndDate;

		private HtmlInputHidden hidNowDate;

		private HtmlInputHidden hidDepositPercent;

		private HtmlInputHidden hidDeposit;

		private HtmlInputHidden hidSupName;

		private HtmlGenericControl divOrderPromotions2;

		private HtmlGenericControl divOrderPromotions3;

		private HtmlGenericControl divOrderPromotions4;

		private Literal litSupplierName;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-PreSaleProductDetails.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			if (!int.TryParse(base.GetParameter("PreSaleId", false), out this.preSaleId))
			{
				base.GotoResourceNotFound();
			}
			ProductPreSaleInfo productPreSaleInfo = ProductPreSaleHelper.GetProductPreSaleInfo(this.preSaleId);
			if (productPreSaleInfo == null)
			{
				base.GotoResourceNotFound();
			}
			this.productId = productPreSaleInfo.ProductId;
			if (productPreSaleInfo.PreSaleEndDate < DateTime.Now)
			{
				this.Page.Response.Redirect("/ProductDetails.aspx?productId=" + this.productId);
				return;
			}
			this.hidSupName = (HtmlInputHidden)this.FindControl("hidSupName");
			this.litSupplierName = (Literal)this.FindControl("litSupplierName");
			this.hiddenpid = (HtmlInputHidden)this.FindControl("hiddenpid");
			this.hidCartQuantity = (HtmlInputHidden)this.FindControl("txCartQuantity");
			this.hiddenpid.Value = this.productId.ToString();
			this.common_Location = (Common_Location)this.FindControl("common_Location");
			this.litProductName = (Literal)this.FindControl("litProductName");
			this.lblStock = (StockLabel)this.FindControl("lblStock");
			this.litUnit = (Literal)this.FindControl("litUnit");
			this.litSaleCounts = (Literal)this.FindControl("litSaleCounts");
			this.lblBuyPrice = (Label)this.FindControl("lblBuyPrice");
			this.lblTotalPrice = (TotalLabel)this.FindControl("lblTotalPrice");
			this.lblSku = (SkuLabel)this.FindControl("lblSku");
			this.litDescription = (Literal)this.FindControl("litDescription");
			this.litShortDescription = (Literal)this.FindControl("litShortDescription");
			this.btnBuy = (BuyButton)this.FindControl("btnBuy");
			this.hpkProductConsultations = (HyperLink)this.FindControl("hpkProductConsultations");
			this.ltlSaleCount = (Literal)this.FindControl("ltlSaleCount");
			this.ltlReviewCount = (Literal)this.FindControl("ltlReviewCount");
			this.litReviewCount = (Literal)this.FindControl("litReviewCount");
			this.ltlConsultation = (Literal)this.FindControl("ltlConsultation");
			this.images = (Common_ProductImages)this.FindControl("common_ProductImages");
			this.rptExpandAttributes = (ThemedTemplatedRepeater)this.FindControl("rptExpandAttributes");
			this.rptOnlineService = (ThemedTemplatedRepeater)this.FindControl("rptOnlineService");
			this.skuSelector = (SKUSelector)this.FindControl("SKUSelector");
			this.consultations = (Common_ProductConsultations)this.FindControl("list_Common_ProductConsultations");
			this.correlative = (Common_GoodsList_Correlative)this.FindControl("list_Common_GoodsList_Correlative");
			this.lbUserProductRefer = (UserProductReferLabel)this.FindControl("lbUserProductRefer");
			this.hidden_skus = (HtmlInputHidden)this.FindControl("hidden_skus");
			this.hidden_skuItem = (HtmlInputHidden)this.FindControl("hidden_skuItem");
			this.hidIsOpenMultiStore = (HtmlInputHidden)this.FindControl("hidIsOpenMultiStore");
			this.aCountDownUrl = (HyperLink)this.FindControl("aCountDownUrl");
			this.imgQrCode = (Image)this.FindControl("imgQrCode");
			this.hidden_productId = (HtmlInputHidden)this.FindControl("hidden_productId");
			this.hidHasStores = (HtmlInputHidden)this.FindControl("hidHasStores");
			this.divGift = (HtmlGenericControl)this.FindControl("divGift");
			this.ltlGiftName = (Literal)this.FindControl("ltlGiftName");
			this.ltlGiftNum = (Literal)this.FindControl("ltlGiftNum");
			this.aBrand = (HtmlAnchor)this.FindControl("aBrand");
			this.imgBrand = (HiImage)this.FindControl("imgBrand");
			this.imgCustomerService = (HtmlImage)this.FindControl("imgCustomerService");
			this.ltlOrderPromotion = (Literal)this.FindControl("ltlOrderPromotion");
			this.divOrderPromotions = (HtmlGenericControl)this.FindControl("divOrderPromotions");
			this.divOrderPromotions2 = (HtmlGenericControl)this.FindControl("divOrderPromotions2");
			this.divOrderPromotions4 = (HtmlGenericControl)this.FindControl("divOrderPromotions4");
			this.divOrderPromotions3 = (HtmlGenericControl)this.FindControl("divOrderPromotions3");
			this.ltlOrderPromotion_free = (Literal)this.FindControl("ltlOrderPromotion_free");
			this.litPhonePrice = (Literal)this.FindControl("litPhonePrice");
			this.litPhonePriceEndDate = (Literal)this.FindControl("litPhonePriceEndDate");
			this.divCuxiao = (HtmlGenericControl)this.FindControl("divCuxiao");
			this.ltlUnit = (Literal)this.FindControl("ltlUnit");
			this.divProductReferral = (HtmlGenericControl)this.FindControl("divProductReferral");
			this.ltlProductSendGifts = (Literal)this.FindControl("ltlProductSendGifts");
			this.ltlPromotionSendGifts = (Literal)this.FindControl("ltlPromotionSendGifts");
			this.setDeliverRegion = (Common_SetDeliveryRegion)this.FindControl("setDeliverRegion");
			this.divqq = (HtmlGenericControl)this.FindControl("divqq");
			this.lblPaymentStartDate = (Label)this.FindControl("lblPaymentStartDate");
			this.lblPaymentEndDate = (Label)this.FindControl("lblPaymentEndDate");
			this.lblDelivery = (Label)this.FindControl("lblDelivery");
			this.lblDepositPercent = (Label)this.FindControl("lblDepositPercent");
			this.lblDeposit = (Label)this.FindControl("lblDeposit");
			this.lblFinalPaymentPercent = (Label)this.FindControl("lblFinalPaymentPercent");
			this.lblFinalPayment = (Label)this.FindControl("lblFinalPayment");
			this.hidEndDate = (HtmlInputHidden)this.FindControl("hidEndDate");
			this.hidNowDate = (HtmlInputHidden)this.FindControl("hidNowDate");
			this.hidDepositPercent = (HtmlInputHidden)this.FindControl("hidDepositPercent");
			this.hidDeposit = (HtmlInputHidden)this.FindControl("hidDeposit");
			ThemedTemplatedRepeater themedTemplatedRepeater = (ThemedTemplatedRepeater)this.FindControl("rptOtherProducts");
			this.aCountDownUrl.Visible = false;
			if (this.Page.IsPostBack)
			{
				return;
			}
			if (this.imgQrCode != null)
			{
				string text = "/Storage/master/QRCode/" + HttpContext.Current.Request.Url.Host + "_" + this.productId + ".png";
				Globals.CreateQRCode(HttpContext.Current.Request.Url.ToString(), text, false, ImageFormats.Png);
				this.imgQrCode.ImageUrl = text;
			}
			ProductBrowseInfo productBrowseInfo = ProductBrowser.GetProductBrowseInfo(this.productId, null, this.sitesettings.OpenMultStore, -1);
			if (productBrowseInfo.Product == null || productBrowseInfo.Product.SaleStatus == ProductSaleStatus.Delete)
			{
				this.Page.Response.Redirect("/ProductDelete.aspx");
				return;
			}
			this.setDeliverRegion.ShippingTemplateId = productBrowseInfo.Product.ShippingTemplateId;
			this.setDeliverRegion.Volume = productBrowseInfo.Product.Weight;
			this.setDeliverRegion.Weight = productBrowseInfo.Product.Weight;
			this.ActivityBusiness();
			if (this.hidCartQuantity != null)
			{
				this.hidCartQuantity.Value = ShoppingCartProcessor.GetQuantity_Product(productBrowseInfo.Product.ProductId);
			}
			IEnumerable value = from item in productBrowseInfo.Product.Skus
			select item.Value;
			if (JsonConvert.SerializeObject(productBrowseInfo.DbSKUs) != null)
			{
				this.hidden_skuItem.Value = JsonConvert.SerializeObject(productBrowseInfo.DbSKUs);
			}
			if (this.hidden_skus != null)
			{
				this.hidden_skus.Value = JsonConvert.SerializeObject(value);
			}
			if (productBrowseInfo.Product.SaleStatus == ProductSaleStatus.UnSale)
			{
				this.Page.Response.Redirect(base.GetRouteUrl("unproductdetails", new
				{
					ProductId = this.productId
				}));
			}
			if (productBrowseInfo.Product.SaleStatus == ProductSaleStatus.OnStock)
			{
				this.Page.Response.Redirect("/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("该商品已入库"));
				return;
			}
			this.hidden_productId.Value = this.productId.ToString();
			this.LoadPageSearch(productBrowseInfo.Product);
			if (this.lbUserProductRefer != null && this.sitesettings.OpenReferral == 1 && this.sitesettings.ShowDeductInProductPage)
			{
				this.lbUserProductRefer.product = productBrowseInfo.Product;
			}
			HyperLink hyperLink = this.hpkProductConsultations;
			int num = productBrowseInfo.ConsultationCount;
			hyperLink.Text = "查看全部" + num.ToString() + "条咨询";
			Literal literal = this.ltlConsultation;
			num = productBrowseInfo.ConsultationCount;
			literal.Text = num.ToString();
			Literal literal2 = this.ltlSaleCount;
			num = productBrowseInfo.SaleCount;
			literal2.Text = num.ToString();
			Literal literal3 = this.ltlReviewCount;
			num = productBrowseInfo.ReviewCount;
			literal3.Text = num.ToString();
			Literal literal4 = this.litReviewCount;
			num = productBrowseInfo.ReviewCount;
			literal4.Text = num.ToString();
			this.hpkProductConsultations.NavigateUrl = $"ProductConsultationsAndReplay.aspx?productId={this.productId}";
			this.LoadProductInfo(productBrowseInfo.Product, productBrowseInfo.BrandName);
			HtmlInputHidden htmlInputHidden = this.hidNowDate;
			DateTime dateTime = DateTime.Now;
			htmlInputHidden.Value = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
			HtmlInputHidden htmlInputHidden2 = this.hidEndDate;
			dateTime = productPreSaleInfo.PreSaleEndDate;
			htmlInputHidden2.Value = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
			Label label = this.lblPaymentStartDate;
			dateTime = productPreSaleInfo.PaymentStartDate;
			label.Text = dateTime.ToString("yyyy.MM.dd");
			Label label2 = this.lblPaymentEndDate;
			dateTime = productPreSaleInfo.PaymentEndDate;
			label2.Text = dateTime.ToString("yyyy.MM.dd");
			Label label3 = this.lblDelivery;
			object text2;
			if (productPreSaleInfo.DeliveryDays <= 0)
			{
				dateTime = productPreSaleInfo.DeliveryDate.Value;
				text2 = dateTime.ToString("yyyy.MM.dd") + "前发货";
			}
			else
			{
				text2 = "尾款支付后" + productPreSaleInfo.DeliveryDays + "天发货";
			}
			label3.Text = (string)text2;
			decimal d;
			if (productPreSaleInfo.DepositPercent > 0)
			{
				this.lblDepositPercent.Text = "定金:" + productPreSaleInfo.DepositPercent + "%";
				this.lblFinalPaymentPercent.Text = "尾款:" + (100 - productPreSaleInfo.DepositPercent) + "%";
				decimal num2 = Math.Round(productBrowseInfo.Product.MinSalePrice * (decimal)productPreSaleInfo.DepositPercent / 100m, 2);
				this.lblDeposit.Text = "￥" + num2;
				this.lblFinalPayment.Text = "￥" + Math.Round(productBrowseInfo.Product.MinSalePrice - num2, 2);
				HtmlInputHidden htmlInputHidden3 = this.hidDepositPercent;
				num = productPreSaleInfo.DepositPercent;
				htmlInputHidden3.Value = num.ToString();
			}
			else
			{
				this.lblDeposit.Text = "定金:￥" + productPreSaleInfo.Deposit;
				this.lblFinalPayment.Text = "尾款:￥" + Math.Round(productBrowseInfo.Product.MinSalePrice - productPreSaleInfo.Deposit, 2);
				HtmlInputHidden htmlInputHidden4 = this.hidDeposit;
				d = productPreSaleInfo.Deposit;
				htmlInputHidden4.Value = d.ToString();
			}
			this.btnBuy.Stock = (this.sitesettings.OpenMultStore ? productBrowseInfo.Product.DefaultSku.MaxStock : productBrowseInfo.Product.Stock);
			this.ltlUnit.Text = productBrowseInfo.Product.Unit;
			this.divqq.Visible = (this.sitesettings.ServiceIsOpen == "1");
			MemberInfo user = HiContext.Current.User;
			if (user != null && user.IsReferral() && (!(this.sitesettings.SubMemberDeduct <= decimal.Zero) || productBrowseInfo.Product.SubMemberDeduct.HasValue))
			{
				if (!productBrowseInfo.Product.SubMemberDeduct.HasValue)
				{
					goto IL_0d79;
				}
				decimal? subMemberDeduct = productBrowseInfo.Product.SubMemberDeduct;
				d = default(decimal);
				if (!(subMemberDeduct.GetValueOrDefault() <= d) || !subMemberDeduct.HasValue)
				{
					goto IL_0d79;
				}
			}
			goto IL_0d9f;
			IL_0da0:
			int num3;
			if (num3 != 0)
			{
				this.divProductReferral.Visible = false;
			}
			BrowsedProductQueue.EnQueue(this.productId);
			this.images.ImageInfo = productBrowseInfo.Product;
			if (productBrowseInfo.DbAttribute != null)
			{
				this.rptExpandAttributes.DataSource = productBrowseInfo.DbAttribute;
				this.rptExpandAttributes.DataBind();
			}
			if (productBrowseInfo.DbSKUs != null)
			{
				this.skuSelector.ProductId = this.productId;
				this.skuSelector.DataSource = productBrowseInfo.DbSKUs;
			}
			if (productBrowseInfo.Product.BrandId.HasValue)
			{
				BrandCategoryInfo brandCategory = CatalogHelper.GetBrandCategory(productBrowseInfo.Product.BrandId.Value);
				if (brandCategory != null && !string.IsNullOrEmpty(brandCategory.Logo))
				{
					this.imgBrand.ImageUrl = brandCategory.Logo;
					this.aBrand.HRef = base.GetRouteUrl("branddetails", new
					{
						brandId = brandCategory.BrandId
					});
				}
			}
			int supplierId = productBrowseInfo.Product.SupplierId;
			if (supplierId > 0)
			{
				SupplierInfo supplierById = SupplierHelper.GetSupplierById(supplierId);
				if (!string.IsNullOrEmpty(supplierById.Picture))
				{
					this.imgBrand.ImageUrl = supplierById.Picture;
				}
				else if (productBrowseInfo.Product.BrandId.HasValue)
				{
					BrandCategoryInfo brandCategory2 = CatalogHelper.GetBrandCategory(productBrowseInfo.Product.BrandId.Value);
					if (brandCategory2 != null && !string.IsNullOrEmpty(brandCategory2.Logo))
					{
						this.imgBrand.ImageUrl = brandCategory2.Logo;
						this.aBrand.HRef = base.GetRouteUrl("branddetails", new
						{
							brandId = brandCategory2.BrandId
						});
					}
				}
				else
				{
					this.imgBrand.Visible = false;
				}
				this.litSupplierName.Text = supplierById.SupplierName;
				this.hidSupName.Value = supplierById.SupplierName;
			}
			else
			{
				this.litSupplierName.Visible = false;
				if (productBrowseInfo.Product.BrandId.HasValue)
				{
					BrandCategoryInfo brandCategory3 = CatalogHelper.GetBrandCategory(productBrowseInfo.Product.BrandId.Value);
					if (brandCategory3 != null && !string.IsNullOrEmpty(brandCategory3.Logo))
					{
						this.imgBrand.ImageUrl = brandCategory3.Logo;
						this.aBrand.HRef = base.GetRouteUrl("branddetails", new
						{
							brandId = brandCategory3.BrandId
						});
					}
				}
			}
			if (productBrowseInfo.DBConsultations != null)
			{
				this.consultations.DataSource = productBrowseInfo.DBConsultations;
				this.consultations.DataBind();
			}
			if (productBrowseInfo.DbCorrelatives != null)
			{
				this.correlative.DataSource = productBrowseInfo.DbCorrelatives;
				this.correlative.DataBind();
			}
			this.BindOrderPromotions();
			if (!this.divOrderPromotions.Visible && !this.divOrderPromotions2.Visible && !this.divOrderPromotions3.Visible)
			{
				this.divCuxiao.Style.Add("display", "none");
			}
			if (this.rptOnlineService != null)
			{
				IList<OnlineServiceInfo> allOnlineService = OnlineServiceHelper.GetAllOnlineService(0, 1);
				IList<OnlineServiceInfo> allOnlineService2 = OnlineServiceHelper.GetAllOnlineService(0, 2);
				if (allOnlineService2 != null)
				{
					foreach (OnlineServiceInfo item in allOnlineService2)
					{
						allOnlineService.Add(item);
					}
				}
				this.rptOnlineService.DataSource = allOnlineService;
				this.rptOnlineService.DataBind();
			}
			return;
			IL_0d9f:
			num3 = 1;
			goto IL_0da0;
			IL_0d79:
			if (HiContext.Current.SiteSettings.OpenReferral == 1)
			{
				num3 = ((!HiContext.Current.SiteSettings.ShowDeductInProductPage) ? 1 : 0);
				goto IL_0da0;
			}
			goto IL_0d9f;
		}

		private string DateDiff(DateTime DateTime1, DateTime DateTime2)
		{
			string text = null;
			TimeSpan timeSpan = new TimeSpan(DateTime1.Ticks);
			TimeSpan ts = new TimeSpan(DateTime2.Ticks);
			TimeSpan timeSpan2 = timeSpan.Subtract(ts).Duration();
			string[] obj = new string[8];
			int num = timeSpan2.Days;
			obj[0] = num.ToString();
			obj[1] = "天";
			num = timeSpan2.Hours;
			obj[2] = num.ToString();
			obj[3] = "小时";
			num = timeSpan2.Minutes;
			obj[4] = num.ToString();
			obj[5] = "分钟";
			num = timeSpan2.Seconds;
			obj[6] = num.ToString();
			obj[7] = "秒";
			return string.Concat(obj);
		}

		private void BindOrderPromotions()
		{
			string phonePriceByProductId = PromoteHelper.GetPhonePriceByProductId(this.productId);
			StoreActivityEntityList storeActivityEntity = PromoteHelper.GetStoreActivityEntity(0, 0);
			if (storeActivityEntity.FullAmountReduceList.Count > 0)
			{
				HtmlGenericControl htmlGenericControl = this.divOrderPromotions;
				Literal literal = this.ltlOrderPromotion;
				bool visible = literal.Visible = true;
				htmlGenericControl.Visible = visible;
				string text = (from t in storeActivityEntity.FullAmountReduceList
				select t.ActivityName).Aggregate((string t, string n) => t + "，" + n);
				this.ltlOrderPromotion.Text = text;
			}
			string productPromotionsInfo = this.GetProductPromotionsInfo();
			if (!string.IsNullOrEmpty(productPromotionsInfo))
			{
				this.ltlPromotionSendGifts.Text = productPromotionsInfo;
				HtmlGenericControl htmlGenericControl2 = this.divOrderPromotions4;
				Literal literal2 = this.ltlPromotionSendGifts;
				bool visible = literal2.Visible = true;
				htmlGenericControl2.Visible = visible;
			}
			if (storeActivityEntity.FullAmountSentGiftList.Count > 0 && storeActivityEntity.FullAmountSentGiftList.Count > 0)
			{
				string text2 = (from t in storeActivityEntity.FullAmountSentGiftList
				select t.ActivityName).Aggregate((string t, string n) => t + "，" + n);
				HtmlGenericControl htmlGenericControl3 = this.divOrderPromotions2;
				Literal literal3 = this.ltlProductSendGifts;
				bool visible = literal3.Visible = true;
				htmlGenericControl3.Visible = visible;
				this.ltlProductSendGifts.Text = text2;
			}
			if (storeActivityEntity.FullAmountSentFreightList.Count > 0)
			{
				HtmlGenericControl htmlGenericControl4 = this.divOrderPromotions3;
				Literal literal4 = this.ltlOrderPromotion_free;
				bool visible = literal4.Visible = true;
				htmlGenericControl4.Visible = visible;
				string text3 = (from t in storeActivityEntity.FullAmountSentFreightList
				select t.ActivityName).Aggregate((string t, string n) => t + "，" + n);
				this.ltlOrderPromotion_free.Text = text3;
			}
		}

		private string GetProductPromotionsInfo()
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
					promsg = "";
					giftDetailsByGiftIds.ForEach(delegate(GiftInfo giftinfo)
					{
						promsg += string.Format("<em><a href=\"{0}\"><img src=\"{1}\" title=\"{2}\"></a></em><b> ×1 </b>", base.GetRouteUrl("FavourableDetails", new
						{
							activityId = promotion.ActivityId
						}), Globals.GetImageServerUrl("http://", giftinfo.ThumbnailUrl40), giftinfo.Name);
					});
					promsg = (promsg ?? "");
				}
			}
			else if (promotion.PromoteType == PromoteType.SentProduct)
			{
				promsg += string.Format("{0}", (promotion.Name.Length > 40) ? (promotion.Name.Substring(0, 40) + "...") : promotion.Name);
			}
			return promsg;
		}

		private void ActivityBusiness()
		{
			CountDownInfo countDownInfo = PromoteHelper.ActiveCountDownByProductId(this.productId, 0);
			GroupBuyInfo groupBuyInfo = PromoteHelper.ActiveGroupBuyByProductId(this.productId);
			if (countDownInfo != null)
			{
				this.Page.Response.Redirect("/CountDownProductsDetails.aspx?countDownId=" + countDownInfo.CountDownId);
			}
			else if (groupBuyInfo != null)
			{
				this.Page.Response.Redirect("/GroupBuyProductDetails.aspx?groupBuyId=" + groupBuyInfo.GroupBuyId);
			}
			else
			{
				int activityStartsImmediatelyAboutCountDown = PromoteHelper.GetActivityStartsImmediatelyAboutCountDown(this.productId);
				if (activityStartsImmediatelyAboutCountDown > 0)
				{
					this.aCountDownUrl.Text = "该商品即将参与抢购活动，     去看看";
					this.aCountDownUrl.NavigateUrl = "/CountDownProductsDetails.aspx?countDownId=" + activityStartsImmediatelyAboutCountDown;
					this.aCountDownUrl.Style.Add("color", "red");
					this.aCountDownUrl.Visible = true;
				}
				else
				{
					int activityStartsImmediatelyAboutGroupBuy = PromoteHelper.GetActivityStartsImmediatelyAboutGroupBuy(this.productId);
					if (activityStartsImmediatelyAboutGroupBuy > 0)
					{
						this.aCountDownUrl.Text = "该商品即将参与团购活动，     去看看";
						this.aCountDownUrl.NavigateUrl = "/GroupBuyProductDetails.aspx?groupBuyId=" + activityStartsImmediatelyAboutGroupBuy;
						this.aCountDownUrl.Style.Add("color", "red");
						this.aCountDownUrl.Visible = true;
					}
				}
			}
		}

		private void LoadPageSearch(ProductInfo productDetails)
		{
			if (!string.IsNullOrEmpty(productDetails.Meta_Keywords))
			{
				MetaTags.AddMetaKeywords(productDetails.Meta_Keywords, HiContext.Current.Context);
			}
			if (!string.IsNullOrEmpty(productDetails.Meta_Description))
			{
				MetaTags.AddMetaDescription(productDetails.Meta_Description, HiContext.Current.Context);
			}
			if (!string.IsNullOrEmpty(productDetails.Title))
			{
				PageTitle.AddSiteNameTitle(productDetails.Title);
			}
			else
			{
				PageTitle.AddSiteNameTitle(productDetails.ProductName);
			}
		}

		private void LoadProductInfo(ProductInfo productDetails, string brandName)
		{
			if (this.common_Location != null && !string.IsNullOrEmpty(productDetails.MainCategoryPath))
			{
				this.common_Location.CateGoryPath = productDetails.MainCategoryPath.Remove(productDetails.MainCategoryPath.Length - 1);
				this.common_Location.ProductName = productDetails.ProductName;
			}
			this.lblSku.Text = productDetails.SKU;
			this.lblSku.Value = productDetails.SkuId;
			this.litProductName.Text = productDetails.ProductName;
			int num;
			if (!this.sitesettings.OpenMultStore)
			{
				this.lblStock.Stock = productDetails.Stock;
				this.litUnit.Text = productDetails.Unit;
				this.hidIsOpenMultiStore.Value = "0";
			}
			else
			{
				this.lblStock.Stock = productDetails.DefaultSku.Stock;
				StockLabel stockLabel = this.lblStock;
				num = productDetails.Stock;
				stockLabel.ShowText = num.ToString();
				this.litUnit.Visible = false;
				this.hidIsOpenMultiStore.Value = "1";
			}
			if (this.litSaleCounts != null)
			{
				Literal literal = this.litSaleCounts;
				num = productDetails.ShowSaleCounts;
				literal.Text = num.ToString();
			}
			if (productDetails.MinSalePrice == productDetails.MaxSalePrice)
			{
				this.lblBuyPrice.Text = productDetails.MinSalePrice.F2ToString("f2");
				this.lblTotalPrice.Value = productDetails.MinSalePrice;
			}
			else
			{
				this.lblBuyPrice.Text = productDetails.MinSalePrice.F2ToString("f2") + " - " + productDetails.MaxSalePrice.F2ToString("f2");
			}
			this.litDescription.Text = productDetails.Description;
			if (this.litDescription != null)
			{
				string text = "";
				Regex regex = new Regex("<script[^>]*?>.*?</script>", RegexOptions.IgnoreCase);
				if (!string.IsNullOrWhiteSpace(productDetails.Description))
				{
					text = regex.Replace(productDetails.Description, "");
				}
				text = text.Replace("src", "data-url");
				this.litDescription.Text = text;
			}
			if (this.litShortDescription != null)
			{
				this.litShortDescription.Text = productDetails.ShortDescription;
			}
		}
	}
}
