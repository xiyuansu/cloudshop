using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Supplier;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Store;
using Hidistro.SaleSystem.Supplier;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class FightGroupActivityDetails : WAPTemplatedWebControl
	{
		private Literal litJoinNumber;

		private Literal litLimitedHour;

		private Literal litEndDate;

		private Literal litProductName;

		private Literal litFGAPrice;

		private Literal litPPrice;

		private Literal litProductReviewCount;

		private Literal litMinPrice;

		private Literal litProductPrice;

		private Image imgEnd;

		private WapTemplatedRepeater rptFightGroups;

		private HyperLink hlProductReview;

		private HtmlControl divFightGroupsHead;

		private HtmlControl divGetBySelf;

		private int fightGroupActivityId;

		private StockLabel lblStock;

		private Literal litUnit;

		private Literal litConsultationsCount;

		private Literal litDescription;

		private Common_SKUSubmitOrder skuSubmitOrder;

		private WapTemplatedRepeater rptProductImages;

		private WapTemplatedRepeater rptProductConsultations;

		private Common_FightGroupRule fightGroupRule;

		private HtmlGenericControl divConsultationEmpty;

		private HtmlInputText hiddenProductId;

		private HtmlInputHidden hdAppId;

		private HtmlInputHidden hdTitle;

		private HtmlInputHidden hdDesc;

		private HtmlInputHidden hdImgUrl;

		private HtmlInputHidden hdLink;

		private HtmlInputHidden hidSupplier;

		private Literal litSupplierName;

		protected override void OnInit(EventArgs e)
		{
			if (base.ClientType == ClientType.VShop)
			{
				if (this.SkinName == null)
				{
					this.SkinName = "skin-FightGroupActivityDetails.html";
				}
				base.OnInit(e);
			}
			else
			{
				HiContext.Current.Context.Response.Redirect("OnlyWXOpenTip");
			}
		}

		protected override void AttachChildControls()
		{
			this.hidSupplier = (HtmlInputHidden)this.FindControl("hidSupplier");
			this.litSupplierName = (Literal)this.FindControl("litSupplierName");
			this.divConsultationEmpty = (HtmlGenericControl)this.FindControl("divConsultationEmpty");
			this.rptProductConsultations = (WapTemplatedRepeater)this.FindControl("rptProductConsultations");
			this.fightGroupRule = (Common_FightGroupRule)this.FindControl("fightGroupRule");
			this.litDescription = (Literal)this.FindControl("litDescription");
			this.litConsultationsCount = (Literal)this.FindControl("litConsultationsCount");
			this.rptProductImages = (WapTemplatedRepeater)this.FindControl("rptProductImages");
			this.divFightGroupsHead = (HtmlControl)this.FindControl("divFightGroupsHead");
			this.skuSubmitOrder = (Common_SKUSubmitOrder)this.FindControl("skuSubmitOrder");
			this.fightGroupActivityId = this.Page.Request["fightGroupActivityId"].ToInt(0);
			this.litJoinNumber = (Literal)this.FindControl("litJoinNumber");
			this.litLimitedHour = (Literal)this.FindControl("litLimitedHour");
			this.litEndDate = (Literal)this.FindControl("litEndDate");
			this.litProductName = (Literal)this.FindControl("litProductName");
			this.litFGAPrice = (Literal)this.FindControl("litFGAPrice");
			this.litPPrice = (Literal)this.FindControl("litPPrice");
			this.litProductReviewCount = (Literal)this.FindControl("litProductReviewCount");
			this.imgEnd = (Image)this.FindControl("imgEnd");
			this.hlProductReview = (HyperLink)this.FindControl("hlProductReview");
			this.rptFightGroups = (WapTemplatedRepeater)this.FindControl("rptFightGroups");
			this.litMinPrice = (Literal)this.FindControl("litMinPrice");
			this.litProductPrice = (Literal)this.FindControl("litProductPrice");
			this.lblStock = (StockLabel)this.FindControl("lblStock");
			this.litUnit = (Literal)this.FindControl("litUnit");
			this.divGetBySelf = (HtmlControl)this.FindControl("divGetBySelf");
			this.hiddenProductId = (HtmlInputText)this.FindControl("hiddenProductId");
			this.hdAppId = (HtmlInputHidden)this.FindControl("hdAppId");
			this.hdTitle = (HtmlInputHidden)this.FindControl("hdTitle");
			this.hdDesc = (HtmlInputHidden)this.FindControl("hdDesc");
			this.hdImgUrl = (HtmlInputHidden)this.FindControl("hdImgUrl");
			this.hdLink = (HtmlInputHidden)this.FindControl("hdLink");
			FightGroupActivityInfo fightGroupActivitieInfo = TradeHelper.GetFightGroupActivitieInfo(this.fightGroupActivityId);
			if (fightGroupActivitieInfo != null)
			{
				HtmlInputText htmlInputText = this.hiddenProductId;
				int num = fightGroupActivitieInfo.ProductId;
				htmlInputText.Value = num.ToString();
				this.fightGroupRule.FightGroupActivityId = fightGroupActivitieInfo.FightGroupActivityId;
				if (fightGroupActivitieInfo.StartDate > DateTime.Now)
				{
					this.Page.Response.Redirect("FightGroupActivityDetailsSoon.aspx?fightGroupActivityId=" + this.fightGroupActivityId);
				}
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				if (masterSettings.OpenMultStore && StoresHelper.ProductHasStores(fightGroupActivitieInfo.ProductId))
				{
					this.divGetBySelf.Visible = true;
				}
				this.skuSubmitOrder.FightGroupActivityId = fightGroupActivitieInfo.FightGroupActivityId;
				IList<FightGroupSkuInfo> fightGroupSkus = VShopHelper.GetFightGroupSkus(this.fightGroupActivityId);
				this.hlProductReview.NavigateUrl = $"/vshop/ProductReview.aspx?ProductId={fightGroupActivitieInfo.ProductId}";
				this.imgEnd.Visible = (fightGroupActivitieInfo.EndDate < DateTime.Now);
				Literal literal = this.litJoinNumber;
				num = fightGroupActivitieInfo.JoinNumber;
				literal.Text = num.ToString();
				Literal literal2 = this.litLimitedHour;
				num = fightGroupActivitieInfo.LimitedHour;
				literal2.Text = num.ToString();
				this.litEndDate.Text = fightGroupActivitieInfo.EndDate.ToString("yy.MM.dd");
				this.litProductName.Text = fightGroupActivitieInfo.ProductName;
				decimal num2 = fightGroupSkus.Min((FightGroupSkuInfo c) => c.SalePrice);
				this.litFGAPrice.Text = num2.F2ToString("f2");
				if (fightGroupSkus.Count() > 1)
				{
					decimal num3 = fightGroupSkus.Max((FightGroupSkuInfo c) => c.SalePrice);
					if (num3 > num2)
					{
						this.litFGAPrice.Text = num2.F2ToString("f2") + "～" + num3.F2ToString("f2");
					}
				}
				this.litMinPrice.Text = num2.F2ToString("f2");
				IList<int> list = null;
				Dictionary<int, IList<int>> dictionary = default(Dictionary<int, IList<int>>);
				ProductInfo productDetails = ProductHelper.GetProductDetails(fightGroupActivitieInfo.ProductId, out dictionary, out list);
				if (productDetails != null)
				{
					this.skuSubmitOrder.ProductInfo = productDetails;
					this.litPPrice.Text = MemberProcessor.GetMemberPrice(productDetails).F2ToString("f2");
					this.litProductPrice.Text = this.litPPrice.Text;
				}
				int supplierId = productDetails.SupplierId;
				if (supplierId > 0)
				{
					SupplierInfo supplierById = SupplierHelper.GetSupplierById(supplierId);
					if (supplierById != null)
					{
						this.hidSupplier.Value = "true";
						this.litSupplierName.Text = supplierById.SupplierName;
					}
				}
				else
				{
					this.hidSupplier.Value = "false";
				}
				Literal literal3 = this.litProductReviewCount;
				num = ProductBrowser.GetProductReviews(new ProductReviewQuery
				{
					PageIndex = 1,
					PageSize = 2147483647,
					ProductId = fightGroupActivitieInfo.ProductId
				}).TotalRecords;
				literal3.Text = num.ToString();
				DataTable fightGroups = VShopHelper.GetFightGroups(this.fightGroupActivityId);
				for (int i = 0; i < fightGroups.Rows.Count; i++)
				{
					fightGroups.Rows[i]["Name"] = DataHelper.GetHiddenUsername(fightGroups.Rows[i]["Name"].ToString());
				}
				this.divFightGroupsHead.Visible = (fightGroups.Rows.Count > 0);
				this.rptFightGroups.DataSource = fightGroups;
				this.rptFightGroups.DataBind();
				this.hdAppId.Value = base.site.WeixinAppId;
				if (string.IsNullOrEmpty(fightGroupActivitieInfo.ShareTitle))
				{
					this.hdTitle.Value = (string.IsNullOrEmpty(productDetails.Title) ? productDetails.ProductName : productDetails.Title);
				}
				else
				{
					this.hdTitle.Value = fightGroupActivitieInfo.ShareTitle.Trim();
				}
				if (string.IsNullOrEmpty(fightGroupActivitieInfo.ShareContent.Trim()))
				{
					this.hdDesc.Value = productDetails.Meta_Description;
				}
				else
				{
					this.hdDesc.Value = fightGroupActivitieInfo.ShareContent.Trim();
				}
				string local = string.IsNullOrEmpty(fightGroupActivitieInfo.Icon) ? SettingsManager.GetMasterSettings().DefaultProductImage : fightGroupActivitieInfo.Icon;
				this.hdImgUrl.Value = Globals.FullPath(local);
				this.hdLink.Value = Globals.FullPath(this.Page.Request.Url.ToString());
				if (this.rptProductImages != null)
				{
					string locationUrl = "javascript:;";
					if (string.IsNullOrEmpty(productDetails.ImageUrl1) && string.IsNullOrEmpty(productDetails.ImageUrl2) && string.IsNullOrEmpty(productDetails.ImageUrl3) && string.IsNullOrEmpty(productDetails.ImageUrl4) && string.IsNullOrEmpty(productDetails.ImageUrl5))
					{
						productDetails.ImageUrl1 = masterSettings.DefaultProductImage;
					}
					DataTable skus = ProductBrowser.GetSkus(fightGroupActivitieInfo.ProductId);
					List<SlideImage> list2 = new List<SlideImage>();
					list2.Add(new SlideImage(productDetails.ImageUrl1, locationUrl));
					list2.Add(new SlideImage(productDetails.ImageUrl2, locationUrl));
					list2.Add(new SlideImage(productDetails.ImageUrl3, locationUrl));
					list2.Add(new SlideImage(productDetails.ImageUrl4, locationUrl));
					list2.Add(new SlideImage(productDetails.ImageUrl5, locationUrl));
					this.rptProductImages.DataSource = from item in list2
					where !string.IsNullOrWhiteSpace(item.ImageUrl)
					select item;
					this.rptProductImages.DataBind();
					Literal control = this.litConsultationsCount;
					num = ProductBrowser.GetProductConsultationsCount(productDetails.ProductId, false);
					control.SetWhenIsNotNull(num.ToString());
					if (this.litDescription != null)
					{
						string text = "";
						Regex regex = new Regex("<script[^>]*?>.*?</script>", RegexOptions.IgnoreCase);
						if (!string.IsNullOrWhiteSpace(productDetails.MobbileDescription))
						{
							text = regex.Replace(productDetails.MobbileDescription, "");
						}
						else if (!string.IsNullOrWhiteSpace(productDetails.Description))
						{
							text = regex.Replace(productDetails.Description, "");
						}
						text = text.Replace("src", "data-url");
						this.litDescription.Text = text;
					}
					DataTable data = ProductBrowser.GetProductConsultations(new ProductConsultationAndReplyQuery
					{
						ProductId = productDetails.ProductId,
						PageIndex = 1,
						PageSize = 2147483647,
						SortOrder = SortAction.Desc,
						SortBy = "ConsultationId",
						HasReplied = true
					}).Data;
					for (int j = 0; j < data.Rows.Count; j++)
					{
						data.Rows[j]["UserName"] = DataHelper.GetHiddenUsername(data.Rows[j]["UserName"].ToNullString());
					}
					this.rptProductConsultations.DataSource = data;
					this.rptProductConsultations.DataBind();
					this.divConsultationEmpty.Visible = data.IsNullOrEmpty();
				}
			}
			else
			{
				base.GotoResourceNotFound("活动不存在");
			}
		}
	}
}
