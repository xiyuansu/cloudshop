using System;

namespace HiShop.API.Setting.Exceptions
{
	public class MessageHandlerException : WeixinException
	{
		public MessageHandlerException(string message)
			: base(message, null)
		{
		}

		public MessageHandlerException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}
