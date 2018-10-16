using System.ComponentModel;

namespace Hidistro.Entities.Orders
{
	public enum OrderType
	{
		[Description("普通商品订单")]
		NormalOrder,
		[Description("经典团购订单")]
		GroupOrder,
		[Description("火拼团订单")]
		FightGroup,
		[Description("限时抢购订单")]
		CountDown,
		[Description("预售订单")]
		PreSale,
		[Description("服务商品订单")]
		ServiceOrder = 6
	}
}
