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
using Hidistro.Messages;
using Hidistro.SaleSystem;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Members;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Shopping;
using Hidistro.SaleSystem.Store;
using Hidistro.SaleSystem.Supplier;
using Hidistro.SaleSystem.Vshop;
using Hidistro.SqlDal.Members;
using Hishop.Plugins;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;

namespace Hidistro.UI.Web.AppShop
{
	public class AppShopHandler : IHttpHandler, IRequiresSessionState
	{
		private static Random r = new Random();

		private IDictionary<string, string> jsondict = new Dictionary<string, string>();

		private SiteSettings siteSettings = SettingsManager.GetMasterSettings();

		private Regex emailR = new Regex("^\\w+((-\\w+)|(\\.\\w+))*\\@[A-Za-z0-9]+((\\.|-)[A-Za-z0-9]+)*\\.[A-Za-z0-9]+$", RegexOptions.Compiled);

		private Regex cellphoneR = new Regex("^0?(13|15|18|14|17)[0-9]{9}$", RegexOptions.Compiled);

		private string message = string.Empty;

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		private void CheckSession(HttpContext context)
		{
			string text = context.Request["sessionid"].ToNullString();
			if (string.IsNullOrEmpty(text) || !this.ValidLoginBySessionID(text))
			{
				this.message = JsonConvert.SerializeObject(new
				{
					ErrorResponse = new
					{
						ErrorCode = 107,
						ErrorMsg = "登录超时，请重新登录"
					}
				});
				context.Response.Write(this.message);
				context.Response.End();
			}
		}

		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			string text = context.Request["action"];
			switch (text)
			{
			case "ProductConsultations":
				this.ProductConsultations(context);
				break;
			case "Favorites":
				this.Favorites(context);
				break;
			case "DeleteFavorites":
				this.DeleteFavorites(context);
				break;
			case "CloseOrder":
				this.CloseOrder(context);
				break;
			case "Orders":
				this.Orders(context);
				break;
			case "AdvanceOpen":
				this.AdvanceOpen(context);
				break;
			case "AdvanceInfo":
				this.AdvanceInfo(context);
				break;
			case "AdvanceDetails":
				this.AdvanceDetails(context);
				break;
			case "AppPushListForUser":
				this.AppPushListForUser(context);
				break;
			case "AppPushSetRead":
				this.AppPushSetRead(context);
				break;
			case "GetCoupon":
				this.UserGetCoupon(context);
				break;
			case "appInit":
				this.ProcessAppInit(context);
				break;
			case "getDefaultData":
				this.GetDefaultData(context);
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
			case "regiester":
				this.ProcessRegiester(context);
				break;
			case "login":
				this.ProcessLogin(context);
				break;
			case "logout":
				this.ProcessLogout(context);
				break;
			case "getMember":
				this.GetMember(context);
				break;
			case "hasBind":
				this.HasBind(context);
				break;
			case "unBind":
				this.UnBind(context);
				break;
			case "getShoppingCart":
				this.GetShoppingCart(context);
				break;
			case "addToCart":
				this.AddToCart(context);
				break;
			case "delCartItem":
				this.DelCartItem(context);
				break;
			case "getProductDetail":
				this.GetProductDetail(context);
				break;
			case "getStoreProductDetail":
				this.GetStoreProductDetail(context);
				break;
			case "getStartImg":
				this.GetStartImg(context);
				break;
			case "getDepletePoints":
				this.GetDepletePoints(context);
				break;
			case "signIn":
				this.SignIn(context);
				break;
			case "lotteryDraw":
				this.LotteryDraw(context);
				break;
			case "AddProductToFavorite":
				this.AddProductToFavorite(context);
				break;
			case "DeleteFavorite":
				this.DeleteFavorite(context);
				break;
			case "SendVerifyCode":
				this.AppSendVerifyCode(context);
				break;
			case "SendEmailVerifyCode":
				this.SendEmailVerifyCode(context);
				break;
			case "GetSMSEnabledState":
				this.GetSMSEnabledState(context);
				break;
			case "VerficationPhoneOrEmail":
				this.VerficationPhoneOrEmail(context);
				break;
			case "ChangePassword":
				this.ChangePassword(context);
				break;
			case "ChangeTranPassword":
				this.ChangeTranPassword(context);
				break;
			case "BindPhoneOrEmail":
				this.BindPhoneOrEmail(context);
				break;
			case "VerficationPhoneOrEmailNoValid":
				this.VerficationPhoneOrEmailNoValid(context);
				break;
			case "GetPointList":
				this.GetPointList(context);
				break;
			case "ValidVerfication":
				this.ValidVerfication(context);
				break;
			case "GetCellPhoneAndEmail":
				this.GetCellPhoneAndEmailByUser(context);
				break;
			case "FinishOrder":
				this.FinishOrder(context);
				break;
			case "FightGroupActivityList":
				this.FightGroupActivityList(context);
				break;
			case "FightGroupActivityDetail":
				this.FightGroupActivityDetail(context);
				break;
			case "FightGroupDetail":
				this.FightGroupDetail(context);
				break;
			case "FightGroupShare":
				this.FightGroupShare(context);
				break;
			case "MyFightGroupList":
				this.MyFightGroupList(context);
				break;
			case "GetPreSaleProductDetail":
				this.GetPreSaleProductDetail(context);
				break;
			case "GetProductConsultation":
				this.GetProductConsultation(context);
				break;
			case "SaveClientIdAndToken":
				this.SaveClientIdAndToken(context);
				break;
			case "GetStoreHomePageBaseInfo":
				this.GetStoreHomePageBaseInfo(context);
				break;
			case "GetStoreHomePageFloorList":
				this.GetStoreHomePageFloorList(context);
				break;
			case "GetMarketingImageBaseInfo":
				this.GetMarketingImageBaseInfo(context);
				break;
			case "GetStoreMarketingProducts":
				this.GetStoreMarketingProducts(context);
				break;
			case "GetStoreListBaners":
				this.GetStoreListBaners(context);
				break;
			case "GetStoreListTags":
				this.GetStoreListTags(context);
				break;
			case "GetStoreList":
				this.GetStoreList(context);
				break;
			case "SearchInStoreList":
				this.SearchInStoreList(context);
				break;
			case "GetPositionPageTurnParam":
				this.GetPositionPageTurnParam(context);
				break;
			case "GetNearestStore":
				this.GetNearestStore(context);
				break;
			case "GetUserShippingAddress":
				this.GetUserShippingAddress(context);
				break;
			case "GetNearAddress":
				this.GetNearAddress(context);
				break;
			case "GetRegionList":
				AppShopHandler.GetRegions(context);
				break;
			case "GetProductSkus":
				this.GetProductSkus(context);
				break;
			case "GetRedEnvelopeShareInfo":
				this.GetRedEnvelopeShareInfo(context);
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
			MemberInfo memberInfo = null;
			string text = context.Request["unionId"].ToNullString();
			string text2 = context.Request["oauthType"].ToNullString();
			string text3 = context.Request["oauthOpenId"].ToNullString();
			string text4 = context.Request["oauthNickName"].ToNullString();
			string text5 = context.Request["oauthAvatar"].ToNullString();
			string userName = context.Request["username"].ToNullString();
			string password = context.Request["password"].ToNullString();
			string text6 = context.Request["from"].ToNullString();
			text2 = text2.ToLower();
			if (text2 == "weixin")
			{
				text2 = "hishop.plugins.openid.appweixin";
			}
			if (text2.ToLower() == "qq")
			{
				text2 = "hishop.plugins.openid.qq.appqqservicet";
			}
			if (text2.ToLower() == "sina" || text2.ToLower() == "weibo" || text2.ToLower() == "sinaweibo")
			{
				text2 = "hishop.plugins.openid.sina.appsinaservice";
			}
			ApiErrorCode apiErrorCode;
			if (string.IsNullOrEmpty(text3) || string.IsNullOrEmpty(text2))
			{
				apiErrorCode = ApiErrorCode.Paramter_Error;
				this.ShowMessageAndCode(context, "缺少必填参数", apiErrorCode.GetHashCode());
			}
			else
			{
				memberInfo = ((!(text2 == "hishop.plugins.openid.weixin")) ? MemberProcessor.GetMemberByOpenId(text2, text3) : MemberProcessor.GetMemberByOpenIdOfQuickLogin(text2, text3));
				if (memberInfo == null && !string.IsNullOrEmpty(text))
				{
					memberInfo = MemberProcessor.GetMemberByUnionId(text);
				}
				MemberInfo memberInfo2 = MemberProcessor.ValidLogin(userName, password);
				if (memberInfo2 == null)
				{
					apiErrorCode = ApiErrorCode.Password_Error;
					this.ShowMessageAndCode(context, "用户名或者密码错误", apiErrorCode.GetHashCode());
				}
				else
				{
					MemberOpenIdInfo memberOpenIdInfo = MemberProcessor.GetMemberOpenIdInfo(memberInfo2.UserId, text2);
					if (memberOpenIdInfo != null && memberOpenIdInfo.OpenId != text3)
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
						memberInfo2.UnionId = text;
						memberInfo2.SessionId = Guid.NewGuid().ToString("N");
						MemberProcessor.UpdateMember(memberInfo2);
						if (!string.IsNullOrEmpty(text3) && memberOpenIdInfo == null)
						{
							MemberOpenIdInfo memberOpenIdInfo2 = new MemberOpenIdInfo();
							memberOpenIdInfo2.UserId = memberInfo2.UserId;
							memberOpenIdInfo2.OpenIdType = text2;
							memberOpenIdInfo2.OpenId = text3;
							MemberProcessor.AddMemberOpenId(memberOpenIdInfo2);
						}
						Users.ClearUserCache(memberInfo2.UserId, "");
						Users.SetCurrentUser(memberInfo2.UserId, 30, false, false);
						HiContext.Current.User = memberInfo2;
						HttpCookie httpCookie = new HttpCookie("openId");
						httpCookie.HttpOnly = true;
						httpCookie.Value = text3;
						httpCookie.Expires = DateTime.MaxValue;
						HttpContext.Current.Response.Cookies.Add(httpCookie);
						this.GetMember(context, memberInfo2);
					}
				}
			}
		}

