using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Shopping;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WAPStoreShoppingCart : WAPMemberTemplatedWebControl
	{
		private WapTemplatedRepeater rptCartStore;

		private WapTemplatedRepeater rptCartGifts;

		private WapTemplatedRepeater rptCartInvalid;

		private SiteSettings setting;

		private HtmlGenericControl cartProducts;

		private HtmlInputHidden hidUserPoints;

		private List<StoresInfo> liststore;

		private int nowStoreId = -1;

		private IList<ShoppingCartItemInfo> listCartItems;

		public IList<ShoppingCartItemInfo> ListCartItems
		{
			get
			{
				return this.listCartItems;
			}
			set
			{
				this.listCartItems = value;
			}
		}

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-StoreShoppingCart.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.rptCartStore = (WapTemplatedRepeater)this.FindControl("rptCartStore");
			this.rptCartGifts = (WapTemplatedRepeater)this.FindControl("rptCartGifts");
			this.rptCartInvalid = (WapTemplatedRepeater)this.FindControl("rptCartInvalid");
			this.cartProducts = (HtmlGenericControl)this.FindControl("cartProducts");
			this.hidUserPoints = (HtmlInputHidden)this.FindControl("hidUserPoints");
			ShoppingCartInfo mobileShoppingCart = ShoppingCartProcessor.GetMobileShoppingCart(null, false, true, -1);
			if (mobileShoppingCart != null)
			{
				this.ListCartItems = mobileShoppingCart.LineItems;
				IEnumerable<ShoppingCartItemInfo> enumerable = from x in this.listCartItems
				where x.IsValid && x.HasEnoughStock
				select x;
				if (this.liststore == null)
				{
					this.liststore = new List<StoresInfo>();
				}
				foreach (ShoppingCartItemInfo item in enumerable)
				{
					StoresInfo storesInfo = new StoresInfo();
					storesInfo.StoreId = item.StoreId;
					storesInfo.StoreName = item.StoreName;
					this.liststore.Add(storesInfo);
				}
				this.rptCartStore.ItemDataBound += this.rptCartStore_ItemDataBound;
				this.rptCartStore.DataSource = enumerable;
				this.rptCartStore.DataBind();
				if (mobileShoppingCart.LineGifts.Count > 0)
				{
					IEnumerable<ShoppingCartGiftInfo> dataSource = from s in mobileShoppingCart.LineGifts
					where s.PromoType == 0
					select s;
					this.rptCartGifts.DataSource = dataSource;
					this.rptCartGifts.DataBind();
				}
				IOrderedEnumerable<ShoppingCartItemInfo> dataSource2 = from x in this.listCartItems
				where !x.IsValid || !x.HasEnoughStock
				orderby x.IsValid descending
				select x;
				this.rptCartInvalid.ItemDataBound += this.rptCartInvalid_ItemDataBound;
				this.rptCartInvalid.DataSource = dataSource2;
				this.rptCartInvalid.DataBind();
				if (mobileShoppingCart.LineItems.Count > 0 || mobileShoppingCart.LineGifts.Count > 0)
				{
					this.cartProducts.Visible = true;
				}
			}
			if (HiContext.Current.UserId > 0)
			{
				this.hidUserPoints.Value = HiContext.Current.User.Points.ToString();
			}
		}

		protected void rptCartStore_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				Control control = e.Item.Controls[0];
				Literal literal = control.FindControl("ltlTop") as Literal;
				Literal literal2 = control.FindControl("ltlBottom") as Literal;
				Control control2 = control.FindControl("divStoreTitle");
				Literal literal3 = control.FindControl("ltlSKUContent") as Literal;
				Repeater repeater = control.FindControl("repProductGifts") as Repeater;
				string[] array = literal3.Text.Split(';');
				string text = string.Empty;
				string[] array2 = array;
				foreach (string text2 in array2)
				{
					string[] array3 = text2.Split('：');
					if (array3.Length == 2)
					{
						text = text + " " + array3[1] + " /";
					}
				}
				string text3 = text;
				char[] trimChars = new char[1]
				{
					'/'
				};
				text = (literal3.Text = text3.TrimEnd(trimChars));
				ShoppingCartItemInfo itemInfo = e.Item.DataItem as ShoppingCartItemInfo;
				int itemIndex = e.Item.ItemIndex;
				if (itemInfo.StoreId == this.nowStoreId)
				{
					literal.Visible = false;
					control2.Visible = false;
				}
				else
				{
					literal.Visible = true;
					control2.Visible = true;
					this.nowStoreId = itemInfo.StoreId;
				}
				if (this.liststore.Count - 1 == itemIndex || this.liststore[itemIndex + 1].StoreId != itemInfo.StoreId)
				{
					literal2.Visible = true;
				}
				else
				{
					literal2.Visible = false;
				}
				PromotionInfo productPromotionInfo = ProductBrowser.GetProductPromotionInfo(itemInfo.ProductId);
				if (productPromotionInfo != null && productPromotionInfo.PromoteType == PromoteType.SentGift)
				{
					List<ShoppingCartGiftInfo> cartGiftList = new List<ShoppingCartGiftInfo>();
					IList<GiftInfo> giftDetailsByGiftIds = ProductBrowser.GetGiftDetailsByGiftIds(productPromotionInfo.GiftIds);
					giftDetailsByGiftIds.ForEach(delegate(GiftInfo gift)
					{
						ShoppingCartGiftInfo shoppingCartGiftInfo = new ShoppingCartGiftInfo();
						shoppingCartGiftInfo.GiftId = gift.GiftId;
						shoppingCartGiftInfo.CostPrice = (gift.CostPrice.HasValue ? gift.CostPrice.Value : decimal.Zero);
						shoppingCartGiftInfo.PromoType = 5;
						shoppingCartGiftInfo.Quantity = itemInfo.ShippQuantity;
						shoppingCartGiftInfo.Weight = gift.Weight;
						shoppingCartGiftInfo.Volume = gift.Volume;
						shoppingCartGiftInfo.NeedPoint = gift.NeedPoint;
						shoppingCartGiftInfo.Name = gift.Name;
						shoppingCartGiftInfo.ThumbnailUrl100 = gift.ThumbnailUrl100;
						shoppingCartGiftInfo.ThumbnailUrl180 = gift.ThumbnailUrl180;
						shoppingCartGiftInfo.ThumbnailUrl40 = gift.ThumbnailUrl40;
						shoppingCartGiftInfo.ThumbnailUrl60 = gift.ThumbnailUrl60;
						shoppingCartGiftInfo.IsExemptionPostage = gift.IsExemptionPostage;
						shoppingCartGiftInfo.ShippingTemplateId = gift.ShippingTemplateId;
						cartGiftList.Add(shoppingCartGiftInfo);
					});
					repeater.DataSource = cartGiftList;
					repeater.DataBind();
				}
			}
		}

		protected void rptCartInvalid_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				Control control = e.Item.Controls[0];
				Literal literal = control.FindControl("ltlSKUContent") as Literal;
				string[] array = literal.Text.Split(';');
				string text = string.Empty;
				string[] array2 = array;
				foreach (string text2 in array2)
				{
					string[] array3 = text2.Split('：');
					if (array3.Length == 2)
					{
						text = text + " " + array3[1] + " /";
					}
				}
				string text3 = text;
				char[] trimChars = new char[1]
				{
					'/'
				};
				text = (literal.Text = text3.TrimEnd(trimChars));
			}
		}
	}
}
