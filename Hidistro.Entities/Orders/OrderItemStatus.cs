using System.ComponentModel;

namespace Hidistro.Entities.Orders
{
	public enum OrderItemStatus
	{
		[Description("正常状态")]
		Nomarl,
		[Description("正在退货")]
		HasReturn = 2,
		[Description("正在换货")]
		HasReplace,
		[Description("正在退货/换货")]
		HasReturnOrReplace,
		[Description("所有商品售后中")]
		AllInAfterSales
	}
}
