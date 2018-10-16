using HiShop.API.HiPOS.Entities;
using HiShop.API.HiPOS.Helpers;
using HiShop.API.Setting.Exceptions;
using SHiShop.API.HiPOS.Entities.Request;
using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace HiShop.API.HiPOS
{
	public static class RequestMessageFactory
	{
		public static IRequestMessageBase GetRequestEntity(XDocument doc, PostModel postModel = null)
		{
			RequestMessageBase requestMessageBase = null;
			try
			{
				RequestMsgType requestMsgType = MsgTypeHelper.GetRequestMsgType(doc);
				switch (requestMsgType)
				{
				case RequestMsgType.Text:
					requestMessageBase = new RequestMessageText();
					break;
				default:
					throw new UnknownRequestMsgTypeException($"MsgType：{requestMsgType} 在RequestMessageFactory中没有对应的处理程序！", new ArgumentOutOfRangeException());
				case RequestMsgType.Event:
					break;
				}
				requestMessageBase.FillEntityWithXml(doc);
			}
			catch (ArgumentException inner)
			{
				throw new WeixinException($"RequestMessage转换出错！可能是MsgType不存在！，XML：{doc.ToString()}", inner);
			}
			return requestMessageBase;
		}

		public static IRequestMessageBase GetRequestEntity(string xml)
		{
			return RequestMessageFactory.GetRequestEntity(XDocument.Parse(xml), null);
		}

		public static IRequestMessageBase GetRequestEntity(Stream stream)
		{
			using (XmlReader reader = XmlReader.Create(stream))
			{
				XDocument doc = XDocument.Load(reader);
				return RequestMessageFactory.GetRequestEntity(doc, null);
			}
		}

		public static XDocument GetRequestEntityDocument(Stream stream)
		{
			using (XmlReader reader = XmlReader.Create(stream))
			{
				return XDocument.Load(reader);
			}
		}
	}
}
