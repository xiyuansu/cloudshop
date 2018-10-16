using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Shopping;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class AppSubmmitOrder : AppshopMemberTemplatedWebControl
	{
		private Literal litShipTo;

		private Literal litCellPhone;

		private Literal litAddress;

		private HtmlInputControl groupbuyHiddenBox;

		private HtmlInputControl countdownHiddenBox;

		private HtmlInputControl fightGroupHiddenBox;

		private HtmlInputControl fightGroupActivityHiddenBox;

		private AppshopTemplatedRepeater rptPromotions;

		private AppshopTemplatedRepeater rptCartGifts;

		private AppshopTemplatedRepeater rptShippingType;

		private AppshopTemplatedRepeater rptCartPointGifts;

		private AppshopTemplatedRepeater rptAddress;

		private APPShop_SubmmitCartProducts rptCartProducts;

		private Common_CouponSelect dropCoupon;

		private Label lblOrderTotal;

		private HtmlInputHidden selectShipTo;

		private HtmlInputHidden hidRegionId;

		private HtmlGenericControl lblTotalPrice;

		private HtmlGenericControl lblTotalPrice1;

		private HtmlGenericControl divGifts;

		private HtmlGenericControl lblTax;

		private Common_AppPaymentTypeSelect paymenttypeselect;

		private HtmlInputControl hdCurrentckIds;

		private HtmlGenericControl divTax;

		private HtmlGenericControl spanTaxRate;

		private HtmlInputHidden hidShoppingDeduction;

		private HtmlInputHidden hidCanPointUseWithCoupon;

		private HtmlInputHidden hidTotalPrice;

		private Label lblMaxPoints;

		private Label lblMaxPointsToPrice;

		private HtmlInputText txtUsePoints;

		private HtmlInputHidden hidShoppingDeductionRatio;

		private HtmlInputHidden hidMyPoints;

		private HtmlInputHidden hidOrderRate;

		private HtmlInputHidden hidGetgoodsOnStores;

		private HtmlInputHidden hidPaymentId_Podrequest;

		private HtmlInputHidden inputShippingModeId;

		private HtmlInputHidden inputPaymentModeId;

		private HtmlInputHidden hidDeliveryTime;

		private HtmlInputHidden hidStoreId;

		private HtmlInputHidden hidStoreName;

		private HtmlInputHidden htmlCouponCode;

		private HtmlInputHidden hidCanUsePoint;

		private HtmlGenericControl lblDeductibleMoney;

		private HtmlGenericControl lblShippModePrice;

		private HtmlGenericControl litCouponAmout;

		private HtmlGenericControl couponName;

		private HtmlInputHidden hidPointRate;

		private Label litTaxRate;

		private HyperLink hlkFeeFreight;

		private HyperLink hlkReducedPromotion;

		private AppshopTemplatedRepeater rptOrderCoupon;

		private HtmlInputHidden hidOnlinePayCount;

		private HtmlInputHidden hidIsPreSale;

		private HtmlGenericControl lblAmount;

		private HtmlGenericControl lblDepositMoney;

		private HtmlGenericControl lblRetainage;

		private Label lblGiftFeright;

		private HtmlInputHidden hidHasSupplierProduct;

		private HtmlGenericControl spandemo;

		private HtmlGenericControl divlinegifts;

		private HtmlInputHidden hidHasStoresInCity;

		private HtmlInputHidden hidIsStoreDelive;

		private HtmlInputHidden hidIsSupportExpress;

		private HtmlInputHidden hidIsGetMinOrderPrice;

		private HtmlInputHidden hidPickeupInStoreRemark;

		private HtmlInputHidden hidPaymentId_Offline;

		private HtmlInputHidden hidChooseStoreId;

		private HtmlInputHidden hidStoreFreight;

		private HtmlInputHidden hidOrderFreight;

		private HtmlInputHidden hidHasTradePassword;

		private HtmlInputHidden hidIsSubmitInTime;

		private HtmlInputHidden hidIsOpenCertification;

		private HtmlGenericControl storeName;

		private HtmlGenericControl storeTel;

		private HtmlGenericControl storeAddress;

		private HtmlGenericControl storeTime;

		private HtmlGenericControl storeDistance;

		private HtmlInputHidden hidIsOfflinePay;

		private HtmlInputHidden hidIsOnlinePay;

		private HtmlInputHidden hidIsCashOnDelivery;

		private HtmlInputHidden hidInvoiceType;

		private HtmlInputHidden hidInvoiceTitle;

		private HtmlInputHidden hidInvoiceTaxpayerNumber;

		private HtmlInputHidden hidOpenBalancePay;

		private HtmlInputHidden hidBalanceAmount;

		private Label lblBalance;

		private HtmlInputHidden hidEnableTax;

		private HtmlInputHidden hidEnableETax;

		private HtmlInputHidden hidEnableVATTax;

		private HtmlInputHidden hidTaxRate;

		private HtmlInputHidden hidVATTaxRate;

		private HtmlInputHidden hidVATInvoiceDays;

		private Literal litAfterSaleDays;

		private Literal litInvoiceSendDays;

		private HtmlInputHidden hidInvoiceId;

		private HtmlInputHidden hidIsPersonal;

		private SiteSettings siteSettings = SettingsManager.GetMasterSettings();

		private HtmlInputHidden hidInvoiceJson;

		private HtmlInputHidden hidFightGroupPickeupInStore;

		private HtmlInputHidden hidIsSupportStoreDelive;

		private int buyAmount;

		private string productSku;

		private int groupBuyId;

		private int countDownId;

		private int fightGroupActivityId;

		private int fightGroupId;

		private int shippingModeId = 0;

		private int paymentModeId = 0;

		private int storeId = 0;

		private int chooseStoreId = 0;

		private string deliveryTime = "任意时间";

		private ShoppingCartInfo cart;

		private GroupBuyInfo groupbuyInfo = null;

		private CountDownInfo countdownInfo = null;

		private FightGroupActivityInfo fightGroupActivitiyInfo;

		private FightGroupInfo fightGroupInfo;

		private string from = "";

		private bool isFightGroup = false;

		private bool isGroupBuy = false;

		private bool isCountDown = false;

		private bool isSignBuy = false;

		private string buytype = "";

		private HtmlInputHidden hidIsMultiStore;

		private bool isPreSale = false;

		private int PresaleId;

		private int RecordId;

		private bool hasError = false;

		private int userRegionId = 0;

		private string addressFullRegionPath = "";

		private string addressLatLng = "";

		private ShippingAddressInfo shipperAddress = null;

		private MemberInfo user = HiContext.Current.User;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VSubmmitOrder.html";
			}
			base.OnInit(e);
		}

		private void CheckParamter()
		{
			string text = "";
			this.from = this.Page.Request.QueryString["from"].ToNullString().ToLower();
			this.productSku = Globals.UrlDecode(this.Page.Request.QueryString["productSku"].ToNullString());
			this.buyAmount = this.Page.Request.QueryString["buyAmount"].ToInt(0);
			this.storeId = this.Page.Request.QueryString["StoreId"].ToInt(0);
			this.chooseStoreId = this.Page.Request.QueryString["ChooseStoreId"].ToInt(0);
			string text2 = this.Page.Request.QueryString["SessionId"];
			if (this.buyAmount <= 0)
			{
				this.buyAmount = 1;
			}
			if (!string.IsNullOrWhiteSpace(text2))
			{
				MemberInfo memberInfo = MemberProcessor.FindMemberBySessionId(text2);
				if (memberInfo == null)
				{
					string msg = "sessionid过期或不存在，请重新登录！";
					this.hasError = true;
					this.ShowWapMessage(msg, "goHomeUrl");
					return;
				}
				HiContext.Current.User = memberInfo;
				Users.SetCurrentUser(memberInfo.UserId, 30, true, false);
				this.user = memberInfo;
			}
			else
			{
				this.user = HiContext.Current.User;
				if (this.user == null || this.user.UserId == 0)
				{
					string msg2 = "用户不存在，请重新登录！";
					this.hasError = true;
					this.ShowWapMessage(msg2, "goHomeUrl");
					return;
				}
			}
			if (this.from == "groupbuy")
			{
				this.isGroupBuy = true;
				this.buytype = "GroupBuy";
			}
			else if (this.from == "countdown")
			{
				this.isCountDown = true;
				this.buytype = "CountDown";
			}
			else if (this.from == "fightgroup")
			{
				this.isFightGroup = true;
			}
			else if (this.from == "signbuy")
			{
				this.isSignBuy = true;
			}
			else
			{
				if (this.from == "presale")
				{
					this.PresaleId = this.Page.Request.QueryString["PresaleId"].ToInt(0);
					this.productSku = DataHelper.CleanSearchString(this.productSku);
					if (this.CheckPresaleInfo())
					{
						this.isPreSale = true;
						goto IL_0407;
					}
					return;
				}
				if (this.from == "prize")
				{
					this.RecordId = this.Page.Request.QueryString["RecordId"].ToInt(0);
				}
				else
				{
					this.productSku = this.Page.Request.QueryString["productSku"].ToNullString();
					if (string.IsNullOrEmpty(this.productSku))
					{
						this.productSku = this.Page.Request.QueryString["ckids"].ToNullString();
					}
					if (this.CheckProductSkuHasProductPreInfo() && this.storeId <= 0)
					{
						this.hasError = true;
						this.ShowWapMessage("您所选商品中有预售商品，请重新选择！", "goShoppingCart");
						return;
					}
					if (!this.isPreSale && !string.IsNullOrEmpty(this.productSku) && !ProductHelper.ProductsIsAllOnSales(this.productSku, this.storeId))
					{
						this.ShowMessage("有商品不存在或者已经下架", false, "/ShoppingCart", 2);
						return;
					}
				}
			}
			goto IL_0407;
			IL_0407:
			if (this.RecordId == 0)
			{
				if (this.storeId == 0 && !HiContext.Current.SiteSettings.OpenMultStore)
				{
					this.cart = ShoppingCartProcessor.GetShoppingCart(this.from, this.productSku, this.buyAmount, 0, true, -1, this.Page.Request["fightGroupActivityId"].ToInt(0));
				}
				else
				{
					this.cart = ShoppingCartProcessor.GetShoppingCart(this.from, this.productSku, this.buyAmount, 0, true, this.storeId, this.Page.Request["fightGroupActivityId"].ToInt(0));
				}
			}
			else
			{
				UserAwardRecordsInfo userAwardRecordsInfo = ActivityHelper.GetUserAwardRecordsInfo(this.RecordId);
				if (userAwardRecordsInfo == null || userAwardRecordsInfo.UserId != HiContext.Current.UserId || userAwardRecordsInfo.PrizeType != 3 || userAwardRecordsInfo.Status == 2)
				{
					this.ShowWapMessage("奖项不存在不存在", "goHomeUrl");
					return;
				}
				string orderIdByUserAwardRecordsId = OrderHelper.GetOrderIdByUserAwardRecordsId(this.RecordId);
				if (!string.IsNullOrWhiteSpace(orderIdByUserAwardRecordsId))
				{
					this.Page.Response.Redirect("MemberOrderDetails.aspx?OrderId=" + orderIdByUserAwardRecordsId, true);
				}
				else
				{
					int prizeValue = userAwardRecordsInfo.PrizeValue;
					this.cart = ShoppingCartProcessor.GetPrizeShoppingCart(prizeValue);
				}
			}
			if (this.cart != null && this.cart.GetQuantity(false) == 0)
			{
				this.buytype = "0";
			}
			if (this.cart == null)
			{
				string msg3 = "购物车无任何商品！";
				this.hasError = true;
				this.ShowWapMessage(msg3, "goHomeUrl");
			}
			else
			{
				if (this.isGroupBuy)
				{
					this.groupbuyInfo = TradeHelper.GetProductGroupBuyInfo(this.cart.LineItems[0].ProductId, this.buyAmount, out text);
					if (this.groupbuyInfo == null)
					{
						this.hasError = true;
						this.ShowWapMessage(Globals.UrlEncode(text), "goHomeUrl");
						return;
					}
				}
				if (this.isCountDown)
				{
					this.countdownInfo = TradeHelper.ProductExistsCountDown(this.cart.LineItems[0].ProductId, "", this.storeId);
					if (this.countdownInfo == null)
					{
						this.hasError = true;
						this.ShowWapMessage(Globals.UrlEncode("该商品未进行抢购活动,或者活动已结束"), "goHomeUrl");
						return;
					}
					if (!StoreActivityHelper.JoinActivity(this.countdownInfo.CountDownId, 2, this.storeId, this.countdownInfo.StoreType))
					{
						this.hasError = true;
						this.ShowWapMessage(Globals.UrlEncode("该门店未参与此抢购活动"), "goHomeUrl");
						return;
					}
					this.countdownInfo = TradeHelper.CheckUserCountDown(this.cart.LineItems[0].ProductId, this.countdownInfo.CountDownId, this.cart.LineItems[0].SkuId, HiContext.Current.UserId, this.buyAmount, "", out text, this.storeId);
					if (this.countdownInfo == null)
					{
						this.hasError = true;
						this.ShowWapMessage(text, "goHomeUrl");
						return;
					}
				}
				if (this.isFightGroup)
				{
					this.fightGroupActivitiyInfo = TradeHelper.GetFightGroupActivitieInfo(this.Page.Request["fightGroupActivityId"].ToInt(0));
					if (this.fightGroupActivitiyInfo == null)
					{
						this.hasError = true;
						this.ShowWapMessage("拼团活动不存在", "goHomeUrl");
						return;
					}
					int num = this.Page.Request["fightGroupId"].ToInt(0);
					this.fightGroupInfo = VShopHelper.GetFightGroup(num);
					DateTime endDate = this.fightGroupActivitiyInfo.EndDate;
					int limitedHour = this.fightGroupActivitiyInfo.LimitedHour;
					if (this.fightGroupInfo == null && num != 0)
					{
						endDate = this.fightGroupInfo.EndTime;
						this.hasError = true;
						this.ShowWapMessage("组团信息不存在", "goHomeUrl");
						return;
					}
					if (this.fightGroupActivitiyInfo.StartDate > DateTime.Now)
					{
						this.hasError = true;
						this.ShowWapMessage("拼团活动还未开始", "goHomeUrl");
						return;
					}
					if (endDate < DateTime.Now)
					{
						this.hasError = true;
						this.ShowWapMessage("活动已结束", "goHomeUrl");
						return;
					}
				}
				this.shippingModeId = this.Page.Request.QueryString["ShippingModeId"].ToInt(0);
				this.paymentModeId = this.Page.Request.QueryString["paymentModeId"].ToInt(0);
				this.deliveryTime = this.Page.Request.QueryString["deliveryTime"].ToNullString();
				if (this.deliveryTime != "任意时间" && this.deliveryTime != "工作日" && this.deliveryTime != "节假日")
				{
					this.deliveryTime = "任意时间";
				}
				if (this.shippingModeId != 0 && this.shippingModeId != -1 && this.shippingModeId != -2)
				{
					this.shippingModeId = 0;
				}
			}
		}

		protected override void AttachChildControls()
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			this.spandemo = (HtmlGenericControl)this.FindControl("spandemo");
			this.spandemo.Visible = masterSettings.IsDemoSite;
			string empty = string.Empty;
			this.litShipTo = (Literal)this.FindControl("litShipTo");
			this.litCellPhone = (Literal)this.FindControl("litCellPhone");
			this.litAddress = (Literal)this.FindControl("litAddress");
			this.rptCartProducts = (APPShop_SubmmitCartProducts)this.FindControl("APPShop_SubmmitCartProducts");
			this.rptCartProducts.ItemDataBound += this.rptCartProducts_ItemDataBound;
			this.rptCartGifts = (AppshopTemplatedRepeater)this.FindControl("rptCartGifts");
			this.rptCartPointGifts = (AppshopTemplatedRepeater)this.FindControl("rptCartPointGifts");
			this.divGifts = (HtmlGenericControl)this.FindControl("divGifts");
			this.dropCoupon = (Common_CouponSelect)this.FindControl("dropCoupon");
			this.lblOrderTotal = (Label)this.FindControl("lblOrderTotal");
			this.groupbuyHiddenBox = (HtmlInputControl)this.FindControl("groupbuyHiddenBox");
			this.countdownHiddenBox = (HtmlInputControl)this.FindControl("countdownHiddenBox");
			this.fightGroupHiddenBox = (HtmlInputControl)this.FindControl("fightGroupHiddenBox");
			this.fightGroupActivityHiddenBox = (HtmlInputControl)this.FindControl("fightGroupActivityHiddenBox");
			this.rptAddress = (AppshopTemplatedRepeater)this.FindControl("rptAddress");
			this.selectShipTo = (HtmlInputHidden)this.FindControl("selectShipTo");
			this.hidRegionId = (HtmlInputHidden)this.FindControl("regionId");
			this.lblTotalPrice = (HtmlGenericControl)this.FindControl("lblTotalPrice");
			this.lblTotalPrice1 = (HtmlGenericControl)this.FindControl("lblTotalPrice1");
			this.lblTax = (HtmlGenericControl)this.FindControl("lblTax");
			this.rptPromotions = (AppshopTemplatedRepeater)this.FindControl("rptPromotions");
			this.rptShippingType = (AppshopTemplatedRepeater)this.FindControl("rptShippingType");
			this.paymenttypeselect = (Common_AppPaymentTypeSelect)this.FindControl("paymenttypeselect");
			this.hidIsMultiStore = (HtmlInputHidden)this.FindControl("hidIsMultiStore");
			this.hdCurrentckIds = (HtmlInputControl)this.FindControl("hdCurrentckIds");
			this.hidShoppingDeduction = (HtmlInputHidden)this.FindControl("hidShoppingDeduction");
			this.hidCanPointUseWithCoupon = (HtmlInputHidden)this.FindControl("hidCanPointUseWithCoupon");
			this.lblMaxPoints = (Label)this.FindControl("lblMaxPoints");
			this.hidTotalPrice = (HtmlInputHidden)this.FindControl("hidTotalPrice");
			this.txtUsePoints = (HtmlInputText)this.FindControl("txtUsePoint");
			this.hidShoppingDeductionRatio = (HtmlInputHidden)this.FindControl("hidShoppingDeductionRatio");
			this.hidMyPoints = (HtmlInputHidden)this.FindControl("hidMyPoints");
			this.hidOrderRate = (HtmlInputHidden)this.FindControl("hidOrderRate");
			this.divTax = (HtmlGenericControl)this.FindControl("divTax");
			this.spanTaxRate = (HtmlGenericControl)this.FindControl("spanTaxRate");
			this.inputPaymentModeId = (HtmlInputHidden)this.FindControl("inputPaymentModeId");
			this.inputShippingModeId = (HtmlInputHidden)this.FindControl("inputShippingModeId");
			this.hidPaymentId_Podrequest = (HtmlInputHidden)this.FindControl("hidPaymentId_Podrequest");
			this.hidGetgoodsOnStores = (HtmlInputHidden)this.FindControl("hidGetgoodsOnStores");
			this.hidDeliveryTime = (HtmlInputHidden)this.FindControl("hidDeliveryTime");
			this.hidStoreId = (HtmlInputHidden)this.FindControl("hidStoreId");
			this.hidChooseStoreId = (HtmlInputHidden)this.FindControl("hidChooseStoreId");
			this.hidStoreFreight = (HtmlInputHidden)this.FindControl("hidStoreFreight");
			this.hidOrderFreight = (HtmlInputHidden)this.FindControl("hidOrderFreight");
			this.hidStoreName = (HtmlInputHidden)this.FindControl("hidStoreName");
			this.lblMaxPointsToPrice = (Label)this.FindControl("lblMaxPointsToPrice");
			this.lblDeductibleMoney = (HtmlGenericControl)this.FindControl("lblDeductibleMoney");
			this.litTaxRate = (Label)this.FindControl("litTaxRate");
			this.hlkFeeFreight = (HyperLink)this.FindControl("hlkFeeFreight");
			this.hlkReducedPromotion = (HyperLink)this.FindControl("hlkReducedPromotion");
			this.lblShippModePrice = (HtmlGenericControl)this.FindControl("lblShippModePrice");
			this.rptOrderCoupon = (AppshopTemplatedRepeater)this.FindControl("rptOrderCoupon");
			this.htmlCouponCode = (HtmlInputHidden)this.FindControl("htmlCouponCode");
			this.couponName = (HtmlGenericControl)this.FindControl("couponName");
			this.litCouponAmout = (HtmlGenericControl)this.FindControl("litCouponAmout");
			this.hidCanUsePoint = (HtmlInputHidden)this.FindControl("hidCanUsePoint");
			this.lblGiftFeright = (Label)this.FindControl("lblGiftFeright");
			this.hidHasSupplierProduct = (HtmlInputHidden)this.FindControl("hidHasSupplierProduct");
			this.hidPointRate = (HtmlInputHidden)this.FindControl("hidPointRate");
			this.divlinegifts = (HtmlGenericControl)this.FindControl("divlinegifts");
			this.hidOnlinePayCount = (HtmlInputHidden)this.FindControl("hidOnlinePayCount");
			this.hidHasStoresInCity = (HtmlInputHidden)this.FindControl("hidHasStoresInCity");
			this.hidIsStoreDelive = (HtmlInputHidden)this.FindControl("hidIsStoreDelive");
			this.hidIsSupportExpress = (HtmlInputHidden)this.FindControl("hidIsSupportExpress");
			this.hidIsGetMinOrderPrice = (HtmlInputHidden)this.FindControl("hidIsGetMinOrderPrice");
			this.hidPickeupInStoreRemark = (HtmlInputHidden)this.FindControl("hidPickeupInStoreRemark");
			this.hidPaymentId_Offline = (HtmlInputHidden)this.FindControl("hidPaymentId_Offline");
			this.storeName = (HtmlGenericControl)this.FindControl("storeName");
			this.storeTel = (HtmlGenericControl)this.FindControl("storeTel");
			this.storeAddress = (HtmlGenericControl)this.FindControl("storeAddress");
			this.storeTime = (HtmlGenericControl)this.FindControl("storeTime");
			this.storeDistance = (HtmlGenericControl)this.FindControl("storeDistance");
			this.hidHasTradePassword = (HtmlInputHidden)this.FindControl("hidHasTradePassword");
			this.hidIsSubmitInTime = (HtmlInputHidden)this.FindControl("hidIsSubmitInTime");
			this.hidIsOpenCertification = (HtmlInputHidden)this.FindControl("hidIsOpenCertification");
			this.hidIsSupportStoreDelive = (HtmlInputHidden)this.FindControl("hidIsSupportStoreDelive");
			if (this.hidIsOpenCertification != null)
			{
				this.hidIsOpenCertification.Value = "0";
			}
			this.hidIsOfflinePay = (HtmlInputHidden)this.FindControl("hidIsOfflinePay");
			this.hidIsOnlinePay = (HtmlInputHidden)this.FindControl("hidIsOnlinePay");
			this.hidIsCashOnDelivery = (HtmlInputHidden)this.FindControl("hidIsCashOnDelivery");
			this.hidInvoiceTitle = (HtmlInputHidden)this.FindControl("hidInvoiceTitle");
			this.hidInvoiceTaxpayerNumber = (HtmlInputHidden)this.FindControl("hidInvoiceTaxpayerNumber");
			this.hidOpenBalancePay = (HtmlInputHidden)this.FindControl("hidOpenBalancePay");
			this.hidBalanceAmount = (HtmlInputHidden)this.FindControl("hidBalanceAmount");
			this.lblBalance = (Label)this.FindControl("lblBalance");
			this.hidEnableTax = (HtmlInputHidden)this.FindControl("hidEnableTax");
			this.hidEnableETax = (HtmlInputHidden)this.FindControl("hidEnableETax");
			this.hidEnableVATTax = (HtmlInputHidden)this.FindControl("hidEnableVATTax");
			this.hidVATTaxRate = (HtmlInputHidden)this.FindControl("hidVATTaxRate");
			this.hidTaxRate = (HtmlInputHidden)this.FindControl("hidTaxRate");
			this.hidVATInvoiceDays = (HtmlInputHidden)this.FindControl("hidVATInvoiceDays");
			this.litAfterSaleDays = (Literal)this.FindControl("litAfterSaleDays");
			this.litInvoiceSendDays = (Literal)this.FindControl("litInvoiceSendDays");
			this.hidInvoiceId = (HtmlInputHidden)this.FindControl("hidInvoiceId");
			this.hidIsPersonal = (HtmlInputHidden)this.FindControl("hidIsPersonal");
			this.hidInvoiceType = (HtmlInputHidden)this.FindControl("hidInvoiceType");
			this.hidInvoiceJson = (HtmlInputHidden)this.FindControl("hidInvoiceJson");
			this.hidFightGroupPickeupInStore = (HtmlInputHidden)this.FindControl("hidFightGroupPickeupInStore");
			this.hidFightGroupPickeupInStore.Value = masterSettings.FitGroupIsOpenPickeupInStore.ToNullString().ToLower();
			this.CheckParamter();
			this.hidOpenBalancePay.Value = masterSettings.OpenBalancePay.ToNullString().ToLower();
			HtmlInputHidden htmlInputHidden = this.hidEnableETax;
			bool flag = masterSettings.EnableE_Invoice;
			htmlInputHidden.Value = flag.ToString().ToLower();
			HtmlInputHidden htmlInputHidden2 = this.hidEnableTax;
			flag = masterSettings.EnableTax;
			htmlInputHidden2.Value = flag.ToString().ToLower();
			HtmlInputHidden htmlInputHidden3 = this.hidEnableVATTax;
			flag = masterSettings.EnableVATInvoice;
			htmlInputHidden3.Value = flag.ToString().ToLower();
			this.hidVATTaxRate.Value = masterSettings.VATTaxRate.ToNullString();
			this.hidTaxRate.Value = masterSettings.TaxRate.ToNullString();
			this.hidVATInvoiceDays.Value = masterSettings.VATInvoiceDays.ToNullString();
			this.litAfterSaleDays.SetWhenIsNotNull(masterSettings.EndOrderDays.ToNullString());
			this.litInvoiceSendDays.SetWhenIsNotNull((masterSettings.EndOrderDays + masterSettings.VATInvoiceDays).ToNullString());
			this.hidIsPreSale = (HtmlInputHidden)this.FindControl("hidIsPreSale");
			this.lblAmount = (HtmlGenericControl)this.FindControl("lblAmount");
			this.lblDepositMoney = (HtmlGenericControl)this.FindControl("lblDepositMoney");
			this.lblRetainage = (HtmlGenericControl)this.FindControl("lblRetainage");
			if (this.hidPointRate != null)
			{
				this.hidPointRate.Value = HiContext.Current.SiteSettings.PointsRate.F2ToString("f2");
			}
			this.buyAmount = this.Page.Request.QueryString["buyAmount"].ToInt(0);
			this.productSku = this.Page.Request.QueryString["productSku"].ToNullString();
			this.groupBuyId = this.Page.Request.QueryString["groupBuyId"].ToInt(0);
			this.countDownId = this.Page.Request.QueryString["countdownId"].ToInt(0);
			this.fightGroupActivityId = this.Page.Request.QueryString["fightGroupActivityId"].ToInt(0);
			this.fightGroupId = this.Page.Request.QueryString["fightGroupId"].ToInt(0);
			this.hidHasStoresInCity.Value = "false";
			if (this.buyAmount > 0 && !string.IsNullOrEmpty(this.productSku) && !string.IsNullOrEmpty(this.from) && (this.from == "signbuy" || this.from == "groupbuy" || this.from == "countdown" || this.from == "fightgroup" || this.from == "presale"))
			{
				if (this.isGroupBuy)
				{
					this.groupbuyHiddenBox.SetWhenIsNotNull(this.groupBuyId.ToString());
					this.cart = ShoppingCartProcessor.GetGroupBuyShoppingCart(this.productSku, this.buyAmount);
				}
				else if (this.isCountDown)
				{
					this.countdownHiddenBox.SetWhenIsNotNull(this.countDownId.ToString());
				}
				else if (this.isFightGroup)
				{
					this.fightGroupHiddenBox.SetWhenIsNotNull(this.fightGroupId.ToString());
					this.fightGroupActivityHiddenBox.SetWhenIsNotNull(this.fightGroupActivityId.ToString());
				}
				else if (this.isPreSale)
				{
					this.hidIsPreSale.Value = "1";
					this.FindControl("ulpresale").Visible = true;
				}
				this.hdCurrentckIds.Value = this.productSku;
			}
			else
			{
				this.productSku = this.Page.Request.QueryString["productSku"].ToNullString();
				if (string.IsNullOrEmpty(this.productSku))
				{
					this.productSku = this.Page.Request.QueryString["ckids"].ToNullString();
				}
				this.hdCurrentckIds.Value = this.productSku;
			}
			if (this.cart != null && ((this.cart.LineItems != null && this.cart.LineItems.Count > 0) || (this.cart.LineGifts != null && this.cart.LineGifts.Count > 0)))
			{
				if (!TradeHelper.CheckShoppingStock(this.cart, out empty, this.storeId))
				{
					this.ShowWapMessage("订单中有商品(" + empty + ")库存不足", "goShoppingCart");
				}
				int num = TradeHelper.WapPaymentTypeCount(ClientType.App, this.isFightGroup).ToInt(0);
				this.hidOnlinePayCount.Value = num.ToString();
				if (num > 0)
				{
					this.hidIsOnlinePay.Value = "true";
				}
				else
				{
					this.hidIsOnlinePay.Value = "false";
				}
				this.bindShippingAddress();
				this.bindShippingPaymentInfo();
				if ((!masterSettings.EnableTax && !masterSettings.EnableE_Invoice && !masterSettings.EnableVATInvoice) || (this.cart != null && this.cart.LineItems.Count() == 0))
				{
					this.divTax.Visible = false;
				}
				else
				{
					this.divTax.Visible = true;
				}
				Label label = this.litTaxRate;
				decimal num2 = masterSettings.TaxRate;
				label.Text = num2.ToString(CultureInfo.InvariantCulture);
				this.spanTaxRate.Visible = (masterSettings.TaxRate > decimal.Zero || masterSettings.VATTaxRate > decimal.Zero);
				if (masterSettings.EnableTax || masterSettings.EnableE_Invoice)
				{
					this.hidOrderRate.Value = ((masterSettings.TaxRate > decimal.Zero) ? (this.cart.GetTotal(this.storeId > 0) * masterSettings.TaxRate / 100m).F2ToString("f2") : "0.00");
				}
				else if (masterSettings.EnableVATInvoice)
				{
					this.hidOrderRate.Value = ((masterSettings.VATTaxRate > decimal.Zero) ? (this.cart.GetTotal(this.storeId > 0) * masterSettings.VATTaxRate / 100m).F2ToString("f2") : "0.00");
				}
				else
				{
					this.hidOrderRate.Value = "0.00";
				}
				this.hidHasTradePassword.Value = (string.IsNullOrWhiteSpace(this.user.TradePassword) ? "0" : "1");
				this.hidBalanceAmount.Value = (this.user.Balance - this.user.RequestBalance).F2ToString("f2");
				this.lblBalance.Text = (this.user.Balance - this.user.RequestBalance).F2ToString("f2");
				this.bindCouponInfo();
				this.rptCartProducts.RegionId = this.userRegionId;
				var dataSource = (from i in (from i in this.cart.LineItems
				select new
				{
					i.SupplierId,
					i.SupplierName
				}).Distinct()
				orderby i.SupplierId
				select i).ToList();
				this.rptCartProducts.DataSource = dataSource;
				this.rptCartProducts.ShoppingCart = this.cart;
				this.rptCartProducts.StoreId = this.storeId;
				this.rptCartProducts.DataBind();
				if ((from i in this.cart.LineItems
				where i.SupplierId == 0
				select i).Count() <= 0)
				{
					this.divTax.Visible = false;
				}
				if ((from i in this.cart.LineItems
				where i.SupplierId > 0
				select i).Count() > 0)
				{
					this.hidHasSupplierProduct.Value = "1";
				}
				if (this.isPreSale)
				{
					decimal num3 = default(decimal);
					ProductPreSaleInfo productPreSaleInfo = ProductPreSaleHelper.GetProductPreSaleInfo(this.PresaleId);
					if (this.cart != null)
					{
						num3 = ((productPreSaleInfo.Deposit == decimal.Zero) ? (this.cart.LineItems[0].MemberPrice * (decimal)productPreSaleInfo.DepositPercent / 100m) : productPreSaleInfo.Deposit) * (decimal)this.cart.LineItems[0].Quantity;
					}
					this.lblTotalPrice.InnerHtml = this.cart.GetAmount(false).F2ToString("f2");
					HtmlGenericControl htmlGenericControl = this.lblTotalPrice1;
					Label label2 = this.lblOrderTotal;
					string text3 = htmlGenericControl.InnerText = (label2.Text = num3.F2ToString("f2"));
					this.lblDepositMoney.InnerText = num3.F2ToString("f2");
					decimal num4 = this.cart.GetTotal(false) - num3;
					if (num4 < decimal.Zero)
					{
						num4 = default(decimal);
					}
					this.lblRetainage.InnerText = num4.F2ToString("f2");
					this.lblAmount.InnerText = (num3 + num4).F2ToString("f2");
					HtmlInputHidden htmlInputHidden4 = this.hidTotalPrice;
					num2 = this.cart.GetTotal(false);
					htmlInputHidden4.Value = num2.ToString();
				}
				else
				{
					HtmlInputHidden htmlInputHidden5 = this.hidTotalPrice;
					Label label3 = this.lblOrderTotal;
					string text3 = htmlInputHidden5.Value = (label3.Text = this.cart.GetTotal(this.storeId > 0).F2ToString("f2"));
					HtmlGenericControl htmlGenericControl2 = this.lblTotalPrice1;
					HtmlGenericControl htmlGenericControl3 = this.lblTotalPrice;
					text3 = (htmlGenericControl2.InnerText = (htmlGenericControl3.InnerHtml = this.cart.GetAmount(this.storeId > 0).F2ToString("f2")));
				}
				if (this.cart != null)
				{
					this.BindSendGifts();
					if (this.storeId <= 0)
					{
						this.BindPointGifts();
					}
				}
				this.hidCanPointUseWithCoupon.Value = "false";
				this.hidCanUsePoint.Value = "false";
				bool flag2 = true;
				if (this.isGroupBuy || this.isCountDown || this.cart.LineItems.Count == 0 || this.isFightGroup)
				{
					flag2 = false;
				}
				if (flag2 && this.user != null && masterSettings.ShoppingDeduction > 0 && this.user.Points > 0)
				{
					int shoppingDeductionRatio = masterSettings.ShoppingDeductionRatio;
					decimal num5 = (decimal)shoppingDeductionRatio * this.cart.GetTotal(this.storeId > 0) * (decimal)masterSettings.ShoppingDeduction / 100m;
					int num6 = ((decimal)this.user.Points > num5) ? ((int)num5) : this.user.Points;
					decimal d = Math.Round((decimal)num6 / (decimal)masterSettings.ShoppingDeduction, 2);
					if (d > decimal.Zero && num6 > 0)
					{
						HtmlInputHidden htmlInputHidden6 = this.hidShoppingDeduction;
						int num7 = masterSettings.ShoppingDeduction;
						htmlInputHidden6.Value = num7.ToString();
						this.lblMaxPoints.Text = num6.ToString();
						this.lblMaxPointsToPrice.Text = (num6 / masterSettings.ShoppingDeduction).F2ToString("f2");
						this.hidCanPointUseWithCoupon.Value = (masterSettings.CanPointUseWithCoupon ? "true" : "false");
						this.hidShoppingDeductionRatio.Value = shoppingDeductionRatio.ToString();
						HtmlInputHidden htmlInputHidden7 = this.hidMyPoints;
						num7 = this.user.Points;
						htmlInputHidden7.Value = num7.ToString();
						this.hidCanUsePoint.Value = "true";
					}
				}
				this.bindPromtionsInfo();
				this.BindLastInvoiceInfo();
				PageTitle.AddSiteNameTitle("订单确认");
			}
			else if (!this.hasError)
			{
				this.ShowWapMessage("该订单没有任何商品，请重新选择", "goHomeUrl");
			}
		}

		private void rptCartProducts_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				HtmlAnchor htmlAnchor = (HtmlAnchor)e.Item.FindControl("lnkProductReview");
				HtmlAnchor htmlAnchor2 = (HtmlAnchor)e.Item.FindControl("lnkProductName");
				Literal literal = e.Item.FindControl("lblName") as Literal;
				HtmlInputHidden htmlInputHidden = (HtmlInputHidden)e.Item.FindControl("hidSkucontent");
				HtmlInputHidden htmlInputHidden2 = (HtmlInputHidden)e.Item.FindControl("hidPromotionName");
				HtmlInputHidden htmlInputHidden3 = (HtmlInputHidden)e.Item.FindControl("hidPromotionShortName");
				Label label = e.Item.FindControl("lblAdjustedPrice") as Label;
				Label label2 = e.Item.FindControl("lblQuantity") as Label;
				Label label3 = e.Item.FindControl("lblSend") as Label;
				label.Text = ((decimal)DataBinder.Eval(e.Item.DataItem, "AdjustedPrice")).F2ToString("f2");
				int num = (int)DataBinder.Eval(e.Item.DataItem, "Quantity");
				int num2 = (int)DataBinder.Eval(e.Item.DataItem, "ShippQuantity");
				label2.Text = num.ToString();
				label3.Text = ((num == num2) ? "" : (" &nbsp;&nbsp;&nbsp;赠送：<i>" + (num2 - num) + "</i>"));
				literal.Text = DataBinder.Eval(e.Item.DataItem, "Name").ToString();
				htmlInputHidden.Value = DataBinder.Eval(e.Item.DataItem, "SkuContent").ToString();
				htmlInputHidden2.Value = DataBinder.Eval(e.Item.DataItem, "PromotionName").ToNullString();
				htmlInputHidden3.Value = PromotionHelper.GetShortName((PromoteType)DataBinder.Eval(e.Item.DataItem, "PromoteType"));
				int num3 = 0;
				int.TryParse(DataBinder.Eval(e.Item.DataItem, "ProductId").ToString(), out num3);
				if (this.storeId > 0)
				{
					HtmlAnchor htmlAnchor3 = htmlAnchor2;
					HtmlAnchor htmlAnchor4 = htmlAnchor;
					string text3 = htmlAnchor3.HRef = (htmlAnchor4.HRef = $"javascript:showStoreProductDetail({num3},{this.storeId})");
				}
				else
				{
					HtmlAnchor htmlAnchor5 = htmlAnchor2;
					HtmlAnchor htmlAnchor6 = htmlAnchor;
					string text3 = htmlAnchor5.HRef = (htmlAnchor6.HRef = $"javascript:showProductDetail({num3})");
				}
			}
		}

		private void BindSendGifts()
		{
			List<ShoppingCartGiftInfo> list = new List<ShoppingCartGiftInfo>();
			list.AddRange((from s in this.cart.LineGifts
			where s.PromoType != 0
			select s).ToList());
			if (!this.isGroupBuy && !this.isCountDown)
			{
				foreach (ShoppingCartItemInfo lineItem in this.cart.LineItems)
				{
					PromotionInfo productPromotionInfo = ProductBrowser.GetProductPromotionInfo(lineItem.ProductId);
					if (productPromotionInfo != null && productPromotionInfo.PromoteType == PromoteType.SentGift && this.storeId <= 0)
					{
						IList<GiftInfo> giftDetailsByGiftIds = ProductBrowser.GetGiftDetailsByGiftIds(productPromotionInfo.GiftIds);
						foreach (GiftInfo item in giftDetailsByGiftIds)
						{
							ShoppingCartGiftInfo shoppingCartGiftInfo = new ShoppingCartGiftInfo();
							shoppingCartGiftInfo.GiftId = item.GiftId;
							shoppingCartGiftInfo.Quantity = lineItem.ShippQuantity;
							shoppingCartGiftInfo.ShippingTemplateId = item.ShippingTemplateId;
							shoppingCartGiftInfo.CostPrice = (item.CostPrice.HasValue ? item.CostPrice.Value : decimal.Zero);
							shoppingCartGiftInfo.Name = item.Name;
							shoppingCartGiftInfo.NeedPoint = item.NeedPoint;
							shoppingCartGiftInfo.PromoType = (int)productPromotionInfo.PromoteType;
							shoppingCartGiftInfo.Volume = item.Volume;
							shoppingCartGiftInfo.Weight = item.Weight;
							shoppingCartGiftInfo.ThumbnailUrl100 = item.ThumbnailUrl100;
							shoppingCartGiftInfo.ThumbnailUrl180 = item.ThumbnailUrl180;
							shoppingCartGiftInfo.ThumbnailUrl40 = item.ThumbnailUrl40;
							shoppingCartGiftInfo.ThumbnailUrl60 = item.ThumbnailUrl60;
							shoppingCartGiftInfo.IsExemptionPostage = item.IsExemptionPostage;
							shoppingCartGiftInfo.ShippingTemplateId = item.ShippingTemplateId;
							list.Add(shoppingCartGiftInfo);
						}
					}
				}
			}
			this.rptCartGifts.DataSource = list;
			this.rptCartGifts.DataBind();
			this.divlinegifts.Visible = (list.Count > 0);
		}

		private void BindPointGifts()
		{
			List<ShoppingCartGiftInfo> list = (from s in this.cart.LineGifts
			where s.PromoType == 0
			select s).ToList();
			if (list.Count() > 0)
			{
				this.rptCartPointGifts.DataSource = list;
				this.rptCartPointGifts.DataBind();
				this.divGifts.Visible = true;
				int num = (from i in this.cart.LineItems
				where i.SupplierId == 0
				select i).Count();
				if (num <= 0 && list.Count() > 0)
				{
					decimal num2 = ShoppingProcessor.CalcGiftFreight(this.userRegionId, this.cart.LineGifts);
					this.lblGiftFeright.Text = "运费：￥" + num2.F2ToString("f2");
				}
			}
		}

		private void bindPromtionsInfo()
		{
			if (this.cart.IsReduced)
			{
				this.hlkReducedPromotion.Text = "<h2>" + this.cart.ReducedPromotionName + string.Format(" 优惠：</h2>{0}", this.cart.ReducedPromotionAmount.F2ToString("f2"));
				this.hlkReducedPromotion.NavigateUrl = base.GetRouteUrl("FavourableDetails", new
				{
					activityId = this.cart.ReducedPromotionId
				});
				this.lblDeductibleMoney.InnerHtml = this.cart.ReducedPromotionAmount.F2ToString("f2");
			}
			if (!this.cart.IsSendTimesPoint)
			{
				goto IL_00b8;
			}
			goto IL_00b8;
			IL_00b8:
			if (this.cart.IsFreightFree)
			{
				this.hlkFeeFreight.Text = $"（{this.cart.FreightFreePromotionName}）";
				this.hlkFeeFreight.NavigateUrl = base.GetRouteUrl("FavourableDetails", new
				{
					activityId = this.cart.FreightFreePromotionId
				});
			}
		}

		private void bindCouponInfo()
		{
			DataTable dataTable = null;
			if (HiContext.Current.User != null && HiContext.Current.UserId != 0 && this.cart.GetTotal(this.storeId > 0) > decimal.Zero)
			{
				dataTable = ShoppingProcessor.GetCoupon(this.cart.GetTotal(this.storeId > 0), this.cart.LineItems, this.isGroupBuy, this.isCountDown, this.isFightGroup);
				this.rptOrderCoupon.DataSource = dataTable;
				this.rptOrderCoupon.DataBind();
			}
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				CouponItemInfo userCouponInfo = ShoppingProcessor.GetUserCouponInfo(this.cart.GetTotal(this.storeId > 0), dataTable.Rows[0]["ClaimCode"].ToString());
				if (userCouponInfo != null)
				{
					this.litCouponAmout.InnerHtml = (userCouponInfo.Price.HasValue ? userCouponInfo.Price.Value.F2ToString("f2") : "0.00");
					this.htmlCouponCode.Value = userCouponInfo.ClaimCode;
					this.couponName.InnerHtml = "-￥" + (userCouponInfo.Price.HasValue ? userCouponInfo.Price.Value.F2ToString("f2") : "0.00");
				}
			}
		}

		private void bindShippingAddress()
		{
			IList<ShippingAddressInfo> list = new List<ShippingAddressInfo>();
			IList<ShippingAddressInfo> shippingAddresses = MemberProcessor.GetShippingAddresses(false);
			if (shippingAddresses == null || shippingAddresses.Count == 0)
			{
				HtmlGenericControl htmlGenericControl = this.lblShippModePrice;
				HtmlInputHidden htmlInputHidden = this.hidOrderFreight;
				string text3 = htmlGenericControl.InnerText = (htmlInputHidden.Value = "0");
			}
			else
			{
				int num = this.Page.Request.QueryString["ShipAddressId"].ToInt(0);
				if (num > 0)
				{
					this.shipperAddress = MemberProcessor.GetShippingAddress(num);
				}
				if (this.shipperAddress == null)
				{
					IList<ShippingAddressInfo> shippingAddresses2 = MemberProcessor.GetShippingAddresses(false);
					if (shippingAddresses2.Count > 0)
					{
						this.shipperAddress = shippingAddresses2[0];
						list.Add(this.shipperAddress);
					}
				}
				else
				{
					this.shipperAddress.FullAddress = RegionHelper.GetFullRegion(this.shipperAddress.RegionId, " ", true, 0) + " " + this.shipperAddress.RegionLocation + " " + this.shipperAddress.Address + " " + this.shipperAddress.BuildingNumber;
					list.Add(this.shipperAddress);
				}
				if (this.shipperAddress != null)
				{
					HtmlInputHidden control = this.selectShipTo;
					int num2 = this.shipperAddress.ShippingId;
					control.SetWhenIsNotNull(num2.ToString());
					HtmlInputHidden control2 = this.hidRegionId;
					num2 = this.shipperAddress.RegionId;
					control2.SetWhenIsNotNull(num2.ToString());
					this.userRegionId = this.shipperAddress.RegionId;
					this.addressFullRegionPath = this.shipperAddress.FullRegionPath;
					this.addressLatLng = this.shipperAddress.LatLng;
					this.rptAddress.DataSource = list;
					this.rptAddress.DataBind();
				}
				decimal num3 = default(decimal);
				if (!this.cart.IsFreightFree)
				{
					num3 = ShoppingProcessor.CalcFreight(this.shipperAddress.RegionId, this.cart);
				}
				if ((this.shippingModeId == -2 && this.chooseStoreId > 0) || this.cart.IsFreightFree)
				{
					this.lblShippModePrice.InnerText = "0";
				}
				else
				{
					this.lblShippModePrice.InnerText = num3.F2ToString("f2");
				}
				this.hidOrderFreight.Value = num3.F2ToString("f2");
				this.BindIDCertification(this.shipperAddress);
			}
		}

		private void bindShippingPaymentInfo()
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			bool flag = masterSettings.OpenMultStore && !this.isPreSale;
			this.hidIsMultiStore.Value = (flag ? "1" : "0");
			this.hidPaymentId_Podrequest.Value = "0";
			int num = 0;
			int num2 = 0;
			if (this.from != "countdown" && this.from != "groupbuy")
			{
				if (this.from != "presale" && SalesHelper.IsSupportPodrequest())
				{
					num = 1;
					this.hidPaymentId_Podrequest.Value = "1";
					this.hidIsCashOnDelivery.Value = "true";
				}
				if (ShoppingProcessor.IsSupportOfflineRequest())
				{
					this.hidPaymentId_Offline.Value = "2";
					num2 = 2;
					this.hidIsOfflinePay.Value = "true";
				}
			}
			if (this.storeId > 0 & flag)
			{
				StoresInfo storeById = DepotHelper.GetStoreById(this.storeId);
				if (storeById != null)
				{
					this.hidIsOfflinePay.Value = storeById.IsOfflinePay.ToNullString().ToLower();
					this.hidIsOnlinePay.Value = storeById.IsOnlinePay.ToNullString().ToLower();
					this.hidIsCashOnDelivery.Value = storeById.IsCashOnDelivery.ToNullString().ToLower();
					if (this.from != "countdown" && this.from != "groupbuy")
					{
						if (this.from != "presale" && storeById.IsCashOnDelivery)
						{
							num = 1;
							this.hidPaymentId_Podrequest.Value = "1";
						}
						if (storeById.IsOfflinePay)
						{
							this.hidPaymentId_Offline.Value = "2";
							num2 = 2;
						}
					}
					if (this.userRegionId > 0 && this.cart.LineItems.Count > 0)
					{
						this.hidIsSubmitInTime.Value = "1";
						this.hidStoreName.Value = storeById.StoreName;
						this.storeName.InnerText = storeById.StoreName;
						this.storeTel.InnerText = storeById.Tel;
						this.storeAddress.InnerText = RegionHelper.GetFullRegion(storeById.RegionId, string.Empty, true, 0) + storeById.Address;
						this.storeTime.InnerText = "营业时间" + storeById.StoreOpenTime;
						this.hidStoreId.Value = this.storeId.ToString();
						if (!SettingsManager.GetMasterSettings().Store_IsOrderInClosingTime)
						{
							DateTime dateTime = DateTime.Now;
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
							if (DateTime.Now < value || DateTime.Now > dateTime2)
							{
								this.hidIsSubmitInTime.Value = "0";
							}
						}
						bool flag2 = DepotHelper.IsStoreInDeliveArea(this.storeId, this.addressFullRegionPath);
						double num3 = MapHelper.GetLatLngDistance(storeById.LatLng, this.addressLatLng);
						if (num3 > 0.0)
						{
							num3 = Math.Round(num3 / 1000.0, 2);
						}
						this.storeDistance.InnerText = "距您" + num3.F2ToString("f2") + "KM";
						decimal amount = this.cart.GetAmount(true);
						this.hidIsSupportStoreDelive.Value = (storeById.IsStoreDelive ? "1" : "0");
						if (storeById.IsStoreDelive)
						{
							if (flag2 || storeById.ServeRadius >= num3)
							{
								this.hidIsStoreDelive.Value = "1";
							}
							decimal d = amount;
							decimal? minOrderPrice = storeById.MinOrderPrice;
							if (d >= minOrderPrice.GetValueOrDefault() && minOrderPrice.HasValue)
							{
								this.hidIsGetMinOrderPrice.Value = "1";
							}
						}
						this.hidIsSupportExpress.Value = (storeById.IsSupportExpress ? "1" : "0");
						this.hidGetgoodsOnStores.Value = (storeById.IsAboveSelf ? "1" : "0");
						if (this.cart.IsFreightFree)
						{
							this.hidStoreFreight.Value = "0";
						}
						else
						{
							HtmlInputHidden htmlInputHidden = this.hidStoreFreight;
							decimal? minOrderPrice = storeById.StoreFreight;
							htmlInputHidden.Value = ((minOrderPrice.GetValueOrDefault() > default(decimal) && minOrderPrice.HasValue) ? storeById.StoreFreight.Value : decimal.Zero).F2ToString("f2");
						}
					}
				}
			}
			else
			{
				this.hidIsSupportExpress.Value = "1";
				if (masterSettings.IsOpenPickeupInStore && this.cart.LineItems.Count > 0 && this.storeId == 0)
				{
					this.hidGetgoodsOnStores.Value = "1";
					if (!flag)
					{
						this.hidPickeupInStoreRemark.Value = masterSettings.PickeupInStoreRemark;
					}
				}
				if (this.paymentModeId != 0 && this.paymentModeId != num && this.paymentModeId != -3 && this.paymentModeId != num2)
				{
					this.paymentModeId = 0;
				}
				if (this.paymentModeId == -3 && this.isFightGroup)
				{
					this.paymentModeId = 0;
				}
				if (flag && this.cart.LineItems.Count > 0)
				{
					if (this.shippingModeId == -2 && this.chooseStoreId <= 0)
					{
						this.shippingModeId = 0;
						if (this.paymentModeId == -3)
						{
							this.paymentModeId = 0;
						}
					}
					if (this.shippingModeId == -2 && this.chooseStoreId > 0 && !string.IsNullOrWhiteSpace(this.addressLatLng))
					{
						StoresInfo storeById2 = DepotHelper.GetStoreById(this.chooseStoreId);
						if (storeById2 != null)
						{
							this.hidStoreName.Value = storeById2.StoreName;
							this.storeName.InnerText = storeById2.StoreName;
							this.storeTel.InnerText = storeById2.Tel;
							this.storeAddress.InnerText = RegionHelper.GetFullRegion(storeById2.RegionId, string.Empty, true, 0) + storeById2.Address;
							this.storeTime.InnerText = "营业时间 " + storeById2.StoreOpenTime;
							double latLngDistance = MapHelper.GetLatLngDistance(storeById2.LatLng, this.addressLatLng);
							this.storeDistance.InnerText = "距您" + (latLngDistance / 1000.0).F2ToString("f2") + "KM";
						}
						else
						{
							this.shippingModeId = 0;
							if (this.paymentModeId == -3)
							{
								this.paymentModeId = 0;
							}
						}
					}
					if (this.paymentModeId == -3 && this.shippingModeId != -2)
					{
						this.paymentModeId = 0;
					}
					this.hidChooseStoreId.Value = this.chooseStoreId.ToString();
					string str3 = this.productSku.Replace(",", "','");
					str3 = "'" + str3 + "'";
					if (this.from.ToLower() != "groupbuy" && (this.from.ToLower() != "fightgroup" || (this.from.ToLower() == "fightgroup" && this.siteSettings.FitGroupIsOpenPickeupInStore)) && !this.isPreSale && ShoppingCartProcessor.CanGetGoodsOnStore(str3) && this.userRegionId > 0)
					{
						bool flag3 = StoresHelper.HasStoresInCity(str3, this.userRegionId);
						this.hidHasStoresInCity.Value = (flag3 ? "true" : "false");
					}
				}
				else
				{
					this.shippingModeId = 0;
					if (this.paymentModeId == -3)
					{
						this.paymentModeId = 0;
					}
				}
				if (num != 1 && this.paymentModeId == 1)
				{
					this.paymentModeId = 0;
				}
				if (num2 != 2 && this.paymentModeId == 2)
				{
					this.paymentModeId = 0;
				}
				this.hidDeliveryTime.Value = this.deliveryTime;
			}
			if (this.paymentModeId == 0)
			{
				if (TradeHelper.WapPaymentTypeCount(ClientType.App, this.isFightGroup) > 0)
				{
					this.inputPaymentModeId.Value = this.paymentModeId.ToString();
				}
			}
			else
			{
				this.inputPaymentModeId.Value = this.paymentModeId.ToString();
			}
			this.inputShippingModeId.Value = this.shippingModeId.ToString();
			if (this.from == "countDown")
			{
				this.paymenttypeselect.OrderSalesPromotion = Common_AppPaymentTypeSelect.EnumOrderSalesPromotion.CountDownBuy;
			}
			this.paymenttypeselect.IsFireGroup = this.isFightGroup;
		}

		private bool CheckPresaleInfo()
		{
			if (this.PresaleId > 0)
			{
				ProductPreSaleInfo productPreSaleInfo = ProductPreSaleHelper.GetProductPreSaleInfo(this.PresaleId);
				if (productPreSaleInfo == null)
				{
					this.ShowWapMessage("活动不存在！", "");
					return false;
				}
				if (this.productSku.Split(',').Length > 1)
				{
					base.GotoResourceNotFound("");
					return false;
				}
				if (!ProductPreSaleHelper.HasProductPreSaleInfo(this.productSku, this.PresaleId))
				{
					base.GotoResourceNotFound("");
					return false;
				}
				return true;
			}
			base.GotoResourceNotFound("");
			return false;
		}

		private bool CheckProductSkuHasProductPreInfo()
		{
			string[] skuIdArray = this.productSku.Split(',');
			return ProductPreSaleHelper.HasProductPreSaleInfoBySkuIds(skuIdArray);
		}

		private void BindIDCertification(ShippingAddressInfo tempShipAddress)
		{
			if (HiContext.Current.SiteSettings.IsOpenCertification && (from i in this.cart.LineItems
			where i.IsCrossborder
			select i).Count() > 0 && this.hidIsOpenCertification != null && tempShipAddress != null)
			{
				if (string.IsNullOrWhiteSpace(tempShipAddress.IDNumber))
				{
					this.hidIsOpenCertification.Value = "1";
				}
				if (HiContext.Current.SiteSettings.CertificationModel == 2 && (string.IsNullOrWhiteSpace(tempShipAddress.IDImage1) || string.IsNullOrWhiteSpace(tempShipAddress.IDImage2)))
				{
					this.hidIsOpenCertification.Value = "1";
				}
			}
		}

		private void BindLastInvoiceInfo()
		{
			IList<UserInvoiceDataInfo> userInvoiceDataList = MemberProcessor.GetUserInvoiceDataList(HiContext.Current.UserId, this.shipperAddress);
			string value = JsonConvert.SerializeObject(new
			{
				List = from i in userInvoiceDataList
				select new
				{
					i.Id,
					i.InvoiceType,
					i.InvoiceTitle,
					i.InvoiceTaxpayerNumber,
					i.OpenBank,
					i.BankAccount,
					i.ReceiveAddress,
					i.ReceiveEmail,
					i.ReceiveName,
					i.ReceivePhone,
					i.ReceiveRegionId,
					i.ReceiveRegionName,
					i.RegisterAddress,
					i.RegisterTel
				}
			});
			this.hidInvoiceJson.Value = value;
		}
	}
}
