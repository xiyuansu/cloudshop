using Hishop.Weixin.MP.Request;
using Hishop.Weixin.MP.Request.Event;
using System;
using System.Xml.Linq;

namespace Hishop.Weixin.MP.Util
{
	public static class RequestMessageFactory
	{
		public static AbstractRequest GetRequestEntity(XDocument doc)
		{
			RequestMsgType msgType = MsgTypeHelper.GetMsgType(doc);
			AbstractRequest abstractRequest = null;
			switch (msgType)
			{
			case RequestMsgType.Text:
				abstractRequest = new TextRequest();
				break;
			case RequestMsgType.Video:
				abstractRequest = new VideoRequest();
				break;
			case RequestMsgType.Voice:
				abstractRequest = new VoiceRequest();
				break;
			case RequestMsgType.Location:
				abstractRequest = new LocationRequest();
				break;
			case RequestMsgType.Image:
				abstractRequest = new ImageRequest();
				break;
			case RequestMsgType.Link:
				abstractRequest = new LinkRequest();
				break;
			case RequestMsgType.Event:
				switch (EventTypeHelper.GetEventType(doc))
				{
				case RequestEventType.Subscribe:
					abstractRequest = new SubscribeEventRequest();
					break;
				case RequestEventType.UnSubscribe:
					abstractRequest = new UnSubscribeEventRequest();
					break;
				case RequestEventType.Scan:
					abstractRequest = new ScanEventRequest();
					break;
				case RequestEventType.Location:
					abstractRequest = new LocationEventRequest();
					break;
				case RequestEventType.Click:
					abstractRequest = new ClickEventRequest();
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			EntityHelper.FillEntityWithXml(abstractRequest, doc);
			return abstractRequest;
		}
	}
}
