using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.Members
{
	public class BalanceDetailQuery : Pagination
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

		public TradeTypes TradeType
		{
			get;
			set;
		}

		public SplittingTypes SplittingTypes
		{
			get;
			set;
		}

		public int? UserId
		{
			get;
			set;
		}

		public string UserName
		{
			get;
			set;
		}

		public string OrderId
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
