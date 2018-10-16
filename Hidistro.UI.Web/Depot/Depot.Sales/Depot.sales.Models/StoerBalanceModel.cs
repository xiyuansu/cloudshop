using System.Collections.Generic;

namespace Hidistro.UI.Web.Depot.sales.Models
{
	public class StoerBalanceModel<T>
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

		public decimal totalAmount
		{
			get;
			set;
		}
	}
}
