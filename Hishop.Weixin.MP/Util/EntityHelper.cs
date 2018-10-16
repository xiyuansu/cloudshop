using Hishop.Weixin.MP.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace Hishop.Weixin.MP.Util
{
	public static class EntityHelper
	{
		public static void FillEntityWithXml<T>(T entity, XDocument doc) where T : AbstractRequest, new()
		{
			entity = (entity ?? new T());
			XElement root = doc.Root;
			PropertyInfo[] properties = entity.GetType().GetProperties();
			PropertyInfo[] array = properties;
			foreach (PropertyInfo propertyInfo in array)
			{
				string name = propertyInfo.Name;
				if (root.Element(name) != null)
				{
					switch (propertyInfo.PropertyType.Name)
					{
					case "DateTime":
						propertyInfo.SetValue(entity, new DateTime(long.Parse(root.Element(name).Value)), null);
						break;
					case "Boolean":
						if (name == "FuncFlag")
						{
							propertyInfo.SetValue(entity, root.Element(name).Value == "1", null);
							break;
						}
						goto default;
					case "Int64":
						propertyInfo.SetValue(entity, long.Parse(root.Element(name).Value), null);
						break;
					case "RequestEventType":
						propertyInfo.SetValue(entity, EventTypeHelper.GetEventType(root.Element(name).Value), null);
						break;
					case "RequestMsgType":
						propertyInfo.SetValue(entity, MsgTypeHelper.GetMsgType(root.Element(name).Value), null);
						break;
					default:
						propertyInfo.SetValue(entity, root.Element(name).Value, null);
						break;
					}
				}
			}
		}

		public static XDocument ConvertEntityToXml<T>(T entity) where T : class, new()
		{
			entity = (entity ?? new T());
			XDocument xDocument = new XDocument();
			xDocument.Add(new XElement("xml"));
			XElement root = xDocument.Root;
			List<string> list = new string[12]
			{
				"ToUserName",
				"FromUserName",
				"CreateTime",
				"MsgType",
				"Content",
				"ArticleCount",
				"Articles",
				"FuncFlag",
				"Title ",
				"Description ",
				"PicUrl",
				"Url"
			}.ToList();
			List<string> list2 = list;
			Func<string, int> orderByPropName = list2.IndexOf;
			List<PropertyInfo> list3 = (from p in entity.GetType().GetProperties()
			orderby orderByPropName(p.Name)
			select p).ToList();
			foreach (PropertyInfo item in list3)
			{
				string name = item.Name;
				if (name == "Articles")
				{
					XElement xElement = new XElement("Articles");
					List<Article> list4 = item.GetValue(entity, null) as List<Article>;
					foreach (Article item2 in list4)
					{
						IEnumerable<XElement> content = EntityHelper.ConvertEntityToXml(item2).Root.Elements();
						xElement.Add(new XElement("item", content));
					}
					root.Add(xElement);
				}
				else
				{
					switch (item.PropertyType.Name)
					{
					case "String":
						root.Add(new XElement(name, new XCData((item.GetValue(entity, null) as string) ?? "")));
						break;
					case "DateTime":
						root.Add(new XElement(name, ((DateTime)item.GetValue(entity, null)).Ticks));
						break;
					case "Boolean":
						if (name == "FuncFlag")
						{
							root.Add(new XElement(name, ((bool)item.GetValue(entity, null)) ? "1" : "0"));
							break;
						}
						goto default;
					case "ResponseMsgType":
						root.Add(new XElement(name, item.GetValue(entity, null).ToString().ToLower()));
						break;
					case "Article":
						root.Add(new XElement(name, item.GetValue(entity, null).ToString().ToLower()));
						break;
					default:
						root.Add(new XElement(name, item.GetValue(entity, null)));
						break;
					}
				}
			}
			return xDocument;
		}
	}
}
