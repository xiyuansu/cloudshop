using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Core.Urls;
using Hidistro.Entities;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Entities.VShop;
using Hidistro.Messages;
using Hidistro.SaleSystem;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Comments;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Members;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Shopping;
using Hidistro.SaleSystem.Statistics;
using Hidistro.SaleSystem.Store;
using Hidistro.SaleSystem.Vshop;
using Hidistro.SqlDal.Commodities;
using Hidistro.SqlDal.Members;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.CodeBehind;
using Hidistro.UI.Web.AppShop;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Senparc.Weixin.MP.CommonAPIs;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;

namespace Hidistro.UI.Web.API
{
	public class VshopProcess : IHttpHandler, IRequiresSessionState
	{
		private IDictionary<string, string> jsondict = new Dictionary<string, string>();

		private Regex emailR = new Regex("^\\w+((-\\w+)|(\\.\\w+))*\\@[A-Za-z0-9]+((\\.|-)[A-Za-z0-9]+)*\\.[A-Za-z0-9]+$", RegexOptions.Compiled);

		private Regex cellphoneR = new Regex("^0?(13|15|18|14|17)[0-9]{9}$", RegexOptions.Compiled);

		private object lockCopyRedEnvelope = new object();

		private static object submitLock = new object();

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public void ProcessRequest(HttpContext context)
		{
			string text = context.Request["action"];
			switch (text)
			{
			case "GetProduct":
				this.GetProduct(context);
				break;
			case "getCategories":
				this.GetCategories(context);
				break;
			case "getAllCategories":
				this.GetAllCategories(context);
				break;
			case "getProducts":
				this.GetProducts(context);
				break;
			case "GetFightGroupActivity":
				this.GetFightGroupActivity(context);
				break;
			case "GenerateTwoDimensionalImage":
				this.GenerateTwoDimensionalImage(context);
				break;
			case "IsDuplicateBuyCountDown":
				this.IsDuplicateBuyCountDown(context);
				break;
			case "AddToCartBySkus":
				this.ProcessAddToCartBySkus(context);
				break;
			case "GetSkuByOptions":
				this.ProcessGetSkuByOptions(context);
				break;
			case "DeleteCartProduct":
				this.ProcessDeleteCartProduct(context);
				break;
			case "ChageQuantity":
				this.ProcessChageQuantity(context);
				break;
			case "Submmitorder":
				this.ProcessSubmmitorder(context);
				break;
			case "AddShippingAddress":
				this.AddShippingAddress(context);
				break;
			case "DelShippingAddress":
				this.DelShippingAddress(context);
				break;
			case "SetDefaultShippingAddress":
				this.SetDefaultShippingAddress(context);
				break;
			case "UpdateShippingAddress":
				this.UpdateShippingAddress(context);
				break;
			case "GetPrize":
				this.GetPrize(context);
				break;
			case "SubmitActivity":
				this.SubmitActivity(context);
				break;
			case "AddSignUp":
				this.AddSignUp(context);
				break;
			case "AddTicket":
				this.AddTicket(context);
				break;
			case "FinishOrder":
				this.FinishOrder(context);
				break;
			case "CloseOrder":
				this.CloseOrder(context);
				break;
			case "AddUserPrize":
				this.AddUserPrize(context);
				break;
			case "SubmitWinnerInfo":
				this.SubmitWinnerInfo(context);
				break;
			case "SetUserName":
				this.SetUserName(context);
				break;
			case "BindUser":
				this.BindUser(context);
				break;
			case "SkipBindUser":
				this.SkipBindUser(context);
				break;
			case "RegisterUser":
				this.RegisterUser(context);
				break;
			case "AddProductConsultations":
				this.AddProductConsultations(context);
				break;
			case "AddProductReview":
				this.AddProductReview(context);
				break;
			case "AddFavorite":
				this.AddFavorite(context);
				break;
			case "DelFavorite":
				this.DelFavorite(context);
				break;
			case "CheckFavorite":
				this.CheckFavorite(context);
				break;
			case "Logistic":
				this.SearchExpressData(context);
				break;
			case "ReturnLogistic":
				this.SearchReturnExpressData(context);
				break;
			case "ReplaceLogistic":
				this.SearchReplaceExpressData(context);
				break;
			case "Transactionsubmitorder":
				this.submitorder(context);
				break;
			case "ReferralRegister":
				this.ReferralRegister(context);
				break;
			case "SplittinDraws":
				this.SplittinDraws(context);
				break;
			case "Vote":
				this.Vote(context);
				break;
			case "OpenBalance":
				this.OpenBalance(context);
				break;
			case "RequestBalanceDraw":
				this.RequestBalanceDraw(context);
				break;
			case "GetCanShipStores":
				this.GetCanShipStores(context);
				break;
			case "GetSkuReferralDeduct":
				this.GetSkuReferralDeduct(context);
				break;
			case "ApplyRefund":
				this.ApplyRefund(context);
				break;
			case "ApplyReturn":
				this.ApplyReturn(context);
				break;
			case "ReturnSendGoods":
				this.ReturnSendGoods(context);
				break;
			case "CheckSendRedEnvelope":
				this.CheckSendRedEnvelope(context);
				break;
			case "AddRedEnvelopeSendRecord":
				this.AddRedEnvelopeSendRecord(context);
				break;
			case "ChageGiftQuantity":
				this.ChageGiftQuantity(context);
				break;
			case "DeleteCartGift":
				this.DeleteCartGift(context);
				break;
			case "signIn":
				this.SignIn(context);
				break;
			case "CheckHasPrized":
				this.CheckHasPrized(context);
				break;
			case "IsUserPrize":
				this.IsUserPrize(context);
				break;
			case "ExChangeGifts":
				this.ExChangeGifts(context);
				break;
			case "BindPhone":
				this.BindPhone(context);
				break;
			case "BindEmail":
				this.BindEmail(context);
				break;
			case "SetPassword":
				this.SetPassword(context);
				break;
			case "ChangePassword":
				this.ChangePassword(context);
				break;
			case "ChangeTranPassword":
				this.ChangeTranPassword(context);
				break;
			case "CheckEmailCode":
				this.CheckEmailCode(context);
				break;
			case "CheckPhoneCode":
				this.CheckPhoneCode(context);
				break;
			case "GetProductFreight":
				this.GetProductFreight(context);
				break;
			case "UpdatePaymentType":
				this.UpdatePaymentType(context);
				break;
			case "AddInpourBlance":
				this.AddInpourBlance(context);
				break;
			case "GetWXShareInfo":
				this.GetWXShareInfo(context);
				break;
			case "PayCheckOrder":
				this.PayCheckOrder(context);
				break;
			case "AdvancePayPassVerify":
				this.AdvancePayPassVerify(context);
				break;
			case "StoreChageQuantity":
				this.StoreChageQuantity(context);
				break;
			case "GetProductConsultation":
				this.GetProductConsultation(context);
				break;
			case "UpdateInformationMember":
				this.UpdateInformationMember(context);
				break;
			case "UploadNameVerify":
				this.UploadNameVerify(context);
				break;
			case "ResetTradePassword":
				this.ResetTradePassword(context);
				break;
			case "ServiceProductSubmitorder":
				this.ServiceProductSubmitorder(context);
				break;
			case "GetLocationInfo":
				this.GetLocationInfo(context);
				break;
			case "GetProductFreightOfLatLng":
				this.GetProductFreightOfLatLng(context);
				break;
			case "UpdateReferralSet":
				this.UpdateReferralSet(context);
				break;
			case "CellPhoneRegister":
				this.CellPhoneRegister(context);
				break;
			case "OpenIdBindUser":
				this.OpenIdBindUser(context);
				break;
			}
		}

		public void OpenIdBindUser(HttpContext context)
		{
			Hidistro.Entities.Members.MemberInfo memberInfo = null;
			string text = context.Request["openId"].ToNullString();
			string text2 = context.Request["openIdType"].ToNullString();
			string text3 = context.Request["nickName"].ToNullString();
			string text4 = context.Request["unionId"].ToNullString();
			string text5 = Globals.UrlDecode(context.Request["headImage"].ToNullString());
			string userName = context.Request["username"].ToNullString();
			string password = context.Request["password"].ToNullString();
			string text6 = context.Request["from"].ToNullString();
			text2 = ((!string.IsNullOrEmpty(text2)) ? text2.ToLower() : "hishop.plugins.openid.weixin");
			ApiErrorCode apiErrorCode;
			if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(text2))
			{
				apiErrorCode = ApiErrorCode.Paramter_Error;
				this.ShowMessageAndCode(context, "缺少必填参数", apiErrorCode.GetHashCode());
			}
			else
			{
				memberInfo = ((!(text2 == "hishop.plugins.openid.weixin")) ? MemberProcessor.GetMemberByOpenId(text2, text) : MemberProcessor.GetMemberByOpenIdOfQuickLogin(text2, text));
				if (memberInfo == null && !string.IsNullOrEmpty(text4))
				{
					memberInfo = MemberProcessor.GetMemberByUnionId(text4);
				}
				Hidistro.Entities.Members.MemberInfo memberInfo2 = MemberProcessor.ValidLogin(userName, password);
				if (memberInfo2 == null)
				{
					apiErrorCode = ApiErrorCode.Password_Error;
					this.ShowMessageAndCode(context, "用户名或者密码错误", apiErrorCode.GetHashCode());
				}
				else
				{
					MemberOpenIdInfo memberOpenIdInfo = MemberProcessor.GetMemberOpenIdInfo(memberInfo2.UserId, text2);
					if (memberOpenIdInfo != null && memberOpenIdInfo.OpenId != text)
					{
						apiErrorCode = ApiErrorCode.UserHasBindTrustLogin;
						this.ShowMessageAndCode(context, "该用户已是绑定过其它信任登录帐号,请换一个帐号", apiErrorCode.GetHashCode());
					}
					else
					{
						if (text2 == "hishop.plugins.openid.weixin")
						{
							memberInfo2.IsQuickLogin = true;
						}
						memberInfo2.IsLogined = true;
						memberInfo2.UnionId = text4;
						MemberProcessor.UpdateMember(memberInfo2);
						if (!string.IsNullOrEmpty(text) && memberOpenIdInfo == null)
						{
							MemberOpenIdInfo memberOpenIdInfo2 = new MemberOpenIdInfo();
							memberOpenIdInfo2.UserId = memberInfo2.UserId;
							memberOpenIdInfo2.OpenIdType = text2;
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
						apiErrorCode = ApiErrorCode.Success;
						this.ShowMessageAndCode(context, "绑定用户成功", apiErrorCode.GetHashCode());
					}
				}
			}
		}

		private void CellPhoneRegister(HttpContext context)
		{
			Hidistro.Entities.Members.MemberInfo memberInfo = null;
			string text = context.Request["openId"].ToNullString();
			string text2 = context.Request["nickName"].ToNullString();
			int referralUserId = context.Request["referralUserId"].ToInt(0);
			string text3 = context.Request["openIdType"].ToNullString();
			string text4 = context.Request["unionId"].ToNullString();
			string text5 = Globals.UrlDecode(context.Request["headImage"].ToNullString());
			string text6 = context.Request["cellphone"].ToNullString();
			string verifyCode = context.Request["VerfiyCode"].ToNullString();
			string text7 = context.Request["password"].ToNullString();
			string text8 = context.Request["from"].ToNullString();
			text3 = ((!string.IsNullOrEmpty(text3)) ? text3.ToLower() : "hishop.plugins.openid.weixin");
			ApiErrorCode apiErrorCode;
			if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(text3))
			{
				apiErrorCode = ApiErrorCode.Paramter_Error;
				this.ShowMessageAndCode(context, "缺少必填参数", apiErrorCode.GetHashCode());
			}
			else if (text7.Length < 6 || text7.Length > 20)
			{
				apiErrorCode = ApiErrorCode.Paramter_Error;
				this.ShowMessageAndCode(context, "请输入用户密码，密码长度为6-20位", apiErrorCode.GetHashCode());
			}
			else if (string.IsNullOrEmpty(text6) || !DataHelper.IsMobile(text6))
			{
				apiErrorCode = ApiErrorCode.Paramter_Error;
				this.ShowMessageAndCode(context, "请输入正确的手机号码", apiErrorCode.GetHashCode());
			}
			else
			{
				string text9 = "";
				if (!HiContext.Current.CheckPhoneVerifyCode(verifyCode, text6, out text9))
				{
					apiErrorCode = ApiErrorCode.Paramter_Error;
					this.ShowMessageAndCode(context, "手机验证码错误", apiErrorCode.GetHashCode());
				}
				else if (MemberProcessor.FindMemberByCellphone(text6) != null)
				{
					apiErrorCode = ApiErrorCode.MobbileIsBinding;
					this.ShowMessageAndCode(context, "手机号已被其它帐号绑定", apiErrorCode.GetHashCode());
				}
				else
				{
					memberInfo = ((!(text3 == "hishop.plugins.openid.weixin")) ? MemberProcessor.GetMemberByOpenId(text3, text) : MemberProcessor.GetMemberByOpenIdOfQuickLogin(text3, text));
					bool flag = false;
					if (memberInfo == null)
					{
						memberInfo = MemberProcessor.GetMemberByUnionId(text4);
						if (memberInfo != null)
						{
							flag = true;
						}
					}
					if (memberInfo?.CellPhoneVerification ?? false)
					{
						apiErrorCode = ApiErrorCode.MobbileIsBinding;
						this.ShowMessageAndCode(context, "帐号已经绑定了手机号码", apiErrorCode.GetHashCode());
					}
					else if (memberInfo != null && memberInfo.Password != Users.EncodePassword(text7, memberInfo.PasswordSalt))
					{
						apiErrorCode = ApiErrorCode.Password_Error;
						this.ShowMessageAndCode(context, "用户密码错误", apiErrorCode.GetHashCode());
					}
					else
					{
						if (memberInfo != null)
						{
							bool flag2 = MemberProcessor.IsBindedWeixin(memberInfo.UserId, text3);
							if (text3 != "hishop.plugins.openid.weixin" & flag2)
							{
								apiErrorCode = ApiErrorCode.ExistTrustLogin;
								this.ShowMessageAndCode(context, "信任登录与其它用户关联", apiErrorCode.GetHashCode());
								return;
							}
							memberInfo.CellPhone = text6;
							memberInfo.CellPhoneVerification = true;
							if (text3 == "hishop.plugins.openid.weixin")
							{
								memberInfo.IsQuickLogin = true;
							}
							memberInfo.IsQuickLogin = true;
							memberInfo.IsLogined = true;
							MemberProcessor.UpdateMember(memberInfo);
							if (!string.IsNullOrEmpty(text5) || text5.StartsWith("http://wx.qlogo.cn/mmopen/"))
							{
								memberInfo.Picture = text5;
							}
							if (!string.IsNullOrEmpty(text4) && memberInfo.UnionId != text4 && !flag && MemberProcessor.GetMemberByUnionId(text4) == null)
							{
								memberInfo.UnionId = text4;
							}
							if (flag)
							{
								if (!flag2)
								{
									MemberOpenIdInfo memberOpenIdInfo = new MemberOpenIdInfo();
									memberOpenIdInfo.UserId = memberInfo.UserId;
									memberOpenIdInfo.OpenIdType = text3;
									memberOpenIdInfo.OpenId = text;
									MemberProcessor.AddMemberOpenId(memberOpenIdInfo);
									memberInfo.IsQuickLogin = true;
								}
								else
								{
									MemberOpenIdInfo memberOpenIdInfo2 = new MemberOpenIdInfo();
									memberOpenIdInfo2.UserId = memberInfo.UserId;
									memberOpenIdInfo2.OpenIdType = text3;
									memberOpenIdInfo2.OpenId = text;
									MemberProcessor.UpdateMemberOpenId(memberOpenIdInfo2);
								}
							}
							MemberProcessor.UpdateMember(memberInfo);
							if (!string.IsNullOrEmpty(text))
							{
								HttpCookie httpCookie = new HttpCookie("openId");
								httpCookie.HttpOnly = true;
								httpCookie.Value = text;
								httpCookie.Expires = DateTime.MaxValue;
								HttpContext.Current.Response.Cookies.Add(httpCookie);
							}
							Users.ClearUserCache(memberInfo.UserId, "");
							Users.SetCurrentUser(memberInfo.UserId, 30, false, false);
							HiContext.Current.User = memberInfo;
						}
						else
						{
							int num = 1;
							memberInfo = new Hidistro.Entities.Members.MemberInfo();
							memberInfo.IsLogined = true;
							if (!string.IsNullOrEmpty(text5) || text5.StartsWith("http://wx.qlogo.cn/mmopen/"))
							{
								memberInfo.Picture = text5;
							}
							int num2 = 0;
							memberInfo.GradeId = MemberProcessor.GetDefaultMemberGrade();
							memberInfo.UserName = text6;
							memberInfo.CellPhone = text6;
							memberInfo.CellPhoneVerification = true;
							if (!string.IsNullOrEmpty(text2))
							{
								memberInfo.NickName = HttpUtility.UrlDecode(text2);
							}
							memberInfo.ReferralUserId = referralUserId;
							string text10 = "Open";
							text7 = (memberInfo.Password = Users.EncodePassword(text7, text10));
							memberInfo.PasswordSalt = text10;
							string text12 = Globals.StripAllTags(context.Request["client"].ToNullString());
							if (string.IsNullOrEmpty(text12))
							{
								text12 = "wap";
							}
							text12 = text12.ToLower();
							memberInfo.RegisteredSource = ((text12 == "wap") ? 2 : ((text12 == "alioh") ? 4 : 3));
							memberInfo.CreateDate = DateTime.Now;
							if (text3 == "hishop.plugins.openid.weixin")
							{
								memberInfo.IsQuickLogin = true;
							}
							memberInfo.IsLogined = true;
							memberInfo.UnionId = text4;
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
									memberOpenIdInfo3.OpenIdType = text3;
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
						}
						apiErrorCode = ApiErrorCode.Success;
						this.ShowMessageAndCode(context, "手机号注册绑定成功", apiErrorCode.GetHashCode());
					}
				}
			}
		}

		public void UpdateReferralSet(HttpContext context)
		{
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
			if (HiContext.Current.UserId == 0)
			{
				this.ShowMessage(context, "请您先登录！", true);
			}
			else if (!HiContext.Current.User.IsReferral())
			{
				this.ShowMessage(context, "您现在还是分销员,请先申请成为分销员", true);
			}
			else if (string.IsNullOrEmpty(text3) || text3.Length > 100)
			{
				this.ShowMessage(context, "请输入店铺名称,长度在1-10个之间", true);
			}
			else
			{
				if (text.Length > 0 && !DataHelper.IsEmail(text))
				{
					this.ShowMessage(context, "请输入正确的邮箱地址", true);
				}
				if (text2.Length > 0 && !DataHelper.IsMobile(text2))
				{
					this.ShowMessage(context, "请输入正确的手机号码", true);
				}
				else
				{
					ReferralInfo referral = HiContext.Current.User.Referral;
					referral.BannerUrl = text4;
					referral.CellPhone = text2;
					referral.Email = text;
					referral.ShopName = text3;
					if (MemberProcessor.ReferralInfoSet(referral))
					{
						Users.ClearUserCache(HiContext.Current.UserId, HiContext.Current.User.SessionId);
						this.ShowMessage(context, "店铺信息保存成功！", false);
					}
					else
					{
						this.ShowMessage(context, "店铺信息保存失败！", false);
					}
				}
			}
		}

		public int GetRegionOfLatLng(string locations, out string msg)
		{
			msg = "";
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			string str = string.IsNullOrEmpty(masterSettings.QQMapAPIKey) ? "SYJBZ-DSLR3-IWX3Q-3XNTM-ELURH-23FTP" : masterSettings.QQMapAPIKey;
			string url = string.Format("https://apis.map.qq.com/ws/geocoder/v1/?key=" + str + "&get_poi=0&location=" + locations);
			string responseResult = Globals.GetResponseResult(url);
			try
			{
				int result = 0;
				QQMapLocationResult qQMapLocationResult = JsonHelper.ParseFormJson<QQMapLocationResult>(responseResult);
				if (qQMapLocationResult != null && qQMapLocationResult.status == 0)
				{
					result = RegionHelper.GetRegionId(qQMapLocationResult.result.address_component.district, qQMapLocationResult.result.address_component.city, qQMapLocationResult.result.address_component.province);
				}
				return result;
			}
			catch (Exception ex)
			{
				msg = ex.Message;
				return 0;
			}
		}

		private void GetProductFreightOfLatLng(HttpContext context)
		{
			string locations = context.Request["locations"].ToNullString();
			string str = "";
			int regionOfLatLng = this.GetRegionOfLatLng(locations, out str);
			if (regionOfLatLng > 0)
			{
				HiContext.Current.DeliveryScopRegionId = regionOfLatLng;
				this.GetProductFreight(context);
			}
			else
			{
				context.Response.Write("{\"Status\":\"Error\",\"Freight\":\"0\",\"Message\":\"" + str + "\"}");
				context.Response.End();
			}
		}

		public void PaySendMessage(OrderInfo order)
		{
			if (order.FightGroupId > 0)
			{
				VShopHelper.SetFightGroupSuccess(order.FightGroupId);
			}
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			if (order.GroupBuyId > 0)
			{
				GroupBuyInfo groupBuy = TradeHelper.GetGroupBuy(order.GroupBuyId);
				if (groupBuy != null && groupBuy.Status == GroupBuyStatus.UnderWay)
				{
					num2 = TradeHelper.GetOrderCount(order.GroupBuyId);
					num3 = order.GetGroupBuyOerderNumber();
					num = groupBuy.MaxCount;
				}
			}
			if (order.GroupBuyId > 0 && num == num2 + num3)
			{
				TradeHelper.SetGroupBuyEndUntreated(order.GroupBuyId);
			}
			if (order.ParentOrderId == "-1")
			{
				OrderQuery orderQuery = new OrderQuery();
				orderQuery.ParentOrderId = order.OrderId;
				IList<OrderInfo> listUserOrder = MemberProcessor.GetListUserOrder(order.UserId, orderQuery);
				foreach (OrderInfo item in listUserOrder)
				{
					OrderHelper.OrderConfirmPaySendMessage(item);
				}
			}
			else
			{
				OrderHelper.OrderConfirmPaySendMessage(order);
			}
		}

		private void GetLocationInfo(HttpContext context)
		{
			string locations = context.Request["locations"].ToNullString();
			string text = "";
			int regionOfLatLng = this.GetRegionOfLatLng(locations, out text);
			if (!string.IsNullOrEmpty(text))
			{
				context.Response.Write("{\"Status\":\"Error\",\"RegionId\":\"0\",\"Message\":\"" + text + "\"}");
				context.Response.End();
			}
			else
			{
				context.Response.Write("{\"Status\":\"OK\",\"RegionId\":\"" + regionOfLatLng + "\"}");
				context.Response.End();
			}
		}

		private void ServiceProductSubmitorder(HttpContext context)
		{
			NameValueCollection param = new NameValueCollection
			{
				context.Request.Form,
				context.Request.QueryString
			};
			try
			{
				string from = "serviceproduct";
				string text = context.Request["couponCode"];
				int num = context.Request["buyAmount"].ToInt(0);
				string text2 = Globals.UrlDecode(context.Request["productSku"]);
				string remark = DataHelper.CleanSearchString(Globals.UrlDecode(context.Request["remark"]));
				int num2 = context.Request["deductionPoints"].ToInt(0);
				int num3 = context.Request["storeId"].ToInt(0);
				bool flag = false;
				flag = context.Request["needInvoice"].ToBool();
				string text3 = context.Request["invoiceTitle"].ToNullString();
				InvoiceType invoiceType = (InvoiceType)context.Request["invoiceType"].ToInt(0);
				string text4 = context.Request["invoiceTaxpayerNumber"].ToNullString();
				decimal num4 = context.Request["UseBalance"].ToDecimal(0);
				string text5 = context.Request["advancePayPass"].ToNullString();
				if (num <= 0)
				{
					this.jsondict.Add("Status", "Error");
					this.jsondict.Add("ErrorMsg", "错误的购买数量");
					this.jsondict.Add("ErrorUrl", "");
					this.WriteJson(context, 0);
				}
				else
				{
					if (num4 > decimal.Zero)
					{
						if (string.IsNullOrEmpty(text5))
						{
							this.jsondict.Add("Status", "Error");
							this.jsondict.Add("ErrorMsg", "请输入交易密码");
							this.jsondict.Add("ErrorUrl", "");
							this.WriteJson(context, 0);
							goto end_IL_002b;
						}
						SiteSettings masterSettings = SettingsManager.GetMasterSettings();
						if (!masterSettings.OpenBalancePay)
						{
							this.jsondict.Add("Status", "Error");
							this.jsondict.Add("ErrorMsg", "系统未开启预付款支付");
							this.jsondict.Add("ErrorUrl", "");
							this.WriteJson(context, 0);
							goto end_IL_002b;
						}
						Hidistro.Entities.Members.MemberInfo user = HiContext.Current.User;
						if (user == null || HiContext.Current.UserId <= 0)
						{
							context.Response.Write("{\"Status\":\"NoLogin\",\"ErrorMsg\":\"您还未登录\"}");
							goto end_IL_002b;
						}
						if (!user.IsOpenBalance || string.IsNullOrEmpty(user.TradePassword) || string.IsNullOrEmpty(user.TradePasswordSalt))
						{
							this.jsondict.Add("Status", "Error");
							this.jsondict.Add("ErrorMsg", "您还未设置交易密码");
							this.jsondict.Add("ErrorUrl", "");
							this.WriteJson(context, 0);
							goto end_IL_002b;
						}
						if (user.Balance - user.RequestBalance < num4)
						{
							this.jsondict.Add("Status", "Error");
							this.jsondict.Add("ErrorMsg", "预付款余额不够用于抵扣");
							this.jsondict.Add("ErrorUrl", "");
							this.WriteJson(context, 0);
							goto end_IL_002b;
						}
						if (!MemberProcessor.ValidTradePassword(text5))
						{
							this.jsondict.Add("Status", "Error");
							this.jsondict.Add("ErrorMsg", "交易密码有误，请重试");
							this.jsondict.Add("ErrorUrl", "");
							this.WriteJson(context, 0);
						}
					}
					List<ProductInputItemInfo> list = new List<ProductInputItemInfo>();
					ShoppingCartInfo shoppingCartInfo = null;
					if (!string.IsNullOrEmpty(text2) && !ProductHelper.ProductsIsAllOnSales(text2, 0))
					{
						context.Response.Write("{\"Status\":\"001\",\"ErrorMsg\":\"订单中有商品已删除或已下架，请重新选择商品\"}");
					}
					else
					{
						shoppingCartInfo = ShoppingCartProcessor.GetShoppingCart(from, text2, num, 0, false, num3, 0);
						if (num <= 0)
						{
							num = 1;
						}
						if (shoppingCartInfo == null)
						{
							context.Response.Write("{\"Status\":\"Error\",\"ErrorMsg\":\"提交订单失败,订单项商品错误\"}");
						}
						else if (shoppingCartInfo.LineItems == null || shoppingCartInfo.LineItems.Count <= 0)
						{
							context.Response.Write("{\"Status\":\"Error\",\"ErrorMsg\":\"提交订单失败,订单项商品错误\"}");
						}
						else
						{
							SiteSettings masterSettings2 = SettingsManager.GetMasterSettings();
							string str = "";
							if (!TradeHelper.CheckShoppingStock(shoppingCartInfo, out str, 0))
							{
								context.Response.Write("{\"Status\":\"002\",\"ErrorMsg\":\"订单中有商品(" + str + ")库存不足\"}");
							}
							else
							{
								OrderInfo order = ShoppingProcessor.ConvertShoppingCartToOrder(shoppingCartInfo, false, false, 0);
								if (order == null || shoppingCartInfo.LineItems.Count <= 0)
								{
									context.Response.Write("{\"Status\":\"Error\",\"ErrorMsg\":\"订单中有商品已删除或已下架，请重新选择商品\"}");
								}
								else if (order.GetTotal(false) < decimal.Zero)
								{
									context.Response.Write("{\"Status\":\"Error\",\"ErrorMsg\":\"订单金额不能为负数，请重新下单\"}");
								}
								else
								{
									order.OrderId = OrderIDFactory.GenerateOrderId();
									order.ParentOrderId = "0";
									order.OrderDate = DateTime.Now;
									order.OrderSource = OrderSource.WeiXin;
									Hidistro.Entities.Members.MemberInfo user2 = HiContext.Current.User;
									order.UserId = user2.UserId;
									order.Username = user2.UserName;
									order.EmailAddress = user2.Email;
									order.RealName = (string.IsNullOrEmpty(user2.RealName) ? user2.NickName : user2.RealName);
									order.QQ = user2.QQ;
									order.Remark = remark;
									order.Tax = decimal.Zero;
									order.OrderStatus = OrderStatus.WaitBuyerPay;
									order.ShipToDate = "";
									order.ShippingRegion = "";
									order.RegionId = 0;
									order.FullRegionPath = "";
									order.Address = "";
									order.ZipCode = "";
									order.ShipTo = "";
									order.TelPhone = "";
									order.CellPhone = "";
									order.ShippingId = 0;
									order.LatLng = "";
									order.ShippingModeId = 0;
									order.ModeName = "服务自享";
									order.StoreId = num3;
									OrderInfo orderInfo = order;
									OrderInfo orderInfo2 = order;
									decimal num7 = orderInfo.AdjustedFreight = (orderInfo2.Freight = default(decimal));
									order.PaymentTypeId = 0;
									order.PaymentType = "在线支付";
									order.Gateway = "";
									order.OrderType = OrderType.ServiceOrder;
									int productId = shoppingCartInfo.LineItems.First().ProductId;
									ProductInfo productBaseDetails = ProductHelper.GetProductBaseDetails(productId);
									foreach (KeyValuePair<string, LineItemInfo> lineItem in order.LineItems)
									{
										LineItemInfo value = lineItem.Value;
										if (value != null)
										{
											ProductInfo productInfo = (value.ProductId == productId) ? productBaseDetails : ProductHelper.GetProductBaseDetails(value.ProductId);
											value.IsValid = productInfo.IsValid;
											if (productInfo.IsValid)
											{
												value.ValidStartDate = null;
												value.ValidEndDate = null;
											}
											else
											{
												value.ValidStartDate = productInfo.ValidStartDate;
												value.ValidEndDate = productInfo.ValidEndDate;
											}
											value.IsRefund = productInfo.IsRefund;
											value.IsOverRefund = productInfo.IsOverRefund;
										}
									}
									List<ProductInputItemInfo> productInputItemList = ProductHelper.GetProductInputItemList(productId);
									if (productInputItemList.Count > 0)
									{
										string value2 = context.Request["InputItems"].ToNullString();
										if (productInputItemList.Any((ProductInputItemInfo d) => d.IsRequired) && string.IsNullOrWhiteSpace(value2))
										{
											context.Response.Write("{\"Status\":\"Error\",\"ErrorMsg\":\"订单信息输入项中有必填项未填写\"}");
											goto end_IL_002b;
										}
										if (!string.IsNullOrWhiteSpace(value2))
										{
											try
											{
												list = JsonConvert.DeserializeObject<List<ProductInputItemInfo>>(value2);
												InputFieldType inputFieldType2;
												if (productBaseDetails.IsGenerateMore && productInputItemList.Any((ProductInputItemInfo d) => d.IsRequired))
												{
													foreach (ProductInputItemInfo item in list)
													{
														if (item.InputFileValues.Count < num)
														{
															context.Response.Write("{\"Status\":\"Error\",\"ErrorMsg\":\"订单信息输入项数量应与购买数量一致\"}");
															return;
														}
														foreach (string inputFileValue in item.InputFileValues)
														{
															if (string.IsNullOrEmpty(inputFileValue) && item.IsRequired)
															{
																context.Response.Write("{\"Status\":\"Error\",\"ErrorMsg\":\"不能为空的信息输入项,必须填写\"}");
																return;
															}
														}
														foreach (string inputFileValue2 in item.InputFileValues)
														{
															if (!string.IsNullOrEmpty(inputFileValue2))
															{
																int inputFieldType = item.InputFieldType;
																inputFieldType2 = InputFieldType.Date;
																if (inputFieldType == inputFieldType2.GetHashCode())
																{
																	DateTime now = DateTime.Now;
																	if (!DateTime.TryParse(inputFileValue2, out now))
																	{
																		context.Response.Write("{\"Status\":\"Error\",\"ErrorMsg\":\"" + item.InputFieldTitle + ",信息输入项格式错误\"}");
																		return;
																	}
																}
																int inputFieldType3 = item.InputFieldType;
																inputFieldType2 = InputFieldType.IdCard;
																if (inputFieldType3 == inputFieldType2.GetHashCode() && !DataHelper.IsIDCard(inputFileValue2))
																{
																	context.Response.Write("{\"Status\":\"Error\",\"ErrorMsg\":\"" + item.InputFieldTitle + ",信息输入项格式错误\"}");
																	return;
																}
																int inputFieldType4 = item.InputFieldType;
																inputFieldType2 = InputFieldType.Number;
																if (inputFieldType4 == inputFieldType2.GetHashCode())
																{
																	int num8 = 0;
																	decimal num9 = default(decimal);
																	if (!int.TryParse(inputFileValue2, out num8) && !decimal.TryParse(inputFileValue2, out num9))
																	{
																		context.Response.Write("{\"Status\":\"Error\",\"ErrorMsg\":\"" + item.InputFieldTitle + ",信息输入项格式错误\"}");
																		return;
																	}
																}
																int inputFieldType5 = item.InputFieldType;
																inputFieldType2 = InputFieldType.Phone;
																if (inputFieldType5 == inputFieldType2.GetHashCode() && !DataHelper.IsTel(inputFileValue2) && !DataHelper.IsMobile(inputFileValue2))
																{
																	context.Response.Write("{\"Status\":\"Error\",\"ErrorMsg\":\"" + item.InputFieldTitle + ",信息输入项格式错误\"}");
																	return;
																}
															}
														}
													}
												}
												string text6 = "";
												int num10 = 1;
												if (productBaseDetails.IsGenerateMore)
												{
													num10 = num;
												}
												for (int i = 0; i < num10; i++)
												{
													foreach (ProductInputItemInfo item2 in list)
													{
														OrderInputItemInfo orderInputItemInfo = new OrderInputItemInfo();
														DateTime now2;
														if (item2.IsRequired)
														{
															int inputFieldType6 = item2.InputFieldType;
															inputFieldType2 = InputFieldType.Date;
															if (inputFieldType6 == inputFieldType2.GetHashCode())
															{
																now2 = DateTime.Now;
																text6 = now2.ToString();
															}
															int inputFieldType7 = item2.InputFieldType;
															inputFieldType2 = InputFieldType.Number;
															if (inputFieldType7 == inputFieldType2.GetHashCode())
															{
																text6 = "0";
															}
														}
														orderInputItemInfo.OrderId = order.OrderId;
														orderInputItemInfo.InputFieldGroup = i + 1;
														orderInputItemInfo.InputFieldTitle = item2.InputFieldTitle;
														orderInputItemInfo.InputFieldType = item2.InputFieldType;
														string text7 = (item2.InputFileValues.Count > i) ? item2.InputFileValues[i] : text6;
														int inputFieldType8 = item2.InputFieldType;
														inputFieldType2 = InputFieldType.Image;
														if (inputFieldType8 == inputFieldType2.GetHashCode() && !string.IsNullOrWhiteSpace(text7))
														{
															string[] array = text7.Split(',');
															for (int j = 0; j < array.Length; j++)
															{
																if (!string.IsNullOrWhiteSpace(array[j]))
																{
																	string text8 = array[j];
																	text8 = text8.Replace(Globals.HostPath(HttpContext.Current.Request.Url), "");
																	if (!string.IsNullOrWhiteSpace(text8) && text8.Substring(0, 1) != "/")
																	{
																		text8 = "/" + text8;
																	}
																	now2 = DateTime.Now;
																	text8 = (array[j] = Globals.SaveFile("InputItems/" + now2.ToString("yyyyMMdd") + "/", text8, "/Storage/master/", true, false, ""));
																}
															}
															text7 = string.Join(",", array);
														}
														orderInputItemInfo.InputFieldValue = text7;
														order.InputItems.Add(orderInputItemInfo);
													}
												}
											}
											catch (Exception ex)
											{
												Globals.WriteExceptionLog_Page(ex, param, "ServiceProductInputItems");
												context.Response.Write("{\"Status\":\"Error\",\"ErrorMsg\":\"信息输入项转换失败\"}");
												return;
											}
										}
									}
									string text9 = "";
									if (!string.IsNullOrEmpty(text))
									{
										CouponItemInfo userCouponInfo = ShoppingProcessor.GetUserCouponInfo(shoppingCartInfo.GetTotal(num3 > 0), text);
										if (userCouponInfo != null && string.IsNullOrEmpty(userCouponInfo.OrderId) && !userCouponInfo.UsedTime.HasValue && HiContext.Current.UserId != 0 && userCouponInfo.UserId == HiContext.Current.UserId && (order.CountDownBuyId == 0 || (order.CountDownBuyId > 0 && userCouponInfo.UseWithPanicBuying.Value)))
										{
											order.CouponName = userCouponInfo.CouponName;
											if (userCouponInfo.OrderUseLimit.HasValue)
											{
												order.CouponAmount = userCouponInfo.OrderUseLimit.Value;
											}
											order.CouponCode = text;
											if (userCouponInfo.Price.Value >= order.GetAmount(false))
											{
												order.CouponValue = order.GetAmount(false);
											}
											else
											{
												order.CouponValue = userCouponInfo.Price.Value;
											}
											text9 = userCouponInfo.CanUseProducts;
										}
									}
									if (num2 > 0 && (masterSettings2.CanPointUseWithCoupon || (!masterSettings2.CanPointUseWithCoupon && string.IsNullOrEmpty(order.CouponCode))) && masterSettings2.ShoppingDeduction > 0)
									{
										int shoppingDeductionRatio = masterSettings2.ShoppingDeductionRatio;
										decimal value3 = (decimal)shoppingDeductionRatio * (order.GetTotal(false) - order.AdjustedFreight - order.Tax) * (decimal)masterSettings2.ShoppingDeduction / 100m;
										if (user2 != null)
										{
											int num11 = (user2.Points > (int)value3) ? ((int)value3) : user2.Points;
											if (num2 > num11)
											{
												num2 = num11;
											}
											decimal value4 = ((decimal)num2 / (decimal)masterSettings2.ShoppingDeduction).F2ToString("f2").ToDecimal(0);
											order.DeductionPoints = num2;
											order.DeductionMoney = value4;
										}
									}
									if (flag)
									{
										int num12 = context.Request["invoiceId"].ToInt(0);
										UserInvoiceDataInfo userInvoiceDataInfo = MemberProcessor.GetUserInvoiceDataInfo(num12);
										bool flag2 = num12 == 0 && true;
										invoiceType = (InvoiceType)context.Request["InvoiceType"].ToInt(0);
										if (userInvoiceDataInfo == null)
										{
											userInvoiceDataInfo = new UserInvoiceDataInfo
											{
												Id = 0,
												InvoiceType = InvoiceType.Personal,
												InvoiceTitle = "个人",
												LastUseTime = DateTime.Now
											};
											flag2 = true;
											invoiceType = InvoiceType.Personal;
										}
										else
										{
											invoiceType = userInvoiceDataInfo.InvoiceType;
										}
										if (invoiceType == InvoiceType.VATInvoice && masterSettings2.VATTaxRate > decimal.Zero && masterSettings2.EnableVATInvoice)
										{
											order.Tax = ((order.GetTotal(false) - order.AdjustedFreight) * masterSettings2.VATTaxRate / 100m).F2ToString("f2").ToDecimal(0);
										}
										else if (masterSettings2.TaxRate > decimal.Zero && (masterSettings2.EnableTax || masterSettings2.EnableE_Invoice))
										{
											order.Tax = ((order.GetTotal(false) - order.AdjustedFreight) * masterSettings2.TaxRate / 100m).F2ToString("f2").ToDecimal(0);
										}
										else
										{
											order.Tax = decimal.Zero;
										}
										order.InvoiceTitle = userInvoiceDataInfo.InvoiceTitle;
										order.InvoiceType = userInvoiceDataInfo.InvoiceType;
										if (order.InvoiceType == InvoiceType.Enterprise || order.InvoiceType == InvoiceType.Enterprise_Electronic || order.InvoiceType == InvoiceType.VATInvoice)
										{
											order.InvoiceTaxpayerNumber = userInvoiceDataInfo.InvoiceTaxpayerNumber;
										}
										order.InvoiceData = JsonHelper.GetJson(userInvoiceDataInfo);
									}
									if (num4 > order.GetTotal(false))
									{
										num4 = order.GetTotal(false);
									}
									order.BalanceAmount = num4;
									if (order.GetTotal(true) <= decimal.Zero && order.BalanceAmount > decimal.Zero)
									{
										order.PaymentTypeId = -99;
										order.PaymentType = "余额支付";
										order.Gateway = "hishop.plugins.payment.advancerequest";
									}
									if (HiContext.Current.UserId != 0)
									{
										order.Points = order.GetPoint(masterSettings2.PointsRate);
									}
									else
									{
										order.Points = 0;
									}
									if (masterSettings2.OpenSupplier)
									{
										order.SupplierId = 0;
										order.ShipperName = "平台";
									}
									List<OrderInfo> list2 = new List<OrderInfo>();
									list2.Add(order);
									if (!ShoppingProcessor.CreatOrder(list2))
									{
										context.Response.Write("{\"Status\":\"Error\",\"ErrorMsg\":\"提交订单失败\"}");
									}
									else
									{
										if (order.BalanceAmount > decimal.Zero)
										{
											TradeHelper.BalanceDeduct(order);
										}
										TransactionAnalysisHelper.AnalysisOrderTranData(order);
										if (order.GetTotal(true) <= decimal.Zero || (order.PreSaleId > 0 && order.BalanceAmount == order.Deposit))
										{
											Task.Factory.StartNew(delegate
											{
												int num13 = 0;
												int num14 = 0;
												int num15 = 0;
												if (order.GroupBuyId > 0)
												{
													GroupBuyInfo groupBuy = TradeHelper.GetGroupBuy(order.GroupBuyId);
													if (groupBuy != null && groupBuy.Status == GroupBuyStatus.UnderWay)
													{
														num14 = TradeHelper.GetOrderCount(order.GroupBuyId);
														num15 = order.GetGroupBuyOerderNumber();
														num13 = groupBuy.MaxCount;
													}
												}
												if (order.CheckAction(OrderActions.BUYER_PAY) && TradeHelper.UpdateOrderStatus(order))
												{
													TradeHelper.UserPayOrder(order, false, true);
													try
													{
														if (order.FightGroupId > 0)
														{
															VShopHelper.SetFightGroupSuccess(order.FightGroupId);
														}
														if (order.GroupBuyId > 0 && num13 == num14 + num15)
														{
															TradeHelper.SetGroupBuyEndUntreated(order.GroupBuyId);
														}
														if (order.ParentOrderId == "-1")
														{
															OrderQuery orderQuery = new OrderQuery();
															orderQuery.ParentOrderId = order.OrderId;
															IList<OrderInfo> listUserOrder = MemberProcessor.GetListUserOrder(order.UserId, orderQuery);
															foreach (OrderInfo item3 in listUserOrder)
															{
																OrderHelper.OrderConfirmPaySendMessage(item3);
															}
														}
														else
														{
															OrderHelper.OrderConfirmPaySendMessage(order);
														}
													}
													catch (Exception ex3)
													{
														IDictionary<string, string> dictionary = new Dictionary<string, string>();
														dictionary.Add("ErrorMessage", ex3.Message);
														dictionary.Add("StackTrace", ex3.StackTrace);
														if (ex3.InnerException != null)
														{
															dictionary.Add("InnerException", ex3.InnerException.ToString());
														}
														if (ex3.GetBaseException() != null)
														{
															dictionary.Add("BaseException", ex3.GetBaseException().Message);
														}
														if (ex3.TargetSite != (MethodBase)null)
														{
															dictionary.Add("TargetSite", ex3.TargetSite.ToString());
														}
														dictionary.Add("ExSource", ex3.Source);
														Globals.AppendLog(dictionary, "支付更新订单收款记录或者消息通知时出错：" + ex3.Message, "", "", "UserPay");
													}
													order.OnPayment();
												}
											});
										}
										Messenger.OrderCreated(order, HiContext.Current.User);
										StringBuilder stringBuilder = new StringBuilder();
										stringBuilder.Append("{\"Status\":\"OK\",");
										if (order.GetTotal(true) <= decimal.Zero)
										{
											stringBuilder.Append("\"paymentType\":\"HASPAY\",");
										}
										else
										{
											stringBuilder.Append("\"paymentType\":\"NO\",");
										}
										stringBuilder.Append("\"FightGroupId\":\"" + order.FightGroupId + "\",");
										stringBuilder.Append("\"ParentOrderId\":\"" + order.ParentOrderId + "\",");
										stringBuilder.AppendFormat("\"OrderId\":\"{0}\"", order.OrderId);
										stringBuilder.Append("}");
										context.Response.Write(stringBuilder);
									}
								}
							}
						}
					}
				}
				end_IL_002b:;
			}
			catch (Exception ex2)
			{
				Globals.WriteExceptionLog_Page(ex2, param, "ServiceProductSumbitOrder");
				context.Response.Write("{\"Status\":\"Error\",\"ErrorMsg\":\"创建订单异常\"}");
			}
		}

