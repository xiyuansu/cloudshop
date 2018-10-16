using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.Store
{
	public class OperationLogQuery
	{
		public Pagination Page
		{
			get;
			set;
		}

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

		public string OperationUserName
		{
			get;
			set;
		}

		public OperationLogQuery()
		{
			this.Page = new Pagination();
		}
	}
}
