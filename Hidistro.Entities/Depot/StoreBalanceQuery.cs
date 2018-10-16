using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.Depot
{
	public class StoreBalanceQuery : Pagination
	{
		public int StoreId
		{
			get;
			set;
		}

		public bool? IsStoreCollect
		{
			get;
			set;
		}

		public DateTime? StartDate
		{
			get;
			set;
		}

		public DateTime? EndDate
		{
			get;
			set;
		}
	}
}
