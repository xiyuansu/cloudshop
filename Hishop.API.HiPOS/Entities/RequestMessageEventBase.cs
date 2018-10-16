using HiShop.API.Setting.Entities;

namespace HiShop.API.HiPOS.Entities
{
	public class RequestMessageEventBase : RequestMessageBase, IRequestMessageEventBase, IRequestMessageBase, HiShop.API.Setting.Entities.IRequestMessageBase, IMessageBase
	{
		public override RequestMsgType MsgType
		{
			get
			{
				return RequestMsgType.Event;
			}
		}

		public virtual Event Event
		{
			get
			{
				return Event.ENTER;
			}
		}
	}
}
