using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Shopping;
using Hidistro.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WAPPointInfo : WAPTemplatedWebControl
	{
		private int giftId;

		private bool isExemptionPostage;

		private HiImage imgGift;

		private Literal litName;

		private Literal litMarketPrice;

		private Literal litNeedPoints;

		private Literal litHasPoints;

		private Literal litMeta_Description;

		private ImageLinkButton btnClearCart;

		private HtmlInputHidden hidGiftId;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-PointInfo.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			if (!int.TryParse(this.Page.Request.QueryString["GiftId"], out this.giftId))
			{
				base.GotoResourceNotFound("");
			}
			this.imgGift = (HiImage)this.FindControl("imgGift");
			this.litName = (Literal)this.FindControl("litName");
			this.litMarketPrice = (Literal)this.FindControl("litMarketPrice");
			this.litNeedPoints = (Literal)this.FindControl("litNeedPoints");
			this.litHasPoints = (Literal)this.FindControl("litHasPoints");
			this.litMeta_Description = (Literal)this.FindControl("litMeta_Description");
			this.hidGiftId = (HtmlInputHidden)this.FindControl("hidGiftId");
			this.btnClearCart = (ImageLinkButton)this.FindControl("btnClearCart");
			this.btnClearCart.Click += this.btnClearCart_Click;
			GiftInfo giftDetails = ProductBrowser.GetGiftDetails(this.giftId);
			if (giftDetails == null)
			{
				this.Page.Response.Redirect("ResourceNotFound?errorMsg=" + Globals.UrlEncode("该件礼品已经不再参与积分兑换；或被管理员删除"));
			}
			else
			{
				this.isExemptionPostage = giftDetails.IsExemptionPostage;
				this.imgGift.ImageUrl = giftDetails.ImageUrl;
				this.litName.Text = giftDetails.Name;
				this.litMarketPrice.Text = Math.Round((!giftDetails.MarketPrice.HasValue) ? decimal.Zero : giftDetails.MarketPrice.Value, 2).ToString();
				Literal literal = this.litNeedPoints;
				int num = giftDetails.NeedPoint;
				literal.Text = num.ToString();
				this.litMeta_Description.Text = giftDetails.LongDescription.ToNullString().Replace("src", "data-url");
				this.hidGiftId.Value = this.giftId.ToString();
				if (HiContext.Current.UserId != 0 && giftDetails.NeedPoint > 0)
				{
					if (HiContext.Current.User.Points < giftDetails.NeedPoint)
					{
						this.btnClearCart.Enabled = false;
						this.btnClearCart.Text = "无法兑换";
					}
					else
					{
						this.btnClearCart.Enabled = true;
						this.btnClearCart.Text = "立即兑换";
					}
					Literal literal2 = this.litHasPoints;
					num = HiContext.Current.User.Points;
					literal2.Text = num.ToString();
				}
				else if (giftDetails.NeedPoint <= 0)
				{
					this.btnClearCart.Enabled = false;
					this.btnClearCart.Text = "礼品不允许兑换";
				}
				else
				{
					this.btnClearCart.Text = "请登录方能兑换";
				}
				if (!giftDetails.IsPointExchange)
				{
					this.btnClearCart.Enabled = false;
					this.btnClearCart.Text = "该件礼品不再参与积分兑换";
				}
				PageTitle.AddSiteNameTitle("积分商城");
			}
		}

		protected void btnClearCart_Click(object sender, EventArgs e)
		{
			if (this.btnClearCart.Text == "请登录方能兑换" || HiContext.Current.User == null)
			{
				this.Page.Response.Redirect("Login.aspx?ReturnUrl=" + HttpContext.Current.Request.RawUrl, true);
			}
			else if (int.Parse(this.litNeedPoints.Text) <= int.Parse(this.litHasPoints.Text))
			{
				ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart(null, false, false, -1);
				if (shoppingCart != null && shoppingCart.LineGifts != null && shoppingCart.LineGifts.Count > 0)
				{
					foreach (ShoppingCartGiftInfo lineGift in shoppingCart.LineGifts)
					{
						if (lineGift.GiftId == this.giftId)
						{
							this.Page.ClientScript.RegisterStartupScript(base.GetType(), "myscript", "<script>$(function(){alert_h(\"购物车中已存在该礼品，请删除购物车中已有的礼品或者下次兑换！\")});</script>");
							return;
						}
					}
				}
				if (ShoppingCartProcessor.AddGiftItem(this.giftId, 1, PromoteType.NotSet, this.isExemptionPostage))
				{
					this.Page.Response.Redirect("/WapShop/ShoppingCart", true);
				}
			}
		}
	}
}
