using HiShop.API.Setting.Entities;

namespace HiShop.API.HiPOS.Entities
{
	public interface IRequestMessageEventBase : IRequestMessageBase, HiShop.API.Setting.Entities.IRequestMessageBase, IMessageBase
	{
		Event Event
		{
			get;
		}
	}
}
