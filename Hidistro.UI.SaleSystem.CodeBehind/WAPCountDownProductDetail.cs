using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Promotions;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class WAPCountDownProductDetail : WAPTemplatedWebControl
	{
		private int countDownId;

		private WapTemplatedRepeater rptProductImages;

		private Literal litProdcutName;

		private Literal litShortDescription;

		private Literal litDescription;

		private Literal litSoldCount;

		private Literal litminCount;

		private Literal litprice;

		private Literal litcontent;

		private Literal litLeftSeconds;

		private HtmlInputControl litGroupBuyId;

		private Literal salePrice;

		private Literal leftCount;

		private Literal minSuccessCount;

		private HtmlInputControl txtProductId;

		private Literal litConsultationsCount;

		private Literal litGroupbuyDescription;

		private Literal litReviewsCount;

		private Literal litMaxCount;

		private Literal litUnit;

		private HtmlInputHidden startTime;

		private HtmlInputHidden endTime;

		private HtmlInputHidden groupBuySoldCount;

		private HtmlInputHidden groupBuyMinCount;

		private HtmlInputHidden nowTime;

		private HtmlInputHidden groupBuyMaxCount;

		private HtmlInputHidden skuStock;

		private HtmlInputHidden hiddenIsLogin;

		private HtmlInputHidden hdAppId;

		private HtmlInputHidden hdTimestamp;

		private HtmlInputHidden hdNonceStr;

		private HtmlInputHidden hdSignature;

		private HtmlInputHidden hdTitle;

		private HtmlInputHidden hdDesc;

		private HtmlInputHidden hdImgUrl;

		private HtmlInputHidden hdLink;

		private HtmlInputHidden hdCountDownId;

		private HtmlInputHidden hidden_IsOver;

		private Common_SKUSubmitOrder skuSubmitOrder;

		private HtmlGenericControl divShortDescription;

		private Common_ExpandAttributes expandAttr;

		protected override void OnInit(EventArgs e)
		{
			this.SkinName = ((this.SkinName == null) ? "Skin-VCountDownProductDetail.html" : this.SkinName);
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.countDownId = this.Page.Request.QueryString["countDownId"].ToInt(0);
			int storeId = this.Page.Request.QueryString["StoreId"].ToInt(0);
			CountDownInfo countDownInfo = PromoteHelper.GetCountDownInfo(this.countDownId, storeId);
			if (countDownInfo == null)
			{
				this.ShowWapMessage("抢购信息不存在", "default.aspx");
			}
			this.FindControls();
			this.SetControlsValue(countDownInfo);
			PageTitle.AddSiteNameTitle("限时抢购商品详情");
		}

		private void FindControls()
		{
			this.divShortDescription = (HtmlGenericControl)this.FindControl("divShortDescription");
			this.rptProductImages = (WapTemplatedRepeater)this.FindControl("rptProductImages");
			this.litProdcutName = (Literal)this.FindControl("litProdcutName");
			this.litShortDescription = (Literal)this.FindControl("litShortDescription");
			this.litDescription = (Literal)this.FindControl("litDescription");
			this.litSoldCount = (Literal)this.FindControl("soldCount");
			this.litprice = (Literal)this.FindControl("price");
			this.litcontent = (Literal)this.FindControl("content");
			this.litminCount = (Literal)this.FindControl("minCount");
			this.litGroupBuyId = (HtmlInputControl)this.FindControl("litGroupbuyId");
			this.hidden_IsOver = (HtmlInputHidden)this.FindControl("hidden_IsOver");
			this.litLeftSeconds = (Literal)this.FindControl("leftSeconds");
			this.skuSubmitOrder = (Common_SKUSubmitOrder)this.FindControl("skuSubmitOrder");
			this.salePrice = (Literal)this.FindControl("salePrice");
			this.leftCount = (Literal)this.FindControl("leftCount");
			this.minSuccessCount = (Literal)this.FindControl("minSuccessCount");
			this.txtProductId = (HtmlInputControl)this.FindControl("txtProductId");
			this.litConsultationsCount = (Literal)this.FindControl("litConsultationsCount");
			this.litReviewsCount = (Literal)this.FindControl("litReviewsCount");
			this.litMaxCount = (Literal)this.FindControl("litMaxCount");
			this.startTime = (HtmlInputHidden)this.FindControl("startTime");
			this.endTime = (HtmlInputHidden)this.FindControl("endTime");
			this.groupBuySoldCount = (HtmlInputHidden)this.FindControl("groupBuySoldCount");
			this.groupBuyMinCount = (HtmlInputHidden)this.FindControl("groupBuyMinCount");
			this.litGroupbuyDescription = (Literal)this.FindControl("litGroupbuyDescription");
			this.groupBuyMaxCount = (HtmlInputHidden)this.FindControl("groupBuyMaxCount");
			this.skuStock = (HtmlInputHidden)this.FindControl("skuStock");
			this.hiddenIsLogin = (HtmlInputHidden)this.FindControl("hiddenIsLogin");
			this.hdAppId = (HtmlInputHidden)this.FindControl("hdAppId");
			this.hdTimestamp = (HtmlInputHidden)this.FindControl("hdTimestamp");
			this.hdNonceStr = (HtmlInputHidden)this.FindControl("hdNonceStr");
			this.hdSignature = (HtmlInputHidden)this.FindControl("hdSignature");
			this.hdTitle = (HtmlInputHidden)this.FindControl("hdTitle");
			this.hdDesc = (HtmlInputHidden)this.FindControl("hdDesc");
			this.hdImgUrl = (HtmlInputHidden)this.FindControl("hdImgUrl");
			this.hdLink = (HtmlInputHidden)this.FindControl("hdLink");
			this.hdCountDownId = (HtmlInputHidden)this.FindControl("hdCountDownId");
			this.expandAttr = (Common_ExpandAttributes)this.FindControl("ExpandAttributes");
			this.litUnit = (Literal)this.FindControl("litUnit");
		}

		private void SetControlsValue(CountDownInfo countDownInfo)
		{
			this.skuSubmitOrder.CountDownId = countDownInfo.CountDownId;
			this.skuSubmitOrder.OrderBusiness = 2;
			HtmlInputHidden htmlInputHidden = this.hdCountDownId;
			int num = countDownInfo.CountDownId;
			htmlInputHidden.Value = num.ToString();
			this.hiddenIsLogin.Value = HiContext.Current.UserId.ToString();
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			ProductBrowseInfo productBrowseInfo = ProductBrowser.GetProductBrowseInfo(countDownInfo.ProductId, null, masterSettings.OpenMultStore, 0);
			if (productBrowseInfo.Product == null || productBrowseInfo.Product.SaleStatus == ProductSaleStatus.Delete)
			{
				this.ShowWapMessage("抢购商品已不存在", "Default.aspx");
			}
			else
			{
				if (productBrowseInfo.Product == null)
				{
					this.hidden_IsOver.Value = "pullOff";
				}
				else if (!productBrowseInfo.Product.SaleStatus.Equals(ProductSaleStatus.OnSale))
				{
					this.hidden_IsOver.Value = "pullOff";
				}
				if (!countDownInfo.IsJoin)
				{
					this.hidden_IsOver.Value = "nojoin";
				}
				else if (countDownInfo.StartDate > DateTime.Now)
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
				if (productBrowseInfo.Product.SaleStatus != ProductSaleStatus.OnSale)
				{
					this.ShowWapMessage("此商品已下架", "Default.aspx");
				}
				if (this.expandAttr != null)
				{
					this.expandAttr.ProductId = productBrowseInfo.Product.ProductId;
				}
				if (this.rptProductImages != null)
				{
					string locationUrl = "javascript:;";
					List<SlideImage> list = new List<SlideImage>
					{
						new SlideImage(productBrowseInfo.Product.ImageUrl1, locationUrl),
						new SlideImage(productBrowseInfo.Product.ImageUrl2, locationUrl),
						new SlideImage(productBrowseInfo.Product.ImageUrl3, locationUrl),
						new SlideImage(productBrowseInfo.Product.ImageUrl4, locationUrl),
						new SlideImage(productBrowseInfo.Product.ImageUrl5, locationUrl)
					};
					IEnumerable<SlideImage> source = from item in list
					where !string.IsNullOrWhiteSpace(item.ImageUrl)
					select item;
					if (source.Count() == 0)
					{
						list.Add(new SlideImage(masterSettings.DefaultProductImage, locationUrl));
					}
					this.rptProductImages.DataSource = from item in list
					where !string.IsNullOrWhiteSpace(item.ImageUrl)
					select item;
					this.rptProductImages.DataBind();
				}
				this.skuSubmitOrder.ProductInfo = productBrowseInfo.Product;
				this.litProdcutName.SetWhenIsNotNull(productBrowseInfo.Product.ProductName);
				Literal control = this.litminCount;
				num = countDownInfo.MaxCount;
				control.SetWhenIsNotNull(num.ToString());
				this.litShortDescription.Text = productBrowseInfo.Product.ShortDescription;
				this.divShortDescription.Visible = !string.IsNullOrEmpty(productBrowseInfo.Product.ShortDescription);
				if (this.litDescription != null)
				{
					string text = "";
					Regex regex = new Regex("<script[^>]*?>.*?</script>", RegexOptions.IgnoreCase);
					if (!string.IsNullOrWhiteSpace(productBrowseInfo.Product.MobbileDescription))
					{
						text = regex.Replace(productBrowseInfo.Product.MobbileDescription, "");
					}
					else if (!string.IsNullOrWhiteSpace(productBrowseInfo.Product.Description))
					{
						text = regex.Replace(productBrowseInfo.Product.Description, "");
					}
					text = text.Replace("src", "data-url");
					this.litDescription.Text = text;
				}
				this.litprice.SetWhenIsNotNull(PromoteHelper.GetCountDownSalePrice(this.countDownId).F2ToString("f2"));
				this.litLeftSeconds.SetWhenIsNotNull(Math.Ceiling((countDownInfo.EndDate - DateTime.Now).TotalSeconds).ToString());
				this.litcontent.SetWhenIsNotNull(countDownInfo.Content);
				HtmlInputControl control2 = this.litGroupBuyId;
				num = countDownInfo.CountDownId;
				control2.SetWhenIsNotNull(num.ToString());
				if (productBrowseInfo.Product.MarketPrice.HasValue && productBrowseInfo.Product.MarketPrice.Value > decimal.Zero)
				{
					this.salePrice.SetWhenIsNotNull(productBrowseInfo.Product.MarketPrice.Value.F2ToString("f2"));
				}
				else
				{
					this.salePrice.SetWhenIsNotNull(productBrowseInfo.Product.Skus.Max((KeyValuePair<string, SKUItem> c) => c.Value.SalePrice).F2ToString("f2"));
				}
				HtmlInputControl control3 = this.txtProductId;
				num = countDownInfo.ProductId;
				control3.SetWhenIsNotNull(num.ToString());
				Literal control4 = this.litConsultationsCount;
				num = productBrowseInfo.ConsultationCount;
				control4.SetWhenIsNotNull(num.ToString());
				Literal control5 = this.litReviewsCount;
				num = productBrowseInfo.ReviewCount;
				control5.SetWhenIsNotNull(num.ToString());
				this.litGroupbuyDescription.SetWhenIsNotNull(countDownInfo.Content);
				Literal control6 = this.litMaxCount;
				num = countDownInfo.MaxCount;
				control6.SetWhenIsNotNull(num.ToString());
				this.nowTime = (HtmlInputHidden)this.FindControl("nowTime");
				HtmlInputHidden control7 = this.nowTime;
				DateTime dateTime = DateTime.Now;
				control7.SetWhenIsNotNull(dateTime.ToString("yyyy/MM/dd HH:mm:ss", DateTimeFormatInfo.InvariantInfo));
				HtmlInputHidden control8 = this.startTime;
				dateTime = countDownInfo.StartDate;
				control8.SetWhenIsNotNull(dateTime.ToString("yyyy/MM/dd HH:mm:ss"));
				HtmlInputHidden control9 = this.endTime;
				dateTime = countDownInfo.EndDate;
				control9.SetWhenIsNotNull(dateTime.ToString("yyyy/MM/dd HH:mm:ss"));
				HtmlInputHidden control10 = this.groupBuyMaxCount;
				num = countDownInfo.MaxCount;
				control10.SetWhenIsNotNull(num.ToString());
				HtmlInputHidden control11 = this.skuStock;
				num = productBrowseInfo.Product.DefaultSku.Stock;
				control11.SetWhenIsNotNull(num.ToString());
				this.litUnit.SetWhenIsNotNull(string.IsNullOrEmpty(productBrowseInfo.Product.Unit) ? "件" : productBrowseInfo.Product.Unit);
				this.SetWXShare(countDownInfo, productBrowseInfo.Product);
			}
		}

		private void SetWXShare(CountDownInfo countDown, ProductInfo product)
		{
			this.hdAppId.Value = base.site.WeixinAppId;
			Task.Factory.StartNew(delegate
			{
				try
				{
					string jsApiTicket = base.GetJsApiTicket(true);
					string text = WAPTemplatedWebControl.GenerateNonceStr();
					string text2 = WAPTemplatedWebControl.GenerateTimeStamp();
					string absoluteUri = this.Page.Request.Url.AbsoluteUri;
					this.hdTimestamp.Value = text2;
					this.hdNonceStr.Value = text;
					this.hdSignature.Value = base.GetSignature(jsApiTicket, text, text2, absoluteUri);
				}
				catch
				{
				}
			});
			this.hdDesc.Value = (string.IsNullOrEmpty(countDown.ShareDetails) ? product.ShortDescription : countDown.ShareDetails);
			string local = string.IsNullOrEmpty(countDown.ShareIcon) ? (string.IsNullOrEmpty(product.ImageUrl1) ? SettingsManager.GetMasterSettings().DefaultProductImage : product.ImageUrl1) : Globals.FullPath(countDown.ShareIcon);
			this.hdImgUrl.Value = Globals.FullPath(local);
			this.hdTitle.Value = (string.IsNullOrEmpty(countDown.ShareTitle) ? product.ProductName : countDown.ShareTitle);
			this.hdLink.Value = Globals.FullPath(this.Page.Request.Url.ToString());
		}
	}
}
