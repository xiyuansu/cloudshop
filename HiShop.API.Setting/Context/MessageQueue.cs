using HiShop.API.Setting.Entities;
using System.Collections.Generic;

namespace HiShop.API.Setting.Context
{
	public class MessageQueue<TM, TRequest, TResponse> : List<TM> where TM : class, IMessageContext<TRequest, TResponse>, new()where TRequest : IRequestMessageBase where TResponse : IResponseMessageBase
	{
	}
}
