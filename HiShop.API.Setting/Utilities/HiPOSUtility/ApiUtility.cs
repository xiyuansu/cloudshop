namespace HiShop.API.Setting.Utilities.HiPOSUtility
{
	public static class ApiUtility
	{
		public static bool IsAppId(string accessTokenOrAppId)
		{
			return accessTokenOrAppId != null && accessTokenOrAppId.Length == 32;
		}
	}
}
