using System.ComponentModel;

namespace Hidistro.Entities
{
	public enum EnumTimeRange
	{
		[Description("全部")]
		All,
		[Description("今天")]
		Today,
		[Description("近七天")]
		SevenDays,
		[Description("本月")]
		ThisMonth,
		[Description("上个月")]
		LastMonth,
		[Description("近三个月")]
		NearlyThreeMonths,
		[Description("自定义时间")]
		CustomTime
	}
}
