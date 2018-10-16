namespace Hishop.Weixin.MP.Request
{
	public abstract class EventRequest : AbstractRequest
	{
		public virtual RequestEventType Event
		{
			get;
			set;
		}

		public string EventKey
		{
			get;
			set;
		}
	}
}
