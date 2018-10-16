using System.ComponentModel;

namespace Hidistro.Entities
{
	public enum EnumPushSendType
	{
		[Description("立即")]
		AtOnce = 1,
		[Description("定时")]
		Timer
	}
}
