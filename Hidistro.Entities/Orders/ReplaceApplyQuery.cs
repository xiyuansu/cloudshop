using Hidistro.Core.Entities;

namespace Hidistro.Entities.Orders
{
	public class ReplaceApplyQuery : Pagination
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

		public int? ReplaceId
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

		public string ReplaceIds
		{
			get;
			set;
		}
	}
}
