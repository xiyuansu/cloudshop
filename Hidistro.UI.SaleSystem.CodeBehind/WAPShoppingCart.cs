using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Supplier;
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
	public class WAPShoppingCart : WAPMemberTemplatedWebControl
	{
		private WapTemplatedRepeater rptCartGifts;

		private Literal litTotal;

		private Repeater rp_guest;

		private HtmlInputHidden hidUserPoints;

		private HtmlInputHidden hidIsOpenStore;

		private Repeater rptSupplier;

		private SiteSettings setting;

		private HtmlGenericControl cartProducts;

		private int invalidSupplierId = -1;

		private List<SupplierInfo> listsupplier;

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
				this.SkinName = "Skin-VShoppingCart.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.setting = SettingsManager.GetMasterSettings();
			if (this.setting.OpenMultStore)
			{
				this.Page.Response.Redirect("StoreShoppingCart.aspx", true);
			}
			this.rptCartGifts = (WapTemplatedRepeater)this.FindControl("rptCartGifts");
			this.litTotal = (Literal)this.FindControl("litTotal");
			this.rp_guest = (Repeater)this.FindControl("rp_guest");
			this.hidUserPoints = (HtmlInputHidden)this.FindControl("hidUserPoints");
			this.hidIsOpenStore = (HtmlInputHidden)this.FindControl("hidIsOpenStore");
			this.cartProducts = (HtmlGenericControl)this.FindControl("cartProducts");
			this.rptSupplier = (Repeater)this.FindControl("rptSupplier");
			ShoppingCartInfo mobileShoppingCart = ShoppingCartProcessor.GetMobileShoppingCart(null, false, true, -1);
			if (mobileShoppingCart != null)
			{
				this.ListCartItems = mobileShoppingCart.LineItems;
				IOrderedEnumerable<ShoppingCartItemInfo> orderedEnumerable = from x in this.listCartItems
				where (!x.IsValid || !x.HasEnoughStock) && x.StoreId == 0
				orderby x.IsValid descending
				select x;
				if (orderedEnumerable != null && orderedEnumerable.Count() > 0)
				{
					this.invalidSupplierId = this.listCartItems.Max((ShoppingCartItemInfo x) => x.SupplierId) + 100;
					foreach (ShoppingCartItemInfo item in orderedEnumerable)
					{
						item.SupplierId = this.invalidSupplierId;
						item.SupplierName = "";
					}
				}
				var orderedEnumerable2 = from x in this.listCartItems
				where x.IsValid && x.HasEnoughStock && x.StoreId == 0
				group x by new
				{
					x.SupplierId,
					x.SupplierName
				} into x
				orderby x.Key.SupplierId
				select x;
				if (this.listsupplier == null)
				{
					this.listsupplier = new List<SupplierInfo>();
				}
				foreach (var item2 in orderedEnumerable2)
				{
					SupplierInfo supplierInfo = new SupplierInfo();
					supplierInfo.SupplierId = item2.Key.SupplierId;
					supplierInfo.SupplierName = item2.Key.SupplierName;
					this.listsupplier.Add(supplierInfo);
				}
				this.rptSupplier.ItemDataBound += this.rptSupplier_ItemDataBound;
				this.rptSupplier.DataSource = orderedEnumerable2;
				this.rptSupplier.DataBind();
				WapTemplatedRepeater wapTemplatedRepeater = (WapTemplatedRepeater)this.FindControl("rptCartProducts_Invalid");
				if (wapTemplatedRepeater != null)
				{
					wapTemplatedRepeater.ItemDataBound += this.rptCartProducts_ItemDataBound;
					wapTemplatedRepeater.DataSource = orderedEnumerable;
					wapTemplatedRepeater.DataBind();
				}
				this.litTotal.Text = "0.00";
				if (mobileShoppingCart.LineGifts.Count > 0)
				{
					IEnumerable<ShoppingCartGiftInfo> dataSource = from s in mobileShoppingCart.LineGifts
					where s.PromoType == 0
					select s;
					this.rptCartGifts.DataSource = dataSource;
					this.rptCartGifts.DataBind();
				}
				if (mobileShoppingCart.LineItems.Count > 0 || mobileShoppingCart.LineGifts.Count > 0)
				{
					IEnumerable<ShoppingCartItemInfo> source = from x in mobileShoppingCart.LineItems
					where x.StoreId == 0
					select x;
					IEnumerable<ShoppingCartGiftInfo> source2 = from s in mobileShoppingCart.LineGifts
					where s.PromoType == 0
					select s;
					if (source.Count() > 0 || source2.Count() > 0)
					{
						this.cartProducts.Visible = true;
					}
				}
			}
			if (this.rp_guest != null)
			{
				IList<int> browedProductList = BrowsedProductQueue.GetBrowedProductList(10);
				this.rp_guest.DataSource = ProductBrowser.GetVistiedProducts(browedProductList);
				this.rp_guest.DataBind();
			}
			if (HiContext.Current.UserId > 0)
			{
				this.hidUserPoints.Value = HiContext.Current.User.Points.ToString();
			}
			this.hidIsOpenStore.Value = (this.setting.IsOpenPickeupInStore ? "true" : "false");
			PageTitle.AddSiteNameTitle("购物车");
			Repeater repeater = new Repeater();
		}

		protected void rptSupplier_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			int itemIndex = e.Item.ItemIndex;
			Literal literal = e.Item.FindControl("ltsupplierName") as Literal;
			HtmlGenericControl htmlGenericControl = e.Item.FindControl("divSupplier") as HtmlGenericControl;
			if (literal != null && this.listsupplier.Count > e.Item.ItemIndex)
			{
				SupplierInfo supplier = this.listsupplier[e.Item.ItemIndex];
				string text = supplier.SupplierName;
				string arg = "stitle";
				if (supplier.SupplierId == 0)
				{
					text = (string.IsNullOrEmpty(text) ? "平台" : text);
					arg = "ztitle";
				}
				literal.Text = $"<span class=\"{arg}\">{text}</span>";
				if (supplier.SupplierId == this.invalidSupplierId || !this.setting.OpenSupplier)
				{
					literal.Visible = false;
					htmlGenericControl.Visible = false;
				}
				WapTemplatedRepeater wapTemplatedRepeater = e.Item.FindControl("rptCartProducts") as WapTemplatedRepeater;
				if (wapTemplatedRepeater != null && this.ListCartItems != null)
				{
					wapTemplatedRepeater.ItemDataBound += this.rptCartProducts_ItemDataBound;
					IEnumerable<ShoppingCartItemInfo> source = from x in this.listCartItems
					where x.SupplierId == supplier.SupplierId && x.StoreId == 0
					select x;
					Func<ShoppingCartItemInfo, bool> keySelector = (ShoppingCartItemInfo x) => x.IsValid;
					IOrderedEnumerable<ShoppingCartItemInfo> orderedEnumerable2 = (IOrderedEnumerable<ShoppingCartItemInfo>)(wapTemplatedRepeater.DataSource = source.OrderByDescending(keySelector));
					wapTemplatedRepeater.DataBind();
				}
			}
		}

		private void rptCartProducts_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (HiContext.Current.User.UserId != 0 && (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem))
			{
				Control control = e.Item.Controls[0];
				Literal literal = control.FindControl("ltlSKUContent") as Literal;
				Repeater repeater = control.FindControl("repProductGifts") as Repeater;
				Control control2 = control.FindControl("divChangeAmount");
				Control control3 = control.FindControl("ck_productId");
				Control control4 = control.FindControl("lblNoStock");
				Control control5 = control.FindControl("lblInValid");
				Control control6 = control.FindControl("divQuantity");
				ShoppingCartItemInfo itemInfo = e.Item.DataItem as ShoppingCartItemInfo;
				if (!itemInfo.IsValid)
				{
					control2.Visible = false;
					control6.Visible = true;
					control3.Visible = false;
					control4.Visible = false;
					control5.Visible = true;
				}
				else
				{
					control5.Visible = false;
					string[] array = itemInfo.SkuContent.Split(';');
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
					int skuStock = ShoppingCartProcessor.GetSkuStock(itemInfo.SkuId, 0);
					if (skuStock < itemInfo.Quantity || skuStock <= 0)
					{
						control2.Visible = false;
						control3.Visible = false;
						control4.Visible = true;
						control6.Visible = true;
					}
					else
					{
						control4.Visible = false;
						control6.Visible = false;
					}
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

		protected void btnClearCart_Click(object sender, EventArgs e)
		{
			string text = this.Page.Request.Form["ck_productId"];
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMessage("请选择要清除的商品", false, "", 1);
			}
			else
			{
				string[] array = text.Split(',');
				foreach (string skuId in array)
				{
					ShoppingCartProcessor.RemoveLineItem(skuId, 0);
				}
			}
			this.Page.Response.Redirect("ShoppingCart.aspx", true);
		}
	}
}
