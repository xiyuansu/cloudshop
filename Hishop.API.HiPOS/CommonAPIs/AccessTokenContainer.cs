using HiShop.API.HiPOS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hishop.API.HiPOS.CommonAPIs
{
	public class AccessTokenContainer
	{
		private static Dictionary<string, AccessTokenBag> AccessTokenCollection = new Dictionary<string, AccessTokenBag>(StringComparer.OrdinalIgnoreCase);

		public static void Register(string appId, string appSecret)
		{
			AccessTokenContainer.AccessTokenCollection[appId] = new AccessTokenBag
			{
				AppId = appId,
				AppSecret = appSecret,
				ExpireTime = DateTime.Now
			};
		}

		public static string TryGetToken(string appId, string appSecret, bool getNewToken = false)
		{
			if (!AccessTokenContainer.CheckRegistered(appId) | getNewToken)
			{
				AccessTokenContainer.Register(appId, appSecret);
			}
			return AccessTokenContainer.GetToken(appId, false);
		}

		public static string GetToken(string appId, bool getNewToken = false)
		{
			return AccessTokenContainer.GetTokenResult(appId, getNewToken).access_token;
		}

		public static AccessTokenResult GetTokenResult(string appId, bool getNewToken = false)
		{
			if (!AccessTokenContainer.AccessTokenCollection.ContainsKey(appId))
			{
				throw new Exception("此appId尚未注册，请先使用AccessTokenContainer.Register完成注册（全局执行一次即可）！");
			}
			AccessTokenBag accessTokenBag = AccessTokenContainer.AccessTokenCollection[appId];
			lock (accessTokenBag.Lock)
			{
				if (getNewToken || accessTokenBag.ExpireTime <= DateTime.Now)
				{
					accessTokenBag.AccessTokenResult = CommonApi.GetToken(accessTokenBag.AppId, accessTokenBag.AppSecret, "client_credentials");
					accessTokenBag.ExpireTime = DateTime.Now.AddSeconds((double)accessTokenBag.AccessTokenResult.expires_in);
				}
			}
			return accessTokenBag.AccessTokenResult;
		}

		public static bool CheckRegistered(string appId)
		{
			return AccessTokenContainer.AccessTokenCollection.ContainsKey(appId);
		}

		public static string GetFirstOrDefaultAppId()
		{
			return AccessTokenContainer.AccessTokenCollection.Keys.FirstOrDefault();
		}
	}
}
