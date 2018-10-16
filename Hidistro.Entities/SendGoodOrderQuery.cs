using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities
{
	public class SendGoodOrderQuery : Pagination
	{
		public int StoreId
		{
			get;
			set;
		}

		public DateTime? ShippingStartDate
		{
			get;
			set;
		}

		public DateTime? ShippingEndDate
		{
			get;
			set;
		}
	}
}
