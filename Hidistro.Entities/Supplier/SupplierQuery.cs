using Hidistro.Core.Entities;

namespace Hidistro.Entities.Supplier
{
	public class SupplierQuery : Pagination
	{
		public int? ManagerId
		{
			get;
			set;
		}

		public string UserName
		{
			get;
			set;
		}

		public string SupplierName
		{
			get;
			set;
		}

		public string Tel
		{
			get;
			set;
		}

		public int? Status
		{
			get;
			set;
		}

		public string StateName
		{
			get;
			set;
		}

		public int OrderNums
		{
			get;
			set;
		}

		public int ProductNums
		{
			get;
			set;
		}
	}
}
