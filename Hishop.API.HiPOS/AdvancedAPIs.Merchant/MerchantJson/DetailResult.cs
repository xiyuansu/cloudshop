using HiShop.API.Setting.Entities;

namespace HiShop.API.HiPOS.AdvancedAPIs.Merchant.MerchantJson
{
	public class DetailResult : HiShopJsonResult
	{
		public DetailResponse merchant_trades_detail_response
		{
			get;
			set;
		}
	}
}
