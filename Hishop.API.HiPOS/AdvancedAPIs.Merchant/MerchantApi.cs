using HiShop.API.HiPOS.AdvancedAPIs.Merchant.MerchantJson;
using Hishop.API.HiPOS.CommonAPIs;
using HiShop.API.Setting.HttpUtility;
using System.Collections.Generic;
using System.Text;

namespace Hishop.API.HiPOS.AdvancedAPIs.Merchant
{
	public class MerchantApi
	{
		public static MerchantResult UpdateMerchant(string accessTokenOrAppId, string merchantId, string name, string contact, string mobile, int timeOut = 10000)
		{
			return ApiHandlerWapper.TryCommonApi(delegate(string accessToken)
			{
				string url = string.Format(HiPOSParameter.UPDATEMERCHANTS, merchantId);
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("name", name);
				dictionary.Add("contact", contact);
				dictionary.Add("mobile", mobile);
				return Post.PostFileGetJson<MerchantResult>(url, null, null, dictionary, null, timeOut, null, null, accessToken, HiPOSParameter.HttpMethod.PUT);
			}, accessTokenOrAppId, true);
		}

		public static AlipayKeyResult GetAlipayKey(string accessTokenOrAppId, string merchantId, int timeOut = 10000)
		{
			return ApiHandlerWapper.TryCommonApi(delegate(string accessToken)
			{
				string url = string.Format(HiPOSParameter.ALIPAYKEY, merchantId);
				return Get.GetJson<AlipayKeyResult>(url, null, string.Empty, string.Empty, accessToken);
			}, accessTokenOrAppId, true);
		}

		public static HishopO2OResult SetHishopO2O(string accessTokenOrAppId, string merchantId, string status, string confirm, string authqr, int timeOut = 10000)
		{
			return ApiHandlerWapper.TryCommonApi(delegate(string accessToken)
			{
				string url = string.Format(HiPOSParameter.HISHOPO2O, merchantId);
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("status", status);
				dictionary.Add("confirm", confirm);
				dictionary.Add("authqr", authqr);
				return Post.PostFileGetJson<HishopO2OResult>(url, null, null, dictionary, null, timeOut, null, null, accessToken, HiPOSParameter.HttpMethod.PUT);
			}, accessTokenOrAppId, true);
		}

		public static PaymentsResult SetPayments(string accessTokenOrAppId, string merchantId, string aliAppId, string alPriKey, string wxAppId, string wxMchId, string wxPaySecret, string wxPayCert, int timeOut = 10000)
		{
			return ApiHandlerWapper.TryCommonApi(delegate(string accessToken)
			{
				string url = string.Format(HiPOSParameter.PAYMENTS, merchantId);
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("ali_app_id", aliAppId);
				dictionary.Add("ali_pri_key", alPriKey);
				dictionary.Add("wx_app_id", wxAppId);
				dictionary.Add("wx_mch_id", wxMchId);
				dictionary.Add("wx_pay_secret", wxPaySecret);
				dictionary.Add("wx_pay_cert", wxPayCert);
				return Post.PostFileGetJson<PaymentsResult>(url, null, null, dictionary, null, timeOut, null, null, accessToken, HiPOSParameter.HttpMethod.PUT);
			}, accessTokenOrAppId, true);
		}

		public static AuthCodeResult GetAuthCode(string accessTokenOrAppId, string merchantId, string storeName, int timeOut = 10000)
		{
			return ApiHandlerWapper.TryCommonApi(delegate(string accessToken)
			{
				string url = string.Format(HiPOSParameter.AUTHCODE, merchantId);
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("store_name", storeName);
				return Post.PostFileGetJson<AuthCodeResult>(url, null, null, dictionary, null, timeOut, null, null, accessToken, HiPOSParameter.HttpMethod.POST);
			}, accessTokenOrAppId, true);
		}

		public static TradesResult GetHishopTrades(string accessTokenOrAppId, string merchantId, string storeName, string from, string to, int page = 1, int page_size = 10, int timeOut = 10000)
		{
			return ApiHandlerWapper.TryCommonApi(delegate(string accessToken)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat(HiPOSParameter.HISHOPTRADES, merchantId);
				stringBuilder.AppendFormat("?store_name={0}&from={1}&to={2}&page={3}&page_size={4}", storeName, from, to, page, page_size);
				return Get.GetJson<TradesResult>(stringBuilder.ToString(), null, string.Empty, string.Empty, accessToken);
			}, accessTokenOrAppId, true);
		}

		public static DetailResult GetHishopTradesDetail(string accessTokenOrAppId, string merchantId, string storeId, string device_id, string code, bool hishop_only, string from, string to, int page = 1, int page_size = 10, int timeOut = 10000)
		{
			return ApiHandlerWapper.TryCommonApi(delegate(string accessToken)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat(HiPOSParameter.STOREDETAIL, merchantId, storeId);
				stringBuilder.AppendFormat("?device_id={0}&hishop_only={1}&from={2}&to={3}&page={4}&page_size={5}&code={6}", device_id, hishop_only.ToString().ToLower(), from, to, page, page_size, code);
				return Get.GetJson<DetailResult>(stringBuilder.ToString(), null, string.Empty, string.Empty, accessToken);
			}, accessTokenOrAppId, true);
		}
	}
}
