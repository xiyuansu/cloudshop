using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.Supplier
{
	public class BalanceOrderQuery : Pagination
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

		public int SupplierId
		{
			get;
			set;
		}
	}
}
