using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.Members
{
	public class BalanceDrawRequestQuery : Pagination
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

		public int? UserId
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
