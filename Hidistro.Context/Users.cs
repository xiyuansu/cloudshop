using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using Hidistro.SqlDal.Members;
using Hidistro.SqlDal.Store;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;

namespace Hidistro.Context
{
	public static class Users
	{
		public static bool MemberEmailIsExist(string email)
		{
			MemberDao memberDao = new MemberDao();
			return memberDao.MemberEmailIsExist(email);
		}

		public static MemberInfo GetUser()
		{
			return Users.GetUser(Users.GetLoggedOnUserId());
		}

		public static MemberInfo GetUser(int userId)
		{
			string key = $"DataCache-MemberCacheKey-{userId}";
			MemberInfo memberInfo = HiCache.Get<MemberInfo>(key);
			if (memberInfo == null)
			{
				memberInfo = new MemberDao().Get<MemberInfo>(userId);
				if (memberInfo == null)
				{
					return null;
				}
				memberInfo.Referral = new ReferralDao().GetReferralInfo(userId);
				memberInfo.MemberOpenIds = new MemberOpenIdDao().GetMemberByOpenIds(userId);
				HiCache.Insert(key, memberInfo, 1800);
			}
			else
			{
				if (memberInfo.Referral == null)
				{
					memberInfo.Referral = new ReferralDao().GetReferralInfo(userId);
				}
				if (memberInfo.MemberOpenIds == null)
				{
					memberInfo.MemberOpenIds = new MemberOpenIdDao().GetMemberByOpenIds(userId);
				}
				HiCache.Insert(key, memberInfo, 1800);
			}
			return memberInfo;
		}

		public static void ClearAppUserCache(string sessionId)
		{
			if (!string.IsNullOrEmpty(sessionId))
			{
				HiCache.Remove($"DataCache-APPMemberCacheKey-{sessionId}");
			}
		}

		public static void ClearUserCache(int userId, string sessionId = "")
		{
			HiCache.Remove($"DataCache-MemberCacheKey-{userId}");
			Users.ClearAppUserCache(sessionId);
		}

		public static ManagerInfo GetManager(int userId)
		{
			string key = $"DataCache-ManagerCacheKey-{userId}";
			ManagerInfo managerInfo = HiCache.Get<ManagerInfo>(key);
			if (managerInfo == null)
			{
				managerInfo = new ManagerDao().Get<ManagerInfo>(userId);
				if (managerInfo != null)
				{
					HiCache.Insert(key, managerInfo, 3600);
				}
			}
			return managerInfo;
		}

		public static bool GetStoreState(int storeId)
		{
			return true;
		}

		private static int GetLoggedOnUserId()
		{
			HttpCookie httpCookie = HiContext.Current.Context.Request.Cookies["PC-Member"];
			if (httpCookie == null)
			{
				httpCookie = HiContext.Current.Context.Request.Cookies["Shop-Member"];
			}
			if (httpCookie != null)
			{
				try
				{
					return int.Parse(HiCryptographer.Decrypt(httpCookie.Value));
				}
				catch (Exception)
				{
					return 0;
				}
			}
			return 0;
		}

		public static void SetCurrentUser(int userId, int days, bool updateLoginStatus = true, bool isPCLogin = false)
		{
			HttpCookie httpCookie = new HttpCookie("Shop-Member");
			if (isPCLogin)
			{
				httpCookie = new HttpCookie("PC-Member");
			}
			httpCookie.Value = HiCryptographer.Encrypt(Globals.UrlEncode(userId.ToString()));
			int num = (days == 0) ? 30 : (days * 24 * 60);
			httpCookie.Expires = DateTime.Now.AddMinutes((double)num);
			HttpContext.Current.Response.Cookies.Add(httpCookie);
			if (updateLoginStatus)
			{
				Users.SetLoginStatus(userId, true);
			}
			Users.SetUserLogoutStatus(false);
		}

		public static void SetUserLogoutStatus(bool isLogout)
		{
			HttpCookie httpCookie = new HttpCookie("Shop-UserIsLogout");
			httpCookie.Value = isLogout.ToString().ToLower();
			httpCookie.Expires = DateTime.Now.AddYears(1);
			HttpContext.Current.Response.Cookies.Add(httpCookie);
		}

		public static bool UserIsLogout()
		{
			HttpCookie httpCookie = HiContext.Current.Context.Request.Cookies["Shop-UserIsLogout"];
			return httpCookie?.Value.ToBool() ?? false;
		}

		public static void ClearWxOpenId(int userId)
		{
			new MemberOpenIdDao().DeleteMemberOpenId(userId, "hishop.plugins.openid.weixin");
		}

		public static void SetLoginStatus(int userId, bool isLogined)
		{
			new MemberDao().SetLoginStatus(userId, isLogined);
		}

		public static string GenerateSalt()
		{
			byte[] array = new byte[16];
			new RNGCryptoServiceProvider().GetBytes(array);
			return Convert.ToBase64String(array);
		}

		public static string EncodePassword_Old(string cleanString, string salt)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(salt.ToLower() + cleanString);
			byte[] value = ((HashAlgorithm)CryptoConfig.CreateFromName("SHA1")).ComputeHash(bytes);
			return BitConverter.ToString(value);
		}

		public static string EncodePassword(string pass, string salt)
		{
			byte[] bytes = Encoding.Unicode.GetBytes(pass);
			byte[] array = Convert.FromBase64String(salt);
			byte[] array2 = new byte[array.Length + bytes.Length];
			byte[] array3 = null;
			Buffer.BlockCopy(array, 0, array2, 0, array.Length);
			Buffer.BlockCopy(bytes, 0, array2, array.Length, bytes.Length);
			HashAlgorithm hashAlgorithm = HashAlgorithm.Create(Membership.HashAlgorithmType);
			array3 = hashAlgorithm.ComputeHash(array2);
			return Convert.ToBase64String(array3);
		}

		public static ReferralInfo GetReferralInfo(int userId)
		{
			ReferralDao referralDao = new ReferralDao();
			return referralDao.Get<ReferralInfo>(userId);
		}
	}
}
