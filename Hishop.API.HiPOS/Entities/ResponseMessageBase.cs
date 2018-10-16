using HiShop.API.HiPOS.Helpers;
using HiShop.API.Setting.Entities;
using HiShop.API.Setting.Exceptions;
using System;
using System.Xml.Linq;

namespace HiShop.API.HiPOS.Entities
{
	public class ResponseMessageBase : HiShop.API.Setting.Entities.ResponseMessageBase, IResponseMessageBase, HiShop.API.Setting.Entities.IResponseMessageBase, IMessageBase
	{
		public virtual ResponseMsgType MsgType
		{
			get
			{
				return ResponseMsgType.Text;
			}
		}

		public static T CreateFromRequestMessage<T>(IRequestMessageBase requestMessage) where T : ResponseMessageBase
		{
			try
			{
				Type typeFromHandle = typeof(T);
				string value = typeFromHandle.Name.Replace("ResponseMessage", "");
				ResponseMsgType msgType = (ResponseMsgType)Enum.Parse(typeof(ResponseMsgType), value);
				return (T)(ResponseMessageBase.CreateFromRequestMessage(requestMessage, msgType) as T);
			}
			catch (Exception inner)
			{
				throw new WeixinException("ResponseMessageBase.CreateFromRequestMessage<T>过程发生异常！", inner);
			}
		}

		[Obsolete("建议使用CreateFromRequestMessage<T>(IRequestMessageBase requestMessage)取代此方法")]
		public static ResponseMessageBase CreateFromRequestMessage(IRequestMessageBase requestMessage, ResponseMsgType msgType)
		{
			ResponseMessageBase responseMessageBase = null;
			try
			{
				throw new UnknownRequestMsgTypeException($"ResponseMsgType没有为 {msgType} 提供对应处理程序。", new ArgumentOutOfRangeException());
			}
			catch (Exception inner)
			{
				throw new WeixinException("CreateFromRequestMessage过程发生异常", inner);
			}
		}

		public static IResponseMessageBase CreateFromResponseXml(string xml)
		{
			try
			{
				if (string.IsNullOrEmpty(xml))
				{
					return null;
				}
				XDocument xDocument = XDocument.Parse(xml);
				ResponseMessageBase responseMessageBase = null;
				ResponseMsgType responseMsgType = (ResponseMsgType)Enum.Parse(typeof(ResponseMsgType), xDocument.Root.Element("MsgType").Value, true);
				ResponseMsgType responseMsgType2 = responseMsgType;
				responseMessageBase.FillEntityWithXml(xDocument);
				return responseMessageBase;
			}
			catch (Exception ex)
			{
				throw new WeixinException("ResponseMessageBase.CreateFromResponseXml<T>过程发生异常！" + ex.Message, ex);
			}
		}
	}
}
