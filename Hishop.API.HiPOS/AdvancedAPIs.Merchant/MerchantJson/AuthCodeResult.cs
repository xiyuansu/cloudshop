using HiShop.API.Setting.Entities;

namespace HiShop.API.HiPOS.AdvancedAPIs.Merchant.MerchantJson
{
	public class AuthCodeResult : HiShopJsonResult
	{
		public AuthCodeResponse merchant_authcode_response
		{
			get;
			set;
		}
	}
}
