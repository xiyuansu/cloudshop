using System.ComponentModel;

namespace Hidistro.Entities.VShop
{
	public enum EnumTopicType
	{
		[Description("微信")]
		WapTopic = 1,
		[Description("APP")]
		AppTopic,
		[Description("PC")]
		PcTopic
	}
}
