using HiShop.API.Setting.Entities;
using System;

namespace HiShop.API.Setting.Context
{
	public class WeixinContextRemovedEventArgs<TRequest, TResponse> : EventArgs where TRequest : IRequestMessageBase where TResponse : IResponseMessageBase
	{
		public string OpenId
		{
			get
			{
				return this.MessageContext.UserName;
			}
		}

		public DateTime LastActiveTime
		{
			get
			{
				return this.MessageContext.LastActiveTime;
			}
		}

		public IMessageContext<TRequest, TResponse> MessageContext
		{
			get;
			set;
		}

		public WeixinContextRemovedEventArgs(IMessageContext<TRequest, TResponse> messageContext)
		{
			this.MessageContext = messageContext;
		}
	}
}
