using System.ComponentModel;

namespace Hidistro.Entities.Depot
{
	public enum EnumStoreActivityType
	{
		[Description("满减")]
		Promotion = 1,
		[Description("限时购")]
		CountDown,
		[Description("优惠券")]
		Coupons = 10
	}
}
