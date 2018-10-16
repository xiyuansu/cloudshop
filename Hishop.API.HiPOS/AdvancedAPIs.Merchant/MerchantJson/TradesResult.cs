using HiShop.API.Setting.Entities;

namespace HiShop.API.HiPOS.AdvancedAPIs.Merchant.MerchantJson
{
	public class TradesResult : HiShopJsonResult
	{
		public TradesResponse merchant_trades_response
		{
			get;
			set;
		}
	}
}
