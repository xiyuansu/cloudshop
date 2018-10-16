using HiShop.API.HiPOS.Entities;
using HiShop.API.Setting.Entities;
using HiShop.API.Setting.Utilities.HiPOSUtility;
using System;

namespace Hishop.API.HiPOS.CommonAPIs
{
	public static class ApiHandlerWapper
	{
		private static readonly int MAXGETTOKENCOUNT = 3;

		private static int getTokenCount = 0;

		public static T TryCommonApi<T>(Func<string, T> fun, string accessTokenOrAppId = null, bool retryIfFaild = true) where T : HiShopJsonResult
		{
			string text = null;
			string text2 = null;
			if (accessTokenOrAppId == null)
			{
				text = AccessTokenContainer.GetFirstOrDefaultAppId();
				if (text == null)
				{
					throw new Exception("尚无已经注册的AppId，请先使用AccessTokenContainer.Register完成注册（全局执行一次即可）！");
				}
			}
			else if (ApiUtility.IsAppId(accessTokenOrAppId))
			{
				if (!AccessTokenContainer.CheckRegistered(accessTokenOrAppId))
				{
					throw new Exception("此appId尚未注册，请先使用AccessTokenContainer.Register完成注册（全局执行一次即可）！");
				}
				text = accessTokenOrAppId;
			}
			else
			{
				text2 = accessTokenOrAppId;
			}
			T val = null;
			try
			{
				if (text2 == null)
				{
					AccessTokenResult tokenResult = AccessTokenContainer.GetTokenResult(text, false);
					text2 = tokenResult.access_token;
				}
				return fun(text2);
			}
			catch (Exception ex)
			{
				if (!string.IsNullOrEmpty(ex.Message) && ApiHandlerWapper.getTokenCount < ApiHandlerWapper.MAXGETTOKENCOUNT)
				{
					ApiHandlerWapper.getTokenCount++;
					AccessTokenResult tokenResult2 = AccessTokenContainer.GetTokenResult(text, true);
					text2 = tokenResult2.access_token;
					return ApiHandlerWapper.TryCommonApi(fun, text, false);
				}
				throw;
			}
		}

		[Obsolete("请使用TryCommonApi()方法")]
		public static T Do<T>(Func<string, T> fun, string appId, string appSecret, bool retryIfFaild = true) where T : WxJsonResult
		{
			T val = null;
			try
			{
				string arg = AccessTokenContainer.TryGetToken(appId, appSecret, false);
				return fun(arg);
			}
			catch (Exception)
			{
				string text = AccessTokenContainer.TryGetToken(appId, appSecret, true);
				return ApiHandlerWapper.Do(fun, appId, appSecret, false);
			}
		}
	}
}
