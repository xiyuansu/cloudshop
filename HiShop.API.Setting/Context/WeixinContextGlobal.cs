namespace HiShop.API.Setting.Context
{
	public static class WeixinContextGlobal
	{
		public static object Lock = new object();

		public static bool UseWeixinContext = true;
	}
}
