using HiShop.API.Setting.Entities;

namespace HiShop.API.HiPOS.Entities
{
	public class AccessTokenResult : HiShopJsonResult
	{
		public string token_type
		{
			get;
			set;
		}

		public string access_token
		{
			get;
			set;
		}

		public int expires_in
		{
			get;
			set;
		}
	}
}
