using System;

namespace Hidistro.Entities.Supplier
{
	[Serializable]
	public class BalanceInfo
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
	}
}
