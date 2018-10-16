using Hidistro.Context;
using Hidistro.Core;
using HiShop.API.HiPOS.AdvancedAPIs.Auth;
using HiShop.API.HiPOS.AdvancedAPIs.Auth.AuthJson;
using Hishop.API.HiPOS.AdvancedAPIs.Merchant;
using HiShop.API.HiPOS.AdvancedAPIs.Merchant.MerchantJson;
using Hishop.API.HiPOS.CommonAPIs;
using System;
using System.IO;
using System.Text;

namespace Hidistro.SaleSystem.Store
{
	public class HiPOSHelper
	{
		private readonly int SECONDS = 300;

		private static SiteSettings siteSettings= SettingsManager.GetMasterSettings();

		public HiPOSHelper()
		{
            if (string.IsNullOrEmpty(siteSettings.HiPOSAppId))
            {
                string siteUrl = siteSettings.SiteUrl;
                string str2 = string.Empty;
                if (siteUrl.IndexOf("http") < 0)
                {
                    str2 = "http://" + siteSettings.SiteUrl + "/API/HiPOSAPI.ashx?action=auth";
                }
                else
                {
                    str2 = siteSettings.SiteUrl + "/API/HiPOSAPI.ashx?action=auth";
                }
                AuthResult auth = AuthApi.GetAuth(siteUrl, str2);
                if (auth.error != null)
                {
                    throw new Exception(auth.error.message);
                }
                siteSettings = SettingsManager.GetMasterSettings();
                AccessTokenContainer.Register(siteSettings.HiPOSAppId, siteSettings.HiPOSAppSecret);
            }
            else
            {
                AccessTokenContainer.Register(siteSettings.HiPOSAppId, siteSettings.HiPOSAppSecret);
            }
        }

		public AuthResult GetAuth(string hostname, string notify_url)
		{
			return AuthApi.GetAuth(hostname, notify_url);
		}

		public MerchantResult UpdateMerchant(string accessTokenOrAppId, string merchantId, string name, string contact, string mobile)
		{
			return MerchantApi.UpdateMerchant(accessTokenOrAppId, merchantId, name, contact, mobile, 10000);
		}

		public HishopO2OResult SetHishopO2O(string accessTokenOrAppId, string merchantId, string status, string confirm, string authqr)
		{
			return MerchantApi.SetHishopO2O(accessTokenOrAppId, merchantId, status, confirm, authqr, 10000);
		}

		public PaymentsResult SetPayments(string accessTokenOrAppId, string merchantId, string aliAppId, string wxAppId, string wxMchId, string wxPaySecret, string wxPayCert, string filename)
		{
			string text = string.Empty;
			using (FileStream fileStream = File.OpenRead(filename))
			{
				byte[] array = new byte[fileStream.Length];
				fileStream.Read(array, 0, array.Length);
				if (array[0] != 48)
				{
					string @string = Encoding.UTF8.GetString(array);
					text = Globals.UrlEncode(@string);
				}
			}
			if (string.IsNullOrEmpty(text))
			{
				throw new Exception("支付宝秘钥错误");
			}
			return MerchantApi.SetPayments(accessTokenOrAppId, merchantId, aliAppId, text, wxAppId, wxMchId, wxPaySecret, wxPayCert, 10000);
		}

		public AlipayKeyResult GetAlipayKey(string accessTokenOrAppId, string merchantId)
		{
			return MerchantApi.GetAlipayKey(accessTokenOrAppId, merchantId, 10000);
		}

		public AuthCodeResult GetAuthCode(string accessTokenOrAppId, string merchantId, string storeName)
		{
			return MerchantApi.GetAuthCode(accessTokenOrAppId, merchantId, storeName, 10000);
		}

		public TradesResult GetHishopTrades(string accessTokenOrAppId, string merchantId, string storeName, string from, string to, int page = 1, int page_size = 10)
		{
			return MerchantApi.GetHishopTrades(accessTokenOrAppId, merchantId, storeName, from, to, page, page_size, 10000);
		}

		public DetailResult GetHishopTradesDetail(string accessTokenOrAppId, string merchantId, string storeId = "", string device_id = "", string code = "", bool hishop_only = false, string from = "", string to = "", int page = 1, int page_size = 10)
		{
			return MerchantApi.GetHishopTradesDetail(accessTokenOrAppId, merchantId, storeId, device_id, code, hishop_only, from, to, page, page_size, 10000);
		}
	}
}
