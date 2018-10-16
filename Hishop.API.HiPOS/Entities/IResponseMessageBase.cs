using HiShop.API.Setting.Entities;

namespace HiShop.API.HiPOS.Entities
{
	public interface IResponseMessageBase : HiShop.API.Setting.Entities.IResponseMessageBase, IMessageBase
	{
		ResponseMsgType MsgType
		{
			get;
		}
	}
}
