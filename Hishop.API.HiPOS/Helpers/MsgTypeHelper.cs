using System;
using System.Xml.Linq;

namespace HiShop.API.HiPOS.Helpers
{
	public static class MsgTypeHelper
	{
		public static RequestMsgType GetRequestMsgType(XDocument doc)
		{
			return MsgTypeHelper.GetRequestMsgType(doc.Root.Element("MsgType").Value);
		}

		public static RequestMsgType GetRequestMsgType(string str)
		{
			return (RequestMsgType)Enum.Parse(typeof(RequestMsgType), str, true);
		}

		public static ResponseMsgType GetResponseMsgType(XDocument doc)
		{
			return MsgTypeHelper.GetResponseMsgType(doc.Root.Element("MsgType").Value);
		}

		public static ResponseMsgType GetResponseMsgType(string str)
		{
			return (ResponseMsgType)Enum.Parse(typeof(ResponseMsgType), str, true);
		}
	}
}
