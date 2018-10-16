using System.ComponentModel;

namespace Hidistro.Entities.Sales
{
	public enum PayModeType
	{
		[Description("支付配置")]
		Pay,
		[Description("放款配置")]
		Outpay,
		[Description("退款配置")]
		Refund,
		[Description("已停用")]
		NoUsed = 99
	}
}
