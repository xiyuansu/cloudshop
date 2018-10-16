using HiShop.API.Setting.Entities;

namespace HiShop.API.HiPOS.Entities
{
	public class RequestMessageBase : HiShop.API.Setting.Entities.RequestMessageBase, IRequestMessageBase, HiShop.API.Setting.Entities.IRequestMessageBase, IMessageBase
	{
		public virtual RequestMsgType MsgType
		{
			get
			{
				return RequestMsgType.Text;
			}
		}

		public string Encrypt
		{
			get;
			set;
		}

		public new long MsgId
		{
			get;
			set;
		}
	}
}
