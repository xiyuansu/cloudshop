using System.ComponentModel;

namespace Hidistro.Entities.Orders
{
	public enum LineItemStatus
	{
		[Description("正常状态|Trade_NORMAL|正常状态|正常状态")]
		Normal,
		[Description("申请退款|TRADE_APPLY_FOR_REFUND|退款中|退款中")]
		RefundApplied = 10,
		[Description("退款完成|TRADE_REFUND_FINISHED|退款完成|退款完成")]
		Refunded,
		[Description("退款被拒绝|TRADE_REFUND_REFUSED|退款被拒绝|退款被拒绝")]
		RefundRefused,
		[Description("申请退货|TRADE_APPLY_FOR_RETURN|退货中|退款中")]
		ReturnApplied = 20,
		[Description("退货待用户发货|WAIT_RETURN_BUYER_SEND_GOODS|退货中|退款中")]
		MerchantsAgreedForReturn,
		[Description("退货用户已发货|WAIT_RETURN_SELLER_GOODS|退货中|退款中")]
		DeliveryForReturn,
		[Description("退货平台已收货|WAIT_REFUND_SELLER_CONFIRM_GOODS|退货中|退款中")]
		GetGoodsForReturn,
		[Description("退货完成|TRADE_RETURNED_FINISHED|退货完成|退款完成")]
		Returned,
		[Description("退货被拒绝|TRADE_RETURN_REFUSED|退货被拒绝|退款被拒绝")]
		ReturnsRefused,
		[Description("申请换货|TRADE_APPLY_FOR_REPLACE|换货中|换货中")]
		ReplaceApplied = 30,
		[Description("换货待用户发货|WAIT_REPLACE_BUYER_SEND_GOODS|换货中|换货中")]
		MerchantsAgreedForReplace,
		[Description("换货用户已发货|WAIT_REPLACE_SELLER_GOODS|换货中|换货中")]
		UserDeliveryForReplace,
		[Description("换货平台已发货|WAIT_REPLACE_BUYER_CONFIRM_GOODS|换货中|换货中")]
		MerchantsDeliveryForRepalce,
		[Description("换货完成|TRADE_REPLACE_FINISHED|换货完成|换货完成")]
		Replaced,
		[Description("拒绝换货|TRADE_REPLACE_REFUSED|拒绝换货|拒绝换货")]
		ReplaceRefused
	}
}
