using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Commodities;
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
	public class MemberGroupDetailsStatus : WAPTemplatedWebControl
	{
		private Image imgStatus;

		private Literal litJoinNumber;

		private Literal litLimitedHour;

		private Literal litEndDate;

		private Literal litProductName;

		private Literal litFGAPrice;

		private Literal litPPrice;

		private Literal litProductReviewCount;

		private Literal litConsultationsCount;

		private Literal litDescription;

		private WapTemplatedRepeater rptMemberGroupDetailsStatus;

		private int fightGroupId;

		private HtmlInputHidden hfEndTime;

		private HtmlInputHidden hfNowTime;

		private HtmlInputHidden hfStartTime;

		private HyperLink hlProductReview;

		private Common_SKUSubmitOrder skuSubmitOrder;

		private Common_FightGroupRule fightGroupRule;

		private WapTemplatedRepeater rptProductImages;

		private WapTemplatedRepeater rptProductConsultations;

		private HtmlGenericControl divConsultationEmpty;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-MemberGroupDetailsStatus.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.divConsultationEmpty = (HtmlGenericControl)this.FindControl("divConsultationEmpty");
			this.rptProductConsultations = (WapTemplatedRepeater)this.FindControl("rptProductConsultations");
			this.fightGroupRule = (Common_FightGroupRule)this.FindControl("fightGroupRule");
			this.fightGroupId = this.Page.Request["fightGroupId"].ToInt(0);
			this.skuSubmitOrder = (Common_SKUSubmitOrder)this.FindControl("skuSubmitOrder");
			this.litDescription = (Literal)this.FindControl("litDescription");
			this.litConsultationsCount = (Literal)this.FindControl("litConsultationsCount");
			this.litJoinNumber = (Literal)this.FindControl("litJoinNumber");
			this.litLimitedHour = (Literal)this.FindControl("litLimitedHour");
			this.litEndDate = (Literal)this.FindControl("litEndDate");
			this.litProductName = (Literal)this.FindControl("litProductName");
			this.litFGAPrice = (Literal)this.FindControl("litFGAPrice");
			this.litPPrice = (Literal)this.FindControl("litPPrice");
			this.rptProductImages = (WapTemplatedRepeater)this.FindControl("rptProductImages");
			this.litProductReviewCount = (Literal)this.FindControl("litProductReviewCount");
			this.imgStatus = (Image)this.FindControl("imgStatus");
			this.hlProductReview = (HyperLink)this.FindControl("hlProductReview");
			this.rptMemberGroupDetailsStatus = (WapTemplatedRepeater)this.FindControl("rptMemberGroupDetailsStatus");
			FightGroupInfo fightGroup = VShopHelper.GetFightGroup(this.fightGroupId);
			if (fightGroup != null)
			{
				if (fightGroup.EndTime <= DateTime.Now && fightGroup.Status == FightGroupStatus.FightGroupIn)
				{
					VShopHelper.DealFightGroupFail(fightGroup.FightGroupId);
					fightGroup.Status = FightGroupStatus.FightGroupFail;
				}
				FightGroupActivityInfo fightGroupActivitieInfo = TradeHelper.GetFightGroupActivitieInfo(fightGroup.FightGroupActivityId);
				this.fightGroupRule.FightGroupActivityId = fightGroup.FightGroupActivityId;
				FightGroupStatus status = fightGroup.Status;
				if (status.Equals(FightGroupStatus.FightGroupFail))
				{
					this.imgStatus.ImageUrl = "/Templates/common/images/fg_fail.png";
					this.imgStatus.Attributes.Add("class", "fg_fail");
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
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				IList<FightGroupSkuInfo> fightGroupSkus = VShopHelper.GetFightGroupSkus(fightGroup.FightGroupActivityId);
				this.hlProductReview.NavigateUrl = $"/vshop/ProductReview.aspx?ProductId={fightGroup.ProductId}";
				Literal literal = this.litJoinNumber;
				int num = fightGroup.JoinNumber;
				literal.Text = num.ToString();
				Literal literal2 = this.litLimitedHour;
				num = fightGroupActivitieInfo.LimitedHour;
				literal2.Text = num.ToString();
				this.litEndDate.Text = fightGroup.EndTime.ToString("yy.MM.dd");
				this.litProductName.Text = fightGroup.ProductName;
				this.litFGAPrice.Text = fightGroupSkus.Min((FightGroupSkuInfo c) => c.SalePrice).F2ToString("f2");
				IList<int> list = null;
				Dictionary<int, IList<int>> dictionary = default(Dictionary<int, IList<int>>);
				ProductInfo productDetails = ProductHelper.GetProductDetails(fightGroup.ProductId, out dictionary, out list);
				if (productDetails != null)
				{
					this.litPPrice.Text = productDetails.MaxSalePrice.F2ToString("f2");
					this.skuSubmitOrder.ProductInfo = productDetails;
				}
				Literal literal3 = this.litProductReviewCount;
				num = ProductBrowser.GetProductReviews(new ProductReviewQuery
				{
					PageIndex = 1,
					PageSize = 2147483647,
					ProductId = fightGroup.ProductId
				}).TotalRecords;
				literal3.Text = num.ToString();
				IList<FightGroupUserModel> fightGroupUsers = VShopHelper.GetFightGroupUsers(fightGroup.FightGroupId);
				int num2 = fightGroupUsers.Count();
				if (fightGroupUsers.Count < fightGroup.JoinNumber)
				{
					for (int i = 0; i < fightGroup.JoinNumber - num2; i++)
					{
						FightGroupUserModel item2 = new FightGroupUserModel();
						fightGroupUsers.Add(item2);
					}
				}
				this.rptMemberGroupDetailsStatus.DataSource = fightGroupUsers;
				this.rptMemberGroupDetailsStatus.DataBind();
				Literal control = this.litConsultationsCount;
				num = ProductBrowser.GetProductConsultationsCount(productDetails.ProductId, false);
				control.SetWhenIsNotNull(num.ToString());
				if (this.litDescription != null)
				{
					Regex regex = new Regex("<script[^>]*?>.*?</script>", RegexOptions.IgnoreCase);
					if (!string.IsNullOrWhiteSpace(productDetails.MobbileDescription))
					{
						this.litDescription.Text = regex.Replace(productDetails.MobbileDescription, "");
					}
					else if (!string.IsNullOrWhiteSpace(productDetails.Description))
					{
						this.litDescription.Text = regex.Replace(productDetails.Description, "");
					}
				}
				this.skuSubmitOrder.FightGroupActivityId = fightGroupActivitieInfo.FightGroupActivityId;
				if (this.rptProductImages != null)
				{
					string locationUrl = "javascript:;";
					if (string.IsNullOrEmpty(productDetails.ImageUrl1) && string.IsNullOrEmpty(productDetails.ImageUrl2) && string.IsNullOrEmpty(productDetails.ImageUrl3) && string.IsNullOrEmpty(productDetails.ImageUrl4) && string.IsNullOrEmpty(productDetails.ImageUrl5))
					{
						productDetails.ImageUrl1 = masterSettings.DefaultProductImage;
					}
					DataTable skus = ProductBrowser.GetSkus(fightGroupActivitieInfo.ProductId);
					List<SlideImage> list2 = new List<SlideImage>();
					foreach (DataRow row in skus.Rows)
					{
						List<SlideImage> list3 = (from s in list2
						where s.ImageUrl == row["ThumbnailUrl410"].ToString()
						select s).ToList();
						if (list3.Count <= 0)
						{
							list2.Add(new SlideImage(row["ThumbnailUrl410"].ToString(), locationUrl));
						}
					}
					list2.Add(new SlideImage(productDetails.ImageUrl1, locationUrl));
					list2.Add(new SlideImage(productDetails.ImageUrl2, locationUrl));
					list2.Add(new SlideImage(productDetails.ImageUrl3, locationUrl));
					list2.Add(new SlideImage(productDetails.ImageUrl4, locationUrl));
					list2.Add(new SlideImage(productDetails.ImageUrl5, locationUrl));
					this.rptProductImages.DataSource = from item in list2
					where !string.IsNullOrWhiteSpace(item.ImageUrl)
					select item;
					this.rptProductImages.DataBind();
					Literal control2 = this.litConsultationsCount;
					num = ProductBrowser.GetProductConsultationsCount(productDetails.ProductId, false);
					control2.SetWhenIsNotNull(num.ToString());
					if (this.litDescription != null)
					{
						string text = "";
						Regex regex2 = new Regex("<script[^>]*?>.*?</script>", RegexOptions.IgnoreCase);
						if (!string.IsNullOrWhiteSpace(productDetails.MobbileDescription))
						{
							text = regex2.Replace(productDetails.MobbileDescription, "");
						}
						else if (!string.IsNullOrWhiteSpace(productDetails.Description))
						{
							text = regex2.Replace(productDetails.Description, "");
						}
						text = text.Replace("src", "data-url");
						this.litDescription.Text = text;
					}
				}
				DataTable data = ProductBrowser.GetProductConsultations(new ProductConsultationAndReplyQuery
				{
					ProductId = productDetails.ProductId,
					PageIndex = 1,
					PageSize = 2147483647,
					SortOrder = SortAction.Desc,
					SortBy = "ConsultationId",
					Type = ConsultationReplyType.Replyed
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
	}
}
