using Hidistro.Core.Entities;
using Hidistro.Entities.Orders;

namespace Hidistro.Entities.Promotions
{
	public class CountDownQuery : Pagination
	{
		public string ProductName
		{
			get;
			set;
		}

		public int State
		{
			get;
			set;
		}

		public int CountDownId
		{
			get;
			set;
		}

		public int StoreId
		{
			get;
			set;
		}

		public OrderStatus OrderState
		{
			get;
			set;
		}

		public CountDownQuery()
		{
			this.StoreId = -1;
		}
	}
}
