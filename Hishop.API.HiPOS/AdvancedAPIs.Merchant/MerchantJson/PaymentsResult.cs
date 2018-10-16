using HiShop.API.Setting.Entities;

namespace HiShop.API.HiPOS.AdvancedAPIs.Merchant.MerchantJson
{
	public class PaymentsResult : HiShopJsonResult
	{
		public PaymentsResponse merchant_payments_response
		{
			get;
			set;
		}
	}
}
