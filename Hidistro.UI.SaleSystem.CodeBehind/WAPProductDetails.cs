using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Supplier;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Shopping;
using Hidistro.SaleSystem.Store;
using Hidistro.SaleSystem.Supplier;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using System;
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
	public class WAPProductDetails : WAPTemplatedWebControl
	{
		private int productId;

		private SiteSettings sitesettings = SettingsManager.GetMasterSettings();

		private WapTemplatedRepeater rptProductImages;

		private WapTemplatedRepeater rptCouponList;

		private WapTemplatedRepeater rptProductConsultations;

		private WapTemplatedRepeater rp_guest;

		private WapTemplatedRepeater rp_com;

		private Literal litProdcutName;

		private Literal litSalePrice;

		private Literal litSoldCount;

		private Literal litMarketPrice;

		private Literal litShortDescription;

		private Literal litDescription;

		private Literal litConsultationsCount;

		private Literal litReviewsCount;

		private Literal ltlcombinamaininfo;

		private Common_ExpandAttributes expandAttr;

		private HtmlInputHidden litHasCollected;

		private HtmlInputHidden hidden_skus;

		private UserProductReferLabel lbUserProductRefer;

		private HtmlButton addcartButton;

		private HtmlButton buyButton;

		private HtmlInputHidden hidden_skuItem;

		private HtmlInputHidden hidCartQuantity;

		private HtmlGenericControl divshiptoregion;

		private HtmlGenericControl divwaplocateaddress;

		private HtmlGenericControl divPodrequest;

		private HtmlGenericControl divGuest;

		private StockLabel lblStock;

		private Literal litUnit;

		private HyperLink aCountDownUrl;

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

		private HtmlGenericControl divActivities;

		private HtmlInputHidden hidden_productId;

		private Literal ltlOrderPromotion;

		private Literal ltlOrderPromotion2;

		private Literal ltlProductSendGifts2;

		private Literal ltlProductSendGifts;

		private Literal ltlOrderPromotion_free2;

		private Literal ltlOrderPromotion_free;

		private HtmlInputHidden hidCouponCount;

		private HtmlInputHidden hidHasStores;

		private Common_SKUSubmitOrder skuSubmitOrder;

		private HtmlGenericControl divConsultationEmpty;

		private HtmlGenericControl ulConsultations;

		private HtmlGenericControl divShortDescription;

		private HtmlGenericControl divCountDownUrl;

		private HtmlGenericControl divcombina;

		private ProductFreightLiteral productFreight;

		private HtmlInputHidden hidUnOnSale;

		private HtmlInputHidden hidUnAudit;

		private HtmlInputHidden hidSupplier;

		private HtmlGenericControl divPhonePrice;

		private Literal litPhonePrice;

		private HtmlGenericControl spdiscount;

		private HtmlGenericControl ulsupplier;

		private Literal litSupplierName;

		private HtmlInputHidden hidCanTakeOnStore;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VProductDetails.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (!int.TryParse(this.Page.Request.QueryString["productId"], out this.productId))
			{
				this.ShowWapMessage("错误的商品ID", "Default.aspx");
			}
			if (base.ClientType.Equals(ClientType.VShop))
			{
				FightGroupActivitiyModel fightGroupActivitiyModel = VShopHelper.GetFightGroupActivities(new FightGroupActivitiyQuery
				{
					PageIndex = 1,
					PageSize = 1,
					ProductId = this.productId,
					Status = EnumFightGroupActivitiyStatus.BeingCarried
				}).Models.FirstOrDefault();
				if (fightGroupActivitiyModel != null)
				{
					this.Page.Response.Redirect("FightGroupActivityDetails.aspx?fightGroupActivityId=" + fightGroupActivitiyModel.FightGroupActivityId);
				}
			}
			this.hidSupplier = (HtmlInputHidden)this.FindControl("hidSupplier");
			this.litSupplierName = (Literal)this.FindControl("litSupplierName");
			this.aCountDownUrl = (HyperLink)this.FindControl("aCountDownUrl");
			this.aCountDownUrl.Visible = false;
			this.divCountDownUrl = (HtmlGenericControl)this.FindControl("divCountDownUrl");
			this.hidCanTakeOnStore = (HtmlInputHidden)this.FindControl("hidCanTakeOnStore");
			this.HasActivitiesToJumpUrl();
			this.rptProductConsultations = (WapTemplatedRepeater)this.FindControl("rptProductConsultations");
			this.rptProductImages = (WapTemplatedRepeater)this.FindControl("rptProductImages");
			this.rptCouponList = (WapTemplatedRepeater)this.FindControl("rptCouponList");
			this.rp_guest = (WapTemplatedRepeater)this.FindControl("rp_guest");
			this.rp_com = (WapTemplatedRepeater)this.FindControl("rp_com");
			this.litProdcutName = (Literal)this.FindControl("litProdcutName");
			this.litSalePrice = (Literal)this.FindControl("litSalePrice");
			this.litMarketPrice = (Literal)this.FindControl("litMarketPrice");
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
			this.ltlProductSendGifts2 = (Literal)this.FindControl("ltlProductSendGifts2");
			this.ltlProductSendGifts = (Literal)this.FindControl("ltlProductSendGifts");
			this.liOrderPromotions = (HtmlGenericControl)this.FindControl("liOrderPromotions");
			this.liOrderPromotions2 = (HtmlGenericControl)this.FindControl("liOrderPromotions2");
			this.liProductSendGifts2 = (HtmlGenericControl)this.FindControl("liProductSendGifts2");
			this.liOrderPromotions_free2 = (HtmlGenericControl)this.FindControl("liOrderPromotions_free2");
			this.liOrderPromotions_free = (HtmlGenericControl)this.FindControl("liOrderPromotions_free");
			this.divActivities = (HtmlGenericControl)this.FindControl("divActivities");
			this.ltlOrderPromotion_free2 = (Literal)this.FindControl("ltlOrderPromotion_free2");
			this.ltlOrderPromotion_free = (Literal)this.FindControl("ltlOrderPromotion_free");
			this.liProductSendGifts = (HtmlGenericControl)this.FindControl("liProductSendGifts");
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
			this.hidUnAudit = (HtmlInputHidden)this.FindControl("hidUnAudit");
			this.divPhonePrice = (HtmlGenericControl)this.FindControl("divPhonePrice");
			this.litPhonePrice = (Literal)this.FindControl("litPhonePrice");
			this.spdiscount = (HtmlGenericControl)this.FindControl("spdiscount");
			this.ulsupplier = (HtmlGenericControl)this.FindControl("ulsupplier");
			this.hdAppId.Value = masterSettings.WeixinAppId;
			HtmlInputHidden htmlInputHidden = this.hidRegionId;
			int num = HiContext.Current.DeliveryScopRegionId;
			htmlInputHidden.Value = num.ToString();
			this.hidden_skuItem = (HtmlInputHidden)this.FindControl("hidden_skuItem");
			this.hidCartQuantity = (HtmlInputHidden)this.FindControl("txCartQuantity");
			this.lblStock = (StockLabel)this.FindControl("lblStock");
			this.litUnit = (Literal)this.FindControl("litUnit");
			ProductBrowseInfo wAPProductBrowseInfo = ProductBrowser.GetWAPProductBrowseInfo(this.productId, null, masterSettings.OpenMultStore, 0);
			if (wAPProductBrowseInfo.Product == null || wAPProductBrowseInfo.Product.SaleStatus == ProductSaleStatus.Delete)
			{
				this.Page.Response.Redirect("ProductDelete.aspx");
				return;
			}
			if (wAPProductBrowseInfo.Product.SaleStatus == ProductSaleStatus.OnStock)
			{
				base.GotoResourceNotFound("商品已经入库");
			}
			if (wAPProductBrowseInfo.Product.ProductType == 1.GetHashCode())
			{
				HttpContext.Current.Response.Redirect("ServiceProductDetails?productId=" + this.productId);
			}
			if (masterSettings.OpenMultStore)
			{
				if (StoresHelper.ProductInStoreAndIsAboveSelf(this.productId))
				{
					this.hidHasStores.Value = "1";
					this.hidCanTakeOnStore.Value = "1";
				}
			}
			else if (masterSettings.IsOpenPickeupInStore && wAPProductBrowseInfo.Product.SupplierId == 0)
			{
				this.hidCanTakeOnStore.Value = "1";
			}
			if (SalesHelper.IsSupportPodrequest() && wAPProductBrowseInfo.Product.SupplierId == 0)
			{
				this.divPodrequest.Visible = true;
			}
			if (wAPProductBrowseInfo.Product.SaleStatus == ProductSaleStatus.UnSale)
			{
				this.hidUnOnSale.Value = "1";
			}
			HtmlInputHidden htmlInputHidden2 = this.hidUnAudit;
			num = (int)wAPProductBrowseInfo.Product.AuditStatus;
			htmlInputHidden2.Value = num.ToString();
			if (this.spdiscount != null && HiContext.Current.User.UserId > 0)
			{
				MemberGradeInfo memberGrade = MemberProcessor.GetMemberGrade(HiContext.Current.User.GradeId);
				this.spdiscount.Visible = true;
				this.spdiscount.InnerHtml = "<strong class='vip_price'><img src='/templates/pccommon/images/vip_price.png' />" + memberGrade.Name + "价</strong>";
			}
			this.skuSubmitOrder.ProductInfo = wAPProductBrowseInfo.Product;
			this.lbUserProductRefer.product = wAPProductBrowseInfo.Product;
			this.productFreight.ShippingTemplateId = wAPProductBrowseInfo.Product.ShippingTemplateId;
			this.productFreight.Volume = wAPProductBrowseInfo.Product.Weight;
			this.productFreight.Weight = wAPProductBrowseInfo.Product.Weight;
			this.hdTitle.Value = Globals.StripAllTags(string.IsNullOrEmpty(wAPProductBrowseInfo.Product.Title) ? wAPProductBrowseInfo.Product.ProductName : wAPProductBrowseInfo.Product.Title);
			this.hdDesc.Value = Globals.StripAllTags(string.IsNullOrEmpty(wAPProductBrowseInfo.Product.ShortDescription) ? this.hdTitle.Value : wAPProductBrowseInfo.Product.ShortDescription);
			string oldValue = "/storage/master/product/images/";
			string newValue = "/storage/master/product/thumbs410/410_";
			string text = wAPProductBrowseInfo.Product.ImageUrl1;
			if (!string.IsNullOrEmpty(text))
			{
				text = text.ToLower().Replace(oldValue, newValue);
			}
			string local = string.IsNullOrEmpty(text) ? SettingsManager.GetMasterSettings().DefaultProductImage : text;
			this.hdImgUrl.Value = Globals.FullPath(local);
			this.hdLink.Value = Globals.FullPath(HttpContext.Current.Request.Url.ToString());
			if (this.hidCartQuantity != null)
			{
				this.hidCartQuantity.Value = ShoppingCartProcessor.GetQuantity_Product(this.productId);
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
			if (user != null && user.IsReferral() && (!(this.sitesettings.SubMemberDeduct <= decimal.Zero) || wAPProductBrowseInfo.Product.SubMemberDeduct.HasValue))
			{
				if (!wAPProductBrowseInfo.Product.SubMemberDeduct.HasValue)
				{
					goto IL_0b8c;
				}
				decimal? subMemberDeduct = wAPProductBrowseInfo.Product.SubMemberDeduct;
				if (!(subMemberDeduct.GetValueOrDefault() <= default(decimal)) || !subMemberDeduct.HasValue)
				{
					goto IL_0b8c;
				}
			}
			goto IL_0bc6;
			IL_0bc6:
			int num2 = 1;
			goto IL_0bc7;
			IL_0bc7:
			if (num2 != 0)
			{
				this.divProductReferral.Visible = false;
			}
			bool flag = true;
			if (this.rptProductImages != null)
			{
				string locationUrl = "javascript:;";
				if (string.IsNullOrEmpty(wAPProductBrowseInfo.Product.ImageUrl1) && string.IsNullOrEmpty(wAPProductBrowseInfo.Product.ImageUrl2) && string.IsNullOrEmpty(wAPProductBrowseInfo.Product.ImageUrl3) && string.IsNullOrEmpty(wAPProductBrowseInfo.Product.ImageUrl4) && string.IsNullOrEmpty(wAPProductBrowseInfo.Product.ImageUrl5))
				{
					wAPProductBrowseInfo.Product.ImageUrl1 = masterSettings.DefaultProductImage;
				}
				DataTable skus = ProductBrowser.GetSkus(this.productId);
				List<SlideImage> list = new List<SlideImage>();
				int supplierId = wAPProductBrowseInfo.Product.SupplierId;
				if (supplierId > 0)
				{
					SupplierInfo supplierById = SupplierHelper.GetSupplierById(supplierId);
					if (supplierById != null)
					{
						this.hidSupplier.Value = "true";
						this.litSupplierName.Text = "<a href=\"SupplierAbout?SupplierId=" + supplierById.SupplierId + "\">" + supplierById.SupplierName + "</a>";
					}
				}
				else
				{
					this.hidSupplier.Value = "false";
					flag = false;
					this.ulsupplier.Style.Add(HtmlTextWriterStyle.Display, "none");
				}
				list.Add(new SlideImage(wAPProductBrowseInfo.Product.ImageUrl1, locationUrl));
				list.Add(new SlideImage(wAPProductBrowseInfo.Product.ImageUrl2, locationUrl));
				list.Add(new SlideImage(wAPProductBrowseInfo.Product.ImageUrl3, locationUrl));
				list.Add(new SlideImage(wAPProductBrowseInfo.Product.ImageUrl4, locationUrl));
				list.Add(new SlideImage(wAPProductBrowseInfo.Product.ImageUrl5, locationUrl));
				this.rptProductImages.DataSource = from item in list
				where !string.IsNullOrWhiteSpace(item.ImageUrl)
				select item;
				this.rptProductImages.DataBind();
			}
			this.litProdcutName.Text = wAPProductBrowseInfo.Product.ProductName;
			this.litSalePrice.Text = ((wAPProductBrowseInfo.Product.MinSalePrice == wAPProductBrowseInfo.Product.MaxSalePrice) ? wAPProductBrowseInfo.Product.MinSalePrice.F2ToString("f2") : (wAPProductBrowseInfo.Product.MinSalePrice.F2ToString("f2") + "~" + wAPProductBrowseInfo.Product.MaxSalePrice.F2ToString("f2")));
			if (wAPProductBrowseInfo.Product.MarketPrice.HasValue)
			{
				this.litMarketPrice.SetWhenIsNotNull(wAPProductBrowseInfo.Product.MarketPrice.GetValueOrDefault(decimal.Zero).F2ToString("f2"));
			}
			this.litShortDescription.Text = wAPProductBrowseInfo.Product.ShortDescription;
			this.divShortDescription.Visible = !string.IsNullOrEmpty(wAPProductBrowseInfo.Product.ShortDescription);
			if (this.litDescription != null)
			{
				string text2 = "";
				Regex regex = new Regex("<script[^>]*?>.*?</script>", RegexOptions.IgnoreCase);
				if (!string.IsNullOrWhiteSpace(wAPProductBrowseInfo.Product.MobbileDescription))
				{
					text2 = regex.Replace(wAPProductBrowseInfo.Product.MobbileDescription, "");
				}
				else if (!string.IsNullOrWhiteSpace(wAPProductBrowseInfo.Product.Description))
				{
					text2 = regex.Replace(wAPProductBrowseInfo.Product.Description, "");
				}
				text2 = text2.Replace("src", "data-url");
				text2 = text2.Replace("vurl", "src");
				this.litDescription.Text = text2;
			}
			Literal control = this.litSoldCount;
			num = wAPProductBrowseInfo.Product.ShowSaleCounts;
			control.SetWhenIsNotNull(num.ToString() + wAPProductBrowseInfo.Product.Unit);
			if (this.expandAttr != null)
			{
				this.expandAttr.ProductId = this.productId;
			}
			Literal control2 = this.litConsultationsCount;
			num = wAPProductBrowseInfo.ConsultationCount;
			control2.SetWhenIsNotNull(num.ToString());
			Literal control3 = this.litReviewsCount;
			num = wAPProductBrowseInfo.ReviewCount;
			control3.SetWhenIsNotNull(num.ToString());
			MemberInfo user2 = HiContext.Current.User;
			bool flag2 = false;
			if (user2 != null)
			{
				flag2 = ProductBrowser.CheckHasCollect(user2.UserId, this.productId);
			}
			this.litHasCollected.SetWhenIsNotNull(flag2 ? "1" : "0");
			this.BindCouponList();
			PageTitle.AddSiteNameTitle(wAPProductBrowseInfo.Product.ProductName);
			this.BindCombinaBuyInfo();
			this.BindPromotionInfo();
			this.BindGuestProducts();
			DataTable dBConsultations = wAPProductBrowseInfo.DBConsultations;
			for (int i = 0; i < dBConsultations.Rows.Count; i++)
			{
				dBConsultations.Rows[i]["UserName"] = DataHelper.GetHiddenUsername(dBConsultations.Rows[i]["UserName"].ToNullString());
			}
			this.rptProductConsultations.DataSource = dBConsultations;
			this.rptProductConsultations.DataBind();
			this.divConsultationEmpty.Visible = dBConsultations.IsNullOrEmpty();
			this.ulConsultations.Visible = !dBConsultations.IsNullOrEmpty();
			string phonePriceByProductId = PromoteHelper.GetPhonePriceByProductId(this.productId);
			if (!string.IsNullOrEmpty(phonePriceByProductId))
			{
				this.divPhonePrice.Visible = true;
				decimal num3 = phonePriceByProductId.Split(',')[0].ToDecimal(0);
				this.litPhonePrice.Text = num3.F2ToString("f2");
				decimal num4 = wAPProductBrowseInfo.Product.MinSalePrice - num3;
				this.litSalePrice.Text = ((num4 > decimal.Zero) ? num4 : decimal.Zero).F2ToString("f2");
				this.lbUserProductRefer.MobileExclusive = num3;
			}
			if (flag || this.liOrderPromotions.Visible || this.liOrderPromotions_free2.Visible || this.liProductSendGifts.Visible || this.rptCouponList.Visible)
			{
				this.divActivities.Visible = true;
			}
			else
			{
				this.divActivities.Visible = false;
			}
			return;
			IL_0b8c:
			if (HiContext.Current.SiteSettings.OpenReferral == 1 && HiContext.Current.SiteSettings.ShowDeductInProductPage && user.Referral != null)
			{
				num2 = (user.Referral.IsRepeled ? 1 : 0);
				goto IL_0bc7;
			}
			goto IL_0bc6;
		}

		private void BindGuestProducts()
		{
			int num = this.Page.Request.QueryString["productId"].ToInt(0);
			List<ProductYouLikeModel> productYouLike = BrowsedProductQueue.GetProductYouLike(num, 0, null, true);
			if (productYouLike.Count > 0)
			{
				this.divGuest.Visible = true;
				this.rp_guest.DataSource = productYouLike;
				this.rp_guest.DataBind();
			}
		}

		private void BindCombinaBuyInfo()
		{
			List<CombinationBuyandProductUnionInfo> combinationProductListByProductId = CombinationBuyHelper.GetCombinationProductListByProductId(this.productId);
			if (combinationProductListByProductId == null || combinationProductListByProductId.Count > 1)
			{
				CombinationBuyandProductUnionInfo combinationBuyandProductUnionInfo = combinationProductListByProductId.FirstOrDefault((CombinationBuyandProductUnionInfo c) => c.ProductId == this.productId);
				if (combinationBuyandProductUnionInfo != null)
				{
					string text = string.IsNullOrEmpty(combinationBuyandProductUnionInfo.ThumbnailUrl180) ? HiContext.Current.SiteSettings.DefaultProductImage : combinationBuyandProductUnionInfo.ThumbnailUrl180;
					this.ltlcombinamaininfo.Text = "<div class=\"pic\"><a href=\"CombinationBuyDetail.aspx?combinaId=" + combinationBuyandProductUnionInfo.CombinationId + "\"><img src=\"" + text + "\"></a></div><div class=\"price\"> <a href=\"CombinationBuyDetail.aspx?combinaId=" + combinationBuyandProductUnionInfo.CombinationId + "\"><label><b>￥" + combinationBuyandProductUnionInfo.MinCombinationPrice + "</b></label></a></div>";
					this.hidCombinaid.Value = combinationBuyandProductUnionInfo.CombinationId.ToString();
					this.divcombina.Visible = true;
					combinationProductListByProductId.Remove(combinationBuyandProductUnionInfo);
					this.rp_com.DataSource = combinationProductListByProductId;
					this.rp_com.DataBind();
					return;
				}
			}
			this.divcombina.Visible = false;
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
			int.TryParse(this.Page.Request.QueryString["productId"], out num);
			DataTable couponList = CouponHelper.GetCouponList(num, userId, false, false, false);
			this.hidCouponCount.Value = couponList.Rows.Count.ToString();
			this.rptCouponList.Visible = (couponList.Rows.Count > 0);
			this.rptCouponList.DataSource = couponList;
			this.rptCouponList.DataBind();
		}

		private string BindProductSendGifts()
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
				string str2 = string.IsNullOrEmpty(text) ? "" : ",";
				decimal num = productPromotionInfo.Condition;
				string arg = num.ToString("f0");
				num = productPromotionInfo.DiscountValue;
				text = str + str2 + string.Format("买{0}送{1} ", arg, num.ToString("f0"));
			}
			return text;
		}

		private void HasActivitiesToJumpUrl()
		{
			string text = string.Empty;
			CountDownInfo countDownInfo = PromoteHelper.ActiveCountDownByProductId(this.productId, 0);
			GroupBuyInfo groupBuyInfo = PromoteHelper.ActiveGroupBuyByProductId(this.productId);
			if (countDownInfo != null)
			{
				text = "/{0}/CountDownProductsDetails.aspx?countDownId=" + countDownInfo.CountDownId;
			}
			else if (groupBuyInfo != null)
			{
				text = "/{0}/GroupBuyProductDetails.aspx?groupBuyId=" + groupBuyInfo.GroupBuyId;
			}
			else
			{
				ProductPreSaleInfo productPreSaleInfoByProductId = ProductPreSaleHelper.GetProductPreSaleInfoByProductId(this.productId);
				if (productPreSaleInfoByProductId != null)
				{
					text = "/{0}/PreSaleproductdetails.aspx?PreSaleId=" + productPreSaleInfoByProductId.PreSaleId;
				}
				else
				{
					int activityStartsImmediatelyAboutCountDown = PromoteHelper.GetActivityStartsImmediatelyAboutCountDown(this.productId);
					if (activityStartsImmediatelyAboutCountDown > 0)
					{
						this.aCountDownUrl.Text = "该商品即将参与抢购活动，     去看看";
						this.aCountDownUrl.NavigateUrl = this.FillStringURL("/{0}/CountDownProductsDetails.aspx?countDownId=" + activityStartsImmediatelyAboutCountDown);
						this.aCountDownUrl.Style.Add("color", "red");
						this.aCountDownUrl.Visible = true;
						this.divCountDownUrl.Visible = true;
					}
					else
					{
						int activityStartsImmediatelyAboutGroupBuy = PromoteHelper.GetActivityStartsImmediatelyAboutGroupBuy(this.productId);
						if (activityStartsImmediatelyAboutGroupBuy > 0)
						{
							this.aCountDownUrl.Text = "该商品即将参与团购活动，     去看看";
							this.aCountDownUrl.NavigateUrl = this.FillStringURL("/{0}//GroupBuyProductDetails.aspx?groupBuyId=" + activityStartsImmediatelyAboutGroupBuy);
							this.aCountDownUrl.Style.Add("color", "red");
							this.aCountDownUrl.Visible = true;
							this.divCountDownUrl.Visible = true;
						}
					}
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.Page.Response.Redirect(this.FillStringURL(text));
			}
		}

		private void BindPromotionInfo()
		{
			StoreActivityEntityList storeActivityEntity = PromoteHelper.GetStoreActivityEntity(0, this.productId);
			if (storeActivityEntity.FullAmountReduceList.Count > 0)
			{
				this.liOrderPromotions.Visible = true;
				this.liOrderPromotions2.Visible = true;
				string empty = string.Empty;
				empty = (from t in storeActivityEntity.FullAmountReduceList
				select t.ActivityName).Aggregate((string t, string n) => t + "，" + n);
				Literal literal = this.ltlOrderPromotion2;
				Literal literal2 = this.ltlOrderPromotion;
				string text3 = literal.Text = (literal2.Text = empty);
			}
			if (storeActivityEntity.FullAmountSentFreightList.Count > 0)
			{
				this.liOrderPromotions_free.Visible = true;
				this.liOrderPromotions_free2.Visible = true;
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
					this.liProductSendGifts2.Visible = true;
					this.liProductSendGifts.Visible = true;
					Literal literal5 = this.ltlProductSendGifts;
					Literal literal6 = this.ltlProductSendGifts2;
					string text3 = literal5.Text = (literal6.Text = text7);
				}
				if (!string.IsNullOrEmpty(text6))
				{
					this.liProductSendGifts2.Visible = true;
					this.liProductSendGifts.Visible = true;
					Literal literal7 = this.ltlProductSendGifts;
					literal7.Text = literal7.Text + (string.IsNullOrEmpty(this.ltlProductSendGifts.Text) ? "" : "，") + text6;
					Literal literal8 = this.ltlProductSendGifts2;
					literal8.Text = literal8.Text + (string.IsNullOrEmpty(this.ltlProductSendGifts2.Text) ? "" : "，") + text6;
				}
			}
		}

		private string FillStringURL(string url)
		{
			string text = url;
			string[] segments = this.Page.Request.Url.Segments;
			foreach (string input in segments)
			{
				string text2 = Globals.GetStringByRegularExpression(input, "[a-zA-Z]").ToLower();
				switch (text2)
				{
				case "vshop":
					text = string.Format(text, "Vshop");
					break;
				case "wapshop":
					text = string.Format(text, "wapshop");
					break;
				case "alioh":
					text = string.Format(text, "alioh");
					break;
				}
			}
			return text;
		}
	}
}
