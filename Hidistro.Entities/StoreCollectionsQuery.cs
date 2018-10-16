using Hidistro.Core.Entities;
using System;
using System.Collections.Generic;

namespace Hidistro.Entities
{
	public class StoreCollectionsQuery : Pagination
	{
		public DateTime? StartPayTime
		{
			get;
			set;
		}

		public DateTime? EndPayTime
		{
			get;
			set;
		}

		public int? PaymentTypeId
		{
			get;
			set;
		}

		public int? CollectionType
		{
			get;
			set;
		}

		public int? OrderType
		{
			get;
			set;
		}

		public int? StoreId
		{
			get;
			set;
		}

		public IList<string> OrderIds
		{
			get;
			set;
		}
	}
}
