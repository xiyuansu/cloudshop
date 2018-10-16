using HiShop.API.Setting.Entities;
using System;

namespace HiShop.API.Setting.Context
{
	public interface IMessageContext<TRequest, TResponse> where TRequest : IRequestMessageBase where TResponse : IResponseMessageBase
	{
		string UserName
		{
			get;
			set;
		}

		DateTime LastActiveTime
		{
			get;
			set;
		}

		MessageContainer<TRequest> RequestMessages
		{
			get;
			set;
		}

		MessageContainer<TResponse> ResponseMessages
		{
			get;
			set;
		}

		int MaxRecordCount
		{
			get;
			set;
		}

		object StorageData
		{
			get;
			set;
		}

		double? ExpireMinutes
		{
			get;
			set;
		}

		AppStoreState AppStoreState
		{
			get;
			set;
		}

		event EventHandler<WeixinContextRemovedEventArgs<TRequest, TResponse>> MessageContextRemoved;

		void OnRemoved();
	}
}
