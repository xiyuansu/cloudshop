using System.ComponentModel;

namespace Hidistro.Core.Enums
{
	public enum SystemRoles
	{
		[Description("平台管理员")]
		SystemAdministrator,
		[Description("门店管理员")]
		StoreAdmin = -1,
		[Description("供应商管理员")]
		SupplierAdmin = -2,
		[Description("门店导购")]
		ShoppingGuider = -3
	}
}
