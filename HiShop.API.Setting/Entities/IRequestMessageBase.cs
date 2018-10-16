namespace HiShop.API.Setting.Entities
{
	public interface IRequestMessageBase : IMessageBase
	{
		long MsgId
		{
			get;
			set;
		}
	}
}
