namespace HiShop.API.Setting.Entities
{
	public abstract class RequestMessageBase : MessageBase, IRequestMessageBase, IMessageBase
	{
		public long MsgId
		{
			get;
			set;
		}

		public RequestMessageBase()
		{
		}
	}
}
