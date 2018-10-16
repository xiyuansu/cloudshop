using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Store;
using Hidistro.Entities.Supplier;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Sales;
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
	public class ProductDetails : HtmlTemplatedWebControl
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

		private Literal ltlOrderPromotion_free;

		private HtmlGenericControl divOrderPromotions;

		private HtmlGenericControl divCuxiao;

		private HtmlGenericControl divPhonePrice;

		private HtmlGenericControl divOrderPromotions2;

		private HtmlGenericControl divOrderPromotions3;

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

		private HtmlInputHidden hidSupName;

		private HtmlGenericControl spdiscount;

		private Literal litSupplierName;

		private HtmlTableRow buyProduct;

		private HtmlTableRow serviceProduct;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-ProductDetails.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			if (!int.TryParse(base.GetParameter("productId", false), out this.productId))
			{
				base.GotoResourceNotFound();
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
			this.ltlOrderPromotion_free = (Literal)this.FindControl("ltlOrderPromotion_free");
			this.divOrderPromotions = (HtmlGenericControl)this.FindControl("divOrderPromotions");
			this.divOrderPromotions2 = (HtmlGenericControl)this.FindControl("divOrderPromotions2");
			this.divOrderPromotions3 = (HtmlGenericControl)this.FindControl("divOrderPromotions3");
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
			this.spdiscount = (HtmlGenericControl)this.FindControl("spdiscount");
			this.aCountDownUrl.Visible = false;
			this.buyProduct = (HtmlTableRow)this.FindControl("buyProduct");
			this.serviceProduct = (HtmlTableRow)this.FindControl("serviceProduct");
			if (this.Page.IsPostBack)
			{
				return;
			}
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (this.spdiscount != null && HiContext.Current.User.UserId > 0)
			{
				MemberGradeInfo memberGrade = MemberProcessor.GetMemberGrade(HiContext.Current.User.GradeId);
				this.spdiscount.Visible = true;
				this.spdiscount.InnerHtml = "<strong class='vip_price'><img src='/templates/pccommon/images/vip_price.png' />" + memberGrade.Name + "价</strong>";
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
			if (productBrowseInfo.Product.SaleStatus == ProductSaleStatus.UnSale)
			{
				this.Page.Response.Redirect(base.GetRouteUrl("unproductdetails", new
				{
					ProductId = this.productId
				}));
			}
			if (productBrowseInfo.Product.SupplierId > 0 && productBrowseInfo.Product.AuditStatus != ProductAuditStatus.Pass)
			{
				this.Page.Response.Redirect(base.GetRouteUrl("unproductdetailsaudit", new
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
			this.ltlUnit.Text = productBrowseInfo.Product.Unit;
			this.divqq.Visible = (this.sitesettings.ServiceIsOpen == "1");
			MemberInfo user = HiContext.Current.User;
			if (user != null && user.IsReferral() && (!(this.sitesettings.SubMemberDeduct <= decimal.Zero) || productBrowseInfo.Product.SubMemberDeduct.HasValue))
			{
				if (!productBrowseInfo.Product.SubMemberDeduct.HasValue)
				{
					goto IL_0c9e;
				}
				decimal? subMemberDeduct = productBrowseInfo.Product.SubMemberDeduct;
				if (!(subMemberDeduct.GetValueOrDefault() <= default(decimal)) || !subMemberDeduct.HasValue)
				{
					goto IL_0c9e;
				}
			}
			goto IL_0cd8;
			IL_0cd9:
			int num2;
			if (num2 != 0)
			{
				this.divProductReferral.Visible = false;
			}
			this.btnaddgouwu.Stock = (this.sitesettings.OpenMultStore ? productBrowseInfo.Product.DefaultSku.MaxStock : productBrowseInfo.Product.Stock);
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
				else
				{
					this.imgBrand.Visible = false;
				}
				this.litSupplierName.Text = "<a href=\"/SupplierAbout?SupplierId=" + supplierById.SupplierId + "\">" + supplierById.SupplierName + "</a>";
				this.hidSupName.Value = supplierById.SupplierName;
			}
			else
			{
				this.litSupplierName.Visible = false;
				if (productBrowseInfo.Product.BrandId.HasValue)
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
			}
			if (SalesHelper.IsSupportPodrequest() && productBrowseInfo.Product.SupplierId == 0)
			{
				this.imgpdorequest.Visible = true;
			}
			if (masterSettings.OpenMultStore)
			{
				if (StoresHelper.ProductInStoreAndIsAboveSelf(this.productId))
				{
					this.imgTakeonstore.Visible = true;
				}
			}
			else if (masterSettings.IsOpenPickeupInStore && productBrowseInfo.Product.SupplierId == 0)
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
			if (!this.divOrderPromotions.Visible && !this.divOrderPromotions2.Visible && !this.divOrderPromotions3.Visible && !this.divPhonePrice.Visible)
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
			if (productBrowseInfo.Product.Stock > 0)
			{
				CombinationBuyInfo combinationBuyByMainProductId = CombinationBuyHelper.GetCombinationBuyByMainProductId(this.productId);
				if (combinationBuyByMainProductId != null)
				{
					List<CombinationBuyandProductUnionInfo> combinationProductListByProductId = CombinationBuyHelper.GetCombinationProductListByProductId(this.productId);
					CombinationBuyandProductUnionInfo combinationBuyandProductUnionInfo = combinationProductListByProductId.FirstOrDefault((CombinationBuyandProductUnionInfo c) => c.ProductId == this.productId);
					if (combinationBuyandProductUnionInfo != null)
					{
						HtmlInputHidden htmlInputHidden = this.hidCombinationId;
						num = combinationBuyandProductUnionInfo.CombinationId;
						htmlInputHidden.Value = num.ToString();
						string value2 = string.IsNullOrEmpty(combinationBuyandProductUnionInfo.ThumbnailUrl100) ? this.sitesettings.DefaultProductThumbnail3 : combinationBuyandProductUnionInfo.ThumbnailUrl100;
						this.imgMainPic.Attributes["data-url"] = value2;
						htmlAnchor.InnerText = combinationBuyandProductUnionInfo.ProductName;
						this.lblMainPrice.Text = combinationBuyandProductUnionInfo.MinCombinationPrice.F2ToString("f2");
						this.lblMainPrice.Attributes["salePrice"] = combinationBuyandProductUnionInfo.MinSalePrice.F2ToString("f2");
					}
					combinationProductListByProductId.Remove(combinationBuyandProductUnionInfo);
					if (combinationProductListByProductId != null && combinationProductListByProductId.Count > 0)
					{
						for (int i = 0; i < combinationProductListByProductId.Count; i++)
						{
							string thumbnailUrl = string.IsNullOrEmpty(combinationProductListByProductId[i].ThumbnailUrl100) ? this.sitesettings.DefaultProductThumbnail3 : combinationProductListByProductId[i].ThumbnailUrl100;
							combinationProductListByProductId[i].ThumbnailUrl100 = thumbnailUrl;
							combinationProductListByProductId[i].Index = i + 1;
						}
						themedTemplatedRepeater.DataSource = combinationProductListByProductId;
						themedTemplatedRepeater.DataBind();
						this.hidShowCombinationBuy.Value = "1";
					}
				}
			}
			return;
			IL_0cd8:
			num2 = 1;
			goto IL_0cd9;
			IL_0c9e:
			if (HiContext.Current.SiteSettings.OpenReferral == 1 && HiContext.Current.SiteSettings.ShowDeductInProductPage && user.Referral != null)
			{
				num2 = (user.Referral.IsRepeled ? 1 : 0);
				goto IL_0cd9;
			}
			goto IL_0cd8;
		}

		private void BindOrderPromotions()
		{
			string phonePriceByProductId = PromoteHelper.GetPhonePriceByProductId(this.productId);
			StoreActivityEntityList storeActivityEntity = PromoteHelper.GetStoreActivityEntity(0, this.productId);
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
			if (storeActivityEntity.FullAmountSentGiftList.Count > 0 || !string.IsNullOrEmpty(productPromotionsInfo))
			{
				if (storeActivityEntity.FullAmountSentGiftList.Count > 0)
				{
					string text2 = (from t in storeActivityEntity.FullAmountSentGiftList
					select t.ActivityName).Aggregate((string t, string n) => t + "，" + n);
					HtmlGenericControl htmlGenericControl2 = this.divOrderPromotions2;
					Literal literal2 = this.ltlProductSendGifts;
					bool visible = literal2.Visible = true;
					htmlGenericControl2.Visible = visible;
					this.ltlProductSendGifts.Text = text2;
				}
				if (!string.IsNullOrEmpty(productPromotionsInfo))
				{
					HtmlGenericControl htmlGenericControl3 = this.divOrderPromotions2;
					Literal literal3 = this.ltlProductSendGifts;
					bool visible = literal3.Visible = true;
					htmlGenericControl3.Visible = visible;
					Literal literal4 = this.ltlProductSendGifts;
					literal4.Text = literal4.Text + (string.IsNullOrEmpty(this.ltlProductSendGifts.Text) ? "" : "，") + productPromotionsInfo;
				}
			}
			if (storeActivityEntity.FullAmountSentFreightList.Count > 0)
			{
				HtmlGenericControl htmlGenericControl4 = this.divOrderPromotions3;
				Literal literal5 = this.ltlOrderPromotion_free;
				bool visible = literal5.Visible = true;
				htmlGenericControl4.Visible = visible;
				string text3 = (from t in storeActivityEntity.FullAmountSentFreightList
				select t.ActivityName).Aggregate((string t, string n) => t + "，" + n);
				this.ltlOrderPromotion_free.Text = text3;
			}
			if (!string.IsNullOrEmpty(phonePriceByProductId) && (this.sitesettings.OpenAliho == 1 || this.sitesettings.OpenMobbile == 1 || this.sitesettings.OpenVstore == 1 || this.sitesettings.OpenWap == 1))
			{
				this.divPhonePrice.Visible = true;
				string s = phonePriceByProductId.Split(',')[0];
				string[] array = this.lblBuyPrice.Text.Split('-');
				decimal num = decimal.Parse(array[0].Trim()) - decimal.Parse(s);
				this.litPhonePrice.Text = ((num > decimal.Zero) ? num : decimal.Zero).F2ToString("f2");
				this.litPhonePriceEndDate.Text = phonePriceByProductId.Split(',')[1];
			}
			else
			{
				this.divPhonePrice.Visible = false;
			}
		}

		private string GetProductPromotionsInfo()
		{
			string text = string.Empty;
			PromotionInfo productPromotionInfo = ProductBrowser.GetProductPromotionInfo(this.productId);
			if (productPromotionInfo == null || (productPromotionInfo.PromoteType != PromoteType.SentGift && productPromotionInfo.PromoteType != PromoteType.SentProduct))
			{
				return string.Empty;
			}
			if (productPromotionInfo.PromoteType == PromoteType.SentGift)
			{
				IList<GiftInfo> giftDetailsByGiftIds = ProductBrowser.GetGiftDetailsByGiftIds(productPromotionInfo.GiftIds);
				if (giftDetailsByGiftIds.Count > 0)
				{
					text = "赠" + (from t in giftDetailsByGiftIds
					select t.Name).Aggregate((string t, string n) => t + "、" + n);
				}
			}
			else if (productPromotionInfo.PromoteType == PromoteType.SentProduct)
			{
				string str = text;
				decimal num = productPromotionInfo.Condition;
				string arg = num.ToString("f0");
				num = productPromotionInfo.DiscountValue;
				text = str + string.Format("买{0}送{1} ", arg, num.ToString("f0"));
			}
			return text;
		}

		private void ActivityBusiness()
		{
			CountDownInfo countDownInfo = PromoteHelper.ActiveCountDownByProductId(this.productId, 0);
			GroupBuyInfo groupBuyInfo = PromoteHelper.ActiveGroupBuyByProductId(this.productId);
			ProductPreSaleInfo productPreSaleInfoByProductId = ProductPreSaleHelper.GetProductPreSaleInfoByProductId(this.productId);
			if (countDownInfo != null)
			{
				this.Page.Response.Redirect("/CountDownProductsDetails.aspx?countDownId=" + countDownInfo.CountDownId);
			}
			else if (groupBuyInfo != null)
			{
				this.Page.Response.Redirect("/GroupBuyProductDetails.aspx?groupBuyId=" + groupBuyInfo.GroupBuyId);
			}
			else if (productPreSaleInfoByProductId != null && productPreSaleInfoByProductId.PreSaleEndDate >= DateTime.Now)
			{
				this.Page.Response.Redirect("/PreSaleProductDetails.aspx?PreSaleId=" + productPreSaleInfoByProductId.PreSaleId);
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
				text = text.Replace("vurl", "src");
				this.litDescription.Text = text;
			}
			if (this.litShortDescription != null)
			{
				this.litShortDescription.Text = productDetails.ShortDescription;
			}
			if (productDetails.ProductType == 1.GetHashCode())
			{
				this.serviceProduct.Visible = true;
				this.buyProduct.Visible = false;
			}
			else
			{
				this.serviceProduct.Visible = false;
				this.buyProduct.Visible = true;
			}
		}
	}
}
