namespace Hidistro.Core
{
	public static class ProductTempSQLADD
	{
		public static string ReturnShowOrder(ProductShowOrderPriority show)
		{
			switch (show)
			{
			case ProductShowOrderPriority.IDDESC:
				return " DisplaySequence DESC";
			case ProductShowOrderPriority.VistiCountsDESC:
				return " VistiCounts    DESC";
			case ProductShowOrderPriority.AddedDateDESC:
				return " AddedDate DESC";
			case ProductShowOrderPriority.AddedDateASC:
				return " ProductId ASC";
			case ProductShowOrderPriority.ShowSaleCountsDESC:
				return " ShowSaleCounts DESC";
			case ProductShowOrderPriority.ShowSaleCountsASC:
				return " ShowSaleCounts ASC";
			case ProductShowOrderPriority.SalePriceDESC:
				return " SalePrice DESC";
			case ProductShowOrderPriority.SalePriceASC:
				return " SalePrice ASC";
			default:
				return "";
			}
		}
	}
}
