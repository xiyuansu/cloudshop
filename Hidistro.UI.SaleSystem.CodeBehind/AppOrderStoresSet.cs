using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Shopping;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class AppOrderStoresSet : AppshopMemberTemplatedWebControl
	{
		private HtmlInputHidden hidGetgoodsOnStores;

		private HtmlInputHidden hidPaymentId_Podrequest;

		private HtmlInputHidden inputShippingModeId;

		private HtmlInputHidden inputPaymentModeId;

		private HtmlInputHidden hidDeliveryTime;

		private HtmlInputHidden hidStoreId;

		private HtmlInputHidden hidShipAddressId;

		private AppshopTemplatedRepeater rptStores;

		private int buyAmount;

		private string productSku;

		private string newProductSku;

		private string from = "";

		private int shippingModeId = 0;

		private int paymentModeId = 0;

		private int storeId = 0;

		private string deliveryTime = "任意时间";

		private ShoppingCartInfo shoppingCart;

		private int combinaid;

		private int fightGroupActivityId = 0;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-OrderStoresSet.html";
			}
			string text = "";
			this.from = this.Page.Request.QueryString["from"].ToNullString().ToLower();
			this.productSku = this.Page.Request.QueryString["productSku"].ToNullString();
			this.buyAmount = this.Page.Request.QueryString["buyAmount"].ToInt(0);
			this.shippingModeId = this.Page.Request.QueryString["ShippingModeId"].ToInt(0);
			this.storeId = this.Page.Request.QueryString["ChooseStoreId"].ToInt(0);
			this.paymentModeId = this.Page.Request.QueryString["paymentModeId"].ToInt(0);
			this.deliveryTime = this.Page.Request.QueryString["deliveryTime"].ToNullString();
			this.fightGroupActivityId = this.Page.Request.QueryString["fightGroupActivityId"].ToInt(0);
			if (this.deliveryTime != "任意时间" && this.deliveryTime != "工作日" && this.deliveryTime != "节假日")
			{
				this.deliveryTime = "任意时间";
			}
			if (this.shippingModeId != 0 && this.shippingModeId != -2)
			{
				this.shippingModeId = 0;
			}
			this.productSku = this.Page.Request.QueryString["productSku"].ToNullString();
			if (string.IsNullOrEmpty(this.productSku))
			{
				this.productSku = this.Page.Request.QueryString["ckids"].ToNullString();
			}
			if (!string.IsNullOrEmpty(this.productSku))
			{
				string[] array = this.productSku.Split(',');
				for (int i = 0; i < array.Length; i++)
				{
					string[] array2 = array[i].Split('|');
					if (!string.IsNullOrEmpty(this.newProductSku))
					{
						this.newProductSku += ",";
					}
					this.newProductSku += array2[0];
				}
			}
			this.shoppingCart = ShoppingCartProcessor.GetShoppingCart(this.from, this.productSku, this.buyAmount, 0, true, this.storeId, this.fightGroupActivityId);
			if (this.shoppingCart == null)
			{
				base.GotoResourceNotFound("购物车中没有任何商品");
			}
			else
			{
				base.OnInit(e);
			}
		}

		protected override void AttachChildControls()
		{
			this.inputPaymentModeId = (HtmlInputHidden)this.FindControl("inputPaymentModeId");
			this.inputShippingModeId = (HtmlInputHidden)this.FindControl("inputShippingModeId");
			this.hidPaymentId_Podrequest = (HtmlInputHidden)this.FindControl("hidPaymentId_Podrequest");
			this.hidGetgoodsOnStores = (HtmlInputHidden)this.FindControl("hidGetgoodsOnStores");
			this.hidDeliveryTime = (HtmlInputHidden)this.FindControl("hidDeliveryTime");
			this.hidStoreId = (HtmlInputHidden)this.FindControl("hidStoreId");
			this.hidShipAddressId = (HtmlInputHidden)this.FindControl("hidShipAddressId");
			this.rptStores = (AppshopTemplatedRepeater)this.FindControl("rptStores");
			if (!this.Page.IsPostBack)
			{
				this.hidGetgoodsOnStores.Value = "false";
				this.hidPaymentId_Podrequest.Value = "0";
				int num = 0;
				if (this.from != "countdown" && this.from != "groupbuy" && SalesHelper.IsSupportPodrequest())
				{
					num = 1;
					this.hidPaymentId_Podrequest.Value = "1";
				}
				if (this.paymentModeId != 0 && this.paymentModeId != num && this.paymentModeId != -3)
				{
					this.paymentModeId = 0;
				}
				this.hidDeliveryTime.Value = this.deliveryTime;
				this.inputPaymentModeId.Value = this.paymentModeId.ToString();
				this.inputShippingModeId.Value = this.shippingModeId.ToString();
				this.hidStoreId.Value = this.storeId.ToString();
				int shipAddressId = 0;
				int.TryParse(this.Page.Request.QueryString["ShipAddressId"].ToNullString(), out shipAddressId);
				IList<ShippingAddressInfo> shippingAddresses = MemberProcessor.GetShippingAddresses(false);
				int num2 = 0;
				IList<ShippingAddressInfo> list = new List<ShippingAddressInfo>();
				ShippingAddressInfo shippingAddressInfo = null;
				string address = "";
				if (shipAddressId > 0)
				{
					shippingAddressInfo = shippingAddresses.FirstOrDefault((ShippingAddressInfo a) => a.ShippingId == shipAddressId);
					if (shippingAddressInfo != null)
					{
						num2 = shippingAddressInfo.RegionId;
						address = shippingAddressInfo.FullAddress;
					}
				}
				else if (shippingAddresses != null && shippingAddresses.Count > 0)
				{
					num2 = shippingAddresses.FirstOrDefault().RegionId;
				}
				this.hidShipAddressId.Value = shipAddressId.ToString();
				StoresQuery storesQuery = new StoresQuery();
				storesQuery.RegionID = num2;
				storesQuery.RegionName = RegionHelper.GetFullRegion(num2, " ", true, 0);
				storesQuery.State = 1;
				storesQuery.CloseStatus = 1;
				DataTable storeList = StoresHelper.GetStoreList(this.newProductSku, num2, address, this.buyAmount);
				this.rptStores.DataSource = storeList;
				this.rptStores.DataBind();
				PageTitle.AddSiteNameTitle("门店选择");
			}
		}
	}
}
