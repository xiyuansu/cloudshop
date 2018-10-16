using System.Collections.Generic;

namespace Hidistro.Entities.VShop
{
	[TableName("vshop_Reply")]
	public class NewsReplyInfo : ReplyInfo
	{
		public IList<NewsMsgInfo> NewsMsg
		{
			get;
			set;
		}

		public NewsReplyInfo()
		{
			base.MessageType = MessageType.List;
		}
	}
}
