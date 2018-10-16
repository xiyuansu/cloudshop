using System.Runtime.InteropServices;

namespace Hishop.API.HiPOS
{
	public sealed class HiPOSParameter
	{
		[StructLayout(LayoutKind.Sequential, Size = 1)]
		public struct HttpMethod
		{
			public static readonly string GET = "GET";

			public static readonly string POST = "POST";

			public static readonly string PUT = "PUT";
		}

		public static readonly string GETAUTH = "http://openapi.huz.com.cn:10800/openapi/auth/hishop";

		public static readonly string GETTOKEN = "http://openapi.huz.com.cn:10800/openapi/token";

		public static readonly string UPDATEMERCHANTS = "http://openapi.huz.com.cn:10800/openapi/merchants/{0}";

		public static readonly string ALIPAYKEY = "http://openapi.huz.com.cn:10800/openapi/merchants/{0}/alipaykey";

		public static readonly string HISHOPO2O = "http://openapi.huz.com.cn:10800/openapi/merchants/{0}/hishopo2o";

		public static readonly string PAYMENTS = "http://openapi.huz.com.cn:10800/openapi/merchants/{0}/payments";

		public static readonly string AUTHCODE = "http://openapi.huz.com.cn:10800/openapi/merchants/{0}/hishop/authcode";

		public static readonly string HISHOPTRADES = "http://openapi.huz.com.cn:10800/openapi/merchants/{0}/hishop/trades";

		public static readonly string STOREDETAIL = "http://openapi.huz.com.cn:10800/openapi/merchants/{0}/hishop/trades/store/{1}/detail";
	}
}
