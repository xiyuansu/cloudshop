using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Commodities;
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
	public class CountDownProductsDetails : HtmlTemplatedWebControl
	{
		private SiteSettings sitesettings = SettingsManager.GetMasterSettings();

		private int countDownId;

		private Common_Location common_Location;

		private Literal litProductCode;

		private Literal litProductName;

		private Literal litmaxcount;

		private SkuLabel lblSku;

		private StockLabel lblStock;

		private Label litWeight;

		private Literal litUnit;

		private Literal litSurplusNumber;

		private Literal litContent;

		private Literal litRemainTime;

		private Literal litBrosedNum;

		private Literal litBrand;

		private FormatedMoneyLabel lblCurrentSalePrice;

		private FormatedTimeLabel lblEndTime;

		private FormatedTimeLabel lblStartTime;

		private FormatedMoneyLabel lblSalePrice;

		private TotalLabel lblTotalPrice;

		private Literal litDescription;

		private Literal litShortDescription;

		private BuyButton btnOrder;

		private HyperLink hpkProductConsultations;

		private Common_ProductImages images;

		private ThemedTemplatedRepeater rptExpandAttributes;

		private SKUSelector skuSelector;

		private Common_ProductConsultations consultations;

		private Common_GoodsList_Correlative correlative;

		private HtmlInputHidden nowTime;

		private HtmlInputHidden hidden_skus;

		private HtmlInputHidden hidden_skuItem;

		private HtmlInputHidden hidden_IsOver;

		private HtmlInputHidden hidden_CountDownId;

		private HtmlInputHidden hidden_productId;

		private HtmlInputHidden hdShareDetails;

		private HtmlInputHidden hdShareIcon;

		private HtmlInputHidden hdShareTitle;

		private Literal ltlSaleCount;

		private Literal ltlConsultation;

		private Literal ltlReviewCount;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-CountDownProductsDetails.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			if (!int.TryParse(base.GetParameter("countDownId", false), out this.countDownId))
			{
				base.GotoResourceNotFound();
			}
			this.common_Location = (Common_Location)this.FindControl("common_Location");
			this.litProductCode = (Literal)this.FindControl("litProductCode");
			this.litProductName = (Literal)this.FindControl("litProductName");
			this.lblSku = (SkuLabel)this.FindControl("lblSku");
			this.lblStock = (StockLabel)this.FindControl("lblStock");
			this.litUnit = (Literal)this.FindControl("litUnit");
			this.litSurplusNumber = (Literal)this.FindControl("litSurplusNumber");
			this.litWeight = (Label)this.FindControl("litWeight");
			this.litBrosedNum = (Literal)this.FindControl("litBrosedNum");
			this.litBrand = (Literal)this.FindControl("litBrand");
			this.litmaxcount = (Literal)this.FindControl("litMaxCount");
			this.lblSalePrice = (FormatedMoneyLabel)this.FindControl("lblSalePrice");
			this.lblTotalPrice = (TotalLabel)this.FindControl("lblTotalPrice");
			this.litDescription = (Literal)this.FindControl("litDescription");
			this.litShortDescription = (Literal)this.FindControl("litShortDescription");
			this.btnOrder = (BuyButton)this.FindControl("btnOrder");
			this.hpkProductConsultations = (HyperLink)this.FindControl("hpkProductConsultations");
			this.ltlSaleCount = (Literal)this.FindControl("ltlSaleCount");
			this.ltlConsultation = (Literal)this.FindControl("ltlConsultation");
			this.ltlReviewCount = (Literal)this.FindControl("ltlReviewCount");
			this.lblCurrentSalePrice = (FormatedMoneyLabel)this.FindControl("lblCurrentSalePrice");
			this.litContent = (Literal)this.FindControl("litContent");
			this.lblEndTime = (FormatedTimeLabel)this.FindControl("lblEndTime");
			this.lblStartTime = (FormatedTimeLabel)this.FindControl("lblStartTime");
			this.litRemainTime = (Literal)this.FindControl("litRemainTime");
			this.images = (Common_ProductImages)this.FindControl("common_ProductImages");
			this.rptExpandAttributes = (ThemedTemplatedRepeater)this.FindControl("rptExpandAttributes");
			this.skuSelector = (SKUSelector)this.FindControl("SKUSelector");
			this.consultations = (Common_ProductConsultations)this.FindControl("list_Common_ProductConsultations");
			this.correlative = (Common_GoodsList_Correlative)this.FindControl("list_Common_GoodsList_Correlative");
			this.nowTime = (HtmlInputHidden)this.FindControl("nowTime");
			this.nowTime.SetWhenIsNotNull(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss", DateTimeFormatInfo.InvariantInfo));
			this.hidden_skus = (HtmlInputHidden)this.FindControl("hidden_skus");
			this.hidden_skuItem = (HtmlInputHidden)this.FindControl("hidden_skuItem");
			this.hidden_IsOver = (HtmlInputHidden)this.FindControl("hidden_IsOver");
			this.hidden_CountDownId = (HtmlInputHidden)this.FindControl("hidden_CountDownId");
			this.hidden_productId = (HtmlInputHidden)this.FindControl("hidden_productId");
			this.hdShareDetails = (HtmlInputHidden)this.FindControl("hdShareDetails");
			this.hdShareIcon = (HtmlInputHidden)this.FindControl("hdShareIcon");
			this.hdShareTitle = (HtmlInputHidden)this.FindControl("hdShareTitle");
			if (!this.Page.IsPostBack)
			{
				CountDownInfo countDownInfo = PromoteHelper.GetCountDownInfo(this.countDownId, 0);
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				if (countDownInfo != null)
				{
					HtmlInputHidden htmlInputHidden = this.hidden_CountDownId;
					int num = countDownInfo.CountDownId;
					htmlInputHidden.Value = num.ToString();
					HtmlInputHidden htmlInputHidden2 = this.hidden_productId;
					num = countDownInfo.ProductId;
					htmlInputHidden2.Value = num.ToString();
					int num2 = 0;
					ProductBrowseInfo productBrowseInfo = ProductBrowser.GetProductBrowseInfo(countDownInfo.ProductId, null, masterSettings.OpenMultStore, 0);
					Literal literal = this.ltlReviewCount;
					num = productBrowseInfo.ReviewCount;
					literal.Text = num.ToString();
					Literal literal2 = this.ltlSaleCount;
					num = productBrowseInfo.SaleCount;
					literal2.Text = num.ToString();
					if (productBrowseInfo.Product == null)
					{
						this.hidden_IsOver.Value = "pullOff";
					}
					else if (!productBrowseInfo.Product.SaleStatus.Equals(ProductSaleStatus.OnSale))
					{
						this.hidden_IsOver.Value = "pullOff";
					}
					if (!string.IsNullOrEmpty(countDownInfo.ShareDetails))
					{
						HtmlInputHidden htmlInputHidden3 = this.hdShareDetails;
						htmlInputHidden3.Value += countDownInfo.ShareDetails;
					}
					else if (!string.IsNullOrEmpty(countDownInfo.Content))
					{
						this.hdShareDetails.Value = "限时抢购简单说明：";
						HtmlInputHidden htmlInputHidden4 = this.hdShareDetails;
						htmlInputHidden4.Value += countDownInfo.Content;
					}
					else
					{
						this.hdShareDetails.Value = "限时抢购简单说明：";
					}
					Literal literal3 = this.ltlConsultation;
					num = productBrowseInfo.ConsultationCount;
					literal3.Text = num.ToString();
					this.hdShareIcon.Value = Globals.FullPath(countDownInfo.ShareIcon);
					this.hdShareTitle.Value = countDownInfo.ShareTitle;
					if (countDownInfo.StartDate > DateTime.Now)
					{
						this.hidden_IsOver.Value = "AboutToBegin";
					}
					else if (countDownInfo.EndDate < DateTime.Now)
					{
						this.hidden_IsOver.Value = "over";
					}
					else if (!countDownInfo.IsRunning)
					{
						this.hidden_IsOver.Value = "true";
					}
					num2 = PromoteHelper.GetCountDownSurplusNumber(this.countDownId);
					num2 = ((num2 >= 0) ? num2 : 0);
					this.litSurplusNumber.Text = num2.ToString();
					Dictionary<string, SKUItem> dictionary = new Dictionary<string, SKUItem>();
					foreach (SKUItem value2 in productBrowseInfo.Product.Skus.Values)
					{
						CountDownSkuInfo countDownSkuInfo = (from c in countDownInfo.CountDownSkuInfo
						where c.SkuId == value2.SkuId
						select c).FirstOrDefault();
						if (countDownSkuInfo != null)
						{
							value2.Stock = ((value2.Stock < countDownSkuInfo.ActivityTotal - countDownSkuInfo.BoughtCount) ? value2.Stock : (countDownSkuInfo.ActivityTotal - countDownSkuInfo.BoughtCount));
						}
						dictionary.Add(value2.SkuId, value2);
					}
					IEnumerable value = from item in dictionary
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
					this.hpkProductConsultations.Target = "_blank";
					this.hpkProductConsultations.Text = "查看全部";
					this.hpkProductConsultations.NavigateUrl = $"ProductConsultationsAndReplay.aspx?productId={countDownInfo.ProductId}";
					this.LoadProductInfo(productBrowseInfo.Product, productBrowseInfo.BrandName);
					this.LoadCountDownBuyInfo(countDownInfo);
					this.btnOrder.Stock = productBrowseInfo.Product.Stock;
					if (!countDownInfo.IsJoin)
					{
						this.hidden_IsOver.Value = "nojoin";
					}
					BrowsedProductQueue.EnQueue(countDownInfo.ProductId);
					this.images.ImageInfo = productBrowseInfo.Product;
					if (productBrowseInfo.DbAttribute != null)
					{
						this.rptExpandAttributes.DataSource = productBrowseInfo.DbAttribute;
						this.rptExpandAttributes.DataBind();
					}
					if (productBrowseInfo.DbSKUs != null)
					{
						this.skuSelector.ProductId = countDownInfo.ProductId;
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
				else
				{
					base.GotoResourceNotFound();
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

		private void LoadCountDownBuyInfo(CountDownInfo countDownInfo)
		{
			decimal countDownSalePrice = PromoteHelper.GetCountDownSalePrice(this.countDownId);
			this.lblCurrentSalePrice.Money = countDownSalePrice;
			this.litContent.Text = countDownInfo.Content;
			this.lblTotalPrice.Value = countDownSalePrice;
			this.lblStartTime.Time = countDownInfo.StartDate;
			this.lblEndTime.Time = countDownInfo.EndDate;
			this.litRemainTime.Text = "";
			this.litmaxcount.Text = Convert.ToString(countDownInfo.MaxCount);
		}

		private void LoadProductInfo(ProductInfo productDetails, string brandName)
		{
			if (this.common_Location != null && !string.IsNullOrEmpty(productDetails.MainCategoryPath))
			{
				this.common_Location.CateGoryPath = productDetails.MainCategoryPath.Remove(productDetails.MainCategoryPath.Length - 1);
				this.common_Location.ProductName = productDetails.ProductName;
			}
			this.litProductCode.Text = productDetails.ProductCode;
			this.litProductName.Text = productDetails.ProductName;
			this.lblSku.Value = productDetails.SkuId;
			this.lblStock.Stock = productDetails.Stock;
			this.litUnit.Text = productDetails.Unit;
			this.lblSku.Text = productDetails.SKU;
			this.lblStock.Stock = productDetails.Stock;
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
			IList<int> list = null;
			Dictionary<int, IList<int>> dictionary = default(Dictionary<int, IList<int>>);
			ProductInfo productDetails2 = ProductHelper.GetProductDetails(productDetails.ProductId, out dictionary, out list);
			if (productDetails2 != null)
			{
				if (productDetails2.MarketPrice.HasValue && productDetails2.MarketPrice.Value > decimal.Zero)
				{
					this.lblSalePrice.Money = productDetails2.MarketPrice.Value;
				}
				else
				{
					this.lblSalePrice.Money = productDetails2.MaxSalePrice;
				}
			}
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
