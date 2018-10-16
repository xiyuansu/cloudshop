namespace Hidistro.Entities.Supplier
{
	public class SupplierSettlementModel
	{
		public string UserName
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

		public decimal FrozenBalance
		{
			get;
			set;
		}

		public decimal CanDrawRequestBalance
		{
			get
			{
				return this.Balance - this.FrozenBalance;
			}
		}

		public decimal BalanceOut
		{
			get;
			set;
		}
	}
}
