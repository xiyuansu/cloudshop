using System.ComponentModel;

namespace Hidistro.Entities.Orders
{
	public enum DadaStatus
	{
		[Description("待接单")]
		WaitOrder = 1,
		[Description("待取货")]
		WaitTake,
		[Description("配送中")]
		Distribution,
		[Description("已完成")]
		Finished,
		[Description("已取消")]
		Cancel,
		[Description("已过期")]
		Expired = 7,
		[Description("指派单")]
		Assigned,
		[Description("系统故障订单发布失败")]
		Failure = 1000
	}
}
