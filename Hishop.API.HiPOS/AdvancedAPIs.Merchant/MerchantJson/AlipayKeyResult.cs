using HiShop.API.Setting.Entities;

namespace HiShop.API.HiPOS.AdvancedAPIs.Merchant.MerchantJson
{
	public class AlipayKeyResult : HiShopJsonResult
	{
		public MerchantAlipayKeyResponse merchant_alipaykey_response
		{
			get;
			set;
		}
	}
}
