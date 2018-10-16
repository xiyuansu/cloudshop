using HiShop.API.HiPOS.Entities;
using HiShop.API.Setting.Helpers;
using System.Reflection;
using System.Xml.Linq;

namespace HiShop.API.HiPOS.Helpers
{
	public static class EntityHelper
	{
		public static void FillEntityWithXml<T>(this T entity, XDocument doc) where T : class, new()
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
						propertyInfo.SetValue(entity, DateTimeHelper.GetDateTimeFromXml(root.Element(name).Value), null);
						break;
					case "Boolean":
						if (name == "FuncFlag")
						{
							propertyInfo.SetValue(entity, root.Element(name).Value == "1", null);
							break;
						}
						goto default;
					case "Int32":
						propertyInfo.SetValue(entity, int.Parse(root.Element(name).Value), null);
						break;
					case "Int64":
						propertyInfo.SetValue(entity, long.Parse(root.Element(name).Value), null);
						break;
					case "Double":
						propertyInfo.SetValue(entity, double.Parse(root.Element(name).Value), null);
						break;
					default:
						propertyInfo.SetValue(entity, root.Element(name).Value, null);
						break;
					case "RequestMsgType":
					case "ResponseMsgType":
					case "Event":
					case "List`1":
						break;
					}
				}
			}
		}

		public static T CreateResponseMessage<T>(this IRequestMessageBase requestMessage) where T : ResponseMessageBase
		{
			return ResponseMessageBase.CreateFromRequestMessage<T>(requestMessage);
		}

		public static IResponseMessageBase CreateResponseMessage(this string xml)
		{
			return ResponseMessageBase.CreateFromResponseXml(xml);
		}
	}
}
