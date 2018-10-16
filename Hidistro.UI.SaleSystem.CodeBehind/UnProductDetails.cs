using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Shopping;
using Hidistro.SaleSystem.Store;
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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class UnProductDetails : HtmlTemplatedWebControl
	{
		private int productId;

		private SiteSettings sitesettings = SettingsManager.GetMasterSettings();

		private Common_Location common_Location;

		private Literal litProductName;

		private StockLabel lblStock;

		private Literal litUnit;

		private Literal litSaleCounts;

		private FormatedMoneyLabel lblMarkerPrice;

		private Label lblBuyPrice;

		private TotalLabel lblTotalPrice;

		private Literal litDescription;

		private Literal litShortDescription;

		private BuyButton btnBuy;

		private AddCartButton btnaddgouwu;

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

		private UserProductReferLabel lbUserProductRefer;

		private HtmlInputHidden hidIsOpenMultiStore;

		private HyperLink aCountDownUrl;

		private Image imgQrCode;

		private Image phonePriceQrCode;

		private HtmlGenericControl liCode;

		private HtmlGenericControl divGift;

		private HtmlInputHidden hidden_productId;

		private HtmlInputHidden hidHasStores;

		private HiImage imgBrand;

		private Literal ltlGiftName;

		private Literal ltlGiftNum;

		private HtmlAnchor aBrand;

		private SkuLabel lblSku;

		private HtmlImage imgpdorequest;

		private HtmlImage imgTakeonstore;

		private HtmlImage imgCustomerService;

		private Literal ltlOrderPromotion;

		private HtmlGenericControl divOrderPromotions;

		private HtmlGenericControl divCuxiao;

		private HtmlGenericControl divPhonePrice;

		private Literal litPhonePrice;

		private Literal litPhonePriceEndDate;

		private Literal ltlUnit;

		private HtmlGenericControl divProductReferral;

		private HtmlGenericControl divqq;

		private Common_SetDeliveryRegion setDeliverRegion;

		private HtmlInputHidden hidShowCombinationBuy;

		private HtmlInputHidden hidCombinationId;

		private HtmlImage imgMainPic;

		private HtmlAnchor aMainName;

		private Label lblMainPrice;

		private ThemedTemplatedRepeater rptOtherProducts;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-UnProductDetails.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			if (!int.TryParse(base.GetParameter("productId", false), out this.productId))
			{
				base.GotoResourceNotFound();
			}
			this.hiddenpid = (HtmlInputHidden)this.FindControl("hiddenpid");
			this.hidCartQuantity = (HtmlInputHidden)this.FindControl("txCartQuantity");
			this.hiddenpid.Value = this.productId.ToString();
			this.common_Location = (Common_Location)this.FindControl("common_Location");
			this.litProductName = (Literal)this.FindControl("litProductName");
			this.lblStock = (StockLabel)this.FindControl("lblStock");
			this.litUnit = (Literal)this.FindControl("litUnit");
			this.litSaleCounts = (Literal)this.FindControl("litSaleCounts");
			this.lblMarkerPrice = (FormatedMoneyLabel)this.FindControl("lblMarkerPrice");
			this.lblBuyPrice = (Label)this.FindControl("lblBuyPrice");
			this.lblTotalPrice = (TotalLabel)this.FindControl("lblTotalPrice");
			this.lblSku = (SkuLabel)this.FindControl("lblSku");
			this.litDescription = (Literal)this.FindControl("litDescription");
			this.litShortDescription = (Literal)this.FindControl("litShortDescription");
			this.btnBuy = (BuyButton)this.FindControl("btnBuy");
			this.btnaddgouwu = (AddCartButton)this.FindControl("btnaddgouwu");
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
			this.phonePriceQrCode = (Image)this.FindControl("phonePriceQrCode");
			this.liCode = (HtmlGenericControl)this.FindControl("liCode");
			this.hidden_productId = (HtmlInputHidden)this.FindControl("hidden_productId");
			this.hidHasStores = (HtmlInputHidden)this.FindControl("hidHasStores");
			this.divGift = (HtmlGenericControl)this.FindControl("divGift");
			this.ltlGiftName = (Literal)this.FindControl("ltlGiftName");
			this.ltlGiftNum = (Literal)this.FindControl("ltlGiftNum");
			this.aBrand = (HtmlAnchor)this.FindControl("aBrand");
			this.imgBrand = (HiImage)this.FindControl("imgBrand");
			this.imgpdorequest = (HtmlImage)this.FindControl("imgpdorequest");
			this.imgTakeonstore = (HtmlImage)this.FindControl("imgTakeonstore");
			this.imgCustomerService = (HtmlImage)this.FindControl("imgCustomerService");
			this.ltlOrderPromotion = (Literal)this.FindControl("ltlOrderPromotion");
			this.divOrderPromotions = (HtmlGenericControl)this.FindControl("divOrderPromotions");
			this.divPhonePrice = (HtmlGenericControl)this.FindControl("divPhonePrice");
			this.litPhonePrice = (Literal)this.FindControl("litPhonePrice");
			this.litPhonePriceEndDate = (Literal)this.FindControl("litPhonePriceEndDate");
			this.divCuxiao = (HtmlGenericControl)this.FindControl("divCuxiao");
			this.ltlUnit = (Literal)this.FindControl("ltlUnit");
			this.divProductReferral = (HtmlGenericControl)this.FindControl("divProductReferral");
			this.ltlProductSendGifts = (Literal)this.FindControl("ltlProductSendGifts");
			this.setDeliverRegion = (Common_SetDeliveryRegion)this.FindControl("setDeliverRegion");
			this.hidShowCombinationBuy = (HtmlInputHidden)this.FindControl("hidShowCombinationBuy");
			this.hidCombinationId = (HtmlInputHidden)this.FindControl("hidCombinationId");
			this.imgMainPic = (HtmlImage)this.FindControl("imgMainPic");
			this.divqq = (HtmlGenericControl)this.FindControl("divqq");
			HtmlAnchor htmlAnchor = (HtmlAnchor)this.FindControl("aMainName");
			this.lblMainPrice = (Label)this.FindControl("lblMainPrice");
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
			if (this.phonePriceQrCode != null)
			{
				string text2 = "/Storage/master/QRCode/" + HttpContext.Current.Request.Url.Host + "_" + this.productId + ".png";
				Globals.CreateQRCode(HttpContext.Current.Request.Url.ToString(), text2, false, ImageFormats.Png);
				this.phonePriceQrCode.ImageUrl = text2;
			}
			if (this.liCode != null && HiContext.Current.SiteSettings.OpenAliho == 0 && HiContext.Current.SiteSettings.OpenVstore == 0 && HiContext.Current.SiteSettings.OpenWap == 0 && HiContext.Current.SiteSettings.OpenMobbile == 0)
			{
				this.liCode.Visible = false;
			}
			ProductBrowseInfo productBrowseInfo = ProductBrowser.GetProductBrowseInfo(this.productId, null, this.sitesettings.OpenMultStore, 0);
			if (productBrowseInfo.Product == null || productBrowseInfo.Product.SaleStatus == ProductSaleStatus.Delete)
			{
				this.Page.Response.Redirect("/ProductDelete.aspx");
				return;
			}
			if (productBrowseInfo.Product.SaleStatus == ProductSaleStatus.OnSale)
			{
				this.Page.Response.Redirect(base.GetRouteUrl("productdetails", new
				{
					ProductId = this.productId
				}));
			}
			if (productBrowseInfo.Product.SaleStatus == ProductSaleStatus.OnStock)
			{
				this.Page.Response.Redirect("/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("该商品已入库"));
				return;
			}
			this.setDeliverRegion.ShippingTemplateId = productBrowseInfo.Product.ShippingTemplateId;
			this.setDeliverRegion.Volume = productBrowseInfo.Product.Weight;
			this.setDeliverRegion.Weight = productBrowseInfo.Product.Weight;
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
			this.btnBuy.Stock = (this.sitesettings.OpenMultStore ? productBrowseInfo.Product.DefaultSku.MaxStock : productBrowseInfo.Product.Stock);
			this.btnBuy.Visible = false;
			this.ltlUnit.Text = productBrowseInfo.Product.Unit;
			this.divqq.Visible = (this.sitesettings.ServiceIsOpen == "1");
			MemberInfo user = HiContext.Current.User;
			if (user != null && user.IsReferral() && (!(this.sitesettings.SubMemberDeduct <= decimal.Zero) || productBrowseInfo.Product.SubMemberDeduct.HasValue))
			{
				if (!productBrowseInfo.Product.SubMemberDeduct.HasValue)
				{
					goto IL_0b27;
				}
				decimal? subMemberDeduct = productBrowseInfo.Product.SubMemberDeduct;
				if (!(subMemberDeduct.GetValueOrDefault() <= default(decimal)) || !subMemberDeduct.HasValue)
				{
					goto IL_0b27;
				}
			}
			goto IL_0b4d;
			IL_0b4d:
			int num2 = 1;
			goto IL_0b4e;
			IL_0b27:
			if (HiContext.Current.SiteSettings.OpenReferral == 1)
			{
				num2 = ((!HiContext.Current.SiteSettings.ShowDeductInProductPage) ? 1 : 0);
				goto IL_0b4e;
			}
			goto IL_0b4d;
			IL_0b4e:
			if (num2 != 0)
			{
				this.divProductReferral.Visible = false;
			}
			this.btnaddgouwu.Stock = (this.sitesettings.OpenMultStore ? productBrowseInfo.Product.DefaultSku.MaxStock : productBrowseInfo.Product.Stock);
			this.btnaddgouwu.Visible = false;
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
			if (SalesHelper.IsSupportPodrequest() && productBrowseInfo.Product.SupplierId == 0)
			{
				this.imgpdorequest.Visible = true;
			}
			if (SettingsManager.GetMasterSettings().OpenMultStore && StoresHelper.ProductHasStores(this.productId) && productBrowseInfo.Product.SupplierId == 0)
			{
				this.imgTakeonstore.Visible = true;
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
			string productPromotionsInfo = this.GetProductPromotionsInfo();
			this.ltlProductSendGifts.Text = productPromotionsInfo;
			if (!this.divOrderPromotions.Visible && productPromotionsInfo == string.Empty)
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
		}

		private void BindOrderPromotions()
		{
			DataTable productDetailOrderPromotions = PromoteHelper.GetProductDetailOrderPromotions();
			if (productDetailOrderPromotions.Rows.Count > 0)
			{
				this.divOrderPromotions.Visible = true;
				string text = string.Empty;
				foreach (DataRow row in productDetailOrderPromotions.Rows)
				{
					text = text + row["Name"].ToNullString() + ",";
				}
				text = text.TrimEnd(',');
				this.ltlOrderPromotion.Text = text;
			}
			string phonePriceByProductId = PromoteHelper.GetPhonePriceByProductId(this.productId);
			if (!string.IsNullOrEmpty(phonePriceByProductId) && (this.sitesettings.OpenAliho == 1 || this.sitesettings.OpenMobbile == 1 || this.sitesettings.OpenVstore == 1 || this.sitesettings.OpenWap == 1))
			{
				this.divPhonePrice.Visible = true;
				string s = phonePriceByProductId.Split(',')[0];
				string[] array = this.lblBuyPrice.Text.Split('-');
				decimal num = decimal.Parse(array[0].Trim()) - decimal.Parse(s);
				this.litPhonePrice.Text = ((num > decimal.Zero) ? num : decimal.Zero).F2ToString("f2");
				this.litPhonePriceEndDate.Text = phonePriceByProductId.Split(',')[1];
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
					promsg += "<div id=\"divGift\"><i class=\"tag2\">赠</i>";
					giftDetailsByGiftIds.ForEach(delegate(GiftInfo giftinfo)
					{
						promsg += string.Format("<em><a href=\"{0}\"><img src=\"{1}\" title=\"{2}\"></a></em><b> ×1 </b>", base.GetRouteUrl("FavourableDetails", new
						{
							activityId = promotion.ActivityId
						}), Globals.GetImageServerUrl("http://", giftinfo.ThumbnailUrl40), giftinfo.Name);
					});
					promsg += "</div>";
				}
			}
			else if (promotion.PromoteType == PromoteType.SentProduct)
			{
				promsg += string.Format("<div id=\"divGift\"><i class=\"tag2\">赠</i><b>{0}</b></div>", (promotion.Name.Length > 40) ? (promotion.Name.Substring(0, 40) + "...") : promotion.Name);
			}
			return promsg;
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
			try
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
				this.lblMarkerPrice.Money = productDetails.MarketPrice;
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
			catch (Exception ex)
			{
				Globals.WriteLog("unproduct.txt", ex.Source);
				Globals.WriteLog("unproduct.txt", ex.StackTrace);
				Globals.WriteLog("unproduct.txt", ex.TargetSite.ToString());
			}
		}
	}
}
