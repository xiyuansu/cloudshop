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
	public class MemberGroupDetails : WAPTemplatedWebControl
	{
		private Image imgThumbnailUrl100;

		private Literal litProductName;

		private Literal litSalePrice;

		private Literal litNeedPerson;

		private Literal litProductReviewCount;

		private Literal litConsultationsCount;

		private Literal litDescription;

		private WapTemplatedRepeater rptFightGroups;

		private WapTemplatedRepeater rptProductConsultations;

		private int fightGroupId;

		private HtmlInputHidden hfEndTime;

		private HtmlInputHidden hfNowTime;

		private HtmlInputHidden hfStartTime;

		private HtmlInputHidden hdTitle;

		private HtmlInputHidden hdDesc;

		private HtmlInputHidden hdAppId;

		private HtmlInputHidden hdTimestamp;

		private HtmlInputHidden hdNonceStr;

		private HtmlInputHidden hdSignature;

		private HtmlInputHidden hdImgUrl;

		private HtmlInputHidden hdLink;

		private Common_FightGroupRule fightGroupRule;

		private HtmlGenericControl divConsultationEmpty;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-MemberGroupDetails.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.divConsultationEmpty = (HtmlGenericControl)this.FindControl("divConsultationEmpty");
			this.rptProductConsultations = (WapTemplatedRepeater)this.FindControl("rptProductConsultations");
			this.fightGroupRule = (Common_FightGroupRule)this.FindControl("fightGroupRule");
			this.fightGroupId = this.Page.Request["fightGroupId"].ToInt(0);
			this.litDescription = (Literal)this.FindControl("litDescription");
			this.litConsultationsCount = (Literal)this.FindControl("litConsultationsCount");
			this.imgThumbnailUrl100 = (Image)this.FindControl("imgThumbnailUrl100");
			this.litSalePrice = (Literal)this.FindControl("litSalePrice");
			this.litNeedPerson = (Literal)this.FindControl("litNeedPerson");
			this.litProductReviewCount = (Literal)this.FindControl("litProductReviewCount");
			this.litProductName = (Literal)this.FindControl("litProductName");
			this.rptFightGroups = (WapTemplatedRepeater)this.FindControl("rptFightGroups");
			this.hfEndTime = (HtmlInputHidden)this.FindControl("hfEndTime");
			this.hfNowTime = (HtmlInputHidden)this.FindControl("hfNowTime");
			this.hfStartTime = (HtmlInputHidden)this.FindControl("hfStartTime");
			this.hdTitle = (HtmlInputHidden)this.FindControl("hdTitle");
			this.hdDesc = (HtmlInputHidden)this.FindControl("hdDesc");
			this.hdAppId = (HtmlInputHidden)this.FindControl("hdAppId");
			this.hdTimestamp = (HtmlInputHidden)this.FindControl("hdTimestamp");
			this.hdNonceStr = (HtmlInputHidden)this.FindControl("hdNonceStr");
			this.hdSignature = (HtmlInputHidden)this.FindControl("hdSignature");
			this.hdTitle = (HtmlInputHidden)this.FindControl("hdTitle");
			this.hdDesc = (HtmlInputHidden)this.FindControl("hdDesc");
			this.hdImgUrl = (HtmlInputHidden)this.FindControl("hdImgUrl");
			this.hdLink = (HtmlInputHidden)this.FindControl("hdLink");
			FightGroupInfo fightGroup = VShopHelper.GetFightGroup(this.fightGroupId);
			if (fightGroup != null)
			{
				if (fightGroup.EndTime <= DateTime.Now && fightGroup.Status == FightGroupStatus.FightGroupIn)
				{
					VShopHelper.DealFightGroupFail(this.fightGroupId);
					fightGroup.Status = FightGroupStatus.FightGroupFail;
				}
				this.fightGroupRule.FightGroupActivityId = fightGroup.FightGroupActivityId;
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				if (fightGroup.Status != 0)
				{
					this.Page.Response.Redirect("MemberGroupDetailsStatus.aspx?fightGroupId=" + fightGroup.FightGroupId);
				}
				HtmlInputHidden htmlInputHidden = this.hfStartTime;
				DateTime dateTime = fightGroup.StartTime;
				htmlInputHidden.Value = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
				HtmlInputHidden htmlInputHidden2 = this.hfEndTime;
				dateTime = fightGroup.EndTime;
				htmlInputHidden2.Value = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
				HtmlInputHidden htmlInputHidden3 = this.hfNowTime;
				dateTime = DateTime.Now;
				htmlInputHidden3.Value = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
				int fightGroupActiveNumber = VShopHelper.GetFightGroupActiveNumber(fightGroup.FightGroupId);
				int num = fightGroup.JoinNumber - fightGroupActiveNumber;
				num = ((num >= 0) ? num : 0);
				this.litNeedPerson.Text = num.ToString();
				this.litProductName.Text = fightGroup.ProductName;
				Literal literal = this.litProductReviewCount;
				int num2 = ProductBrowser.GetProductReviews(new ProductReviewQuery
				{
					PageIndex = 1,
					PageSize = 2147483647,
					ProductId = fightGroup.ProductId
				}).TotalRecords;
				literal.Text = num2.ToString();
				this.litSalePrice.Text = VShopHelper.GetFightGroupSkus(fightGroup.FightGroupActivityId).Min((FightGroupSkuInfo c) => c.SalePrice).F2ToString("f2");
				IList<FightGroupUserModel> fightGroupUsers = VShopHelper.GetFightGroupUsers(fightGroup.FightGroupId);
				foreach (FightGroupUserModel item in fightGroupUsers)
				{
					item.Name = DataHelper.GetHiddenUsername(item.Name);
				}
				this.rptFightGroups.DataSource = fightGroupUsers;
				this.rptFightGroups.DataBind();
				IList<int> list = null;
				Dictionary<int, IList<int>> dictionary = default(Dictionary<int, IList<int>>);
				ProductInfo productDetails = ProductHelper.GetProductDetails(fightGroup.ProductId, out dictionary, out list);
				this.SetWXShare(fightGroup, productDetails);
				this.imgThumbnailUrl100.ImageUrl = productDetails.ImageUrl1;
				Literal control = this.litConsultationsCount;
				num2 = ProductBrowser.GetProductConsultationsCount(productDetails.ProductId, true);
				control.SetWhenIsNotNull(num2.ToString());
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
				DataTable data = ProductBrowser.GetProductConsultations(new ProductConsultationAndReplyQuery
				{
					ProductId = productDetails.ProductId,
					PageIndex = 1,
					PageSize = 2147483647,
					SortOrder = SortAction.Desc,
					SortBy = "ConsultationId"
				}).Data;
				for (int i = 0; i < data.Rows.Count; i++)
				{
					data.Rows[i]["UserName"] = DataHelper.GetHiddenUsername(data.Rows[i]["UserName"].ToNullString());
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

		private void SetWXShare(FightGroupInfo fightGroup, ProductInfo product)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			string absoluteUri = this.Page.Request.Url.AbsoluteUri;
			FightGroupActivityInfo fightGroupActivitieInfo = TradeHelper.GetFightGroupActivitieInfo(fightGroup.FightGroupActivityId);
			if (fightGroupActivitieInfo != null)
			{
				string jsApiTicket = base.GetJsApiTicket(true);
				string text = WAPTemplatedWebControl.GenerateNonceStr();
				string text2 = WAPTemplatedWebControl.GenerateTimeStamp();
				this.hdAppId.Value = base.site.WeixinAppId;
				this.hdTimestamp.Value = text2;
				this.hdNonceStr.Value = text;
				this.hdSignature.Value = base.GetSignature(jsApiTicket, text, text2, absoluteUri);
				string icon = fightGroupActivitieInfo.Icon;
				this.hdImgUrl.Value = Globals.FullPath(string.IsNullOrEmpty(icon) ? masterSettings.DefaultProductThumbnail8 : icon);
				this.hdTitle.Value = fightGroupActivitieInfo.ShareTitle;
				HtmlInputHidden htmlInputHidden = this.hdDesc;
				HtmlInputHidden htmlInputHidden2 = this.hdLink;
				string text5 = htmlInputHidden.Value = (htmlInputHidden2.Value = Globals.FullPath($"/vshop/FightGroupDetails.aspx?fightGroupId={fightGroup.FightGroupId}"));
			}
		}
	}
}
