using System.ComponentModel;

namespace Hidistro.Entities
{
	public enum EnumMarktingType : byte
	{
		[Description("限时抢购")]
		CountDownProducts = 1,
		[Description("优惠券")]
		Coupon,
		[Description("火拼团")]
		FightGroup,
		[Description("积分商城")]
		PointMall,
		[Description("注册送券")]
		RegisteredCoupon
	}
}
