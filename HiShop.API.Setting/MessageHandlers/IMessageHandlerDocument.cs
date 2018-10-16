using System.Xml.Linq;

namespace HiShop.API.Setting.MessageHandlers
{
	public interface IMessageHandlerDocument
	{
		XDocument RequestDocument
		{
			get;
			set;
		}

		XDocument ResponseDocument
		{
			get;
		}

		XDocument FinalResponseDocument
		{
			get;
		}

		string TextResponseMessage
		{
			get;
			set;
		}
	}
}
