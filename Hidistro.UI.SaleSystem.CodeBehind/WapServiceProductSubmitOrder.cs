using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
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
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WapServiceProductSubmitOrder : WAPMemberTemplatedWebControl
	{
		private Literal litShipTo;

		private Literal litCellPhone;

		private Literal litAddress;

		private HtmlInputControl groupbuyHiddenBox;

		private HtmlInputControl countdownHiddenBox;

		private HtmlInputControl fightGroupHiddenBox;

		private HtmlInputControl fightGroupActivityHiddenBox;

		private WapTemplatedRepeater rptPromotions;

		private WapTemplatedRepeater rptCartGifts;

		private WapTemplatedRepeater rptShippingType;

		private WapTemplatedRepeater rptCartPointGifts;

		private Common_SubmmitCartProducts rptCartProducts;

		private Common_CouponSelect dropCoupon;

		private Label lblOrderTotal;

		private HtmlGenericControl lblTotalPrice;

		private HtmlGenericControl lblTotalPrice1;

		private HtmlGenericControl divGifts;

		private HtmlGenericControl lblTax;

		private Common_WAPPaymentTypeSelect paymenttypeselect;

		private HtmlInputControl hdCurrentckIds;

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

		private HtmlInputHidden inputPaymentModeId;

		private HtmlInputHidden hidStoreId;

		private HtmlInputHidden hidStoreName;

		private HtmlInputHidden htmlCouponCode;

		private HtmlInputHidden hidCanUsePoint;

		private HtmlGenericControl lblDeductibleMoney;

		private HtmlGenericControl litCouponAmout;

		private HtmlGenericControl couponName;

		private HtmlInputHidden hidIsPreSale;

		private HtmlInputHidden hidOnlinePayCount;

		private Label litTaxRate;

		private HyperLink hlkFeeFreight;

		private HyperLink hlkReducedPromotion;

		private WapTemplatedRepeater rptOrderCoupon;

		private HtmlGenericControl divTax;

		private HtmlGenericControl spanTaxRate;

		private HtmlGenericControl lblAmount;

		private HtmlGenericControl lblDepositMoney;

		private HtmlGenericControl lblRetainage;

		private Label lblGiftFeright;

		private HtmlInputHidden hidHasSupplierProduct;

		private HtmlInputHidden hidPointRate;

		private HtmlGenericControl spandemo;

		private HtmlGenericControl divlinegifts;

		private HtmlInputHidden hidHasStoresInCity;

		private HtmlInputHidden hidPaymentId_Offline;

		private HtmlInputHidden hidChooseStoreId;

		private HtmlInputHidden hidHasTradePassword;

		private HtmlInputHidden hidIsSubmitInTime;

		private HtmlInputHidden hidIsGeneralMuti;

		private HtmlGenericControl storeName;

		private HtmlGenericControl storeTel;

		private HtmlGenericControl storeAddress;

		private HtmlGenericControl storeTime;

		private HtmlGenericControl storeDistance;

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

		private HtmlInputHidden hidInvoiceJson;

		private ShippingAddressInfo shipperAddress = null;

		private int buyAmount;

		private string productSku;

		private int groupBuyId;

		private int countDownId;

		private int fightGroupActivityId;

		private int fightGroupId;

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

		private bool isPreSale = false;

		private HtmlInputHidden hidIsMultiStore;

		private int PresaleId;

		private bool hasError = false;

		private int userRegionId = 0;

		private string addressFullRegionPath = "";

		private string addressLatLng = "";

		private HtmlInputHidden hidInputItemsJson;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-ServiceProductSubmitOrder.html";
			}
			base.OnInit(e);
			string urlToEncode = "";
			this.from = this.Page.Request.QueryString["from"].ToNullString().ToLower();
			this.productSku = Globals.UrlDecode(this.Page.Request.QueryString["productSku"].ToNullString());
			this.buyAmount = this.Page.Request.QueryString["buyAmount"].ToInt(0);
			this.storeId = this.Page.Request.QueryString["StoreId"].ToInt(0);
			this.chooseStoreId = this.Page.Request.QueryString["ChooseStoreId"].ToInt(0);
			if (this.from == "groupbuy")
			{
				this.isGroupBuy = true;
			}
			else if (this.from == "countdown")
			{
				this.isCountDown = true;
			}
			else if (this.from == "fightgroup")
			{
				this.isFightGroup = true;
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
						goto IL_01cd;
					}
					return;
				}
				this.from = "serviceproduct";
			}
			goto IL_01cd;
			IL_01cd:
			if (this.storeId == 0 && !HiContext.Current.SiteSettings.OpenMultStore)
			{
				this.cart = ShoppingCartProcessor.GetShoppingCart(this.from, this.productSku, this.buyAmount, 0, true, -1, this.Page.Request["fightGroupActivityId"].ToInt(0));
			}
			else
			{
				this.cart = ShoppingCartProcessor.GetShoppingCart(this.from, this.productSku, this.buyAmount, 0, true, this.storeId, this.Page.Request["fightGroupActivityId"].ToInt(0));
			}
			if (this.cart == null)
			{
				string msg = "购物车无任何商品！";
				this.hasError = true;
				this.ShowWapMessage(msg, "default.aspx");
			}
			else
			{
				if (this.isGroupBuy)
				{
					this.groupbuyInfo = TradeHelper.GetProductGroupBuyInfo(this.cart.LineItems[0].ProductId, this.buyAmount, out urlToEncode);
					if (this.groupbuyInfo == null)
					{
						this.hasError = true;
						this.ShowWapMessage(Globals.UrlEncode(urlToEncode), "default.aspx");
						return;
					}
				}
				if (this.isCountDown)
				{
					this.countdownInfo = TradeHelper.ProductExistsCountDown(this.cart.LineItems[0].ProductId, "", this.storeId);
					if (this.countdownInfo == null)
					{
						this.hasError = true;
						this.ShowWapMessage("该商品未进行抢购活动,或者活动已结束", "Default.aspx");
						return;
					}
					if (!StoreActivityHelper.JoinActivity(this.countdownInfo.CountDownId, 2, this.storeId, this.countdownInfo.StoreType))
					{
						this.hasError = true;
						this.ShowWapMessage("该门店未参与此抢购活动", "Default.aspx");
						return;
					}
					this.countdownInfo = TradeHelper.CheckUserCountDown(this.cart.LineItems[0].ProductId, this.countdownInfo.CountDownId, this.cart.LineItems[0].SkuId, HiContext.Current.UserId, this.buyAmount, "", out urlToEncode, this.storeId);
					if (this.countdownInfo == null)
					{
						this.ShowWapMessage(Globals.UrlEncode(urlToEncode), "Default.aspx");
						return;
					}
				}
				if (this.isFightGroup)
				{
					this.fightGroupActivitiyInfo = TradeHelper.GetFightGroupActivitieInfo(this.Page.Request["fightGroupActivityId"].ToInt(0));
					if (this.fightGroupActivitiyInfo == null)
					{
						this.hasError = true;
						this.ShowWapMessage("拼团活动不存在", "Default.aspx");
					}
					else
					{
						int num = this.Page.Request["fightGroupId"].ToInt(0);
						this.fightGroupInfo = VShopHelper.GetFightGroup(num);
						if (this.fightGroupInfo == null && num != 0)
						{
							this.hasError = true;
							this.ShowWapMessage("拼团活动不存在", "Default.aspx");
						}
					}
				}
			}
		}

		protected override void AttachChildControls()
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			string empty = string.Empty;
			this.spandemo = (HtmlGenericControl)this.FindControl("spandemo");
			this.spandemo.Visible = masterSettings.IsDemoSite;
			this.litShipTo = (Literal)this.FindControl("litShipTo");
			this.litCellPhone = (Literal)this.FindControl("litCellPhone");
			this.litAddress = (Literal)this.FindControl("litAddress");
			this.rptCartProducts = (Common_SubmmitCartProducts)this.FindControl("Common_SubmmitCartProducts");
			this.rptCartProducts.ItemDataBound += this.rptCartProducts_ItemDataBound;
			this.rptCartGifts = (WapTemplatedRepeater)this.FindControl("rptCartGifts");
			this.rptCartPointGifts = (WapTemplatedRepeater)this.FindControl("rptCartPointGifts");
			this.divGifts = (HtmlGenericControl)this.FindControl("divGifts");
			this.dropCoupon = (Common_CouponSelect)this.FindControl("dropCoupon");
			this.lblOrderTotal = (Label)this.FindControl("lblOrderTotal");
			this.groupbuyHiddenBox = (HtmlInputControl)this.FindControl("groupbuyHiddenBox");
			this.countdownHiddenBox = (HtmlInputControl)this.FindControl("countdownHiddenBox");
			this.fightGroupHiddenBox = (HtmlInputControl)this.FindControl("fightGroupHiddenBox");
			this.fightGroupActivityHiddenBox = (HtmlInputControl)this.FindControl("fightGroupActivityHiddenBox");
			this.lblTotalPrice = (HtmlGenericControl)this.FindControl("lblTotalPrice");
			this.lblTotalPrice1 = (HtmlGenericControl)this.FindControl("lblTotalPrice1");
			this.lblTax = (HtmlGenericControl)this.FindControl("lblTax");
			this.rptPromotions = (WapTemplatedRepeater)this.FindControl("rptPromotions");
			this.rptShippingType = (WapTemplatedRepeater)this.FindControl("rptShippingType");
			this.paymenttypeselect = (Common_WAPPaymentTypeSelect)this.FindControl("paymenttypeselect");
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
			this.inputPaymentModeId = (HtmlInputHidden)this.FindControl("inputPaymentModeId");
			this.hidStoreId = (HtmlInputHidden)this.FindControl("hidStoreId");
			this.hidChooseStoreId = (HtmlInputHidden)this.FindControl("hidChooseStoreId");
			this.hidStoreName = (HtmlInputHidden)this.FindControl("hidStoreName");
			this.lblMaxPointsToPrice = (Label)this.FindControl("lblMaxPointsToPrice");
			this.lblDeductibleMoney = (HtmlGenericControl)this.FindControl("lblDeductibleMoney");
			this.litTaxRate = (Label)this.FindControl("litTaxRate");
			this.hlkFeeFreight = (HyperLink)this.FindControl("hlkFeeFreight");
			this.hlkReducedPromotion = (HyperLink)this.FindControl("hlkReducedPromotion");
			this.rptOrderCoupon = (WapTemplatedRepeater)this.FindControl("rptOrderCoupon");
			this.htmlCouponCode = (HtmlInputHidden)this.FindControl("htmlCouponCode");
			this.couponName = (HtmlGenericControl)this.FindControl("couponName");
			this.litCouponAmout = (HtmlGenericControl)this.FindControl("litCouponAmout");
			this.hidCanUsePoint = (HtmlInputHidden)this.FindControl("hidCanUsePoint");
			this.divTax = (HtmlGenericControl)this.FindControl("divTax");
			this.spanTaxRate = (HtmlGenericControl)this.FindControl("spanTaxRate");
			this.lblGiftFeright = (Label)this.FindControl("lblGiftFeright");
			this.hidHasSupplierProduct = (HtmlInputHidden)this.FindControl("hidHasSupplierProduct");
			this.hidPointRate = (HtmlInputHidden)this.FindControl("hidPointRate");
			this.divlinegifts = (HtmlGenericControl)this.FindControl("divlinegifts");
			this.hidOnlinePayCount = (HtmlInputHidden)this.FindControl("hidOnlinePayCount");
			this.hidHasStoresInCity = (HtmlInputHidden)this.FindControl("hidHasStoresInCity");
			this.hidPaymentId_Offline = (HtmlInputHidden)this.FindControl("hidPaymentId_Offline");
			this.storeName = (HtmlGenericControl)this.FindControl("storeName");
			this.storeTel = (HtmlGenericControl)this.FindControl("storeTel");
			this.storeAddress = (HtmlGenericControl)this.FindControl("storeAddress");
			this.storeTime = (HtmlGenericControl)this.FindControl("storeTime");
			this.storeDistance = (HtmlGenericControl)this.FindControl("storeDistance");
			this.hidIsSubmitInTime = (HtmlInputHidden)this.FindControl("hidIsSubmitInTime");
			this.hidInputItemsJson = (HtmlInputHidden)this.FindControl("hidInputItemsJson");
			this.hidInvoiceType = (HtmlInputHidden)this.FindControl("hidInvoiceType");
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
			this.hidInvoiceTitle = (HtmlInputHidden)this.FindControl("hidInvoiceTitle");
			this.hidInvoiceTaxpayerNumber = (HtmlInputHidden)this.FindControl("hidInvoiceTaxpayerNumber");
			this.hidOpenBalancePay.Value = masterSettings.OpenBalancePay.ToNullString().ToLower();
			HtmlInputHidden htmlInputHidden = this.hidEnableETax;
			bool flag = masterSettings.EnableTax;
			htmlInputHidden.Value = flag.ToString().ToLower();
			HtmlInputHidden htmlInputHidden2 = this.hidEnableTax;
			flag = masterSettings.EnableE_Invoice;
			htmlInputHidden2.Value = flag.ToString().ToLower();
			HtmlInputHidden htmlInputHidden3 = this.hidEnableVATTax;
			flag = masterSettings.EnableVATInvoice;
			htmlInputHidden3.Value = flag.ToString().ToLower();
			this.hidVATTaxRate.Value = masterSettings.VATTaxRate.ToNullString();
			this.hidTaxRate.Value = masterSettings.TaxRate.ToNullString();
			this.hidVATInvoiceDays.Value = masterSettings.VATInvoiceDays.ToNullString();
			this.litAfterSaleDays.SetWhenIsNotNull(masterSettings.EndOrderDays.ToNullString());
			this.litInvoiceSendDays.SetWhenIsNotNull((masterSettings.EndOrderDays + masterSettings.VATInvoiceDays).ToNullString());
			this.hidOpenBalancePay.Value = masterSettings.OpenBalancePay.ToNullString().ToLower();
			this.hidIsPreSale = (HtmlInputHidden)this.FindControl("hidIsPreSale");
			this.lblAmount = (HtmlGenericControl)this.FindControl("lblAmount");
			this.lblDepositMoney = (HtmlGenericControl)this.FindControl("lblDepositMoney");
			this.lblRetainage = (HtmlGenericControl)this.FindControl("lblRetainage");
			this.hidHasTradePassword = (HtmlInputHidden)this.FindControl("hidHasTradePassword");
			this.hidIsGeneralMuti = (HtmlInputHidden)this.FindControl("hidIsGeneralMuti");
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
				string value = string.Empty;
				if (string.IsNullOrEmpty(this.productSku))
				{
					HttpCookie httpCookie = HiContext.Current.Context.Request.Cookies["ckids"];
					if (httpCookie != null && !string.IsNullOrEmpty(httpCookie.Value))
					{
						value = Globals.UrlDecode(httpCookie.Value);
					}
				}
				else
				{
					value = this.productSku;
				}
				this.hdCurrentckIds.Value = value;
			}
			if (this.cart != null && ((this.cart.LineItems != null && this.cart.LineItems.Count > 0) || (this.cart.LineGifts != null && this.cart.LineGifts.Count > 0)))
			{
				if (!TradeHelper.CheckShoppingStock(this.cart, out empty, this.storeId))
				{
					this.ShowWapMessage("订单中有商品(" + empty + ")库存不足", "ShoppingCart.aspx");
				}
				this.hidOnlinePayCount.Value = TradeHelper.WapPaymentTypeCount(base.ClientType, this.isFightGroup).ToNullString();
				this.bindShippingPaymentInfo();
				this.divTax.Visible = false;
				Label label = this.litTaxRate;
				decimal num = masterSettings.TaxRate;
				label.Text = num.ToString(CultureInfo.InvariantCulture);
				this.spanTaxRate.Visible = (masterSettings.TaxRate > decimal.Zero);
				this.hidOrderRate.Value = ((masterSettings.TaxRate > decimal.Zero) ? (this.cart.GetTotal(this.storeId > 0) * masterSettings.TaxRate / 100m).F2ToString("f2") : "0.00");
				MemberInfo user = Users.GetUser(HiContext.Current.UserId);
				this.hidHasTradePassword.Value = (string.IsNullOrWhiteSpace(user.TradePassword) ? "0" : "1");
				this.hidBalanceAmount.Value = (user.Balance - user.RequestBalance).F2ToString("f2");
				this.lblBalance.Text = (user.Balance - user.RequestBalance).F2ToString("f2");
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
					decimal num2 = default(decimal);
					ProductPreSaleInfo productPreSaleInfo = ProductPreSaleHelper.GetProductPreSaleInfo(this.PresaleId);
					if (this.cart != null)
					{
						num2 = ((productPreSaleInfo.Deposit == decimal.Zero) ? (this.cart.LineItems[0].MemberPrice * (decimal)productPreSaleInfo.DepositPercent / 100m) : productPreSaleInfo.Deposit) * (decimal)this.cart.LineItems[0].Quantity;
					}
					this.lblTotalPrice.InnerHtml = this.cart.GetAmount(false).F2ToString("f2");
					HtmlGenericControl htmlGenericControl = this.lblTotalPrice1;
					Label label2 = this.lblOrderTotal;
					string text3 = htmlGenericControl.InnerText = (label2.Text = num2.F2ToString("f2"));
					this.lblDepositMoney.InnerText = num2.F2ToString("f2");
					decimal num3 = this.cart.GetTotal(false) - num2;
					if (num3 < decimal.Zero)
					{
						num3 = default(decimal);
					}
					this.lblRetainage.InnerText = num3.F2ToString("f2");
					this.lblAmount.InnerText = (num2 + num3).F2ToString("f2");
					HtmlInputHidden htmlInputHidden4 = this.hidTotalPrice;
					num = this.cart.GetTotal(false);
					htmlInputHidden4.Value = num.ToString();
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
				if (flag2 && user != null && masterSettings.ShoppingDeduction > 0 && user.Points > 0)
				{
					int shoppingDeductionRatio = masterSettings.ShoppingDeductionRatio;
					decimal num4 = (decimal)shoppingDeductionRatio * this.cart.GetTotal(this.storeId > 0) * (decimal)masterSettings.ShoppingDeduction / 100m;
					int num5 = ((decimal)user.Points > num4) ? ((int)num4) : user.Points;
					decimal d2 = Math.Round((decimal)num5 / (decimal)masterSettings.ShoppingDeduction, 2);
					if (d2 > decimal.Zero && num5 > 0)
					{
						HtmlInputHidden htmlInputHidden6 = this.hidShoppingDeduction;
						int num6 = masterSettings.ShoppingDeduction;
						htmlInputHidden6.Value = num6.ToString();
						this.lblMaxPoints.Text = num5.ToString();
						this.lblMaxPointsToPrice.Text = (num5 / masterSettings.ShoppingDeduction).F2ToString("f2");
						this.hidCanPointUseWithCoupon.Value = (masterSettings.CanPointUseWithCoupon ? "true" : "false");
						this.hidShoppingDeductionRatio.Value = shoppingDeductionRatio.ToString();
						HtmlInputHidden htmlInputHidden7 = this.hidMyPoints;
						num6 = user.Points;
						htmlInputHidden7.Value = num6.ToString();
						this.hidCanUsePoint.Value = "true";
					}
				}
				this.bindPromtionsInfo();
				this.BindLastInvoiceInfo();
				ShoppingCartItemInfo shoppingCartItemInfo = this.cart.LineItems.FirstOrDefault();
				int productId = shoppingCartItemInfo.ProductId;
				ProductInfo productBaseDetails = ProductHelper.GetProductBaseDetails(productId);
				IList<ProductInputItemInfo> productInputItemList = ProductHelper.GetProductInputItemList(productId);
				this.hidIsGeneralMuti.Value = productBaseDetails.IsGenerateMore.ToNullString().ToLower();
				string value2 = JsonConvert.SerializeObject(new
				{
					list = from d in productInputItemList
					select new
					{
						d.Id,
						d.InputFieldTitle,
						d.InputFieldType,
						d.IsRequired,
						d.InputFileValues
					}
				});
				this.hidInputItemsJson.Value = value2;
				PageTitle.AddSiteNameTitle("订单确认");
			}
			else if (!this.hasError)
			{
				this.ShowWapMessage("该订单没有任何商品，请重新选择", "default.aspx");
			}
		}

		private void BindSendGifts()
		{
			if (HiContext.Current.User.UserId != 0)
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
		}

		private void rptCartProducts_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				HtmlAnchor htmlAnchor = (HtmlAnchor)e.Item.FindControl("lnkProductReview");
				HtmlAnchor htmlAnchor2 = (HtmlAnchor)e.Item.FindControl("lnkProductName");
				Literal literal = e.Item.FindControl("lblPName") as Literal;
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
					string text3 = htmlAnchor3.HRef = (htmlAnchor4.HRef = $"StoreProductDetails.aspx?ProductId={num3}&StoreId={this.storeId}");
				}
				else
				{
					HtmlAnchor htmlAnchor5 = htmlAnchor2;
					HtmlAnchor htmlAnchor6 = htmlAnchor;
					string text3 = htmlAnchor5.HRef = (htmlAnchor6.HRef = $"ProductDetails.aspx?ProductId={num3}");
				}
			}
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

		private void bindShippingPaymentInfo()
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			bool flag = masterSettings.OpenMultStore && !this.isPreSale;
			this.hidIsMultiStore.Value = (flag ? "1" : "0");
			if (this.storeId > 0 & flag)
			{
				StoresInfo storeById = DepotHelper.GetStoreById(this.storeId);
				if (storeById != null && this.cart.LineItems.Count > 0)
				{
					this.hidIsSubmitInTime.Value = "1";
					this.hidStoreName.Value = storeById.StoreName;
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
				}
			}
			if (this.paymentModeId == 0)
			{
				if (TradeHelper.WapPaymentTypeCount(base.ClientType, this.isFightGroup) > 0)
				{
					this.inputPaymentModeId.Value = this.paymentModeId.ToString();
				}
			}
			else
			{
				this.inputPaymentModeId.Value = this.paymentModeId.ToString();
			}
			if (this.isCountDown)
			{
				this.paymenttypeselect.OrderSalesPromotion = Common_WAPPaymentTypeSelect.EnumOrderSalesPromotion.CountDownBuy;
			}
			this.paymenttypeselect.IsServiceProduct = true;
			this.paymenttypeselect.IsFireGroup = this.isFightGroup;
			this.paymenttypeselect.ClientType = base.ClientType;
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

		private void BindLastInvoiceInfo()
		{
			int num = this.Page.Request.QueryString["ShipAddressId"].ToInt(0);
			if (num > 0)
			{
				this.shipperAddress = MemberProcessor.GetShippingAddress(num);
			}
			if (this.shipperAddress == null)
			{
				IList<ShippingAddressInfo> shippingAddresses = MemberProcessor.GetShippingAddresses(false);
				if (shippingAddresses.Count > 0)
				{
					this.shipperAddress = shippingAddresses[0];
				}
			}
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
