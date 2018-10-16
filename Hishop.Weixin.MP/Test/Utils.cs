using Hishop.Weixin.MP.Request;
using Hishop.Weixin.MP.Response;
using Hishop.Weixin.MP.Util;
using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace Hishop.Weixin.MP.Test
{
	internal class Utils
	{
		private const string xml = "<xml><ToUserName><![CDATA[gh_ef4e2090afe3]]></ToUserName><FromUserName><![CDATA[opUMDj9jbOmTtbZuE2hM6wnv27B0]]></FromUserName><CreateTime>1385887183</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[s]]></Content><MsgId>5952340126940580233</MsgId></xml>";

		public void Test04()
		{
			A a = new A("<xml><ToUserName><![CDATA[gh_ef4e2090afe3]]></ToUserName><FromUserName><![CDATA[opUMDj9jbOmTtbZuE2hM6wnv27B0]]></FromUserName><CreateTime>1385887183</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[s]]></Content><MsgId>5952340126940580233</MsgId></xml>");
			int num = 0;
			object requestDocument = a.RequestDocument;
		}

		public string MethodName()
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml("<xml><ToUserName><![CDATA[gh_ef4e2090afe3]]></ToUserName><FromUserName><![CDATA[opUMDj9jbOmTtbZuE2hM6wnv27B0]]></FromUserName><CreateTime>1385887183</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[s]]></Content><MsgId>5952340126940580233</MsgId></xml>");
			return xmlDocument.SelectSingleNode("xml/ToUserName").InnerText;
		}

		public AbstractRequest ConvertRequest<T>(Stream inputStream) where T : AbstractRequest
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(inputStream);
			string a = xmlDocument.SelectSingleNode("xml/MsgType").InnerText.ToLower();
			if (a != "text")
			{
				return null;
			}
			TextRequest textRequest = new TextRequest();
			textRequest.Content = xmlDocument.SelectSingleNode("xml/Content").InnerText;
			textRequest.FromUserName = xmlDocument.SelectSingleNode("xml/FromUserName").InnerText;
			textRequest.MsgId = Convert.ToInt32(xmlDocument.SelectSingleNode("xml/FromUserName").InnerText);
			return textRequest;
		}

		public void Test02()
		{
			XDocument doc = XDocument.Parse("<xml><ToUserName><![CDATA[gh_ef4e2090afe3]]></ToUserName><FromUserName><![CDATA[opUMDj9jbOmTtbZuE2hM6wnv27B0]]></FromUserName><CreateTime>1385887183</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[s]]></Content><MsgId>5952340126940580233</MsgId></xml>");
			TextRequest entity = new TextRequest();
			EntityHelper.FillEntityWithXml(entity, doc);
		}

		public string Test03()
		{
			TextResponse textResponse = new TextResponse();
			textResponse.Content = "hah";
			textResponse.FromUserName = "123";
			textResponse.ToUserName = "456";
			XDocument xDocument = EntityHelper.ConvertEntityToXml(textResponse);
			return xDocument.ToString();
		}
	}
}
