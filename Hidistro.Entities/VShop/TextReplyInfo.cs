namespace Hidistro.Entities.VShop
{
	[TableName("vshop_Reply")]
	public class TextReplyInfo : ReplyInfo
	{
		public string Text
		{
			get;
			set;
		}
	}
}
