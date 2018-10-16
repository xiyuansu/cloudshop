using System.ComponentModel;

namespace Hidistro.Entities.Orders
{
	public enum OrderStatus
	{
		[Description("所有订单|Trade_ALL|所有订单")]
		All,
		[Description("等待买家付款|WAIT_BUYER_PAY|待付款")]
		WaitBuyerPay,
		[Description("待发货|WAIT_SELLER_SEND_GOODS|待发货")]
		BuyerAlreadyPaid,
		[Description("卖家已发货|WAIT_BUYER_CONFIRM_GOODS|卖家已发货")]
		SellerAlreadySent,
		[Description("交易关闭|TRADE_CLOSED|交易关闭")]
		Closed,
		[Description("交易完成|TRADE_FINISHED|交易完成")]
		Finished,
		[Description("等待评论|TRADE_WAITREVIEW|等待评论")]
		WaitReview = 21,
		[Description("申请退款|TRADE_APPLY_FOR_REFUND|退款中")]
		ApplyForRefund = 6,
		[Description("退款完成|TRADE_REFUND_FINISHED|退款完成")]
		Refunded = 9,
		[Description("退款被拒绝|TRADE_REFUND_REFUSED|退款被拒绝")]
		RefundRefused = 18,
		[Description("历史订单|TRADE_HISTORY|历史订单")]
		History = 99
	}
}
