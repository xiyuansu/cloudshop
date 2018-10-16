using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.Messages;
using Hidistro.SaleSystem.Members;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Store;
using Hidistro.SqlDal.Members;
using Hidistro.SqlDal.Orders;
using Hidistro.SqlDal.Sales;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Hidistro.SaleSystem.Member
{
	public static class MemberProcessor
	{
		private static Regex emailR = new Regex("^\\w+((-\\w+)|(\\.\\w+))*\\@[A-Za-z0-9]+((\\.|-)[A-Za-z0-9]+)*\\.[A-Za-z0-9]+$", RegexOptions.Compiled);

		private static Regex cellphoneR = new Regex("^0?(13|15|18|14|17)[0-9]{9}$", RegexOptions.Compiled);

		public static int GetDefaultMemberGrade()
		{
			return new MemberGradeDao().GetDefaultMemberGrade();
		}

		public static MemberGradeInfo GetMemberGrade(int gradeId)
		{
			return new MemberGradeDao().Get<MemberGradeInfo>(gradeId);
		}

		public static int CreateMember(MemberInfo member)
		{
			member.IsOpenBalance = true;
			if (member.ShoppingGuiderId <= 0)
			{
				member.ShoppingGuiderId = HiContext.Current.ShoppingGuiderId.ToInt(0);
			}
			int num = 0;
			ManagerInfo managerInfo = new MemberDao().Get<ManagerInfo>(member.ShoppingGuiderId);
			if (managerInfo != null)
			{
				num = managerInfo.StoreId;
			}
			member.StoreId = num;
			member.O2OStoreId = num;
			int num2 = (int)new MemberDao().Add(member, null);
			if (num2 > 0)
			{
				member.UserId = num2;
				if (member.ReferralUserId <= 0 && HiContext.Current.ReferralUserId > 0)
				{
					member.ReferralUserId = HiContext.Current.ReferralUserId;
				}
				if (member.ReferralUserId > 0)
				{
					MemberProcessor.AddRegReferralDeduct(member.ReferralUserId, member.UserId, Globals.IPAddress);
					MemberInfo user = Users.GetUser(member.ReferralUserId);
					Messenger.SubMemberDevelopment(user, member.UserName);
				}
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				if (masterSettings.MemberRegistrationPoint > 0)
				{
					int memberRegistrationPoint = masterSettings.MemberRegistrationPoint;
					PointDetailInfo pointDetailInfo = new PointDetailInfo();
					pointDetailInfo.OrderId = "";
					pointDetailInfo.UserId = num2;
					pointDetailInfo.TradeDate = DateTime.Now;
					pointDetailInfo.TradeType = PointTradeType.MemberRegistration;
					pointDetailInfo.Increased = memberRegistrationPoint;
					pointDetailInfo.Points = memberRegistrationPoint;
					if (pointDetailInfo.Increased > 0)
					{
						PointDetailDao pointDetailDao = new PointDetailDao();
						pointDetailDao.Add(pointDetailInfo, null);
						member.Points = pointDetailInfo.Points;
						MemberDao memberDao = new MemberDao();
						int historyPoint = pointDetailDao.GetHistoryPoint(member.UserId, null);
						memberDao.ChangeMemberGrade(member.UserId, member.GradeId, historyPoint, null);
					}
				}
				if (masterSettings.RegisterBecomePromoter)
				{
					MemberProcessor.AddRegisterReferral(num2, ReferralApplyStatus.Audited);
				}
			}
			HttpCookie httpCookie = HiContext.Current.Context.Request.Cookies["Site_ReferralUser"];
			if (httpCookie != null && !string.IsNullOrEmpty(httpCookie.Value))
			{
				httpCookie.Expires = new DateTime(1911, 10, 12);
				HttpContext.Current.Response.Cookies.Add(httpCookie);
			}
			HttpCookie httpCookie2 = HiContext.Current.Context.Request.Cookies["Store_ShoppingGuider"];
			if (httpCookie2 != null && !string.IsNullOrEmpty(httpCookie2.Value))
			{
				httpCookie2.Expires = new DateTime(1911, 10, 12);
				HttpContext.Current.Response.Cookies.Add(httpCookie2);
			}
			Users.ClearUserCache(num2, "");
			return num2;
		}

		public static bool AddRegisterReferral(int userId, ReferralApplyStatus referralType)
		{
			MemberInfo user = Users.GetUser(userId);
			if (user == null)
			{
				return false;
			}
			ReferralInfo referralInfo = new ReferralInfo();
			referralInfo.AuditDate = DateTime.Now;
			referralInfo.ReferralStatus = (int)referralType;
			referralInfo.RefusalReason = string.Empty;
			referralInfo.RequetDate = DateTime.Now;
			referralInfo.RequetReason = string.Empty;
			referralInfo.UserId = userId;
			referralInfo.ShopName = (string.IsNullOrEmpty(user.NickName) ? user.UserName : user.NickName) + "的小店";
			ReferralGradeInfo defaultOrFirstReferralGradeInfo = MemberProcessor.GetDefaultOrFirstReferralGradeInfo();
			if (defaultOrFirstReferralGradeInfo != null && defaultOrFirstReferralGradeInfo.CommissionThreshold == decimal.Zero)
			{
				referralInfo.GradeId = defaultOrFirstReferralGradeInfo.GradeId;
			}
			return new ReferralDao().Add(referralInfo, null) > 0;
		}

		public static MemberInfo FindMemberByUsername(string userName)
		{
			return new MemberDao().GetMember(userName);
		}

		public static MemberInfo FindMemberBySessionId(string sessionId)
		{
			MemberInfo memberInfo = HiCache.Get<MemberInfo>($"DataCache-APPMemberCacheKey-{sessionId}");
			if (memberInfo == null)
			{
				memberInfo = new MemberDao().FindMemberBySessionId(sessionId);
				if (memberInfo != null)
				{
					HiCache.Insert($"DataCache-APPMemberCacheKey-{sessionId}", memberInfo, 1800);
				}
			}
			return memberInfo;
		}

		public static bool ValidCellPhone(string cellPhone)
		{
			MemberInfo user = HiContext.Current.User;
			if (user == null)
			{
				return false;
			}
			user.CellPhoneVerification = true;
			user.CellPhone = cellPhone;
			bool flag = new MemberDao().Update(user, null);
			if (flag)
			{
				Users.ClearUserCache(user.UserId, user.SessionId);
			}
			return flag;
		}

		public static bool ValidEmail(string email)
		{
			MemberInfo user = HiContext.Current.User;
			if (user == null)
			{
				return false;
			}
			user.EmailVerification = true;
			user.Email = email;
			bool flag = new MemberDao().Update(user, null);
			if (flag)
			{
				Users.ClearUserCache(user.UserId, user.SessionId);
			}
			return flag;
		}

		public static bool ChangePassword(MemberInfo memberInfo, string newPassword)
		{
			if (string.IsNullOrEmpty(memberInfo.PasswordSalt))
			{
				memberInfo.PasswordSalt = Globals.RndStr(128, true);
			}
			memberInfo.Password = Users.EncodePassword(newPassword, memberInfo.PasswordSalt);
			return new MemberDao().Update(memberInfo, null);
		}

		public static bool UpdateMember(MemberInfo member)
		{
			bool flag = new MemberDao().Update(member, null);
			if (flag)
			{
				Users.ClearUserCache(member.UserId, member.SessionId);
			}
			return flag;
		}

		public static bool ChangeTradePassword(MemberInfo memberInfo, string newPassword)
		{
			if (string.IsNullOrEmpty(memberInfo.TradePasswordSalt))
			{
				memberInfo.TradePasswordSalt = Globals.RndStr(128, true);
			}
			memberInfo.TradePassword = Users.EncodePassword(newPassword, memberInfo.TradePasswordSalt);
			return new MemberDao().Update(memberInfo, null);
		}

		public static bool OpenBalance(string password)
		{
			MemberInfo user = HiContext.Current.User;
			user.IsOpenBalance = true;
			if (!string.IsNullOrWhiteSpace(user.TradePasswordSalt))
			{
				user.TradePasswordSalt = user.TradePasswordSalt;
			}
			else
			{
				user.TradePasswordSalt = Globals.RndStr(128, true);
			}
			user.TradePassword = Users.EncodePassword(password, user.TradePasswordSalt);
			return new MemberDao().Update(user, null);
		}

		public static MemberInfo ValidLogin(string userName, string password)
		{
			MemberInfo memberInfo = new MemberDao().GetMember(userName);
			if (memberInfo == null && MemberProcessor.emailR.IsMatch(userName))
			{
				memberInfo = new MemberDao().FindMemberByEmail(userName);
				if (memberInfo == null)
				{
					return null;
				}
			}
			if (memberInfo == null && MemberProcessor.cellphoneR.IsMatch(userName))
			{
				memberInfo = new MemberDao().FindMemberByCellphone(userName);
				if (memberInfo == null)
				{
					return null;
				}
			}
			if (memberInfo == null || memberInfo.Password != Users.EncodePassword(password, memberInfo.PasswordSalt))
			{
				return null;
			}
			return memberInfo;
		}

		public static bool ValidTradePassword(string tradePassword)
		{
			MemberInfo user = HiContext.Current.User;
			if (user == null)
			{
				return false;
			}
			if (!string.IsNullOrWhiteSpace(tradePassword) && (user.TradePassword == Users.EncodePassword(tradePassword, user.TradePasswordSalt) || user.TradePassword == Users.EncodePassword_Old(tradePassword, user.TradePasswordSalt)))
			{
				return true;
			}
			return false;
		}

		public static bool IsUseEmail(string email)
		{
			MemberInfo memberInfo = new MemberDao().FindMemberByEmail(email);
			if (memberInfo == null)
			{
				return false;
			}
			return true;
		}

		public static bool IsUseCellphone(string cellphone)
		{
			MemberInfo memberInfo = new MemberDao().FindMemberByCellphone(cellphone);
			if (memberInfo == null)
			{
				return false;
			}
			return true;
		}

		public static MemberInfo FindMemberByCellphone(string cellphone)
		{
			return new MemberDao().FindMemberByCellphone(cellphone);
		}

		public static MemberInfo FindMemberByEmail(string email)
		{
			return new MemberDao().FindMemberByEmail(email);
		}

		public static bool UpdateMemberOpenId(MemberOpenIdInfo memberOpenId)
		{
			return new MemberOpenIdDao().Update(memberOpenId, null);
		}

		public static bool AddMemberOpenId(MemberOpenIdInfo memberOpenId)
		{
			if (!OpenIdHelper.IsExistOpenIdSetting(memberOpenId.OpenIdType))
			{
				string openIdName = MemberProcessor.GetOpenIdName(memberOpenId.OpenIdType);
				OpenIdSettingInfo openIdSettingInfo = new OpenIdSettingInfo();
				openIdSettingInfo.Description = openIdName;
				openIdSettingInfo.Name = openIdName;
				openIdSettingInfo.OpenIdType = memberOpenId.OpenIdType;
				openIdSettingInfo.Settings = "";
				OpenIdHelper.SaveSettings(openIdSettingInfo);
			}
			return new MemberOpenIdDao().Add(memberOpenId, null) > 0;
		}

		public static string GetOpenIdName(string openIdType)
		{
			string result = "";
			switch (openIdType.ToLower())
			{
			case "hishop.plugins.openid.appweixin":
				result = "APP微信信任登录";
				break;
			case "hishop.plugins.openid.qq.appqqservicet":
				result = "APP腾讯QQ信任登录";
				break;
			case "hishop.plugins.openid.sina.appsinaservice":
				result = "APP新浪微博信任登录";
				break;
			case "hishop.plugins.openid.weixin":
				result = "微信信任登录";
				break;
			case "hishop.plugins.openid.qq.qqservice":
				result = "腾讯QQ信任登录";
				break;
			case "hishop.plugins.openid.wxapplet":
				result = "小程序微信信任登录";
				break;
			case "hishop.plugins.openid.o2owxapplet":
				result = "O2O小程序微信信任登录";
				break;
			case "hishop.plugins.openid.alipay.alipayservice":
				result = "支付宝快捷登录";
				break;
			case "hishop.plugins.openid.taobao.taobaoservice":
				result = "淘宝信任登录";
				break;
			case "hishop.plugins.openid.sina.sinaservice":
				result = "新浪微博登录";
				break;
			case "hishop.plugins.openid.weixin.weixinservice":
				result = "微信扫码登录";
				break;
			}
			return result;
		}

		public static bool DeleteMemberOpenId(int userId, string openIdType)
		{
			return new MemberOpenIdDao().DeleteMemberOpenId(userId, openIdType);
		}

		public static MemberOpenIdInfo GetMemberOpenId(string openIdType, string openId)
		{
			return new MemberOpenIdDao().GetMemberByOpenId(openIdType, openId);
		}

		public static MemberInfo GetMemberByOpenId(string openType, string openId)
		{
			return new MemberDao().GetMemberByOpenId(openType, openId);
		}

		public static MemberInfo GetMemberByOpenIdOfQuickLogin(string openType, string openId)
		{
			return new MemberDao().GetMemberByOpenIdOfQuickLogin(openType, openId);
		}

		public static void UpdateWXUserIsSubscribeStatus(string openId, bool isSubscribe)
		{
			IList<int> memberIdsByWXOpenId = new MemberDao().GetMemberIdsByWXOpenId(openId);
			if (memberIdsByWXOpenId != null && memberIdsByWXOpenId.Count > 0)
			{
				string userIds = string.Join(",", memberIdsByWXOpenId);
				if (new MemberDao().UpdateWXUserIsSubscribeStatus(userIds, isSubscribe))
				{
					foreach (int item in memberIdsByWXOpenId)
					{
						Users.ClearUserCache(item, "");
					}
				}
			}
		}

		public static bool IsBindedWeixin(int userId, string openIdType = "hishop.plugins.openid.weixin")
		{
			return new MemberOpenIdDao().IsBindedWeixin(userId, openIdType);
		}

		public static MemberInfo GetMemberByOpenId_App(string openIdType, string openId)
		{
			return new MemberDao().GetMemberByOpenId_App(openIdType, openIdType.Replace("app", ""), openId);
		}

		public static MemberInfo GetMemberByUnionId(string unionId)
		{
			if (string.IsNullOrEmpty(unionId) || unionId.Trim() == "")
			{
				return null;
			}
			return new MemberDao().GetMemberByUnionId(unionId);
		}

		public static MemberOpenIdInfo GetMemberOpenIdInfo(int userId, string openIdType)
		{
			return new MemberOpenIdDao().GetMemberOpenIdInfo(userId, openIdType);
		}

		public static bool ChangePasswordQuestionAndAnswer(string answer, string newQuestion, string newAnswer)
		{
			MemberInfo user = HiContext.Current.User;
			if (!string.IsNullOrEmpty(answer) && user.PasswordAnswer != answer)
			{
				return false;
			}
			user.PasswordQuestion = newQuestion;
			user.PasswordAnswer = newAnswer;
			bool flag = new MemberDao().Update(user, null);
			if (flag)
			{
				Users.ClearUserCache(user.UserId, user.SessionId);
			}
			return flag;
		}

		public static DbQueryResult GetMySubUsers(MemberQuery query)
		{
			return new MemberDao().GetMySubUsers(query, HiContext.Current.UserId);
		}

		public static PageModel<SubMember> GetMySubUserList(MemberQuery query)
		{
			return new MemberDao().GetMySubUserList(query, HiContext.Current.UserId);
		}

		public static SubReferralUser GetMyReferralSubUser(int UserId)
		{
			return new MemberDao().GetMyReferralSubUser(UserId);
		}

		public static SubMember GetMySubUser(int UserId)
		{
			return new MemberDao().GetMySubMember(UserId);
		}

		public static int GetMySubUsersCount(MemberQuery query, int UserId)
		{
			return new MemberDao().GetMySubUsersCount(query, UserId);
		}

		public static DbQueryResult GetMySplittinDetails(BalanceDetailQuery query, bool? isUse)
		{
			return new ReferralDao().GetSplittinDetails(query, isUse, 0);
		}

		public static SplittinDrawInfo GetSplittinDraw(long journalNumber)
		{
			return new ReferralDao().Get<SplittinDrawInfo>(journalNumber);
		}

		public static SplittinDetailInfo GetSplittinDetail(long JournalNumber)
		{
			return new ReferralDao().Get<SplittinDetailInfo>(JournalNumber);
		}

		public static bool ReferralInfoSet(ReferralInfo referral)
		{
			if (referral == null)
			{
				return false;
			}
			return new ReferralDao().Update(referral, null);
		}

		public static bool ReferralRequest(int userId, string realName, string cellPhone, int topRegionId, int regionId, string address, string email, string shopName, string bannerUrl)
		{
			ReferralInfo referralInfo = Users.GetReferralInfo(userId);
			MemberInfo user = Users.GetUser(userId);
			if (referralInfo == null)
			{
				referralInfo = new ReferralInfo();
				if (string.IsNullOrEmpty(shopName))
				{
					if (user != null)
					{
						referralInfo.ShopName = (string.IsNullOrEmpty(user.NickName) ? user.UserName : user.NickName) + "的小店";
					}
				}
				else
				{
					referralInfo.ShopName = shopName;
				}
				referralInfo.BannerUrl = bannerUrl;
				referralInfo.CellPhone = cellPhone;
				referralInfo.Email = email;
				if (HiContext.Current.SiteSettings.RegisterBecomePromoter)
				{
					referralInfo.ReferralStatus = 2;
					referralInfo.AuditDate = DateTime.Now;
				}
				else
				{
					referralInfo.ReferralStatus = 1;
				}
				referralInfo.RequetReason = MemberProcessor.GetRequestReason(realName, cellPhone, email, address, regionId);
				referralInfo.RequetDate = DateTime.Now;
				referralInfo.UserId = userId;
				ReferralGradeInfo defaultOrFirstReferralGradeInfo = MemberProcessor.GetDefaultOrFirstReferralGradeInfo();
				if (defaultOrFirstReferralGradeInfo != null && defaultOrFirstReferralGradeInfo.CommissionThreshold == decimal.Zero)
				{
					referralInfo.GradeId = defaultOrFirstReferralGradeInfo.GradeId;
				}
				return new ReferralDao().Add(referralInfo, null) > 0;
			}
			if (string.IsNullOrEmpty(shopName))
			{
				if (user != null)
				{
					referralInfo.ShopName = (string.IsNullOrEmpty(user.NickName) ? user.UserName : user.NickName) + "的小店";
				}
			}
			else
			{
				referralInfo.ShopName = shopName;
			}
			referralInfo.BannerUrl = bannerUrl;
			referralInfo.CellPhone = cellPhone;
			referralInfo.Email = email;
			if (HiContext.Current.SiteSettings.RegisterBecomePromoter)
			{
				referralInfo.ReferralStatus = 2;
				referralInfo.AuditDate = DateTime.Now;
			}
			else
			{
				referralInfo.ReferralStatus = 1;
			}
			referralInfo.RequetReason = MemberProcessor.GetRequestReason(realName, cellPhone, email, address, regionId);
			referralInfo.RequetDate = DateTime.Now;
			referralInfo.UserId = userId;
			ReferralGradeInfo defaultOrFirstReferralGradeInfo2 = MemberProcessor.GetDefaultOrFirstReferralGradeInfo();
			if (defaultOrFirstReferralGradeInfo2 != null && defaultOrFirstReferralGradeInfo2.CommissionThreshold == decimal.Zero)
			{
				referralInfo.GradeId = defaultOrFirstReferralGradeInfo2.GradeId;
			}
			return new ReferralDao().Update(referralInfo, null);
		}

		public static string GetRequestReason(string realName, string cellPhone, string email, string address, int regionId)
		{
			ReferralExtInfo obj = new ReferralExtInfo
			{
				RealName = realName,
				CellPhone = cellPhone,
				Email = email,
				Address = address,
				RegionId = regionId
			};
			return JsonHelper.GetJson(obj);
		}

		public static DbQueryResult GetMySplittinDraws(BalanceDrawRequestQuery query, int? auditStatus)
		{
			return new ReferralDao().GetSplittinDraws(query, auditStatus);
		}

		public static PageModel<SplittinDrawInfo> GetMySplittinDrawList(BalanceDrawRequestQuery query, int? auditStatus)
		{
			return new ReferralDao().GetSplittinDrawList(query, auditStatus);
		}

		public static SplittinDrawInfo GetMyRecentlySplittinDraws()
		{
			if (HiContext.Current.UserId == 0)
			{
				return null;
			}
			return new ReferralDao().GetMyRecentlySplittinDraws(HiContext.Current.UserId);
		}

		public static bool SplittinDrawRequest(SplittinDrawInfo splittinDraw)
		{
			return new ReferralDao().SplittinDrawRequest(splittinDraw);
		}

		public static decimal GetUserWithdrawalsSplittin(int userId)
		{
			return new ReferralDao().GetUserWithdrawalsSplittin(userId);
		}

		public static decimal GetUserAllSplittin(int userId)
		{
			return new ReferralDao().GetUserAllSplittin(userId);
		}

		public static decimal GetUserUseSplittin(int userId)
		{
			return new ReferralDao().GetUserUseSplittin(userId);
		}

		public static decimal GetUserNoUseSplittin(int userId)
		{
			return new ReferralDao().GetUserNoUseSplittin(userId);
		}

		public static decimal GetLowerSaleTotalByUserId(int userId)
		{
			return new ReferralDao().GetLowerSaleTotalByUserId(userId);
		}

		public static int GetLowerNumByUserId(int userId)
		{
			return new ReferralDao().GetLowerNumByUserId(userId);
		}

		public static int GetLowerNumByUserIdNowMonth(int userId)
		{
			return new ReferralDao().GetLowerNumByUserIdNowMonth(userId);
		}

		public static DataSet GetUserOrder(int userId, OrderQuery query)
		{
			return new OrderDao().GetUserOrder(userId, query);
		}

		public static List<OrderInfo> GetListUserOrder(int userId, OrderQuery query)
		{
			return new OrderDao().GetListUserOrder(userId, query);
		}

		public static List<OrderInfo> GetPageListUserOrder(int userId, OrderQuery query)
		{
			return new OrderDao().GetPageListUserOrder(userId, query);
		}

		public static PageModel<AfterSaleRecordModel> GetUserAfterOrders(int userId, AfterSalesQuery query)
		{
			return new OrderDao().GetUserAfterOrders(userId, query);
		}

		public static int GetUserOrderCount(int userId, OrderQuery query)
		{
			return new OrderDao().GetUserOrderCount(userId, query);
		}

		public static int AddShippingAddress(ShippingAddressInfo shippingAddress)
		{
			ShippingAddressDao shippingAddressDao = new ShippingAddressDao();
			int num = (int)shippingAddressDao.Add(shippingAddress, null);
			if (num > 0 && shippingAddress.IsDefault)
			{
				new ShippingAddressDao().SetDefaultShippingAddress(num, shippingAddress.UserId);
			}
			return num;
		}

		public static int GetShippingAddressCount(int userId)
		{
			IList<ShippingAddressInfo> shippingAddresses = new ShippingAddressDao().GetShippingAddresses(userId, false);
			if (shippingAddresses == null)
			{
				return 0;
			}
			return shippingAddresses.Count;
		}

		public static IList<ShippingAddressInfo> GetShippingAddresses(bool forStoreSelect = false)
		{
			IList<ShippingAddressInfo> shippingAddresses = new ShippingAddressDao().GetShippingAddresses(HiContext.Current.UserId, forStoreSelect);
			foreach (ShippingAddressInfo item in shippingAddresses)
			{
				item.FullAddress = RegionHelper.GetFullRegion(item.RegionId, " ", true, 0) + " " + item.Address + " " + item.BuildingNumber;
			}
			return shippingAddresses;
		}

		public static ShippingAddressInfo GetDefaultShippingAddress()
		{
			IList<ShippingAddressInfo> shippingAddresses = new ShippingAddressDao().GetShippingAddresses(HiContext.Current.UserId, false);
			foreach (ShippingAddressInfo item in shippingAddresses)
			{
				if (item.IsDefault)
				{
					return item;
				}
			}
			return null;
		}

		public static ShippingAddressInfo GetShippingAddress(int shippingId)
		{
			return new ShippingAddressDao().Get<ShippingAddressInfo>(shippingId);
		}

		public static bool UpdateShippingAddress(ShippingAddressInfo shippingAddress)
		{
			bool flag = new ShippingAddressDao().Update(shippingAddress, null);
			if (flag && shippingAddress.IsDefault)
			{
				new ShippingAddressDao().SetDefaultShippingAddress(shippingAddress.ShippingId, shippingAddress.UserId);
			}
			return flag;
		}

		public static bool SetDefaultShippingAddress(int shippingId, int UserId)
		{
			return new ShippingAddressDao().SetDefaultShippingAddress(shippingId, UserId);
		}

		public static bool DelShippingAddress(int shippingid, int userid)
		{
			return new ShippingAddressDao().DelShippingAddress(shippingid, userid);
		}

		public static OpenIdSettingInfo GetOpenIdSettings(string openIdType)
		{
			return new OpenIdSettingDao().GetOpenIdSetting(openIdType);
		}

		public static IList<OpenIdSettingInfo> GetConfigedItems()
		{
			return new OpenIdSettingDao().Gets<OpenIdSettingInfo>("OpenIdType", SortAction.Asc, null);
		}

		public static DbQueryResult GetBalanceDetails(BalanceDetailQuery query)
		{
			return new BalanceDetailDao().GetBalanceDetails(query);
		}

		public static DateTime? GetUserBalanceLastActivityTime(int userId)
		{
			return new BalanceDetailDao().GetUserBalanceLastActivityTime(userId);
		}

		public static bool Recharge(BalanceDetailInfo balanceDetails)
		{
			InpourRequestDao inpourRequestDao = new InpourRequestDao();
			bool flag = inpourRequestDao.IsRecharge(balanceDetails.InpourId);
			if (!flag)
			{
				flag = (new BalanceDetailDao().Add(balanceDetails, null) > 0);
				inpourRequestDao.RemoveInpourRequest(balanceDetails.InpourId);
				if (flag)
				{
					SiteSettings masterSettings = SettingsManager.GetMasterSettings();
					if (masterSettings.IsOpenRechargeGift)
					{
						decimal rechargeGiftMoney = PromoteHelper.GetRechargeGiftMoney(balanceDetails.Income.ToDecimal(0));
						if (rechargeGiftMoney > decimal.Zero)
						{
							balanceDetails.TradeType = TradeTypes.RechargeGift;
							balanceDetails.Income = rechargeGiftMoney;
							balanceDetails.Balance += rechargeGiftMoney;
							balanceDetails.Remark = "充值赠送金额：" + rechargeGiftMoney.F2ToString("f2");
							new BalanceDetailDao().Add(balanceDetails, null);
						}
					}
				}
				MemberInfo user = Users.GetUser(balanceDetails.UserId);
				if (user != null)
				{
					Users.ClearUserCache(balanceDetails.UserId, user.SessionId);
				}
			}
			return flag;
		}

		public static bool IsRechargeSuccess(string inpourId)
		{
			InpourRequestDao inpourRequestDao = new InpourRequestDao();
			return inpourRequestDao.IsRecharge(inpourId);
		}

		public static bool AddSplittinDrawBalance(BalanceDetailInfo balanceDetails)
		{
			return new BalanceDetailDao().Add(balanceDetails, null) > 0;
		}

		public static bool AddInpourBlance(InpourRequestInfo inpourRequest)
		{
			return new InpourRequestDao().Add(inpourRequest, null) > 0;
		}

		public static InpourRequestInfo GetInpourBlance(string inpourId)
		{
			return new InpourRequestDao().GetInpourBlance(inpourId);
		}

		public static void RemoveInpourRequest(string inpourId)
		{
			new InpourRequestDao().RemoveInpourRequest(inpourId);
		}

		public static IDictionary<string, int> GetStatisticsNum()
		{
			return new MemberDao().GetStatisticsNum(HiContext.Current.UserId, (HiContext.Current.User == null) ? "" : HiContext.Current.User.UserName);
		}

		public static bool BalanceDrawRequest(BalanceDrawRequestInfo balanceDrawRequest)
		{
			Globals.EntityCoding(balanceDrawRequest, true);
			if (!balanceDrawRequest.IsWeixin.HasValue)
			{
				balanceDrawRequest.IsWeixin = false;
			}
			if (!balanceDrawRequest.IsAlipay.HasValue)
			{
				balanceDrawRequest.IsAlipay = false;
			}
			bool flag = new BalanceDetailDao().Add(balanceDrawRequest, null) > 0;
			if (flag)
			{
				Users.ClearUserCache(HiContext.Current.UserId, (HiContext.Current.User == null) ? "" : HiContext.Current.User.SessionId);
			}
			return flag;
		}

		public static bool HasInFilterIP(string filterIP, string userIP)
		{
			string[] array = filterIP.Split(',');
			foreach (string filterIP2 in array)
			{
				if (MemberProcessor.IsFilterIp(filterIP2, userIP))
				{
					return true;
				}
			}
			return false;
		}

		public static bool IsFilterIp(string filterIP, string userIP)
		{
			if (filterIP == userIP)
			{
				return true;
			}
			string text = "";
			string text2 = "";
			string[] array = filterIP.Split('-');
			if (array.Length > 1)
			{
				text = array[0];
				text2 = array[1];
			}
			string[] array2 = userIP.Split('.');
			if (text2 == "")
			{
				array = text.Split('.');
				if (array.Length != array2.Length)
				{
					return false;
				}
				for (int i = 0; i < array2.Length; i++)
				{
					if (array2[i] != array[i] && array[i] != "*")
					{
						return false;
					}
				}
				return true;
			}
			string[] array3 = text.Split('.');
			string[] array4 = text2.Split('.');
			if (array3.Length != array4.Length || array2.Length != array3.Length)
			{
				return false;
			}
			int num = 0;
			int num2 = 0;
			for (int j = 0; j < array3.Length; j++)
			{
				if (array3[j] != array4[j])
				{
					num = j;
					num2++;
					if (num2 >= 2)
					{
						return false;
					}
				}
			}
			for (int k = 0; k < array2.Length; k++)
			{
				if (array2[k] != array3[k] && num != k)
				{
					return false;
				}
			}
			int num3 = 0;
			int.TryParse(array3[num], out num3);
			int num4 = 0;
			int.TryParse(array4[num], out num4);
			if (num3 > num4)
			{
				return false;
			}
			int num5 = 0;
			if (int.TryParse(array2[num], out num5) && num5 >= num3 && num5 <= num4)
			{
				return true;
			}
			return false;
		}

		public static bool AddBalanceDetailInfo(InpourRequestInfo inpourRequest, string inpourType)
		{
			DateTime now = DateTime.Now;
			TradeTypes tradeType = TradeTypes.SelfhelpInpour;
			MemberInfo user = Users.GetUser(inpourRequest.UserId);
			decimal balance = user.Balance + inpourRequest.InpourBlance;
			BalanceDetailInfo balanceDetailInfo = new BalanceDetailInfo();
			balanceDetailInfo.UserId = inpourRequest.UserId;
			balanceDetailInfo.UserName = user.UserName;
			balanceDetailInfo.TradeDate = now;
			balanceDetailInfo.TradeType = tradeType;
			balanceDetailInfo.Income = inpourRequest.InpourBlance;
			balanceDetailInfo.Balance = balance;
			balanceDetailInfo.InpourId = inpourRequest.InpourId;
			balanceDetailInfo.Remark = "充值支付方式：" + inpourType;
			if (MemberProcessor.Recharge(balanceDetailInfo))
			{
				return true;
			}
			MemberProcessor.RemoveInpourRequest(inpourRequest.InpourId);
			return false;
		}

		public static bool AddRegReferralDeduct(int referralUserId, int userId, string userIp)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			bool writeLog = true;
			HttpBrowserCapabilities browser = HttpContext.Current.Request.Browser;
			string text = "正在使用   " + browser.Browser + "   v. " + browser.Version + ",你的运行平台是   " + browser.Platform;
			string text2 = "";
			bool flag = false;
			if (HttpContext.Current.Request.Cookies["Hishop-Referral"] == null)
			{
				text2 = Guid.NewGuid().ToString();
				flag = true;
				HttpCookie httpCookie = new HttpCookie("Hishop-Referral");
				httpCookie.HttpOnly = true;
				httpCookie.Value = Globals.UrlEncode(text2);
				httpCookie.Expires = DateTime.Now.AddDays(1.0);
				HttpContext.Current.Response.Cookies.Add(httpCookie);
			}
			else
			{
				text2 = HttpContext.Current.Request.Cookies["Hishop-Referral"].Value;
			}
			string text3 = "";
			if (userId > 0)
			{
				MemberInfo user = Users.GetUser(userId);
				if (user != null)
				{
					text3 = user.UserName;
				}
			}
			if (masterSettings.RegReferralDeduct <= decimal.Zero)
			{
				return false;
			}
			if (!flag)
			{
				MemberProcessor.WriteReferralLog(userId, text3, userIp, "注册奖励", masterSettings.RegReferralDeduct, "同一会话在指定时间内获取过奖励", writeLog);
				return false;
			}
			if (!new ReferralDao().CheckRegReferralStatus(userIp, 24m, text2))
			{
				MemberProcessor.WriteReferralLog(userId, text3, userIp, "注册奖励", masterSettings.RegReferralDeduct, "数据库中存在奖励记录", writeLog);
				return false;
			}
			SplittinDetailInfo splittinDetailInfo = new SplittinDetailInfo();
			ReferralDao referralDao = new ReferralDao();
			MemberInfo user2 = Users.GetUser(referralUserId);
			splittinDetailInfo.OrderId = "";
			splittinDetailInfo.UserIp = userIp;
			splittinDetailInfo.UserId = referralUserId;
			splittinDetailInfo.UserName = ((user2 == null) ? "" : user2.UserName.ToNullString());
			splittinDetailInfo.SubUserId = userId;
			splittinDetailInfo.IsUse = true;
			splittinDetailInfo.TradeDate = DateTime.Now;
			splittinDetailInfo.TradeType = SplittingTypes.RegReferralDeduct;
			splittinDetailInfo.Income = masterSettings.RegReferralDeduct;
			splittinDetailInfo.Balance = referralDao.GetUserUseSplittin(referralUserId) + masterSettings.RegReferralDeduct;
			splittinDetailInfo.Remark = "注册用户为：" + text3 + " 注册奖励：" + masterSettings.RegReferralDeduct;
			splittinDetailInfo.SessionID = text2;
			referralDao.Add(splittinDetailInfo, null);
			IList<int> list = new List<int>();
			list.Add(referralUserId);
			MemberProcessor.UpdateUserReferralGrade(list);
			MemberInfo user3 = Users.GetUser(referralUserId);
			Messenger.GetCommission(user3, text3, "", masterSettings.RegReferralDeduct, SplittingTypes.RegReferralDeduct, splittinDetailInfo.TradeDate);
			return true;
		}

		public static void WriteReferralLog(int userId, string username, string userIp, string referralType, decimal referralDebuct, string error, bool WriteLog)
		{
			if (WriteLog)
			{
				string userAgent = HttpContext.Current.Request.UserAgent;
				HttpBrowserCapabilities browser = HttpContext.Current.Request.Browser;
				IDictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("UserInfo", username + " / " + userId);
				dictionary.Add("IPAddress", userIp);
				dictionary.Add("奖励类型", referralType);
				dictionary.Add("金额", referralDebuct.ToString());
				dictionary.Add("UserAgent", userAgent);
				dictionary.Add("Browser", browser.Browser + "-" + browser.Version);
				dictionary.Add("Platform", browser.Platform);
				dictionary.Add("UrlReferrer", (HttpContext.Current.Request.UrlReferrer == (Uri)null) ? "" : HttpContext.Current.Request.UrlReferrer.ToString());
				dictionary.Add("RawUrl", HttpContext.Current.Request.RawUrl);
				Globals.AppendLog(dictionary, error, "", HttpContext.Current.Request.Url.ToString(), "ReferralLog");
			}
		}

		public static void LoadMemberExpandInfo(int gradeId, string userName, out string gradeName, out int messageNum)
		{
			new MemberDao().LoadMemberExpandInfo(gradeId, userName, out gradeName, out messageNum);
		}

		public static bool UpdateUserPhone(int userId, string phone)
		{
			return new MemberDao().UpdateUserPhone(userId, phone);
		}

		public static bool UpdateUserEmail(int userId, string email)
		{
			return new MemberDao().UpdateUserEmail(userId, email);
		}

		public static bool UpdateUserO2OStoreId(int userId, int storeId)
		{
			return new MemberDao().UpdateUserO2OStoreId(userId, storeId);
		}

		public static MemberInfo GetCellPhoneAndEmailByUser(int userId)
		{
			return new MemberDao().Get<MemberInfo>(userId);
		}

		public static int GetUserAfterSaleCount(int userId, bool isCountReplace = false, ProductType? productType = default(ProductType?))
		{
			return new OrderDao().GetUserAfterSaleCount(userId, isCountReplace, productType);
		}

		public static BalanceDetailInfo GetBalanceDetailInfoOfInpurId(string inpourId)
		{
			return new BalanceDetailDao().GetBalanceDetailInfoOfInpurId(inpourId);
		}

		public static MemberConsumeModel GetMemberConsumeList(Pagination page, int userId, bool isStatistics)
		{
			return new MemberDao().GetMemberConsumeList(page, userId, isStatistics);
		}

		public static PageModel<StoreMemberStatisticsModel> GetStoreMemberStatisticsList(StoreMemberStatisticsQuery query, int consumeTimesInOneMonth, int consumeTimesInThreeMonth, int consumeTimesInSixMonth)
		{
			return new MemberDao().GetStoreMemberStatisticsList(query, consumeTimesInOneMonth, consumeTimesInThreeMonth, consumeTimesInSixMonth);
		}

		public static bool GetIsRechargeGift()
		{
			return new BalanceDetailDao().GetIsRechargeGift();
		}

		public static decimal GetMemberPrice(ProductInfo product)
		{
			if (product == null)
			{
				return decimal.Zero;
			}
			int num = 0;
			if (HiContext.Current.User.UserId > 0)
			{
				num = HiContext.Current.User.GradeId;
			}
			decimal num2 = product.MinSalePrice;
			if (num > 0)
			{
				bool flag = false;
				foreach (string key in product.Skus.Keys)
				{
					if (product.Skus[key].MemberPrices.ContainsKey(num))
					{
						flag = true;
						if (num2 > product.Skus[key].MemberPrices[num])
						{
							num2 = product.Skus[key].MemberPrices[num];
						}
					}
				}
				if (!flag)
				{
					MemberGradeInfo memberGrade = MemberHelper.GetMemberGrade(num);
					if (memberGrade != null)
					{
						num2 = product.MinSalePrice * ((double)memberGrade.Discount / 100.0).ToDecimal(0);
					}
				}
			}
			return num2;
		}

		public static IList<UserInvoiceDataInfo> GetUserInvoiceDataList(int userId)
		{
			return new UserInvoiceDataDao().GetUserInvoiceDataList(userId);
		}

		public static IList<UserInvoiceDataInfo> GetUserInvoiceDataList(int userId, ShippingAddressInfo addressInfo)
		{
			if (addressInfo == null)
			{
				addressInfo = new ShippingAddressInfo();
			}
			IList<UserInvoiceDataInfo> list = MemberProcessor.GetUserInvoiceDataList(HiContext.Current.UserId);
			if (list == null)
			{
				list = new List<UserInvoiceDataInfo>();
			}
			foreach (UserInvoiceDataInfo item in list)
			{
				if (item.InvoiceType == InvoiceType.Personal || item.InvoiceType == InvoiceType.Enterprise)
				{
					if (string.IsNullOrEmpty(item.ReceiveEmail) && !string.IsNullOrEmpty(HiContext.Current.User.Email))
					{
						item.ReceiveEmail = HiContext.Current.User.Email;
					}
					if (string.IsNullOrEmpty(item.ReceivePhone) && !string.IsNullOrEmpty(addressInfo.CellPhone))
					{
						item.ReceivePhone = addressInfo.CellPhone;
					}
				}
				if (item.InvoiceType == InvoiceType.VATInvoice)
				{
					if (string.IsNullOrEmpty(item.ReceiveAddress) && !string.IsNullOrEmpty(addressInfo.Address))
					{
						item.ReceiveAddress = addressInfo.Address;
						item.ReceiveRegionId = addressInfo.RegionId;
						item.ReceiveRegionName = ((addressInfo.RegionId > 0) ? RegionHelper.GetFullRegion(addressInfo.RegionId, "", true, 0) : "");
					}
					if (string.IsNullOrEmpty(item.ReceiveName) && !string.IsNullOrEmpty(addressInfo.ShipTo))
					{
						item.ReceiveName = addressInfo.ShipTo;
					}
				}
			}
			if ((from i in list
			where i.InvoiceType == InvoiceType.Personal || i.InvoiceType == InvoiceType.Personal_Electronic
			select i).Count() <= 0)
			{
				list.Add(new UserInvoiceDataInfo
				{
					InvoiceType = InvoiceType.Personal_Electronic,
					InvoiceTitle = "个人",
					ReceiveEmail = HiContext.Current.User.Email,
					ReceivePhone = addressInfo.CellPhone,
					Id = 0,
					LastUseTime = DateTime.Now,
					UserId = HiContext.Current.UserId
				});
			}
			if ((from i in list
			where i.InvoiceType == InvoiceType.Enterprise || i.InvoiceType == InvoiceType.Enterprise_Electronic
			select i).Count() <= 0)
			{
				list.Add(new UserInvoiceDataInfo
				{
					InvoiceType = InvoiceType.Enterprise_Electronic,
					InvoiceTitle = "",
					Id = 0,
					ReceiveEmail = HiContext.Current.User.Email,
					ReceivePhone = addressInfo.CellPhone.ToNullString(),
					ReceiveAddress = addressInfo.Address.ToNullString(),
					ReceiveName = addressInfo.ShipTo.ToNullString(),
					ReceiveRegionId = addressInfo.RegionId,
					ReceiveRegionName = ((addressInfo.RegionId > 0) ? RegionHelper.GetFullRegion(addressInfo.RegionId, "", true, 0) : ""),
					UserId = HiContext.Current.UserId,
					LastUseTime = DateTime.Now
				});
			}
			if ((from i in list
			where i.InvoiceType == InvoiceType.VATInvoice
			select i).Count() <= 0)
			{
				list.Add(new UserInvoiceDataInfo
				{
					InvoiceType = InvoiceType.VATInvoice,
					InvoiceTitle = "",
					Id = 0,
					ReceiveEmail = HiContext.Current.User.Email,
					ReceivePhone = addressInfo.CellPhone.ToNullString(),
					ReceiveAddress = addressInfo.Address.ToNullString(),
					ReceiveName = addressInfo.ShipTo.ToNullString(),
					ReceiveRegionId = addressInfo.RegionId,
					ReceiveRegionName = ((addressInfo.RegionId > 0) ? RegionHelper.GetFullRegion(addressInfo.RegionId, "", true, 0) : ""),
					UserId = HiContext.Current.UserId,
					LastUseTime = DateTime.Now
				});
			}
			return list;
		}

		public static UserInvoiceDataInfo GetUserInvoiceDataInfo(int id)
		{
			return new UserInvoiceDataDao().Get<UserInvoiceDataInfo>(id);
		}

		public static UserInvoiceDataInfo GetUserInvoiceDataInfoByTitle(string invoiceTitle)
		{
			int userId = HiContext.Current.UserId;
			return new UserInvoiceDataDao().GetUserInvoiceDataInfoByTitle(invoiceTitle, userId);
		}

		public static bool DeleteUserInvoiceDataInfo(int id)
		{
			return new UserInvoiceDataDao().Delete<UserInvoiceDataInfo>(id);
		}

		public static bool UpdateUserInvoiceDataInfo(UserInvoiceDataInfo data)
		{
			return new UserInvoiceDataDao().Update(data, null);
		}

		public static long AddUserInvoiceDataInfo(UserInvoiceDataInfo data)
		{
			return new UserInvoiceDataDao().Add(data, null);
		}

		public static bool IsTrustLoginUser(MemberInfo member)
		{
			return member.IsQuickLogin || new MemberDao().IsTrustLoginUser(member.UserId);
		}

		public static decimal GetSplittinTotal(int userId)
		{
			return new ReferralDao().GetSplittinTotal(userId);
		}

		public static IList<ReferralGradeInfo> GetReferralGrades()
		{
			IList<ReferralGradeInfo> list = (IList<ReferralGradeInfo>)HiCache.Get("DataCache-ReferralGradeInfo");
			if (list == null)
			{
				list = new ReferralDao().GetReferralGrades();
				HiCache.Insert("DataCache-ReferralGradeInfo", list);
			}
			return list;
		}

		public static string GetReferralExtShowInfo(string extinfo)
		{
			ReferralExtInfo referralExtInfo = null;
			try
			{
				referralExtInfo = JsonHelper.ParseFormJson<ReferralExtInfo>(extinfo);
				string text = "";
				if (!string.IsNullOrEmpty(referralExtInfo.RealName))
				{
					text = text + "真实姓名：" + referralExtInfo.RealName;
				}
				if (!string.IsNullOrEmpty(referralExtInfo.CellPhone))
				{
					if (text != "")
					{
						text += "<br />";
					}
					text = text + "电话号码：" + referralExtInfo.CellPhone;
				}
				if (!string.IsNullOrEmpty(referralExtInfo.Email))
				{
					if (text != "")
					{
						text += "<br />";
					}
					text = text + "邮箱：" + referralExtInfo.Email;
				}
				if (!string.IsNullOrEmpty(referralExtInfo.Address))
				{
					if (text != "")
					{
						text += "<br />";
					}
					text = text + "地址：" + RegionHelper.GetFullRegion(referralExtInfo.RegionId, "", true, 0) + referralExtInfo.Address;
				}
				return text;
			}
			catch (Exception)
			{
				return extinfo;
			}
		}

		public static ReferralExtInfo GetReferralExtInfo(string extinfo)
		{
			ReferralExtInfo referralExtInfo = null;
			try
			{
				referralExtInfo = JsonHelper.ParseFormJson<ReferralExtInfo>(extinfo);
			}
			catch (Exception)
			{
				referralExtInfo = new ReferralExtInfo();
				if (!string.IsNullOrEmpty(extinfo))
				{
					referralExtInfo.RegionId = 0;
					string[] separator = new string[1]
					{
						"<br />"
					};
					string[] array = extinfo.Split(separator, StringSplitOptions.None);
					string[] array2 = array;
					foreach (string text in array2)
					{
						if (text.StartsWith("真实姓名：") && text.Length > 5)
						{
							referralExtInfo.RealName = text.Substring(5);
						}
						if (text.StartsWith("电话号码：") && text.Length > 5)
						{
							referralExtInfo.CellPhone = text.Substring(5);
						}
						if (text.StartsWith("邮箱：") && text.Length > 3)
						{
							referralExtInfo.Email = text.Substring(3);
						}
						if (text.StartsWith("地址：") && text.Length > 3)
						{
							referralExtInfo.Address = text.Substring(3);
						}
					}
				}
			}
			return referralExtInfo;
		}

		public static ReferralGradeInfo GetNextReferralGradeInfo(int currentGradeId)
		{
			return new ReferralDao().GetNextReferralGradeInfo(currentGradeId);
		}

		public static int GetRequestState(SplittinDrawInfo drawInfo)
		{
			if ((drawInfo.IsAlipay.HasValue && drawInfo.IsAlipay.Value) || (drawInfo.IsWeixin.HasValue && drawInfo.IsWeixin.Value))
			{
				return (drawInfo.AuditStatus == 0) ? drawInfo.RequestState.ToInt(0) : 4;
			}
			return (drawInfo.AuditStatus == 0) ? 1 : 4;
		}

		public static ReferralGradeInfo GetPreReferralGradeInfo(int currentGradeId)
		{
			return new ReferralDao().GetPreReferralGradeInfo(currentGradeId);
		}

		public static bool DeleteReferralGrade(int gradeId)
		{
			ReferralGradeInfo referralGradeInfo = new ReferralDao().Get<ReferralGradeInfo>(gradeId);
			if (referralGradeInfo == null)
			{
				return false;
			}
			if (new ReferralDao().Delete<ReferralGradeInfo>(gradeId))
			{
				EventLogs.WriteOperationLog(Privilege.DeleteMemberGrade, string.Format(CultureInfo.InvariantCulture, "删除了编号为 “{0}” 的分销员等级", new object[1]
				{
					referralGradeInfo.GradeId
				}), false);
				HiCache.Remove("DataCache-ReferralGradeInfo");
				return true;
			}
			return false;
		}

		public static ReferralGradeInfo GetReferralGradeInfo(int gradeId)
		{
			return new ReferralDao().Get<ReferralGradeInfo>(gradeId);
		}

		public static bool HasSameCommissionThresholdGrade(decimal commissionThreshold, int gradeId = 0)
		{
			return new ReferralDao().HasSameCommissionThresholdGrade(commissionThreshold, gradeId);
		}

		public static bool AddReferralGrade(ReferralGradeInfo gradeInfo)
		{
			if (new ReferralDao().Add(gradeInfo, null) > 0)
			{
				HiCache.Remove("DataCache-ReferralGradeInfo");
				EventLogs.WriteOperationLog(Privilege.DeleteMemberGrade, string.Format(CultureInfo.InvariantCulture, "添加了编号为 “{0}” 的分销员等级", new object[1]
				{
					gradeInfo.GradeId
				}), false);
				return true;
			}
			return false;
		}

		public static bool UpdateReferralGrade(ReferralGradeInfo gradeInfo)
		{
			if (new ReferralDao().Update(gradeInfo, null))
			{
				EventLogs.WriteOperationLog(Privilege.DeleteMemberGrade, string.Format(CultureInfo.InvariantCulture, "更新了编号为 “{0}” 的分销员等级", new object[1]
				{
					gradeInfo.GradeId
				}), false);
				HiCache.Remove("DataCache-ReferralGradeInfo");
				return true;
			}
			return false;
		}

		public static void UpdateUserReferralGrade(IList<int> userlist = null)
		{
			IList<ReferralGradeInfo> referralGrades = MemberProcessor.GetReferralGrades();
			new ReferralDao().UpdateUserReferralGrade(referralGrades, userlist);
			HiCache.Remove("DataCache-ReferralGradeInfo");
		}

		public static ReferralGradeInfo GetDefaultOrFirstReferralGradeInfo()
		{
			IList<ReferralGradeInfo> referralGrades = MemberProcessor.GetReferralGrades();
			if (referralGrades != null && referralGrades.Count() > 0)
			{
				return referralGrades[0];
			}
			return null;
		}

		public static decimal GetUserDrawSplittin()
		{
			return new ReferralDao().GetUserDrawSplittin(HiContext.Current.UserId);
		}

		public static string GetAppletReferralPostUrl(out decimal top, out decimal left, out decimal width)
		{
			top = default(decimal);
			left = default(decimal);
			width = default(decimal);
			string text = "";
			if (HiContext.Current.UserId == 0)
			{
				return "";
			}
			MemberInfo user = Users.GetUser(HiContext.Current.UserId);
			if (user != null && user.UserId > 0 && user.IsReferral())
			{
				string content = Globals.HostPath(HttpContext.Current.Request.Url) + "/WapShop/ReferralAgreement?ReferralUserId=" + HiContext.Current.User.UserId;
				string text2 = Globals.CreateQRCode(content, "/Storage/master/QRCode/" + HiContext.Current.SiteSettings.SiteUrl.Replace(".", "").Replace("https://", "").Replace("http://", "")
					.Replace("/", "")
					.Replace("\\", "") + "_Referral_" + HiContext.Current.User.UserId, false, ImageFormats.Png);
				string value = File.ReadAllText(HttpRuntime.AppDomainAppPath.ToString() + "Storage/data/Utility/ReferralPoster.js");
				FileInfo fileInfo = new FileInfo(HttpRuntime.AppDomainAppPath.ToString() + "Storage/data/Utility/ReferralPoster.js");
				DateTime lastWriteTime = fileInfo.LastWriteTime;
				FileInfo fileInfo2 = new FileInfo(HttpRuntime.AppDomainAppPath.ToString() + Globals.GetStoragePath() + "/ReferralPoster/Poster_Applet_" + user.UserId + ".jpg");
				JObject jObject = JsonConvert.DeserializeObject(value) as JObject;
				if (jObject != null && jObject["writeDate"] != null && jObject["posList"] != null && jObject["DefaultHead"] != null && jObject["myusername"] != null && jObject["shopname"] != null)
				{
					left = (decimal)jObject["posList"][3]["left"];
					top = (decimal)jObject["posList"][3]["top"];
					width = (decimal)jObject["posList"][3]["width"];
					if (fileInfo.Exists && fileInfo2.Exists && fileInfo2.LastWriteTime > lastWriteTime)
					{
						return Globals.GetStoragePath() + "/ReferralPoster/Poster_Applet_" + user.UserId + ".jpg";
					}
					string siteName = HiContext.Current.SiteSettings.SiteName;
					string text3 = user.Picture;
					string text4 = string.IsNullOrEmpty(user.NickName) ? user.RealName : user.NickName;
					int userId = user.UserId;
					int userId2 = user.UserId;
					if (jObject == null || jObject["BgImg"] == null)
					{
						return "";
					}
					Bitmap bitmap = null;
					if (text2.Contains("weixin.qq.com"))
					{
						bitmap = ResourcesHelper.GetNetImg(text2);
					}
					else
					{
						bitmap = (Bitmap)Image.FromFile(HttpContext.Current.Server.MapPath(text2));
					}
					int num = int.Parse(jObject["DefaultHead"].ToString());
					if (string.IsNullOrEmpty(text3) || (!text3.ToLower().StartsWith("http") && !text3.ToLower().StartsWith("https") && !File.Exists(Globals.MapPath(text3))))
					{
						text3 = "/Utility/pics/imgnopic.jpg";
					}
					if (num == 2)
					{
						text3 = "";
					}
					Image image = (!text3.ToLower().StartsWith("http") && !text3.ToLower().StartsWith("https")) ? ((string.IsNullOrEmpty(text3) || !File.Exists(Globals.MapPath(text3))) ? new Bitmap(100, 100) : Image.FromFile(Globals.MapPath(text3))) : ResourcesHelper.GetNetImg(text3);
					GraphicsPath graphicsPath = new GraphicsPath();
					graphicsPath.AddEllipse(new Rectangle(0, 0, image.Width, image.Width));
					Bitmap image2 = new Bitmap(image.Width, image.Width);
					using (Graphics graphics = Graphics.FromImage(image2))
					{
						graphics.SetClip(graphicsPath);
						graphics.DrawImage(image, 0, 0, image.Width, image.Width);
					}
					image.Dispose();
					Bitmap bitmap2 = new Bitmap(480, 735);
					Graphics graphics2 = Graphics.FromImage(bitmap2);
					graphics2.SmoothingMode = SmoothingMode.HighQuality;
					graphics2.CompositingQuality = CompositingQuality.HighQuality;
					graphics2.InterpolationMode = InterpolationMode.High;
					graphics2.Clear(Color.White);
					Bitmap image3 = new Bitmap(100, 100);
					if (jObject["BgImg"] != null && File.Exists(Globals.MapPath(jObject["BgImg"].ToString())))
					{
						image3 = (Bitmap)Image.FromFile(Globals.MapPath(jObject["BgImg"].ToString()));
						image3 = ResourcesHelper.GetThumbnail(image3, 735, 480);
					}
					graphics2.DrawImage(image3, 0, 0, 480, 735);
					Font font = new Font("微软雅黑", (float)(jObject["myusernameSize"].ToInt(0) * 6 / 5));
					Font font2 = new Font("微软雅黑", (float)(jObject["shopnameSize"].ToInt(0) * 6 / 5));
					graphics2.DrawImage(image2, (int)((decimal)jObject["posList"][0]["left"] * 480m), (int)jObject["posList"][0]["top"] * 735 / 490, (int)((decimal)jObject["posList"][0]["width"] * 480m), (int)((decimal)jObject["posList"][0]["width"] * 480m));
					StringFormat format = new StringFormat(StringFormatFlags.DisplayFormatControl);
					string text5 = jObject["myusername"].ToString().Replace("{{昵称}}", "$");
					string text6 = jObject["shopname"].ToString().Replace("{{商城名称}}", "$");
					string[] array = text5.Split('$');
					string[] array2 = text6.Split('$');
					graphics2.DrawString(array[0], font, new SolidBrush(ColorTranslator.FromHtml(jObject["myusernameColor"].ToString())), (float)(int)((decimal)jObject["posList"][1]["left"] * 480m), (float)((int)jObject["posList"][1]["top"] * 735 / 490), format);
					if (array.Length > 1)
					{
						SizeF sizeF = graphics2.MeasureString(" ", font);
						SizeF sizeF2 = graphics2.MeasureString(array[0], font);
						graphics2.DrawString(text4, font, new SolidBrush(ColorTranslator.FromHtml(jObject["nickNameColor"].ToString())), (float)(int)((decimal)jObject["posList"][1]["left"] * 480m) + sizeF2.Width - sizeF.Width, (float)((int)jObject["posList"][1]["top"] * 735 / 490), format);
						SizeF sizeF3 = graphics2.MeasureString(text4, font);
						graphics2.DrawString(array[1], font, new SolidBrush(ColorTranslator.FromHtml(jObject["myusernameColor"].ToString())), (float)(int)((decimal)jObject["posList"][1]["left"] * 480m) + sizeF2.Width - sizeF.Width * 2f + sizeF3.Width, (float)((int)jObject["posList"][1]["top"] * 735 / 490), format);
					}
					int num2 = 660 - (int)((decimal)jObject["posList"][2]["left"] * 480m);
					float num3 = 0f;
					int num4 = 0;
					int num5 = 0;
					for (int i = 0; i < array2[0].Length; i++)
					{
						if (i < array2[0].Length)
						{
							string text7 = array2[0].Substring(i, 1);
							SizeF sizeF4 = graphics2.MeasureString(text7, font2);
							num3 += sizeF4.Width;
							if (num3 > (float)num2 - sizeF4.Width && num3 <= (float)num2)
							{
								graphics2.DrawString(array2[0].Substring(num4, i - num4), font2, new SolidBrush(ColorTranslator.FromHtml(jObject["shopnameColor"].ToString())), (float)(int)((decimal)jObject["posList"][2]["left"] * 480m), (float)((int)jObject["posList"][2]["top"] * 735 / 490 + num5 * (int)sizeF4.Height));
								num4 = i;
								num5++;
								num3 = 0f;
							}
						}
					}
					if (num4 < array2[0].Length)
					{
						string text8 = array2[0].Substring(num4, 1);
						SizeF sizeF5 = graphics2.MeasureString(text8, font2);
						graphics2.DrawString(array2[0].Substring(num4, array2[0].Length - num4), font2, new SolidBrush(ColorTranslator.FromHtml(jObject["shopnameColor"].ToString())), (float)(int)((decimal)jObject["posList"][2]["left"] * 480m), (float)((int)jObject["posList"][2]["top"] * 735 / 490 + num5 * (int)sizeF5.Height));
					}
					if (array2.Length > 1)
					{
						SizeF sizeF6 = graphics2.MeasureString(" ", font2);
						SizeF sizeF7 = graphics2.MeasureString(array2[0], font2);
						graphics2.DrawString(siteName, font2, new SolidBrush(ColorTranslator.FromHtml(jObject["storeNameColor"].ToString())), (float)(int)((decimal)jObject["posList"][2]["left"] * 480m) + sizeF7.Width - sizeF6.Width, (float)((int)jObject["posList"][2]["top"] * 735 / 490), format);
						SizeF sizeF8 = graphics2.MeasureString(siteName, font2);
						graphics2.DrawString(array2[1], font2, new SolidBrush(ColorTranslator.FromHtml(jObject["shopnameColor"].ToString())), (float)(int)((decimal)jObject["posList"][2]["left"] * 480m) + sizeF7.Width - sizeF6.Width * 2f + sizeF8.Width, (float)((int)jObject["posList"][2]["top"] * 735 / 490), format);
					}
					left = (decimal)jObject["posList"][3]["left"];
					top = (decimal)jObject["posList"][3]["top"];
					width = (decimal)jObject["posList"][3]["width"];
					if (!Directory.Exists(Globals.MapPath(Globals.GetStoragePath() + "/ReferralPoster")))
					{
						Directory.CreateDirectory(Globals.MapPath(Globals.GetStoragePath() + "/ReferralPoster"));
					}
					bitmap2.Save(Globals.MapPath(Globals.GetStoragePath() + "/ReferralPoster/Poster_Applet_" + userId + ".jpg"), ImageFormat.Jpeg);
					Random random = new Random();
					text = Globals.GetStoragePath() + "/ReferralPoster/Poster_Applet_" + userId + ".jpg?rnd=" + random.Next();
					bitmap2.Dispose();
				}
				else
				{
					text = "";
				}
			}
			else
			{
				text = "";
			}
			return text;
		}
	}
}
