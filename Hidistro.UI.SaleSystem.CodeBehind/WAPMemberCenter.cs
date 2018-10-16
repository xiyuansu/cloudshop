using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WAPMemberCenter : WAPMemberTemplatedWebControl
	{
		private Literal litUserName;

		private Literal litExpenditure;

		private Literal litPoints;

		private Literal litPoints1;

		private Literal litMemberGrade;

		private Literal litWaitForRecieveCount;

		private Literal litWaitForSend;

		private Literal litWaitForPayCount;

		private Literal litFavoritesCount;

		private Literal litTakeOnStoreCount;

		private Literal litAfterSaleCount;

		private Literal litWaitReviewCount;

		private Literal ltFightGroupActiveNumber;

		private Literal litPaymentBalance;

		private HtmlAnchor aTakeOnStoreCount;

		private HtmlAnchor aWaitForSend;

		private HtmlAnchor aWaitReview;

		private HtmlAnchor referralLink;

		private HtmlGenericControl liReferralLink;

		private HtmlGenericControl liMySplittin;

		private HtmlGenericControl liBindPhoneLink;

		private HtmlGenericControl liBindEmailLink;

		private HtmlGenericControl liGroupLink;

		private Literal litUserLink;

		private Literal litCouponsCount;

		private Literal litCouponsWeiXinRedEnvelopeCount;

		private Literal litMyConsulations;

		private Literal litPrizeCount;

		private Literal litAccountTip;

		private Image userPicture;

		private HtmlGenericControl iWaitForPayCount;

		private HtmlGenericControl iWaitForSend;

		private HtmlGenericControl iWaitForRecieveCount;

		private HtmlGenericControl iTakeOnStoreCount;

		private HtmlGenericControl iAfterSaleCount;

		private HtmlGenericControl iWaitReviewCount;

		private HtmlGenericControl divReferralCenter;

		private HtmlAnchor aSignIn;

		private Literal requestBalance;

		private HtmlGenericControl divRedEnvelope;

		private SiteSettings sitesettings = SettingsManager.GetMasterSettings();

		private HtmlInputHidden hidAccountStatus;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VMemberCenter.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			PageTitle.AddSiteNameTitle("会员中心");
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			MemberInfo user = HiContext.Current.User;
			this.aTakeOnStoreCount = (HtmlAnchor)this.FindControl("aTakeOnStoreCount");
			this.aWaitForSend = (HtmlAnchor)this.FindControl("aWaitForSend");
			if (!masterSettings.OpenMultStore)
			{
				goto IL_005b;
			}
			goto IL_005b;
			IL_005b:
			this.litAccountTip = (Literal)this.FindControl("litAccountTip");
			this.ltFightGroupActiveNumber = (Literal)this.FindControl("ltFightGroupActiveNumber");
			this.litUserLink = (Literal)this.FindControl("litUserLink");
			this.litUserName = (Literal)this.FindControl("litUserName");
			this.litPaymentBalance = (Literal)this.FindControl("litPaymentBalance");
			this.litExpenditure = (Literal)this.FindControl("litExpenditure");
			this.litCouponsCount = (Literal)this.FindControl("litCouponsCount");
			this.litPrizeCount = (Literal)this.FindControl("litPrizeCount");
			this.litCouponsWeiXinRedEnvelopeCount = (Literal)this.FindControl("litCouponsWeiXinRedEnvelopeCount");
			this.litExpenditure.SetWhenIsNotNull(user.Expenditure.F2ToString("f2"));
			this.litPaymentBalance.SetWhenIsNotNull(user.Balance.F2ToString("f2"));
			this.litPoints = (Literal)this.FindControl("litPoints");
			this.litPoints1 = (Literal)this.FindControl("litPoints1");
			this.userPicture = (Image)this.FindControl("userPicture");
			Literal control = this.litPoints;
			int points = user.Points;
			control.SetWhenIsNotNull(points.ToString());
			Literal control2 = this.litPoints1;
			points = user.Points;
			control2.SetWhenIsNotNull(points.ToString());
			Control control3 = this.FindControl("liMySplittin");
			this.liReferralLink = (HtmlGenericControl)this.FindControl("liReferralLink");
			this.referralLink = (HtmlAnchor)this.FindControl("referralLink");
			this.liGroupLink = (HtmlGenericControl)this.FindControl("liGroupLink");
			Control control4 = this.FindControl("aLinkSignIn");
			this.liBindPhoneLink = (HtmlGenericControl)this.FindControl("liBindPhoneLink");
			this.liBindEmailLink = (HtmlGenericControl)this.FindControl("liBindEmailLink");
			this.litMyConsulations = (Literal)this.FindControl("litMyConsulations");
			this.requestBalance = (Literal)this.FindControl("requestBalance");
			this.divRedEnvelope = (HtmlGenericControl)this.FindControl("divRedEnvelope");
			this.hidAccountStatus = (HtmlInputHidden)this.FindControl("hidAccountStatus");
			this.divReferralCenter = (HtmlGenericControl)this.FindControl("divReferralCenter");
			if (string.IsNullOrWhiteSpace(user.TradePassword))
			{
				this.litAccountTip.SetWhenIsNotNull("<em class=\"fontstyle nopassword\">未设置交易密码</em>");
			}
			else if (HiContext.Current.SiteSettings.IsOpenRechargeGift)
			{
				this.litAccountTip.SetWhenIsNotNull("<em class=\"fontstyle\">充值享优惠</em>");
			}
			else if (!string.IsNullOrWhiteSpace(user.TradePassword))
			{
				this.litAccountTip.SetWhenIsNotNull("<em class=\"fontstyle\">" + user.Balance.F2ToString("f2") + "</em>");
			}
			if (this.liBindPhoneLink != null)
			{
				if (string.IsNullOrEmpty(user.CellPhone) && HiContext.Current.SiteSettings.SMSEnabled && !string.IsNullOrEmpty(HiContext.Current.SiteSettings.SMSSettings))
				{
					this.liBindPhoneLink.Visible = true;
				}
				else
				{
					this.liBindPhoneLink.Visible = false;
				}
			}
			if (this.liBindEmailLink != null)
			{
				if (string.IsNullOrEmpty(user.Email))
				{
					this.liBindEmailLink.Visible = true;
				}
				else
				{
					this.liBindEmailLink.Visible = false;
				}
			}
			if (control4 is HtmlAnchor)
			{
				this.aSignIn = (control4 as HtmlAnchor);
			}
			if (control3 is HtmlGenericControl)
			{
				this.liMySplittin = (control3 as HtmlGenericControl);
			}
			this.litMemberGrade = (Literal)this.FindControl("litMemberGrade");
			MemberGradeInfo memberGrade = MemberProcessor.GetMemberGrade(user.GradeId);
			if (memberGrade != null)
			{
				this.litMemberGrade.SetWhenIsNotNull(memberGrade.Name);
			}
			this.litUserName.Text = (string.IsNullOrEmpty(user.NickName) ? (string.IsNullOrEmpty(user.RealName) ? user.UserName : user.RealName) : user.NickName);
			if (!string.IsNullOrEmpty(user.Picture.ToNullString()))
			{
				this.userPicture.ImageUrl = user.Picture;
			}
			if (string.IsNullOrEmpty(user.CellPhone) && string.IsNullOrEmpty(user.Email))
			{
				this.hidAccountStatus.Value = "1";
			}
			this.litWaitForRecieveCount = (Literal)this.FindControl("litWaitForRecieveCount");
			this.litWaitForSend = (Literal)this.FindControl("litWaitForSend");
			this.litTakeOnStoreCount = (Literal)this.FindControl("litTakeOnStoreCount");
			this.litAfterSaleCount = (Literal)this.FindControl("litAfterSaleCount");
			this.iAfterSaleCount = (HtmlGenericControl)this.FindControl("iAfterSaleCount");
			this.litWaitForPayCount = (Literal)this.FindControl("litWaitForPayCount");
			int userFavoriteCount = ProductBrowser.GetUserFavoriteCount();
			this.litFavoritesCount = (Literal)this.FindControl("litFavoritesCount");
			this.litWaitReviewCount = (Literal)this.FindControl("litWaitReviewCount");
			this.litFavoritesCount.SetWhenIsNotNull(userFavoriteCount.ToString());
			this.iWaitForPayCount = (HtmlGenericControl)this.FindControl("iWaitForPayCount");
			this.iWaitForSend = (HtmlGenericControl)this.FindControl("iWaitForSend");
			this.iWaitForRecieveCount = (HtmlGenericControl)this.FindControl("iWaitForRecieveCount");
			this.iWaitReviewCount = (HtmlGenericControl)this.FindControl("iWaitReviewCount");
			this.iTakeOnStoreCount = (HtmlGenericControl)this.FindControl("iTakeOnStoreCount");
			OrderQuery orderQuery = new OrderQuery();
			orderQuery.IsAllOrder = true;
			orderQuery.ShowGiftOrder = true;
			orderQuery.Status = OrderStatus.WaitBuyerPay;
			int myFightGroupActiveNumber = VShopHelper.GetMyFightGroupActiveNumber(0);
			this.ltFightGroupActiveNumber.Text = ((myFightGroupActiveNumber == 0) ? string.Empty : (myFightGroupActiveNumber + "个团正在火拼中"));
			int num = MemberProcessor.GetUserOrderCount(HiContext.Current.UserId, orderQuery);
			if (num > 0)
			{
				this.litWaitForPayCount.SetWhenIsNotNull(num.ToString());
			}
			else if (this.iWaitForPayCount != null)
			{
				this.iWaitForPayCount.Visible = false;
			}
			else
			{
				this.litWaitForPayCount.SetWhenIsNotNull("0");
			}
			orderQuery.Status = OrderStatus.SellerAlreadySent;
			num = MemberProcessor.GetUserOrderCount(HiContext.Current.UserId, orderQuery);
			if (num > 0)
			{
				this.litWaitForRecieveCount.SetWhenIsNotNull(num.ToString());
			}
			else if (this.iWaitForRecieveCount != null)
			{
				this.iWaitForRecieveCount.Visible = false;
			}
			else
			{
				this.litWaitForRecieveCount.SetWhenIsNotNull("0");
			}
			orderQuery.Status = OrderStatus.BuyerAlreadyPaid;
			num = MemberProcessor.GetUserOrderCount(HiContext.Current.UserId, orderQuery);
			if (num > 0)
			{
				this.litWaitForSend.SetWhenIsNotNull(num.ToString());
			}
			else if (this.iWaitForSend != null)
			{
				this.iWaitForSend.Visible = false;
			}
			else
			{
				this.litWaitForSend.SetWhenIsNotNull("0");
			}
			orderQuery.Status = OrderStatus.WaitReview;
			num = MemberProcessor.GetUserOrderCount(HiContext.Current.UserId, orderQuery);
			if (num > 0)
			{
				this.litWaitReviewCount.SetWhenIsNotNull(num.ToString());
			}
			else if (this.iWaitReviewCount != null)
			{
				this.iWaitReviewCount.Visible = false;
			}
			else
			{
				this.litWaitReviewCount.SetWhenIsNotNull("0");
			}
			orderQuery.ItemStatus = 0;
			orderQuery.Status = OrderStatus.All;
			orderQuery.IsAfterSales = true;
			num = MemberProcessor.GetUserAfterSaleCount(HiContext.Current.UserId, false, null);
			if (num > 0)
			{
				this.litAfterSaleCount.SetWhenIsNotNull(num.ToString());
			}
			else if (this.iAfterSaleCount != null)
			{
				this.iAfterSaleCount.Visible = false;
			}
			else
			{
				this.litAfterSaleCount.SetWhenIsNotNull("0");
			}
			orderQuery.TakeOnStore = true;
			orderQuery.ItemStatus = 0;
			orderQuery.IsAfterSales = false;
			num = MemberProcessor.GetUserOrderCount(HiContext.Current.UserId, orderQuery);
			if (num > 0)
			{
				this.litTakeOnStoreCount.SetWhenIsNotNull(num.ToString());
			}
			else if (this.iTakeOnStoreCount != null)
			{
				this.iTakeOnStoreCount.Visible = false;
			}
			else
			{
				this.litTakeOnStoreCount.SetWhenIsNotNull("0");
			}
			int num2 = 0;
			int userObtainCouponNum = CouponHelper.GetUserObtainCouponNum(user.UserId);
			if (userObtainCouponNum > 0)
			{
				this.litCouponsCount.SetWhenIsNotNull(userObtainCouponNum.ToNullString() + "张");
			}
			int num3 = ActivityHelper.CountCurrUserNoReceiveAward(user.UserId);
			if (num3 > 0)
			{
				this.litPrizeCount.SetWhenIsNotNull(num3.ToNullString());
			}
			DataTable userWeiXinRedEnvelope = CouponHelper.GetUserWeiXinRedEnvelope(HiContext.Current.UserId, 1);
			num2 = ((userWeiXinRedEnvelope != null && userWeiXinRedEnvelope.Rows.Count > 0) ? userWeiXinRedEnvelope.Rows.Count : 0);
			if (num2 > 0)
			{
				this.litCouponsWeiXinRedEnvelopeCount.SetWhenIsNotNull(num2.ToString());
			}
			if (this.litUserLink != null)
			{
				Uri url = HttpContext.Current.Request.Url;
				string text = "";
				this.litUserLink.Text = string.Format(CultureInfo.InvariantCulture, "{0}://{1}{2}", new object[3]
				{
					Globals.GetProtocal(HttpContext.Current),
					url.Host,
					text
				}) + "/WapShop/?ReferralUserId=" + HiContext.Current.UserId;
			}
			SiteSettings masterSettings2 = SettingsManager.GetMasterSettings();
			if (masterSettings2.OpenReferral != 1)
			{
				this.liReferralLink.Visible = false;
				this.divReferralCenter.Visible = false;
			}
			else if (base.ClientType == ClientType.VShop && !masterSettings2.EnableVshopReferral)
			{
				this.liReferralLink.Visible = false;
				this.divReferralCenter.Visible = false;
			}
			else if (user.Referral == null)
			{
				this.referralLink.HRef = "SplittinRule";
			}
			else if (user.Referral.ReferralStatus == 1 || user.Referral.ReferralStatus == 3)
			{
				this.referralLink.HRef = "ReferralRegisterresults.aspx";
			}
			this.litMyConsulations.Text = ProductBrowser.GetUserProductConsultaionsCount(HiContext.Current.UserId).ToNullString();
		}
	}
}
