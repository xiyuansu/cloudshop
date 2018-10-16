using System.ComponentModel;

namespace Hidistro.Entities
{
	public enum EnumPushStoreAction
	{
		[Description("退款订单待处理")]
		StoreOrderRefundApply = 5,
		[Description("退货订单待处理")]
		StoreOrderReturnApply,
		[Description("上门自提待确认")]
		TakeOnStoreOrderWaitConfirm,
		[Description("门店订单待发货")]
		StoreOrderWaitSendGoods,
		[Description("门店订单已支付")]
		StoreOrderPayed,
		[Description("门店库存警告")]
		StoreStockWarning,
		[Description("换货订单待处理")]
		StoreOrderReplaceApply
	}
}
