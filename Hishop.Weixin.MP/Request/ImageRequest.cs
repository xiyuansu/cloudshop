namespace Hishop.Weixin.MP.Request
{
	public class ImageRequest : AbstractRequest
	{
		public string PicUrl
		{
			get;
			set;
		}

		public int MediaId
		{
			get;
			set;
		}
	}
}
