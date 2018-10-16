using Hishop.Weixin.MP.Domain;

namespace Hishop.Weixin.MP.Response
{
	public class VideoResponse : AbstractResponse
	{
		public Video Video
		{
			get;
			set;
		}

		public override ResponseMsgType MsgType
		{
			get
			{
				return ResponseMsgType.Video;
			}
		}
	}
}
