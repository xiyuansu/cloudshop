using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_SKUSubmitOrder : WAPTemplatedWebControl
	{
		public enum EnumOrderBusiness
		{
			Product,
			FightGroup,
			CountDown,
			GroupBuy,
			PreSale
		}

		private Image imgSKUSubmitOrderProduct;

		private Label lblSKUSubmitOrderPrice;

		private Label lblSKUSubmitOrderStockNow;

		private Label lblSKUSubmitOrderPrePrice;

		private HtmlInputHidden hidden_SKUSubmitOrderProductId;

		private HtmlInputHidden hidden_SKUSubmitOrderFightGroupActivityId;

		private HtmlInputHidden hidden_SKUSubmitOrderFightGroupId;

		private HtmlInputHidden hidden_SKUSubmitOrderMinSalePriceSkuId;

		private HtmlInputHidden hidden_SKUSubmitOrderBusiness;

		private HtmlInputHidden hidden_SKUSubmitOrderSelectedSkuId;

		private HtmlInputHidden hidden_SKUSubmitOrderFightGroupActivityMinPrice;

		private HtmlInputHidden hidden_SKUSubmitOrderProductMinPrice;

		private HtmlInputHidden hidden_SKUSubmitOrderFightGroupActivityStock;

		private HtmlInputHidden hidden_SKUSubmitOrderProductStock;

		private HtmlInputHidden hidden_SKUSubmitOrderCountDownId;

		private HtmlInputHidden hidden_SKUSubmitOrderCountDownStock;

		private HtmlInputHidden hidden_SKUSubmitOrderCountDownMinPrice;

		private HtmlInputHidden hidden_SKUSubmitOrderGroupBuyId;

		private HtmlInputHidden hidden_SKUSubmitOrderGroupBuyStock;

		private HtmlInputHidden hidden_SKUSubmitOrderGroupBuyMinPrice;

		private HtmlInputHidden hidden_SKUSubmitOrderPreSaleId;

		private HtmlInputHidden hidden_SKUSubmitOrderDepositPercent;

		private Common_SKUSelector SKUSubmitOrderSelector;

		private Literal litMaxCount;

		private Literal litUnit;

		public ProductInfo ProductInfo
		{
			get;
			set;
		}

		public int FightGroupActivityId
		{
			get;
			set;
		}

		public int FightGroupId
		{
			get;
			set;
		}

		public int CountDownId
		{
			get;
			set;
		}

		public int GroupBuyId
		{
			get;
			set;
		}

		public int PreSaleId
		{
			get;
			set;
		}

		public GroupBuyInfo GroupBuyInfo
		{
			get;
			set;
		}

		public ProductPreSaleInfo productPreSaleInfo
		{
			get;
			set;
		}

		public DataTable GroupBuySkus
		{
			get;
			set;
		}

		public int GroupBuySoldCount
		{
			get;
			set;
		}

		public int OrderBusiness
		{
			get;
			set;
		}

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-SKUSubmitOrder.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.litMaxCount = (Literal)this.FindControl("litMaxCount");
			this.imgSKUSubmitOrderProduct = (Image)this.FindControl("imgSKUSubmitOrderProduct");
			this.lblSKUSubmitOrderPrice = (Label)this.FindControl("lblSKUSubmitOrderPrice");
			this.lblSKUSubmitOrderStockNow = (Label)this.FindControl("lblSKUSubmitOrderStockNow");
			this.lblSKUSubmitOrderPrePrice = (Label)this.FindControl("lblSKUSubmitOrderPrePrice");
			this.hidden_SKUSubmitOrderCountDownMinPrice = (HtmlInputHidden)this.FindControl("hidden_SKUSubmitOrderCountDownMinPrice");
			this.hidden_SKUSubmitOrderCountDownStock = (HtmlInputHidden)this.FindControl("hidden_SKUSubmitOrderCountDownStock");
			this.hidden_SKUSubmitOrderCountDownId = (HtmlInputHidden)this.FindControl("hidden_SKUSubmitOrderCountDownId");
			this.hidden_SKUSubmitOrderPreSaleId = (HtmlInputHidden)this.FindControl("hidden_SKUSubmitOrderPreSaleId");
			this.hidden_SKUSubmitOrderGroupBuyId = (HtmlInputHidden)this.FindControl("hidden_SKUSubmitOrderGroupBuyId");
			this.hidden_SKUSubmitOrderGroupBuyStock = (HtmlInputHidden)this.FindControl("hidden_SKUSubmitOrderGroupBuyStock");
			this.hidden_SKUSubmitOrderGroupBuyMinPrice = (HtmlInputHidden)this.FindControl("hidden_SKUSubmitOrderGroupBuyMinPrice");
			this.hidden_SKUSubmitOrderProductStock = (HtmlInputHidden)this.FindControl("hidden_SKUSubmitOrderProductStock");
			this.hidden_SKUSubmitOrderProductId = (HtmlInputHidden)this.FindControl("hidden_SKUSubmitOrderProductId");
			this.hidden_SKUSubmitOrderFightGroupActivityId = (HtmlInputHidden)this.FindControl("hidden_SKUSubmitOrderFightGroupActivityId");
			this.hidden_SKUSubmitOrderFightGroupId = (HtmlInputHidden)this.FindControl("hidden_SKUSubmitOrderFightGroupId");
			this.hidden_SKUSubmitOrderFightGroupActivityStock = (HtmlInputHidden)this.FindControl("hidden_SKUSubmitOrderFightGroupActivityStock");
			this.hidden_SKUSubmitOrderMinSalePriceSkuId = (HtmlInputHidden)this.FindControl("hidden_SKUSubmitOrderMinSalePriceSkuId");
			this.hidden_SKUSubmitOrderBusiness = (HtmlInputHidden)this.FindControl("hidden_SKUSubmitOrderBusiness");
			this.hidden_SKUSubmitOrderFightGroupActivityMinPrice = (HtmlInputHidden)this.FindControl("hidden_SKUSubmitOrderFightGroupActivityMinPrice");
			this.hidden_SKUSubmitOrderProductMinPrice = (HtmlInputHidden)this.FindControl("hidden_SKUSubmitOrderProductMinPrice");
			this.hidden_SKUSubmitOrderSelectedSkuId = (HtmlInputHidden)this.FindControl("hidden_SKUSubmitOrderSelectedSkuId");
			this.hidden_SKUSubmitOrderDepositPercent = (HtmlInputHidden)this.FindControl("hidden_SKUSubmitOrderDepositPercent");
			this.SKUSubmitOrderSelector = (Common_SKUSelector)this.FindControl("SKUSubmitOrderSelector");
			this.litUnit = (Literal)this.FindControl("litUnit");
			this.hidden_SKUSubmitOrderBusiness.Value = this.OrderBusiness.ToString();
			if (this.litUnit != null)
			{
				this.litUnit.Text = ((this.ProductInfo == null || string.IsNullOrEmpty(this.ProductInfo.Unit)) ? "件" : this.ProductInfo.Unit);
			}
			this.ProductBusiness();
			this.FightGrouptBusiness();
			this.GroupBuyBusiness();
			this.CountDownBusiness();
		}

		private void CountDownBusiness()
		{
			if (this.CountDownId > 0)
			{
				CountDownInfo countDownInfo = PromoteHelper.GetCountDownInfo(this.CountDownId, 0);
				if (countDownInfo != null)
				{
					HtmlInputHidden htmlInputHidden = this.hidden_SKUSubmitOrderCountDownId;
					int num = this.CountDownId;
					htmlInputHidden.Value = num.ToString();
					List<CountDownSkuInfo> countDownSkuInfo = countDownInfo.CountDownSkuInfo;
					if (countDownSkuInfo.Count > 0)
					{
						this.hidden_SKUSubmitOrderCountDownMinPrice.Value = (from t in countDownSkuInfo
						orderby t.SalePrice
						select t).FirstOrDefault().SalePrice.F2ToString("f2");
						CountDownSkuInfo countDownSkuInfo2 = (from t in countDownSkuInfo
						orderby t.TotalCount - t.BoughtCount descending
						select t).FirstOrDefault();
						int num2 = 0;
						int num3 = countDownSkuInfo2.ActivityTotal - countDownSkuInfo2.BoughtCount;
						num2 = ((countDownSkuInfo2.TotalCount <= num3) ? countDownSkuInfo2.TotalCount : num3);
						this.hidden_SKUSubmitOrderCountDownStock.Value = ((num2 > 0) ? num2.ToString() : "0");
						Label label = this.lblSKUSubmitOrderStockNow;
						num = countDownSkuInfo.Min((CountDownSkuInfo t) => t.TotalCount);
						label.Text = num.ToString();
					}
					Literal literal = this.litMaxCount;
					num = countDownInfo.MaxCount;
					literal.Text = num.ToString();
				}
			}
		}

		private void GroupBuyBusiness()
		{
			if (this.GroupBuyInfo != null)
			{
				this.hidden_SKUSubmitOrderGroupBuyId.Value = this.GroupBuyInfo.GroupBuyId.ToString();
				this.hidden_SKUSubmitOrderGroupBuyMinPrice.Value = this.GroupBuyInfo.Price.F2ToString("f2");
				int num = this.GroupBuyInfo.MaxCount - this.GroupBuySoldCount;
				this.hidden_SKUSubmitOrderGroupBuyStock.Value = ((num > 0) ? num.ToString() : "0");
				this.litMaxCount.Text = num.ToString();
			}
		}

		private void ProductBusiness()
		{
			if (this.ProductInfo != null)
			{
				HtmlInputHidden htmlInputHidden = this.hidden_SKUSubmitOrderProductId;
				int num = this.ProductInfo.ProductId;
				htmlInputHidden.Value = num.ToString();
				this.SKUSubmitOrderSelector.ProductId = this.ProductInfo.ProductId;
				Dictionary<string, SKUItem> dictionary;
				if (this.PreSaleId == 0)
				{
					dictionary = ProductBrowser.GetProductSkuSaleInfo(this.ProductInfo.ProductId, 0);
				}
				else
				{
					if (this.productPreSaleInfo != null)
					{
						this.lblSKUSubmitOrderPrePrice.Text = ((this.productPreSaleInfo.Deposit > decimal.Zero) ? this.productPreSaleInfo.Deposit.F2ToString("f2") : ((decimal)this.productPreSaleInfo.DepositPercent * this.ProductInfo.MinSalePrice / 100m).F2ToString("f2"));
					}
					else
					{
						this.lblSKUSubmitOrderPrePrice.Text = "0";
					}
					HtmlInputHidden htmlInputHidden2 = this.hidden_SKUSubmitOrderDepositPercent;
					num = this.productPreSaleInfo.DepositPercent;
					htmlInputHidden2.Value = num.ToString();
					HtmlInputHidden htmlInputHidden3 = this.hidden_SKUSubmitOrderPreSaleId;
					num = this.PreSaleId;
					htmlInputHidden3.Value = num.ToString();
					dictionary = ProductBrowser.GetPreSaleProductSkuSaleInfo(this.ProductInfo.ProductId);
				}
				if (dictionary != null)
				{
					this.imgSKUSubmitOrderProduct.ImageUrl = Globals.GetImageServerUrl("http://", string.IsNullOrEmpty(this.ProductInfo.ThumbnailUrl160) ? base.site.DefaultProductThumbnail4 : this.ProductInfo.ThumbnailUrl160);
					this.lblSKUSubmitOrderPrice.Text = this.ProductInfo.MinSalePrice.F2ToString("f2");
					Label label = this.lblSKUSubmitOrderStockNow;
					num = this.ProductInfo.Stock;
					label.Text = num.ToString();
					this.hidden_SKUSubmitOrderSelectedSkuId.Value = this.ProductInfo.DefaultSku.SkuId;
					this.hidden_SKUSubmitOrderProductMinPrice.Value = MemberProcessor.GetMemberPrice(this.ProductInfo).F2ToString("f2");
					HtmlInputHidden htmlInputHidden4 = this.hidden_SKUSubmitOrderProductStock;
					num = this.ProductInfo.Stock;
					htmlInputHidden4.Value = num.ToString();
					if (this.PreSaleId == 0)
					{
						string phonePriceByProductId = PromoteHelper.GetPhonePriceByProductId(this.ProductInfo.ProductId);
						if (!string.IsNullOrEmpty(phonePriceByProductId))
						{
							string s = phonePriceByProductId.Split(',')[0];
							decimal num2 = (this.ProductInfo.MinSalePrice - decimal.Parse(s) > decimal.Zero) ? (this.ProductInfo.MinSalePrice - decimal.Parse(s)) : decimal.Zero;
							HtmlInputHidden htmlInputHidden5 = this.hidden_SKUSubmitOrderProductMinPrice;
							Label label2 = this.lblSKUSubmitOrderPrice;
							string text3 = htmlInputHidden5.Value = (label2.Text = num2.F2ToString("f2"));
						}
					}
				}
			}
		}

		private void FightGrouptBusiness()
		{
			if (this.FightGroupActivityId > 0)
			{
				FightGroupActivityInfo fightGroupActivitieInfo = VShopHelper.GetFightGroupActivitieInfo(this.FightGroupActivityId);
				int num;
				if (fightGroupActivitieInfo != null)
				{
					Literal literal = this.litMaxCount;
					num = fightGroupActivitieInfo.MaxCount;
					literal.Text = num.ToString();
				}
				if (fightGroupActivitieInfo == null)
				{
					FightGroupInfo fightGroup = VShopHelper.GetFightGroup(this.FightGroupId);
					if (fightGroup != null)
					{
						fightGroupActivitieInfo = VShopHelper.GetFightGroupActivitieInfo(fightGroup.FightGroupActivityId);
						Literal literal2 = this.litMaxCount;
						num = fightGroupActivitieInfo.MaxCount;
						literal2.Text = num.ToString();
					}
				}
				HtmlInputHidden htmlInputHidden = this.hidden_SKUSubmitOrderFightGroupActivityId;
				num = this.FightGroupActivityId;
				htmlInputHidden.Value = num.ToString();
				HtmlInputHidden htmlInputHidden2 = this.hidden_SKUSubmitOrderFightGroupId;
				num = this.FightGroupId;
				htmlInputHidden2.Value = num.ToString();
				IList<FightGroupSkuInfo> fightGroupSkus = VShopHelper.GetFightGroupSkus(this.FightGroupActivityId);
				this.hidden_SKUSubmitOrderFightGroupActivityMinPrice.Value = fightGroupSkus.Min((FightGroupSkuInfo c) => c.SalePrice).F2ToString("f2");
				int num2 = fightGroupSkus.Sum(delegate(FightGroupSkuInfo c)
				{
					if (c.Stock >= c.TotalCount - c.BoughtCount)
					{
						return c.TotalCount - c.BoughtCount;
					}
					return c.Stock;
				});
				this.hidden_SKUSubmitOrderFightGroupActivityStock.Value = ((num2 > 0) ? num2.ToString() : "0");
			}
		}
	}
}
