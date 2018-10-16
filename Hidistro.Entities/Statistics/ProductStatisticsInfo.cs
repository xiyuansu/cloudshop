namespace Hidistro.Entities.Statistics
{
	public class ProductStatisticsInfo
	{
		public int ProductId
		{
			get;
			set;
		}

		public string ProductName
		{
			get;
			set;
		}

		public int PV
		{
			get;
			set;
		}

		public int UV
		{
			get;
			set;
		}

		public int PaymentNum
		{
			get;
			set;
		}

		public int SaleQuantity
		{
			get;
			set;
		}

		public decimal SaleAmount
		{
			get;
			set;
		}

		public decimal ProductConversionRate
		{
			get;
			set;
		}
	}
}
