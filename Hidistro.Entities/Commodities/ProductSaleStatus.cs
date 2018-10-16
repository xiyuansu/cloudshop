using System.ComponentModel;

namespace Hidistro.Entities.Commodities
{
	public enum ProductSaleStatus
	{
		[Description("所有|ALL")]
		All = -1,
		[Description("回收站|Delete")]
		Delete,
		[Description("出售中|On_Sale")]
		OnSale,
		[Description("下架中|Un_Sale")]
		UnSale,
		[Description("仓库中|In_Stock")]
		OnStock
	}
}
