using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.Supplier
{
	public class BalanceDetailSupplierQuery : Pagination
	{
		public DateTime? FromDate
		{
			get;
			set;
		}

		public DateTime? ToDate
		{
			get;
			set;
		}

		public int TradeType
		{
			get;
			set;
		}

		public int? SupplierId
		{
			get;
			set;
		}

		public string Key
		{
			get;
			set;
		}
	}
}
