using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.Depot
{
	public class StoreStockLogQuery : Pagination
	{
		public int? StoreId
		{
			get;
			set;
		}

		public DateTime? StartTime
		{
			get;
			set;
		}

		public DateTime? EndTime
		{
			get;
			set;
		}

		public string Operator
		{
			get;
			set;
		}

		public int? ProductId
		{
			get;
			set;
		}
	}
}
