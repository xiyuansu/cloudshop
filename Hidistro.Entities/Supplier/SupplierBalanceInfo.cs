namespace Hidistro.Entities.Supplier
{
	public class SupplierBalanceInfo
	{
		public int SupplierId
		{
			get;
			set;
		}

		public string SupplierName
		{
			get;
			set;
		}

		public string Tel
		{
			get;
			set;
		}

		public decimal Balance
		{
			get;
			set;
		}

		public decimal BalanceFozen
		{
			get;
			set;
		}

		public decimal BalanceOut
		{
			get;
			set;
		}
	}
}