		private void ResetTradePassword(HttpContext context)
		{
			string text = context.Request["password"].ToNullString();
			string text2 = context.Request["repassword"].ToNullString();
			string verifyCode = context.Request["verifycode"].ToNullString();
			int num = context.Request["CodeType"].ToInt(0);
			Hidistro.Entities.Members.MemberInfo user = HiContext.Current.User;
			if (user == null || HiContext.Current.UserId <= 0)
			{
				context.Response.Write("{\"Status\":\"NoLogin\",\"msg\":\"您还未登录\"}");
			}
			else if (string.IsNullOrEmpty(text))
			{
				context.Response.Write("{\"Status\":\"PasswordError\",\"msg\":\"新密码不能为空\"}");
			}
			else if (text != text2)
			{
				context.Response.Write("{\"Status\":\"PasswordError\",\"msg\":\"两次密码输入不一致\"}");
			}
			else
			{
				bool flag = true;
				flag = (num == 1 && true);
				string str = "验证码错误";
				if ((flag && HiContext.Current.CheckPhoneVerifyCode(verifyCode, user.CellPhone, out str)) || (!flag && HiContext.Current.CheckVerifyCode(verifyCode, "")))
				{
					if (MemberProcessor.ChangeTradePassword(user, text2))
					{
						Messenger.UserPasswordChanged(user, text2);
						Users.SetCurrentUser(user.UserId, 1, true, false);
					}
					context.Response.Write("{\"Status\":\"true\",\"msg\":\"重置交易密码成功\"}");
				}
				else
				{
					context.Response.Write("{\"Status\":\"false\",\"msg\":\"重置交易密码失败," + str + "\"}");
				}
			}
		}

		private void UploadNameVerify(HttpContext context)
		{
			string empty = string.Empty;
			string empty2 = string.Empty;
			try
			{
				if (this.ValIDCard(context, ref empty2))
				{
					string text = context.Request["IDNumber"];
					int num = context.Request["ShippingId"].ToInt(0);
					if (num > 0)
					{
						empty = ((!this.UpdateShippingAddressInfo(context, num, out empty2)) ? ("{\"Status\":\"Error\",\"Result\":\"" + empty2 + "\"}") : "{\"Status\":\"OK\",\"Result\":\"\"}");
					}
					else if (!string.IsNullOrWhiteSpace(context.Request["OrderId"]))
					{
						OrderInfo orderInfo = TradeHelper.GetOrderInfo(context.Request["OrderId"]);
						if (orderInfo != null)
						{
							orderInfo.IDNumber = HiCryptographer.Encrypt(text.Trim());
							if (HiContext.Current.SiteSettings.CertificationModel == 2)
							{
								string imageServerUrl = Globals.GetImageServerUrl();
								orderInfo.IDImage1 = (string.IsNullOrEmpty(imageServerUrl) ? Globals.SaveFile("user", context.Request["IDImage1"].Trim(), "/Storage/master/", true, false, "") : context.Request["IDImage1"].Trim());
								orderInfo.IDImage2 = (string.IsNullOrEmpty(imageServerUrl) ? Globals.SaveFile("user", context.Request["IDImage2"].Trim(), "/Storage/master/", true, false, "") : context.Request["IDImage2"].Trim());
							}
							empty = ((!TradeHelper.UpdateOrderInfo(orderInfo)) ? "{\"Status\":\"Error\",\"Result\":\"保存证件信息失败\"}" : ((!this.UpdateShippingAddressInfo(context, orderInfo.ShippingId, out empty2)) ? ("{\"Status\":\"Error\",\"Result\":\"更新订单证件信息成功," + empty2 + "\"}") : "{\"Status\":\"OK\",\"Result\":\"\"}"));
						}
						else
						{
							empty = "{\"Status\":\"Error\",\"Result\":\"获取模型错误\"}";
						}
					}
					else
					{
						empty = "{\"Status\":\"Error\",\"Result\":\"请求的地址错误\"}";
					}
				}
				else
				{
					empty = "{\"Status\":\"Error\",\"Result\":\"" + empty2 + "\"}";
				}
			}
			catch
			{
				empty = "{\"Status\":\"Error\",\"Result\":\"系统异常\"}";
			}
			context.Response.ContentType = "text/plain";
			context.Response.Write(empty);
		}

		private bool UpdateShippingAddressInfo(HttpContext context, int shippingId, out string msg)
		{
			bool result = false;
			msg = string.Empty;
			ShippingAddressInfo shippingAddress = MemberProcessor.GetShippingAddress(shippingId);
			if (shippingAddress != null)
			{
				shippingAddress.IDNumber = HiCryptographer.Encrypt(context.Request["IDNumber"].Trim());
				if (HiContext.Current.SiteSettings.CertificationModel == 2)
				{
					string imageServerUrl = Globals.GetImageServerUrl();
					shippingAddress.IDImage1 = (string.IsNullOrEmpty(imageServerUrl) ? Globals.SaveFile("user", context.Request["IDImage1"].Trim(), "/Storage/master/", true, false, "") : context.Request["IDImage1"].Trim());
					shippingAddress.IDImage2 = (string.IsNullOrEmpty(imageServerUrl) ? Globals.SaveFile("user", context.Request["IDImage2"].Trim(), "/Storage/master/", true, false, "") : context.Request["IDImage2"].Trim());
				}
				if (MemberProcessor.UpdateShippingAddress(shippingAddress))
				{
					result = true;
				}
				else
				{
					msg = "更新收货地址证件信息失败";
				}
			}
			else
			{
				msg = "获取收货地址模型错误";
			}
			return result;
		}

		private bool ValIDCard(HttpContext context, ref string erromsg)
		{
			SiteSettings siteSettings = HiContext.Current.SiteSettings;
			if (!siteSettings.IsOpenCertification)
			{
				erromsg = "实名认证已关闭";
				return false;
			}
			Regex regex = new Regex("(^\\d{15}$)|(^\\d{18}$)|(^\\d{17}(\\d|X|x)$)");
			if (string.IsNullOrEmpty(context.Request.Params["IDNumber"]) || !regex.IsMatch(context.Request.Params["IDNumber"].Trim()))
			{
				erromsg = "身份证号格式错误";
				return false;
			}
			if (siteSettings.CertificationModel == 2)
			{
				if (string.IsNullOrEmpty(context.Request.Params["IDImage1"]))
				{
					erromsg = "请上传证件照正面";
					return false;
				}
				if (string.IsNullOrEmpty(context.Request.Params["IDImage2"]))
				{
					erromsg = "请上传证件照反面";
					return false;
				}
			}
			return true;
		}

		private void ChangeTranPassword(HttpContext context)
		{
			string text = context.Request["password"].ToNullString();
			string pass = context.Request["oldPassword"].ToNullString();
			Hidistro.Entities.Members.MemberInfo user = Users.GetUser(HiContext.Current.UserId);
			if (user == null)
			{
				context.Response.Write("{\"Status\":\"nologined\",\"msg\":\"请您先登录\"}");
			}
			else if (!string.IsNullOrEmpty(user.TradePassword) && user.TradePassword != Users.EncodePassword(pass, user.TradePasswordSalt))
			{
				context.Response.Write("{\"Status\":\"erroldpwd\",\"msg\":\"原始交易密码不正确\"}");
			}
			else if (text.Length < 6 || text.Length > 20)
			{
				context.Response.Write("{\"Status\":\"errpwd\",\"msg\":\"交易密码必须在6-20个字符之间！\"}");
			}
			else if (MemberProcessor.ChangeTradePassword(user, text))
			{
				Messenger.UserDealPasswordChanged(user, text);
				context.Response.Write("{\"Status\":\"ok\",\"msg\":\"修改交易密码成功\"}");
			}
			else
			{
				context.Response.Write("{\"Status\":\"unknow\",\"msg\":\"修改交易密码失败\"}");
			}
		}

		private void StoreChageQuantity(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			string skuId = context.Request["skuId"];
			int storeId = context.Request["storeId"].ToInt(0);
			int num = 1;
			int.TryParse(context.Request["quantity"], out num);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("{");
			int skuStock = ShoppingCartProcessor.GetSkuStock(skuId, storeId);
			if (num > skuStock)
			{
				stringBuilder.AppendFormat("\"Status\":\"{0}\"", skuStock);
				ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart(null, false, false, -1);
				ShoppingCartItemInfo shoppingCartItemInfo = shoppingCart.LineItems.FirstOrDefault((ShoppingCartItemInfo a) => a.SkuId == skuId && a.StoreId == storeId);
				stringBuilder.AppendFormat(",\"Quantity\":\"{0}\"", shoppingCartItemInfo.Quantity);
				goto IL_0200;
			}
			stringBuilder.Append("\"Status\":\"OK\"");
			ShoppingCartProcessor.UpdateLineItemQuantity(skuId, (num <= 0) ? 1 : num, storeId);
			ShoppingCartInfo shoppingCart2 = ShoppingCartProcessor.GetShoppingCart(null, false, false, -1);
			ShoppingCartItemInfo shoppingCartItemInfo2 = shoppingCart2.LineItems.FirstOrDefault((ShoppingCartItemInfo a) => a.SkuId == skuId && a.StoreId == storeId);
			if (shoppingCartItemInfo2 != null)
			{
				if (shoppingCartItemInfo2.StoreId > 0)
				{
					shoppingCartItemInfo2.AdjustedPrice = shoppingCartItemInfo2.MemberPrice;
				}
				else
				{
					PromotionInfo productQuantityDiscountPromotion = ShoppingCartProcessor.GetProductQuantityDiscountPromotion(skuId, HiContext.Current.User.GradeId);
					if (productQuantityDiscountPromotion != null && (decimal)num >= productQuantityDiscountPromotion.Condition)
					{
						shoppingCartItemInfo2.AdjustedPrice = shoppingCartItemInfo2.MemberPrice * productQuantityDiscountPromotion.DiscountValue;
					}
					else
					{
						shoppingCartItemInfo2.AdjustedPrice = shoppingCartItemInfo2.MemberPrice;
					}
				}
				stringBuilder.AppendFormat(",\"adjustedPrice\":\"{0}\"", shoppingCartItemInfo2.AdjustedPrice.F2ToString("f2") ?? "");
				goto IL_0200;
			}
			return;
			IL_0200:
			stringBuilder.Append("}");
			context.Response.ContentType = "application/json";
			context.Response.Write(stringBuilder.ToString());
		}

		private void GetProductConsultation(HttpContext context)
		{
			int productId = context.Request.QueryString["productId"].ToInt(0);
			List<ProductConsultationInfo> productConsultationList = ProductCommentHelper.GetProductConsultationList(productId);
			IsoDateTimeConverter isoDateTimeConverter = new IsoDateTimeConverter();
			isoDateTimeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
			string s = JsonConvert.SerializeObject(productConsultationList, Formatting.Indented, isoDateTimeConverter);
			context.Response.ContentType = "text/json";
			context.Response.Write(s);
		}

