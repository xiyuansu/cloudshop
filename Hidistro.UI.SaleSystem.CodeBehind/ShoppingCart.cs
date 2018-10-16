using Hidistro.Context;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Shopping;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class ShoppingCart : HtmlTemplatedWebControl
	{
		private TextBox txtSKU;

		private Button btnSKU;

		private Common_ShoppingCart_ProductList shoppingCartProductList;

		private Common_ShoppingCart_GiftList shoppingCartGiftList;

		private Common_ShoppingCart_StoreList shoppingCartStoreList;

		private FormatedMoneyLabel lblTotalPrice;

		private Button btnCheckout;

		private HtmlInputButton btnUnCheckout;

		private Panel pnlShopCart;

		private Panel pnlPromoGift;

		private Panel pnlNoProduct;

		private ShoppingCartInfo cartInfo;

		private HiddenField hfdIsLogin;

		private HiddenField hidCurrentPoints;

		private HtmlInputHidden hidPointGifts;

		private SiteSettings setting = SettingsManager.GetMasterSettings();

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-ShoppingCart.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.txtSKU = (TextBox)this.FindControl("txtSKU");
			this.btnSKU = (Button)this.FindControl("btnSKU");
			this.shoppingCartProductList = (Common_ShoppingCart_ProductList)this.FindControl("Common_ShoppingCart_ProductList");
			this.shoppingCartGiftList = (Common_ShoppingCart_GiftList)this.FindControl("Common_ShoppingCart_GiftList");
			this.shoppingCartStoreList = (Common_ShoppingCart_StoreList)this.FindControl("Common_ShoppingCart_StoreList");
			this.lblTotalPrice = (FormatedMoneyLabel)this.FindControl("lblTotalPrice");
			this.btnCheckout = (Button)this.FindControl("btnCheckout");
			this.btnUnCheckout = (HtmlInputButton)this.FindControl("btnUnCheckout");
			this.pnlShopCart = (Panel)this.FindControl("pnlShopCart");
			this.pnlPromoGift = (Panel)this.FindControl("pnlPromoGift");
			this.pnlNoProduct = (Panel)this.FindControl("pnlNoProduct");
			this.hfdIsLogin = (HiddenField)this.FindControl("hfdIsLogin");
			this.hidCurrentPoints = (HiddenField)this.FindControl("hidCurrentPoints");
			this.btnSKU.Click += this.btnSKU_Click;
			this.shoppingCartProductList.ItemDataBound += this.shoppingCartProductList_ItemDataBound;
			this.hidPointGifts = (HtmlInputHidden)this.FindControl("hidPointGifts");
			this.shoppingCartGiftList.ItemCommand += this.shoppingCartGiftList_ItemCommand;
			this.btnCheckout.Click += this.btnCheckout_Click;
			this.BindShoppingCart();
			if (HiContext.Current.UserId != 0)
			{
				this.hfdIsLogin.Value = "logined";
				this.hidCurrentPoints.Value = HiContext.Current.User.Points.ToString();
			}
		}

		private void shoppingCartProductList_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				TextBox textBox = (TextBox)e.Item.FindControl("txtBuyNum");
				Control control = e.Item.FindControl("spanStock");
				Control control2 = e.Item.FindControl("divAmount");
				Control control3 = e.Item.FindControl("divCheck");
				Control control4 = e.Item.FindControl("divNoCheck");
				Control control5 = e.Item.FindControl("divInValid");
				Control control6 = e.Item.FindControl("divIsStock");
				Control control7 = e.Item.FindControl("divValidInfo");
				if (HiContext.Current.User.UserId == 0)
				{
					control5.Visible = false;
					control6.Visible = false;
					control4.Visible = false;
					control7.Visible = false;
				}
				else
				{
					Repeater repeater = e.Item.FindControl("rptPromotionGifts") as Repeater;
					ShoppingCartItemInfo shoppingCartItemInfo = e.Item.DataItem as ShoppingCartItemInfo;
					PromotionInfo productPromotionInfo = ProductBrowser.GetProductPromotionInfo(shoppingCartItemInfo.ProductId);
					if (productPromotionInfo != null && productPromotionInfo.PromoteType == PromoteType.SentGift)
					{
						IList<GiftInfo> list = (IList<GiftInfo>)(repeater.DataSource = ProductBrowser.GetGiftDetailsByGiftIds(productPromotionInfo.GiftIds));
						repeater.DataBind();
					}
					if (!shoppingCartItemInfo.IsValid)
					{
						control3.Visible = false;
						control5.Visible = true;
						control7.Visible = true;
						control6.Visible = false;
						control2.Visible = false;
					}
					else
					{
						control5.Visible = false;
						control7.Visible = false;
						control6.Visible = false;
						int num = default(int);
						int.TryParse(textBox.Text, out num);
						int skuStock = ShoppingCartProcessor.GetSkuStock(shoppingCartItemInfo.SkuId, 0);
						if (skuStock < num || skuStock <= 0)
						{
							control.Visible = true;
							control2.Visible = false;
							control3.Visible = false;
							control4.Visible = true;
							control6.Visible = true;
							this.btnCheckout.Style.Add(HtmlTextWriterStyle.Display, "none");
							this.btnUnCheckout.Visible = true;
						}
						else
						{
							control4.Visible = false;
						}
					}
				}
			}
		}

		protected void btnSKU_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(this.txtSKU.Text.Trim()))
			{
				this.ShowMessage("请输入货号", false, "", 1);
			}
			else
			{
				IList<string> skuIdsBysku = ShoppingProcessor.GetSkuIdsBysku(this.txtSKU.Text.Trim());
				if (skuIdsBysku == null || skuIdsBysku.Count == 0)
				{
					this.ShowMessage("货号无效，请确认后重试", false, "", 1);
				}
				else
				{
					bool flag = false;
					foreach (string item in skuIdsBysku)
					{
						if (ShoppingCartProcessor.GetSkuStock(item, 0) > 1)
						{
							ShoppingCartProcessor.AddLineItem(item, 1, false, 0);
						}
						else
						{
							flag = true;
						}
					}
					if (flag)
					{
						this.ShowMessage("指定的货号库存不足", false, "", 1);
					}
					this.BindShoppingCart();
				}
			}
		}

		protected void shoppingCartGiftList_FreeItemCommand(object sender, DataListCommandEventArgs e)
		{
			if (e.CommandName == "delete")
			{
				string text = e.CommandArgument.ToString();
				int giftId = 0;
				if (!string.IsNullOrEmpty(text) && int.TryParse(text, out giftId))
				{
					ShoppingCartProcessor.RemoveGiftItem(giftId, PromoteType.SentGift);
				}
				this.Page.Response.Redirect("/ShoppingCart", true);
			}
		}

		protected void shoppingCartGiftList_ItemCommand(object sender, DataListCommandEventArgs e)
		{
			Control control = e.Item.Controls[0];
			TextBox textBox = (TextBox)control.FindControl("txtBuyNum");
			Literal literal = (Literal)control.FindControl("litGiftId");
			Literal literal2 = (Literal)control.FindControl("lblPointTotal");
			HiddenField hiddenField = (HiddenField)control.FindControl("hidOldBuyNum");
			int num = default(int);
			if (!int.TryParse(textBox.Text, out num) || textBox.Text.IndexOf(".") != -1)
			{
				this.ShowMessage("兑换数量必须为整数", false, "", 1);
				textBox.Text = hiddenField.Value;
			}
			else if (num <= 0)
			{
				this.ShowMessage("兑换数量必须为大于0的整数", false, "", 1);
				textBox.Text = hiddenField.Value;
			}
			else
			{
				if (e.CommandName == "updateBuyNum")
				{
					ShoppingCartProcessor.UpdateGiftItemQuantity(Convert.ToInt32(literal.Text), num, PromoteType.NotSet);
				}
				if (e.CommandName == "delete")
				{
					ShoppingCartProcessor.RemoveGiftItem(Convert.ToInt32(literal.Text), PromoteType.NotSet);
				}
				this.Page.Response.Redirect("/ShoppingCart", true);
			}
		}

		protected void btnCheckout_Click(object sender, EventArgs e)
		{
			string text = this.Page.Request.Form["ck_productId"];
			if (!this.CheckProductStock(text))
			{
				this.ShowMessage("购物车中有商品库存不足，无法结算", false, "", 1);
			}
			else
			{
				HttpCookie cookie = new HttpCookie("ckids", text);
				HiContext.Current.Context.Response.Cookies.Add(cookie);
				HiContext.Current.Context.Response.Redirect($"/submmitOrder?productSku={text}");
			}
		}

		protected bool CheckProductStock(string ckproductids)
		{
			ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart(null, false, true, -1);
			if (shoppingCart == null)
			{
				return false;
			}
			bool result = true;
			if (!string.IsNullOrEmpty(ckproductids))
			{
				string[] source = ckproductids.Split(',');
				foreach (ShoppingCartItemInfo lineItem in shoppingCart.LineItems)
				{
					if (source.Contains(lineItem.SkuId))
					{
						int skuStock = ShoppingCartProcessor.GetSkuStock(lineItem.SkuId, 0);
						if (skuStock < lineItem.Quantity)
						{
							result = false;
							break;
						}
					}
				}
			}
			return result;
		}

		private void BindShoppingCart()
		{
			this.cartInfo = ShoppingCartProcessor.GetShoppingCart(null, true, true, -1);
			if (this.cartInfo == null)
			{
				this.pnlShopCart.Visible = false;
				this.pnlNoProduct.Visible = true;
				ShoppingCartProcessor.ClearShoppingCart();
			}
			else
			{
				this.pnlShopCart.Visible = true;
				this.pnlNoProduct.Visible = false;
				if (this.cartInfo.LineItems.Count > 0)
				{
					this.shoppingCartProductList.ListCartItems = this.cartInfo.LineItems;
					this.shoppingCartProductList.settings = this.setting;
					this.shoppingCartProductList.DataBind();
					this.shoppingCartProductList.ShowProductCart();
					this.shoppingCartStoreList.ListCartItems = this.cartInfo.LineItems;
					this.shoppingCartStoreList.DataBind();
				}
				if (this.cartInfo.LineGifts.Count > 0)
				{
					IEnumerable<ShoppingCartGiftInfo> enumerable = from s in this.cartInfo.LineGifts
					where s.PromoType == 0
					select s;
					IEnumerable<ShoppingCartGiftInfo> enumerable2 = from s in this.cartInfo.LineGifts
					where s.PromoType == 5
					select s;
					this.hidPointGifts.Value = enumerable.Count().ToString();
					this.shoppingCartGiftList.DataSource = enumerable;
					this.shoppingCartGiftList.DataBind();
					this.shoppingCartGiftList.ShowGiftCart(enumerable.Count() > 0);
				}
				this.lblTotalPrice.Money = this.cartInfo.GetTotal(false);
			}
		}
	}
}
