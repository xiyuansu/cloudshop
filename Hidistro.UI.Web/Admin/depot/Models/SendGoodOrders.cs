using System.Collections.Generic;

namespace Hidistro.UI.Web.Admin.depot.Models
{
	public class SendGoodOrders<T>
	{
		public List<T> rows
		{
			get;
			set;
		}

		public int total
		{
			get;
			set;
		}

		public decimal OrderSummaryTotal
		{
			get;
			set;
		}

		public decimal OrderSummaryProfit
		{
			get;
			set;
		}
	}
}
