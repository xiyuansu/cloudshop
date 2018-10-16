using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.Depot
{
	public class StoreBalanceDetailQuery : Pagination
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

		public StoreTradeTypes TradeType
		{
			get;
			set;
		}

		public int? StoreId
		{
			get;
			set;
		}

		public string StoreName
		{
			get;
			set;
		}

		public string TradeNo
		{
			get;
			set;
		}

		public DateTime? FromDateJS
		{
			get;
			set;
		}

		public DateTime? ToDateJS
		{
			get;
			set;
		}
	}
}
