using HiShop.API.Setting.Context;
using HiShop.API.Setting.Entities;
using HiShop.API.Setting.XmlUtility;
using System;
using System.IO;
using System.Xml.Linq;

namespace HiShop.API.Setting.MessageHandlers
{
	public abstract class MessageHandler<TC, TRequest, TResponse> : IMessageHandler<TRequest, TResponse>, IMessageHandlerDocument where TC : class, IMessageContext<TRequest, TResponse>, new()where TRequest : IRequestMessageBase where TResponse : IResponseMessageBase
	{
		private string _textResponseMessage = null;

		public abstract WeixinContext<TC, TRequest, TResponse> WeixinContext
		{
			get;
		}

		public virtual TC CurrentMessageContext
		{
			get
			{
				return this.WeixinContext.GetMessageContext(this.RequestMessage);
			}
		}

		public string WeixinOpenId
		{
			get
			{
				if (this.RequestMessage != null)
				{
					return this.RequestMessage.FromUserName;
				}
				return null;
			}
		}

		[Obsolete("UserName属性从v0.6起已过期，建议使用WeixinOpenId")]
		public string UserName
		{
			get
			{
				return this.WeixinOpenId;
			}
		}

		public bool CancelExcute
		{
			get;
			set;
		}

		public XDocument RequestDocument
		{
			get;
			set;
		}

		public abstract XDocument ResponseDocument
		{
			get;
		}

		public abstract XDocument FinalResponseDocument
		{
			get;
		}

		public virtual TRequest RequestMessage
		{
			get;
			set;
		}

		public virtual TResponse ResponseMessage
		{
			get;
			set;
		}

		public bool UsedMessageAgent
		{
			get;
			set;
		}

		public bool OmitRepeatedMessage
		{
			get;
			set;
		}

		public string TextResponseMessage
		{
			get
			{
				if (this._textResponseMessage == null)
				{
					return (this.ResponseDocument == null) ? null : this.ResponseDocument.ToString();
				}
				return this._textResponseMessage;
			}
			set
			{
				this._textResponseMessage = value;
			}
		}

		public void CommonInitialize(XDocument postDataDocument, int maxRecordCount, object postData)
		{
			this.WeixinContext.MaxRecordCount = maxRecordCount;
			this.RequestDocument = this.Init(postDataDocument, postData);
		}

		public MessageHandler(Stream inputStream, int maxRecordCount = 0, object postData = null)
		{
			XDocument postDataDocument = HiShop.API.Setting.XmlUtility.XmlUtility.Convert(inputStream);
			this.CommonInitialize(postDataDocument, maxRecordCount, postData);
		}

		public MessageHandler(XDocument postDataDocument, int maxRecordCount = 0, object postData = null)
		{
			this.CommonInitialize(postDataDocument, maxRecordCount, postData);
		}

		public MessageHandler(RequestMessageBase requestMessageBase, int maxRecordCount = 0, object postData = null)
		{
		}

		public abstract XDocument Init(XDocument requestDocument, object postData = null);

		public abstract void Execute();

		public virtual void OnExecuting()
		{
		}

		public virtual void OnExecuted()
		{
		}
	}
}
