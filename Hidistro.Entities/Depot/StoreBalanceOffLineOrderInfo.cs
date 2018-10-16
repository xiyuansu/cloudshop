using System;

namespace Hidistro.Entities.Depot
{
	public class StoreBalanceOffLineOrderInfo
	{
		public DateTime PayTime
		{
			get;
			set;
		}

		public DateTime OverBalanceDate
		{
			get;
			set;
		}

		public string PaymentTypeName
		{
			get;
			set;
		}

		public decimal OrderTotal
		{
			get;
			set;
		}
	}
}
