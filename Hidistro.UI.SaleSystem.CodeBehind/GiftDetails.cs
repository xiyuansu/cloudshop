using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Shopping;
using Hidistro.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class GiftDetails : HtmlTemplatedWebControl
	{
		private int giftId;

		private bool isexemptionpostage;

		private Literal litGiftTite;

		private Literal litGiftName;

		private FormatedMoneyLabel lblMarkerPrice;

		private Label litNeedPoint;

		private Label litCurrentPoint;

		private Literal litShortDescription;

		private Literal litDescription;

		private HiImage imgGiftImage;

		private Button btnChage;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-GiftDetails.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			if (!int.TryParse(base.GetParameter("giftId", false), out this.giftId))
			{
				base.GotoResourceNotFound();
			}
			this.litGiftTite = (Literal)this.FindControl("litGiftTite");
			this.litGiftName = (Literal)this.FindControl("litGiftName");
			this.lblMarkerPrice = (FormatedMoneyLabel)this.FindControl("lblMarkerPrice");
			this.litNeedPoint = (Label)this.FindControl("litNeedPoint");
			this.litCurrentPoint = (Label)this.FindControl("litCurrentPoint");
			this.litShortDescription = (Literal)this.FindControl("litShortDescription");
			this.litDescription = (Literal)this.FindControl("litDescription");
			this.imgGiftImage = (HiImage)this.FindControl("imgGiftImage");
			this.btnChage = (Button)this.FindControl("btnChage");
			this.btnChage.Click += this.btnChage_Click;
			GiftInfo gift = ProductBrowser.GetGift(this.giftId);
			if (gift == null)
			{
				this.Page.Response.Redirect("/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("该件礼品已经不再参与积分兑换；或被管理员删除"));
			}
			else if (!gift.IsPointExchange)
			{
				this.Page.Response.Redirect("/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("该件礼品不再参与积分兑换"));
			}
			else
			{
				this.isexemptionpostage = gift.IsExemptionPostage;
				int num;
				if (!this.Page.IsPostBack)
				{
					this.litGiftName.Text = gift.Name;
					this.lblMarkerPrice.Money = gift.MarketPrice;
					Label label = this.litNeedPoint;
					num = gift.NeedPoint;
					label.Text = num.ToString();
					this.litShortDescription.Text = gift.ShortDescription;
					if (!string.IsNullOrEmpty(gift.LongDescription))
					{
						this.litDescription.Text = gift.LongDescription.Replace("src", "data-url");
					}
					this.imgGiftImage.ImageUrl = gift.ThumbnailUrl310;
					this.LoadPageSearch(gift);
				}
				if (HiContext.Current.UserId != 0 && gift.NeedPoint > 0)
				{
					this.btnChage.Enabled = true;
					this.btnChage.Text = "立即兑换";
					Label label2 = this.litCurrentPoint;
					num = HiContext.Current.User.Points;
					label2.Text = num.ToString();
				}
				else if (gift.NeedPoint <= 0)
				{
					this.btnChage.Enabled = false;
					this.btnChage.Text = "礼品不允许兑换";
				}
				else
				{
					this.btnChage.Text = "请登录方能兑换";
				}
			}
		}

		private void btnChage_Click(object sender, EventArgs e)
		{
			if (this.btnChage.Text == "请登录方能兑换")
			{
				this.Page.Response.Redirect("/User/Login.aspx?ReturnUrl=" + HttpContext.Current.Request.RawUrl);
			}
			else if (HiContext.Current.User == null)
			{
				this.Page.Response.Redirect("/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("请登录后才能购买"));
			}
			else if (int.Parse(this.litNeedPoint.Text) <= int.Parse(this.litCurrentPoint.Text))
			{
				ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart(null, false, false, -1);
				if (shoppingCart != null && shoppingCart.LineGifts != null && shoppingCart.LineGifts.Count > 0)
				{
					foreach (ShoppingCartGiftInfo lineGift in shoppingCart.LineGifts)
					{
						if (lineGift.GiftId == this.giftId)
						{
							string str = string.Format("ShowMsg(\"{0}\", false);", "购物车中已存在该礼品，请删除购物车中已有的礼品或者下次兑换！");
							if (!this.Page.ClientScript.IsClientScriptBlockRegistered("ServerMessageScript"))
							{
								this.Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScript", "<script language='JavaScript' defer='defer'>setTimeout(function(){" + str + "},300);</script>");
							}
							return;
						}
					}
				}
				if (ShoppingCartProcessor.AddGiftItem(this.giftId, 1, PromoteType.NotSet, this.isexemptionpostage))
				{
					this.Page.Response.Redirect("/ShoppingCart", true);
				}
			}
		}

		private void LoadPageSearch(GiftInfo gift)
		{
			if (!string.IsNullOrEmpty(gift.Meta_Keywords))
			{
				MetaTags.AddMetaKeywords(gift.Meta_Keywords, HiContext.Current.Context);
			}
			if (!string.IsNullOrEmpty(gift.Meta_Description))
			{
				MetaTags.AddMetaDescription(gift.Meta_Description, HiContext.Current.Context);
			}
			if (!string.IsNullOrEmpty(gift.Title))
			{
				PageTitle.AddSiteNameTitle(gift.Title);
			}
			else
			{
				PageTitle.AddSiteNameTitle(gift.Name);
			}
		}
	}
}
