using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Members;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Shopping;
using Hidistro.SaleSystem.Store;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WapReferralAgreement : WAPTemplatedWebControl
	{
		private Literal litReferralRegisterAgreement;

		private object lockCopyRedEnvelope = new object();

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-ReferralAgreement.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			string a = this.Page.Request.QueryString["again"];
			string text = this.Page.Request.QueryString["ReferralUserId"];
			if (a == "1" || text.ToInt(0) == 0)
			{
				MemberInfo user = HiContext.Current.User;
				if (user != null && user.UserId > 0)
				{
					this.Page.Response.Redirect($"/{HiContext.Current.GetClientPath}/ReferralRegisterAgreement.aspx");
				}
				this.litReferralRegisterAgreement = (Literal)this.FindControl("litReferralRegisterAgreement");
				PageTitle.AddSiteNameTitle("我要成为分销员");
				this.litReferralRegisterAgreement.Text = HiContext.Current.SiteSettings.ReferralIntroduction;
			}
			else if (base.ClientType == ClientType.AliOH)
			{
				this.Page.Response.Redirect("/AliOH/Login.aspx?action=register&ReferralUserId=" + text);
			}
			else if (base.ClientType == ClientType.App)
			{
				this.Page.Response.Redirect("/Appshop/Login.aspx?action=register&ReferralUserId=" + text);
			}
			else if (base.ClientType == ClientType.VShop)
			{
				if (base.site.OpenVstore == 1 && !string.IsNullOrEmpty(base.site.WeixinAppId) && !string.IsNullOrEmpty(base.site.WeixinAppSecret))
				{
					OAuthUserInfo oAuthUserInfo = base.GetOAuthUserInfo(true);
					string text2 = oAuthUserInfo.OpenId.ToNullString();
					string text3 = oAuthUserInfo.NickName.ToNullString();
					string text4 = oAuthUserInfo.unionId.ToNullString();
					string text5 = oAuthUserInfo.HeadImageUrl.ToNullString();
					if (string.IsNullOrEmpty(text2) && string.IsNullOrEmpty(text4))
					{
						IDictionary<string, string> dictionary = new Dictionary<string, string>();
						dictionary.Add("openid", text2);
						dictionary.Add("weixinNickName", text3);
						dictionary.Add("unionId", text4);
						dictionary.Add("headimgurl", text5);
						Globals.AppendLog(dictionary, oAuthUserInfo.ErrMsg, "", "", "ReferralAutoRegister");
						this.Page.Response.Redirect("/wapshop/Login.aspx?action=register&ReferralUserId=" + text);
					}
					int num = this.SkipWeixinOpenId(text2, text3, text4, text5, text, oAuthUserInfo.IsAttention);
					if (num == 1)
					{
						Globals.AppendLog("status:" + num, "", "", "");
						SiteSettings masterSettings = SettingsManager.GetMasterSettings();
						if (masterSettings.IsOpenGiftCoupons)
						{
							int num2 = 0;
							string[] array = masterSettings.GiftCouponList.Split(',');
							foreach (string obj in array)
							{
								if (obj.ToInt(0) > 0 && CouponHelper.AddCouponItemInfo(HiContext.Current.User, obj.ToInt(0)) == CouponActionStatus.Success)
								{
									num2++;
								}
							}
						}
					}
					this.Page.Response.Redirect("/VShop/Default");
				}
				else
				{
					this.Page.Response.Redirect("/wapshop/Login.aspx?action=register&ReferralUserId=" + text);
				}
			}
			else if (base.ClientType == ClientType.WAP)
			{
				this.Page.Response.Redirect("/wapshop/Login.aspx?action=register&ReferralUserId=" + text);
			}
		}

		protected int SkipWeixinOpenId(string openId, string weixinNickName, string unionId, string headimgurl, string ReferralUserId, bool isSubscribe)
		{
			int num = 1;
			MemberInfo memberInfo = MemberProcessor.GetMemberByOpenId("hishop.plugins.openid.weixin", openId);
			bool flag = false;
			if (memberInfo == null)
			{
				memberInfo = MemberProcessor.GetMemberByUnionId(unionId);
				flag = true;
			}
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			ShoppingCartInfo cookieShoppingCart = ShoppingCartProcessor.GetCookieShoppingCart();
			bool flag2 = false;
			if (memberInfo != null)
			{
				num = 2;
				if (memberInfo.IsSubscribe != isSubscribe)
				{
					memberInfo.IsSubscribe = isSubscribe;
					flag2 = true;
				}
				bool flag3 = MemberProcessor.IsBindedWeixin(memberInfo.UserId, "hishop.plugins.openid.weixin");
				memberInfo.Picture = headimgurl;
				if (!string.IsNullOrEmpty(unionId) && memberInfo.UnionId != unionId && !flag)
				{
					memberInfo.UnionId = unionId;
					flag2 = true;
				}
				if (flag)
				{
					if (!flag3)
					{
						MemberOpenIdInfo memberOpenIdInfo = new MemberOpenIdInfo();
						memberOpenIdInfo.UserId = memberInfo.UserId;
						memberOpenIdInfo.OpenIdType = "hishop.plugins.openid.weixin";
						memberOpenIdInfo.OpenId = openId;
						MemberProcessor.AddMemberOpenId(memberOpenIdInfo);
						memberInfo.IsQuickLogin = true;
						flag2 = true;
					}
					else
					{
						MemberOpenIdInfo memberOpenIdInfo2 = new MemberOpenIdInfo();
						memberOpenIdInfo2.UserId = memberInfo.UserId;
						memberOpenIdInfo2.OpenIdType = "hishop.plugins.openid.weixin";
						memberOpenIdInfo2.OpenId = openId;
						MemberProcessor.UpdateMemberOpenId(memberOpenIdInfo2);
					}
				}
				if (flag2)
				{
					MemberProcessor.UpdateMember(memberInfo);
				}
				Users.SetCurrentUser(memberInfo.UserId, 30, true, false);
				HiContext.Current.User = memberInfo;
				if (cookieShoppingCart != null)
				{
					ShoppingCartProcessor.ConvertShoppingCartToDataBase(cookieShoppingCart);
					ShoppingCartProcessor.ClearCookieShoppingCart();
				}
				if (!string.IsNullOrEmpty(openId))
				{
					HttpCookie httpCookie = new HttpCookie("openId");
					httpCookie.HttpOnly = true;
					httpCookie.Value = openId;
					httpCookie.Expires = DateTime.MaxValue;
					HttpContext.Current.Response.Cookies.Add(httpCookie);
				}
				lock (this.lockCopyRedEnvelope)
				{
					this.CopyRedEnvelope(openId, memberInfo);
				}
				return num;
			}
			memberInfo = new MemberInfo();
			memberInfo.Picture = headimgurl;
			memberInfo.IsSubscribe = isSubscribe;
			int num2 = 0;
			if (ReferralUserId.ToInt(0) > 0)
			{
				memberInfo.ReferralUserId = ReferralUserId.ToInt(0);
			}
			MemberWXReferralInfo wXReferral = VShopHelper.GetWXReferral(openId.Trim());
			if (wXReferral != null)
			{
				VShopHelper.DeleteWXReferral(openId.Trim());
			}
			memberInfo.GradeId = MemberProcessor.GetDefaultMemberGrade();
			if (!string.IsNullOrEmpty(weixinNickName))
			{
				MemberInfo memberInfo2 = memberInfo;
				MemberInfo memberInfo3 = memberInfo;
				string text3 = memberInfo2.UserName = (memberInfo3.NickName = HttpUtility.UrlDecode(weixinNickName));
			}
			if (string.IsNullOrEmpty(memberInfo.UserName))
			{
				memberInfo.UserName = "weixin" + this.GenerateUsername(8);
			}
			if (MemberProcessor.FindMemberByUsername(memberInfo.UserName) != null)
			{
				memberInfo.UserName = "weixin" + this.GenerateUsername(9);
				if (MemberProcessor.FindMemberByUsername(memberInfo.UserName) != null)
				{
					memberInfo.UserName = this.GenerateUsername();
					if (MemberProcessor.FindMemberByUsername(memberInfo.UserName) != null)
					{
						num = -1;
					}
				}
			}
			if (num == 1)
			{
				string text4 = this.GeneratePassword();
				string text5 = "Open";
				string text6 = text4;
				text4 = (memberInfo.Password = Users.EncodePassword(text4, text5));
				memberInfo.PasswordSalt = text5;
				memberInfo.RegisteredSource = 3;
				memberInfo.CreateDate = DateTime.Now;
				memberInfo.IsQuickLogin = true;
				memberInfo.IsLogined = true;
				memberInfo.UnionId = unionId;
				num2 = MemberProcessor.CreateMember(memberInfo);
				if (num2 <= 0)
				{
					num = -1;
				}
			}
			if (num == 1)
			{
				memberInfo.UserId = num2;
				memberInfo.UserName = MemberHelper.GetUserName(memberInfo.UserId);
				MemberHelper.Update(memberInfo, true);
				Users.SetCurrentUser(memberInfo.UserId, 30, false, false);
				HiContext.Current.User = memberInfo;
				if (cookieShoppingCart != null)
				{
					ShoppingCartProcessor.ConvertShoppingCartToDataBase(cookieShoppingCart);
					ShoppingCartProcessor.ClearCookieShoppingCart();
				}
				if (!string.IsNullOrEmpty(openId))
				{
					MemberOpenIdInfo memberOpenIdInfo3 = new MemberOpenIdInfo();
					memberOpenIdInfo3.UserId = memberInfo.UserId;
					memberOpenIdInfo3.OpenIdType = "hishop.plugins.openid.weixin";
					memberOpenIdInfo3.OpenId = openId;
					if (MemberProcessor.GetMemberByOpenId(memberOpenIdInfo3.OpenIdType, openId) == null)
					{
						MemberProcessor.AddMemberOpenId(memberOpenIdInfo3);
					}
					if (!string.IsNullOrEmpty(openId))
					{
						HttpCookie httpCookie2 = new HttpCookie("openId");
						httpCookie2.HttpOnly = true;
						httpCookie2.Value = openId;
						httpCookie2.Expires = DateTime.MaxValue;
						HttpContext.Current.Response.Cookies.Add(httpCookie2);
					}
					lock (this.lockCopyRedEnvelope)
					{
						this.CopyRedEnvelope(openId, memberInfo);
					}
				}
			}
			return num;
		}

		public void CopyRedEnvelope(string openId, MemberInfo memberInfo)
		{
			IDictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("OpenId", openId);
			dictionary.Add("UserName", memberInfo.UserName);
			try
			{
				IList<RedEnvelopeGetRecordInfo> list = WeiXinRedEnvelopeProcessor.GettWaitToUserRedEnvelopeGetRecord(openId);
				if (list == null || list.Count == 0)
				{
					dictionary.Add("RedEnvelopeErrMsg", "红包记录为空");
				}
				int num = 1;
				foreach (RedEnvelopeGetRecordInfo item in list)
				{
					WeiXinRedEnvelopeInfo weiXinRedEnvelope = WeiXinRedEnvelopeProcessor.GetWeiXinRedEnvelope(item.RedEnvelopeId);
					if (weiXinRedEnvelope == null)
					{
						dictionary.Add("RedEnvelopeErrMsg" + num, "红包信息为空" + item.RedEnvelopeId.ToNullString());
					}
					else
					{
						CouponItemInfo couponItemInfo = new CouponItemInfo();
						couponItemInfo.UserId = memberInfo.UserId;
						couponItemInfo.UserName = memberInfo.UserName;
						couponItemInfo.CanUseProducts = "";
						couponItemInfo.ClosingTime = weiXinRedEnvelope.EffectivePeriodEndTime;
						couponItemInfo.RedEnvelopeId = weiXinRedEnvelope.Id;
						couponItemInfo.CouponName = weiXinRedEnvelope.Name;
						couponItemInfo.OrderUseLimit = weiXinRedEnvelope.EnableUseMinAmount;
						couponItemInfo.Price = item.Amount;
						couponItemInfo.StartTime = weiXinRedEnvelope.EffectivePeriodStartTime;
						couponItemInfo.UseWithGroup = false;
						couponItemInfo.UseWithPanicBuying = false;
						couponItemInfo.GetDate = DateTime.Now;
						if (WeiXinRedEnvelopeProcessor.SetRedEnvelopeGetRecordToMember(item.Id, memberInfo.UserName))
						{
							CouponActionStatus couponActionStatus = CouponHelper.AddRedEnvelopeItemInfo(couponItemInfo);
							if (couponActionStatus != 0)
							{
								dictionary.Add("SendDiffentTypeClaimCodesErrMsg" + num, "发送优惠券失败-" + couponActionStatus.ToString());
							}
							num++;
						}
						else
						{
							dictionary.Add("SetRedEnvelopeGetRecordToMemberErrMsg" + num, "设置红包记录给会员失败");
						}
					}
				}
			}
			catch (Exception ex)
			{
				Globals.WriteExceptionLog(ex, dictionary, "RedEnvelope");
			}
		}

		private string GenerateUsername(int length)
		{
			return this.GenerateRndString(length, "h");
		}

		private string GenerateUsername()
		{
			return this.GenerateRndString(10, "h");
		}

		private string GenerateUsername(int length, string prex)
		{
			return this.GenerateRndString(length, prex);
		}

		private string GeneratePassword()
		{
			return this.GenerateRndString(8, "");
		}

		private string GenerateRndString(int length, string prefix)
		{
			string text = string.Empty;
			Random random = new Random();
			while (text.Length < 10)
			{
				int num = random.Next();
				text += ((char)((num % 3 != 0) ? ((ushort)(48 + (ushort)(num % 10))) : ((ushort)(97 + (ushort)(num % 26))))).ToString();
			}
			return prefix + text;
		}
	}
}
