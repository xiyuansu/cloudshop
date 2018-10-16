using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class FightGroupSuccess : WAPTemplatedWebControl
	{
		private HtmlGenericControl litJoin;

		private HtmlGenericControl litCreate;

		private HtmlGenericControl litNeedPerson;

		private HtmlInputHidden hdTitle;

		private HtmlInputHidden hdDesc;

		private HtmlInputHidden hdAppId;

		private HtmlInputHidden hdTimestamp;

		private HtmlInputHidden hdNonceStr;

		private HtmlInputHidden hdSignature;

		private HtmlInputHidden hdImgUrl;

		private HtmlInputHidden hdLink;

		private HtmlControl divIng;

		private HtmlControl divSuccess;

		private string orderId
		{
			get;
			set;
		}

		protected override void OnInit(EventArgs e)
		{
			if (base.ClientType == ClientType.VShop)
			{
				if (this.SkinName == null)
				{
					this.SkinName = "skin-FightGroupSuccess.html";
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
			this.orderId = this.Page.Request["orderId"].ToNullString();
			this.divIng = (HtmlControl)this.FindControl("divIng");
			this.divSuccess = (HtmlControl)this.FindControl("divSuccess");
			this.litJoin = (HtmlGenericControl)this.FindControl("litJoin");
			this.litCreate = (HtmlGenericControl)this.FindControl("litCreate");
			this.litNeedPerson = (HtmlGenericControl)this.FindControl("litNeedPerson");
			this.hdDesc = (HtmlInputHidden)this.FindControl("hdDesc");
			this.hdAppId = (HtmlInputHidden)this.FindControl("hdAppId");
			this.hdTimestamp = (HtmlInputHidden)this.FindControl("hdTimestamp");
			this.hdNonceStr = (HtmlInputHidden)this.FindControl("hdNonceStr");
			this.hdSignature = (HtmlInputHidden)this.FindControl("hdSignature");
			this.hdTitle = (HtmlInputHidden)this.FindControl("hdTitle");
			this.hdDesc = (HtmlInputHidden)this.FindControl("hdDesc");
			this.hdImgUrl = (HtmlInputHidden)this.FindControl("hdImgUrl");
			this.hdLink = (HtmlInputHidden)this.FindControl("hdLink");
			OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.orderId);
			if (orderInfo == null)
			{
				base.GotoResourceNotFound("错误的订单信息");
			}
			else
			{
				FightGroupInfo fightGroup = VShopHelper.GetFightGroup(orderInfo.FightGroupId);
				if (fightGroup == null)
				{
					base.GotoResourceNotFound("错误的拼团信息");
				}
				else
				{
					int fightGroupActiveNumber = VShopHelper.GetFightGroupActiveNumber(orderInfo.FightGroupId);
					int num = fightGroup.JoinNumber - fightGroupActiveNumber;
					num = ((num > 0) ? num : 0);
					if (orderInfo.IsFightGroupHead)
					{
						this.litJoin.Visible = false;
						this.litNeedPerson.InnerText = $"{num}";
					}
					else
					{
						this.litJoin.InnerText = $"您已参团成功，还差{num}人就可以拼团成功了!";
						this.litCreate.Visible = false;
					}
					if (num == 0)
					{
						this.divIng.Visible = false;
					}
					else
					{
						this.divSuccess.Visible = false;
					}
					IList<int> list = null;
					Dictionary<int, IList<int>> dictionary = default(Dictionary<int, IList<int>>);
					ProductInfo productDetails = ProductHelper.GetProductDetails(fightGroup.ProductId, out dictionary, out list);
					this.SetWXShare(fightGroup, productDetails);
				}
			}
		}

		private void SetWXShare(FightGroupInfo fightGroup, ProductInfo product)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			string jsApiTicket = base.GetJsApiTicket(true);
			string text = WAPTemplatedWebControl.GenerateNonceStr();
			string text2 = WAPTemplatedWebControl.GenerateTimeStamp();
			string absoluteUri = this.Page.Request.Url.AbsoluteUri;
			this.hdAppId.Value = base.site.WeixinAppId;
			this.hdTimestamp.Value = text2;
			this.hdNonceStr.Value = text;
			this.hdSignature.Value = base.GetSignature(jsApiTicket, text, text2, absoluteUri);
			FightGroupActivityInfo fightGroupActivitieInfo = TradeHelper.GetFightGroupActivitieInfo(fightGroup.FightGroupActivityId);
			if (fightGroupActivitieInfo == null)
			{
				base.GotoResourceNotFound("活动已结束或者已删除");
			}
			else
			{
				string icon = fightGroupActivitieInfo.Icon;
				this.hdImgUrl.Value = Globals.FullPath(string.IsNullOrEmpty(icon) ? masterSettings.DefaultProductThumbnail8 : icon);
				if (string.IsNullOrEmpty(fightGroupActivitieInfo.ShareTitle))
				{
					this.hdTitle.Value = (string.IsNullOrEmpty(product.Title) ? product.ProductName : product.Title);
				}
				else
				{
					this.hdTitle.Value = fightGroupActivitieInfo.ShareTitle.Trim();
				}
				if (string.IsNullOrEmpty(fightGroupActivitieInfo.ShareContent.Trim()))
				{
					this.hdDesc.Value = product.Meta_Description;
				}
				else
				{
					this.hdDesc.Value = fightGroupActivitieInfo.ShareContent.Trim();
				}
				if (HiContext.Current.User.IsReferral())
				{
					this.hdLink.Value = Globals.FullPath(string.Format("/vshop/FightGroupDetails.aspx?fightGroupId={0}&ReferralUserId=" + HiContext.Current.User.UserId, fightGroup.FightGroupId));
				}
				else
				{
					this.hdLink.Value = Globals.FullPath($"/vshop/FightGroupDetails.aspx?fightGroupId={fightGroup.FightGroupId}");
				}
			}
		}
	}
}
