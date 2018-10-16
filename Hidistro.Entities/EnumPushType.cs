using System.ComponentModel;

namespace Hidistro.Entities
{
	public enum EnumPushType
	{
		[Description("链接")]
		Link = 1,
		[Description("专题")]
		ProductTopic,
		[Description("活动")]
		Activity,
		[Description("商品")]
		Product
	}
}
