using HiShop.API.Setting.Entities;

namespace HiShop.API.HiPOS.AdvancedAPIs.Auth.AuthJson
{
	public class AuthResult : HiShopJsonResult
	{
		public HishopAuthResponse hishop_auth_response
		{
			get;
			set;
		}
	}
}
