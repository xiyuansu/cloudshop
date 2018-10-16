using Hidistro.Core.Entities;
using System;
using System.Collections.Generic;

namespace Hidistro.Entities.Depot
{
	public class StoreManagersQuery : Pagination
	{
		private IList<int> _RoleIds = null;

		public int StoreId
		{
			get;
			set;
		}

		public IList<int> RoleIds
		{
			get
			{
				if (this._RoleIds == null)
				{
					this._RoleIds = new List<int>();
				}
				return this._RoleIds;
			}
		}

		public string UserName
		{
			get;
			set;
		}

		public DateTime? StartTime
		{
			get;
			set;
		}

		public DateTime? EndTime
		{
			get;
			set;
		}
	}
}
