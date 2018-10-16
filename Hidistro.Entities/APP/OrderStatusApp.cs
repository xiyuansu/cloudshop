using System.ComponentModel;

namespace Hidistro.Entities.APP
{
	public enum OrderStatusApp
	{
		[Description("待付款")]
		WaitBuyerPay = 1,
		[Description("待发货")]
		BuyerAlreadyPaid,
		[Description("已发货")]
		SellerAlreadySent,
		[Description("交易关闭")]
		Closed,
		[Description("已完成")]
		Finished,
		[Description("门店配货中")]
		StoreDistribution = 7,
		[Description("待上门自提")]
		SinceDoor,
		[Description("申请退款")]
		ApplyForRefund = 6,
		[Description("退款完成")]
		Refunded = 9,
		[Description("退款被拒绝")]
		RefundRefused = 18
	}
}
