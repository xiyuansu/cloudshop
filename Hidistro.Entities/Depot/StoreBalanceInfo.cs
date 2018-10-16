using System;

namespace Hidistro.Entities.Depot
{
	[Serializable]
	public class StoreBalanceInfo
	{
		public decimal Balance
		{
			get;
			set;
		}

		public decimal BalanceForzen
		{
			get;
			set;
		}

		public decimal BalanceOut
		{
			get;
			set;
		}

		public decimal FinishOrderBalance
		{
			get;
			set;
		}

		public decimal NoFinishOrderBalance
		{
			get;
			set;
		}
	}
}
