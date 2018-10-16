using Hishop.Alipay.OpenHome.Model;
using Hishop.Alipay.OpenHome.Utility;
using System;

namespace Hishop.Alipay.OpenHome.Request
{
	public class MessagePushRequest : IRequest
	{
		private Message message;

		public string GetMethodName()
		{
			return "alipay.mobile.public.message.push";
		}

		public MessagePushRequest(string appid, string toUserId, Articles articles, int articleCount, string agreementId = null, string msgType = "image-text")
		{
			Message message = this.message = new Message
			{
				AgreementId = agreementId,
				AppId = appid,
				Articles = articles,
				ArticleCount = articleCount,
				ToUserId = toUserId,
				CreateTime = TimeHelper.TransferToMilStartWith1970(DateTime.Now).ToString("F0"),
				MsgType = msgType
			};
		}

		public string GetBizContent()
		{
			return XmlSerialiseHelper.Serialise(this.message);
		}
	}
}
