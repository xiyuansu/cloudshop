using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Shopping;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WapOrderShippingPaymentSet : WAPMemberTemplatedWebControl
	{
		private HtmlInputHidden hidGetgoodsOnStores;

		private HtmlInputHidden hidHasStoresInCity;

		private HtmlInputHidden hidPaymentId_Podrequest;

		private HtmlInputHidden inputShippingModeId;

		private HtmlInputHidden inputPaymentModeId;

		private HtmlInputHidden hidDeliveryTime;

		private HtmlInputHidden hidStoreId;

		private HtmlInputHidden hidPaymentId_Offline;

		private HtmlInputHidden hidShipAddressId;

		private HtmlInputHidden hidOnlinePayCount;

		private Literal litStoreName;

		private Literal litAddress;

		private Literal litTel;

		private HtmlInputHidden hidHasSupplierProduct;

		private int buyAmount;

		private string productSku;

		private bool isGroupBuy = false;

		private bool isCountDown = false;

		private bool isSignBuy = false;

		private bool ispresale = false;

		private bool isFightGroup = false;

		private int combinaid = 0;

		private int presaleid = 0;

		private string from = "";

		private int shippingModeId = 0;

		private int paymentModeId = 0;

		private int storeId = 0;

		private string deliveryTime = "任意时间";

		private ShoppingCartInfo shoppingCart;

		private int hasSupplierProduct = 0;

		private int RecordId = 0;

		protected override void OnInit(EventArgs e)
		{
			string errorMsg = "";
			this.from = this.Page.Request.QueryString["from"].ToNullString().ToLower();
			this.productSku = this.Page.Request.QueryString["productSku"].ToNullString();
			this.buyAmount = this.Page.Request.QueryString["buyAmount"].ToInt(0);
			this.shippingModeId = this.Page.Request.QueryString["ShippingModeId"].ToInt(0);
			this.storeId = this.Page.Request.QueryString["StoreId"].ToInt(0);
			this.paymentModeId = this.Page.Request.QueryString["paymentModeId"].ToInt(0);
			this.deliveryTime = this.Page.Request.QueryString["deliveryTime"].ToNullString();
			this.hasSupplierProduct = this.Page.Request.QueryString["hasSupplierProduct"].ToInt(0);
			if (this.deliveryTime != "任意时间" && this.deliveryTime != "工作日" && this.deliveryTime != "节假日")
			{
				this.deliveryTime = "任意时间";
			}
			if (this.shippingModeId != 0 && this.shippingModeId != -2)
			{
				this.shippingModeId = 0;
			}
			if (this.from == "groupbuy")
			{
				this.isGroupBuy = true;
			}
			else if (this.from == "countdown")
			{
				this.isCountDown = true;
			}
			else if (this.from == "signbuy")
			{
				this.isSignBuy = true;
			}
			else if (this.from == "combinationbuy")
			{
				this.combinaid = this.Page.Request.QueryString["combinaid"].ToInt(0);
			}
			else if (this.from == "presale")
			{
				this.ispresale = true;
				this.presaleid = this.Page.Request.QueryString["presaleid"].ToInt(0);
			}
			else if (this.from == "fightgroup")
			{
				this.isFightGroup = true;
			}
			else if (this.from == "prize")
			{
				this.RecordId = this.Page.Request.QueryString["RecordId"].ToInt(0);
			}
			else
			{
				HttpCookie httpCookie = HiContext.Current.Context.Request.Cookies["ckids"];
				if (httpCookie != null && !string.IsNullOrEmpty(httpCookie.Value))
				{
					this.productSku = Globals.UrlDecode(httpCookie.Value);
				}
			}
			if (this.RecordId > 0)
			{
				UserAwardRecordsInfo userAwardRecordsInfo = ActivityHelper.GetUserAwardRecordsInfo(this.RecordId);
				if (userAwardRecordsInfo == null)
				{
					base.GotoResourceNotFound("购物车中没有任何商品");
					return;
				}
				int prizeValue = userAwardRecordsInfo.PrizeValue;
				this.shoppingCart = ShoppingCartProcessor.GetPrizeShoppingCart(prizeValue);
			}
			else
			{
				this.shoppingCart = ShoppingCartProcessor.GetShoppingCart(this.from, this.productSku, this.buyAmount, this.combinaid, true, -1, 0);
			}
			if (this.shoppingCart == null)
			{
				base.GotoResourceNotFound("购物车中没有任何商品");
			}
			else if (this.ispresale && !ProductPreSaleHelper.HasProductPreSaleInfo(this.productSku, this.presaleid))
			{
				base.GotoResourceNotFound("商品不在预售活动中");
			}
			else
			{
				if (this.isGroupBuy)
				{
					GroupBuyInfo productGroupBuyInfo = TradeHelper.GetProductGroupBuyInfo(this.shoppingCart.LineItems[0].ProductId, this.buyAmount, out errorMsg);
					if (productGroupBuyInfo == null)
					{
						base.GotoResourceNotFound(errorMsg);
						return;
					}
				}
				if (this.SkinName == null)
				{
					this.SkinName = "Skin-OrderShippingPaymentSet.html";
				}
				base.OnInit(e);
			}
		}

		protected override void AttachChildControls()
		{
			this.inputPaymentModeId = (HtmlInputHidden)this.FindControl("inputPaymentModeId");
			this.inputShippingModeId = (HtmlInputHidden)this.FindControl("inputShippingModeId");
			this.hidPaymentId_Podrequest = (HtmlInputHidden)this.FindControl("hidPaymentId_Podrequest");
			this.hidPaymentId_Offline = (HtmlInputHidden)this.FindControl("hidPaymentId_Offline");
			this.hidGetgoodsOnStores = (HtmlInputHidden)this.FindControl("hidGetgoodsOnStores");
			this.hidHasStoresInCity = (HtmlInputHidden)this.FindControl("hidHasStoresInCity");
			this.hidDeliveryTime = (HtmlInputHidden)this.FindControl("hidDeliveryTime");
			this.hidStoreId = (HtmlInputHidden)this.FindControl("hidStoreId");
			this.hidShipAddressId = (HtmlInputHidden)this.FindControl("hidShipAddressId");
			this.litStoreName = (Literal)this.FindControl("litStoreName");
			this.litAddress = (Literal)this.FindControl("litAddress");
			this.litTel = (Literal)this.FindControl("litTel");
			this.hidOnlinePayCount = (HtmlInputHidden)this.FindControl("hidOnlinePayCount");
			this.hidHasSupplierProduct = (HtmlInputHidden)this.FindControl("hidHasSupplierProduct");
			if (!this.Page.IsPostBack)
			{
				this.hidHasSupplierProduct.Value = this.hasSupplierProduct.ToString();
				this.hidOnlinePayCount.Value = TradeHelper.WapPaymentTypeCount(base.ClientType, this.isFightGroup).ToNullString();
				this.hidGetgoodsOnStores.Value = "false";
				this.hidHasStoresInCity.Value = "false";
				this.hidPaymentId_Podrequest.Value = "0";
				int num = 0;
				int num2 = 0;
				if (this.from != "countdown" && this.from != "groupbuy" && this.hasSupplierProduct != 1)
				{
					if (this.from != "presale" && SalesHelper.IsSupportPodrequest())
					{
						num = 1;
						this.hidPaymentId_Podrequest.Value = "1";
					}
					if (ShoppingProcessor.IsSupportOfflineRequest())
					{
						this.hidPaymentId_Offline.Value = "2";
						num2 = 2;
					}
				}
				if (this.paymentModeId != 0 && this.paymentModeId != num && this.paymentModeId != num2 && this.paymentModeId != -3)
				{
					this.paymentModeId = 0;
				}
				this.hidDeliveryTime.Value = this.deliveryTime;
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
				this.inputShippingModeId.Value = this.shippingModeId.ToString();
				int shipAddressId = 0;
				int.TryParse(this.Page.Request.QueryString["ShipAddressId"].ToNullString(), out shipAddressId);
				IList<ShippingAddressInfo> shippingAddresses = MemberProcessor.GetShippingAddresses(false);
				int regionId = 0;
				IList<ShippingAddressInfo> list = new List<ShippingAddressInfo>();
				ShippingAddressInfo shippingAddressInfo = null;
				if (shipAddressId > 0)
				{
					shippingAddressInfo = shippingAddresses.FirstOrDefault((ShippingAddressInfo a) => a.ShippingId == shipAddressId);
					if (shippingAddressInfo != null)
					{
						regionId = shippingAddressInfo.RegionId;
					}
				}
				else if (shippingAddresses != null && shippingAddresses.Count > 0)
				{
					regionId = shippingAddresses.FirstOrDefault().RegionId;
				}
				this.hidShipAddressId.Value = shipAddressId.ToString();
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				if (masterSettings.OpenMultStore)
				{
					this.hidStoreId.Value = this.storeId.ToString();
					if (this.storeId > 0)
					{
						StoresInfo storeById = DepotHelper.GetStoreById(this.storeId);
						this.litStoreName.Text = (string.IsNullOrEmpty(storeById.StoreOpenTime) ? storeById.StoreName : (storeById.StoreName + " [营业时间:" + storeById.StoreOpenTime + "]"));
						this.litAddress.Text = RegionHelper.GetFullRegion(storeById.RegionId, string.Empty, true, 0) + storeById.Address;
						this.litTel.Text = storeById.Tel;
					}
					if (this.from != "countdown" && this.from != "groupbuy" && this.from != "presale" && this.shoppingCart.LineItems.Count > 0)
					{
						string str = this.productSku.Replace(",", "','");
						str = "'" + str + "'";
						if (ShoppingCartProcessor.CanGetGoodsOnStore(str))
						{
							this.hidGetgoodsOnStores.Value = "true";
							bool flag = StoresHelper.HasStoresInCity(str, regionId);
							this.hidHasStoresInCity.Value = (flag ? "true" : "false");
						}
					}
					else
					{
						this.hidGetgoodsOnStores.Value = "false";
					}
				}
			}
		}
	}
}
