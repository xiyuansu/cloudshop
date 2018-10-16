using Hidistro.Core.Entities;

namespace Hidistro.Entities.Orders
{
	public class RefundApplyQuery : Pagination
	{
		public string OrderId
		{
			get;
			set;
		}

		public int? HandleStatus
		{
			get;
			set;
		}

		public int? UserId
		{
			get;
			set;
		}

		public int? StoreId
		{
			get;
			set;
		}

		public int? RefundId
		{
			get;
			set;
		}

		public int? SupplierId
		{
			get;
			set;
		}

		public string RefundIds
		{
			get;
			set;
		}
	}
}
