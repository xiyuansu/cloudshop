using HiShop.API.Setting.Entities;

namespace HiShop.API.Setting.MessageHandlers
{
	public interface IMessageHandler<TRequest, TResponse> : IMessageHandlerDocument where TRequest : IRequestMessageBase where TResponse : IResponseMessageBase
	{
		string WeixinOpenId
		{
			get;
		}

		bool CancelExcute
		{
			get;
			set;
		}

		TRequest RequestMessage
		{
			get;
			set;
		}

		TResponse ResponseMessage
		{
			get;
			set;
		}

		bool UsedMessageAgent
		{
			get;
			set;
		}

		bool OmitRepeatedMessage
		{
			get;
			set;
		}

		void Execute();
	}
}
