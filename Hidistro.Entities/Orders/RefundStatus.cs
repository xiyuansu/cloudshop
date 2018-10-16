using System.ComponentModel;

namespace Hidistro.Entities.Orders
{
	public enum RefundStatus
	{
		[Description("退款中")]
		Applied,
		[Description("退款完成")]
		Refunded,
		[Description("拒绝退款")]
		Refused
	}
}
