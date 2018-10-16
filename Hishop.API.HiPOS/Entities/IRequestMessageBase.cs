using HiShop.API.Setting.Entities;

namespace HiShop.API.HiPOS.Entities
{
	public interface IRequestMessageBase : HiShop.API.Setting.Entities.IRequestMessageBase, IMessageBase
	{
		RequestMsgType MsgType
		{
			get;
		}

		string Encrypt
		{
			get;
			set;
		}

		new long MsgId
		{
			get;
			set;
		}
	}
}
