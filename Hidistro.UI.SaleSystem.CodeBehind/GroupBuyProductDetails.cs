using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Promotions;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class GroupBuyProductDetails : HtmlTemplatedWebControl
	{
		private SiteSettings sitesettings = SettingsManager.GetMasterSettings();

		private int groupBuyId;

		private Common_Location common_Location;

		private Literal litProductName;

		private SkuLabel lblSku;

		private StockLabel lblStock;

		private Label litWeight;

		private Literal litUnit;

		private Label litCount;

		private Label litMaxCount;

		private Literal litRemainTime;

		private Literal litBrosedNum;

		private Literal litBrand;

		private Literal ltlReviewCount;

		private Literal litContent;

		private Literal litNeedCount;

		private FormatedMoneyLabel lblCurrentSalePrice;

		private FormatedMoneyLabel lblNeedPrice;

		private FormatedTimeLabel lblEndTime;

		private FormatedTimeLabel lblStartTime;

		private FormatedMoneyLabel lblSalePrice;

		private TotalLabel lblTotalPrice;

		private Literal litDescription;

		private Literal litShortDescription;

		private BuyButton btnOrder;

		private Common_ProductImages images;

		private ThemedTemplatedRepeater rptExpandAttributes;

		private SKUSelector skuSelector;

		private Common_ProductConsultations consultations;

		private Common_GoodsList_Correlative correlative;

		private HtmlInputHidden txtMaxCount;

		private HtmlInputHidden txtSoldCount;

		private HtmlInputHidden nowTime;

		private HtmlInputHidden hidden_skus;

		private HtmlInputHidden hidden_skuItem;

		private HtmlInputHidden hidden_IsOver;

		private HtmlInputHidden hidden_GroupBuyId;

		private HtmlInputHidden hidden_productId;

		private Literal ltlSaleCount;

		private Literal ltlConsultation;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-GroupBuyProductDetails.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			if (!int.TryParse(base.GetParameter("groupBuyId", false), out this.groupBuyId))
			{
				base.GotoResourceNotFound();
			}
			this.ltlConsultation = (Literal)this.FindControl("ltlConsultation");
			this.ltlSaleCount = (Literal)this.FindControl("ltlSaleCount");
			this.common_Location = (Common_Location)this.FindControl("common_Location");
			this.litProductName = (Literal)this.FindControl("litProductName");
			this.lblSku = (SkuLabel)this.FindControl("lblSku");
			this.lblStock = (StockLabel)this.FindControl("lblStock");
			this.litUnit = (Literal)this.FindControl("litUnit");
			this.litWeight = (Label)this.FindControl("litWeight");
			this.litBrosedNum = (Literal)this.FindControl("litBrosedNum");
			this.litBrand = (Literal)this.FindControl("litBrand");
			this.litContent = (Literal)this.FindControl("litContent");
			this.lblSalePrice = (FormatedMoneyLabel)this.FindControl("lblSalePrice");
			this.lblTotalPrice = (TotalLabel)this.FindControl("lblTotalPrice");
			this.litDescription = (Literal)this.FindControl("litDescription");
			this.litShortDescription = (Literal)this.FindControl("litShortDescription");
			this.btnOrder = (BuyButton)this.FindControl("btnOrder");
			this.ltlReviewCount = (Literal)this.FindControl("ltlReviewCount");
			this.txtMaxCount = (HtmlInputHidden)this.FindControl("txtMaxCount");
			this.txtSoldCount = (HtmlInputHidden)this.FindControl("txtSoldCount");
			this.hidden_IsOver = (HtmlInputHidden)this.FindControl("hidden_IsOver");
			this.hidden_GroupBuyId = (HtmlInputHidden)this.FindControl("hidden_GroupBuyId");
			this.hidden_productId = (HtmlInputHidden)this.FindControl("hidden_productId");
			this.lblCurrentSalePrice = (FormatedMoneyLabel)this.FindControl("lblCurrentSalePrice");
			this.litCount = (Label)this.FindControl("litCount");
			this.lblNeedPrice = (FormatedMoneyLabel)this.FindControl("lblNeedPrice");
			this.lblEndTime = (FormatedTimeLabel)this.FindControl("lblEndTime");
			this.lblStartTime = (FormatedTimeLabel)this.FindControl("lblStartTime");
			this.litRemainTime = (Literal)this.FindControl("litRemainTime");
			this.litNeedCount = (Literal)this.FindControl("litNeedCount");
			this.litMaxCount = (Label)this.FindControl("litMaxCount");
			this.images = (Common_ProductImages)this.FindControl("common_ProductImages");
			this.rptExpandAttributes = (ThemedTemplatedRepeater)this.FindControl("rptExpandAttributes");
			this.skuSelector = (SKUSelector)this.FindControl("SKUSelector");
			this.consultations = (Common_ProductConsultations)this.FindControl("list_Common_ProductConsultations");
			this.correlative = (Common_GoodsList_Correlative)this.FindControl("list_Common_GoodsList_Correlative");
			this.nowTime = (HtmlInputHidden)this.FindControl("nowTime");
			this.nowTime.SetWhenIsNotNull(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss", DateTimeFormatInfo.InvariantInfo));
			this.hidden_skus = (HtmlInputHidden)this.FindControl("hidden_skus");
			this.hidden_skuItem = (HtmlInputHidden)this.FindControl("hidden_skuItem");
			if (!this.Page.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				GroupBuyInfo groupBuy = PromoteHelper.GetGroupBuy(this.groupBuyId);
				if (groupBuy == null)
				{
					base.GotoResourceNotFound();
				}
				HtmlInputHidden htmlInputHidden = this.hidden_GroupBuyId;
				int num = groupBuy.GroupBuyId;
				htmlInputHidden.Value = num.ToString();
				ProductBrowseInfo productBrowseInfo = ProductBrowser.GetProductBrowseInfo(groupBuy.ProductId, null, masterSettings.OpenMultStore, 0);
				Literal literal = this.ltlSaleCount;
				num = productBrowseInfo.SaleCount;
				literal.Text = num.ToString();
				if (productBrowseInfo.Product == null)
				{
					this.hidden_IsOver.Value = "pullOff";
				}
				else if (!productBrowseInfo.Product.SaleStatus.Equals(ProductSaleStatus.OnSale))
				{
					this.hidden_IsOver.Value = "pullOff";
				}
				if (productBrowseInfo.Product == null || groupBuy == null)
				{
					this.Page.Response.Redirect("/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("该件商品参与的团购活动已经结束；或已被管理员删除"));
				}
				else
				{
					if (groupBuy != null)
					{
						if (groupBuy.Status != GroupBuyStatus.UnderWay)
						{
							this.Page.Response.Redirect("/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("该团购活动已经结束"));
							return;
						}
						HtmlInputHidden htmlInputHidden2 = this.hidden_productId;
						num = groupBuy.ProductId;
						htmlInputHidden2.Value = num.ToString();
						if (groupBuy.StartDate > DateTime.Now)
						{
							this.hidden_IsOver.Value = "AboutToBegin";
						}
						else if (groupBuy.EndDate < DateTime.Now)
						{
							this.hidden_IsOver.Value = "over";
						}
					}
					Literal literal2 = this.ltlConsultation;
					num = productBrowseInfo.ConsultationCount;
					literal2.Text = num.ToString();
					Literal literal3 = this.ltlReviewCount;
					num = productBrowseInfo.ReviewCount;
					literal3.Text = num.ToString();
					IEnumerable value = from item in productBrowseInfo.Product.Skus
					select item.Value;
					if (JsonConvert.SerializeObject(productBrowseInfo.DbSKUs) != null)
					{
						this.hidden_skuItem.Value = JsonConvert.SerializeObject(productBrowseInfo.DbSKUs);
					}
					if (this.hidden_skus != null)
					{
						this.hidden_skus.Value = JsonConvert.SerializeObject(value);
					}
					this.LoadPageSearch(productBrowseInfo.Product);
					this.LoadProductInfo(productBrowseInfo.Product, productBrowseInfo.BrandName);
					this.LoadProductGroupBuyInfo(groupBuy);
					this.btnOrder.Stock = productBrowseInfo.Product.Stock;
					BrowsedProductQueue.EnQueue(groupBuy.ProductId);
					this.images.ImageInfo = productBrowseInfo.Product;
					this.litContent.Text = groupBuy.Content;
					if (productBrowseInfo.DbAttribute != null)
					{
						this.rptExpandAttributes.DataSource = productBrowseInfo.DbAttribute;
						this.rptExpandAttributes.DataBind();
					}
					if (productBrowseInfo.DbSKUs != null)
					{
						this.skuSelector.ProductId = groupBuy.ProductId;
						this.skuSelector.DataSource = productBrowseInfo.DbSKUs;
					}
					if (productBrowseInfo.DBConsultations != null)
					{
						this.consultations.DataSource = productBrowseInfo.DBConsultations;
						this.consultations.DataBind();
					}
					if (productBrowseInfo.DbCorrelatives != null)
					{
						this.correlative.DataSource = productBrowseInfo.DbCorrelatives;
						this.correlative.DataBind();
					}
				}
			}
		}

		private void LoadPageSearch(ProductInfo productDetails)
		{
			if (!string.IsNullOrEmpty(productDetails.Meta_Keywords))
			{
				MetaTags.AddMetaKeywords(productDetails.Meta_Keywords, HiContext.Current.Context);
			}
			if (!string.IsNullOrEmpty(productDetails.Meta_Description))
			{
				MetaTags.AddMetaDescription(productDetails.Meta_Description, HiContext.Current.Context);
			}
			if (!string.IsNullOrEmpty(productDetails.Title))
			{
				PageTitle.AddSiteNameTitle(productDetails.Title);
			}
			else
			{
				PageTitle.AddSiteNameTitle(productDetails.ProductName);
			}
		}

		private void LoadProductGroupBuyInfo(GroupBuyInfo groupBuy)
		{
			int orderCount = ProductBrowser.GetOrderCount(groupBuy.GroupBuyId);
			decimal currentPrice = ProductBrowser.GetCurrentPrice(groupBuy.GroupBuyId);
			this.litCount.Text = orderCount.ToString();
			this.lblCurrentSalePrice.Money = currentPrice;
			this.lblNeedPrice.Money = groupBuy.NeedPrice;
			Label label = this.litMaxCount;
			int num = groupBuy.MaxCount;
			label.Text = num.ToString();
			HtmlInputHidden control = this.txtMaxCount;
			num = groupBuy.MaxCount;
			control.SetWhenIsNotNull(num.ToString());
			this.txtSoldCount.SetWhenIsNotNull(orderCount.ToString());
			this.lblTotalPrice.Value = currentPrice;
			this.lblEndTime.Time = groupBuy.EndDate;
			this.lblStartTime.Time = groupBuy.StartDate;
			this.litRemainTime.Text = "";
			Literal literal = this.litNeedCount;
			num = groupBuy.Count;
			literal.Text = num.ToString();
		}

		private void LoadProductInfo(ProductInfo productDetails, string brandName)
		{
			if (this.common_Location != null && !string.IsNullOrEmpty(productDetails.MainCategoryPath))
			{
				this.common_Location.CateGoryPath = productDetails.MainCategoryPath.Remove(productDetails.MainCategoryPath.Length - 1);
				this.common_Location.ProductName = productDetails.ProductName;
			}
			this.litProductName.Text = productDetails.ProductName;
			this.lblSku.Value = productDetails.SkuId;
			this.lblSku.Text = productDetails.SKU;
			this.lblStock.Stock = productDetails.Stock;
			this.lblStock.Stock = productDetails.Stock;
			this.litUnit.Text = productDetails.Unit;
			this.litUnit.Text = productDetails.Unit;
			if (productDetails.Weight > decimal.Zero)
			{
				this.litWeight.Text = string.Format("{0} g", productDetails.Weight.F2ToString("f2"));
			}
			else
			{
				this.litWeight.Text = "无";
			}
			this.litBrosedNum.Text = productDetails.VistiCounts.ToString();
			this.litBrand.Text = brandName;
			this.lblSalePrice.Money = productDetails.MaxSalePrice;
			string text = "";
			Regex regex = new Regex("<script[^>]*?>.*?</script>", RegexOptions.IgnoreCase);
			if (!string.IsNullOrWhiteSpace(productDetails.Description))
			{
				text = regex.Replace(productDetails.Description, "");
			}
			this.litDescription.Text = text.Replace("src", "data-url");
			if (this.litShortDescription != null)
			{
				this.litShortDescription.Text = productDetails.ShortDescription;
			}
		}
	}
}
