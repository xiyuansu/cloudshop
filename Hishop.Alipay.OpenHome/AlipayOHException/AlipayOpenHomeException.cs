using System;

namespace Hishop.Alipay.OpenHome.AlipayOHException
{
	public class AlipayOpenHomeException : ApplicationException
	{
		public AlipayOpenHomeException()
		{
		}

		public AlipayOpenHomeException(string message)
			: base(message)
		{
		}

		public AlipayOpenHomeException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
