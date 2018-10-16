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
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class WAPGroupBuyProductDetail : WAPTemplatedWebControl
	{
		private int groupbuyId;

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

		private Literal litUnit1;

		private Literal litUnit2;

		private Literal litUnit3;

		private HtmlInputHidden startTime;

		private HtmlInputHidden endTime;

		private HtmlInputHidden groupBuySoldCount;

		private HtmlInputHidden groupBuyMinCount;

		private HtmlInputHidden nowTime;

		private HtmlInputHidden groupBuyMaxCount;

		private HtmlInputHidden skuStock;

		private HtmlInputHidden hdAppId;

		private HtmlInputHidden hdTitle;

		private HtmlInputHidden hdDesc;

		private HtmlInputHidden hdImgUrl;

		private HtmlInputHidden hdLink;

		private HtmlInputHidden hdSource;

		private Common_SKUSubmitOrder skuSubmitOrder;

		private HtmlGenericControl divShortDescription;

		private Common_ExpandAttributes expandAttr;

		protected override void OnInit(EventArgs e)
		{
			this.SkinName = ((this.SkinName == null) ? "Skin-VGroupBuyProductDetail.html" : this.SkinName);
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			if (!this.CheckGroupbuyIdExist())
			{
				this.ShowWapMessage("团购信息不存在", "default.aspx");
			}
			this.FindControls();
			this.SetControlsValue(this.groupbuyId);
			PageTitle.AddSiteNameTitle("团购商品详情");
		}

		private bool CheckGroupbuyIdExist()
		{
			int.TryParse(this.Page.Request.QueryString["GroupBuyId"], out this.groupbuyId);
			if (this.groupbuyId <= 0)
			{
				int num = 0;
				int.TryParse(this.Page.Request.QueryString["ProductId"], out num);
				if (num > 0)
				{
					GroupBuyInfo groupByProdctId = ProductBrowser.GetGroupByProdctId(num);
					if (groupByProdctId != null)
					{
						this.groupbuyId = groupByProdctId.GroupBuyId;
					}
				}
			}
			return this.groupbuyId > 0;
		}

		private void FindControls()
		{
			this.divShortDescription = (HtmlGenericControl)this.FindControl("divShortDescription");
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			this.rptProductImages = (WapTemplatedRepeater)this.FindControl("rptProductImages");
			this.litProdcutName = (Literal)this.FindControl("litProdcutName");
			this.litShortDescription = (Literal)this.FindControl("litShortDescription");
			this.litDescription = (Literal)this.FindControl("litDescription");
			this.litSoldCount = (Literal)this.FindControl("soldCount");
			this.litprice = (Literal)this.FindControl("price");
			this.litcontent = (Literal)this.FindControl("content");
			this.litminCount = (Literal)this.FindControl("minCount");
			this.litGroupBuyId = (HtmlInputControl)this.FindControl("litGroupbuyId");
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
			this.hdAppId = (HtmlInputHidden)this.FindControl("hdAppId");
			this.hdTitle = (HtmlInputHidden)this.FindControl("hdTitle");
			this.hdDesc = (HtmlInputHidden)this.FindControl("hdDesc");
			this.hdImgUrl = (HtmlInputHidden)this.FindControl("hdImgUrl");
			this.hdLink = (HtmlInputHidden)this.FindControl("hdLink");
			this.hdSource = (HtmlInputHidden)this.FindControl("hdSource");
			this.hdAppId.Value = masterSettings.WeixinAppId;
			this.expandAttr = (Common_ExpandAttributes)this.FindControl("ExpandAttributes");
			this.litUnit1 = (Literal)this.FindControl("litUnit1");
			this.litUnit2 = (Literal)this.FindControl("litUnit2");
			this.litUnit3 = (Literal)this.FindControl("litUnit3");
		}

		private void SetControlsValue(int groupbuyId)
		{
			this.hdSource.Value = "groupbuy";
			GroupBuyInfo groupBuy = ProductBrowser.GetGroupBuy(groupbuyId);
			if (groupBuy == null)
			{
				base.GotoResourceNotFound("团购已经被管理员删除");
			}
			else if (groupBuy.Status != GroupBuyStatus.UnderWay)
			{
				this.Page.Response.Redirect("/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("该团购活动已经结束"));
			}
			else
			{
				this.skuSubmitOrder.OrderBusiness = 3;
				int soldCount = PromoteHelper.GetSoldCount(groupbuyId);
				this.skuSubmitOrder.GroupBuyInfo = groupBuy;
				this.skuSubmitOrder.GroupBuySoldCount = soldCount;
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				ProductBrowseInfo productBrowseInfo = ProductBrowser.GetProductBrowseInfo(groupBuy.ProductId, null, masterSettings.OpenMultStore, 0);
				if (productBrowseInfo.Product == null || productBrowseInfo.Product.SaleStatus == ProductSaleStatus.Delete)
				{
					base.GotoResourceNotFound("团购商品已经被管理员删除");
				}
				else
				{
					if (productBrowseInfo.Product.SaleStatus != ProductSaleStatus.OnSale)
					{
						base.GotoResourceNotFound("此商品已下架");
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
					if (this.expandAttr != null)
					{
						this.expandAttr.ProductId = productBrowseInfo.Product.ProductId;
					}
					this.skuSubmitOrder.ProductInfo = productBrowseInfo.Product;
					this.hdTitle.Value = Globals.StripAllTags(string.IsNullOrEmpty(productBrowseInfo.Product.Title) ? productBrowseInfo.Product.ProductName : productBrowseInfo.Product.Title);
					this.hdDesc.Value = Globals.StripAllTags(string.IsNullOrEmpty(productBrowseInfo.Product.ShortDescription) ? this.hdTitle.Value : productBrowseInfo.Product.ShortDescription);
					this.hdImgUrl.Value = Globals.FullPath(productBrowseInfo.Product.ImageUrl1);
					this.hdLink.Value = Globals.FullPath(HttpContext.Current.Request.Url.ToString());
					this.litProdcutName.SetWhenIsNotNull(productBrowseInfo.Product.ProductName);
					this.litSoldCount.SetWhenIsNotNull(soldCount.ToString());
					Literal control = this.litminCount;
					int num = groupBuy.Count;
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
					this.litprice.SetWhenIsNotNull(groupBuy.Price.F2ToString("f2"));
					this.litLeftSeconds.SetWhenIsNotNull(Math.Ceiling((groupBuy.EndDate - DateTime.Now).TotalSeconds).ToString());
					this.litcontent.SetWhenIsNotNull(groupBuy.Content);
					HtmlInputControl control2 = this.litGroupBuyId;
					num = groupBuy.GroupBuyId;
					control2.SetWhenIsNotNull(num.ToString());
					this.salePrice.SetWhenIsNotNull(productBrowseInfo.Product.MaxSalePrice.F2ToString("f2"));
					Literal control3 = this.leftCount;
					num = groupBuy.MaxCount - soldCount;
					control3.SetWhenIsNotNull(num.ToString());
					int num2 = groupBuy.Count - soldCount;
					Literal control4 = this.minSuccessCount;
					num = ((num2 > 0) ? num2 : 0);
					control4.SetWhenIsNotNull(num.ToString());
					HtmlInputControl control5 = this.txtProductId;
					num = groupBuy.ProductId;
					control5.SetWhenIsNotNull(num.ToString());
					this.groupBuySoldCount.SetWhenIsNotNull(soldCount.ToString());
					Literal control6 = this.litConsultationsCount;
					num = productBrowseInfo.ConsultationCount;
					control6.SetWhenIsNotNull(num.ToString());
					HtmlInputHidden control7 = this.groupBuyMinCount;
					num = groupBuy.Count;
					control7.SetWhenIsNotNull(num.ToString());
					Literal control8 = this.litReviewsCount;
					num = productBrowseInfo.ReviewCount;
					control8.SetWhenIsNotNull(num.ToString());
					this.litGroupbuyDescription.SetWhenIsNotNull(groupBuy.Content);
					Literal control9 = this.litMaxCount;
					num = groupBuy.MaxCount;
					control9.SetWhenIsNotNull(num.ToString());
					this.nowTime = (HtmlInputHidden)this.FindControl("nowTime");
					HtmlInputHidden control10 = this.nowTime;
					DateTime dateTime = DateTime.Now;
					control10.SetWhenIsNotNull(dateTime.ToString("yyyy/MM/dd HH:mm:ss", DateTimeFormatInfo.InvariantInfo));
					HtmlInputHidden control11 = this.startTime;
					dateTime = groupBuy.StartDate;
					control11.SetWhenIsNotNull(dateTime.ToString("yyyy/MM/dd HH:mm:ss"));
					HtmlInputHidden control12 = this.endTime;
					dateTime = groupBuy.EndDate;
					control12.SetWhenIsNotNull(dateTime.ToString("yyyy/MM/dd HH:mm:ss"));
					HtmlInputHidden control13 = this.groupBuyMaxCount;
					num = groupBuy.MaxCount;
					control13.SetWhenIsNotNull(num.ToString());
					HtmlInputHidden control14 = this.skuStock;
					num = productBrowseInfo.Product.DefaultSku.Stock;
					control14.SetWhenIsNotNull(num.ToString());
					this.litUnit1.SetWhenIsNotNull(productBrowseInfo.Product.Unit);
					this.litUnit2.SetWhenIsNotNull(productBrowseInfo.Product.Unit);
					this.litUnit3.SetWhenIsNotNull(productBrowseInfo.Product.Unit);
				}
			}
		}
	}
}
