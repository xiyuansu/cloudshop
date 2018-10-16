using System.ComponentModel;

namespace Hidistro.Entities.Commodities
{
	public enum ProductType
	{
		[Description("所有商品")]
		All = -1,
		[Description("实物商品")]
		PhysicalProduct,
		[Description("服务商品")]
		ServiceProduct
	}
}
