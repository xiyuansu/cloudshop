using System;

namespace Hidistro.Entities.Members
{
	public class StoreUserOrderInfo
	{
		public string OrderId
		{
			get;
			set;
		}

		public decimal OrderTotal
		{
			get;
			set;
		}

		public DateTime OrderDate
		{
			get;
			set;
		}
	}
}
