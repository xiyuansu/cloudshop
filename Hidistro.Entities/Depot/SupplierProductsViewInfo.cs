namespace Hidistro.Entities.Depot
{
	public class SupplierProductsViewInfo
	{
		public int SupplierId
		{
			get;
			set;
		}

		public int ProductId
		{
			get;
			set;
		}

		public int? Stock
		{
			get;
			set;
		}

		public string ProductCode
		{
			get;
			set;
		}

		public string ProductName
		{
			get;
			set;
		}

		public int? CategoryId
		{
			get;
			set;
		}

		public int? SaleStatus
		{
			get;
			set;
		}

		public decimal? SalePrice
		{
			get;
			set;
		}

		public decimal? CostPrice
		{
			get;
			set;
		}

		public decimal? MarketPrice
		{
			get;
			set;
		}

		public string ThumbnailUrl40
		{
			get;
			set;
		}

		public string MainCategoryPath
		{
			get;
			set;
		}

		public string ExtendCategoryPath
		{
			get;
			set;
		}

		public string ExtendCategoryPath1
		{
			get;
			set;
		}

		public string ExtendCategoryPath2
		{
			get;
			set;
		}

		public string ExtendCategoryPath3
		{
			get;
			set;
		}

		public string ExtendCategoryPath4
		{
			get;
			set;
		}

		public int? WarningStockNum
		{
			get;
			set;
		}

		public int? DisplaySequence
		{
			get;
			set;
		}

		public int MainStock
		{
			get;
			set;
		}

		public string SaleStatusText
		{
			get
			{
				string text = "";
				switch (this.SaleStatus)
				{
				case 1:
					return "出售中";
				case 2:
					return "下架区";
				default:
					return "仓库中";
				}
			}
		}
	}
}
