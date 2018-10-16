using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Store;
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
	public class FightGroupDetails : WAPTemplatedWebControl
	{
		private Common_SKUSubmitOrder skuSubmitOrder;

		private Literal litJoinNumber;

		private Literal litLimitedHour;

		private Literal litProductName;

		private Literal litFGAPrice;

		private Literal litPPrice;

		private Literal litProductReviewCount;

		private Literal litNeedPerson;

		private Literal litConsultationsCount;

		private Literal litDescription;

		private Image imgStatus;

		private HyperLink hlProductReview;

		private HtmlControl divIn;

		private HtmlControl divOver;

		private HtmlControl divGetBySelf;

		private WapTemplatedRepeater rptFightGroupDetails;

		private WapTemplatedRepeater rptFightGroupDetailsNeed;

		private HtmlInputHidden hfEndTime;

		private HtmlInputHidden hfNowTime;

		private HtmlInputHidden hfStartTime;

		private WapTemplatedRepeater rptProductImages;

		private WapTemplatedRepeater rptProductConsultations;

		private Common_FightGroupRule fightGroupRule;

		private HtmlGenericControl divConsultationEmpty;

		private HtmlInputText hiddenProductId;

		private HtmlInputText hiddenJoinedUser;

		private HtmlInputHidden hdAppId;

		private HtmlInputHidden hdTitle;

		private HtmlInputHidden hdDesc;

		private HtmlInputHidden hdImgUrl;

		private HtmlInputHidden hdLink;

		private HtmlInputHidden hidFightGroupActivityStatus;

		private HtmlGenericControl spanTime;

		private HtmlGenericControl litRemainTimeHtml;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-FightGroupDetails.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.divConsultationEmpty = (HtmlGenericControl)this.FindControl("divConsultationEmpty");
			this.rptProductConsultations = (WapTemplatedRepeater)this.FindControl("rptProductConsultations");
			this.fightGroupRule = (Common_FightGroupRule)this.FindControl("fightGroupRule");
			this.rptProductImages = (WapTemplatedRepeater)this.FindControl("rptProductImages");
			int fightGroupId = this.Page.Request["fightGroupId"].ToInt(0);
			this.litDescription = (Literal)this.FindControl("litDescription");
			this.litConsultationsCount = (Literal)this.FindControl("litConsultationsCount");
			this.hfStartTime = (HtmlInputHidden)this.FindControl("hfStartTime");
			this.hiddenJoinedUser = (HtmlInputText)this.FindControl("hiddenJoinedUser");
			this.hfEndTime = (HtmlInputHidden)this.FindControl("hfEndTime");
			this.hfNowTime = (HtmlInputHidden)this.FindControl("hfNowTime");
			this.rptFightGroupDetails = (WapTemplatedRepeater)this.FindControl("rptFightGroupDetails");
			this.rptFightGroupDetailsNeed = (WapTemplatedRepeater)this.FindControl("rptFightGroupDetailsNeed");
			this.litNeedPerson = (Literal)this.FindControl("litNeedPerson");
			this.divIn = (HtmlControl)this.FindControl("divIn");
			this.divOver = (HtmlControl)this.FindControl("divOver");
			this.skuSubmitOrder = (Common_SKUSubmitOrder)this.FindControl("skuSubmitOrder");
			this.litProductReviewCount = (Literal)this.FindControl("litProductReviewCount");
			this.litJoinNumber = (Literal)this.FindControl("litJoinNumber");
			this.litLimitedHour = (Literal)this.FindControl("litLimitedHour");
			this.litProductName = (Literal)this.FindControl("litProductName");
			this.litFGAPrice = (Literal)this.FindControl("litFGAPrice");
			this.litPPrice = (Literal)this.FindControl("litPPrice");
			this.imgStatus = (Image)this.FindControl("imgStatus");
			this.litRemainTimeHtml = (HtmlGenericControl)this.FindControl("litRemainTimeHtml");
			this.spanTime = (HtmlGenericControl)this.FindControl("spanTime");
			this.hiddenProductId = (HtmlInputText)this.FindControl("hiddenProductId");
			this.hlProductReview = (HyperLink)this.FindControl("hlProductReview");
			this.hidFightGroupActivityStatus = (HtmlInputHidden)this.FindControl("hidFightGroupActivityStatus");
			this.divGetBySelf = (HtmlControl)this.FindControl("divGetBySelf");
			this.hdAppId = (HtmlInputHidden)this.FindControl("hdAppId");
			this.hdTitle = (HtmlInputHidden)this.FindControl("hdTitle");
			this.hdDesc = (HtmlInputHidden)this.FindControl("hdDesc");
			this.hdImgUrl = (HtmlInputHidden)this.FindControl("hdImgUrl");
			this.hdLink = (HtmlInputHidden)this.FindControl("hdLink");
			this.hdAppId.Value = base.site.WeixinAppId;
			FightGroupInfo fightGroup = VShopHelper.GetFightGroup(fightGroupId);
			if (fightGroup != null)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				if (masterSettings.OpenMultStore && StoresHelper.ProductHasStores(fightGroup.ProductId))
				{
					this.divGetBySelf.Visible = true;
				}
				if (fightGroup.EndTime <= DateTime.Now && fightGroup.Status == FightGroupStatus.FightGroupIn)
				{
					VShopHelper.DealFightGroupFail(fightGroupId);
					fightGroup.Status = FightGroupStatus.FightGroupFail;
				}
				this.fightGroupRule.FightGroupActivityId = fightGroup.FightGroupActivityId;
				FightGroupActivityInfo fightGroupActivitieInfo = TradeHelper.GetFightGroupActivitieInfo(fightGroup.FightGroupActivityId);
				HtmlInputText htmlInputText = this.hiddenProductId;
				int num = fightGroup.ProductId;
				htmlInputText.Value = num.ToString();
				this.hidFightGroupActivityStatus.Value = ((fightGroupActivitieInfo.EndDate > DateTime.Now) ? "1" : "0");
				HtmlInputHidden htmlInputHidden = this.hfNowTime;
				DateTime dateTime = DateTime.Now;
				htmlInputHidden.Value = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
				HtmlInputHidden htmlInputHidden2 = this.hfEndTime;
				dateTime = fightGroup.EndTime;
				htmlInputHidden2.Value = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
				HtmlInputHidden htmlInputHidden3 = this.hfStartTime;
				dateTime = fightGroup.StartTime;
				htmlInputHidden3.Value = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
				this.hlProductReview.NavigateUrl = $"/vshop/ProductReview.aspx?ProductId={fightGroup.ProductId}";
				IList<FightGroupSkuInfo> fightGroupSkus = VShopHelper.GetFightGroupSkus(fightGroup.FightGroupActivityId);
				Literal literal = this.litJoinNumber;
				num = fightGroup.JoinNumber;
				literal.Text = num.ToString();
				Literal literal2 = this.litLimitedHour;
				num = fightGroupActivitieInfo.LimitedHour;
				literal2.Text = num.ToString();
				this.litProductName.Text = fightGroup.ProductName;
				this.litFGAPrice.Text = fightGroupSkus.Min((FightGroupSkuInfo c) => c.SalePrice).F2ToString("f2");
				IList<int> list = null;
				Dictionary<int, IList<int>> dictionary = default(Dictionary<int, IList<int>>);
				ProductInfo productDetails = ProductHelper.GetProductDetails(fightGroup.ProductId, out dictionary, out list);
				if (productDetails != null)
				{
					this.litPPrice.Text = productDetails.MaxSalePrice.F2ToString("f2");
					this.skuSubmitOrder.ProductInfo = productDetails;
					this.skuSubmitOrder.OrderBusiness = 1;
					this.skuSubmitOrder.FightGroupActivityId = fightGroup.FightGroupActivityId;
					this.skuSubmitOrder.FightGroupId = fightGroup.FightGroupId;
				}
				Literal literal3 = this.litProductReviewCount;
				num = ProductBrowser.GetProductReviews(new ProductReviewQuery
				{
					PageIndex = 1,
					PageSize = 2147483647,
					ProductId = fightGroupActivitieInfo.ProductId
				}).TotalRecords;
				literal3.Text = num.ToString();
				FightGroupStatus status = fightGroup.Status;
				if (status.Equals(FightGroupStatus.FightGroupFail))
				{
					this.imgStatus.ImageUrl = "/Templates/common/images/fg_fail.png";
					this.imgStatus.Attributes.Add("class", "fg_fail");
					this.spanTime.Style.Add("text-align", "center");
					this.litRemainTimeHtml.Style.Add("display", "none");
				}
				else
				{
					status = fightGroup.Status;
					if (status.Equals(FightGroupStatus.FightGroupSuccess))
					{
						this.imgStatus.ImageUrl = "/Templates/common/images/fg_ok.png";
						this.imgStatus.Attributes.Add("class", "fg_ok");
					}
				}
				Image image = this.imgStatus;
				status = fightGroup.Status;
				image.Visible = !status.Equals(FightGroupStatus.FightGroupIn);
				int fightGroupActiveNumber = VShopHelper.GetFightGroupActiveNumber(fightGroup.FightGroupId);
				int num2 = fightGroup.JoinNumber - fightGroupActiveNumber;
				num2 = ((num2 >= 0) ? num2 : 0);
				this.litNeedPerson.Text = num2.ToString();
				HtmlControl htmlControl = this.divIn;
				status = fightGroup.Status;
				htmlControl.Visible = status.Equals(FightGroupStatus.FightGroupIn);
				HtmlControl htmlControl2 = this.divOver;
				status = fightGroup.Status;
				htmlControl2.Visible = !status.Equals(FightGroupStatus.FightGroupIn);
				IList<FightGroupUserModel> fightGroupUsers = VShopHelper.GetFightGroupUsers(fightGroup.FightGroupId);
				foreach (FightGroupUserModel item in fightGroupUsers)
				{
					item.Name = DataHelper.GetHiddenUsername(item.Name);
					if (HiContext.Current.UserId == item.UserId)
					{
						this.hiddenJoinedUser.Value = "true";
					}
				}
				this.rptFightGroupDetails.DataSource = fightGroupUsers;
				this.rptFightGroupDetails.DataBind();
				List<int> list2 = new List<int>();
				for (int i = 0; i < num2; i++)
				{
					list2.Add(i);
				}
				this.rptFightGroupDetailsNeed.DataSource = list2;
				this.rptFightGroupDetailsNeed.DataBind();
				Literal control = this.litConsultationsCount;
				num = ProductBrowser.GetProductConsultationsCount(productDetails.ProductId, true);
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
				if (this.rptProductImages != null)
				{
					string locationUrl = "javascript:;";
					if (string.IsNullOrEmpty(productDetails.ImageUrl1) && string.IsNullOrEmpty(productDetails.ImageUrl2) && string.IsNullOrEmpty(productDetails.ImageUrl3) && string.IsNullOrEmpty(productDetails.ImageUrl4) && string.IsNullOrEmpty(productDetails.ImageUrl5))
					{
						productDetails.ImageUrl1 = masterSettings.DefaultProductImage;
					}
					DataTable skus = ProductBrowser.GetSkus(fightGroupActivitieInfo.ProductId);
					List<SlideImage> list3 = new List<SlideImage>();
					foreach (DataRow row in skus.Rows)
					{
						List<SlideImage> list4 = (from s in list3
						where s.ImageUrl == row["ThumbnailUrl410"].ToString()
						select s).ToList();
						if (list4.Count <= 0)
						{
							list3.Add(new SlideImage(row["ThumbnailUrl410"].ToString(), locationUrl));
						}
					}
					list3.Add(new SlideImage(productDetails.ImageUrl1, locationUrl));
					list3.Add(new SlideImage(productDetails.ImageUrl2, locationUrl));
					list3.Add(new SlideImage(productDetails.ImageUrl3, locationUrl));
					list3.Add(new SlideImage(productDetails.ImageUrl4, locationUrl));
					list3.Add(new SlideImage(productDetails.ImageUrl5, locationUrl));
					this.rptProductImages.DataSource = from item in list3
					where !string.IsNullOrWhiteSpace(item.ImageUrl)
					select item;
					this.rptProductImages.DataBind();
				}
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
				string icon = fightGroupActivitieInfo.Icon;
				this.hdImgUrl.Value = Globals.FullPath(string.IsNullOrEmpty(icon) ? base.site.DefaultProductThumbnail8 : icon);
				this.hdLink.Value = Globals.FullPath(this.Page.Request.Url.ToString());
				DataTable data = ProductBrowser.GetProductConsultations(new ProductConsultationAndReplyQuery
				{
					ProductId = productDetails.ProductId,
					PageIndex = 1,
					PageSize = 2147483647,
					SortOrder = SortAction.Desc,
					SortBy = "ConsultationId"
				}).Data;
				for (int j = 0; j < data.Rows.Count; j++)
				{
					data.Rows[j]["UserName"] = DataHelper.GetHiddenUsername(data.Rows[j]["UserName"].ToNullString());
				}
				this.rptProductConsultations.DataSource = data;
				this.rptProductConsultations.DataBind();
				this.divConsultationEmpty.Visible = data.IsNullOrEmpty();
			}
			else
			{
				base.GotoResourceNotFound("活动不存在");
			}
		}
	}
}
