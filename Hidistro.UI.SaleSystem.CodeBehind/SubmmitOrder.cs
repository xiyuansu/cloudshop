using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Messages;
using Hidistro.SaleSystem;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Shopping;
using Hidistro.SaleSystem.Statistics;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class SubmmitOrder : MemberTemplatedWebControl
	{
		private static object submitLock = new object();

		private RegionSelector dropRegions;

		private TextBox txtShipTo;

		private TextBox txtAddress;

		private TextBox txtZipcode;

		private TextBox txtCellPhone;

		private TextBox txtTelPhone;

		private HtmlSelect drpShipToDate;

		private TextBox txtBuilderNumber;

		private HtmlInputHidden inputPaymentModeId;

		private HtmlInputHidden inputShippingModeId;

		private HtmlInputHidden hdbuytype;

		private HtmlInputHidden hidStoreId;

		private HtmlInputHidden hidStoreCount;

		private HtmlInputHidden hidShipperId;

		private HtmlInputHidden hidIsAnonymous;

		private HtmlInputHidden hidCanUsePoint;

		private Panel pannel_useraddress;

		private Panel pannel_noaddress;

		private Common_SubmmintOrder_ProductList cartProductList;

		private Common_SubmmintOrder_GiftList cartGiftList;

		private Common_PointGiftList common_pointgiftlist;

		private FormatedMoneyLabel lblShippModePrice;

		private Literal litProductBundling;

		private Label litAllWeight;

		private Label litPoint;

		private HyperLink hlkSentTimesPoint;

		private FormatedMoneyLabel lblOrderTotal;

		private TextBox txtMessage;

		private Label litTaxRate;

		private HyperLink hlkFeeFreight;

		private HyperLink hlkReducedPromotion;

		private FormatedMoneyLabel lblTotalPrice;

		private HtmlInputHidden htmlCouponCode;

		private FormatedMoneyLabel litCouponAmout;

		private HtmlInputCheckBox chkTax;

		private HtmlSelect CmbCoupCode;

		private TextBox txtInvoiceTitle;

		private TextBox txtInvoiceTaxpayerNumber;

		private RadioButton radInvoiceType;

		private RadioButton radInvoiceType2;

		private IButton btnCreateOrder;

		private Label lblMaxPoints;

		private TextBox txtUsePoints;

		private FormatedMoneyLabel lblDeductibleMoney;

		private FormatedMoneyLabel litPointAmount;

		private HtmlInputHidden hidShoppingDeduction;

		private HtmlInputHidden hidShoppingDeductionRatio;

		private HtmlInputHidden hidCanPointUseWithCoupon;

		private HtmlInputHidden hidMyPoints;

		private HtmlInputHidden hidGetgoodsOnStores;

		private HtmlInputHidden hidIsCloseStoreButGetGoods;

		private HtmlInputHidden hidGetGoodsRemark;

		private HtmlInputHidden hidPaymentId_Podrequest;

		private HtmlInputHidden hidPaymentId_Offline;

		private HtmlInputHidden hidTotalPrice;

		private HtmlInputCheckBox chkIsUsePoints;

		private HtmlGenericControl divcopue;

		private HtmlContainerControl divProductList;

		private HtmlContainerControl spanTaxRate;

		private HtmlInputHidden hidOnlinePayCount;

		private HtmlInputHidden hidPointRate;

		private ShoppingCartInfo shoppingCart;

		private HtmlInputHidden hidIsPreSale;

		private FormatedMoneyLabel lblDeposit;

		private FormatedMoneyLabel lblFinalPayment;

		private FormatedMoneyLabel lblPreSaleOrderTotal;

		private FormatedMoneyLabel lblDepositPay;

		private Label lblGiftFeright;

		private HtmlInputHidden hidHasSupplierProduct;

		private HtmlInputHidden hidTimePoint;

		private HtmlInputHidden hidIsGiftOrder;

		private HtmlInputHidden hidOpenBalancePay;

		private HtmlInputHidden hidBalanceAmount;

		private HtmlInputHidden hidHasTradePassword;

		private TextBox hidTradePassword;

		private TextBox hidUseBalance;

		private Label lblBalance;

		private SiteSettings siteSettings;

		private HtmlInputHidden hidInvoiceJson;

		private int buyAmount;

		private string productSku;

		private bool isGroupBuy = false;

		private bool isCountDown = false;

		private bool isSignBuy = false;

		private bool isFireGroup = false;

		private bool isCombina = false;

		private bool isPreSale = false;

		private string buytype = "";

		private int bundlingid = 0;

		private GroupBuyInfo groupbuyInfo = null;

		private CountDownInfo countdownInfo = null;

		private string from = "";

		private int combinationId = 0;

		private int preSaleId;

		private int userRegionId = 0;

		private string couponUseProducts = "";

		private HtmlInputHidden hidIsOpenCertification;

		private HtmlInputHidden hidCertificationModel;

		private HtmlInputHidden hidIsGetIDInfo;

		private HtmlInputHidden hididCardJustImg;

		private HtmlInputHidden hididCardAntiImg;

		private TextBox txtIDNumber;

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

		private HtmlInputHidden hidInvoiceType;

		protected override void OnInit(EventArgs e)
		{
			this.siteSettings = SettingsManager.GetMasterSettings();
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-SubmmitOrder.html";
			}
			base.OnInit(e);
		}

		private void CheckParamter()
		{
			if (HiContext.Current.UserId <= 0)
			{
				this.ShowMessage("请您先登录！", false, "/User/login?ReturnUrl=" + HttpContext.Current.Request.Url.ToString(), 5);
				return;
			}
			string text = "";
			this.from = this.Page.Request.QueryString["from"].ToNullString().ToLower();
			this.productSku = Globals.UrlDecode(this.Page.Request.QueryString["productSku"].ToNullString());
			this.buyAmount = this.Page.Request.QueryString["buyAmount"].ToInt(0);
			this.bundlingid = this.Page.Request.QueryString["Bundlingid"].ToInt(0);
			this.combinationId = this.Page.Request.QueryString["CombinationId"].ToInt(0);
			if (this.buyAmount <= 0)
			{
				this.buyAmount = 1;
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
			else if (this.from == "signbuy")
			{
				if (ProductPreSaleHelper.HasProductPreSaleInfo(this.productSku, 0))
				{
					base.GotoResourceNotFound();
					return;
				}
				this.isSignBuy = true;
			}
			else if (this.from == "fightgroup")
			{
				this.isFireGroup = true;
				this.buytype = "FightGroup";
			}
			else if (this.from == "combinationbuy")
			{
				this.isCombina = true;
				this.productSku = DataHelper.CleanSearchString(this.productSku);
			}
			else
			{
				if (this.from == "presale")
				{
					this.preSaleId = this.Page.Request.QueryString["PreSaleId"].ToInt(0);
					this.productSku = DataHelper.CleanSearchString(this.productSku);
					if (this.CheckPreSaleInfo())
					{
						this.isPreSale = true;
						goto IL_032c;
					}
					return;
				}
				HttpCookie httpCookie = HiContext.Current.Context.Request.Cookies["ckids"];
				if (httpCookie != null && !string.IsNullOrEmpty(httpCookie.Value))
				{
					this.productSku = Globals.UrlDecode(httpCookie.Value);
				}
				if (this.CheckProductSkuHasProductPreInfo())
				{
					string url = "/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("您所选商品中有预售商品，请重新选择！");
					this.Page.Response.Redirect(url);
					return;
				}
			}
			goto IL_032c;
			IL_03f9:
			if (this.shoppingCart != null && this.shoppingCart.GetQuantity(false) == 0)
			{
				this.buytype = "0";
			}
			if (this.shoppingCart == null)
			{
				string url2 = "/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("该件商品已经被管理员删除");
				if (!string.IsNullOrEmpty(this.from))
				{
					this.Page.Response.Redirect(url2);
				}
				else
				{
					this.Page.Response.Redirect("/ResourceNotFound.aspx");
				}
			}
			if (HiContext.Current.UserId == 0 && (this.isGroupBuy || this.isCountDown || this.isCombina))
			{
				HttpContext.Current.Response.Redirect("/User/login?ReturnUrl=" + Globals.UrlDecode(HttpContext.Current.Request.Url.ToString()));
			}
			else
			{
				if (this.isGroupBuy)
				{
					this.groupbuyInfo = TradeHelper.GetProductGroupBuyInfo(this.shoppingCart.LineItems[0].ProductId, this.buyAmount, out text);
					if (this.groupbuyInfo == null)
					{
						this.Page.Response.Redirect("/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode(text));
						return;
					}
				}
				if (this.isCountDown)
				{
					this.countdownInfo = TradeHelper.ProductExistsCountDown(this.shoppingCart.LineItems[0].ProductId, "", 0);
					if (this.countdownInfo == null)
					{
						this.Page.Response.Redirect("/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("该商品未进行抢购活动,或者活动已结束"));
					}
					else if (this.siteSettings.OpenMultStore && !StoreActivityHelper.JoinActivity(this.countdownInfo.CountDownId, 2, 0, this.countdownInfo.StoreType))
					{
						this.Page.Response.Redirect("/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("该门店未参与此抢购活动"));
					}
					else
					{
						this.countdownInfo = TradeHelper.CheckUserCountDown(this.shoppingCart.LineItems[0].ProductId, this.countdownInfo.CountDownId, this.shoppingCart.LineItems[0].SkuId, HiContext.Current.UserId, this.buyAmount, "", out text, 0);
						if (this.countdownInfo == null)
						{
							this.btnCreateOrder = ButtonManager.Create(this.FindControl("btnCreateOrder"));
							this.btnCreateOrder.Visible = false;
							this.ShowMessage(text, false, "/CountDownProducts", 5);
						}
						else if (TradeHelper.ExistCountDownOverbBought(this.buyAmount, this.countdownInfo.CountDownId, this.shoppingCart.LineItems[0].SkuId))
						{
							this.ShowMessage("抢购商品活动库存不足,无法购买", false, "", 1);
						}
					}
				}
			}
			return;
			IL_032c:
			if (!this.isPreSale && !string.IsNullOrEmpty(this.productSku) && !ProductHelper.ProductsIsAllOnSales(this.productSku, 0))
			{
				this.ShowMessage("有商品不存在或者已经下架", false, "/ShoppingCart", 2);
				return;
			}
			if (this.isCombina)
			{
				if (this.combinationId <= 0)
				{
					base.GotoResourceNotFound();
				}
				if (this.CheckCombinaInfo())
				{
					this.shoppingCart = ShoppingCartProcessor.GetCombinationShoppingCart(this.combinationId, this.productSku, this.buyAmount, true);
					goto IL_03f9;
				}
				return;
			}
			this.shoppingCart = ShoppingCartProcessor.GetShoppingCart(this.from, this.productSku, this.buyAmount, 0, false, 0, 0);
			goto IL_03f9;
		}

		private bool CheckCombinaInfo()
		{
			List<ViewCombinationBuySkuInfo> combinaSkusInfoByCombinaId = CombinationBuyHelper.GetCombinaSkusInfoByCombinaId(this.combinationId);
			if (combinaSkusInfoByCombinaId == null || combinaSkusInfoByCombinaId.Count == 0)
			{
				base.GotoResourceNotFound();
				return false;
			}
			DateTime startDate = combinaSkusInfoByCombinaId[0].StartDate;
			DateTime endDate = combinaSkusInfoByCombinaId[0].EndDate;
			DateTime date = DateTime.Now.Date;
			if (startDate > date || endDate < date)
			{
				this.ShowMessage("未到活动时间或者活动已结束", false, "", 1);
				return false;
			}
			int num = Convert.ToInt32(combinaSkusInfoByCombinaId[0].MainProductId);
			bool flag = false;
			string[] array = this.productSku.Split(',');
			if (array.Length <= 1)
			{
				base.GotoResourceNotFound();
			}
			string[] array2 = array;
			foreach (string item in array2)
			{
				ViewCombinationBuySkuInfo viewCombinationBuySkuInfo = combinaSkusInfoByCombinaId.FirstOrDefault((ViewCombinationBuySkuInfo c) => c.SkuId == item);
				if (viewCombinationBuySkuInfo == null)
				{
					this.ShowMessage("订单中有商品不在组合购产品中,请联系管理员！", false, "", 1);
					return false;
				}
				if (viewCombinationBuySkuInfo.ProductId == viewCombinationBuySkuInfo.MainProductId)
				{
					flag = true;
				}
			}
			if (!flag)
			{
				base.GotoResourceNotFound();
				return false;
			}
			return true;
		}

		private bool CheckPreSaleInfo()
		{
			if (this.preSaleId > 0)
			{
				ProductPreSaleInfo productPreSaleInfo = ProductPreSaleHelper.GetProductPreSaleInfo(this.preSaleId);
				if (productPreSaleInfo == null)
				{
					this.ShowMessage("活动不存在！", false, "", 1);
					return false;
				}
				if (this.productSku.Split(',').Length > 1)
				{
					base.GotoResourceNotFound();
					return false;
				}
				if (!ProductPreSaleHelper.HasProductPreSaleInfo(this.productSku, this.preSaleId))
				{
					base.GotoResourceNotFound();
					return false;
				}
				return true;
			}
			base.GotoResourceNotFound();
			return false;
		}

		protected override void OnLoad(EventArgs e)
		{
			this.Page.Response.Expires = -1;
			base.OnLoad(e);
		}

		protected override void AttachChildControls()
		{
			this.dropRegions = (RegionSelector)this.FindControl("dropRegions");
			this.txtShipTo = (TextBox)this.FindControl("txtShipTo");
			this.txtAddress = (TextBox)this.FindControl("txtAddress");
			this.txtZipcode = (TextBox)this.FindControl("txtZipcode");
			this.txtCellPhone = (TextBox)this.FindControl("txtCellPhone");
			this.txtTelPhone = (TextBox)this.FindControl("txtTelPhone");
			this.txtInvoiceTitle = (TextBox)this.FindControl("txtInvoiceTitle");
			this.txtInvoiceTaxpayerNumber = (TextBox)this.FindControl("txtInvoiceTaxpayerNumber");
			this.radInvoiceType = (RadioButton)this.FindControl("radInvoiceType");
			this.radInvoiceType2 = (RadioButton)this.FindControl("radInvoiceType2");
			this.drpShipToDate = (HtmlSelect)this.FindControl("drpShipToDate");
			this.litTaxRate = (Label)this.FindControl("litTaxRate");
			this.hidPointRate = (HtmlInputHidden)this.FindControl("hidPointRate");
			this.inputPaymentModeId = (HtmlInputHidden)this.FindControl("inputPaymentModeId");
			this.inputShippingModeId = (HtmlInputHidden)this.FindControl("inputShippingModeId");
			this.hdbuytype = (HtmlInputHidden)this.FindControl("hdbuytype");
			this.hidStoreId = (HtmlInputHidden)this.FindControl("hidStoreId");
			this.hidStoreCount = (HtmlInputHidden)this.FindControl("hidStoreCount");
			this.hidShipperId = (HtmlInputHidden)this.FindControl("hidShipperId");
			this.hidIsAnonymous = (HtmlInputHidden)this.FindControl("hidIsAnonymous");
			this.pannel_useraddress = (Panel)this.FindControl("pannel_useraddress");
			this.pannel_noaddress = (Panel)this.FindControl("pannel_noaddress");
			this.lblShippModePrice = (FormatedMoneyLabel)this.FindControl("lblShippModePrice");
			this.chkTax = (HtmlInputCheckBox)this.FindControl("chkTax");
			this.cartProductList = (Common_SubmmintOrder_ProductList)this.FindControl("Common_SubmmintOrder_ProductList");
			this.cartProductList.ItemDataBound += this.cartProductList_ItemDataBound;
			this.cartGiftList = (Common_SubmmintOrder_GiftList)this.FindControl("Common_SubmmintOrder_GiftList");
			this.common_pointgiftlist = (Common_PointGiftList)this.FindControl("Common_PointGiftList");
			this.litProductBundling = (Literal)this.FindControl("litProductBundling");
			this.litAllWeight = (Label)this.FindControl("litAllWeight");
			this.litPoint = (Label)this.FindControl("litPoint");
			this.hlkSentTimesPoint = (HyperLink)this.FindControl("hlkSentTimesPoint");
			this.lblOrderTotal = (FormatedMoneyLabel)this.FindControl("lblOrderTotal");
			this.txtMessage = (TextBox)this.FindControl("txtMessage");
			this.hlkFeeFreight = (HyperLink)this.FindControl("hlkFeeFreight");
			this.hlkReducedPromotion = (HyperLink)this.FindControl("hlkReducedPromotion");
			this.lblTotalPrice = (FormatedMoneyLabel)this.FindControl("lblTotalPrice");
			this.htmlCouponCode = (HtmlInputHidden)this.FindControl("htmlCouponCode");
			this.CmbCoupCode = (HtmlSelect)this.FindControl("CmbCoupCode");
			this.divProductList = (HtmlGenericControl)this.FindControl("divProductList");
			this.spanTaxRate = (HtmlGenericControl)this.FindControl("spanTaxRate");
			this.litCouponAmout = (FormatedMoneyLabel)this.FindControl("litCouponAmout");
			this.hidUseBalance = (TextBox)this.FindControl("hidUseBalance");
			this.hidTradePassword = (TextBox)this.FindControl("hidTradePassword");
			this.btnCreateOrder = ButtonManager.Create(this.FindControl("btnCreateOrder"));
			this.hidCanUsePoint = (HtmlInputHidden)this.FindControl("hidCanUsePoint");
			this.lblMaxPoints = (Label)this.FindControl("lblMaxPoints");
			this.txtUsePoints = (TextBox)this.FindControl("txtUsePoints");
			this.lblDeductibleMoney = (FormatedMoneyLabel)this.FindControl("lblDeductibleMoney");
			this.litPointAmount = (FormatedMoneyLabel)this.FindControl("litPointAmount");
			this.hidShoppingDeduction = (HtmlInputHidden)this.FindControl("hidShoppingDeduction");
			this.hidShoppingDeductionRatio = (HtmlInputHidden)this.FindControl("hidShoppingDeductionRatio");
			this.hidCanPointUseWithCoupon = (HtmlInputHidden)this.FindControl("hidCanPointUseWithCoupon");
			this.hidMyPoints = (HtmlInputHidden)this.FindControl("hidMyPoints");
			this.hidPaymentId_Podrequest = (HtmlInputHidden)this.FindControl("hidPaymentId_Podrequest");
			this.hidGetgoodsOnStores = (HtmlInputHidden)this.FindControl("hidGetgoodsOnStores");
			this.hidIsCloseStoreButGetGoods = (HtmlInputHidden)this.FindControl("hidIsCloseStoreButGetGoods");
			this.hidGetGoodsRemark = (HtmlInputHidden)this.FindControl("hidGetGoodsRemark");
			this.hidTotalPrice = (HtmlInputHidden)this.FindControl("hidTotalPrice");
			this.chkIsUsePoints = (HtmlInputCheckBox)this.FindControl("chkIsUsePoints");
			this.divcopue = (HtmlGenericControl)this.FindControl("divcopue");
			this.hidPaymentId_Offline = (HtmlInputHidden)this.FindControl("hidPaymentId_Offline");
			this.hidIsPreSale = (HtmlInputHidden)this.FindControl("hidIsPreSale");
			this.lblDeposit = (FormatedMoneyLabel)this.FindControl("lblDeposit");
			this.lblFinalPayment = (FormatedMoneyLabel)this.FindControl("lblFinalPayment");
			this.lblPreSaleOrderTotal = (FormatedMoneyLabel)this.FindControl("lblPreSaleOrderTotal");
			this.lblDepositPay = (FormatedMoneyLabel)this.FindControl("lblDepositPay");
			this.hidOnlinePayCount = (HtmlInputHidden)this.FindControl("hidOnlinePayCount");
			this.lblGiftFeright = (Label)this.FindControl("lblGiftFeright");
			this.hidHasSupplierProduct = (HtmlInputHidden)this.FindControl("hidHasSupplierProduct");
			this.hidTimePoint = (HtmlInputHidden)this.FindControl("hidTimePoint");
			this.hidIsGiftOrder = (HtmlInputHidden)this.FindControl("hidIsGiftOrder");
			this.hidIsOpenCertification = (HtmlInputHidden)this.FindControl("hidIsOpenCertification");
			this.hidCertificationModel = (HtmlInputHidden)this.FindControl("hidCertificationModel");
			this.hidIsGetIDInfo = (HtmlInputHidden)this.FindControl("hidIsGetIDInfo");
			this.txtIDNumber = (TextBox)this.FindControl("txtIDNumber");
			this.hididCardJustImg = (HtmlInputHidden)this.FindControl("hididCardJustImg");
			this.hididCardAntiImg = (HtmlInputHidden)this.FindControl("hididCardAntiImg");
			this.hidOpenBalancePay = (HtmlInputHidden)this.FindControl("hidOpenBalancePay");
			this.hidBalanceAmount = (HtmlInputHidden)this.FindControl("hidBalanceAmount");
			this.lblBalance = (Label)this.FindControl("lblBalance");
			this.hidHasTradePassword = (HtmlInputHidden)this.FindControl("hidHasTradePassword");
			this.txtBuilderNumber = (TextBox)this.FindControl("txtBuilderNumber");
			this.hidInvoiceJson = (HtmlInputHidden)this.FindControl("hidInvoiceJson");
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
			HtmlInputHidden htmlInputHidden = this.hidEnableETax;
			bool flag = this.siteSettings.EnableE_Invoice;
			htmlInputHidden.Value = flag.ToString().ToLower();
			HtmlInputHidden htmlInputHidden2 = this.hidEnableTax;
			flag = this.siteSettings.EnableTax;
			htmlInputHidden2.Value = flag.ToString().ToLower();
			HtmlInputHidden htmlInputHidden3 = this.hidEnableVATTax;
			flag = this.siteSettings.EnableVATInvoice;
			htmlInputHidden3.Value = flag.ToString().ToLower();
			this.hidVATTaxRate.Value = this.siteSettings.VATTaxRate.ToNullString();
			this.hidTaxRate.Value = this.siteSettings.TaxRate.ToNullString();
			this.hidVATInvoiceDays.Value = this.siteSettings.VATInvoiceDays.ToNullString();
			this.litAfterSaleDays.SetWhenIsNotNull(this.siteSettings.EndOrderDays.ToNullString());
			this.litInvoiceSendDays.SetWhenIsNotNull((this.siteSettings.EndOrderDays + this.siteSettings.VATInvoiceDays).ToNullString());
			this.hidOpenBalancePay.Value = this.siteSettings.OpenBalancePay.ToNullString().ToLower();
			if (this.hidOnlinePayCount != null)
			{
				this.hidOnlinePayCount.Value = TradeHelper.GetPaymentModeCount(PayApplicationType.payOnPC).ToNullString();
			}
			if (this.hidPointRate != null)
			{
				this.hidPointRate.Value = this.siteSettings.PointsRate.F2ToString("f2");
			}
			Hidistro.Entities.Members.MemberInfo user = HiContext.Current.User;
			this.hidHasTradePassword.Value = (string.IsNullOrWhiteSpace(user.TradePassword) ? "0" : "1");
			this.hidBalanceAmount.Value = (user.Balance - user.RequestBalance).F2ToString("f2");
			this.lblBalance.Text = (user.Balance - user.RequestBalance).F2ToString("f2");
			this.CheckParamter();
			bool flag2 = false;
			string str = "";
			if (this.shoppingCart == null && this.from == "countdown")
			{
				this.ShowMessage("抢购活动已经结束，请选择其他抢购活动。", false, "", 1);
			}
			else
			{
				if ((!this.siteSettings.EnableTax && !this.siteSettings.EnableVATInvoice && !this.siteSettings.EnableE_Invoice) || (this.shoppingCart != null && this.shoppingCart.LineItems.Count() == 0))
				{
					this.divcopue.Visible = false;
				}
				else
				{
					this.divcopue.Visible = true;
				}
				if (!this.Page.IsPostBack)
				{
					if (!TradeHelper.CheckShoppingStock(this.shoppingCart, out str, 0))
					{
						this.ShowMessage("订单中有商品(" + str + ")库存不足", false, "", 1);
						flag2 = true;
					}
					if (!flag2)
					{
						this.btnCreateOrder.Click += this.btnCreateOrder_Click;
					}
					else
					{
						this.btnCreateOrder.Attributes.Add("onclick", "AlertStock()");
						this.btnCreateOrder.CausesValidation = false;
						if (!this.Page.ClientScript.IsClientScriptBlockRegistered("AlertStockScript"))
						{
							this.Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "AlertStockScript", "function AlertStock(){alert('订单中有商品(" + str + ")库存不足,请返回购物车修改库存.');document.location.href=\"ShoppingCart.aspx\";}", true);
						}
						this.btnCreateOrder.Click += this.backShoppingCart_Click;
					}
				}
				else
				{
					this.btnCreateOrder.Click += this.btnCreateOrder_Click;
				}
				if (!this.Page.IsPostBack)
				{
					this.hidGetgoodsOnStores.Value = "false";
					this.hidIsCloseStoreButGetGoods.Value = "false";
					this.BindUserAddress();
					this.BindLastInvoiceInfo();
					this.hidPaymentId_Podrequest.Value = "0";
					int num;
					if (this.from != "countdown" && this.from != "groupbuy")
					{
						if (this.from != "presale")
						{
							PaymentModeInfo paymentMode = TradeHelper.GetPaymentMode(ShoppingProcessor.GetPaymentGateway(EnumPaymentType.CashOnDelivery));
							if (paymentMode != null)
							{
								HtmlInputHidden htmlInputHidden4 = this.hidPaymentId_Podrequest;
								num = paymentMode.ModeId;
								htmlInputHidden4.Value = num.ToString();
							}
						}
						PaymentModeInfo paymentMode2 = TradeHelper.GetPaymentMode(ShoppingProcessor.GetPaymentGateway(EnumPaymentType.OfflinePay));
						if (paymentMode2 != null)
						{
							HtmlInputHidden htmlInputHidden5 = this.hidPaymentId_Offline;
							num = paymentMode2.ModeId;
							htmlInputHidden5.Value = num.ToString();
						}
					}
					if (this.shoppingCart != null && ((this.shoppingCart.LineItems != null && this.shoppingCart.LineItems.Count > 0) || (this.shoppingCart.LineGifts != null && this.shoppingCart.LineGifts.Count > 0)))
					{
						bool flag3 = true;
						this.litTaxRate.Text = this.siteSettings.TaxRate.ToString(CultureInfo.InvariantCulture);
						this.spanTaxRate.Visible = (this.siteSettings.TaxRate > decimal.Zero || this.siteSettings.VATTaxRate > decimal.Zero);
						this.BindShoppingCartInfo(this.shoppingCart);
						if (this.isGroupBuy || this.isCountDown || this.isFireGroup || this.shoppingCart.LineItems.Count == 0)
						{
							flag3 = false;
						}
						if (HiContext.Current.User != null && HiContext.Current.UserId != 0 && this.shoppingCart.GetTotal(false) > decimal.Zero)
						{
							this.CmbCoupCode.DataTextField = "CouponName";
							this.CmbCoupCode.DataValueField = "ClaimCode";
							DataTable dataTable = null;
							dataTable = ShoppingProcessor.GetCoupon(this.shoppingCart.GetTotal(false), this.shoppingCart.LineItems, this.isGroupBuy, this.isCountDown, this.isFireGroup);
							this.CmbCoupCode.DataSource = dataTable;
							this.CmbCoupCode.DataBind();
						}
						ListItem item = new ListItem("不使用优惠券", "0");
						this.CmbCoupCode.Items.Insert(0, item);
						if (this.CmbCoupCode.Items.Count > 1)
						{
							this.CmbCoupCode.SelectedIndex = 1;
							CouponItemInfo userCouponInfo = ShoppingProcessor.GetUserCouponInfo(this.shoppingCart.GetTotal(false), this.CmbCoupCode.Items[1].Value);
							if (userCouponInfo != null)
							{
								this.litCouponAmout.Text = (userCouponInfo.Price.HasValue ? userCouponInfo.Price.Value.F2ToString("f2") : "0.00");
								this.htmlCouponCode.Value = userCouponInfo.ClaimCode;
							}
						}
						this.hdbuytype.Value = this.buytype;
						if (flag3)
						{
							this.hidCanPointUseWithCoupon.Value = "False";
							this.hidCanUsePoint.Value = "false";
							if (user != null && user.UserId > 0 && this.siteSettings.ShoppingDeduction > 0 && user.Points > 0)
							{
								int shoppingDeductionRatio = this.siteSettings.ShoppingDeductionRatio;
								decimal num2 = this.shoppingCart.GetTotal(false);
								decimal num3 = default(decimal);
								if (decimal.TryParse(this.litCouponAmout.Text, out num3) && num3 > decimal.Zero)
								{
									num2 = ((num2 - num3 > decimal.Zero) ? (num2 - num3) : decimal.Zero);
								}
								decimal num4 = (decimal)shoppingDeductionRatio * num2 * (decimal)this.siteSettings.ShoppingDeduction / 100m;
								int num5 = ((decimal)user.Points > num4) ? ((int)num4) : user.Points;
								decimal d = ((decimal)num5 / (decimal)this.siteSettings.ShoppingDeduction).F2ToString("f2").ToDecimal(0);
								if (d > decimal.Zero || num5 > 0)
								{
									this.lblMaxPoints.Text = num5.ToString();
									this.hidCanUsePoint.Value = "true";
								}
								HtmlInputHidden htmlInputHidden6 = this.hidShoppingDeduction;
								num = this.siteSettings.ShoppingDeduction;
								htmlInputHidden6.Value = num.ToString();
								this.hidShoppingDeductionRatio.Value = shoppingDeductionRatio.ToString();
								HtmlInputHidden htmlInputHidden7 = this.hidCanPointUseWithCoupon;
								flag = this.siteSettings.CanPointUseWithCoupon;
								htmlInputHidden7.Value = flag.ToString();
								HtmlInputHidden htmlInputHidden8 = this.hidMyPoints;
								num = user.Points;
								htmlInputHidden8.Value = num.ToString();
							}
						}
						this.pannel_useraddress.Visible = (MemberProcessor.GetShippingAddressCount(HiContext.Current.UserId) > 0);
						this.pannel_noaddress.Visible = !this.pannel_useraddress.Visible;
					}
					else
					{
						this.ShowMessage("购物车中已经没有任何商品", false, "", 1);
					}
				}
				else if (this.shoppingCart != null && ((this.shoppingCart.LineItems != null && this.shoppingCart.LineItems.Count > 0) || (this.shoppingCart.LineGifts != null && this.shoppingCart.LineGifts.Count > 0)))
				{
					this.BindShoppingCartInfo(this.shoppingCart);
				}
				else
				{
					this.ShowMessage("购物车中已经没有任何商品", false, "", 1);
				}
			}
		}

		private void cartProductList_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (HiContext.Current.User.UserId != 0 && !this.isGroupBuy && !this.isCountDown && (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem))
			{
				Repeater repeater = e.Item.FindControl("rptPromotionGifts") as Repeater;
				ShoppingCartItemInfo shoppingCartItemInfo = e.Item.DataItem as ShoppingCartItemInfo;
				PromotionInfo productPromotionInfo = ProductBrowser.GetProductPromotionInfo(shoppingCartItemInfo.ProductId);
				if (productPromotionInfo != null && productPromotionInfo.PromoteType == PromoteType.SentGift)
				{
					IList<GiftInfo> list = (IList<GiftInfo>)(repeater.DataSource = ProductBrowser.GetGiftDetailsByGiftIds(productPromotionInfo.GiftIds));
					repeater.DataBind();
				}
			}
		}

		public void backShoppingCart_Click(object sender, EventArgs e)
		{
			HttpContext.Current.Response.Redirect("ShoppingCart.aspx");
		}

		public void btnCreateOrder_Click(object sender, EventArgs e)
		{
			if (HiContext.Current.UserId <= 0)
			{
				this.ShowMessage("请您先登录！", false, "/User/login?ReturnUrl=" + HttpContext.Current.Request.Url.ToString(), 5);
			}
			else if (this.ValidateCreateOrder())
			{
				if (this.shoppingCart == null || ((this.shoppingCart.LineItems == null || this.shoppingCart.LineItems.Count == 0) && (this.shoppingCart.LineGifts == null || this.shoppingCart.LineGifts.Count == 0)))
				{
					this.ShowMessage("购物车中已经没有任何商品", false, "", 1);
				}
				else if (this.shoppingCart != null && ((this.shoppingCart.LineItems != null && (from a in this.shoppingCart.LineItems
				where a.Quantity <= 0
				select a).Count() > 0) || (this.shoppingCart.LineGifts != null && (from a in this.shoppingCart.LineGifts
				where a.Quantity <= 0
				select a).Count() > 0)))
				{
					this.ShowMessage("购买数量不合法", false, "", 1);
				}
				else
				{
					OrderInfo orderInfo = this.GetOrderInfo(this.shoppingCart);
					if (orderInfo.BalanceAmount > orderInfo.GetTotal(false))
					{
						orderInfo.BalanceAmount = orderInfo.GetTotal(false);
					}
					if (this.isPreSale && orderInfo.BalanceAmount > decimal.Zero)
					{
						if (orderInfo.BalanceAmount > orderInfo.Deposit)
						{
							orderInfo.BalanceAmount = orderInfo.Deposit;
						}
						if (orderInfo.BalanceAmount == orderInfo.Deposit)
						{
							orderInfo.DepositGatewayOrderId = "";
						}
					}
					if (orderInfo.GetTotal(true) <= decimal.Zero && orderInfo.BalanceAmount > decimal.Zero)
					{
						orderInfo.PaymentTypeId = -99;
						orderInfo.PaymentType = "余额支付";
						orderInfo.Gateway = "hishop.plugins.payment.advancerequest";
					}
					if (orderInfo.RegionId <= 0 || string.IsNullOrEmpty(orderInfo.ShippingRegion))
					{
						this.ShowMessage("错误的收货地址,请重新选择或者修改收货地址", false, "", 1);
					}
					else
					{
						string orderId = orderInfo.OrderId;
						var list = (from i in this.shoppingCart.LineItems
						select new
						{
							i.SupplierId,
							i.SupplierName
						}).Distinct().ToList();
						if ((from i in list
						where i.SupplierId == 0
						select i).Count() <= 0 && this.shoppingCart.LineGifts.Count() > 0)
						{
							list.Add(new
							{
								SupplierId = 0,
								SupplierName = "平台"
							});
						}
						list = (from i in list
						orderby i.SupplierId
						select i).ToList();
						List<OrderInfo> list2 = new List<OrderInfo>();
						if (this.siteSettings.OpenSupplier)
						{
							if (list.Count() > 1)
							{
								Hidistro.Entities.Members.MemberInfo user = Users.GetUser(HiContext.Current.UserId);
								string orderId2 = orderInfo.OrderId;
								orderInfo.ParentOrderId = "-1";
								orderInfo.OrderId = "P" + orderId2;
								orderInfo.StoreId = 0;
								decimal num = orderInfo.ReducedPromotionAmount;
								decimal num2 = orderInfo.CouponValue;
								int num3 = orderInfo.DeductionPoints.HasValue ? orderInfo.DeductionPoints.Value : 0;
								decimal num4 = (!orderInfo.DeductionMoney.HasValue) ? decimal.Zero : orderInfo.DeductionMoney.Value;
								int num5 = orderInfo.Points;
								decimal num6 = default(decimal);
								decimal num7 = orderInfo.BalanceAmount;
								string arg = orderId2;
								int num8 = 0;
								for (int j = 0; j < list.Count; j++)
								{
									var anon = list[j];
									OrderInfo orderInfo2 = new OrderInfo();
									ShoppingProcessor.CreateDetailOrderInfo(orderInfo, orderInfo2);
									orderInfo2.OrderId = arg + num8;
									orderInfo2.ParentOrderId = orderInfo.OrderId;
									orderInfo2.SupplierId = anon.SupplierId;
									orderInfo2.ShipperName = anon.SupplierName;
									ShoppingProcessor.BindDetailOrderItemsAndGifts(orderInfo2, this.shoppingCart, anon.SupplierId);
									if (anon.SupplierId > 0)
									{
										orderInfo2.Tax = decimal.Zero;
										orderInfo2.InvoiceTitle = "";
										orderInfo2.InvoiceTaxpayerNumber = "";
									}
									if (orderInfo2.LineItems.Count <= 0)
									{
										OrderInfo orderInfo3 = orderInfo2;
										OrderInfo orderInfo4 = orderInfo2;
										OrderInfo orderInfo5 = orderInfo2;
										OrderInfo orderInfo6 = orderInfo2;
										OrderInfo orderInfo7 = orderInfo2;
										int num10 = orderInfo7.PreSaleId = 0;
										int num12 = orderInfo6.GroupBuyId = num10;
										int num14 = orderInfo5.FightGroupId = num12;
										int num17 = orderInfo3.CountDownBuyId = (orderInfo4.FightGroupActivityId = num14);
										orderInfo2.Deposit = decimal.Zero;
										orderInfo2.FinalPayment = decimal.Zero;
									}
									decimal d = (orderInfo.GetAmount(false) == decimal.Zero) ? decimal.Zero : (orderInfo2.GetAmount(false) / orderInfo.GetAmount(false));
									if (orderInfo.IsReduced)
									{
										if (j == list.Count - 1)
										{
											orderInfo2.ReducedPromotionAmount = ((num < decimal.Zero) ? decimal.Zero : num);
										}
										else
										{
											decimal d2 = orderInfo2.ReducedPromotionAmount = (d * orderInfo.ReducedPromotionAmount).F2ToString("f2").ToDecimal(0);
											num -= d2;
										}
									}
									if (orderInfo.CouponValue > decimal.Zero)
									{
										if (j == list.Count - 1)
										{
											orderInfo2.CouponValue = ((num2 < decimal.Zero) ? decimal.Zero : num2);
										}
										else
										{
											decimal num19 = default(decimal);
											num19 = (orderInfo2.CouponValue = (d * orderInfo.CouponValue).F2ToString("f2").ToDecimal(0));
											num2 -= num19;
										}
									}
									orderInfo2.Freight = ShoppingProcessor.CalcSupplierFreight(anon.SupplierId, orderInfo.RegionId, this.shoppingCart);
									if (!orderInfo.IsFreightFree)
									{
										orderInfo2.AdjustedFreight = orderInfo2.Freight;
									}
									decimal? deductionMoney = orderInfo.DeductionMoney;
									if (deductionMoney.GetValueOrDefault() > default(decimal) && deductionMoney.HasValue)
									{
										if (j == list.Count - 1)
										{
											orderInfo2.DeductionPoints = ((num3 >= 0) ? num3 : 0);
											orderInfo2.DeductionMoney = ((num4 < decimal.Zero) ? decimal.Zero : num4);
										}
										else
										{
											decimal d3 = orderInfo2.GetAmount(false) - orderInfo2.ReducedPromotionAmount - orderInfo2.CouponValue;
											decimal d4 = orderInfo.GetAmount(false) - orderInfo.ReducedPromotionAmount - orderInfo.CouponValue;
											decimal num21 = (d3 == decimal.Zero) ? decimal.Zero : (d3 / d4);
											decimal value = num21;
											decimal? d5 = (decimal?)orderInfo.DeductionPoints;
											int num22 = (int)((decimal?)value * d5).Value;
											if (user.Points < num22)
											{
												num22 = user.Points;
											}
											decimal num23 = ((decimal)num22 / (decimal)this.siteSettings.ShoppingDeduction).F2ToString("f2").ToDecimal(0);
											if (orderInfo.GetTotal(false) == decimal.Zero)
											{
												decimal total = orderInfo2.GetTotal(false);
												if (total > decimal.Zero)
												{
													num23 -= total;
													num22 = (int)(num23 * (decimal)this.siteSettings.ShoppingDeduction);
												}
											}
											if (num23 < decimal.Zero)
											{
												num23 = default(decimal);
											}
											if (num22 < 0)
											{
												num22 = 0;
											}
											orderInfo2.DeductionPoints = num22;
											orderInfo2.DeductionMoney = num23;
											num3 -= num22;
											num4 -= num23;
										}
									}
									if (orderInfo.Points > 0)
									{
										if (j == list.Count - 1)
										{
											orderInfo2.Points = num5;
										}
										else
										{
											int num24 = orderInfo2.Points = orderInfo2.GetPoint(HiContext.Current.SiteSettings.PointsRate);
											num5 -= num24;
										}
									}
									if (orderInfo.BalanceAmount > decimal.Zero)
									{
										if (j == list.Count - 1)
										{
											orderInfo2.BalanceAmount = num7;
										}
										else
										{
											decimal d6 = orderInfo2.BalanceAmount = (d * orderInfo.BalanceAmount).F2ToString("f2").ToDecimal(0);
											num7 -= d6;
										}
									}
									orderInfo2.ExchangePoints = orderInfo2.GetTotalNeedPoint();
									if (anon.SupplierId == 0)
									{
										orderId = orderInfo2.OrderId;
									}
									list2.Add(orderInfo2);
									num8++;
								}
							}
							else
							{
								var anon2 = list[0];
								orderInfo.SupplierId = anon2.SupplierId;
								orderInfo.ShipperName = anon2.SupplierName;
								orderInfo.Freight = ShoppingProcessor.CalcSupplierFreight(anon2.SupplierId, orderInfo.RegionId, this.shoppingCart);
							}
						}
						list2.Add(orderInfo);
						if (this.shoppingCart.GetQuantity(false) > 1)
						{
							this.isSignBuy = false;
						}
						if (orderInfo == null)
						{
							this.ShowMessage("购物车中已经没有任何商品", false, "", 1);
						}
						else if (orderInfo.GetTotal(false) < decimal.Zero)
						{
							this.ShowMessage("订单金额不能为负", false, "", 1);
						}
						else
						{
							string str = "";
							if (!TradeHelper.CheckShoppingStock(this.shoppingCart, out str, 0))
							{
								this.ShowMessage("订单中有商品(" + str + ")库存不足", false, "", 1);
							}
							else
							{
								if (HiContext.Current.UserId != 0)
								{
									int totalNeedPoint = this.shoppingCart.GetTotalNeedPoint();
									int points = HiContext.Current.User.Points;
									if (points >= 0 && totalNeedPoint > points)
									{
										this.ShowMessage("您当前的积分不够兑换所需礼品！", false, "", 1);
										return;
									}
								}
								try
								{
									if (ShoppingProcessor.CreatOrder(list2))
									{
										if (orderInfo.BalanceAmount > decimal.Zero)
										{
											TradeHelper.BalanceDeduct(orderInfo);
										}
										TransactionAnalysisHelper.AnalysisOrderTranData(orderInfo);
										if (orderInfo.StoreId > 0 && orderInfo.PaymentTypeId == -3 && !this.isPreSale)
										{
											VShopHelper.AppPsuhRecordForStore(orderInfo.StoreId, orderInfo.OrderId, "", EnumPushStoreAction.TakeOnStoreOrderWaitConfirm);
										}
										Messenger.OrderCreated(orderInfo, HiContext.Current.User);
										if (orderInfo.Gateway == "hishop.plugins.payment.podrequest")
										{
											StoresInfo store = null;
											if (orderInfo.StoreId > 0)
											{
												store = DepotHelper.GetStoreById(orderInfo.StoreId);
											}
											ShippersInfo defaultOrFirstShipper = SalesHelper.GetDefaultOrFirstShipper(0);
											Messenger.OrderPaymentToShipper(defaultOrFirstShipper, store, null, orderInfo, orderInfo.GetTotal(false));
										}
										orderInfo.OnCreated();
										if (this.shoppingCart.GetTotalNeedPoint() > 0)
										{
											ShoppingProcessor.CutNeedPoint(this.shoppingCart.GetTotalNeedPoint(), orderId, PointTradeType.Change, HiContext.Current.UserId);
										}
										if (HiContext.Current.UserId > 0 && MemberProcessor.GetShippingAddressCount(HiContext.Current.UserId) == 0)
										{
											ShippingAddressInfo shippingAddressInfo = new ShippingAddressInfo();
											shippingAddressInfo.UserId = HiContext.Current.UserId;
											shippingAddressInfo.ShipTo = Globals.HtmlEncode(this.txtShipTo.Text);
											shippingAddressInfo.RegionId = this.dropRegions.GetSelectedRegionId().Value;
											shippingAddressInfo.Address = Globals.HtmlEncode(this.txtAddress.Text);
											shippingAddressInfo.Zipcode = "";
											shippingAddressInfo.BuildingNumber = Globals.StripAllTags(this.txtBuilderNumber.Text);
											shippingAddressInfo.CellPhone = this.txtCellPhone.Text;
											shippingAddressInfo.TelPhone = this.txtTelPhone.Text;
											shippingAddressInfo.FullRegionPath = RegionHelper.GetFullPath(shippingAddressInfo.RegionId, true);
											if (this.siteSettings.IsOpenCertification && orderInfo.IsincludeCrossBorderGoods)
											{
												shippingAddressInfo.IDNumber = orderInfo.IDNumber;
												if (this.siteSettings.CertificationModel == 2)
												{
													shippingAddressInfo.IDImage1 = orderInfo.IDImage1;
													shippingAddressInfo.IDImage2 = orderInfo.IDImage2;
												}
											}
											int num26 = MemberProcessor.AddShippingAddress(shippingAddressInfo);
											if (num26 > 0)
											{
												OrderInfo orderInfo8 = TradeHelper.GetOrderInfo(orderInfo.OrderId);
												if (orderInfo8 != null)
												{
													orderInfo8.ShippingId = num26;
													TradeHelper.UpdateOrderInfo(orderInfo8);
												}
											}
										}
										else if (this.siteSettings.IsOpenCertification && orderInfo.IsincludeCrossBorderGoods)
										{
											ShippingAddressInfo shippingAddress = MemberProcessor.GetShippingAddress(this.hidShipperId.Value.ToInt(0));
											if (shippingAddress != null)
											{
												shippingAddress.IDNumber = orderInfo.IDNumber;
												if (this.siteSettings.CertificationModel == 2)
												{
													shippingAddress.IDImage1 = orderInfo.IDImage1;
													shippingAddress.IDImage2 = orderInfo.IDImage2;
												}
												MemberProcessor.UpdateShippingAddress(shippingAddress);
											}
										}
										if (this.from != "signbuy" && this.from != "groupbuy" && this.from != "combinationbuy" && this.from != "presale")
										{
											foreach (ShoppingCartItemInfo lineItem in this.shoppingCart.LineItems)
											{
												ShoppingCartProcessor.RemoveLineItem(lineItem.SkuId, 0);
											}
											foreach (ShoppingCartGiftInfo lineGift in this.shoppingCart.LineGifts)
											{
												ShoppingCartProcessor.RemoveGiftItem(lineGift.GiftId, (PromoteType)lineGift.PromoType);
											}
										}
										if (orderInfo.GetTotal(true) == decimal.Zero || (orderInfo.PreSaleId > 0 && orderInfo.BalanceAmount == orderInfo.Deposit))
										{
											Task.Factory.StartNew(delegate
											{
												try
												{
													int num27 = 0;
													int num28 = 0;
													int num29 = 0;
													if (orderInfo.GroupBuyId > 0)
													{
														GroupBuyInfo groupBuy = TradeHelper.GetGroupBuy(orderInfo.GroupBuyId);
														if (groupBuy != null && groupBuy.Status == GroupBuyStatus.UnderWay)
														{
															num28 = TradeHelper.GetOrderCount(orderInfo.GroupBuyId);
															num29 = orderInfo.GetGroupBuyOerderNumber();
															num27 = groupBuy.MaxCount;
														}
													}
													if (orderInfo.CheckAction(OrderActions.BUYER_PAY) && TradeHelper.UpdateOrderStatus(orderInfo))
													{
														TradeHelper.UserPayOrder(orderInfo, false, true);
														if (orderInfo.FightGroupId > 0)
														{
															VShopHelper.SetFightGroupSuccess(orderInfo.FightGroupId);
														}
														if (orderInfo.GroupBuyId > 0 && num27 == num28 + num29)
														{
															TradeHelper.SetGroupBuyEndUntreated(orderInfo.GroupBuyId);
														}
														if (orderInfo.ParentOrderId == "-1")
														{
															OrderQuery orderQuery = new OrderQuery();
															orderQuery.ParentOrderId = orderInfo.OrderId;
															IList<OrderInfo> listUserOrder = MemberProcessor.GetListUserOrder(orderInfo.UserId, orderQuery);
															foreach (OrderInfo item in listUserOrder)
															{
																OrderHelper.OrderConfirmPaySendMessage(item);
															}
														}
														else
														{
															OrderHelper.OrderConfirmPaySendMessage(orderInfo);
														}
														orderInfo.OnPayment();
													}
												}
												catch (Exception ex2)
												{
													IDictionary<string, string> dictionary2 = new Dictionary<string, string>();
													dictionary2.Add("ErrorMessage", ex2.Message);
													dictionary2.Add("StackTrace", ex2.StackTrace);
													if (ex2.InnerException != null)
													{
														dictionary2.Add("InnerException", ex2.InnerException.ToString());
													}
													if (ex2.GetBaseException() != null)
													{
														dictionary2.Add("BaseException", ex2.GetBaseException().Message);
													}
													if (ex2.TargetSite != (MethodBase)null)
													{
														dictionary2.Add("TargetSite", ex2.TargetSite.ToString());
													}
													dictionary2.Add("ExSource", ex2.Source);
													Globals.AppendLog(dictionary2, "支付更新订单收款记录或者消息通知时出错：" + ex2.Message, "", "", "UserPay");
												}
											});
										}
										this.Page.Response.Redirect(base.GetRouteUrl("FinishOrder", new
										{
											orderId = orderInfo.OrderId
										}), false);
									}
								}
								catch (Exception ex)
								{
									IDictionary<string, string> dictionary = new Dictionary<string, string>();
									dictionary.Add("ErrorMessage", ex.Message);
									dictionary.Add("StackTrace", ex.StackTrace);
									if (ex.InnerException != null)
									{
										dictionary.Add("InnerException", ex.InnerException.ToString());
									}
									if (ex.GetBaseException() != null)
									{
										dictionary.Add("BaseException", ex.GetBaseException().Message);
									}
									if (ex.TargetSite != (MethodBase)null)
									{
										dictionary.Add("TargetSite", ex.TargetSite.ToString());
									}
									dictionary.Add("ExSource", ex.Source);
									Globals.AppendLog(dictionary, "支付更新订单收款记录或者消息通知时出错：" + ex.Message, "", "", "OrderCreate");
									this.ShowMessage(ex.ToString(), false, "", 1);
								}
							}
						}
					}
				}
			}
		}

		private bool CheckProductSkuHasProductPreInfo()
		{
			string[] skuIdArray = this.productSku.Split(',');
			return ProductPreSaleHelper.HasProductPreSaleInfoBySkuIds(skuIdArray);
		}

		private void ReBindPayment()
		{
			IList<PaymentModeInfo> list = ShoppingProcessor.GetPaymentModes(PayApplicationType.payOnPC);
			IList<PaymentModeInfo> list2 = new List<PaymentModeInfo>();
			if (HiContext.Current.User.UserId.Equals(0))
			{
				IList<string> paymentTypes = new List<string>();
				paymentTypes.Add("hishop.plugins.payment.advancerequest");
				list = (from item in list
				where !paymentTypes.Contains(item.Gateway)
				select item).ToList();
			}
			if (this.from == "groupbuy")
			{
				IList<string> paymentTypes2 = new List<string>();
				paymentTypes2.Add("hishop.plugins.payment.podrequest");
				paymentTypes2.Add("hishop.plugins.payment.bankrequest");
				list = (from item in list
				where !paymentTypes2.Contains(item.Gateway)
				select item).ToList();
			}
			if (this.from == "countdown")
			{
				IList<string> paymentTypes3 = new List<string>();
				paymentTypes3.Add("hishop.plugins.payment.podrequest");
				paymentTypes3.Add("hishop.plugins.payment.bankrequest");
				list = (from item in list
				where !paymentTypes3.Contains(item.Gateway)
				select item).ToList();
			}
			HttpCookie httpCookie = HiContext.Current.Context.Request.Cookies["Token_" + HiContext.Current.UserId];
			if (httpCookie != null && !string.IsNullOrEmpty(httpCookie.Value))
			{
				foreach (PaymentModeInfo item in list)
				{
					if (string.Compare(item.Gateway, "hishop.plugins.payment.alipay_shortcut.shortcutrequest", true) == 0 || string.Compare(item.Gateway, "hishop.plugins.payment.alipaydirect.directrequest", true) == 0 || string.Compare(item.Gateway, "hishop.plugins.payment.alipayassure.assurerequest", true) == 0 || string.Compare(item.Gateway, "hishop.plugins.payment.alipay.standardrequest", true) == 0 || (string.Compare(item.Gateway, "hishop.plugins.payment.advancerequest", true) == 0 && HiContext.Current.UserId != 0))
					{
						list2.Add(item);
					}
				}
			}
			else
			{
				foreach (PaymentModeInfo item2 in list)
				{
					if (string.Compare(item2.Gateway, "hishop.plugins.payment.alipay_shortcut.shortcutrequest", true) != 0)
					{
						list2.Add(item2);
					}
					if (string.Compare(item2.Gateway, "hishop.plugins.payment.advancerequest", true) == 0 && HiContext.Current.User == null)
					{
						list2.Remove(item2);
					}
				}
			}
			if (list2.Count == 0)
			{
				this.ShowMessage("商城暂未配置支付方式，请稍后提交订单", false, "", 1);
				this.btnCreateOrder.Visible = false;
			}
		}

		private void BindUserAddress()
		{
			ShippingAddressInfo shippingAddressInfo = null;
			int regionId = 0;
			if (HiContext.Current.UserId != 0)
			{
				IList<ShippingAddressInfo> shippingAddresses = MemberProcessor.GetShippingAddresses(false);
				if (shippingAddresses.Count > 0)
				{
					shippingAddressInfo = shippingAddresses[0];
					this.txtShipTo.Text = shippingAddressInfo.ShipTo;
					this.dropRegions.SetSelectedRegionId(shippingAddressInfo.RegionId);
					this.dropRegions.DataBind();
					this.txtAddress.Text = shippingAddressInfo.RegionLocation + shippingAddressInfo.Address;
					this.txtTelPhone.Text = shippingAddressInfo.TelPhone;
					this.txtCellPhone.Text = shippingAddressInfo.CellPhone;
					this.hidShipperId.Value = shippingAddressInfo.ShippingId.ToString();
					this.txtBuilderNumber.Text = shippingAddressInfo.BuildingNumber;
					regionId = shippingAddressInfo.RegionId;
				}
				else
				{
					Hidistro.Entities.Members.MemberInfo user = HiContext.Current.User;
					this.txtShipTo.Text = user.RealName;
					this.dropRegions.SetSelectedRegionId(user.RegionId);
					this.dropRegions.DataBind();
					this.txtAddress.Text = user.Address;
					this.txtCellPhone.Text = user.CellPhone;
				}
				this.hidIsAnonymous.Value = "0";
				if (this.siteSettings.OpenMultStore && this.siteSettings.IsOpenPickeupInStore)
				{
					if (this.from.ToLower() != "groupbuy" && this.from.ToLower() != "bundling" && StoresHelper.IsSupportGetgoodsOnStores(this.shoppingCart) && !this.isPreSale && this.shoppingCart.LineItems.Count > 0)
					{
						this.hidGetgoodsOnStores.Value = "true";
					}
				}
				else if (!this.siteSettings.OpenMultStore && this.siteSettings.IsOpenPickeupInStore)
				{
					this.hidIsCloseStoreButGetGoods.Value = "true";
					this.hidGetGoodsRemark.Value = this.siteSettings.PickeupInStoreRemark;
				}
			}
			else
			{
				this.hidIsAnonymous.Value = "1";
			}
			this.userRegionId = regionId;
			this.lblShippModePrice.Text = ShoppingProcessor.CalcFreight(regionId, this.shoppingCart).F2ToString("f2");
			this.lblShippModePrice.Attributes.Add("Freight", this.lblShippModePrice.Text);
			IList<UserInvoiceDataInfo> userInvoiceDataList = MemberProcessor.GetUserInvoiceDataList(HiContext.Current.UserId, shippingAddressInfo);
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

		private void BindLastInvoiceInfo()
		{
			if (HiContext.Current.UserId <= 0)
			{
				return;
			}
		}

		private void BindShoppingCartInfo(ShoppingCartInfo shoppingCart)
		{
			int num;
			if (shoppingCart.LineItems.Count > 0)
			{
				var dataSource = (from i in (from i in shoppingCart.LineItems
				select new
				{
					SupplierId = i.SupplierId,
					SupplierName = ((this.siteSettings.OpenMultStore && this.hidStoreId.ToInt(0) > 0) ? i.SupplierName : ((this.siteSettings.OpenSupplier && i.SupplierId > 0) ? i.SupplierName : "平台"))
				}).Distinct()
				orderby i.SupplierId
				select i).ToList();
				this.cartProductList.DataSource = dataSource;
				this.cartProductList.ShoppingCart = shoppingCart;
				this.cartProductList.DataBind();
				if ((from i in shoppingCart.LineItems
				where i.SupplierId == 0
				select i).Count() <= 0)
				{
					this.divcopue.Visible = false;
				}
				if ((from i in shoppingCart.LineItems
				where i.SupplierId > 0
				select i).Count() > 0 && this.siteSettings.OpenSupplier)
				{
					this.hidHasSupplierProduct.Value = "1";
				}
				if ((from i in shoppingCart.LineItems
				where i.IsCrossborder
				select i).Count() > 0 && this.siteSettings.IsOpenCertification)
				{
					this.hidIsOpenCertification.Value = "1";
					HtmlInputHidden htmlInputHidden = this.hidCertificationModel;
					num = this.siteSettings.CertificationModel;
					htmlInputHidden.Value = num.ToString();
				}
				else
				{
					this.hidIsOpenCertification.Value = "0";
				}
			}
			else
			{
				this.divProductList.Visible = false;
			}
			if (shoppingCart.LineGifts.Count > 0)
			{
				IEnumerable<ShoppingCartGiftInfo> enumerable = from s in shoppingCart.LineGifts
				where s.PromoType == 0
				select s;
				this.common_pointgiftlist.DataSource = enumerable;
				this.common_pointgiftlist.DataBind();
				this.common_pointgiftlist.ShowPointGifts(enumerable.Count() > 0);
				IEnumerable<ShoppingCartGiftInfo> enumerable2 = from a in shoppingCart.LineGifts
				where a.PromoType == 15
				select a;
				this.cartGiftList.DataSource = enumerable2;
				this.cartGiftList.DataBind();
				this.cartGiftList.ShowGiftCart(enumerable2.Count() > 0, false, false);
				int num2 = (from i in shoppingCart.LineItems
				where i.SupplierId == 0
				select i).Count();
				if (num2 <= 0 && enumerable.Count() > 0)
				{
					decimal num3 = ShoppingProcessor.CalcGiftFreight(this.userRegionId, shoppingCart.LineGifts);
					this.lblGiftFeright.Text = "运费：￥" + num3.F2ToString("f2");
				}
			}
			if (shoppingCart.LineItems.Count == 0 && shoppingCart.LineGifts.Count > 0)
			{
				this.hidIsGiftOrder.Value = "1";
			}
			if (shoppingCart.IsReduced)
			{
				this.hlkReducedPromotion.Text = "<h2>" + shoppingCart.ReducedPromotionName + string.Format(" 优惠：</h2>{0}", shoppingCart.ReducedPromotionAmount.F2ToString("f2"));
				this.hlkReducedPromotion.NavigateUrl = base.GetRouteUrl("FavourableDetails", new
				{
					activityId = shoppingCart.ReducedPromotionId
				});
				this.lblDeductibleMoney.Text = shoppingCart.ReducedPromotionAmount.F2ToString("f2");
			}
			if (shoppingCart.IsFreightFree)
			{
				this.hlkFeeFreight.Text = $"（{shoppingCart.FreightFreePromotionName}）";
				this.hlkFeeFreight.NavigateUrl = base.GetRouteUrl("FavourableDetails", new
				{
					activityId = shoppingCart.FreightFreePromotionId
				});
			}
			this.lblTotalPrice.Money = shoppingCart.GetAmount(false);
			this.hidTotalPrice.Value = shoppingCart.GetTotal(false).ToString();
			this.lblOrderTotal.Money = shoppingCart.GetTotal(false);
			Label label = this.litPoint;
			num = shoppingCart.GetPoint(this.siteSettings.PointsRate);
			label.Text = num.ToString();
			if (this.isPreSale && shoppingCart.LineItems.Count > 0)
			{
				decimal num4 = default(decimal);
				ProductPreSaleInfo productPreSaleInfo = ProductPreSaleHelper.GetProductPreSaleInfo(this.preSaleId);
				if (productPreSaleInfo != null)
				{
					num4 = ((productPreSaleInfo.Deposit == decimal.Zero) ? (shoppingCart.LineItems[0].MemberPrice * (decimal)productPreSaleInfo.DepositPercent / 100m) : productPreSaleInfo.Deposit) * (decimal)shoppingCart.LineItems[0].Quantity;
					decimal num5 = shoppingCart.GetTotal(false) - num4;
					FormatedMoneyLabel formatedMoneyLabel = this.lblDeposit;
					FormatedMoneyLabel formatedMoneyLabel2 = this.lblDepositPay;
					object obj3 = formatedMoneyLabel.Money = (formatedMoneyLabel2.Money = num4);
					this.lblFinalPayment.Money = ((num5 > decimal.Zero) ? num5 : decimal.Zero);
					this.lblPreSaleOrderTotal.Money = shoppingCart.GetTotal(false);
					this.hidIsPreSale.Value = "1";
				}
			}
			this.litAllWeight.Text = shoppingCart.TotalWeight.F2ToString("f2");
			if (shoppingCart.IsSendTimesPoint)
			{
				this.hlkSentTimesPoint.Text = string.Format("（{0}；送{1}倍）", shoppingCart.SentTimesPointPromotionName, shoppingCart.TimesPoint.F2ToString("f2"));
				this.hlkSentTimesPoint.NavigateUrl = base.GetRouteUrl("FavourableDetails", new
				{
					activityId = shoppingCart.SentTimesPointPromotionId
				});
				this.hidTimePoint.Value = shoppingCart.TimesPoint.F2ToString("f2");
			}
		}

		private bool ValidateCreateOrder()
		{
			if (string.IsNullOrEmpty(this.hidShipperId.Value) && MemberProcessor.GetShippingAddressCount(HiContext.Current.UserId) > 0)
			{
				this.ShowMessage("请选择收货地址", false, "", 1);
				return false;
			}
			string pattern = "[\\u4e00-\\u9fa5a-zA-Z]+[\\u4e00-\\u9fa5_a-zA-Z0-9]*";
			Regex regex = new Regex(pattern);
			if (string.IsNullOrEmpty(this.txtShipTo.Text) || !regex.IsMatch(this.txtShipTo.Text.Trim()))
			{
				this.ShowMessage("请输入正确的收货人姓名", false, "", 1);
				return false;
			}
			if (this.txtShipTo.Text.Length < 2 || this.txtShipTo.Text.Length > 20)
			{
				this.ShowMessage("收货人姓名长度应在2-20个字符之间", false, "", 1);
				return false;
			}
			if (string.IsNullOrEmpty(this.txtAddress.Text))
			{
				this.ShowMessage("请输入收货人详细地址", false, "", 1);
				return false;
			}
			if (string.IsNullOrEmpty(this.inputShippingModeId.Value))
			{
				this.ShowMessage("请选择配送方式", false, "", 1);
				return false;
			}
			if (string.IsNullOrEmpty(this.inputPaymentModeId.Value) && this.shoppingCart.GetTotal(false) > decimal.Zero)
			{
				this.ShowMessage("请选择支付方式", false, "", 1);
				return false;
			}
			if (this.inputPaymentModeId.Value.ToInt(0) == 0 && TradeHelper.GetPaymentModeCount(PayApplicationType.payOnPC) <= 0)
			{
				this.ShowMessage("商城未配置在线支付,请选择其他支付方式", false, "", 1);
				return false;
			}
			int num = this.hidStoreId.Value.ToInt(0);
			StoresInfo storesInfo = null;
			if (num > 0)
			{
				storesInfo = DepotHelper.GetStoreById(num);
				if (storesInfo == null)
				{
					this.ShowMessage("错误的门店信息,请重新选择", false, "", 1);
					return false;
				}
			}
			if (storesInfo != null)
			{
				if (this.inputShippingModeId.Value.ToInt(0) == -2 && !storesInfo.IsOnlinePay && this.inputPaymentModeId.Value.ToInt(0) == 0)
				{
					this.ShowMessage("门店不支持在线支付,请重新选择支付方式", false, "", 1);
					return false;
				}
				if (this.inputShippingModeId.Value.ToInt(0) == -2 && !storesInfo.IsOfflinePay && this.inputPaymentModeId.Value.ToInt(0) == -3)
				{
					this.ShowMessage("门店不支持到店支付,请重新选择支付方式", false, "", 1);
					return false;
				}
			}
			if (this.inputPaymentModeId.Value.ToInt(0) == -3 && this.inputShippingModeId.Value != "-2")
			{
				this.ShowMessage("错误的支付方式(到店支付）,请重新选择", false, "", 1);
				return false;
			}
			if (this.inputPaymentModeId.Value.ToInt(0) != 0 && this.inputPaymentModeId.Value.ToInt(0) != -3 && TradeHelper.GetPaymentMode(this.inputPaymentModeId.Value.ToInt(0)) == null)
			{
				this.ShowMessage("错误的支付方式,请重新选择", false, "", 1);
				return false;
			}
			if (string.IsNullOrEmpty(this.txtTelPhone.Text.Trim()) && string.IsNullOrEmpty(this.txtCellPhone.Text.Trim()))
			{
				this.ShowMessage("电话号码和手机号码必填其一", false, "", 1);
				return false;
			}
			if (this.chkTax.Checked)
			{
				int id = this.hidInvoiceId.Value.ToInt(0);
				bool flag = this.hidIsPersonal.Value == "true" && true;
				UserInvoiceDataInfo userInvoiceDataInfo = MemberProcessor.GetUserInvoiceDataInfo(id);
				InvoiceType invoiceType = (InvoiceType)this.hidInvoiceType.Value.ToInt(0);
				if (userInvoiceDataInfo == null && !flag)
				{
					this.ShowMessage("请完善发票信息", false, "", 1);
					return false;
				}
			}
			if ((from i in this.shoppingCart.LineItems
			where i.IsCrossborder
			select i).Count() > 0 && this.siteSettings.IsOpenCertification)
			{
				if (this.hidIsGetIDInfo.Value == "1")
				{
					if (string.IsNullOrWhiteSpace(this.txtIDNumber.Text.Trim()) || !new Regex("(^\\d{15}$)|(^\\d{18}$)|(^\\d{17}(\\d|X|x)$)").IsMatch(this.txtIDNumber.Text.Trim()))
					{
						this.ShowMessage("身份证号格式错误", false, "", 1);
						return false;
					}
					if (HiContext.Current.SiteSettings.CertificationModel == 2)
					{
						if (string.IsNullOrEmpty(this.hididCardJustImg.Value))
						{
							this.ShowMessage("请上传证件照正面", false, "", 1);
							return false;
						}
						if (string.IsNullOrEmpty(this.hididCardAntiImg.Value))
						{
							this.ShowMessage("请上传证件照反面", false, "", 1);
							return false;
						}
					}
				}
				else
				{
					ShippingAddressInfo shippingAddress = MemberProcessor.GetShippingAddress(Convert.ToInt32(this.hidShipperId.Value));
					if (shippingAddress != null)
					{
						if (string.IsNullOrWhiteSpace(shippingAddress.IDNumber))
						{
							this.ShowMessage("请在该收货地址填写正确身份证号", false, "", 1);
							return false;
						}
						if (HiContext.Current.SiteSettings.CertificationModel == 2)
						{
							if (string.IsNullOrEmpty(shippingAddress.IDImage1))
							{
								this.ShowMessage("请在该收货地址上传证件照正面", false, "", 1);
								return false;
							}
							if (string.IsNullOrEmpty(shippingAddress.IDImage2))
							{
								this.ShowMessage("请在该收货地址上传证件照反面", false, "", 1);
								return false;
							}
						}
					}
				}
			}
			string text = this.hidTradePassword.Text;
			decimal num2 = this.hidUseBalance.Text.ToDecimal(0);
			if (num2 > decimal.Zero)
			{
				if (string.IsNullOrEmpty(text))
				{
					this.ShowMessage("你输入正确的交易密码！", false, "", 1);
					return false;
				}
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				if (!masterSettings.OpenBalancePay)
				{
					this.ShowMessage("系统未开启预付款支付！", false, "", 1);
					return false;
				}
				Hidistro.Entities.Members.MemberInfo user = HiContext.Current.User;
				if (!user.IsOpenBalance || string.IsNullOrEmpty(user.TradePassword) || string.IsNullOrEmpty(user.TradePasswordSalt))
				{
					this.ShowMessage("您还没有设置交易密码！", false, "", 1);
					return false;
				}
				if (user.Balance - user.RequestBalance < num2)
				{
					this.ShowMessage("预付款余额不够用于抵扣！", false, "", 1);
					return false;
				}
				if (!MemberProcessor.ValidTradePassword(text))
				{
					this.ShowMessage("交易密码有误，请重试！", false, "", 1);
					return false;
				}
			}
			return true;
		}

		private OrderInfo GetOrderInfo(ShoppingCartInfo shoppingCartInfo)
		{
			OrderInfo orderInfo = ShoppingProcessor.ConvertShoppingCartToOrder(shoppingCartInfo, this.isGroupBuy, this.isCountDown, 0);
			if (orderInfo == null)
			{
				return null;
			}
			string text = "";
			if (this.isGroupBuy)
			{
				orderInfo.GroupBuyId = this.groupbuyInfo.GroupBuyId;
				orderInfo.NeedPrice = this.groupbuyInfo.NeedPrice;
			}
			if (this.isCountDown)
			{
				orderInfo.CountDownBuyId = this.countdownInfo.CountDownId;
			}
			orderInfo.BalanceAmount = this.hidUseBalance.Text.ToDecimal(0);
			orderInfo.OrderId = OrderIDFactory.GenerateOrderId();
			orderInfo.ParentOrderId = "0";
			orderInfo.OrderDate = DateTime.Now;
			Hidistro.Entities.Members.MemberInfo user = HiContext.Current.User;
			orderInfo.UserId = user.UserId;
			orderInfo.Username = user.UserName;
			orderInfo.EmailAddress = user.Email;
			orderInfo.RealName = (string.IsNullOrEmpty(user.RealName) ? user.NickName : user.RealName);
			orderInfo.QQ = user.QQ;
			orderInfo.Wangwang = user.Wangwang;
			orderInfo.MSN = user.WeChat;
			orderInfo.Remark = DataHelper.CleanSearchString(Globals.HtmlEncode(this.txtMessage.Text));
			orderInfo.OrderStatus = OrderStatus.WaitBuyerPay;
			this.FillOrderCoupon(orderInfo);
			this.FillOrderShippingMode(orderInfo, shoppingCartInfo);
			this.FillOrderPaymentMode(orderInfo);
			orderInfo.OrderSource = OrderSource.PC;
			this.FillDeductionPoints(orderInfo);
			if (this.chkTax.Checked)
			{
				int id = this.hidInvoiceId.Value.ToInt(0);
				UserInvoiceDataInfo userInvoiceDataInfo = MemberProcessor.GetUserInvoiceDataInfo(id);
				bool flag = this.hidIsPersonal.Value == "true" && true;
				InvoiceType invoiceType;
				if (this.hidInvoiceType.Value.ToInt(0) == 0 || userInvoiceDataInfo == null)
				{
					userInvoiceDataInfo = new UserInvoiceDataInfo
					{
						Id = 0,
						InvoiceType = InvoiceType.Personal,
						InvoiceTitle = "个人",
						LastUseTime = DateTime.Now
					};
					flag = true;
					invoiceType = InvoiceType.Personal;
				}
				else
				{
					invoiceType = userInvoiceDataInfo.InvoiceType;
				}
				if (invoiceType == InvoiceType.VATInvoice && this.siteSettings.VATTaxRate > decimal.Zero && this.siteSettings.EnableVATInvoice)
				{
					orderInfo.Tax = ((orderInfo.GetTotal(false) - orderInfo.AdjustedFreight) * this.siteSettings.VATTaxRate / 100m).F2ToString("f2").ToDecimal(0);
				}
				else if (this.siteSettings.TaxRate > decimal.Zero && (this.siteSettings.EnableTax || this.siteSettings.EnableE_Invoice))
				{
					orderInfo.Tax = ((orderInfo.GetTotal(false) - orderInfo.AdjustedFreight) * this.siteSettings.TaxRate / 100m).F2ToString("f2").ToDecimal(0);
				}
				else
				{
					orderInfo.Tax = decimal.Zero;
				}
				orderInfo.InvoiceTitle = userInvoiceDataInfo.InvoiceTitle;
				orderInfo.InvoiceType = userInvoiceDataInfo.InvoiceType;
				if (orderInfo.InvoiceType == InvoiceType.Enterprise || orderInfo.InvoiceType == InvoiceType.Enterprise_Electronic || orderInfo.InvoiceType == InvoiceType.VATInvoice)
				{
					orderInfo.InvoiceTaxpayerNumber = userInvoiceDataInfo.InvoiceTaxpayerNumber;
				}
				orderInfo.InvoiceData = JsonHelper.GetJson(userInvoiceDataInfo);
			}
			if (this.isPreSale)
			{
				orderInfo.PreSaleId = this.preSaleId;
				orderInfo.Deposit = (decimal)this.lblDeposit.Money;
				orderInfo.FinalPayment = orderInfo.GetFinalPayment();
			}
			if (HiContext.Current.UserId != 0)
			{
				orderInfo.Points = orderInfo.GetPoint(this.siteSettings.PointsRate);
			}
			else
			{
				orderInfo.Points = 0;
			}
			orderInfo.ExchangePoints = shoppingCartInfo.GetTotalNeedPoint();
			if (orderInfo.IsincludeCrossBorderGoods && this.siteSettings.IsOpenCertification)
			{
				string imageServerUrl = Globals.GetImageServerUrl();
				if (this.hidIsGetIDInfo.Value == "1")
				{
					orderInfo.IDNumber = HiCryptographer.Encrypt(this.txtIDNumber.Text.Trim());
					if (HiContext.Current.SiteSettings.CertificationModel == 2)
					{
						orderInfo.IDImage1 = (string.IsNullOrEmpty(imageServerUrl) ? Globals.SaveFile("user", this.hididCardJustImg.Value.Trim(), "/Storage/master/", true, false, "") : this.hididCardJustImg.Value.Trim());
						orderInfo.IDImage2 = (string.IsNullOrEmpty(imageServerUrl) ? Globals.SaveFile("user", this.hididCardAntiImg.Value.Trim(), "/Storage/master/", true, false, "") : this.hididCardAntiImg.Value.Trim());
					}
					orderInfo.IDStatus = 1;
				}
				else
				{
					ShippingAddressInfo shippingAddress = MemberProcessor.GetShippingAddress(Convert.ToInt32(this.hidShipperId.Value));
					if (shippingAddress != null)
					{
						orderInfo.IDNumber = shippingAddress.IDNumber;
						if (HiContext.Current.SiteSettings.CertificationModel == 2)
						{
							orderInfo.IDImage1 = (string.IsNullOrEmpty(imageServerUrl) ? Globals.SaveFile("user", shippingAddress.IDImage1.Trim(), "/Storage/master/", true, false, "") : shippingAddress.IDImage1.Trim());
							orderInfo.IDImage2 = (string.IsNullOrEmpty(imageServerUrl) ? Globals.SaveFile("user", shippingAddress.IDImage2.Trim(), "/Storage/master/", true, false, "") : shippingAddress.IDImage2.Trim());
						}
						orderInfo.IDStatus = 1;
					}
				}
			}
			return orderInfo;
		}

		private void FillDeductionPoints(OrderInfo orderInfo)
		{
			int num = default(int);
			if (this.chkIsUsePoints.Checked && (this.siteSettings.CanPointUseWithCoupon || string.IsNullOrEmpty(orderInfo.CouponCode)) && !string.IsNullOrEmpty(this.txtUsePoints.Text) && int.TryParse(this.txtUsePoints.Text, out num) && this.siteSettings.ShoppingDeduction > 0 && num > 0)
			{
				int shoppingDeductionRatio = this.siteSettings.ShoppingDeductionRatio;
				decimal value = (decimal)shoppingDeductionRatio * (orderInfo.GetTotal(false) - orderInfo.Tax - orderInfo.AdjustedFreight) * (decimal)this.siteSettings.ShoppingDeduction / 100m;
				Hidistro.Entities.Members.MemberInfo user = Users.GetUser(HiContext.Current.UserId);
				if (user != null)
				{
					int num2 = (user.Points > (int)value) ? ((int)value) : user.Points;
					if (num > num2)
					{
						num = num2;
					}
					decimal value2 = ((decimal)num / (decimal)this.siteSettings.ShoppingDeduction).F2ToString("f2").ToDecimal(0);
					orderInfo.DeductionPoints = num;
					orderInfo.DeductionMoney = value2;
				}
			}
		}

		private void FillOrderCoupon(OrderInfo orderInfo)
		{
			if (string.IsNullOrEmpty(this.htmlCouponCode.Value))
			{
				return;
			}
			CouponItemInfo userCouponInfo = ShoppingProcessor.GetUserCouponInfo(Convert.ToDecimal(this.lblOrderTotal.Money), this.htmlCouponCode.Value);
			if (userCouponInfo == null || !string.IsNullOrEmpty(userCouponInfo.OrderId) || userCouponInfo.UsedTime.HasValue || HiContext.Current.UserId == 0 || userCouponInfo.UserId != HiContext.Current.UserId)
			{
				return;
			}
			if (orderInfo.CountDownBuyId == 0 && orderInfo.GroupBuyId == 0)
			{
				goto IL_00e4;
			}
			if (orderInfo.GroupBuyId > 0 && userCouponInfo.UseWithGroup.Value)
			{
				goto IL_00e4;
			}
			int num = (orderInfo.CountDownBuyId > 0 && userCouponInfo.UseWithPanicBuying.Value) ? 1 : 0;
			goto IL_00e5;
			IL_00e5:
			if (num != 0)
			{
				orderInfo.CouponName = userCouponInfo.CouponName;
				if (userCouponInfo.OrderUseLimit.HasValue)
				{
					orderInfo.CouponAmount = userCouponInfo.OrderUseLimit.Value;
				}
				orderInfo.CouponCode = this.htmlCouponCode.Value;
				if (userCouponInfo.Price.Value >= orderInfo.GetAmount(false))
				{
					orderInfo.CouponValue = orderInfo.GetAmount(false);
				}
				else
				{
					orderInfo.CouponValue = userCouponInfo.Price.Value;
				}
				this.couponUseProducts = userCouponInfo.CanUseProducts;
			}
			return;
			IL_00e4:
			num = 1;
			goto IL_00e5;
		}

		private void FillOrderShippingMode(OrderInfo orderInfo, ShoppingCartInfo shoppingCartInfo)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			ShippingAddressInfo shippingAddress = MemberProcessor.GetShippingAddress(this.hidShipperId.Value.ToInt(0));
			if (shippingAddress == null)
			{
				orderInfo.RegionId = this.dropRegions.GetSelectedRegionId().Value;
				orderInfo.ShippingRegion = RegionHelper.GetFullRegion(this.dropRegions.GetSelectedRegionId().Value, ",", true, 0);
			}
			else
			{
				orderInfo.RegionId = shippingAddress.RegionId;
				orderInfo.ShippingRegion = RegionHelper.GetFullRegion(shippingAddress.RegionId, ",", true, 0);
				orderInfo.ShippingId = shippingAddress.ShippingId;
			}
			orderInfo.FullRegionPath = RegionHelper.GetFullPath(orderInfo.RegionId, true);
			orderInfo.Address = Globals.HtmlEncode(this.txtAddress.Text);
			orderInfo.ZipCode = "";
			orderInfo.ShipTo = Globals.HtmlEncode(this.txtShipTo.Text);
			orderInfo.TelPhone = this.txtTelPhone.Text;
			orderInfo.CellPhone = this.txtCellPhone.Text;
			if (!string.IsNullOrEmpty(this.inputShippingModeId.Value))
			{
				orderInfo.ShippingModeId = this.inputShippingModeId.Value.ToInt(0);
			}
			orderInfo.ShipToDate = this.drpShipToDate.Value;
			if (masterSettings.OpenMultStore && !this.isPreSale && !this.isGroupBuy && !this.isFireGroup)
			{
				if (orderInfo.ShippingModeId == -2)
				{
					orderInfo.ModeName = "上门自提";
					orderInfo.StoreId = this.hidStoreId.Value.ToInt(0);
				}
				else
				{
					orderInfo.ShippingModeId = 0;
					orderInfo.ModeName = "快递配送";
					if (masterSettings.AutoAllotOrder && orderInfo.LineItems.Count > 0 && orderInfo.CountDownBuyId == 0 && orderInfo.StoreId <= 0)
					{
						int num = 0;
						if (shippingAddress != null && !string.IsNullOrWhiteSpace(shippingAddress.LatLng))
						{
							string[] array = shippingAddress.LatLng.Split(',');
							IList<StoreLocationInfo> storeLocationInfoByOpenId = DepotHelper.GetStoreLocationInfoByOpenId("pc-" + shippingAddress.UserId + "-" + shippingAddress.ShippingId, array[1], array[0]);
							if (storeLocationInfoByOpenId != null && storeLocationInfoByOpenId.Count() > 0)
							{
								List<StoreLocationInfo> source = (from d in storeLocationInfoByOpenId
								orderby d.Distances
								select d).ToList();
								int num2 = 0;
								for (int i = 0; i < source.Count(); i++)
								{
									StoreLocationInfo storeLocationInfo = storeLocationInfoByOpenId[i];
									num = storeLocationInfo.StoreId;
									StoresInfo storeById = DepotHelper.GetStoreById(num);
									if (storeById == null)
									{
										num = 0;
									}
									else if (!storeById.IsStoreDelive && !storeById.IsSupportExpress)
									{
										num = 0;
									}
									else
									{
										if (storeById.IsSupportExpress && num2 == 0)
										{
											num2 = num;
										}
										if (!storeById.IsSupportExpress && storeById.ServeRadius.HasValue && (storeById.ServeRadius.Value * 1000.0 < storeLocationInfo.Distances || !DepotHelper.IsStoreInDeliveArea(num, shippingAddress.FullRegionPath)))
										{
											num = 0;
										}
										else
										{
											foreach (ShoppingCartItemInfo lineItem in this.shoppingCart.LineItems)
											{
												if (!StoresHelper.StoreHasProductSku(storeLocationInfo.StoreId, lineItem.SkuId) || !StoresHelper.StoreHasStock(storeLocationInfo.StoreId, lineItem.SkuId, lineItem.Quantity))
												{
													if (num2 == storeLocationInfo.StoreId)
													{
														num2 = 0;
													}
													num = 0;
													break;
												}
											}
											if (num > 0)
											{
												break;
											}
										}
									}
								}
								if (num == 0)
								{
									num = num2;
								}
							}
						}
						if (num == 0)
						{
							num = StoresHelper.GetStoreAutoAllotOrder(shoppingCartInfo, orderInfo.RegionId);
						}
						if (num > 0)
						{
							orderInfo.ShippingModeId = -1;
							orderInfo.ModeName = "门店配送";
							orderInfo.StoreId = num;
						}
						else
						{
							orderInfo.StoreId = 0;
						}
					}
				}
			}
			else if (orderInfo.ShippingModeId == -2 && this.siteSettings.IsOpenPickeupInStore)
			{
				orderInfo.ModeName = "上门自提";
				orderInfo.StoreId = 0;
			}
			else
			{
				orderInfo.ShippingModeId = 0;
				orderInfo.ModeName = "快递配送";
			}
			if (orderInfo.ShippingModeId == -2)
			{
				decimal num5 = orderInfo.AdjustedFreight = (orderInfo.Freight = default(decimal));
			}
			else if (!shoppingCartInfo.IsFreightFree)
			{
				decimal num5 = orderInfo.AdjustedFreight = (orderInfo.Freight = ShoppingProcessor.CalcFreight(orderInfo.RegionId, shoppingCartInfo));
			}
			else
			{
				decimal num5 = orderInfo.AdjustedFreight = (orderInfo.Freight = default(decimal));
			}
		}

		private void FillOrderPaymentMode(OrderInfo orderInfo)
		{
			int num = 0;
			int.TryParse(this.inputPaymentModeId.Value, out num);
			int num2;
			switch (num)
			{
			case 0:
				orderInfo.PaymentTypeId = 0;
				orderInfo.PaymentType = "在线支付";
				orderInfo.Gateway = "";
				return;
			case -3:
				if (!this.isPreSale)
				{
					num2 = ((orderInfo.LineItems.Count > 0) ? 1 : 0);
					break;
				}
				goto default;
			default:
				num2 = 0;
				break;
			}
			if (num2 != 0)
			{
				orderInfo.PaymentTypeId = -3;
				orderInfo.PaymentType = "到店支付";
				orderInfo.Gateway = "hishop.plugins.payment.payonstore";
			}
			else
			{
				orderInfo.PaymentTypeId = num;
				PaymentModeInfo paymentMode = ShoppingProcessor.GetPaymentMode(orderInfo.PaymentTypeId);
				if (paymentMode != null)
				{
					orderInfo.PaymentType = Globals.HtmlEncode(paymentMode.Name);
					orderInfo.Gateway = paymentMode.Gateway;
				}
			}
		}
	}
}
