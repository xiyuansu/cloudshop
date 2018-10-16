using System.ComponentModel;

namespace Hidistro.Entities.Sales
{
	public enum ServiceOrderStatus
	{
		[Description("待付款")]
		WaitBuyerPay = 1,
		[Description("待消费")]
		WaitConsumption,
		[Description("退款中")]
		Refunding,
		[Description("已过期")]
		Expired,
		[Description("已关闭")]
		Closed,
		[Description("已完成")]
		Finished
	}
}