		public void PCAdvancePayPassVerify(HttpContext context)
		{
			string text = context.Request["OrderId"].ToNullString();
			string text2 = context.Request["AdvancePayPass"].ToNullString();
			if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text2))
			{
				Hidistro.Entities.Members.MemberInfo user = HiContext.Current.User;
				if (user.IsOpenBalance)
				{
					OrderInfo orderInfo = TradeHelper.GetOrderInfo(text);
					if (orderInfo == null)
					{
						context.Response.Write("{\"Status\":\"No\",\"Message\":\"请检查订单是否异常！\"}");
						context.Response.End();
					}
					string empty = string.Empty;
					int num = 0;
					int num2 = 0;
					int num3 = 0;
					if (orderInfo.CountDownBuyId > 0)
					{
						foreach (KeyValuePair<string, LineItemInfo> lineItem in orderInfo.LineItems)
						{
							CountDownInfo countDownInfo = TradeHelper.CheckUserCountDown(lineItem.Value.ProductId, orderInfo.CountDownBuyId, lineItem.Value.SkuId, HiContext.Current.UserId, orderInfo.GetAllQuantity(true), orderInfo.OrderId, out empty, orderInfo.StoreId);
							if (countDownInfo == null)
							{
								context.Response.Write("{\"Status\":\"No\",\"Message\":\"" + empty + "\"}");
								context.Response.End();
								return;
							}
						}
					}
					if (orderInfo.FightGroupId > 0)
					{
						foreach (KeyValuePair<string, LineItemInfo> lineItem2 in orderInfo.LineItems)
						{
							FightGroupActivityInfo fightGroupActivityInfo = VShopHelper.CheckUserFightGroup(lineItem2.Value.ProductId, orderInfo.FightGroupActivityId, orderInfo.FightGroupId, lineItem2.Value.SkuId, HiContext.Current.UserId, orderInfo.GetAllQuantity(true), orderInfo.OrderId, lineItem2.Value.Quantity, out empty);
							if (fightGroupActivityInfo == null)
							{
								context.Response.Write("{\"Status\":\"No\",\"Message\":\"" + empty + "\"}");
								context.Response.End();
								return;
							}
						}
					}
					if (orderInfo.GroupBuyId > 0)
					{
						GroupBuyInfo groupBuy = TradeHelper.GetGroupBuy(orderInfo.GroupBuyId);
						if (groupBuy == null || groupBuy.Status != GroupBuyStatus.UnderWay)
						{
							context.Response.Write("{\"Status\":\"No\",\"Message\":\"当前的订单为团购订单，此团购活动已结束，所以不能支付\"}");
							context.Response.End();
							return;
						}
						num2 = TradeHelper.GetOrderCount(orderInfo.GroupBuyId);
						num3 = orderInfo.GetGroupBuyOerderNumber();
						num = groupBuy.MaxCount;
						if (num < num2 + num3)
						{
							context.Response.Write("{\"Status\":\"No\",\"Message\":\"当前的订单为团购订单，订购数量已超过订购总数，所以不能支付\"}");
							context.Response.End();
							return;
						}
					}
					ProductPreSaleInfo productPreSaleInfo = null;
					if (orderInfo.PreSaleId > 0)
					{
						productPreSaleInfo = ProductPreSaleHelper.GetProductPreSaleInfo(orderInfo.PreSaleId);
						if (productPreSaleInfo == null)
						{
							context.Response.Write("{\"Status\":\"No\",\"Message\":\"预售活动不存在不能支付\"}");
							context.Response.End();
							return;
						}
						if (!orderInfo.DepositDate.HasValue && orderInfo.OrderStatus == OrderStatus.WaitBuyerPay)
						{
							if (productPreSaleInfo.PreSaleEndDate < DateTime.Now)
							{
								context.Response.Write("{\"Status\":\"No\",\"Message\":\"您支付晚了，预售活动已经结束\"}");
								context.Response.End();
								return;
							}
							if (user.Balance - user.RequestBalance < orderInfo.Deposit - orderInfo.BalanceAmount)
							{
								context.Response.Write("{\"Status\":\"No\",\"Message\":\"预付款余额不足,支付失败\"}");
								context.Response.End();
								return;
							}
						}
						if (orderInfo.DepositDate.HasValue && orderInfo.OrderStatus == OrderStatus.WaitBuyerPay)
						{
							if (productPreSaleInfo.PaymentStartDate > DateTime.Now)
							{
								context.Response.Write("{\"Status\":\"No\",\"Message\":\"尾款支付尚未开始\"}");
								context.Response.End();
								return;
							}
							DateTime dateTime = productPreSaleInfo.PaymentEndDate;
							DateTime date = dateTime.Date;
							dateTime = DateTime.Now;
							if (date < dateTime.Date)
							{
								context.Response.Write("{\"Status\":\"No\",\"Message\":\"尾款支付已结束\"}");
								context.Response.End();
								return;
							}
							if (user.Balance - user.RequestBalance < orderInfo.FinalPayment)
							{
								context.Response.Write("{\"Status\":\"No\",\"Message\":\"预付款余额不足,支付失败\"}");
								context.Response.End();
								return;
							}
						}
					}
					else if (user.Balance - user.RequestBalance < orderInfo.GetTotal(false))
					{
						context.Response.Write("{\"Status\":\"No\",\"Message\":\"预付款余额不足,支付失败\"}");
						context.Response.End();
						return;
					}
					if (!orderInfo.CheckAction(OrderActions.BUYER_PAY))
					{
						context.Response.Write("{\"Status\":\"No\",\"Message\":\"当前的订单订单状态不是等待付款，所以不能支付\"}");
						context.Response.End();
					}
					else if (HiContext.Current.UserId != orderInfo.UserId)
					{
						context.Response.Write("{\"Status\":\"No\",\"Message\":\"预付款只能为自己下的订单付款,查一查该订单是不是你的\"}");
						context.Response.End();
					}
					else if (MemberProcessor.ValidTradePassword(text2))
					{
						string str = "";
						switch (TradeHelper.CheckOrderBeforePay(orderInfo, out str))
						{
						case 1:
							TradeHelper.CloseOrder(orderInfo.OrderId, "订单中有商品(" + str + ")规格被删除或者下架");
							context.Response.Write("{\"Status\":\"001\",\"Message\":\"订单中有商品(" + str + ")已下架或被删除\"}");
							context.Response.End();
							break;
						case 2:
							context.Response.Write("{\"Status\":\"002\",\"Message\":\"订单中有商品(" + str + ")库存不足\"}");
							context.Response.End();
							break;
						default:
						{
							PaymentModeInfo paymentMode = ShoppingProcessor.GetPaymentMode("hishop.plugins.payment.advancerequest");
							if (paymentMode == null)
							{
								context.Response.Write("{\"Status\":\"No\",\"Message\":\"系统未开启预付款支付\"}");
								context.Response.End();
							}
							else
							{
								orderInfo.PaymentTypeId = paymentMode.ModeId;
								orderInfo.Gateway = paymentMode.Gateway;
								orderInfo.PaymentType = paymentMode.Name;
								if (!TradeHelper.UpdateOrderPaymentType(orderInfo))
								{
									context.Response.Write("{\"Status\":\"No\",\"Message\":\"系统繁忙，请重试！\"}");
									context.Response.End();
								}
								else if (TradeHelper.UserPayOrder(orderInfo, true, false))
								{
									this.PaySendMessage(orderInfo);
									if (orderInfo.FightGroupId > 0)
									{
										VShopHelper.SetFightGroupSuccess(orderInfo.FightGroupId);
										string s = JsonConvert.SerializeObject(new
										{
											Status = "OK",
											mesg = "交易成功",
											isFightGroup = true,
											orderId = orderInfo.OrderId
										});
										context.Response.Write(s);
									}
									else
									{
										string routeUrl = RouteConfig.GetRouteUrl(HttpContext.Current, "FinishOrder", new
										{
											orderId = orderInfo.OrderId
										});
										context.Response.Write("{\"Status\":\"OK\",\"Message\":\"" + orderInfo.OrderId + "\",\"url\":\"" + routeUrl + "\"}");
										context.Response.End();
									}
								}
								else
								{
									context.Response.Write("{\"Status\":\"No\",\"Message\":\"" + $"对订单{orderInfo.OrderId} 支付失败" + "\"}");
									context.Response.End();
								}
							}
							break;
						}
						}
					}
					else
					{
						context.Response.Write("{\"Status\":\"003\",\"Message\":\"交易密码有误，请重试\"}");
						context.Response.End();
					}
				}
			}
		}

		public void AdvancePayPassVerify(HttpContext context)
		{
			string text = context.Request["AdvancePayPass"].ToNullString();
			decimal d = context.Request["PayAmount"].ToDecimal(0);
			if (string.IsNullOrEmpty(text))
			{
				context.Response.Write("{\"Status\":\"No\",\"Message\":\"请输入交易密码\"}");
				context.Response.End();
			}
			else
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				if (!masterSettings.OpenBalancePay)
				{
					context.Response.Write("{\"Status\":\"No\",\"Message\":\"系统未开启预付款支付\"}");
					context.Response.End();
				}
				else
				{
					Hidistro.Entities.Members.MemberInfo user = HiContext.Current.User;
					if (!user.IsOpenBalance || string.IsNullOrEmpty(user.TradePassword) || string.IsNullOrEmpty(user.TradePasswordSalt))
					{
						context.Response.Write("{\"Status\":\"NoTradePassword\",\"Message\":\"您还未设置交易密码\"}");
						context.Response.End();
					}
					else if (user.Balance - user.RequestBalance < d)
					{
						context.Response.Write("{\"Status\":\"No\",\"Message\":\"预付款余额不足,支付失败\"}");
						context.Response.End();
					}
					else if (MemberProcessor.ValidTradePassword(text))
					{
						context.Response.Write("{\"Status\":\"OK\",\"Message\":\"验证成功\"}");
						context.Response.End();
					}
					else
					{
						context.Response.Write("{\"Status\":\"003\",\"Message\":\"交易密码有误，请重试\"}");
						context.Response.End();
					}
				}
			}
		}

		public void AddInpourBlance(HttpContext context)
		{
			if (HiContext.Current.UserId == 0)
			{
				context.Response.Write("{\"Status\":\"NoLogined\",\"Message\":\"您还未登录,请您先登录。\"}");
				context.Response.End();
			}
			if (!HiContext.Current.User.IsOpenBalance)
			{
				context.Response.Write("{\"Status\":\"NoOpenBalance\",\"Message\":\"未开启预付款帐号。\"}");
				context.Response.End();
			}
			decimal num = context.Request["ReChargeBalance"].ToDecimal(0);
			string text = context.Request["PaymentType"].ToNullString();
			bool flag = true;
			int num2 = num.ToString().IndexOf(".");
			string text2 = (num2 > -1) ? (num.ToString() + "0").Substring(num2 + 1) : "";
			if (text2.TrimEnd('0').Length > 2)
			{
				flag = false;
			}
			if (num < decimal.Zero || num > 10000000m || !flag)
			{
				context.Response.Write("{\"Status\":\"MoneyError\",\"Message\":\"请输入大于0的充值金额且金额必须大于0且小于10000000,并且小数位置不能超过2位。\"}");
				context.Response.End();
			}
			if (string.IsNullOrEmpty(text))
			{
				context.Response.Write("{\"Status\":\"PaymentTypeError\",\"Message\":\"请选择支付方式！\"}");
				context.Response.End();
			}
			PayGatewayInfo gatewayInfo = this.GetGatewayInfo(text);
			string generateId = Globals.GetGenerateId();
			InpourRequestInfo inpourRequest = new InpourRequestInfo
			{
				InpourId = generateId,
				TradeDate = DateTime.Now,
				InpourBlance = num,
				UserId = HiContext.Current.UserId,
				PaymentId = gatewayInfo.GatewayTypeId
			};
			if (MemberProcessor.AddInpourBlance(inpourRequest))
			{
				context.Response.Write("{\"Status\":\"OK\",\"InpourBlanceId\":\"" + generateId + "\"}");
				context.Response.End();
			}
			else
			{
				context.Response.Write("{\"Status\":\"Failed\",\"Message\":\"添加记录失败！\"}");
				context.Response.End();
			}
		}

		private PayGatewayInfo GetGatewayInfo(string paymentType)
		{
			PayGatewayInfo payGatewayInfo = new PayGatewayInfo();
			payGatewayInfo.GatewayTypeId = Convert.ToInt16(paymentType);
			if (paymentType == "-2")
			{
				payGatewayInfo.PaymentName = "微信支付";
				payGatewayInfo.GatewayTypeName = "hishop.plugins.payment.weixinrequest";
			}
			else if (paymentType == "-10")
			{
				payGatewayInfo.PaymentName = "支付宝app支付";
				payGatewayInfo.GatewayTypeName = "hishop.plugins.payment.ws_apppay.wswappayrequest";
			}
			else if (paymentType == "-4")
			{
				payGatewayInfo.PaymentName = "支付宝H5网页支付";
				payGatewayInfo.GatewayTypeName = "hishop.plugins.payment.ws_wappay.wswappayrequest";
			}
			else if (paymentType == "-5")
			{
				payGatewayInfo.PaymentName = "盛付通手机网页支付";
				payGatewayInfo.GatewayTypeName = "Hishop.Plugins.Payment.ShengPayMobile.ShengPayMobileRequest";
			}
			else if (paymentType == "-7")
			{
				payGatewayInfo.PaymentName = "银联全渠道支付";
				payGatewayInfo.GatewayTypeName = "hishop.plugins.payment.bankuniongateway.bankuniongetwayrequest";
			}
			else if (paymentType == "-20")
			{
				payGatewayInfo.PaymentName = "支付宝微信端支付";
				payGatewayInfo.GatewayTypeName = "hishop.plugins.payment.alipaywx.alipaywxrequest";
			}
			else
			{
				PaymentModeInfo paymentMode = ShoppingProcessor.GetPaymentMode(paymentType);
				if (paymentMode != null)
				{
					payGatewayInfo.GatewayTypeId = paymentMode.ModeId;
					payGatewayInfo.PaymentName = paymentMode.Name;
					payGatewayInfo.GatewayTypeName = paymentMode.Gateway;
				}
			}
			return payGatewayInfo;
		}

		public void UpdatePaymentType(HttpContext context)
		{
			string text = context.Request["orderId"].ToNullString();
			bool flag = context.Request["fromUserApp"].ToBool();
			if (string.IsNullOrEmpty(text))
			{
				context.Response.Write("{\"Status\":\"ErrorOrderId\",\"Message\":\"错误的订单信息\"}");
				context.Response.End();
			}
			if (HiContext.Current.UserId == 0 && !flag)
			{
				context.Response.Write("{\"Status\":\"NoLogined\",\"Message\":\"您还未登录,请您先登录。\"}");
				context.Response.End();
			}
			OrderInfo orderInfo = TradeHelper.GetOrderInfo(text);
			if (orderInfo == null || orderInfo.UserId != HiContext.Current.UserId)
			{
				context.Response.Write("{\"Status\":\"OrderError\",\"Message\":\"订单信息与当前会员不匹配。\"}");
				context.Response.End();
			}
			if (orderInfo.OrderStatus != OrderStatus.WaitBuyerPay)
			{
				context.Response.Write("{\"Status\":\"OrderStatusError\",\"Message\":\"订单状态错误。\"}");
				context.Response.End();
			}
			switch (orderInfo.PaymentTypeId = context.Request["paymentTypeId"].ToInt(0))
			{
			case -2:
				orderInfo.PaymentType = "微信支付";
				orderInfo.Gateway = "hishop.plugins.payment.weixinrequest";
				break;
			case -22:
				orderInfo.PaymentType = "H5微信支付";
				orderInfo.Gateway = "hishop.plugins.payment.weixinrequest";
				break;
			case -1:
				orderInfo.PaymentType = "线下付款";
				orderInfo.Gateway = "hishop.plugins.payment.bankrequest";
				break;
			case -10:
				orderInfo.PaymentType = "支付宝app支付";
				orderInfo.Gateway = "hishop.plugins.payment.ws_apppay.wswappayrequest";
				break;
			case -4:
				orderInfo.PaymentType = "支付宝H5网页支付";
				orderInfo.Gateway = "hishop.plugins.payment.ws_wappay.wswappayrequest";
				break;
			case -5:
				orderInfo.PaymentType = "盛付通手机网页支付";
				orderInfo.Gateway = "Hishop.Plugins.Payment.ShengPayMobile.ShengPayMobileRequest";
				break;
			case -6:
				orderInfo.PaymentType = "预付款帐户支付";
				orderInfo.Gateway = "hishop.plugins.payment.advancerequest";
				break;
			case -7:
				orderInfo.PaymentType = "银联全渠道支付";
				orderInfo.Gateway = "hishop.plugins.payment.bankuniongateway.bankuniongetwayrequest";
				break;
			case -8:
				orderInfo.PaymentType = "APP微信支付";
				orderInfo.Gateway = "hishop.plugins.payment.appwxrequest";
				break;
			case -9:
				orderInfo.PaymentType = "支付宝跨境支付";
				orderInfo.Gateway = "hishop.plugins.payment.alipaycrossbordermobilepayment.alipaycrossbordermobilepaymentrequest";
				break;
			case -20:
				orderInfo.PaymentType = "微信端支付宝支付";
				orderInfo.Gateway = "hishop.plugins.payment.alipaywx.alipaywxrequest";
				break;
			}
			if (TradeHelper.UpdateOrderPaymentType(orderInfo))
			{
				string text2 = "";
				text2 = ((!(orderInfo.Gateway.ToLower() != "hishop.plugins.payment.advancerequest")) ? ("TransactionPwd?orderId=" + text + "&totalAmount=" + orderInfo.GetTotal(false).F2ToString("f2")) : ("FinishOrder?action=topay&orderId=" + text));
				context.Response.Write("{\"Status\":\"OK\",\"ToUrl\":\"" + text2 + "\"}");
				context.Response.End();
			}
			else
			{
				context.Response.Write("{\"Status\":\"UpdateError\",\"Message\":\"更新支付方式时出错。\"}");
				context.Response.End();
			}
		}

		public void GetProductFreight(HttpContext context)
		{
			decimal num = default(decimal);
			int num2 = 0;
			num2 = context.Request["ProductId"].ToInt(0);
			string text = context.Request["SkuId"].ToString();
			int num3 = 0;
			num3 = context.Request["regionId"].ToInt(0);
			int num4 = context.Request["Quantity"].ToInt(0);
			if (num4 <= 0)
			{
				num4 = 1;
			}
			if (num2 > 0)
			{
				ProductInfo productSimpleInfo = ProductBrowser.GetProductSimpleInfo(num2);
				if (productSimpleInfo != null)
				{
					decimal weight = productSimpleInfo.Weight;
					decimal weight2 = productSimpleInfo.Weight;
					decimal amount = productSimpleInfo.MinSalePrice;
					if (!string.IsNullOrEmpty(text) && productSimpleInfo.HasSKU && productSimpleInfo.Skus.ContainsKey(text))
					{
						weight = productSimpleInfo.Skus[text].Weight;
						weight2 = weight;
						amount = productSimpleInfo.Skus[text].SalePrice;
					}
					if (num3 <= 0)
					{
						num3 = HiContext.Current.DeliveryScopRegionId;
					}
					num = ShoppingProcessor.CalcProductFreight(num3, productSimpleInfo.ShippingTemplateId, weight, weight, num4, amount);
				}
			}
			context.Response.Write("{\"Status\":\"OK\",\"Freight\":\"" + num.F2ToString("f2") + "\"}");
			context.Response.End();
		}

		public void CheckEmailCode(HttpContext context)
		{
			string verifyCode = context.Request.Form["VerifyCode"].ToNullString();
			bool flag = context.Request.Form["UpdateStatus"].ToBool();
			if (HiContext.Current.CheckVerifyCode(verifyCode, ""))
			{
				Hidistro.Entities.Members.MemberInfo user = Users.GetUser(HiContext.Current.UserId);
				if (user == null)
				{
					context.Response.Write("{\"Status\":\"nologined\",\"msg\":\"请您先登录\"}");
				}
				else if (flag && user.EmailVerification)
				{
					context.Response.Write("{\"Status\":\"verifyed\",\"msg\":\"邮箱已经验证过了,请不要重复验证\"}");
				}
				else
				{
					if (flag)
					{
						user.EmailVerification = true;
						MemberProcessor.UpdateMember(user);
					}
					context.Response.Write("{\"Status\":\"ok\",\"msg\":\"验证完成\"}");
				}
			}
			else
			{
				context.Response.Write("{\"Status\":\"errcode\",\"msg\":\"错误的验证码\"}");
			}
		}

		public void CheckPhoneCode(HttpContext context)
		{
			string verifyCode = context.Request.Form["VerifyCode"].ToNullString();
			bool flag = context.Request.Form["UpdateStatus"].ToBool();
			Hidistro.Entities.Members.MemberInfo user = Users.GetUser(HiContext.Current.UserId);
			if (user == null)
			{
				context.Response.Write("{\"Status\":\"nologined\",\"msg\":\"请您先登录\"}");
			}
			else
			{
				string cellPhone = user.CellPhone;
				cellPhone = context.Request.Form["CellPhone"].ToNullString();
				if (string.IsNullOrEmpty(cellPhone))
				{
					cellPhone = user.CellPhone;
				}
				string str = "错误的验证码";
				if (HiContext.Current.CheckPhoneVerifyCode(verifyCode, cellPhone, out str))
				{
					if (flag && user.CellPhoneVerification)
					{
						context.Response.Write("{\"Status\":\"verifyed\",\"msg\":\"手机已经验证过了,请不要重复验证\"}");
					}
					else
					{
						if (flag)
						{
							user.CellPhoneVerification = true;
							MemberProcessor.UpdateMember(user);
						}
						context.Response.Write("{\"Status\":\"ok\",\"msg\":\"验证完成\"}");
					}
				}
				else
				{
					context.Response.Write("{\"Status\":\"errcode\",\"msg\":\"" + str + "\"}");
				}
			}
		}

		private void ChangePassword(HttpContext context)
		{
			string text = context.Request["password"].ToNullString();
			string changedPassword = text;
			string pass = context.Request["oldPassword"].ToNullString();
			Hidistro.Entities.Members.MemberInfo user = Users.GetUser(HiContext.Current.UserId);
			if (user == null)
			{
				context.Response.Write("{\"Status\":\"nologined\",\"msg\":\"请您先登录\"}");
			}
			else if (user.Password != Users.EncodePassword(pass, user.PasswordSalt))
			{
				context.Response.Write("{\"Status\":\"erroldpwd\",\"msg\":\"原始登录密码不正确\"}");
			}
			else if (text.Length < 6 || text.Length > 20)
			{
				context.Response.Write("{\"Status\":\"errpwd\",\"msg\":\"密码必须在6-20个字符之间！\"}");
			}
			else
			{
				text = (user.Password = Users.EncodePassword(text, user.PasswordSalt));
				if (MemberProcessor.UpdateMember(user))
				{
					Messenger.UserPasswordChanged(HiContext.Current.User, changedPassword);
					context.Response.Write("{\"Status\":\"ok\",\"msg\":\"修改密码成功\"}");
				}
				else
				{
					context.Response.Write("{\"Status\":\"unknow\",\"msg\":\"修改密码失败\"}");
				}
			}
		}

		private void IsUserPrize(HttpContext context)
		{
		}

		private void SetPassword(HttpContext context)
		{
			string text = context.Request["password"].ToNullString();
			string changedPassword = text;
			Hidistro.Entities.Members.MemberInfo user = Users.GetUser(HiContext.Current.UserId);
			if (user == null)
			{
				context.Response.Write("{\"Status\":\"nologined\",\"msg\":\"请您先登录\"}");
			}
			else if (user.PasswordSalt != "Open")
			{
				context.Response.Write("{\"Status\":\"hasbind\",\"msg\":\"您的帐号不是一键登录帐号或者已设置过密码了\"}");
			}
			else if (text.Length < 6 || text.Length > 20)
			{
				context.Response.Write("{\"Status\":\"errpwd\",\"msg\":\"密码必须在6-20个字符之间！\"}");
			}
			else
			{
				string text2 = Globals.RndStr(128, true);
				text = (user.Password = Users.EncodePassword(text, text2));
				user.PasswordSalt = text2;
				if (MemberProcessor.UpdateMember(user))
				{
					Messenger.UserPasswordChanged(HiContext.Current.User, changedPassword);
					context.Response.Write("{\"Status\":\"ok\",\"msg\":\"设置密码成功\"}");
				}
				else
				{
					context.Response.Write("{\"Status\":\"unknow\",\"msg\":\"设置密码失败\"}");
				}
			}
		}

		private void BindPhone(HttpContext context)
		{
			string text = context.Request["phone"].ToNullString();
			string verifyCode = context.Request["VerifyCode"].ToNullString();
			if (string.IsNullOrEmpty(text))
			{
				context.Response.Write("{\"Status\":\"errphone\",\"msg\":\"请填写正确的手机号码\"}");
			}
			if (!DataHelper.IsMobile(text))
			{
				context.Response.Write("{\"Status\":\"errphone\",\"msg\":\"请输入正确的手机号码\"}");
			}
			else if (MemberProcessor.FindMemberByCellphone(text) != null)
			{
				context.Response.Write("{\"Status\":\"hasbind\",\"msg\":\"该手机号码已被其它帐号绑定\"}");
			}
			else
			{
				Hidistro.Entities.Members.MemberInfo user = Users.GetUser(HiContext.Current.UserId);
				if (user == null)
				{
					context.Response.Write("{\"Status\":\"nologined\",\"msg\":\"请您先登录\"}");
				}
				else
				{
					string str = "";
					if (!HiContext.Current.CheckPhoneVerifyCode(verifyCode, text, out str))
					{
						context.Response.Write("{\"Status\":\"errcode\",\"msg\":\"" + str + "\"}");
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
							context.Response.Write("{\"Status\":\"ok\",\"msg\":\"绑定成功\",\"SetPwd\":\"false\"}");
						}
						else
						{
							context.Response.Write("{\"Status\":\"unknow\",\"msg\":\"绑定失败\"}");
						}
					}
				}
			}
		}

		private void BindEmail(HttpContext context)
		{
			string verifyCode = context.Request["VerifyCode"].ToNullString();
			Hidistro.Entities.Members.MemberInfo user = Users.GetUser(HiContext.Current.UserId);
			if (user == null)
			{
				context.Response.Write("{\"Status\":\"nologined\",\"msg\":\"请您先登录\"}");
			}
			else
			{
				string text = context.Request["email"].ToNullString();
				if (string.IsNullOrEmpty(text) || !DataHelper.IsEmail(text))
				{
					context.Response.Write("{\"Status\":\"erremail\",\"msg\":\"错误的邮箱号码\"}");
				}
				else if (MemberProcessor.IsUseEmail(text))
				{
					context.Response.Write("{\"Status\":\"hasbind\",\"msg\":\"该邮箱已被其它帐号绑定\"}");
				}
				else if (!HiContext.Current.CheckVerifyCode(verifyCode, ""))
				{
					context.Response.Write("{\"Status\":\"errcode\",\"msg\":\"错误的验证码\"}");
				}
				else
				{
					if (user.UserName.IndexOf("YSC_") >= 0 || user.UserName == user.Email)
					{
						user.UserName = text;
					}
					user.Email = text;
					user.EmailVerification = true;
					if (MemberProcessor.UpdateMember(user))
					{
						context.Response.Write("{\"Status\":\"ok\",\"msg\":\"绑定成功\",\"SetPwd\":\"false\"}");
					}
					else
					{
						context.Response.Write("{\"Status\":\"unknow\",\"msg\":\"绑定失败\"}");
					}
				}
			}
		}

		private void SignIn(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			string source = context.Request["SignInSource"];
			Hidistro.Entities.Members.MemberInfo user = Users.GetUser(HiContext.Current.UserId);
			if (user != null)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				int signInPoint = masterSettings.SignInPoint;
				int continuousDays = masterSettings.ContinuousDays;
				int continuousPoint = masterSettings.ContinuousPoint;
				int num = MemberHelper.UserSignIn(user.UserId, continuousDays);
				StringBuilder stringBuilder = new StringBuilder("{\"result\":{");
				if (num > 0)
				{
					int num2 = 0;
					if (signInPoint > 0)
					{
						VshopProcess.AddPoints(user, signInPoint, PointTradeType.SignIn, source);
					}
					if (continuousDays > 0 && continuousPoint > 0)
					{
						num2 = MemberHelper.GetContinuousDays(user.UserId);
						if (num2 == 0)
						{
							VshopProcess.AddPoints(user, continuousPoint, PointTradeType.ContinuousSign, source);
						}
					}
					stringBuilder.Append("\"status\":\"1\",");
					stringBuilder.AppendFormat("\"points\":\"{0}\",", signInPoint);
					stringBuilder.AppendFormat("\"continuDays\":\"{0}\",", num2);
					stringBuilder.AppendFormat("\"settingDays\":\"{0}\",", continuousDays);
					stringBuilder.AppendFormat("\"continuPoints\":\"{0}\",", continuousPoint);
					stringBuilder.AppendFormat("\"integral\":\"{0}\"", user.Points);
				}
				else
				{
					stringBuilder.AppendFormat("\"status\":\"{0}\",", 2);
					stringBuilder.AppendFormat("\"points\":\"0\",");
					stringBuilder.AppendFormat("\"integral\":\"{0}\"", user.Points);
				}
				stringBuilder.Append("}}");
				context.Response.Write(stringBuilder);
			}
			context.Response.End();
		}

		private static void AddPoints(Hidistro.Entities.Members.MemberInfo member, int points, PointTradeType type)
		{
			VshopProcess.AddPoints(member, points, type, "");
		}

		private static void AddPoints(Hidistro.Entities.Members.MemberInfo member, int points, PointTradeType type, string source)
		{
			PointDetailDao pointDetailDao = new PointDetailDao();
			PointDetailInfo pointDetailInfo = new PointDetailInfo();
			pointDetailInfo.UserId = member.UserId;
			pointDetailInfo.TradeDate = DateTime.Now;
			pointDetailInfo.TradeType = type;
			pointDetailInfo.Increased = points;
			pointDetailInfo.Points = points + member.Points;
			if (pointDetailInfo.Points > 2147483647)
			{
				pointDetailInfo.Points = 2147483647;
			}
			if (pointDetailInfo.Points < 0)
			{
				pointDetailInfo.Points = 0;
			}
			if (!string.IsNullOrEmpty(source))
			{
				pointDetailInfo.SignInSource = int.Parse(source);
			}
			member.Points = pointDetailInfo.Points;
			pointDetailDao.Add(pointDetailInfo, null);
		}

		private void DeleteCartGift(HttpContext context)
		{
			string s = "";
			string s2 = context.Request["giftId"];
			int giftId = default(int);
			if (int.TryParse(s2, out giftId))
			{
				ShoppingCartProcessor.RemoveGiftItem(giftId, PromoteType.NotSet);
				s = "OK";
			}
			context.Response.Write(s);
		}

		private void ChageGiftQuantity(HttpContext context)
		{
			string s = "";
			string value = context.Request["giftId"];
			string s2 = context.Request["quantity"];
			int num = default(int);
			if (!int.TryParse(s2, out num))
			{
				s = "兑换数量必须为整数";
			}
			else if (num <= 0)
			{
				s = "兑换数量必须为大于0的整数";
			}
			else if (!string.IsNullOrEmpty(value))
			{
				ShoppingCartProcessor.UpdateGiftItemQuantity(Convert.ToInt32(value), num, PromoteType.NotSet);
			}
			context.Response.Write(s);
		}

		private void GetProduct(HttpContext context)
		{
			int productId = context.Request["productId"].ToInt(0);
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			ProductInfo productSimpleInfo = ProductBrowser.GetProductSimpleInfo(productId);
			string s = JsonConvert.SerializeObject(productSimpleInfo);
			context.Response.Write(s);
		}

		private string GetSubCategoryNames(int parentCategoryId)
		{
			IEnumerable<CategoryInfo> subCategories = CatalogHelper.GetSubCategories(parentCategoryId);
			StringBuilder stringBuilder = new StringBuilder();
			if (subCategories != null)
			{
				foreach (CategoryInfo item in subCategories)
				{
					stringBuilder.Append(item.Name).Append(" ");
				}
				if (stringBuilder.ToString().Length > 1)
				{
					stringBuilder.Remove(stringBuilder.Length - 1, 1);
				}
			}
			return stringBuilder.ToString();
		}

		private void GetCategories(HttpContext context)
		{
			int num = context.Request["pid"].ToInt(0);
			if (string.IsNullOrEmpty(context.Request["pid"]) || num < 0)
			{
				context.Response.Write(this.GetErrorJosn(102, "数字类型转换错误"));
			}
			else
			{
				IEnumerable<CategoryInfo> subCategories = CatalogHelper.GetSubCategories(num);
				if (subCategories == null)
				{
					context.Response.Write(this.GetErrorJosn(103, "没获取到相应的分类"));
				}
				else
				{
					var result = (from c in subCategories
					select new
					{
						cid = c.CategoryId,
						name = c.Name,
						icon = (string.IsNullOrEmpty(c.Icon) ? "/templates/common/images/catedefaulticon.jpg" : Globals.FullPath(c.Icon)),
						bigImageUrl = Globals.FullPath(c.BigImageUrl),
						hasChildren = c.HasChildren.ToString().ToLower(),
						description = this.GetSubCategoryNames(c.CategoryId)
					}).ToList();
					string s = JsonConvert.SerializeObject(new
					{
						Result = result
					});
					context.Response.Write(s);
				}
			}
		}

		private void GetAllCategories(HttpContext context)
		{
			AppShopHandler appShopHandler = new AppShopHandler();
			appShopHandler.GetAllCategories(context);
		}

		private void GetProducts(HttpContext context)
		{
			int pageIndex = context.Request["pageIndex"].ToInt(0);
			int pageSize = context.Request["pageSize"].ToInt(0);
			int num = context.Request["storeId"].ToInt(0);
			ProductBrowseQuery productBrowseQuery = new ProductBrowseQuery();
			productBrowseQuery.PageIndex = pageIndex;
			productBrowseQuery.PageSize = pageSize;
			if (!string.IsNullOrEmpty(context.Request["cId"]))
			{
				int categoryId = context.Request["cId"].ToInt(0);
				productBrowseQuery.Category = CatalogHelper.GetCategory(categoryId);
			}
			string text = context.Request["sortBy"].ToNullString();
			if (string.IsNullOrWhiteSpace(text) || (text.ToLower() != "addeddate" && text.ToLower() != "saleprice" && text.ToLower() != "visticounts" && text.ToLower() != "showsalecounts"))
			{
				text = "DisplaySequence";
			}
			productBrowseQuery.Keywords = Globals.StripAllTags(context.Request["keyword"].ToNullString());
			productBrowseQuery.SortBy = "DisplaySequence";
			productBrowseQuery.SortOrder = SortAction.Desc;
			productBrowseQuery.SortBy = text;
			productBrowseQuery.SortOrder = ((context.Request["sortOrder"].ToNullString() == "asc") ? SortAction.Asc : SortAction.Desc);
			productBrowseQuery.StoreId = num;
			int couponId = context.Request["couponId"].ToInt(0);
			CouponInfo eFCoupon = CouponHelper.GetEFCoupon(couponId);
			if (eFCoupon != null)
			{
				productBrowseQuery.CanUseProducts = eFCoupon.CanUseProducts;
			}
			DbQueryResult storeProductList = StoresHelper.GetStoreProductList(productBrowseQuery);
			DataTable data = storeProductList.Data;
			string empty = string.Empty;
			if (num > 0)
			{
				empty = "/WapShop/StoreProductDetails.aspx";
				if (context.Request.UrlReferrer != (Uri)null && context.Request.UrlReferrer.ToString().ToLower().IndexOf("/vshop/") != -1)
				{
					empty = "/VShop/StoreProductDetails.aspx";
				}
			}
			else
			{
				empty = "/WapShop/ProductDetails.aspx";
				if (context.Request.UrlReferrer != (Uri)null && context.Request.UrlReferrer.ToString().ToLower().IndexOf("/vshop/") != -1)
				{
					empty = "/VShop/ProductDetails.aspx";
				}
			}
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			List<VProductsModel> list = new List<VProductsModel>();
			foreach (DataRow row in data.Rows)
			{
				VProductsModel vProductsModel = new VProductsModel();
				list.Add(vProductsModel);
				vProductsModel.name = row["ProductName"].ToNullString();
				vProductsModel.pic = ((row["ThumbnailUrl310"].ToNullString() == "") ? Globals.FullPath(masterSettings.DefaultProductThumbnail4) : Globals.FullPath((string)row["ThumbnailUrl310"]));
				vProductsModel.price = ((decimal)row["SalePrice"]).F2ToString("f2");
				vProductsModel.saleCounts = ((int)row["SaleCounts"]).ToString();
				vProductsModel.marketPrice = ((decimal)row["MarketPrice"]).F2ToString("f2");
				vProductsModel.url = Globals.FullPath(string.Format("{0}?productId={1}&storeId={2}", (row["ProductType"].ToInt(0) == 1.GetHashCode()) ? empty.Replace("StoreProductDetails", "ProductDetails").Replace("ProductDetails", "ServiceProductDetails") : empty, row["ProductId"], num));
				vProductsModel.pid = row["ProductId"].ToNullString();
			}
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					RecordCount = storeProductList.TotalRecords,
					List = list
				}
			});
			context.Response.Write(s);
		}

		private void GetFightGroupActivity(HttpContext context)
		{
			int fightGroupActivityId = context.Request["FightGroupActivityId"].ToInt(0);
			string skuId = context.Request["SkuId"].ToNullString();
			FightGroupSkuInfo fightGroupSku = VShopHelper.GetFightGroupSku(fightGroupActivityId, skuId);
			string s = JsonConvert.SerializeObject(new
			{
				Stock = fightGroupSku.TotalCount - fightGroupSku.BoughtCount,
				SalePrice = fightGroupSku.SalePrice
			});
			context.Response.Write(s);
		}

		private void GenerateTwoDimensionalImage(HttpContext context)
		{
			string text = Globals.HtmlDecode(context.Request["url"].ToString());
			if (!string.IsNullOrEmpty(text))
			{
				MemoryStream memoryStream = Globals.GenerateTwoDimensionalImage(text);
				context.Response.ClearContent();
				context.Response.ContentType = "image/Jpeg";
				context.Response.BinaryWrite(memoryStream.ToArray());
				context.Response.End();
			}
		}

		private void IsDuplicateBuyCountDown(HttpContext context)
		{
			int countDownId = context.Request["countDownId"].ToInt(0);
			string text = context.Request["skuId"].ToNullString();
			CountDownInfo countDownInfo = PromoteHelper.GetCountDownInfo(countDownId, 0);
			if (countDownInfo != null)
			{
				if (HiContext.Current.UserId <= 0)
				{
					context.Response.Write("{\"success\":\"false\",\"msg\":\"请登录后再抢购\"}");
				}
				else if (PromoteHelper.CheckDuplicateBuyCountDown(countDownInfo.ProductId, HiContext.Current.UserId))
				{
					context.Response.Write("{\"success\":\"false\",\"msg\":\"请勿重复抢购\"}");
				}
				else
				{
					int countDownSurplusNumber = PromoteHelper.GetCountDownSurplusNumber(countDownId);
					if (countDownInfo.EndDate <= DateTime.Now)
					{
						context.Response.Write("{\"success\":\"false\",\"msg\":\"活动已结束\"}");
					}
					else if (countDownSurplusNumber <= 0)
					{
						context.Response.Write("{\"success\":\"false\",\"msg\":\"活动已经抢完\"}");
					}
					else
					{
						context.Response.Write("{\"success\":\"true\",\"msg\":\"\"}");
					}
				}
			}
			else
			{
				context.Response.Write("{\"success\":\"false\",\"msg\":\"限时购不存在\"}");
			}
		}

		public void AddRedEnvelopeSendRecord(HttpContext context)
		{
			string text = context.Request["SendCode"];
			string text2 = context.Request["redEnvelopeId"];
			string text3 = context.Request["OrderId"];
			if (string.IsNullOrEmpty(text))
			{
				context.Response.Write("{\"success\":\"false\",\"msg\":\"发送码错误\"}");
			}
			if (string.IsNullOrEmpty(text2))
			{
				context.Response.Write("{\"success\":\"false\",\"msg\":\"红包ID错误\"}");
			}
			if (string.IsNullOrEmpty(text3))
			{
				context.Response.Write("{\"success\":\"false\",\"msg\":\"订单ID错误\"}");
			}
			try
			{
				RedEnvelopeSendRecord redEnvelopeSendRecord = WeiXinRedEnvelopeProcessor.GetRedEnvelopeSendRecord(Guid.Parse(text), "", text2);
				if (redEnvelopeSendRecord == null)
				{
					redEnvelopeSendRecord = new RedEnvelopeSendRecord();
					redEnvelopeSendRecord.SendCode = Guid.Parse(text);
					redEnvelopeSendRecord.RedEnvelopeId = Convert.ToInt32(text2);
					redEnvelopeSendRecord.OrderId = text3;
					redEnvelopeSendRecord.SendTime = DateTime.Now;
					if (WeiXinRedEnvelopeProcessor.AddRedEnvelopeSendRecord(redEnvelopeSendRecord))
					{
						context.Response.Write("{\"success\":\"true\",\"msg\":\"添加成功\"}");
					}
				}
				else
				{
					context.Response.Write("{\"success\":\"true\",\"msg\":\"已经存在\"}");
				}
			}
			catch (Exception)
			{
				context.Response.Write("{\"success\":\"false\",\"msg\":\"添加异常\"}");
			}
		}

		private string SaveCredentialImages(string credentialImg)
		{
			string text = HiContext.Current.GetStoragePath() + "user/Credentials/";
			if (!Globals.PathExist(text, false))
			{
				Globals.CreatePath(text);
			}
			string str = HttpContext.Current.Server.MapPath(text);
			if (credentialImg.Length == 0)
			{
				return "";
			}
			string text2 = "";
			string[] array = credentialImg.Split(',');
			foreach (string text3_i in array)
			{
				string text3 = text3_i.Replace("//", "/");
				if (text3.Length != 0)
				{
					string text4 = (text3.Split('/').Length == 6) ? text3.Split('/')[5] : text3.Split('/')[4];
					if (!File.Exists(str + text4))
					{
						File.Copy(HttpContext.Current.Server.MapPath(text3), str + text4);
						if (File.Exists(HttpContext.Current.Server.MapPath(text3)))
						{
							File.Delete(HttpContext.Current.Server.MapPath(text3));
						}
						text2 = text2 + (string.IsNullOrEmpty(text2) ? "" : ",") + text + text4;
					}
				}
			}
			return text2;
		}

		public void ReturnSendGoods(HttpContext context)
		{
			int num = 0;
			int.TryParse(context.Request["ReturnsId"], out num);
			string skuId = (context.Request["SkuId"] == null) ? "" : Globals.StripAllTags(context.Request["SkuId"]);
			string orderId = (context.Request["OrderId"] == null) ? "" : Globals.StripAllTags(context.Request["OrderId"]);
			ReturnInfo returnInfo = null;
			returnInfo = ((num <= 0) ? TradeHelper.GetReturnInfo(orderId, skuId) : TradeHelper.GetReturnInfo(num));
			if (returnInfo == null)
			{
				this.ShowMessageAndCode(context, "错误的退货信息", -1);
			}
			else
			{
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(returnInfo.OrderId);
				if (orderInfo == null)
				{
					this.ShowMessageAndCode(context, "错误的订单信息", -2);
				}
				else if (orderInfo.LineItems.ContainsKey(returnInfo.SkuId))
				{
					if (orderInfo.LineItems[returnInfo.SkuId].Status != LineItemStatus.MerchantsAgreedForReturn && orderInfo.LineItems[returnInfo.SkuId].Status != LineItemStatus.DeliveryForReturn)
					{
						this.ShowMessageAndCode(context, "商品退货状态不正确", -4);
					}
					else
					{
						string text = (context.Request["express"] == null) ? "" : Globals.StripAllTags(context.Request["express"]);
						string text2 = (context.Request["shipOrderNumber"] == null) ? "" : Globals.StripAllTags(context.Request["shipOrderNumber"]);
						if (string.IsNullOrEmpty(text))
						{
							this.ShowMessageAndCode(context, "请选择一个快递公司！", -6);
						}
						else
						{
							string text3 = "";
							string text4 = "";
							ExpressCompanyInfo expressCompanyInfo = ExpressHelper.FindNode(text);
							if (text != null)
							{
								text3 = expressCompanyInfo.Kuaidi100Code;
								text4 = expressCompanyInfo.Name;
								if (text2.Trim() == "" || text2.Length > 20)
								{
									this.ShowMessageAndCode(context, "请输入快递编号，长度为1-20位！", -8);
								}
								else if (TradeHelper.UserSendGoodsForReturn(returnInfo.ReturnId, text3, text4, text2, orderInfo.OrderId, returnInfo.SkuId))
								{
									if (text3.ToUpper() == "HTKY")
									{
										ExpressHelper.GetDataByKuaidi100(text3, text2);
									}
									this.ShowMessageAndCode(context, "发货成功！", 1);
								}
								else
								{
									this.ShowMessageAndCode(context, "发货失败！", 0);
								}
							}
							else
							{
								this.ShowMessageAndCode(context, "请选择快递公司", -7);
							}
						}
					}
				}
				else
				{
					this.ShowMessageAndCode(context, "订单中不包含商品信息", -5);
				}
			}
		}

		public void ApplyReturn(HttpContext context)
		{
			Hidistro.Entities.Members.MemberInfo user = HiContext.Current.User;
			string orderId = Globals.StripAllTags(context.Request["orderId"].ToNullString());
			string text = Globals.StripAllTags(context.Request["skuId"].ToNullString());
			int num = 0;
			int num2 = 0;
			num2 = context.Request["RefundType"].ToInt(0);
			num = context.Request["Quantity"].ToInt(0);
			string userRemark = Globals.StripAllTags(context.Request["Remark"].ToNullString());
			string text2 = Globals.StripAllTags(context.Request["UserCredentials"].ToNullString());
			string text3 = text2.Trim();
			string imageServerUrl = Globals.GetImageServerUrl();
			if (text3.Length > 0)
			{
				string[] array = text3.Split(',');
				text2 = "";
				for (int i = 0; i < array.Length; i++)
				{
					text2 += (string.IsNullOrEmpty(imageServerUrl) ? (Globals.SaveFile("user\\Credentials", array[i], "/Storage/master/", true, false, "") + "|") : (array[i] + "|"));
				}
				text2 = text2.TrimEnd('|');
			}
			string text4 = Globals.StripAllTags(context.Request["RefundReason"].ToNullString());
			int num3 = context.Request["afterSaleType"].ToInt(0);
			if (!Enum.IsDefined(typeof(AfterSaleTypes), num3))
			{
				this.ShowMessageAndCode(context, "错误的售后类型！", -8);
			}
			else if (!Enum.IsDefined(typeof(RefundTypes), num2))
			{
				this.ShowMessageAndCode(context, "请选择退款方式", -2);
			}
			else
			{
				string text5 = Globals.StripAllTags(context.Request["BankName"].ToNullString());
				string text6 = Globals.StripAllTags(context.Request["BankAccountName"].ToNullString());
				string text7 = Globals.StripAllTags(context.Request["BankAccountNo"].ToNullString());
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(orderId);
				if (orderInfo == null)
				{
					this.ShowMessageAndCode(context, "错误的订单信息！", -9);
				}
				else if (string.IsNullOrEmpty(text) || !orderInfo.LineItems.ContainsKey(text))
				{
					this.ShowMessageAndCode(context, "请选择要进行售后的商品", -1);
				}
				else if (!TradeHelper.CanReturn(orderInfo, text))
				{
					this.ShowMessageAndCode(context, "该商品正在售后中！", -1);
				}
				else if (!Enum.IsDefined(typeof(RefundTypes), num2))
				{
					this.ShowMessageAndCode(context, "请选择退款方式", -2);
				}
				else
				{
					string enumDescription = EnumDescription.GetEnumDescription((Enum)(object)EnumPaymentType.AdvancePay, 1);
					if ((orderInfo.Gateway.ToLower() == enumDescription || orderInfo.DepositGatewayOrderId.ToNullString().ToLower() == enumDescription) && num2 != 1)
					{
						this.ShowMessageAndCode(context, "预付款支付的订单只能退回到预付款帐号", -11);
					}
					else if (num2 == 2 && (string.IsNullOrEmpty(text5) || string.IsNullOrEmpty(text6) || string.IsNullOrEmpty(text7)))
					{
						this.ShowMessageAndCode(context, "您选择了银行退款,请在退款说明中输入退款的银行卡信息！", -3);
					}
					else if (user != null && num2 == 1 && !user.IsOpenBalance)
					{
						this.ShowMessageAndCode(context, "请先开通预付款帐号", -4);
					}
					else if (string.IsNullOrEmpty(text4))
					{
						this.ShowMessageAndCode(context, "请选择退款原因", -5);
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
							this.ShowMessageAndCode(context, "数量不能大于购买商品的数量", -6);
							return;
						}
						decimal num4 = default(decimal);
						decimal.TryParse(context.Request["RefundAmount"], out num4);
						decimal canRefundAmount = orderInfo.GetCanRefundAmount(text, null, num);
						if (num3 == 3 && canRefundAmount <= decimal.Zero)
						{
							this.ShowMessageAndCode(context, "订单支付金额为0时不能进行仅退款操作。", -10);
						}
						else if (num4 < decimal.Zero && num3 != 2)
						{
							this.ShowMessageAndCode(context, "退款金额必须大于等于0", -8);
						}
						else if (num4 > canRefundAmount)
						{
							this.ShowMessageAndCode(context, string.Format("退款金额不能大于最大可退款金额({0})", canRefundAmount.F2ToString("f2")), -7);
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
								ReturnReason = text4,
								RefundType = (RefundTypes)num2,
								RefundGateWay = refundGateWay,
								RefundOrderId = generateId,
								RefundAmount = num4,
								StoreId = orderInfo.StoreId,
								ApplyForTime = DateTime.Now,
								BankName = text5,
								BankAccountName = text6,
								BankAccountNo = text7,
								HandleStatus = ReturnStatus.Applied,
								OrderId = orderInfo.OrderId,
								SkuId = text,
								Quantity = num,
								UserCredentials = text2,
								AfterSaleType = (AfterSaleTypes)num3
							};
							string str = (returnInfo.AfterSaleType == AfterSaleTypes.OnlyRefund) ? "退款" : ((returnInfo.AfterSaleType == AfterSaleTypes.ReturnAndRefund) ? "退货" : "换货");
							if (TradeHelper.ApplyForReturn(returnInfo))
							{
								if (orderInfo.StoreId > 0)
								{
									VShopHelper.AppPsuhRecordForStore(orderInfo.StoreId, orderInfo.OrderId, "", EnumPushStoreAction.StoreOrderReturnApply);
								}
								this.ShowMessageAndCode(context, "成功的申请了" + str, 1);
							}
							else
							{
								this.ShowMessageAndCode(context, "申请退货失败" + str, 0);
							}
						}
					}
				}
			}
		}

		public void ApplyRefund(HttpContext context)
		{
			Hidistro.Entities.Members.MemberInfo user = HiContext.Current.User;
			string orderId = (context.Request["orderId"] == null) ? "" : Globals.StripAllTags(context.Request["orderId"]);
			string skuId = (context.Request["skuId"] == null) ? "" : Globals.StripAllTags(context.Request["skuId"]);
			string text = DataHelper.CleanSearchString(Globals.UrlDecode(context.Request["ValidCodes"].ToNullString()));
			int num = 0;
			int.TryParse(context.Request["RefundType"], out num);
			int num2 = text.Split(',').Length;
			int num3 = 0;
			OrderInfo orderInfo = TradeHelper.GetOrderInfo(orderId);
			if (orderInfo == null)
			{
				this.ShowMessageAndCode(context, "错误的订单信息；", -30);
			}
			else
			{
				num3 = orderInfo.GetAllQuantity(true);
				GroupBuyInfo groupbuy = null;
				if (orderInfo.GroupBuyId > 0)
				{
					groupbuy = TradeHelper.GetGroupBuy(orderInfo.GroupBuyId);
				}
				decimal num4 = orderInfo.GetCanRefundAmount("", groupbuy, 0);
				if (orderInfo.OrderType == OrderType.ServiceOrder)
				{
					if (orderInfo.LineItems == null || orderInfo.LineItems.Count == 0)
					{
						this.ShowMessageAndCode(context, "错误的订单信息", -30);
						return;
					}
					if (string.IsNullOrEmpty(text))
					{
						this.ShowMessageAndCode(context, "请选择要退款的核销码", -31);
						return;
					}
					if (num == 2)
					{
						this.ShowMessageAndCode(context, "服务类订单不支持退款到银行卡！", -3);
						return;
					}
					if (!TradeHelper.CheckValidCodeForRefund(orderId, text))
					{
						this.ShowMessageAndCode(context, "核销码验证失败！", -32);
						return;
					}
					LineItemInfo lineItemInfo = orderInfo.LineItems.Values.FirstOrDefault();
					int productId = orderInfo.LineItems.Values.FirstOrDefault().ProductId;
					ProductInfo productBaseDetails = ProductHelper.GetProductBaseDetails(productId);
					if (productBaseDetails == null)
					{
						this.ShowMessageAndCode(context, "商品信息不存在！", -33);
						return;
					}
					if (num2 > TradeHelper.GetCanRefundQuantity(orderInfo.OrderId, productBaseDetails.IsOverRefund))
					{
						this.ShowMessageAndCode(context, "可退款的核销码数量错误！", -34);
						return;
					}
					num4 = ((decimal)num2 * (orderInfo.GetTotal(false) / (decimal)lineItemInfo.Quantity * 1.0m) * 1.0m).F2ToString("f2").ToDecimal(0);
				}
				else if (!TradeHelper.CanRefund(orderInfo, skuId))
				{
					this.ShowMessageAndCode(context, "已有待确认的订单或者订单商品的退款/退货/换货申请！", -1);
					return;
				}
				if (orderInfo.FightGroupId > 0)
				{
					FightGroupInfo fightGroup = VShopHelper.GetFightGroup(orderInfo.FightGroupId);
					if (fightGroup != null && fightGroup.Status == FightGroupStatus.FightGroupIn)
					{
						this.ShowMessageAndCode(context, "拼团过程中时，已完成支付的订单不能发起退款；", -24);
						return;
					}
				}
				if (TradeHelper.IsOnlyOneSku(orderInfo))
				{
					skuId = "";
				}
				if (!Enum.IsDefined(typeof(RefundTypes), num))
				{
					this.ShowMessageAndCode(context, "请选择退款方式", -2);
				}
				else
				{
					string enumDescription = EnumDescription.GetEnumDescription((Enum)(object)EnumPaymentType.AdvancePay, 1);
					if ((orderInfo.Gateway.ToLower() == enumDescription || orderInfo.DepositGatewayOrderId.ToNullString().ToLower() == enumDescription) && num != 1)
					{
						this.ShowMessageAndCode(context, "预付款支付的订单只能退回到预付款帐号", -11);
					}
					else
					{
						string text2 = Globals.StripAllTags(context.Request["BankName"].ToNullString());
						string text3 = Globals.StripAllTags(context.Request["BankAccountName"].ToNullString());
						string text4 = Globals.StripAllTags(context.Request["BankAccountNo"].ToNullString());
						string userRemark = (context.Request["Remark"] == null) ? "" : Globals.StripAllTags(context.Request["Remark"]);
						if (num == 2 && (string.IsNullOrEmpty(text2) || string.IsNullOrEmpty(text3) || string.IsNullOrEmpty(text4)))
						{
							this.ShowMessageAndCode(context, "您选择了银行退款,请在退款说明中输入退款的银行卡信息！", -3);
						}
						else if (user != null && num == 1 && !user.IsOpenBalance)
						{
							this.ShowMessageAndCode(context, "请先开通预付款帐号", -4);
						}
						else
						{
							string text5 = (context.Request["RefundReason"] == null) ? "" : Globals.StripAllTags(context.Request["RefundReason"]);
							if (string.IsNullOrEmpty(text5))
							{
								this.ShowMessageAndCode(context, "请选择退款原因", -5);
							}
							else
							{
								string refundGateWay = string.IsNullOrEmpty(orderInfo.Gateway) ? "" : orderInfo.Gateway.ToLower().Replace(".payment.", ".refund.");
								string generateId = Globals.GetGenerateId();
								RefundInfo refund = new RefundInfo
								{
									UserRemark = userRemark,
									RefundReason = text5,
									RefundType = (RefundTypes)num,
									RefundGateWay = refundGateWay,
									RefundOrderId = generateId,
									RefundAmount = num4,
									StoreId = orderInfo.StoreId,
									ApplyForTime = DateTime.Now,
									BankName = text2,
									BankAccountName = text3,
									BankAccountNo = text4,
									OrderId = orderInfo.OrderId,
									IsServiceProduct = (orderInfo.OrderType == OrderType.ServiceOrder),
									Quantity = num2,
									ValidCodes = text,
									HandleStatus = RefundStatus.Applied
								};
								if (orderInfo.OrderType == OrderType.ServiceOrder)
								{
									string text6 = "申请失败";
									try
									{
										int num5 = TradeHelper.ServiceOrderApplyForRefund(refund);
										if (num5 > 0)
										{
											refund = TradeHelper.GetRefundInfo(num5);
											if (refund.Quantity == orderInfo.GetAllQuantity(true))
											{
												OrderHelper.UpdateOrderStatus(orderInfo, OrderStatus.ApplyForRefund);
											}
											SiteSettings masterSettings = SettingsManager.GetMasterSettings();
											if (masterSettings.IsAutoDealRefund)
											{
												if (orderInfo.GetTotal(false) == decimal.Zero)
												{
													if (OrderHelper.CheckRefund(orderInfo, refund, decimal.Zero, "", "自动退款", true, true))
													{
														VShopHelper.AppPushRecordForOrder(orderInfo.OrderId, "", EnumPushOrderAction.OrderRefund);
														Messenger.OrderRefund(user, orderInfo, "");
													}
													else
													{
														TradeHelper.SetOrderVerificationItemStatus(orderId, text, VerificationStatus.ApplyRefund);
													}
												}
												else if (refund.RefundType == RefundTypes.InBalance)
												{
													if (OrderHelper.CheckRefund(orderInfo, refund, num4, "", "自动退款", true, true))
													{
														VShopHelper.AppPushRecordForOrder(orderInfo.OrderId, "", EnumPushOrderAction.OrderRefund);
														Messenger.OrderRefund(user, orderInfo, "");
													}
													else
													{
														TradeHelper.SetOrderVerificationItemStatus(orderId, text, VerificationStatus.ApplyRefund);
													}
												}
												else
												{
													string text7 = TradeHelper.SendWxRefundRequest(orderInfo, num4, refund.RefundOrderId);
													if (text7 == "")
													{
														if (OrderHelper.CheckRefund(orderInfo, refund, num4, "", "自动退款", true, true))
														{
															VShopHelper.AppPushRecordForOrder(orderInfo.OrderId, "", EnumPushOrderAction.OrderRefund);
															Messenger.OrderRefund(user, orderInfo, "");
														}
														else
														{
															TradeHelper.SetOrderVerificationItemStatus(orderId, text, VerificationStatus.ApplyRefund);
														}
													}
													else
													{
														TradeHelper.SaveRefundErr(num5, text7, true);
														TradeHelper.SetOrderVerificationItemStatus(orderId, text, VerificationStatus.ApplyRefund);
													}
												}
											}
											else
											{
												TradeHelper.SetOrderVerificationItemStatus(orderId, text, VerificationStatus.ApplyRefund);
											}
											if (orderInfo.StoreId > 0)
											{
												VShopHelper.AppPsuhRecordForStore(orderInfo.StoreId, orderInfo.OrderId, "", EnumPushStoreAction.StoreOrderRefundApply);
											}
											text6 = "申请成功,退款将在1-7个工作日内到您的帐号,请即时查收";
											this.ShowMessageAndCode(context, text6, 1);
										}
									}
									catch (Exception ex)
									{
										text6 = ex.Message;
										this.ShowMessageAndCode(context, "申请退款失败," + text6, 0);
										NameValueCollection param = new NameValueCollection
										{
											context.Request.QueryString,
											context.Request.Form
										};
										Globals.WriteExceptionLog_Page(ex, param, "O2ORefundError");
									}
								}
								else if (TradeHelper.ApplyForRefund(refund))
								{
									if (orderInfo.StoreId > 0)
									{
										VShopHelper.AppPsuhRecordForStore(orderInfo.StoreId, orderInfo.OrderId, "", EnumPushStoreAction.StoreOrderRefundApply);
									}
									this.ShowMessageAndCode(context, "成功的申请了退款", 1);
								}
								else
								{
									this.ShowMessageAndCode(context, "申请退款失败", 0);
								}
							}
						}
					}
				}
			}
		}

		public void CheckSendRedEnvelope(HttpContext context)
		{
			string userAgent = context.Request.UserAgent;
			if (userAgent.ToLower().Contains("micromessenger"))
			{
				string text = context.Request["OrderId"];
				OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(text);
				WeiXinRedEnvelopeInfo openedWeiXinRedEnvelope = WeiXinRedEnvelopeProcessor.GetOpenedWeiXinRedEnvelope();
				if (openedWeiXinRedEnvelope == null)
				{
					context.Response.Write("{\"success\":\"false\",\"msg\":\"没有任何红包促销活动\"}");
				}
				else
				{
					if (openedWeiXinRedEnvelope.ActiveStartTime > DateTime.Now)
					{
						context.Response.Write("{\"success\":\"false\",\"msg\":\"活动还未开始\"}");
					}
					if (openedWeiXinRedEnvelope.ActiveEndTime < DateTime.Now)
					{
						context.Response.Write("{\"success\":\"false\",\"msg\":\"活动已过期\"}");
					}
					decimal amount = orderInfo.GetAmount(false);
					if (amount > decimal.Zero && amount >= openedWeiXinRedEnvelope.EnableIssueMinAmount)
					{
						context.Response.Write("{\"success\":\"true\",\"msg\":\"满足发红包条件\"}");
						HttpCookie httpCookie = new HttpCookie("OrderIdCookie");
						httpCookie.HttpOnly = true;
						httpCookie.Value = text;
						httpCookie.Expires = DateTime.Now.AddMinutes(20.0);
						HttpContext.Current.Response.Cookies.Add(httpCookie);
					}
					else
					{
						context.Response.Write("{\"success\":\"false\",\"msg\":\"条件未满足\"}");
					}
				}
			}
			else
			{
				context.Response.Write("{\"success\":\"false\",\"msg\":\"请使用微信浏览器\"}");
			}
		}

		public void GetSkuReferralDeduct(HttpContext context)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			int num = 0;
			int.TryParse(context.Request["ProductId"], out num);
			string text = "";
			text = Globals.StripAllTags(context.Request["SkuId"]);
			if (num == 0)
			{
				this.ShowMessage(context, "商品数据错误！", true);
			}
			else
			{
				ProductInfo productSimpleInfo = ProductBrowser.GetProductSimpleInfo(num);
				if (productSimpleInfo == null)
				{
					this.ShowMessage(context, "商品数据错误！", true);
				}
				else if (string.IsNullOrEmpty(text) || !productSimpleInfo.Skus.ContainsKey(text))
				{
					this.ShowMessage(context, "规格错误！", true);
				}
				else
				{
					SKUItem sKUItem = productSimpleInfo.Skus[text];
					decimal d = productSimpleInfo.SubMemberDeduct.HasValue ? productSimpleInfo.SubMemberDeduct.Value : decimal.Zero;
					if (d <= decimal.Zero)
					{
						d = masterSettings.SubMemberDeduct;
					}
					decimal num2 = default(decimal);
					if (d > decimal.Zero)
					{
						if (sKUItem.MemberPrices.ContainsKey(HiContext.Current.User.GradeId))
						{
							num2 = sKUItem.MemberPrices[HiContext.Current.User.GradeId] * (d / 100m);
						}
						else
						{
							MemberGradeInfo memberGrade = MemberHelper.GetMemberGrade(HiContext.Current.User.GradeId);
							if (memberGrade != null)
							{
								num2 = sKUItem.SalePrice * (decimal)memberGrade.Discount / 100m * (d / 100m);
							}
						}
					}
					context.Response.Write("{\"success\":\"true\",\"ReferralDeduct\":\"" + num2.F2ToString("f2") + "\"}");
					context.Response.End();
				}
			}
		}

		public void RequestBalanceDraw(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			Hidistro.Entities.Members.MemberInfo user = HiContext.Current.User;
			BalanceDrawRequestInfo balanceDrawRequestInfo = this.GetBalanceDrawRequestInfo(context);
			if (user.RequestBalance > decimal.Zero)
			{
				this.ShowMessage(context, "上笔提现管理员还没有处理，只有处理完后才能再次申请提现！", true);
			}
			else if (balanceDrawRequestInfo.Amount <= decimal.Zero)
			{
				this.ShowMessage(context, "提现金额输入错误,请重新输入提现金额！", true);
			}
			else
			{
				decimal amount = balanceDrawRequestInfo.Amount;
				string text = amount.ToString();
				decimal d = default(decimal);
				if (text.IndexOf(".") > 0)
				{
					d = text.Substring(text.IndexOf(".") + 1).Length;
				}
				if (d > 2m)
				{
					this.ShowMessage(context, "提现金额不能超过2位小数！", true);
				}
				else
				{
					decimal num = user.Balance - user.RequestBalance;
					if (num <= decimal.Zero)
					{
						this.ShowMessage(context, "预付款余额不足！", true);
					}
					else
					{
						decimal d2 = balanceDrawRequestInfo.Amount.ToDecimal(0);
						if (d2 > num)
						{
							this.ShowMessage(context, "预付款余额不足,请重新输入提现金额！", true);
						}
						else
						{
							SiteSettings masterSettings = SettingsManager.GetMasterSettings();
							decimal minimumSingleShot = masterSettings.MinimumSingleShot;
							if (d2 < minimumSingleShot || (balanceDrawRequestInfo.IsWeixin.HasValue && balanceDrawRequestInfo.IsWeixin.Value && d2 < decimal.One))
							{
								this.ShowMessage(context, "提现金额必须大于或者等于单次提现最小限额，微信支付最小限额为1元！", true);
							}
							else
							{
								Regex regex = new Regex("(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
								Regex regex2 = regex;
								amount = balanceDrawRequestInfo.Amount;
								if (!regex2.IsMatch(amount.ToString()))
								{
									this.ShowMessage(context, "请输入有效金额！", true);
								}
								else
								{
									if (balanceDrawRequestInfo.IsAlipay.HasValue && balanceDrawRequestInfo.IsAlipay.Value)
									{
										if (string.IsNullOrEmpty(balanceDrawRequestInfo.AlipayRealName))
										{
											this.ShowMessage(context, "真实姓名不能为空！", true);
											return;
										}
										if (balanceDrawRequestInfo.AlipayRealName.Length > 20)
										{
											this.ShowMessage(context, "真实姓名长度限制在20字符以内！", true);
											return;
										}
										if (string.IsNullOrEmpty(balanceDrawRequestInfo.AlipayCode))
										{
											this.ShowMessage(context, "收款账号不能为空！", true);
											return;
										}
										if (balanceDrawRequestInfo.AlipayCode.Length > 60)
										{
											this.ShowMessage(context, "收款账号长度限制在60字符以内！", true);
											return;
										}
									}
									else if (!balanceDrawRequestInfo.IsWeixin.HasValue || !balanceDrawRequestInfo.IsWeixin.Value)
									{
										if (string.IsNullOrEmpty(balanceDrawRequestInfo.BankName))
										{
											this.ShowMessage(context, "开户银行不能为空！", true);
											return;
										}
										if (balanceDrawRequestInfo.BankName.Length > 60)
										{
											this.ShowMessage(context, "开户银行长度限制在60字符以内！", true);
											return;
										}
										if (string.IsNullOrEmpty(balanceDrawRequestInfo.AccountName))
										{
											this.ShowMessage(context, "银行开户名不能为空！", true);
											return;
										}
										if (balanceDrawRequestInfo.AccountName.Length > 30)
										{
											this.ShowMessage(context, "银行开户名长度限制在30字符以内！", true);
											return;
										}
										if (string.IsNullOrEmpty(balanceDrawRequestInfo.MerchantCode))
										{
											this.ShowMessage(context, "银行开户名不能为空！", true);
											return;
										}
										if (balanceDrawRequestInfo.MerchantCode.Length > 100)
										{
											this.ShowMessage(context, "提现账号限制在100个字符以内！", true);
											return;
										}
									}
									string empty = string.Empty;
									if (!string.IsNullOrEmpty(context.Request["TradePassword"]))
									{
										empty = Globals.UrlDecode(context.Request["TradePassword"]);
										if (empty.Length < 6 || empty.Length > 20)
										{
											this.ShowMessage(context, "交易密码必须在6-20个字符之间！", true);
										}
										else if (!MemberProcessor.ValidTradePassword(empty))
										{
											this.ShowMessage(context, "交易密码不正确,请重新输入！", true);
										}
										else if (balanceDrawRequestInfo.Remark.Length > 300)
										{
											this.ShowMessage(context, "请填写详细的提现备注，大小限制300个字符以内！", true);
										}
										else if (MemberProcessor.BalanceDrawRequest(balanceDrawRequestInfo))
										{
											this.ShowMessage(context, "提现申请提交成功！", false);
										}
										else
										{
											this.ShowMessage(context, "申请提现过程中出现未知错误！", true);
										}
									}
									else
									{
										this.ShowMessage(context, "请输入交易密码！", true);
									}
								}
							}
						}
					}
				}
			}
		}

		private BalanceDrawRequestInfo GetBalanceDrawRequestInfo(HttpContext context)
		{
			BalanceDrawRequestInfo balanceDrawRequestInfo = new BalanceDrawRequestInfo();
			balanceDrawRequestInfo.UserId = HiContext.Current.UserId;
			balanceDrawRequestInfo.UserName = HiContext.Current.User.UserName;
			balanceDrawRequestInfo.RequestTime = DateTime.Now;
			int num = 0;
			if (!string.IsNullOrEmpty(context.Request["drawtype"]))
			{
				OnLinePayment onLinePayment;
				switch (Globals.UrlDecode(context.Request["drawtype"]).ToInt(0))
				{
				case 2:
				{
					balanceDrawRequestInfo.IsWeixin = true;
					BalanceDrawRequestInfo balanceDrawRequestInfo3 = balanceDrawRequestInfo;
					onLinePayment = OnLinePayment.NoPay;
					balanceDrawRequestInfo3.RequestState = onLinePayment.GetHashCode().ToNullString();
					break;
				}
				case 3:
				{
					balanceDrawRequestInfo.IsAlipay = true;
					BalanceDrawRequestInfo balanceDrawRequestInfo2 = balanceDrawRequestInfo;
					onLinePayment = OnLinePayment.NoPay;
					balanceDrawRequestInfo2.RequestState = onLinePayment.GetHashCode().ToNullString();
					break;
				}
				}
			}
			if (!string.IsNullOrEmpty(context.Request["RealName"]))
			{
				balanceDrawRequestInfo.AlipayRealName = Globals.UrlDecode(context.Request["RealName"]);
			}
			else
			{
				balanceDrawRequestInfo.AlipayRealName = string.Empty;
			}
			if (!string.IsNullOrEmpty(context.Request["Code"]))
			{
				balanceDrawRequestInfo.AlipayCode = Globals.UrlDecode(context.Request["Code"]);
			}
			else
			{
				balanceDrawRequestInfo.AlipayCode = string.Empty;
			}
			if (!string.IsNullOrEmpty(context.Request["BankName"]))
			{
				balanceDrawRequestInfo.BankName = Globals.UrlDecode(context.Request["BankName"]);
			}
			else
			{
				balanceDrawRequestInfo.BankName = string.Empty;
			}
			if (!string.IsNullOrEmpty(context.Request["AccountName"]))
			{
				balanceDrawRequestInfo.AccountName = Globals.UrlDecode(context.Request["AccountName"]);
			}
			else
			{
				balanceDrawRequestInfo.AccountName = string.Empty;
			}
			if (!string.IsNullOrEmpty(context.Request["MerchantCode"]))
			{
				balanceDrawRequestInfo.MerchantCode = Globals.UrlDecode(context.Request["MerchantCode"]);
			}
			else
			{
				balanceDrawRequestInfo.MerchantCode = string.Empty;
			}
			decimal amount = default(decimal);
			if (!string.IsNullOrEmpty(context.Request["Amount"]) && decimal.TryParse(context.Request["Amount"], out amount))
			{
				balanceDrawRequestInfo.Amount = amount;
			}
			else
			{
				balanceDrawRequestInfo.Amount = decimal.Zero;
			}
			if (!string.IsNullOrEmpty(context.Request["Remark"]))
			{
				balanceDrawRequestInfo.Remark = Globals.UrlDecode(context.Request["Remark"]);
			}
			else
			{
				balanceDrawRequestInfo.Remark = string.Empty;
			}
			return balanceDrawRequestInfo;
		}

		public void OpenBalance(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			string empty = string.Empty;
			string empty2 = string.Empty;
			empty = context.Request["password"];
			empty2 = context.Request["confirmPassword"];
			if (string.IsNullOrEmpty(empty))
			{
				this.ShowMessage(context, "请输入交易密码！", true);
			}
			else if (empty.Length < 6 || empty.Length > 20)
			{
				this.ShowMessage(context, "交易密码限制为6-20个字符！", true);
			}
			else if (string.IsNullOrEmpty(empty2))
			{
				this.ShowMessage(context, "请确认交易密码！", true);
			}
			else if (string.Compare(empty, empty2) != 0)
			{
				this.ShowMessage(context, "两次输入的交易密码不一致！", true);
			}
			else if (MemberProcessor.OpenBalance(empty))
			{
				Users.ClearUserCache(HiContext.Current.UserId, HiContext.Current.User.SessionId);
				this.ShowMessage(context, "预付款账户开通成功！", false);
			}
		}

		public void SplittinDraws(HttpContext context)
		{
			SplittinDrawInfo splittinDrawRequestInfo = this.GetSplittinDrawRequestInfo(context);
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			Hidistro.Entities.Members.MemberInfo user = HiContext.Current.User;
			if (user == null || user.UserId == 0)
			{
				this.ShowMessage(context, "您还未登录会员", true);
			}
			else
			{
				BalanceDrawRequestQuery balanceDrawRequestQuery = new BalanceDrawRequestQuery();
				balanceDrawRequestQuery.PageIndex = 1;
				balanceDrawRequestQuery.PageSize = 1;
				balanceDrawRequestQuery.UserId = HiContext.Current.UserId;
				DbQueryResult mySplittinDraws = MemberProcessor.GetMySplittinDraws(balanceDrawRequestQuery, 1);
				if (mySplittinDraws.TotalRecords > 0)
				{
					this.ShowMessage(context, "上笔提现管理员还没有处理，只有处理完后才能再次申请提现", true);
				}
				else if (!masterSettings.SplittinDraws_CashToDeposit && !masterSettings.SplittinDraws_CashToBankCard && !masterSettings.SplittinDraws_CashToWeiXin && !masterSettings.SplittinDraws_CashToALiPay)
				{
					this.ShowMessage(context, "没有合适的提现方式，请与管理员联系", true);
				}
				else
				{
					int num = context.Request["drawtype"].ToInt(0);
					if (num < 1 || num > 4)
					{
						this.ShowMessage(context, "请选择正确的提现方式", true);
					}
					else if (num == 4 && !HiContext.Current.User.IsOpenBalance)
					{
						this.ShowMessage(context, "您还没有开启余额帐号", true);
					}
					else if (num == 2 && !masterSettings.EnableBulkPaymentWeixin)
					{
						this.ShowMessage(context, "系统不支持提现到微信", true);
					}
					else if (num == 3 && !masterSettings.EnableBulkPaymentAliPay)
					{
						this.ShowMessage(context, "系统不支持提现到支付宝", true);
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
							this.ShowMessage(context, "提现金额不能超过2位小数！", true);
						}
						else if (splittinDrawRequestInfo.Amount < masterSettings.MinimumSingleShot)
						{
							this.ShowMessage(context, "提现金额必须大于或者等于单次提现最小限额", true);
						}
						else
						{
							decimal userUseSplittin = MemberProcessor.GetUserUseSplittin(HiContext.Current.UserId);
							if (splittinDrawRequestInfo.Amount <= decimal.Zero)
							{
								this.ShowMessage(context, "提现金额输入错误,请重新输入提现金额！", true);
							}
							else if (splittinDrawRequestInfo.Amount > userUseSplittin)
							{
								this.ShowMessage(context, "可提现奖励不足,请重新输入提现金额！", true);
							}
							else
							{
								Regex regex = new Regex("(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
								Regex regex2 = regex;
								amount = splittinDrawRequestInfo.Amount;
								if (!regex2.IsMatch(amount.ToString()))
								{
									this.ShowMessage(context, "请输入有效金额！", true);
								}
								else
								{
									if (splittinDrawRequestInfo.IsAlipay.HasValue && splittinDrawRequestInfo.IsAlipay.Value)
									{
										if (string.IsNullOrEmpty(splittinDrawRequestInfo.AlipayRealName))
										{
											this.ShowMessage(context, "真实姓名不能为空！", true);
											return;
										}
										if (splittinDrawRequestInfo.AlipayRealName.Length > 20)
										{
											this.ShowMessage(context, "真实姓名长度限制在20字符以内！", true);
											return;
										}
										if (string.IsNullOrEmpty(splittinDrawRequestInfo.AlipayCode))
										{
											this.ShowMessage(context, "收款账号不能为空！", true);
											return;
										}
										if (splittinDrawRequestInfo.AlipayCode.Length > 60)
										{
											this.ShowMessage(context, "收款账号长度限制在60字符以内！", true);
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
												this.ShowMessage(context, "开户银行不能为空！", true);
												return;
											}
											if (splittinDrawRequestInfo.BankName.Length > 60)
											{
												this.ShowMessage(context, "开户银行长度限制在60字符以内！", true);
												return;
											}
											if (string.IsNullOrEmpty(splittinDrawRequestInfo.AccountName))
											{
												this.ShowMessage(context, "银行开户名不能为空！", true);
												return;
											}
											if (splittinDrawRequestInfo.AccountName.Length > 30)
											{
												this.ShowMessage(context, "银行开户名长度限制在30字符以内！", true);
												return;
											}
											if (string.IsNullOrEmpty(splittinDrawRequestInfo.MerchantCode))
											{
												this.ShowMessage(context, "银行开户名不能为空！", true);
												return;
											}
											if (splittinDrawRequestInfo.MerchantCode.Length > 100)
											{
												this.ShowMessage(context, "提现账号限制在100个字符以内！", true);
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
											this.ShowMessage(context, "交易密码必须在6-20个字符之间！", true);
										}
										else if (!MemberProcessor.ValidTradePassword(empty))
										{
											this.ShowMessage(context, "交易密码不正确,请重新输入！", true);
										}
										else if (splittinDrawRequestInfo.Remark.Length > 300)
										{
											this.ShowMessage(context, "请填写详细的提现备注，大小限制300个字符以内！", true);
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
												this.ShowMessage(context, "提现成功，申请金额已转至您的预付款账户", false);
											}
											else
											{
												this.ShowMessage(context, "提现申请成功，等待管理员的审核", false);
											}
										}
										else
										{
											this.ShowMessage(context, "提现申请失败，请重试", true);
										}
									}
									else
									{
										this.ShowMessage(context, "请输入交易密码！", true);
									}
								}
							}
						}
					}
				}
			}
		}

		private void SaveBalance(int UserId, decimal Amount)
		{
			Hidistro.Entities.Members.MemberInfo user = Users.GetUser(UserId);
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

		public void ShowMessage(HttpContext context, string msg, bool IsError = true)
		{
			if (IsError)
			{
				context.Response.Write("{\"success\":\"false\",\"msg\":\"" + msg + "\"}");
			}
			else
			{
				context.Response.Write("{\"success\":\"true\",\"msg\":\"" + msg + "\"}");
			}
		}

		public void ShowMessageAndCode(HttpContext context, string msg, int msgCode)
		{
			context.Response.Write("{\"Status\":\"" + msgCode + "\",\"msg\":\"" + msg + "\"}");
		}

		public void ReferralRegister(HttpContext context)
		{
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
			if (HiContext.Current.SiteSettings.ApplyReferralCondition == 1 && HiContext.Current.User.Expenditure < HiContext.Current.SiteSettings.ApplyReferralNeedAmount)
			{
				this.ShowMessage(context, "需要累计消费金额达到" + HiContext.Current.SiteSettings.ApplyReferralNeedAmount.F2ToString("f2") + "元才可申请哦", true);
			}
			else
			{
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
				if (HiContext.Current.UserId == 0)
				{
					this.ShowMessage(context, "请您先登录！", true);
				}
				else if (string.IsNullOrEmpty(text7) || text7.Length > 100)
				{
					this.ShowMessage(context, "请输入店铺名称,长度在1-10个之间", true);
				}
				else if (string.IsNullOrEmpty(text) & flag)
				{
					this.ShowMessage(context, "请填写真实姓名,长度在1-10个之间", true);
				}
				else if (string.IsNullOrEmpty(text3) & flag3)
				{
					this.ShowMessage(context, "请填写邮箱", true);
				}
				else
				{
					if (text3.Length > 0)
					{
						Hidistro.Entities.Members.MemberInfo memberInfo = MemberProcessor.FindMemberByEmail(text3);
						if (memberInfo != null && memberInfo.UserId != HiContext.Current.UserId)
						{
							this.ShowMessage(context, "该邮箱地址已被其他用户使用", true);
							return;
						}
					}
					if (string.IsNullOrEmpty(text4) & flag2)
					{
						this.ShowMessage(context, "请填写手机号码", true);
					}
					else if (text4.Length > 0 && !DataHelper.IsMobile(text4))
					{
						this.ShowMessage(context, "请输入正确的手机号码", true);
					}
					else
					{
						if (text4.Length > 0)
						{
							Hidistro.Entities.Members.MemberInfo memberInfo2 = MemberProcessor.FindMemberByCellphone(text4);
							if (memberInfo2 != null && memberInfo2.UserId != HiContext.Current.UserId)
							{
								this.ShowMessage(context, "该手机号码已被其他用户使用", true);
								return;
							}
						}
						if (isPromoterValidatePhone & flag2)
						{
							if (string.IsNullOrEmpty(text5))
							{
								this.ShowMessage(context, "请填写图形验证码", true);
								return;
							}
							if (!HiContext.Current.CheckVerifyCode(text5, ""))
							{
								this.ShowMessage(context, "图形验证码填写错误", true);
								return;
							}
							if (string.IsNullOrEmpty(text6))
							{
								this.ShowMessage(context, "请填写短信验证码", true);
								return;
							}
							string msg = "";
							if (!HiContext.Current.CheckPhoneVerifyCode(text6, text4, out msg))
							{
								this.ShowMessage(context, msg, true);
								return;
							}
						}
						if (topRegionId == 0 & flag4)
						{
							this.ShowMessage(context, "请填写地址", true);
						}
						else if (string.IsNullOrEmpty(text2) & flag4)
						{
							this.ShowMessage(context, "请填写详细地址", true);
						}
						else if (MemberProcessor.ReferralRequest(HiContext.Current.UserId, text, text4, topRegionId, num, text2, text3, text7, text8))
						{
							Users.ClearUserCache(HiContext.Current.UserId, HiContext.Current.User.SessionId);
							this.ShowMessage(context, "申请提交成功！", false);
						}
						else
						{
							this.ShowMessage(context, "提交申请失败！", true);
						}
					}
				}
			}
		}

		private void submitorder(HttpContext context)
		{
		}

		private void CheckFavorite(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			Hidistro.Entities.Members.MemberInfo user = HiContext.Current.User;
			if (user.UserId == 0)
			{
				context.Response.Write("{\"success\":false}");
			}
			else
			{
				int productId = context.Request["ProductId"].ToInt(0);
				if (ProductBrowser.GetProductSimpleInfo(productId) != null)
				{
					context.Response.Write("{\"success\":true}");
				}
				else
				{
					context.Response.Write("{\"success\":false}");
				}
			}
		}

		private void DelFavorite(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			string text = Globals.StripAllTags(context.Request["favoriteId"].ToNullString());
			string[] array = text.Split(',');
			try
			{
				for (int i = 0; i < array.Length; i++)
				{
					ProductBrowser.DeleteFavorite(Convert.ToInt32(array[i]));
				}
				context.Response.Write("{\"success\":true}");
			}
			catch (Exception)
			{
				context.Response.Write("{\"success\":false, \"msg\":\"删除失败\"}");
			}
		}

		private void AddFavorite(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			Hidistro.Entities.Members.MemberInfo user = HiContext.Current.User;
			if (user.UserId == 0)
			{
				context.Response.Write("{\"success\":false, \"msg\":\"请先登录才可以收藏商品\"}");
			}
			else
			{
				int productId = context.Request["ProductId"].ToInt(0);
				int storeId = context.Request["StoreId"].ToInt(0);
				int num = ProductBrowser.AddProductToFavorite(productId, user.UserId, storeId);
				if (num > 0)
				{
					context.Response.Write("{\"success\":true,\"status\":" + num + "}");
				}
				else
				{
					context.Response.Write("{\"success\":false, \"msg\":\"提交失败\"}");
				}
			}
		}

		private void AddProductReview(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			Hidistro.Entities.Members.MemberInfo user = HiContext.Current.User;
			int productId = 0;
			if (!int.TryParse((context.Request["ProductId"] == null) ? "0" : context.Request["ProductId"].ToString(), out productId))
			{
				context.Response.Write("{\"success\":false, \"msg\":\"您没有购买此商品(或此商品的订单尚未完成)，因此不能进行评论\"}");
			}
			else
			{
				string text = (context.Request["OrderId"] == null) ? "" : context.Request["OrderId"].ToString();
				if (string.IsNullOrEmpty(text))
				{
					context.Response.Write("{\"success\":false, \"msg\":\"您没有购买此商品(或此商品的订单尚未完成)，因此不能进行评论\"}");
				}
				else
				{
					string strToStrip = (context.Request["SkuId"] == null) ? "" : context.Request["SkuId"].ToString();
					strToStrip = Globals.StripAllTags(strToStrip);
					OrderInfo orderInfo = OrderHelper.GetOrderInfo(text);
					if (orderInfo.OrderStatus != OrderStatus.Finished && (orderInfo.OrderStatus != OrderStatus.Closed || orderInfo.OnlyReturnedCount != orderInfo.LineItems.Count))
					{
						context.Response.Write("{\"success\":false, \"msg\":\"您的订单还未完成，因此不能对该商品进行评论\"}");
					}
					else
					{
						int num = default(int);
						int num2 = default(int);
						ProductBrowser.LoadProductReview(productId, out num, out num2, text);
						if (num == 0)
						{
							context.Response.Write("{\"success\":false, \"msg\":\"您没有购买此商品(或此商品的订单尚未完成)，因此不能进行评论\"}");
						}
						else if (num2 >= num)
						{
							context.Response.Write("{\"success\":false, \"msg\":\"您已经对此商品进行过评论(或此商品的订单尚未完成)，因此不能再次进行评论\"}");
						}
						else
						{
							ProductReviewInfo productReviewInfo = new ProductReviewInfo();
							productReviewInfo.ReviewDate = DateTime.Now;
							productReviewInfo.ReviewText = Globals.StripAllTags(context.Request["ReviewText"].ToNullString());
							productReviewInfo.ProductId = productId;
							productReviewInfo.UserEmail = user.Email;
							productReviewInfo.UserId = user.UserId;
							productReviewInfo.UserName = user.UserName;
							productReviewInfo.OrderId = text;
							productReviewInfo.SkuId = strToStrip;
							if (ProductBrowser.InsertProductReview(productReviewInfo))
							{
								context.Response.Write("{\"success\":true}");
							}
							else
							{
								context.Response.Write("{\"success\":false, \"msg\":\"提交失败\"}");
							}
						}
					}
				}
			}
		}

		private void AddProductConsultations(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			string text = Globals.StripAllTags(context.Request["userName"].ToNullString());
			Hidistro.Entities.Members.MemberInfo user = HiContext.Current.User;
			if (user == null && string.IsNullOrEmpty(text))
			{
				context.Response.Write("{\"success\":false, \"msg\":\"您还未登录\"}");
			}
			else
			{
				ProductConsultationInfo productConsultationInfo = new ProductConsultationInfo();
				productConsultationInfo.ConsultationDate = DateTime.Now;
				productConsultationInfo.ConsultationText = Globals.StripAllTags(context.Request["ConsultationText"].ToNullString());
				productConsultationInfo.ProductId = context.Request["ProductId"].ToInt(0);
				productConsultationInfo.UserEmail = ((user == null) ? "" : user.Email);
				productConsultationInfo.UserId = (user?.UserId ?? 0);
				productConsultationInfo.UserName = (string.IsNullOrEmpty(text) ? ((user == null) ? "" : user.UserName) : text);
				if (ProductBrowser.InsertProductConsultation(productConsultationInfo))
				{
					context.Response.Write("{\"success\":true}");
				}
				else
				{
					context.Response.Write("{\"success\":false, \"msg\":\"提交失败\"}");
				}
			}
		}

		private void FinishOrder(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			string orderId = Globals.StripAllTags(context.Request["orderId"].ToNullString());
			OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(orderId);
			if (orderInfo != null && TradeHelper.ConfirmOrderFinish(orderInfo))
			{
				context.Response.Write("{\"success\":true}");
			}
			else
			{
				context.Response.Write("{\"success\":false, \"msg\":\"订单当前状态不允许完成\"}");
			}
		}

		private void CloseOrder(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			string orderId = Globals.StripAllTags(context.Request["orderId"].ToNullString());
			OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(orderId);
			if (orderInfo != null && TradeHelper.CloseOrder(orderInfo.OrderId, "会员主动关闭"))
			{
				Messenger.OrderClosed(HiContext.Current.User, orderInfo, "用户自己关闭订单");
				context.Response.Write("{\"success\":true}");
			}
			else
			{
				context.Response.Write("{\"success\":false, \"msg\":\"订单当前状态不允许取消\"}");
			}
		}

		public void SearchReplaceExpressData(HttpContext context)
		{
			string text = "{\"success\":\"false\";}";
			if (!string.IsNullOrEmpty(context.Request["OperId"]))
			{
				int replaceId = 0;
				int.TryParse(context.Request["OperId"], out replaceId);
				ReplaceInfo replaceInfo = TradeHelper.GetReplaceInfo(replaceId);
				string text2 = (context.Request["InfoType"] == null) ? "" : Globals.StripAllTags(context.Request["InfoType"]);
				if (replaceInfo != null)
				{
					if (string.IsNullOrEmpty(text2))
					{
						if (replaceInfo.HandleStatus == ReplaceStatus.UserDelivery && !string.IsNullOrEmpty(replaceInfo.UserExpressCompanyAbb))
						{
							text = ExpressHelper.GetDataByKuaidi100(replaceInfo.UserExpressCompanyAbb, replaceInfo.UserShipOrderNumber);
						}
						if ((replaceInfo.HandleStatus == ReplaceStatus.MerchantsDelivery || replaceInfo.HandleStatus == ReplaceStatus.Replaced) && !string.IsNullOrEmpty(replaceInfo.ExpressCompanyAbb))
						{
							text = ExpressHelper.GetDataByKuaidi100(replaceInfo.ExpressCompanyAbb, replaceInfo.ShipOrderNumber);
						}
					}
					else
					{
						if (text2 == "User" && !string.IsNullOrEmpty(replaceInfo.UserExpressCompanyAbb) && !string.IsNullOrEmpty(replaceInfo.UserShipOrderNumber))
						{
							text = ExpressHelper.GetDataByKuaidi100(replaceInfo.UserExpressCompanyAbb, replaceInfo.UserShipOrderNumber);
						}
						if (text2 == "Mall" && !string.IsNullOrEmpty(replaceInfo.ExpressCompanyAbb) && !string.IsNullOrEmpty(replaceInfo.ShipOrderNumber))
						{
							text = ExpressHelper.GetDataByKuaidi100(replaceInfo.ExpressCompanyAbb, replaceInfo.ShipOrderNumber);
						}
					}
					if (text == "暂时没有此快递单号的信息")
					{
						text = "{\"success\":\"false\",\"shipperCode\":\"" + replaceInfo.ExpressCompanyAbb + "\",\"logisticsCode\":\"" + replaceInfo.ShipOrderNumber + "\";}";
					}
				}
			}
			context.Response.ContentType = "application/json";
			context.Response.Write(text);
			context.Response.End();
		}

		public void SearchReturnExpressData(HttpContext context)
		{
			string text = "{\"success\":\"false\";}";
			if (!string.IsNullOrEmpty(context.Request["OperId"]))
			{
				int returnId = 0;
				int.TryParse(context.Request["OperId"], out returnId);
				ReturnInfo returnInfo = TradeHelper.GetReturnInfo(returnId);
				if (returnInfo != null && (returnInfo.HandleStatus == ReturnStatus.Deliverying || returnInfo.HandleStatus == ReturnStatus.GetGoods || returnInfo.HandleStatus == ReturnStatus.Returned) && !string.IsNullOrEmpty(returnInfo.ExpressCompanyAbb))
				{
					text = ExpressHelper.GetDataByKuaidi100(returnInfo.ExpressCompanyAbb, returnInfo.ShipOrderNumber);
					if (text == "暂时没有此快递单号的信息")
					{
						text = "{\"success\":\"false\",\"shipperCode\":\"" + returnInfo.ExpressCompanyAbb + "\",\"logisticsCode\":\"" + returnInfo.ShipOrderNumber + "\";}";
					}
				}
			}
			context.Response.ContentType = "application/json";
			context.Response.Write(text);
			context.Response.End();
		}

		private void SearchExpressData(HttpContext context)
		{
			string text = "{\"success\":\"false\"}";
			if (!string.IsNullOrEmpty(context.Request["OrderId"]))
			{
				string orderId = Globals.StripAllTags(context.Request["OrderId"].ToNullString());
				OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(orderId);
				if (orderInfo != null && (orderInfo.OrderStatus == OrderStatus.SellerAlreadySent || orderInfo.OrderStatus == OrderStatus.Finished) && !string.IsNullOrEmpty(orderInfo.ExpressCompanyAbb))
				{
					text = ExpressHelper.GetDataByKuaidi100(orderInfo.ExpressCompanyAbb, orderInfo.ShipOrderNumber);
					if (text.Contains("此单无物流信息"))
					{
						text = "{\"success\":\"false\",\"shipperCode\":\"" + orderInfo.ExpressCompanyAbb + "\",\"logisticsCode\":\"" + orderInfo.ShipOrderNumber + "\";}";
					}
				}
			}
			context.Response.ContentType = "application/json";
			context.Response.Write(text);
			context.Response.End();
		}

		private void AddSignUp(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			int num = context.Request["id"].ToInt(0);
			string b = Globals.StripAllTags(context.Request["code"]);
			LotteryTicketInfo lotteryTicket = VshopBrowser.GetLotteryTicket(num);
			if (!string.IsNullOrEmpty(lotteryTicket.InvitationCode) && lotteryTicket.InvitationCode != b)
			{
				context.Response.Write("{\"success\":false, \"msg\":\"邀请码不正确\"}");
			}
			else if (lotteryTicket.EndTime < DateTime.Now)
			{
				context.Response.Write("{\"success\":false, \"msg\":\"活动已结束\"}");
			}
			else if (lotteryTicket.OpenTime < DateTime.Now)
			{
				context.Response.Write("{\"success\":false, \"msg\":\"报名已结束\"}");
			}
			else
			{
				PrizeRecordInfo userPrizeRecord = VshopBrowser.GetUserPrizeRecord(num);
				if (userPrizeRecord == null)
				{
					userPrizeRecord = new PrizeRecordInfo();
					userPrizeRecord.ActivityID = num;
					Hidistro.Entities.Members.MemberInfo user = HiContext.Current.User;
					userPrizeRecord.UserID = user.UserId;
					userPrizeRecord.UserName = user.UserName;
					userPrizeRecord.IsPrize = true;
					userPrizeRecord.Prizelevel = "已报名";
					VshopBrowser.AddPrizeRecord(userPrizeRecord);
					context.Response.Write("{\"success\":true, \"msg\":\"报名成功\"}");
				}
				else
				{
					context.Response.Write("{\"success\":false, \"msg\":\"你已经报名了，请不要重复报名！\"}");
				}
			}
		}

		private void AddTicket(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			int num = context.Request["activityid"].ToInt(0);
			LotteryTicketInfo lotteryTicket = VshopBrowser.GetLotteryTicket(num);
			Hidistro.Entities.Members.MemberInfo user = HiContext.Current.User;
			if (user.UserId != 0 && !lotteryTicket.GradeIds.Contains(user.GradeId.ToString()))
			{
				context.Response.Write("{\"success\":false, \"msg\":\"您的会员等级不在此活动范围内\"}");
			}
			else if (lotteryTicket.EndTime < DateTime.Now)
			{
				context.Response.Write("{\"success\":false, \"msg\":\"活动已结束\"}");
			}
			else if (DateTime.Now < lotteryTicket.OpenTime)
			{
				context.Response.Write("{\"success\":false, \"msg\":\"抽奖还未开始\"}");
			}
			else if (VshopBrowser.GetCountBySignUp(num) < lotteryTicket.MinValue)
			{
				context.Response.Write("{\"success\":false, \"msg\":\"还未达到人数下限\"}");
			}
			else
			{
				PrizeRecordInfo userPrizeRecord = VshopBrowser.GetUserPrizeRecord(num);
				try
				{
					if (!lotteryTicket.IsOpened)
					{
						VshopBrowser.OpenTicket(num);
						userPrizeRecord = VshopBrowser.GetUserPrizeRecord(num);
					}
					else if (!string.IsNullOrWhiteSpace(userPrizeRecord.RealName) && !string.IsNullOrWhiteSpace(userPrizeRecord.CellPhone))
					{
						context.Response.Write("{\"success\":false, \"msg\":\"您已经抽过奖了\"}");
						return;
					}
					if (userPrizeRecord == null || string.IsNullOrEmpty(userPrizeRecord.PrizeName))
					{
						context.Response.Write("{\"success\":false, \"msg\":\"很可惜,你未中奖\"}");
						return;
					}
					if (!userPrizeRecord.PrizeTime.HasValue)
					{
						userPrizeRecord.PrizeTime = DateTime.Now;
						VshopBrowser.UpdatePrizeRecord(userPrizeRecord);
					}
				}
				catch (Exception ex)
				{
					context.Response.Write("{\"success\":false, \"msg\":\"" + ex.Message + "\"}");
					return;
				}
				context.Response.Write("{\"success\":true, \"msg\":\"恭喜你获得" + userPrizeRecord.Prizelevel + "\"}");
			}
		}

		private void SubmitActivity(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			Hidistro.Entities.Members.MemberInfo user = HiContext.Current.User;
			if (user.UserId == 0)
			{
				context.Response.Write("{\"success\":false}");
			}
			else
			{
				int activityId = Convert.ToInt32(context.Request.Form.Get("id"));
				VActivityInfo activity = VshopBrowser.GetActivity(activityId);
				if (DateTime.Now < activity.StartDate || DateTime.Now > activity.EndDate)
				{
					context.Response.Write("{\"success\":false, \"msg\":\"报名还未开始或已结束\"}");
				}
				else if (VshopBrowser.GetActivityCount(activityId) >= activity.MaxValue && activity.MaxValue > 0)
				{
					context.Response.Write("{\"success\":false, \"msg\":\"报名人数已达到限制人数\"}");
				}
				else
				{
					ActivitySignUpInfo activitySignUpInfo = new ActivitySignUpInfo();
					activitySignUpInfo.ActivityId = context.Request.Form.Get("id").ToInt(0);
					activitySignUpInfo.Item1 = Globals.StripAllTags(context.Request.Form.Get("item1"));
					activitySignUpInfo.Item2 = Globals.StripAllTags(context.Request.Form.Get("item2"));
					activitySignUpInfo.Item3 = Globals.StripAllTags(context.Request.Form.Get("item3"));
					activitySignUpInfo.Item4 = Globals.StripAllTags(context.Request.Form.Get("item4"));
					activitySignUpInfo.Item5 = Globals.StripAllTags(context.Request.Form.Get("item5"));
					activitySignUpInfo.RealName = user.RealName;
					activitySignUpInfo.SignUpDate = DateTime.Now;
					activitySignUpInfo.UserId = user.UserId;
					activitySignUpInfo.UserName = user.UserName;
					string s = VshopBrowser.SaveActivitySignUp(activitySignUpInfo) ? "{\"success\":true}" : "{\"success\":false, \"msg\":\"你已经报过名了,请勿重复报名\"}";
					context.Response.Write(s);
				}
			}
		}

		private void DelShippingAddress(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			Hidistro.Entities.Members.MemberInfo user = HiContext.Current.User;
			if (user.UserId == 0)
			{
				context.Response.Write("{\"success\":false}");
			}
			else
			{
				int userId = user.UserId;
				int shippingid = context.Request.Form["shippingid"].ToInt(0);
				if (MemberProcessor.DelShippingAddress(shippingid, userId))
				{
					context.Response.Write("{\"success\":true}");
				}
				else
				{
					context.Response.Write("{\"success\":false}");
				}
			}
		}

		private void SetDefaultShippingAddress(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			Hidistro.Entities.Members.MemberInfo user = HiContext.Current.User;
			if (user.UserId == 0)
			{
				context.Response.Write("{\"success\":false}");
			}
			else
			{
				int userId = user.UserId;
				int shippingId = context.Request.Form["shippingid"].ToInt(0);
				if (MemberProcessor.SetDefaultShippingAddress(shippingId, userId))
				{
					context.Response.Write("{\"success\":true}");
				}
				else
				{
					context.Response.Write("{\"success\":false}");
				}
			}
		}

		private void AddShippingAddress(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			Hidistro.Entities.Members.MemberInfo user = HiContext.Current.User;
			if (user.UserId == 0)
			{
				context.Response.Write("{\"success\":false,\"msg\":\"用户未登录\"}");
			}
			else if (MemberProcessor.GetShippingAddressCount(user.UserId) >= HiContext.Current.SiteSettings.UserAddressMaxCount)
			{
				context.Response.Write("{\"success\":false,\"msg\":\"收货地址已超过限制个数！\"}");
			}
			else
			{
				string str = "";
				if (!this.ValShippingAddress(context, ref str))
				{
					context.Response.Write("{\"success\":false,\"msg\":\"" + str + "\"}");
				}
				else
				{
					ShippingAddressInfo shippingAddressInfo = new ShippingAddressInfo();
					shippingAddressInfo.Address = Globals.StripAllTags(context.Request.Form["address"].ToNullString());
					shippingAddressInfo.CellPhone = Globals.StripAllTags(context.Request.Form["cellphone"].ToNullString());
					shippingAddressInfo.ShipTo = Globals.StripAllTags(context.Request.Form["shipTo"].ToNullString());
					shippingAddressInfo.Zipcode = Globals.StripAllTags(context.Request.Form["zipCode"].ToNullString());
					shippingAddressInfo.IsDefault = context.Request.Form["IsDefault"].ToBool();
					shippingAddressInfo.UserId = user.UserId;
					shippingAddressInfo.BuildingNumber = Globals.StripAllTags(context.Request["BuildingNumber"].ToNullString());
					string text = context.Request.Form["latitude"].ToNullString();
					string str2 = context.Request.Form["longitude"].ToNullString();
					string regionLocation = context.Request.Form["regionLocation"].ToNullString();
					if (text.ToDouble(0) > 0.0)
					{
						shippingAddressInfo.LatLng = text + "," + str2;
						shippingAddressInfo.RegionLocation = regionLocation;
					}
					shippingAddressInfo.RegionId = context.Request.Form["regionSelectorValue"].ToInt(0);
					if (shippingAddressInfo.RegionId <= 0 || string.IsNullOrEmpty(RegionHelper.GetFullRegion(shippingAddressInfo.RegionId, " ", true, 0)))
					{
						context.Response.Write("{\"success\":false}");
					}
					else
					{
						shippingAddressInfo.FullRegionPath = RegionHelper.GetFullPath(shippingAddressInfo.RegionId, true);
						int num = MemberProcessor.AddShippingAddress(shippingAddressInfo);
						if (shippingAddressInfo.RegionId <= 0 || RegionHelper.GetRegion(shippingAddressInfo.RegionId, true) == null)
						{
							context.Response.Write("{\"success\":false,\"msg\":\"错误的地区信息\"}");
						}
						else if (num > 0)
						{
							context.Response.Write("{\"success\":true,\"shipAddressId\":\"" + num + "\"}");
						}
						else
						{
							context.Response.Write("{\"success\":false,\"shipAddressId\":\"" + num + "\",\"msg\":\"未知错误\"}");
						}
					}
				}
			}
		}

		private void UpdateShippingAddress(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			Hidistro.Entities.Members.MemberInfo user = HiContext.Current.User;
			if (user.UserId == 0)
			{
				context.Response.Write("{\"success\":false,\"msg\":\"用户未登录\"}");
			}
			else
			{
				string str = "";
				if (!this.ValShippingAddress(context, ref str))
				{
					context.Response.Write("{\"success\":false,\"msg\":\"" + str + "\"}");
				}
				else
				{
					ShippingAddressInfo shippingAddress = MemberProcessor.GetShippingAddress(context.Request.Form["shippingid"].ToInt(0));
					shippingAddress.Address = Globals.StripAllTags(context.Request.Form["address"].ToNullString());
					shippingAddress.CellPhone = Globals.StripAllTags(context.Request.Form["cellphone"].ToNullString());
					shippingAddress.ShipTo = Globals.StripAllTags(context.Request.Form["shipTo"].ToNullString());
					shippingAddress.Zipcode = Globals.StripAllTags(context.Request.Form["zipCode"].ToNullString());
					shippingAddress.IsDefault = context.Request.Form["IsDefault"].ToBool();
					shippingAddress.UserId = user.UserId;
					shippingAddress.BuildingNumber = Globals.StripAllTags(context.Request["BuildingNumber"].ToNullString());
					string text = context.Request.Form["latitude"].ToNullString();
					string str2 = context.Request.Form["longitude"].ToNullString();
					string regionLocation = context.Request.Form["regionLocation"].ToNullString();
					if (text.ToDouble(0) > 0.0)
					{
						shippingAddress.LatLng = text + "," + str2;
						shippingAddress.RegionLocation = regionLocation;
					}
					shippingAddress.ShippingId = context.Request.Form["shippingid"].ToInt(0);
					shippingAddress.RegionId = context.Request.Form["regionSelectorValue"].ToInt(0);
					if (shippingAddress.RegionId <= 0 || string.IsNullOrEmpty(RegionHelper.GetFullRegion(shippingAddress.RegionId, " ", true, 0)))
					{
						context.Response.Write("{\"success\":false}");
					}
					else
					{
						shippingAddress.FullRegionPath = RegionHelper.GetFullPath(shippingAddress.RegionId, true);
						if (shippingAddress.RegionId <= 0 || RegionHelper.GetRegion(shippingAddress.RegionId, true) == null)
						{
							context.Response.Write("{\"success\":false,\"msg\":\"错误的地区信息\"}");
						}
						else if (MemberProcessor.UpdateShippingAddress(shippingAddress))
						{
							context.Response.Write("{\"success\":true}");
						}
						else
						{
							context.Response.Write("{\"success\":false,\"msg\":\"未知错误\"}");
						}
					}
				}
			}
		}

		private bool ValShippingAddress(HttpContext context, ref string erromsg)
		{
			Regex regex = new Regex("[\\u4e00-\\u9fa5a-zA-Z ]+[\\u4e00-\\u9fa5_a-zA-Z0-9]*");
			string text = context.Request.Params["shipTo"].ToNullString().Trim();
			string text2 = context.Request.Params["address"].ToNullString().Trim();
			int num = context.Request.Params["regionSelectorValue"].ToInt(0);
			string text3 = Globals.StripAllTags(context.Request.Form["zipCode"].ToNullString());
			if (string.IsNullOrEmpty(text) || !regex.IsMatch(text))
			{
				erromsg = "收货人名字不能为空，只能是汉字或字母开头，长度在2-20个字符之间";
				return false;
			}
			if (string.IsNullOrEmpty(text2))
			{
				erromsg = "详细地址不能为空";
				return false;
			}
			if (text2.Length < 3 || text2.Trim().Length > 60)
			{
				erromsg = "详细地址长度在3-60个字符之间";
				return false;
			}
			if (!string.IsNullOrEmpty(text3) && text3.Length > 20)
			{
				erromsg = "请输入正确的邮编号码";
				return false;
			}
			if (num <= 0)
			{
				erromsg = "请选择收货地址";
				return false;
			}
			string text4 = context.Request.Params["cellphone"].ToNullString().Trim();
			if (!string.IsNullOrEmpty(text4) && (text4.Length < 3 || text4.Length > 20))
			{
				erromsg = "手机号码长度限制在3-20个字符之间";
				return false;
			}
			if (!string.IsNullOrEmpty(text4) && !DataHelper.IsMobile(text4))
			{
				erromsg = "请输入正确的手机号码";
				return false;
			}
			return true;
		}

		private void SubmitWinnerInfo(HttpContext context)
		{
			Hidistro.Entities.Members.MemberInfo user = HiContext.Current.User;
			if (user.UserId == 0)
			{
				context.Response.Write("{\"success\":false}");
			}
			else
			{
				int activityId = Convert.ToInt32(context.Request.Form.Get("id"));
				string realName = context.Request.Form.Get("name");
				string cellPhone = context.Request.Form.Get("phone");
				string s = VshopBrowser.UpdatePrizeRecord(activityId, user.UserId, realName, cellPhone) ? "{\"success\":true}" : "{\"success\":false}";
				context.Response.ContentType = "application/json";
				context.Response.Write(s);
			}
		}

		private void ProcessAddToCartBySkus(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			int num = int.Parse(context.Request["quantity"], NumberStyles.None);
			string text = context.Request["productSkuId"];
			int num2 = context.Request["productSkuId"].ToNullString().Split('_')[0].ToInt(0);
			int storeId = context.Request["storeId"].ToInt(0);
			if (num <= 0)
			{
				num = 1;
			}
			if (!ProductHelper.ProductsIsAllOnSales(text, storeId))
			{
				context.Response.Write("{\"Status\":\"0\"}");
			}
			else
			{
				ShoppingCartProcessor.AddLineItem(text, num, false, storeId);
				ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart(null, false, false, -1);
				if (shoppingCart != null)
				{
					context.Response.Write("{\"Status\":\"OK\",\"TotalMoney\":\"" + shoppingCart.GetTotal(false).ToString(".00") + "\",\"Quantity\":\"" + shoppingCart.GetQuantity(false).ToString() + "\",\"SkuQuantity\":\"" + shoppingCart.GetQuantity_Sku(text) + "\"}");
				}
				else
				{
					context.Response.Write("{\"Status\":\"0\"}");
				}
			}
		}

		private void ProcessGetSkuByOptions(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			int productId = int.Parse(context.Request["productId"], NumberStyles.None);
			string text = context.Request["options"];
			if (string.IsNullOrEmpty(text))
			{
				context.Response.Write("{\"Status\":\"0\"}");
			}
			else
			{
				if (text.EndsWith(","))
				{
					text = text.Substring(0, text.Length - 1);
				}
				SKUItem productAndSku = ShoppingProcessor.GetProductAndSku(productId, text);
				if (productAndSku == null)
				{
					context.Response.Write("{\"Status\":\"1\"}");
				}
				else
				{
					decimal num = default(decimal);
					if (context.Request.UrlReferrer.AbsoluteUri.ToLower().Contains("groupbuy"))
					{
						if (ProductBrowser.IsActiveGroupByProductId(productId))
						{
							GroupBuyInfo groupByProdctId = ProductBrowser.GetGroupByProdctId(productId);
							productAndSku.Stock = groupByProdctId.MaxCount - PromoteHelper.GetSoldCount(groupByProdctId.GroupBuyId);
							productAndSku.SalePrice = groupByProdctId.Price;
						}
					}
					else if (context.Request.UrlReferrer.AbsoluteUri.ToLower().Contains("countdown"))
					{
						int countDownId = context.Request["countDownId"].ToInt(0);
						DataTable countDownSkus = PromoteHelper.GetCountDownSkus(countDownId, 0, false);
						DataRow[] array = countDownSkus.Select($" SkuId='{productAndSku.SkuId}'");
						if (array.Count() > 0)
						{
							int num2 = array[0]["TotalCount"].ToInt(0) - array[0]["BoughtCount"].ToInt(0);
							productAndSku.Stock = ((num2 >= 0) ? num2 : 0);
							productAndSku.SalePrice = array[0]["SalePrice"].ToDecimal(0);
							num = array[0]["OldSalePrice"].ToDecimal(0);
						}
						else
						{
							DataTable theSku = new SkuDao().GetTheSku(productAndSku.SkuId);
							if (theSku != null && theSku.Rows.Count > 0)
							{
								productAndSku.Stock = 0;
								productAndSku.SalePrice = theSku.Rows[0]["SalePrice"].ToDecimal(0);
								num = theSku.Rows[0]["MarketPrice"].ToDecimal(0);
							}
						}
					}
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append("{");
					if (num > decimal.Zero)
					{
						stringBuilder.AppendFormat("\"OldSalePrice\":\"{0}\",", num.F2ToString("f2"));
					}
					stringBuilder.Append("\"Status\":\"OK\",");
					stringBuilder.AppendFormat("\"SkuId\":\"{0}\",", productAndSku.SkuId);
					stringBuilder.AppendFormat("\"SKU\":\"{0}\",", productAndSku.SKU);
					stringBuilder.AppendFormat("\"Weight\":\"{0}\",", productAndSku.Weight);
					stringBuilder.AppendFormat("\"Stock\":\"{0}\",", productAndSku.Stock);
					stringBuilder.AppendFormat("\"SalePrice\":\"{0}\"", productAndSku.SalePrice.F2ToString("f2"));
					stringBuilder.Append("}");
					context.Response.ContentType = "application/json";
					context.Response.Write(stringBuilder.ToString());
				}
			}
		}

		private void ProcessDeleteCartProduct(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			string skuId = context.Request["skuId"];
			StringBuilder stringBuilder = new StringBuilder();
			ShoppingCartProcessor.RemoveLineItem(skuId, 0);
			stringBuilder.Append("{");
			stringBuilder.Append("\"Status\":\"OK\"");
			stringBuilder.Append("}");
			context.Response.ContentType = "application/json";
			context.Response.Write(stringBuilder.ToString());
		}

		private void ProcessChageQuantity(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			string skuId = context.Request["skuId"];
			int num = 1;
			int.TryParse(context.Request["quantity"], out num);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("{");
			int skuStock = ShoppingCartProcessor.GetSkuStock(skuId, 0);
			if (num > skuStock)
			{
				stringBuilder.AppendFormat("\"Status\":\"{0}\"", skuStock);
				ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart(null, false, false, -1);
				ShoppingCartItemInfo shoppingCartItemInfo = shoppingCart.LineItems.FirstOrDefault((ShoppingCartItemInfo a) => a.SkuId == skuId && a.StoreId == 0);
				stringBuilder.AppendFormat(",\"Quantity\":\"{0}\"", shoppingCartItemInfo.Quantity);
				goto IL_01b5;
			}
			stringBuilder.Append("\"Status\":\"OK\"");
			ShoppingCartProcessor.UpdateLineItemQuantity(skuId, (num <= 0) ? 1 : num, 0);
			ShoppingCartInfo shoppingCart2 = ShoppingCartProcessor.GetShoppingCart(null, false, false, -1);
			ShoppingCartItemInfo shoppingCartItemInfo2 = shoppingCart2.LineItems.FirstOrDefault((ShoppingCartItemInfo a) => a.SkuId == skuId && a.StoreId == 0);
			if (shoppingCartItemInfo2 != null)
			{
				PromotionInfo productQuantityDiscountPromotion = ShoppingCartProcessor.GetProductQuantityDiscountPromotion(skuId, HiContext.Current.User.GradeId);
				if (productQuantityDiscountPromotion != null && (decimal)num >= productQuantityDiscountPromotion.Condition)
				{
					shoppingCartItemInfo2.AdjustedPrice = shoppingCartItemInfo2.MemberPrice * productQuantityDiscountPromotion.DiscountValue;
				}
				else
				{
					shoppingCartItemInfo2.AdjustedPrice = shoppingCartItemInfo2.MemberPrice;
				}
				stringBuilder.AppendFormat(",\"adjustedPrice\":\"{0}\"", shoppingCartItemInfo2.AdjustedPrice.F2ToString("f2") ?? "");
				goto IL_01b5;
			}
			return;
			IL_01b5:
			stringBuilder.Append("}");
			context.Response.ContentType = "application/json";
			context.Response.Write(stringBuilder.ToString());
		}

		public void WriteJson(HttpContext context, int status = 0)
		{
			context.Response.ContentType = "application/json";
			StringBuilder stringBuilder = new StringBuilder("{");
			if (this.jsondict.Count > 0)
			{
				int num = 0;
				foreach (string key in this.jsondict.Keys)
				{
					if (num == 0)
					{
						stringBuilder.AppendFormat("\"{0}\":\"{1}\"", key, this.jsondict[key]);
					}
					else
					{
						stringBuilder.AppendFormat(",\"{0}\":\"{1}\"", key, this.jsondict[key]);
					}
					num++;
				}
			}
			else
			{
				stringBuilder.AppendFormat("\"{0}\":\"{1}\"", "status", status);
			}
			stringBuilder.Append("}");
			context.Response.Write(stringBuilder.ToString());
			context.ApplicationInstance.CompleteRequest();
		}

		private void ProcessSubmmitorder(HttpContext context)
		{
			try
			{
				context.Response.ContentType = "application/json";
				string text = "";
				bool flag = false;
				if (!string.IsNullOrEmpty(context.Request["from"]))
				{
					text = context.Request["from"].ToString().ToLower();
				}
				if (text == "undefined")
				{
					text = "";
				}
				if (text == "signbuy")
				{
					flag = true;
				}
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("{");
				int num = context.Request["shippingType"].ToInt(0);
				int num2 = context.Request["paymentType"].ToInt(0);
				int shippingId = context.Request["shippingId"].ToInt(0);
				int num3 = context.Request["storeId"].ToInt(0);
				int num4 = context.Request["chooseStoreId"].ToInt(0);
				int num5 = context.Request["storeCount"].ToInt(0);
				int fightGroupActivityId = context.Request["fightGroupActivityId"].ToInt(0);
				int num6 = context.Request["fightGroupId"].ToInt(0);
				OrderSource orderSource = (OrderSource)context.Request["orderSource"].ToInt(0);
				string text2 = context.Request["couponCode"];
				int num7 = 0;
				int num8 = context.Request["deductionPoints"].ToInt(0);
				bool flag2 = text == "countdown" && int.TryParse(context.Request["countDownId"], out num7);
				if (!flag2)
				{
					num7 = 0;
				}
				int num9 = 0;
				bool flag3 = text == "groupbuy" && int.TryParse(context.Request["groupbuyId"], out num9);
				int num10 = 0;
				bool flag4 = text == "combinationbuy" && int.TryParse(context.Request["combinaid"], out num10);
				if (!flag3)
				{
					num9 = 0;
				}
				int num11 = context.Request["buyAmount"].ToInt(0);
				if (text != "" && text != "prize" && num11 <= 0)
				{
					this.jsondict.Add("Status", "Error");
					this.jsondict.Add("ErrorMsg", "错误的购买数量");
					this.jsondict.Add("ErrorUrl", "");
					this.WriteJson(context, 0);
					goto end_IL_0001;
				}
				bool flag5 = text == "fightgroup";
				int preSaleId = 0;
				bool flag6 = text == "presale" && int.TryParse(context.Request["presaleId"], out preSaleId);
				int num12 = 0;
				bool flag7 = text == "prize" && int.TryParse(context.Request["RecordId"], out num12);
				string text3 = DataHelper.CleanSearchString(Globals.UrlDecode(context.Request["productSku"].ToNullString()));
				string remark = DataHelper.CleanSearchString(Globals.UrlDecode(context.Request["remark"].ToNullString()));
				decimal num13 = context.Request["UseBalance"].ToDecimal(0);
				string text4 = context.Request["advancePayPass"].ToNullString();
				if (num13 > decimal.Zero)
				{
					if (string.IsNullOrEmpty(text4))
					{
						this.jsondict.Add("Status", "Error");
						this.jsondict.Add("ErrorMsg", "请输入交易密码");
						this.jsondict.Add("ErrorUrl", "");
						this.WriteJson(context, 0);
						goto end_IL_0001;
					}
					SiteSettings masterSettings = SettingsManager.GetMasterSettings();
					if (!masterSettings.OpenBalancePay)
					{
						this.jsondict.Add("Status", "Error");
						this.jsondict.Add("ErrorMsg", "系统未开启预付款支付");
						this.jsondict.Add("ErrorUrl", "");
						this.WriteJson(context, 0);
						goto end_IL_0001;
					}
					Hidistro.Entities.Members.MemberInfo user = HiContext.Current.User;
					if (!user.IsOpenBalance || string.IsNullOrEmpty(user.TradePassword) || string.IsNullOrEmpty(user.TradePasswordSalt))
					{
						this.jsondict.Add("Status", "Error");
						this.jsondict.Add("ErrorMsg", "您还未设置交易密码");
						this.jsondict.Add("ErrorUrl", "");
						this.WriteJson(context, 0);
						goto end_IL_0001;
					}
					if (user.Balance - user.RequestBalance < num13)
					{
						this.jsondict.Add("Status", "Error");
						this.jsondict.Add("ErrorMsg", "预付款余额不够用于抵扣");
						this.jsondict.Add("ErrorUrl", "");
						this.WriteJson(context, 0);
						goto end_IL_0001;
					}
					if (!MemberProcessor.ValidTradePassword(text4))
					{
						this.jsondict.Add("Status", "Error");
						this.jsondict.Add("ErrorMsg", "交易密码有误，请重试");
						this.jsondict.Add("ErrorUrl", "");
						this.WriteJson(context, 0);
					}
				}
				ShoppingCartInfo shoppingCartInfo = null;
				string text5 = "";
				bool flag8 = false;
				flag8 = context.Request["needInvoice"].ToBool();
				string text6 = context.Request["invoiceTitle"].ToNullString();
				InvoiceType invoiceType = (InvoiceType)context.Request["invoiceType"].ToInt(0);
				string text7 = context.Request["invoiceTaxpayerNumber"].ToNullString();
				GroupBuyInfo groupBuyInfo = null;
				CountDownInfo countDownInfo = null;
				FightGroupActivityInfo fightGroupActivityInfo = null;
				FightGroupInfo fightGroupInfo = null;
				if (!flag6 && !string.IsNullOrEmpty(text3) && !ProductHelper.ProductsIsAllOnSales(text3, num3))
				{
					this.jsondict.Add("Status", "Error");
					this.jsondict.Add("ErrorMsg", "订单中有商品已删除或已下架，请重新选择商品");
					this.jsondict.Add("ErrorUrl", "ShoppingCart");
					this.WriteJson(context, 0);
					goto end_IL_0001;
				}
				if (flag4)
				{
					text5 = this.CheckCombinaInfo(num10, text3);
					if (!string.IsNullOrEmpty(text5))
					{
						this.jsondict.Add("Status", "Error");
						this.jsondict.Add("ErrorMsg", text5);
						this.WriteJson(context, 0);
						goto end_IL_0001;
					}
				}
				ProductPreSaleInfo productPreSaleInfo = null;
				if (flag6)
				{
					string value = "";
					productPreSaleInfo = ProductPreSaleHelper.GetProductPreSaleInfo(preSaleId);
					if (!this.CheckPresaleInfo(productPreSaleInfo, text3, out value))
					{
						this.jsondict.Add("Status", "Error");
						this.jsondict.Add("ErrorMsg", value);
						this.WriteJson(context, 0);
						goto end_IL_0001;
					}
				}
				if (!flag7)
				{
					shoppingCartInfo = ShoppingCartProcessor.GetShoppingCart(text, text3, num11, num10, true, num3, fightGroupActivityId);
				}
				else
				{
					UserAwardRecordsInfo userAwardRecordsInfo = ActivityHelper.GetUserAwardRecordsInfo(num12);
					if (userAwardRecordsInfo == null || userAwardRecordsInfo.UserId != HiContext.Current.UserId || userAwardRecordsInfo.PrizeType != 3 || userAwardRecordsInfo.Status == 2)
					{
						text5 = "奖项不存在不存在";
						this.jsondict.Add("Status", "Error");
						this.jsondict.Add("ErrorMsg", text5);
						this.WriteJson(context, 0);
						goto end_IL_0001;
					}
					string orderIdByUserAwardRecordsId = OrderHelper.GetOrderIdByUserAwardRecordsId(num12);
					if (!string.IsNullOrWhiteSpace(orderIdByUserAwardRecordsId))
					{
						text5 = "该奖品存在未付款的订单";
						this.jsondict.Add("Status", "Error");
						this.jsondict.Add("ErrorMsg", text5);
						this.WriteJson(context, 0);
						goto end_IL_0001;
					}
					int prizeValue = userAwardRecordsInfo.PrizeValue;
					shoppingCartInfo = ShoppingCartProcessor.GetPrizeShoppingCart(prizeValue);
				}
				if (shoppingCartInfo != null && ((shoppingCartInfo.LineItems != null && (from a in shoppingCartInfo.LineItems
				where a.Quantity <= 0
				select a).Count() > 0) || (shoppingCartInfo.LineGifts != null && (from a in shoppingCartInfo.LineGifts
				where a.Quantity <= 0
				select a).Count() > 0)))
				{
					this.jsondict.Add("Status", "Error");
					this.jsondict.Add("ErrorMsg", "购买数量不合法");
					this.WriteJson(context, 0);
					goto end_IL_0001;
				}
				if (num11 <= 0)
				{
					num11 = 1;
				}
				if (flag3)
				{
					groupBuyInfo = TradeHelper.GetProductGroupBuyInfo(shoppingCartInfo.LineItems[0].ProductId, num11, out text5);
				}
				if (flag5 && string.IsNullOrEmpty(text5))
				{
					fightGroupActivityInfo = VShopHelper.GetFightGroupActivitieInfo(fightGroupActivityId);
					if (fightGroupActivityInfo == null)
					{
						text5 = "拼团活动不存在";
					}
					fightGroupInfo = VShopHelper.GetFightGroup(num6);
					if (fightGroupInfo == null && num6 != 0)
					{
						text5 = "拼团活动不存在";
					}
					fightGroupActivityInfo = VShopHelper.CheckUserFightGroup(shoppingCartInfo.LineItems[0].ProductId, fightGroupActivityInfo.FightGroupActivityId, num6, shoppingCartInfo.LineItems[0].SkuId, HiContext.Current.UserId, num11, "", shoppingCartInfo.LineItems[0].Quantity, out text5);
					if (!string.IsNullOrEmpty(text5))
					{
						this.jsondict.Add("Status", "Error");
						this.jsondict.Add("ErrorMsg", text5);
						this.WriteJson(context, 0);
						goto end_IL_0001;
					}
				}
				SiteSettings masterSettings2 = SettingsManager.GetMasterSettings();
				if (flag2 && text5 == "")
				{
					countDownInfo = TradeHelper.ProductExistsCountDown(shoppingCartInfo.LineItems[0].ProductId, "", num3);
					if (countDownInfo == null)
					{
						text5 = "该商品未进行抢购活动,或者活动已结束";
					}
					if (masterSettings2.OpenMultStore && !StoreActivityHelper.JoinActivity(countDownInfo.CountDownId, 2, num3, countDownInfo.StoreType))
					{
						text5 = "该门店未参与此抢购活动";
					}
					if (text5 == "")
					{
						countDownInfo = TradeHelper.CheckUserCountDown(shoppingCartInfo.LineItems[0].ProductId, countDownInfo.CountDownId, shoppingCartInfo.LineItems[0].SkuId, HiContext.Current.UserId, num11, "", out text5, num3);
					}
				}
				if (text5 == "" && (shoppingCartInfo == null || (shoppingCartInfo != null && (shoppingCartInfo.LineItems == null || shoppingCartInfo.LineItems.Count == 0) && (shoppingCartInfo.LineGifts == null || shoppingCartInfo.LineGifts.Count == 0))))
				{
					text5 = "订单中有商品已删除或已下架，请重新选择商品";
					this.jsondict.Add("ErrorUrl", "ShoppingCart");
				}
				string str = "";
				if (text5 == "" && !TradeHelper.CheckShoppingStock(shoppingCartInfo, out str, num3))
				{
					text5 = "订单中有商品(" + str + ")库存不足";
				}
				if (text5 == "" && HiContext.Current.UserId != 0)
				{
					int totalNeedPoint = shoppingCartInfo.GetTotalNeedPoint();
					int points = HiContext.Current.User.Points;
					if (points >= 0 && totalNeedPoint > points)
					{
						text5 = "您当前的积分不够兑换所需礼品！";
					}
				}
				StoresInfo storesInfo = null;
				if (num3 > 0 || num4 > 0)
				{
					storesInfo = ((num4 <= 0) ? DepotHelper.GetStoreById(num3) : DepotHelper.GetStoreById(num4));
				}
				if (storesInfo != null && string.IsNullOrEmpty(text5))
				{
					if (num == -2 && !storesInfo.IsOnlinePay && num2 == 0)
					{
						text5 = "门店不支持在线支付,请重新选择支付方式";
					}
					if (num == -2 && !storesInfo.IsOfflinePay && num2 == -3)
					{
						text5 = "门店不支持到店支付,请重新选择支付方式";
					}
				}
				if (storesInfo != null && num == -1)
				{
					if (!storesInfo.IsStoreDelive)
					{
						text5 = "门店不支持门店配送";
					}
					decimal amount = shoppingCartInfo.GetAmount(true);
					decimal? minOrderPrice = storesInfo.MinOrderPrice;
					if (amount < minOrderPrice.GetValueOrDefault() && minOrderPrice.HasValue)
					{
						text5 = "商品金额未达到门店起送价";
					}
				}
				if (!string.IsNullOrEmpty(text5))
				{
					this.jsondict.Add("Status", "Error");
					this.jsondict.Add("ErrorMsg", text5);
					this.WriteJson(context, 0);
					goto end_IL_0001;
				}
				OrderInfo order = ShoppingProcessor.ConvertShoppingCartToOrder(shoppingCartInfo, flag3, flag2, num3);
				ShippingAddressInfo shippingAddress;
				if (order != null && (order.GetTotal(false) >= decimal.Zero || (shoppingCartInfo.LineItems.Count == 0 && shoppingCartInfo.LineGifts.Count > 0)))
				{
					order.OrderId = OrderIDFactory.GenerateOrderId();
					order.ParentOrderId = "0";
					order.OrderDate = DateTime.Now;
					order.OrderSource = orderSource;
					Hidistro.Entities.Members.MemberInfo user2 = HiContext.Current.User;
					order.UserId = user2.UserId;
					order.Username = user2.UserName;
					order.EmailAddress = user2.Email;
					order.RealName = (string.IsNullOrEmpty(user2.RealName) ? user2.NickName : user2.RealName);
					order.QQ = user2.QQ;
					order.Remark = remark;
					if (flag3)
					{
						order.GroupBuyId = groupBuyInfo.GroupBuyId;
						order.NeedPrice = groupBuyInfo.NeedPrice;
						order.GroupBuyStatus = groupBuyInfo.Status;
					}
					if (flag2)
					{
						order.CountDownBuyId = countDownInfo.CountDownId;
					}
					if (flag5)
					{
						order.FightGroupId = (fightGroupInfo?.FightGroupId ?? 0);
						order.FightGroupActivityId = fightGroupActivityInfo.FightGroupActivityId;
					}
					if (flag7)
					{
						order.UserAwardRecordsId = num12;
					}
					order.OrderStatus = OrderStatus.WaitBuyerPay;
					order.ShipToDate = context.Request["shiptoDate"];
					shippingAddress = MemberProcessor.GetShippingAddress(shippingId);
					if (shippingAddress != null)
					{
						order.ShippingRegion = RegionHelper.GetFullRegion(shippingAddress.RegionId, "，", true, 0);
						order.RegionId = shippingAddress.RegionId;
						order.FullRegionPath = RegionHelper.GetFullPath(order.RegionId, true);
						order.Address = shippingAddress.RegionLocation + shippingAddress.Address + shippingAddress.BuildingNumber;
						order.ZipCode = "";
						order.ShipTo = shippingAddress.ShipTo;
						order.TelPhone = shippingAddress.TelPhone;
						order.CellPhone = shippingAddress.CellPhone;
						order.ShippingId = shippingAddress.ShippingId;
						order.LatLng = shippingAddress.LatLng;
						MemberProcessor.SetDefaultShippingAddress(shippingId, HiContext.Current.UserId);
						if (HiContext.Current.SiteSettings.IsOpenCertification && order.IsincludeCrossBorderGoods)
						{
							if (string.IsNullOrWhiteSpace(shippingAddress.IDNumber))
							{
								this.jsondict.Add("Status", "Error");
								this.jsondict.Add("ErrorMsg", "请在该收获地址填写实名验证身份证号");
								this.WriteJson(context, 0);
								goto end_IL_0001;
							}
							order.IDNumber = shippingAddress.IDNumber;
							if (HiContext.Current.SiteSettings.CertificationModel == 2)
							{
								if (string.IsNullOrWhiteSpace(shippingAddress.IDImage1) || string.IsNullOrWhiteSpace(shippingAddress.IDImage2))
								{
									this.jsondict.Add("Status", "Error");
									this.jsondict.Add("ErrorMsg", "请在该收获地址上传实名验证证件照");
									this.WriteJson(context, 0);
									goto end_IL_0001;
								}
								order.IDImage1 = shippingAddress.IDImage1;
								order.IDImage2 = shippingAddress.IDImage2;
							}
							order.IDStatus = 1;
						}
					}
					if (shippingAddress == null || order.RegionId == 0 || string.IsNullOrEmpty(order.ShippingRegion))
					{
						this.jsondict.Add("Status", "Error");
						this.jsondict.Add("ErrorMsg", "错误的收货地址,请重新选择或者修改收货地址。");
						this.WriteJson(context, 0);
						goto end_IL_0001;
					}
					if (num == 0 || num == -1 || num == -2)
					{
						order.ShippingModeId = num;
						order.StoreId = num3;
						if (num3 == 0 && num4 > 0 && num != 0)
						{
							order.StoreId = num4;
						}
						if (num == -2 && (num4 > 0 || num3 > 0 || (num3 == 0 && masterSettings2.IsOpenPickeupInStore)) && order.LineItems.Count > 0 && !flag3)
						{
							switch (flag5)
							{
							case false:
								goto IL_134e;
							}
							if (masterSettings2.FitGroupIsOpenPickeupInStore)
							{
								goto IL_134e;
							}
						}
						goto IL_1359;
					}
					goto IL_152c;
				}
				if (order.GetTotal(true) < decimal.Zero)
				{
					stringBuilder.Append("\"Status\":\"Error\"");
					stringBuilder.AppendFormat(",\"ErrorMsg\":\"订单金额不能为负\"");
				}
				else
				{
					stringBuilder.Append("\"Status\":\"None\"");
				}
				goto IL_2fa7;
				IL_152c:
				string text8 = "";
				CouponItemInfo userCouponInfo;
				int num14;
				if (!string.IsNullOrEmpty(text2))
				{
					userCouponInfo = ShoppingProcessor.GetUserCouponInfo(shoppingCartInfo.GetTotal(num3 > 0), text2);
					if (userCouponInfo != null && string.IsNullOrEmpty(userCouponInfo.OrderId) && !userCouponInfo.UsedTime.HasValue && HiContext.Current.UserId != 0 && userCouponInfo.UserId == HiContext.Current.UserId)
					{
						if (order.CountDownBuyId == 0 && order.GroupBuyId == 0)
						{
							goto IL_1620;
						}
						if (order.GroupBuyId > 0 && userCouponInfo.UseWithGroup.Value)
						{
							goto IL_1620;
						}
						num14 = ((order.CountDownBuyId > 0 && userCouponInfo.UseWithPanicBuying.Value) ? 1 : 0);
						goto IL_1621;
					}
				}
				goto IL_16e7;
				IL_134e:
				int num15;
				if (!flag6)
				{
					num15 = ((!flag7) ? 1 : 0);
					goto IL_135a;
				}
				goto IL_1359;
				IL_1359:
				num15 = 0;
				goto IL_135a;
				IL_2fa7:
				stringBuilder.Append("}");
				context.Response.ContentType = "application/json";
				context.Response.Write(stringBuilder.ToString());
				goto end_IL_0001;
				IL_1620:
				num14 = 1;
				goto IL_1621;
				IL_135a:
				if (num15 != 0)
				{
					order.ModeName = "上门自提";
					order.TakeCode = "";
					OrderInfo orderInfo = order;
					OrderInfo orderInfo2 = order;
					decimal num18 = orderInfo.AdjustedFreight = (orderInfo2.Freight = default(decimal));
				}
				else if (num == -1 && (num4 > 0 || num3 > 0) && order.LineItems.Count > 0 && !flag3 && !flag5 && !flag6 && !flag7)
				{
					order.ModeName = "门店配送";
					if (storesInfo != null && !shoppingCartInfo.IsFreightFree)
					{
						OrderInfo orderInfo3 = order;
						OrderInfo orderInfo4 = order;
						decimal? minOrderPrice = storesInfo.StoreFreight;
						bool num19 = minOrderPrice.GetValueOrDefault() > default(decimal) && minOrderPrice.HasValue;
						decimal num18 = orderInfo3.AdjustedFreight = (orderInfo4.Freight = (num19 ? storesInfo.StoreFreight.Value : decimal.Zero));
					}
					else
					{
						OrderInfo orderInfo5 = order;
						OrderInfo orderInfo6 = order;
						decimal num18 = orderInfo5.AdjustedFreight = (orderInfo6.Freight = default(decimal));
					}
				}
				else
				{
					order.ShippingModeId = 0;
					order.ModeName = "快递配送";
					if (!shoppingCartInfo.IsFreightFree)
					{
						OrderInfo orderInfo7 = order;
						OrderInfo orderInfo8 = order;
						decimal num18 = orderInfo7.AdjustedFreight = (orderInfo8.Freight = ShoppingProcessor.CalcFreight(order.RegionId, shoppingCartInfo));
					}
					else
					{
						OrderInfo orderInfo9 = order;
						OrderInfo orderInfo10 = order;
						decimal num18 = orderInfo9.AdjustedFreight = (orderInfo10.Freight = default(decimal));
					}
				}
				goto IL_152c;
				IL_16e7:
				if (num8 > 0 && (masterSettings2.CanPointUseWithCoupon || (!masterSettings2.CanPointUseWithCoupon && string.IsNullOrEmpty(order.CouponCode))) && masterSettings2.ShoppingDeduction > 0)
				{
					int shoppingDeductionRatio = masterSettings2.ShoppingDeductionRatio;
					decimal value2 = (decimal)shoppingDeductionRatio * (order.GetTotal(false) - order.AdjustedFreight - order.Tax) * (decimal)masterSettings2.ShoppingDeduction / 100m;
					Hidistro.Entities.Members.MemberInfo user3 = Users.GetUser(HiContext.Current.UserId);
					if (user3 != null)
					{
						int num28 = (user3.Points > (int)value2) ? ((int)value2) : user3.Points;
						if (num8 > num28)
						{
							num8 = num28;
						}
						decimal value3 = ((decimal)num8 / (decimal)masterSettings2.ShoppingDeduction).F2ToString("f2").ToDecimal(0);
						order.DeductionPoints = num8;
						order.DeductionMoney = value3;
					}
				}
				if (flag8)
				{
					int num29 = context.Request["invoiceId"].ToInt(0);
					UserInvoiceDataInfo userInvoiceDataInfo = MemberProcessor.GetUserInvoiceDataInfo(num29);
					bool flag9 = num29 == 0 && true;
					invoiceType = (InvoiceType)context.Request["InvoiceType"].ToInt(0);
					if (userInvoiceDataInfo == null)
					{
						userInvoiceDataInfo = new UserInvoiceDataInfo
						{
							Id = 0,
							InvoiceType = InvoiceType.Personal,
							InvoiceTitle = "个人",
							LastUseTime = DateTime.Now
						};
						flag9 = true;
						invoiceType = InvoiceType.Personal;
					}
					else
					{
						invoiceType = userInvoiceDataInfo.InvoiceType;
					}
					if (invoiceType == InvoiceType.VATInvoice && masterSettings2.VATTaxRate > decimal.Zero && masterSettings2.EnableVATInvoice)
					{
						order.Tax = ((order.GetTotal(false) - order.AdjustedFreight) * masterSettings2.VATTaxRate / 100m).F2ToString("f2").ToDecimal(0);
					}
					else if (masterSettings2.TaxRate > decimal.Zero && (masterSettings2.EnableTax || masterSettings2.EnableE_Invoice))
					{
						order.Tax = ((order.GetTotal(false) - order.AdjustedFreight) * masterSettings2.TaxRate / 100m).F2ToString("f2").ToDecimal(0);
					}
					else
					{
						order.Tax = decimal.Zero;
					}
					order.InvoiceTitle = userInvoiceDataInfo.InvoiceTitle;
					order.InvoiceType = userInvoiceDataInfo.InvoiceType;
					if (order.InvoiceType == InvoiceType.Enterprise || order.InvoiceType == InvoiceType.Enterprise_Electronic || order.InvoiceType == InvoiceType.VATInvoice)
					{
						order.InvoiceTaxpayerNumber = userInvoiceDataInfo.InvoiceTaxpayerNumber;
					}
					order.InvoiceData = JsonHelper.GetJson(userInvoiceDataInfo);
				}
				if (!string.IsNullOrEmpty(text5))
				{
					this.jsondict.Add("Status", "Error");
					this.jsondict.Add("ErrorMsg", text5);
					this.WriteJson(context, 0);
					goto end_IL_0001;
				}
				if (order.StoreId <= 0 && masterSettings2.OpenMultStore && !flag3 && !flag5 && !flag2 && !flag6 && masterSettings2.AutoAllotOrder && order.LineItems.Count > 0)
				{
					int num30 = 0;
					if (shippingAddress != null && !string.IsNullOrWhiteSpace(shippingAddress.LatLng))
					{
						string[] array = shippingAddress.LatLng.Split(',');
						IList<StoreLocationInfo> storeLocationInfoByOpenId = DepotHelper.GetStoreLocationInfoByOpenId("pc-" + shippingAddress.UserId + "-" + shippingAddress.ShippingId, array[1], array[0]);
						if (storeLocationInfoByOpenId != null && storeLocationInfoByOpenId.Count() > 0)
						{
							List<StoreLocationInfo> source = (from d in storeLocationInfoByOpenId
							orderby d.Distances
							select d).ToList();
							int num31 = 0;
							for (int j = 0; j < source.Count(); j++)
							{
								StoreLocationInfo storeLocationInfo = storeLocationInfoByOpenId[j];
								num30 = storeLocationInfo.StoreId;
								StoresInfo storeById = DepotHelper.GetStoreById(num30);
								if (storeById == null)
								{
									num30 = 0;
								}
								else if (!storeById.IsStoreDelive && !storeById.IsSupportExpress)
								{
									num30 = 0;
								}
								else
								{
									if (storeById.IsSupportExpress && num31 == 0)
									{
										num31 = num30;
									}
									if (!storeById.IsSupportExpress && storeById.ServeRadius.HasValue && (storeById.ServeRadius.Value * 1000.0 < storeLocationInfo.Distances || !DepotHelper.IsStoreInDeliveArea(num30, shippingAddress.FullRegionPath)))
									{
										num30 = 0;
									}
									else
									{
										foreach (ShoppingCartItemInfo lineItem in shoppingCartInfo.LineItems)
										{
											if (!StoresHelper.StoreHasProductSku(storeLocationInfo.StoreId, lineItem.SkuId) || !StoresHelper.StoreHasStock(storeLocationInfo.StoreId, lineItem.SkuId, lineItem.Quantity))
											{
												if (num31 == storeLocationInfo.StoreId)
												{
													num31 = 0;
												}
												num30 = 0;
												break;
											}
										}
										if (num30 > 0)
										{
											break;
										}
									}
								}
							}
							if (num30 == 0)
							{
								num30 = num31;
							}
						}
					}
					if (num30 == 0)
					{
						num30 = StoresHelper.GetStoreAutoAllotOrder(shoppingCartInfo, order.RegionId);
					}
					if (num30 > 0)
					{
						order.StoreId = num30;
						order.ModeName = "门店配送";
						order.ShippingModeId = -1;
					}
					else
					{
						order.StoreId = num30;
					}
				}
				if (HiContext.Current.UserId != 0)
				{
					order.Points = order.GetPoint(masterSettings2.PointsRate);
				}
				else
				{
					order.Points = 0;
				}
				order.ExchangePoints = order.GetTotalNeedPoint();
				if (num13 > decimal.Zero && num13 > order.GetTotal(false))
				{
					num13 = order.GetTotal(false);
				}
				order.BalanceAmount = num13;
				if (order.GetTotal(true) <= decimal.Zero && order.BalanceAmount > decimal.Zero)
				{
					order.PaymentTypeId = -99;
					order.PaymentType = "余额支付";
					order.Gateway = "hishop.plugins.payment.advancerequest";
				}
				else if (num2 == 0 || order.GetTotal(false) <= decimal.Zero)
				{
					order.PaymentTypeId = 0;
					order.PaymentType = "在线支付";
					order.Gateway = "";
				}
				else
				{
					switch (num2)
					{
					case -3:
						if (flag6)
						{
							text5 = "预售不支持到店支付";
						}
						order.PaymentTypeId = -3;
						order.PaymentType = "到店支付";
						order.Gateway = "hishop.plugins.payment.payonstore";
						break;
					case 2:
					{
						PaymentModeInfo paymentMode2 = ShoppingProcessor.GetPaymentMode(ShoppingProcessor.GetPaymentGateway(EnumPaymentType.OfflinePay));
						if (paymentMode2 != null)
						{
							order.PaymentTypeId = paymentMode2.ModeId;
							order.PaymentType = paymentMode2.Name;
							order.Gateway = paymentMode2.Gateway;
						}
						break;
					}
					default:
					{
						if (flag6)
						{
							text5 = "预售不支持货到付款";
						}
						PaymentModeInfo paymentMode = ShoppingProcessor.GetPaymentMode(ShoppingProcessor.GetPaymentGateway(EnumPaymentType.CashOnDelivery));
						if (paymentMode != null)
						{
							order.PaymentTypeId = paymentMode.ModeId;
							order.PaymentType = paymentMode.Name;
							order.Gateway = paymentMode.Gateway;
						}
						else
						{
							order.PaymentTypeId = 0;
							order.PaymentType = "货到付款";
							order.Gateway = "hishop.plugins.payment.podrequest";
						}
						break;
					}
					}
				}
				try
				{
					if (flag6)
					{
						order.PreSaleId = preSaleId;
						order.Deposit = ((productPreSaleInfo.Deposit == decimal.Zero) ? ((decimal)productPreSaleInfo.DepositPercent * shoppingCartInfo.LineItems[0].MemberPrice / 100m) : productPreSaleInfo.Deposit) * (decimal)shoppingCartInfo.LineItems[0].Quantity;
						order.FinalPayment = order.GetFinalPayment();
						if (order.BalanceAmount >= order.Deposit)
						{
							order.BalanceAmount = order.Deposit;
							order.DepositGatewayOrderId = "";
						}
					}
					string orderId = order.OrderId;
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
					if (masterSettings2.OpenSupplier)
					{
						if (list.Count() > 1)
						{
							string orderId2 = order.OrderId;
							order.ParentOrderId = "-1";
							order.OrderId = "P" + orderId2;
							order.StoreId = 0;
							decimal num32 = order.ReducedPromotionAmount;
							decimal num33 = order.CouponValue;
							int num34 = order.DeductionPoints.HasValue ? order.DeductionPoints.Value : 0;
							decimal num35 = (!order.DeductionMoney.HasValue) ? decimal.Zero : order.DeductionMoney.Value;
							int num36 = order.Points;
							string arg = orderId2;
							int num37 = 0;
							decimal num38 = order.BalanceAmount;
							for (int k = 0; k < list.Count; k++)
							{
								if (k >= 100 && k % 100 == 0)
								{
									arg = OrderIDFactory.GenerateOrderId();
									num37 = 0;
								}
								var anon = list[k];
								OrderInfo orderInfo11 = new OrderInfo();
								ShoppingProcessor.CreateDetailOrderInfo(order, orderInfo11);
								orderInfo11.OrderId = arg + num37;
								orderInfo11.ParentOrderId = order.OrderId;
								orderInfo11.SupplierId = anon.SupplierId;
								orderInfo11.ShipperName = anon.SupplierName;
								ShoppingProcessor.BindDetailOrderItemsAndGifts(orderInfo11, shoppingCartInfo, anon.SupplierId);
								if (anon.SupplierId > 0)
								{
									orderInfo11.Tax = decimal.Zero;
									orderInfo11.InvoiceTitle = "";
									orderInfo11.InvoiceTaxpayerNumber = "";
								}
								if (orderInfo11.LineItems.Count <= 0)
								{
									OrderInfo orderInfo12 = orderInfo11;
									OrderInfo orderInfo13 = orderInfo11;
									OrderInfo orderInfo14 = orderInfo11;
									OrderInfo orderInfo15 = orderInfo11;
									OrderInfo orderInfo16 = orderInfo11;
									int num40 = orderInfo16.PreSaleId = 0;
									int num42 = orderInfo15.GroupBuyId = num40;
									int num44 = orderInfo14.FightGroupId = num42;
									int num47 = orderInfo12.CountDownBuyId = (orderInfo13.FightGroupActivityId = num44);
									orderInfo11.Deposit = decimal.Zero;
									orderInfo11.FinalPayment = decimal.Zero;
								}
								decimal d2 = (order.GetAmount(false) == decimal.Zero) ? decimal.Zero : (orderInfo11.GetAmount(false) / order.GetAmount(false));
								if (order.IsReduced)
								{
									if (k == list.Count - 1)
									{
										orderInfo11.ReducedPromotionAmount = ((num32 < decimal.Zero) ? decimal.Zero : num32);
									}
									else
									{
										decimal d3 = orderInfo11.ReducedPromotionAmount = (d2 * order.ReducedPromotionAmount).F2ToString("f2").ToDecimal(0);
										num32 -= d3;
									}
								}
								if (order.CouponValue > decimal.Zero)
								{
									if (k == list.Count - 1)
									{
										orderInfo11.CouponValue = ((num33 < decimal.Zero) ? decimal.Zero : num33);
									}
									else
									{
										decimal num49 = default(decimal);
										num49 = (orderInfo11.CouponValue = (d2 * order.CouponValue).F2ToString("f2").ToDecimal(0));
										num33 -= num49;
									}
								}
								orderInfo11.Freight = ShoppingProcessor.CalcSupplierFreight(anon.SupplierId, order.RegionId, shoppingCartInfo);
								if (!order.IsFreightFree)
								{
									orderInfo11.AdjustedFreight = orderInfo11.Freight;
								}
								decimal? minOrderPrice = order.DeductionMoney;
								if (minOrderPrice.GetValueOrDefault() > default(decimal) && minOrderPrice.HasValue)
								{
									if (k == list.Count - 1)
									{
										orderInfo11.DeductionPoints = ((num34 >= 0) ? num34 : 0);
										orderInfo11.DeductionMoney = ((num35 < decimal.Zero) ? decimal.Zero : num35);
									}
									else
									{
										decimal d4 = orderInfo11.GetAmount(false) - orderInfo11.ReducedPromotionAmount - orderInfo11.CouponValue;
										decimal d5 = order.GetAmount(false) - order.ReducedPromotionAmount - order.CouponValue;
										decimal num51 = (d4 == decimal.Zero) ? decimal.Zero : (d4 / d5);
										decimal num18 = num51;
										decimal? d6 = (decimal?)order.DeductionPoints;
										int num52 = (int)((decimal?)num18 * d6).Value;
										if (HiContext.Current.User.Points < num52)
										{
											num52 = HiContext.Current.User.Points;
										}
										decimal num53 = (num52 / masterSettings2.ShoppingDeduction).F2ToString("f2").ToDecimal(0);
										if (order.GetTotal(true) == decimal.Zero)
										{
											decimal total = orderInfo11.GetTotal(true);
											if (total > decimal.Zero)
											{
												num53 -= total;
												num52 = (int)(num53 * (decimal)masterSettings2.ShoppingDeduction);
											}
										}
										if (num53 < decimal.Zero)
										{
											num53 = default(decimal);
										}
										if (num52 < 0)
										{
											num52 = 0;
										}
										orderInfo11.DeductionPoints = num52;
										orderInfo11.DeductionMoney = num53;
										num34 -= num52;
										num35 -= num53;
									}
								}
								if (order.Points > 0)
								{
									if (k == list.Count - 1)
									{
										orderInfo11.Points = num36;
									}
									else
									{
										int num54 = orderInfo11.Points = orderInfo11.GetPoint(HiContext.Current.SiteSettings.PointsRate);
										num36 -= num54;
									}
								}
								if (order.BalanceAmount > decimal.Zero)
								{
									if (k == list.Count - 1)
									{
										orderInfo11.BalanceAmount = num38;
									}
									else
									{
										decimal d7 = orderInfo11.BalanceAmount = (d2 * order.BalanceAmount).F2ToString("f2").ToDecimal(0);
										num38 -= d7;
									}
								}
								orderInfo11.ExchangePoints = orderInfo11.GetTotalNeedPoint();
								if (anon.SupplierId == 0)
								{
									orderId = orderInfo11.OrderId;
								}
								num37++;
								list2.Add(orderInfo11);
							}
						}
						else
						{
							var anon2 = list[0];
							order.SupplierId = anon2.SupplierId;
							order.ShipperName = anon2.SupplierName;
						}
					}
					list2.Add(order);
					if (ShoppingProcessor.CreatOrder(list2))
					{
						if (order.BalanceAmount > decimal.Zero)
						{
							TradeHelper.BalanceDeduct(order);
						}
						TransactionAnalysisHelper.AnalysisOrderTranData(order);
						if (order.StoreId > 0 && order.ShippingModeId == -2 && order.PaymentType == "到店支付")
						{
							VShopHelper.AppPsuhRecordForStore(order.StoreId, order.OrderId, "", EnumPushStoreAction.TakeOnStoreOrderWaitConfirm);
						}
						if (order.GetTotal(true) == decimal.Zero || (order.PreSaleId > 0 && order.BalanceAmount == order.Deposit))
						{
							int maxCount = 0;
							int yetOrderNum = 0;
							int currentOrderNum = 0;
							if (order.GroupBuyId > 0)
							{
								GroupBuyInfo groupBuy = TradeHelper.GetGroupBuy(order.GroupBuyId);
								if (groupBuy != null && groupBuy.Status == GroupBuyStatus.UnderWay)
								{
									yetOrderNum = TradeHelper.GetOrderCount(order.GroupBuyId);
									currentOrderNum = order.GetGroupBuyOerderNumber();
									maxCount = groupBuy.MaxCount;
								}
							}
							Task.Factory.StartNew(delegate
							{
								if (order.CheckAction(OrderActions.BUYER_PAY) && TradeHelper.UpdateOrderStatus(order))
								{
									TradeHelper.UserPayOrder(order, false, true);
									try
									{
										if (order.FightGroupId > 0)
										{
											VShopHelper.SetFightGroupSuccess(order.FightGroupId);
										}
										if (order.GroupBuyId > 0 && maxCount == yetOrderNum + currentOrderNum)
										{
											TradeHelper.SetGroupBuyEndUntreated(order.GroupBuyId);
										}
										if (order.ParentOrderId == "-1")
										{
											OrderQuery orderQuery = new OrderQuery();
											orderQuery.ParentOrderId = order.OrderId;
											IList<OrderInfo> listUserOrder = MemberProcessor.GetListUserOrder(order.UserId, orderQuery);
											foreach (OrderInfo item in listUserOrder)
											{
												OrderHelper.OrderConfirmPaySendMessage(item);
											}
										}
										else
										{
											OrderHelper.OrderConfirmPaySendMessage(order);
										}
									}
									catch (Exception ex3)
									{
										IDictionary<string, string> dictionary = new Dictionary<string, string>();
										dictionary.Add("ErrorMessage", ex3.Message);
										dictionary.Add("StackTrace", ex3.StackTrace);
										if (ex3.InnerException != null)
										{
											dictionary.Add("InnerException", ex3.InnerException.ToString());
										}
										if (ex3.GetBaseException() != null)
										{
											dictionary.Add("BaseException", ex3.GetBaseException().Message);
										}
										if (ex3.TargetSite != (MethodBase)null)
										{
											dictionary.Add("TargetSite", ex3.TargetSite.ToString());
										}
										dictionary.Add("ExSource", ex3.Source);
										Globals.AppendLog(dictionary, "支付更新订单收款记录或者消息通知时出错：" + ex3.Message, "", "", "UserPay");
									}
									order.OnPayment();
								}
							});
						}
						if (shoppingCartInfo.GetTotalNeedPoint() > 0)
						{
							ShoppingProcessor.CutNeedPoint(shoppingCartInfo.GetTotalNeedPoint(), orderId, PointTradeType.Change, HiContext.Current.User.UserId);
						}
						Messenger.OrderCreated(order, HiContext.Current.User);
						if (order.Gateway == "hishop.plugins.payment.podrequest")
						{
							if (order.StoreId > 0)
							{
								storesInfo = DepotHelper.GetStoreById(order.StoreId);
							}
							ShippersInfo defaultOrFirstShipper = SalesHelper.GetDefaultOrFirstShipper(0);
							Messenger.OrderPaymentToShipper(defaultOrFirstShipper, storesInfo, null, order, order.GetTotal(false));
						}
						if (text != "signbuy" && text != "groupbuy" && text != "combinationbuy" && text != "presale" && text != "prize")
						{
							foreach (ShoppingCartItemInfo lineItem2 in shoppingCartInfo.LineItems)
							{
								ShoppingCartProcessor.RemoveLineItem(lineItem2.SkuId, num3);
							}
							foreach (ShoppingCartGiftInfo lineGift in shoppingCartInfo.LineGifts)
							{
								ShoppingCartProcessor.RemoveGiftItem(lineGift.GiftId, (PromoteType)lineGift.PromoType);
							}
						}
						stringBuilder.Append("\"Status\":\"OK\",");
						if (order.GetTotal(true) == decimal.Zero || (flag6 && order.BalanceAmount == order.Deposit))
						{
							stringBuilder.Append("\"paymentType\":\"HASPAY\",");
						}
						else if (num2 == -6)
						{
							stringBuilder.Append("\"paymentType\":\"NO\",");
						}
						else
						{
							stringBuilder.Append("\"paymentType\":\"OK\",");
						}
						stringBuilder.Append("\"FightGroupId\":\"" + order.FightGroupId + "\",");
						stringBuilder.Append("\"ParentOrderId\":\"" + order.ParentOrderId + "\",");
						stringBuilder.AppendFormat("\"OrderId\":\"{0}\"", order.OrderId);
					}
					else
					{
						stringBuilder.Append("\"Status\":\"Error\"");
					}
				}
				catch (Exception ex)
				{
					stringBuilder.Append("\"Status\":\"Error\"");
					stringBuilder.AppendFormat(",\"ErrorMsg\":\"{0}\"", ex.Message);
				}
				goto IL_2fa7;
				IL_1621:
				if (num14 != 0)
				{
					order.CouponName = userCouponInfo.CouponName;
					if (userCouponInfo.OrderUseLimit.HasValue)
					{
						order.CouponAmount = userCouponInfo.OrderUseLimit.Value;
					}
					order.CouponCode = text2;
					if (userCouponInfo.Price.Value >= order.GetAmount(false))
					{
						order.CouponValue = order.GetAmount(false);
					}
					else
					{
						order.CouponValue = userCouponInfo.Price.Value;
					}
					text8 = userCouponInfo.CanUseProducts;
				}
				goto IL_16e7;
				end_IL_0001:;
			}
			catch (Exception ex2)
			{
				NameValueCollection param = new NameValueCollection
				{
					context.Request.Form,
					context.Request.QueryString
				};
				Globals.WriteExceptionLog_Page(ex2, param, "CreateOrderException");
			}
		}

		private string CheckCombinaInfo(int Combinaid, string productSku)
		{
			List<ViewCombinationBuySkuInfo> combinaSkusInfoByCombinaId = CombinationBuyHelper.GetCombinaSkusInfoByCombinaId(Combinaid);
			if (combinaSkusInfoByCombinaId == null || combinaSkusInfoByCombinaId.Count == 0)
			{
				return "不存在的活动信息";
			}
			DateTime startDate = combinaSkusInfoByCombinaId[0].StartDate;
			DateTime endDate = combinaSkusInfoByCombinaId[0].EndDate;
			DateTime date = DateTime.Now.Date;
			if (startDate > date || endDate < date)
			{
				return "未到活动时间或者活动已结束";
			}
			int num = Convert.ToInt32(combinaSkusInfoByCombinaId[0].MainProductId);
			bool flag = false;
			string[] array = productSku.Split(',');
			if (array.Length <= 1)
			{
				return "订单信息错误！";
			}
			string[] array2 = productSku.Split(',');
			foreach (string item in array2)
			{
				ViewCombinationBuySkuInfo viewCombinationBuySkuInfo = combinaSkusInfoByCombinaId.FirstOrDefault((ViewCombinationBuySkuInfo c) => c.SkuId == item);
				if (viewCombinationBuySkuInfo == null)
				{
					return "订单中有商品已从组合购活动中移除，请重新下单或联系管理员！";
				}
				if (viewCombinationBuySkuInfo.ProductId == viewCombinationBuySkuInfo.MainProductId)
				{
					flag = true;
				}
			}
			if (!flag)
			{
				return "订单中未包含主商品！请重新下单";
			}
			if (this.CheckProductSkuHasProductPreInfo(productSku))
			{
				return "订单中有商品为预售商品，请联系管理员！";
			}
			return "";
		}

		protected void ExChangeGifts(HttpContext context)
		{
			int num = context.Request["giftId"].ToInt(0);
			if (HiContext.Current.User == null || HiContext.Current.User.UserId == 0)
			{
				context.Response.Write("{\"Status\":\"unlogin\",\"msg\":\"没有登录\"}");
			}
			else
			{
				GiftInfo giftDetails = ProductBrowser.GetGiftDetails(num);
				if (giftDetails == null)
				{
					context.Response.Write("{\"Status\":\"notexists\",\"msg\":\"该礼品不存在或已被删除\"}");
				}
				else if (!giftDetails.IsPointExchange)
				{
					context.Response.Write("{\"Status\":\"notexchange\",\"msg\":\"该礼品不允许使用积分兑换\"}");
				}
				else
				{
					int needPoint = giftDetails.NeedPoint;
					if (giftDetails.NeedPoint > 0)
					{
						int points = HiContext.Current.User.Points;
						if (needPoint <= points)
						{
							ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart(null, false, false, -1);
							if (shoppingCart != null && shoppingCart.LineGifts != null && shoppingCart.LineGifts.Count > 0)
							{
								foreach (ShoppingCartGiftInfo lineGift in shoppingCart.LineGifts)
								{
									if (lineGift.GiftId == num)
									{
										context.Response.Write("{\"Status\":\"hasexists\",\"msg\":\"购物车中已存在该礼品\"}");
										return;
									}
								}
							}
							if (ShoppingCartProcessor.AddGiftItem(num, 1, PromoteType.NotSet, giftDetails.IsExemptionPostage))
							{
								context.Response.Write("{\"Status\":\"success\",\"msg\":\"成功加入购物车\"}");
							}
							context.Response.End();
						}
					}
				}
			}
		}

		private void AddUserPrize(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			int num = 1;
			int.TryParse(context.Request["activityid"], out num);
			string text = context.Request["prize"];
			LotteryActivityInfo lotteryActivity = VshopBrowser.GetLotteryActivity(num);
			PrizeRecordInfo prizeRecordInfo = new PrizeRecordInfo();
			prizeRecordInfo.PrizeTime = DateTime.Now;
			prizeRecordInfo.UserID = HiContext.Current.UserId;
			prizeRecordInfo.ActivityName = lotteryActivity.ActivityName;
			prizeRecordInfo.ActivityID = num;
			prizeRecordInfo.Prizelevel = text;
			switch (text)
			{
			case "一等奖":
				prizeRecordInfo.PrizeName = lotteryActivity.PrizeSettingList[0].PrizeName;
				prizeRecordInfo.IsPrize = true;
				break;
			case "二等奖":
			{
				PrizeRecordInfo prizeRecordInfo2 = prizeRecordInfo;
				PrizeRecordInfo prizeRecordInfo3 = prizeRecordInfo;
				string text3 = prizeRecordInfo2.PrizeName = (prizeRecordInfo3.PrizeName = lotteryActivity.PrizeSettingList[1].PrizeName);
				prizeRecordInfo.IsPrize = true;
				break;
			}
			case "三等奖":
				prizeRecordInfo.PrizeName = lotteryActivity.PrizeSettingList[2].PrizeName;
				prizeRecordInfo.IsPrize = true;
				break;
			case "四等奖":
				prizeRecordInfo.PrizeName = lotteryActivity.PrizeSettingList[3].PrizeName;
				prizeRecordInfo.IsPrize = true;
				break;
			case "五等奖":
				prizeRecordInfo.PrizeName = lotteryActivity.PrizeSettingList[4].PrizeName;
				prizeRecordInfo.IsPrize = true;
				break;
			case "六等奖":
				prizeRecordInfo.PrizeName = lotteryActivity.PrizeSettingList[5].PrizeName;
				prizeRecordInfo.IsPrize = true;
				break;
			default:
				prizeRecordInfo.IsPrize = false;
				break;
			}
			int num2 = VshopBrowser.AddPrizeRecord(prizeRecordInfo);
			if (num2 > 0)
			{
				int userPrizeCount = VshopBrowser.GetUserPrizeCount(lotteryActivity.ActivityId);
				this.ChangePoint(lotteryActivity, userPrizeCount);
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("{");
			stringBuilder.Append("\"Status\":\"OK\"");
			stringBuilder.Append("}");
			context.Response.Write(stringBuilder);
		}

		public void ChangePoint(LotteryActivityInfo activity, int count)
		{
			if (activity.UsePoints > 0)
			{
				int num = (activity.MaxNum == 0) ? 1 : activity.MaxNum;
				if (count > num)
				{
					Hidistro.Entities.Members.MemberInfo user = HiContext.Current.User;
					if (user.UserId != 0)
					{
						PointDetailInfo pointDetailInfo = new PointDetailInfo();
						pointDetailInfo.OrderId = "";
						pointDetailInfo.UserId = user.UserId;
						pointDetailInfo.TradeDate = DateTime.Now;
						pointDetailInfo.TradeType = this.GetTradeType(activity.ActivityType);
						pointDetailInfo.Reduced = activity.UsePoints;
						pointDetailInfo.Points = user.Points - activity.UsePoints;
						if (pointDetailInfo.Points > 2147483647 || pointDetailInfo.Points < 0)
						{
							pointDetailInfo.Points = 0;
						}
						if (new PointDetailDao().Add(pointDetailInfo, null) > 0)
						{
							user.Points = pointDetailInfo.Points;
						}
					}
				}
			}
		}

		public PointTradeType GetTradeType(int actType)
		{
			switch (actType)
			{
			case 1:
				return PointTradeType.JoinRotaryTable;
			case 2:
				return PointTradeType.JoinScratchCard;
			case 3:
				return PointTradeType.JoinSmashingGoldenEgg;
			case 4:
				return PointTradeType.JoinWeiLuckDraw;
			default:
				return PointTradeType.JoinRotaryTable;
			}
		}

		private void CheckHasPrized(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			int activityid = 0;
			int.TryParse(context.Request["activityid"], out activityid);
			LotteryActivityInfo lotteryActivity = VshopBrowser.GetLotteryActivity(activityid);
			if (lotteryActivity != null)
			{
				PrizeRecordInfo prizeRecordInfo = VshopBrowser.LastPrizeRecord(activityid);
				if (prizeRecordInfo != null && prizeRecordInfo.IsPrize && string.IsNullOrEmpty(prizeRecordInfo.RealName) && string.IsNullOrEmpty(prizeRecordInfo.CellPhone))
				{
					context.Response.Write("{\"No\":\"-2\"}");
					context.Response.End();
				}
				else if (prizeRecordInfo?.IsPrize ?? false)
				{
					context.Response.Write("{\"No\":\"-3\"}");
					context.Response.End();
				}
				else if (DateTime.Now < lotteryActivity.StartTime || DateTime.Now > lotteryActivity.EndTime)
				{
					context.Response.Write("{\"No\":\"-4\"}");
					context.Response.End();
				}
				else
				{
					context.Response.Write("{\"No\":\"0\"}");
					context.Response.End();
				}
			}
			else
			{
				context.Response.Write("{\"No\":\"-1\"}");
				context.Response.End();
			}
		}

		private void GetPrize(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			int num = 1;
			int.TryParse(context.Request["activityid"], out num);
			LotteryActivityInfo lotteryActivity = VshopBrowser.GetLotteryActivity(num);
			int userPrizeCount = VshopBrowser.GetUserPrizeCount(num);
			userPrizeCount++;
			Hidistro.Entities.Members.MemberInfo user = HiContext.Current.User;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("{");
			PrizeRecordInfo prizeRecordInfo = VshopBrowser.LastPrizeRecord(num);
			if (prizeRecordInfo != null && prizeRecordInfo.IsPrize && string.IsNullOrEmpty(prizeRecordInfo.RealName) && string.IsNullOrEmpty(prizeRecordInfo.CellPhone))
			{
				stringBuilder.Append("\"No\":\"-18\"");
				stringBuilder.Append("}");
				context.Response.Write(stringBuilder.ToString());
				context.Response.End();
			}
			else if (prizeRecordInfo?.IsPrize ?? false)
			{
				stringBuilder.Append("\"No\":\"-19\"");
				stringBuilder.Append("}");
				context.Response.Write(stringBuilder.ToString());
				context.Response.End();
			}
			else
			{
				if (userPrizeCount > lotteryActivity.MaxNum)
				{
					if (lotteryActivity.UsePoints <= 0)
					{
						stringBuilder.Append("\"No\":\"-1\"");
						stringBuilder.Append("}");
						context.Response.Write(stringBuilder.ToString());
						context.Response.End();
						return;
					}
					if (user.Points < lotteryActivity.UsePoints)
					{
						stringBuilder.Append("\"No\":\"-4\"");
						stringBuilder.Append("}");
						context.Response.Write(stringBuilder.ToString());
						context.Response.End();
						return;
					}
				}
				if (DateTime.Now < lotteryActivity.StartTime || DateTime.Now > lotteryActivity.EndTime)
				{
					stringBuilder.Append("\"No\":\"-3\"");
					stringBuilder.Append("}");
					context.Response.Write(stringBuilder.ToString());
				}
				else
				{
					PrizeQuery prizeQuery = new PrizeQuery();
					prizeQuery.ActivityId = num;
					List<PrizeRecordInfo> prizeList = VshopBrowser.GetPrizeList(prizeQuery);
					int num2 = 0;
					int num3 = 0;
					int num4 = 0;
					int num5 = 0;
					int num6 = 0;
					int num7 = 0;
					if (prizeList != null && prizeList.Count > 0)
					{
						num2 = prizeList.Count((PrizeRecordInfo a) => a.Prizelevel == "一等奖");
						num3 = prizeList.Count((PrizeRecordInfo a) => a.Prizelevel == "二等奖");
						num4 = prizeList.Count((PrizeRecordInfo a) => a.Prizelevel == "三等奖");
					}
					PrizeRecordInfo prizeRecordInfo2 = new PrizeRecordInfo();
					prizeRecordInfo2.PrizeTime = DateTime.Now;
					prizeRecordInfo2.UserID = HiContext.Current.UserId;
					prizeRecordInfo2.ActivityName = lotteryActivity.ActivityName;
					prizeRecordInfo2.ActivityID = num;
					prizeRecordInfo2.IsPrize = true;
					List<PrizeSetting> prizeSettingList = lotteryActivity.PrizeSettingList;
					decimal d = prizeSettingList[0].Probability * 100m;
					decimal d2 = prizeSettingList[1].Probability * 100m;
					decimal d3 = prizeSettingList[2].Probability * 100m;
					Random random = new Random(Guid.NewGuid().GetHashCode());
					int value = random.Next(1, 10001);
					if (prizeSettingList.Count > 3)
					{
						decimal d4 = prizeSettingList[3].Probability * 100m;
						decimal d5 = prizeSettingList[4].Probability * 100m;
						decimal d6 = prizeSettingList[5].Probability * 100m;
						num5 = prizeList.Count((PrizeRecordInfo a) => a.Prizelevel == "四等奖");
						num6 = prizeList.Count((PrizeRecordInfo a) => a.Prizelevel == "五等奖");
						num7 = prizeList.Count((PrizeRecordInfo a) => a.Prizelevel == "六等奖");
						if ((decimal)value < d && prizeSettingList[0].PrizeNum > num2)
						{
							stringBuilder.Append("\"No\":\"9\"");
							prizeRecordInfo2.Prizelevel = "一等奖";
							prizeRecordInfo2.PrizeName = prizeSettingList[0].PrizeName;
						}
						else if ((decimal)value < d2 && prizeSettingList[1].PrizeNum > num3)
						{
							stringBuilder.Append("\"No\":\"11\"");
							prizeRecordInfo2.Prizelevel = "二等奖";
							prizeRecordInfo2.PrizeName = prizeSettingList[1].PrizeName;
						}
						else if ((decimal)value < d3 && prizeSettingList[2].PrizeNum > num4)
						{
							stringBuilder.Append("\"No\":\"1\"");
							prizeRecordInfo2.Prizelevel = "三等奖";
							prizeRecordInfo2.PrizeName = prizeSettingList[2].PrizeName;
						}
						else if ((decimal)value < d4 && prizeSettingList[3].PrizeNum > num5)
						{
							stringBuilder.Append("\"No\":\"3\"");
							prizeRecordInfo2.Prizelevel = "四等奖";
							prizeRecordInfo2.PrizeName = prizeSettingList[3].PrizeName;
						}
						else if ((decimal)value < d5 && prizeSettingList[4].PrizeNum > num6)
						{
							stringBuilder.Append("\"No\":\"5\"");
							prizeRecordInfo2.Prizelevel = "五等奖";
							prizeRecordInfo2.PrizeName = prizeSettingList[4].PrizeName;
						}
						else if ((decimal)value < d6 && prizeSettingList[5].PrizeNum > num7)
						{
							stringBuilder.Append("\"No\":\"7\"");
							prizeRecordInfo2.Prizelevel = "六等奖";
							prizeRecordInfo2.PrizeName = prizeSettingList[5].PrizeName;
						}
						else
						{
							prizeRecordInfo2.IsPrize = false;
							stringBuilder.Append("\"No\":\"0\"");
						}
					}
					else if ((decimal)value < d && prizeSettingList[0].PrizeNum > num2)
					{
						stringBuilder.Append("\"No\":\"9\"");
						prizeRecordInfo2.Prizelevel = "一等奖";
						prizeRecordInfo2.PrizeName = prizeSettingList[0].PrizeName;
					}
					else if ((decimal)value < d2 && prizeSettingList[1].PrizeNum > num3)
					{
						stringBuilder.Append("\"No\":\"11\"");
						prizeRecordInfo2.Prizelevel = "二等奖";
						prizeRecordInfo2.PrizeName = prizeSettingList[1].PrizeName;
					}
					else if ((decimal)value < d3 && prizeSettingList[2].PrizeNum > num4)
					{
						stringBuilder.Append("\"No\":\"1\"");
						prizeRecordInfo2.Prizelevel = "三等奖";
						prizeRecordInfo2.PrizeName = prizeSettingList[2].PrizeName;
					}
					else
					{
						prizeRecordInfo2.IsPrize = false;
						stringBuilder.Append("\"No\":\"0\"");
					}
					stringBuilder.Append("}");
					if (context.Request["activitytype"] != "scratch")
					{
						VshopBrowser.AddPrizeRecord(prizeRecordInfo2);
						this.ChangePoint(lotteryActivity, userPrizeCount);
					}
					context.Response.Write(stringBuilder.ToString());
				}
			}
		}

		private void Vote(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			int voteId = 1;
			int.TryParse(context.Request["voteId"], out voteId);
			string text = context.Request["itemIds"];
			text = text.Remove(text.Length - 1);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("{");
			if (VshopBrowser.Vote(voteId, text))
			{
				stringBuilder.Append("\"Status\":\"OK\"");
			}
			else
			{
				stringBuilder.Append("\"Status\":\"Error\"");
			}
			stringBuilder.Append("}");
			context.Response.Write(stringBuilder.ToString());
		}

		public void SetUserName(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			Hidistro.Entities.Members.MemberInfo user = HiContext.Current.User;
			string text = context.Request["CellPhone"];
			if (text != null)
			{
				text = text.Trim();
			}
			if (!string.IsNullOrEmpty(text) && HiContext.Current.User.CellPhone != text && HiContext.Current.User.UserName != text)
			{
				Hidistro.Entities.Members.MemberInfo memberInfo = MemberProcessor.FindMemberByUsername(text);
				if (MemberProcessor.IsUseCellphone(text) || (memberInfo != null && memberInfo.UserName == text))
				{
					context.Response.Write("{\"success\":\"false\",\"msg\":\"手机号码已存在!\"}");
					return;
				}
				user.CellPhoneVerification = false;
			}
			user.CellPhone = context.Request["CellPhone"];
			user.QQ = context.Request["QQ"];
			user.RealName = context.Request["RealName"];
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("{");
			if (MemberProcessor.UpdateMember(user))
			{
				stringBuilder.Append("\"Status\":\"OK\"");
			}
			else
			{
				stringBuilder.Append("\"Status\":\"Error\"");
			}
			stringBuilder.Append("}");
			context.Response.Write(stringBuilder.ToString());
		}

		public void SkipBindUser(HttpContext context)
		{
			bool flag = true;
			int num = 0;
			string text = context.Request["OpenIdType"];
			if (HiContext.Current.SiteSettings.QuickLoginIsForceBindingMobbile && text.ToLower() != "hishop.plugins.openid.weixin")
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("{");
				stringBuilder.Append("\"Status\":\"-2\"");
				stringBuilder.Append(",\"StoreId\":0");
				stringBuilder.Append("}");
				context.Response.Write(stringBuilder.ToString());
				context.Response.End();
			}
			switch (text.ToLower())
			{
			case "hishop.plugins.openid.alipay.alipayservice":
				num = this.SkipAlipayOpenId(context);
				break;
			case "hishop.plugins.openid.qq.qqservice":
				num = this.SkipQQOpenId(context);
				break;
			case "hishop.plugins.openid.taobao.taobaoservice":
				num = this.SkipTaoBaoOpenId(context);
				break;
			case "hishop.plugins.openid.sina.sinaservice":
				num = this.SkipSinaOpenId(context);
				break;
			case "weixin":
			case "hishop.plugins.openid.weixin":
				num = this.SkipWeixinOpenId(context, out flag);
				break;
			}
			context.Response.ContentType = "application/json";
			StringBuilder stringBuilder2 = new StringBuilder();
			stringBuilder2.Append("{");
			stringBuilder2.Append("\"Status\":\"" + num + "\"");
			stringBuilder2.AppendFormat(",\"StoreId\":{0}", (HiContext.Current.User != null) ? HiContext.Current.User.StoreId : 0);
			if (num == 1 & flag)
			{
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
					if (num2 > 0)
					{
						stringBuilder2.Append(",\"GiftCouponMsg\":\"恭喜您注册成功，" + num2 + " 张优惠券已经放入您的账户，可在会员中心我的优惠券中进行查看\"");
					}
				}
			}
			stringBuilder2.Append("}");
			context.Response.Write(stringBuilder2.ToString());
		}

		public void BindUser(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			string text = context.Request["openId"];
			string text2 = context.Request["OpenIdType"];
			if (!string.IsNullOrEmpty(text2))
			{
				text2 = text2.ToLower();
				if (text2 == "weixin")
				{
					text2 = "hishop.plugins.openid.weixin";
				}
			}
			string userName = Globals.StripAllTags(context.Request["userName"].ToNullString());
			string password = Globals.StripAllTags(context.Request["password"].ToNullString());
			string text3 = Globals.StripAllTags(context.Request["nickname"].ToNullString());
			string text4 = Globals.StripAllTags(context.Request["client"].ToNullString());
			string text5 = Globals.StripAllTags(context.Request["unionId"].ToNullString());
			bool flag = context.Request["IsSubscribe"].ToBool();
			if (!string.IsNullOrEmpty(text4))
			{
				text4 = text4.ToLower();
			}
			StringBuilder stringBuilder = new StringBuilder();
			Hidistro.Entities.Members.MemberInfo memberInfo = MemberProcessor.ValidLogin(userName, password);
			stringBuilder.Append("{");
			if (memberInfo != null)
			{
				if (!string.IsNullOrEmpty(text2) && !string.IsNullOrEmpty(text))
				{
					Hidistro.Entities.Members.MemberInfo memberByOpenId = MemberProcessor.GetMemberByOpenId(text2, text);
					if (text2 != "hishop.plugins.openid.weixin" && memberByOpenId != null && memberByOpenId.UserId != memberInfo.UserId)
					{
						stringBuilder.Append("\"Status\":\"" + -3 + "\"");
						stringBuilder.Append("}");
						context.Response.Write(stringBuilder.ToString());
						context.Response.End();
						return;
					}
					MemberOpenIdInfo memberOpenIdInfo = MemberProcessor.GetMemberOpenIdInfo(memberInfo.UserId, text2);
					if (text2 != "hishop.plugins.openid.weixin" && memberOpenIdInfo != null && memberOpenIdInfo.OpenId != text)
					{
						stringBuilder.Append("\"Status\":\"" + -3 + "\"");
						stringBuilder.Append("}");
						context.Response.Write(stringBuilder.ToString());
						return;
					}
				}
				bool flag2 = true;
				memberInfo.IsLogined = true;
				Users.ClearUserCache(memberInfo.UserId, "");
				Users.SetCurrentUser(memberInfo.UserId, 30, false, false);
				HiContext.Current.User = memberInfo;
				if (text2 == "hishop.plugins.openid.weixin" && context.Request["IsSubscribe"] != null && memberInfo.IsSubscribe != flag)
				{
					memberInfo.IsSubscribe = flag;
					flag2 = true;
				}
				if (memberInfo.IsQuickLogin && !memberInfo.IsDefaultDevice)
				{
					memberInfo.IsDefaultDevice = true;
					flag2 = true;
				}
				if (!memberInfo.IsQuickLogin)
				{
					if (text2 == "hishop.plugins.openid.weixin" && !string.IsNullOrEmpty(text))
					{
						Hidistro.Entities.Members.MemberInfo memberByOpenId2 = MemberProcessor.GetMemberByOpenId(text2, text);
						if (memberByOpenId2 == null)
						{
							memberInfo.IsDefaultDevice = true;
							flag2 = true;
						}
						else
						{
							memberInfo.IsDefaultDevice = false;
							flag2 = true;
							if (!memberByOpenId2.IsQuickLogin)
							{
								MemberProcessor.DeleteMemberOpenId(memberByOpenId2.UserId, text2);
								memberByOpenId2.IsLogined = false;
							}
							else
							{
								memberByOpenId2.IsLogined = false;
							}
							MemberProcessor.UpdateMember(memberByOpenId2);
						}
					}
					if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text2))
					{
						this.AddOrUpdateOpenId(memberInfo.UserId, text2, text);
					}
				}
				else if (text2 != "hishop.plugins.openid.weixin" && !string.IsNullOrEmpty(text))
				{
					this.AddOrUpdateOpenId(memberInfo.UserId, text2, text);
				}
				if (!string.IsNullOrEmpty(text))
				{
					HttpCookie httpCookie = new HttpCookie("openId");
					httpCookie.HttpOnly = true;
					httpCookie.Value = text;
					httpCookie.Expires = DateTime.MaxValue;
					HttpContext.Current.Response.Cookies.Add(httpCookie);
				}
				lock (this.lockCopyRedEnvelope)
				{
					this.CopyRedEnvelope(text, memberInfo);
				}
				if (!string.IsNullOrEmpty(text5) && MemberProcessor.GetMemberByUnionId(text5) == null)
				{
					memberInfo.UnionId = text5;
					flag2 = true;
				}
				if (string.IsNullOrEmpty(memberInfo.NickName) && !string.IsNullOrEmpty(text3))
				{
					memberInfo.NickName = HttpUtility.UrlDecode(text3);
					flag2 = true;
				}
				if (flag2)
				{
					MemberProcessor.UpdateMember(memberInfo);
				}
				ShoppingCartInfo cookieShoppingCart = ShoppingCartProcessor.GetCookieShoppingCart();
				if (cookieShoppingCart != null)
				{
					ShoppingCartProcessor.ConvertShoppingCartToDataBase(cookieShoppingCart);
					ShoppingCartProcessor.ClearCookieShoppingCart();
				}
				stringBuilder.Append("\"Status\":\"OK\"");
			}
			else
			{
				stringBuilder.Append("\"Status\":\"" + -1 + "\"");
			}
			stringBuilder.Append("}");
			context.Response.Write(stringBuilder.ToString());
		}

		public void AddOrUpdateOpenId(int userId, string openIdType, string openId)
		{
			MemberOpenIdInfo memberOpenIdInfo = new MemberOpenIdDao().GetMemberOpenIdInfo(userId, openIdType);
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

		public void CopyRedEnvelope(string openId, Hidistro.Entities.Members.MemberInfo memberInfo)
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
						couponItemInfo.CouponName = weiXinRedEnvelope.Name + "_" + Guid.NewGuid().ToString();
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

		public void RegisterUser(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			string text = Globals.StripAllTags(context.Request["client"].ToNullString());
			if (string.IsNullOrEmpty(text))
			{
				text = "wap";
			}
			text = text.ToLower();
			string text2 = Globals.StripAllTags(context.Request["openId"].ToNullString());
			string text3 = Globals.StripAllTags(context.Request["OpenIdType"].ToNullString());
			if (!string.IsNullOrEmpty(text3))
			{
				text3 = text3.ToLower();
				if (text3 == "weixin")
				{
					text3 = "hishop.plugins.openid.weixin";
				}
			}
			else
			{
				text3 = "";
			}
			string text4 = Globals.StripAllTags(context.Request["userName"].ToNullString());
			string text5 = Globals.StripAllTags(context.Request["email"].ToNullString());
			string text6 = context.Request["password"].ToNullString();
			string text7 = Globals.StripAllTags(context.Request["nickname"].ToNullString());
			string verifyCode = Globals.StripAllTags(context.Request["VerifyCode"].ToNullString());
			string verifyCode2 = Globals.StripAllTags(context.Request["emailCode"].ToNullString());
			string unionId = Globals.StripAllTags(context.Request["unionId"].ToNullString());
			string realName = Globals.StripAllTags(context.Request["realName"].ToNullString());
			string text8 = Globals.StripAllTags(context.Request["gender"].ToNullString());
			bool isSubscribe = context.Request["isSubscribe"].ToBool();
			DateTime? birthDate = context.Request["birthday"].ToDateTime();
			StringBuilder stringBuilder = new StringBuilder();
			SiteSettings siteSettings = HiContext.Current.SiteSettings;
			Hidistro.Entities.Members.MemberInfo memberInfo = new Hidistro.Entities.Members.MemberInfo();
			if (HiContext.Current.ReferralUserId > 0)
			{
				memberInfo.ReferralUserId = HiContext.Current.ReferralUserId;
			}
			else if (text3 == "hishop.plugins.openid.weixin" && !string.IsNullOrEmpty(text2))
			{
				MemberWXReferralInfo wXReferral = VShopHelper.GetWXReferral(text2.Trim());
				if (wXReferral != null)
				{
					memberInfo.ReferralUserId = wXReferral.ReferralUserId;
					VShopHelper.DeleteWXReferral(text2.Trim());
				}
			}
			if (HiContext.Current.ShoppingGuiderId > 0)
			{
				memberInfo.ShoppingGuiderId = HiContext.Current.ShoppingGuiderId;
			}
			else if (text3 == "hishop.plugins.openid.weixin" && !string.IsNullOrEmpty(text2))
			{
				MemberWXShoppingGuiderInfo memberWXShoppingGuider = MemberHelper.GetMemberWXShoppingGuider(text2.Trim());
				if (memberWXShoppingGuider != null)
				{
					memberInfo.ShoppingGuiderId = memberWXShoppingGuider.ShoppingGuiderId;
					MemberHelper.DeleteWXShoppingGuider(text2.Trim());
				}
			}
			memberInfo.GradeId = MemberProcessor.GetDefaultMemberGrade();
			memberInfo.IsSubscribe = isSubscribe;
			MemberDao memberDao = new MemberDao();
			if (memberDao.GetMember(text4) != null)
			{
				stringBuilder.Append("{");
				stringBuilder.Append("\"Status\":\"1\"");
				stringBuilder.Append("}");
				context.Response.Write(stringBuilder.ToString());
			}
			else
			{
				if (this.emailR.IsMatch(text4))
				{
					if (memberDao.MemberEmailIsExist(text4))
					{
						stringBuilder.Append("{");
						stringBuilder.Append("\"Status\":\"2\"");
						stringBuilder.Append("}");
						context.Response.Write(stringBuilder.ToString());
						return;
					}
					bool emailVerification = false;
					if (siteSettings.IsNeedValidEmail)
					{
						if (!HiContext.Current.CheckVerifyCode(verifyCode2, ""))
						{
							stringBuilder.Append("{");
							stringBuilder.Append("\"Status\":\"8\"");
							stringBuilder.Append("}");
							context.Response.Write(stringBuilder.ToString());
							return;
						}
						emailVerification = true;
					}
					memberInfo.Email = text4;
					memberInfo.EmailVerification = emailVerification;
				}
				if (this.cellphoneR.IsMatch(text4))
				{
					if (!siteSettings.SMSEnabled || string.IsNullOrEmpty(siteSettings.SMSSettings))
					{
						stringBuilder.Append("{");
						stringBuilder.Append("\"Status\":\"5\"");
						stringBuilder.Append("}");
						context.Response.Write(stringBuilder.ToString());
						return;
					}
					if (!siteSettings.IsOpenGeetest && !HiContext.Current.CheckVerifyCode(context.Request.Form["imgCode"], ""))
					{
						stringBuilder.Append("{");
						stringBuilder.Append("\"Status\":\"6\"");
						stringBuilder.Append("}");
						context.Response.Write(stringBuilder.ToString());
						return;
					}
					if (siteSettings.IsOpenGeetest)
					{
						byte b = (byte)HiCache.Get("gt_server_status");
						GeetestLib geetestLib = new GeetestLib(siteSettings.GeetestKey, siteSettings.GeetestId);
						string challenge = context.Request["geetest_challenge"].ToNullString();
						string validate = context.Request["geetest_validate"].ToNullString();
						string seccode = context.Request["geetest_seccode"].ToNullString();
						int num = 0;
						num = ((b != 1) ? geetestLib.failbackValidateRequest(challenge, validate, seccode) : geetestLib.enhencedValidateRequest(challenge, validate, seccode, "mec"));
						if (num != 1)
						{
							stringBuilder.Append("{");
							stringBuilder.Append("\"Status\":\"9\"");
							stringBuilder.Append("}");
							context.Response.Write(stringBuilder.ToString());
							return;
						}
					}
					string arg = "";
					if (!HiContext.Current.CheckPhoneVerifyCode(verifyCode, text4, out arg))
					{
						stringBuilder.Append("{");
						stringBuilder.Append("\"Status\":\"3\",");
						stringBuilder.AppendFormat("\"Msg\":\"{0}\"", arg);
						stringBuilder.Append("}");
						context.Response.Write(stringBuilder.ToString());
						context.Response.End();
						return;
					}
					if (memberDao.MemberCellphoneIsExist(text4))
					{
						stringBuilder.Append("{");
						stringBuilder.Append("\"Status\":\"4\"");
						stringBuilder.Append("}");
						context.Response.Write(stringBuilder.ToString());
						return;
					}
					memberInfo.CellPhone = text4;
					memberInfo.CellPhoneVerification = true;
				}
				memberInfo.UserName = text4;
				if (MemberProcessor.FindMemberByUsername(text4) != null)
				{
					stringBuilder.Append("{");
					stringBuilder.Append("\"Status\":\"1\"");
					stringBuilder.Append("}");
					context.Response.Write(stringBuilder.ToString());
				}
				else
				{
					if (!string.IsNullOrEmpty(text7))
					{
						memberInfo.NickName = HttpUtility.UrlDecode(text7);
					}
					string text9 = Globals.RndStr(128, true);
					string text10 = text6;
					text6 = (memberInfo.Password = Users.EncodePassword(text6, text9));
					memberInfo.PasswordSalt = text9;
					memberInfo.RealName = realName;
					memberInfo.BirthDate = birthDate;
					memberInfo.Gender = ((!string.IsNullOrEmpty(text8)) ? ((text8 == "男士") ? Gender.Male : Gender.Female) : Gender.NotSet);
					memberInfo.RegisteredSource = ((text == "wap") ? 2 : ((text == "alioh") ? 4 : 3));
					memberInfo.CreateDate = DateTime.Now;
					memberInfo.IsLogined = true;
					if (MemberProcessor.GetMemberByUnionId(unionId) == null)
					{
						memberInfo.UnionId = unionId;
					}
					int num2 = MemberProcessor.CreateMember(memberInfo);
					stringBuilder.Append("{");
					if (num2 > 0)
					{
						Messenger.UserRegister(memberInfo, text6);
						memberInfo.UserId = num2;
						if (!string.IsNullOrEmpty(text2) && !string.IsNullOrEmpty(text3))
						{
							MemberOpenIdInfo memberOpenIdInfo = new MemberOpenIdInfo();
							memberOpenIdInfo.UserId = memberInfo.UserId;
							memberOpenIdInfo.OpenIdType = text3;
							memberOpenIdInfo.OpenId = text2;
							if (text3 == "hishop.plugins.openid.weixin")
							{
								Hidistro.Entities.Members.MemberInfo memberByOpenId = MemberProcessor.GetMemberByOpenId(text3, text2);
								if (memberByOpenId != null)
								{
									if (!memberByOpenId.IsQuickLogin)
									{
										MemberProcessor.DeleteMemberOpenId(memberByOpenId.UserId, text3);
										memberByOpenId.IsLogined = false;
									}
									else
									{
										memberByOpenId.IsLogined = false;
									}
									MemberProcessor.UpdateMember(memberByOpenId);
								}
							}
							MemberProcessor.AddMemberOpenId(memberOpenIdInfo);
							if (!string.IsNullOrEmpty(text2))
							{
								HttpCookie httpCookie = new HttpCookie("openId");
								httpCookie.HttpOnly = true;
								httpCookie.Value = text2;
								httpCookie.Expires = DateTime.MaxValue;
								HttpContext.Current.Response.Cookies.Add(httpCookie);
							}
							lock (this.lockCopyRedEnvelope)
							{
								this.CopyRedEnvelope(text2, memberInfo);
							}
						}
						Users.SetCurrentUser(num2, 30, false, false);
						HiContext.Current.User = memberInfo;
						stringBuilder.Append("\"Status\":\"OK\"");
						stringBuilder.AppendFormat(",\"StoreId\":{0}", HiContext.Current.User.StoreId);
						ShoppingCartInfo cookieShoppingCart = ShoppingCartProcessor.GetCookieShoppingCart();
						if (cookieShoppingCart != null)
						{
							ShoppingCartProcessor.ConvertShoppingCartToDataBase(cookieShoppingCart);
							ShoppingCartProcessor.ClearCookieShoppingCart();
						}
						stringBuilder.Append(",\"StoreId\":\"" + memberInfo.StoreId + "\"");
						SiteSettings masterSettings = SettingsManager.GetMasterSettings();
						if (masterSettings.IsOpenGiftCoupons)
						{
							int num3 = 0;
							string[] array = masterSettings.GiftCouponList.Split(',');
							foreach (string obj in array)
							{
								if (obj.ToInt(0) > 0 && CouponHelper.AddCouponItemInfo(memberInfo, obj.ToInt(0)) == CouponActionStatus.Success)
								{
									num3++;
								}
							}
							if (num3 > 0)
							{
								stringBuilder.Append(",\"GiftCouponMsg\":\"恭喜您注册成功，" + num3 + " 张优惠券已经放入您的账户，可在会员中心我的优惠券中进行查看\"");
							}
						}
					}
					else
					{
						stringBuilder.Append("\"Status\":\"0\"");
					}
					stringBuilder.Append("}");
					context.Response.Write(stringBuilder.ToString());
				}
			}
		}

		public void UpdateInformationMember(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			StringBuilder stringBuilder = new StringBuilder();
			int gender = context.Request["gender"].ToInt(0);
			string realName = Globals.StripAllTags(context.Request["RealName"].ToNullString());
			DateTime? birthDate = context.Request["birthday"].ToDateTime();
			string qQ = Globals.StripAllTags(context.Request["QQ"].ToNullString());
			string nickName = Globals.StripAllTags(context.Request["MSN"].ToNullString());
			string text = Globals.StripAllTags(context.Request["picture"].ToNullString()).ToLower();
			Hidistro.Entities.Members.MemberInfo user = HiContext.Current.User;
			string text2 = user.Picture;
			int num = context.Request["RegionId"].ToInt(0);
			string address = Globals.StripAllTags(context.Request["Address"].ToNullString());
			if (!string.IsNullOrEmpty(text) && !text.StartsWith("http://") && !text.StartsWith("https://") && !string.IsNullOrEmpty(text) && !text.EndsWith(".jpg") && !text.EndsWith(".jpeg") && !text.EndsWith(".png") && !text.EndsWith(".gif"))
			{
				stringBuilder.Append("{");
				stringBuilder.Append("\"Status\":\"2\",");
				stringBuilder.AppendFormat("\"Msg\":\"{0}\"", "头像的图片格式不正确");
				stringBuilder.Append("}");
				context.Response.Write(stringBuilder.ToString());
				context.Response.End();
			}
			else
			{
				if (!string.IsNullOrEmpty(text) && !text.StartsWith("http://") && !text.StartsWith("https://") && text != user.Picture.ToNullString().ToLower())
				{
					text2 = "/Storage/master/user/" + text.Substring(text.LastIndexOf("/") + 1);
					File.Copy(HttpContext.Current.Server.MapPath(text), HttpContext.Current.Server.MapPath(text2), true);
				}
				user.Gender = (Gender)gender;
				user.RealName = realName;
				user.BirthDate = birthDate;
				user.QQ = qQ;
				user.NickName = nickName;
				user.Picture = text2;
				if (num > 0)
				{
					user.RegionId = num;
					user.Address = address;
				}
				if (MemberProcessor.UpdateMember(user))
				{
					stringBuilder.Append("{");
					stringBuilder.Append("\"Status\":\"1\",");
					stringBuilder.AppendFormat("\"Msg\":\"{0}\"", "保存成功");
					stringBuilder.Append("}");
					context.Response.Write(stringBuilder.ToString());
					context.Response.End();
				}
				else
				{
					stringBuilder.Append("{");
					stringBuilder.Append("\"Status\":\"2\",");
					stringBuilder.AppendFormat("\"Msg\":\"{0}\"", "保存失败，请重新保存");
					stringBuilder.Append("}");
					context.Response.Write(stringBuilder.ToString());
					context.Response.End();
				}
			}
		}

		private void GetCanShipStores(HttpContext context)
		{
			int regionId = context.Request["regionId"].ToInt(0);
			IList<StoresInfo> canShipStores = DepotHelper.GetCanShipStores(regionId, false);
			StringBuilder strData = new StringBuilder();
			if (canShipStores.Count == 0)
			{
				strData.Append("[]");
			}
			else
			{
				strData.Append("[");
				canShipStores.ForEach(delegate(StoresInfo x)
				{
					strData.Append("{");
					strData.AppendFormat("\"StoreId\":{0},\"StoreName\":\"{1}\",", x.StoreId, x.StoreName);
					string fullRegion = RegionHelper.GetFullRegion(x.RegionId, "&nbsp;&nbsp;", true, 0);
					strData.AppendFormat("\"StoreInfo\":\"{0}\"", x.StoreName + "&nbsp;&nbsp;&nbsp;&nbsp;" + fullRegion + "&nbsp;&nbsp;" + x.Address + "&nbsp;&nbsp;&nbsp;&nbsp;电话：" + x.Tel);
					strData.Append("},");
				});
				strData.Remove(strData.Length - 1, 1);
				strData.Append("]");
			}
			context.Response.ContentType = "text/plain";
			context.Response.Write(strData);
		}

		public void PayCheckOrder(HttpContext context)
		{
			string text = Globals.StripAllTags(context.Request["OrderId"].ToNullString());
			if (HiContext.Current.UserId == 0)
			{
				context.Response.Write("{\"ErrorCode\":\"001\",\"msg\":\"请先登入！\"}");
			}
			else if (string.IsNullOrEmpty(text))
			{
				context.Response.Write("{\"ErrorCode\":\"002\",\"msg\":\"参数错误！请检查\"}");
			}
			else
			{
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(text);
				if (orderInfo == null)
				{
					context.Response.Write("{\"ErrorCode\":\"002\",\"msg\":\"参数错误！请检查\"}");
				}
				else if (orderInfo.UserId == HiContext.Current.UserId)
				{
					string str = "";
					switch (TradeHelper.CheckOrderBeforePay(orderInfo, out str))
					{
					case 0:
						context.Response.Write("{\"ErrorCode\":\"0\",\"msg\":\"ok\"}");
						break;
					case 1:
						TradeHelper.CloseOrder(orderInfo.OrderId, "订单中有商品(" + str + ")规格被删除或者下架");
						context.Response.Write("{\"ErrorCode\":\"004\",\"msg\":\"" + str + "\"}");
						break;
					case 2:
						context.Response.Write("{\"ErrorCode\":\"005\",\"msg\":\"" + str + "\"}");
						break;
					}
				}
			}
		}

		private bool CheckPresaleInfo(ProductPreSaleInfo Info, string productSku, out string Msg)
		{
			Msg = "";
			if (Info == null)
			{
				Msg = "活动不存在！";
				return false;
			}
			if (productSku.Split(',').Length > 1)
			{
				Msg = "订单信息错误";
				return false;
			}
			if (!ProductPreSaleHelper.HasProductPreSaleInfo(productSku, Info.PreSaleId))
			{
				Msg = "订单信息错误";
				return false;
			}
			return true;
		}

		private bool CheckProductSkuHasProductPreInfo(string productSku)
		{
			string[] skuIdArray = productSku.Split(',');
			return ProductPreSaleHelper.HasProductPreSaleInfoBySkuIds(skuIdArray);
		}

		protected int SkipAlipayOpenId(HttpContext context)
		{
			int num = 1;
			string openId = Globals.StripAllTags(context.Request["OpenId"].ToNullString());
			string openIdType = Globals.StripAllTags(context.Request["openIdType"].ToNullString());
			string value = Globals.StripAllTags(context.Request["token"].ToNullString());
			string text = Globals.StripAllTags(context.Request["real_name"].ToNullString());
			string str = Globals.StripAllTags(context.Request["user_id"].ToNullString());
			string text2 = Globals.StripAllTags(context.Request["email"].ToNullString());
			Hidistro.Entities.Members.MemberInfo memberInfo = new Hidistro.Entities.Members.MemberInfo();
			int num2 = 0;
			if (HiContext.Current.ReferralUserId > 0)
			{
				memberInfo.ReferralUserId = HiContext.Current.ReferralUserId;
			}
			memberInfo.GradeId = MemberProcessor.GetDefaultMemberGrade();
			memberInfo.UserName = text;
			memberInfo.NickName = text;
			if (string.IsNullOrEmpty(memberInfo.UserName))
			{
				memberInfo.UserName = "alipay" + str;
			}
			if (MemberProcessor.FindMemberByUsername(memberInfo.UserName) != null)
			{
				memberInfo.UserName = "alipay" + this.GenerateUsername(8);
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
				string pass = this.GeneratePassword();
				string text3 = "Open";
				pass = (memberInfo.Password = Users.EncodePassword(pass, text3));
				memberInfo.PasswordSalt = text3;
				string text5 = Globals.StripAllTags(context.Request["client"].ToNullString());
				if (string.IsNullOrEmpty(text5))
				{
					text5 = "wap";
				}
				text5 = text5.ToLower();
				memberInfo.RegisteredSource = ((text5 == "wap") ? 2 : ((text5 == "alioh") ? 4 : 3));
				memberInfo.CreateDate = DateTime.Now;
				memberInfo.IsLogined = true;
				num2 = MemberProcessor.CreateMember(memberInfo);
				if (num2 <= 0)
				{
					num = -1;
				}
			}
			if (num == 1)
			{
				memberInfo.UserId = num2;
				MemberOpenIdInfo memberOpenIdInfo = new MemberOpenIdInfo();
				memberOpenIdInfo.UserId = memberInfo.UserId;
				memberOpenIdInfo.OpenIdType = openIdType;
				memberOpenIdInfo.OpenId = openId;
				if (MemberProcessor.GetMemberByOpenId(memberOpenIdInfo.OpenIdType, memberOpenIdInfo.OpenId) == null)
				{
					MemberProcessor.AddMemberOpenId(memberOpenIdInfo);
				}
				memberInfo.IsLogined = true;
				memberInfo.UserName = MemberHelper.GetUserName(num2);
				MemberHelper.Update(memberInfo, true);
				Users.SetCurrentUser(memberInfo.UserId, 30, false, false);
				HiContext.Current.User = memberInfo;
				ShoppingCartInfo cookieShoppingCart = ShoppingCartProcessor.GetCookieShoppingCart();
				if (cookieShoppingCart != null)
				{
					ShoppingCartProcessor.ConvertShoppingCartToDataBase(cookieShoppingCart);
					ShoppingCartProcessor.ClearCookieShoppingCart();
				}
				if (!string.IsNullOrEmpty(value))
				{
					HttpCookie httpCookie = new HttpCookie("Token_" + HiContext.Current.UserId.ToString());
					httpCookie.HttpOnly = true;
					httpCookie.Expires = DateTime.Now.AddMinutes(30.0);
					httpCookie.Value = value;
					HttpContext.Current.Response.Cookies.Add(httpCookie);
				}
			}
			return num;
		}

		protected int SkipQQOpenId(HttpContext context)
		{
			int num = 1;
			string openId = Globals.StripAllTags(context.Request["OpenId"].ToNullString());
			string openIdType = Globals.StripAllTags(context.Request["openIdType"].ToNullString());
			string value = Globals.StripAllTags(context.Request["token"].ToNullString());
			Hidistro.Entities.Members.MemberInfo memberInfo = new Hidistro.Entities.Members.MemberInfo();
			int num2 = 0;
			if (HiContext.Current.ReferralUserId > 0)
			{
				memberInfo.ReferralUserId = HiContext.Current.ReferralUserId;
			}
			memberInfo.GradeId = MemberProcessor.GetDefaultMemberGrade();
			string text = Globals.StripAllTags(HttpUtility.UrlDecode(context.Request["real_name"].ToNullString()));
			if (string.IsNullOrEmpty(text))
			{
				HttpCookie httpCookie = HttpContext.Current.Request.Cookies["NickName"];
				if (httpCookie != null)
				{
					text = Globals.StripAllTags(HttpUtility.UrlDecode(httpCookie.Value.ToNullString()));
				}
			}
			memberInfo.NickName = text;
			memberInfo.UserName = text;
			if (string.IsNullOrEmpty(memberInfo.UserName))
			{
				memberInfo.UserName = "tencent" + this.GenerateUsername(8);
			}
			if (MemberProcessor.FindMemberByUsername(memberInfo.UserName) != null)
			{
				memberInfo.UserName = "tencent" + this.GenerateUsername(8);
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
				string pass = this.GeneratePassword();
				string text2 = "Open";
				pass = (memberInfo.Password = Users.EncodePassword(pass, text2));
				memberInfo.PasswordSalt = text2;
				string text4 = Globals.StripAllTags(context.Request["client"].ToNullString());
				if (string.IsNullOrEmpty(text4))
				{
					text4 = "wap";
				}
				text4 = text4.ToLower();
				memberInfo.RegisteredSource = ((text4 == "wap") ? 2 : ((text4 == "alioh") ? 4 : 3));
				memberInfo.CreateDate = DateTime.Now;
				memberInfo.IsLogined = true;
				num2 = MemberProcessor.CreateMember(memberInfo);
				if (num2 <= 0)
				{
					num = -1;
				}
			}
			if (num == 1)
			{
				memberInfo.UserId = num2;
				MemberOpenIdInfo memberOpenIdInfo = new MemberOpenIdInfo();
				memberOpenIdInfo.UserId = memberInfo.UserId;
				memberOpenIdInfo.OpenIdType = openIdType;
				memberOpenIdInfo.OpenId = openId;
				if (MemberProcessor.GetMemberByOpenId(memberOpenIdInfo.OpenIdType, memberOpenIdInfo.OpenId) == null)
				{
					MemberProcessor.AddMemberOpenId(memberOpenIdInfo);
				}
				memberInfo.IsLogined = true;
				memberInfo.UserName = MemberHelper.GetUserName(num2);
				MemberHelper.Update(memberInfo, true);
				Users.SetCurrentUser(memberInfo.UserId, 30, false, false);
				HiContext.Current.User = memberInfo;
				ShoppingCartInfo cookieShoppingCart = ShoppingCartProcessor.GetCookieShoppingCart();
				if (cookieShoppingCart != null)
				{
					ShoppingCartProcessor.ConvertShoppingCartToDataBase(cookieShoppingCart);
					ShoppingCartProcessor.ClearCookieShoppingCart();
				}
				if (!string.IsNullOrEmpty(value))
				{
					HttpCookie httpCookie2 = new HttpCookie("Token_" + HiContext.Current.UserId.ToString());
					httpCookie2.HttpOnly = true;
					httpCookie2.Expires = DateTime.Now.AddMinutes(30.0);
					httpCookie2.Value = value;
					HttpContext.Current.Response.Cookies.Add(httpCookie2);
				}
			}
			return num;
		}

		protected int SkipTaoBaoOpenId(HttpContext context)
		{
			int num = 1;
			string openId = Globals.StripAllTags(context.Request["OpenId"].ToNullString());
			string openIdType = Globals.StripAllTags(context.Request["openIdType"].ToNullString());
			string value = Globals.StripAllTags(context.Request["token"].ToNullString());
			Hidistro.Entities.Members.MemberInfo memberInfo = new Hidistro.Entities.Members.MemberInfo();
			int num2 = 0;
			if (HiContext.Current.ReferralUserId > 0)
			{
				memberInfo.ReferralUserId = HiContext.Current.ReferralUserId;
			}
			memberInfo.GradeId = MemberProcessor.GetDefaultMemberGrade();
			string text = Globals.StripAllTags(HttpUtility.UrlDecode(context.Request["real_name"].ToNullString()));
			if (string.IsNullOrEmpty(text))
			{
				HttpCookie httpCookie = HttpContext.Current.Request.Cookies["NickName"];
				if (httpCookie != null)
				{
					text = Globals.StripAllTags(HttpUtility.UrlDecode(httpCookie.Value.ToNullString()));
				}
			}
			memberInfo.NickName = text;
			memberInfo.UserName = text;
			if (string.IsNullOrEmpty(memberInfo.UserName))
			{
				memberInfo.UserName = "taobao" + this.GenerateUsername(8);
			}
			if (MemberProcessor.FindMemberByUsername(memberInfo.UserName) != null)
			{
				memberInfo.UserName = "taobao" + this.GenerateUsername(8);
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
				string pass = this.GeneratePassword();
				string text2 = "Open";
				pass = (memberInfo.Password = Users.EncodePassword(pass, text2));
				memberInfo.PasswordSalt = text2;
				string text4 = Globals.StripAllTags(context.Request["client"].ToNullString());
				if (string.IsNullOrEmpty(text4))
				{
					text4 = "wap";
				}
				text4 = text4.ToLower();
				memberInfo.RegisteredSource = ((text4 == "wap") ? 2 : ((text4 == "alioh") ? 4 : 3));
				memberInfo.CreateDate = DateTime.Now;
				memberInfo.IsLogined = true;
				num2 = MemberProcessor.CreateMember(memberInfo);
				if (num2 <= 0)
				{
					num = -1;
				}
			}
			if (num == 1)
			{
				memberInfo.UserId = num2;
				MemberOpenIdInfo memberOpenIdInfo = new MemberOpenIdInfo();
				memberOpenIdInfo.UserId = memberInfo.UserId;
				memberOpenIdInfo.OpenIdType = openIdType;
				memberOpenIdInfo.OpenId = openId;
				if (MemberProcessor.GetMemberByOpenId(memberOpenIdInfo.OpenIdType, memberOpenIdInfo.OpenId) == null)
				{
					MemberProcessor.AddMemberOpenId(memberOpenIdInfo);
				}
				memberInfo.IsLogined = true;
				memberInfo.UserName = MemberHelper.GetUserName(num2);
				MemberHelper.Update(memberInfo, true);
				Users.SetCurrentUser(memberInfo.UserId, 30, true, false);
				HiContext.Current.User = memberInfo;
				ShoppingCartInfo cookieShoppingCart = ShoppingCartProcessor.GetCookieShoppingCart();
				if (cookieShoppingCart != null)
				{
					ShoppingCartProcessor.ConvertShoppingCartToDataBase(cookieShoppingCart);
					ShoppingCartProcessor.ClearCookieShoppingCart();
				}
				if (!string.IsNullOrEmpty(value))
				{
					HttpCookie httpCookie2 = new HttpCookie("Token_" + HiContext.Current.UserId.ToString());
					httpCookie2.HttpOnly = true;
					httpCookie2.Expires = DateTime.Now.AddMinutes(30.0);
					httpCookie2.Value = value;
					HttpContext.Current.Response.Cookies.Add(httpCookie2);
				}
			}
			return num;
		}

		protected int SkipSinaOpenId(HttpContext context)
		{
			int num = 1;
			string openId = Globals.StripAllTags(context.Request["OpenId"].ToNullString());
			string openIdType = Globals.StripAllTags(context.Request["openIdType"].ToNullString());
			string value = Globals.StripAllTags(context.Request["token"].ToNullString());
			Hidistro.Entities.Members.MemberInfo memberInfo = new Hidistro.Entities.Members.MemberInfo();
			int num2 = 0;
			if (HiContext.Current.ReferralUserId > 0)
			{
				memberInfo.ReferralUserId = HiContext.Current.ReferralUserId;
			}
			memberInfo.GradeId = MemberProcessor.GetDefaultMemberGrade();
			string text = Globals.StripAllTags(HttpUtility.UrlDecode(context.Request["real_name"].ToNullString()));
			if (string.IsNullOrEmpty(text))
			{
				HttpCookie httpCookie = HttpContext.Current.Request.Cookies["SinaNickName"];
				if (httpCookie != null)
				{
					text = Globals.StripAllTags(HttpUtility.UrlDecode(httpCookie.Value.ToNullString()));
				}
			}
			memberInfo.NickName = text;
			memberInfo.UserName = text;
			if (string.IsNullOrEmpty(memberInfo.UserName))
			{
				memberInfo.UserName = "sinaweibo" + this.GenerateUsername(8);
			}
			if (MemberProcessor.FindMemberByUsername(memberInfo.UserName) != null)
			{
				memberInfo.UserName = "sinaweibo" + this.GenerateUsername(8);
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
				string pass = this.GeneratePassword();
				string text2 = "Open";
				pass = (memberInfo.Password = Users.EncodePassword(pass, text2));
				memberInfo.PasswordSalt = text2;
				string text4 = Globals.StripAllTags(context.Request["client"].ToNullString());
				if (string.IsNullOrEmpty(text4))
				{
					text4 = "wap";
				}
				text4 = text4.ToLower();
				memberInfo.RegisteredSource = ((text4 == "wap") ? 2 : ((text4 == "alioh") ? 4 : 3));
				memberInfo.CreateDate = DateTime.Now;
				memberInfo.IsLogined = true;
				num2 = MemberProcessor.CreateMember(memberInfo);
				if (num2 <= 0)
				{
					num = -1;
				}
			}
			if (num == 1)
			{
				memberInfo.UserId = num2;
				MemberOpenIdInfo memberOpenIdInfo = new MemberOpenIdInfo();
				memberOpenIdInfo.UserId = memberInfo.UserId;
				memberOpenIdInfo.OpenIdType = openIdType;
				memberOpenIdInfo.OpenId = openId;
				if (MemberProcessor.GetMemberByOpenId(memberOpenIdInfo.OpenIdType, memberOpenIdInfo.OpenId) == null)
				{
					MemberProcessor.AddMemberOpenId(memberOpenIdInfo);
				}
				memberInfo.IsLogined = true;
				memberInfo.UserName = MemberHelper.GetUserName(num2);
				MemberHelper.Update(memberInfo, true);
				Users.SetCurrentUser(memberInfo.UserId, 30, false, false);
				HiContext.Current.User = memberInfo;
				ShoppingCartInfo cookieShoppingCart = ShoppingCartProcessor.GetCookieShoppingCart();
				if (cookieShoppingCart != null)
				{
					ShoppingCartProcessor.ConvertShoppingCartToDataBase(cookieShoppingCart);
					ShoppingCartProcessor.ClearCookieShoppingCart();
				}
				if (!string.IsNullOrEmpty(value))
				{
					HttpCookie httpCookie2 = new HttpCookie("Token_" + HiContext.Current.UserId.ToString());
					httpCookie2.HttpOnly = true;
					httpCookie2.Expires = DateTime.Now.AddMinutes(30.0);
					httpCookie2.Value = value;
					HttpContext.Current.Response.Cookies.Add(httpCookie2);
				}
			}
			return num;
		}

		protected int SkipWeixinOpenId(HttpContext context, out bool isNewRegisterUser)
		{
			isNewRegisterUser = true;
			int num = 1;
			string text = Globals.StripAllTags(context.Request["openId"].ToNullString().ToNullString());
			string text2 = Globals.StripAllTags(context.Request["real_name"].ToNullString().ToNullString());
			string text3 = Globals.StripAllTags(context.Request["unionId"].ToNullString().ToNullString());
			bool flag = context.Request["IsSubscribe"].ToBool();
			Hidistro.Entities.Members.MemberInfo memberInfo = MemberProcessor.GetMemberByOpenIdOfQuickLogin("hishop.plugins.openid.weixin", text);
			bool flag2 = false;
			if (memberInfo == null)
			{
				memberInfo = MemberProcessor.GetMemberByUnionId(text3);
				flag2 = true;
			}
			string text4 = context.Request["token"].ToNullString();
			string text5 = Globals.StripAllTags(Globals.UrlDecode(context.Request["HeadImage"].ToNullString()));
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			ShoppingCartInfo cookieShoppingCart = ShoppingCartProcessor.GetCookieShoppingCart();
			bool flag3 = false;
			if (memberInfo != null)
			{
				memberInfo.IsLogined = true;
				if (memberInfo.IsSubscribe != flag && context.Request["IsSubscribe"] != null)
				{
					memberInfo.IsSubscribe = flag;
					flag3 = true;
				}
				bool flag4 = MemberProcessor.IsBindedWeixin(memberInfo.UserId, "hishop.plugins.openid.weixin");
				if (!string.IsNullOrEmpty(text5) || text5.StartsWith("http://wx.qlogo.cn/mmopen/"))
				{
					memberInfo.Picture = text5;
				}
				if (!string.IsNullOrEmpty(text3) && memberInfo.UnionId != text3 && !flag2 && MemberProcessor.GetMemberByUnionId(text3) == null)
				{
					memberInfo.UnionId = text3;
					flag3 = true;
				}
				if (flag2)
				{
					if (!flag4)
					{
						MemberOpenIdInfo memberOpenIdInfo = new MemberOpenIdInfo();
						memberOpenIdInfo.UserId = memberInfo.UserId;
						memberOpenIdInfo.OpenIdType = "hishop.plugins.openid.weixin";
						memberOpenIdInfo.OpenId = text;
						MemberProcessor.AddMemberOpenId(memberOpenIdInfo);
						memberInfo.IsQuickLogin = true;
						memberInfo.IsDefaultDevice = true;
						flag3 = true;
					}
					else
					{
						MemberOpenIdInfo memberOpenIdInfo2 = new MemberOpenIdInfo();
						memberOpenIdInfo2.UserId = memberInfo.UserId;
						memberOpenIdInfo2.OpenIdType = "hishop.plugins.openid.weixin";
						memberOpenIdInfo2.OpenId = text;
						MemberProcessor.UpdateMemberOpenId(memberOpenIdInfo2);
					}
				}
				if (flag3)
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
				if (!string.IsNullOrEmpty(text))
				{
					HttpCookie httpCookie = new HttpCookie("openId");
					httpCookie.HttpOnly = true;
					httpCookie.Value = text;
					httpCookie.Expires = DateTime.MaxValue;
					HttpContext.Current.Response.Cookies.Add(httpCookie);
				}
				lock (this.lockCopyRedEnvelope)
				{
					this.CopyRedEnvelope(text, memberInfo);
				}
				isNewRegisterUser = false;
				return num;
			}
			if (masterSettings.QuickLoginIsForceBindingMobbile)
			{
				return -2;
			}
			memberInfo = new Hidistro.Entities.Members.MemberInfo();
			memberInfo.IsLogined = true;
			memberInfo.IsSubscribe = flag;
			if (!string.IsNullOrEmpty(text5) || text5.StartsWith("http://wx.qlogo.cn/mmopen/"))
			{
				memberInfo.Picture = text5;
			}
			int num2 = 0;
			if (HiContext.Current.ReferralUserId > 0)
			{
				memberInfo.ReferralUserId = HiContext.Current.ReferralUserId;
			}
			else if (!string.IsNullOrEmpty(text))
			{
				MemberWXReferralInfo wXReferral = VShopHelper.GetWXReferral(text.Trim());
				if (wXReferral != null)
				{
					memberInfo.ReferralUserId = wXReferral.ReferralUserId;
					VShopHelper.DeleteWXReferral(text.Trim());
				}
			}
			if (HiContext.Current.ShoppingGuiderId > 0)
			{
				memberInfo.ShoppingGuiderId = HiContext.Current.ShoppingGuiderId;
			}
			else if (!string.IsNullOrEmpty(text))
			{
				MemberWXShoppingGuiderInfo memberWXShoppingGuider = MemberHelper.GetMemberWXShoppingGuider(text.Trim());
				if (memberWXShoppingGuider != null)
				{
					memberInfo.ShoppingGuiderId = memberWXShoppingGuider.ShoppingGuiderId;
					MemberHelper.DeleteWXShoppingGuider(text.Trim());
				}
			}
			memberInfo.GradeId = MemberProcessor.GetDefaultMemberGrade();
			if (!string.IsNullOrEmpty(text2))
			{
				Hidistro.Entities.Members.MemberInfo memberInfo2 = memberInfo;
				Hidistro.Entities.Members.MemberInfo memberInfo3 = memberInfo;
				string text8 = memberInfo2.UserName = (memberInfo3.NickName = HttpUtility.UrlDecode(text2));
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
				string pass = this.GeneratePassword();
				string text9 = "Open";
				pass = (memberInfo.Password = Users.EncodePassword(pass, text9));
				memberInfo.PasswordSalt = text9;
				memberInfo.RegisteredSource = 3;
				memberInfo.CreateDate = DateTime.Now;
				memberInfo.IsQuickLogin = true;
				memberInfo.IsDefaultDevice = true;
				memberInfo.IsLogined = true;
				memberInfo.UnionId = text3;
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
				if (cookieShoppingCart != null)
				{
					ShoppingCartProcessor.ConvertShoppingCartToDataBase(cookieShoppingCart);
					ShoppingCartProcessor.ClearCookieShoppingCart();
				}
				if (!string.IsNullOrEmpty(text))
				{
					MemberOpenIdInfo memberOpenIdInfo3 = new MemberOpenIdInfo();
					memberOpenIdInfo3.UserId = memberInfo.UserId;
					memberOpenIdInfo3.OpenIdType = "hishop.plugins.openid.weixin";
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
					lock (this.lockCopyRedEnvelope)
					{
						this.CopyRedEnvelope(text, memberInfo);
					}
				}
			}
			return num;
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

		private void GetWXShareInfo(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			string jsApiTicket = this.GetJsApiTicket(true);
			string text = this.GenerateNonceStr();
			string text2 = this.GenerateTimeStamp();
			string url = context.Request.UrlReferrer.ToString();
			string signature = this.GetSignature(jsApiTicket, text, text2, url);
			context.Response.Write("{\"timestamp\":\"" + text2 + "\",\"noncestr\":\"" + text + "\",\"signature\":\"" + signature + "\"}");
			context.Response.End();
		}

		private string GetJsApiTicket(bool first = true)
		{
			string accessToken = this.GetAccessToken(!first);
			string result = string.Empty;
			string format = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi";
			format = string.Format(format, accessToken);
			try
			{
				string responseResult = this.GetResponseResult(format);
				if (responseResult.Contains("ticket"))
				{
					JObject jObject = JsonConvert.DeserializeObject(responseResult) as JObject;
					result = jObject["ticket"].ToString();
				}
				else
				{
					Globals.AppendLog(responseResult, accessToken, "", "GetJsApiTicket");
					if (responseResult.Contains("access_token is invalid or not latest") & first)
					{
						return this.GetJsApiTicket(false);
					}
				}
			}
			catch (Exception ex)
			{
				IDictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("access_token", accessToken);
				dictionary.Add("jsapi_ticketurl", format);
				Globals.WriteExceptionLog(ex, null, "GetJsApiTicketEx");
			}
			return result;
		}

		private string GetAccessToken(bool getNew = true)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			string format = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";
			format = string.Format(format, masterSettings.WeixinAppId, masterSettings.WeixinAppSecret);
			string empty = string.Empty;
			try
			{
				return AccessTokenContainer.TryGetToken(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, getNew);
			}
			catch (Exception ex)
			{
				IDictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("access_tokenurl", format);
				Globals.WriteExceptionLog(ex, null, "GetAccessTokenEx");
			}
			return empty;
		}

		private string GetResponseResult(string url)
		{
			ServicePointManager.ServerCertificateValidationCallback = VshopProcess.CheckValidationResult;
			WebRequest webRequest = WebRequest.Create(url);
			using (HttpWebResponse httpWebResponse = (HttpWebResponse)webRequest.GetResponse())
			{
				using (Stream stream = httpWebResponse.GetResponseStream())
				{
					using (StreamReader streamReader = new StreamReader(stream, Encoding.UTF8))
					{
						return streamReader.ReadToEnd();
					}
				}
			}
		}

		private string GenerateNonceStr()
		{
			return Guid.NewGuid().ToString().Replace("-", "");
		}

		private string GenerateTimeStamp()
		{
			return Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds).ToString();
		}

		private string GetSignature(string jsapiticket, string noncestr, string timestamp, string url)
		{
			string s = $"jsapi_ticket={jsapiticket}&noncestr={noncestr}&timestamp={timestamp}&url={url}";
			SHA1 sHA = new SHA1CryptoServiceProvider();
			byte[] bytes = Encoding.Default.GetBytes(s);
			byte[] value = sHA.ComputeHash(bytes);
			string text = BitConverter.ToString(value);
			return text.Replace("-", "").ToLower();
		}

		private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
		{
			return true;
		}
	}
}
