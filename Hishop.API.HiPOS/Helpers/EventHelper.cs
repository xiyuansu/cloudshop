using System;
using System.Xml.Linq;

namespace HiShop.API.HiPOS.Helpers
{
	public class EventHelper
	{
		public static Event GetEventType(XDocument doc)
		{
			return EventHelper.GetEventType(doc.Root.Element("Event").Value);
		}

		public static Event GetEventType(string str)
		{
			return (Event)Enum.Parse(typeof(Event), str, true);
		}
	}
}
