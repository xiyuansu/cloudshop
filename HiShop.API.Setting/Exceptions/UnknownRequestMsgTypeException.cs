using System;

namespace HiShop.API.Setting.Exceptions
{
	public class UnknownRequestMsgTypeException : WeixinException
	{
		public UnknownRequestMsgTypeException(string message)
			: base(message, null)
		{
		}

		public UnknownRequestMsgTypeException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}
