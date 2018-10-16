using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Helper;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Promotions;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class WAPCountDownStoreProductDetail : WAPTemplatedWebControl
	{
		private int countDownId;

		private int storeId;

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

		private Literal litPdImgSlidess;

		private Literal leftCount;

		private Literal minSuccessCount;

		private HtmlInputControl txtProductId;

		private Literal ltlStoreName;

		private Literal ltlStoreAddress;

		private Literal ltlDistance;

		private Literal ltlDelivery;

		private Literal ltlDeliveryDetail;

		private Literal litConsultationsCount;

		private Literal litGroupbuyDescription;

		private Literal litPdImgSlides;

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

		private HtmlInputHidden hidRecommend;

		private HtmlInputHidden hidNoData;

		private HtmlInputHidden hidStoreType;

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

		private HtmlInputHidden hidden_pdEx;

		private HtmlInputHidden hidStoreId;

		private HtmlInputHidden hidLngLat;

		private HtmlInputHidden hidStoreName;

		private HtmlInputHidden hidQQMapKey;

		private HtmlInputHidden hidLanLng;

		private Common_SKUSubmitStoreOrder skuSubmitOrder;

		private HtmlGenericControl divShortDescription;

		private Common_ExpandAttributes expandAttr;

		protected override void OnInit(EventArgs e)
		{
			this.SkinName = ((this.SkinName == null) ? "Skin-VCountDownProductStoreDetail.html" : this.SkinName);
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.countDownId = this.Page.Request.QueryString["countDownId"].ToInt(0);
			this.storeId = this.Page.Request.QueryString["StoreId"].ToInt(0);
			CountDownInfo countDownInfo = PromoteHelper.GetCountDownInfo(this.countDownId, this.storeId);
			if (countDownInfo == null)
			{
				this.ShowWapMessage("抢购信息不存在", "default.aspx");
			}
			MemberInfo user = HiContext.Current.User;
			if (user.UserId != 0 && user.IsReferral() && string.IsNullOrEmpty(this.Page.Request.QueryString["ReferralUserId"]))
			{
				string text = HttpContext.Current.Request.Url.ToString();
				text = ((text.IndexOf("?") <= -1) ? (text + "?ReferralUserId=" + HiContext.Current.UserId) : (text + "&ReferralUserId=" + HiContext.Current.UserId));
				this.Page.Response.Redirect(text);
			}
			else
			{
				this.FindControls();
				this.SetControlsValue(countDownInfo);
				PageTitle.AddSiteNameTitle("限时抢购商品详情");
			}
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
			this.hidden_pdEx = (HtmlInputHidden)this.FindControl("hidden_pdEx");
			this.litLeftSeconds = (Literal)this.FindControl("leftSeconds");
			this.skuSubmitOrder = (Common_SKUSubmitStoreOrder)this.FindControl("skuSubmitOrder");
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
			this.hidLngLat = (HtmlInputHidden)this.FindControl("hidLngLat");
			this.litPdImgSlides = (Literal)this.FindControl("litPdImgSlides");
			this.hidNoData = (HtmlInputHidden)this.FindControl("hidNoData");
			this.hidRecommend = (HtmlInputHidden)this.FindControl("hidRecommend");
			this.hidStoreType = (HtmlInputHidden)this.FindControl("hidStoreType");
			this.hidStoreId = (HtmlInputHidden)this.FindControl("hidStoreId");
			this.hidStoreName = (HtmlInputHidden)this.FindControl("hidStoreName");
			this.hidQQMapKey = (HtmlInputHidden)this.FindControl("hidQQMapKey");
			this.hidLanLng = (HtmlInputHidden)this.FindControl("hidLanLng");
			this.expandAttr = (Common_ExpandAttributes)this.FindControl("ExpandAttributes");
			this.litUnit = (Literal)this.FindControl("litUnit");
		}

		private void SetControlsValue(CountDownInfo countDownInfo)
		{
			this.skuSubmitOrder.CountDownId = countDownInfo.CountDownId;
			this.skuSubmitOrder.CountDownInfo = countDownInfo;
			this.skuSubmitOrder.OrderBusiness = 2;
			HtmlInputHidden htmlInputHidden = this.hdCountDownId;
			int num = countDownInfo.CountDownId;
			htmlInputHidden.Value = num.ToString();
			this.hiddenIsLogin.Value = HiContext.Current.UserId.ToString();
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			this.hidRecommend.Value = (masterSettings.Store_IsRecommend ? "1" : "0");
			StoreProductQuery storeProductQuery = new StoreProductQuery
			{
				ProductId = countDownInfo.ProductId,
				StoreId = this.storeId,
				CountDownId = this.countDownId
			};
			string cookie = WebHelper.GetCookie("UserCoordinateCookie", "NewCoordinate");
			if (!string.IsNullOrEmpty(cookie))
			{
				string[] array = cookie.Split(',');
				storeProductQuery.Position = new PositionInfo(array[0].ToDouble(0), array[1].ToDouble(0));
				storeProductQuery.Position.CityId = WebHelper.GetCookie("UserCoordinateCookie", "CityRegionId").ToInt(0);
				storeProductQuery.Position.AreaId = WebHelper.GetCookie("UserCoordinateCookie", "RegionId").ToInt(0);
				StoreEntityQuery storeEntityQuery = new StoreEntityQuery();
				storeEntityQuery.Position = new PositionInfo(array[0].ToDouble(0), array[1].ToDouble(0));
				storeEntityQuery.RegionId = WebHelper.GetCookie("UserCoordinateCookie", "CityRegionId").ToInt(0);
				storeEntityQuery.FullAreaPath = WebHelper.GetCookie("UserCoordinateCookie", "FullRegionPath");
				this.hidLanLng.Value = JsonConvert.SerializeObject(storeEntityQuery);
			}
			else
			{
				storeProductQuery.Position = new PositionInfo(0.0, 0.0);
				storeProductQuery.Position.CityId = 0;
				storeProductQuery.Position.AreaId = 0;
			}
			this.hidStoreId.Value = this.storeId.ToString();
			HtmlInputHidden htmlInputHidden2 = this.hidStoreType;
			num = countDownInfo.StoreType;
			htmlInputHidden2.Value = num.ToString();
			ProductModel storeProduct = ProductBrowser.GetStoreProduct(storeProductQuery);
			if (storeProduct == null || storeProduct.SaleStatus == ProductSaleStatus.Delete)
			{
				this.ShowWapMessage("抢购商品已不存在", "Default.aspx");
			}
			else
			{
				if (!storeProduct.SaleStatus.Equals(ProductSaleStatus.OnSale))
				{
					this.hidden_IsOver.Value = "pullOff";
				}
				this.litShortDescription.Text = storeProduct.ShortDescription;
				this.divShortDescription.Visible = !string.IsNullOrEmpty(storeProduct.ShortDescription);
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
				if (storeProduct.SaleStatus != ProductSaleStatus.OnSale)
				{
					this.ShowWapMessage("此商品已下架", "Default.aspx");
				}
				if (this.expandAttr != null)
				{
					this.expandAttr.ProductId = storeProduct.ProductId;
				}
				this.skuSubmitOrder.ProductInfo = storeProduct;
				this.litProdcutName.SetWhenIsNotNull(storeProduct.ProductName);
				Literal control = this.litminCount;
				num = countDownInfo.MaxCount;
				control.SetWhenIsNotNull(num.ToString());
				storeProduct.ImgUrlList.ForEach(delegate(string i)
				{
					Literal literal4 = this.litPdImgSlides;
					literal4.Text += $"<img src=\"{i}\" />";
				});
				if (this.litDescription != null && !string.IsNullOrWhiteSpace(storeProduct.Description))
				{
					Regex regex = new Regex("<script[^>]*?>.*?</script>", RegexOptions.IgnoreCase);
					this.litDescription.Text = regex.Replace(storeProduct.Description, "").Replace("src", "data-url");
				}
				this.litprice.SetWhenIsNotNull(PromoteHelper.GetCountDownSalePrice(this.countDownId).F2ToString("f2"));
				this.litLeftSeconds.SetWhenIsNotNull(Math.Ceiling((countDownInfo.EndDate - DateTime.Now).TotalSeconds).ToString());
				this.litcontent.SetWhenIsNotNull(countDownInfo.Content);
				HtmlInputControl control2 = this.litGroupBuyId;
				num = countDownInfo.CountDownId;
				control2.SetWhenIsNotNull(num.ToString());
				if (storeProduct.MarketPrice > decimal.Zero)
				{
					this.salePrice.SetWhenIsNotNull(storeProduct.MarketPrice.F2ToString("f2"));
				}
				else
				{
					this.salePrice.Text = storeProduct.MaxSalePrice.F2ToString("f2");
				}
				HtmlInputControl control3 = this.txtProductId;
				num = countDownInfo.ProductId;
				control3.SetWhenIsNotNull(num.ToString());
				Literal control4 = this.litConsultationsCount;
				num = storeProduct.ConsultationCount;
				control4.SetWhenIsNotNull(num.ToString());
				Literal control5 = this.litReviewsCount;
				num = storeProduct.ReviewCount;
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
				if (storeProduct.StoreInfo.IsInServiceArea)
				{
					this.ltlStoreName = (Literal)this.FindControl("ltlStoreName");
					this.ltlStoreName.Text = storeProduct.StoreInfo.StoreName;
					this.ltlDistance = (Literal)this.FindControl("ltlDistance");
					this.ltlDistance.Text = storeProduct.StoreInfo.Distance;
					this.ltlDelivery = (Literal)this.FindControl("ltlDelivery");
					this.ltlDelivery.Text = storeProduct.StoreInfo.Delivery.DeliveryList.Aggregate(string.Empty, (string t, string n) => "<li>" + t + "</li><li>" + n + "</li>");
					this.ltlDeliveryDetail = (Literal)this.FindControl("ltlDeliveryDetail");
					if (storeProduct.StoreInfo.Delivery.IsStoreDelive)
					{
						if (storeProduct.StoreInfo.Delivery.MinOrderPrice > decimal.Zero)
						{
							Literal literal = this.ltlDeliveryDetail;
							num = storeProduct.StoreInfo.Delivery.MinOrderPrice.ToInt(0);
							literal.Text = $"￥<em>{num.ToString()}</em>起送";
							if (storeProduct.StoreInfo.Delivery.StoreFreight > decimal.Zero)
							{
								Literal literal2 = this.ltlDeliveryDetail;
								string text = literal2.Text;
								num = storeProduct.StoreInfo.Delivery.StoreFreight.ToInt(0);
								literal2.Text = text + $",配送费￥<em>{num.ToString()}</em>";
							}
							else
							{
								Literal literal3 = this.ltlDeliveryDetail;
								literal3.Text += string.Format(",免配送费");
							}
						}
						else if (storeProduct.StoreInfo.Delivery.StoreFreight > decimal.Zero)
						{
							this.ltlDeliveryDetail.Text = $"配送费￥<em>{storeProduct.StoreInfo.Delivery.StoreFreight.ToInt(0)}</em>";
						}
						else
						{
							this.ltlDeliveryDetail.Text = string.Format("免配送费");
						}
					}
					this.ltlStoreAddress = (Literal)this.FindControl("ltlStoreAddress");
					this.ltlStoreAddress.Text = storeProduct.StoreInfo.AddressSimply;
					this.hidLngLat.Value = $"{storeProduct.StoreInfo.Position.Longitude},{storeProduct.StoreInfo.Position.Latitude}";
					this.hidStoreName.Value = storeProduct.StoreInfo.StoreName;
					this.hidQQMapKey.Value = masterSettings.QQMapAPIKey;
				}
				if (storeProduct.ExStatus == DetailException.Nomal)
				{
					this.hidNoData.Value = "0";
				}
				else
				{
					this.hidNoData.Value = "1";
				}
				this.litUnit.SetWhenIsNotNull(string.IsNullOrEmpty(storeProduct.Unit) ? "件" : storeProduct.Unit);
				this.SetWXShare(countDownInfo, storeProduct);
				this.ProcessException(storeProduct);
			}
		}

		private void ProcessException(ProductModel pd)
		{
			switch (pd.ExStatus)
			{
			case DetailException.StopService:
				this.hidden_pdEx.Value = string.Format("歇业中(营业时间：{0})", pd.StoreInfo.CloseEndTime.Value.ToString("yyyy年MM月dd号 HH:mm"));
				break;
			case DetailException.NoStock:
				this.hidden_pdEx.Value = "已售罄";
				break;
			case DetailException.OverServiceArea:
				this.hidden_pdEx.Value = "服务范围超区";
				break;
			case DetailException.IsNotWorkTime:
				this.hidden_pdEx.Value = "非营业时间";
				break;
			}
			if (pd.ExStatus != DetailException.Nomal)
			{
				this.hidden_IsOver.Value = "ex";
			}
		}

		private void SetWXShare(CountDownInfo countDown, ProductModel product)
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
			string local = string.IsNullOrEmpty(countDown.ShareIcon) ? ((product.ImgUrlList.Count == 0) ? SettingsManager.GetMasterSettings().DefaultProductImage : product.ImgUrlList[0]) : Globals.FullPath(countDown.ShareIcon);
			this.hdImgUrl.Value = Globals.FullPath(local);
			this.hdTitle.Value = (string.IsNullOrEmpty(countDown.ShareTitle) ? product.ProductName : countDown.ShareTitle);
			this.hdLink.Value = Globals.FullPath(this.Page.Request.Url.ToString());
		}
	}
}
