namespace Hidistro.Entities.Supplier
{
	public class SupplierStatisticsInfo
	{
		public decimal OrderPriceToday
		{
			get;
			set;
		}

		public int OrderNumbToday
		{
			get;
			set;
		}

		public int ProductNumbOnSale
		{
			get;
			set;
		}

		public decimal Balance
		{
			get;
			set;
		}

		public decimal ApplyRequestWaitDispose
		{
			get;
			set;
		}

		public decimal BalanceDrawRequested
		{
			get;
			set;
		}

		public int OrderNumbWaitConsignment
		{
			get;
			set;
		}

		public int OrderReturnNum
		{
			get;
			set;
		}

		public int OrderReplaceNum
		{
			get;
			set;
		}

		public int ProductNumStokWarning
		{
			get;
			set;
		}
	}
}
