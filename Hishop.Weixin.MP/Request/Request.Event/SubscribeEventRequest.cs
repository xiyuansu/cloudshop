namespace Hishop.Weixin.MP.Request.Event
{
	public class SubscribeEventRequest : EventRequest
	{
		public override RequestEventType Event
		{
			get
			{
				return RequestEventType.Subscribe;
			}
			set
			{
			}
		}
	}
}
