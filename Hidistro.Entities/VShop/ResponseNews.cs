using System.Collections.Generic;

namespace Hidistro.Entities.VShop
{
	public class ResponseNews : AbstractResponseMessage
	{
		public int ArticleCount
		{
			get
			{
				return (this.MessageInfo != null) ? this.MessageInfo.Count : 0;
			}
		}

		public IList<MessageInfo> MessageInfo
		{
			get;
			set;
		}

		public ResponseNews()
		{
			base.MsgType = "news";
		}
	}
}
