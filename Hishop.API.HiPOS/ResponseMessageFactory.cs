using HiShop.API.HiPOS.Entities;
using HiShop.API.HiPOS.Helpers;
using HiShop.API.Setting.Exceptions;
using System;
using System.Xml.Linq;

namespace HiShop.API.HiPOS
{
	public static class ResponseMessageFactory
	{
		public static IResponseMessageBase GetResponseEntity(XDocument doc)
		{
			ResponseMessageBase responseMessageBase = null;
			try
			{
				ResponseMsgType responseMsgType = MsgTypeHelper.GetResponseMsgType(doc);
				ResponseMsgType responseMsgType2 = responseMsgType;
				throw new UnknownRequestMsgTypeException($"MsgType：{responseMsgType} 在ResponseMessageFactory中没有对应的处理程序！", new ArgumentOutOfRangeException());
			}
			catch (ArgumentException inner)
			{
				throw new WeixinException($"ResponseMessage转换出错！可能是MsgType不存在！，XML：{doc.ToString()}", inner);
			}
		}

		public static IResponseMessageBase GetResponseEntity(string xml)
		{
			return ResponseMessageFactory.GetResponseEntity(XDocument.Parse(xml));
		}
	}
}
