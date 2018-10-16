using System;

namespace Hishop.Alipay.OpenHome.AlipayOHException
{
	public class AliResponseException : AlipayOpenHomeException
	{
		public string ResponseCode
		{
			get;
			set;
		}

		public AliResponseException()
		{
		}

		public AliResponseException(string code, string message, Exception innerException)
			: base(message, innerException)
		{
			this.ResponseCode = code;
		}

		public AliResponseException(string code, string message)
			: base(message)
		{
			this.ResponseCode = code;
		}
	}
}
