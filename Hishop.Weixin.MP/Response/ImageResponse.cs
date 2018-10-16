using Hishop.Weixin.MP.Domain;

namespace Hishop.Weixin.MP.Response
{
	public class ImageResponse : AbstractResponse
	{
		public Image Image
		{
			get;
			set;
		}

		public override ResponseMsgType MsgType
		{
			get
			{
				return ResponseMsgType.Image;
			}
		}
	}
}
