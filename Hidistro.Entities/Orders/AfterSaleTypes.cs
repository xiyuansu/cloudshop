using System.ComponentModel;

namespace Hidistro.Entities.Orders
{
	public enum AfterSaleTypes
	{
		[Description("订单退款")]
		OrderRefund,
		[Description("退货退款")]
		ReturnAndRefund,
		[Description("换货")]
		Replace,
		[Description("仅退款")]
		OnlyRefund
	}
}
