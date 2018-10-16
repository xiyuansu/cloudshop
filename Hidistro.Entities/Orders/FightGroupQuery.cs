using Hidistro.Core.Entities;
using System.Collections.Generic;

namespace Hidistro.Entities.Orders
{
	public class FightGroupQuery : Pagination
	{
		public int? Status
		{
			get;
			set;
		}

		public int? UserId
		{
			get;
			set;
		}

		public IEnumerable<int> OrderStatus
		{
			get;
			set;
		}
	}
}
