using Hishop.Weixin.MP.Domain;

namespace Hishop.Weixin.MP.Response
{
	public class VoiceResponse : AbstractResponse
	{
		public Voice Voice
		{
			get;
			set;
		}

		public override ResponseMsgType MsgType
		{
			get
			{
				return ResponseMsgType.Voice;
			}
		}
	}
}
