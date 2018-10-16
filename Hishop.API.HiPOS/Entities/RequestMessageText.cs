using HiShop.API.Setting.Entities;

namespace HiShop.API.HiPOS.Entities
{
	public class RequestMessageText : RequestMessageBase, IRequestMessageBase, HiShop.API.Setting.Entities.IRequestMessageBase, IMessageBase
	{
		public override RequestMsgType MsgType
		{
			get
			{
				return RequestMsgType.Text;
			}
		}

		public string Content
		{
			get;
			set;
		}
	}
}
