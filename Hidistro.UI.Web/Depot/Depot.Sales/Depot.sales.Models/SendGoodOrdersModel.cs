using Hidistro.UI.Web.ashxBase;

namespace Hidistro.UI.Web.Depot.sales.Models
{
	public class SendGoodOrdersModel<T> : DataGridViewModel<T>
	{
		public decimal OrderSummaryTotal
		{
			get;
			set;
		}

		public decimal OrderProfitTotal
		{
			get;
			set;
		}
	}
}
