using System.ComponentModel;

namespace Hidistro.Entities.Orders
{
	public enum VerificationStatus
	{
		[Description("未核销")]
		Applied,
		[Description("已核销")]
		Finished,
		[Description("已过期")]
		Expired = 3,
		[Description("退款中")]
		ApplyRefund,
		[Description("已退款")]
		Refunded
	}
}
