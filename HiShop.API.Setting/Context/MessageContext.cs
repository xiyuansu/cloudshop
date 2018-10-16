using HiShop.API.Setting.Entities;
using System;
using System.Threading;

namespace HiShop.API.Setting.Context
{
	public class MessageContext<TRequest, TResponse> : IMessageContext<TRequest, TResponse> where TRequest : IRequestMessageBase where TResponse : IResponseMessageBase
	{
		private int _maxRecordCount;

        public event EventHandler<WeixinContextRemovedEventArgs<TRequest, TResponse>> MessageContextRemoved;


        public string UserName
		{
			get;
			set;
		}

		public DateTime LastActiveTime
		{
			get;
			set;
		}

		public MessageContainer<TRequest> RequestMessages
		{
			get;
			set;
		}

		public MessageContainer<TResponse> ResponseMessages
		{
			get;
			set;
		}

		public int MaxRecordCount
		{
			get
			{
				return this._maxRecordCount;
			}
			set
			{
				this.RequestMessages.MaxRecordCount = value;
				this.ResponseMessages.MaxRecordCount = value;
				this._maxRecordCount = value;
			}
		}

		public object StorageData
		{
			get;
			set;
		}

		public double? ExpireMinutes
		{
			get;
			set;
		}

		public AppStoreState AppStoreState
		{
			get;
			set;
		}

		//public virtual event EventHandler<WeixinContextRemovedEventArgs<TRequest, TResponse>> MessageContextRemoved
		//{
		//	add
		//	{
		//		EventHandler<WeixinContextRemovedEventArgs<TRequest, TResponse>> eventHandler = this.MessageContextRemoved;
		//		EventHandler<WeixinContextRemovedEventArgs<TRequest, TResponse>> eventHandler2;
		//		do
		//		{
		//			eventHandler2 = eventHandler;
		//			EventHandler<WeixinContextRemovedEventArgs<TRequest, TResponse>> value2 = (EventHandler<WeixinContextRemovedEventArgs<TRequest, TResponse>>)Delegate.Combine(eventHandler2, value);
		//			eventHandler = Interlocked.CompareExchange<EventHandler<WeixinContextRemovedEventArgs<TRequest, TResponse>>>(ref this.MessageContextRemoved, value2, eventHandler2);
		//		}
		//		while ((object)eventHandler != eventHandler2);
		//	}
		//	remove
		//	{
		//		EventHandler<WeixinContextRemovedEventArgs<TRequest, TResponse>> eventHandler = this.MessageContextRemoved;
		//		EventHandler<WeixinContextRemovedEventArgs<TRequest, TResponse>> eventHandler2;
		//		do
		//		{
		//			eventHandler2 = eventHandler;
		//			EventHandler<WeixinContextRemovedEventArgs<TRequest, TResponse>> value2 = (EventHandler<WeixinContextRemovedEventArgs<TRequest, TResponse>>)Delegate.Remove(eventHandler2, value);
		//			eventHandler = Interlocked.CompareExchange<EventHandler<WeixinContextRemovedEventArgs<TRequest, TResponse>>>(ref this.MessageContextRemoved, value2, eventHandler2);
		//		}
		//		while ((object)eventHandler != eventHandler2);
		//	}
		//}

		private void OnMessageContextRemoved(WeixinContextRemovedEventArgs<TRequest, TResponse> e)
		{
            EventHandler<WeixinContextRemovedEventArgs<TRequest, TResponse>> messageContextRemoved = this.MessageContextRemoved;
            if (messageContextRemoved != null)
            {
                messageContextRemoved(this, e);
            }

        }

		public MessageContext()
		{
			this.MessageContextRemoved = null;
			 
			this.RequestMessages = new MessageContainer<TRequest>(this.MaxRecordCount);
			this.ResponseMessages = new MessageContainer<TResponse>(this.MaxRecordCount);
			this.LastActiveTime = DateTime.Now;
		}

		public virtual void OnRemoved()
		{
			WeixinContextRemovedEventArgs<TRequest, TResponse> e = new WeixinContextRemovedEventArgs<TRequest, TResponse>(this);
			this.OnMessageContextRemoved(e);
		}
	}
}
