using System.ComponentModel;

namespace Hidistro.Entities.Commodities
{
	public enum DetailException
	{
		[Description("歇业中")]
		StopService = 1,
		[Description("无库存")]
		NoStock,
		[Description("服务超区")]
		OverServiceArea,
		[Description("非营业时间")]
		IsNotWorkTime,
		[Description("正常")]
		Nomal
	}
}
