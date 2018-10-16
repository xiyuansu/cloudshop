namespace Hidistro.Entities
{
	public class StoreProductBaseModel
	{
		public int StoreId
		{
			get;
			set;
		}

		public int ProductId
		{
			get;
			set;
		}

		public int Stock
		{
			get;
			set;
		}

		public string ProductName
		{
			get;
			set;
		}

		public string ProductImage
		{
			get;
			set;
		}

		public string ProductCode
		{
			get;
			set;
		}

		public decimal Price
		{
			get;
			set;
		}

		public int SaleCounts
		{
			get;
			set;
		}

		public int ShowSaleCounts
		{
			get;
			set;
		}

		public int WarningStock
		{
			get;
			set;
		}

		public int ProductType
		{
			get;
			set;
		}

		public bool HasSKU
		{
			get;
			set;
		}

		public decimal MarketPrice
		{
			get;
			set;
		}

		public string DefaultSkuId
		{
			get;
			set;
		}
	}
}
