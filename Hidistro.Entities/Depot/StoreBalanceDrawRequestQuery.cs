using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.Depot
{
	public class StoreBalanceDrawRequestQuery : Pagination
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

		public int? StoreId
		{
			get;
			set;
		}

		public int? Id
		{
			get;
			set;
		}

		public int? JournalNumber
		{
			get;
			set;
		}

		public string StoreName
		{
			get;
			set;
		}

		public string UserName
		{
			get;
			set;
		}

		public int DrawRequestType
		{
			get;
			set;
		}

		public int AuditStatus
		{
			get;
			set;
		}
	}
}
