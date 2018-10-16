using System.ComponentModel;

namespace Hidistro.Entities
{
	public enum EnumFightGroupActivitiyStatus
	{
		[Description("请选择")]
		PleaseSelect,
		[Description("正在进行")]
		BeingCarried,
		[Description("即将开始")]
		BeginInAMinute,
		[Description("已结束")]
		Ended
	}
}
