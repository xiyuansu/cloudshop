using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Configuration;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.APP;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.Entities.Supplier;
using Hidistro.Entities.VShop;
using Hidistro.Entities.WeChatApplet;
using Hidistro.Messages;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Members;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Shopping;
using Hidistro.SaleSystem.Statistics;
using Hidistro.SaleSystem.Store;
using Hidistro.SaleSystem.Supplier;
using Hidistro.SaleSystem.WeChartApplet;
using Hidistro.SqlDal.Members;
using Hidistro.UI.Common.Controls;
using Hishop.Components.Validation;
using Hishop.Plugins;
using Hishop.Weixin.Pay;
using Hishop.Weixin.Pay.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Hidistro.UI.Web.API
{
	public class WeChatApplet : IHttpHandler
	{
		private HttpContext context;

		private const string openIdType = "hishop.plugins.openid.wxapplet";

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			try
			{
				string text = context.Request["action"];
				if (string.IsNullOrEmpty(text))
				{
					context.Response.Write(this.GetErrorJson("参数错误"));
				}
				else
				{
					this.context = context;
					switch (text)
					{
					case "LoginByOpenId":
						this.LoginByOpenId();
						break;
					case "LoginByUserName":
						this.LoginByUserName();
						break;
					case "QuickLogin":
						this.QuickLogin();
						break;
					case "logout":
						this.ProcessLogout(context);
						break;
					case "GetProductDetail":
						this.GetProductDetail();
						break;
					case "GetCountDownProductDetail":
						this.GetCountDownProductDetail();
						break;
					case "UserGetCoupon":
						this.UserGetCoupon();
						break;
					case "LoadCoupon":
						this.LoadCoupon();
						break;
					case "LoadSiteCoupon":
						this.LoadSiteCoupon();
						break;
					case "GetCouponDetail":
						this.GetCouponDetail();
						break;
					case "GetUserShippingAddress":
						this.GetUserShippingAddress();
						break;
					case "AddShippingAddress":
						this.AddShippingAddress();
						break;
					case "UpdateShippingAddress":
						this.UpdateShippingAddress();
						break;
					case "SetDefaultShippingAddress":
						this.SetDefaultShippingAddress();
						break;
					case "DelShippingAddress":
						this.DelShippingAddress();
						break;
					case "GetShippingAddressById":
						this.GetShippingAddressById();
						break;
					case "AddWXChooseAddress":
						this.AddWXChooseAddress();
						break;
					case "OrderList":
						this.OrderList();
						break;
					case "CloseOrder":
						this.CloseOrder();
						break;
					case "FinishOrder":
						this.FinishOrder();
						break;
					case "GetLogistic":
						this.GetLogistic();
						break;
					case "GetPayParam":
						this.GetPayParam();
						break;
					case "GetShoppingCart":
						this.GetShoppingCart();
						break;
					case "SubmitOrder":
						this.SubmitOrder();
						break;
					case "GetProducts":
						this.GetProducts();
						break;
					case "InitVeCode":
						this.InitVeCode(context);
						break;
					case "GetIndexData":
						this.GetIndexData();
						break;
					case "GetIndexProductData":
						this.GetIndexProductData();
						break;
					case "GetOrderDetail":
						this.GetOrderDetail();
						break;
					case "GetRegionsOfProvinceCity":
						this.GetRegionsOfProvinceCity(context);
						break;
					case "GetRegions":
						this.GetRegions(context);
						break;
					case "getShoppingCartList":
						this.GetShoppingCartList(context);
						break;
					case "addToCart":
						this.AddToCart(context);
						break;
					case "delCartItem":
						this.DelCartItem(context);
						break;
					case "CanSubmitOrder":
						this.CanSubmitOrder(context);
						break;
					case "GetAllCategories":
						this.GetAllCategories(context);
						break;
					case "GetProductSkus":
						this.GetProductSkus(context);
						break;
					case "GetCountDownList":
						this.GetCountDownList(context);
						break;
					case "GetAllAfterSaleList":
						this.GetAllAfterSaleList(context);
						break;
					case "ApplyRefund":
						this.ApplyRefund(context);
						break;
					case "UploadAppletImage":
						this.UploadAppletImage(context);
						break;
					case "ApplyReturn":
						this.ApplyReturn(context);
						break;
					case "GetExpressList":
						this.GetExpressList(context);
						break;
					case "ReturnSendGoods":
						this.ReturnSendGoods(context);
						break;
					case "GetRefundDetail":
						this.GetRefundDetail(context);
						break;
					case "GetReturnDetail":
						this.GetReturnDetail(context);
						break;
					case "GetOrderProduct":
						this.GetOrderProduct(context);
						break;
					case "AddProductReview":
						this.AddProductReview(context);
						break;
					case "StatisticsReview":
						this.StatisticsReview(context);
						break;
					case "LoadReview":
						this.LoadReview(context);
						break;
					case "GetOpenId":
						this.GetOpenId(context);
						break;
					case "AfterSalePreCheck":
						this.AfterSalePreCheck(context);
						break;
					case "GetUserPoints":
						this.GetUserPoints();
						break;
					case "DelUserInvoice":
						this.DelUserInvoice(context);
						break;
					case "UpdateUserInvoice":
						this.UpdateUserInvoice(context);
						break;
					case "GetRegionByLatLng":
						this.GetRegionByLatLng(context);
						break;
					case "SendVerifyCode":
						this.SendVerifyCode(context);
						break;
					case "CellPhoneRegister":
						this.CellPhoneRegister(context);
						break;
					case "BindUser":
						this.BindUser(context);
						break;
					case "BindPhone":
						this.BindPhone(context);
						break;
					case "ReferralRegister":
						this.ReferralRegister(context);
						break;
					case "SplittinList":
						this.SplittinList(context);
						break;
					case "SubMembers":
						this.SubMembers(context);
						break;
					case "SplittinDraw":
						this.SplittinDraw(context);
						break;
					case "SplittinDrawList":
						this.SplittinDrawList(context);
						break;
					case "SplittinDrawDetail":
						this.SplittinDrawDetail(context);
						break;
					case "SetReferralShopInfo":
						this.SetReferralShopInfo(context);
						break;
					case "GetReferralInfo":
						this.GetReferralInfo(context);
						break;
					case "InitTradePassword":
						this.InitTradePassword(context);
						break;
					case "ChangeTradePassword":
						this.ChangeTradePassword(context);
						break;
					case "SendEmailVerifyCode":
						this.SendEmailVerifyCode(context);
						break;
					case "SendFindPasswordCode":
						this.SendFindPasswordCode(context);
						break;
					case "ResetTradePassword":
						this.ResetTradePassword(context);
						break;
					}
				}
			}
			catch (Exception ex)
			{
				Globals.WriteExceptionLog(ex, null, "WeChatApplet");
			}
		}

		private void SendFindPasswordCode(HttpContext context)
		{
			this.CheckOpenId();
			string text = context.Request["OpenId"].ToNullString();
			SiteSettings siteSettings = HiContext.Current.SiteSettings;
			MemberInfo user = HiContext.Current.User;
			int num = context.Request["CodeType"].ToInt(0);
			ApiErrorCode apiErrorCode;
			if (num != 1 && num != 2)
			{
				HttpResponse response = context.Response;
				apiErrorCode = ApiErrorCode.Paramter_Error;
				response.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "错误的找回方式"));
			}
			else
			{
				if (num == 1)
				{
					if (!DataHelper.IsMobile(user.UserName) && !user.CellPhoneVerification)
					{
						HttpResponse response2 = context.Response;
						apiErrorCode = ApiErrorCode.Paramter_Error;
						response2.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "手机号码未绑定"));
						return;
					}
					if (!siteSettings.SMSEnabled || string.IsNullOrEmpty(siteSettings.SMSSettings))
					{
						HttpResponse response3 = context.Response;
						apiErrorCode = ApiErrorCode.Paramter_Diffrent;
						response3.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "短信发送未开启"));
						return;
					}
				}
				if (num == 2)
				{
					if (!DataHelper.IsEmail(user.UserName) && !user.EmailVerification)
					{
						HttpResponse response4 = context.Response;
						apiErrorCode = ApiErrorCode.Paramter_Error;
						response4.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "邮箱未绑定"));
						return;
					}
					if (!siteSettings.EmailEnabled || string.IsNullOrEmpty(siteSettings.EmailSettings))
					{
						HttpResponse response5 = context.Response;
						apiErrorCode = ApiErrorCode.Paramter_Diffrent;
						response5.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "邮件发送未开启"));
						return;
					}
				}
				string verifyCode = context.Request["ImgCode"].ToNullString();
				if (!HiContext.Current.CheckVerifyCode(verifyCode, text))
				{
					HttpResponse response6 = context.Response;
					apiErrorCode = ApiErrorCode.Paramter_Error;
					response6.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "图形验证码错误"));
				}
				else if (num == 1)
				{
					string code = HiContext.Current.CreatePhoneCode(4, user.CellPhone, VerifyCodeType.Digital);
					this.SendVerifyCode(siteSettings, user.CellPhone, code, text);
				}
				else
				{
					string code2 = HiContext.Current.CreateVerifyCode(4, VerifyCodeType.Digital, text);
					this.SendEmailVerifyCode(siteSettings, user.Email, code2, text);
				}
			}
		}

		private void SendEmailVerifyCode(HttpContext context)
		{
			this.CheckOpenId();
			string openId = context.Request["openId"].ToNullString();
			string code = HiContext.Current.CreateVerifyCode(4, VerifyCodeType.Digital, openId);
			string text = context.Request["Email"].ToNullString();
			MemberInfo user = HiContext.Current.User;
			string text2 = Globals.StripAllTags(context.Request["username"].ToNullString());
			int num;
			if ((string.IsNullOrEmpty(text) || !DataHelper.IsEmail(text)) && user != null)
			{
				num = ((user.UserId > 0) ? 1 : 0);
				goto IL_0088;
			}
			num = 0;
			goto IL_0088;
			IL_0088:
			if (num != 0)
			{
				text = HiContext.Current.User.Email.ToNullString();
			}
			if (!DataHelper.IsEmail(text) && !string.IsNullOrEmpty(text2))
			{
				user = MemberProcessor.FindMemberByUsername(text2);
				if (user != null)
				{
					text = user.Email;
				}
			}
			int num2 = context.Request["needValidate"].ToInt(0);
			ApiErrorCode apiErrorCode;
			if (!DataHelper.IsEmail(text))
			{
				HttpResponse response = context.Response;
				apiErrorCode = ApiErrorCode.Paramter_Error;
				response.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "错误的邮箱号码"));
			}
			else if (num2 == 1 && !MemberProcessor.IsUseEmail(text))
			{
				HttpResponse response2 = context.Response;
				apiErrorCode = ApiErrorCode.Paramter_Error;
				response2.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "邮箱未绑定会员"));
			}
			else if (num2 == 2 && MemberProcessor.IsUseEmail(text))
			{
				HttpResponse response3 = context.Response;
				apiErrorCode = ApiErrorCode.Paramter_Error;
				response3.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "邮箱已被占用"));
			}
			else
			{
				SiteSettings siteSettings = HiContext.Current.SiteSettings;
				if (!siteSettings.EmailEnabled || string.IsNullOrEmpty(siteSettings.EmailSettings))
				{
					HttpResponse response4 = context.Response;
					apiErrorCode = ApiErrorCode.Paramter_Diffrent;
					response4.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "邮件服务未配置"));
				}
				else
				{
					this.SendEmailVerifyCode(siteSettings, text, code, context.Session.SessionID);
				}
			}
		}

		private void SendEmailVerifyCode(SiteSettings settings, string email, string code, string sessionID)
		{
			ApiErrorCode apiErrorCode;
			try
			{
				int num = 0;
				DateTime dateTime = DateTime.Now;
				DateTime value = dateTime.AddSeconds(-61.0);
				object obj = HiCache.Get($"DataCache-LastSendMailTimeCacheKey-{email}");
				if (obj != null)
				{
					DateTime.TryParse(obj.ToString(), out value);
				}
				dateTime = DateTime.Now;
				TimeSpan timeSpan = dateTime.Subtract(value);
				if (timeSpan.TotalSeconds < 60.0)
				{
					HttpResponse response = this.context.Response;
					apiErrorCode = ApiErrorCode.Paramter_Diffrent;
					response.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "验证码发送太频繁"));
				}
				else
				{
					ConfigData configData = new ConfigData(HiCryptographer.TryDecypt(settings.EmailSettings));
					string body = string.Format("您的验证码为：{1},请在3分钟内完成验证", email, code);
					MailMessage mailMessage = new MailMessage
					{
						IsBodyHtml = true,
						Priority = MailPriority.High,
						SubjectEncoding = Encoding.UTF8,
						BodyEncoding = Encoding.UTF8,
						Body = body,
						Subject = "来自" + settings.SiteName
					};
					mailMessage.To.Add(email);
					EmailSender emailSender = EmailSender.CreateInstance(settings.EmailSender, configData.SettingsXml);
					if (emailSender.Send(mailMessage, Encoding.GetEncoding(HiConfiguration.GetConfig().EmailEncoding)))
					{
						this.context.Response.Write(this.GetOKJson("发送邮件成功"));
						string key = $"DataCache-LastSendMailTimeCacheKey-{email}";
						object obj2 = DateTime.Now;
						dateTime = DateTime.Now;
						dateTime = dateTime.Date;
						dateTime = dateTime.AddDays(1.0);
						timeSpan = dateTime.Subtract(DateTime.Now);
						HiCache.Insert(key, obj2, (int)timeSpan.TotalSeconds);
						HiCache.Insert($"DataCache-EmailCode-{email}", code, 10800);
					}
					else
					{
						HttpResponse response2 = this.context.Response;
						apiErrorCode = ApiErrorCode.Paramter_Diffrent;
						response2.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "发送邮件失败"));
					}
				}
			}
			catch (Exception ex)
			{
				NameValueCollection param = new NameValueCollection
				{
					this.context.Request.QueryString,
					this.context.Request.Form
				};
				Globals.WriteExceptionLog_Page(ex, param, "SendEmailVerifyCode");
				HttpResponse response3 = this.context.Response;
				apiErrorCode = ApiErrorCode.Unknown_Error;
				response3.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "邮件配置错误"));
			}
		}

		private void ResetTradePassword(HttpContext context)
		{
			this.CheckOpenId();
			string openId = context.Request["OpenId"].ToNullString();
			string text = context.Request["password"].ToNullString();
			string text2 = context.Request["repassword"].ToNullString();
			string verifyCode = context.Request["verifycode"].ToNullString();
			int num = context.Request["CodeType"].ToInt(0);
			MemberInfo user = HiContext.Current.User;
			ApiErrorCode apiErrorCode;
			if (string.IsNullOrEmpty(text))
			{
				HttpResponse response = context.Response;
				apiErrorCode = ApiErrorCode.Paramter_Error;
				response.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "新密码不能为空"));
			}
			else if (text != text2)
			{
				HttpResponse response2 = context.Response;
				apiErrorCode = ApiErrorCode.Paramter_Error;
				response2.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "两次密码不一致"));
			}
			else
			{
				bool flag = true;
				flag = (num == 1 && true);
				string text3 = "验证码错误";
				if ((flag && HiContext.Current.CheckPhoneVerifyCode(verifyCode, user.CellPhone, out text3)) || (!flag && HiContext.Current.CheckVerifyCode(verifyCode, openId)))
				{
					if (MemberProcessor.ChangeTradePassword(user, text2))
					{
						Messenger.UserPasswordChanged(user, text2);
						Users.SetCurrentUser(user.UserId, 1, true, false);
					}
					context.Response.Write(this.GetOKJson("重置密码成功"));
				}
				else
				{
					HttpResponse response3 = context.Response;
					apiErrorCode = ApiErrorCode.Unknown_Error;
					response3.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "重置密码失败"));
				}
			}
		}

		private void ChangeTradePassword(HttpContext context)
		{
			this.CheckOpenId();
			string text = context.Request["password"].ToNullString();
			string pass = context.Request["oldPassword"].ToNullString();
			MemberInfo user = HiContext.Current.User;
			ApiErrorCode apiErrorCode;
			if (!string.IsNullOrEmpty(user.TradePassword) && user.TradePassword != Users.EncodePassword(pass, user.TradePasswordSalt))
			{
				HttpResponse response = context.Response;
				apiErrorCode = ApiErrorCode.TradePassword_Error;
				response.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "原交易密码错误"));
			}
			else if (text.Length < 6 || text.Length > 20)
			{
				HttpResponse response2 = context.Response;
				apiErrorCode = ApiErrorCode.Paramter_Error;
				response2.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "密码长度错误"));
			}
			else if (MemberProcessor.ChangeTradePassword(user, text))
			{
				Messenger.UserDealPasswordChanged(user, text);
				context.Response.Write(this.GetOKJson("设置密码成功"));
			}
			else
			{
				HttpResponse response3 = context.Response;
				apiErrorCode = ApiErrorCode.Unknown_Error;
				response3.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "设置密码失败"));
			}
		}

		public void InitTradePassword(HttpContext context)
		{
			this.CheckOpenId();
			MemberInfo user = HiContext.Current.User;
			ApiErrorCode apiErrorCode;
			if (!string.IsNullOrEmpty(user.TradePassword))
			{
				HttpResponse response = context.Response;
				apiErrorCode = ApiErrorCode.TradePasswordAlreadySet;
				response.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "已设置交易密码"));
			}
			else
			{
				string text = context.Request["password"].ToNullString();
				string b = context.Request["repassword"].ToNullString();
				if (string.IsNullOrEmpty(text))
				{
					HttpResponse response2 = context.Response;
					apiErrorCode = ApiErrorCode.Paramter_Error;
					response2.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "请输入交易密码"));
				}
				else if (text.Length < 6 || text.Length > 20)
				{
					HttpResponse response3 = context.Response;
					apiErrorCode = ApiErrorCode.Paramter_Error;
					response3.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "密码长度错误"));
				}
				else if (text != b)
				{
					HttpResponse response4 = context.Response;
					apiErrorCode = ApiErrorCode.Paramter_Error;
					response4.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "两次密码不一致"));
				}
				else
				{
					user.TradePasswordSalt = Globals.RndStr(128, true);
					user.TradePassword = Users.EncodePassword(text, user.TradePasswordSalt);
					MemberProcessor.UpdateMember(user);
					Users.ClearUserCache(user.UserId, user.SessionId);
					context.Response.Write(this.GetOKJson("设置密码成功"));
				}
			}
		}

		public void GetReferralInfo(HttpContext context)
		{
			this.BindUserByOpenId();
			decimal qrCodePositionX = default(decimal);
			decimal qrCodePositionY = default(decimal);
			decimal qrCodeWidth = default(decimal);
			string referralPosterUrl = Globals.HttpsFullPath(MemberProcessor.GetAppletReferralPostUrl(out qrCodePositionX, out qrCodePositionY, out qrCodeWidth));
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			MemberInfo memberInfo = null;
			int num = context.Request["ReferralUserId"].ToInt(0);
			if (num > 0)
			{
				memberInfo = Users.GetUser(num);
			}
			if (memberInfo == null)
			{
				memberInfo = Users.GetUser(HiContext.Current.UserId);
			}
			if (memberInfo == null)
			{
				context.Response.Write(this.GetErrorJson(208.GetHashCode(), "分销员帐号不存在"));
			}
			else
			{
				ReferralGradeInfo referralGradeInfo = (memberInfo.Referral == null) ? null : MemberProcessor.GetNextReferralGradeInfo(memberInfo.Referral.GradeId);
				decimal num2 = default(decimal);
				decimal num3 = default(decimal);
				num3 = ((memberInfo.Referral == null) ? decimal.Zero : MemberProcessor.GetSplittinTotal(memberInfo.UserId).F2ToString("f2").ToDecimal(0));
				if (referralGradeInfo != null)
				{
					num2 = referralGradeInfo.CommissionThreshold - num3;
					if (num2 < decimal.Zero)
					{
						num2 = default(decimal);
					}
				}
				SplittinDrawInfo myRecentlySplittinDraws = MemberProcessor.GetMyRecentlySplittinDraws();
				ReferralExtInfo referralExtInfo = new ReferralExtInfo();
				if (memberInfo.Referral != null)
				{
					referralExtInfo = MemberProcessor.GetReferralExtInfo(memberInfo.Referral.RefusalReason);
				}
				if (string.IsNullOrEmpty(referralExtInfo.CellPhone))
				{
					referralExtInfo.CellPhone = memberInfo.CellPhone;
				}
				if (string.IsNullOrEmpty(referralExtInfo.Email))
				{
					referralExtInfo.Email = memberInfo.Email;
				}
				if (string.IsNullOrEmpty(referralExtInfo.RealName))
				{
					referralExtInfo.RealName = memberInfo.RealName;
				}
				if (string.IsNullOrEmpty(referralExtInfo.Address))
				{
					referralExtInfo.Address = memberInfo.Address;
					referralExtInfo.RegionId = memberInfo.RegionId;
				}
				string s = JsonConvert.SerializeObject(new
				{
					referral_get_response = new
					{
						IsReferral = memberInfo.IsReferral(),
						ReferralStatus = ((memberInfo.Referral != null) ? memberInfo.Referral.ReferralStatus : 0),
						ReferralStatusText = ((memberInfo.Referral == null) ? "" : EnumDescription.GetEnumDescription((Enum)(object)(ReferralApplyStatus)memberInfo.Referral.ReferralStatus, 0)),
						ShopName = ((memberInfo.Referral == null) ? "" : memberInfo.Referral.ShopName),
						NickName = memberInfo.NickName,
						RealName = memberInfo.RealName,
						BannerUrl = ((memberInfo.Referral == null) ? "" : Globals.HttpsFullPath(memberInfo.Referral.BannerUrl)),
						IsRepeled = (memberInfo.Referral != null && memberInfo.Referral.IsRepeled),
						RefusalReason = ((memberInfo.Referral == null) ? "" : memberInfo.Referral.RefusalReason),
						ReferralExtInfo = referralExtInfo,
						RepelTime = ((memberInfo.Referral == null) ? new DateTime?(DateTime.MinValue) : memberInfo.Referral.RepelTime),
						ReferralGradeId = ((memberInfo.Referral != null) ? memberInfo.Referral.GradeId : 0),
						ReferralGradeName = ((memberInfo.Referral == null) ? "" : memberInfo.Referral.GradeName),
						RepelReason = ((memberInfo.Referral == null) ? "" : memberInfo.Referral.RepelReason),
						SplittinTotal = ((memberInfo.Referral == null) ? decimal.Zero : MemberProcessor.GetSplittinTotal(memberInfo.UserId).F2ToString("f2").ToDecimal(0)),
						NextReferralGradeName = ((referralGradeInfo == null) ? "" : referralGradeInfo.Name),
						UpgradeNeedSplittin = num2,
						DrawSplittinTotal = MemberProcessor.GetUserDrawSplittin().F2ToString("f2").ToDecimal(0),
						HistorySplittin = ((memberInfo.Referral == null) ? decimal.Zero : MemberProcessor.GetUserAllSplittin(memberInfo.UserId).F2ToString("f2").ToDecimal(0)),
						CanDrawSplittin = ((memberInfo.Referral == null) ? decimal.Zero : MemberProcessor.GetUserUseSplittin(memberInfo.UserId).F2ToString("f2").ToDecimal(0)),
						NoSettlementSplttin = ((memberInfo.Referral == null) ? decimal.Zero : MemberProcessor.GetUserNoUseSplittin(memberInfo.UserId).F2ToString("f2").ToDecimal(0)),
						ReferralEmail = ((memberInfo.Referral == null) ? "" : memberInfo.Referral.Email),
						ReferralCellPhone = ((memberInfo.Referral == null) ? "" : memberInfo.Referral.CellPhone),
						SplittinDraws_CashToALiPay = masterSettings.SplittinDraws_CashToALiPay,
						SplittinDraws_CashToBankCard = masterSettings.SplittinDraws_CashToBankCard,
						SplittinDraws_CashToDeposit = masterSettings.SplittinDraws_CashToDeposit,
						SplittinDraws_CashToWeiXin = masterSettings.SplittinDraws_CashToWeiXin,
						DeductMinDraw = masterSettings.MinimumSingleShot,
						LastDrawTime = ((myRecentlySplittinDraws == null) ? "" : myRecentlySplittinDraws.RequestDate.ToString("yyyy-MM-dd HH:mm:ss")),
						ReferralPosterUrl = referralPosterUrl,
						QrCodePositionX = qrCodePositionX,
						QrCodePositionY = qrCodePositionY,
						QrCodeWidth = qrCodeWidth
					}
				});
				context.Response.Write(s);
				context.Response.End();
			}
		}

		public void SetReferralShopInfo(HttpContext context)
		{
			this.CheckOpenId();
			string text = Globals.StripAllTags(context.Request["Email"].ToNullString());
			string text2 = Globals.StripAllTags(context.Request["Phone"].ToNullString());
			string text3 = Globals.StripAllTags(context.Request["shopName"].ToNullString());
			string text4 = Globals.StripAllTags(context.Request["bannerUrl"].ToNullString());
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			string imageServerUrl = Globals.GetImageServerUrl();
			if (text4.Length > 0)
			{
				string[] array = text4.Split(',');
				text4 = "";
				for (int i = 0; i < array.Length; i++)
				{
					text4 += (string.IsNullOrEmpty(imageServerUrl) ? (Globals.SaveFile("referral\\banner", array[i], "/Storage/master/", true, false, "") + "|") : (array[i] + "|"));
				}
				text4 = text4.TrimEnd('|');
			}
			context.Response.ContentType = "application/json";
			ApiErrorCode apiErrorCode;
			if (HiContext.Current.UserId == 0)
			{
				HttpResponse response = context.Response;
				apiErrorCode = ApiErrorCode.UserNoLogin;
				response.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "请您先登录"));
			}
			else
			{
				MemberInfo user = Users.GetUser(HiContext.Current.UserId);
				if (!user.IsReferral())
				{
					HttpResponse response2 = context.Response;
					apiErrorCode = ApiErrorCode.UserIsNotReferral;
					response2.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "您现在还不是分销员,请先申请成为分销员"));
				}
				else if (string.IsNullOrEmpty(text3) || text3.Length > 100)
				{
					HttpResponse response3 = context.Response;
					apiErrorCode = ApiErrorCode.Paramter_Error;
					response3.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "请输入店铺名称"));
				}
				else
				{
					if (text.Length > 0 && !DataHelper.IsEmail(text))
					{
						HttpResponse response4 = context.Response;
						apiErrorCode = ApiErrorCode.Paramter_Error;
						response4.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "错误的邮箱地址"));
					}
					if (text2.Length > 0 && !DataHelper.IsMobile(text2))
					{
						HttpResponse response5 = context.Response;
						apiErrorCode = ApiErrorCode.Paramter_Error;
						response5.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "错误的手机号码"));
					}
					else
					{
						ReferralInfo referral = user.Referral;
						referral.BannerUrl = text4;
						referral.CellPhone = text2;
						referral.Email = text;
						referral.ShopName = text3;
						if (MemberProcessor.ReferralInfoSet(referral))
						{
							Users.ClearUserCache(HiContext.Current.UserId, HiContext.Current.User.SessionId);
							context.Response.Write(this.GetOKJson("保存成功"));
						}
						else
						{
							HttpResponse response6 = context.Response;
							apiErrorCode = ApiErrorCode.Unknown_Error;
							response6.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "保存失败!"));
						}
					}
				}
			}
		}

		public void SplittinDrawDetail(HttpContext context)
		{
			this.CheckOpenId();
			int num = context.Request["RequestId"].ToInt(0);
			SplittinDrawInfo splittinDraw = MemberProcessor.GetSplittinDraw(num);
			if (splittinDraw == null || splittinDraw.UserId != HiContext.Current.UserId)
			{
				context.Response.Write(this.GetErrorJson(101.GetHashCode(), "错误的数据ID"));
			}
			else
			{
				object accountDate;
				DateTime dateTime;
				if (!splittinDraw.AccountDate.HasValue)
				{
					accountDate = "";
				}
				else
				{
					dateTime = splittinDraw.AccountDate.Value;
					accountDate = dateTime.ToString("yyyy-MM-dd");
				}
				string accountName = splittinDraw.AccountName;
				string alipayCode = splittinDraw.AlipayCode;
				string alipayRealName = splittinDraw.AlipayRealName;
				decimal amount = splittinDraw.Amount.F2ToString("f2").ToDecimal(0);
				int auditStatus = splittinDraw.AuditStatus;
				string auditStatusText = (splittinDraw.AuditStatus == 0) ? "待审核" : "已审核";
				decimal balance = splittinDraw.Balance.F2ToString("f2").ToDecimal(0);
				string bankName = splittinDraw.BankName;
				bool isAlipay = splittinDraw.IsAlipay.HasValue && splittinDraw.IsAlipay.Value;
				bool isWeixin = splittinDraw.IsWeixin.HasValue && splittinDraw.IsWeixin.Value;
				bool isWithdrawToAccount = splittinDraw.IsWithdrawToAccount;
				int journalNumber = splittinDraw.JournalNumber;
				string managerRemark = splittinDraw.ManagerRemark;
				string managerUserName = splittinDraw.ManagerUserName;
				string merchantCode = splittinDraw.MerchantCode;
				string remark = splittinDraw.Remark;
				dateTime = splittinDraw.RequestDate;
				string s = JsonConvert.SerializeObject(new
				{
					SplittinDraw_get_response = new
					{
						AccountDate = (string)accountDate,
						AccountName = accountName,
						AlipayCode = alipayCode,
						AlipayRealName = alipayRealName,
						Amount = amount,
						AuditStatus = auditStatus,
						AuditStatusText = auditStatusText,
						Balance = balance,
						BankName = bankName,
						IsAlipay = isAlipay,
						IsWeixin = isWeixin,
						IsWithdrawToAccount = isWithdrawToAccount,
						JournalNumber = journalNumber,
						ManagerRemark = managerRemark,
						ManagerUserName = managerUserName,
						MerchantCode = merchantCode,
						Remark = remark,
						RequestDate = dateTime.ToString("yyyy-MM-dd"),
						RequestError = splittinDraw.RequestError,
						RequestState = MemberProcessor.GetRequestState(splittinDraw),
						RequestStateText = EnumDescription.GetEnumDescription((Enum)(object)(OnLinePayment)MemberProcessor.GetRequestState(splittinDraw), 1),
						UserId = splittinDraw.UserId,
						UserName = splittinDraw.UserName
					}
				});
				context.Response.Write(s);
				context.Response.End();
			}
		}

		public void SplittinDrawList(HttpContext context)
		{
			this.CheckOpenId();
			int num = context.Request["pageIndex"].ToInt(0);
			if (num <= 0)
			{
				num = 1;
			}
			int num2 = context.Request["pageSize"].ToInt(0);
			if (num2 < 1)
			{
				num2 = 10;
			}
			MemberInfo user = HiContext.Current.User;
			BalanceDrawRequestQuery balanceDrawRequestQuery = new BalanceDrawRequestQuery();
			balanceDrawRequestQuery.PageIndex = num;
			balanceDrawRequestQuery.PageSize = num2;
			balanceDrawRequestQuery.UserId = HiContext.Current.UserId;
			PageModel<SplittinDrawInfo> mySplittinDrawList = MemberProcessor.GetMySplittinDrawList(balanceDrawRequestQuery, null);
			string s2 = JsonConvert.SerializeObject(new
			{
				SplittinDraw_get_response = new
				{
					RecordCount = mySplittinDrawList.Total,
					SplittinDraws = mySplittinDrawList.Models.Select(delegate(SplittinDrawInfo s)
					{
						object accountDate;
						DateTime dateTime;
						if (!s.AccountDate.HasValue)
						{
							accountDate = "";
						}
						else
						{
							dateTime = s.AccountDate.Value;
							accountDate = dateTime.ToString("yyyy-MM-dd");
						}
						string accountName = s.AccountName;
						string alipayCode = s.AlipayCode;
						string alipayRealName = s.AlipayRealName;
						decimal amount = s.Amount.F2ToString("f2").ToDecimal(0);
						int auditStatus = s.AuditStatus;
						string auditStatusText = (s.AuditStatus == 0) ? "待审核" : "已审核";
						decimal balance = s.Balance.F2ToString("f2").ToDecimal(0);
						string bankName = s.BankName;
						bool isAlipay = s.IsAlipay.HasValue && s.IsAlipay.Value;
						bool isWeixin = s.IsWeixin.HasValue && s.IsWeixin.Value;
						bool isWithdrawToAccount = s.IsWithdrawToAccount;
						int journalNumber = s.JournalNumber;
						string managerRemark = s.ManagerRemark;
						string managerUserName = s.ManagerUserName;
						string merchantCode = s.MerchantCode;
						string remark = s.Remark;
						dateTime = s.RequestDate;
						return new
						{
							AccountDate = (string)accountDate,
							AccountName = accountName,
							AlipayCode = alipayCode,
							AlipayRealName = alipayRealName,
							Amount = amount,
							AuditStatus = auditStatus,
							AuditStatusText = auditStatusText,
							Balance = balance,
							BankName = bankName,
							IsAlipay = isAlipay,
							IsWeixin = isWeixin,
							IsWithdrawToAccount = isWithdrawToAccount,
							JournalNumber = journalNumber,
							ManagerRemark = managerRemark,
							ManagerUserName = managerUserName,
							MerchantCode = merchantCode,
							Remark = remark,
							RequestDate = dateTime.ToString("yyyy-MM-dd"),
							RequestError = s.RequestError,
							RequestState = MemberProcessor.GetRequestState(s),
							RequestStateText = EnumDescription.GetEnumDescription((Enum)(object)(OnLinePayment)MemberProcessor.GetRequestState(s), 1),
							UserId = s.UserId,
							UserName = s.UserName
						};
					})
				}
			});
			context.Response.Write(s2);
			context.Response.End();
		}

		private SplittinDrawInfo GetSplittinDrawRequestInfo(HttpContext context)
		{
			SplittinDrawInfo splittinDrawInfo = new SplittinDrawInfo();
			splittinDrawInfo.UserId = HiContext.Current.UserId;
			splittinDrawInfo.UserName = HiContext.Current.User.UserName;
			splittinDrawInfo.RequestDate = DateTime.Now;
			if (!string.IsNullOrEmpty(context.Request["Remark"]))
			{
				splittinDrawInfo.Remark = Globals.UrlDecode(context.Request["Remark"]);
			}
			else
			{
				splittinDrawInfo.Remark = string.Empty;
			}
			int num = 0;
			if (!string.IsNullOrEmpty(context.Request["drawtype"]))
			{
				OnLinePayment onLinePayment;
				switch (Globals.UrlDecode(context.Request["drawtype"]).ToInt(0))
				{
				case 2:
				{
					splittinDrawInfo.IsWeixin = true;
					SplittinDrawInfo splittinDrawInfo2 = splittinDrawInfo;
					onLinePayment = OnLinePayment.NoPay;
					splittinDrawInfo2.RequestState = onLinePayment.GetHashCode().ToNullString();
					break;
				}
				case 3:
				{
					splittinDrawInfo.IsAlipay = true;
					SplittinDrawInfo splittinDrawInfo3 = splittinDrawInfo;
					onLinePayment = OnLinePayment.NoPay;
					splittinDrawInfo3.RequestState = onLinePayment.GetHashCode().ToNullString();
					break;
				}
				case 4:
					splittinDrawInfo.IsWithdrawToAccount = true;
					splittinDrawInfo.AccountDate = DateTime.Now;
					splittinDrawInfo.ManagerRemark = "提现至预付款账户" + (string.IsNullOrEmpty(splittinDrawInfo.Remark.ToNullString()) ? "" : ("（买家备注：" + splittinDrawInfo.Remark.ToNullString() + "）"));
					break;
				}
			}
			if (splittinDrawInfo.IsWithdrawToAccount)
			{
				splittinDrawInfo.AuditStatus = 2;
			}
			else
			{
				splittinDrawInfo.AuditStatus = 1;
			}
			if (!string.IsNullOrEmpty(context.Request["RealName"]))
			{
				splittinDrawInfo.AlipayRealName = Globals.UrlDecode(context.Request["RealName"]);
			}
			else
			{
				splittinDrawInfo.AlipayRealName = string.Empty;
			}
			if (!string.IsNullOrEmpty(context.Request["Code"]))
			{
				splittinDrawInfo.AlipayCode = Globals.UrlDecode(context.Request["Code"]);
			}
			else
			{
				splittinDrawInfo.AlipayCode = string.Empty;
			}
			if (!string.IsNullOrEmpty(context.Request["BankName"]))
			{
				splittinDrawInfo.BankName = Globals.UrlDecode(context.Request["BankName"]);
			}
			else
			{
				splittinDrawInfo.BankName = string.Empty;
			}
			if (!string.IsNullOrEmpty(context.Request["AccountName"]))
			{
				splittinDrawInfo.AccountName = Globals.UrlDecode(context.Request["AccountName"]);
			}
			else
			{
				splittinDrawInfo.AccountName = string.Empty;
			}
			if (!string.IsNullOrEmpty(context.Request["MerchantCode"]))
			{
				splittinDrawInfo.MerchantCode = Globals.UrlDecode(context.Request["MerchantCode"]);
			}
			else
			{
				splittinDrawInfo.MerchantCode = string.Empty;
			}
			decimal amount = default(decimal);
			if (!string.IsNullOrEmpty(context.Request["Amount"]) && decimal.TryParse(context.Request["Amount"], out amount))
			{
				splittinDrawInfo.Amount = amount;
			}
			else
			{
				splittinDrawInfo.Amount = decimal.Zero;
			}
			return splittinDrawInfo;
		}

		private void SaveBalance(int UserId, decimal Amount)
		{
			MemberInfo user = Users.GetUser(UserId);
			decimal balance = user.Balance + Amount;
			BalanceDetailInfo balanceDetailInfo = new BalanceDetailInfo();
			balanceDetailInfo.UserId = UserId;
			balanceDetailInfo.UserName = user.UserName;
			balanceDetailInfo.TradeDate = DateTime.Now;
			balanceDetailInfo.TradeType = TradeTypes.Commission;
			balanceDetailInfo.Income = Amount;
			balanceDetailInfo.Balance = balance;
			MemberProcessor.AddSplittinDrawBalance(balanceDetailInfo);
		}

		public void SplittinDraw(HttpContext context)
		{
			SplittinDrawInfo splittinDrawRequestInfo = this.GetSplittinDrawRequestInfo(context);
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			BalanceDrawRequestQuery balanceDrawRequestQuery = new BalanceDrawRequestQuery();
			balanceDrawRequestQuery.PageIndex = 1;
			balanceDrawRequestQuery.PageSize = 1;
			balanceDrawRequestQuery.UserId = HiContext.Current.UserId;
			DbQueryResult mySplittinDraws = MemberProcessor.GetMySplittinDraws(balanceDrawRequestQuery, 1);
			ApiErrorCode apiErrorCode;
			if (mySplittinDraws.TotalRecords > 0)
			{
				HttpResponse response = context.Response;
				apiErrorCode = ApiErrorCode.Paramter_Error;
				response.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "上笔提现管理员还没有处理，只有处理完后才能再次申请提现"));
			}
			else if (!masterSettings.SplittinDraws_CashToDeposit && !masterSettings.SplittinDraws_CashToBankCard && !masterSettings.SplittinDraws_CashToWeiXin && !masterSettings.SplittinDraws_CashToALiPay)
			{
				HttpResponse response2 = context.Response;
				apiErrorCode = ApiErrorCode.Paramter_Error;
				response2.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "没有合适的提现方式，请与管理员联系"));
			}
			else
			{
				int num = context.Request["drawtype"].ToInt(0);
				if (num < 1 || num > 4)
				{
					HttpResponse response3 = context.Response;
					apiErrorCode = ApiErrorCode.Paramter_Error;
					response3.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "错误的提现方式"));
				}
				else if (num == 4 && !HiContext.Current.User.IsOpenBalance)
				{
					HttpResponse response4 = context.Response;
					apiErrorCode = ApiErrorCode.UserNotOpenBalance;
					response4.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "余额帐号未开启"));
				}
				else if (num == 4 && string.IsNullOrEmpty(HiContext.Current.User.TradePassword))
				{
					HttpResponse response5 = context.Response;
					apiErrorCode = ApiErrorCode.TradePassword_NoSet;
					response5.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "交易密码未设置"));
				}
				else if (num == 2 && !masterSettings.EnableBulkPaymentWeixin)
				{
					HttpResponse response6 = context.Response;
					apiErrorCode = ApiErrorCode.Paramter_Error;
					response6.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "不支持微信提现"));
				}
				else if (num == 3 && !masterSettings.EnableBulkPaymentAliPay)
				{
					HttpResponse response7 = context.Response;
					apiErrorCode = ApiErrorCode.Paramter_Error;
					response7.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "不支持支付宝提现"));
				}
				else
				{
					decimal amount = splittinDrawRequestInfo.Amount;
					string text = amount.ToString();
					decimal d = default(decimal);
					if (text.IndexOf(".") > 0)
					{
						d = text.Substring(text.IndexOf(".") + 1).Length;
					}
					if (d > 2m)
					{
						HttpResponse response8 = context.Response;
						apiErrorCode = ApiErrorCode.Paramter_Error;
						response8.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "提现金额错误"));
					}
					else if (splittinDrawRequestInfo.Amount < masterSettings.MinimumSingleShot)
					{
						HttpResponse response9 = context.Response;
						apiErrorCode = ApiErrorCode.Paramter_Error;
						response9.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "提现金额必须大于或者等于单次提现最小限额"));
					}
					else
					{
						decimal userUseSplittin = MemberProcessor.GetUserUseSplittin(HiContext.Current.UserId);
						if (splittinDrawRequestInfo.Amount <= decimal.Zero)
						{
							HttpResponse response10 = context.Response;
							apiErrorCode = ApiErrorCode.Paramter_Error;
							response10.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "提现金额输入错误"));
						}
						else if (splittinDrawRequestInfo.Amount > userUseSplittin)
						{
							HttpResponse response11 = context.Response;
							apiErrorCode = ApiErrorCode.Paramter_Error;
							response11.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "可提现奖励不足"));
						}
						else
						{
							Regex regex = new Regex("(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
							Regex regex2 = regex;
							amount = splittinDrawRequestInfo.Amount;
							if (!regex2.IsMatch(amount.ToString()))
							{
								HttpResponse response12 = context.Response;
								apiErrorCode = ApiErrorCode.Paramter_Error;
								response12.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "请输入有效金额"));
							}
							else
							{
								if (splittinDrawRequestInfo.IsAlipay.HasValue && splittinDrawRequestInfo.IsAlipay.Value)
								{
									if (string.IsNullOrEmpty(splittinDrawRequestInfo.AlipayRealName))
									{
										HttpResponse response13 = context.Response;
										apiErrorCode = ApiErrorCode.Paramter_Error;
										response13.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "真实姓名为空"));
										return;
									}
									if (splittinDrawRequestInfo.AlipayRealName.Length > 20)
									{
										HttpResponse response14 = context.Response;
										apiErrorCode = ApiErrorCode.Paramter_Error;
										response14.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "姓名长度错误"));
										return;
									}
									if (string.IsNullOrEmpty(splittinDrawRequestInfo.AlipayCode))
									{
										HttpResponse response15 = context.Response;
										apiErrorCode = ApiErrorCode.Paramter_Error;
										response15.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "收款账号为空"));
										return;
									}
									if (splittinDrawRequestInfo.AlipayCode.Length > 60)
									{
										HttpResponse response16 = context.Response;
										apiErrorCode = ApiErrorCode.Paramter_Error;
										response16.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "账号长度错误"));
										return;
									}
								}
								else if (!splittinDrawRequestInfo.IsWeixin.HasValue || !splittinDrawRequestInfo.IsWeixin.Value)
								{
									if (splittinDrawRequestInfo.IsWithdrawToAccount)
									{
										splittinDrawRequestInfo.AuditStatus = 2;
									}
									else
									{
										if (string.IsNullOrEmpty(splittinDrawRequestInfo.BankName))
										{
											HttpResponse response17 = context.Response;
											apiErrorCode = ApiErrorCode.Paramter_Error;
											response17.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "开户银行为空"));
											return;
										}
										if (splittinDrawRequestInfo.BankName.Length > 60)
										{
											HttpResponse response18 = context.Response;
											apiErrorCode = ApiErrorCode.Paramter_Error;
											response18.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "开户银行错误"));
											return;
										}
										if (string.IsNullOrEmpty(splittinDrawRequestInfo.AccountName))
										{
											HttpResponse response19 = context.Response;
											apiErrorCode = ApiErrorCode.Paramter_Error;
											response19.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "银行开户名为空"));
											return;
										}
										if (splittinDrawRequestInfo.AccountName.Length > 30)
										{
											HttpResponse response20 = context.Response;
											apiErrorCode = ApiErrorCode.Paramter_Error;
											response20.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "银行开户名错误"));
											return;
										}
										if (string.IsNullOrEmpty(splittinDrawRequestInfo.MerchantCode))
										{
											HttpResponse response21 = context.Response;
											apiErrorCode = ApiErrorCode.Paramter_Error;
											response21.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "提现帐号为空"));
											return;
										}
										if (splittinDrawRequestInfo.MerchantCode.Length > 100)
										{
											HttpResponse response22 = context.Response;
											apiErrorCode = ApiErrorCode.Paramter_Error;
											response22.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "提现账号错误"));
											return;
										}
									}
								}
								string empty = string.Empty;
								if (!string.IsNullOrEmpty(context.Request["TradePassword"]))
								{
									empty = Globals.UrlDecode(context.Request["TradePassword"]);
									if (empty.Length < 6 || empty.Length > 20)
									{
										HttpResponse response23 = context.Response;
										apiErrorCode = ApiErrorCode.Paramter_Error;
										response23.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "密码长度错误"));
									}
									else if (!MemberProcessor.ValidTradePassword(empty))
									{
										HttpResponse response24 = context.Response;
										apiErrorCode = ApiErrorCode.Paramter_Error;
										response24.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "交易密码不正确"));
									}
									else if (splittinDrawRequestInfo.Remark.Length > 300)
									{
										HttpResponse response25 = context.Response;
										apiErrorCode = ApiErrorCode.Paramter_Error;
										response25.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "备注长度错误"));
									}
									else if (MemberProcessor.SplittinDrawRequest(splittinDrawRequestInfo))
									{
										if (splittinDrawRequestInfo.IsWithdrawToAccount)
										{
											ReferralDao referralDao = new ReferralDao();
											SplittinDetailInfo splittinDetailInfo = new SplittinDetailInfo();
											splittinDetailInfo.OrderId = string.Empty;
											splittinDetailInfo.UserId = splittinDrawRequestInfo.UserId;
											splittinDetailInfo.UserName = splittinDrawRequestInfo.UserName;
											splittinDetailInfo.IsUse = true;
											splittinDetailInfo.TradeDate = DateTime.Now;
											splittinDetailInfo.TradeType = SplittingTypes.DrawRequest;
											splittinDetailInfo.Expenses = splittinDrawRequestInfo.Amount;
											splittinDetailInfo.Balance = referralDao.GetUserUseSplittin(splittinDrawRequestInfo.UserId) - splittinDrawRequestInfo.Amount;
											splittinDetailInfo.Remark = "提现到预付款自动处理";
											splittinDetailInfo.ManagerUserName = "";
											referralDao.Add(splittinDetailInfo, null);
											this.SaveBalance(splittinDrawRequestInfo.UserId, splittinDrawRequestInfo.Amount);
											Users.ClearUserCache(HiContext.Current.UserId, HiContext.Current.User.SessionId);
											context.Response.Write(this.GetOKJson("提现成功"));
										}
										else
										{
											context.Response.Write(this.GetOKJson("提现申请成功"));
										}
									}
									else
									{
										HttpResponse response26 = context.Response;
										apiErrorCode = ApiErrorCode.Unknown_Error;
										response26.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "提现申请失败"));
									}
								}
								else
								{
									HttpResponse response27 = context.Response;
									apiErrorCode = ApiErrorCode.Paramter_Error;
									response27.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "请输入交易密码"));
								}
							}
						}
					}
				}
			}
		}

		public void SubMembers(HttpContext context)
		{
			this.CheckOpenId();
			int num = context.Request["pageIndex"].ToInt(0);
			if (num <= 0)
			{
				num = 1;
			}
			int num2 = context.Request["pageSize"].ToInt(0);
			if (num2 < 1)
			{
				num2 = 10;
			}
			MemberInfo user = HiContext.Current.User;
			MemberQuery memberQuery = new MemberQuery();
			memberQuery.PageIndex = num;
			memberQuery.PageSize = num2;
			memberQuery.UserName = Globals.StripAllTags(context.Request["keyword"].ToNullString());
			memberQuery.RealName = Globals.StripAllTags(context.Request["realname"].ToNullString());
			memberQuery.CellPhone = Globals.StripAllTags(context.Request["cellphone"].ToNullString());
			PageModel<SubMember> mySubUserList = MemberProcessor.GetMySubUserList(memberQuery);
			string s2 = JsonConvert.SerializeObject(new
			{
				SubMember_get_response = new
				{
					RecordCount = mySubUserList.Total,
					ExpandMemberInMonth = MemberProcessor.GetLowerNumByUserIdNowMonth(user.UserId),
					ExpandMemberAll = MemberProcessor.GetLowerNumByUserId(user.UserId),
					LowerUserSaleTotal = MemberProcessor.GetLowerSaleTotalByUserId(user.UserId),
					SubMembers = mySubUserList.Models.Select(delegate(SubMember s)
					{
						string cellPhone = string.IsNullOrEmpty(s.ReferralCellPhone) ? s.CellPhone : s.ReferralCellPhone;
						DateTime dateTime = s.CreateDate;
						string createDate = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
						object lastReferralDate;
						if (!s.LastReferralDate.HasValue)
						{
							lastReferralDate = "";
						}
						else
						{
							dateTime = s.LastReferralDate.Value;
							lastReferralDate = dateTime.ToString("yyyy-MM-dd");
						}
						int orderNumber = s.OrderNumber;
						string nickName = s.NickName;
						object referralAuditDate;
						if (!s.ReferralAuditDate.HasValue)
						{
							referralAuditDate = "";
						}
						else
						{
							dateTime = s.ReferralAuditDate.Value;
							referralAuditDate = dateTime.ToString("yyyy-MM-dd");
						}
						return new
						{
							CellPhone = cellPhone,
							CreateDate = createDate,
							LastReferralDate = (string)lastReferralDate,
							OrderNumber = orderNumber,
							RealName = nickName,
							ReferralAuditDate = (string)referralAuditDate,
							ReferralOrderNumber = s.ReferralOrderNumber,
							SubMemberAllSplittin = s.SubMemberAllSplittin.F2ToString("f2").ToDecimal(0),
							SubReferralSplittin = s.SubReferralSplittin.F2ToString("f2").ToDecimal(0),
							SubSumOrderTotal = s.SubSumOrderTotal.F2ToString("f2").ToDecimal(0),
							UserID = s.UserID,
							UserName = s.UserName
						};
					})
				}
			});
			context.Response.Write(s2);
			context.Response.End();
		}

		public void SplittinList(HttpContext context)
		{
			this.CheckOpenId();
			int num = context.Request["pageIndex"].ToInt(0);
			if (num <= 0)
			{
				num = 1;
			}
			int num2 = context.Request["pageSize"].ToInt(0);
			if (num2 < 1)
			{
				num2 = 10;
			}
			BalanceDetailQuery balanceDetailQuery = new BalanceDetailQuery();
			MemberInfo user = Users.GetUser(HiContext.Current.UserId);
			balanceDetailQuery.UserId = HiContext.Current.UserId;
			balanceDetailQuery.PageIndex = num;
			balanceDetailQuery.PageSize = num2;
			PageModel<SplittinDetailInfo> splittinDetailList = MemberHelper.GetSplittinDetailList(balanceDetailQuery);
			string s2 = JsonConvert.SerializeObject(new
			{
				splittin_get_response = new
				{
					RecordCount = splittinDetailList.Total,
					SplittinTotal = ((user.Referral == null) ? decimal.Zero : MemberProcessor.GetSplittinTotal(user.UserId).F2ToString("f2").ToDecimal(0)),
					HistorySplittin = ((user.Referral == null) ? decimal.Zero : MemberProcessor.GetUserAllSplittin(user.UserId).F2ToString("f2").ToDecimal(0)),
					CanDrawSplittin = ((user.Referral == null) ? decimal.Zero : MemberProcessor.GetUserUseSplittin(user.UserId).F2ToString("f2").ToDecimal(0)),
					NoSettlementSplttin = ((user.Referral == null) ? decimal.Zero : MemberProcessor.GetUserNoUseSplittin(user.UserId).F2ToString("f2").ToDecimal(0)),
					DrawSplittinTotal = MemberProcessor.GetUserDrawSplittin().F2ToString("f2").ToDecimal(0),
					SplittinList = splittinDetailList.Models.Select(delegate(SplittinDetailInfo s)
					{
						DateTime dateTime = s.TradeDate;
						string tradeDate = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
						SplittingTypes tradeType = s.TradeType;
						string enumDescription = EnumDescription.GetEnumDescription((Enum)(object)s.TradeType, 0);
						int userId = s.UserId;
						decimal balance = s.Balance.F2ToString("f2").ToDecimal(0);
						decimal expenses = s.Expenses.HasValue ? s.Expenses.F2ToString("f2").ToDecimal(0) : 0.00m;
						object finishDate;
						if (s.FinishDate.HasValue)
						{
							dateTime = s.FinishDate.Value;
							finishDate = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
						}
						else
						{
							finishDate = "";
						}
						return new
						{
							TradeDate = tradeDate,
							TradeType = tradeType,
							TradeTypeText = enumDescription,
							UserId = userId,
							Balance = balance,
							Expenses = expenses,
							FinishDate = (string)finishDate,
							FromUserName = s.FromUserName,
							Income = (s.Income.HasValue ? s.Income.F2ToString("f2").ToDecimal(0) : 0.00m),
							IsUse = s.IsUse,
							JournalNumber = s.JournalNumber,
							OrderId = s.OrderId,
							OrderTotal = s.OrderTotal.F2ToString("f2").ToDecimal(0),
							Remark = s.Remark,
							SubUserId = s.SubUserId
						};
					})
				}
			});
			context.Response.Write(s2);
			context.Response.End();
		}

		public void ReferralRegister(HttpContext context)
		{
			this.CheckOpenId();
			string openId = context.Request["openId"].ToNullString();
			string text = Globals.StripAllTags(context.Request["RealName"].ToNullString());
			string text2 = Globals.StripAllTags(context.Request["Address"].ToNullString());
			string s = Globals.StripAllTags(context.Request["RegionId"].ToNullString());
			string text3 = Globals.StripAllTags(context.Request["Email"].ToNullString());
			string text4 = Globals.StripAllTags(context.Request["Phone"].ToNullString());
			string text5 = Globals.StripAllTags(context.Request["NumberCode"].ToNullString());
			string text6 = Globals.StripAllTags(context.Request["PhoneCode"].ToNullString());
			string text7 = Globals.StripAllTags(context.Request["shopName"].ToNullString());
			string text8 = Globals.StripAllTags(context.Request["bannerUrl"].ToNullString());
			int num = 0;
			int.TryParse(s, out num);
			int topRegionId = RegionHelper.GetTopRegionId(num, true);
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			string promoterNeedInfo = masterSettings.PromoterNeedInfo;
			bool flag = promoterNeedInfo.Contains("1");
			bool flag2 = promoterNeedInfo.Contains("2");
			bool flag3 = promoterNeedInfo.Contains("3");
			bool flag4 = promoterNeedInfo.Contains("4");
			bool isPromoterValidatePhone = masterSettings.IsPromoterValidatePhone;
			string imageServerUrl = Globals.GetImageServerUrl();
			if (text8.Length > 0)
			{
				string[] array = text8.Split(',');
				text8 = "";
				for (int i = 0; i < array.Length; i++)
				{
					text8 += (string.IsNullOrEmpty(imageServerUrl) ? (Globals.SaveFile("referral\\banner", array[i], "/Storage/master/", true, false, "") + "|") : (array[i] + "|"));
				}
				text8 = text8.TrimEnd('|');
			}
			context.Response.ContentType = "application/json";
			ApiErrorCode apiErrorCode;
			if (HiContext.Current.UserId == 0)
			{
				HttpResponse response = context.Response;
				apiErrorCode = ApiErrorCode.UserNoLogin;
				response.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "请您先登录"));
			}
			else if (HiContext.Current.SiteSettings.ApplyReferralCondition == 1 && HiContext.Current.User.Expenditure < HiContext.Current.SiteSettings.ApplyReferralNeedAmount)
			{
				HttpResponse response2 = context.Response;
				apiErrorCode = ApiErrorCode.UserConsumeNotEnough;
				response2.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "您的消费金额还没达到系统设置的金额(" + HiContext.Current.SiteSettings.ApplyReferralNeedAmount.F2ToString("f2") + ")元"));
			}
			else if (string.IsNullOrEmpty(text7) || text7.Length > 100)
			{
				HttpResponse response3 = context.Response;
				apiErrorCode = ApiErrorCode.Paramter_Error;
				response3.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "请输入店铺名称"));
			}
			else if (string.IsNullOrEmpty(text) & flag)
			{
				HttpResponse response4 = context.Response;
				apiErrorCode = ApiErrorCode.Paramter_Error;
				response4.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "请填写真实姓名"));
			}
			else if (string.IsNullOrEmpty(text3) & flag3)
			{
				HttpResponse response5 = context.Response;
				apiErrorCode = ApiErrorCode.Paramter_Error;
				response5.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "请填写邮箱"));
			}
			else
			{
				if (text3.Length > 0)
				{
					MemberInfo memberInfo = MemberProcessor.FindMemberByEmail(text3);
					if (memberInfo != null && memberInfo.UserId != HiContext.Current.UserId)
					{
						HttpResponse response6 = context.Response;
						apiErrorCode = ApiErrorCode.Paramter_Error;
						response6.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "邮箱已被占用"));
						return;
					}
				}
				if (string.IsNullOrEmpty(text4) & flag2)
				{
					HttpResponse response7 = context.Response;
					apiErrorCode = ApiErrorCode.Paramter_Error;
					response7.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "请填写手机号码"));
				}
				else if (text4.Length > 0 && !DataHelper.IsMobile(text4))
				{
					HttpResponse response8 = context.Response;
					apiErrorCode = ApiErrorCode.Paramter_Error;
					response8.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "错误的手机号码"));
				}
				else
				{
					if (text4.Length > 0)
					{
						MemberInfo memberInfo2 = MemberProcessor.FindMemberByCellphone(text4);
						if (memberInfo2 != null && memberInfo2.UserId != HiContext.Current.UserId)
						{
							HttpResponse response9 = context.Response;
							apiErrorCode = ApiErrorCode.MobbileIsBinding;
							response9.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "手机号码被占用"));
							return;
						}
					}
					if (isPromoterValidatePhone & flag2)
					{
						if (string.IsNullOrEmpty(text5))
						{
							HttpResponse response10 = context.Response;
							apiErrorCode = ApiErrorCode.Paramter_Error;
							response10.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "图形验证码错误"));
							return;
						}
						if (!HiContext.Current.CheckVerifyCode(text5, openId))
						{
							HttpResponse response11 = context.Response;
							apiErrorCode = ApiErrorCode.Paramter_Error;
							response11.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "图形验证码错误"));
							return;
						}
						if (string.IsNullOrEmpty(text6))
						{
							HttpResponse response12 = context.Response;
							apiErrorCode = ApiErrorCode.Paramter_Error;
							response12.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "短信验证码错误"));
							return;
						}
						string text9 = "";
						if (!HiContext.Current.CheckPhoneVerifyCode(text6, text4, out text9))
						{
							HttpResponse response13 = context.Response;
							apiErrorCode = ApiErrorCode.Paramter_Error;
							response13.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "短信验证码错误"));
							return;
						}
					}
					if (topRegionId == 0 & flag4)
					{
						HttpResponse response14 = context.Response;
						apiErrorCode = ApiErrorCode.Paramter_Error;
						response14.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "请填写详细地址"));
					}
					else if (string.IsNullOrEmpty(text2) & flag4)
					{
						HttpResponse response15 = context.Response;
						apiErrorCode = ApiErrorCode.Paramter_Error;
						response15.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "请填写详细地址"));
					}
					else if (MemberProcessor.ReferralRequest(HiContext.Current.UserId, text, text4, topRegionId, num, text2, text3, text7, text8))
					{
						context.Response.Write(this.GetOKJson("申请提交成功"));
						Users.ClearUserCache(HiContext.Current.UserId, HiContext.Current.User.SessionId);
					}
					else
					{
						HttpResponse response16 = context.Response;
						apiErrorCode = ApiErrorCode.Unknown_Error;
						response16.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "提交申请失败"));
					}
				}
			}
		}

		private void BindPhone(HttpContext context)
		{
			this.CheckOpenId();
			string text = context.Request["phone"].ToNullString();
			string verifyCode = context.Request["VerifyCode"].ToNullString();
			ApiErrorCode apiErrorCode;
			if (string.IsNullOrEmpty(text))
			{
				HttpResponse response = context.Response;
				apiErrorCode = ApiErrorCode.Paramter_Error;
				response.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "错误的手机号码"));
			}
			if (!DataHelper.IsMobile(text))
			{
				HttpResponse response2 = context.Response;
				apiErrorCode = ApiErrorCode.Paramter_Error;
				response2.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "错误的手机号码"));
			}
			else if (MemberProcessor.FindMemberByCellphone(text) != null)
			{
				HttpResponse response3 = context.Response;
				apiErrorCode = ApiErrorCode.MobbileIsBinding;
				response3.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "手机号码被占用"));
			}
			else
			{
				MemberInfo user = HiContext.Current.User;
				if (user == null)
				{
					HttpResponse response4 = context.Response;
					apiErrorCode = ApiErrorCode.UserNoLogin;
					response4.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "用户没有登录"));
				}
				else
				{
					string errorMsg = "";
					if (!HiContext.Current.CheckPhoneVerifyCode(verifyCode, text, out errorMsg))
					{
						HttpResponse response5 = context.Response;
						apiErrorCode = ApiErrorCode.Paramter_Error;
						response5.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), errorMsg));
					}
					else
					{
						if (user.UserName.IndexOf("YSC_") >= 0 || user.UserName == user.CellPhone)
						{
							user.UserName = text;
						}
						user.CellPhone = text;
						user.CellPhoneVerification = true;
						if (MemberProcessor.UpdateMember(user))
						{
							context.Response.Write(this.GetOKJson("手机绑定成功"));
						}
						else
						{
							HttpResponse response6 = context.Response;
							apiErrorCode = ApiErrorCode.Unknown_Error;
							response6.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "手机绑定失败"));
						}
					}
				}
			}
		}

		private void SendVerifyCode(HttpContext context)
		{
			string text = context.Request["Phone"];
			string text2 = context.Request["openId"];
			string verifyCode = context.Request["imgCode"].ToNullString();
			bool flag = context.Request["IsValidPhone"].ToBool();
			ApiErrorCode apiErrorCode;
			if (!DataHelper.IsMobile(text))
			{
				HttpResponse response = context.Response;
				apiErrorCode = ApiErrorCode.Paramter_Error;
				response.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "错误的手机号码"));
			}
			else
			{
				string code = HiContext.Current.CreatePhoneCode(4, text, VerifyCodeType.Digital);
				if (flag)
				{
					MemberInfo memberInfo = MemberProcessor.FindMemberByCellphone(text);
					if (memberInfo != null)
					{
						HttpResponse response2 = context.Response;
						apiErrorCode = ApiErrorCode.MobbileIsBinding;
						response2.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "手机号码被占用"));
						return;
					}
				}
				bool flag2 = true;
				if (!string.IsNullOrEmpty(context.Request["imgCode"]) && context.Request["imgCode"] == "0")
				{
					flag2 = false;
				}
				if (flag2 && !HiContext.Current.CheckVerifyCode(verifyCode, text2))
				{
					HttpResponse response3 = context.Response;
					apiErrorCode = ApiErrorCode.Paramter_Error;
					response3.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "图形验证码错误"));
				}
				else
				{
					SiteSettings siteSettings = HiContext.Current.SiteSettings;
					if (!siteSettings.SMSEnabled || string.IsNullOrEmpty(siteSettings.SMSSettings))
					{
						HttpResponse response4 = context.Response;
						apiErrorCode = ApiErrorCode.Paramter_Error;
						response4.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "短信服务未配置"));
					}
					else
					{
						this.SendVerifyCode(siteSettings, text, code, text2);
					}
				}
			}
		}

		private void SendVerifyCode(SiteSettings settings, string cellphone, string code, string sessionID)
		{
			ApiErrorCode apiErrorCode;
			try
			{
                string TemplateCode = "";
                string iPAddress = Globals.IPAddress;
				if (!new MemberDao().ValidateIPCanSendSMS(iPAddress, settings.IPSMSCount))
				{
					HttpResponse response = this.context.Response;
					apiErrorCode = ApiErrorCode.Paramter_Error;
					response.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "验证码发送过多"));
				}
				else
				{
					int phoneSendSmsTimes = new MemberDao().GetPhoneSendSmsTimes(cellphone);
					if (phoneSendSmsTimes >= settings.PhoneSMSCount)
					{
						HttpResponse response2 = this.context.Response;
						apiErrorCode = ApiErrorCode.Paramter_Error;
						response2.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "验证码发送过多"));
					}
					else
					{
						int num = 0;
						DateTime dateTime = DateTime.Now;
						DateTime value = dateTime.AddSeconds(-121.0);
						object obj = HiCache.Get($"DataCache-LastSendSMSTimeCacheKey-{sessionID}");
						if (obj != null)
						{
							DateTime.TryParse(obj.ToString(), out value);
						}
						dateTime = DateTime.Now;
						TimeSpan timeSpan = dateTime.Subtract(value);
						if (timeSpan.TotalSeconds < 120.0)
						{
							HttpResponse response3 = this.context.Response;
							apiErrorCode = ApiErrorCode.Paramter_Error;
							response3.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "验证码发送太频繁"));
						}
						else
						{
							object obj2 = HiCache.Get($"DataCache-SendSMSTimesCacheKey-{sessionID}");
							if (obj2 != null)
							{
								int.TryParse(obj2.ToString(), out num);
							}
							if (num >= 10)
							{
								HttpResponse response4 = this.context.Response;
								apiErrorCode = ApiErrorCode.Paramter_Error;
								response4.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "验证码发送过多"));
							}
							else
							{
								ConfigData configData = new ConfigData(HiCryptographer.TryDecypt(settings.SMSSettings));
								SMSSender sMSSender = SMSSender.CreateInstance(settings.SMSSender, configData.SettingsXml);
								string message = string.Format("{\"code\":\"{0}\"}", code);
								string okMsg = "";
								if (sMSSender.Send(cellphone, TemplateCode, message, out okMsg, "2"))
								{
									string key = $"DataCache-SendSMSTimesCacheKey-{sessionID}";
									object obj3 = num + 1;
									dateTime = DateTime.Now;
									dateTime = dateTime.Date;
									dateTime = dateTime.AddDays(1.0);
									timeSpan = dateTime.Subtract(DateTime.Now);
									HiCache.Insert(key, obj3, (int)timeSpan.TotalSeconds);
									string key2 = $"DataCache-LastSendSMSTimeCacheKey-{sessionID}";
									object obj4 = DateTime.Now;
									dateTime = DateTime.Now;
									dateTime = dateTime.Date;
									dateTime = dateTime.AddDays(1.0);
									timeSpan = dateTime.Subtract(DateTime.Now);
									HiCache.Insert(key2, obj4, (int)timeSpan.TotalSeconds);
									HiCache.Insert($"DataCache-PhoneCode-{cellphone}", HiCryptographer.Encrypt(code), 10800);
									okMsg = "短信发送成功";
									new MemberDao().SaveSmsIp(iPAddress);
									new MemberDao().SavePhoneSendTimes(cellphone);
									this.context.Response.Write(this.GetOKJson(okMsg));
								}
								else
								{
									HttpResponse response5 = this.context.Response;
									apiErrorCode = ApiErrorCode.Paramter_Error;
									response5.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "短信发送失败"));
								}
							}
						}
					}
				}
			}
			catch (Exception)
			{
				HttpResponse response6 = this.context.Response;
				apiErrorCode = ApiErrorCode.Paramter_Error;
				response6.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "短信发送失败"));
			}
		}

		private void CellPhoneRegister(HttpContext context)
		{
			MemberInfo memberInfo = null;
			string text = context.Request["openId"].ToNullString();
			string text2 = context.Request["nickName"].ToNullString();
			int referralUserId = context.Request["referralUserId"].ToInt(0);
			string unionId = this.getUnionId();
			string text3 = Globals.UrlDecode(context.Request["headImage"].ToNullString());
			string text4 = context.Request["cellphone"].ToNullString();
			string verifyCode = context.Request["verifyCode"].ToNullString();
			string text5 = context.Request["password"].ToNullString();
			ApiErrorCode apiErrorCode;
			if (text5.Length < 6 || text5.Length > 20)
			{
				HttpResponse response = context.Response;
				apiErrorCode = ApiErrorCode.Paramter_Error;
				response.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "请输入用户密码"));
			}
			else if (string.IsNullOrEmpty(text4) || !DataHelper.IsMobile(text4))
			{
				HttpResponse response2 = context.Response;
				apiErrorCode = ApiErrorCode.Paramter_Error;
				response2.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "错误的手机号码"));
			}
			else
			{
				string text6 = "";
				if (!HiContext.Current.CheckPhoneVerifyCode(verifyCode, text4, out text6))
				{
					HttpResponse response3 = context.Response;
					apiErrorCode = ApiErrorCode.Paramter_Error;
					response3.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "手机验证码错误"));
				}
				else if (MemberProcessor.FindMemberByCellphone(text4) != null)
				{
					HttpResponse response4 = context.Response;
					apiErrorCode = ApiErrorCode.MobbileIsBinding;
					response4.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "手机号码被占用"));
				}
				else
				{
					memberInfo = MemberProcessor.GetMemberByOpenIdOfQuickLogin("hishop.plugins.openid.wxapplet", text);
					bool flag = false;
					if (memberInfo == null)
					{
						memberInfo = MemberProcessor.GetMemberByUnionId(unionId);
						if (memberInfo != null)
						{
							flag = true;
						}
					}
					if (memberInfo?.CellPhoneVerification ?? false)
					{
						HttpResponse response5 = context.Response;
						apiErrorCode = ApiErrorCode.MobbileIsBinding;
						response5.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "已绑定手机号码"));
					}
					else if (memberInfo != null && memberInfo.Password != Users.EncodePassword(text5, memberInfo.PasswordSalt))
					{
						HttpResponse response6 = context.Response;
						apiErrorCode = ApiErrorCode.Password_Error;
						response6.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "用户密码错误"));
					}
					else if (memberInfo != null)
					{
						memberInfo.CellPhone = text4;
						memberInfo.CellPhoneVerification = true;
						memberInfo.IsQuickLogin = true;
						memberInfo.IsLogined = true;
						MemberProcessor.UpdateMember(memberInfo);
						bool flag2 = MemberProcessor.IsBindedWeixin(memberInfo.UserId, "hishop.plugins.openid.wxapplet");
						if (!string.IsNullOrEmpty(text3) || text3.StartsWith("http://wx.qlogo.cn/mmopen/"))
						{
							memberInfo.Picture = text3;
						}
						if (!string.IsNullOrEmpty(unionId) && memberInfo.UnionId != unionId && !flag && MemberProcessor.GetMemberByUnionId(unionId) == null)
						{
							memberInfo.UnionId = unionId;
						}
						if (flag)
						{
							if (!flag2)
							{
								MemberOpenIdInfo memberOpenIdInfo = new MemberOpenIdInfo();
								memberOpenIdInfo.UserId = memberInfo.UserId;
								memberOpenIdInfo.OpenIdType = "hishop.plugins.openid.wxapplet";
								memberOpenIdInfo.OpenId = text;
								MemberProcessor.AddMemberOpenId(memberOpenIdInfo);
								memberInfo.IsQuickLogin = true;
							}
							else
							{
								MemberOpenIdInfo memberOpenIdInfo2 = new MemberOpenIdInfo();
								memberOpenIdInfo2.UserId = memberInfo.UserId;
								memberOpenIdInfo2.OpenIdType = "hishop.plugins.openid.wxapplet";
								memberOpenIdInfo2.OpenId = text;
								MemberProcessor.UpdateMemberOpenId(memberOpenIdInfo2);
							}
						}
						MemberProcessor.UpdateMember(memberInfo);
						Users.SetCurrentUser(memberInfo.UserId, 30, true, false);
						HiContext.Current.User = memberInfo;
						if (!string.IsNullOrEmpty(text))
						{
							HttpCookie httpCookie = new HttpCookie("openId");
							httpCookie.HttpOnly = true;
							httpCookie.Value = text;
							httpCookie.Expires = DateTime.MaxValue;
							HttpContext.Current.Response.Cookies.Add(httpCookie);
						}
						this.GetMember(memberInfo, text);
					}
					else
					{
						int num = 1;
						memberInfo = new MemberInfo();
						memberInfo.IsLogined = true;
						if (!string.IsNullOrEmpty(text3) || text3.StartsWith("http://wx.qlogo.cn/mmopen/"))
						{
							memberInfo.Picture = text3;
						}
						int num2 = 0;
						memberInfo.GradeId = MemberProcessor.GetDefaultMemberGrade();
						memberInfo.UserName = text4;
						memberInfo.CellPhone = text4;
						memberInfo.CellPhoneVerification = true;
						if (!string.IsNullOrEmpty(text2))
						{
							memberInfo.NickName = HttpUtility.UrlDecode(text2);
						}
						memberInfo.ReferralUserId = referralUserId;
						string text7 = "Open";
						text5 = (memberInfo.Password = Users.EncodePassword(text5, text7));
						memberInfo.PasswordSalt = text7;
						memberInfo.RegisteredSource = 6;
						memberInfo.CreateDate = DateTime.Now;
						memberInfo.IsQuickLogin = true;
						memberInfo.IsLogined = true;
						memberInfo.UnionId = unionId;
						num2 = MemberProcessor.CreateMember(memberInfo);
						if (num2 <= 0)
						{
							num = -1;
						}
						if (num == 1)
						{
							memberInfo.UserId = num2;
							memberInfo.UserName = MemberHelper.GetUserName(num2);
							MemberHelper.Update(memberInfo, true);
							if (!string.IsNullOrEmpty(text))
							{
								MemberOpenIdInfo memberOpenIdInfo3 = new MemberOpenIdInfo();
								memberOpenIdInfo3.UserId = memberInfo.UserId;
								memberOpenIdInfo3.OpenIdType = "hishop.plugins.openid.wxapplet";
								memberOpenIdInfo3.OpenId = text;
								if (MemberProcessor.GetMemberByOpenId(memberOpenIdInfo3.OpenIdType, text) == null)
								{
									MemberProcessor.AddMemberOpenId(memberOpenIdInfo3);
								}
								if (!string.IsNullOrEmpty(text))
								{
									HttpCookie httpCookie2 = new HttpCookie("openId");
									httpCookie2.HttpOnly = true;
									httpCookie2.Value = text;
									httpCookie2.Expires = DateTime.MaxValue;
									HttpContext.Current.Response.Cookies.Add(httpCookie2);
								}
							}
							Users.ClearUserCache(memberInfo.UserId, "");
							Users.SetCurrentUser(memberInfo.UserId, 30, false, false);
							HiContext.Current.User = memberInfo;
						}
						this.GetMember(memberInfo, text);
					}
				}
			}
		}

		public void BindUser(HttpContext context)
		{
			MemberInfo memberInfo = null;
			string text = context.Request["openId"].ToNullString();
			string text2 = context.Request["nickName"].ToNullString();
			string unionId = this.getUnionId();
			string text3 = Globals.UrlDecode(context.Request["headImage"].ToNullString());
			string userName = context.Request["username"].ToNullString();
			string password = context.Request["password"].ToNullString();
			memberInfo = MemberProcessor.GetMemberByOpenIdOfQuickLogin("hishop.plugins.openid.wxapplet", text);
			if (memberInfo == null)
			{
				memberInfo = MemberProcessor.GetMemberByUnionId(unionId);
			}
			MemberInfo memberInfo2 = MemberProcessor.ValidLogin(userName, password);
			ApiErrorCode apiErrorCode;
			if (memberInfo2 == null)
			{
				HttpResponse response = context.Response;
				apiErrorCode = ApiErrorCode.Password_Error;
				response.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "用户名密码错误"));
			}
			else
			{
				MemberOpenIdInfo memberOpenIdInfo = MemberProcessor.GetMemberOpenIdInfo(memberInfo2.UserId, "hishop.plugins.openid.wxapplet");
				if (memberOpenIdInfo != null && memberOpenIdInfo.OpenId != text)
				{
					HttpResponse response2 = context.Response;
					apiErrorCode = ApiErrorCode.UserHasBindTrustLogin;
					response2.Write(this.GetErrorJson(apiErrorCode.GetHashCode(), "已绑定其它帐号"));
				}
				else
				{
					memberInfo2.IsQuickLogin = true;
					memberInfo2.IsLogined = true;
					memberInfo2.UnionId = unionId;
					MemberProcessor.UpdateMember(memberInfo2);
					if (!string.IsNullOrEmpty(text) && memberOpenIdInfo == null)
					{
						MemberOpenIdInfo memberOpenIdInfo2 = new MemberOpenIdInfo();
						memberOpenIdInfo2.UserId = memberInfo2.UserId;
						memberOpenIdInfo2.OpenIdType = "hishop.plugins.openid.wxapplet";
						memberOpenIdInfo2.OpenId = text;
						MemberProcessor.AddMemberOpenId(memberOpenIdInfo2);
					}
					Users.ClearUserCache(memberInfo2.UserId, "");
					Users.SetCurrentUser(memberInfo2.UserId, 30, false, false);
					HiContext.Current.User = memberInfo2;
					HttpCookie httpCookie = new HttpCookie("openId");
					httpCookie.HttpOnly = true;
					httpCookie.Value = text;
					httpCookie.Expires = DateTime.MaxValue;
					HttpContext.Current.Response.Cookies.Add(httpCookie);
					this.GetMember(memberInfo2, text);
				}
			}
		}

		public void GetRegionByLatLng(HttpContext context)
		{
			this.BindUserByOpenId();
			string latLng = context.Request["fromLatLng"].ToNullString();
			string address = "";
			string province = "";
			string city = "";
			string text = "";
			string country = "";
			DepotHelper.GetAddressByLatLng(latLng, ref address, ref province, ref city, ref country, ref text);
			int regionId = RegionHelper.GetRegionId("", country, city, province);
			Hidistro.Entities.Store.RegionInfo regionInfo = new Hidistro.Entities.Store.RegionInfo();
			if (regionId > 0)
			{
				regionInfo = RegionHelper.GetRegion(regionId, true);
			}
			string s = JsonConvert.SerializeObject(new
			{
				RegionId = regionId,
				FullRegionName = RegionHelper.GetFullRegion(regionId, " ", true, 0),
				Address = address
			});
			context.Response.Write(s);
		}

		private void UpdateUserInvoice(HttpContext context)
		{
			this.CheckOpenId();
			int userId = HiContext.Current.UserId;
			if (userId <= 0)
			{
				context.Response.Write(this.GetErrorJson("用户未登录"));
			}
			else
			{
				string text = context.Request["data"].ToNullString();
				UserInvoiceDataInfo userInvoiceDataInfo = new UserInvoiceDataInfo();
				if (string.IsNullOrEmpty(text))
				{
					context.Response.Write(this.GetErrorJson("发票数据为空"));
				}
				else
				{
					userInvoiceDataInfo = JsonHelper.ParseFormJson<UserInvoiceDataInfo>(text);
					if (userInvoiceDataInfo == null)
					{
						context.Response.Write(this.GetErrorJson("发票数据错误"));
					}
					else
					{
						userInvoiceDataInfo.LastUseTime = DateTime.Now;
						if (userInvoiceDataInfo.Id > 0)
						{
							UserInvoiceDataInfo userInvoiceDataInfo2 = MemberProcessor.GetUserInvoiceDataInfo(userInvoiceDataInfo.Id);
							if (userInvoiceDataInfo2 == null || userInvoiceDataInfo2.UserId != userId)
							{
								context.Response.Write(this.GetErrorJson("发票数据错误"));
								return;
							}
							if (userInvoiceDataInfo2.InvoiceTitle != userInvoiceDataInfo.InvoiceTitle && userInvoiceDataInfo.InvoiceType != InvoiceType.VATInvoice)
							{
								userInvoiceDataInfo.Id = 0;
							}
						}
						else
						{
							UserInvoiceDataInfo userInvoiceDataInfoByTitle = MemberProcessor.GetUserInvoiceDataInfoByTitle(userInvoiceDataInfo.InvoiceTitle);
							if (userInvoiceDataInfoByTitle != null)
							{
								userInvoiceDataInfo.Id = userInvoiceDataInfoByTitle.Id;
								if (userInvoiceDataInfo.InvoiceType == InvoiceType.Enterprise)
								{
									userInvoiceDataInfo.InvoiceType = userInvoiceDataInfoByTitle.InvoiceType;
								}
							}
						}
						string text2 = "";
						if (!this.ValidationInvoice(userInvoiceDataInfo, out text2))
						{
							context.Response.Write(this.GetErrorJson("发票验证错误"));
						}
						else
						{
							userInvoiceDataInfo.UserId = userId;
							int newInvoiceId = userInvoiceDataInfo.Id;
							if (userInvoiceDataInfo.Id > 0)
							{
								MemberProcessor.UpdateUserInvoiceDataInfo(userInvoiceDataInfo);
							}
							else
							{
								newInvoiceId = MemberProcessor.AddUserInvoiceDataInfo(userInvoiceDataInfo).ToInt(0);
							}
							IList<UserInvoiceDataInfo> userInvoiceDataList = MemberProcessor.GetUserInvoiceDataList(userId);
							string s = JsonConvert.SerializeObject(new
							{
								Status = "OK",
								NewInvoiceId = newInvoiceId,
								List = from i in userInvoiceDataList
								select new
								{
									Id = i.Id,
									InvoiceType = i.InvoiceType,
									InvoiceTitle = i.InvoiceTitle.ToNullString(),
									InvoiceTaxpayerNumber = i.InvoiceTaxpayerNumber.ToNullString(),
									OpenBank = i.OpenBank.ToNullString(),
									BankAccount = i.BankAccount.ToNullString(),
									ReceiveAddress = i.ReceiveAddress.ToNullString(),
									ReceiveEmail = i.ReceiveEmail.ToNullString(),
									ReceiveName = i.ReceiveName.ToNullString(),
									ReceivePhone = i.ReceivePhone.ToNullString(),
									ReceiveRegionId = i.ReceiveRegionId,
									ReceiveRegionName = i.ReceiveRegionName.ToNullString(),
									RegisterAddress = i.RegisterAddress.ToNullString(),
									RegisterTel = i.RegisterTel.ToNullString()
								}
							});
							context.Response.Write(s);
						}
					}
				}
			}
		}

		private bool ValidationInvoice(UserInvoiceDataInfo userInvoiceInfo, out string msg)
		{
			msg = "";
			ValidationResults validationResults = Validation.Validate(userInvoiceInfo, "ValInvoice");
			string text = string.Empty;
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult item in (IEnumerable<ValidationResult>)validationResults)
				{
					text += Formatter.FormatErrorMessage(item.Message);
				}
				msg = text;
			}
			bool flag = true;
			if ((userInvoiceInfo.InvoiceType == InvoiceType.Enterprise_Electronic || userInvoiceInfo.InvoiceType == InvoiceType.Personal_Electronic) && (string.IsNullOrEmpty(userInvoiceInfo.ReceiveEmail) || string.IsNullOrEmpty(userInvoiceInfo.ReceivePhone) || !DataHelper.IsEmail(userInvoiceInfo.ReceiveEmail) || !DataHelper.IsMobile(userInvoiceInfo.ReceivePhone)))
			{
				msg += "请输入正确的收票人邮箱和电话";
				flag = false;
			}
			if ((userInvoiceInfo.InvoiceType == InvoiceType.Enterprise_Electronic || userInvoiceInfo.InvoiceType == InvoiceType.Enterprise) && (string.IsNullOrEmpty(userInvoiceInfo.InvoiceTaxpayerNumber) || string.IsNullOrEmpty(userInvoiceInfo.InvoiceTitle)))
			{
				msg += "请输入发票抬头和纳税人识别号";
				flag = false;
			}
			if (userInvoiceInfo.InvoiceType == InvoiceType.VATInvoice && (!DataHelper.IsMobile(userInvoiceInfo.ReceivePhone) || string.IsNullOrEmpty(userInvoiceInfo.BankAccount) || string.IsNullOrEmpty(userInvoiceInfo.OpenBank) || string.IsNullOrEmpty(userInvoiceInfo.ReceiveAddress) || string.IsNullOrEmpty(userInvoiceInfo.ReceiveName) || string.IsNullOrEmpty(userInvoiceInfo.ReceiveRegionName) || string.IsNullOrEmpty(userInvoiceInfo.RegisterAddress) || string.IsNullOrEmpty(userInvoiceInfo.RegisterTel) || string.IsNullOrEmpty(userInvoiceInfo.InvoiceTaxpayerNumber) || string.IsNullOrEmpty(userInvoiceInfo.InvoiceTitle) || string.IsNullOrEmpty(userInvoiceInfo.ReceivePhone)))
			{
				msg += "请输入正确的专票信息";
				flag = false;
			}
			return validationResults.IsValid & flag;
		}

		private void DelUserInvoice(HttpContext context)
		{
			this.CheckOpenId();
			int userId = HiContext.Current.UserId;
			int id = context.Request["invoiceId"].ToInt(0);
			if (userId <= 0)
			{
				context.Response.Write(this.GetErrorJson("用户未登录"));
			}
			else
			{
				UserInvoiceDataInfo userInvoiceDataInfo = MemberProcessor.GetUserInvoiceDataInfo(id);
				if (userInvoiceDataInfo == null || userInvoiceDataInfo.UserId != userId)
				{
					context.Response.Write(this.GetErrorJson("错误的发票ID"));
				}
				else
				{
					MemberProcessor.DeleteUserInvoiceDataInfo(userId);
					IList<UserInvoiceDataInfo> userInvoiceDataList = MemberProcessor.GetUserInvoiceDataList(userId);
					string s = JsonConvert.SerializeObject(new
					{
						Status = "OK",
						List = from i in userInvoiceDataList
						select new
						{
							Id = i.Id,
							InvoiceType = i.InvoiceType,
							InvoiceTitle = i.InvoiceTitle.ToNullString(),
							InvoiceTaxpayerNumber = i.InvoiceTaxpayerNumber.ToNullString(),
							OpenBank = i.OpenBank.ToNullString(),
							BankAccount = i.BankAccount.ToNullString(),
							ReceiveAddress = i.ReceiveAddress.ToNullString(),
							ReceiveEmail = i.ReceiveEmail.ToNullString(),
							ReceiveName = i.ReceiveName.ToNullString(),
							ReceivePhone = i.ReceivePhone.ToNullString(),
							ReceiveRegionId = i.ReceiveRegionId,
							ReceiveRegionName = i.ReceiveRegionName.ToNullString(),
							RegisterAddress = i.RegisterAddress.ToNullString(),
							RegisterTel = i.RegisterTel.ToNullString()
						}
					});
					context.Response.Write(s);
				}
			}
		}

		private void GetUserPoints()
		{
			this.BindUserByOpenId();
			int num = this.context.Request["PageIndex"].ToInt(0);
			int num2 = this.context.Request["pagesize"].ToInt(0);
			if (num <= 0)
			{
				num = 1;
			}
			if (num2 <= 0)
			{
				num2 = 10;
			}
			PointQuery pointQuery = new PointQuery();
			pointQuery.PageIndex = num;
			pointQuery.PageSize = num2;
			PageModel<PointDetailInfo> userPoints = MemberHelper.GetUserPoints(pointQuery);
			string s = JsonConvert.SerializeObject(new
			{
				userpoint_get_response = new
				{
					Total = userPoints.Total,
					Points = HiContext.Current.User.Points,
					List = from p in userPoints.Models
					select new
					{
						JournalNumber = p.JournalNumber,
						Increased = p.Increased,
						OrderId = p.OrderId,
						Points = p.Points,
						Reduced = p.Reduced,
						Remark = p.Remark,
						SignInSource = p.SignInSource,
						TradeDate = p.TradeDate.ToString("yyyy-MM-dd HH:mm:ss"),
						TradeTypeName = p.TradeTypeName
					}
				}
			});
			this.context.Response.Write(s);
			this.context.Response.End();
		}

		private void AfterSalePreCheck(HttpContext context)
		{
			this.CheckOpenId();
			bool flag = context.Request["IsReturn"].ToBool();
			string text = Globals.StripAllTags(context.Request["OrderId"].ToNullString());
			string text2 = Globals.StripAllTags(context.Request["SkuId"].ToNullString());
			int maxRefundQuantity = 0;
			if (string.IsNullOrEmpty(text))
			{
				context.Response.Write(this.GetErrorJson("参数错误"));
				context.Response.End();
			}
			OrderInfo orderInfo = OrderHelper.GetOrderInfo(text);
			if (orderInfo == null)
			{
				context.Response.Write(this.GetErrorJson("错误的订单ID"));
				context.Response.End();
			}
			if (orderInfo.LineItems == null || orderInfo.LineItems.Count == 0)
			{
				context.Response.Write(this.GetErrorJson("礼品不支持" + (flag ? "退货" : "退款")));
				context.Response.End();
			}
			if (flag)
			{
				if (!string.IsNullOrEmpty(text2))
				{
					if (!orderInfo.LineItems.ContainsKey(text2))
					{
						context.Response.Write(this.GetErrorJson("错误的商品信息"));
						context.Response.End();
					}
				}
				else
				{
					context.Response.Write(this.GetErrorJson("错误的退货商品"));
					context.Response.End();
				}
				maxRefundQuantity = TradeHelper.GetMaxQuantity(orderInfo, text2);
			}
			if (flag)
			{
				if (!TradeHelper.CanReturn(orderInfo, text2))
				{
					context.Response.Write(this.GetErrorJson("订单状态错误"));
					context.Response.End();
				}
			}
			else if (!TradeHelper.CanRefund(orderInfo, text2))
			{
				context.Response.Write(this.GetErrorJson("订单状态错误"));
				context.Response.End();
			}
			bool canBackReturn = TradeHelper.GatewayIsCanBackReturn(orderInfo.Gateway) && orderInfo.BalanceAmount <= decimal.Zero;
			bool canReturnOnStore = orderInfo.Gateway.ToLower() == "hishop.plugins.payment.cashreceipts" && orderInfo.StoreId > 0 && orderInfo.ShippingModeId == -2;
			GroupBuyInfo groupbuy = null;
			if (orderInfo.GroupBuyId > 0)
			{
				groupbuy = ProductBrowser.GetGroupBuy(orderInfo.GroupBuyId);
			}
			bool canToBalance = false;
			MemberInfo user = Users.GetUser(orderInfo.UserId);
			if (user != null && user.IsOpenBalance && !string.IsNullOrEmpty(user.TradePassword))
			{
				canToBalance = true;
			}
			string s = JsonConvert.SerializeObject(new
			{
				Status = "OK",
				CanBackReturn = canBackReturn,
				CanToBalance = canToBalance,
				CanReturnOnStore = canReturnOnStore,
				MaxRefundAmount = orderInfo.GetCanRefundAmount(text2, groupbuy, 0),
				MaxRefundQuantity = maxRefundQuantity,
				oneReundAmount = orderInfo.GetCanRefundAmount(text2, groupbuy, 1)
			});
			context.Response.Write(s);
		}

		private void GetOpenId(HttpContext context)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			string text = masterSettings.WxAppletAppId;
			if (string.IsNullOrEmpty(text))
			{
				text = context.Request["appid"].ToNullString();
			}
			string text2 = masterSettings.WxAppletAppSecrect;
			if (string.IsNullOrEmpty(text2))
			{
				text2 = context.Request["secret"].ToNullString();
			}
			string text3 = context.Request["js_code"].ToNullString();
			string text4 = "https://api.weixin.qq.com/sns/jscode2session?appid=" + text + "&secret=" + text2 + "&js_code=" + text3 + "&grant_type=authorization_code";
			string text5 = "";
			try
			{
				text5 = Globals.GetResponseResult(text4);
				context.Response.Write(text5);
				context.Response.End();
			}
			catch (Exception ex)
			{
				IDictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("requestUrl", text4);
				dictionary.Add("result", text5);
				Globals.WriteExceptionLog(ex, dictionary, "GetOpenId");
			}
		}

		private string GetErrorJosn(int errorCode, string errorMsg)
		{
			return JsonConvert.SerializeObject(new
			{
				ErrorResponse = new
				{
					ErrorCode = errorCode,
					ErrorMsg = errorMsg
				}
			});
		}

		private void GetAllCategories(HttpContext context)
		{
			IEnumerable<CategoryInfo> mainCategories = CatalogHelper.GetMainCategories();
			if (mainCategories == null)
			{
				context.Response.Write(this.GetErrorJson("未找到分类信息"));
			}
			else
			{
				var data = (from c in mainCategories
				select new
				{
					cid = c.CategoryId,
					name = c.Name,
					subs = from a in CatalogHelper.GetSubCategories(c.CategoryId)
					select new
					{
						cid = a.CategoryId,
						name = a.Name
					}
				}).ToList();
				string s = JsonConvert.SerializeObject(new
				{
					Status = "OK",
					Data = data
				});
				context.Response.Write(s);
			}
		}

		private void GetProductSkus(HttpContext context)
		{
			int num = context.Request["ProductId"].ToInt(0);
			this.BindUserByOpenId();
			if (num <= 0)
			{
				context.Response.Write(this.GetErrorJson("商品已下架"));
				context.Response.End();
			}
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			int gradeId = 0;
			if (HiContext.Current != null && HiContext.Current.User != null && HiContext.Current.User.UserId > 0)
			{
				gradeId = HiContext.Current.User.GradeId;
			}
			ProductInfo productSimpleInfo = ProductBrowser.GetProductSimpleInfo(num);
			SKUItem sKUItem = new SKUItem();
			ProductModel productSkus = ProductBrowser.GetProductSkus(num, gradeId, false, 0);
			if (productSkus == null || productSkus.Skus == null || productSkus.Skus.Count == 0 || productSkus.SaleStatus != ProductSaleStatus.OnSale)
			{
				context.Response.Write(this.GetErrorJson("商品已下架"));
				context.Response.End();
			}
			else
			{
				sKUItem = productSkus.Skus.FirstOrDefault();
			}
			IList<SKUItem> list = new List<SKUItem>();
			IList<SkuItem> list2 = new List<SkuItem>();
			if (productSkus.SkuTable != null && productSkus.SkuTable.Rows.Count > 0)
			{
				foreach (DataRow row in productSkus.SkuTable.Rows)
				{
					if ((from c in list2
					where c.AttributeName == row["AttributeName"].ToNullString()
					select c).Count() == 0)
					{
						SkuItem skuItem = new SkuItem();
						skuItem.AttributeName = row["AttributeName"].ToNullString();
						skuItem.AttributeId = row["AttributeId"].ToNullString();
						skuItem.AttributeValue = new List<AttributeValue>();
						IList<string> list3 = new List<string>();
						foreach (DataRow row2 in productSkus.SkuTable.Rows)
						{
							if (string.Compare((string)row["AttributeName"], (string)row2["AttributeName"]) == 0 && !list3.Contains((string)row2["ValueStr"]))
							{
								AttributeValue attributeValue = new AttributeValue();
								list3.Add((string)row2["ValueStr"]);
								attributeValue.ValueId = row2["ValueId"].ToNullString();
								attributeValue.UseAttributeImage = row2["UseAttributeImage"].ToNullString();
								attributeValue.Value = row2["ValueStr"].ToNullString();
								attributeValue.ImageUrl = Globals.HttpsFullPath(row2["ImageUrl"].ToNullString());
								skuItem.AttributeValue.Add(attributeValue);
							}
						}
						list2.Add(skuItem);
					}
				}
			}
			if (productSkus.Skus == null || productSkus.Skus.Count == 0)
			{
				productSkus.Skus = new List<SKUItem>();
			}
			foreach (SKUItem sku in productSkus.Skus)
			{
				sku.SalePrice = decimal.Parse(sku.SalePrice.F2ToString("f2"));
				sku.CostPrice = decimal.Parse(sku.CostPrice.F2ToString("f2"));
				list.Add(sku);
			}
			if (productSkus.Skus.Sum((SKUItem s) => s.Stock) <= 0)
			{
				context.Response.Write(this.GetErrorJson("商品已售罄"));
				context.Response.End();
			}
			else
			{
				ShoppingCartInfo CartInfo = ShoppingCartProcessor.GetMobileShoppingCart(null, true, true, -1);
				this.FilterShoppingCart(CartInfo);
				string s2 = JsonConvert.SerializeObject(new
				{
					Status = "OK",
					Data = new
					{
						ProductId = productSimpleInfo.ProductId,
						ProductName = productSimpleInfo.ProductName,
						ImageUrl = this.GetImageFullPath(productSkus.SubmitOrderImg),
						Stock = list.Sum((SKUItem s) => s.Stock),
						SkuItems = from s in list2
						select new
						{
							AttributeId = s.AttributeId,
							AttributeName = s.AttributeName,
							AttributeValue = from av in s.AttributeValue
							select new
							{
								ValueId = av.ValueId,
								Value = av.Value,
								UseAttributeImage = av.UseAttributeImage,
								ImageUrl = this.GetImageFullPath(av.ImageUrl, true)
							}
						},
						Skus = from s in list
						select new
						{
							SkuId = s.SkuId,
							SKU = s.SKU,
							Weight = s.Weight,
							Stock = s.Stock,
							SalePrice = s.SalePrice,
							CartQuantity = this.GetCartProductQuantity(CartInfo, 0, s.SkuId),
							ImageUrl = this.GetImageFullPath(s.ImageUrl, true)
						},
						DefaultSku = new
						{
							SkuId = sKUItem.SkuId,
							SKU = sKUItem.SKU,
							Weight = sKUItem.Weight,
							Stock = sKUItem.Stock,
							SalePrice = sKUItem.SalePrice,
							CartQuantity = this.GetCartProductQuantity(CartInfo, 0, sKUItem.SkuId),
							ImageUrl = this.GetImageFullPath(sKUItem.ImageUrl, true)
						}
					}
				});
				context.Response.Write(s2);
				context.Response.End();
			}
		}

		private string GetImageFullPath(string imageUrl, bool allowEmpty = false)
		{
			if (string.IsNullOrEmpty(imageUrl))
			{
				if (allowEmpty)
				{
					return "";
				}
				return Globals.HttpsFullPath(HiContext.Current.SiteSettings.DefaultProductImage);
			}
			if (imageUrl.StartsWith("http://"))
			{
				return imageUrl;
			}
			return Globals.HttpsFullPath(imageUrl);
		}

		public void GetCountDownList(HttpContext context)
		{
			int num = context.Request["pageIndex"].ToInt(0);
			int num2 = context.Request["pageSize"].ToInt(0);
			if (num < 1)
			{
				num = 1;
			}
			if (num2 < 1)
			{
				num2 = 3;
			}
			ProductBrowseQuery productBrowseQuery = new ProductBrowseQuery();
			productBrowseQuery.IsCount = true;
			productBrowseQuery.PageIndex = num;
			productBrowseQuery.PageSize = num2;
			productBrowseQuery.SortBy = "CountDownId";
			productBrowseQuery.SortOrder = SortAction.Desc;
			productBrowseQuery.StoreId = 0;
			DbQueryResult countDownProductList = PromoteHelper.GetCountDownProductList(productBrowseQuery);
			string s = JsonConvert.SerializeObject(new
			{
				Status = "OK",
				Data = from d in countDownProductList.Data.AsEnumerable()
				select new
				{
					CountDownId = d.Field<int>("CountDownId"),
					ProductId = d.Field<int>("ProductId"),
					ProductName = d.Field<string>("ProductName"),
					SalePrice = d.Field<decimal>("SalePrice").F2ToString("f2"),
					CountDownPrice = d.Field<decimal>("CountDownPrice").F2ToString("f2"),
					CountDownType = ((DateTime.Now < d.Field<DateTime>("StartDate")) ? 1 : 2),
					ThumbnailUrl160 = this.GetProductImageFullPath(d.Field<string>("ThumbnailUrl410"))
				}
			});
			context.Response.Write(s);
		}

		public void GetServicePhone(HttpContext context)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			string s = JsonConvert.SerializeObject(new
			{
				Status = "OK",
				Data = new
				{
					masterSettings.ServicePhone
				}
			});
			context.Response.Write(s);
		}

		private void GetAllAfterSaleList(HttpContext context)
		{
			this.CheckOpenId();
			int num = context.Request["pageIndex"].ToInt(0);
			if (num <= 0)
			{
				num = 1;
			}
			int num2 = context.Request["pageSize"].ToInt(0);
			if (num2 < 1)
			{
				num2 = 10;
			}
			AfterSalesQuery afterSalesQuery = new AfterSalesQuery();
			afterSalesQuery.PageIndex = num;
			afterSalesQuery.PageSize = num2;
			afterSalesQuery.ProductType = ProductType.PhysicalProduct;
			PageModel<AfterSaleRecordModel> userAfterOrders = MemberProcessor.GetUserAfterOrders(HiContext.Current.UserId, afterSalesQuery);
			List<OrderInfo> list = new List<OrderInfo>();
			IList<AfterSaleRecordModel> list2 = userAfterOrders.Models.ToList();
			for (int j = 0; j < list2.Count; j++)
			{
				AfterSaleRecordModel afterSaleRecordModel = list2[j];
				list2[j].ProductItems = TradeHelper.GetOrderItems(afterSaleRecordModel.OrderId, afterSaleRecordModel.SkuId);
			}
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			string s = JsonConvert.SerializeObject(new
			{
				Status = "OK",
				RecordCount = userAfterOrders.Total,
				Data = from c in list2
				select new
				{
					OrderId = c.OrderId,
					Status = c.HandleStatus,
					StatusText = c.StatusText,
					AdminRemark = c.AdminRemark,
					AfterSaleId = c.AfterSaleId,
					AfterSaleType = c.AfterSaleType,
					ApplyForTime = c.ApplyForTime,
					ExpressCompanyAbb = c.ExpressCompanyAbb,
					ExpressCompanyName = c.ExpressCompanyName,
					RefundAmount = c.RefundAmount,
					RefundType = (int)c.RefundType,
					RefundTypeText = EnumDescription.GetEnumDescription((Enum)(object)c.RefundType, 0),
					ShipOrderNumber = c.ShipOrderNumber,
					SkuId = c.SkuId,
					OrderTotal = c.TradeTotal,
					QuantityTotal = c.ProductItems.Sum((LineItemInfo i) => i.Quantity),
					UserExpressCompanyAbb = c.UserExpressCompanyAbb,
					UserExpressCompanyName = c.UserExpressCompanyName,
					UserRemark = c.UserRemark,
					UserShipOrderNumber = c.UserShipOrderNumber,
					IsRefund = (c.AfterSaleType == AfterSaleTypes.OrderRefund),
					IsReturn = (c.AfterSaleType == AfterSaleTypes.ReturnAndRefund || c.AfterSaleType == AfterSaleTypes.OnlyRefund),
					IsReplace = (c.AfterSaleType == AfterSaleTypes.Replace),
					IsWaitToDeal = (c.HandleStatus == 0 && ((c.IsStoreCollect && c.AfterSaleType == AfterSaleTypes.OrderRefund) || (c.AfterSaleType == AfterSaleTypes.OnlyRefund && c.IsStoreCollect) || c.AfterSaleType == AfterSaleTypes.Replace || c.AfterSaleType == AfterSaleTypes.ReturnAndRefund)),
					IsWaitFinishReturn = (c.AfterSaleType == AfterSaleTypes.ReturnAndRefund && c.HandleStatus == 4),
					IsShowReturnLogistics = (c.AfterSaleType == AfterSaleTypes.ReturnAndRefund && c.HandleStatus == 4),
					IsWaitGetGoodsOfReplace = (c.AfterSaleType == AfterSaleTypes.Replace && c.HandleStatus == 4),
					IsWaitConfirmReplace = (c.AfterSaleType == AfterSaleTypes.Replace && c.HandleStatus == 0),
					LineItems = from d in c.ProductItems
					select new
					{
						Status = d.Status,
						StatusText = d.StatusText,
						SkuId = d.SkuId,
						Name = d.ItemDescription,
						Price = d.ItemAdjustedPrice.F2ToString("f2"),
						Amount = (d.ItemAdjustedPrice * (decimal)d.Quantity).F2ToString("f2"),
						Quantity = d.ShipmentQuantity,
						Image = this.GetImageFullPath(d.ThumbnailsUrl),
						SkuText = this.newSKUContent(d.SKUContent),
						ProductId = d.ProductId
					}
				}
			});
			context.Response.Write(s);
		}

		private string newSKUContent(string SKUContent)
		{
			string text = "";
			if (!string.IsNullOrEmpty(SKUContent))
			{
				string[] array = SKUContent.Split(';');
				for (int i = 0; i < array.Length; i++)
				{
					if (!string.IsNullOrEmpty(array[i]))
					{
						string[] array2 = array[i].Replace(":", "：").Split('：');
						if (array2.Length > 1)
						{
							text = text + "\"" + array2[1] + "\"  ";
						}
					}
				}
			}
			return text;
		}

		public void ApplyRefund(HttpContext context)
		{
			this.CheckOpenId();
			MemberInfo user = HiContext.Current.User;
			string text = (context.Request["orderId"] == null) ? "" : Globals.StripAllTags(context.Request["orderId"]);
			string skuId = (context.Request["skuId"] == null) ? "" : Globals.StripAllTags(context.Request["skuId"]);
			string text2 = DataHelper.CleanSearchString(Globals.UrlDecode(context.Request["formId"]));
			int num = 0;
			int.TryParse(context.Request["RefundType"], out num);
			if (string.IsNullOrEmpty(text))
			{
				context.Response.Write(this.GetErrorJson("参数错误"));
			}
			else
			{
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(text);
				if (orderInfo == null)
				{
					context.Response.Write(this.GetErrorJson("错误的订单信息"));
				}
				else
				{
					if (orderInfo.FightGroupId > 0)
					{
						FightGroupInfo fightGroup = VShopHelper.GetFightGroup(orderInfo.FightGroupId);
						if (fightGroup != null && fightGroup.Status == FightGroupStatus.FightGroupIn)
						{
							context.Response.Write(this.GetErrorJson("拼团过程中时，已完成支付的订单不能发起退款；"));
							return;
						}
					}
					if (TradeHelper.IsOnlyOneSku(orderInfo))
					{
						skuId = "";
					}
					if (!TradeHelper.CanRefund(orderInfo, skuId))
					{
						context.Response.Write(this.GetErrorJson("已有待确认的订单或者订单商品的退款/退货/换货申请"));
					}
					else if (!Enum.IsDefined(typeof(RefundTypes), num))
					{
						context.Response.Write(this.GetErrorJson("请选择退款方式"));
					}
					else
					{
						string enumDescription = EnumDescription.GetEnumDescription((Enum)(object)EnumPaymentType.AdvancePay, 1);
						if ((orderInfo.Gateway.ToLower() == enumDescription || orderInfo.DepositGatewayOrderId.ToNullString().ToLower() == enumDescription) && num != 1)
						{
							context.Response.Write(this.GetErrorJson("预付款支付的订单只能退回到预付款帐号"));
						}
						else
						{
							string text3 = Globals.StripAllTags(context.Request["BankName"].ToNullString());
							string text4 = Globals.StripAllTags(context.Request["BankAccountName"].ToNullString());
							string text5 = Globals.StripAllTags(context.Request["BankAccountNo"].ToNullString());
							string userRemark = (context.Request["Remark"] == null) ? "" : Globals.StripAllTags(context.Request["Remark"]);
							if (num == 2 && (string.IsNullOrEmpty(text3) || string.IsNullOrEmpty(text4) || string.IsNullOrEmpty(text5)))
							{
								context.Response.Write(this.GetErrorJson("银行卡信息为空"));
							}
							else if (user != null && num == 1 && !user.IsOpenBalance)
							{
								context.Response.Write(this.GetErrorJson("未开通余额帐号"));
							}
							else
							{
								string text6 = (context.Request["RefundReason"] == null) ? "" : Globals.StripAllTags(context.Request["RefundReason"]);
								if (string.IsNullOrEmpty(text6))
								{
									context.Response.Write(this.GetErrorJson("请选择退款原因"));
								}
								else
								{
									string refundGateWay = string.IsNullOrEmpty(orderInfo.Gateway) ? "" : orderInfo.Gateway.ToLower().Replace(".payment.", ".refund.");
									int num2 = 0;
									num2 = orderInfo.GetAllQuantity(true);
									GroupBuyInfo groupbuy = null;
									if (orderInfo.GroupBuyId > 0)
									{
										groupbuy = TradeHelper.GetGroupBuy(orderInfo.GroupBuyId);
									}
									decimal canRefundAmount = orderInfo.GetCanRefundAmount("", groupbuy, 0);
									string generateId = Globals.GetGenerateId();
									Hidistro.Entities.Orders.RefundInfo refund = new Hidistro.Entities.Orders.RefundInfo
									{
										UserRemark = userRemark,
										RefundReason = text6,
										RefundType = (RefundTypes)num,
										RefundGateWay = refundGateWay,
										RefundOrderId = generateId,
										RefundAmount = canRefundAmount,
										StoreId = orderInfo.StoreId,
										ApplyForTime = DateTime.Now,
										BankName = text3,
										BankAccountName = text4,
										BankAccountNo = text5,
										OrderId = orderInfo.OrderId,
										HandleStatus = RefundStatus.Applied
									};
									if (TradeHelper.ApplyForRefund(refund))
									{
										Hidistro.Entities.Orders.RefundInfo refundInfo = TradeHelper.GetRefundInfo(orderInfo.OrderId);
										if (orderInfo.StoreId > 0)
										{
											VShopHelper.AppPsuhRecordForStore(orderInfo.StoreId, orderInfo.OrderId, "", EnumPushStoreAction.StoreOrderRefundApply);
										}
										if (!string.IsNullOrEmpty(text2))
										{
											WeChartAppletHelper.AddFormData(WXAppletEvent.ApplyRefund, refundInfo.RefundId.ToString(), text2);
										}
										context.Response.Write(this.GetOKJson("申请退款成功"));
									}
									else
									{
										context.Response.Write(this.GetErrorJson("申请退款失败"));
									}
								}
							}
						}
					}
				}
			}
		}

		private void UploadAppletImage(HttpContext context)
		{
			this.CheckOpenId();
			IList<string> list = new List<string>();
			HttpFileCollection files = context.Request.Files;
			try
			{
				if (files != null)
				{
					for (int i = 0; i < files.Count; i++)
					{
						HttpPostedFile httpPostedFile = files[i];
						if (ResourcesHelper.CheckPostedFile(httpPostedFile, "image", null))
						{
							string str = ResourcesHelper.GenerateFilename(Path.GetExtension(httpPostedFile.FileName));
							string text = HiContext.Current.GetStoragePath() + "/applet/" + str;
							string filename = HiContext.Current.Context.Request.MapPath(text);
							if (httpPostedFile.ContentLength > 2097152)
							{
								Bitmap bitmap = new Bitmap(100, 100);
								bitmap = (Bitmap)Image.FromStream(httpPostedFile.InputStream);
								bitmap = ResourcesHelper.GetThumbnail(bitmap, 735, 480);
								bitmap.Save(filename);
							}
							else
							{
								httpPostedFile.SaveAs(filename);
							}
							list.Add(text);
						}
					}
				}
			}
			catch (Exception ex)
			{
				IDictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("count", dictionary.Count.ToString());
				Globals.WriteExceptionLog(ex, dictionary, "UploadAppletImage");
				list.Add("upload error");
			}
			string s = JsonConvert.SerializeObject(new
			{
				Status = "OK",
				Count = list.Count,
				Data = from c in list
				select new
				{
					ImageUrl = c
				}
			});
			context.Response.Write(s);
		}

		public void ApplyReturn(HttpContext context)
		{
			this.CheckOpenId();
			MemberInfo user = HiContext.Current.User;
			string orderId = Globals.StripAllTags(context.Request["orderId"].ToNullString());
			string text = Globals.StripAllTags(context.Request["skuId"].ToNullString());
			string text2 = DataHelper.CleanSearchString(Globals.UrlDecode(context.Request["formId"]));
			int num = 0;
			int num2 = 0;
			num2 = context.Request["RefundType"].ToInt(0);
			num = context.Request["Quantity"].ToInt(0);
			string userRemark = Globals.StripAllTags(context.Request["Remark"].ToNullString());
			string text3 = Globals.StripAllTags(context.Request["UserCredentials"].ToNullString());
			string text4 = text3.Trim();
			string imageServerUrl = Globals.GetImageServerUrl();
			if (text4.Length > 0)
			{
				string[] array = text4.Split(',');
				text3 = "";
				string text5 = "";
				for (int i = 0; i < array.Length; i++)
				{
					text5 = array[i].ToLower();
					text5 = text5.Replace("http://", "https://").Replace(Globals.HttpsHostPath(HttpContext.Current.Request.Url), "");
					text3 += (string.IsNullOrEmpty(imageServerUrl) ? (Globals.SaveFile("user\\Credentials", text5, "/Storage/master/", true, false, "") + "|") : (text5 + "|"));
				}
				text3 = text3.TrimEnd('|');
			}
			string text6 = Globals.StripAllTags(context.Request["RefundReason"].ToNullString());
			int num3 = context.Request["afterSaleType"].ToInt(0);
			if (!Enum.IsDefined(typeof(AfterSaleTypes), num3))
			{
				context.Response.Write(this.GetErrorJson("错误的售后类型"));
			}
			else if (!Enum.IsDefined(typeof(RefundTypes), num2))
			{
				context.Response.Write(this.GetErrorJson("请选择退款方式"));
			}
			else
			{
				string text7 = Globals.StripAllTags(context.Request["BankName"].ToNullString());
				string text8 = Globals.StripAllTags(context.Request["BankAccountName"].ToNullString());
				string text9 = Globals.StripAllTags(context.Request["BankAccountNo"].ToNullString());
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(orderId);
				if (orderInfo == null)
				{
					context.Response.Write(this.GetErrorJson("错误的订单信息"));
				}
				else if (string.IsNullOrEmpty(text) || !orderInfo.LineItems.ContainsKey(text))
				{
					context.Response.Write(this.GetErrorJson("请选择售后商品"));
				}
				else if (!TradeHelper.CanReturn(orderInfo, text))
				{
					context.Response.Write(this.GetErrorJson("该商品正在售后"));
				}
				else if (!Enum.IsDefined(typeof(RefundTypes), num2))
				{
					context.Response.Write(this.GetErrorJson("请选择退款方式"));
				}
				else
				{
					string enumDescription = EnumDescription.GetEnumDescription((Enum)(object)EnumPaymentType.AdvancePay, 1);
					if ((orderInfo.Gateway.ToLower() == enumDescription || orderInfo.DepositGatewayOrderId.ToNullString().ToLower() == enumDescription) && num2 != 1)
					{
						context.Response.Write(this.GetErrorJson("预付款支付的订单只能退回到预付款帐号"));
					}
					else if (num2 == 2 && (string.IsNullOrEmpty(text7) || string.IsNullOrEmpty(text8) || string.IsNullOrEmpty(text9)))
					{
						context.Response.Write(this.GetErrorJson("银行卡信息为空"));
					}
					else if (user != null && num2 == 1 && !user.IsOpenBalance)
					{
						context.Response.Write(this.GetErrorJson("未开通余额帐号"));
					}
					else if (string.IsNullOrEmpty(text6))
					{
						context.Response.Write(this.GetErrorJson("请选择退款原因"));
					}
					else
					{
						string refundGateWay = string.IsNullOrEmpty(orderInfo.Gateway) ? "" : orderInfo.Gateway.ToLower().Replace(".payment.", ".refund.");
						LineItemInfo lineItemInfo = null;
						string orderId2 = orderInfo.OrderId;
						lineItemInfo = orderInfo.LineItems[text];
						if (num3 == 3)
						{
							num = lineItemInfo.ShipmentQuantity;
						}
						else if (num == 0)
						{
							num = TradeHelper.GetMaxQuantity(orderInfo, text);
						}
						else if (num > lineItemInfo.ShipmentQuantity)
						{
							context.Response.Write(this.GetErrorJson("退货数量错误"));
							return;
						}
						decimal num4 = default(decimal);
						decimal.TryParse(context.Request["RefundAmount"], out num4);
						decimal canRefundAmount = orderInfo.GetCanRefundAmount(text, null, num);
						if (num3 == 3 && canRefundAmount <= decimal.Zero)
						{
							context.Response.Write(this.GetErrorJson("订单支付金额为0时不能进行仅退款操作"));
						}
						else if (num4 < decimal.Zero && num3 != 2)
						{
							context.Response.Write(this.GetErrorJson("退款金额错误"));
						}
						else if (num4 > canRefundAmount)
						{
							context.Response.Write(this.GetErrorJson(string.Format("退款金额不能大于最大可退款金额({0})", canRefundAmount.F2ToString("f2"))));
						}
						else
						{
							if (lineItemInfo != null)
							{
								orderId2 = lineItemInfo.ItemDescription + lineItemInfo.SKUContent;
							}
							string generateId = Globals.GetGenerateId();
							ReturnInfo returnInfo = new ReturnInfo
							{
								UserRemark = userRemark,
								ReturnReason = text6,
								RefundType = (RefundTypes)num2,
								RefundGateWay = refundGateWay,
								RefundOrderId = generateId,
								RefundAmount = num4,
								StoreId = orderInfo.StoreId,
								ApplyForTime = DateTime.Now,
								BankName = text7,
								BankAccountName = text8,
								BankAccountNo = text9,
								HandleStatus = ReturnStatus.Applied,
								OrderId = orderInfo.OrderId,
								SkuId = text,
								Quantity = num,
								UserCredentials = text3,
								AfterSaleType = (AfterSaleTypes)num3
							};
							string str = (returnInfo.AfterSaleType == AfterSaleTypes.OnlyRefund) ? "退款" : "退货";
							if (TradeHelper.ApplyForReturn(returnInfo))
							{
								ReturnInfo returnInfo2 = TradeHelper.GetReturnInfo(orderInfo.OrderId, text);
								if (orderInfo.StoreId > 0)
								{
									VShopHelper.AppPsuhRecordForStore(orderInfo.StoreId, orderInfo.OrderId, "", EnumPushStoreAction.StoreOrderReturnApply);
								}
								if (!string.IsNullOrEmpty(text2))
								{
									WeChartAppletHelper.AddFormData(WXAppletEvent.ApplyAfterSale, returnInfo2.ReturnId.ToString(), text2);
								}
								context.Response.Write(this.GetOKJson("申请" + str + "成功"));
							}
							else
							{
								context.Response.Write(this.GetErrorJson("申请" + str + "失败"));
							}
						}
					}
				}
			}
		}

		private void GetExpressList(HttpContext context)
		{
			IList<ExpressCompanyInfo> allExpress = ExpressHelper.GetAllExpress(false);
			string s = JsonConvert.SerializeObject(new
			{
				Status = "OK",
				Data = from c in allExpress
				select new
				{
					ExpressName = c.Name,
					Kuaidi100Code = c.Kuaidi100Code,
					TaobaoCode = c.TaobaoCode
				}
			});
			context.Response.Write(s);
			context.Response.End();
		}

		public void ReturnSendGoods(HttpContext context)
		{
			this.CheckOpenId();
			int num = 0;
			int.TryParse(context.Request["ReturnsId"], out num);
			string skuId = (context.Request["SkuId"] == null) ? "" : Globals.StripAllTags(context.Request["SkuId"]);
			string orderId = (context.Request["OrderId"] == null) ? "" : Globals.StripAllTags(context.Request["OrderId"]);
			string text = DataHelper.CleanSearchString(Globals.UrlDecode(context.Request["formId"]));
			ReturnInfo returnInfo = null;
			returnInfo = ((num <= 0) ? TradeHelper.GetReturnInfo(orderId, skuId) : TradeHelper.GetReturnInfo(num));
			if (returnInfo == null)
			{
				context.Response.Write(this.GetErrorJson("错误的退货信息"));
			}
			else
			{
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(returnInfo.OrderId);
				if (orderInfo == null)
				{
					context.Response.Write(this.GetErrorJson("错误的订单信息"));
				}
				else if (orderInfo.LineItems.ContainsKey(returnInfo.SkuId))
				{
					if (orderInfo.LineItems[returnInfo.SkuId].Status != LineItemStatus.MerchantsAgreedForReturn && orderInfo.LineItems[returnInfo.SkuId].Status != LineItemStatus.DeliveryForReturn)
					{
						context.Response.Write(this.GetErrorJson("商品状态错误"));
					}
					else
					{
						string text2 = (context.Request["express"] == null) ? "" : Globals.StripAllTags(context.Request["express"]);
						string text3 = (context.Request["shipOrderNumber"] == null) ? "" : Globals.StripAllTags(context.Request["shipOrderNumber"]);
						if (string.IsNullOrEmpty(text2))
						{
							context.Response.Write(this.GetErrorJson("请选择快递公司"));
						}
						else
						{
							string text4 = "";
							string text5 = "";
							ExpressCompanyInfo expressCompanyInfo = ExpressHelper.FindNode(text2);
							if (expressCompanyInfo != null)
							{
								text4 = expressCompanyInfo.Kuaidi100Code;
								text5 = expressCompanyInfo.Name;
								if (text3.Trim() == "" || text3.Length > 20)
								{
									context.Response.Write(this.GetErrorJson("请输入快递编号"));
								}
								else if (TradeHelper.UserSendGoodsForReturn(returnInfo.ReturnId, text4, text5, text3, orderInfo.OrderId, returnInfo.SkuId))
								{
									if (text4.ToUpper() == "HTKY")
									{
										ExpressHelper.GetDataByKuaidi100(text4, text3);
									}
									if (!string.IsNullOrEmpty(text))
									{
										WeChartAppletHelper.AddFormData(WXAppletEvent.ReturnSendGoods, returnInfo.ReturnId.ToString(), text);
									}
									context.Response.Write(this.GetOKJson("发货成功"));
								}
								else
								{
									context.Response.Write(this.GetErrorJson("发货失败"));
								}
							}
							else
							{
								context.Response.Write(this.GetErrorJson("请选择快递公司"));
							}
						}
					}
				}
				else
				{
					context.Response.Write(this.GetErrorJson("错误的商品信息"));
				}
			}
		}

		private void GetRefundDetail(HttpContext context)
		{
			this.CheckOpenId();
			int refundId = context.Request["RefundId"].ToInt(0);
			Hidistro.Entities.Orders.RefundInfo refundInfo = TradeHelper.GetRefundInfo(refundId);
			if (refundInfo == null)
			{
				context.Response.Write(this.GetErrorJson("错误的售后记录"));
			}
			else
			{
				OrderInfo orderInfo = OrderHelper.GetOrderInfo(refundInfo.OrderId);
				if (orderInfo == null)
				{
					context.Response.Write(this.GetErrorJson("错误的售后记录"));
				}
				else
				{
					string text = "";
					if (text.StartsWith(refundInfo.OrderId) && orderInfo.LineItems.Count > 0)
					{
						using (Dictionary<string, LineItemInfo>.ValueCollection.Enumerator enumerator = orderInfo.LineItems.Values.GetEnumerator())
						{
							if (enumerator.MoveNext())
							{
								LineItemInfo current = enumerator.Current;
								text = current.ItemDescription;
							}
						}
					}
					string s = JsonConvert.SerializeObject(new
					{
						Status = "OK",
						Data = new
						{
							AdminRemark = refundInfo.AdminRemark,
							ApplyForTime = refundInfo.ApplyForTime,
							Remark = refundInfo.UserRemark,
							Status = (int)refundInfo.HandleStatus,
							StatusText = EnumDescription.GetEnumDescription((Enum)(object)refundInfo.HandleStatus, 0),
							DealTime = refundInfo.AgreedOrRefusedTime,
							Operator = refundInfo.Operator,
							Reason = refundInfo.RefundReason,
							RefundId = refundInfo.RefundId,
							OrderId = refundInfo.OrderId,
							Quantity = 0,
							RefundMoney = refundInfo.RefundAmount.F2ToString("f2"),
							RefundType = EnumDescription.GetEnumDescription((Enum)(object)refundInfo.RefundType, 0),
							ProductName = text,
							BankAccountName = refundInfo.BankAccountName,
							BankAccountNo = refundInfo.BankAccountNo,
							BankName = refundInfo.BankName,
							OrderTotal = orderInfo.GetTotal(false).F2ToString("f2"),
							ProductInfo = from l in orderInfo.LineItems.Values
							select new
							{
								ProductId = l.ProductId,
								ProductName = l.ItemDescription,
								SKU = l.SKU,
								SKUContent = this.newSKUContent(l.SKUContent),
								Price = l.ItemAdjustedPrice.F2ToString("f2"),
								Quantity = l.ShipmentQuantity,
								ThumbnailsUrl = this.GetImageFullPath(l.ThumbnailsUrl)
							}
						}
					});
					context.Response.Write(s);
					context.Response.End();
				}
			}
		}

		private void GetReturnDetail(HttpContext context)
		{
			this.CheckOpenId();
			int returnId = context.Request["returnId"].ToInt(0);
			ReturnInfo returns = TradeHelper.GetReturnInfo(returnId);
			if (returns == null)
			{
				context.Response.Write(this.GetErrorJson("错误的售后记录"));
			}
			else
			{
				OrderInfo orderInfo = OrderHelper.GetOrderInfo(returns.OrderId);
				if (orderInfo == null)
				{
					context.Response.Write(this.GetErrorJson("错误的售后记录"));
				}
				else
				{
					string adminCellPhone = returns.AdminCellPhone;
					string adminShipAddress = returns.AdminShipAddress;
					string adminShipTo = returns.AdminShipTo;
					IList<LineItemInfo> source = (from i in orderInfo.LineItems.Values
					where i.SkuId == returns.SkuId
					select i).ToList();
					string s = JsonConvert.SerializeObject(new
					{
						Status = "OK",
						Data = new
						{
							SkuId = returns.SkuId,
							Cellphone = adminCellPhone,
							AdminRemark = returns.AdminRemark,
							ShipAddress = adminShipAddress,
							ShipTo = adminShipTo,
							ApplyForTime = returns.ApplyForTime,
							Remark = returns.UserRemark,
							Status = (int)returns.HandleStatus,
							StatusText = ((returns.AfterSaleType == AfterSaleTypes.OnlyRefund) ? EnumDescription.GetEnumDescription((Enum)(object)returns.HandleStatus, 3) : EnumDescription.GetEnumDescription((Enum)(object)returns.HandleStatus, 0)),
							DealTime = returns.AgreedOrRefusedTime,
							FinishTime = returns.FinishTime,
							UserSendGoodsTime = returns.UserSendGoodsTime,
							ConfirmGoodsTime = returns.ConfirmGoodsTime,
							Operator = returns.Operator,
							Reason = returns.ReturnReason,
							ReturnId = returns.ReturnId,
							ShipOrderNumber = returns.ShipOrderNumber,
							OrderId = returns.OrderId,
							Quantity = returns.Quantity,
							OrderTotal = orderInfo.GetTotal(false),
							IsOnlyRefund = (returns.AfterSaleType == AfterSaleTypes.OnlyRefund),
							RefundMoney = returns.RefundAmount.F2ToString("f2"),
							RefundType = EnumDescription.GetEnumDescription((Enum)(object)returns.RefundType, 0),
							UserCredentials = this.GetImagesFullPath(returns.UserCredentials, '|'),
							BankAccountName = returns.BankAccountName,
							BankAccountNo = returns.BankAccountNo,
							BankName = returns.BankName,
							ProductInfo = from l in source
							select new
							{
								ProductId = l.ProductId,
								ProductName = l.ItemDescription,
								SKU = l.SKU,
								SKUContent = this.newSKUContent(l.SKUContent),
								Price = l.ItemAdjustedPrice.F2ToString("f2"),
								Quantity = l.ShipmentQuantity,
								ThumbnailsUrl = this.GetImageFullPath(l.ThumbnailsUrl)
							}
						}
					});
					context.Response.Write(s);
					context.Response.End();
				}
			}
		}

		private IList<string> GetImagesFullPath(string images, char splitchar = '|')
		{
			IList<string> list = new List<string>();
			if (string.IsNullOrEmpty(images))
			{
				return list;
			}
			string[] array = images.Split(splitchar);
			foreach (string imageUrl in array)
			{
				list.Add(this.GetImageFullPath(imageUrl));
			}
			return list;
		}

		private void GetOrderProduct(HttpContext context)
		{
			this.CheckOpenId();
			string text = context.Request["orderId"];
			if (string.IsNullOrEmpty(text))
			{
				context.Response.Write(this.GetErrorJson("参数错误"));
			}
			else
			{
				OrderInfo order = OrderHelper.GetOrderInfo(text);
				if (order == null)
				{
					context.Response.Write(this.GetErrorJson("订单错误"));
				}
				else
				{
					string s = JsonConvert.SerializeObject(new
					{
						Status = "OK",
						Data = from d in order.LineItems.Keys
						select new
						{
							ProductId = order.LineItems[d].ProductId,
							SkuId = order.LineItems[d].SkuId,
							SkuContent = order.LineItems[d].SKUContent,
							Price = order.LineItems[d].ItemAdjustedPrice.F2ToString("f2"),
							Image = this.GetImageFullPath(order.LineItems[d].ThumbnailsUrl)
						}
					});
					context.Response.Write(s);
				}
			}
		}

		private void AddProductReview(HttpContext context)
		{
			this.CheckOpenId();
			string szJson = context.Request["DataJson"];
			string text = "";
			int num = 0;
			List<ProductReviewInfo> list = JsonHelper.ParseFormJson<List<ProductReviewInfo>>(szJson);
			if (list != null && list.Count > 0)
			{
				string orderId = list[0].OrderId;
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(orderId);
				if (orderInfo.OrderStatus != OrderStatus.Finished && (orderInfo.OrderStatus != OrderStatus.Closed || orderInfo.OnlyReturnedCount != orderInfo.LineItems.Count))
				{
					context.Response.Write(this.GetErrorJson("订单未完成"));
					return;
				}
				int num2 = 0;
				int num3 = 0;
				foreach (ProductReviewInfo item in list)
				{
					ProductBrowser.LoadProductReview(item.ProductId, out num2, out num3, item.OrderId);
					if (num2 == 0)
					{
						context.Response.Write(this.GetErrorJson("您没有购买此商品(或此商品的订单尚未完成)，因此不能进行评论"));
						return;
					}
					if (num3 >= num2)
					{
						context.Response.Write(this.GetErrorJson("您已经对此商品进行过评论(或此商品的订单尚未完成)，因此不能再次进行评论"));
						return;
					}
					item.ReviewDate = DateTime.Now;
					item.UserId = HiContext.Current.UserId;
					item.UserName = HiContext.Current.User.UserName.ToNullString();
					item.UserEmail = HiContext.Current.User.Email.ToNullString();
					int num4 = 0;
					string[] array = item.ImageUrl1.Split(',');
					foreach (string text2 in array)
					{
						if (!string.IsNullOrEmpty(text2))
						{
							string text3 = text2;
							if (text2.ToLower().IndexOf(Globals.GetStoragePath().ToLower()) > 0)
							{
								text3 = text2.Substring(text2.ToLower().IndexOf(Globals.GetStoragePath().ToLower()));
							}
							text3 = text3.Replace("//", "/");
							string text4 = Globals.SaveFile("review", text3, "/Storage/master/", true, false, "");
							num4++;
							switch (num4)
							{
							case 1:
								item.ImageUrl1 = text4;
								break;
							case 2:
								item.ImageUrl2 = text4;
								break;
							case 3:
								item.ImageUrl3 = text4;
								break;
							case 4:
								item.ImageUrl4 = text4;
								break;
							case 5:
								item.ImageUrl5 = text4;
								break;
							}
						}
					}
					ValidationResults validationResults = Validation.Validate(item, "Refer");
					text = string.Empty;
					if (!validationResults.IsValid)
					{
						foreach (ValidationResult item2 in (IEnumerable<ValidationResult>)validationResults)
						{
							text += Formatter.FormatErrorMessage(item2.Message);
						}
						break;
					}
					if (!ProductBrowser.InsertProductReview(item))
					{
						text = "评论失败请重试";
						break;
					}
					MemberInfo user = Users.GetUser(HiContext.Current.UserId);
					SiteSettings masterSettings = SettingsManager.GetMasterSettings();
					if (user != null && masterSettings != null && masterSettings.ProductCommentPoint > 0 && TradeHelper.ProductPreviewAddPoint(user, masterSettings.ProductCommentPoint, PointTradeType.ProductCommentPoint))
					{
						num += masterSettings.ProductCommentPoint;
					}
				}
			}
			else
			{
				text = "请输入评价内容";
			}
			if (text != "")
			{
				context.Response.Write(this.GetErrorJson(text));
			}
			else
			{
				context.Response.Write(this.GetOKJson((num > 0) ? $"评价成功,恭喜您获得{num}积分奖励" : "评价成功"));
			}
		}

		public void StatisticsReview(HttpContext context)
		{
			int reviewNum = 0;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = context.Request["ProductId"].ToInt(0);
			if (num5 <= 0)
			{
				context.Response.Write(this.GetErrorJson("商品已下架"));
				context.Response.End();
			}
			DataTable productReviewScore = ProductBrowser.GetProductReviewScore(num5);
			ProductInfo productSimpleInfo = ProductBrowser.GetProductSimpleInfo(num5);
			if (productReviewScore != null && productReviewScore.Rows.Count > 0)
			{
				reviewNum = productReviewScore.Rows.Count;
				foreach (DataRow row in productReviewScore.Rows)
				{
					if (row["Score"].ToInt(0) > 3)
					{
						num++;
					}
					else if (row["Score"].ToInt(0) > 1)
					{
						num2++;
					}
					else
					{
						num3++;
					}
					if (row["ImageUrl1"].ToNullString() != "")
					{
						num4++;
					}
				}
			}
			string s = JsonConvert.SerializeObject(new
			{
				Status = "OK",
				Data = new
				{
					productName = productSimpleInfo.ProductName,
					reviewNum = reviewNum,
					reviewNum1 = num,
					reviewNum2 = num2,
					reviewNum3 = num3,
					reviewNumImg = num4
				}
			});
			context.Response.Write(s);
			context.Response.End();
		}

		public void LoadReview(HttpContext context)
		{
			int num = context.Request["PageSize"].ToInt(0);
			int num2 = context.Request["PageIndex"].ToInt(0);
			int num3 = context.Request["ProductId"].ToInt(0);
			int value = context.Request["type"].ToInt(0);
			if (num2 <= 0)
			{
				num2 = 1;
			}
			if (num < 1)
			{
				num = 10;
			}
			if (num3 <= 0)
			{
				context.Response.Write(this.GetErrorJson("商品已下架"));
				context.Response.End();
			}
			ProductReviewQuery productReviewQuery = new ProductReviewQuery();
			productReviewQuery.PageIndex = num2;
			productReviewQuery.PageSize = num;
			productReviewQuery.ProductId = num3;
			productReviewQuery.SortBy = "ReviewDate";
			productReviewQuery.ProductSearchType = value;
			productReviewQuery.SortOrder = SortAction.Desc;
			DbQueryResult productReviews = ProductBrowser.GetProductReviews(productReviewQuery);
			DataTable data = productReviews.Data;
			string s = JsonConvert.SerializeObject(new
			{
				Status = "OK",
				totalCount = productReviews.TotalRecords,
				Data = data.AsEnumerable().Select(delegate(DataRow d)
				{
					string hiddenUsername = DataHelper.GetHiddenUsername(d.Field<string>("UserName"));
					string picture = string.IsNullOrWhiteSpace(d.Field<string>("Picture")) ? Globals.HttpsFullPath("/templates/pccommon/images/users/hyzx_25.jpg") : Globals.HttpsFullPath(d.Field<string>("Picture"));
					int productId = d.Field<int>("ProductId");
					string thumbnailUrl = d.Field<string>("ThumbnailUrl220");
					string productName = d.Field<string>("ProductName");
					string sKUContent = this.newSKUContent(d.Field<string>("SKUContent"));
					string reviewText = d.Field<string>("ReviewText");
					int score = d.Field<int>("Score");
					string imageUrl = Globals.HttpsFullPath(d.Field<string>("ImageUrl1"));
					string imageUrl2 = Globals.HttpsFullPath(d.Field<string>("ImageUrl2"));
					string imageUrl3 = Globals.HttpsFullPath(d.Field<string>("ImageUrl3"));
					string imageUrl4 = Globals.HttpsFullPath(d.Field<string>("ImageUrl4"));
					string imageUrl5 = Globals.HttpsFullPath(d.Field<string>("ImageUrl5"));
					string replyText = d.Field<string>("ReplyText");
					object reviewDate;
					DateTime value2;
					if (!d.Field<DateTime?>("ReviewDate").ToDateTime().HasValue)
					{
						reviewDate = "";
					}
					else
					{
						value2 = d.Field<DateTime?>("ReviewDate").ToDateTime().Value;
						reviewDate = value2.ToString("yyyy/MM/dd");
					}
					object replyDate;
					if (!d.Field<DateTime?>("ReplyDate").ToDateTime().HasValue)
					{
						replyDate = "";
					}
					else
					{
						value2 = d.Field<DateTime?>("ReplyDate").ToDateTime().Value;
						replyDate = value2.ToString("yyyy/MM/dd");
					}
					return new
					{
						UserName = hiddenUsername,
						Picture = picture,
						ProductId = productId,
						ThumbnailUrl100 = thumbnailUrl,
						ProductName = productName,
						SKUContent = sKUContent,
						ReviewText = reviewText,
						Score = score,
						ImageUrl1 = imageUrl,
						ImageUrl2 = imageUrl2,
						ImageUrl3 = imageUrl3,
						ImageUrl4 = imageUrl4,
						ImageUrl5 = imageUrl5,
						ReplyText = replyText,
						ReviewDate = (string)reviewDate,
						ReplyDate = (string)replyDate
					};
				})
			});
			context.Response.Write(s);
			context.Response.End();
		}

		public void GetShoppingCartList(HttpContext context)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			context.Response.ContentType = "application/json";
			this.CheckOpenId();
			ShoppingCartInfo CartInfo = ShoppingCartProcessor.GetShoppingCart(null, true, true, 0);
			GetShoppingCartModel model = new GetShoppingCartModel();
			this.FilterShoppingCart(CartInfo);
			if (CartInfo != null)
			{
				model.CartItemInfo = new List<CartItemInfo>();
				int num;
				bool flag;
				foreach (ShoppingCartItemInfo lineItem in CartInfo.LineItems)
				{
					CartItemInfo cartItemInfo = new CartItemInfo();
					int skuStock = ShoppingCartProcessor.GetSkuStock(lineItem.SkuId, lineItem.StoreId);
					cartItemInfo.SkuID = lineItem.SkuId;
					CartItemInfo cartItemInfo2 = cartItemInfo;
					num = lineItem.Quantity;
					cartItemInfo2.Quantity = num.ToString();
					CartItemInfo cartItemInfo3 = cartItemInfo;
					num = lineItem.ShippQuantity;
					cartItemInfo3.ShippQuantity = num.ToString();
					CartItemInfo cartItemInfo4 = cartItemInfo;
					flag = lineItem.IsfreeShipping;
					cartItemInfo4.IsfreeShipping = flag.ToString();
					CartItemInfo cartItemInfo5 = cartItemInfo;
					flag = lineItem.IsSendGift;
					cartItemInfo5.IsSendGift = flag.ToString();
					cartItemInfo.MemberPrice = lineItem.MemberPrice.F2ToString("f2");
					cartItemInfo.Name = lineItem.Name;
					CartItemInfo cartItemInfo6 = cartItemInfo;
					num = lineItem.ProductId;
					cartItemInfo6.ProductId = num.ToString();
					cartItemInfo.PromoteType = lineItem.PromoteType.ToString();
					CartItemInfo cartItemInfo7 = cartItemInfo;
					num = lineItem.PromotionId;
					cartItemInfo7.PromotionId = num.ToString();
					cartItemInfo.PromotionName = lineItem.PromotionName;
					cartItemInfo.SKU = lineItem.SKU;
					cartItemInfo.SkuContent = this.newSKUContent(lineItem.SkuContent);
					cartItemInfo.SubTotal = lineItem.SubTotal.F2ToString("f2");
					cartItemInfo.ThumbnailUrl100 = this.GetImageFullPath(lineItem.ThumbnailUrl180);
					cartItemInfo.ThumbnailUrl40 = this.GetImageFullPath(lineItem.ThumbnailUrl40);
					cartItemInfo.ThumbnailUrl60 = this.GetImageFullPath(lineItem.ThumbnailUrl60);
					cartItemInfo.Weight = lineItem.Weight.F2ToString("f2");
					cartItemInfo.Stock = skuStock;
					cartItemInfo.HasStore = lineItem.HasStore.ToNullString();
					cartItemInfo.IsMobileExclusive = lineItem.IsMobileExclusive;
					cartItemInfo.IsValid = lineItem.IsValid;
					cartItemInfo.HasEnoughStock = (skuStock > 0 && skuStock >= lineItem.Quantity);
					cartItemInfo.SupplierId = lineItem.SupplierId;
					cartItemInfo.SupplierName = lineItem.SupplierName;
					cartItemInfo.CostPrice = lineItem.CostPrice;
					cartItemInfo.StoreId = lineItem.StoreId;
					cartItemInfo.StoreName = lineItem.StoreName;
					cartItemInfo.StoreStatus = DetailException.Nomal;
					if (lineItem.StoreId > 0)
					{
						StoresInfo storeById = StoresHelper.GetStoreById(lineItem.StoreId);
						if (!storeById.CloseStatus && storeById.CloseEndTime.HasValue && storeById.CloseBeginTime.HasValue)
						{
							if (storeById.CloseEndTime.Value > DateTime.Now && storeById.CloseBeginTime.Value < DateTime.Now)
							{
								cartItemInfo.StoreStatus = DetailException.StopService;
							}
						}
						else if (skuStock == 0)
						{
							cartItemInfo.StoreStatus = DetailException.NoStock;
						}
						else if (!masterSettings.Store_IsOrderInClosingTime && lineItem.StoreId > 0)
						{
							DateTime dateTime = DateTime.Now;
							string str = dateTime.ToString("yyyy-MM-dd");
							dateTime = storeById.OpenStartDate;
							DateTime value = (str + " " + dateTime.ToString("HH:mm")).ToDateTime().Value;
							dateTime = DateTime.Now;
							string str2 = dateTime.ToString("yyyy-MM-dd");
							dateTime = storeById.OpenEndDate;
							DateTime dateTime2 = (str2 + " " + dateTime.ToString("HH:mm")).ToDateTime().Value;
							if (dateTime2 <= value)
							{
								dateTime2 = dateTime2.AddDays(1.0);
							}
							if (DateTime.Now < value || DateTime.Now > dateTime2)
							{
								cartItemInfo.StoreStatus = DetailException.IsNotWorkTime;
							}
						}
					}
					else if (skuStock == 0)
					{
						cartItemInfo.StoreStatus = DetailException.NoStock;
					}
					cartItemInfo.SendGift = null;
					model.CartItemInfo.Add(cartItemInfo);
				}
				model.GiftInfo = new List<Hidistro.Entities.APP.GiftInfo>();
				for (int j = 0; j < (from a in CartInfo.LineGifts
				where a.PromoType == 0
				select a).Count(); j++)
				{
					Hidistro.Entities.APP.GiftInfo giftInfo = new Hidistro.Entities.APP.GiftInfo();
					ShoppingCartGiftInfo shoppingCartGiftInfo = CartInfo.LineGifts[j];
					Hidistro.Entities.APP.GiftInfo giftInfo2 = giftInfo;
					num = shoppingCartGiftInfo.GiftId;
					giftInfo2.GiftId = num.ToString();
					giftInfo.Name = shoppingCartGiftInfo.Name;
					Hidistro.Entities.APP.GiftInfo giftInfo3 = giftInfo;
					num = shoppingCartGiftInfo.NeedPoint;
					giftInfo3.NeedPoint = num.ToString();
					Hidistro.Entities.APP.GiftInfo giftInfo4 = giftInfo;
					num = shoppingCartGiftInfo.PromoType;
					giftInfo4.PromoType = num.ToString();
					Hidistro.Entities.APP.GiftInfo giftInfo5 = giftInfo;
					num = shoppingCartGiftInfo.Quantity;
					giftInfo5.Quantity = num.ToString();
					Hidistro.Entities.APP.GiftInfo giftInfo6 = giftInfo;
					num = shoppingCartGiftInfo.SubPointTotal;
					giftInfo6.SubPointTotal = num.ToString();
					giftInfo.ThumbnailUrl100 = this.GetImageFullPath(shoppingCartGiftInfo.ThumbnailUrl180);
					giftInfo.ThumbnailUrl40 = this.GetImageFullPath(shoppingCartGiftInfo.ThumbnailUrl40);
					giftInfo.ThumbnailUrl60 = this.GetImageFullPath(shoppingCartGiftInfo.ThumbnailUrl60);
					model.GiftInfo.Add(giftInfo);
				}
				model.RecordCount = CartInfo.GetQuantity(false);
				model.Amount = CartInfo.StrAmount;
				model.Point = CartInfo.GetPoint(masterSettings.PointsRate);
				model.Total = CartInfo.StrTotalAmount;
				GetShoppingCartModel getShoppingCartModel = model;
				flag = CartInfo.IsFreightFree;
				getShoppingCartModel.IsFreightFree = flag.ToString();
				GetShoppingCartModel getShoppingCartModel2 = model;
				flag = CartInfo.IsReduced;
				getShoppingCartModel2.IsReduced = flag.ToString();
				GetShoppingCartModel getShoppingCartModel3 = model;
				flag = CartInfo.IsSendGift;
				getShoppingCartModel3.IsSendGift = flag.ToString();
				GetShoppingCartModel getShoppingCartModel4 = model;
				flag = CartInfo.IsSendTimesPoint;
				getShoppingCartModel4.IsSendTimesPoint = flag.ToString();
				model.ReducedPromotionAmount = CartInfo.StrReducedPromotionAmount;
				model.ReducedPromotionId = CartInfo.ReducedPromotionId;
				model.ReducedPromotionName = CartInfo.ReducedPromotionName;
				model.SendGiftPromotionId = CartInfo.SendGiftPromotionId;
				model.SendGiftPromotionName = CartInfo.SendGiftPromotionName;
				model.SentTimesPointPromotionId = CartInfo.SentTimesPointPromotionId;
				model.SentTimesPointPromotionName = CartInfo.SentTimesPointPromotionName;
				model.TimesPoint = CartInfo.TimesPoint;
				model.TotalWeight = CartInfo.TotalWeight;
				model.Weight = CartInfo.Weight;
				model.FreightFreePromotionId = CartInfo.FreightFreePromotionId;
				model.FreightFreePromotionName = CartInfo.FreightFreePromotionName;
			}
			else
			{
				model.RecordCount = 0;
				model.CartItemInfo = new List<CartItemInfo>();
				model.GiftInfo = new List<Hidistro.Entities.APP.GiftInfo>();
			}
			IList<SupplierBaseInfo> list = new List<SupplierBaseInfo>();
			List<CartItemInfo> list2 = (from i in model.CartItemInfo
			group i by new
			{
				i.SupplierId
			} into g
			select g.First() into o
			orderby o.SupplierId
			select o).ToList();
			foreach (CartItemInfo item in list2)
			{
				list.Add(new SupplierBaseInfo
				{
					SupplierId = item.SupplierId,
					SupplierName = item.SupplierName
				});
			}
			model.Suppliers = list;
			List<CartItemInfo> cartItemInfo8 = (from info in model.CartItemInfo
			orderby info.IsValid descending, info.HasEnoughStock descending
			select info).ToList();
			model.CartItemInfo = cartItemInfo8;
			string s2 = JsonConvert.SerializeObject(new
			{
				Status = "OK",
				Data = new
				{
					RecordCount = model.RecordCount,
					Amount = model.Amount,
					FreightFreePromotionId = model.FreightFreePromotionId,
					FreightFreePromotionName = model.FreightFreePromotionName,
					GiftInfo = model.GiftInfo,
					Point = model.Point,
					IsFreightFree = model.IsFreightFree,
					IsReduced = model.IsReduced,
					IsSendGift = model.IsSendGift,
					IsSendTimesPoint = model.IsSendTimesPoint,
					ReducedPromotionAmount = model.ReducedPromotionAmount,
					ReducedPromotionId = model.ReducedPromotionId,
					ReducedPromotionName = model.ReducedPromotionName,
					SendGiftPromotionId = model.SendGiftPromotionId,
					SendGiftPromotionName = model.SendGiftPromotionName,
					SentTimesPointPromotionId = model.SentTimesPointPromotionId,
					TimesPoint = model.TimesPoint,
					Total = model.Total,
					TotalWeight = model.TotalWeight,
					Weight = model.Weight,
					Suppliers = from s in model.Suppliers
					select new
					{
						SupplierId = s.SupplierId,
						SupplierName = s.SupplierName,
						SupplierTotal = (from c in CartInfo.LineItems
						where c.SupplierId == s.SupplierId
						select c).Sum((ShoppingCartItemInfo ci) => ci.SubTotal),
						CartItemInfo = (from c in model.CartItemInfo
						where c.SupplierId == s.SupplierId
						select c).ToList()
					},
					CartItemInfo = model.CartItemInfo
				}
			});
			context.Response.Write(s2);
			context.Response.End();
		}

		public void InitVeCode(HttpContext context)
		{
			string format = "{{\"success\":{0},\"msg\":{1}}}";
			context.Response.ContentType = "application/json";
			string text = context.Request["Vid"].ToNullString();
			if (string.IsNullOrEmpty(text))
			{
				context.Response.Write("{\"success\":flase,\"msg\":\"版本号为空\"}");
			}
			else
			{
				SiteSettings siteSettings = HiContext.Current.SiteSettings;
				long xcxHomeVersionCode = siteSettings.XcxHomeVersionCode;
				format = ((xcxHomeVersionCode <= long.Parse(text)) ? string.Format(format, false, "版本不需要更新") : string.Format(format, true, "版本需要更新"));
				context.Response.Write(format);
				context.Response.End();
			}
		}

		private void AddToCart(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			this.CheckOpenId();
			string text = context.Request["SkuID"].ToNullString();
			int num = context.Request["Quantity"].ToInt(0);
			int giftId = context.Request["GiftID"].ToInt(0);
			AddCartItemStatus addCartItemStatus;
			if (!string.IsNullOrEmpty(text))
			{
				if (!ProductHelper.ProductsIsAllOnSales(text, 0))
				{
					context.Response.Write(this.GetErrorJosn(401, "商品已下架"));
					return;
				}
				addCartItemStatus = ShoppingCartProcessor.AddLineItem(text, num, true, 0);
			}
			else
			{
				if (num < 0)
				{
					ShoppingCartProcessor.UpdateOrDeleteGiftQuantity(giftId, num, HiContext.Current.User.UserId, PromoteType.NotSet);
				}
				else
				{
					ShoppingCartProcessor.AddGiftItem(giftId, num, PromoteType.NotSet);
				}
				addCartItemStatus = AddCartItemStatus.Successed;
			}
			switch (addCartItemStatus)
			{
			case AddCartItemStatus.Successed:
				this.GetShoppingCartList(context);
				break;
			case AddCartItemStatus.InvalidUser:
				context.Response.Write(this.GetErrorJosn(108, "错误的用户信息"));
				break;
			case AddCartItemStatus.Offsell:
				context.Response.Write(this.GetErrorJosn(401, "商品已下架"));
				break;
			case AddCartItemStatus.ProductNotExists:
				context.Response.Write(this.GetErrorJosn(404, "商品不存在"));
				break;
			case AddCartItemStatus.Shortage:
				context.Response.Write(this.GetErrorJosn(109, "商品库存不足"));
				break;
			}
			context.Response.End();
		}

		private void DelCartItem(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			this.CheckOpenId();
			string text = context.Request["SkuIDs"].ToNullString();
			string text2 = context.Request["GiftIDs"].ToNullString();
			if (!string.IsNullOrEmpty(text))
			{
				string[] array = text.Split(',');
				foreach (string text3 in array)
				{
					int storeId = 0;
					string text4 = text3.Trim();
					if (text4.Split('*').Length == 2)
					{
						storeId = text4.Split('*')[1].ToInt(0);
						text4 = text4.Split('*')[0];
					}
					ShoppingCartProcessor.RemoveLineItem(text4, storeId);
				}
			}
			if (!string.IsNullOrEmpty(text2))
			{
				string[] array2 = text2.Split(',');
				foreach (string s in array2)
				{
					ShoppingCartProcessor.RemoveGiftItem(int.Parse(s), PromoteType.NotSet);
				}
			}
			this.GetShoppingCartList(context);
		}

		private void CanSubmitOrder(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			this.CheckOpenId();
			string text = context.Request["skus"].ToNullString();
			string text2 = "";
			if (!string.IsNullOrEmpty(text))
			{
				List<string> skus = (from d in text.Split(',')
				where !string.IsNullOrWhiteSpace(d)
				select d).ToList();
				text2 = (ShoppingCartProcessor.HasInvalidProduct(skus, ClientType.WeChatApplet, 0) ? this.GetErrorJson("有失效商品") : this.GetOKJson("OK"));
			}
			else
			{
				text2 = this.GetErrorJson("请选择商品");
			}
			context.Response.Write(text2);
			context.Response.End();
		}

		private void GetRegionsOfProvinceCity(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			string text = context.Request["parentRegionId"];
			IList<Hidistro.Entities.Store.RegionInfo> allProvinceLists = RegionHelper.GetAllProvinceLists(false);
			if (allProvinceLists != null && allProvinceLists.Count > 0)
			{
				string s = JsonConvert.SerializeObject(new
				{
					Status = "OK",
					province = from p in allProvinceLists
					select new
					{
						id = p.RegionId,
						name = p.RegionName,
						city = from c in RegionHelper.GetRegionChildList(p.RegionId, false)
						select new
						{
							id = c.RegionId,
							name = c.RegionName,
							area = from a in RegionHelper.GetRegionChildList(c.RegionId, false)
							select new
							{
								id = a.RegionId,
								name = a.RegionName
							}
						}
					}
				});
				context.Response.Write(s);
				context.Response.End();
			}
		}

		private void GetRegions(HttpContext context)
		{
			try
			{
				context.Response.ContentType = "application/json";
				int num = 0;
				int.TryParse(context.Request["parentId"], out num);
				int num2 = 1;
				IDictionary<int, string> dictionary;
				if (num == 0)
				{
					dictionary = RegionHelper.GetAllProvinces(false);
					num2 = 1;
				}
				else
				{
					Hidistro.Entities.Store.RegionInfo region = RegionHelper.GetRegion(num, true);
					if (region == null)
					{
						context.Response.Write("{\"Status\":\"0\"}");
						goto end_IL_0001;
					}
					num2 = region.Depth + 1;
					dictionary = ((region.Depth != 1) ? ((region.Depth != 2) ? RegionHelper.GetStreets(num, false) : RegionHelper.GetCountys(num, false)) : RegionHelper.GetCitys(num, false));
					if (dictionary == null || dictionary.Count == 0)
					{
						context.Response.Write("{\"Status\":\"0\"}");
						goto end_IL_0001;
					}
				}
				IList<Hidistro.Entities.Store.RegionInfo> list = new List<Hidistro.Entities.Store.RegionInfo>();
				foreach (int key in dictionary.Keys)
				{
					Hidistro.Entities.Store.RegionInfo regionInfo = new Hidistro.Entities.Store.RegionInfo();
					regionInfo.RegionId = key;
					regionInfo.RegionName = dictionary[key];
					list.Add(regionInfo);
				}
				string s = JsonConvert.SerializeObject(new
				{
					Status = "OK",
					Depth = num2,
					Regions = from r in list
					select new
					{
						id = r.RegionId,
						name = r.RegionName
					}
				});
				context.Response.Write(s);
				end_IL_0001:;
			}
			catch (Exception ex)
			{
				Globals.WriteExceptionLog(ex, null, "GetRegions");
				context.Response.Write(this.GetErrorJson(ex.Message));
			}
		}

		private void GetOrderDetail()
		{
			this.CheckOpenId();
			string text = this.context.Request["orderId"];
			if (string.IsNullOrEmpty(text))
			{
				this.context.Response.Write(this.GetErrorJson("参数错误"));
			}
			else
			{
				OrderInfo order = OrderHelper.GetOrderInfo(text);
				if (order == null)
				{
					this.context.Response.Write(this.GetErrorJson("订单错误"));
				}
				else
				{
					string text2 = "";
					if (order != null && (order.OrderStatus == OrderStatus.SellerAlreadySent || order.OrderStatus == OrderStatus.Finished) && !string.IsNullOrEmpty(order.ExpressCompanyAbb))
					{
						text2 = ExpressHelper.GetDataByKuaidi100(order.ExpressCompanyAbb, order.ShipOrderNumber);
						if (string.IsNullOrEmpty(text2) || text2 == "此单无物流信息")
						{
							text2 = "";
						}
					}
					UserInvoiceDataInfo userInvoiceDataInfo = order.InvoiceInfo;
					if (userInvoiceDataInfo == null)
					{
						userInvoiceDataInfo = new UserInvoiceDataInfo
						{
							InvoiceTaxpayerNumber = order.InvoiceTaxpayerNumber,
							InvoiceType = order.InvoiceType,
							InvoiceTitle = order.InvoiceTitle
						};
					}
					OrderStatusApp orderStatus = (OrderStatusApp)this.GetOrderStatus(order);
					var source = (from i in (from i in order.LineItems.Values
					select new
					{
						i.SupplierId,
						i.SupplierName
					}).Distinct()
					orderby i.SupplierId
					select i).ToList();
					string logisticsData = text2;
					string expressCompanyName = order.ExpressCompanyName;
					string shipOrderNumber = order.ShipOrderNumber;
					string orderId = order.OrderId;
					DateTime dateTime;
					object takeCodeUsedTime;
					if (!string.IsNullOrEmpty(order.TakeTime))
					{
						dateTime = order.TakeTime.ToDateTime().Value;
						takeCodeUsedTime = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
					}
					else
					{
						takeCodeUsedTime = "";
					}
					string takeCode = order.TakeCode.ToNullString().ToLower().Replace("ysc", "");
					string enumDescription = EnumDescription.GetEnumDescription((Enum)(object)orderStatus, 0);
					OrderStatus orderStatus2 = order.OrderStatus;
					OrderItemStatus itemStatus = order.ItemStatus;
					string enumDescription2 = EnumDescription.GetEnumDescription((Enum)(object)order.ItemStatus, 0);
					dateTime = order.OrderDate;
					string s2 = JsonConvert.SerializeObject(new
					{
						Status = "OK",
						Data = new
						{
							LogisticsData = logisticsData,
							ExpressCompanyName = expressCompanyName,
							ShipOrderNumber = shipOrderNumber,
							OrderId = orderId,
							TakeCodeIsUsed = false,
							TakeCodeUsedTime = (string)takeCodeUsedTime,
							TakeCode = takeCode,
							StatusText = enumDescription,
							Status = (int)orderStatus2,
							ItemStatus = (int)itemStatus,
							ItemStatusText = enumDescription2,
							OrderDate = dateTime.ToString("yyyy-MM-dd HH:mm:ss"),
							ShipTo = order.ShipTo,
							ShipToDate = order.ShipToDate,
							Cellphone = (string.IsNullOrEmpty(order.CellPhone) ? order.TelPhone : order.CellPhone),
							Address = order.ShippingRegion.Replace("，", ",").Replace(",", "") + order.Address,
							OrderTotal = order.GetTotal(false).F2ToString("f2"),
							OrderAmount = order.GetAmount(false).F2ToString("f2"),
							FreightFreePromotionName = order.FreightFreePromotionName,
							ReducedPromotionName = order.ReducedPromotionName,
							ReducedPromotionAmount = order.ReducedPromotionAmount,
							SentTimesPointPromotionName = order.SentTimesPointPromotionName,
							CanBackReturn = TradeHelper.IsCanBackReturn(order),
							CanCashierReturn = order.IsStoreCollect,
							PaymentType = order.PaymentType,
							DeductionPoints = order.DeductionPoints,
							CouponAmount = order.CouponValue,
							CouponName = order.CouponName,
							DeductionMoney = order.DeductionMoney,
							RefundAmount = order.RefundAmount,
							Remark = order.Remark,
							InvoiceTitle = order.InvoiceTitle,
							Tax = order.Tax,
							AdjustedFreight = order.AdjustedFreight.F2ToString("f2"),
							Freight = order.Freight.F2ToString("f2"),
							ModeName = order.ModeName,
							CountDownId = order.CountDownBuyId,
							GroupBuyId = order.GroupBuyId,
							PreSaleId = order.PreSaleId,
							HasInvoice = !string.IsNullOrEmpty(order.InvoiceTitle),
							InvoiceTaxpayerNumber = order.InvoiceTaxpayerNumber,
							InvoceTypeText = userInvoiceDataInfo.InvoceTypeText,
							InvoiceType = userInvoiceDataInfo.InvoiceType,
							Id = userInvoiceDataInfo.Id,
							OpenBank = userInvoiceDataInfo.OpenBank.ToNullString(),
							BankAccount = userInvoiceDataInfo.BankAccount.ToNullString(),
							ReceiveAddress = userInvoiceDataInfo.ReceiveAddress.ToNullString(),
							ReceiveEmail = userInvoiceDataInfo.ReceiveEmail.ToNullString(),
							ReceiveName = userInvoiceDataInfo.ReceiveName.ToNullString(),
							ReceivePhone = userInvoiceDataInfo.ReceivePhone.ToNullString(),
							ReceiveRegionName = userInvoiceDataInfo.ReceiveRegionName.ToNullString(),
							ReceiveRegionId = userInvoiceDataInfo.ReceiveRegionId.ToInt(0),
							RegisterAddress = userInvoiceDataInfo.RegisterAddress.ToNullString(),
							RegisterTel = userInvoiceDataInfo.RegisterTel.ToNullString(),
							AdjustedDiscount = order.AdjustedDiscount,
							IsShowRefund = ((order.FightGroupId > 0 && VShopHelper.IsFightGroupCanRefund(order.FightGroupId) && order.IsCanRefund) || (order.FightGroupId <= 0 && order.IsCanRefund)),
							IsShowPay = (order.OrderStatus == OrderStatus.WaitBuyerPay && order.Gateway != "hishop.plugins.payment.payonstore" && order.Gateway != "hishop.plugins.payment.podrequest" && order.Gateway != "hishop.plugins.payment.bankrequest"),
							StoreId = order.StoreId,
							StoreName = order.StoreName,
							Suppliers = from s in source
							select new
							{
								SupplierId = s.SupplierId,
								SupplierName = s.SupplierName,
								LineItems = from l in order.LineItems.Values
								where l.SupplierId == s.SupplierId
								select l into si
								select new
								{
									Status = si.Status,
									StatusText = si.StatusText.Replace("正常状态", ""),
									Id = si.SkuId,
									Name = si.ItemDescription,
									Price = si.ItemAdjustedPrice.F2ToString("f2"),
									Amount = si.Quantity,
									Image = this.GetImageFullPath(si.ThumbnailsUrl),
									SkuText = this.newSKUContent(si.SKUContent),
									ProductId = si.ProductId,
									PromotionName = si.PromotionName,
									SendCount = ((si.ShipmentQuantity > si.Quantity) ? (si.ShipmentQuantity - si.Quantity) : 0),
									StoreId = order.StoreId,
									StoreName = order.StoreName
								}
							},
							Gifts = from g in order.Gifts
							select new
							{
								GiftId = g.GiftId,
								GiftName = g.GiftName,
								PromoteType = g.PromoteType,
								Quantity = g.Quantity,
								ImageUrl = this.GetImageFullPath(g.ThumbnailsUrl)
							}
						}
					});
					this.context.Response.Write(s2);
				}
			}
		}

		private void GetShippingAddressById()
		{
			this.CheckOpenId();
			int num = this.context.Request["shippingId"].ToInt(0);
			if (num <= 0)
			{
				this.context.Response.Write(this.GetErrorJson("参数错误"));
			}
			else
			{
				ShippingAddressInfo shippingAddress = MemberProcessor.GetShippingAddress(num);
				if (shippingAddress != null)
				{
					shippingAddress.FullRegionPath = RegionHelper.GetFullRegion(shippingAddress.RegionId, " ", true, 0);
					shippingAddress.FullAddress = shippingAddress.FullRegionPath + " " + shippingAddress.RegionLocation + " " + shippingAddress.Address + " " + shippingAddress.BuildingNumber;
					string s = JsonConvert.SerializeObject(new
					{
						Status = "OK",
						Data = new
						{
							ShippingAddressInfo = shippingAddress
						}
					});
					this.context.Response.Write(s);
				}
				else
				{
					this.context.Response.Write(this.GetErrorJson("系统错误"));
				}
			}
		}

		private void AddWXChooseAddress()
		{
			this.CheckOpenId();
			string text = this.context.Request["city"];
			string text2 = this.context.Request["county"];
			string text3 = this.context.Request["address"];
			int num = this.context.Request["regionId"].ToInt(0);
			if (string.IsNullOrWhiteSpace(text3))
			{
				this.context.Response.Write(this.GetErrorJson("请填写详细地址"));
			}
			else if (num <= 0 && (string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(text2)))
			{
				this.context.Response.Write(this.GetErrorJson("参数错误"));
			}
			else
			{
				if (num <= 0)
				{
					Hidistro.Entities.Store.RegionInfo cityByRegionName = RegionHelper.GetCityByRegionName(text, text2);
					if (cityByRegionName != null)
					{
						num = cityByRegionName.RegionId;
					}
				}
				if (num <= 0 || string.IsNullOrEmpty(RegionHelper.GetFullRegion(num, " ", true, 0)))
				{
					this.context.Response.Write(this.GetErrorJson("错误的地区信息"));
				}
				else
				{
					MemberInfo user = HiContext.Current.User;
					if (MemberProcessor.GetShippingAddressCount(user.UserId) >= HiContext.Current.SiteSettings.UserAddressMaxCount)
					{
						this.context.Response.Write(this.GetErrorJson("收货地址已超过限制个数"));
					}
					else
					{
						IList<ShippingAddressInfo> shippingAddresses = MemberProcessor.GetShippingAddresses(false);
						foreach (ShippingAddressInfo item in shippingAddresses)
						{
							if (num == item.RegionId && text3 == item.Address)
							{
								this.context.Response.Write(this.GetOKJson(item.ShippingId.ToString()));
								return;
							}
						}
						ShippingAddressInfo shippingAddressInfo = new ShippingAddressInfo();
						shippingAddressInfo.Address = Globals.StripAllTags(text3);
						shippingAddressInfo.CellPhone = Globals.StripAllTags(this.context.Request["cellphone"]);
						shippingAddressInfo.ShipTo = Globals.StripAllTags(this.context.Request["shipTo"]);
						shippingAddressInfo.Zipcode = "";
						shippingAddressInfo.IsDefault = this.context.Request["isDefault"].ToBool();
						shippingAddressInfo.UserId = user.UserId;
						shippingAddressInfo.RegionId = num;
						shippingAddressInfo.FullRegionPath = RegionHelper.GetFullPath(shippingAddressInfo.RegionId, true);
						int num2 = MemberProcessor.AddShippingAddress(shippingAddressInfo);
						if (num2 > 0)
						{
							this.context.Response.Write(this.GetOKJson(num2.ToString()));
						}
						else
						{
							this.context.Response.Write(this.GetErrorJson("系统错误"));
						}
					}
				}
			}
		}

		private void GetIndexData()
		{
			this.BindUserByOpenId();
			MemberInfo user = HiContext.Current.User;
			string homeTopicPath = "https://" + HttpContext.Current.Request.Url.Host + "/Templates/xcxshop/data/default.txt";
			long xcxHomeVersionCode = HiContext.Current.SiteSettings.XcxHomeVersionCode;
			List<string> list = new List<string>();
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			string[] array = masterSettings.AppletIndexImages.Split(',');
			string[] array2 = array;
			foreach (string local in array2)
			{
				list.Add(Globals.HttpsFullPath(local));
			}
			string s = JsonConvert.SerializeObject(new
			{
				Status = "OK",
				Data = new
				{
					ImgList = list,
					HomeTopicPath = homeTopicPath,
					Vid = xcxHomeVersionCode.ToString(),
					SiteInfo = new
					{
						QuickLoginIsForceBindingMobbile = (masterSettings.QuickLoginIsForceBindingMobbile && masterSettings.SMSEnabled && !string.IsNullOrEmpty(masterSettings.SMSSettings)),
						ApplyReferralCondition = masterSettings.ApplyReferralCondition,
						ApplyReferralNeedAmount = masterSettings.ApplyReferralNeedAmount,
						UserLoginIsForceBindingMobbile = (masterSettings.UserLoginIsForceBindingMobbile && masterSettings.SMSEnabled && !string.IsNullOrEmpty(masterSettings.SMSSettings)),
						ReferralIntroduction = masterSettings.ReferralIntroduction,
						RecruitmentAgreement = masterSettings.RecruitmentAgreement,
						OpenRecruitmentAgreement = masterSettings.OpenRecruitmentAgreement,
						ReferralPostJson = Globals.HttpsFullPath("/Storage/data/Utility/ReferralPoster.js"),
						ReferralShareTitle = masterSettings.ExtendShareTitle,
						ReferralSharePic = Globals.HttpsFullPath(masterSettings.ExtendSharePic),
						ReferralShareDetail = masterSettings.ExtendShareDetail,
						PromoterNeedInfo = masterSettings.PromoterNeedInfo,
						IsPromoterValidatePhone = masterSettings.IsPromoterValidatePhone,
						EmailEnabled = (masterSettings.EmailEnabled && !string.IsNullOrEmpty(masterSettings.EmailSettings)),
						SMSEnabled = (masterSettings.SMSEnabled && !string.IsNullOrEmpty(masterSettings.SMSSettings)),
						QQMapAPIKey = masterSettings.QQMapAPIKey,
						SplittinDraws_CashToALiPay = masterSettings.SplittinDraws_CashToALiPay,
						EnableBulkPaymentAdvance = masterSettings.EnableBulkPaymentAdvance,
						EnableBulkPaymentAliPay = masterSettings.EnableBulkPaymentAliPay,
						SplittinDraws_CashToBankCard = masterSettings.SplittinDraws_CashToBankCard,
						SplittinDraws_CashToDeposit = masterSettings.SplittinDraws_CashToDeposit,
						SplittinDraws_CashToWeiXin = masterSettings.SplittinDraws_CashToWeiXin,
						OpenReferral = (masterSettings.OpenReferral == 1 && true)
					}
				}
			});
			this.context.Response.Write(s);
		}

		private void GetIndexProductData()
		{
			this.BindUserByOpenId();
			MemberInfo user = HiContext.Current.User;
			int pageIndex = this.context.Request["pageIndex"].ToInt(0);
			int pageSize = this.context.Request["pageSize"].ToInt(0);
			DbQueryResult showProductList = WeChartAppletHelper.GetShowProductList(user.GradeId, pageIndex, pageSize, 0, ProductType.PhysicalProduct);
			ShoppingCartInfo CartInfo = ShoppingCartProcessor.GetMobileShoppingCart(null, true, true, -1);
			int activetype = 0;
			EnumerableRowCollection<DataRow> source = from d in showProductList.Data.AsEnumerable()
			where this.ActivityBusiness(d.Field<int>("ProductId"), out activetype) >= 0 && activetype <= 3
			select d;
			string s = JsonConvert.SerializeObject(new
			{
				Status = "OK",
				Data = new
				{
					ChoiceProducts = from d in source.AsEnumerable()
					select new
					{
						ProductId = d.Field<int>("ProductId"),
						ProductName = d.Field<string>("ProductName"),
						SalePrice = d.Field<decimal>("SalePrice").F2ToString("f2"),
						ThumbnailUrl160 = ((!string.IsNullOrEmpty(d.Field<string>("ThumbnailUrl410"))) ? Globals.HttpsFullPath(d.Field<string>("ThumbnailUrl410")) : Globals.HttpsFullPath(HiContext.Current.SiteSettings.DefaultProductThumbnail8)),
						MarketPrice = d.Field<decimal>("MarketPrice").F2ToString("f2"),
						CartQuantity = this.GetCartProductQuantity(CartInfo, d.Field<int>("ProductId"), ""),
						HasSKU = d.Field<bool>("HasSKU"),
						SkuId = d.Field<string>("SkuId"),
						ActiveId = this.ActivityBusiness(d.Field<int>("ProductId"), out activetype),
						ActiveType = activetype
					}
				}
			});
			this.context.Response.Write(s);
		}

		private string GetProductImageFullPath(string path)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (string.IsNullOrEmpty(path))
			{
				return Globals.HttpsFullPath(masterSettings.DefaultProductThumbnail8);
			}
			if (path.StartsWith("http://") || path.StartsWith("https://"))
			{
				return path;
			}
			if (HttpContext.Current != null)
			{
				if (File.Exists(HttpContext.Current.Server.MapPath(path)))
				{
					return Globals.HttpsFullPath(path);
				}
				return Globals.HttpsFullPath(masterSettings.DefaultProductThumbnail8);
			}
			return Globals.HttpsFullPath(path);
		}

		private void GetProducts()
		{
			this.BindUserByOpenId();
			int num = this.context.Request["pageIndex"].ToInt(0);
			int num2 = this.context.Request["pageSize"].ToInt(0);
			if (num < 1)
			{
				num = 1;
			}
			if (num2 < 1)
			{
				num2 = 10;
			}
			ProductBrowseQuery productBrowseQuery = new ProductBrowseQuery();
			productBrowseQuery.PageIndex = num;
			productBrowseQuery.PageSize = num2;
			if (!string.IsNullOrEmpty(this.context.Request["cId"]))
			{
				int categoryId = this.context.Request["cId"].ToInt(0);
				productBrowseQuery.Category = CatalogHelper.GetCategory(categoryId);
			}
			ShoppingCartInfo CartInfo = ShoppingCartProcessor.GetMobileShoppingCart(null, true, true, -1);
			productBrowseQuery.Keywords = this.context.Request["keyword"];
			productBrowseQuery.SortBy = "DisplaySequence";
			productBrowseQuery.SortOrder = SortAction.Desc;
			productBrowseQuery.SortBy = this.context.Request["sortBy"].ToNullString();
			productBrowseQuery.SortOrder = ((this.context.Request["sortOrder"] == "asc") ? SortAction.Asc : SortAction.Desc);
			productBrowseQuery.StoreId = 0;
			productBrowseQuery.ProductType = ProductType.PhysicalProduct;
			DbQueryResult storeProductList = StoresHelper.GetStoreProductList(productBrowseQuery);
			int activetype = 0;
			EnumerableRowCollection<DataRow> source = from d in storeProductList.Data.AsEnumerable()
			where this.ActivityBusiness(d.Field<int>("ProductId"), out activetype) >= 0 && activetype < 3
			select d;
			string s = JsonConvert.SerializeObject(new
			{
				Status = "OK",
				Data = from d in source.AsEnumerable()
				select new
				{
					ProductId = d.Field<int>("ProductId"),
					ProductName = d.Field<string>("ProductName"),
					Pic = this.GetProductImageFullPath(d.Field<string>("ThumbnailUrl410")),
					MarketPrice = d.Field<decimal>("MarketPrice").F2ToString("f2"),
					SalePrice = d.Field<decimal>("SalePrice").F2ToString("f2"),
					SaleCounts = d.Field<int>("SaleCounts"),
					CartQuantity = this.GetCartProductQuantity(CartInfo, d.Field<int>("ProductId"), ""),
					HasSKU = d.Field<bool>("HasSKU"),
					SkuId = d.Field<string>("SkuId"),
					ActiveId = this.ActivityBusiness(d.Field<int>("ProductId"), out activetype),
					ActiveType = activetype
				}
			});
			this.context.Response.Write(s);
		}

		private int GetCartProductQuantity(ShoppingCartInfo cartInfo, int productId = 0, string stuId = "")
		{
			int num = 0;
			if (cartInfo == null)
			{
				return 0;
			}
			for (int i = 0; i < cartInfo.LineItems.Count(); i++)
			{
				if (productId > 0)
				{
					if (cartInfo.LineItems[i].ProductId == productId)
					{
						num += cartInfo.LineItems[i].Quantity;
					}
				}
				else if (cartInfo.LineItems[i].SkuId == stuId)
				{
					num += cartInfo.LineItems[i].Quantity;
				}
			}
			return num;
		}

		public void SubmitOrder()
		{
			try
			{
				this.CheckOpenId();
				string text = "";
				if (!string.IsNullOrEmpty(this.context.Request["fromPage"]))
				{
					text = this.context.Request["fromPage"].ToLower();
				}
				int shippingId = int.Parse(this.context.Request["shippingId"]);
				string text2 = this.context.Request["couponCode"];
				int num = this.context.Request["countDownId"].ToInt(0);
				bool flag = text == "countdown" && num > 0;
				if (!flag)
				{
					num = 0;
				}
				int num2 = this.context.Request["buyAmount"].ToInt(0);
				string text3 = Globals.UrlDecode(this.context.Request["productSku"]);
				string remark = DataHelper.CleanSearchString(Globals.UrlDecode(this.context.Request["remark"]));
				string text4 = DataHelper.CleanSearchString(Globals.UrlDecode(this.context.Request["formId"]));
				int num3 = this.context.Request["deductionPoints"].ToInt(0);
				bool flag2 = this.context.Request["needInvoice"].ToBool();
				int id = this.context.Request["InvoiceId"].ToInt(0);
				ShoppingCartInfo shoppingCartInfo = null;
				CountDownInfo countDownInfo = null;
				if (text == "undefined")
				{
					text = "";
				}
				if (text != "" && num2 <= 0)
				{
					this.context.Response.Write(this.GetErrorJson("参数异常"));
				}
				else if (!string.IsNullOrEmpty(text3) && !ProductHelper.ProductsIsAllOnSales(text3, 0))
				{
					this.context.Response.Write(this.GetErrorJson("订单中有商品已删除或已下架，请重新选择商品"));
				}
				else if (shoppingCartInfo != null && ((shoppingCartInfo.LineItems != null && (from a in shoppingCartInfo.LineItems
				where a.Quantity <= 0
				select a).Count() > 0) || (shoppingCartInfo.LineGifts != null && (from a in shoppingCartInfo.LineGifts
				where a.Quantity <= 0
				select a).Count() > 0)))
				{
					this.context.Response.Write(this.GetErrorJson("购买数量不合法"));
				}
				else
				{
					shoppingCartInfo = ShoppingCartProcessor.GetShoppingCart(text, text3, num2, 0, false, -1, 0);
					if (num2 <= 0)
					{
						num2 = 1;
					}
					if (shoppingCartInfo == null)
					{
						this.context.Response.Write(this.GetErrorJson("提交订单失败"));
					}
					else
					{
						this.FilterShoppingCart(shoppingCartInfo);
						if (shoppingCartInfo.LineItems == null || shoppingCartInfo.LineItems.Count <= 0)
						{
							this.context.Response.Write(this.GetErrorJson("提交订单失败"));
						}
						else
						{
							SiteSettings masterSettings = SettingsManager.GetMasterSettings();
							string text5 = "";
							if (flag)
							{
								countDownInfo = TradeHelper.ProductExistsCountDown(shoppingCartInfo.LineItems[0].ProductId, "", 0);
								if (countDownInfo == null)
								{
									this.context.Response.Write(this.GetErrorJson("抢购活动不存在"));
									goto end_IL_0001;
								}
								countDownInfo = TradeHelper.CheckUserCountDown(shoppingCartInfo.LineItems[0].ProductId, countDownInfo.CountDownId, shoppingCartInfo.LineItems[0].SkuId, HiContext.Current.UserId, num2, "", out text5, 0);
								if (countDownInfo == null)
								{
									this.context.Response.Write(this.GetErrorJson(text5));
									goto end_IL_0001;
								}
							}
							if (shoppingCartInfo == null || (shoppingCartInfo != null && (shoppingCartInfo.LineItems == null || shoppingCartInfo.LineItems.Count == 0) && (shoppingCartInfo.LineGifts == null || shoppingCartInfo.LineGifts.Count == 0)))
							{
								this.context.Response.Write(this.GetErrorJson("订单中有商品已删除或已下架，请重新选择商品"));
							}
							else if (!TradeHelper.CheckShoppingStock(shoppingCartInfo, out text5, 0))
							{
								this.context.Response.Write(this.GetErrorJson("订单中有商品(" + text5 + ")库存不足"));
							}
							else
							{
								OrderInfo orderInfo = ShoppingProcessor.ConvertShoppingCartToOrder(shoppingCartInfo, false, flag, 0);
								if (orderInfo == null || shoppingCartInfo.LineItems.Count <= 0)
								{
									this.context.Response.Write(this.GetErrorJson("订单中有商品已删除或已下架，请重新选择商品"));
								}
								else if (orderInfo.GetTotal(false) < decimal.Zero)
								{
									this.context.Response.Write(this.GetErrorJson("订单金额错误"));
								}
								else
								{
									orderInfo.OrderId = OrderIDFactory.GenerateOrderId();
									orderInfo.ParentOrderId = "0";
									orderInfo.OrderDate = DateTime.Now;
									orderInfo.OrderSource = OrderSource.Applet;
									MemberInfo user = HiContext.Current.User;
									orderInfo.UserId = user.UserId;
									orderInfo.Username = user.UserName;
									orderInfo.EmailAddress = user.Email;
									orderInfo.RealName = user.RealName;
									orderInfo.QQ = user.QQ;
									orderInfo.Remark = remark;
									orderInfo.Tax = decimal.Zero;
									if (flag)
									{
										orderInfo.CountDownBuyId = countDownInfo.CountDownId;
									}
									orderInfo.OrderStatus = OrderStatus.WaitBuyerPay;
									orderInfo.ShipToDate = "任意时间";
									ShippingAddressInfo shippingAddress = MemberProcessor.GetShippingAddress(shippingId);
									if (shippingAddress != null)
									{
										orderInfo.ShippingRegion = RegionHelper.GetFullRegion(shippingAddress.RegionId, "，", true, 0);
										orderInfo.RegionId = shippingAddress.RegionId;
										orderInfo.FullRegionPath = RegionHelper.GetFullPath(orderInfo.RegionId, true);
										orderInfo.Address = shippingAddress.RegionLocation + shippingAddress.Address + shippingAddress.BuildingNumber;
										orderInfo.ZipCode = "";
										orderInfo.ShipTo = shippingAddress.ShipTo;
										orderInfo.TelPhone = shippingAddress.TelPhone;
										orderInfo.CellPhone = shippingAddress.CellPhone;
										orderInfo.ShippingId = shippingAddress.ShippingId;
										orderInfo.LatLng = shippingAddress.LatLng;
										MemberProcessor.SetDefaultShippingAddress(shippingId, HiContext.Current.UserId);
									}
									if (shippingAddress == null || orderInfo.RegionId == 0 || string.IsNullOrEmpty(orderInfo.ShippingRegion))
									{
										this.context.Response.Write(this.GetErrorJson("错误的收货地址"));
									}
									else
									{
										orderInfo.ShippingModeId = 0;
										orderInfo.ModeName = "快递配送";
										orderInfo.StoreId = 0;
										if (!shoppingCartInfo.IsFreightFree)
										{
											OrderInfo orderInfo2 = orderInfo;
											OrderInfo orderInfo3 = orderInfo;
											decimal num6 = orderInfo2.AdjustedFreight = (orderInfo3.Freight = ShoppingProcessor.CalcFreight(orderInfo.RegionId, shoppingCartInfo));
										}
										else
										{
											OrderInfo orderInfo4 = orderInfo;
											OrderInfo orderInfo5 = orderInfo;
											decimal num6 = orderInfo4.AdjustedFreight = (orderInfo5.Freight = default(decimal));
										}
										orderInfo.PaymentTypeId = 0;
										orderInfo.PaymentType = "在线支付";
										orderInfo.Gateway = "";
										string text6 = "";
										if (!string.IsNullOrEmpty(text2))
										{
											CouponItemInfo userCouponInfo = ShoppingProcessor.GetUserCouponInfo(shoppingCartInfo.GetTotal(false), text2);
											if (userCouponInfo != null && string.IsNullOrEmpty(userCouponInfo.OrderId) && !userCouponInfo.UsedTime.HasValue && HiContext.Current.UserId != 0 && userCouponInfo.UserId == HiContext.Current.UserId && (orderInfo.CountDownBuyId == 0 || (orderInfo.CountDownBuyId > 0 && userCouponInfo.UseWithPanicBuying.Value)))
											{
												orderInfo.CouponName = userCouponInfo.CouponName;
												if (userCouponInfo.OrderUseLimit.HasValue)
												{
													orderInfo.CouponAmount = userCouponInfo.OrderUseLimit.Value;
												}
												orderInfo.CouponCode = text2;
												if (userCouponInfo.Price.Value >= orderInfo.GetAmount(false))
												{
													orderInfo.CouponValue = orderInfo.GetAmount(false);
												}
												else
												{
													orderInfo.CouponValue = userCouponInfo.Price.Value;
												}
												text6 = userCouponInfo.CanUseProducts;
											}
										}
										if (num3 > 0 && (masterSettings.CanPointUseWithCoupon || (!masterSettings.CanPointUseWithCoupon && string.IsNullOrEmpty(orderInfo.CouponCode))) && masterSettings.ShoppingDeduction > 0)
										{
											int shoppingDeductionRatio = masterSettings.ShoppingDeductionRatio;
											decimal value = (decimal)shoppingDeductionRatio * (orderInfo.GetTotal(false) - orderInfo.AdjustedFreight - orderInfo.Tax) * (decimal)masterSettings.ShoppingDeduction / 100m;
											if (user != null)
											{
												int num9 = (user.Points > (int)value) ? ((int)value) : user.Points;
												if (num3 > num9)
												{
													num3 = num9;
												}
												decimal value2 = ((decimal)num3 / (decimal)masterSettings.ShoppingDeduction).F2ToString("f2").ToDecimal(0);
												orderInfo.DeductionPoints = num3;
												orderInfo.DeductionMoney = value2;
											}
										}
										if (flag2)
										{
											UserInvoiceDataInfo userInvoiceDataInfo = MemberProcessor.GetUserInvoiceDataInfo(id);
											InvoiceType invoiceType = InvoiceType.Personal;
											if (userInvoiceDataInfo == null)
											{
												userInvoiceDataInfo = new UserInvoiceDataInfo
												{
													Id = 0,
													InvoiceType = InvoiceType.Personal,
													InvoiceTitle = "个人",
													LastUseTime = DateTime.Now
												};
												invoiceType = InvoiceType.Personal;
											}
											else
											{
												invoiceType = userInvoiceDataInfo.InvoiceType;
											}
											if (invoiceType == InvoiceType.VATInvoice && masterSettings.VATTaxRate > decimal.Zero && masterSettings.EnableVATInvoice)
											{
												orderInfo.Tax = ((orderInfo.GetTotal(false) - orderInfo.AdjustedFreight) * masterSettings.VATTaxRate / 100m).F2ToString("f2").ToDecimal(0);
											}
											else if (masterSettings.TaxRate > decimal.Zero && (masterSettings.EnableTax || masterSettings.EnableE_Invoice))
											{
												orderInfo.Tax = ((orderInfo.GetTotal(false) - orderInfo.AdjustedFreight) * masterSettings.TaxRate / 100m).F2ToString("f2").ToDecimal(0);
											}
											else
											{
												orderInfo.Tax = decimal.Zero;
											}
											orderInfo.InvoiceTitle = userInvoiceDataInfo.InvoiceTitle;
											orderInfo.InvoiceType = userInvoiceDataInfo.InvoiceType;
											if (orderInfo.InvoiceType == InvoiceType.Enterprise || orderInfo.InvoiceType == InvoiceType.Enterprise_Electronic || orderInfo.InvoiceType == InvoiceType.VATInvoice)
											{
												orderInfo.InvoiceTaxpayerNumber = userInvoiceDataInfo.InvoiceTaxpayerNumber;
											}
											orderInfo.InvoiceData = JsonHelper.GetJson(userInvoiceDataInfo);
										}
										if (HiContext.Current.UserId != 0)
										{
											orderInfo.Points = orderInfo.GetPoint(masterSettings.PointsRate);
										}
										else
										{
											orderInfo.Points = 0;
										}
										string orderId = orderInfo.OrderId;
										var list = (from i in shoppingCartInfo.LineItems
										select new
										{
											i.SupplierId,
											i.SupplierName
										}).Distinct().ToList();
										if ((from i in list
										where i.SupplierId == 0
										select i).Count() <= 0 && shoppingCartInfo.LineGifts.Count() > 0)
										{
											list.Add(new
											{
												SupplierId = 0,
												SupplierName = "平台"
											});
										}
										list = (from i in list
										orderby i.SupplierId
										select i).ToList();
										List<OrderInfo> list2 = new List<OrderInfo>();
										if (masterSettings.OpenSupplier)
										{
											if (list.Count() > 1)
											{
												string orderId2 = orderInfo.OrderId;
												orderInfo.ParentOrderId = "-1";
												orderInfo.OrderId = "P" + orderId2;
												orderInfo.StoreId = 0;
												decimal num10 = orderInfo.ReducedPromotionAmount;
												decimal num11 = orderInfo.CouponValue;
												int num12 = orderInfo.DeductionPoints.HasValue ? orderInfo.DeductionPoints.Value : 0;
												decimal num13 = (!orderInfo.DeductionMoney.HasValue) ? decimal.Zero : orderInfo.DeductionMoney.Value;
												int num14 = orderInfo.Points;
												decimal num15 = orderInfo.BalanceAmount;
												string arg = orderId2;
												int num16 = 0;
												for (int j = 0; j < list.Count; j++)
												{
													if (j >= 100 && j % 100 == 0)
													{
														arg = OrderIDFactory.GenerateOrderId();
														num16 = 0;
													}
													var anon = list[j];
													OrderInfo orderInfo6 = new OrderInfo();
													ShoppingProcessor.CreateDetailOrderInfo(orderInfo, orderInfo6);
													orderInfo6.OrderId = arg + num16;
													orderInfo6.ParentOrderId = orderInfo.OrderId;
													orderInfo6.SupplierId = anon.SupplierId;
													orderInfo6.ShipperName = anon.SupplierName;
													ShoppingProcessor.BindDetailOrderItemsAndGifts(orderInfo6, shoppingCartInfo, anon.SupplierId);
													if (anon.SupplierId > 0)
													{
														orderInfo6.Tax = decimal.Zero;
														orderInfo6.InvoiceTitle = "";
														orderInfo6.InvoiceTaxpayerNumber = "";
													}
													if (orderInfo6.LineItems.Count <= 0)
													{
														OrderInfo orderInfo7 = orderInfo6;
														OrderInfo orderInfo8 = orderInfo6;
														OrderInfo orderInfo9 = orderInfo6;
														OrderInfo orderInfo10 = orderInfo6;
														OrderInfo orderInfo11 = orderInfo6;
														int num18 = orderInfo11.PreSaleId = 0;
														int num20 = orderInfo10.GroupBuyId = num18;
														int num22 = orderInfo9.FightGroupId = num20;
														int num25 = orderInfo7.CountDownBuyId = (orderInfo8.FightGroupActivityId = num22);
														orderInfo6.Deposit = decimal.Zero;
														orderInfo6.FinalPayment = decimal.Zero;
													}
													decimal d = (orderInfo.GetAmount(false) == decimal.Zero) ? decimal.Zero : (orderInfo6.GetAmount(false) / orderInfo.GetAmount(false));
													if (orderInfo.IsReduced)
													{
														if (j == list.Count - 1)
														{
															orderInfo6.ReducedPromotionAmount = ((num10 < decimal.Zero) ? decimal.Zero : num10);
														}
														else
														{
															decimal d2 = orderInfo6.ReducedPromotionAmount = (d * orderInfo.ReducedPromotionAmount).F2ToString("f2").ToDecimal(0);
															num10 -= d2;
														}
													}
													if (orderInfo.CouponValue > decimal.Zero)
													{
														if (j == list.Count - 1)
														{
															orderInfo6.CouponValue = ((num11 < decimal.Zero) ? decimal.Zero : num11);
														}
														else
														{
															decimal num27 = default(decimal);
															num27 = (orderInfo6.CouponValue = (d * orderInfo.CouponValue).F2ToString("f2").ToDecimal(0));
															num11 -= num27;
														}
													}
													orderInfo6.Freight = ShoppingProcessor.CalcSupplierFreight(anon.SupplierId, orderInfo.RegionId, shoppingCartInfo);
													if (!orderInfo.IsFreightFree)
													{
														orderInfo6.AdjustedFreight = orderInfo6.Freight;
													}
													decimal? deductionMoney = orderInfo.DeductionMoney;
													if (deductionMoney.GetValueOrDefault() > default(decimal) && deductionMoney.HasValue)
													{
														if (j == list.Count - 1)
														{
															orderInfo6.DeductionPoints = ((num12 >= 0) ? num12 : 0);
															orderInfo6.DeductionMoney = ((num13 < decimal.Zero) ? decimal.Zero : num13);
														}
														else
														{
															decimal d3 = orderInfo6.GetAmount(false) - orderInfo6.ReducedPromotionAmount - orderInfo6.CouponValue;
															decimal d4 = orderInfo.GetAmount(false) - orderInfo.ReducedPromotionAmount - orderInfo.CouponValue;
															decimal num29 = (d3 == decimal.Zero) ? decimal.Zero : (d3 / d4);
															decimal num6 = num29;
															decimal? d5 = (decimal?)orderInfo.DeductionPoints;
															int num30 = (int)((decimal?)num6 * d5).Value;
															if (HiContext.Current.User.Points < num30)
															{
																num30 = HiContext.Current.User.Points;
															}
															decimal num31 = ((decimal)num30 / (decimal)masterSettings.ShoppingDeduction).F2ToString("f2").ToDecimal(0);
															if (orderInfo.GetTotal(true) == decimal.Zero)
															{
																decimal total = orderInfo6.GetTotal(true);
																if (total > decimal.Zero)
																{
																	num31 -= total;
																	num30 = (int)(num31 * (decimal)masterSettings.ShoppingDeduction);
																}
															}
															if (num31 < decimal.Zero)
															{
																num31 = default(decimal);
															}
															if (num30 < 0)
															{
																num30 = 0;
															}
															orderInfo6.DeductionPoints = num30;
															orderInfo6.DeductionMoney = num31;
															num12 -= num30;
															num13 -= num31;
														}
													}
													if (orderInfo.Points > 0)
													{
														if (j == list.Count - 1)
														{
															orderInfo6.Points = num14;
														}
														else
														{
															int num32 = orderInfo6.Points = orderInfo6.GetPoint(HiContext.Current.SiteSettings.PointsRate);
															num14 -= num32;
														}
													}
													if (orderInfo.BalanceAmount > decimal.Zero)
													{
														if (j == list.Count - 1)
														{
															orderInfo6.BalanceAmount = num15;
														}
														else
														{
															decimal d6 = orderInfo6.BalanceAmount = (d * orderInfo.BalanceAmount).F2ToString("f2").ToDecimal(0);
															num15 -= d6;
														}
													}
													orderInfo6.ExchangePoints = orderInfo6.GetTotalNeedPoint();
													if (anon.SupplierId == 0)
													{
														orderId = orderInfo6.OrderId;
													}
													num16++;
													list2.Add(orderInfo6);
												}
											}
											else
											{
												var anon2 = list[0];
												orderInfo.SupplierId = anon2.SupplierId;
												orderInfo.ShipperName = anon2.SupplierName;
											}
										}
										list2.Add(orderInfo);
										if (!ShoppingProcessor.CreatOrder(list2))
										{
											this.context.Response.Write(this.GetErrorJson("提交订单失败"));
										}
										else
										{
											if (!string.IsNullOrEmpty(text4))
											{
												WeChartAppletHelper.AddFormData(WXAppletEvent.CreateOrder, orderInfo.OrderId, text4);
											}
											TransactionAnalysisHelper.AnalysisOrderTranData(orderInfo);
											if (orderInfo.GetTotal(false) <= decimal.Zero && orderInfo.CheckAction(OrderActions.BUYER_PAY))
											{
												TradeHelper.UserPayOrder(orderInfo, false, false);
												orderInfo.OnPayment();
											}
											Messenger.OrderCreated(orderInfo, HiContext.Current.User);
											if (text != "signbuy" && text != "groupbuy" && text != "combinationbuy" && text != "presale" && text != "prize")
											{
												foreach (ShoppingCartItemInfo lineItem in shoppingCartInfo.LineItems)
												{
													ShoppingCartProcessor.RemoveLineItem(lineItem.SkuId, 0);
												}
												foreach (ShoppingCartGiftInfo lineGift in shoppingCartInfo.LineGifts)
												{
													ShoppingCartProcessor.RemoveGiftItem(lineGift.GiftId, (PromoteType)lineGift.PromoType);
												}
											}
											string s = JsonConvert.SerializeObject(new
											{
												Status = "OK",
												Message = "订单提交成功",
												OrderId = orderInfo.OrderId,
												OrderTotal = orderInfo.GetTotal(false)
											});
											this.context.Response.Write(s);
										}
									}
								}
							}
						}
					}
				}
				end_IL_0001:;
			}
			catch (Exception ex)
			{
				Globals.WriteExceptionLog(ex, null, "AppletSumbitOrder");
				this.context.Response.Write(this.GetErrorJson("系统错误"));
			}
		}

		public void GetShoppingCart()
		{
			this.CheckOpenId();
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			bool flag = true;
			string text = this.context.Request["productSku"];
			string text2 = this.context.Request["fromPage"];
			int num = this.context.Request["countDownId"].ToInt(0);
			int buyAmount = this.context.Request["buyAmount"].ToInt(0);
			int shipAddressId = this.context.Request["shipAddressId"].ToInt(0);
			if (string.IsNullOrEmpty(text))
			{
				this.context.Response.Write(this.GetErrorJson("参数错误"));
			}
			else
			{
				ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart(text2, text, buyAmount, 0, false, 0, 0);
				if (shoppingCart == null)
				{
					this.context.Response.Write(this.GetErrorJson("购物车为空"));
				}
				else
				{
					this.FilterShoppingCart(shoppingCart);
					if (shoppingCart.LineItems == null || shoppingCart.LineItems.Count <= 0)
					{
						this.context.Response.Write(this.GetErrorJson("购物车为空"));
					}
					else
					{
						var source = (from i in (from i in shoppingCart.LineItems
						select new
						{
							i.SupplierId,
							i.SupplierName
						}).Distinct()
						orderby i.SupplierId
						select i).ToList();
						string text3 = "";
						if (text2 == "countdown")
						{
							CountDownInfo countDownInfo = TradeHelper.ProductExistsCountDown(shoppingCart.LineItems[0].ProductId, "", 0);
							if (countDownInfo == null)
							{
								this.context.Response.Write(this.GetErrorJson("抢购活动不存在"));
								return;
							}
							countDownInfo = TradeHelper.CheckUserCountDown(shoppingCart.LineItems[0].ProductId, countDownInfo.CountDownId, shoppingCart.LineItems[0].SkuId, HiContext.Current.UserId, buyAmount, "", out text3, 0);
							if (countDownInfo == null)
							{
								this.context.Response.Write(this.GetErrorJson(text3));
								return;
							}
							flag = false;
						}
						if (!TradeHelper.CheckShoppingStock(shoppingCart, out text3, 0))
						{
							this.context.Response.Write(this.GetErrorJson("订单中有商品(" + text3 + ")库存不足"));
						}
						else
						{
							decimal orderFreight = default(decimal);
							int shipRegionId = 0;
							IList<ShippingAddressInfo> shippingAddresses = MemberProcessor.GetShippingAddresses(false);
							ShippingAddressInfo shippingAddressInfo = null;
							if (shippingAddresses != null && shippingAddresses.Count > 0)
							{
								shippingAddressInfo = ((shipAddressId <= 0) ? (from a in shippingAddresses
								orderby a.IsDefault descending
								select a).FirstOrDefault() : shippingAddresses.FirstOrDefault((ShippingAddressInfo a) => a.ShippingId == shipAddressId));
								shipRegionId = shippingAddressInfo.RegionId;
								if (!shoppingCart.IsFreightFree && shippingAddressInfo != null)
								{
									orderFreight = ShoppingProcessor.CalcFreight(shippingAddressInfo.RegionId, shoppingCart);
								}
							}
							if (shoppingCart.IsFreightFree)
							{
								orderFreight = default(decimal);
							}
							string obj = "";
							string defaultCouponCode = "";
							DataTable dataTable = null;
							if (shoppingCart.GetTotal(false) > decimal.Zero)
							{
								try
								{
									dataTable = ShoppingProcessor.GetCoupon(shoppingCart.GetTotal(false), shoppingCart.LineItems, false, num > 0, false);
									if (dataTable != null && dataTable.Rows.Count > 0)
									{
										dataTable.Columns.Add("LimitText", typeof(string));
										dataTable.Columns.Add("CanUseProduct", typeof(string));
										dataTable.Columns.Add("StartTimeText", typeof(string));
										dataTable.Columns.Add("ClosingTimeText", typeof(string));
										foreach (DataRow row in dataTable.Rows)
										{
											row["LimitText"] = ((row["OrderUseLimit"].ToDecimal(0) > decimal.Zero) ? ("订单满" + string.Format("{0:f2}", row["OrderUseLimit"].ToDecimal(0)) + "元可用") : "订单金额无限制");
											row["CanUseProduct"] = (string.IsNullOrEmpty(row["CanUseProducts"].ToNullString()) ? "全场通用" : "部分商品可用");
											DataRow dataRow2 = row;
											DateTime value = row["StartTime"].ToDateTime().Value;
											dataRow2["StartTimeText"] = value.ToString("yyyy.MM.dd");
											DataRow dataRow3 = row;
											value = row["ClosingTime"].ToDateTime().Value;
											dataRow3["ClosingTimeText"] = value.ToString("yyyy.MM.dd");
											row["Price"] = decimal.Parse(row["Price"].ToString()).F2ToString("f2");
										}
									}
								}
								catch (Exception ex)
								{
									Globals.WriteExceptionLog(ex, null, "GetShoppingCart");
								}
							}
							if (dataTable != null && dataTable.Rows.Count > 0)
							{
								CouponItemInfo userCouponInfo = ShoppingProcessor.GetUserCouponInfo(shoppingCart.GetTotal(false), dataTable.Rows[0]["ClaimCode"].ToString());
								if (userCouponInfo != null)
								{
									obj = (userCouponInfo.Price.HasValue ? userCouponInfo.Price.Value.F2ToString("f2") : "0");
									defaultCouponCode = userCouponInfo.ClaimCode;
								}
							}
							if (shoppingCart.LineItems != null && shoppingCart.LineItems.Count > 0)
							{
								for (int j = 0; j < shoppingCart.LineItems.Count; j++)
								{
									shoppingCart.LineItems[j].MemberPrice = shoppingCart.LineItems[j].MemberPrice.F2ToString("f2").ToDecimal(0);
									shoppingCart.LineItems[j].AdjustedPrice = shoppingCart.LineItems[j].AdjustedPrice.F2ToString("f2").ToDecimal(0);
									shoppingCart.LineItems[j].ThumbnailUrl100 = this.GetProductImageFullPath(shoppingCart.LineItems[j].ThumbnailUrl180);
									shoppingCart.LineItems[j].ThumbnailUrl180 = this.GetProductImageFullPath(shoppingCart.LineItems[j].ThumbnailUrl180);
									shoppingCart.LineItems[j].ThumbnailUrl40 = this.GetProductImageFullPath(shoppingCart.LineItems[j].ThumbnailUrl40);
									shoppingCart.LineItems[j].ThumbnailUrl60 = this.GetProductImageFullPath(shoppingCart.LineItems[j].ThumbnailUrl60);
								}
							}
							int maxUsePoint = 0;
							decimal maxPointDiscount = 0.0m;
							int shoppingDeduction = 0;
							bool canPointUseWithCoupon = false;
							int pointDeductionRate = 0;
							int myPoints = 0;
							if (flag)
							{
								flag = false;
								MemberInfo user = Users.GetUser(HiContext.Current.UserId);
								if (masterSettings.ShoppingDeduction > 0 && user.Points > 0)
								{
									int shoppingDeductionRatio = masterSettings.ShoppingDeductionRatio;
									decimal num2 = (decimal)shoppingDeductionRatio * shoppingCart.GetTotal(false) * (decimal)masterSettings.ShoppingDeduction / 100m;
									int num3 = ((decimal)user.Points > num2) ? ((int)num2) : user.Points;
									decimal d = ((decimal)num3 / (decimal)masterSettings.ShoppingDeduction).F2ToString("f2").ToDecimal(0);
									if (d > decimal.Zero && num3 > 0)
									{
										shoppingDeduction = masterSettings.ShoppingDeduction;
										maxUsePoint = num3;
										maxPointDiscount = (decimal)num3 / (decimal)masterSettings.ShoppingDeduction;
										canPointUseWithCoupon = masterSettings.CanPointUseWithCoupon;
										pointDeductionRate = shoppingDeductionRatio;
										myPoints = user.Points;
										flag = true;
									}
								}
							}
							IList<UserInvoiceDataInfo> userInvoiceDataList = MemberProcessor.GetUserInvoiceDataList(HiContext.Current.UserId, shippingAddressInfo);
							string s2 = JsonConvert.SerializeObject(new
							{
								Status = "OK",
								Data = new
								{
									Suppliers = from s in source
									select new
									{
										SupplierId = s.SupplierId,
										SupplierName = s.SupplierName,
										SupplierTotal = (from c in shoppingCart.LineItems
										where c.SupplierId == s.SupplierId
										select c).Sum((ShoppingCartItemInfo ci) => ci.SubTotal),
										Feright = ShoppingProcessor.CalcSupplierFreight(s.SupplierId, shipRegionId, shoppingCart),
										ProductItems = (from c in shoppingCart.LineItems
										where c.SupplierId == s.SupplierId
										select c).ToList()
									},
									TotalPrice = shoppingCart.StrTotalAmount.ToDecimal(0),
									ProductAmount = shoppingCart.StrAmount.ToDecimal(0),
									OrderFreight = orderFreight,
									FullDiscount = shoppingCart.ReducedPromotionAmount.F2ToString("f2").ToDecimal(0),
									FullFreeFreight = shoppingCart.IsFreightFree,
									FullSendPoint = shoppingCart.IsSendTimesPoint,
									IsFreightFree = shoppingCart.IsFreightFree.ToString(),
									FullSendPointTimes = shoppingCart.TimesPoint,
									DefaultCouponPrice = obj.ToDecimal(0),
									DefaultCouponCode = defaultCouponCode,
									CouponList = dataTable,
									DefaultShippingAddress = shippingAddressInfo,
									MaxUsePoint = maxUsePoint,
									MaxPointDiscount = maxPointDiscount,
									ShoppingDeduction = shoppingDeduction,
									CanPointUseWithCoupon = canPointUseWithCoupon,
									PointDeductionRate = pointDeductionRate,
									ProductItems = shoppingCart.LineItems,
									MyPoints = myPoints,
									EnableTax = masterSettings.EnableTax,
									EnableVATInvoice = masterSettings.EnableVATInvoice,
									EnableE_Invoice = masterSettings.EnableE_Invoice,
									TaxRate = masterSettings.TaxRate,
									VATTaxRate = masterSettings.VATTaxRate,
									EndOrderDays = masterSettings.EndOrderDays,
									VATInvoiceDays = masterSettings.EndOrderDays + masterSettings.VATInvoiceDays,
									InvoiceList = from i in userInvoiceDataList
									select new
									{
										Id = i.Id,
										InvoiceType = i.InvoiceType,
										InvoiceTitle = i.InvoiceTitle.ToNullString(),
										InvoiceTaxpayerNumber = i.InvoiceTaxpayerNumber.ToNullString(),
										OpenBank = i.OpenBank.ToNullString(),
										BankAccount = i.BankAccount.ToNullString(),
										ReceiveAddress = i.ReceiveAddress.ToNullString(),
										ReceiveEmail = i.ReceiveEmail.ToNullString(),
										ReceiveName = i.ReceiveName.ToNullString(),
										ReceivePhone = i.ReceivePhone.ToNullString(),
										ReceiveRegionId = i.ReceiveRegionId,
										ReceiveRegionName = i.ReceiveRegionName.ToNullString(),
										RegisterAddress = i.RegisterAddress.ToNullString(),
										RegisterTel = i.RegisterTel.ToNullString()
									}
								}
							});
							this.context.Response.Write(s2);
						}
					}
				}
			}
		}

		public void GetPayParam()
		{
			this.CheckOpenId();
			string text = this.context.Request["orderId"].ToNullString();
			if (string.IsNullOrEmpty(text))
			{
				this.context.Response.Write(this.GetErrorJson("参数错误"));
			}
			else
			{
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(text);
				if (orderInfo == null)
				{
					this.context.Response.Write(this.GetErrorJson("订单错误"));
				}
				else
				{
					orderInfo.Gateway = "hishop.plugins.payment.wxappletpay";
					orderInfo.PaymentType = "小程序微信支付";
					orderInfo.PaymentTypeId = -9;
					TradeHelper.UpdateOrderPaymentType(orderInfo);
					decimal d = default(decimal);
					if (orderInfo.PreSaleId > 0)
					{
						if (!orderInfo.DepositDate.HasValue && orderInfo.OrderStatus == OrderStatus.WaitBuyerPay)
						{
							d = orderInfo.Deposit;
						}
						else if (orderInfo.DepositDate.HasValue && orderInfo.OrderStatus == OrderStatus.WaitBuyerPay)
						{
							if (orderInfo.PayRandCode.ToInt(0) == 0)
							{
								int num = orderInfo.PayRandCode.ToInt(0);
								num = ((num >= 100) ? (num + 1) : 100);
								orderInfo.PayRandCode = num.ToString();
								OrderHelper.UpdateOrderPaymentTypeOfAPI(orderInfo);
							}
							d = orderInfo.FinalPayment;
						}
					}
					else
					{
						d = orderInfo.GetTotal(false);
					}
					if (orderInfo.UserId != HiContext.Current.UserId)
					{
						this.context.Response.Write(this.GetErrorJson("订单错误"));
					}
					else
					{
						string str = "";
						string text2 = this.context.Request["openId"].ToNullString();
						switch (TradeHelper.CheckOrderBeforePay(orderInfo, out str))
						{
						case 0:
						{
							SiteSettings siteSettings = HiContext.Current.SiteSettings;
							if (siteSettings.OpenWxAppletWxPay && !string.IsNullOrEmpty(siteSettings.WxAppletAppId) && !string.IsNullOrEmpty(siteSettings.WxApplectMchId) && !string.IsNullOrEmpty(siteSettings.WxApplectKey))
							{
								try
								{
									PackageInfo packageInfo = new PackageInfo();
									packageInfo.Attach = "";
									packageInfo.Body = text;
									packageInfo.NotifyUrl = Globals.HttpsFullPath("/pay/wxapplet_Pay");
									if (orderInfo.PreSaleId > 0 && !orderInfo.DepositDate.HasValue && orderInfo.OrderStatus == OrderStatus.WaitBuyerPay)
									{
										packageInfo.OutTradeNo = text;
									}
									else
									{
										packageInfo.OutTradeNo = orderInfo.PayOrderId;
									}
									packageInfo.TotalFee = (int)(d * 100m);
									if (packageInfo.TotalFee < decimal.One)
									{
										packageInfo.TotalFee = decimal.One;
									}
									if (string.IsNullOrEmpty(text2) && HiContext.Current.UserId > 0)
									{
										Users.ClearUserCache(HiContext.Current.UserId, "");
										MemberInfo user = Users.GetUser(HiContext.Current.UserId);
										if (user.MemberOpenIds != null && user.MemberOpenIds.Count() > 0)
										{
											MemberOpenIdInfo memberOpenIdInfo = user.MemberOpenIds.FirstOrDefault((MemberOpenIdInfo item) => item.OpenIdType.ToLower() == "hishop.plugins.openid.wxapplet");
											if (memberOpenIdInfo != null)
											{
												text2 = memberOpenIdInfo.OpenId;
											}
										}
									}
									packageInfo.OpenId = text2;
									PayClient payClient = new PayClient(siteSettings.WxAppletAppId, siteSettings.WxAppletAppSecrect, siteSettings.WxApplectMchId, siteSettings.WxApplectKey, "", "", "", "");
									PayRequestInfo payRequestInfo = payClient.BuildPayRequest(packageInfo);
									if (payRequestInfo != null && !string.IsNullOrEmpty(payRequestInfo.prepayid))
									{
										WeChartAppletHelper.AddFormData(WXAppletEvent.Pay, orderInfo.OrderId, payRequestInfo.prepayid);
									}
									string s = JsonConvert.SerializeObject(new
									{
										Status = "OK",
										Data = new
										{
											prepayId = payRequestInfo.prepayid,
											nonceStr = payRequestInfo.nonceStr,
											timeStamp = payRequestInfo.timeStamp,
											sign = payRequestInfo.paySign
										}
									});
									this.context.Response.Write(s);
								}
								catch (Exception ex)
								{
									IDictionary<string, string> dictionary = new Dictionary<string, string>();
									dictionary.Add("OpenId", text2);
									dictionary.Add("UserId", HiContext.Current.UserId.ToString());
									Globals.WriteExceptionLog(ex, dictionary, "GetPayParam");
									this.context.Response.Write(this.GetErrorJson("微信支付配置错误"));
								}
							}
							else
							{
								this.context.Response.Write(this.GetErrorJson("未开通微信支付"));
							}
							break;
						}
						case 1:
							TradeHelper.CloseOrder(orderInfo.OrderId, "订单中有商品(" + str + ")规格被删除或者下架");
							this.context.Response.Write(this.GetErrorJson("订单中有商品(" + str + ")规格被删除或者下架"));
							break;
						case 2:
							this.context.Response.Write(this.GetErrorJson("订单中有商品(" + str + ")库存不足"));
							break;
						default:
							this.context.Response.Write(this.GetErrorJson("系统错误"));
							break;
						}
					}
				}
			}
		}

		private void GetLogistic()
		{
			string text = this.context.Request["orderId"].ToNullString();
			if (string.IsNullOrEmpty(text))
			{
				this.context.Response.Write(this.GetErrorJson("参数错误"));
			}
			else
			{
				string text2 = "";
				OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(text);
				if (orderInfo != null && (orderInfo.OrderStatus == OrderStatus.SellerAlreadySent || orderInfo.OrderStatus == OrderStatus.Finished) && !string.IsNullOrEmpty(orderInfo.ExpressCompanyAbb))
				{
					text2 = ExpressHelper.GetDataByKuaidi100(orderInfo.ExpressCompanyAbb, orderInfo.ShipOrderNumber);
					string text3 = "";
					text3 = ((string.IsNullOrEmpty(text2) || !(text2 != "此单无物流信息")) ? JsonConvert.SerializeObject(new
					{
						Status = "NO",
						Data = "[]"
					}) : JsonConvert.SerializeObject(new
					{
						Status = "OK",
						Data = new
						{
							LogisticsData = text2,
							ExpressCompanyName = orderInfo.ExpressCompanyName,
							ShipOrderNumber = orderInfo.ShipOrderNumber,
							ShipTo = orderInfo.ShipTo,
							CellPhone = orderInfo.CellPhone,
							Address = orderInfo.ShippingRegion.Replace("，", ",").Replace(",", "") + orderInfo.Address
						}
					}));
					this.context.Response.Write(text3);
				}
				else
				{
					this.context.Response.Write(this.GetErrorJson("暂无物流信息"));
				}
			}
		}

		private void FinishOrder()
		{
			string text = this.context.Request["orderId"].ToNullString();
			try
			{
				this.CheckOpenId();
				if (string.IsNullOrEmpty(text))
				{
					this.context.Response.Write(this.GetErrorJson("参数错误"));
				}
				else
				{
					OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(text);
					if (orderInfo != null && TradeHelper.ConfirmOrderFinish(orderInfo))
					{
						this.context.Response.Write(this.GetOKJson("确认收货成功"));
					}
					else
					{
						this.context.Response.Write(this.GetErrorJson("确认收货失败"));
					}
				}
			}
			catch (Exception ex)
			{
				Globals.WriteLog("ConfirmOrderFinish.txt", text + "|" + ex.Message);
				this.context.Response.Write(this.GetErrorJson("系统错误"));
			}
		}

		public void CloseOrder()
		{
			this.CheckOpenId();
			string text = this.context.Request["orderId"].ToNullString();
			if (string.IsNullOrEmpty(text))
			{
				this.context.Response.Write(this.GetErrorJson("参数错误"));
			}
			else
			{
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(text);
				if (orderInfo != null && orderInfo.ItemStatus == OrderItemStatus.Nomarl && TradeHelper.CloseOrder(orderInfo.OrderId, "会员主动关闭"))
				{
					if (orderInfo.ShippingModeId == -2 && orderInfo.IsConfirm)
					{
						OrderHelper.CloseDeportOrderReturnStock(orderInfo, "会员" + HiContext.Current.User.UserName + "关闭订单");
					}
					Messenger.OrderClosed(HiContext.Current.User, orderInfo, "用户自己关闭订单");
					this.context.Response.Write(this.GetOKJson("取消订单成功"));
				}
				else
				{
					this.context.Response.Write(this.GetErrorJson("取消订单失败"));
				}
			}
		}

		public void OrderList()
		{
			this.CheckOpenId();
			OrderStatus orderStatus = (OrderStatus)this.context.Request["status"].ToInt(0);
			int pageSize = this.context.Request["pageSize"].ToInt(0);
			int pageIndex = this.context.Request["pageIndex"].ToInt(0);
			if (orderStatus != 0 && orderStatus != OrderStatus.WaitBuyerPay && orderStatus != OrderStatus.SellerAlreadySent && orderStatus != OrderStatus.BuyerAlreadyPaid && orderStatus != OrderStatus.WaitReview)
			{
				this.context.Response.Write(this.GetErrorJson("参数错误"));
			}
			else
			{
				OrderQuery orderQuery = new OrderQuery();
				orderQuery.Status = orderStatus;
				orderQuery.ShowGiftOrder = true;
				orderQuery.PageIndex = pageIndex;
				orderQuery.PageSize = pageSize;
				orderQuery.IsServiceOrder = false;
				orderQuery.IsAllOrder = false;
				orderQuery.ShowGiftOrder = false;
				orderQuery.IsIncludePreSaleOrder = false;
				List<OrderInfo> pageListUserOrder = MemberProcessor.GetPageListUserOrder(HiContext.Current.UserId, orderQuery);
				DataTable dtWaitReviewOrderIds = TradeHelper.GetWaitReviewOrderIds(HiContext.Current.UserId, "");
				SiteSettings setting = SettingsManager.GetMasterSettings();
				string s = JsonConvert.SerializeObject(new
				{
					Status = "OK",
					Data = pageListUserOrder.Select(delegate(OrderInfo c)
					{
						string orderId = c.OrderId;
						int orderStatus2 = this.GetOrderStatus(c);
						int allQuantity = c.GetAllQuantity(true);
						decimal total = c.GetTotal(false);
						string orderStatusText = this.GetOrderStatusText(c);
						bool isShowClose = c.OrderStatus == OrderStatus.WaitBuyerPay && !c.DepositDate.HasValue;
						bool isShowFinishOrder = c.OrderStatus == OrderStatus.SellerAlreadySent && c.ItemStatus == OrderItemStatus.Nomarl;
						if (c.OrderStatus == OrderStatus.Finished && c.LineItems.Count > 0)
						{
							goto IL_0101;
						}
						if (c.OrderStatus == OrderStatus.Closed && c.OnlyReturnedCount == c.LineItems.Count && c.LineItems.Count > 0)
						{
							goto IL_0101;
						}
						int isShowPreview = 0;
						goto IL_0145;
						IL_01eb:
						int isShowCreview;
						return new
						{
							OrderId = orderId,
							Status = orderStatus2,
							Quantity = allQuantity,
							Amount = total,
							StatusText = orderStatusText,
							IsShowClose = isShowClose,
							IsShowFinishOrder = isShowFinishOrder,
							IsShowPreview = ((byte)isShowPreview != 0),
							IsShowCreview = ((byte)isShowCreview != 0),
							CreviewText = ((setting.ProductCommentPoint > 0) ? $"评价得{setting.ProductCommentPoint * c.LineItems.Count}积分" : "评价订单"),
							ProductCommentPoint = setting.ProductCommentPoint * c.LineItems.Count,
							IsShowRefund = (c.FightGroupId > 0 && VShopHelper.GetFightGroup(c.FightGroupId).Status == FightGroupStatus.FightGroupSuccess && c.OrderStatus == OrderStatus.BuyerAlreadyPaid && c.ItemStatus == OrderItemStatus.Nomarl && c.LineItems.Count != 0),
							IsShowReturn = false,
							IsShowTakeCodeQRCode = (!string.IsNullOrEmpty(c.TakeCode) && (c.OrderStatus == OrderStatus.BuyerAlreadyPaid || c.OrderStatus == OrderStatus.WaitBuyerPay)),
							IsShowLogistics = ((c.OrderStatus == OrderStatus.SellerAlreadySent || c.OrderStatus == OrderStatus.Finished) && !string.IsNullOrEmpty(c.ShipOrderNumber)),
							ShipOrderNumber = c.ShipOrderNumber.ToNullString(),
							OrderDate = c.OrderDate.ToString("yyyy-MM-dd HH:mm:ss"),
							SupplierId = c.SupplierId,
							ShipperName = ((c.ShipperName == null) ? "" : ((c.ShipperName.Length > 12) ? (c.ShipperName.Substring(0, 12) + "...") : c.ShipperName)),
							StoreName = ((c.StoreName == null) ? "" : ((c.StoreName.Length > 12) ? (c.StoreName.Substring(0, 12) + "...") : c.StoreName)),
							IsShowCertification = (setting.IsOpenCertification && c.IsincludeCrossBorderGoods && c.IDStatus == 0),
							IsShowDadalogistics = (c.ExpressCompanyName == "同城物流配送"),
							IsShowPay = (c.OrderStatus == OrderStatus.WaitBuyerPay && c.Gateway != "hishop.plugins.payment.payonstore" && c.Gateway != "hishop.plugins.payment.podrequest" && c.Gateway != "hishop.plugins.payment.bankrequest"),
							LineItems = from d in c.LineItems.Keys
							select new
							{
								Status = c.LineItems[d].Status,
								StatusText = ((c.LineItems[d].Status == LineItemStatus.Normal) ? "" : c.LineItems[d].StatusSimpleText),
								Id = c.LineItems[d].SkuId,
								Name = c.LineItems[d].ItemDescription,
								Price = c.LineItems[d].ItemAdjustedPrice.F2ToString("f2"),
								IsShowRefund = false,
								IsShowAfterSale = ((c.OrderStatus == OrderStatus.SellerAlreadySent || (c.OrderStatus == OrderStatus.Finished && !c.IsServiceOver)) && c.CountDownBuyId == 0 && c.GroupBuyId == 0 && (c.LineItems[d].ReturnInfo == null || c.LineItems[d].ReturnInfo.HandleStatus == ReturnStatus.Refused) && (c.LineItems[d].ReplaceInfo == null || c.LineItems[d].ReplaceInfo.HandleStatus == ReplaceStatus.Refused || c.LineItems[d].ReplaceInfo.HandleStatus == ReplaceStatus.Replaced)),
								Amount = c.LineItems[d].Quantity,
								ShipmentQuantity = c.LineItems[d].ShipmentQuantity,
								SendCount = ((c.LineItems[d].ShipmentQuantity > c.LineItems[d].Quantity) ? (c.LineItems[d].ShipmentQuantity - c.LineItems[d].Quantity) : 0),
								Image = Globals.HttpsFullPath(string.IsNullOrEmpty(c.LineItems[d].ThumbnailsUrl) ? setting.DefaultProductImage : c.LineItems[d].ThumbnailsUrl),
								SkuText = this.newSKUContent(c.LineItems[d].SKUContent)
							},
							Gifts = from g in c.Gifts
							select new
							{
								GiftId = g.GiftId,
								GiftName = g.GiftName,
								PromoteType = g.PromoteType,
								Quantity = g.Quantity,
								SkuId = g.SkuId,
								Image = Globals.HttpsFullPath(string.IsNullOrEmpty(g.ThumbnailsUrl) ? setting.DefaultProductImage : g.ThumbnailsUrl),
								CostPrice = g.CostPrice
							}
						};
						IL_0101:
						isShowPreview = ((dtWaitReviewOrderIds.Rows.Count <= 0 || dtWaitReviewOrderIds.Select("OrderId = '" + c.OrderId + "'").Count() <= 0) ? 1 : 0);
						goto IL_0145;
						IL_0145:
						if (c.OrderStatus == OrderStatus.Finished && c.LineItems.Count > 0)
						{
							goto IL_01a7;
						}
						if (c.OrderStatus == OrderStatus.Closed && c.OnlyReturnedCount == c.LineItems.Count && c.LineItems.Count > 0)
						{
							goto IL_01a7;
						}
						isShowCreview = 0;
						goto IL_01eb;
						IL_01a7:
						isShowCreview = ((dtWaitReviewOrderIds.Rows.Count > 0 && dtWaitReviewOrderIds.Select("OrderId = '" + c.OrderId + "'").Count() > 0) ? 1 : 0);
						goto IL_01eb;
					})
				});
				this.context.Response.Write(s);
			}
		}

		private void DelShippingAddress()
		{
			this.CheckOpenId();
			MemberInfo user = HiContext.Current.User;
			int shippingid = Convert.ToInt32(this.context.Request["shippingId"]);
			if (MemberProcessor.DelShippingAddress(shippingid, user.UserId))
			{
				this.context.Response.Write(this.GetOKJson("删除成功"));
			}
			else
			{
				this.context.Response.Write(this.GetErrorJson("删除失败"));
			}
		}

		private void SetDefaultShippingAddress()
		{
			this.CheckOpenId();
			MemberInfo user = HiContext.Current.User;
			int shippingId = Convert.ToInt32(this.context.Request["shippingId"]);
			if (MemberProcessor.SetDefaultShippingAddress(shippingId, user.UserId))
			{
				this.context.Response.Write(this.GetOKJson("设置成功"));
			}
			else
			{
				this.context.Response.Write(this.GetErrorJson("设置失败"));
			}
		}

		private void UpdateShippingAddress()
		{
			this.CheckOpenId();
			MemberInfo user = HiContext.Current.User;
			int num = this.context.Request["shippingId"].ToInt(0);
			if (num <= 0)
			{
				this.context.Response.Write(this.GetErrorJson("参数错误"));
			}
			else
			{
				ShippingAddressInfo shippingAddress = MemberProcessor.GetShippingAddress(num);
				shippingAddress.Address = Globals.StripAllTags(this.context.Request["address"].ToNullString());
				shippingAddress.CellPhone = Globals.StripAllTags(this.context.Request["cellphone"].ToNullString());
				shippingAddress.ShipTo = Globals.StripAllTags(this.context.Request["shipTo"].ToNullString());
				shippingAddress.Zipcode = "";
				shippingAddress.IsDefault = this.context.Request.Form["isDefault"].ToBool();
				shippingAddress.RegionId = this.context.Request.Form["regionId"].ToInt(0);
				shippingAddress.BuildingNumber = Globals.StripAllTags(this.context.Request["BuildingNumber"].ToNullString());
				shippingAddress.LatLng = this.context.Request["LatLng"].ToNullString();
				if (shippingAddress.RegionId <= 0 || string.IsNullOrEmpty(RegionHelper.GetFullRegion(shippingAddress.RegionId, " ", true, 0)))
				{
					this.context.Response.Write(this.GetErrorJson("错误的地区信息"));
				}
				else
				{
					shippingAddress.FullRegionPath = RegionHelper.GetFullPath(shippingAddress.RegionId, true);
					if (MemberProcessor.UpdateShippingAddress(shippingAddress))
					{
						this.context.Response.Write(this.GetOKJson(shippingAddress.ShippingId.ToString()));
					}
					else
					{
						this.context.Response.Write(this.GetErrorJson("系统错误"));
					}
				}
			}
		}

		private void AddShippingAddress()
		{
			this.CheckOpenId();
			MemberInfo user = HiContext.Current.User;
			SiteSettings siteSettings = HiContext.Current.SiteSettings;
			if (MemberProcessor.GetShippingAddressCount(user.UserId) >= HiContext.Current.SiteSettings.UserAddressMaxCount)
			{
				this.context.Response.Write(this.GetErrorJson("收货地址已超过限制个数"));
			}
			else
			{
				ShippingAddressInfo shippingAddressInfo = new ShippingAddressInfo();
				shippingAddressInfo.Address = Globals.StripAllTags(this.context.Request["address"]);
				shippingAddressInfo.CellPhone = Globals.StripAllTags(this.context.Request["cellphone"]);
				shippingAddressInfo.ShipTo = Globals.StripAllTags(this.context.Request["shipTo"]);
				shippingAddressInfo.Zipcode = "";
				shippingAddressInfo.IsDefault = this.context.Request["isDefault"].ToBool();
				shippingAddressInfo.UserId = user.UserId;
				shippingAddressInfo.RegionId = this.context.Request["regionId"].ToInt(0);
				shippingAddressInfo.BuildingNumber = Globals.StripAllTags(this.context.Request["BuildingNumber"].ToNullString());
				shippingAddressInfo.LatLng = this.context.Request["LatLng"].ToNullString();
				if (shippingAddressInfo.RegionId <= 0 || string.IsNullOrEmpty(RegionHelper.GetFullRegion(shippingAddressInfo.RegionId, " ", true, 0)))
				{
					this.context.Response.Write(this.GetErrorJson("错误的地区信息"));
				}
				else
				{
					shippingAddressInfo.FullRegionPath = RegionHelper.GetFullPath(shippingAddressInfo.RegionId, true);
					int num = MemberProcessor.AddShippingAddress(shippingAddressInfo);
					if (num > 0)
					{
						this.context.Response.Write(this.GetOKJson(num.ToString()));
					}
					else
					{
						this.context.Response.Write(this.GetErrorJson("系统错误"));
					}
				}
			}
		}

		public void GetUserShippingAddress()
		{
			this.CheckOpenId();
			IList<ShippingAddressInfo> shippingAddresses = MemberProcessor.GetShippingAddresses(false);
			string text = "";
			text = ((shippingAddresses == null || shippingAddresses.Count() <= 0) ? JsonConvert.SerializeObject(new
			{
				Status = "NO",
				Data = "[]"
			}) : JsonConvert.SerializeObject(new
			{
				Status = "OK",
				Data = from s in shippingAddresses
				select new
				{
					Address = s.Address,
					CellPhone = s.CellPhone,
					FullAddress = s.FullAddress,
					FullRegionPath = s.FullRegionPath,
					ShipTo = s.ShipTo,
					ShippingId = s.ShippingId,
					TelPhone = s.TelPhone,
					LatLng = s.LatLng,
					RegionLocation = s.RegionLocation,
					FullRegionName = RegionHelper.GetFullRegion(s.RegionId, " ", true, 0),
					IsDefault = s.IsDefault,
					BuildingNumber = s.BuildingNumber,
					RegionId = s.RegionId
				}
			}));
			this.context.Response.Write(text);
		}

		public void LoadCoupon()
		{
			this.CheckOpenId();
			if (!string.IsNullOrEmpty(this.context.Request["pageSize"]) && !string.IsNullOrEmpty(this.context.Request["pageIndex"]) && !string.IsNullOrEmpty(this.context.Request["couponType"]))
			{
				int pageSize = this.context.Request["pageSize"].ToInt(0);
				int pageIndex = this.context.Request["pageIndex"].ToInt(0);
				int value = this.context.Request["couponType"].ToInt(0);
				CouponItemInfoQuery couponItemInfoQuery = new CouponItemInfoQuery();
				couponItemInfoQuery.PageIndex = pageIndex;
				couponItemInfoQuery.PageSize = pageSize;
				couponItemInfoQuery.UserId = HiContext.Current.UserId;
				couponItemInfoQuery.CouponStatus = value;
				couponItemInfoQuery.SortBy = "GetDate";
				couponItemInfoQuery.SortOrder = SortAction.Desc;
				DbQueryResult couponsUseList = CouponHelper.GetCouponsUseList(couponItemInfoQuery);
				string text = "";
				text = ((couponsUseList.Data == null || couponsUseList.Data.Rows.Count <= 0) ? JsonConvert.SerializeObject(new
				{
					Status = "NO",
					Data = "[]"
				}) : JsonConvert.SerializeObject(new
				{
					Status = "OK",
					Data = couponsUseList.Data
				}));
				this.context.Response.Write(text);
			}
		}

		public void LoadSiteCoupon()
		{
			if (string.IsNullOrEmpty(this.context.Request["PageSize"]) || string.IsNullOrEmpty(this.context.Request["pageIndex"]))
			{
				this.context.Response.Write(this.GetErrorJson("参数错误"));
			}
			else
			{
				int pageSize = int.Parse(this.context.Request["PageSize"]);
				int pageIndex = int.Parse(this.context.Request["pageIndex"]);
				int value = 0;
				int.TryParse(this.context.Request["obtainWay"], out value);
				int num = 0;
				MemberInfo user = Users.GetUser(HiContext.Current.UserId);
				if (user != null)
				{
					num = user.UserId;
				}
				CouponsSearch couponsSearch = new CouponsSearch();
				couponsSearch.ObtainWay = value;
				couponsSearch.PageIndex = pageIndex;
				couponsSearch.PageSize = pageSize;
				couponsSearch.IsValid = true;
				DbQueryResult couponInfos = CouponHelper.GetCouponInfos(couponsSearch, "");
				DataTable data = couponInfos.Data;
				string str = "{\"Status\":\"OK\",\"totalCount\":\"" + couponInfos.TotalRecords + "\",\"Data\":[";
				string text = "";
				for (int i = 0; i < data.Rows.Count; i++)
				{
					if (text != "")
					{
						text += ",";
					}
					object[] obj = new object[24]
					{
						text,
						"{\"CouponId\":\"",
						data.Rows[i]["CouponId"],
						"\",\"CouponName\":\"",
						data.Rows[i]["CouponName"],
						"\",\"Price\":\"",
						data.Rows[i]["Price"],
						"\",\"SendCount\":\"",
						data.Rows[i]["SendCount"],
						"\",\"UserLimitCount\":\"",
						data.Rows[i]["UserLimitCount"],
						"\",\"OrderUseLimit\":\"",
						data.Rows[i]["OrderUseLimit"],
						"\",\"StartTime\":\"",
						null,
						null,
						null,
						null,
						null,
						null,
						null,
						null,
						null,
						null
					};
					DateTime dateTime = DateTime.Parse(data.Rows[i]["StartTime"].ToString());
					obj[14] = dateTime.ToString("yyyy.MM.dd");
					obj[15] = "\",\"ClosingTime\":\"";
					dateTime = DateTime.Parse(data.Rows[i]["ClosingTime"].ToString());
					obj[16] = dateTime.ToString("yyyy.MM.dd");
					obj[17] = "\",\"CanUseProducts\":\"";
					obj[18] = data.Rows[i]["CanUseProducts"];
					obj[19] = "\",\"ObtainWay\":\"";
					obj[20] = data.Rows[i]["ObtainWay"];
					obj[21] = "\",\"NeedPoint\":\"";
					obj[22] = data.Rows[i]["NeedPoint"];
					obj[23] = "\"}";
					text = string.Concat(obj);
				}
				str += text;
				str += "]}";
				this.context.Response.Write(str);
			}
		}

		public void GetCouponDetail()
		{
			int num = 0;
			if (!int.TryParse(this.context.Request.QueryString["couponId"], out num))
			{
				num = 0;
			}
			if (num <= 0)
			{
				this.context.Response.Write(this.GetErrorJson("参数错误"));
			}
			else
			{
				CouponInfo eFCoupon = CouponHelper.GetEFCoupon(num);
				if (eFCoupon == null)
				{
					this.context.Response.Write(this.GetErrorJson("优惠券编号错误"));
				}
				else
				{
					string s = JsonConvert.SerializeObject(new
					{
						Status = "OK",
						Data = eFCoupon
					});
					this.context.Response.Write(s);
				}
			}
		}

		public void UserGetCoupon()
		{
			this.CheckOpenId();
			string s = this.context.Request["couponId"];
			MemberInfo user = Users.GetUser(HiContext.Current.UserId);
			int num = 0;
			if (!int.TryParse(s, out num) || num <= 0)
			{
				this.context.Response.Write(this.GetErrorJson(((Enum)(object)ApiErrorCode.CouponParamter_Error).ToDescription()));
			}
			else
			{
				switch (CouponHelper.UserGetCoupon(user, num))
				{
				case CouponActionStatus.Success:
					this.context.Response.Write(this.GetOKJson("领取成功"));
					break;
				case CouponActionStatus.NotExists:
					this.context.Response.Write(this.GetErrorJson(((Enum)(object)ApiErrorCode.CouponNotExists_Error).ToDescription()));
					break;
				case CouponActionStatus.InconsistentInformationUser:
					this.context.Response.Write(this.GetErrorJson(((Enum)(object)ApiErrorCode.CouponUserInfo_Error).ToDescription()));
					break;
				case CouponActionStatus.InadequateInventory:
					this.context.Response.Write(this.GetErrorJson(((Enum)(object)ApiErrorCode.CouponNotStock_Error).ToDescription()));
					break;
				case CouponActionStatus.CannotReceive:
				{
					CouponInfo eFCoupon = CouponHelper.GetEFCoupon(num);
					this.context.Response.Write(this.GetErrorJson("每人限领" + eFCoupon.UserLimitCount + "张"));
					break;
				}
				case CouponActionStatus.CanNotGet:
					this.context.Response.Write(this.GetErrorJson(((Enum)(object)ApiErrorCode.CouponNotSupportGet_Error).ToDescription()));
					break;
				case CouponActionStatus.Overdue:
					this.context.Response.Write(this.GetErrorJson(((Enum)(object)ApiErrorCode.CouponOverDue_Error).ToDescription()));
					break;
				default:
					this.context.Response.Write(this.GetErrorJson(((Enum)(object)ApiErrorCode.Unknown_Error).ToDescription()));
					break;
				}
			}
		}

		public void GetCountDownProductDetail()
		{
			int countDownId = this.context.Request["countDownId"].ToInt(0);
			CountDownInfo countDownInfo = PromoteHelper.GetCountDownInfo(countDownId, 0);
			if (countDownInfo == null)
			{
				this.context.Response.Write(this.GetErrorJson("抢购活动不存在"));
			}
			else
			{
				int gradeId = HiContext.Current.User.GradeId;
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				ProductBrowseInfo appletProductBrowseInfo = ProductBrowser.GetAppletProductBrowseInfo(countDownInfo.ProductId, 0);
				if (appletProductBrowseInfo.Product == null || appletProductBrowseInfo.Product.SaleStatus == ProductSaleStatus.Delete)
				{
					this.context.Response.Write(this.GetErrorJson("抢购商品不存在"));
				}
				else
				{
					string text = "";
					text = (appletProductBrowseInfo.Product.SaleStatus.Equals(ProductSaleStatus.OnSale) ? (countDownInfo.IsJoin ? ((!(countDownInfo.StartDate > DateTime.Now)) ? ((!(countDownInfo.EndDate < DateTime.Now)) ? (countDownInfo.IsRunning ? "Normal" : "SoldOut") : "ActivityEnd") : "AboutToBegin") : "NoJoin") : "PullOff");
					List<string> list = new List<string>();
					if (string.IsNullOrEmpty(appletProductBrowseInfo.Product.ImageUrl1) && string.IsNullOrEmpty(appletProductBrowseInfo.Product.ImageUrl2) && string.IsNullOrEmpty(appletProductBrowseInfo.Product.ImageUrl3) && string.IsNullOrEmpty(appletProductBrowseInfo.Product.ImageUrl4) && string.IsNullOrEmpty(appletProductBrowseInfo.Product.ImageUrl5))
					{
						list.Add(this.GetProductImageFullPath(masterSettings.DefaultProductImage));
					}
					else
					{
						if (!string.IsNullOrEmpty(appletProductBrowseInfo.Product.ImageUrl1))
						{
							list.Add(this.GetProductImageFullPath(appletProductBrowseInfo.Product.ImageUrl1));
						}
						if (!string.IsNullOrEmpty(appletProductBrowseInfo.Product.ImageUrl2))
						{
							list.Add(this.GetProductImageFullPath(appletProductBrowseInfo.Product.ImageUrl2));
						}
						if (!string.IsNullOrEmpty(appletProductBrowseInfo.Product.ImageUrl3))
						{
							list.Add(this.GetProductImageFullPath(appletProductBrowseInfo.Product.ImageUrl3));
						}
						if (!string.IsNullOrEmpty(appletProductBrowseInfo.Product.ImageUrl4))
						{
							list.Add(this.GetProductImageFullPath(appletProductBrowseInfo.Product.ImageUrl4));
						}
						if (!string.IsNullOrEmpty(appletProductBrowseInfo.Product.ImageUrl5))
						{
							list.Add(this.GetProductImageFullPath(appletProductBrowseInfo.Product.ImageUrl5));
						}
					}
					List<SkuItem> list2 = new List<SkuItem>();
					string empty = string.Empty;
					if (appletProductBrowseInfo.DbSKUs != null && appletProductBrowseInfo.DbSKUs.Rows.Count > 0)
					{
						foreach (DataRow row in appletProductBrowseInfo.DbSKUs.Rows)
						{
							if ((from c in list2
							where c.AttributeId == row["AttributeId"].ToNullString()
							select c).Count() == 0)
							{
								SkuItem skuItem = new SkuItem();
								skuItem.AttributeName = row["AttributeName"].ToNullString();
								skuItem.AttributeId = row["AttributeId"].ToNullString();
								skuItem.AttributeValue = new List<AttributeValue>();
								IList<string> list3 = new List<string>();
								foreach (DataRow row2 in appletProductBrowseInfo.DbSKUs.Rows)
								{
									if (string.Compare(row["AttributeId"].ToString(), row2["AttributeId"].ToString()) == 0 && !list3.Contains((string)row2["ValueStr"]))
									{
										AttributeValue attributeValue = new AttributeValue();
										list3.Add((string)row2["ValueStr"]);
										attributeValue.ValueId = row2["ValueId"].ToNullString();
										attributeValue.UseAttributeImage = row2["UseAttributeImage"].ToNullString();
										attributeValue.Value = row2["ValueStr"].ToNullString();
										attributeValue.ImageUrl = Globals.HttpsFullPath(row2["ImageUrl"].ToNullString());
										skuItem.AttributeValue.Add(attributeValue);
									}
								}
								list2.Add(skuItem);
							}
						}
					}
					DataTable cdSkus = PromoteHelper.GetCountDownSkus(countDownInfo.CountDownId, 0, false);
					List<SKUItem> list4 = new List<SKUItem>();
					foreach (SKUItem value in appletProductBrowseInfo.Product.Skus.Values)
					{
						value.SalePrice = decimal.Parse(value.SalePrice.F2ToString("f2"));
						value.CostPrice = decimal.Parse(value.CostPrice.F2ToString("f2"));
						value.ImageUrl = this.GetImageFullPath(value.ImageUrl);
						value.ThumbnailUrl40 = this.GetImageFullPath(value.ThumbnailUrl40);
						value.ThumbnailUrl410 = this.GetImageFullPath(value.ThumbnailUrl410);
						list4.Add(value);
					}
					decimal num = ShoppingProcessor.CalcProductFreight(HiContext.Current.DeliveryScopRegionId, appletProductBrowseInfo.Product.ShippingTemplateId, appletProductBrowseInfo.Product.Weight, appletProductBrowseInfo.Product.Weight, 1, appletProductBrowseInfo.Product.MinSalePrice);
					decimal d = default(decimal);
					decimal d2 = (appletProductBrowseInfo.Product.MinSalePrice - d > decimal.Zero) ? (appletProductBrowseInfo.Product.MinSalePrice - d) : decimal.Zero;
					decimal d3 = masterSettings.ShowDeductInProductPage ? (appletProductBrowseInfo.Product.SubMemberDeduct.HasValue ? appletProductBrowseInfo.Product.SubMemberDeduct.Value : HiContext.Current.SiteSettings.SubMemberDeduct) : decimal.Zero;
					decimal referralMoney = (d2 * (d3 / 100m)).F2ToString("f2").ToDecimal(0);
					var enumerable = from sku in list4
					select new
					{
						SkuId = sku.SkuId,
						SKU = sku.SKU,
						Weight = sku.Weight,
						Stock = sku.Stock,
						WarningStock = sku.WarningStock,
						ActivityStock = cdSkus.AsEnumerable().First((DataRow fgs) => fgs.Field<string>("SkuId") == sku.SkuId).Field<int>("TotalCount") - cdSkus.AsEnumerable().First((DataRow fgs) => fgs.Field<string>("SkuId") == sku.SkuId).Field<int>("BoughtCount"),
						ActivityPrice = cdSkus.AsEnumerable().First((DataRow fgs) => fgs.Field<string>("SkuId") == sku.SkuId).Field<decimal>("SalePrice"),
						SalePrice = sku.SalePrice,
						ThumbnailUrl40 = sku.ThumbnailUrl40
					};
					DataTable couponList = CouponHelper.GetCouponList(countDownInfo.ProductId, HiContext.Current.UserId, false, true, false);
					DateTime dateTime;
					if (couponList != null && couponList.Rows.Count > 0)
					{
						couponList.Columns.Add("LimitText", typeof(string));
						couponList.Columns.Add("CanUseProduct", typeof(string));
						couponList.Columns.Add("StartTimeText", typeof(string));
						couponList.Columns.Add("ClosingTimeText", typeof(string));
						foreach (DataRow row3 in couponList.Rows)
						{
							row3["LimitText"] = ((row3["OrderUseLimit"].ToDecimal(0) > decimal.Zero) ? ("订单满" + string.Format("{0:f2}", row3["OrderUseLimit"].ToDecimal(0)) + "元可用") : "订单金额无限制");
							row3["CanUseProduct"] = (string.IsNullOrEmpty(row3["CanUseProducts"].ToNullString()) ? "全场通用" : "部分商品可用");
							DataRow dataRow3 = row3;
							dateTime = row3["StartTime"].ToDateTime().Value;
							dataRow3["StartTimeText"] = dateTime.ToString("yyyy.MM.dd");
							DataRow dataRow4 = row3;
							dateTime = row3["ClosingTime"].ToDateTime().Value;
							dataRow4["ClosingTimeText"] = dateTime.ToString("yyyy.MM.dd");
							row3["Price"] = decimal.Parse(row3["Price"].ToString()).F2ToString("f2");
						}
					}
					int countDownId2 = countDownInfo.CountDownId;
					int maxCount = countDownInfo.MaxCount;
					string countDownStatus = text;
					dateTime = countDownInfo.StartDate;
					string startDate = dateTime.ToString("yyyy/MM/dd HH:mm:ss");
					dateTime = countDownInfo.EndDate;
					string endDate = dateTime.ToString("yyyy/MM/dd HH:mm:ss");
					dateTime = DateTime.Now;
					string s = JsonConvert.SerializeObject(new
					{
						Status = "OK",
						Data = new
						{
							CountDownId = countDownId2,
							MaxCount = maxCount,
							CountDownStatus = countDownStatus,
							StartDate = startDate,
							EndDate = endDate,
							NowTime = dateTime.ToString("yyyy/MM/dd HH:mm:ss", DateTimeFormatInfo.InvariantInfo),
							ProductId = appletProductBrowseInfo.Product.ProductId,
							ProductName = appletProductBrowseInfo.Product.ProductName,
							MetaDescription = (string.IsNullOrEmpty(appletProductBrowseInfo.Product.MobbileDescription.ToNullString()) ? appletProductBrowseInfo.Product.Description.ToNullString() : appletProductBrowseInfo.Product.MobbileDescription.ToNullString()).Replace("\"/Storage/master/gallery/", "\"" + Globals.HttpsFullPath("/Storage/master/gallery/")),
							ShortDescription = appletProductBrowseInfo.Product.ShortDescription,
							ShowSaleCounts = appletProductBrowseInfo.Product.ShowSaleCounts.ToString(),
							MinSalePrice = enumerable.Min(c => c.ActivityPrice).F2ToString("f2"),
							MaxSalePrice = enumerable.Max(c => c.ActivityPrice).F2ToString("f2"),
							ReviewCount = appletProductBrowseInfo.ReviewCount,
							Stock = enumerable.Max(c => c.ActivityStock),
							MarketPrice = (appletProductBrowseInfo.Product.MarketPrice.HasValue ? appletProductBrowseInfo.Product.MarketPrice.Value.F2ToString("f2") : appletProductBrowseInfo.Product.Skus.Min((KeyValuePair<string, SKUItem> c) => c.Value.SalePrice).F2ToString("f2")),
							IsfreeShipping = appletProductBrowseInfo.Product.IsfreeShipping.ToString(),
							ThumbnailUrl60 = this.GetProductImageFullPath(appletProductBrowseInfo.Product.ThumbnailUrl220),
							ProductImgs = list,
							SkuItemList = list2,
							Skus = enumerable,
							Freight = num.F2ToString("f2"),
							Coupons = couponList,
							ExtendAttribute = ProductBrowser.GetExpandAttributeList(appletProductBrowseInfo.Product.ProductId),
							ReferralMoney = referralMoney
						}
					});
					this.context.Response.Write(s);
				}
			}
		}

		public void GetProductDetail()
		{
			this.BindUserByOpenId();
			int num = this.context.Request["productId"].ToInt(0);
			int gradeId = HiContext.Current.User.GradeId;
			if (num <= 0)
			{
				this.context.Response.Write(this.GetErrorJson("商品不存在"));
			}
			else
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				ProductBrowseInfo appletProductBrowseInfo = ProductBrowser.GetAppletProductBrowseInfo(num, gradeId);
				if (appletProductBrowseInfo.Product == null || appletProductBrowseInfo.Product.SaleStatus == ProductSaleStatus.Delete)
				{
					this.context.Response.Write(this.GetErrorJson("商品不存在"));
				}
				else
				{
					List<string> list = new List<string>();
					if (string.IsNullOrEmpty(appletProductBrowseInfo.Product.ImageUrl1) && string.IsNullOrEmpty(appletProductBrowseInfo.Product.ImageUrl2) && string.IsNullOrEmpty(appletProductBrowseInfo.Product.ImageUrl3) && string.IsNullOrEmpty(appletProductBrowseInfo.Product.ImageUrl4) && string.IsNullOrEmpty(appletProductBrowseInfo.Product.ImageUrl5))
					{
						list.Add(this.GetProductImageFullPath(masterSettings.DefaultProductImage));
					}
					else
					{
						if (!string.IsNullOrEmpty(appletProductBrowseInfo.Product.ImageUrl1))
						{
							list.Add(this.GetProductImageFullPath(appletProductBrowseInfo.Product.ImageUrl1));
						}
						if (!string.IsNullOrEmpty(appletProductBrowseInfo.Product.ImageUrl2))
						{
							list.Add(this.GetProductImageFullPath(appletProductBrowseInfo.Product.ImageUrl2));
						}
						if (!string.IsNullOrEmpty(appletProductBrowseInfo.Product.ImageUrl3))
						{
							list.Add(this.GetProductImageFullPath(appletProductBrowseInfo.Product.ImageUrl3));
						}
						if (!string.IsNullOrEmpty(appletProductBrowseInfo.Product.ImageUrl4))
						{
							list.Add(this.GetProductImageFullPath(appletProductBrowseInfo.Product.ImageUrl4));
						}
						if (!string.IsNullOrEmpty(appletProductBrowseInfo.Product.ImageUrl5))
						{
							list.Add(this.GetProductImageFullPath(appletProductBrowseInfo.Product.ImageUrl5));
						}
					}
					decimal d2 = default(decimal);
					appletProductBrowseInfo.Product.DefaultSku.SalePrice = decimal.Parse(appletProductBrowseInfo.Product.DefaultSku.SalePrice.F2ToString("f2"));
					decimal d3 = (appletProductBrowseInfo.Product.MinSalePrice - d2 > decimal.Zero) ? (appletProductBrowseInfo.Product.MinSalePrice - d2) : decimal.Zero;
					decimal d4 = appletProductBrowseInfo.Product.SubMemberDeduct.HasValue ? appletProductBrowseInfo.Product.SubMemberDeduct.Value : HiContext.Current.SiteSettings.SubMemberDeduct;
					decimal referralMoney = masterSettings.ShowDeductInProductPage ? (d3 * (d4 / 100m)).F2ToString("f2").ToDecimal(0) : decimal.Zero;
					List<SkuItem> list2 = new List<SkuItem>();
					string empty = string.Empty;
					if (appletProductBrowseInfo.DbSKUs != null && appletProductBrowseInfo.DbSKUs.Rows.Count > 0)
					{
						foreach (DataRow row in appletProductBrowseInfo.DbSKUs.Rows)
						{
							if ((from c in list2
							where c.AttributeId == row["AttributeId"].ToNullString()
							select c).Count() == 0)
							{
								SkuItem skuItem = new SkuItem();
								skuItem.AttributeName = row["AttributeName"].ToNullString();
								skuItem.AttributeId = row["AttributeId"].ToNullString();
								skuItem.AttributeValue = new List<AttributeValue>();
								IList<string> list3 = new List<string>();
								foreach (DataRow row2 in appletProductBrowseInfo.DbSKUs.Rows)
								{
									if (string.Compare(row["AttributeId"].ToString(), row2["AttributeId"].ToString()) == 0 && !list3.Contains((string)row2["ValueStr"]))
									{
										AttributeValue attributeValue = new AttributeValue();
										list3.Add((string)row2["ValueStr"]);
										attributeValue.ValueId = row2["ValueId"].ToNullString();
										attributeValue.UseAttributeImage = row2["UseAttributeImage"].ToNullString();
										attributeValue.Value = row2["ValueStr"].ToNullString();
										attributeValue.ImageUrl = Globals.HttpsFullPath(row2["ImageUrl"].ToNullString());
										skuItem.AttributeValue.Add(attributeValue);
									}
								}
								list2.Add(skuItem);
							}
						}
					}
					List<SKUItem> list4 = new List<SKUItem>();
					foreach (SKUItem value2 in appletProductBrowseInfo.Product.Skus.Values)
					{
						value2.SalePrice = decimal.Parse(value2.SalePrice.F2ToString("f2"));
						value2.CostPrice = decimal.Parse(value2.CostPrice.F2ToString("f2"));
						list4.Add(value2);
					}
					decimal num2 = ShoppingProcessor.CalcProductFreight(HiContext.Current.DeliveryScopRegionId, appletProductBrowseInfo.Product.ShippingTemplateId, appletProductBrowseInfo.Product.Weight, appletProductBrowseInfo.Product.Weight, 1, appletProductBrowseInfo.Product.MinSalePrice);
					DataTable couponList = CouponHelper.GetCouponList(num, HiContext.Current.UserId, false, false, false);
					if (couponList != null && couponList.Rows.Count > 0)
					{
						couponList.Columns.Add("LimitText", typeof(string));
						couponList.Columns.Add("CanUseProduct", typeof(string));
						couponList.Columns.Add("StartTimeText", typeof(string));
						couponList.Columns.Add("ClosingTimeText", typeof(string));
						foreach (DataRow row3 in couponList.Rows)
						{
							row3["LimitText"] = ((row3["OrderUseLimit"].ToDecimal(0) > decimal.Zero) ? ("订单满" + string.Format("{0:f2}", row3["OrderUseLimit"].ToDecimal(0)) + "元可用") : "订单金额无限制");
							row3["CanUseProduct"] = (string.IsNullOrEmpty(row3["CanUseProducts"].ToNullString()) ? "全场通用" : "部分商品可用");
							DataRow dataRow3 = row3;
							DateTime value = row3["StartTime"].ToDateTime().Value;
							dataRow3["StartTimeText"] = value.ToString("yyyy.MM.dd");
							DataRow dataRow4 = row3;
							value = row3["ClosingTime"].ToDateTime().Value;
							dataRow4["ClosingTimeText"] = value.ToString("yyyy.MM.dd");
							row3["Price"] = decimal.Parse(row3["Price"].ToString()).F2ToString("f2");
						}
					}
					int activeType = 0;
					int activeId = this.ActivityBusiness(appletProductBrowseInfo.Product.ProductId, out activeType);
					StoreActivityEntityList storeActivityEntity = PromoteHelper.GetStoreActivityEntity(0, num);
					storeActivityEntity.FullAmountSentGiftList = (from d in storeActivityEntity.FullAmountSentGiftList
					where d.PromoteType == 16
					select d).ToList();
					SupplierInfo supplierInfo = null;
					if (appletProductBrowseInfo.Product.SupplierId > 0)
					{
						supplierInfo = SupplierHelper.GetSupplierById(appletProductBrowseInfo.Product.SupplierId);
					}
					string s = JsonConvert.SerializeObject(new
					{
						Status = "OK",
						Data = new
						{
							ProductId = appletProductBrowseInfo.Product.ProductId,
							ProductName = appletProductBrowseInfo.Product.ProductName,
							MetaDescription = (string.IsNullOrEmpty(appletProductBrowseInfo.Product.MobbileDescription) ? appletProductBrowseInfo.Product.Description : appletProductBrowseInfo.Product.MobbileDescription).ToNullString().Replace("\"/Storage/master/gallery/", "\"" + Globals.HttpsFullPath("/Storage/master/gallery/")),
							ShortDescription = appletProductBrowseInfo.Product.ShortDescription,
							ShowSaleCounts = appletProductBrowseInfo.Product.ShowSaleCounts.ToString(),
							MarketPrice = appletProductBrowseInfo.Product.MarketPrice.ToDecimal(0).F2ToString("f2"),
							IsfreeShipping = appletProductBrowseInfo.Product.IsfreeShipping.ToString(),
							MaxSalePrice = appletProductBrowseInfo.Product.MaxSalePrice.F2ToString("f2"),
							MinSalePrice = appletProductBrowseInfo.Product.MinSalePrice.F2ToString("f2"),
							ThumbnailUrl60 = this.GetProductImageFullPath(appletProductBrowseInfo.Product.ThumbnailUrl220),
							SupplierId = appletProductBrowseInfo.Product.SupplierId,
							SupplierName = ((supplierInfo == null) ? "平台" : supplierInfo.SupplierName),
							ProductImgs = list,
							ReviewCount = appletProductBrowseInfo.ReviewCount,
							Stock = appletProductBrowseInfo.Product.Stock,
							SkuItemList = list2,
							Skus = list4,
							Freight = num2.F2ToString("f2"),
							Coupons = couponList,
							Promotes = storeActivityEntity,
							IsUnSale = (appletProductBrowseInfo.Product.SaleStatus == ProductSaleStatus.UnSale && true),
							IsOnSale = (appletProductBrowseInfo.Product.SaleStatus == ProductSaleStatus.OnSale && true),
							ActiveId = activeId,
							ActiveType = activeType,
							ExtendAttribute = ProductBrowser.GetExpandAttributeList(num),
							ReferralMoney = referralMoney
						}
					});
					this.context.Response.Write(s);
				}
			}
		}

		private int ActivityBusiness(int productId, out int typeId)
		{
			int result = 0;
			typeId = 0;
			CountDownInfo countDownInfo = PromoteHelper.ActiveCountDownByProductId(productId, 0);
			GroupBuyInfo groupBuyInfo = PromoteHelper.ActiveGroupBuyByProductId(productId);
			ProductPreSaleInfo productPreSaleInfoByProductId = ProductPreSaleHelper.GetProductPreSaleInfoByProductId(productId);
			if (countDownInfo != null)
			{
				result = countDownInfo.CountDownId;
				typeId = 1;
			}
			else if (groupBuyInfo != null)
			{
				result = groupBuyInfo.GroupBuyId;
				typeId = 2;
			}
			else if (productPreSaleInfoByProductId != null && productPreSaleInfoByProductId.PreSaleEndDate >= DateTime.Now)
			{
				result = productPreSaleInfoByProductId.PreSaleId;
				typeId = 3;
			}
			else
			{
				int activityStartsImmediatelyAboutCountDown = PromoteHelper.GetActivityStartsImmediatelyAboutCountDown(productId);
				if (activityStartsImmediatelyAboutCountDown > 0)
				{
					typeId = 4;
					result = activityStartsImmediatelyAboutCountDown;
				}
				else
				{
					int activityStartsImmediatelyAboutGroupBuy = PromoteHelper.GetActivityStartsImmediatelyAboutGroupBuy(productId);
					if (activityStartsImmediatelyAboutGroupBuy > 0)
					{
						result = activityStartsImmediatelyAboutGroupBuy;
						typeId = 5;
					}
				}
			}
			return result;
		}

		private void CheckOpenId()
		{
			string text = this.context.Request["openId"];
			if (string.IsNullOrEmpty(text) || !this.ValidLoginByOpenId(text))
			{
				this.context.Response.Write(this.GetErrorJson("NOUser"));
				this.context.Response.End();
			}
		}

		private bool ValidLoginByOpenId(string openId)
		{
			try
			{
				MemberInfo memberByOpenId = MemberProcessor.GetMemberByOpenId("hishop.plugins.openid.wxapplet", openId);
				if (memberByOpenId == null)
				{
					Globals.AppendLog((memberByOpenId == null) ? "" : memberByOpenId.UserId.ToString(), (memberByOpenId == null) ? "" : memberByOpenId.UserName, openId, "ValidLoginByOpenId2");
					return false;
				}
				HiContext.Current.UserId = memberByOpenId.UserId;
				HiContext.Current.User = memberByOpenId;
				return true;
			}
			catch (Exception ex)
			{
				Globals.WriteExceptionLog(ex, null, "ValidLoginByOpenIdEx");
				return false;
			}
		}

		private void BindUserByOpenId()
		{
			try
			{
				string text = this.context.Request["openId"];
				if (!string.IsNullOrEmpty(text))
				{
					MemberInfo memberByOpenId = MemberProcessor.GetMemberByOpenId("hishop.plugins.openid.wxapplet", text);
					if (memberByOpenId != null)
					{
						HiContext.Current.UserId = memberByOpenId.UserId;
						HiContext.Current.User = memberByOpenId;
					}
				}
			}
			catch
			{
			}
		}

		public string getUnionId()
		{
			string result = "";
			string text = this.context.Request["encryptedData"].ToNullString();
			string text2 = this.context.Request["iv"].ToNullString();
			string text3 = this.context.Request["session_key"].ToNullString();
			if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text2) && !string.IsNullOrEmpty(text3))
			{
				try
				{
					string text4 = this.AESDecrypt(text3, text2, text);
					if (!string.IsNullOrEmpty(text4))
					{
						WxAppletUserInfo wxAppletUserInfo = JsonHelper.ParseFormJson<WxAppletUserInfo>(text4);
						result = wxAppletUserInfo.unionId;
					}
				}
				catch (Exception ex)
				{
					IDictionary<string, string> dictionary = new Dictionary<string, string>();
					dictionary.Add("encryptedData", text);
					dictionary.Add("iv", text2);
					dictionary.Add("sessionKey", text3);
					Globals.WriteExceptionLog(ex, dictionary, "getUnionId");
				}
			}
			return result;
		}

		public void LoginByOpenId()
		{
			string text = this.context.Request["openId"];
			string unionId = this.getUnionId();
			try
			{
				if (!string.IsNullOrEmpty(unionId))
				{
					MemberInfo memberByUnionId = MemberProcessor.GetMemberByUnionId(unionId);
					if (memberByUnionId != null)
					{
						Users.ClearUserCache(memberByUnionId.UserId, "");
						Users.SetCurrentUser(memberByUnionId.UserId, 30, true, false);
						HiContext.Current.User = memberByUnionId;
						this.GetMember(memberByUnionId, text);
					}
					else
					{
						Globals.AppendLog("没有找到相应账号", text, "hishop.plugins.openid.wxapplet", "LoginByOpenId");
						this.context.Response.Write(this.GetErrorJson("账号或密码错误"));
					}
				}
				else if (!string.IsNullOrEmpty(text))
				{
					MemberInfo memberByOpenId = MemberProcessor.GetMemberByOpenId("hishop.plugins.openid.wxapplet", text);
					if (memberByOpenId != null)
					{
						Users.ClearUserCache(memberByOpenId.UserId, "");
						Users.SetCurrentUser(memberByOpenId.UserId, 30, true, false);
						HiContext.Current.User = memberByOpenId;
						this.GetMember(memberByOpenId, text);
					}
					else
					{
						Globals.AppendLog("没有找到相应账号", text, "hishop.plugins.openid.wxapplet", "LoginByOpenId");
						this.context.Response.Write(this.GetErrorJson("账号或密码错误"));
					}
				}
			}
			catch (Exception ex)
			{
				IDictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("OpenId", text);
				Globals.WriteExceptionLog(ex, dictionary, "LoginByOpenId");
			}
		}

		public void LoginByUserName()
		{
			string text = this.context.Request["openId"];
			string userName = this.context.Request["userName"].ToNullString();
			string password = this.context.Request["password"].ToNullString();
			string text2 = this.context.Request["nickName"].ToNullString();
			string unionId = this.getUnionId();
			MemberInfo memberInfo = MemberProcessor.ValidLogin(userName, password);
			if (memberInfo != null)
			{
				bool flag = true;
				memberInfo.IsLogined = true;
				Users.ClearUserCache(memberInfo.UserId, "");
				Users.SetCurrentUser(memberInfo.UserId, 30, false, false);
				HiContext.Current.User = memberInfo;
				if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty("hishop.plugins.openid.wxapplet"))
				{
					this.AddOrUpdateOpenId(memberInfo.UserId, "hishop.plugins.openid.wxapplet", text);
				}
				if (!string.IsNullOrEmpty(text))
				{
					HttpCookie httpCookie = new HttpCookie("openId");
					httpCookie.HttpOnly = true;
					httpCookie.Value = text;
					httpCookie.Expires = DateTime.MaxValue;
					HttpContext.Current.Response.Cookies.Add(httpCookie);
				}
				if (!string.IsNullOrEmpty(unionId) && MemberProcessor.GetMemberByUnionId(unionId) == null)
				{
					memberInfo.UnionId = unionId;
					flag = true;
				}
				if (string.IsNullOrEmpty(memberInfo.NickName) && !string.IsNullOrEmpty(text2))
				{
					memberInfo.NickName = HttpUtility.UrlDecode(text2);
					flag = true;
				}
				if (flag)
				{
					MemberProcessor.UpdateMember(memberInfo);
				}
				this.GetMember(memberInfo, text);
			}
			else
			{
				this.context.Response.Write(this.GetErrorJson("账号或密码错误"));
			}
		}

		public void QuickLogin()
		{
			bool flag = true;
			int num = 0;
			MemberInfo memberInfo = null;
			string text = this.context.Request["openId"].ToNullString();
			try
			{
				string text2 = this.context.Request["nickName"].ToNullString();
				int num2 = this.context.Request["referralUserId"].ToInt(0);
				string unionId = this.getUnionId();
				string text3 = Globals.UrlDecode(this.context.Request["headImage"].ToNullString());
				memberInfo = MemberProcessor.GetMemberByOpenIdOfQuickLogin("hishop.plugins.openid.wxapplet", text);
				bool flag2 = false;
				if (memberInfo == null)
				{
					memberInfo = MemberProcessor.GetMemberByUnionId(unionId);
					if (memberInfo != null)
					{
						flag2 = true;
					}
				}
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				if (memberInfo == null && !masterSettings.QuickLoginIsForceBindingMobbile)
				{
					num = this.SkipWeixinOpenId(out memberInfo, out flag);
				}
				else if (memberInfo != null)
				{
					memberInfo.IsLogined = true;
					bool flag3 = MemberProcessor.IsBindedWeixin(memberInfo.UserId, "hishop.plugins.openid.wxapplet");
					if (!string.IsNullOrEmpty(text3) || text3.StartsWith("http://wx.qlogo.cn/mmopen/"))
					{
						memberInfo.Picture = text3;
					}
					if (!string.IsNullOrEmpty(unionId) && memberInfo.UnionId != unionId && !flag2 && MemberProcessor.GetMemberByUnionId(unionId) == null)
					{
						memberInfo.UnionId = unionId;
					}
					if (flag2)
					{
						if (!flag3)
						{
							MemberOpenIdInfo memberOpenIdInfo = new MemberOpenIdInfo();
							memberOpenIdInfo.UserId = memberInfo.UserId;
							memberOpenIdInfo.OpenIdType = "hishop.plugins.openid.wxapplet";
							memberOpenIdInfo.OpenId = text;
							MemberProcessor.AddMemberOpenId(memberOpenIdInfo);
							memberInfo.IsQuickLogin = true;
						}
						else
						{
							MemberOpenIdInfo memberOpenIdInfo2 = new MemberOpenIdInfo();
							memberOpenIdInfo2.UserId = memberInfo.UserId;
							memberOpenIdInfo2.OpenIdType = "hishop.plugins.openid.wxapplet";
							memberOpenIdInfo2.OpenId = text;
							MemberProcessor.UpdateMemberOpenId(memberOpenIdInfo2);
						}
					}
					MemberProcessor.UpdateMember(memberInfo);
					Users.SetCurrentUser(memberInfo.UserId, 30, true, false);
					HiContext.Current.User = memberInfo;
					if (!string.IsNullOrEmpty(text))
					{
						HttpCookie httpCookie = new HttpCookie("openId");
						httpCookie.HttpOnly = true;
						httpCookie.Value = text;
						httpCookie.Expires = DateTime.MaxValue;
						HttpContext.Current.Response.Cookies.Add(httpCookie);
					}
					num = 1;
				}
				else
				{
					num = 0;
				}
				if (num == 1)
				{
					if (flag && masterSettings.IsOpenGiftCoupons)
					{
						int num3 = 0;
						string[] array = masterSettings.GiftCouponList.Split(',');
						foreach (string obj in array)
						{
							if (obj.ToInt(0) > 0 && CouponHelper.AddCouponItemInfo(HiContext.Current.User, obj.ToInt(0)) == CouponActionStatus.Success)
							{
								num3++;
							}
						}
					}
					this.GetMember(memberInfo, text);
				}
				else
				{
					this.GetMember(null, text);
				}
			}
			catch (Exception ex)
			{
				IDictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("OpenId", text);
				Globals.WriteExceptionLog(ex, dictionary, "QuickLogin");
			}
		}

		protected int SkipWeixinOpenId(out MemberInfo memberInfo, out bool isNewRegisterUser)
		{
			int referralUserId = this.context.Request["referralUserId"].ToInt(0);
			isNewRegisterUser = true;
			int num = 1;
			string text = this.context.Request["openId"].ToNullString();
			string text2 = this.context.Request["nickName"].ToNullString();
			string unionId = this.getUnionId();
			string text3 = Globals.UrlDecode(this.context.Request["headImage"].ToNullString());
			memberInfo = MemberProcessor.GetMemberByOpenId("hishop.plugins.openid.wxapplet", text);
			bool flag = false;
			if (memberInfo == null)
			{
				memberInfo = MemberProcessor.GetMemberByUnionId(unionId);
				flag = true;
			}
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			bool flag2 = false;
			if (memberInfo != null)
			{
				memberInfo.IsLogined = true;
				bool flag3 = MemberProcessor.IsBindedWeixin(memberInfo.UserId, "hishop.plugins.openid.wxapplet");
				if (!string.IsNullOrEmpty(text3) || text3.StartsWith("http://wx.qlogo.cn/mmopen/"))
				{
					memberInfo.Picture = text3;
				}
				if (!string.IsNullOrEmpty(unionId) && memberInfo.UnionId != unionId && !flag && MemberProcessor.GetMemberByUnionId(unionId) == null)
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
						memberOpenIdInfo.OpenIdType = "hishop.plugins.openid.wxapplet";
						memberOpenIdInfo.OpenId = text;
						MemberProcessor.AddMemberOpenId(memberOpenIdInfo);
						memberInfo.IsQuickLogin = true;
						flag2 = true;
					}
					else
					{
						MemberOpenIdInfo memberOpenIdInfo2 = new MemberOpenIdInfo();
						memberOpenIdInfo2.UserId = memberInfo.UserId;
						memberOpenIdInfo2.OpenIdType = "hishop.plugins.openid.wxapplet";
						memberOpenIdInfo2.OpenId = text;
						MemberProcessor.UpdateMemberOpenId(memberOpenIdInfo2);
					}
				}
				if (flag2)
				{
					MemberProcessor.UpdateMember(memberInfo);
				}
				Users.SetCurrentUser(memberInfo.UserId, 30, true, false);
				HiContext.Current.User = memberInfo;
				if (!string.IsNullOrEmpty(text))
				{
					HttpCookie httpCookie = new HttpCookie("openId");
					httpCookie.HttpOnly = true;
					httpCookie.Value = text;
					httpCookie.Expires = DateTime.MaxValue;
					HttpContext.Current.Response.Cookies.Add(httpCookie);
				}
				isNewRegisterUser = false;
				return num;
			}
			memberInfo = new MemberInfo();
			memberInfo.IsLogined = true;
			if (!string.IsNullOrEmpty(text3) || text3.StartsWith("http://wx.qlogo.cn/mmopen/"))
			{
				memberInfo.Picture = text3;
			}
			int num2 = 0;
			memberInfo.GradeId = MemberProcessor.GetDefaultMemberGrade();
			if (!string.IsNullOrEmpty(text2))
			{
				MemberInfo obj = memberInfo;
				MemberInfo obj2 = memberInfo;
				string text6 = obj.UserName = (obj2.NickName = HttpUtility.UrlDecode(text2));
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
					memberInfo.UserName = this.GenerateUsername(10);
					if (MemberProcessor.FindMemberByUsername(memberInfo.UserName) != null)
					{
						num = -1;
					}
				}
			}
			if (num == 1)
			{
				string pass = this.GeneratePassword();
				string text7 = "Open";
				pass = Users.EncodePassword(pass, text7);
				memberInfo.Password = pass;
				memberInfo.PasswordSalt = text7;
				memberInfo.RegisteredSource = 6;
				memberInfo.CreateDate = DateTime.Now;
				memberInfo.IsQuickLogin = true;
				memberInfo.IsLogined = true;
				memberInfo.UnionId = unionId;
				memberInfo.ReferralUserId = referralUserId;
				num2 = MemberProcessor.CreateMember(memberInfo);
				if (num2 <= 0)
				{
					num = -1;
				}
			}
			if (num == 1)
			{
				memberInfo.UserId = num2;
				memberInfo.UserName = MemberHelper.GetUserName(num2);
				MemberHelper.Update(memberInfo, true);
				Users.SetCurrentUser(memberInfo.UserId, 30, false, false);
				HiContext.Current.User = memberInfo;
				if (!string.IsNullOrEmpty(text))
				{
					MemberOpenIdInfo memberOpenIdInfo3 = new MemberOpenIdInfo();
					memberOpenIdInfo3.UserId = memberInfo.UserId;
					memberOpenIdInfo3.OpenIdType = "hishop.plugins.openid.wxapplet";
					memberOpenIdInfo3.OpenId = text;
					if (MemberProcessor.GetMemberByOpenId(memberOpenIdInfo3.OpenIdType, text) == null)
					{
						MemberProcessor.AddMemberOpenId(memberOpenIdInfo3);
					}
					if (!string.IsNullOrEmpty(text))
					{
						HttpCookie httpCookie2 = new HttpCookie("openId");
						httpCookie2.HttpOnly = true;
						httpCookie2.Value = text;
						httpCookie2.Expires = DateTime.MaxValue;
						HttpContext.Current.Response.Cookies.Add(httpCookie2);
					}
				}
			}
			return num;
		}

		public void AddOrUpdateOpenId(int userId, string openIdType, string openId)
		{
			MemberOpenIdInfo memberOpenIdInfo = MemberProcessor.GetMemberOpenIdInfo(userId, openIdType);
			if (memberOpenIdInfo == null)
			{
				MemberOpenIdInfo memberOpenIdInfo2 = new MemberOpenIdInfo();
				memberOpenIdInfo2.UserId = userId;
				memberOpenIdInfo2.OpenIdType = openIdType;
				memberOpenIdInfo2.OpenId = openId;
				MemberProcessor.AddMemberOpenId(memberOpenIdInfo2);
			}
			else
			{
				MemberOpenIdInfo memberOpenIdInfo3 = new MemberOpenIdInfo();
				memberOpenIdInfo3.UserId = userId;
				memberOpenIdInfo3.OpenIdType = openIdType;
				memberOpenIdInfo3.OpenId = openId;
				MemberProcessor.UpdateMemberOpenId(memberOpenIdInfo3);
			}
		}

		private void ProcessLogout(HttpContext context)
		{
			this.ClearLoginStatus();
		}

		public void ClearLoginStatus()
		{
			this.BindUserByOpenId();
			try
			{
				if (HiContext.Current.User.UserId > 0)
				{
					if (HiContext.Current.User.IsQuickLogin)
					{
						MemberProcessor.DeleteMemberOpenId(HiContext.Current.User.UserId, "hishop.plugins.openid.wxapplet");
					}
					Users.SetLoginStatus(HiContext.Current.User.UserId, false);
				}
				HttpCookie httpCookie = HiContext.Current.Context.Request.Cookies["Token_" + HiContext.Current.UserId.ToString()];
				DateTime now;
				if (httpCookie != null && !string.IsNullOrEmpty(httpCookie.Value))
				{
					HttpCookie httpCookie2 = httpCookie;
					now = DateTime.Now;
					httpCookie2.Expires = now.AddDays(-1.0);
					HttpContext.Current.Response.Cookies.Add(httpCookie);
				}
				HttpCookie httpCookie3 = HiContext.Current.Context.Request.Cookies["Shop-Member"];
				if (httpCookie3 != null && !string.IsNullOrEmpty(httpCookie3.Value))
				{
					HttpCookie httpCookie4 = httpCookie3;
					now = DateTime.Now;
					httpCookie4.Expires = now.AddDays(-1.0);
					HttpContext.Current.Response.Cookies.Add(httpCookie3);
				}
				HttpCookie httpCookie5 = HttpContext.Current.Request.Cookies["Site_ReferralUser"];
				if (httpCookie5 != null && !string.IsNullOrEmpty(httpCookie5.Value))
				{
					httpCookie5.Expires = new DateTime(1911, 10, 12);
					HttpContext.Current.Response.Cookies.Add(httpCookie5);
				}
				HttpCookie httpCookie6 = HttpContext.Current.Request.Cookies["Store_ShoppingGuider"];
				if (httpCookie6 != null && !string.IsNullOrEmpty(httpCookie6.Value))
				{
					httpCookie6.Expires = new DateTime(1911, 10, 12);
					HttpContext.Current.Response.Cookies.Add(httpCookie6);
				}
				HttpCookie httpCookie7 = HiContext.Current.Context.Request.Cookies["UserCoordinateCookie"];
				if (httpCookie7 != null && !string.IsNullOrEmpty(httpCookie7.Value))
				{
					httpCookie7.Expires = new DateTime(1911, 10, 12);
					HttpContext.Current.Response.Cookies.Add(httpCookie7);
				}
				HttpCookie httpCookie8 = HiContext.Current.Context.Request.Cookies["UserCoordinateTimeCookie"];
				if (httpCookie8 != null && !string.IsNullOrEmpty(httpCookie8.Value))
				{
					httpCookie8.Expires = new DateTime(1911, 10, 12);
					HttpContext.Current.Response.Cookies.Add(httpCookie8);
				}
				HttpCookie httpCookie9 = HiContext.Current.Context.Request.Cookies["openId"];
				if (httpCookie9 != null && !string.IsNullOrEmpty(httpCookie9.Value))
				{
					httpCookie9.Expires = new DateTime(1911, 10, 12);
					HttpContext.Current.Response.Cookies.Add(httpCookie9);
				}
				HiContext.Current.UserId = 0;
				HiContext.Current.User = null;
			}
			catch
			{
			}
		}

		private int GetOrderStatus(OrderInfo order)
		{
			int num = 0;
			if (order.ShippingModeId == -2 && !order.IsConfirm && (order.OrderStatus == OrderStatus.WaitBuyerPay || order.OrderStatus == OrderStatus.BuyerAlreadyPaid) && order.ItemStatus == OrderItemStatus.Nomarl)
			{
				return 7;
			}
			if (order.OrderStatus == OrderStatus.WaitBuyerPay && order.Gateway == "hishop.plugins.payment.podrequest")
			{
				return 2;
			}
			switch (order.OrderStatus)
			{
			case OrderStatus.WaitBuyerPay:
				return 1;
			case OrderStatus.BuyerAlreadyPaid:
				return 2;
			case OrderStatus.SellerAlreadySent:
				return 3;
			case OrderStatus.Closed:
				return 4;
			case OrderStatus.Finished:
				return 5;
			default:
				return -1;
			}
		}

		private string GetOrderStatusText(OrderInfo order)
		{
			string empty = string.Empty;
			if (order.ExpressCompanyName == "同城物流配送")
			{
				return EnumDescription.GetEnumDescription((Enum)(object)order.DadaStatus, 0);
			}
			if (order.ShippingModeId == -2 && (order.OrderStatus == OrderStatus.WaitBuyerPay || order.OrderStatus == OrderStatus.BuyerAlreadyPaid) && order.ItemStatus == OrderItemStatus.Nomarl)
			{
				if (!order.IsConfirm)
				{
					return "门店配货中";
				}
				return "待上门自提";
			}
			if (order.OrderStatus == OrderStatus.WaitBuyerPay && order.Gateway == "hishop.plugins.payment.podrequest")
			{
				return "等待发货";
			}
			if (order.PreSaleId > 0 && order.OrderStatus == OrderStatus.WaitBuyerPay)
			{
				if (!order.DepositDate.HasValue)
				{
					return "等待支付定金";
				}
				ProductPreSaleInfo productPreSaleInfo = ProductPreSaleHelper.GetProductPreSaleInfo(order.PreSaleId);
				if (productPreSaleInfo.PaymentStartDate > DateTime.Now)
				{
					return "等待尾款支付开始";
				}
				return "等待支付尾款";
			}
			return ((Enum)(object)order.OrderStatus).ToDescription();
		}

		private void GetMember(MemberInfo member, string openId)
		{
			if (member == null)
			{
				string s = JsonConvert.SerializeObject(new
				{
					Status = "OK",
					Data = new
					{
						IsBindUser = false
					}
				});
				this.context.Response.Write(s);
			}
			else
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				MemberGradeInfo memberGrade = MemberProcessor.GetMemberGrade(member.GradeId);
				string gradeName = (memberGrade == null) ? "" : memberGrade.Name;
				member.Referral = MemberHelper.GetReferral(member.UserId);
				OrderQuery orderQuery = new OrderQuery();
				orderQuery.IsServiceOrder = false;
				orderQuery.IsAllOrder = false;
				orderQuery.ShowGiftOrder = false;
				orderQuery.Status = OrderStatus.WaitBuyerPay;
				orderQuery.IsIncludePreSaleOrder = false;
				int userOrderCount = MemberProcessor.GetUserOrderCount(member.UserId, orderQuery);
				orderQuery.Status = OrderStatus.SellerAlreadySent;
				int userOrderCount2 = MemberProcessor.GetUserOrderCount(member.UserId, orderQuery);
				orderQuery.Status = OrderStatus.BuyerAlreadyPaid;
				int userOrderCount3 = MemberProcessor.GetUserOrderCount(member.UserId, orderQuery);
				orderQuery.Status = OrderStatus.WaitReview;
				int userOrderCount4 = MemberProcessor.GetUserOrderCount(member.UserId, orderQuery);
				orderQuery.Status = OrderStatus.All;
				orderQuery.IsAfterSales = true;
				int userAfterSaleCount = MemberProcessor.GetUserAfterSaleCount(member.UserId, false, ProductType.PhysicalProduct);
				int couponsCount = 0;
				DataTable userCoupons = CouponHelper.GetUserCoupons(member.UserId, 1);
				if (userCoupons != null && userCoupons.Rows.Count > 0)
				{
					couponsCount = userCoupons.Rows.Count;
				}
				ReferralGradeInfo referralGradeInfo = (member.Referral == null) ? null : MemberProcessor.GetNextReferralGradeInfo(member.Referral.GradeId);
				decimal num = default(decimal);
				decimal num2 = default(decimal);
				num2 = ((member.Referral == null) ? decimal.Zero : MemberProcessor.GetSplittinTotal(member.UserId).F2ToString("f2").ToDecimal(0));
				if (referralGradeInfo != null)
				{
					num = referralGradeInfo.CommissionThreshold - num2;
					if (num < decimal.Zero)
					{
						num = default(decimal);
					}
				}
				SplittinDrawInfo myRecentlySplittinDraws = MemberProcessor.GetMyRecentlySplittinDraws();
				string s2 = JsonConvert.SerializeObject(new
				{
					Status = "OK",
					Data = new
					{
						IsBindUser = true,
						couponsCount = couponsCount,
						picture = (string.IsNullOrEmpty(member.Picture) ? Globals.HttpsFullPath("/templates/common/images/headerimg.png") : ((member.Picture.StartsWith("http://") || member.Picture.StartsWith("https://")) ? member.Picture : Globals.HttpsFullPath(member.Picture))),
						points = member.Points,
						waitPayCount = userOrderCount,
						waitSendCount = userOrderCount3,
						waitFinishCount = userOrderCount2,
						waitReviewCount = userOrderCount4,
						afterSalesCount = userAfterSaleCount,
						realName = (string.IsNullOrEmpty(member.NickName) ? (string.IsNullOrEmpty(member.RealName) ? member.UserName : member.RealName) : member.NickName),
						gradeId = member.GradeId,
						gradeName = gradeName,
						UserName = member.UserName,
						UserId = member.UserId,
						OpenId = openId,
						ServicePhone = masterSettings.ServicePhone,
						IsReferral = member.IsReferral(),
						ReferralStatus = ((member.Referral != null) ? member.Referral.ReferralStatus : 0),
						ReferralStatusText = ((member.Referral == null) ? "" : EnumDescription.GetEnumDescription((Enum)(object)(ReferralApplyStatus)member.Referral.ReferralStatus, 0)),
						Expenditure = member.Expenditure.F2ToString("f2").ToDecimal(0),
						OrderNumber = member.OrderNumber,
						CellPhone = member.CellPhone,
						CellPhoneVerification = member.CellPhoneVerification,
						IsTrustLogon = MemberProcessor.IsTrustLoginUser(member),
						IsOpenBalance = member.IsOpenBalance,
						EmailVerification = member.EmailVerification,
						Email = member.Email
					}
				});
				this.context.Response.Write(s2);
			}
		}

		public string AESDecrypt(string AesKey, string AesIV, string inputdata)
		{
			try
			{
				AesIV = AesIV.Replace(" ", "+");
				AesKey = AesKey.Replace(" ", "+");
				inputdata = inputdata.Replace(" ", "+");
				byte[] array = Convert.FromBase64String(inputdata);
				RijndaelManaged rijndaelManaged = new RijndaelManaged();
				rijndaelManaged.Key = Convert.FromBase64String(AesKey);
				rijndaelManaged.IV = Convert.FromBase64String(AesIV);
				rijndaelManaged.Mode = CipherMode.CBC;
				rijndaelManaged.Padding = PaddingMode.PKCS7;
				ICryptoTransform cryptoTransform = rijndaelManaged.CreateDecryptor();
				byte[] bytes = cryptoTransform.TransformFinalBlock(array, 0, array.Length);
				return Encoding.UTF8.GetString(bytes);
			}
			catch (Exception ex)
			{
				IDictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("AesKey", AesKey);
				dictionary.Add("AesIV", AesIV);
				dictionary.Add("inputdata", inputdata);
				Globals.WriteExceptionLog(ex, dictionary, "AESDecrypt");
				return "";
			}
		}

		private string GetImageFullPath(string imageUrl)
		{
			if (string.IsNullOrEmpty(imageUrl))
			{
				return Globals.HttpsFullPath(HiContext.Current.SiteSettings.DefaultProductThumbnail8);
			}
			if (imageUrl.StartsWith("http://"))
			{
				return imageUrl;
			}
			return Globals.HttpsFullPath(imageUrl);
		}

		private string GenerateUsername(int length)
		{
			return this.GenerateRndString(length, "h");
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

		private string GetErrorJson(int code, string errorMsg)
		{
			return JsonConvert.SerializeObject(new
			{
				error_response = new
				{
					code = code,
					sub_msg = errorMsg
				}
			});
		}

		private string GetErrorJson(string errorMsg)
		{
			return JsonConvert.SerializeObject(new
			{
				Status = "NO",
				Message = errorMsg
			});
		}

		private string GetOKJson(string okMsg)
		{
			return JsonConvert.SerializeObject(new
			{
				Status = "OK",
				Message = okMsg
			});
		}

		private void FilterShoppingCart(ShoppingCartInfo cartInfo)
		{
			cartInfo?.LineGifts.Clear();
		}
	}
}
