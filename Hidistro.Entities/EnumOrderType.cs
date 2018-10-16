using System.ComponentModel;

namespace Hidistro.Entities
{
	public enum EnumOrderType
	{
		[Description("上门自提订单")]
		TakeOnDoorOrder = 1,
		[Description("门店配送订单")]
		StoreDeliveryOrder = 3,
		[Description("线下门店订单")]
		StoreOfflineOrder = 2
	}
}
