using System;

namespace Hishop.Alipay.OpenHome.AlipayOHException
{
	public class SignatureException : AlipayOpenHomeException
	{
		public SignatureException()
		{
		}

		public SignatureException(string message)
			: base(message)
		{
		}

		public SignatureException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
