using System.ComponentModel;

namespace Hidistro.Entities
{
	public enum EnumPushStatus
	{
		[Description("未推送")]
		NoPush = 1,
		[Description("推送失败")]
		PushFailure,
		[Description("推送成功")]
		PushSucceed
	}
}
