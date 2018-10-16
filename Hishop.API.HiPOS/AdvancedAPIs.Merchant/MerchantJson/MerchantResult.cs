using HiShop.API.Setting.Entities;

namespace HiShop.API.HiPOS.AdvancedAPIs.Merchant.MerchantJson
{
	public class MerchantResult : HiShopJsonResult
	{
		public MerchantUpdateResponse merchant_update_response
		{
			get;
			set;
		}
	}
}
