namespace HiShop.API.Setting.Utilities.WeixinUtility
{
	public static class ApiUtility
	{
		public static bool IsAppId(string accessTokenOrAppId)
		{
			return accessTokenOrAppId != null && accessTokenOrAppId.Length <= 18;
		}
	}
}
