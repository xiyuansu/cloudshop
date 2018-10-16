using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.Depot
{
	public class StoreBalanceOrderQuery : Pagination
	{
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

		public string OrderId
		{
			get;
			set;
		}

		public bool IsBalanceOver
		{
			get;
			set;
		}

		public int StoreId
		{
			get;
			set;
		}

		public DateTime? OverStartDate
		{
			get;
			set;
		}

		public DateTime? OverEndDate
		{
			get;
			set;
		}
	}
}
