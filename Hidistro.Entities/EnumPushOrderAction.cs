using System.ComponentModel;

namespace Hidistro.Entities
{
	public enum EnumPushOrderAction
	{
		[Description("订单发货")]
		OrderSended = 1,
		[Description("退款订单完成")]
		OrderRefund,
		[Description("退货订单确认")]
		OrderReturnConfirm,
		[Description("退货订单完成")]
		OrderReturnFinish,
		[Description("预售订单支付提醒")]
		OrderPaymentToPreSale
	}
}
