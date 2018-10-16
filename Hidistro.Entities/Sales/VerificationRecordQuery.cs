using Hidistro.Core.Entities;
using Hidistro.Entities.Orders;
using System;

namespace Hidistro.Entities.Sales
{
	public class VerificationRecordQuery : Pagination
	{
		public int? ManagerId
		{
			get;
			set;
		}

		public int? StoreId
		{
			get;
			set;
		}

		public string OrderId
		{
			get;
			set;
		}

		public string Code
		{
			get;
			set;
		}

		public VerificationStatus? Status
		{
			get;
			set;
		}

		public DateTime? StartCreateDate
		{
			get;
			set;
		}

		public DateTime? EndCreateDate
		{
			get;
			set;
		}

		public DateTime? StartVerificationDate
		{
			get;
			set;
		}

		public DateTime? EndVerificationDate
		{
			get;
			set;
		}
	}
}