		private void CellPhoneRegister(HttpContext context)
		{
			MemberInfo memberInfo = null;
			int referralUserId = context.Request["referralUserId"].ToInt(0);
			string text = context.Request["cellphone"].ToNullString();
			string verifyCode = context.Request["verifyCode"].ToNullString();
			string text2 = context.Request["password"].ToNullString();
			string text3 = context.Request["from"].ToNullString();
			string text4 = context.Request["unionId"].ToNullString();
			string text5 = context.Request["oauthType"].ToNullString();
			string text6 = context.Request["oauthOpenId"].ToNullString();
			string text7 = context.Request["oauthNickName"].ToNullString();
			string text8 = context.Request["oauthAvatar"].ToNullString();
			string text9 = context.Request["username"].ToNullString();
			text5 = text5.ToLower();
			if (text5 == "weixin")
			{
				text5 = "hishop.plugins.openid.appweixin";
			}
			if (text5.ToLower() == "qq")
			{
				text5 = "hishop.plugins.openid.qq.appqqservicet";
			}
			if (text5.ToLower() == "sina" || text5.ToLower() == "weibo" || text5.ToLower() == "sinaweibo")
			{
				text5 = "hishop.plugins.openid.sina.appsinaservice";
			}
			ApiErrorCode apiErrorCode;
			if (string.IsNullOrEmpty(text6) || string.IsNullOrEmpty(text5))
			{
				apiErrorCode = ApiErrorCode.Paramter_Error;
				this.ShowMessageAndCode(context, "缺少必填参数", apiErrorCode.GetHashCode());
			}
			else if (text2.Length < 6 || text2.Length > 20)
			{
				apiErrorCode = ApiErrorCode.Paramter_Error;
				this.ShowMessageAndCode(context, "请输入用户密码，密码长度为6-20位", apiErrorCode.GetHashCode());
			}
			else if (string.IsNullOrEmpty(text) || !DataHelper.IsMobile(text))
			{
				apiErrorCode = ApiErrorCode.Paramter_Error;
				this.ShowMessageAndCode(context, "请输入正确的手机号码", apiErrorCode.GetHashCode());
			}
			else
			{
				string text10 = "";
				if (!HiContext.Current.CheckPhoneVerifyCode(verifyCode, text, out text10))
				{
					apiErrorCode = ApiErrorCode.Paramter_Error;
					this.ShowMessageAndCode(context, "手机验证码错误", apiErrorCode.GetHashCode());
				}
				else if (MemberProcessor.FindMemberByCellphone(text) != null)
				{
					apiErrorCode = ApiErrorCode.MobbileIsBinding;
					this.ShowMessageAndCode(context, "手机号已被其它帐号绑定", apiErrorCode.GetHashCode());
				}
				else
				{
					memberInfo = ((!(text5 == "hishop.plugins.openid.weixin")) ? MemberProcessor.GetMemberByOpenId(text5, text6) : MemberProcessor.GetMemberByOpenIdOfQuickLogin(text5, text6));
					bool flag = false;
					if (memberInfo == null)
					{
						memberInfo = MemberProcessor.GetMemberByUnionId(text4);
						if (memberInfo != null)
						{
							flag = true;
						}
					}
					Guid guid;
					if (memberInfo?.CellPhoneVerification ?? false)
					{
						apiErrorCode = ApiErrorCode.MobbileIsBinding;
						this.ShowMessageAndCode(context, "帐号已经绑定了手机号码", apiErrorCode.GetHashCode());
					}
					else if (memberInfo != null && memberInfo.Password != Users.EncodePassword(text2, memberInfo.PasswordSalt))
					{
						apiErrorCode = ApiErrorCode.Password_Error;
						this.ShowMessageAndCode(context, "用户密码错误", apiErrorCode.GetHashCode());
					}
					else if (memberInfo != null)
					{
						bool flag2 = MemberProcessor.IsBindedWeixin(memberInfo.UserId, text5);
						if (text5 != "hishop.plugins.openid.weixin" & flag2)
						{
							apiErrorCode = ApiErrorCode.ExistTrustLogin;
							this.ShowMessageAndCode(context, "信任登录与其它用户关联", apiErrorCode.GetHashCode());
						}
						else
						{
							memberInfo.CellPhone = text;
							memberInfo.CellPhoneVerification = true;
							if (text5 == "hishop.plugins.openid.weixin")
							{
								memberInfo.IsQuickLogin = true;
							}
							memberInfo.IsQuickLogin = true;
							memberInfo.IsLogined = true;
							MemberInfo memberInfo2 = memberInfo;
							guid = Guid.NewGuid();
							memberInfo2.SessionId = guid.ToString("N");
							MemberProcessor.UpdateMember(memberInfo);
							if (!string.IsNullOrEmpty(text8) || text8.StartsWith("http://wx.qlogo.cn/mmopen/"))
							{
								memberInfo.Picture = text8;
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
									memberOpenIdInfo.OpenIdType = text5;
									memberOpenIdInfo.OpenId = text6;
									MemberProcessor.AddMemberOpenId(memberOpenIdInfo);
									memberInfo.IsQuickLogin = true;
								}
								else
								{
									MemberOpenIdInfo memberOpenIdInfo2 = new MemberOpenIdInfo();
									memberOpenIdInfo2.UserId = memberInfo.UserId;
									memberOpenIdInfo2.OpenIdType = text5;
									memberOpenIdInfo2.OpenId = text6;
									MemberProcessor.UpdateMemberOpenId(memberOpenIdInfo2);
								}
							}
							MemberProcessor.UpdateMember(memberInfo);
							if (!string.IsNullOrEmpty(text6))
							{
								HttpCookie httpCookie = new HttpCookie("openId");
								httpCookie.HttpOnly = true;
								httpCookie.Value = text6;
								httpCookie.Expires = DateTime.MaxValue;
								HttpContext.Current.Response.Cookies.Add(httpCookie);
							}
							Users.ClearUserCache(memberInfo.UserId, "");
							Users.SetCurrentUser(memberInfo.UserId, 30, false, false);
							HiContext.Current.User = memberInfo;
							this.GetMember(context, memberInfo);
						}
					}
					else
					{
						int num = 1;
						memberInfo = new MemberInfo();
						memberInfo.IsLogined = true;
						if (!string.IsNullOrEmpty(text8) || text8.StartsWith("http://wx.qlogo.cn/mmopen/"))
						{
							memberInfo.Picture = text8;
						}
						int num2 = 0;
						memberInfo.GradeId = MemberProcessor.GetDefaultMemberGrade();
						memberInfo.UserName = text;
						memberInfo.CellPhone = text;
						memberInfo.CellPhoneVerification = true;
						if (!string.IsNullOrEmpty(text7))
						{
							memberInfo.NickName = HttpUtility.UrlDecode(text7);
						}
						memberInfo.ReferralUserId = referralUserId;
						string text11 = "Open";
						text2 = (memberInfo.Password = Users.EncodePassword(text2, text11));
						memberInfo.PasswordSalt = text11;
						memberInfo.RegisteredSource = 5;
						memberInfo.CreateDate = DateTime.Now;
						if (text5 == "hishop.plugins.openid.weixin")
						{
							memberInfo.IsQuickLogin = true;
						}
						memberInfo.IsLogined = true;
						memberInfo.UnionId = text4;
						MemberInfo memberInfo3 = memberInfo;
						guid = Guid.NewGuid();
						memberInfo3.SessionId = guid.ToString("N");
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
							if (!string.IsNullOrEmpty(text6))
							{
								MemberOpenIdInfo memberOpenIdInfo3 = new MemberOpenIdInfo();
								memberOpenIdInfo3.UserId = memberInfo.UserId;
								memberOpenIdInfo3.OpenIdType = text5;
								memberOpenIdInfo3.OpenId = text6;
								if (MemberProcessor.GetMemberByOpenId(memberOpenIdInfo3.OpenIdType, text6) == null)
								{
									MemberProcessor.AddMemberOpenId(memberOpenIdInfo3);
								}
								if (!string.IsNullOrEmpty(text6))
								{
									HttpCookie httpCookie2 = new HttpCookie("openId");
									httpCookie2.HttpOnly = true;
									httpCookie2.Value = text6;
									httpCookie2.Expires = DateTime.MaxValue;
									HttpContext.Current.Response.Cookies.Add(httpCookie2);
								}
							}
							Users.ClearUserCache(memberInfo.UserId, "");
							Users.SetCurrentUser(memberInfo.UserId, 30, false, false);
							HiContext.Current.User = memberInfo;
						}
						this.GetMember(context, memberInfo);
					}
				}
			}
		}

		public void ShowMessageAndCode(HttpContext context, string msg, int msgCode)
		{
			context.Response.Write("{\"Status\":\"" + msgCode + "\",\"msg\":\"" + msg + "\"}");
		}

		public void GetRedEnvelopeShareInfo(HttpContext context)
		{
			string text = context.Request["OrderId"];
			OrderInfo orderInfo = TradeHelper.GetOrderInfo(text);
			WeiXinRedEnvelopeInfo openedWeiXinRedEnvelope = WeiXinRedEnvelopeProcessor.GetOpenedWeiXinRedEnvelope();
			string text2 = "";
			if (orderInfo == null)
			{
				text2 = "错误的订单信息";
			}
			else if (openedWeiXinRedEnvelope == null)
			{
				text2 = "没有找到任何红包活动";
			}
			else
			{
				if (openedWeiXinRedEnvelope.ActiveStartTime > DateTime.Now)
				{
					text2 = "红包活动还没有开始";
				}
				if (openedWeiXinRedEnvelope.ActiveEndTime < DateTime.Now)
				{
					text2 = "红包活动已经过期";
				}
				decimal amount = orderInfo.GetAmount(false);
				if (amount < openedWeiXinRedEnvelope.EnableIssueMinAmount)
				{
					text2 = "你没有满足发红包的金额条件";
				}
			}
			if (!string.IsNullOrEmpty(text2))
			{
				string s = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						Status = "ERROR",
						Msg = text2
					}
				});
				context.Response.Write(s);
				context.Response.End();
			}
			else
			{
				RedEnvelopeSendRecord redEnvelopeSendRecord = WeiXinRedEnvelopeProcessor.GetRedEnvelopeSendRecord(text, openedWeiXinRedEnvelope.Id.ToString());
				int num;
				if (redEnvelopeSendRecord != null)
				{
					Guid sendCode = redEnvelopeSendRecord.SendCode;
					num = 0;
				}
				else
				{
					num = 1;
				}
				Guid guid;
				string text3;
				if (num != 0)
				{
					guid = Guid.NewGuid();
					text3 = guid.ToString();
				}
				else
				{
					guid = redEnvelopeSendRecord.SendCode;
					text3 = guid.ToString();
				}
				string s2 = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						Status = "SUCCESS",
						Msg = "",
						RedEnvelopeId = openedWeiXinRedEnvelope.Id,
						Title = openedWeiXinRedEnvelope.Name,
						Descrption = openedWeiXinRedEnvelope.ShareDetails,
						ImageUrl = Globals.FullPath(openedWeiXinRedEnvelope.ShareIcon),
						SendCode = text3,
						LinkUrl = Globals.FullPath("/Vshop/GetRedEnvelope?SendCode=" + text3 + "&OrderId=" + text)
					}
				});
				context.Response.Write(s2);
				context.Response.End();
			}
		}

		private void ChangeTranPassword(HttpContext context)
		{
			string text = context.Request["password"].ToNullString();
			string pass = context.Request["oldPassword"].ToNullString();
			MemberInfo user = Users.GetUser(HiContext.Current.UserId);
			if (user == null)
			{
				string s = JsonConvert.SerializeObject(new
				{
					Success = new
					{
						Status = "nologined",
						Msg = "请您先登录"
					}
				});
				context.Response.Write(s);
			}
			else if (user.TradePasswordSalt.ToLower() != "open" && user.TradePassword != Users.EncodePassword(pass, user.TradePasswordSalt))
			{
				string s2 = JsonConvert.SerializeObject(new
				{
					Success = new
					{
						Status = "erroldpwd",
						Msg = "原始交易密码不正确"
					}
				});
				context.Response.Write(s2);
			}
			else if (text.Length < 6 || text.Length > 20)
			{
				string s3 = JsonConvert.SerializeObject(new
				{
					Success = new
					{
						Status = "errpwd",
						Msg = "交易密码必须在6-20个字符之间！"
					}
				});
				context.Response.Write(s3);
			}
			else if (MemberProcessor.ChangeTradePassword(user, text))
			{
				Users.ClearUserCache(HiContext.Current.UserId, HiContext.Current.User.SessionId);
				Messenger.UserDealPasswordChanged(user, text);
				string s4 = JsonConvert.SerializeObject(new
				{
					Success = new
					{
						Status = "ok",
						Msg = "修改交易密码成功"
					}
				});
				context.Response.Write(s4);
			}
			else
			{
				string s5 = JsonConvert.SerializeObject(new
				{
					Success = new
					{
						Status = "unknow",
						Msg = "修改交易密码失败"
					}
				});
				context.Response.Write(s5);
			}
		}

		private void GetProductSkus(HttpContext context)
		{
			bool flag = false;
			int num = context.Request["ProductId"].ToInt(0);
			int num2 = context.Request["StoreId"].ToInt(0);
			if (num <= 0)
			{
				context.Response.Write(this.GetErrorJosn(101, "商品已下架或者已删除"));
			}
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			int gradeId = 0;
			if (HiContext.Current != null && HiContext.Current.User != null && HiContext.Current.User.UserId > 0)
			{
				gradeId = HiContext.Current.User.GradeId;
			}
			string text = "";
			if (num2 == 0)
			{
				ProductPreSaleInfo productPreSaleInfoByProductId = ProductPreSaleHelper.GetProductPreSaleInfoByProductId(num);
				if (productPreSaleInfoByProductId != null)
				{
					flag = true;
					text = "PreSaleproductdetails?PreSaleId=" + productPreSaleInfoByProductId.PreSaleId;
				}
			}
			if (string.IsNullOrEmpty(text))
			{
				CountDownInfo countDownInfo = PromoteHelper.ActiveCountDownByProductId(num, num2);
				if (countDownInfo != null)
				{
					text = ((num2 != 0) ? $"CountDownStoreProductsDetails?countDownId={countDownInfo.CountDownId}&StoreId={num2}" : $"CountDownProductsDetails?countDownId={countDownInfo.CountDownId}");
				}
			}
			if (string.IsNullOrEmpty(text) && num2 == 0)
			{
				GroupBuyInfo groupBuyInfo = null;
				groupBuyInfo = PromoteHelper.ActiveGroupBuyByProductId(num);
				if (groupBuyInfo != null)
				{
					text = $"GroupBuyProductDetails?GroupBuyId={groupBuyInfo.GroupBuyId}";
				}
			}
			SKUItem sKUItem = new SKUItem();
			int fightGroupActivityId = 0;
			if (string.IsNullOrEmpty(text) && num2 == 0)
			{
				FightGroupActivitiyModel fightGroupActivitiyModel = VShopHelper.GetFightGroupActivities(new FightGroupActivitiyQuery
				{
					PageIndex = 1,
					PageSize = 1,
					ProductId = num,
					Status = EnumFightGroupActivitiyStatus.BeingCarried
				}).Models.FirstOrDefault();
				if (fightGroupActivitiyModel != null)
				{
					fightGroupActivityId = fightGroupActivitiyModel.FightGroupActivityId;
				}
			}
			ProductInfo productSimpleInfo = ProductBrowser.GetProductSimpleInfo(num);
			if (productSimpleInfo.ProductType == 1.GetHashCode())
			{
				text = $"ServiceProductDetails?StoreId={num2}&ProductId={num}";
			}
			DetailException ex = DetailException.Nomal;
			ProductModel productModel = new ProductModel();
			if (string.IsNullOrEmpty(text))
			{
				productModel = ProductBrowser.GetProductSkus(num, gradeId, masterSettings.OpenMultStore, num2);
				if (productModel == null || productModel.Skus == null || productModel.Skus.Count == 0 || (num2 == 0 && productModel.SaleStatus != ProductSaleStatus.OnSale))
				{
					context.Response.Write(this.GetErrorJosn(101, "商品已下架或者已删除"));
					context.Response.End();
				}
				else
				{
					sKUItem = productModel.Skus.FirstOrDefault();
				}
				if (num2 > 0)
				{
					StoresInfo storeById = StoresHelper.GetStoreById(num2);
					if (storeById == null)
					{
						ex = DetailException.StopService;
					}
					else if (!storeById.CloseStatus && storeById.CloseEndTime.HasValue && storeById.CloseBeginTime.HasValue && storeById.CloseEndTime.Value > DateTime.Now && storeById.CloseBeginTime.Value < DateTime.Now)
					{
						ex = DetailException.StopService;
					}
					else if (productModel.Skus.Sum((SKUItem s) => s.Stock) <= 0)
					{
						ex = DetailException.NoStock;
					}
					else if (!SettingsManager.GetMasterSettings().Store_IsOrderInClosingTime)
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
							ex = DetailException.IsNotWorkTime;
						}
					}
				}
			}
			else
			{
				productModel = new ProductModel();
				productModel.SkuTable = null;
				productModel.Skus = new List<SKUItem>();
			}
			IList<SKUItem> list = new List<SKUItem>();
			IList<SkuItem> list2 = new List<SkuItem>();
			if (productModel.SkuTable != null && productModel.SkuTable.Rows.Count > 0)
			{
				foreach (DataRow row in productModel.SkuTable.Rows)
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
						foreach (DataRow row2 in productModel.SkuTable.Rows)
						{
							if (string.Compare((string)row["AttributeName"], (string)row2["AttributeName"]) == 0 && !list3.Contains((string)row2["ValueStr"]))
							{
								AttributeValue attributeValue = new AttributeValue();
								list3.Add((string)row2["ValueStr"]);
								attributeValue.ValueId = row2["ValueId"].ToNullString();
								attributeValue.UseAttributeImage = row2["UseAttributeImage"].ToNullString();
								attributeValue.Value = row2["ValueStr"].ToNullString();
								attributeValue.ImageUrl = Globals.FullPath(row2["ImageUrl"].ToNullString());
								skuItem.AttributeValue.Add(attributeValue);
							}
						}
						list2.Add(skuItem);
					}
				}
			}
			if (productModel.Skus == null || productModel.Skus.Count == 0)
			{
				productModel.Skus = new List<SKUItem>();
			}
			foreach (SKUItem sku in productModel.Skus)
			{
				sku.SalePrice = decimal.Parse(sku.SalePrice.F2ToString("f2"));
				sku.CostPrice = decimal.Parse(sku.CostPrice.F2ToString("f2"));
				list.Add(sku);
			}
			if (string.IsNullOrEmpty(text) && productModel.Skus.Sum((SKUItem s) => s.Stock) <= 0)
			{
				context.Response.Write(this.GetErrorJosn(101, "商品已售罄"));
				context.Response.End();
			}
			else
			{
				string s2 = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						StoreStatus = (int)ex,
						StoreStatusText = ((Enum)(object)ex).ToDescription(),
						ProductId = num,
						ImageUrl = this.GetImageFullPath(productModel.SubmitOrderImg),
						Stock = list.Sum((SKUItem s) => s.Stock),
						ActivityUrl = text,
						FightGroupActivityId = fightGroupActivityId,
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
						skus = from s in list
						select new
						{
							SkuId = s.SkuId,
							SKU = s.SKU,
							Weight = s.Weight,
							Stock = s.Stock,
							StoreStock = s.StoreStock,
							SalePrice = s.SalePrice,
							StoreSalePrice = s.StoreSalePrice,
							ImageUrl = this.GetImageFullPath(s.ImageUrl, true)
						},
						DefaultSku = new
						{
							SkuId = sKUItem.SkuId,
							SKU = sKUItem.SKU,
							Weight = sKUItem.Weight,
							Stock = sKUItem.Stock,
							StoreStock = sKUItem.StoreStock,
							SalePrice = sKUItem.SalePrice,
							StoreSalePrice = sKUItem.StoreSalePrice,
							ImageUrl = this.GetImageFullPath(sKUItem.ImageUrl, true)
						}
					}
				});
				context.Response.Write(s2);
				context.Response.End();
			}
		}

		private static void GetRegions(HttpContext context)
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
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("{");
				stringBuilder.Append("\"Status\":\"OK\",");
				stringBuilder.Append("\"Regions\":[");
				foreach (int key in dictionary.Keys)
				{
					stringBuilder.Append("{");
					stringBuilder.AppendFormat("\"RegionId\":\"{0}\",", key.ToString(CultureInfo.InvariantCulture));
					stringBuilder.AppendFormat("\"RegionName\":\"{0}\"", dictionary[key]);
					stringBuilder.Append("},");
				}
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
				stringBuilder.Append("]}");
				dictionary.Clear();
				context.Response.Write(stringBuilder.ToString());
				end_IL_0001:;
			}
			catch (Exception ex)
			{
				context.Response.Write(ex.Message);
			}
		}

		public void GetNearAddress(HttpContext context)
		{
			int pageSize = context.Request["pageSize"].ToInt(0);
			int pageIndex = context.Request["pageIndex"].ToInt(0);
			string keyWord = context.Request["keyWord"].ToNullString();
			string cityName = context.Request["cityName"].ToNullString();
			IList<POIInfo> nearAddress = DepotHelper.GetNearAddress(keyWord, cityName, pageIndex, pageSize);
			string text = "";
			text = ((nearAddress == null || nearAddress.Count() <= 0) ? JsonConvert.SerializeObject(new
			{
				Result = new
				{
					Status = "NO",
					Data = "[]"
				}
			}) : JsonConvert.SerializeObject(new
			{
				Result = new
				{
					Status = "OK",
					Data = nearAddress
				}
			}));
			context.Response.Write(text);
			context.Response.End();
		}

		public void GetUserShippingAddress(HttpContext context)
		{
			this.CheckSession(context);
			bool forStoreSelect = context.Request["justHasLatLng"].ToBool();
			IList<ShippingAddressInfo> shippingAddresses = MemberProcessor.GetShippingAddresses(forStoreSelect);
			string text = "";
			text = ((shippingAddresses == null || shippingAddresses.Count() <= 0) ? JsonConvert.SerializeObject(new
			{
				Result = new
				{
					Status = "NO",
					Data = "[]"
				}
			}) : JsonConvert.SerializeObject(new
			{
				Result = new
				{
					Status = "OK",
					Data = shippingAddresses
				}
			}));
			context.Response.Write(text);
			context.Response.End();
		}

		private void SaveClientIdAndToken(HttpContext context)
		{
			string text = context.Request["SendClientId"].ToNullString();
			string text2 = context.Request["SendToken"].ToNullString();
			int num = context.Request["UserId"].ToInt(0);
			int num2 = context.Request["ManagerId"].ToInt(0);
			bool flag = false;
			string text3 = "";
			if ((num <= 0 && num2 <= 0) || (text.Length <= 0 && text2.Length <= 0))
			{
				flag = false;
				text3 = "缺少必要参数";
			}
			else
			{
				bool flag2 = false;
				if (num > 0)
				{
					flag2 = MemberHelper.SaveClientIdAndToken(text, text2, num);
				}
				else if (num2 > 0)
				{
					flag2 = DepotHelper.SaveClientIdAndToken(text, text2, num2);
				}
				if (flag2)
				{
					flag = true;
					text3 = "保存成功";
				}
				else
				{
					flag = false;
					text3 = "保存失败";
				}
			}
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					Status = flag,
					Msg = text3
				}
			});
			context.Response.Write(s);
		}

		private void MyFightGroupList(HttpContext context)
		{
			this.CheckSession(context);
			int num = context.Request["PageIndex"].ToInt(0);
			int num2 = context.Request["PageSize"].ToInt(0);
			num2 = ((num2 <= 0) ? 3 : num2);
			num = ((num <= 0) ? 1 : num);
			List<int> list = new List<int>();
			list.Add(1);
			list.Add(4);
			FightGroupQuery fightGroupQuery = new FightGroupQuery();
			fightGroupQuery.PageIndex = num;
			fightGroupQuery.PageSize = num2;
			fightGroupQuery.SortBy = "StartTime";
			fightGroupQuery.SortOrder = SortAction.Asc;
			fightGroupQuery.UserId = HiContext.Current.UserId;
			fightGroupQuery.OrderStatus = list;
			PageModel<UserFightGroupActivitiyModel> myFightGroups = VShopHelper.GetMyFightGroups(fightGroupQuery);
			DateTime now = DateTime.Now;
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					RecordTotal = myFightGroups.Total,
					List = myFightGroups.Models.Select(delegate(UserFightGroupActivitiyModel g)
					{
						int fightGroupId = g.FightGroupId;
						int productId = g.ProductId;
						string productName = g.ProductName;
						string imageFullPath = this.GetImageFullPath(g.ImageUrl1);
						string imageFullPath2 = this.GetImageFullPath(g.Icon);
						DateTime dateTime = g.StartTime;
						string startTime = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
						dateTime = g.EndTime;
						string endTime = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
						DateTime createTime2 = g.CreateTime;
						object createTime;
						if (!(g.CreateTime == DateTime.MinValue))
						{
							dateTime = g.CreateTime;
							createTime = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
						}
						else
						{
							createTime = "";
						}
						int joinNumber = g.JoinNumber;
						int limitedHour = g.LimitedHour;
						string salePrice = g.SalePrice.F2ToString("f2");
						string fightPrice = VShopHelper.GetUserFightPrice(g.FightGroupId, HiContext.Current.UserId).F2ToString("f2");
						int status = g.Status;
						bool isGroupMaster = VShopHelper.UserIsFightGroupHead(g.FightGroupId, HiContext.Current.UserId);
						string statusText = (g.Status == 2) ? "即将开始" : ((g.Status == 1) ? "进行中" : "已结束");
						int groupStatus;
						TimeSpan timeSpan;
						if (g.GroupStatus != 0)
						{
							groupStatus = (int)g.GroupStatus;
						}
						else
						{
							timeSpan = now - g.StartTime;
							groupStatus = ((Math.Ceiling(timeSpan.TotalSeconds) >= (double)(g.LimitedHour * 3600)) ? 2 : 0);
						}
						int num3;
						if (g.GroupStatus != 0)
						{
							num3 = (int)g.GroupStatus;
						}
						else
						{
							timeSpan = now - g.StartTime;
							num3 = ((Math.Ceiling(timeSpan.TotalSeconds) >= (double)(g.LimitedHour * 3600)) ? 2 : 0);
						}
						string groupStatusText = ((Enum)(object)(FightGroupStatus)num3).ToDescription();
						int fightGroupActivityId = g.FightGroupActivityId;
						timeSpan = now - g.StartTime;
						double remainTime;
						if (!(Math.Ceiling(timeSpan.TotalSeconds) > (double)(g.LimitedHour * 3600)))
						{
							double num4 = (double)(g.LimitedHour * 3600);
							timeSpan = now - g.StartTime;
							remainTime = num4 - Math.Ceiling(timeSpan.TotalSeconds);
						}
						else
						{
							remainTime = 0.0;
						}
						return new
						{
							GroupId = fightGroupId,
							ProductId = productId,
							ProductName = productName,
							ProductImage = imageFullPath,
							ActivityImage = imageFullPath2,
							StartTime = startTime,
							EndTime = endTime,
							CreateTime = (string)createTime,
							MaxJoinCount = joinNumber,
							LimitedHour = limitedHour,
							SalePrice = salePrice,
							FightPrice = fightPrice,
							Status = status,
							IsGroupMaster = isGroupMaster,
							StatusText = statusText,
							GroupStatus = groupStatus,
							GroupStatusText = groupStatusText,
							ActivityId = fightGroupActivityId,
							RemainTime = remainTime,
							OrderId = g.OrderId,
							GroupMembers = from fgu in VShopHelper.GetFightGroupUsers(g.FightGroupId)
							orderby fgu.IsFightGroupHead descending
							select fgu into m
							select new
							{
								UserId = m.UserId,
								NickName = m.Name,
								HeadImage = (string.IsNullOrEmpty(m.Picture.ToNullString()) ? Globals.FullPath("/templates/common/images/headerimg.png") : Globals.FullPath(m.Picture.ToNullString())),
								IsMaster = m.IsFightGroupHead
							}
						};
					})
				}
			});
			context.Response.Write(s);
			context.Response.End();
		}

		private void FightGroupShare(HttpContext context)
		{
			int num = 1;
			int needJoinNumber = 0;
			int num2 = context.Request["FightGroupId"].ToInt(0);
			string orderId = context.Request["OrderId"].ToNullString();
			OrderInfo orderInfo = null;
			if (num2 <= 0)
			{
				orderInfo = OrderHelper.GetOrderInfo(orderId);
				if (orderInfo == null)
				{
					context.Response.Write(this.GetErrorJosn(101, ((Enum)(object)ApiErrorCode.Paramter_Error).ToDescription()));
					return;
				}
				num2 = orderInfo.FightGroupId;
			}
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (num2 <= 0)
			{
				context.Response.Write(this.GetErrorJosn(101, ((Enum)(object)ApiErrorCode.Paramter_Error).ToDescription()));
			}
			else
			{
				FightGroupInfo fightGroup = VShopHelper.GetFightGroup(num2);
				if (fightGroup == null)
				{
					context.Response.Write(this.GetErrorJosn(703, ((Enum)(object)ApiErrorCode.FightGroup_NoExist).ToDescription()));
				}
				else
				{
					FightGroupActivityInfo fightGroupActivitieInfo = TradeHelper.GetFightGroupActivitieInfo(fightGroup.FightGroupActivityId);
					if (fightGroupActivitieInfo == null)
					{
						context.Response.Write(this.GetErrorJosn(701, ((Enum)(object)ApiErrorCode.ImageIdNotExists_Error).ToDescription()));
					}
					else
					{
						ProductInfo productSimpleInfo = ProductBrowser.GetProductSimpleInfo(fightGroupActivitieInfo.ProductId);
						if (productSimpleInfo == null)
						{
							context.Response.Write(this.GetErrorJosn(702, ((Enum)(object)ApiErrorCode.Activity_RelationProduct_NoExist).ToDescription()));
						}
						else
						{
							bool flag = false;
							if (orderInfo != null)
							{
								flag = orderInfo.IsFightGroupHead;
							}
							IList<FightGroupUserModel> fightGroupUsers = VShopHelper.GetFightGroupUsers(num2);
							num = (flag ? 1 : ((fightGroupUsers == null || fightGroupUsers.Count >= fightGroup.JoinNumber) ? 3 : 2));
							if (num != 3)
							{
								needJoinNumber = fightGroup.JoinNumber - fightGroupUsers.Count;
							}
							string text = Globals.FullPath(masterSettings.SiteUrl.ToNullString());
							string shareTitle = (!string.IsNullOrEmpty(fightGroupActivitieInfo.ShareTitle)) ? fightGroupActivitieInfo.ShareTitle.Trim() : (string.IsNullOrEmpty(productSimpleInfo.Title) ? productSimpleInfo.ProductName : productSimpleInfo.Title);
							string shareContent = (!string.IsNullOrEmpty(fightGroupActivitieInfo.ShareContent.Trim())) ? fightGroupActivitieInfo.ShareContent.Trim() : productSimpleInfo.Meta_Description;
							string icon = fightGroupActivitieInfo.Icon;
							string shareLink = Globals.FullPath($"/vshop/FightGroupDetails.aspx?fightGroupId={fightGroup.FightGroupId}");
							MemberInfo user = HiContext.Current.User;
							if (user.IsReferral())
							{
								shareLink = Globals.FullPath($"/vshop/FightGroupDetails.aspx?fightGroupId={fightGroup.FightGroupId}&ReferralUserId={user.UserId}");
							}
							string s = JsonConvert.SerializeObject(new
							{
								Result = new
								{
									Status = num,
									NeedJoinNumber = needJoinNumber,
									ShareImage = this.GetImageFullPath(icon),
									ShareTitle = shareTitle,
									ShareContent = shareContent,
									ShareLink = shareLink
								}
							});
							context.Response.Write(s);
							context.Response.End();
						}
					}
				}
			}
		}

		private void FightGroupDetail(HttpContext context)
		{
			string text = context.Request["SessionId"].ToNullString();
			MemberInfo user = HiContext.Current.User;
			if (!string.IsNullOrEmpty(text))
			{
				user = MemberProcessor.FindMemberBySessionId(text);
			}
			int gradeId = 0;
			int fightGroupId = context.Request["FightGroupId"].ToInt(0);
			FightGroupModel fightGroupInfo = VShopHelper.GetFightGroupInfo(fightGroupId);
			if (fightGroupInfo == null)
			{
				context.Response.Write(this.GetErrorJosn(703, ((Enum)(object)ApiErrorCode.FightGroup_NoExist).ToDescription()));
			}
			else
			{
				FightGroupActivityInfo fightGroupActivitieInfo = VShopHelper.GetFightGroupActivitieInfo(fightGroupInfo.FightGroupActivityId);
				if (fightGroupActivitieInfo == null)
				{
					context.Response.Write(this.GetErrorJosn(701, ((Enum)(object)ApiErrorCode.ImageIdNotExists_Error).ToDescription()));
				}
				else
				{
					IList<FightGroupUserModel> fightGroupUsers = VShopHelper.GetFightGroupUsers(fightGroupId);
					SiteSettings masterSettings = SettingsManager.GetMasterSettings();
					ProductBrowseInfo productBrowseInfo = ProductBrowser.GetProductBrowseInfo(fightGroupActivitieInfo.ProductId, null, masterSettings.OpenMultStore, gradeId);
					if (productBrowseInfo == null || productBrowseInfo.Product == null)
					{
						context.Response.Write(this.GetErrorJosn(702, ((Enum)(object)ApiErrorCode.Activity_RelationProduct_NoExist).ToDescription()));
					}
					else
					{
						IList<SkuItem> list = null;
						string empty = string.Empty;
						string text2 = "";
						IList<FightGroupSkuInfo> fightGroupSkus = VShopHelper.GetFightGroupSkus(fightGroupActivitieInfo.FightGroupActivityId);
						bool hasSku = false;
						if (productBrowseInfo.DbSKUs == null || productBrowseInfo.DbSKUs.Rows.Count == 0)
						{
							list = new List<SkuItem>();
						}
						else
						{
							hasSku = true;
							list = this.GetProductAppSkuItems(productBrowseInfo);
						}
						bool isSupportTakeOnStore = false;
						if (masterSettings.OpenMultStore)
						{
							isSupportTakeOnStore = StoresHelper.ProductHasStores(productBrowseInfo.Product.ProductId);
						}
						IList<SKUItem> list2 = new List<SKUItem>();
						if (productBrowseInfo.Product.Skus.Count == 0)
						{
							list2.Add(productBrowseInfo.Product.DefaultSku);
							list2[0].SalePrice = decimal.Parse(productBrowseInfo.Product.DefaultSku.SalePrice.F2ToString("f2"));
							list2[0].CostPrice = decimal.Parse(productBrowseInfo.Product.DefaultSku.CostPrice.F2ToString("f2"));
						}
						else
						{
							foreach (SKUItem value in productBrowseInfo.Product.Skus.Values)
							{
								value.SalePrice = decimal.Parse(value.SalePrice.F2ToString("f2"));
								value.CostPrice = decimal.Parse(value.CostPrice.F2ToString("f2"));
								list2.Add(value);
							}
						}
						DateTime now = DateTime.Now;
						TimeSpan timeSpan = now - fightGroupInfo.StartTime;
						if (Math.Ceiling(timeSpan.TotalSeconds) > (double)(fightGroupActivitieInfo.LimitedHour * 3600) && fightGroupInfo.GroupStatus == FightGroupStatus.FightGroupIn)
						{
							VShopHelper.DealFightGroupFail(fightGroupId);
						}
						IList<FightGroupSkuInfo> fightGroupSkus2 = VShopHelper.GetFightGroupSkus(fightGroupActivitieInfo.FightGroupActivityId);
						decimal num = default(decimal);
						if (user != null)
						{
							num = VShopHelper.GetUserFightPrice(fightGroupId, user.UserId);
						}
						if (user == null || num == decimal.Zero)
						{
							num = fightGroupSkus2.Min((FightGroupSkuInfo f) => f.SalePrice);
						}
						int fightGroupId2 = fightGroupInfo.FightGroupId;
						int productId = fightGroupInfo.ProductId;
						string productName = fightGroupInfo.ProductName;
						string imageFullPath = this.GetImageFullPath(fightGroupInfo.ImageUrl1);
						string imageFullPath2 = this.GetImageFullPath(fightGroupInfo.Icon);
						DateTime dateTime = fightGroupInfo.StartTime;
						string startTime = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
						dateTime = fightGroupInfo.EndTime;
						string endTime = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
						DateTime createTime2 = fightGroupInfo.CreateTime;
						object createTime;
						if (!(fightGroupInfo.CreateTime == DateTime.MinValue))
						{
							dateTime = fightGroupInfo.CreateTime;
							createTime = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
						}
						else
						{
							createTime = "";
						}
						int joinNumber = fightGroupInfo.JoinNumber;
						int maxCount = fightGroupActivitieInfo.MaxCount;
						int limitedHour = fightGroupActivitieInfo.LimitedHour;
						string salePrice = productBrowseInfo.Product.MaxSalePrice.ToNullString().ToDecimal(0).F2ToString("f2");
						string fightPrice = num.F2ToString("f2");
						bool isGroupMaster = user != null && VShopHelper.UserIsFightGroupHead(fightGroupInfo.FightGroupId, user.UserId);
						int status = (!(now < fightGroupActivitieInfo.StartDate)) ? ((now >= fightGroupActivitieInfo.StartDate && now <= fightGroupActivitieInfo.EndDate) ? 1 : 2) : 0;
						string statusText = (now < fightGroupActivitieInfo.StartDate) ? "即将开始" : ((now >= fightGroupActivitieInfo.StartDate && now <= fightGroupActivitieInfo.EndDate) ? "进行中" : "已结束");
						int groupStatus;
						if (fightGroupInfo.GroupStatus != 0)
						{
							groupStatus = (int)fightGroupInfo.GroupStatus;
						}
						else
						{
							timeSpan = now - fightGroupInfo.StartTime;
							groupStatus = ((Math.Ceiling(timeSpan.TotalSeconds) >= (double)(fightGroupInfo.LimitedHour * 3600)) ? 2 : 0);
						}
						int num2;
						if (fightGroupInfo.GroupStatus != 0)
						{
							num2 = (int)fightGroupInfo.GroupStatus;
						}
						else
						{
							timeSpan = now - fightGroupInfo.StartTime;
							num2 = ((Math.Ceiling(timeSpan.TotalSeconds) >= (double)(fightGroupInfo.LimitedHour * 3600)) ? 2 : 0);
						}
						string groupStatusText = ((Enum)(object)(FightGroupStatus)num2).ToDescription();
						int fightGroupActivityId = fightGroupInfo.FightGroupActivityId;
						timeSpan = now - fightGroupInfo.StartTime;
						double remainTime;
						if (!(Math.Ceiling(timeSpan.TotalSeconds) > (double)(fightGroupActivitieInfo.LimitedHour * 3600)))
						{
							double num3 = (double)(fightGroupActivitieInfo.LimitedHour * 3600);
							timeSpan = now - fightGroupInfo.StartTime;
							remainTime = num3 - Math.Ceiling(timeSpan.TotalSeconds);
						}
						else
						{
							remainTime = 0.0;
						}
						string s = JsonConvert.SerializeObject(new
						{
							Result = new
							{
								GroupId = fightGroupId2,
								ProductId = productId,
								ProductName = productName,
								ProductImage = imageFullPath,
								ActivityImage = imageFullPath2,
								StartTime = startTime,
								EndTime = endTime,
								CreateTime = (string)createTime,
								MaxJoinCount = joinNumber,
								MaxCount = maxCount,
								LimitedHour = limitedHour,
								SalePrice = salePrice,
								FightPrice = fightPrice,
								IsGroupMaster = isGroupMaster,
								Status = status,
								StatusText = statusText,
								GroupStatus = groupStatus,
								GroupStatusText = groupStatusText,
								ActivityId = fightGroupActivityId,
								RemainTime = remainTime,
								ConsultationCount = productBrowseInfo.ConsultationCount,
								ReviewCount = productBrowseInfo.ReviewCount,
								UserIsJoinGroup = (user != null && (from m in fightGroupUsers
								where m.UserId == user.UserId
								select m).Count() > 0),
								ProductInfo = new
								{
									ProductId = productBrowseInfo.Product.ProductId,
									Stock = productBrowseInfo.Product.Stock,
									ActivityStock = fightGroupSkus2.Sum((FightGroupSkuInfo fs) => fs.TotalCount) - fightGroupSkus2.Sum((FightGroupSkuInfo fs) => fs.BoughtCount),
									SalePrice = productBrowseInfo.Product.MaxSalePrice,
									CostPrice = productBrowseInfo.Product.CostPrice,
									PromotionInfo = this.GetOrderPromotionInfo(productBrowseInfo.Product.ProductId),
									DefaultSku = productBrowseInfo.Product.DefaultSku,
									HasSku = hasSku,
									SkuItem = list,
									ConsultationCount = productBrowseInfo.ConsultationCount,
									ReviewCount = productBrowseInfo.ReviewCount,
									IsSupportPodrequest = false,
									IsSupportTakeOnStore = isSupportTakeOnStore,
									ProductImages = this.GetProductImages(productBrowseInfo.Product),
									DefaultImage = this.GetImageFullPath(productBrowseInfo.Product.ThumbnailUrl410),
									Description = (string.IsNullOrEmpty(productBrowseInfo.Product.MobbileDescription) ? productBrowseInfo.Product.Description : productBrowseInfo.Product.MobbileDescription).ToNullString().Replace("\"/Storage/master/gallery/", "\"" + Globals.FullPath("/Storage/master/gallery/")),
									Skus = from sku in list2
									select new
									{
										SkuId = sku.SkuId,
										SKU = sku.SKU,
										Weight = sku.Weight,
										Stock = sku.Stock,
										WarningStock = sku.WarningStock,
										ActivityStock = (from fgs in fightGroupSkus
										where fgs.SkuId == sku.SkuId
										select fgs).FirstOrDefault().TotalCount - (from fgs in fightGroupSkus
										where fgs.SkuId == sku.SkuId
										select fgs).FirstOrDefault().BoughtCount,
										BoughtCount = (from fgs in fightGroupSkus
										where fgs.SkuId == sku.SkuId
										select fgs).FirstOrDefault().BoughtCount,
										ActivityPrice = (from fgs in fightGroupSkus
										where fgs.SkuId == sku.SkuId
										select fgs).FirstOrDefault().SalePrice.F2ToString("f2"),
										SalePrice = sku.SalePrice.F2ToString("f2"),
										ImageUrl = this.GetImageFullPath(sku.ImageUrl, true)
									}
								},
								GroupMembers = from jmo in fightGroupUsers
								orderby jmo.IsFightGroupHead descending
								select jmo into m
								select new
								{
									UserId = m.UserId,
									NickName = DataHelper.GetHiddenUsername(m.Name),
									HeadImage = (string.IsNullOrEmpty(m.Picture.ToNullString()) ? Globals.FullPath("/templates/common/images/headerimg.png") : Globals.FullPath(m.Picture.ToNullString())),
									JoinTime = m.OrderDate,
									IsMaster = m.IsFightGroupHead
								}
							}
						});
						context.Response.Write(s);
						context.Response.End();
					}
				}
			}
		}

		private IList<SkuItem> GetProductAppSkuItems(ProductBrowseInfo product)
		{
			IList<SkuItem> list = new List<SkuItem>();
			foreach (DataRow row in product.DbSKUs.Rows)
			{
				if ((from c in list
				where c.AttributeName == row["AttributeName"].ToNullString()
				select c).Count() == 0)
				{
					SkuItem skuItem = new SkuItem();
					skuItem.AttributeName = row["AttributeName"].ToNullString();
					skuItem.AttributeId = row["AttributeId"].ToNullString();
					skuItem.AttributeValue = new List<AttributeValue>();
					IList<string> list2 = new List<string>();
					foreach (DataRow row2 in product.DbSKUs.Rows)
					{
						if (string.Compare((string)row["AttributeName"], (string)row2["AttributeName"]) == 0 && !list2.Contains((string)row2["ValueStr"]))
						{
							AttributeValue attributeValue = new AttributeValue();
							list2.Add((string)row2["ValueStr"]);
							attributeValue.ValueId = row2["ValueId"].ToNullString();
							attributeValue.UseAttributeImage = row2["UseAttributeImage"].ToNullString();
							attributeValue.Value = row2["ValueStr"].ToNullString();
							attributeValue.ImageUrl = this.GetImageFullPath(row2["ImageUrl"].ToNullString(), true);
							skuItem.AttributeValue.Add(attributeValue);
						}
					}
					list.Add(skuItem);
				}
			}
			return list;
		}

		private void FightGroupActivityDetail(HttpContext context)
		{
			int fightGroupActivityId = context.Request["FightGroupActivityId"].ToInt(0);
			FightGroupActivityInfo fightGroupActivity = TradeHelper.GetFightGroupActivitieInfo(fightGroupActivityId);
			int gradeId = 0;
			string text = context.Request["SessionId"].ToNullString();
			if (!string.IsNullOrEmpty(text))
			{
				MemberInfo memberInfo = MemberProcessor.FindMemberBySessionId(text);
				if (memberInfo != null)
				{
					gradeId = memberInfo.GradeId;
				}
			}
			if (fightGroupActivity == null)
			{
				context.Response.Write(this.GetErrorJosn(701, ((Enum)(object)ApiErrorCode.ImageIdNotExists_Error).ToDescription()));
			}
			else
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				ProductBrowseInfo productBrowseInfo = ProductBrowser.GetProductBrowseInfo(fightGroupActivity.ProductId, null, masterSettings.OpenMultStore, gradeId);
				if (productBrowseInfo == null || productBrowseInfo.Product == null)
				{
					context.Response.Write(this.GetErrorJosn(702, ((Enum)(object)ApiErrorCode.Activity_RelationProduct_NoExist).ToDescription()));
				}
				else
				{
					string empty = string.Empty;
					int supplierId = productBrowseInfo.Product.SupplierId;
					if (supplierId > 0)
					{
						SupplierInfo supplierById = SupplierHelper.GetSupplierById(supplierId);
						empty = ((supplierById == null) ? "" : supplierById.SupplierName);
					}
					else
					{
						empty = "";
					}
					IList<SkuItem> list = null;
					string empty2 = string.Empty;
					string nickName = "";
					IList<FightGroupSkuInfo> fightGroupSkus = VShopHelper.GetFightGroupSkus(fightGroupActivityId);
					bool hasSku = false;
					if (productBrowseInfo.DbSKUs == null || productBrowseInfo.DbSKUs.Rows.Count == 0)
					{
						list = new List<SkuItem>();
					}
					else
					{
						hasSku = true;
						list = this.GetProductAppSkuItems(productBrowseInfo);
					}
					bool isSupportTakeOnStore = false;
					if (masterSettings.OpenMultStore)
					{
						isSupportTakeOnStore = StoresHelper.ProductHasStores(productBrowseInfo.Product.ProductId);
					}
					IList<SKUItem> list2 = new List<SKUItem>();
					if (productBrowseInfo.Product.Skus.Count == 0)
					{
						list2.Add(productBrowseInfo.Product.DefaultSku);
						list2[0].SalePrice = decimal.Parse(productBrowseInfo.Product.DefaultSku.SalePrice.F2ToString("f2"));
						list2[0].CostPrice = decimal.Parse(productBrowseInfo.Product.DefaultSku.CostPrice.F2ToString("f2"));
					}
					else
					{
						foreach (SKUItem value in productBrowseInfo.Product.Skus.Values)
						{
							value.SalePrice = decimal.Parse(value.SalePrice.F2ToString("f2"));
							value.CostPrice = decimal.Parse(value.CostPrice.F2ToString("f2"));
							list2.Add(value);
						}
					}
					int joinedCounts = 0;
					DateTime now = DateTime.Now;
					IList<FightGroupModel> list3 = (from fg in VShopHelper.GetAllFightGroups(fightGroupActivityId)
					where fg.GroupStatus == FightGroupStatus.FightGroupIn && DateTime.Now <= fg.EndTime
					select fg).ToList();
					string text2 = "";
					decimal num = fightGroupSkus.Min((FightGroupSkuInfo f) => f.SalePrice);
					text2 = num.F2ToString("f2");
					if (fightGroupSkus.Count() > 1)
					{
						decimal num2 = fightGroupSkus.Max((FightGroupSkuInfo f) => f.SalePrice);
						if (num2 > num)
						{
							text2 = num.F2ToString("f2") + "～" + num2.F2ToString("f2");
						}
					}
					int status = 1;
					DateTime now2 = DateTime.Now;
					if (fightGroupActivity.StartDate <= now2 && fightGroupActivity.EndDate >= now2)
					{
						status = 1;
					}
					if (fightGroupActivity.StartDate > now2)
					{
						status = 2;
					}
					if (fightGroupActivity.EndDate < now2)
					{
						status = 3;
					}
					int fightGroupActivityId2 = fightGroupActivity.FightGroupActivityId;
					int productId = fightGroupActivity.ProductId;
					string productName = fightGroupActivity.ProductName;
					string imageFullPath = this.GetImageFullPath(fightGroupActivity.Icon);
					DateTime dateTime = fightGroupActivity.StartDate;
					string startTime = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
					dateTime = fightGroupActivity.EndDate;
					string s = JsonConvert.SerializeObject(new
					{
						Result = new
						{
							ActivityId = fightGroupActivityId2,
							ProductId = productId,
							ProductName = productName,
							ActivityImage = imageFullPath,
							StartTime = startTime,
							EndTime = dateTime.ToString("yyyy-MM-dd HH:mm:ss"),
							MaxJoinCount = fightGroupActivity.JoinNumber,
							LimitedHour = fightGroupActivity.LimitedHour,
							MaxCount = fightGroupActivity.MaxCount,
							SalePrice = productBrowseInfo.Product.MaxSalePrice.F2ToString("f2"),
							FightPrice = num.F2ToString("f2"),
							ShowPrice = text2,
							Status = status,
							StatusText = ((now < fightGroupActivity.StartDate) ? "即将开始" : ((now >= fightGroupActivity.StartDate && now <= fightGroupActivity.EndDate) ? "进行中" : "已结束")),
							GroupCounts = list3.Count,
							RemainTime = ((now >= fightGroupActivity.EndDate) ? 0.0 : (fightGroupActivity.EndDate - now).TotalSeconds),
							SupplierName = empty,
							ProductInfo = new
							{
								ProductId = productBrowseInfo.Product.ProductId,
								Stock = productBrowseInfo.Product.Stock,
								ActivityStock = fightGroupSkus.Sum((FightGroupSkuInfo fs) => fs.TotalCount) - fightGroupSkus.Sum((FightGroupSkuInfo fs) => fs.BoughtCount),
								SalePrice = productBrowseInfo.Product.MaxSalePrice,
								CostPrice = productBrowseInfo.Product.CostPrice,
								PromotionInfo = this.GetOrderPromotionInfo(productBrowseInfo.Product.ProductId),
								DefaultSku = productBrowseInfo.Product.DefaultSku,
								HasSku = hasSku,
								SkuItem = list,
								ConsultationCount = productBrowseInfo.ConsultationCount,
								ReviewCount = productBrowseInfo.ReviewCount,
								IsSupportPodrequest = false,
								IsSupportTakeOnStore = isSupportTakeOnStore,
								ProductImages = this.GetProductImages(productBrowseInfo.Product),
								DefaultImage = this.GetImageFullPath(productBrowseInfo.Product.ThumbnailUrl410),
								Description = (string.IsNullOrEmpty(productBrowseInfo.Product.MobbileDescription) ? productBrowseInfo.Product.Description : productBrowseInfo.Product.MobbileDescription).ToNullString().Replace("\"/Storage/master/gallery/", "\"" + Globals.FullPath("/Storage/master/gallery/")),
								ExtendAttribute = ProductBrowser.GetExpandAttributeList(productBrowseInfo.Product.ProductId),
								Skus = from sku in list2
								select new
								{
									SkuId = sku.SkuId,
									SKU = sku.SKU,
									Weight = sku.Weight,
									Stock = sku.Stock,
									WarningStock = sku.WarningStock,
									ActivityStock = ((sku.Stock < (from fgs in fightGroupSkus
									where fgs.SkuId == sku.SkuId
									select fgs).FirstOrDefault().TotalCount) ? sku.Stock : (from fgs in fightGroupSkus
									where fgs.SkuId == sku.SkuId
									select fgs).FirstOrDefault().TotalCount) - (from fgs in fightGroupSkus
									where fgs.SkuId == sku.SkuId
									select fgs).FirstOrDefault().BoughtCount,
									BoughtCount = (from fgs in fightGroupSkus
									where fgs.SkuId == sku.SkuId
									select fgs).FirstOrDefault().BoughtCount,
									ActivityPrice = (from fgs in fightGroupSkus
									where fgs.SkuId == sku.SkuId
									select fgs).FirstOrDefault().SalePrice.F2ToString("f2"),
									SalePrice = sku.SalePrice.F2ToString("f2"),
									ImageUrl = this.GetImageFullPath(sku.ImageUrl, true)
								}
							},
							GroupItems = (from fgo in list3
							orderby fgo.IsFightGroupHead descending
							select fgo).Select(delegate(FightGroupModel fg)
							{
								int fightGroupId = fg.FightGroupId;
								DateTime startTime2 = fg.StartTime;
								DateTime dateTime2 = fg.StartTime;
								dateTime2 = dateTime2.AddHours((double)fightGroupActivity.LimitedHour);
								string endTime = dateTime2.ToString("yyyy-MM-dd HH:mm:ss");
								int joinNumber = fg.JoinNumber;
								DateTime createTime2 = fg.CreateTime;
								object createTime;
								if (!(fg.CreateTime == DateTime.MinValue))
								{
									dateTime2 = fg.CreateTime;
									createTime = dateTime2.ToString("yyyy-MM-dd HH:mm:ss");
								}
								else
								{
									createTime = "";
								}
								FightGroupStatus status2 = (fg.GroupStatus == FightGroupStatus.FightGroupIn) ? ((fg.EndTime < DateTime.Now) ? FightGroupStatus.FightGroupFail : fg.GroupStatus) : fg.GroupStatus;
								string statusText = ((Enum)(object)((fg.GroupStatus == FightGroupStatus.FightGroupIn) ? ((fg.EndTime < DateTime.Now) ? FightGroupStatus.FightGroupFail : fg.GroupStatus) : fg.GroupStatus)).ToDescription();
								string groupUserInfo = this.GetGroupUserInfo(fg.FightGroupId, out joinedCounts, out nickName);
								int needJoinNumber = (joinedCounts < fightGroupActivity.JoinNumber) ? (fightGroupActivity.JoinNumber - joinedCounts) : 0;
								TimeSpan timeSpan = now - fg.StartTime;
								double remainTime;
								if (!(Math.Ceiling(timeSpan.TotalSeconds) > (double)(fightGroupActivity.LimitedHour * 3600)))
								{
									double num3 = (double)(fightGroupActivity.LimitedHour * 3600);
									timeSpan = now - fg.StartTime;
									remainTime = num3 - Math.Ceiling(timeSpan.TotalSeconds);
								}
								else
								{
									remainTime = 0.0;
								}
								return new
								{
									FightGroupId = fightGroupId,
									StartTime = startTime2,
									EndTime = endTime,
									JoinNumber = joinNumber,
									CreateTime = (string)createTime,
									Status = status2,
									StatusText = statusText,
									HeadImage = groupUserInfo,
									NeedJoinNumber = needJoinNumber,
									RemainTime = remainTime,
									NickName = nickName
								};
							})
						}
					});
					context.Response.Write(s);
					context.Response.End();
				}
			}
		}

		private string GetGroupUserInfo(int fightGroupId, out int joinCount, out string nickName)
		{
			joinCount = 0;
			string result = "";
			nickName = "";
			FightGroupInfo fightGroup = VShopHelper.GetFightGroup(fightGroupId);
			IList<FightGroupUserModel> fightGroupUsers = VShopHelper.GetFightGroupUsers(fightGroupId);
			if (fightGroupUsers == null || fightGroupUsers.Count == 0)
			{
				return "";
			}
			joinCount = fightGroupUsers.Count;
			foreach (FightGroupUserModel item in fightGroupUsers)
			{
				if (item.IsFightGroupHead)
				{
					nickName = DataHelper.GetHiddenUsername(item.Name);
					return string.IsNullOrEmpty(item.Picture.ToNullString()) ? Globals.FullPath("/templates/common/images/headerimg.png") : Globals.FullPath(item.Picture);
				}
			}
			return result;
		}

		private string GetProductImages(ProductInfo product)
		{
			string text = "";
			if (!string.IsNullOrEmpty(product.ImageUrl1))
			{
				text = text + Globals.FullPath(product.ImageUrl1) + ",";
			}
			if (!string.IsNullOrEmpty(product.ImageUrl2))
			{
				text = text + Globals.FullPath(product.ImageUrl2) + ",";
			}
			if (!string.IsNullOrEmpty(product.ImageUrl3))
			{
				text = text + Globals.FullPath(product.ImageUrl3) + ",";
			}
			if (!string.IsNullOrEmpty(product.ImageUrl4))
			{
				text = text + Globals.FullPath(product.ImageUrl4) + ",";
			}
			if (!string.IsNullOrEmpty(product.ImageUrl5))
			{
				text = text + Globals.FullPath(product.ImageUrl5) + ",";
			}
			return text.TrimEnd(',');
		}

		private void FightGroupActivityList(HttpContext context)
		{
			int num = context.Request["PageIndex"].ToInt(0);
			int num2 = context.Request["PageSize"].ToInt(0);
			FightGroupActivityQuery fightGroupActivityQuery = new FightGroupActivityQuery();
			fightGroupActivityQuery.PageIndex = ((num == 0) ? 1 : num);
			fightGroupActivityQuery.PageSize = ((num2 == 0) ? 3 : num2);
			fightGroupActivityQuery.SortBy = "DisplaySequence DESC,FightGroupActivityId";
			fightGroupActivityQuery.SortOrder = SortAction.Asc;
			fightGroupActivityQuery.IsCount = true;
			PageModel<FightGroupActivitiyModel> fightGroupActivitieLists = VShopHelper.GetFightGroupActivitieLists(fightGroupActivityQuery);
			DateTime now = DateTime.Now;
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					RecordTotal = fightGroupActivitieLists.Total,
					List = fightGroupActivitieLists.Models.Select(delegate(FightGroupActivitiyModel g)
					{
						int fightGroupActivityId = g.FightGroupActivityId;
						int productId = g.ProductId;
						string productName = g.ProductName;
						string imageFullPath = this.GetImageFullPath(g.Icon);
						DateTime dateTime = g.StartDate;
						string startTime = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
						dateTime = g.EndDate;
						return new
						{
							ActivityId = fightGroupActivityId,
							ProductId = productId,
							ProductName = productName,
							ActivityImage = imageFullPath,
							StartTime = startTime,
							EndTime = dateTime.ToString("yyyy-MM-dd HH:mm:ss"),
							MaxJoinCount = g.JoinNumber,
							LimitedHour = g.LimitedHour,
							MaxCount = g.MaxCount,
							SalePrice = g.SalePrice.F2ToString("f2"),
							FightPrice = g.FightPrice.F2ToString("f2"),
							Status = g.Status,
							StatusText = g.StatusText
						};
					})
				}
			});
			context.Response.Write(s);
			context.Response.End();
		}

		private string GetImageFullPath(string imageUrl, bool allowEmpty = false)
		{
			if (string.IsNullOrEmpty(imageUrl))
			{
				if (allowEmpty)
				{
					return "";
				}
				return Globals.FullPath(HiContext.Current.SiteSettings.DefaultProductImage);
			}
			if (imageUrl.StartsWith("http://"))
			{
				return imageUrl;
			}
			return Globals.FullPath(imageUrl);
		}

		protected void ProductConsultations(HttpContext context)
		{
			if (string.IsNullOrEmpty(context.Request["productId"].ToNullString()) || string.IsNullOrEmpty(context.Request["userName"].ToNullString()) || string.IsNullOrEmpty(context.Request["consultationText"].ToNullString()))
			{
				context.Response.Write(this.GetErrorJosn(101, "缺少必填参数"));
			}
			else
			{
				ProductConsultationInfo productConsultationInfo = new ProductConsultationInfo();
				productConsultationInfo.ConsultationDate = DateTime.Now;
				productConsultationInfo.ProductId = context.Request["productId"].ToInt(0);
				productConsultationInfo.UserId = HiContext.Current.UserId;
				string text = context.Request["sessionid"].ToNullString();
				MemberInfo memberInfo = null;
				if (!string.IsNullOrEmpty(text))
				{
					memberInfo = MemberProcessor.FindMemberBySessionId(text);
				}
				productConsultationInfo.UserId = (memberInfo?.UserId ?? 0);
				productConsultationInfo.UserName = context.Request["userName"].ToNullString();
				productConsultationInfo.UserEmail = ((memberInfo != null && !string.IsNullOrEmpty(memberInfo.Email)) ? memberInfo.Email : context.Request["userName"].ToNullString());
				productConsultationInfo.ConsultationText = Globals.HtmlEncode(context.Request["consultationText"].ToNullString());
				bool flag = ProductBrowser.InsertProductConsultation(productConsultationInfo);
				string s = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						Status = flag,
						Msg = (flag ? "咨询成功" : "咨询失败")
					}
				});
				context.Response.Write(s);
				context.Response.End();
			}
		}

		public void Favorites(HttpContext context)
		{
			this.CheckSession(context);
			string keyword = Globals.HtmlDecode(context.Request["keyword"].ToNullString());
			string tags = Globals.HtmlDecode(context.Request["tags"].ToNullString());
			int num = context.Request["pageIndex"].ToInt(0);
			int num2 = context.Request["pageSize"].ToInt(0);
			if (num <= 0 || num2 <= 0)
			{
				context.Response.Write(this.GetErrorJosn(101, "缺少必填参数"));
			}
			else
			{
				Pagination pagination = new Pagination();
				pagination.PageIndex = num;
				pagination.PageSize = num2;
				DbQueryResult favorites = ProductBrowser.GetFavorites(keyword, tags, pagination, false);
				DataTable data = favorites.Data;
				if (data != null && data.Rows.Count > 0)
				{
					for (int i = 0; i < data.Rows.Count; i++)
					{
						data.Rows[i]["ThumbnailUrl60"] = ((data.Rows[i]["ThumbnailUrl60"].ToNullString() == "") ? Globals.FullPath(this.siteSettings.DefaultProductThumbnail1) : Globals.FullPath(data.Rows[i]["ThumbnailUrl60"].ToString()));
						data.Rows[i]["ThumbnailUrl100"] = ((data.Rows[i]["ThumbnailUrl100"].ToNullString() == "") ? Globals.FullPath(this.siteSettings.DefaultProductThumbnail2) : Globals.FullPath(data.Rows[i]["ThumbnailUrl100"].ToString()));
					}
				}
				string s = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						RecordCount = favorites.TotalRecords,
						List = favorites.Data
					}
				});
				context.Response.Write(s);
			}
		}

		private void FinishOrder(HttpContext context)
		{
			string text = context.Request["orderId"].ToNullString();
			try
			{
				this.CheckSession(context);
				OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(text);
				if (orderInfo != null && TradeHelper.ConfirmOrderFinish(orderInfo))
				{
					string s = JsonConvert.SerializeObject(new
					{
						Success = new
						{
							Status = true,
							Msg = "成功完成订单"
						}
					});
					context.Response.Write(s);
					context.Response.End();
				}
				else
				{
					string s2 = JsonConvert.SerializeObject(new
					{
						Success = new
						{
							Status = false,
							Msg = "当前状态不允许完成订单"
						}
					});
					context.Response.Write(s2);
					context.ApplicationInstance.CompleteRequest();
				}
			}
			catch (Exception ex)
			{
				Globals.WriteLog("ConfirmOrderFinish.txt", text + "|" + ex.Message);
			}
		}

		protected void DeleteFavorites(HttpContext context)
		{
			this.CheckSession(context);
			if (string.IsNullOrEmpty(context.Request["FavoriteIds"].ToNullString()))
			{
				context.Response.Write(this.GetErrorJosn(101, "缺少必填参数"));
			}
			else
			{
				string text = context.Request["FavoriteIds"].ToNullString();
				text = text.TrimEnd(',');
				bool flag = ProductBrowser.DeleteFavorites(text);
				string s = JsonConvert.SerializeObject(new
				{
					Success = new
					{
						Status = flag,
						Msg = (flag ? "删除成功" : "删除失败")
					}
				});
				context.Response.Write(s);
				context.Response.End();
			}
		}

		public void CloseOrder(HttpContext context)
		{
			this.CheckSession(context);
			string text = context.Request["orderId"].ToNullString();
			if (string.IsNullOrEmpty(text))
			{
				context.Response.Write(this.GetErrorJosn(101, "缺少必填参数"));
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
					string s = JsonConvert.SerializeObject(new
					{
						Success = new
						{
							Status = true,
							Msg = "关闭订单成功"
						}
					});
					context.Response.Write(s);
					context.Response.End();
				}
				else
				{
					string s2 = JsonConvert.SerializeObject(new
					{
						Success = new
						{
							Status = false,
							Msg = "关闭订单失败"
						}
					});
					context.Response.Write(s2);
					context.Response.End();
				}
			}
		}

		public void AdvanceDetails(HttpContext context)
		{
			this.CheckSession(context);
			int num = context.Request["pageIndex"].ToInt(0);
			int num2 = context.Request["pageSize"].ToInt(0);
			if (num <= 0 || num2 <= 0)
			{
				context.Response.Write(this.GetErrorJosn(101, "缺少必填参数"));
			}
			else
			{
				BalanceDetailQuery query = new BalanceDetailQuery
				{
					PageIndex = num,
					PageSize = num2,
					UserId = HiContext.Current.User.UserId
				};
				DbQueryResult balanceDetails = MemberProcessor.GetBalanceDetails(query);
				string s = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						RecordCount = balanceDetails.TotalRecords,
						List = balanceDetails.Data
					}
				});
				context.Response.Write(s);
				context.Response.End();
			}
		}

		public void AdvanceInfo(HttpContext context)
		{
			this.CheckSession(context);
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					HiContext.Current.User.UserId,
					HiContext.Current.User.IsOpenBalance,
					HiContext.Current.User.Balance,
					HiContext.Current.User.RequestBalance
				}
			});
			context.Response.Write(s);
			context.Response.End();
		}

		public bool IsFightGroupCanRefund(int fightgroupId)
		{
			FightGroupInfo fightGroup = VShopHelper.GetFightGroup(fightgroupId);
			if (fightGroup != null)
			{
				return fightGroup.Status == FightGroupStatus.FightGroupSuccess;
			}
			return true;
		}

		public void Orders(HttpContext context)
		{
			this.CheckSession(context);
			OrderStatus orderStatus = (OrderStatus)context.Request["status"].ToInt(0);
			if (orderStatus != 0 && orderStatus != OrderStatus.WaitBuyerPay && orderStatus != OrderStatus.SellerAlreadySent && orderStatus != OrderStatus.BuyerAlreadyPaid && orderStatus != OrderStatus.Finished)
			{
				context.Response.Write(this.GetErrorJosn(101, "缺少必填参数"));
			}
			else
			{
				OrderQuery orderQuery = new OrderQuery();
				orderQuery.Status = orderStatus;
				if (orderStatus == OrderStatus.Finished)
				{
					orderQuery.Status = OrderStatus.WaitReview;
				}
				orderQuery.ShowGiftOrder = true;
				orderQuery.IsServiceOrder = false;
				List<OrderInfo> listUserOrder = MemberProcessor.GetListUserOrder(HiContext.Current.UserId, orderQuery);
				DataTable dtWaitReviewOrderIds = TradeHelper.GetWaitReviewOrderIds(HiContext.Current.UserId, "");
				SiteSettings setting = SettingsManager.GetMasterSettings();
				WeiXinRedEnvelopeInfo weiXinRedEnvelopeInfo = WeiXinRedEnvelopeProcessor.GetOpenedWeiXinRedEnvelope();
				string siteUrl = (setting.SiteUrl.ToLower().StartsWith("http://") || setting.SiteUrl.ToLower().StartsWith("https://")) ? setting.SiteUrl : ("http://" + setting.SiteUrl);
				string s = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						RecordCount = listUserOrder.Count,
						List = listUserOrder.Select(delegate(OrderInfo c)
						{
							string orderId = c.OrderId;
							int orderStatus2 = this.GetOrderStatus(c);
							int allQuantity = c.GetAllQuantity(true);
							decimal total = c.GetTotal(false);
							string orderStatusText = this.GetOrderStatusText(c);
							var showRedEnvelope = new
							{
								IsShowRedEnvelope = this.GetRedEnvelopeStatus(c, weiXinRedEnvelopeInfo),
								Href = siteUrl + "/vshop/SendRedEnvelope.aspx?OrderId=" + c.OrderId,
								ShareTitle = ((weiXinRedEnvelopeInfo != null) ? weiXinRedEnvelopeInfo.ShareTitle : ""),
								ShareDetails = ((weiXinRedEnvelopeInfo != null) ? weiXinRedEnvelopeInfo.ShareDetails : ""),
								ShareIcon = ((weiXinRedEnvelopeInfo != null) ? weiXinRedEnvelopeInfo.ShareIcon : ""),
								RedEnvelopeID = ((weiXinRedEnvelopeInfo != null) ? weiXinRedEnvelopeInfo.Id.ToString() : ""),
								SendCode = Guid.NewGuid().ToString()
							};
							bool isShowClose = c.OrderStatus == OrderStatus.WaitBuyerPay && !c.DepositDate.HasValue;
							bool isShowFinishOrder = c.OrderStatus == OrderStatus.SellerAlreadySent && c.ItemStatus == OrderItemStatus.Nomarl;
							if (c.OrderStatus == OrderStatus.Finished && c.LineItems.Count > 0)
							{
								goto IL_01bb;
							}
							if (c.OrderStatus == OrderStatus.Closed && c.OnlyReturnedCount == c.LineItems.Count && c.LineItems.Count > 0)
							{
								goto IL_01bb;
							}
							int isShowPreview = 0;
							goto IL_01ff;
							IL_02a5:
							int isShowCreview;
							return new
							{
								OrderId = orderId,
								Status = orderStatus2,
								Quantity = allQuantity,
								Amount = total,
								StatusText = orderStatusText,
								ShowRedEnvelope = showRedEnvelope,
								IsShowClose = isShowClose,
								IsShowFinishOrder = isShowFinishOrder,
								IsShowPreview = ((byte)isShowPreview != 0),
								IsShowCreview = ((byte)isShowCreview != 0),
								CreviewText = ((this.siteSettings.ProductCommentPoint > 0) ? $"评价得{this.siteSettings.ProductCommentPoint * c.LineItems.Count}积分" : "评价订单"),
								ProductCommentPoint = this.siteSettings.ProductCommentPoint * c.LineItems.Count,
								IsShowRefund = ((c.FightGroupId > 0 && VShopHelper.IsFightGroupCanRefund(c.FightGroupId) && c.IsCanRefund) || (c.FightGroupId <= 0 && c.IsCanRefund)),
								IsShowReturn = false,
								IsShowTakeCodeQRCode = (!string.IsNullOrEmpty(c.TakeCode) && (c.OrderStatus == OrderStatus.BuyerAlreadyPaid || c.OrderStatus == OrderStatus.WaitBuyerPay)),
								IsShowLogistics = ((c.OrderStatus == OrderStatus.SellerAlreadySent || c.OrderStatus == OrderStatus.Finished) && !string.IsNullOrEmpty(c.ExpressCompanyName) && !string.IsNullOrEmpty(c.ShipOrderNumber)),
								ShipOrderNumber = c.ShipOrderNumber.ToNullString(),
								OrderDate = c.OrderDate.ToString("yyyy-MM-dd HH:mm:ss"),
								SupplierId = c.SupplierId,
								ShipperName = ((c.ShipperName == null) ? "" : ((c.ShipperName.Length > 12) ? (c.ShipperName.Substring(0, 12) + "...") : c.ShipperName)),
								StoreName = ((c.StoreName == null) ? "" : ((c.StoreName.Length > 12) ? (c.StoreName.Substring(0, 12) + "...") : c.StoreName)),
								IsShowCertification = (this.siteSettings.IsOpenCertification && c.IsincludeCrossBorderGoods && c.IDStatus == 0),
								LineItems = from d in c.LineItems.Keys
								select new
								{
									Status = c.LineItems[d].Status,
									StatusText = ((c.LineItems[d].Status == LineItemStatus.Normal) ? "" : c.LineItems[d].StatusSimpleText),
									Id = c.LineItems[d].SkuId,
									Name = c.LineItems[d].ItemDescription,
									Price = c.LineItems[d].ItemAdjustedPrice,
									IsShowRefund = false,
									IsShowAfterSale = ((c.OrderStatus == OrderStatus.SellerAlreadySent || (c.OrderStatus == OrderStatus.Finished && !c.IsServiceOver)) && c.CountDownBuyId == 0 && c.GroupBuyId == 0 && (c.LineItems[d].ReturnInfo == null || c.LineItems[d].ReturnInfo.HandleStatus == ReturnStatus.Refused) && (c.LineItems[d].ReplaceInfo == null || c.LineItems[d].ReplaceInfo.HandleStatus == ReplaceStatus.Refused || c.LineItems[d].ReplaceInfo.HandleStatus == ReplaceStatus.Replaced)),
									Amount = c.LineItems[d].Quantity,
									SendCount = ((c.LineItems[d].ShipmentQuantity > c.LineItems[d].Quantity) ? (c.LineItems[d].ShipmentQuantity - c.LineItems[d].Quantity) : 0),
									Image = Globals.FullPath(string.IsNullOrEmpty(c.LineItems[d].ThumbnailsUrl) ? setting.DefaultProductImage : c.LineItems[d].ThumbnailsUrl),
									SkuText = c.LineItems[d].SKUContent
								},
								Gifts = from g in c.Gifts
								select new
								{
									GiftId = g.GiftId,
									GiftName = g.GiftName,
									PromoteType = g.PromoteType,
									Quantity = g.Quantity,
									SkuId = g.SkuId,
									Image = Globals.FullPath(string.IsNullOrEmpty(g.ThumbnailsUrl) ? setting.DefaultProductImage : g.ThumbnailsUrl),
									CostPrice = g.CostPrice
								}
							};
							IL_01bb:
							isShowPreview = ((dtWaitReviewOrderIds.Rows.Count <= 0 || dtWaitReviewOrderIds.Select("OrderId = '" + c.OrderId + "'").Count() <= 0) ? 1 : 0);
							goto IL_01ff;
							IL_01ff:
							if (c.OrderStatus == OrderStatus.Finished && c.LineItems.Count > 0)
							{
								goto IL_0261;
							}
							if (c.OrderStatus == OrderStatus.Closed && c.OnlyReturnedCount == c.LineItems.Count && c.LineItems.Count > 0)
							{
								goto IL_0261;
							}
							isShowCreview = 0;
							goto IL_02a5;
							IL_0261:
							isShowCreview = ((dtWaitReviewOrderIds.Rows.Count > 0 && dtWaitReviewOrderIds.Select("OrderId = '" + c.OrderId + "'").Count() > 0) ? 1 : 0);
							goto IL_02a5;
						})
					}
				});
				context.Response.Write(s);
			}
		}

		private bool GetRedEnvelopeStatus(OrderInfo order, WeiXinRedEnvelopeInfo weiXinRedEnvelopeInfo)
		{
			if (order == null || order.LineItems.Count == 0)
			{
				return false;
			}
			if ((order.OrderStatus == OrderStatus.BuyerAlreadyPaid || order.OrderStatus == OrderStatus.Finished || order.OrderStatus == OrderStatus.WaitReview || order.OrderStatus == OrderStatus.History) && weiXinRedEnvelopeInfo != null && weiXinRedEnvelopeInfo.EnableIssueMinAmount <= order.GetPayTotal() && order.OrderDate >= weiXinRedEnvelopeInfo.ActiveStartTime && order.OrderDate <= weiXinRedEnvelopeInfo.ActiveEndTime)
			{
				return true;
			}
			return false;
		}

		private int GetOrderStatus(OrderInfo order)
		{
			int num = 0;
			if (order.ItemStatus == OrderItemStatus.Nomarl)
			{
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
			return -1;
		}

		private string GetOrderStatusText(OrderInfo order)
		{
			string result = string.Empty;
			if (order.ExpressCompanyName == "同城物流配送")
			{
				result = EnumDescription.GetEnumDescription((Enum)(object)order.DadaStatus, 0);
			}
			else if (order.ItemStatus == OrderItemStatus.Nomarl)
			{
				if (order.ShippingModeId == -2 && !order.IsConfirm && (order.OrderStatus == OrderStatus.WaitBuyerPay || order.OrderStatus == OrderStatus.BuyerAlreadyPaid) && order.ItemStatus == OrderItemStatus.Nomarl)
				{
					result = ((order.OrderStatus != OrderStatus.WaitBuyerPay || !(order.Gateway != "hishop.plugins.payment.payonstore")) ? (order.IsConfirm ? "待上门自提" : "门店配货中") : "等待买家付款");
				}
				else if (order.OrderStatus == OrderStatus.WaitBuyerPay && order.Gateway == "hishop.plugins.payment.podrequest")
				{
					result = "待发货";
				}
				else if (order.PreSaleId > 0 && order.OrderStatus == OrderStatus.WaitBuyerPay)
				{
					if (!order.DepositDate.HasValue)
					{
						result = "等待支付定金";
					}
					else
					{
						ProductPreSaleInfo productPreSaleInfo = ProductPreSaleHelper.GetProductPreSaleInfo(order.PreSaleId);
						result = ((!(productPreSaleInfo.PaymentStartDate > DateTime.Now)) ? "等待支付尾款" : "等待尾款支付开始");
					}
				}
				else
				{
					result = ((Enum)(object)order.OrderStatus).ToDescription();
				}
			}
			else
			{
				if (order.ItemStatus == OrderItemStatus.HasReplace)
				{
					result = "申请换货";
				}
				if (order.ItemStatus == OrderItemStatus.HasReturn)
				{
					result = "申请退货";
				}
				if (order.ItemStatus == OrderItemStatus.HasReturnOrReplace)
				{
					result = ((order.OrderStatus != OrderStatus.BuyerAlreadyPaid) ? "退货/换货中" : "申请退款");
				}
			}
			return result;
		}

		public void AdvanceOpen(HttpContext context)
		{
			this.CheckSession(context);
			string text = context.Request["password"].ToNullString();
			if (string.IsNullOrEmpty(text))
			{
				context.Response.Write(this.GetErrorJosn(101, "缺少必填参数"));
			}
			else
			{
				bool flag = MemberProcessor.OpenBalance(text);
				string s = JsonConvert.SerializeObject(new
				{
					Success = new
					{
						Status = flag,
						Msg = (flag ? "预付款开启成功" : "预付款开启失败")
					}
				});
				Users.ClearUserCache(HiContext.Current.UserId, HiContext.Current.User.SessionId);
				context.Response.Write(s);
			}
		}

		public void AppPushSetRead(HttpContext context)
		{
			this.CheckSession(context);
			int num = context.Request["pushRecordId"].ToInt(0);
			if (num <= 0)
			{
				context.Response.Write(this.GetErrorJosn(101, "缺少必填参数"));
			}
			else
			{
				VShopHelper.AppPushSetReadForIOS(num, HiContext.Current.User.UserId);
				this.AppPushListForUser(context);
			}
		}

		public void AppPushListForUser(HttpContext context)
		{
			this.CheckSession(context);
			List<IOSAppPushRecord> list = (from c in VShopHelper.AppPushListByUserForIOS(HiContext.Current.User.UserId)
			select new IOSAppPushRecord
			{
				Type = c.PushType,
				PushTitle = c.PushTitle,
				PushContent = c.PushContent,
				PushSendTime = c.PushSendTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
				Extras = c.Extras,
				PushRecordId = c.PushRecordId
			} into c
			orderby c.PushSendTime descending
			select c).ToList();
			list.ForEach(delegate(IOSAppPushRecord c)
			{
				List<string> list2 = c.Extras.Split(VShopHelper.SEPARATOREVERY).ToList();
				foreach (string item in list2)
				{
					char sEPARATORCONTEXT;
					if (item.Contains("url"))
					{
						string text = item;
						sEPARATORCONTEXT = VShopHelper.SEPARATORCONTEXT;
						c.Extras = text.Replace("url" + sEPARATORCONTEXT.ToString(), string.Empty);
					}
					else if (item.Contains("productid"))
					{
						string text2 = item;
						sEPARATORCONTEXT = VShopHelper.SEPARATORCONTEXT;
						c.Extras = text2.Replace("productid" + sEPARATORCONTEXT.ToString(), string.Empty);
					}
				}
			});
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					RecordCount = list.Count,
					List = list
				}
			});
			context.Response.Write(s);
			context.Response.End();
		}

		private void GetCellPhoneAndEmailByUser(HttpContext context)
		{
			this.CheckSession(context);
			MemberInfo cellPhoneAndEmailByUser = MemberProcessor.GetCellPhoneAndEmailByUser(HiContext.Current.User.UserId);
			string cellphone = string.Empty;
			string email = string.Empty;
			if (cellPhoneAndEmailByUser != null)
			{
				cellphone = (string.IsNullOrEmpty(cellPhoneAndEmailByUser.CellPhone) ? "" : cellPhoneAndEmailByUser.CellPhone);
				email = (string.IsNullOrEmpty(cellPhoneAndEmailByUser.Email) ? "" : cellPhoneAndEmailByUser.Email);
			}
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					cellphone,
					email
				}
			});
			context.Response.Write(s);
			context.Response.End();
		}

		private void ValidVerfication(HttpContext context)
		{
			string text = context.Request["value"];
			if (string.IsNullOrEmpty(text))
			{
				this.message = JsonConvert.SerializeObject(new
				{
					Success = new
					{
						Status = false,
						Msg = "输入的手机号或邮箱不允许为空"
					}
				});
			}
			string value = context.Request["verifyCode"];
			if (string.IsNullOrEmpty(value))
			{
				this.message = JsonConvert.SerializeObject(new
				{
					Success = new
					{
						Status = false,
						Msg = "验证码不能为空"
					}
				});
			}
			string text2 = HiCache.Get($"DataCache-PhoneCode-{text}").ToNullString();
			if (string.IsNullOrEmpty(text2))
			{
				text2 = HiCache.Get($"DataCache-EmailCode-{text}").ToNullString();
			}
			text2 = HiCryptographer.TryDecypt(text2);
			if (text2.Equals(value))
			{
				this.message = JsonConvert.SerializeObject(new
				{
					Success = new
					{
						Status = true,
						Msg = "验证通过"
					}
				});
			}
			else
			{
				this.message = JsonConvert.SerializeObject(new
				{
					Success = new
					{
						Status = false,
						Msg = "验证码错误"
					}
				});
			}
			context.Response.Write(this.message);
			context.Response.End();
		}

		private void GetPointList(HttpContext context)
		{
			this.CheckSession(context);
			if (string.IsNullOrEmpty(context.Request["pageNumber"].ToNullString()))
			{
				context.Response.Write(this.GetErrorJosn(101, "缺少必填参数"));
			}
			else
			{
				int pageIndex = context.Request["pageNumber"].ToInt(0);
				List<GetPointListModel> list = new List<GetPointListModel>();
				int points = HiContext.Current.User.Points;
				PointQuery pointQuery = new PointQuery();
				pointQuery.PageIndex = pageIndex;
				pointQuery.PageSize = 10;
				pointQuery.UserId = HiContext.Current.UserId;
				PageModel<PointDetailInfo> userPoints = MemberHelper.GetUserPoints(pointQuery);
				IList<PointDetailInfo> list2 = userPoints.Models.ToList();
				foreach (PointDetailInfo item in list2)
				{
					string text = "0";
					int num;
					if (item.Increased == 0)
					{
						text = "-" + item.Reduced;
					}
					else
					{
						num = item.Increased;
						text = num.ToString();
					}
					GetPointListModel getPointListModel = new GetPointListModel();
					getPointListModel.JournalNumber = item.JournalNumber.ToString();
					getPointListModel.OrderId = item.OrderId;
					GetPointListModel getPointListModel2 = getPointListModel;
					num = item.UserId;
					getPointListModel2.UserId = num.ToString();
					getPointListModel.TradeDate = item.TradeDate.ToString("yyyy-MM-dd HH:mm:ss");
					getPointListModel.TradeType = item.TradeType.ToString();
					getPointListModel.TradeTypeName = item.TradeTypeName;
					getPointListModel.Point = text;
					getPointListModel.Remark = item.Remark;
					GetPointListModel getPointListModel3 = getPointListModel;
					num = item.SignInSource;
					getPointListModel3.SignInSource = num.ToString();
					list.Add(getPointListModel);
				}
				string s = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						RecordCount = userPoints.Total,
						TotalPoints = points,
						List = list
					}
				});
				context.Response.Write(s);
				context.Response.End();
			}
		}

		private void ChangePassword(HttpContext context)
		{
			string text = context.Request["password"];
			string value = context.Request["againPassword"];
			string text2 = context.Request["userName"];
			string text3 = context.Request["verifyCode"];
			if (string.IsNullOrEmpty(text2))
			{
				string s = JsonConvert.SerializeObject(new
				{
					Success = new
					{
						Status = false,
						Msg = "手机或邮箱号不能为空"
					}
				});
				context.Response.Write(s);
				context.Response.End();
			}
			if (string.IsNullOrEmpty(text3))
			{
				string s2 = JsonConvert.SerializeObject(new
				{
					Success = new
					{
						Status = false,
						Msg = "验证码不能为空"
					}
				});
				context.Response.Write(s2);
				context.Response.End();
			}
			if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(value))
			{
				string s3 = JsonConvert.SerializeObject(new
				{
					Success = new
					{
						Status = false,
						Msg = "密码不能为空"
					}
				});
				context.Response.Write(s3);
				context.Response.End();
			}
			if (!text.Equals(value))
			{
				string s4 = JsonConvert.SerializeObject(new
				{
					Success = new
					{
						Status = false,
						Msg = "两次输入的密码需一致"
					}
				});
				context.Response.Write(s4);
				context.Response.End();
			}
			MemberDao memberDao = new MemberDao();
			MemberInfo memberInfo = null;
			if (DataHelper.IsEmail(text2))
			{
				memberInfo = memberDao.FindMemberByEmail(text2);
			}
			else if (DataHelper.IsMobile(text2))
			{
				memberInfo = memberDao.FindMemberByCellphone(text2);
			}
			if (memberInfo == null)
			{
				string s5 = JsonConvert.SerializeObject(new
				{
					Success = new
					{
						Status = false,
						Msg = "账号不存在"
					}
				});
				context.Response.Write(s5);
				context.Response.End();
			}
			else
			{
				bool flag = false;
				string msg = "验证码错误";
				if (DataHelper.IsEmail(text2))
				{
					flag = HiContext.Current.AppCheckEmailVerifyCode(text2, text3);
				}
				else if (DataHelper.IsMobile(text2))
				{
					flag = HiContext.Current.AppCheckVerifyCode(text2, text3, out msg);
				}
				if (flag)
				{
					if (MemberProcessor.ChangePassword(memberInfo, text))
					{
						string s6 = JsonConvert.SerializeObject(new
						{
							Success = new
							{
								Status = true,
								Msg = "修改密码成功"
							}
						});
						context.Response.Write(s6);
						context.Response.End();
					}
					else
					{
						string s7 = JsonConvert.SerializeObject(new
						{
							Success = new
							{
								Status = false,
								Msg = "修改密码失败"
							}
						});
						context.Response.Write(s7);
						context.Response.End();
					}
				}
				else
				{
					string s8 = JsonConvert.SerializeObject(new
					{
						Success = new
						{
							Status = false,
							Msg = msg
						}
					});
					context.Response.Write(s8);
					context.Response.End();
				}
			}
		}

		private bool CheckRegisteParam(HttpContext context)
		{
			string text = context.Request["userName"];
			string text2 = context.Request["password"];
			string text3 = context.Request["verifyCode"];
			MemberDao memberDao = new MemberDao();
			if (string.IsNullOrEmpty(text))
			{
				context.Response.Write(this.GetErrorJosn(101, "用户名不能为空"));
				return false;
			}
			if (memberDao.GetMember(text) != null)
			{
				context.Response.Write(this.GetErrorJosn(101, "已经存在相同的用户名"));
				return false;
			}
			if (this.emailR.IsMatch(text))
			{
				MemberInfo memberInfo = MemberProcessor.FindMemberByUsername(text);
				if (memberDao.MemberEmailIsExist(text) || memberInfo != null)
				{
					context.Response.Write(this.GetErrorJosn(101, "此邮箱已被使用"));
					return false;
				}
				if (this.siteSettings.IsNeedValidEmail && !HiContext.Current.AppCheckEmailVerifyCode(text, text3))
				{
					context.Response.Write(this.GetErrorJosn(101, "邮箱验证码错误"));
					return false;
				}
			}
			if (this.cellphoneR.IsMatch(text))
			{
				MemberInfo memberInfo2 = MemberProcessor.FindMemberByUsername(text);
				if (memberDao.MemberCellphoneIsExist(text) || memberInfo2 != null)
				{
					context.Response.Write(this.GetErrorJosn(101, "此手机已被使用"));
					return false;
				}
				if (string.IsNullOrEmpty(text3))
				{
					context.Response.Write(this.GetErrorJosn(101, "验证码不能为空"));
					return false;
				}
				string errorMsg = "";
				if (!HiContext.Current.AppCheckVerifyCode(text, text3.Trim(), out errorMsg))
				{
					context.Response.Write(this.GetErrorJosn(101, errorMsg));
					return false;
				}
			}
			if (text.Trim().Length < 6 || text.Trim().Length > 50)
			{
				context.Response.Write(this.GetErrorJosn(101, "用户名不能为空，且在6-50个字符之间"));
				return false;
			}
			if (text2.Length == 0)
			{
				context.Response.Write(this.GetErrorJosn(101, "密码不能为空"));
				return false;
			}
			if (text2.Length < 6 || text2.Length > HiConfiguration.GetConfig().PasswordMaxLength)
			{
				context.Response.Write(this.GetErrorJosn(101, $"密码的长度只能在{6}和{HiConfiguration.GetConfig().PasswordMaxLength}个字符之间"));
				return false;
			}
			return true;
		}

		private void GetSMSEnabledState(HttpContext context)
		{
			SiteSettings siteSettings = HiContext.Current.SiteSettings;
			if (!siteSettings.SMSEnabled || string.IsNullOrEmpty(siteSettings.SMSSettings))
			{
				this.message = JsonConvert.SerializeObject(new
				{
					Success = new
					{
						Status = false,
						Msg = "短信服务未配置"
					}
				});
			}
			else
			{
				this.message = JsonConvert.SerializeObject(new
				{
					Success = new
					{
						Status = true,
						Msg = "短信服务已配置"
					}
				});
			}
			context.Response.Write(this.message);
			context.Response.End();
		}

		private void AppSendVerifyCode(HttpContext context)
		{
			string text = context.Request["Phone"];
			string verifyCode = context.Request["imgCode"].ToNullString();
			bool flag = true;
			if (!string.IsNullOrEmpty(context.Request["imgCode"]) && context.Request["imgCode"] == "0")
			{
				flag = false;
			}
			if (flag && !HiContext.Current.CheckVerifyCode(verifyCode, ""))
			{
				this.message = "{\"success\":\"false\",\"msg\":\"图形验证码错误\"}";
				context.Response.Write(this.message);
				context.Response.End();
			}
			else
			{
				string code = HiContext.Current.CreatePhoneCode(4, text, VerifyCodeType.Digital);
				SiteSettings siteSettings = HiContext.Current.SiteSettings;
				if (!siteSettings.SMSEnabled || string.IsNullOrEmpty(siteSettings.SMSSettings))
				{
					this.message = JsonConvert.SerializeObject(new
					{
						Success = new
						{
							Status = false,
							Msg = "短信服务未配置"
						}
					});
					context.Response.Write(this.message);
					context.Response.End();
				}
				this.message = this.SendVerifyCode(siteSettings, text, code, context.Session.SessionID);
				context.Response.Write(this.message);
				context.Response.End();
			}
		}

		private string SendVerifyCode(SiteSettings settings, string cellphone, string code, string sessionID)
		{
			try
			{
                string TemplateCode = "";
                string iPAddress = Globals.IPAddress;
				if (!new MemberDao().ValidateIPCanSendSMS(iPAddress, settings.IPSMSCount))
				{
					this.message = JsonConvert.SerializeObject(new
					{
						Success = new
						{
							Status = false,
							Msg = "您今天已发送了" + settings.IPSMSCount + "次验证码,请明天再重试！"
						}
					});
					return this.message;
				}
				int phoneSendSmsTimes = new MemberDao().GetPhoneSendSmsTimes(cellphone);
				if (phoneSendSmsTimes >= settings.PhoneSMSCount)
				{
					this.message = JsonConvert.SerializeObject(new
					{
						Success = new
						{
							Status = false,
							Msg = "该手机号今天已发送了" + settings.PhoneSMSCount + "次验证码,请明天再重试！"
						}
					});
					return this.message;
				}
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
					this.message = JsonConvert.SerializeObject(new
					{
						Success = new
						{
							Status = false,
							Msg = "验证码发送时间，间隔为120秒，请稍后重试！"
						}
					});
					return this.message;
				}
				object obj2 = HiCache.Get($"DataCache-SendSMSTimesCacheKey-{sessionID}");
				if (obj2 != null)
				{
					int.TryParse(obj2.ToString(), out num);
				}
				if (num >= 10)
				{
					this.message = JsonConvert.SerializeObject(new
					{
						Success = new
						{
							Status = false,
							Msg = "您今天已发送了10次验证码,请明天再重试！"
						}
					});
					return this.message;
				}
				ConfigData configData = new ConfigData(HiCryptographer.TryDecypt(settings.SMSSettings));
				SMSSender sMSSender = SMSSender.CreateInstance(settings.SMSSender, configData.SettingsXml);
				string text = string.Format("{\"code\":\"{0}\"}", code);
				string msg ="";
				if (sMSSender.Send(cellphone, TemplateCode, text, out msg,"", "2"))
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
					msg = "短信发送成功";
					new MemberDao().SaveSmsIp(iPAddress);
					new MemberDao().SavePhoneSendTimes(cellphone);
					this.message = JsonConvert.SerializeObject(new
					{
						Success = new
						{
							Status = true,
							Msg = msg
						}
					});
				}
				else
				{
					msg = "短信发送失败";
					this.message = JsonConvert.SerializeObject(new
					{
						Success = new
						{
							Status = false,
							Msg = msg
						}
					});
				}
				return this.message;
			}
			catch (Exception)
			{
				this.message = JsonConvert.SerializeObject(new
				{
					Success = new
					{
						Status = false,
						Msg = "未知错误"
					}
				});
				return this.message;
			}
		}

		private void VerficationPhoneOrEmail(HttpContext context)
		{
			string text = context.Request["value"];
			string a = "";
			SiteSettings siteSettings = HiContext.Current.SiteSettings;
			if (string.IsNullOrEmpty(text))
			{
				this.message = JsonConvert.SerializeObject(new
				{
					Success = new
					{
						Status = false,
						Msg = "输入的手机号或邮箱不允许为空"
					}
				});
				context.Response.Write(this.message);
				context.Response.End();
			}
			if (!DataHelper.IsMobile(text) && !DataHelper.IsEmail(text))
			{
				this.message = JsonConvert.SerializeObject(new
				{
					Success = new
					{
						Status = false,
						Msg = "请输入正确的手机号或邮箱账号"
					}
				});
				context.Response.Write(this.message);
				context.Response.End();
			}
			if (DataHelper.IsEmail(text) && MemberProcessor.IsUseEmail(text))
			{
				a = "email";
			}
			else if (DataHelper.IsMobile(text) && MemberProcessor.IsUseCellphone(text))
			{
				a = "phone";
			}
			else
			{
				this.message = JsonConvert.SerializeObject(new
				{
					Success = new
					{
						Status = false,
						Msg = "该手机号或邮箱绑定的账号不存在"
					}
				});
				context.Response.Write(this.message);
				context.Response.End();
			}
			string empty = string.Empty;
			empty = HiContext.Current.CreateVerifyCode(4, VerifyCodeType.Digital, "");
			if (a == "phone")
			{
				if (!siteSettings.SMSEnabled || string.IsNullOrEmpty(siteSettings.SMSSettings))
				{
					this.message = JsonConvert.SerializeObject(new
					{
						Success = new
						{
							Status = false,
							Msg = "短信服务未配置"
						}
					});
					context.Response.Write(this.message);
					context.Response.End();
				}
				this.message = this.SendVerifyCode(siteSettings, text, empty, context.Session.SessionID);
			}
			else if (a == "email")
			{
				if (!siteSettings.EmailEnabled || string.IsNullOrEmpty(siteSettings.EmailSettings))
				{
					this.message = JsonConvert.SerializeObject(new
					{
						Success = new
						{
							Status = false,
							Msg = "邮件服务未配置"
						}
					});
					context.Response.Write(this.message);
					context.Response.End();
				}
				this.message = this.SendEmail(siteSettings, text, empty, context.Session.SessionID);
			}
			context.Response.Write(this.message);
			context.Response.End();
		}

		private void VerficationPhoneOrEmailNoValid(HttpContext context)
		{
			string text = context.Request["value"];
			string sessionId = context.Request["SessionId"].ToNullString();
			string a = "";
			SiteSettings siteSettings = HiContext.Current.SiteSettings;
			MemberInfo memberInfo = MemberProcessor.FindMemberBySessionId(sessionId);
			string b = "";
			string b2 = "";
			if (memberInfo != null)
			{
				b = memberInfo.CellPhone;
				b2 = memberInfo.Email;
			}
			if (string.IsNullOrEmpty(text))
			{
				this.message = JsonConvert.SerializeObject(new
				{
					Success = new
					{
						Status = false,
						Msg = "输入的手机号或邮箱不允许为空"
					}
				});
				context.Response.Write(this.message);
				context.Response.End();
			}
			if (DataHelper.IsMobile(text))
			{
				a = "phone";
				if (text != b && MemberProcessor.IsUseCellphone(text))
				{
					this.message = JsonConvert.SerializeObject(new
					{
						Success = new
						{
							Status = false,
							Msg = "该手机号码已被其他账号使用"
						}
					});
					context.Response.Write(this.message);
					context.Response.End();
				}
			}
			else if (DataHelper.IsEmail(text))
			{
				a = "email";
				if (text != b2 && MemberProcessor.IsUseEmail(text))
				{
					this.message = JsonConvert.SerializeObject(new
					{
						Success = new
						{
							Status = false,
							Msg = "该邮箱地址已被其他账号使用"
						}
					});
					context.Response.Write(this.message);
					context.Response.End();
				}
			}
			else
			{
				this.message = JsonConvert.SerializeObject(new
				{
					Success = new
					{
						Status = false,
						Msg = "请输入正确的手机号或邮箱账号"
					}
				});
				context.Response.Write(this.message);
				context.Response.End();
			}
			string code = HiContext.Current.CreateVerifyCode(4, VerifyCodeType.Digital, "");
			if (a == "phone")
			{
				if (!siteSettings.SMSEnabled || string.IsNullOrEmpty(siteSettings.SMSSettings))
				{
					this.message = JsonConvert.SerializeObject(new
					{
						Success = new
						{
							Status = false,
							Msg = "短信服务未配置"
						}
					});
					context.Response.Write(this.message);
					context.Response.End();
				}
				this.message = this.SendVerifyCode(siteSettings, text, code, context.Session.SessionID);
			}
			else if (a == "email")
			{
				if (!siteSettings.EmailEnabled || string.IsNullOrEmpty(siteSettings.EmailSettings))
				{
					this.message = JsonConvert.SerializeObject(new
					{
						Success = new
						{
							Status = false,
							Msg = "邮件服务未配置"
						}
					});
					context.Response.Write(this.message);
					context.Response.End();
				}
				this.message = this.SendEmail(siteSettings, text, code, context.Session.SessionID);
			}
			context.Response.Write(this.message);
			context.Response.End();
		}

		private void SendEmailVerifyCode(HttpContext context)
		{
			string text = context.Request["value"];
			SiteSettings siteSettings = HiContext.Current.SiteSettings;
			if (!DataHelper.IsEmail(text))
			{
				this.message = JsonConvert.SerializeObject(new
				{
					Success = new
					{
						Status = false,
						Msg = "邮箱格式不正确"
					}
				});
				context.Response.Write(this.message);
				context.Response.End();
			}
			else if (MemberProcessor.IsUseEmail(text))
			{
				this.message = JsonConvert.SerializeObject(new
				{
					Success = new
					{
						Status = false,
						Msg = "该邮箱地址已被其他账号使用"
					}
				});
				context.Response.Write(this.message);
				context.Response.End();
			}
			if (!siteSettings.EmailEnabled || string.IsNullOrEmpty(siteSettings.EmailSettings))
			{
				this.message = JsonConvert.SerializeObject(new
				{
					Success = new
					{
						Status = false,
						Msg = "邮件服务未配置"
					}
				});
				context.Response.Write(this.message);
				context.Response.End();
			}
			string code = HiContext.Current.CreateVerifyCode(4, VerifyCodeType.Digital, "");
			this.message = this.SendEmail(siteSettings, text, code, context.Session.SessionID);
			context.Response.Write(this.message);
			context.Response.End();
		}

		private string SendEmail(SiteSettings settings, string email, string code, string sessionId)
		{
			string text = "";
			try
			{
				int num = 0;
				DateTime dateTime = DateTime.Now;
				DateTime value = dateTime.AddSeconds(-121.0);
				object obj = HiCache.Get($"DataCache-LastSendMailTimeCacheKey-{sessionId}");
				if (obj != null)
				{
					DateTime.TryParse(obj.ToString(), out value);
				}
				dateTime = DateTime.Now;
				TimeSpan timeSpan = dateTime.Subtract(value);
				if (timeSpan.TotalSeconds < 120.0)
				{
					return JsonConvert.SerializeObject(new
					{
						Success = new
						{
							Status = false,
							Msg = "验证码发送时间，间隔为120秒，请稍后重试！"
						}
					});
				}
				ConfigData configData = new ConfigData(HiCryptographer.TryDecypt(settings.EmailSettings));
				string body = string.Format("尊敬的会员{0}您好：欢迎使用" + settings.SiteName + "系统，此次验证码为：{1},请在2分钟内完成验证", email, code);
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
					text = JsonConvert.SerializeObject(new
					{
						Success = new
						{
							Status = true,
							Msg = "发送邮件成功，请查收"
						}
					});
					string key = $"DataCache-LastSendMailTimeCacheKey-{sessionId}";
					object obj2 = DateTime.Now;
					dateTime = DateTime.Now;
					dateTime = dateTime.Date;
					dateTime = dateTime.AddDays(1.0);
					timeSpan = dateTime.Subtract(DateTime.Now);
					HiCache.Insert(key, obj2, (int)timeSpan.TotalSeconds);
					HiCache.Insert($"DataCache-EmailCode-{email}", HiCryptographer.Encrypt(code), 120);
					return text;
				}
				return JsonConvert.SerializeObject(new
				{
					Success = new
					{
						Status = false,
						Msg = "发送邮件失败，请检查邮箱账号是否存在"
					}
				});
			}
			catch (Exception)
			{
				return JsonConvert.SerializeObject(new
				{
					Success = new
					{
						Status = false,
						Msg = "发送失败，请检查邮箱账号是否存在"
					}
				});
			}
		}

		private void GetDepletePoints(HttpContext context)
		{
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					Points = this.siteSettings.AppDepletePoints
				}
			});
			context.Response.Write(s);
			context.Response.End();
		}

		private void SignIn(HttpContext context)
		{
			this.CheckSession(context);
			MemberInfo user = HiContext.Current.User;
			context.Response.ContentType = "application/json";
			string source = context.Request["SignInSource"];
			int signInPoint = this.siteSettings.SignInPoint;
			int continuousDays = this.siteSettings.ContinuousDays;
			int continuousPoint = this.siteSettings.ContinuousPoint;
			int num = APPHelper.UserSignIn(user.UserId, continuousDays);
			if (num > 0)
			{
				int num2 = 0;
				if (signInPoint > 0)
				{
					AppShopHandler.AddPoints(user, signInPoint, PointTradeType.SignIn, source);
				}
				if (continuousDays > 0 || continuousPoint > 0)
				{
					num2 = APPHelper.GetContinuousDays(user.UserId);
					if (num2 == 0)
					{
						AppShopHandler.AddPoints(user, continuousPoint, PointTradeType.ContinuousSign, source);
					}
				}
				string s = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						status = 1,
						points = signInPoint,
						continuDays = num2,
						settingDays = continuousDays,
						continuPoints = continuousPoint,
						integral = user.Points
					}
				});
				Users.ClearUserCache(HiContext.Current.UserId, context.Request["sessionid"].ToNullString());
				context.Response.Write(s);
				context.Response.End();
			}
			else
			{
				string s2 = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						status = 2,
						points = 0,
						integral = user.Points
					}
				});
				context.Response.Write(s2);
				context.Response.End();
			}
		}

		public void UserGetCoupon(HttpContext context)
		{
			this.CheckSession(context);
			string s = context.Request["CouponId"];
			MemberInfo user = Users.GetUser(HiContext.Current.UserId);
			int num = 0;
			if (!int.TryParse(s, out num) || num <= 0)
			{
				context.Response.Write(this.GetErrorJosn(601, ((Enum)(object)ApiErrorCode.CouponParamter_Error).ToDescription()));
				context.Response.End();
			}
			switch (CouponHelper.UserGetCoupon(user, num))
			{
			case CouponActionStatus.Success:
			{
				string s2 = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						Status = "SUCCESS"
					}
				});
				context.Response.Write(s2);
				context.Response.End();
				break;
			}
			case CouponActionStatus.NotExists:
				context.Response.Write(this.GetErrorJosn(602, ((Enum)(object)ApiErrorCode.CouponNotExists_Error).ToDescription()));
				context.Response.End();
				break;
			case CouponActionStatus.InconsistentInformationUser:
				context.Response.Write(this.GetErrorJosn(603, ((Enum)(object)ApiErrorCode.CouponUserInfo_Error).ToDescription()));
				context.Response.End();
				break;
			case CouponActionStatus.InadequateInventory:
				context.Response.Write(this.GetErrorJosn(604, ((Enum)(object)ApiErrorCode.CouponNotStock_Error).ToDescription()));
				context.Response.End();
				break;
			case CouponActionStatus.CannotReceive:
			{
				CouponInfo eFCoupon = CouponHelper.GetEFCoupon(num);
				context.Response.Write(this.GetErrorJosn(607, "每人限领" + eFCoupon.UserLimitCount + "张"));
				context.Response.End();
				break;
			}
			case CouponActionStatus.CanNotGet:
				context.Response.Write(this.GetErrorJosn(605, ((Enum)(object)ApiErrorCode.CouponNotSupportGet_Error).ToDescription()));
				context.Response.End();
				break;
			case CouponActionStatus.Overdue:
				context.Response.Write(this.GetErrorJosn(606, ((Enum)(object)ApiErrorCode.CouponOverDue_Error).ToDescription()));
				context.Response.End();
				break;
			default:
				context.Response.Write(this.GetErrorJosn(999, ((Enum)(object)ApiErrorCode.Unknown_Error).ToDescription()));
				context.Response.End();
				break;
			}
		}

		private void LotteryDraw(HttpContext context)
		{
			this.CheckSession(context);
			if (!this.siteSettings.EnableAppShake)
			{
				context.Response.Write(this.GetErrorJosn(502, "商城已关闭摇一摇抽奖活动"));
				context.Response.End();
			}
			MemberInfo user = HiContext.Current.User;
			if (user.Points < this.siteSettings.AppDepletePoints)
			{
				context.Response.Write(this.GetErrorJosn(501, "积分不足"));
				context.Response.End();
			}
			int lotteryDrawResult = this.GetLotteryDrawResult();
			int num = 1;
			int prize = this.GetPrize(lotteryDrawResult, out num);
			switch (num)
			{
			case 1:
			{
				AppShopHandler.AddPoints(user, prize, PointTradeType.LotteryDrawReduced);
				AppLotteryDraw appLotteryDraw2 = new AppLotteryDraw();
				appLotteryDraw2.CreatTime = DateTime.Now;
				appLotteryDraw2.DrawType = 1;
				appLotteryDraw2.Content = prize + "积分";
				appLotteryDraw2.DrawValue = prize;
				appLotteryDraw2.UserId = user.UserId;
				APPHelper.SaveAppLotteryDraw(appLotteryDraw2);
				this.ReducedPoints(user);
				string s2 = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						integral = user.Points,
						type = num,
						content = prize,
						times = ((this.siteSettings.AppDepletePoints > 0) ? (user.Points / this.siteSettings.AppDepletePoints) : 0)
					}
				});
				Users.ClearUserCache(HiContext.Current.UserId, context.Request["sessionid"].ToNullString());
				context.Response.Write(s2);
				context.Response.End();
				break;
			}
			case 2:
			{
				CouponInfo coupon = CouponHelper.GetCoupon(prize);
				if (coupon != null)
				{
					CouponActionStatus couponActionStatus = CouponHelper.AddCouponItemInfo(user, coupon.CouponId);
					if (couponActionStatus == CouponActionStatus.Success)
					{
						AppLotteryDraw appLotteryDraw = new AppLotteryDraw();
						appLotteryDraw.CreatTime = DateTime.Now;
						appLotteryDraw.DrawType = 2;
						appLotteryDraw.Content = "价值" + coupon.Price + "元的优惠券";
						appLotteryDraw.DrawValue = (int)coupon.Price;
						appLotteryDraw.UserId = user.UserId;
						APPHelper.SaveAppLotteryDraw(appLotteryDraw);
						this.ReducedPoints(user);
						string s = JsonConvert.SerializeObject(new
						{
							Result = new
							{
								integral = user.Points,
								type = num,
								name = coupon.CouponName,
								content = (int)coupon.Price,
								times = ((this.siteSettings.AppDepletePoints > 0) ? (user.Points / this.siteSettings.AppDepletePoints) : 0)
							}
						});
						Users.ClearUserCache(HiContext.Current.UserId, context.Request["sessionid"].ToNullString());
						context.Response.Write(s);
						context.Response.End();
					}
					else
					{
						string text = "";
						switch (couponActionStatus)
						{
						case CouponActionStatus.NotExists:
							text = "该优惠券已下线,不扣除积分";
							break;
						case CouponActionStatus.Overdue:
							text = "该优惠券已过期,不扣除积分";
							break;
						case CouponActionStatus.InadequateInventory:
							text = "该优惠券被领取完了,不扣除积分";
							break;
						case CouponActionStatus.CannotReceive:
							text = "该优惠券领取数量有限,不扣除积分";
							break;
						default:
							text = "未知错误,不扣除积分";
							break;
						}
						context.Response.Write(this.GetErrorJosn(500, text));
						context.Response.End();
					}
				}
				else
				{
					context.Response.Write(this.GetErrorJosn(500, "商城配置错误"));
					context.Response.End();
				}
				break;
			}
			default:
				context.Response.Write(this.GetErrorJosn(500, "商城配置错误"));
				context.Response.End();
				break;
			}
		}

		private static void AddPoints(MemberInfo member, int points, PointTradeType type)
		{
			AppShopHandler.AddPoints(member, points, type, "");
		}

		private static void AddPoints(MemberInfo member, int points, PointTradeType type, string source)
		{
			if (points > 0)
			{
				PointDetailDao pointDetailDao = new PointDetailDao();
				PointDetailInfo pointDetailInfo = new PointDetailInfo();
				pointDetailInfo.UserId = member.UserId;
				pointDetailInfo.TradeDate = DateTime.Now;
				pointDetailInfo.TradeType = type;
				if (PointTradeType.LotteryDrawReduced == type)
				{
					pointDetailInfo.Remark = "抽奖获得积分";
				}
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
		}

		private void ReducedPoints(MemberInfo member)
		{
			if (this.siteSettings.AppDepletePoints > 0)
			{
				PointDetailDao pointDetailDao = new PointDetailDao();
				PointDetailInfo pointDetailInfo = new PointDetailInfo();
				pointDetailInfo.UserId = member.UserId;
				pointDetailInfo.TradeDate = DateTime.Now;
				pointDetailInfo.TradeType = PointTradeType.LotteryDrawReduced;
				pointDetailInfo.Reduced = this.siteSettings.AppDepletePoints;
				pointDetailInfo.Remark = "抽奖消耗积分";
				pointDetailInfo.Points = member.Points - this.siteSettings.AppDepletePoints;
				if (pointDetailInfo.Points > 2147483647)
				{
					pointDetailInfo.Points = 2147483647;
				}
				if (pointDetailInfo.Points < 0)
				{
					pointDetailInfo.Points = 0;
				}
				member.Points = pointDetailInfo.Points;
				pointDetailDao.Add(pointDetailInfo, null);
			}
		}

		private int GetLotteryDrawResult()
		{
			float[] source = new float[4]
			{
				(float)this.siteSettings.AppFirstPrizePercent,
				(float)this.siteSettings.AppSecondPrizePercent,
				(float)this.siteSettings.AppThirdPrizePercent,
				(float)this.siteSettings.AppFourPrizePercent
			};
			int result = 0;
			int maxValue = 100000;
			float num = (float)AppShopHandler.r.Next(0, maxValue) / 1000f;
			for (int i = 1; i <= 4; i++)
			{
				float num2 = source.Take(i - 1).Sum();
				float num3 = source.Take(i).Sum();
				if (num >= num2 && num < num3)
				{
					result = i;
					break;
				}
			}
			return result;
		}

		private int GetPrize(int ranking, out int prizeType)
		{
			if (ranking == 1)
			{
				if (this.siteSettings.AppFirstPrizeType == 1)
				{
					prizeType = 1;
					return this.siteSettings.AppFirstPrizePoints;
				}
				if (this.siteSettings.AppFirstPrizeType == 2)
				{
					prizeType = 2;
					return this.siteSettings.AppFirstPrizeCouponId;
				}
			}
			if (ranking == 2)
			{
				if (this.siteSettings.AppSecondPrizeType == 1)
				{
					prizeType = 1;
					return this.siteSettings.AppSecondPrizePoints;
				}
				if (this.siteSettings.AppSecondPrizeType == 2)
				{
					prizeType = 2;
					return this.siteSettings.AppSecondPrizeCouponId;
				}
			}
			if (ranking == 3)
			{
				if (this.siteSettings.AppThirdPrizeType == 1)
				{
					prizeType = 1;
					return this.siteSettings.AppThirdPrizePoints;
				}
				if (this.siteSettings.AppThirdPrizeType == 2)
				{
					prizeType = 2;
					return this.siteSettings.AppThirdPrizeCouponId;
				}
			}
			if (ranking == 4)
			{
				if (this.siteSettings.AppFourPrizeType == 1)
				{
					prizeType = 1;
					return this.siteSettings.AppFourPrizePoints;
				}
				if (this.siteSettings.AppFourPrizeType == 2)
				{
					prizeType = 2;
					return this.siteSettings.AppFourPrizeCouponId;
				}
			}
			prizeType = 0;
			return 0;
		}

		private void GetStartImg(HttpContext context)
		{
			int num = context.Request["Client"].ToInt(0);
			if (num == 1)
			{
				this.message = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						StartImg = Globals.FullPath(this.siteSettings.AndroidStartImg)
					}
				});
			}
			else
			{
				this.message = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						StartImg = Globals.FullPath(this.siteSettings.IOSStartImg)
					}
				});
			}
			context.Response.Write(this.message);
			context.Response.End();
		}

		private string GetImageFullPath(string imageUrl)
		{
			if (string.IsNullOrEmpty(imageUrl))
			{
				return Globals.FullPath(HiContext.Current.SiteSettings.DefaultProductThumbnail1);
			}
			if (imageUrl.StartsWith("http://"))
			{
				return imageUrl;
			}
			return Globals.FullPath(imageUrl);
		}

		public void GetProductDetail(HttpContext context)
		{
			GetProductDetailModel getProductDetailModel = new GetProductDetailModel();
			int num = context.Request["ProductID"].ToInt(0);
			int gradeId = context.Request["GradeId"].ToInt(0);
			string text = context.Request["LikePid"].ToNullString();
			List<int> list = null;
			if (!string.IsNullOrEmpty(text))
			{
				list = new List<int>();
				string[] array = text.Split(',');
				foreach (string obj in array)
				{
					if (obj.ToInt(0) > 0)
					{
						list.Add(obj.ToInt(0));
					}
				}
			}
			if (num <= 0)
			{
				context.Response.Write(this.GetErrorJosn(100, "商品不存在或被删除"));
			}
			else
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				ProductBrowseInfo wAPProductBrowseInfo = ProductBrowser.GetWAPProductBrowseInfo(num, null, masterSettings.OpenMultStore, gradeId);
				if (wAPProductBrowseInfo.Product == null || wAPProductBrowseInfo.Product.SaleStatus == ProductSaleStatus.Delete)
				{
					context.Response.Write(this.GetErrorJosn(100, "商品不存在或被删除"));
				}
				else
				{
					string text2 = context.Request["SessionID"];
					bool flag = false;
					flag = (!string.IsNullOrEmpty(text2) && this.ValidLoginBySessionID(text2) && ProductBrowser.ExistsProduct(num, HiContext.Current.UserId, 0));
					if (string.IsNullOrEmpty(wAPProductBrowseInfo.Product.ImageUrl1) && string.IsNullOrEmpty(wAPProductBrowseInfo.Product.ImageUrl2) && string.IsNullOrEmpty(wAPProductBrowseInfo.Product.ImageUrl3) && string.IsNullOrEmpty(wAPProductBrowseInfo.Product.ImageUrl4) && string.IsNullOrEmpty(wAPProductBrowseInfo.Product.ImageUrl5))
					{
						wAPProductBrowseInfo.Product.ImageUrl1 = Globals.FullPath(masterSettings.DefaultProductImage);
						wAPProductBrowseInfo.Product.ThumbnailUrl40 = Globals.FullPath(masterSettings.DefaultProductThumbnail1);
						wAPProductBrowseInfo.Product.ThumbnailUrl60 = Globals.FullPath(masterSettings.DefaultProductThumbnail2);
						wAPProductBrowseInfo.Product.ThumbnailUrl100 = Globals.FullPath(masterSettings.DefaultProductThumbnail3);
						wAPProductBrowseInfo.Product.ThumbnailUrl160 = Globals.FullPath(masterSettings.DefaultProductThumbnail4);
						wAPProductBrowseInfo.Product.ThumbnailUrl180 = Globals.FullPath(masterSettings.DefaultProductThumbnail5);
						wAPProductBrowseInfo.Product.ThumbnailUrl220 = Globals.FullPath(masterSettings.DefaultProductThumbnail6);
						wAPProductBrowseInfo.Product.ThumbnailUrl310 = Globals.FullPath(masterSettings.DefaultProductThumbnail7);
						wAPProductBrowseInfo.Product.ThumbnailUrl410 = Globals.FullPath(masterSettings.DefaultProductThumbnail8);
					}
					else
					{
						wAPProductBrowseInfo.Product.ImageUrl1 = Globals.FullPath(wAPProductBrowseInfo.Product.ImageUrl1);
						wAPProductBrowseInfo.Product.ImageUrl2 = Globals.FullPath(wAPProductBrowseInfo.Product.ImageUrl2);
						wAPProductBrowseInfo.Product.ImageUrl3 = Globals.FullPath(wAPProductBrowseInfo.Product.ImageUrl3);
						wAPProductBrowseInfo.Product.ImageUrl4 = Globals.FullPath(wAPProductBrowseInfo.Product.ImageUrl4);
						wAPProductBrowseInfo.Product.ImageUrl5 = Globals.FullPath(wAPProductBrowseInfo.Product.ImageUrl5);
						wAPProductBrowseInfo.Product.ThumbnailUrl60 = Globals.FullPath(wAPProductBrowseInfo.Product.ThumbnailUrl60);
						wAPProductBrowseInfo.Product.ThumbnailUrl100 = Globals.FullPath(wAPProductBrowseInfo.Product.ThumbnailUrl100);
						wAPProductBrowseInfo.Product.ThumbnailUrl180 = Globals.FullPath(wAPProductBrowseInfo.Product.ThumbnailUrl180);
						wAPProductBrowseInfo.Product.ThumbnailUrl220 = Globals.FullPath(wAPProductBrowseInfo.Product.ThumbnailUrl220);
						wAPProductBrowseInfo.Product.ThumbnailUrl310 = Globals.FullPath(wAPProductBrowseInfo.Product.ThumbnailUrl310);
						wAPProductBrowseInfo.Product.ThumbnailUrl40 = Globals.FullPath(wAPProductBrowseInfo.Product.ThumbnailUrl40);
						wAPProductBrowseInfo.Product.ThumbnailUrl410 = Globals.FullPath(wAPProductBrowseInfo.Product.ThumbnailUrl410);
					}
					MemberGradeInfo memberGrade = MemberProcessor.GetMemberGrade(gradeId);
					if (memberGrade != null)
					{
						getProductDetailModel.GradeName = memberGrade.Name.ToNullString();
					}
					else
					{
						getProductDetailModel.GradeName = "";
					}
					getProductDetailModel.ProductName = wAPProductBrowseInfo.Product.ProductName;
					getProductDetailModel.MetaDescription = wAPProductBrowseInfo.Product.Meta_Description;
					getProductDetailModel.ShortDescription = wAPProductBrowseInfo.Product.ShortDescription;
					GetProductDetailModel getProductDetailModel2 = getProductDetailModel;
					int num2 = wAPProductBrowseInfo.Product.SaleCounts;
					getProductDetailModel2.SaleCounts = num2.ToString();
					GetProductDetailModel getProductDetailModel3 = getProductDetailModel;
					num2 = wAPProductBrowseInfo.Product.ShowSaleCounts;
					getProductDetailModel3.ShowSaleCounts = num2.ToString();
					getProductDetailModel.Weight = wAPProductBrowseInfo.Product.Weight.ToString();
					GetProductDetailModel getProductDetailModel4 = getProductDetailModel;
					num2 = wAPProductBrowseInfo.Product.VistiCounts;
					getProductDetailModel4.VistiCounts = num2.ToString();
					getProductDetailModel.CostPrice = wAPProductBrowseInfo.Product.CostPrice.F2ToString("f2");
					getProductDetailModel.MarketPrice = wAPProductBrowseInfo.Product.MarketPrice.ToDecimal(0).F2ToString("f2");
					getProductDetailModel.IsfreeShipping = wAPProductBrowseInfo.Product.IsfreeShipping.ToString();
					getProductDetailModel.MaxSalePrice = wAPProductBrowseInfo.Product.MaxSalePrice.F2ToString("f2");
					getProductDetailModel.MinSalePrice = wAPProductBrowseInfo.Product.MinSalePrice.F2ToString("f2");
					getProductDetailModel.IsFavorite = (flag ? "true" : "false");
					getProductDetailModel.ImageUrl1 = wAPProductBrowseInfo.Product.ImageUrl1;
					getProductDetailModel.ImageUrl2 = wAPProductBrowseInfo.Product.ImageUrl2;
					getProductDetailModel.ImageUrl3 = wAPProductBrowseInfo.Product.ImageUrl3;
					getProductDetailModel.ImageUrl4 = wAPProductBrowseInfo.Product.ImageUrl4;
					getProductDetailModel.ImageUrl5 = wAPProductBrowseInfo.Product.ImageUrl5;
					getProductDetailModel.ThumbnailUrl40 = wAPProductBrowseInfo.Product.ThumbnailUrl100;
					getProductDetailModel.ThumbnailUrl410 = wAPProductBrowseInfo.Product.ThumbnailUrl410;
					string phonePriceByProductId = PromoteHelper.GetPhonePriceByProductId(num);
					if (string.IsNullOrEmpty(phonePriceByProductId))
					{
						getProductDetailModel.MobileExclusive = decimal.Zero;
					}
					else
					{
						getProductDetailModel.MobileExclusive = Convert.ToDecimal(phonePriceByProductId.Split(',')[0]);
					}
					int supplierId = wAPProductBrowseInfo.Product.SupplierId;
					if (supplierId > 0)
					{
						SupplierInfo supplierById = SupplierHelper.GetSupplierById(supplierId);
						if (supplierById != null)
						{
							getProductDetailModel.SupplierName = supplierById.SupplierName;
						}
						else
						{
							getProductDetailModel.SupplierName = "";
						}
					}
					else
					{
						getProductDetailModel.SupplierName = "";
					}
					getProductDetailModel.SupplierId = supplierId;
					getProductDetailModel.ProductReduce = (masterSettings.ShowDeductInProductPage ? this.GetProductReduceInfo(num, wAPProductBrowseInfo.Product, getProductDetailModel.MobileExclusive) : "0");
					ProductPreSaleInfo productPreSaleInfoByProductId = ProductPreSaleHelper.GetProductPreSaleInfoByProductId(num);
					if (productPreSaleInfoByProductId != null)
					{
						getProductDetailModel.ActivityUrl = "PreSaleproductdetails.aspx?PreSaleId=" + productPreSaleInfoByProductId.PreSaleId;
					}
					else if (wAPProductBrowseInfo.Product.SaleStatus == ProductSaleStatus.OnStock)
					{
						getProductDetailModel.ActivityUrl = "ResourceNotFound?msg=商品已经入库";
					}
					else
					{
						CountDownInfo countDownInfo = PromoteHelper.ActiveCountDownByProductId(wAPProductBrowseInfo.Product.ProductId, 0);
						GroupBuyInfo groupBuyInfo = null;
						if (countDownInfo != null)
						{
							getProductDetailModel.ActivityUrl = $"CountDownProductsDetails.aspx?countDownId={countDownInfo.CountDownId}";
						}
						else
						{
							groupBuyInfo = PromoteHelper.ActiveGroupBuyByProductId(wAPProductBrowseInfo.Product.ProductId);
							if (groupBuyInfo != null)
							{
								getProductDetailModel.ActivityUrl = $"GroupBuyProductDetails.aspx?GroupBuyId={groupBuyInfo.GroupBuyId}";
							}
						}
						if (countDownInfo == null && groupBuyInfo == null)
						{
							getProductDetailModel.ActivityUrl = string.Empty;
						}
					}
					int fightGroupActivityId = 0;
					FightGroupActivitiyModel fightGroupActivitiyModel = VShopHelper.GetFightGroupActivities(new FightGroupActivitiyQuery
					{
						PageIndex = 1,
						PageSize = 1,
						ProductId = num,
						Status = EnumFightGroupActivitiyStatus.BeingCarried
					}).Models.FirstOrDefault();
					if (fightGroupActivitiyModel != null)
					{
						fightGroupActivityId = fightGroupActivitiyModel.FightGroupActivityId;
					}
					wAPProductBrowseInfo.Product.DefaultSku.SalePrice = decimal.Parse(wAPProductBrowseInfo.Product.DefaultSku.SalePrice.F2ToString("f2"));
					wAPProductBrowseInfo.Product.DefaultSku.CostPrice = decimal.Parse(wAPProductBrowseInfo.Product.DefaultSku.CostPrice.F2ToString("f2"));
					getProductDetailModel.DefaultSku = wAPProductBrowseInfo.Product.DefaultSku;
					getProductDetailModel.Stock = wAPProductBrowseInfo.Product.Stock;
					getProductDetailModel.SkuItem = null;
					string empty = string.Empty;
					getProductDetailModel.FightGroupActivityId = fightGroupActivityId;
					if (wAPProductBrowseInfo.DbSKUs == null || wAPProductBrowseInfo.DbSKUs.Rows.Count == 0)
					{
						getProductDetailModel.SkuItem = new List<SkuItem>();
					}
					else
					{
						getProductDetailModel.SkuItem = new List<SkuItem>();
						foreach (DataRow row in wAPProductBrowseInfo.DbSKUs.Rows)
						{
							if ((from c in getProductDetailModel.SkuItem
							where c.AttributeName == row["AttributeName"].ToNullString()
							select c).Count() == 0)
							{
								SkuItem skuItem = new SkuItem();
								skuItem.AttributeName = row["AttributeName"].ToNullString();
								skuItem.AttributeId = row["AttributeId"].ToNullString();
								skuItem.AttributeValue = new List<AttributeValue>();
								IList<string> list2 = new List<string>();
								foreach (DataRow row2 in wAPProductBrowseInfo.DbSKUs.Rows)
								{
									if (string.Compare((string)row["AttributeName"], (string)row2["AttributeName"]) == 0 && !list2.Contains((string)row2["ValueStr"]))
									{
										AttributeValue attributeValue = new AttributeValue();
										list2.Add((string)row2["ValueStr"]);
										attributeValue.ValueId = row2["ValueId"].ToNullString();
										attributeValue.UseAttributeImage = row2["UseAttributeImage"].ToNullString();
										attributeValue.Value = row2["ValueStr"].ToNullString();
										attributeValue.ImageUrl = Globals.FullPath(row2["ImageUrl"].ToNullString());
										skuItem.AttributeValue.Add(attributeValue);
									}
								}
								getProductDetailModel.SkuItem.Add(skuItem);
							}
						}
					}
					getProductDetailModel.Skus = new List<SKUItem>();
					foreach (SKUItem value2 in wAPProductBrowseInfo.Product.Skus.Values)
					{
						value2.SalePrice = decimal.Parse(value2.SalePrice.F2ToString("f2"));
						value2.CostPrice = decimal.Parse(value2.CostPrice.F2ToString("f2"));
						value2.ImageUrl = this.GetImageFullPath(value2.ImageUrl);
						value2.ThumbnailUrl40 = this.GetImageFullPath(value2.ThumbnailUrl40);
						value2.ThumbnailUrl410 = this.GetImageFullPath(value2.ThumbnailUrl410);
						getProductDetailModel.Skus.Add(value2);
					}
					int count = 12;
					getProductDetailModel.GuessYouLikeProducts = ProductBrowser.GetNewProductYouLikeModel(wAPProductBrowseInfo.Product.ProductId, 0, count, list, false);
					getProductDetailModel.ReviewCount = wAPProductBrowseInfo.ReviewCount;
					SiteSettings masterSettings2 = SettingsManager.GetMasterSettings();
					getProductDetailModel.CanTakeOnStore = false;
					if (masterSettings2.OpenMultStore)
					{
						if (StoresHelper.ProductInStoreAndIsAboveSelf(num))
						{
							getProductDetailModel.HasStores = true;
							getProductDetailModel.CanTakeOnStore = true;
						}
					}
					else
					{
						getProductDetailModel.HasStores = false;
						if (masterSettings2.IsOpenPickeupInStore && wAPProductBrowseInfo.Product.SupplierId == 0)
						{
							getProductDetailModel.CanTakeOnStore = true;
						}
					}
					getProductDetailModel.IsSupportPodrequest = (wAPProductBrowseInfo.Product.SupplierId == 0 && SalesHelper.IsSupportPodrequest());
					getProductDetailModel.Freight = ShoppingProcessor.CalcProductFreight(HiContext.Current.DeliveryScopRegionId, wAPProductBrowseInfo.Product.ShippingTemplateId, wAPProductBrowseInfo.Product.Weight, wAPProductBrowseInfo.Product.Weight, 1, wAPProductBrowseInfo.Product.MinSalePrice);
					DataTable couponList = CouponHelper.GetCouponList(num, HiContext.Current.UserId, false, false, false);
					if (couponList.IsNullOrEmpty())
					{
						getProductDetailModel.Coupons = new DataTable();
					}
					else
					{
						couponList.Columns.Add("LimitText", typeof(string));
						couponList.Columns.Add("StartTimeText", typeof(string));
						couponList.Columns.Add("ClosingTimeText", typeof(string));
						foreach (DataRow row3 in couponList.Rows)
						{
							row3["LimitText"] = ((row3["OrderUseLimit"].ToDecimal(0) > decimal.Zero) ? ("订单满" + string.Format("{0:F2}", row3["OrderUseLimit"].ToDecimal(0)) + "元可用") : "无限制");
							DataRow dataRow3 = row3;
							DateTime value = row3["StartTime"].ToDateTime().Value;
							dataRow3["StartTimeText"] = value.ToString("yyyy-MM-dd");
							DataRow dataRow4 = row3;
							value = row3["ClosingTime"].ToDateTime().Value;
							dataRow4["ClosingTimeText"] = value.ToString("yyyy-MM-dd");
						}
						getProductDetailModel.Coupons = couponList;
					}
					getProductDetailModel.Stores = StoresHelper.GetNearbyStores(string.Empty, num, "", false);
					getProductDetailModel.ConsultationCount = wAPProductBrowseInfo.ConsultationCount;
					StoreActivityEntityList storeActivityEntity = PromoteHelper.GetStoreActivityEntity(0, num);
					if (storeActivityEntity.FullAmountReduceList.Count > 0)
					{
						getProductDetailModel.FullAmountReduce = (from t in storeActivityEntity.FullAmountReduceList
						select t.ActivityName).Aggregate((string t, string n) => t + "，" + n);
					}
					string productPromotionsInfo = this.GetProductPromotionsInfo(num);
					if (storeActivityEntity.FullAmountSentGiftList.Count > 0 || !string.IsNullOrEmpty(productPromotionsInfo))
					{
						if (storeActivityEntity.FullAmountSentGiftList.Count > 0)
						{
							getProductDetailModel.FullAmountSentGift = (from t in storeActivityEntity.FullAmountSentGiftList
							select t.ActivityName).Aggregate((string t, string n) => t + "，" + n);
						}
						if (!string.IsNullOrEmpty(productPromotionsInfo))
						{
							GetProductDetailModel getProductDetailModel5 = getProductDetailModel;
							getProductDetailModel5.FullAmountSentGift = getProductDetailModel5.FullAmountSentGift + (string.IsNullOrEmpty(getProductDetailModel.FullAmountSentGift) ? "赠" : "，赠") + productPromotionsInfo;
						}
					}
					if (storeActivityEntity.FullAmountSentFreightList.Count > 0)
					{
						getProductDetailModel.FullAmountSentFreight = (from t in storeActivityEntity.FullAmountSentFreightList
						select t.ActivityName).Aggregate((string t, string n) => t + "，" + n);
					}
					getProductDetailModel.Description = (string.IsNullOrEmpty(wAPProductBrowseInfo.Product.MobbileDescription) ? wAPProductBrowseInfo.Product.Description : wAPProductBrowseInfo.Product.MobbileDescription).ToNullString().Replace("\"/Storage/master/gallery/", "\"" + Globals.FullPath("/Storage/master/gallery/"));
					getProductDetailModel.IsUnSale = (wAPProductBrowseInfo.Product.SaleStatus == ProductSaleStatus.UnSale && true);
					getProductDetailModel.ExtendAttribute = ProductBrowser.GetExpandAttributeList(wAPProductBrowseInfo.Product.ProductId);
					string s = JsonConvert.SerializeObject(new
					{
						Result = getProductDetailModel
					});
					context.Response.Write(s);
					context.Response.End();
				}
			}
		}

		private string GetProductPromotionsInfo(int productId)
		{
			string promsg = string.Empty;
			PromotionInfo productPromotionInfo = ProductBrowser.GetProductPromotionInfo(productId);
			if (productPromotionInfo == null || (productPromotionInfo.PromoteType != PromoteType.SentGift && productPromotionInfo.PromoteType != PromoteType.SentProduct))
			{
				return string.Empty;
			}
			if (productPromotionInfo.PromoteType == PromoteType.SentGift)
			{
				IList<Hidistro.Entities.Promotions.GiftInfo> giftDetailsByGiftIds = ProductBrowser.GetGiftDetailsByGiftIds(productPromotionInfo.GiftIds);
				if (giftDetailsByGiftIds.Count > 0)
				{
					giftDetailsByGiftIds.ForEach(delegate(Hidistro.Entities.Promotions.GiftInfo giftinfo)
					{
						promsg = promsg + giftinfo.Name + " ";
					});
				}
			}
			else if (productPromotionInfo.PromoteType == PromoteType.SentProduct)
			{
				string str = promsg;
				string str2 = string.IsNullOrEmpty(promsg) ? "" : ",";
				decimal num = productPromotionInfo.Condition;
				string arg = num.ToString("f0");
				num = productPromotionInfo.DiscountValue;
				promsg = str + str2 + string.Format("买{0}送{1} ", arg, num.ToString("f0"));
			}
			return promsg;
		}

		public void GetStoreProductDetail(HttpContext context)
		{
			int num = context.Request["ProductID"].ToInt(0);
			int num2 = context.Request["GradeId"].ToInt(0);
			int num3 = context.Request["StoreId"].ToInt(0);
			double num4 = context.Request["Lan"].ToDouble(0);
			double num5 = context.Request["Lng"].ToDouble(0);
			if (num <= 0)
			{
				context.Response.Write(this.GetErrorJosn(100, "商品不存在或被删除"));
			}
			else if (num3 <= 0)
			{
				context.Response.Write(this.GetErrorJosn(100, "门店不存在或被删除"));
			}
			else
			{
				CountDownInfo countDownInfo = PromoteHelper.ActiveCountDownByProductId(num, num3);
				if (countDownInfo != null && countDownInfo.CountDownId > 0)
				{
					context.Response.Write(this.GetErrorJosn(1, string.Format("{2}/CountDownStoreProductsDetails.aspx?countDownId={0}&storeId={1}", countDownInfo.CountDownId, num3, Globals.HostPath(context.Request.Url) + "/appshop")));
				}
				else
				{
					string text = context.Request["SessionID"];
					if (!string.IsNullOrEmpty(text))
					{
						this.ValidLoginBySessionID(text);
					}
					int num6 = 0;
					int areaId = 0;
					if (Math.Abs(num4 * num5) > 0.0)
					{
						DepotHelper.GetRegionIdFromLonLan(context.Request["Lan"].ToString() + "," + context.Request["Lng"].ToString(), out num6, out areaId);
					}
					StoreProductQuery storeProductQuery = new StoreProductQuery
					{
						ProductId = num,
						StoreId = num3,
						Position = new PositionInfo(num4, num5)
						{
							AreaId = areaId,
							CityId = num6
						}
					};
					ProductModel storeProduct = ProductBrowser.GetStoreProduct(storeProductQuery);
					if ((storeProduct.ExStatus == DetailException.NoStock || storeProduct.ExStatus == DetailException.OverServiceArea) && SettingsManager.GetMasterSettings().Store_IsRecommend)
					{
						StoreEntityQuery query = new StoreEntityQuery
						{
							AreaId = areaId,
							RegionId = num6,
							Position = storeProductQuery.Position,
							ProductId = storeProduct.ProductId
						};
						storeProduct.RecommendStore = StoreListHelper.GetStoreRecommendByProductId(query);
					}
					if (storeProduct == null || storeProduct.SaleStatus == ProductSaleStatus.Delete)
					{
						context.Response.Write(this.GetErrorJosn(100, "商品不存在或被删除"));
					}
					else
					{
						storeProduct.StoreActivityEntityList = PromoteHelper.GetStoreActivityEntity(num3, num);
						if (storeProduct.SkuTable == null || storeProduct.SkuTable.Rows.Count == 0)
						{
							storeProduct.SkuItem = new List<SkuItem>();
						}
						else
						{
							storeProduct.SkuItem = new List<SkuItem>();
							foreach (DataRow row in storeProduct.SkuTable.Rows)
							{
								if ((from c in storeProduct.SkuItem
								where c.AttributeName == row["AttributeName"].ToNullString()
								select c).Count() == 0)
								{
									SkuItem skuItem = new SkuItem();
									skuItem.AttributeName = row["AttributeName"].ToNullString();
									skuItem.AttributeId = row["AttributeId"].ToNullString();
									skuItem.AttributeValue = new List<AttributeValue>();
									IList<string> list = new List<string>();
									foreach (DataRow row2 in storeProduct.SkuTable.Rows)
									{
										if (string.Compare((string)row["AttributeName"], (string)row2["AttributeName"]) == 0 && !list.Contains((string)row2["ValueStr"]))
										{
											AttributeValue attributeValue = new AttributeValue();
											list.Add((string)row2["ValueStr"]);
											attributeValue.ValueId = row2["ValueId"].ToNullString();
											attributeValue.UseAttributeImage = row2["UseAttributeImage"].ToNullString();
											attributeValue.Value = row2["ValueStr"].ToNullString();
											attributeValue.ImageUrl = Globals.FullPath(row2["ImageUrl"].ToNullString());
											skuItem.AttributeValue.Add(attributeValue);
										}
									}
									storeProduct.SkuItem.Add(skuItem);
								}
							}
						}
						foreach (SKUItem sku in storeProduct.Skus)
						{
							sku.ImageUrl = Globals.FullPath(sku.ImageUrl);
							sku.ThumbnailUrl40 = Globals.FullPath(sku.ThumbnailUrl40);
							sku.ThumbnailUrl410 = Globals.FullPath(sku.ThumbnailUrl410);
						}
						storeProduct.ProductYouLikeModel = BrowsedProductQueue.GetProductYouLike(num, num3, null, false);
						storeProduct.SubmitOrderImg = Globals.FullPath(storeProduct.SubmitOrderImg);
						for (int i = 0; i < storeProduct.ImgUrlList.Count; i++)
						{
							storeProduct.ImgUrlList[i] = Globals.FullPath(storeProduct.ImgUrlList[i]);
						}
						DataTable couponList = CouponHelper.GetCouponList(num, HiContext.Current.UserId, false, false, false);
						if (couponList.IsNullOrEmpty())
						{
							storeProduct.Coupons = new DataTable();
						}
						else
						{
							couponList.Columns.Add("LimitText", typeof(string));
							couponList.Columns.Add("StartTimeText", typeof(string));
							couponList.Columns.Add("ClosingTimeText", typeof(string));
							foreach (DataRow row3 in couponList.Rows)
							{
								row3["LimitText"] = ((row3["OrderUseLimit"].ToDecimal(0) > decimal.Zero) ? ("订单满" + string.Format("{0:F2}", row3["OrderUseLimit"].ToDecimal(0)) + "元可用") : "无限制");
								DataRow dataRow3 = row3;
								DateTime value = row3["StartTime"].ToDateTime().Value;
								dataRow3["StartTimeText"] = value.ToString("yyyy-MM-dd");
								DataRow dataRow4 = row3;
								value = row3["ClosingTime"].ToDateTime().Value;
								dataRow4["ClosingTimeText"] = value.ToString("yyyy-MM-dd");
							}
							storeProduct.Coupons = couponList;
						}
						storeProduct.ExtendAttribute = ProductBrowser.GetExpandAttributeList(storeProduct.ProductId);
						IsoDateTimeConverter isoDateTimeConverter = new IsoDateTimeConverter();
						isoDateTimeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
						string s = JsonConvert.SerializeObject(storeProduct, Formatting.Indented, isoDateTimeConverter);
						context.Response.Write(s);
						context.Response.End();
					}
				}
			}
		}

		private string ProcessImg(string imageUrl)
		{
			if (string.IsNullOrEmpty(imageUrl))
			{
				return Globals.FullPath(SettingsManager.GetMasterSettings().DefaultProductThumbnail4);
			}
			return Globals.FullPath(imageUrl);
		}

		private string GetProductReduceInfo(int ProductID, ProductInfo product, decimal MobileExclusive)
		{
			if (this.siteSettings.OpenReferral != 1 || !this.siteSettings.ShowDeductInProductPage)
			{
				return string.Empty;
			}
			MemberInfo user = HiContext.Current.User;
			if (user.UserId == 0 || !user.IsReferral() || user.Referral == null || user.Referral.IsRepeled)
			{
				return string.Empty;
			}
			if (product == null)
			{
				product = ProductBrowser.GetProductSimpleInfo(ProductID);
			}
			if (product == null)
			{
				return string.Empty;
			}
			decimal d = (product.MinSalePrice - MobileExclusive > decimal.Zero) ? (product.MinSalePrice - MobileExclusive) : decimal.Zero;
			decimal d2 = product.SubMemberDeduct.HasValue ? product.SubMemberDeduct.Value : this.siteSettings.SubMemberDeduct;
			if (d2 > decimal.Zero)
			{
				return (d * (d2 / 100m)).F2ToString("f2");
			}
			return string.Empty;
		}

		private string GetOrderPromotionInfo(int productId)
		{
			DataTable productDetailOrderPromotions = PromoteHelper.GetProductDetailOrderPromotions();
			if (productDetailOrderPromotions.Rows.Count > 0)
			{
				string text = string.Empty;
				foreach (DataRow row in productDetailOrderPromotions.Rows)
				{
					if (row["PromoteType"].ToInt(0) != 4 || PromoteHelper.IsProductInPromotion(productId, row["ActivityId"].ToInt(0)))
					{
						text = text + row["Name"].ToNullString() + ",";
					}
				}
				return text.TrimEnd(',');
			}
			return string.Empty;
		}

		private string GetProductSendGifts(int productId)
		{
			PromotionInfo productPromotionInfo = ProductBrowser.GetProductPromotionInfo(productId);
			if (productPromotionInfo != null)
			{
				string promsg = string.Empty;
				if (productPromotionInfo.PromoteType == PromoteType.SentGift && !string.IsNullOrEmpty(productPromotionInfo.GiftIds))
				{
					IList<Hidistro.Entities.Promotions.GiftInfo> giftDetailsByGiftIds = ProductBrowser.GetGiftDetailsByGiftIds(productPromotionInfo.GiftIds);
					if (giftDetailsByGiftIds.Count > 0)
					{
						giftDetailsByGiftIds.ForEach(delegate(Hidistro.Entities.Promotions.GiftInfo giftinfo)
						{
							promsg = promsg + giftinfo.Name + " × 1,";
						});
						promsg = promsg.TrimEnd(',');
					}
				}
				else
				{
					promsg += productPromotionInfo.Name;
				}
				return promsg;
			}
			return string.Empty;
		}

		private void DelCartItem(HttpContext context)
		{
			this.CheckSession(context);
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
			this.GetShoppingCart(context);
		}

		private bool ValidLoginBySessionID(string sessionId)
		{
			MemberInfo memberInfo = MemberProcessor.FindMemberBySessionId(sessionId);
			if (memberInfo == null)
			{
				return false;
			}
			if (HiContext.Current.UserId == 0 || HiContext.Current.User == null)
			{
				memberInfo = Users.GetUser(memberInfo.UserId);
				HiContext.Current.UserId = memberInfo.UserId;
				HiContext.Current.User = memberInfo;
			}
			return true;
		}

		private void AddToCart(HttpContext context)
		{
			this.CheckSession(context);
			string text = context.Request["SkuID"].ToNullString();
			int num = context.Request["Quantity"].ToInt(0);
			int giftId = context.Request["GiftID"].ToInt(0);
			int storeId = context.Request["StoreId"].ToInt(0);
			AddCartItemStatus addCartItemStatus;
			if (!string.IsNullOrEmpty(text))
			{
				if (!ProductHelper.ProductsIsAllOnSales(text, storeId))
				{
					context.Response.Write(this.GetErrorJosn(401, "商品已下架或者已入库"));
					return;
				}
				addCartItemStatus = ShoppingCartProcessor.AddLineItem(text, num, true, storeId);
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
				this.GetShoppingCart(context);
				break;
			case AddCartItemStatus.InvalidUser:
				context.Response.Write(this.GetErrorJosn(108, "错误的用户信息"));
				break;
			case AddCartItemStatus.Offsell:
				context.Response.Write(this.GetErrorJosn(401, "商品已下架或者已入库"));
				break;
			case AddCartItemStatus.ProductNotExists:
				context.Response.Write(this.GetErrorJosn(404, "商品不存在"));
				break;
			case AddCartItemStatus.Shortage:
				context.Response.Write(this.GetErrorJosn(109, "商品库存不足,或者您的购物车中已存在该商品并且数量大于等于库存了"));
				break;
			}
			context.Response.End();
		}

		private void GetShoppingCart(HttpContext context)
		{
			this.CheckSession(context);
			ShoppingCartInfo mobileShoppingCart = ShoppingCartProcessor.GetMobileShoppingCart(null, false, true, -1);
			GetShoppingCartModel getShoppingCartModel = new GetShoppingCartModel();
			if (mobileShoppingCart != null)
			{
				getShoppingCartModel.RecordCount = mobileShoppingCart.GetQuantity(false);
				getShoppingCartModel.Amount = mobileShoppingCart.StrAmount;
				getShoppingCartModel.Point = mobileShoppingCart.GetPoint(this.siteSettings.PointsRate);
				getShoppingCartModel.Total = mobileShoppingCart.StrTotalAmount;
				GetShoppingCartModel getShoppingCartModel2 = getShoppingCartModel;
				bool flag = mobileShoppingCart.IsFreightFree;
				getShoppingCartModel2.IsFreightFree = flag.ToString();
				GetShoppingCartModel getShoppingCartModel3 = getShoppingCartModel;
				flag = mobileShoppingCart.IsReduced;
				getShoppingCartModel3.IsReduced = flag.ToString();
				GetShoppingCartModel getShoppingCartModel4 = getShoppingCartModel;
				flag = mobileShoppingCart.IsSendGift;
				getShoppingCartModel4.IsSendGift = flag.ToString();
				GetShoppingCartModel getShoppingCartModel5 = getShoppingCartModel;
				flag = mobileShoppingCart.IsSendTimesPoint;
				getShoppingCartModel5.IsSendTimesPoint = flag.ToString();
				getShoppingCartModel.ReducedPromotionAmount = mobileShoppingCart.StrReducedPromotionAmount;
				getShoppingCartModel.ReducedPromotionId = mobileShoppingCart.ReducedPromotionId;
				getShoppingCartModel.ReducedPromotionName = mobileShoppingCart.ReducedPromotionName;
				getShoppingCartModel.SendGiftPromotionId = mobileShoppingCart.SendGiftPromotionId;
				getShoppingCartModel.SendGiftPromotionName = mobileShoppingCart.SendGiftPromotionName;
				getShoppingCartModel.SentTimesPointPromotionId = mobileShoppingCart.SentTimesPointPromotionId;
				getShoppingCartModel.SentTimesPointPromotionName = mobileShoppingCart.SentTimesPointPromotionName;
				getShoppingCartModel.TimesPoint = mobileShoppingCart.TimesPoint;
				getShoppingCartModel.TotalWeight = mobileShoppingCart.TotalWeight;
				getShoppingCartModel.Weight = mobileShoppingCart.Weight;
				getShoppingCartModel.FreightFreePromotionId = mobileShoppingCart.FreightFreePromotionId;
				getShoppingCartModel.FreightFreePromotionName = mobileShoppingCart.FreightFreePromotionName;
				getShoppingCartModel.CartItemInfo = new List<CartItemInfo>();
				int num;
				for (int i = 0; i < mobileShoppingCart.LineItems.Count(); i++)
				{
					CartItemInfo cartItemInfo = new CartItemInfo();
					ShoppingCartItemInfo item = mobileShoppingCart.LineItems[i];
					int skuStock = ShoppingCartProcessor.GetSkuStock(item.SkuId, item.StoreId);
					cartItemInfo.SkuID = item.SkuId;
					CartItemInfo cartItemInfo2 = cartItemInfo;
					num = item.Quantity;
					cartItemInfo2.Quantity = num.ToString();
					CartItemInfo cartItemInfo3 = cartItemInfo;
					num = item.ShippQuantity;
					cartItemInfo3.ShippQuantity = num.ToString();
					CartItemInfo cartItemInfo4 = cartItemInfo;
					flag = item.IsfreeShipping;
					cartItemInfo4.IsfreeShipping = flag.ToString();
					CartItemInfo cartItemInfo5 = cartItemInfo;
					flag = item.IsSendGift;
					cartItemInfo5.IsSendGift = flag.ToString();
					cartItemInfo.MemberPrice = item.MemberPrice.F2ToString("f2");
					cartItemInfo.Name = item.Name;
					CartItemInfo cartItemInfo6 = cartItemInfo;
					num = item.ProductId;
					cartItemInfo6.ProductId = num.ToString();
					cartItemInfo.PromoteType = item.PromoteType.ToString();
					CartItemInfo cartItemInfo7 = cartItemInfo;
					num = item.PromotionId;
					cartItemInfo7.PromotionId = num.ToString();
					cartItemInfo.PromotionName = item.PromotionName;
					cartItemInfo.SKU = item.SKU;
					cartItemInfo.SkuContent = item.SkuContent;
					cartItemInfo.SubTotal = item.SubTotal.F2ToString("f2");
					cartItemInfo.ThumbnailUrl100 = this.GetImageFullPath(item.ThumbnailUrl100);
					cartItemInfo.ThumbnailUrl40 = this.GetImageFullPath(item.ThumbnailUrl40);
					cartItemInfo.ThumbnailUrl60 = this.GetImageFullPath(item.ThumbnailUrl60);
					cartItemInfo.Weight = item.Weight.F2ToString("f2");
					cartItemInfo.Stock = skuStock;
					cartItemInfo.HasStore = item.HasStore.ToNullString();
					cartItemInfo.IsMobileExclusive = item.IsMobileExclusive;
					cartItemInfo.IsValid = item.IsValid;
					cartItemInfo.HasEnoughStock = (skuStock > 0 && skuStock >= item.Quantity);
					cartItemInfo.SupplierId = item.SupplierId;
					cartItemInfo.SupplierName = item.SupplierName;
					cartItemInfo.CostPrice = item.CostPrice;
					cartItemInfo.StoreId = item.StoreId;
					cartItemInfo.StoreName = item.StoreName;
					cartItemInfo.StoreStatus = DetailException.Nomal;
					if (item.StoreId > 0)
					{
						StoresInfo storeById = StoresHelper.GetStoreById(item.StoreId);
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
						else if (!this.siteSettings.Store_IsOrderInClosingTime && item.StoreId > 0)
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
					List<ShoppingCartSendGift> cartGiftList = new List<ShoppingCartSendGift>();
					PromotionInfo productPromotionInfo = ProductBrowser.GetProductPromotionInfo(item.ProductId);
					if (productPromotionInfo != null && productPromotionInfo.PromoteType == PromoteType.SentGift)
					{
						IList<Hidistro.Entities.Promotions.GiftInfo> giftDetailsByGiftIds = ProductBrowser.GetGiftDetailsByGiftIds(productPromotionInfo.GiftIds);
						giftDetailsByGiftIds.ForEach(delegate(Hidistro.Entities.Promotions.GiftInfo gift)
						{
							ShoppingCartSendGift shoppingCartSendGift = new ShoppingCartSendGift();
							shoppingCartSendGift.GiftId = gift.GiftId;
							shoppingCartSendGift.Quantity = item.ShippQuantity;
							shoppingCartSendGift.Name = gift.Name;
							shoppingCartSendGift.ThumbnailUrl100 = gift.ThumbnailUrl100;
							shoppingCartSendGift.ThumbnailUrl180 = gift.ThumbnailUrl180;
							shoppingCartSendGift.ThumbnailUrl40 = gift.ThumbnailUrl40;
							shoppingCartSendGift.ThumbnailUrl60 = gift.ThumbnailUrl60;
							cartGiftList.Add(shoppingCartSendGift);
						});
					}
					cartItemInfo.SendGift = cartGiftList;
					getShoppingCartModel.CartItemInfo.Add(cartItemInfo);
				}
				getShoppingCartModel.GiftInfo = new List<Hidistro.Entities.APP.GiftInfo>();
				for (int j = 0; j < (from a in mobileShoppingCart.LineGifts
				where a.PromoType == 0
				select a).Count(); j++)
				{
					Hidistro.Entities.APP.GiftInfo giftInfo = new Hidistro.Entities.APP.GiftInfo();
					ShoppingCartGiftInfo shoppingCartGiftInfo = mobileShoppingCart.LineGifts[j];
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
					giftInfo.ThumbnailUrl100 = this.GetImageFullPath(shoppingCartGiftInfo.ThumbnailUrl100);
					giftInfo.ThumbnailUrl40 = this.GetImageFullPath(shoppingCartGiftInfo.ThumbnailUrl40);
					giftInfo.ThumbnailUrl60 = this.GetImageFullPath(shoppingCartGiftInfo.ThumbnailUrl60);
					getShoppingCartModel.GiftInfo.Add(giftInfo);
				}
			}
			else
			{
				getShoppingCartModel.RecordCount = 0;
				getShoppingCartModel.CartItemInfo = new List<CartItemInfo>();
				getShoppingCartModel.GiftInfo = new List<Hidistro.Entities.APP.GiftInfo>();
			}
			IOrderedEnumerable<CartItemInfo> source = from info in getShoppingCartModel.CartItemInfo
			orderby info.IsValid descending
			select info;
			Func<CartItemInfo, bool> keySelector = (CartItemInfo info) => info.HasEnoughStock;
			List<CartItemInfo> list2 = getShoppingCartModel.CartItemInfo = source.ThenByDescending(keySelector).ToList();
			string s = JsonConvert.SerializeObject(new
			{
				Result = getShoppingCartModel
			});
			context.Response.Write(s);
			context.Response.End();
		}

		private void ProcessLogout(HttpContext context)
		{
			this.ClearLoginStatus();
		}

		private void UnBind(HttpContext context)
		{
			string text = context.Request["userName"];
			int userId = context.Request["userId"].ToInt(0);
			MemberInfo user = Users.GetUser(userId);
			string s = JsonConvert.SerializeObject(new
			{
				Success = new
				{
					Status = (user != null),
					Msg = "删除失败"
				}
			});
			context.Response.Write(s);
		}

		private void ProcessAppInit(HttpContext context)
		{
			string text = context.Request["VID"];
			string text2 = context.Request["device"];
			string text3 = context.Request["version"];
			string text4 = context.Request["isFirst"];
			if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(text2) || string.IsNullOrEmpty(text3) || string.IsNullOrEmpty(text4))
			{
				context.Response.Write(this.GetErrorJosn(101, "缺少必填参数"));
			}
			else
			{
				string text5 = text3.ToNullString();
				if (text4.ToLower() == "true")
				{
					AppInstallRecordInfo appInstallRecordInfo = new AppInstallRecordInfo();
					appInstallRecordInfo.VID = text;
					appInstallRecordInfo.Device = text2;
					APPHelper.AddAppInstallRecord(appInstallRecordInfo);
				}
				AppVersionRecordInfo appVersionRecordInfo = APPHelper.GetLatestAppVersionRecord(text2);
				bool flag = false;
				if (appVersionRecordInfo == null)
				{
					appVersionRecordInfo = new AppVersionRecordInfo();
					appVersionRecordInfo.Version = text5;
					flag = appVersionRecordInfo.IsForcibleUpgrade;
				}
				bool flag2 = APPHelper.IsExistNewVersion(appVersionRecordInfo.Version, text5);
				string version = HiConfiguration.GetConfig().Version;
				Regex regex = new Regex("<p>.*</p>");
				version = version.Replace("(", "<p>").Replace(")", "</p>");
				version = regex.Replace(version, "");
				flag &= flag2;
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("{\"Result\":{");
				stringBuilder.AppendFormat("\"version\":\"{0}\",", appVersionRecordInfo.Version);
				stringBuilder.AppendFormat("\"existNew\":\"{0}\",", flag2.ToString().ToLower());
				stringBuilder.AppendFormat("\"forcible\":\"{0}\",", flag.ToString().ToLower());
				stringBuilder.AppendFormat("\"description\":\"{0}\",", appVersionRecordInfo.Description);
				stringBuilder.AppendFormat("\"upgradeUrl\":\"{0}\",", appVersionRecordInfo.UpgradeUrl);
				stringBuilder.AppendFormat("\"CurrentVersion\":\"{0}\",", version);
				stringBuilder.AppendFormat("\"AppAuditAPIUrl\":\"{0}\",", this.siteSettings.AppAuditAPIUrl);
				if (text2 == "android")
				{
					stringBuilder.AppendFormat("\"StartImg\":\"{0}\"", Globals.FullPath(this.siteSettings.AndroidStartImg));
				}
				else
				{
					stringBuilder.AppendFormat("\"StartImg\":\"{0}\"", Globals.FullPath(this.siteSettings.IOSStartImg));
				}
				stringBuilder.Append("}}");
				context.Response.Write(context.Server.HtmlDecode(stringBuilder.ToString()));
			}
		}

		private void GetDefaultData(HttpContext context)
		{
			GetDefaultDataModel getDefaultDataModel = new GetDefaultDataModel();
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			getDefaultDataModel.IsSuportPhoneRegister = masterSettings.IsSurportPhone;
			getDefaultDataModel.IsSuportEmailRegister = masterSettings.IsSurportEmail;
			getDefaultDataModel.IsValidEmail = masterSettings.IsNeedValidEmail;
			getDefaultDataModel.RegisterExtendInfo = masterSettings.RegistExtendInfo;
			getDefaultDataModel.IsOpenSupplier = masterSettings.OpenSupplier;
			getDefaultDataModel.AppCategoryTemplateStatus = masterSettings.AppCategoryTemplateStatus;
			getDefaultDataModel.IsOpenAppPromoteCoupons = masterSettings.IsOpenAppPromoteCoupons;
			getDefaultDataModel.IsOpenReferral = (masterSettings.OpenReferral == 1);
			if (getDefaultDataModel.IsOpenAppPromoteCoupons)
			{
				getDefaultDataModel.AppPromoteCouponsAmount = CouponHelper.GetCouponsAmount(masterSettings.AppPromoteCouponList);
			}
			else
			{
				getDefaultDataModel.AppPromoteCouponsAmount = decimal.Zero;
			}
			string text = context.Request["HomeTopicVersionCode"];
			getDefaultDataModel.HomeTopicVersion = this.siteSettings.AppHomeTopicVersionCode.ToString();
			getDefaultDataModel.HomeTopicPath = Globals.FullPath("/Templates/appshop/data/default.txt");
			getDefaultDataModel.tagProducts = new List<TagProducts>();
			DataTable homeProducts = VShopHelper.GetHomeProducts();
			if (homeProducts != null && homeProducts.Rows.Count > 0)
			{
				foreach (DataRow row in homeProducts.Rows)
				{
					TagProducts tagProducts = new TagProducts();
					tagProducts.pid = row["ProductId"].ToNullString();
					tagProducts.name = row["ProductName"].ToString().Replace("\\", "").Replace("\\", "");
					tagProducts.pic = ((row["ThumbnailUrl410"].ToNullString() != "") ? Globals.FullPath((string)row["ThumbnailUrl410"]) : Globals.FullPath(this.siteSettings.DefaultProductImage));
					tagProducts.price = row["SalePrice"].ToDecimal(0).F2ToString("f2");
					tagProducts.saleCounts = ((int)row["ShowSaleCounts"]).ToString();
					tagProducts.url = Globals.FullPath(string.Format("/AppShop/ProductDetails.aspx?productId={0}", row["ProductId"]));
					getDefaultDataModel.tagProducts.Add(tagProducts);
				}
			}
			getDefaultDataModel.IsOpenMeiQiaService = (HiContext.Current.SiteSettings.MeiQiaActivated.ToInt(0) == 1);
			getDefaultDataModel.IsOpenMultStore = this.siteSettings.OpenMultStore;
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					AppCategoryTemplateStatus = getDefaultDataModel.AppCategoryTemplateStatus,
					AppPromoteCouponsAmount = getDefaultDataModel.AppPromoteCouponsAmount,
					HomeTopicPath = getDefaultDataModel.HomeTopicPath,
					HomeTopicVersion = getDefaultDataModel.HomeTopicVersion,
					IsOpenAppPromoteCoupons = getDefaultDataModel.IsOpenAppPromoteCoupons,
					IsOpenMeiQiaService = getDefaultDataModel.IsOpenMeiQiaService,
					IsOpenMultStore = getDefaultDataModel.IsOpenMultStore,
					IsOpenReferral = getDefaultDataModel.IsOpenReferral,
					IsOpenSupplier = getDefaultDataModel.IsOpenSupplier,
					IsSuportEmailRegister = getDefaultDataModel.IsSuportEmailRegister,
					IsSuportPhoneRegister = getDefaultDataModel.IsSuportPhoneRegister,
					IsValidEmail = getDefaultDataModel.IsValidEmail,
					RegisterExtendInfo = getDefaultDataModel.RegisterExtendInfo,
					tagProducts = getDefaultDataModel.tagProducts,
					QuickLoginIsForceBindingMobbile = (this.siteSettings.QuickLoginIsForceBindingMobbile && this.siteSettings.SMSEnabled && !string.IsNullOrEmpty(this.siteSettings.SMSSettings)),
					ApplyReferralCondition = this.siteSettings.ApplyReferralCondition,
					ApplyReferralNeedAmount = this.siteSettings.ApplyReferralNeedAmount,
					UserLoginIsForceBindingMobbile = (this.siteSettings.UserLoginIsForceBindingMobbile && this.siteSettings.SMSEnabled && !string.IsNullOrEmpty(this.siteSettings.SMSSettings)),
					IsOpenRechargeGift = this.siteSettings.IsOpenRechargeGift
				}
			});
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
						icon = (string.IsNullOrEmpty(c.Icon) ? "/templates/appshop/images/catedefaulticon.jpg" : Globals.FullPath(c.Icon)),
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

		public void GetAllCategories(HttpContext context)
		{
			IEnumerable<CategoryInfo> mainCategories = CatalogHelper.GetMainCategories();
			if (mainCategories == null)
			{
				context.Response.Write(this.GetErrorJosn(103, "没获取到相应的分类"));
			}
			else
			{
				GetAllCategoriesModel getAllCategoriesModel = new GetAllCategoriesModel();
				getAllCategoriesModel.Subs = new List<Sub>();
				foreach (CategoryInfo item in mainCategories)
				{
					Sub sub = new Sub();
					getAllCategoriesModel.Subs.Add(sub);
					sub.cid = item.CategoryId;
					sub.name = item.Name;
					sub.icon = Globals.FullPath(item.Icon);
					sub.icon = (string.IsNullOrEmpty(sub.icon) ? "/templates/appshop/images/catedefaulticon.jpg" : sub.icon);
					sub.bigImageUrl = Globals.FullPath(item.BigImageUrl);
					sub.bigImageUrl = (string.IsNullOrEmpty(sub.bigImageUrl) ? "/templates/appshop/images/catedefault.jpg" : sub.bigImageUrl);
					Sub sub2 = sub;
					bool hasChildren = item.HasChildren;
					sub2.hasChildren = hasChildren.ToString().ToLower();
					sub.description = this.GetSubCategoryNames(item.CategoryId);
					sub.Subs = new List<TowLevelSubs>();
					IEnumerable<CategoryInfo> subCategories = CatalogHelper.GetSubCategories(item.CategoryId);
					if (subCategories != null)
					{
						foreach (CategoryInfo item2 in subCategories)
						{
							TowLevelSubs towLevelSubs = new TowLevelSubs();
							sub.Subs.Add(towLevelSubs);
							towLevelSubs.cid = item2.CategoryId;
							towLevelSubs.name = item2.Name;
							towLevelSubs.icon = Globals.FullPath(item2.Icon);
							towLevelSubs.icon = (string.IsNullOrEmpty(towLevelSubs.icon) ? "/templates/appshop/images/catedefaulticon.jpg" : towLevelSubs.icon);
							TowLevelSubs towLevelSubs2 = towLevelSubs;
							hasChildren = item2.HasChildren;
							towLevelSubs2.hasChildren = hasChildren.ToString().ToLower();
							towLevelSubs.Subs = new List<ThreeLevelSubs>();
							IEnumerable<CategoryInfo> subCategories2 = CatalogHelper.GetSubCategories(item2.CategoryId);
							if (subCategories2 != null)
							{
								foreach (CategoryInfo item3 in subCategories2)
								{
									ThreeLevelSubs threeLevelSubs = new ThreeLevelSubs();
									towLevelSubs.Subs.Add(threeLevelSubs);
									threeLevelSubs.cid = item3.CategoryId;
									threeLevelSubs.name = item3.Name;
									threeLevelSubs.icon = Globals.FullPath(item3.Icon);
								}
							}
						}
					}
				}
				string s = JsonConvert.SerializeObject(new
				{
					Result = getAllCategoriesModel
				});
				context.Response.Write(s);
			}
		}

		private void GetProducts(HttpContext context)
		{
			int num = context.Request["pageIndex"].ToInt(0);
			int num2 = context.Request["pageSize"].ToInt(0);
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
			if (!string.IsNullOrEmpty(context.Request["cId"]))
			{
				int categoryId = context.Request["cId"].ToInt(0);
				productBrowseQuery.Category = CatalogHelper.GetCategory(categoryId);
			}
			productBrowseQuery.Keywords = context.Request["keyword"];
			productBrowseQuery.SortBy = "DisplaySequence";
			productBrowseQuery.SortOrder = SortAction.Desc;
			if (!string.IsNullOrEmpty(context.Request["sortBy"].ToNullString()))
			{
				productBrowseQuery.SortBy = context.Request["sortBy"].ToNullString();
			}
			if (!string.IsNullOrEmpty(context.Request["sortOrder"].ToNullString()))
			{
				productBrowseQuery.SortOrder = ((context.Request["sortOrder"].ToNullString().ToLower() == "asc") ? SortAction.Asc : SortAction.Desc);
			}
			int couponId = context.Request["couponId"].ToInt(0);
			CouponInfo eFCoupon = CouponHelper.GetEFCoupon(couponId);
			if (eFCoupon != null)
			{
				productBrowseQuery.CanUseProducts = eFCoupon.CanUseProducts;
			}
			int num3 = context.Request["storeId"].ToInt(0);
			if (num3 > 0)
			{
				productBrowseQuery.StoreId = num3;
			}
			productBrowseQuery.ProductType = ProductType.PhysicalProduct;
			DbQueryResult storeProductList = StoresHelper.GetStoreProductList(productBrowseQuery);
			DataTable data = storeProductList.Data;
			List<GetProductsModel> list = new List<GetProductsModel>();
			foreach (DataRow row in data.Rows)
			{
				GetProductsModel getProductsModel = new GetProductsModel();
				list.Add(getProductsModel);
				getProductsModel.name = row["ProductName"].ToNullString();
				getProductsModel.pic = ((row["ThumbnailUrl310"].ToNullString() == "") ? Globals.FullPath(this.siteSettings.DefaultProductThumbnail7) : Globals.FullPath((string)row["ThumbnailUrl310"]));
				getProductsModel.price = row["SalePrice"].ToDecimal(0).F2ToString("f2");
				getProductsModel.saleCounts = row["SaleCounts"].ToInt(0).ToString();
				getProductsModel.url = Globals.FullPath(string.Format("/wapShop/" + ((row["ProductType"].ToInt(0) == 1.GetHashCode()) ? "ServiceProductDetails.aspx" : "StoreProductDetails.aspx") + "?productId={0}{1}", row["ProductId"], (num3 > 0) ? ("&StoreId=" + num3) : ""));
				getProductsModel.pid = row["ProductId"].ToNullString();
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

		private void SendAppPromoteCoupons(MemberInfo memberInfo, bool isUpdate = true)
		{
			if (this.siteSettings.IsOpenAppPromoteCoupons && memberInfo != null && !memberInfo.IsSendAppCoupons)
			{
				int num = 0;
				string[] array = this.siteSettings.AppPromoteCouponList.Split(',');
				foreach (string obj in array)
				{
					if (obj.ToInt(0) > 0 && CouponHelper.AddCouponItemInfo(memberInfo, obj.ToInt(0)) == CouponActionStatus.Success)
					{
						num++;
					}
				}
				memberInfo.IsSendAppCoupons = true;
				if (isUpdate)
				{
					MemberProcessor.UpdateMember(memberInfo);
				}
			}
		}

		private void ProcessRegiester(HttpContext context)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string text = context.Request["userName"].ToNullString();
			string text2 = context.Request["password"].ToNullString();
			string text3 = context.Request["oauthType"].ToNullString();
			string text4 = context.Request["oauthOpenId"].ToNullString();
			string text5 = context.Request["oauthNickName"].ToNullString();
			string text6 = context.Request["oauthAvatar"].ToNullString();
			string realName = context.Request["realName"].ToNullString();
			string text7 = context.Request["sex"].ToNullString();
			DateTime? birthDate = context.Request["birthDay"].ToDateTime();
			bool flag = false;
			string text8 = "";
			string text9 = context.Request["UnionId"].ToNullString();
			MemberInfo memberInfo = null;
			if (text9 == "null" || text9 == "undefined")
			{
				text9 = "";
			}
			if (string.IsNullOrEmpty(text9) && text3 == "weixin")
			{
				text9 = text4;
			}
			Guid guid;
			if (!string.IsNullOrEmpty(text9))
			{
				memberInfo = MemberProcessor.GetMemberByUnionId(text9);
				if (memberInfo != null)
				{
					MemberInfo memberInfo2 = memberInfo;
					guid = Guid.NewGuid();
					memberInfo2.SessionId = guid.ToString("N");
					MemberHelper.Update(memberInfo, false);
					this.SendAppPromoteCoupons(memberInfo, true);
					this.ToLogin(memberInfo, false);
					this.GetMember(context, memberInfo);
					return;
				}
			}
			if (!string.IsNullOrEmpty(text4) && !string.IsNullOrEmpty(text3))
			{
				flag = true;
				text3 = text3.ToLower();
				if (!string.IsNullOrEmpty(text9) && text3 != "weixin")
				{
					return;
				}
				if (text3 == "weixin")
				{
					text3 = "hishop.plugins.openid.appweixin";
					text8 = "APP微信信任登录";
				}
				if (text3.ToLower() == "qq")
				{
					text3 = "hishop.plugins.openid.qq.appqqservicet";
					text8 = "APP腾讯信任登录";
				}
				if (text3.ToLower() == "sina" || text3.ToLower() == "weibo" || text3.ToLower() == "sinaweibo")
				{
					text3 = "hishop.plugins.openid.sina.appsinaservice";
					text8 = "APP新浪信任登录";
				}
				NameValueCollection nameValueCollection = new NameValueCollection
				{
					context.Request.QueryString,
					context.Request.Form
				};
				memberInfo = MemberProcessor.GetMemberByOpenId_App(text3, text4);
				if (memberInfo != null)
				{
					this.SendAppPromoteCoupons(memberInfo, true);
					MemberInfo memberInfo3 = memberInfo;
					guid = Guid.NewGuid();
					memberInfo3.SessionId = guid.ToString("N");
					if (memberInfo.UnionId != text9 && !string.IsNullOrEmpty(text9) && MemberProcessor.GetMemberByUnionId(text9) == null)
					{
						memberInfo.UnionId = text9;
					}
					MemberProcessor.UpdateMember(memberInfo);
					this.ToLogin(memberInfo, false);
					this.GetMember(context, memberInfo);
					return;
				}
			}
			if (!flag)
			{
				if (this.CheckRegisteParam(context))
				{
					if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(text2))
					{
						context.Response.Write(this.GetErrorJosn(101, "缺少必填参数"));
					}
					else
					{
						MemberInfo memberInfo4 = new MemberInfo();
						MemberInfo memberInfo5 = memberInfo4;
						guid = Guid.NewGuid();
						memberInfo5.SessionId = guid.ToString("N");
						memberInfo4.Picture = text6;
						if (HiContext.Current.ReferralUserId > 0)
						{
							memberInfo4.ReferralUserId = HiContext.Current.ReferralUserId;
						}
						memberInfo4.GradeId = MemberProcessor.GetDefaultMemberGrade();
						memberInfo4.UserName = text;
						memberInfo4.RealName = realName;
						memberInfo4.Gender = ((!string.IsNullOrEmpty(text7)) ? ((text7 == "男士") ? Gender.Male : Gender.Female) : Gender.NotSet);
						memberInfo4.BirthDate = birthDate;
						if (this.emailR.IsMatch(memberInfo4.UserName))
						{
							memberInfo4.Email = memberInfo4.UserName;
							memberInfo4.EmailVerification = this.siteSettings.IsNeedValidEmail;
						}
						if (this.cellphoneR.IsMatch(memberInfo4.UserName))
						{
							memberInfo4.CellPhone = memberInfo4.UserName;
							memberInfo4.CellPhoneVerification = true;
						}
						string text10 = Globals.RndStr(128, true);
						text2 = (memberInfo4.Password = Users.EncodePassword(text2, text10));
						memberInfo4.PasswordSalt = text10;
						memberInfo4.RegisteredSource = 5;
						memberInfo4.CreateDate = DateTime.Now;
						memberInfo4.UnionId = text9;
						int num = MemberProcessor.CreateMember(memberInfo4);
						if (num > 0)
						{
							Messenger.UserRegister(memberInfo4, text2);
							if (!string.IsNullOrEmpty(text4) && !string.IsNullOrEmpty(text3))
							{
								MemberOpenIdInfo memberOpenIdInfo = new MemberOpenIdInfo();
								memberOpenIdInfo.UserId = memberInfo4.UserId;
								memberOpenIdInfo.OpenIdType = text3;
								memberOpenIdInfo.OpenId = text4;
								if (MemberProcessor.GetMemberByOpenId_App(text3, text4) == null)
								{
									MemberProcessor.AddMemberOpenId(memberOpenIdInfo);
								}
							}
							if (this.siteSettings.IsOpenGiftCoupons)
							{
								int num2 = 0;
								string[] array = this.siteSettings.GiftCouponList.Split(',');
								foreach (string obj in array)
								{
									if (obj.ToInt(0) > 0 && CouponHelper.AddCouponItemInfo(memberInfo4, obj.ToInt(0)) == CouponActionStatus.Success)
									{
										num2++;
									}
								}
							}
							this.SendAppPromoteCoupons(memberInfo4, true);
							this.GetMember(context, memberInfo4);
						}
						else
						{
							context.Response.Write(this.GetErrorJosn(121, "注册用户失败"));
						}
					}
				}
			}
			else if (!this.siteSettings.QuickLoginIsForceBindingMobbile)
			{
				if (!string.IsNullOrEmpty(text5))
				{
					text5 = HttpUtility.UrlDecode(text5);
				}
				if (!string.IsNullOrEmpty(text5.Trim()))
				{
					if (MemberProcessor.FindMemberByUsername(text5) != null)
					{
						text5 = this.GenerateUsername(8);
						if (MemberProcessor.FindMemberByUsername(text5) != null)
						{
							text5 = this.GenerateUsername();
							if (MemberProcessor.FindMemberByUsername(text5) != null)
							{
								context.Response.Write(this.GetErrorJosn(122, "随机用户名尝试失败"));
								return;
							}
						}
					}
					if (!string.IsNullOrEmpty(text6))
					{
						text6 = HttpUtility.UrlDecode(text6);
					}
					MemberInfo memberInfo6 = new MemberInfo();
					memberInfo6.SessionId = Globals.GetGenerateId();
					memberInfo6.Picture = text6;
					if (HiContext.Current.ReferralUserId > 0)
					{
						memberInfo6.ReferralUserId = HiContext.Current.ReferralUserId;
					}
					memberInfo6.GradeId = MemberProcessor.GetDefaultMemberGrade();
					memberInfo6.UserName = text5;
					memberInfo6.NickName = text5;
					if (this.emailR.IsMatch(memberInfo6.UserName))
					{
						memberInfo6.Email = memberInfo6.UserName;
					}
					if (this.cellphoneR.IsMatch(memberInfo6.UserName))
					{
						memberInfo6.CellPhone = memberInfo6.UserName;
						memberInfo6.CellPhoneVerification = true;
					}
					text2 = this.GeneratePassword();
					string text12 = "Open";
					string text13 = text2;
					text2 = (memberInfo6.Password = Users.EncodePassword(text2, text12));
					memberInfo6.PasswordSalt = text12;
					memberInfo6.RegisteredSource = 5;
					memberInfo6.CreateDate = DateTime.Now;
					memberInfo6.UnionId = text9;
					int num3 = MemberProcessor.CreateMember(memberInfo6);
					if (num3 > 0)
					{
						Messenger.UserRegister(memberInfo6, text2);
						if (!string.IsNullOrEmpty(text4) && !string.IsNullOrEmpty(text3))
						{
							MemberOpenIdInfo memberOpenIdInfo2 = new MemberOpenIdInfo();
							memberOpenIdInfo2.UserId = num3;
							memberOpenIdInfo2.OpenIdType = text3;
							memberOpenIdInfo2.OpenId = text4;
							if (MemberProcessor.GetMemberByOpenId_App(text3, text4) == null)
							{
								MemberProcessor.AddMemberOpenId(memberOpenIdInfo2);
							}
						}
						memberInfo6.UserName = MemberHelper.GetUserName(num3);
						MemberHelper.Update(memberInfo6, true);
						if (this.siteSettings.IsOpenGiftCoupons)
						{
							int num4 = 0;
							string[] array2 = this.siteSettings.GiftCouponList.Split(',');
							foreach (string obj2 in array2)
							{
								if (obj2.ToInt(0) > 0 && CouponHelper.AddCouponItemInfo(memberInfo6, obj2.ToInt(0)) == CouponActionStatus.Success)
								{
									num4++;
								}
							}
						}
						this.SendAppPromoteCoupons(memberInfo6, true);
						this.GetMember(context, memberInfo6);
					}
				}
			}
			else
			{
				this.GetMember(context, null);
			}
		}

		public void HasBind(HttpContext context)
		{
			string text = context.Request["oauthType"].ToNullString();
			string text2 = context.Request["oauthOpenId"].ToNullString();
			if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(text2))
			{
				context.Response.Write(this.GetErrorJosn(224, "OpenId或者OpenIdType为空"));
			}
			text = text.ToLower();
			if (text == "weixin")
			{
				text = "wxsession";
			}
			MemberInfo memberByOpenId_App = MemberProcessor.GetMemberByOpenId_App(text2, text);
			string s = JsonConvert.SerializeObject(new
			{
				Success = new
				{
					Status = (memberByOpenId_App != null),
					Msg = string.Empty
				}
			});
			context.Response.Write(s);
		}

		public void ClearLoginStatus()
		{
			try
			{
				if (HiContext.Current.User.UserId > 0)
				{
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
				HttpCookie httpCookie8 = HiContext.Current.Context.Request.Cookies["UserCoordinateTimeCookie"];
				if (httpCookie7 != null && !string.IsNullOrEmpty(httpCookie7.Value))
				{
					httpCookie7.Expires = new DateTime(1911, 10, 12);
					HttpContext.Current.Response.Cookies.Add(httpCookie7);
				}
				if (httpCookie8 != null && !string.IsNullOrEmpty(httpCookie8.Value))
				{
					httpCookie8.Expires = new DateTime(1911, 10, 12);
					HttpContext.Current.Response.Cookies.Add(httpCookie8);
				}
				HiContext.Current.UserId = 0;
				HiContext.Current.User = null;
			}
			catch
			{
			}
		}

		private void ToLogin(MemberInfo userToLogin, bool updateUser = false)
		{
			Users.SetCurrentUser(userToLogin.UserId, 30, true, false);
			HiContext.Current.User = userToLogin;
		}

		protected void AddProductToFavorite(HttpContext context)
		{
			this.CheckSession(context);
			int userId = HiContext.Current.User.UserId;
			int productId = context.Request["ProductId"].ToInt(0);
			int storeId = context.Request["StoreId"].ToInt(0);
			if (!ProductBrowser.ExistsProduct(productId, userId, storeId))
			{
				int num = ProductBrowser.AddProduct(productId, userId, storeId);
				this.message = JsonConvert.SerializeObject(new
				{
					Success = new
					{
						Status = (num > 0),
						Msg = string.Empty
					}
				});
			}
			else
			{
				this.message = JsonConvert.SerializeObject(new
				{
					Success = new
					{
						Status = true,
						Msg = string.Empty
					}
				});
			}
			context.Response.Write(this.message);
			context.Response.End();
		}

		protected void DeleteFavorite(HttpContext context)
		{
			this.CheckSession(context);
			int userId = HiContext.Current.User.UserId;
			int productId = context.Request["ProductId"].ToInt(0);
			int storeId = context.Request["StoreId"].ToInt(0);
			string s = JsonConvert.SerializeObject(new
			{
				Success = new
				{
					Status = (ProductBrowser.DeleteFavorite(userId, productId, storeId) > 0),
					Msg = string.Empty
				}
			});
			context.Response.Write(s);
			context.Response.End();
		}

		private void ProcessLogin(HttpContext context)
		{
			string text = "";
			string text2 = context.Request["userName"].ToNullString();
			string text3 = context.Request["password"].ToNullString();
			string text4 = context.Request["oauthType"].ToNullString();
			string text5 = context.Request["oauthOpenId"].ToNullString();
			string text6 = context.Request["oauthNickName"].ToNullString();
			string text7 = context.Request["oauthAvatar"].ToNullString();
			bool flag = false;
			string text8 = context.Request["UnionId"].ToNullString();
			NameValueCollection nameValueCollection = new NameValueCollection
			{
				context.Request.QueryString,
				context.Request.Form
			};
			string[] array = new string[7]
			{
				"userName=" + text2,
				"password=" + text3,
				"openIdType=" + text4,
				"openId=" + text5,
				"nickname=" + text6,
				"avatar=" + text7,
				"trustLogin=" + flag.ToString()
			};
			if (HiContext.Current.UserId != 0)
			{
				this.ClearLoginStatus();
			}
			bool flag2 = false;
			MemberInfo memberInfo = null;
			if (text8 == "null")
			{
				text8 = "";
			}
			if (string.IsNullOrEmpty(text8) && text4 == "weixin")
			{
				text8 = text5;
			}
			if (!string.IsNullOrEmpty(text8))
			{
				memberInfo = MemberProcessor.GetMemberByUnionId(text8);
				if (memberInfo != null)
				{
					flag2 = true;
					text4 = "hishop.plugins.openid.appweixin";
				}
			}
			if (!flag2)
			{
				if (!string.IsNullOrEmpty(text4) && !string.IsNullOrEmpty(text5))
				{
					text4 = text4.ToLower();
					if (text4 == "weixin")
					{
						text4 = "hishop.plugins.openid.appweixin";
						text = "App微信信任登录";
					}
					if (text4.ToLower() == "qq")
					{
						text4 = "hishop.plugins.openid.qq.appqqservicet";
						text = "App腾讯信任登录";
					}
					if (text4.ToLower() == "sina")
					{
						text4 = "hishop.plugins.openid.sina.appsinaservice";
						text = "APP新浪信任登录";
					}
					memberInfo = MemberProcessor.GetMemberByOpenId_App(text4, text5);
					if (memberInfo != null)
					{
						text2 = memberInfo.UserName;
						flag = true;
					}
					else
					{
						if (string.IsNullOrEmpty(text2) || string.IsNullOrEmpty(text3))
						{
							context.Response.Write(this.GetErrorJosn(104, "未绑定商城帐号"));
							return;
						}
						memberInfo = MemberProcessor.ValidLogin(text2, text3);
						if (memberInfo == null)
						{
							context.Response.Write(this.GetErrorJosn(206, "用户名或密码错误"));
							return;
						}
					}
				}
				else
				{
					if (string.IsNullOrEmpty(text2) || (string.IsNullOrEmpty(text3) && !flag))
					{
						context.Response.Write(this.GetErrorJosn(101, "缺少必填参数"));
						return;
					}
					memberInfo = MemberProcessor.ValidLogin(text2, text3);
					if (memberInfo == null)
					{
						context.Response.Write(this.GetErrorJosn(206, "用户名或密码错误"));
						return;
					}
				}
			}
			if (!string.IsNullOrEmpty(text5) && !string.IsNullOrEmpty(text4) && !string.IsNullOrEmpty(text2) && !flag)
			{
				if (text4 == "hishop.plugins.openid.appweixin")
				{
					if (!string.IsNullOrEmpty(memberInfo.UnionId) && memberInfo.UnionId != text8)
					{
						context.Response.Write(this.GetErrorJosn(207, "该用户已存在信任登录绑定关系,请选择其它帐号"));
						return;
					}
				}
				else
				{
					MemberInfo memberByOpenId = MemberProcessor.GetMemberByOpenId(text4, text5);
					if (memberByOpenId != null && memberByOpenId.UserId != memberInfo.UserId)
					{
						context.Response.Write(this.GetErrorJosn(207, "该用户已存在信任登录绑定关系,请选择其它帐号"));
						return;
					}
					MemberOpenIdInfo memberOpenIdInfo = MemberProcessor.GetMemberOpenIdInfo(memberInfo.UserId, text4);
					if (memberOpenIdInfo != null && memberOpenIdInfo.OpenId != text5)
					{
						context.Response.Write(this.GetErrorJosn(207, "该用户已存在信任登录绑定关系,请选择其它帐号"));
						return;
					}
				}
				MemberOpenIdInfo memberOpenIdInfo2 = new MemberOpenIdInfo();
				memberOpenIdInfo2.UserId = memberInfo.UserId;
				memberOpenIdInfo2.OpenIdType = text4;
				memberOpenIdInfo2.OpenId = text5;
				if (MemberProcessor.GetMemberByOpenId_App(text4, text5) == null)
				{
					MemberProcessor.AddMemberOpenId(memberOpenIdInfo2);
				}
			}
			if (!string.IsNullOrEmpty(text7) && string.IsNullOrEmpty(memberInfo.Picture))
			{
				memberInfo.Picture = text7;
			}
			if (!string.IsNullOrEmpty(text6) && string.IsNullOrEmpty(memberInfo.NickName))
			{
				memberInfo.NickName = text6;
			}
			SiteSettings siteSettings = HiContext.Current.SiteSettings;
			string sessionId = memberInfo.SessionId;
			this.SendAppPromoteCoupons(memberInfo, false);
			this.ToLogin(memberInfo, true);
			memberInfo.SessionId = Guid.NewGuid().ToString("N");
			memberInfo.UnionId = text8;
			MemberProcessor.UpdateMember(memberInfo);
			HiCache.Remove($"DataCache-APPMemberCacheKey-{sessionId}");
			this.GetMember(context, memberInfo);
		}

		private void GetMember(HttpContext context)
		{
			string text = context.Request["sessionid"];
			int num = 0;
			if (context.Request["userid"] != null && !string.IsNullOrEmpty(context.Request["userid"]))
			{
				int.TryParse(context.Request["userid"].ToString(), out num);
			}
			if (string.IsNullOrEmpty(text) && num <= 0)
			{
				context.Response.Write(this.GetErrorJosn(101, "缺少必填参数"));
			}
			else
			{
				MemberInfo memberInfo = null;
				if (!string.IsNullOrEmpty(text))
				{
					memberInfo = MemberProcessor.FindMemberBySessionId(text);
				}
				if (memberInfo == null && num > 0)
				{
					memberInfo = Users.GetUser(num);
				}
				if (memberInfo == null)
				{
					if (num <= 0)
					{
						context.Response.Write(this.GetErrorJosn(107, "sessionid过期或不存在，请重新登录"));
					}
					else
					{
						context.Response.Write(this.GetErrorJosn(107, "userID不存在，请重新登录"));
					}
				}
				else
				{
					this.ToLogin(memberInfo, false);
					this.GetMember(context, memberInfo);
				}
			}
		}

		private void GetMember(HttpContext context, MemberInfo member)
		{
			if (member == null)
			{
				string s = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						Status = 100,
						IsBindUser = false
					}
				});
				context.Response.Write(s);
			}
			else
			{
				MemberGradeInfo memberGrade = MemberProcessor.GetMemberGrade(member.GradeId);
				string gradeName = (memberGrade == null) ? "" : memberGrade.Name;
				OrderQuery orderQuery = new OrderQuery();
				orderQuery.Status = OrderStatus.WaitBuyerPay;
				orderQuery.IsServiceOrder = false;
				orderQuery.IsAllOrder = true;
				int userOrderCount = MemberProcessor.GetUserOrderCount(member.UserId, orderQuery);
				int couponsCount = 0;
				DataTable userCoupons = CouponHelper.GetUserCoupons(member.UserId, 1);
				if (userCoupons != null && userCoupons.Rows.Count > 0)
				{
					couponsCount = userCoupons.Rows.Count;
				}
				orderQuery.Status = OrderStatus.SellerAlreadySent;
				int userOrderCount2 = MemberProcessor.GetUserOrderCount(member.UserId, orderQuery);
				orderQuery.Status = OrderStatus.BuyerAlreadyPaid;
				int userOrderCount3 = MemberProcessor.GetUserOrderCount(member.UserId, orderQuery);
				orderQuery.Status = OrderStatus.WaitReview;
				int userOrderCount4 = MemberProcessor.GetUserOrderCount(member.UserId, orderQuery);
				ReferralInfo referralInfo = new ReferralDao().Get<ReferralInfo>(member.UserId);
				if (referralInfo != null)
				{
					member.Referral = referralInfo;
				}
				int notification = (VShopHelper.AppPushRecordForIOS(member.UserId) != 0) ? 1 : 0;
				string s2 = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						Status = 100,
						IsBindUser = true,
						ReferralStatus = ((member.Referral != null) ? member.Referral.ReferralStatus : 0),
						notification = notification,
						Balance = member.Balance.F2ToString("f2"),
						IsOpenBalance = member.IsOpenBalance,
						couponsCount = couponsCount,
						picture = (string.IsNullOrEmpty(member.Picture) ? "" : ((member.Picture.StartsWith("http://") || member.Picture.StartsWith("https://")) ? member.Picture : Globals.FullPath(member.Picture))),
						points = member.Points,
						expenditure = member.Expenditure.F2ToString("f2"),
						orderNumber = member.OrderNumber,
						waitFinishCount = userOrderCount2,
						waitPayCount = userOrderCount,
						waitSendCount = userOrderCount3,
						realName = (string.IsNullOrEmpty(member.NickName) ? (string.IsNullOrEmpty(member.RealName) ? member.UserName : member.RealName) : member.NickName),
						gradeName = gradeName,
						gradeId = member.GradeId,
						sessionid = member.SessionId,
						userName = member.UserName,
						uid = member.UserId,
						Cellphone = member.CellPhone,
						CellPhoneVerification = member.CellPhoneVerification,
						Email = member.Email,
						EmailVerification = member.EmailVerification,
						EnableAppShake = this.siteSettings.EnableAppShake,
						FightGroupActiveNumber = VShopHelper.GetMyFightGroupActiveNumber(member.UserId),
						IsReferral = (member.IsReferral() && member.Referral != null && !member.Referral.IsRepeled),
						EnableBulkPaymentAdvance = this.siteSettings.EnableBulkPaymentAdvance,
						PrizeCount = ActivityHelper.CountCurrUserNoReceiveAward(member.UserId),
						IsOpenRechargeGift = this.siteSettings.IsOpenRechargeGift,
						IsSetTradePassword = (!string.IsNullOrEmpty(member.TradePassword) && !string.IsNullOrEmpty(member.TradePasswordSalt) && true),
						AfterSaleCount = MemberProcessor.GetUserAfterSaleCount(HiContext.Current.UserId, false, null),
						ReferralStatusText = ((member.Referral == null) ? "" : EnumDescription.GetEnumDescription((Enum)(object)(ReferralApplyStatus)member.Referral.ReferralStatus, 0)),
						ShopName = ((member.Referral == null) ? "" : member.Referral.ShopName),
						BannerUrl = ((member.Referral == null) ? "" : Globals.HttpsFullPath(member.Referral.BannerUrl)),
						IsRepeled = (member.Referral != null && member.Referral.IsRepeled),
						RefusalReason = ((member.Referral == null) ? "" : member.Referral.RefusalReason),
						RepelTime = ((member.Referral == null) ? new DateTime?(DateTime.MinValue) : member.Referral.RepelTime),
						ReferralGradeId = ((member.Referral != null) ? member.Referral.GradeId : 0),
						ReferralGradeName = ((member.Referral == null) ? "" : member.Referral.GradeName),
						RepelReason = ((member.Referral == null) ? "" : member.Referral.RepelReason),
						Expenditure = member.Expenditure,
						OrderNumber = member.OrderNumber,
						IsTrustLogon = MemberProcessor.IsTrustLoginUser(member),
						ProductConsultaionsCount = ProductBrowser.GetUserProductConsultaionsCount(HiContext.Current.UserId),
						FavoriteCount = ProductBrowser.GetUserFavoriteCount(),
						waitReviewCount = userOrderCount4
					}
				});
				context.Response.Write(s2);
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

		private string GenerateUsername(int length)
		{
			return this.GenerateRndString(length, "u_");
		}

		private string GenerateUsername()
		{
			return this.GenerateRndString(10, "u_");
		}

		private string GeneratePassword()
		{
			return this.GenerateRndString(8, "");
		}

		private string GenerateRndString(int length, string prefix)
		{
			string text = string.Empty;
			Random random = new Random();
			while (text.Length < length)
			{
				int num = random.Next();
				text += ((char)((num % 3 != 0) ? ((ushort)(48 + (ushort)(num % 10))) : ((ushort)(97 + (ushort)(num % 26))))).ToString();
			}
			return prefix + text;
		}

		protected void BindPhoneOrEmail(HttpContext context)
		{
			this.CheckSession(context);
			int userId = HiContext.Current.User.UserId;
			context.Response.ContentType = "application/json";
			string text = context.Request["value"];
			if (string.IsNullOrEmpty(text))
			{
				this.message = JsonConvert.SerializeObject(new
				{
					Success = new
					{
						Status = false,
						Msg = "输入的手机号或邮箱不允许为空"
					}
				});
				context.Response.Write(this.message);
				context.Response.End();
			}
			else
			{
				string value = context.Request["verifyCode"];
				if (string.IsNullOrEmpty(value))
				{
					this.message = JsonConvert.SerializeObject(new
					{
						Success = new
						{
							Status = false,
							Msg = "验证码不能为空"
						}
					});
					context.Response.Write(this.message);
					context.Response.End();
				}
				else
				{
					string text2 = HiCache.Get($"DataCache-PhoneCode-{text}").ToNullString();
					if (string.IsNullOrEmpty(text2))
					{
						text2 = HiCache.Get($"DataCache-EmailCode-{text}").ToNullString();
					}
					text2 = HiCryptographer.TryDecypt(text2);
					if (!text2.Equals(value))
					{
						this.message = JsonConvert.SerializeObject(new
						{
							Success = new
							{
								Status = false,
								Msg = "验证码错误"
							}
						});
						context.Response.Write(this.message);
						context.Response.End();
					}
					else
					{
						MemberInfo user = Users.GetUser(userId);
						if (DataHelper.IsMobile(text))
						{
							MemberInfo memberInfo = MemberProcessor.FindMemberByCellphone(text);
							if (memberInfo != null && memberInfo.UserId != user.UserId)
							{
								this.message = JsonConvert.SerializeObject(new
								{
									Success = new
									{
										Status = false,
										Msg = "手机号码已被其它用户绑定"
									}
								});
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
									Users.ClearUserCache(userId, context.Request["sessionId"].ToNullString());
									this.message = JsonConvert.SerializeObject(new
									{
										Success = new
										{
											Status = true,
											Msg = "成功绑定手机号"
										}
									});
								}
								else
								{
									this.message = JsonConvert.SerializeObject(new
									{
										Success = new
										{
											Status = false,
											Msg = "绑定手机号错误"
										}
									});
								}
							}
							context.Response.Write(this.message);
							context.Response.End();
						}
						else if (DataHelper.IsEmail(text))
						{
							MemberInfo memberInfo2 = MemberProcessor.FindMemberByEmail(text);
							if (memberInfo2 != null && memberInfo2.UserId != user.UserId)
							{
								this.message = JsonConvert.SerializeObject(new
								{
									Success = new
									{
										Status = false,
										Msg = "邮箱已被其它帐号绑定"
									}
								});
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
									Users.ClearUserCache(userId, context.Request["sessionId"].ToNullString());
									this.message = JsonConvert.SerializeObject(new
									{
										Success = new
										{
											Status = true,
											Msg = "成功绑定邮箱"
										}
									});
								}
								else
								{
									this.message = JsonConvert.SerializeObject(new
									{
										Success = new
										{
											Status = false,
											Msg = "绑定邮箱错误"
										}
									});
								}
							}
							context.Response.Write(this.message);
							context.Response.End();
						}
						else
						{
							this.message = JsonConvert.SerializeObject(new
							{
								Success = new
								{
									Status = false,
									Msg = "请输入正确的手机号或邮箱账号"
								}
							});
							context.Response.Write(this.message);
							context.Response.End();
						}
					}
				}
			}
		}

		private void GetPreSaleProductDetail(HttpContext context)
		{
			int num = context.Request["ProSaleId"].ToInt(0);
			if (num <= 0)
			{
				context.Response.Write(this.GetErrorJosn(1006, "数据错误"));
			}
			else
			{
				ProductPreSaleInfo productPreSaleInfo = ProductPreSaleHelper.GetProductPreSaleInfo(num);
				if (productPreSaleInfo == null)
				{
					context.Response.Write(this.GetErrorJosn(1006, "活动不存在"));
				}
				else
				{
					string text = context.Request["LikePid"].ToNullString();
					List<int> list = null;
					if (!string.IsNullOrEmpty(text))
					{
						list = new List<int>();
						string[] array = text.Split(',');
						foreach (string obj in array)
						{
							if (obj.ToInt(0) > 0)
							{
								list.Add(obj.ToInt(0));
							}
						}
					}
					GetPreSaleProductDetailModel getPreSaleProductDetailModel = new GetPreSaleProductDetailModel();
					int productId = productPreSaleInfo.ProductId;
					SiteSettings masterSettings = SettingsManager.GetMasterSettings();
					ProductBrowseInfo productPreSaleBrowseInfo = ProductBrowser.GetProductPreSaleBrowseInfo(productId, false);
					if (productPreSaleBrowseInfo.Product == null || productPreSaleBrowseInfo.Product.SaleStatus == ProductSaleStatus.OnStock)
					{
						context.Response.Write(this.GetErrorJosn(1000, "商品不存在"));
					}
					else if (productPreSaleBrowseInfo.Product.SaleStatus == ProductSaleStatus.Delete)
					{
						context.Response.Write(this.GetErrorJosn(1001, "商品被删除"));
					}
					else if (productPreSaleBrowseInfo.Product.SaleStatus == ProductSaleStatus.UnSale)
					{
						context.Response.Write(this.GetErrorJosn(1002, "商品已被下架"));
					}
					else
					{
						getPreSaleProductDetailModel.PreSaleInfo = productPreSaleInfo;
						bool flag = false;
						if (HiContext.Current.User.UserId > 0)
						{
							flag = ProductBrowser.ExistsProduct(productId, HiContext.Current.UserId, 0);
						}
						int supplierId = productPreSaleBrowseInfo.Product.SupplierId;
						if (supplierId > 0)
						{
							SupplierInfo supplierById = SupplierHelper.GetSupplierById(supplierId);
							if (supplierById != null)
							{
								getPreSaleProductDetailModel.SupplierName = supplierById.SupplierName;
							}
							else
							{
								getPreSaleProductDetailModel.SupplierName = "";
							}
						}
						else
						{
							getPreSaleProductDetailModel.SupplierName = "";
						}
						if (string.IsNullOrEmpty(productPreSaleBrowseInfo.Product.ImageUrl1) && string.IsNullOrEmpty(productPreSaleBrowseInfo.Product.ImageUrl2) && string.IsNullOrEmpty(productPreSaleBrowseInfo.Product.ImageUrl3) && string.IsNullOrEmpty(productPreSaleBrowseInfo.Product.ImageUrl4) && string.IsNullOrEmpty(productPreSaleBrowseInfo.Product.ImageUrl5))
						{
							productPreSaleBrowseInfo.Product.ImageUrl1 = Globals.FullPath(masterSettings.DefaultProductImage);
						}
						productPreSaleBrowseInfo.Product.ImageUrl1 = Globals.FullPath(productPreSaleBrowseInfo.Product.ImageUrl1);
						productPreSaleBrowseInfo.Product.ImageUrl2 = Globals.FullPath(productPreSaleBrowseInfo.Product.ImageUrl2);
						productPreSaleBrowseInfo.Product.ImageUrl3 = Globals.FullPath(productPreSaleBrowseInfo.Product.ImageUrl3);
						productPreSaleBrowseInfo.Product.ImageUrl4 = Globals.FullPath(productPreSaleBrowseInfo.Product.ImageUrl4);
						productPreSaleBrowseInfo.Product.ImageUrl5 = Globals.FullPath(productPreSaleBrowseInfo.Product.ImageUrl5);
						productPreSaleBrowseInfo.Product.ThumbnailUrl60 = Globals.FullPath(productPreSaleBrowseInfo.Product.ThumbnailUrl60);
						productPreSaleBrowseInfo.Product.ThumbnailUrl100 = Globals.FullPath(productPreSaleBrowseInfo.Product.ThumbnailUrl100);
						productPreSaleBrowseInfo.Product.ThumbnailUrl180 = Globals.FullPath(productPreSaleBrowseInfo.Product.ThumbnailUrl180);
						productPreSaleBrowseInfo.Product.ThumbnailUrl220 = Globals.FullPath(productPreSaleBrowseInfo.Product.ThumbnailUrl220);
						productPreSaleBrowseInfo.Product.ThumbnailUrl310 = Globals.FullPath(productPreSaleBrowseInfo.Product.ThumbnailUrl310);
						productPreSaleBrowseInfo.Product.ThumbnailUrl40 = Globals.FullPath(productPreSaleBrowseInfo.Product.ThumbnailUrl40);
						productPreSaleBrowseInfo.Product.ThumbnailUrl410 = Globals.FullPath(productPreSaleBrowseInfo.Product.ThumbnailUrl410);
						getPreSaleProductDetailModel.ProductName = productPreSaleBrowseInfo.Product.ProductName;
						getPreSaleProductDetailModel.MetaDescription = productPreSaleBrowseInfo.Product.Meta_Description;
						getPreSaleProductDetailModel.ShortDescription = productPreSaleBrowseInfo.Product.ShortDescription;
						GetPreSaleProductDetailModel getPreSaleProductDetailModel2 = getPreSaleProductDetailModel;
						int num2 = productPreSaleBrowseInfo.Product.SaleCounts;
						getPreSaleProductDetailModel2.SaleCounts = num2.ToString();
						getPreSaleProductDetailModel.Weight = productPreSaleBrowseInfo.Product.Weight.ToString();
						GetPreSaleProductDetailModel getPreSaleProductDetailModel3 = getPreSaleProductDetailModel;
						num2 = productPreSaleBrowseInfo.Product.VistiCounts;
						getPreSaleProductDetailModel3.VistiCounts = num2.ToString();
						getPreSaleProductDetailModel.CostPrice = productPreSaleBrowseInfo.Product.CostPrice.F2ToString("f2");
						getPreSaleProductDetailModel.MarketPrice = productPreSaleBrowseInfo.Product.MarketPrice.ToDecimal(0).F2ToString("f2");
						getPreSaleProductDetailModel.IsfreeShipping = productPreSaleBrowseInfo.Product.IsfreeShipping.ToString();
						getPreSaleProductDetailModel.MaxSalePrice = productPreSaleBrowseInfo.Product.MaxSalePrice.F2ToString("f2");
						getPreSaleProductDetailModel.MinSalePrice = productPreSaleBrowseInfo.Product.MinSalePrice.F2ToString("f2");
						getPreSaleProductDetailModel.IsFavorite = (flag ? "true" : "false");
						getPreSaleProductDetailModel.ImageUrl1 = productPreSaleBrowseInfo.Product.ImageUrl1;
						getPreSaleProductDetailModel.ImageUrl2 = productPreSaleBrowseInfo.Product.ImageUrl2;
						getPreSaleProductDetailModel.ImageUrl3 = productPreSaleBrowseInfo.Product.ImageUrl3;
						getPreSaleProductDetailModel.ImageUrl4 = productPreSaleBrowseInfo.Product.ImageUrl4;
						getPreSaleProductDetailModel.ImageUrl5 = productPreSaleBrowseInfo.Product.ImageUrl5;
						getPreSaleProductDetailModel.ProductReduce = (masterSettings.ShowDeductInProductPage ? this.GetProductReduceInfo(productId, productPreSaleBrowseInfo.Product, decimal.Zero) : "0");
						productPreSaleBrowseInfo.Product.DefaultSku.SalePrice = decimal.Parse(productPreSaleBrowseInfo.Product.DefaultSku.SalePrice.F2ToString("f2"));
						productPreSaleBrowseInfo.Product.DefaultSku.CostPrice = decimal.Parse(productPreSaleBrowseInfo.Product.DefaultSku.CostPrice.F2ToString("f2"));
						getPreSaleProductDetailModel.DefaultSku = productPreSaleBrowseInfo.Product.DefaultSku;
						getPreSaleProductDetailModel.Stock = productPreSaleBrowseInfo.Product.Stock;
						getPreSaleProductDetailModel.OrderPromotionInfo = this.GetOrderPromotionInfo(productPreSaleBrowseInfo.Product.ProductId);
						getPreSaleProductDetailModel.SkuItem = null;
						string empty = string.Empty;
						if (productPreSaleBrowseInfo.DbSKUs == null || productPreSaleBrowseInfo.DbSKUs.Rows.Count == 0)
						{
							getPreSaleProductDetailModel.SkuItem = new List<SkuItem>();
						}
						else
						{
							getPreSaleProductDetailModel.SkuItem = new List<SkuItem>();
							foreach (DataRow row in productPreSaleBrowseInfo.DbSKUs.Rows)
							{
								if ((from c in getPreSaleProductDetailModel.SkuItem
								where c.AttributeName == row["AttributeName"].ToNullString()
								select c).Count() == 0)
								{
									SkuItem skuItem = new SkuItem();
									skuItem.AttributeName = row["AttributeName"].ToNullString();
									skuItem.AttributeId = row["AttributeId"].ToNullString();
									skuItem.AttributeValue = new List<AttributeValue>();
									IList<string> list2 = new List<string>();
									foreach (DataRow row2 in productPreSaleBrowseInfo.DbSKUs.Rows)
									{
										if (string.Compare((string)row["AttributeName"], (string)row2["AttributeName"]) == 0 && !list2.Contains((string)row2["ValueStr"]))
										{
											AttributeValue attributeValue = new AttributeValue();
											list2.Add((string)row2["ValueStr"]);
											attributeValue.ValueId = row2["ValueId"].ToNullString();
											attributeValue.UseAttributeImage = row2["UseAttributeImage"].ToNullString();
											attributeValue.Value = row2["ValueStr"].ToNullString();
											attributeValue.ImageUrl = Globals.FullPath(row2["ImageUrl"].ToNullString());
											skuItem.AttributeValue.Add(attributeValue);
										}
									}
									getPreSaleProductDetailModel.SkuItem.Add(skuItem);
								}
							}
						}
						getPreSaleProductDetailModel.Skus = new List<SKUItem>();
						foreach (SKUItem value2 in productPreSaleBrowseInfo.Product.Skus.Values)
						{
							value2.SalePrice = decimal.Parse(value2.SalePrice.F2ToString("f2"));
							value2.CostPrice = decimal.Parse(value2.CostPrice.F2ToString("f2"));
							getPreSaleProductDetailModel.Skus.Add(value2);
						}
						int count = 12;
						getPreSaleProductDetailModel.GuessYouLikeProducts = ProductBrowser.GetNewProductYouLikeModel(productId, 0, count, list, false);
						getPreSaleProductDetailModel.ReviewCount = productPreSaleBrowseInfo.ReviewCount;
						getPreSaleProductDetailModel.IsSupportPodrequest = false;
						getPreSaleProductDetailModel.Freight = ShoppingProcessor.CalcProductFreight(HiContext.Current.DeliveryScopRegionId, productPreSaleBrowseInfo.Product.ShippingTemplateId, productPreSaleBrowseInfo.Product.Weight, productPreSaleBrowseInfo.Product.Weight, 1, productPreSaleBrowseInfo.Product.MinSalePrice);
						DataTable couponList = CouponHelper.GetCouponList(productId, HiContext.Current.UserId, false, false, false);
						if (couponList.IsNullOrEmpty())
						{
							getPreSaleProductDetailModel.Coupons = new DataTable();
						}
						else
						{
							couponList.Columns.Add("LimitText", typeof(string));
							couponList.Columns.Add("StartTimeText", typeof(string));
							couponList.Columns.Add("ClosingTimeText", typeof(string));
							foreach (DataRow row3 in couponList.Rows)
							{
								row3["LimitText"] = ((row3["OrderUseLimit"].ToDecimal(0) > decimal.Zero) ? ("订单满" + string.Format("{0:F2}", row3["OrderUseLimit"].ToDecimal(0)) + "元可用") : "无限制");
								DataRow dataRow3 = row3;
								DateTime value = row3["StartTime"].ToDateTime().Value;
								dataRow3["StartTimeText"] = value.ToString("yyyy-MM-dd");
								DataRow dataRow4 = row3;
								value = row3["ClosingTime"].ToDateTime().Value;
								dataRow4["ClosingTimeText"] = value.ToString("yyyy-MM-dd");
							}
							getPreSaleProductDetailModel.Coupons = couponList;
						}
						getPreSaleProductDetailModel.ConsultationCount = productPreSaleBrowseInfo.ConsultationCount;
						getPreSaleProductDetailModel.ProductSendGiftsInfo = this.GetProductSendGifts(productId);
						Regex regex = new Regex("<script[^>]*?>.*?</script>", RegexOptions.IgnoreCase);
						if (!string.IsNullOrWhiteSpace(productPreSaleBrowseInfo.Product.MobbileDescription))
						{
							getPreSaleProductDetailModel.Description = regex.Replace(productPreSaleBrowseInfo.Product.MobbileDescription, "").ToNullString().Replace("\"/Storage/master/gallery/", "\"" + Globals.FullPath("/Storage/master/gallery/"));
						}
						else if (!string.IsNullOrWhiteSpace(productPreSaleBrowseInfo.Product.Description))
						{
							getPreSaleProductDetailModel.Description = regex.Replace(productPreSaleBrowseInfo.Product.Description, "").ToNullString().Replace("\"/Storage/master/gallery/", "\"" + Globals.FullPath("/Storage/master/gallery/"));
						}
						string s = JsonConvert.SerializeObject(new
						{
							Result = getPreSaleProductDetailModel
						});
						context.Response.Write(s);
						context.Response.End();
					}
				}
			}
		}

		public void GetProductConsultation(HttpContext context)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			num = context.Request["productId"].ToInt(0);
			num2 = context.Request["PageIndex"].ToInt(0);
			num3 = context.Request["PageSize"].ToInt(0);
			if (num <= 0 || num3 <= 0 || num2 <= 0)
			{
				context.Response.Write(this.GetErrorJosn(1007, "无数据"));
			}
			else
			{
				PageModel<ProductConsultationInfo> productConsultationList = ProductBrowser.GetProductConsultationList(num, num2, num3);
				if (productConsultationList.Models.Count() == 0)
				{
					context.Response.Write(this.GetErrorJosn(1007, "无数据"));
				}
				else
				{
					GetProductConsultationModel getProductConsultationModel = new GetProductConsultationModel();
					getProductConsultationModel.ProductConsultationlst = productConsultationList.Models.ToList();
					getProductConsultationModel.PageIndex = num2;
					getProductConsultationModel.PageSize = num3;
					getProductConsultationModel.Total = productConsultationList.Total;
					string s = JsonConvert.SerializeObject(new
					{
						Result = getProductConsultationModel
					});
					context.Response.Write(s);
					context.Response.End();
				}
			}
		}

		public void GetStoreHomePageBaseInfo(HttpContext context)
		{
			int num = context.Request["StoreId"].ToInt(0);
			if (num <= 0)
			{
				context.Response.Write(this.GetErrorJosn(101, "缺少必填参数"));
			}
			else
			{
				StoresInfo store = StoresHelper.GetStoreById(num);
				if (store == null)
				{
					context.Response.Write(this.GetErrorJosn(1007, "无数据"));
				}
				else
				{
					List<StoreMarktingInfo> storeMarktingInfoList = StoreMarktingHelper.GetStoreMarktingInfoList();
					StoreActivityEntityList storeActivity = StoresHelper.GetStoreActivity(num);
					DetailException storeStatus = DetailException.Nomal;
					DateTime dateTime;
					if (!store.CloseStatus && store.CloseEndTime.HasValue && store.CloseBeginTime.HasValue)
					{
						if (store.CloseEndTime.Value > DateTime.Now && store.CloseBeginTime.Value < DateTime.Now)
						{
							storeStatus = DetailException.StopService;
						}
					}
					else if (!this.siteSettings.Store_IsOrderInClosingTime)
					{
						dateTime = DateTime.Now;
						string str = dateTime.ToString("yyyy-MM-dd");
						dateTime = store.OpenStartDate;
						DateTime value = (str + " " + dateTime.ToString("HH:mm")).ToDateTime().Value;
						dateTime = DateTime.Now;
						string str2 = dateTime.ToString("yyyy-MM-dd");
						dateTime = store.OpenEndDate;
						DateTime dateTime2 = (str2 + " " + dateTime.ToString("HH:mm")).ToDateTime().Value;
						if (dateTime2 <= value)
						{
							dateTime2 = dateTime2.AddDays(1.0);
						}
						if (DateTime.Now < value || DateTime.Now > dateTime2)
						{
							storeStatus = DetailException.IsNotWorkTime;
						}
					}
					int storeId = store.StoreId;
					string storeName = store.StoreName;
					string address = store.Address;
					double? latitude = store.Latitude;
					double? longitude = store.Longitude;
					bool isStoreDelive = store.IsStoreDelive;
					string minOrderPrice = store.MinOrderPrice.ToDecimal(0).F2ToString("f2");
					string storeFreight = store.StoreFreight.ToDecimal(0).F2ToString("f2");
					dateTime = store.OpenStartDate;
					string openStartDate = dateTime.ToString("HH:mm");
					dateTime = store.OpenEndDate;
					string s = JsonConvert.SerializeObject(new
					{
						Result = new
						{
							StoreId = storeId,
							StoreName = storeName,
							StoreAddress = address,
							Latitude = latitude,
							Longitude = longitude,
							IsStoreDelive = isStoreDelive,
							MinOrderPrice = minOrderPrice,
							StoreFreight = storeFreight,
							OpenStartDate = openStartDate,
							OpenEndDate = dateTime.ToString("HH:mm"),
							StoreStatus = storeStatus,
							StoreLogo = Globals.FullPath(store.StoreImages),
							ActivityList = storeActivity,
							Tel = store.Tel,
							Marktings = from d in storeMarktingInfoList
							select new
							{
								IconUrl = Globals.FullPath(d.IconUrl),
								IconName = d.MarktingTypeText,
								RedirectTo = this.RedirectToFullPath(d.RedirectTo, store.StoreId)
							}
						}
					});
					context.Response.Write(s);
					context.Response.End();
				}
			}
		}

		private string RedirectToFullPath(string url, int StoreId)
		{
			if (url.IndexOf("page:") > -1)
			{
				return url + "/null/StoreId=" + StoreId;
			}
			if (url.IndexOf('/') == -1)
			{
				if (url.Contains("FightGroupActivities"))
				{
					return "page:FightGroupList";
				}
				url = "/appshop/" + url;
			}
			return Globals.FullPath(url) + "?StoreId=" + StoreId;
		}

		public void GetStoreHomePageFloorList(HttpContext context)
		{
			int num = context.Request["PageIndex"].ToInt(0);
			int num2 = context.Request["PageSize"].ToInt(0);
			num2 = ((num2 <= 0) ? 2 : num2);
			num = ((num <= 0) ? 1 : num);
			int num3 = context.Request["StoreId"].ToInt(0);
			if (num3 <= 0)
			{
				context.Response.Write(this.GetErrorJosn(101, "缺少必填参数"));
			}
			else
			{
				StoreFloorQuery storeFloorQuery = new StoreFloorQuery();
				storeFloorQuery.PageIndex = num;
				storeFloorQuery.PageSize = num2;
				storeFloorQuery.StoreID = num3;
				storeFloorQuery.SortBy = "DisplaySequence";
				storeFloorQuery.SortOrder = SortAction.Asc;
				storeFloorQuery.ProductType = ProductType.PhysicalProduct;
				PageModel<StoreFloorInfo> storeFloorList = StoresHelper.GetStoreFloorList(storeFloorQuery);
				if (storeFloorList.Models.Count() == 0)
				{
					context.Response.Write("{\"Result\":[]}");
				}
				else
				{
					string s2 = JsonConvert.SerializeObject(new
					{
						Result = from s in storeFloorList.Models
						select new
						{
							FloorId = s.FloorId,
							FloorName = s.FloorName,
							ImageId = s.ImageId,
							ImageUrl = Globals.FullPath(s.ImageUrl),
							DisplaySequence = s.DisplaySequence,
							Quantity = s.Quantity,
							Products = from d in s.Products
							select new
							{
								StoreId = d.StoreId,
								ProductId = d.ProductId,
								ProductName = d.ProductName,
								ProductImage = this.ProcessImg(d.ProductImage),
								Price = d.Price.F2ToString("f2")
							}
						}
					});
					context.Response.Write(s2);
					context.Response.End();
				}
			}
		}

		public void GetMarketingImageBaseInfo(HttpContext context)
		{
			int num = context.Request["ImageId"].ToInt(0);
			if (num <= 0)
			{
				context.Response.Write(this.GetErrorJosn(101, "缺少必填参数"));
			}
			else
			{
				MarketingImagesInfo marketingImagesInfo = MarketingImagesHelper.GetMarketingImagesInfo(num);
				if (marketingImagesInfo == null)
				{
					context.Response.Write(this.GetErrorJosn(1007, "无数据"));
				}
				else
				{
					string s = JsonConvert.SerializeObject(new
					{
						Result = new
						{
							ImageName = marketingImagesInfo.ImageName,
							ImageUrl = Globals.FullPath(marketingImagesInfo.ImageUrl),
							Description = marketingImagesInfo.Description
						}
					});
					context.Response.Write(s);
					context.Response.End();
				}
			}
		}

		public void GetStoreMarketingProducts(HttpContext context)
		{
			int num = context.Request["PageIndex"].ToInt(0);
			int num2 = context.Request["PageSize"].ToInt(0);
			num2 = ((num2 <= 0) ? 6 : num2);
			num = ((num <= 0) ? 1 : num);
			int num3 = context.Request["StoreId"].ToInt(0);
			if (num3 <= 0)
			{
				context.Response.Write(this.GetErrorJosn(101, "缺少必填参数"));
			}
			else
			{
				int num4 = context.Request["ImageId"].ToInt(0);
				if (num4 <= 0)
				{
					context.Response.Write(this.GetErrorJosn(101, "缺少必填参数"));
				}
				else
				{
					StoreMarketingImagesInfo storeMarketingImages = MarketingImagesHelper.GetStoreMarketingImages(num3, num4);
					if (storeMarketingImages == null)
					{
						context.Response.Write(this.GetErrorJosn(1007, "无数据"));
					}
					else
					{
						ProductBrowseQuery productBrowseQuery = new ProductBrowseQuery();
						productBrowseQuery.PageIndex = num;
						productBrowseQuery.PageSize = num2;
						productBrowseQuery.StoreId = num3;
						productBrowseQuery.CanUseProducts = storeMarketingImages.ProductIds;
						productBrowseQuery.ProductType = ProductType.PhysicalProduct;
						DbQueryResult storeProductList = StoresHelper.GetStoreProductList(productBrowseQuery);
						DataTable data = storeProductList.Data;
						if (data == null || data.Rows.Count == 0)
						{
							context.Response.Write("{\"Result\":[]}");
						}
						else
						{
							List<GetProductsModel> list = new List<GetProductsModel>();
							foreach (DataRow row in data.Rows)
							{
								GetProductsModel getProductsModel = new GetProductsModel();
								getProductsModel.name = row["ProductName"].ToNullString();
								getProductsModel.pic = ((row["ThumbnailUrl310"].ToNullString() == "") ? Globals.FullPath(this.siteSettings.DefaultProductThumbnail4) : Globals.FullPath((string)row["ThumbnailUrl310"]));
								getProductsModel.price = row["SalePrice"].ToDecimal(0).F2ToString("f2");
								getProductsModel.saleCounts = row["SaleCounts"].ToInt(0).ToString();
								getProductsModel.url = Globals.FullPath(string.Format("/wapShop/" + ((row["ProductType"].ToInt(0) == 1.GetHashCode()) ? "ServiceProductDetails.aspx" : "StoreProductDetails.aspx") + "?productId={0}{1}", row["ProductId"], (num3 > 0) ? ("&StoreId=" + num3) : ""));
								getProductsModel.pid = row["ProductId"].ToNullString();
								list.Add(getProductsModel);
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
							context.Response.End();
						}
					}
				}
			}
		}

		public void GetStoreListBaners(HttpContext context)
		{
			string urlPre = Globals.HostPath(context.Request.Url);
			var result = from t in StoreListHelper.GetBannerList()
			select new
			{
				ImgSrc = urlPre + t.Key,
				Href = ((string.IsNullOrEmpty(t.Value) || t.Value.ToLower().StartsWith("http")) ? t.Value : (urlPre + t.Value))
			};
			string s = JsonConvert.SerializeObject(new
			{
				Result = result
			});
			context.Response.Write(s);
			context.Response.End();
		}

		public void GetStoreListTags(HttpContext context)
		{
			string urlPre = Globals.HostPath(context.Request.Url);
			List<StoreTagInfo> tagsList = StoreListHelper.GetTagsList();
			tagsList.ForEach(delegate(StoreTagInfo t)
			{
				t.TagImgSrc = urlPre + t.TagImgSrc;
			});
			string s = JsonConvert.SerializeObject(new
			{
				Result = tagsList
			});
			context.Response.Write(s);
			context.Response.End();
		}

		public void GetStoreList(HttpContext context)
		{
			if (context.Request["Lan"].ToInt(0) == 0 || context.Request["Lng"].ToInt(0) == 0)
			{
				context.Response.Write(this.GetErrorJosn(101, ((Enum)(object)ApiErrorCode.Paramter_Error).ToDescription()));
			}
			else
			{
				StoreEntityQuery storeEntityQuery = AppShopHandler.BuildQuery(context);
				storeEntityQuery.ProductType = ProductType.PhysicalProduct;
				PageModel<StoreEntity> storeRecommend = StoreListHelper.GetStoreRecommend(storeEntityQuery);
				string s = JsonConvert.SerializeObject(new
				{
					Result = storeRecommend
				});
				context.Response.Write(s);
				context.Response.End();
			}
		}

		private static StoreEntityQuery BuildQuery(HttpContext context)
		{
			int num = context.Request["PageIndex"].ToInt(0);
			int num2 = context.Request["PageSize"].ToInt(0);
			num2 = ((num2 <= 0) ? 3 : num2);
			num = ((num <= 0) ? 1 : num);
			int tagId = 0;
			if (!string.IsNullOrEmpty(context.Request["TagId"]))
			{
				tagId = context.Request["TagId"].ToInt(0);
			}
			int regionId = 0;
			int areaId = 0;
			DepotHelper.GetRegionIdFromLonLan(context.Request["Lan"].ToString() + "," + context.Request["Lng"].ToString(), out regionId, out areaId);
			return new StoreEntityQuery
			{
				TagId = tagId,
				AreaId = areaId,
				PageIndex = num,
				PageSize = num2,
				Position = new PositionInfo(context.Request["Lan"].ToDouble(0), context.Request["Lng"].ToDouble(0)),
				RegionId = regionId,
				Key = context.Request["Key"],
				CategoryId = context.Request["CategoryId"].ToInt(0),
				MainCategoryPath = ((context.Request["CategoryId"].ToInt(0) > 0) ? CatalogHelper.GetCategory(context.Request["CategoryId"].ToInt(0)).Path : "")
			};
		}

		private void SearchInStoreList(HttpContext context)
		{
			if (string.IsNullOrEmpty(context.Request["Key"]) && context.Request["CategoryId"].ToInt(0) == 0)
			{
				context.Response.Write(this.GetErrorJosn(101, "请输入关键字"));
			}
			else if (context.Request["Lan"].ToInt(0) == 0 || context.Request["Lng"].ToInt(0) == 0)
			{
				context.Response.Write(this.GetErrorJosn(101, "定位失败，无法获取数据"));
			}
			else
			{
				StoreEntityQuery storeEntityQuery = AppShopHandler.BuildQuery(context);
				storeEntityQuery.ProductType = ProductType.PhysicalProduct;
				PageModel<StoreEntity> pageModel = StoreListHelper.SearchPdInStoreList(storeEntityQuery);
				pageModel.Models.ForEach(delegate(StoreEntity r)
				{
					r.ProductList.ForEach(delegate(StoreProductEntity p)
					{
						p.ThumbnailUrl220 = this.ProcessImg(p.ThumbnailUrl220);
					});
				});
				string s = JsonConvert.SerializeObject(new
				{
					Result = pageModel
				});
				context.Response.Write(s);
				context.Response.End();
			}
		}

		public void GetPositionPageTurnParam(HttpContext context)
		{
			string text = Globals.UrlDecode(context.Request["fromLatLng"].ToNullString());
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			string s = JsonConvert.SerializeObject(new
			{
				masterSettings.OpenMultStore,
				masterSettings.Store_PositionRouteTo,
				masterSettings.Store_PositionNoMatchTo
			});
			context.Response.Write(s);
			context.Response.End();
		}

		public void GetNearestStore(HttpContext context)
		{
			string text = Globals.UrlDecode(context.Request["fromLatLng"].ToNullString());
			string sessionId = context.Request["sessionId"].ToNullString();
			context.Response.ContentType = "text/json";
			if (string.IsNullOrEmpty(text))
			{
				context.Response.Write("{\"Status\":\"noLatLng\",\"Message\":\"定位失败！\"}");
			}
			else
			{
				try
				{
					SiteSettings masterSettings = SettingsManager.GetMasterSettings();
					if (!masterSettings.OpenMultStore)
					{
						context.Response.Write("{\"Status\":\"platform\",\"Message\":\"进入平台页面！\"}");
						goto end_IL_0065;
					}
					string store_PositionRouteTo = masterSettings.Store_PositionRouteTo;
					text = text.Trim().Replace(" ", "");
					MemberInfo memberInfo = MemberProcessor.FindMemberBySessionId(sessionId);
					if (memberInfo == null)
					{
						memberInfo = HiContext.Current.User;
					}
					if (memberInfo.UserId > 0)
					{
						goto IL_00e6;
					}
					goto IL_00e6;
					IL_00e6:
					if (store_PositionRouteTo.Equals("NearestStore"))
					{
						if (memberInfo != null && memberInfo.StoreId > 0 && masterSettings.Store_IsMemberVisitBelongStore)
						{
							DepotHelper.CookieUserCoordinate(text);
							context.Response.Write("{\"Status\":\"goToStore\",\"Message\":\"进入门店首页！\",\"StoreId\":\"" + memberInfo.StoreId + "\"}");
						}
						else
						{
							StoresInfo nearDeliveStores = DepotHelper.GetNearDeliveStores(text, false);
							if (nearDeliveStores == null || nearDeliveStores.StoreId <= 0)
							{
								string store_PositionNoMatchTo = masterSettings.Store_PositionNoMatchTo;
								if (store_PositionNoMatchTo == "Platform")
								{
									context.Response.Write("{\"Status\":\"platform\",\"Message\":\"进入平台页面！\"}");
								}
								else
								{
									context.Response.Write("{\"Status\":\"nothing\",\"Message\":\"进入无平台提示页面！\"}");
								}
							}
							else
							{
								context.Response.Write("{\"Status\":\"goToStore\",\"Message\":\"进入门店首页！\",\"StoreId\":\"" + nearDeliveStores.StoreId + "\"}");
							}
						}
					}
					else if (store_PositionRouteTo.Equals("StoreList"))
					{
						StoresInfo nearDeliveStores2 = DepotHelper.GetNearDeliveStores(text, false);
						if (nearDeliveStores2 == null || nearDeliveStores2.StoreId <= 0)
						{
							string store_PositionNoMatchTo2 = masterSettings.Store_PositionNoMatchTo;
							if (store_PositionNoMatchTo2 == "Platform")
							{
								context.Response.Write("{\"Status\":\"platform\",\"Message\":\"进入平台页面！\"}");
							}
							else
							{
								context.Response.Write("{\"Status\":\"nothing\",\"Message\":\"进入无平台提示页面！\"}");
							}
						}
						else
						{
							context.Response.Write("{\"Status\":\"storeList\",\"Message\":\"进入多门店首页！\"}");
						}
					}
					else if (store_PositionRouteTo.Equals("Platform"))
					{
						DepotHelper.CookieUserCoordinate(text);
						if (memberInfo != null && memberInfo.StoreId > 0 && masterSettings.Store_IsMemberVisitBelongStore)
						{
							context.Response.Write("{\"Status\":\"goToStore\",\"Message\":\"进入门店首页！\",\"StoreId\":\"" + memberInfo.StoreId + "\"}");
						}
						else
						{
							context.Response.Write("{\"Status\":\"platform\",\"Message\":\"进入平台页面！\"}");
						}
					}
					end_IL_0065:;
				}
				catch (Exception ex)
				{
					NameValueCollection param = new NameValueCollection
					{
						context.Request.QueryString,
						context.Request.Form
					};
					Globals.WriteExceptionLog_Page(ex, param, "AppGetNearestStore");
					context.Response.Write("{\"Status\":\"error\",\"Message\":\"" + ex.Message + "\"}");
				}
			}
		}
	}
}
