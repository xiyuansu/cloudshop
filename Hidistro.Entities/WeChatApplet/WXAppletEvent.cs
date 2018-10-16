using System.ComponentModel;

namespace Hidistro.Entities.WeChatApplet
{
	public enum WXAppletEvent
	{
		[Description("创建订单")]
		CreateOrder = 1,
		[Description("支付")]
		Pay,
		[Description("申请退款")]
		ApplyRefund,
		[Description("申请售后")]
		ApplyAfterSale,
		[Description("退货发货")]
		ReturnSendGoods,
		[Description("服务核销码核销")]
		ServiceProductValid
	}
}
