using System;

namespace Hishop.Alipay.OpenHome.AlipayOHException
{
	public class RequestException : AlipayOpenHomeException
	{
		public RequestException()
		{
		}

		public RequestException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
