namespace Hishop.Weixin.MP.Request.Event
{
	public class ClickEventRequest : EventRequest
	{
		public new string EventKey
		{
			get;
			set;
		}

		public override RequestEventType Event
		{
			get
			{
				return RequestEventType.Click;
			}
			set
			{
			}
		}
	}
}
