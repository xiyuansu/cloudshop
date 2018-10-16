using HiShop.API.Setting.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HiShop.API.Setting.Context
{
	public class WeixinContext<TM, TRequest, TResponse> where TM : class, IMessageContext<TRequest, TResponse>, new()where TRequest : IRequestMessageBase where TResponse : IResponseMessageBase
	{
		private int _maxRecordCount;

		public Dictionary<string, TM> MessageCollection
		{
			get;
			set;
		}

		public MessageQueue<TM, TRequest, TResponse> MessageQueue
		{
			get;
			set;
		}

		public double ExpireMinutes
		{
			get;
			set;
		}

		public int MaxRecordCount
		{
			get;
			set;
		}

		public WeixinContext()
		{
			this.Restore();
		}

		public void Restore()
		{
			this.MessageCollection = new Dictionary<string, TM>(StringComparer.OrdinalIgnoreCase);
			this.MessageQueue = new MessageQueue<TM, TRequest, TResponse>();
			this.ExpireMinutes = 90.0;
		}

		private TM GetMessageContext(string userName)
		{
			while (this.MessageQueue.Count > 0)
			{
				TM val = ((List<TM>)this.MessageQueue)[0];
				TimeSpan timeSpan = DateTime.Now - val.LastActiveTime;
				double num = val.ExpireMinutes.HasValue ? val.ExpireMinutes.Value : this.ExpireMinutes;
				if (!(timeSpan.TotalMinutes >= num))
				{
					break;
				}
				this.MessageQueue.RemoveAt(0);
				this.MessageCollection.Remove(val.UserName);
				val.OnRemoved();
			}
			if (!this.MessageCollection.ContainsKey(userName))
			{
				return null;
			}
			return this.MessageCollection[userName];
		}

		private TM GetMessageContext(string userName, bool createIfNotExists)
		{
			TM messageContext = this.GetMessageContext(userName);
			if (messageContext == null)
			{
				if (!createIfNotExists)
				{
					return null;
				}
				Dictionary<string, TM> messageCollection = this.MessageCollection;
				TM value = new TM();
				value.UserName = userName;
				value.MaxRecordCount = this.MaxRecordCount;
				messageCollection[userName] = value;
				messageContext = this.GetMessageContext(userName);
				this.MessageQueue.Add(messageContext);
			}
			return messageContext;
		}

		public TM GetMessageContext(TRequest requestMessage)
		{
			lock (WeixinContextGlobal.Lock)
			{
				return this.GetMessageContext(requestMessage.FromUserName, true);
			}
		}

		public TM GetMessageContext(TResponse responseMessage)
		{
			lock (WeixinContextGlobal.Lock)
			{
				return this.GetMessageContext(responseMessage.ToUserName, true);
			}
		}

		public void InsertMessage(TRequest requestMessage)
		{
			lock (WeixinContextGlobal.Lock)
			{
				string userName = requestMessage.FromUserName;
				TM messageContext = this.GetMessageContext(userName, true);
				if (messageContext.RequestMessages.Count > 0)
				{
					int num = this.MessageQueue.FindIndex((TM z) => z.UserName == userName);
					if (num >= 0)
					{
						this.MessageQueue.RemoveAt(num);
						this.MessageQueue.Add(messageContext);
					}
				}
				messageContext.LastActiveTime = DateTime.Now;
				messageContext.RequestMessages.Add(requestMessage);
			}
		}

		public void InsertMessage(TResponse responseMessage)
		{
			lock (WeixinContextGlobal.Lock)
			{
				TM messageContext = this.GetMessageContext(responseMessage.ToUserName, true);
				messageContext.ResponseMessages.Add(responseMessage);
			}
		}

		public TRequest GetLastRequestMessage(string userName)
		{
			lock (WeixinContextGlobal.Lock)
			{
				TM messageContext = this.GetMessageContext(userName, true);
				return Enumerable.LastOrDefault<TRequest>((IEnumerable<TRequest>)messageContext.RequestMessages);
			}
		}

		public TResponse GetLastResponseMessage(string userName)
		{
			lock (WeixinContextGlobal.Lock)
			{
				TM messageContext = this.GetMessageContext(userName, true);
				return Enumerable.LastOrDefault<TResponse>((IEnumerable<TResponse>)messageContext.ResponseMessages);
			}
		}
	}
}
