using System.ComponentModel;

namespace Hidistro.Entities.Depot
{
	public enum EnumStoreCollectionStatus
	{
		[Description("未支付")]
		NoPay,
		[Description("已支付")]
		Payed,
		[Description("已退款")]
		Refunded = 3
	}
}
