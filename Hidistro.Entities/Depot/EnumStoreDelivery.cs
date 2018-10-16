using System.ComponentModel;

namespace Hidistro.Entities.Depot
{
	public enum EnumStoreDelivery
	{
		[Description("快递")]
		SupportExpress,
		[Description("自提")]
		PickeupInStore,
		[Description("门店配送")]
		StoreDelive
	}
}
