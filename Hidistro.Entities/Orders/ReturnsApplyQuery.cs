using Hidistro.Core.Entities;

namespace Hidistro.Entities.Orders
{
	public class ReturnsApplyQuery : Pagination
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

		public bool? IsNoCompleted
		{
			get;
			set;
		}

		public int? ReturnId
		{
			get;
			set;
		}

		public string ProductName
		{
			get;
			set;
		}

		public int? SupplierId
		{
			get;
			set;
		}

		public bool? SupplierNoCompleted
		{
			get;
			set;
		}

		public string ReturnIds
		{
			get;
			set;
		}
	}
}
